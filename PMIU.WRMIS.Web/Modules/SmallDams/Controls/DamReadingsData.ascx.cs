using System;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.SmallDams.DamSearch;

namespace PMIU.WRMIS.Web.Modules.SmallDams.Controls
{
    public partial class DamReadingsData : System.Web.UI.UserControl
    {
        public static long? _DAMID;
        public static DateTime? _DATE;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_DAMID!=null && _DATE != null)
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
                object _DAMDetail = new SmallDamsBLL().GetDamReadingsData(_ID);
                if (_DAMDetail != null)
                {
                    
                    DivisionText.Text = Utility.GetDynamicPropertyValue(_DAMDetail, "Division");

                    SubDivisionText.Text = Utility.GetDynamicPropertyValue(_DAMDetail, "SubDivision");

                    DamNameText.Text = Utility.GetDynamicPropertyValue(_DAMDetail, "DamName");
                    
                    DateText.Text = String.Format("{0:dd-MMM-yyyy}",_DATE);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}