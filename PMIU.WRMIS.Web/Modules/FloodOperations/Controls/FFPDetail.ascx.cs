using System;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.FloodOperations.FFP;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class FFPDetail : System.Web.UI.UserControl
    {
        public static long? _FFPID;

        protected void Page_Load(object sender, EventArgs e)
        {
           // long FFPID = 0;

            try
            {
               // long.TryParse(Convert.ToString(_FFPID), out FFPID);
                if (_FFPID!=null)
                {
                    LoadFFPDetail(_FFPID);
                    _FFPID = null;
                }
                
                
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadFFPDetail(long? _ID)
        {
            try
            {
                object _FFPDetail = new FloodFightingPlanBLL().GetFFPDetails(_ID, null, null, null, null, null);
                if (_FFPDetail != null)
                {
                    lblZone.Text = Utility.GetDynamicPropertyValue(_FFPDetail, "FFPZone");
                    lblCircle.Text = Utility.GetDynamicPropertyValue(_FFPDetail, "FFPCircle");
                    lblDivision.Text = Utility.GetDynamicPropertyValue(_FFPDetail, "FFPDivision");
                    lblFFPStatus.Text = Utility.GetDynamicPropertyValue(_FFPDetail, "FFPStatus");
                    lblYear.Text = Utility.GetDynamicPropertyValue(_FFPDetail, "FFPYear");

                   // DivisionItemFFP._DivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_FFPDetail, "FFPDivisionID"));
                  //  DivisionItemFFP._Year = Convert.ToInt32(Utility.GetDynamicPropertyValue(_FFPDetail, "FFPYear"));

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}