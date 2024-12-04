using System;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.SmallDams.DamSearch;

namespace PMIU.WRMIS.Web.Modules.SmallDams.Controls
{
    public partial class DamInfo : System.Web.UI.UserControl
    {
        public static long? _DAMID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_DAMID!=null)
                {
                    LoadDamInfo(_DAMID);
                    _DAMID = null;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadDamInfo(Int64? _ID)
        {
            try
            {
                object _DAMInfo = new SmallDamsBLL().GetDamInfo(_ID);
                if (_DAMInfo != null)
                {
                    DamNameText.Text = Utility.GetDynamicPropertyValue(_DAMInfo, "DamNameText");
                    DamTypeText.Text = Utility.GetDynamicPropertyValue(_DAMInfo, "DamTypeText");
                    CostProjectText.Text = String.Format("{0:n0}",Convert.ToInt64(Utility.GetDynamicPropertyValue(_DAMInfo, "CostProject")));
                    YearCompletionText.Text = Utility.GetDynamicPropertyValue(_DAMInfo, "YearCompletion");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}