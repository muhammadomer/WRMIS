using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class DepartmentalInspectionDetail : System.Web.UI.UserControl
    {
        private long FloodInspectionID = 0;
        private bool ShowInspectionStatus = true;
        public long FloodInspectionIDProp { get { return FloodInspectionID; } set { FloodInspectionID = value; } }
        public bool ShowInspectionStatusProp { get { return ShowInspectionStatus; } set { ShowInspectionStatus = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            //long floodInspectionID = 0;
            try
            {
                //long.TryParse(Convert.ToString(DepartmentalInspectionDetail.FloodInspectionID), out floodInspectionID);
                //LoadDepartmentalInspectionDetail(floodInspectionID);
                LoadDepartmentalInspectionDetail(FloodInspectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadDepartmentalInspectionDetail(long _FloodInspectionID)
        {
            try
            {
                object departmentalInspectionDetail = new FloodInspectionsBLL().GetDepartmentalInspectionByID(_FloodInspectionID);

                lblDivision.Text = Convert.ToString(departmentalInspectionDetail.GetType().GetProperty("DivisionName").GetValue(departmentalInspectionDetail));
                lblInspectionDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(departmentalInspectionDetail.GetType().GetProperty("InspectionDate").GetValue(departmentalInspectionDetail)));
                
                FloodInspectionIDProp = _FloodInspectionID;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}