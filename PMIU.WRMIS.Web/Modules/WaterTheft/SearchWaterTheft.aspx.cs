using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class SearchWaterTheft : BasePage
    {
        #region Hash Table Keys

        public const string CaseIDKey = "CaseID";
        public const string CanalWireNoKey = "CanalWire";
        public const string OffenceSiteKey = "OffenceSite";
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string DivisionIDKey = "DivisionID";
        public const string ChannelIDKey = "ChannelID";
        public const string StatusIDKey = "StatusID";
        public const string AssignedToIDKey = "AssignedToID";
        public const string OffenceTypeKey = "OffenceType";
        public const string PageIndexKey = "PageIndex";

        #endregion

        #region Grid Data Key Index

        public const int WaterTheftIDIndex = 0;
        public const int AssignedByDesignationIDIndex = 1;
        public const int AssignedToDesignationIDIndex = 2;
        public const int StatusIDIndex = 3;

        #endregion

        #region Redirect Urls

        public const string SDOSBEUrl = "~/Modules/WaterTheft/WTSBESDOWorking.aspx?WaterTheftID={0}";
        public const string SDOTawaanXENSECheifUrl = "~/Modules/WaterTheft/WaterTheftIncidentWorking.aspx?WaterTheftID={0}&AA={1}";
        public const string ZiladaarUrl = "~/Modules/WaterTheft/ZiladaarOutletWorking.aspx?WaterTheftID={0}&OS={1}";

        #endregion

        #region Screen Constants

        public const bool View = true;
        public const bool Working = false;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();

                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    BindDivisionDropdown(mdlUser);
                    BindChannelDropdown(mdlUser);
                    BindStatusDropdown();
                    BindAssignedToDropdown();

                    ShowHideCaseIDTextBox(mdlUser);
                    ShowHideCanalWireTextBox(mdlUser);

                    string Now = Utility.GetFormattedDate(DateTime.Now);
                    string Yesterday = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));

                    txtFromDate.Text = Yesterday;
                    txtToDate.Text = Now;

                    this.Form.DefaultButton = btnSearch.UniqueID;

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchWaterTheftCriteria] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowMessage"]))
                    {
                        bool ShowMessage = Convert.ToBoolean(Request.QueryString["ShowMessage"]);

                        if (ShowMessage)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        }
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 06-05-2016
        /// </summary>        
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchWaterTheft);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the Division dropdown
        /// Created on 06-05-2016
        /// </summary>
        ///<param name="_MdlUser"></param>
        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);

                ddlDivision.Enabled = false;
            }
            else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
            else
            {
                Dropdownlist.DDLDivisions(ddlDivision, false, -1, -1, (int)Constants.DropDownFirstOption.All);
            }
        }

        /// <summary>
        /// This function binds the Channel dropdown
        /// Created on 10-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>
        private void BindChannelDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                ddlChannel.Enabled = true;

                long DivisionID = -1;

                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.All);
            }
        }

        /// <summary>
        /// This function binds the Status dropdown
        /// Created on 10-05-2016
        /// </summary>
        private void BindStatusDropdown()
        {
            Dropdownlist.DDLWaterTheftStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds the Assign To dropdown
        /// Created on 17-05-2016
        /// </summary>
        private void BindAssignedToDropdown()
        {
            ddlAssignto.Items.Add(new ListItem("All", ""));
            ddlAssignto.Items.Add(new ListItem(Constants.Designation.SBE.ToString(), ((long)Constants.Designation.SBE).ToString()));
            ddlAssignto.Items.Add(new ListItem(Constants.Designation.SDO.ToString(), ((long)Constants.Designation.SDO).ToString()));
            ddlAssignto.Items.Add(new ListItem(Constants.Designation.Ziladaar.ToString(), ((long)Constants.Designation.Ziladaar).ToString()));
            ddlAssignto.Items.Add(new ListItem(Constants.Designation.XEN.ToString(), ((long)Constants.Designation.XEN).ToString()));
        }

        /// <summary>
        /// This function binds the history data from session.
        /// Created On 20-05-2016
        /// </summary>
        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchWaterTheftCriteria];

            txtCaseID.Text = (string)SearchCriteria[CaseIDKey];
            txtCanalWire.Text = (string)SearchCriteria[CanalWireNoKey];
            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];

            string DivisionID = (string)SearchCriteria[DivisionIDKey];
            string ChannelID = (string)SearchCriteria[ChannelIDKey];

            if (DivisionID != string.Empty && ddlDivision.Enabled == true)
            {
                ddlDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);

                ddlDivision_SelectedIndexChanged(null, null);

                if (ChannelID != string.Empty)
                {
                    ddlChannel.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlChannel, ChannelID);
                }
            }
            else if (ddlDivision.Enabled == false && ChannelID != string.Empty)
            {
                ddlChannel.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlChannel, ChannelID);
            }

            string StatusID = (string)SearchCriteria[StatusIDKey];

            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }

            string AssignedToID = (string)SearchCriteria[AssignedToIDKey];

            if (AssignedToID != String.Empty)
            {
                ddlAssignto.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlAssignto, AssignedToID);
            }

            string OffenceSite = (string)SearchCriteria[OffenceSiteKey];

            if (OffenceSite != String.Empty)
            {
                ddlOffenceSite.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlOffenceSite, OffenceSite);

                ddlOffenceSite_SelectedIndexChanged(null, null);

                string TypeOfOffence = (string)SearchCriteria[OffenceTypeKey];

                if (TypeOfOffence != String.Empty)
                {
                    ddlTypeofOffence.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlTypeofOffence, TypeOfOffence);
                }
            }

            gvSearchWaterTheft.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindSearchResultsGrid();
        }

        /// <summary>
        /// This function shows or hides the Canal Wire div and adjusts the Label accordingly.
        /// Created On 20-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>
        public void ShowHideCanalWireTextBox(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SDO)
            {
                dvCanalWire.Visible = true;
                lblCanalWire.Text = string.Format("{0} Canal Wire #", Constants.Designation.SBE.ToString());
            }
            else if (_MdlUser.DesignationID == (long)Constants.Designation.Ziladaar || _MdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                dvCanalWire.Visible = true;
                lblCanalWire.Text = string.Format("{0} Canal Wire #", Constants.Designation.SDO.ToString());
            }
            else if (_MdlUser.DesignationID == (long)Constants.Designation.SE || _MdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
            {
                dvCanalWire.Visible = true;
                lblCanalWire.Text = string.Format("{0} Canal Wire #", Constants.Designation.XEN.ToString());
            }
            else
            {
                dvCanalWire.Visible = false;
            }
        }

        /// <summary>
        /// This function shows or hides the Case ID div.
        /// Created On 23-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>
        public void ShowHideCaseIDTextBox(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                dvCaseID.Visible = false;
                txtCanalWire.Focus();
            }
            else
            {
                dvCaseID.Visible = true;
                txtCaseID.Focus();
            }
        }

        protected void btnIncidentSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindSearchResultsGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long SelectedDivisionID = -1;
                long SelectedChannelID = -1;
                long SelectedStatusID = -1;
                long SelectedAssigntoID = -1;
                long SelectedOffenceTypeID = -1;
                long CanalWireDesignationID = -1;

                DateTime? FromDate = null;
                DateTime? ToDate = null;

                string EnteredCaseID = txtCaseID.Text.Trim();
                SearchCriteria.Add(CaseIDKey, EnteredCaseID);

                string EnteredCanalWireNo = txtCanalWire.Text.Trim();
                SearchCriteria.Add(CanalWireNoKey, EnteredCanalWireNo);

                string SelectedOffenceSite = ddlOffenceSite.SelectedItem.Value;
                SearchCriteria.Add(OffenceSiteKey, SelectedOffenceSite);

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }

                if (ddlDivision.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlChannel.SelectedItem.Value != String.Empty)
                {
                    SelectedChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                }

                SearchCriteria.Add(ChannelIDKey, ddlChannel.SelectedItem.Value);

                if (ddlStatus.SelectedItem.Value != String.Empty)
                {
                    SelectedStatusID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                }

                SearchCriteria.Add(StatusIDKey, ddlStatus.SelectedItem.Value);

                if (ddlAssignto.SelectedItem.Value != String.Empty)
                {
                    SelectedAssigntoID = Convert.ToInt64(ddlAssignto.SelectedItem.Value);
                }

                SearchCriteria.Add(AssignedToIDKey, ddlAssignto.SelectedItem.Value);

                if (ddlTypeofOffence.SelectedItem.Value != String.Empty)
                {
                    SelectedOffenceTypeID = Convert.ToInt64(ddlTypeofOffence.SelectedItem.Value);
                }

                SearchCriteria.Add(OffenceTypeKey, ddlTypeofOffence.SelectedItem.Value);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.DesignationID == (long)Constants.Designation.SDO)
                {
                    CanalWireDesignationID = (long)Constants.Designation.SBE;
                }
                else if (mdlUser.DesignationID == (long)Constants.Designation.Ziladaar || mdlUser.DesignationID == (long)Constants.Designation.XEN)
                {
                    CanalWireDesignationID = (long)Constants.Designation.SDO;
                }
                else if (mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                {
                    CanalWireDesignationID = (long)Constants.Designation.XEN;
                }

                IEnumerable<DataRow> ieWaterTheftCases = new WaterTheftBLL().GetWaterTheftCases(mdlUser.ID, (mdlUser.DesignationID == null ? null : mdlUser.UA_Designations.IrrigationLevelID),
                        SelectedDivisionID, SelectedChannelID, SelectedStatusID, SelectedAssigntoID, SelectedOffenceTypeID, SelectedOffenceSite,
                        EnteredCaseID, FromDate, ToDate, EnteredCanalWireNo, CanalWireDesignationID);

                var lstWaterTheftCases = ieWaterTheftCases.Select(dataRow => new
                {
                    ID = dataRow.Field<long>("ID"),
                    Channel = dataRow.Field<string>("Channel"),
                    TypeOfOffence = dataRow.Field<string>("TypeOfOffence"),
                    ReducedDistance = dataRow.Field<int>("ReducedDistance"),
                    OffenceSite = dataRow.Field<string>("OffenceSite"),
                    IncidentDateTime = dataRow.Field<DateTime>("IncidentDateTime"),
                    Status = dataRow.Field<string>("Status"),
                    AssignedToDesignation = dataRow.Field<string>("AssignedToDesignation"),
                    AssignedByDesignationID = dataRow.Field<long>("AssignedByDesignationID"),
                    CaseNo = dataRow.Field<string>("CaseNo"),
                    AssignedToDesignationID = dataRow.Field<long>("AssignedToDesignationID"),
                    StatusID = dataRow.Field<long>("StatusID")
                }).ToList();

                gvSearchWaterTheft.DataSource = lstWaterTheftCases;
                gvSearchWaterTheft.DataBind();

                SearchCriteria.Add(PageIndexKey, gvSearchWaterTheft.PageIndex);

                Session[SessionValues.SearchWaterTheftCriteria] = SearchCriteria;
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
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    ddlChannel.Enabled = true;

                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, (mdlUser.DesignationID == null ? null : mdlUser.UA_Designations.IrrigationLevelID), DivisionID, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    ddlChannel.SelectedIndex = 0;

                    ddlChannel.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchWaterTheft_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblReducedDistance = (Label)e.Row.FindControl("lblReducedDistance");
                    lblReducedDistance.Text = Calculations.GetRDText(Convert.ToDouble(lblReducedDistance.Text));

                    Label lblOffenceSite = (Label)e.Row.FindControl("lblOffenceSite");
                    ListItem liOffenceSite = ddlOffenceSite.Items.FindByValue(lblOffenceSite.Text);
                    lblOffenceSite.Text = liOffenceSite.Text;

                    Label lblIncidentDate = (Label)e.Row.FindControl("lblIncidentDate");
                    DateTime Date = DateTime.Parse(lblIncidentDate.Text);
                    lblIncidentDate.Text = Utility.GetFormattedDate(Date);

                    Label lblAssignedTo = (Label)e.Row.FindControl("lblAssignedTo");
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    long WaterTheftID = Convert.ToInt64(gvSearchWaterTheft.DataKeys[e.Row.RowIndex].Values[WaterTheftIDIndex]);
                    long AssignedByDesignationID = Convert.ToInt64(gvSearchWaterTheft.DataKeys[e.Row.RowIndex].Values[AssignedByDesignationIDIndex]);
                    long AssignedToDesignationID = Convert.ToInt64(gvSearchWaterTheft.DataKeys[e.Row.RowIndex].Values[AssignedToDesignationIDIndex]);
                    long StatusID = Convert.ToInt64(gvSearchWaterTheft.DataKeys[e.Row.RowIndex].Values[StatusIDIndex]);

                    if (mdlUser.DesignationID != null && AssignedToDesignationID == mdlUser.DesignationID && StatusID != (long)Constants.WTCaseStatus.Closed
                        && StatusID != (long)Constants.WTCaseStatus.NA)
                    {
                        lblAssignedTo.Attributes.Add("style", "color:red;");
                    }

                    HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");

                    int OS = (liOffenceSite.Value == "C") ? 1 : 2;

                    if (mdlUser.DesignationID == (long)Constants.Designation.SBE)
                    {
                        if (AssignedToDesignationID == (long)Constants.Designation.SBE)
                        {
                            hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                        }
                        else
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.SDO)
                    {
                        if (AssignedToDesignationID == (long)Constants.Designation.SDO)
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                        else
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE || AssignedToDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
                    {
                        if (AssignedToDesignationID == (long)Constants.Designation.Ziladaar)
                        {
                            hlView.NavigateUrl = string.Format(ZiladaarUrl, WaterTheftID, OS);
                        }
                        else
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE || AssignedToDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                    {
                        if (AssignedToDesignationID == (long)Constants.Designation.XEN)
                        {
                            hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                        }
                        else
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE || AssignedToDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                    {
                        if (StatusID == (long)Constants.WTCaseStatus.Closed)
                        {
                            hlView.Visible = false;

                            HyperLink hlAppeal = (HyperLink)e.Row.FindControl("hlAppeal");
                            HyperLink hlAssignBack = (HyperLink)e.Row.FindControl("hlAssignBack");
                            hlAppeal.Visible = true;
                            hlAssignBack.Visible = true;

                            hlAppeal.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 1);
                            hlAssignBack.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 2);
                        }
                        else
                        {
                            if (AssignedByDesignationID == (long)Constants.Designation.SBE || AssignedToDesignationID == (long)Constants.Designation.SBE)
                            {
                                hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                            }
                            else
                            {
                                hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                            }
                        }
                    }
                    else
                    {
                        if (AssignedByDesignationID == (long)Constants.Designation.SBE || AssignedToDesignationID == (long)Constants.Designation.SBE)
                        {
                            hlView.NavigateUrl = string.Format(SDOSBEUrl, WaterTheftID);
                        }
                        else
                        {
                            hlView.NavigateUrl = string.Format(SDOTawaanXENSECheifUrl, WaterTheftID, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchWaterTheft_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchWaterTheft.PageIndex = e.NewPageIndex;

                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlOffenceSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlOffenceSite.SelectedItem.Value != string.Empty)
                {
                    ddlTypeofOffence.Enabled = true;

                    string OffenceSite = ddlOffenceSite.SelectedItem.Value;

                    Dropdownlist.DDLOffenceType(ddlTypeofOffence, OffenceSite, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    ddlTypeofOffence.SelectedIndex = 0;

                    ddlTypeofOffence.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                long ID = Convert.ToInt64(gvSearchWaterTheft.DataKeys[gvrCurrent.RowIndex].Values[WaterTheftIDIndex]);
                Label lblOffenceSite = (Label)gvrCurrent.FindControl("lblOffenceSite");

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("ID", ID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                if (lblOffenceSite.Text.ToUpper() == "Channel".ToUpper())
                {
                    mdlReportData.Name = Constants.WaterTheftChannelCase;
                }
                else
                {
                    mdlReportData.Name = Constants.WaterTheftOutletCase;
                }

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}