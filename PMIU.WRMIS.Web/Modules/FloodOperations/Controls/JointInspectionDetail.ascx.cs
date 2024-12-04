using System;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL.FloodOperations;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class JointInspectionDetail : System.Web.UI.UserControl
    {

        public static long _FloodInspectionID;
        protected void Page_Load(object sender, EventArgs e)
        {
            long FloodInspectionID = 0;
            try
            {
                long.TryParse(Convert.ToString(_FloodInspectionID), out FloodInspectionID);
                LoadJointInspectionDetail(FloodInspectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region JointInspectionDetails
        private void LoadJointInspectionDetail(long _ID)
        {
            try
            {
                object _InspectionDetail = new FloodInspectionsBLL().GetjointInspectionDetail(_ID);
                if (_InspectionDetail != null)
                {
                    lblDivision.Text = Utility.GetDynamicPropertyValue(_InspectionDetail, "DivisionName");
                    lblInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(_InspectionDetail, "InspectionDate"))));

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
    }
}