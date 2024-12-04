using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class DailyOperationalData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    BindDropdownlists();
                    EnableControls();
                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //Constants.SessionOrShift session = Utility.GetSession(DateTime.Now);

                    //Dropdownlist.SetSelectedValue(ddlSession, Convert.ToString((int)session));
                    Dropdownlist.SetSelectedValue(ddlSession, Convert.ToString((int)Constants.SessionOrShift.Morning));
                }
                catch (Exception ex)
                {
                    new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyOperationalData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void AddRequiredAttribute(WebControl _controlID)
        {
            _controlID.Attributes.Add("required", "required");
        }
        private void RemoveRequiredAttribute(WebControl _ControlID)
        {
            _ControlID.Attributes.Remove("required");
        }
        private void AddClass(WebControl _ControlID)
        {
            _ControlID.CssClass += " required";
        }
        private void EnableControls()
        {
            switch (SessionManagerFacade.UserInformation.DesignationID)
            {
                case (long)Constants.Designation.DataAnalyst:
                case (long)Constants.Designation.ChiefMonitoring:
                    ddlZone.Enabled = true;
                    ddlCircle.Enabled = true;
                    ddlDivision.Enabled = true;
                    ddlSubDivision.Enabled = true;
                    break;
                case (long)Constants.Designation.ChiefIrrigation:
                    ddlCircle.Enabled = true;
                    ddlDivision.Enabled = true;
                    ddlSubDivision.Enabled = true;
                    break;
                case (long)Constants.Designation.SE:
                    ddlDivision.Enabled = true;
                    ddlSubDivision.Enabled = true;
                    break;
                case (long)Constants.Designation.XEN:
                case (long)Constants.Designation.MA:
                case (long)Constants.Designation.ADM:
                    ddlSubDivision.Enabled = true;
                    break;
                default:
                    ddlZone.Enabled = false;
                    ddlCircle.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlSubDivision.Enabled = false;
                    break;
            }
        }
        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                UA_AssociatedLocation userLocation = SessionManagerFacade.UserAssociatedLocations;
                switch (SessionManagerFacade.UserInformation.DesignationID)
                {
                    case (long)Constants.Designation.SDO:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.SubDivision == userLocation.IrrigationLevelID)
                        {
                            LoadAllDropdownlistsData(new SubDivisionBLL().GetByID(userLocation.IrrigationBoundryID.Value).DivisionID.Value);
                            Dropdownlist.SetSelectedValue(ddlSubDivision, Convert.ToString(userLocation.IrrigationBoundryID.Value));
                        }
                        break;
                    case (long)Constants.Designation.XEN:
                    case (long)Constants.Designation.MA:
                    case (long)Constants.Designation.ADM:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Division == userLocation.IrrigationLevelID)
                            LoadAllDropdownlistsData(userLocation.IrrigationBoundryID.Value);
                        break;
                    case (long)Constants.Designation.DataAnalyst:
                    case (long)Constants.Designation.ChiefMonitoring:
                        Dropdownlist.DDLZones(ddlZone);
                        DDLEmptyCircleDivisionSubDivision();
                        break;
                    case (long)Constants.Designation.ChiefIrrigation:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Zone == userLocation.IrrigationLevelID)
                        {
                            // Bind Zone dropdownlist 
                            Dropdownlist.DDLZones(ddlZone);
                            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(userLocation.IrrigationBoundryID.Value));
                            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value));
                            DDLEmptyDivisionSubDivision();
                        }
                        break;
                    case (long)Constants.Designation.SE:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Circle == userLocation.IrrigationLevelID)
                        {
                            CO_Circle circle = new CircleBLL().GetByID(userLocation.IrrigationBoundryID.Value);
                            // Bind Zone dropdownlist 
                            Dropdownlist.DDLZones(ddlZone);
                            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
                            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value));
                            Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
                            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value));
                            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, -1, (int)Constants.DropDownFirstOption.All);
                        }
                        break;
                }
                Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetSession());
                Dropdownlist.BindDropdownlist<List<CO_ReasonForChange>>(ddlReasonForChange, new DailyDataBLL().GetReasonForChange());

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void LoadAllDropdownlistsData(long _DivisionID)
        {
            CO_Division division = new DivisionBLL().GetByID(_DivisionID);
            CO_Circle circle = new CircleBLL().GetByID(division.CircleID.Value);
            // Bind Zone dropdownlist 
            Dropdownlist.DDLZones(ddlZone);
            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value));
            Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value));
            Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(division.ID));
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyCircleDivisionSubDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset division dropdownlist
                Dropdownlist.DDLDivisions(ddlDivision, true);
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        protected void btnLoadDailyData_Click(object sender, EventArgs e)
        {
            try
            {
                string a = hdnSession.Value;
                gvDailyOperationalData.PageIndex = 0;
                gvDailyOperationalData.EditIndex = -1;
                BindDailyOperationalDataGrid(0);
                //if (Utility.GetParsedDate(txtDate.Text) == DateTime.Now.Date)
                //    ddlSession.Enabled = false;
                //else
                //    ddlSession.Enabled = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindDailyOperationalDataGrid(int _PageIndex)
        {
            try
            {
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                long SessionID = ddlSession.SelectedItem.Value == string.Empty ? 1 : Convert.ToInt16(ddlSession.SelectedItem.Value);
                string ChannelName = txtChannelName.Text;

                List<dynamic> lstDailyOperationalData = new DailyDataBLL().GetDailyGaugeReadingData(ZoneID
                    , CircleID
                    , DivisionID
                    , SubDivisionID
                    , ChannelName
                    , Utility.GetParsedDate(txtDate.Text)
                    , Convert.ToInt16(SessionID)
                    , (_PageIndex + 1)
                    , 10);

                if (lstDailyOperationalData.Count != 0)
                {
                    dynamic dailyData = lstDailyOperationalData[0];
                    string TotalRecords = Convert.ToString(dailyData.GetType().GetProperty("TotalRecords").GetValue(dailyData, null));

                    gvDailyOperationalData.VirtualItemCount = Convert.ToInt16(TotalRecords);
                }

                gvDataCount = lstDailyOperationalData.Count;

                gvDailyOperationalData.DataSource = lstDailyOperationalData;
                gvDailyOperationalData.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvDailyOperationalData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            long designationID = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvDailyOperationalData.DataKeys[e.Row.RowIndex];
                    string isCurrent = Convert.ToString(key["IsCurrent"]);
                    Button btnViewGaugeImage = (Button)e.Row.FindControl("btnViewGaugeImage");
                    Button btnAuditTrail = (Button)e.Row.FindControl("btnAuditTrail");
                    Button btnEditGauge = (Button)e.Row.FindControl("btnEditGauge");

                    if (Convert.ToBoolean(isCurrent) == false)
                        e.Row.CssClass = "myAltRowClass";

                    if ((SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN
                        || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DataAnalyst
                        || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ChiefMonitoring)
                        && string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                        e.Row.Cells[0].Visible = true;
                    else
                        e.Row.Cells[0].Visible = false;


                    if (Convert.ToInt64(key["DailyGaugeReadingID"]) == 0 || Convert.ToString(key["Close"]).Equals("Yes"))
                    {
                        if (btnViewGaugeImage != null)
                            btnViewGaugeImage.CssClass += " disabled";
                        if (btnAuditTrail != null)
                            btnAuditTrail.CssClass += " disabled";
                        if (btnEditGauge != null)
                            btnEditGauge.CssClass += " disabled";
                    }

                    long ChiefMonitoringID = 0;
                    designationID = (long)Constants.Designation.DataAnalyst;
                    ChiefMonitoringID = (long)Constants.Designation.ChiefMonitoring;
                    Constants.SessionOrShift userSession = (Constants.SessionOrShift)(Convert.ToInt32(ddlSession.SelectedItem.Value));
                    Constants.SessionOrShift systemSession = Utility.GetSession(DateTime.Now);
                    DateTime currentDate = Utility.GetParsedDate(txtDate.Text);
                    DateTime systemDate = DateTime.Now.Date;
                    //If a previous day is selected along with a Session, all the relevant records will be shown in read-only mode with no Edit Icon
                    if (currentDate == systemDate && (int)systemSession == (int)userSession && base.CanEdit == true)
                    {
                        if (designationID == SessionManagerFacade.UserInformation.DesignationID
                                && (designationID != Convert.ToInt64(key["DesignationID"])
                                || designationID == Convert.ToInt64(key["DesignationID"])))
                            btnEditGauge.Visible = true;
                        else if (designationID != SessionManagerFacade.UserInformation.DesignationID &&
                                designationID != Convert.ToInt64(key["DesignationID"]))
                            btnEditGauge.Visible = true;
                    }
                    else if (designationID == SessionManagerFacade.UserInformation.DesignationID || ChiefMonitoringID == SessionManagerFacade.UserInformation.DesignationID)
                    {
                        btnEditGauge.Visible = true;
                    }
                    else
                        btnEditGauge.Visible = false;

                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (SessionManagerFacade.UserInformation.DesignationID != (long)Constants.Designation.ChiefIrrigation &&
                            SessionManagerFacade.UserInformation.DesignationID != (long)Constants.Designation.SE &&
                                string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                    {
                        e.Row.Cells[0].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDailyOperationalData_PreRender(object sender, EventArgs e)
        {
            try
            {
                MergeGridviewRows(gvDailyOperationalData);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void MergeGridviewRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                if (((Label)row.FindControl("lblSubDivisionName")).Text == ((Label)previousRow.FindControl("lblSubDivisionName")).Text)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 : previousRow.Cells[0].RowSpan + 1;
                    row.Cells[0].Attributes.CssStyle.Add("vertical-align", "middle");
                    previousRow.Cells[0].Visible = false;

                }
                if (((Label)row.FindControl("lblChannalName")).Text == ((Label)previousRow.FindControl("lblChannalName")).Text
                    && ((Label)row.FindControl("lblClose")).Text == ((Label)previousRow.FindControl("lblClose")).Text)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 : previousRow.Cells[1].RowSpan + 1;
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
                    row.Cells[1].Attributes.CssStyle.Add("vertical-align", "middle");
                    row.Cells[2].Attributes.CssStyle.Add("vertical-align", "middle");
                    previousRow.Cells[1].Visible = false;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
        protected void gvDailyOperationalData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDailyOperationalData.PageIndex = e.NewPageIndex;

                BindDailyOperationalDataGrid(e.NewPageIndex);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDailyOperationalData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvDailyOperationalData.DataKeys[gvrow.RowIndex];
                if (e.CommandName.Equals("AuditTrail"))
                {
                    lblChannelName.Text = Convert.ToString(key["channelNameForAuditTrail"]);
                    lblGuageType.Text = Convert.ToString(((Label)gvrow.FindControl("lblGaugeName")).Text);
                    lblGuageRD.Text = Convert.ToString(((Label)gvrow.FindControl("lblRDs")).Text);
                    lblSection.Text = Convert.ToString(((Label)gvrow.FindControl("lblSectionName")).Text);
                    lblSession.Text = Convert.ToString(ddlSession.SelectedItem.Text);

                    List<object> lstAuditTrail = new DailyDataBLL().GetAuditTrail(Utility.GetParsedDate(txtDate.Text)
                , Convert.ToInt16(ddlSession.SelectedItem.Value)
                , Convert.ToInt64(key["DailyGaugeReadingID"])
                , 1
                , 10);

                    gvAuditTrail.DataSource = lstAuditTrail;
                    gvAuditTrail.DataBind();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#auditTrail').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "auditTrailModalScript", sb.ToString(), false);
                }
                if (e.CommandName.Equals("EditGauge"))
                {
                    long ChannelID = Convert.ToInt64(key["ChannelID"]);
                    Tuple<double?, double?> tplChannelTypeMinMaxValue = new DailyDataBLL().GetChannelTypeMinMaxValueByChannelID(ChannelID);
                    double minValue = tplChannelTypeMinMaxValue.Item1.HasValue ? tplChannelTypeMinMaxValue.Item1.Value : 0;
                    double maxValue = tplChannelTypeMinMaxValue.Item2.HasValue ? tplChannelTypeMinMaxValue.Item2.Value : 0;
                    txtNewGaugeValue.Attributes.Add("placeholder", Convert.ToString(minValue) + " - " + Convert.ToString(maxValue));
                    lblCurrentGaugeValue.Text = Convert.ToString(((Label)gvrow.FindControl("lblGaugeValue")).Text);
                    txtNewGaugeValue.Text = string.Empty;
                    ddlReasonForChange.SelectedIndex = 0;
                    hdnGaugeID.Value = Convert.ToString(key["GaugeID"]);
                    hdnGaugeReadingID.Value = Convert.ToString(key["DailyGaugeReadingID"]);
                    bool OffTakes = new DailyDataBLL().HasOffTake(Convert.ToInt64(hdnGaugeID.Value));
                    if (OffTakes)
                    {
                        //divNewGauge.Visible = false;
                        Master.ShowMessage(Message.MultipleOffTakes.Description, SiteMaster.MessageType.Error);
                    }
                    else
                    {
                        txtNewGaugeValue.Attributes.Add("required", "required");
                        ddlReasonForChange.Attributes.Add("required", "required");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#editvalue').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "editvalueModalScript", sb.ToString(), false);
                    }
                }
                if (e.CommandName.Equals("ViewGaugeImage"))
                {
                    //gvDailyOperationalData.PageIndex = gvrow.RowIndex;
                    lblUploadedBy.Text = Convert.ToString(key["SubmittedBy"]);
                    lblDateTime.Text = Utility.GetFormattedDate(Convert.ToDateTime(Convert.ToString(key["ReadingDate"]))) + " " + Utility.GetFormattedTime(Convert.ToDateTime(Convert.ToString(key["ReadingDate"])));
                    lblGISX.Text = Convert.ToString(key["GIS_X"]);
                    lblGISY.Text = Convert.ToString(key["GIS_Y"]);
                    imgGaugeImage.ImageUrl = Utility.GetImageURL(Configuration.DailyData, Convert.ToString(key["GaugePhoto"]));
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#viewimage').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScript", sb.ToString(), false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuditTrail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvAuditTrail.DataKeys[e.Row.RowIndex];
                    string isCurrent = Convert.ToString(key["IsCurrent"]);

                    if (Convert.ToBoolean(isCurrent) == false)
                        e.Row.CssClass = "myAltRowClass";
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSaveGaugeValue_Click(object sender, EventArgs e)
        {
            try
            {
                object Limits = new DailyDataBLL().GetChannelLimit(Convert.ToInt64(hdnGaugeID.Value));
                if (Limits != null)
                {
                    double? MinValue = 0;
                    double? MaxValue = 0;

                    if (Limits.GetType().GetProperty("MinValue").GetValue(Limits) != null)
                        MinValue = Convert.ToDouble(Limits.GetType().GetProperty("MinValue").GetValue(Limits));

                    if (Limits.GetType().GetProperty("MaxValue").GetValue(Limits) != null)
                        MaxValue = Convert.ToDouble(Limits.GetType().GetProperty("MaxValue").GetValue(Limits));

                    if (!string.IsNullOrEmpty(txtNewGaugeValue.Text) && (Convert.ToDouble(txtNewGaugeValue.Text) >= MinValue && Convert.ToDouble(txtNewGaugeValue.Text) <= MaxValue))
                    {
                        if (!string.IsNullOrEmpty(ddlReasonForChange.SelectedItem.Value))
                        {
                            double? discharge = new DailyDataBLL().CalculateDischarge(Convert.ToInt64(hdnGaugeID.Value), Convert.ToDouble(txtNewGaugeValue.Text));

                            if (discharge != null)
                            {

                                long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);

                                bool result = new DailyDataBLL().UpdateDischarge(Convert.ToInt64(hdnGaugeReadingID.Value), Convert.ToDouble(discharge), Convert.ToInt64(ddlReasonForChange.SelectedItem.Value), Convert.ToDouble(Convert.ToDouble(txtNewGaugeValue.Text)), LoggedUser);
                                if (result)
                                {
                                    double? DefaultDesignDisc = new DailyDataBLL().GetDefaultDesignDischarge(Convert.ToInt64(hdnGaugeID.Value));
                                    if (DefaultDesignDisc != null)
                                    {
                                        double DesignDischargePercentage = Convert.ToDouble((DefaultDesignDisc * 115) / 100);

                                        if (discharge > DesignDischargePercentage)
                                        {
                                            Master.ShowMessage(Message.ExcessiveDischarge.Description, SiteMaster.MessageType.Warning);
                                        }
                                    }
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append(@"<script type='text/javascript'>");
                                    sb.Append("$('#editvalue').modal('hide');");
                                    sb.Append(@"</script>");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "editvalueModalScript", sb.ToString(), false);

                                    long _DailyGaugeReadingID = Convert.ToInt64(hdnGaugeReadingID.Value);

                                    NotifyEvent _event = new NotifyEvent();
                                    _event.Parameters.Add("DailyGaugeReadingID", _DailyGaugeReadingID);
                                    _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.EditDailyData, SessionManagerFacade.UserInformation.ID);

                                    Master.ShowMessage(Message.RecordSaved.Description);
                                    BindDailyOperationalDataGrid(gvDailyOperationalData.PageIndex);
                                }
                            }
                            else
                            {
                                Master.ShowMessage(Message.DischargeCannotBeCalculated.Description, SiteMaster.MessageType.Error);
                            }
                        }
                        else
                            Master.ShowMessage(Message.ReasonForChange.Description, SiteMaster.MessageType.Error);
                    }
                    else
                        Master.ShowMessage("Gauge value should be between " + MinValue.ToString() + " to " + MaxValue.ToString(), SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDailyOperationalData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                if (e.Row.RowIndex > this.gvDataCount)
                    e.Row.Visible = false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private int gvDataCount
        {
            get
            {
                object count = ViewState["gvDataCount"];
                if (count == null)
                    count = gvDailyOperationalData.PageSize;
                return Convert.ToInt32(count);
            }
            set
            {
                ViewState["gvDataCount"] = value;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.Items.Count > 0)
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                    int SessionID = Convert.ToInt32(ddlSession.SelectedItem.Value);

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());

                    ReportData mdlReportData = new ReportData();

                    ReportParameter ReportParameter = new ReportParameter("Date", SelectedDate.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("Session", SessionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("ZoneID", ZoneID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("CircleID", CircleID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("DivisionID", DivisionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("SubDivisionID", SubDivisionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.DailyGaugeData;

                    Session[SessionValues.ReportData] = mdlReportData;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}