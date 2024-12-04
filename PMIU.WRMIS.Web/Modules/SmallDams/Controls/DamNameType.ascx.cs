using System;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.SmallDams.DamSearch;

namespace PMIU.WRMIS.Web.Modules.SmallDams.Controls
{
    public partial class DamNameType: System.Web.UI.UserControl
    {
        public static long? _DAMID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_DAMID!=null)
                {
                    LoadDamDetail(_DAMID);
                    _DAMID = null;
                }
                
                
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadDamDetail(Int64? _ID)
        {
            try
            {
                object _DAMDetail = new SmallDamsBLL().GetDamNametype(_ID);
                if (_DAMDetail != null)
                {
                    DamNameText.Text = Utility.GetDynamicPropertyValue(_DAMDetail, "DamName");
                    DamTypeText.Text = Utility.GetDynamicPropertyValue(_DAMDetail, "DamType");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}