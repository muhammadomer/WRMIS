using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.WaterTheft;
namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class ChannelWaterTheftIncident : BasePage
    {
        private const string BasePath = "~/Modules/WaterTheft/Controls/";
        long waterTheftlID = 0;
        int os = 0; // Dynamically load Outlet/Channel user control 
        int requestForPage = 0;

        private string CurrentControl
        {
            get { return ViewState["CurrentControl"] == null ? string.Empty : Convert.ToString(ViewState["CurrentControl"]); }
            set { ViewState["CurrentControl"] = value; }
        }
        private enum ControlToLoad
        {
            Channel = 1,
            Outlet
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlToLoad controlToLoad = ControlToLoad.Channel;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["WaterTheftID"]) && !string.IsNullOrEmpty(Request.QueryString["OS"]) && !string.IsNullOrEmpty(Request.QueryString["RP"]))
                    {
                        SetUserControlsVariableValues();
                        DisplaySDOWorkingFields();
                    }
                }
                if ((int)ControlToLoad.Outlet == os)
                    controlToLoad = ControlToLoad.Outlet;
                LoadWaterTheftUserControl(controlToLoad);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelPhysicalLocation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void SetUserControlsVariableValues()
        {
            waterTheftlID = Convert.ToInt64(Request.QueryString["WaterTheftID"]);
            os = Convert.ToInt32(Request.QueryString["OS"]);
            requestForPage = Convert.ToInt16(Request.QueryString["RP"]);
            PMIU.WRMIS.Web.Modules.WaterTheft.Controls.ChannelWaterTheftIncident.WaterTheftID = waterTheftlID;
            PMIU.WRMIS.Web.Modules.WaterTheft.Controls.OutletWaterTheftIncident.WaterTheftID = waterTheftlID;
            PMIU.WRMIS.Web.Modules.WaterTheft.Controls.ChannelWaterTheftIncident.RequestedPage = requestForPage;
            //  PMIU.WRMIS.Web.Modules.WaterTheft.Controls.OutletWaterTheftIncident.RequestedPage = requestForPage;
        }
        private void DisplaySDOWorkingFields()
        {
            if ((int)PMIU.WRMIS.Web.Modules.WaterTheft.Controls.ChannelWaterTheftIncident.RequestForPage.SDO == requestForPage)
            {
                DivHideClosingRepairDateField.Visible = false; // Hide Closing/Repair date field
                btnAssignToSDO.Visible = false; // Hide Assign to SDO button
                pnlIsSDOWorking.Visible = true;
            }
            else
            {
                DivHideClosingRepairDateField.Visible = true; // Display Closing/Repair date field
                btnAssignToSDO.Visible = true; // Display Assign to SDO button
                pnlIsSDOWorking.Visible = false;
            }
        }
        private void LoadWaterTheftUserControl(ControlToLoad _ControlToLoad = ControlToLoad.Channel)
        {
            string controlPath = string.Empty;
            switch (_ControlToLoad)
            {
                case ControlToLoad.Channel:
                    controlPath = BasePath + "ChannelWaterTheftIncident.ascx";
                    break;
                case ControlToLoad.Outlet:
                    controlPath = BasePath + "OutletWaterTheftIncident.ascx";
                    break;
            }

            WaterTheftIncidentInformation.Controls.Clear();
            UserControl userControl = (UserControl)LoadControl(controlPath);
            WaterTheftIncidentInformation.Controls.Add(userControl);
            this.CurrentControl = controlPath;
        }
        protected void btnAssignToSDO_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
                int isAssigned = new WaterTheftBLL().AssignWaterTheftCaseToSDO(WTCanalWire, txtClosingRepairDate.Text);
                if (isAssigned == 1)
                    Master.ShowMessage(Message.RecordSaved.Description);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }
        private WT_WaterTheftCanalWire GetWTCanalWireEntity()
        {
            WT_WaterTheftCanalWire WTCanalWire = new WT_WaterTheftCanalWire();
            WTCanalWire.WaterTheftID = waterTheftlID;
            WTCanalWire.CanalWireNo = Convert.ToString(txtCanalWireNo.Text);
            WTCanalWire.CanalWireDate = Utility.GetParsedDate(txtCanalWireDate.Text);
            WTCanalWire.Remarks = txtComments.Text;
            WTCanalWire.UserID = Convert.ToInt64(Session[SessionValues.UserID]);
            return WTCanalWire;
        }

        protected void btnMarkCaseNA_Click(object sender, EventArgs e)
        {
            try
            {
                bool isMarkedASNA = new WaterTheftBLL().MarkCaseASNA(waterTheftlID);
                if (isMarkedASNA)
                    Master.ShowMessage(Message.RecordSaved.Description);
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void btnAssignToZiladar_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }
    }
}