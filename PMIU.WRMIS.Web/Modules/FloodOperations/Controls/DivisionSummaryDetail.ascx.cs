using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class DivisionSummaryDetail : System.Web.UI.UserControl
    {
        public static long _DivisionSummaryID;
        protected void Page_Load(object sender, EventArgs e)
        {
            long DivisionSummaryID = 0;
            try
            {
                long.TryParse(Convert.ToString(_DivisionSummaryID), out DivisionSummaryID);
                LoadDivisionSummaryDetail(DivisionSummaryID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadDivisionSummaryDetail(long _ID)
        {
            try
            {
                object _DivisionSummaryDetail = new FloodOperationsBLL().GetDivisionSummaryDetailByID(_ID);
                if (_DivisionSummaryDetail != null)
                {
                    lblZone.Text = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Zone");
                    lblCircle.Text = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Circle");
                    lblDivision.Text = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Division");
                    lblDivisionSummaryStatus.Text = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Status");
                    lblYear.Text = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Year");

                    DivisionSummaryStatus.Visible = false;
                    lblDivisionSummaryStatus.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}