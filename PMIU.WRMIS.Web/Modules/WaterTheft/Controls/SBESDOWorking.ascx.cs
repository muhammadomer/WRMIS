using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public partial class SBESDOWorking : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!IsPostBack)
                //{
                if (string.IsNullOrEmpty(Request.QueryString["ET"])) 
                    DisplaySBESDOWorkingInformation(WaterTheftCase.WaterTheftID);
                //}
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void DisplaySBESDOWorkingInformation(long _WaterTheftID)
        {
            if ((int)Constants.Designation.SBE == SessionManagerFacade.UserInformation.DesignationID.Value
                && (int)Constants.Designation.SBE == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID))
            {
                DisplaySBEWorking(false, _WaterTheftID);
                DisplaySDOWorking(false, _WaterTheftID);
            }
            else if ((int)Constants.Designation.SBE == SessionManagerFacade.UserInformation.DesignationID.Value
                && (int)Constants.Designation.SDO == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID)
                && (int)Constants.Designation.SBE == Convert.ToInt32(WaterTheftCase.AssignedByDesignationID))
            {
                DisplaySBEWorking(true, _WaterTheftID);
                DisplaySDOWorking(false, _WaterTheftID);
            }
            else if ((int)Constants.Designation.SDO == SessionManagerFacade.UserInformation.DesignationID.Value
                && (int)Constants.Designation.SDO == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID)
                && (int)Constants.Designation.SBE == Convert.ToInt32(WaterTheftCase.AssignedByDesignationID))
            {
                DisplaySBEWorking(true, _WaterTheftID);
                DisplaySDOWorking(false, _WaterTheftID);
            }
            else if ((int)Constants.Designation.Ziladaar == SessionManagerFacade.UserInformation.DesignationID.Value)
            {
                DisplaySBEWorking(false, _WaterTheftID);
                DisplaySDOWorking(false, _WaterTheftID);
            }
            else if ((int)Constants.Designation.SDO == SessionManagerFacade.UserInformation.DesignationID.Value
                && (int)Constants.Designation.SDO == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID)
                && (int)Constants.Designation.Ziladaar == Convert.ToInt32(WaterTheftCase.AssignedByDesignationID))
            {
                DisplaySBEWorking(true, _WaterTheftID);
                DisplaySDOWorking(true, _WaterTheftID);
            }
            else
            {
                DisplaySBEWorking(true, _WaterTheftID);
                DisplaySDOWorking(true, _WaterTheftID);
            }

            LoadSBESDOWorking(_WaterTheftID);
        }
        private void DisplaySBEWorking(bool _IsToDisplay, long _WaterTheftID)
        {
            SBEWorkingTitle.Visible = _IsToDisplay;
            tblSBEWorking.Visible = _IsToDisplay;
        }
        private void DisplaySDOWorking(bool _IsToDisplay, long _WaterTheftID)
        {
            SDOWorkingTitle.Visible = _IsToDisplay;
            tblSDOWorking.Visible = _IsToDisplay;
        }
        private void LoadSBESDOWorking(long _WaterTheftID)
        {
            List<dynamic> lstWTCaseWorking = new WaterTheftBLL().GetSBEWorkingInformation(_WaterTheftID);
            if (lstWTCaseWorking != null && lstWTCaseWorking.Count > 0)
            {
                lblSBECanalWireNo.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[0], "CanalWireNo");
                lblSBECanalWireDate.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[0], "CanalWireDate");
                lblDateOfClosingRepair.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[0], "ClosingRepairDate");

                if (lstWTCaseWorking.Count > 1)
                {
                    lblSDOCanalWireNo.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[1], "CanalWireNo");
                    lblSDOCanalWireDate.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[1], "CanalWireDate");
                    lblRemarks.Text = Utility.GetDynamicPropertyValue(lstWTCaseWorking[1], "Remarks");
                }
            }
            else
            {
                DisplaySBEWorking(false, _WaterTheftID);
                DisplaySDOWorking(false, _WaterTheftID);
            }
        }
    }
}