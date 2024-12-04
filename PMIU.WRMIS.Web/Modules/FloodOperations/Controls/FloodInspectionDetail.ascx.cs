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
using PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class FloodInspectionDetail : System.Web.UI.UserControl
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
                //long.TryParse(Convert.ToString(FloodInspectionDetail.FloodInspectionID), out floodInspectionID);
                //LoadFloodInspectionDetail(floodInspectionID);
                LoadFloodInspectionDetail(FloodInspectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadFloodInspectionDetail(long _FloodInspectionID)
        {
            try
            {
                object floodInspectionDetail = new FloodInspectionsBLL().GetFloodInspectionByID(_FloodInspectionID);
                lblInfrastructureName.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InfrastructureName").GetValue(floodInspectionDetail));
                lblDivision.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("DivisionName").GetValue(floodInspectionDetail));
                lblInfrastructureType.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InfrastructureType").GetValue(floodInspectionDetail));
                lblInspectionDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(floodInspectionDetail.GetType().GetProperty("InspectionDate").GetValue(floodInspectionDetail)));
                lblInspectionStatus.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InspectionStatus").GetValue(floodInspectionDetail));
                lblInspectionType.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InspectionTypeName").GetValue(floodInspectionDetail));

                if (floodInspectionDetail.GetType().GetProperty("OutFallType") != null && floodInspectionDetail.GetType().GetProperty("OutFallName").GetValue(floodInspectionDetail).ToString() != "")
                {
                    lblInspectionCategory.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InspectionCategoryName").GetValue(floodInspectionDetail));
                    lblOutfallType.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("OutFallType").GetValue(floodInspectionDetail));
                    lblOutfallName.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("OutFallName").GetValue(floodInspectionDetail));
                    lblInspectionStatusDrain.Text = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InspectionStatus").GetValue(floodInspectionDetail));
                    lblInspectionStatusDrain.Visible = true;
                    lblInspectionStatus.Visible = false;
                    strInspectionStatus.Visible = false;
                    strInspectionCategory.Visible = true;
                    lblInspectionCategory.Visible = true;
                    dvDrain.Visible = true;
                }

                if (ShowInspectionStatus == false)
                {
                    lblInspectionStatus.Visible = false;
                    strInspectionStatus.Visible = false;
                }

                FloodInspectionIDProp = _FloodInspectionID;

                MeasuringBookStatusPostFlood._DivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(floodInspectionDetail, "DivisionID"));
                MeasuringBookStatusPostFlood._Year = Convert.ToInt32(Utility.GetDynamicPropertyValue(floodInspectionDetail, "InspectionYear"));
                MeasuringBookStatusPostFlood._StructureTypeID = Convert.ToInt64(Utility.GetDynamicPropertyValue(floodInspectionDetail, "StructureType"));
                MeasuringBookStatusPostFlood._StructureID = Convert.ToInt64(Utility.GetDynamicPropertyValue(floodInspectionDetail, "StructureID"));
                RDWiseConditions._infrastructureType = Convert.ToString(floodInspectionDetail.GetType().GetProperty("InfrastructureType").GetValue(floodInspectionDetail));


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}