using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.ComplaintsManagement;
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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement
{
    public partial class SearchComplaints : BasePage
    {
        #region Hash Table Keys

        public const string ComplaintIDKey = "ComplaintNumber";
        public const string ComplainantNameKey = "ComplainantName";
        public const string ComplainantCellKey = "ComplainantCell";
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string DivisionIDKey = "DivisionID";
        public const string ChannelIDKey = "ChannelID";
        public const string StatusIDKey = "StatusID";
        public const string DomainIDKey = "DomainID";
        public const string OffenceTypeKey = "OffenceType";
        public const string PageIndexKey = "PageIndex";
        public const string ComplaintSourceKey = "ComplaintSource";
        public const string ActionIDKey = "ActionID";
        public const string ComplaintTypeIDKey = "ComplaintTypeID";
        public const string chkboxOtherThenPMIUStaffKey = "chkboxOtherThenPMIUStaff";

        #endregion
        public const int ComplaintIDIndex = 0;
        double ComplaintsCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    btnBulkActivity.Visible = false;
                    SetTitle();
                    hlAddComplaint.NavigateUrl = "~/Modules/ComplaintsManagement/AddComplaint.aspx?FromSearchComplaint=true";
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindDomainDropdown(mdlUser);
                    BindDropdowns();

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchComplaintCriteria] != null)
                            {
                                BindHistoryData();
                            }
                        }
                        else
                        {
                            DateTime CurrentDate = DateTime.Now;
                            txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                            txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-30)));
                            // BindSearchResultsGrid();
                        }
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                    {
                        string Status = Request.QueryString["Status"];
                        string ToDate = Request.QueryString["ToDate"];
                        string FromDate = Request.QueryString["FromDate"];
                        ddlStatus.SelectedIndex = Convert.ToInt32(Status);
                        txtFromDate.Text = FromDate;
                        txtToDate.Text = ToDate;
                        BindSearchResultsGrid();
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["ComplaintTypeStatus"]))
                    {
                        string ComplaintTypeID = Request.QueryString["ComplaintTypeStatus"];
                        string ToDate = Request.QueryString["ToDate"];
                        string FromDate = Request.QueryString["FromDate"];
                        string Status = Request.QueryString["CompStatus"];
                        ddlStatus.SelectedIndex = Convert.ToInt32(Status);
                        ddlComplaintType.SelectedIndex = Convert.ToInt32(ComplaintTypeID);
                        txtFromDate.Text = FromDate;
                        txtToDate.Text = ToDate;
                        BindSearchResultsGrid();
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSource"]))
                    {
                        string ComplaintSource = Request.QueryString["ComplaintSource"];
                        long ComplaintSourceID = 0;
                        if (ComplaintSource == "DH")
                            ComplaintSourceID = (long)Constants.ComplaintSource.DefaultHelpline;
                        else if (ComplaintSource == "AG")
                            ComplaintSourceID = (long)Constants.ComplaintSource.Automatic;
                        else if (ComplaintSource == "CM")
                            ComplaintSourceID = (long)Constants.ComplaintSource.ChiefMinister;
                        else if (ComplaintSource == "CS")
                            ComplaintSourceID = (long)Constants.ComplaintSource.ChiefSecretary;
                        else if (ComplaintSource == "MI")
                            ComplaintSourceID = (long)Constants.ComplaintSource.MinisterIrrigation;
                        else if (ComplaintSource == "SI")
                            ComplaintSourceID = (long)Constants.ComplaintSource.SecretaryIrrigation;
                        else if (ComplaintSource == "TP")
                            ComplaintSourceID = (long)Constants.ComplaintSource.TehsilProgramme;

                        string ToDate = Request.QueryString["ToDate"];
                        string FromDate = Request.QueryString["FromDate"];
                        string Status = Request.QueryString["CompStatus"];
                        ddlStatus.SelectedIndex = Convert.ToInt32(Status);
                        ddlComplaintSource.SelectedValue = Convert.ToString(ComplaintSourceID);
                        txtFromDate.Text = FromDate;
                        txtToDate.Text = ToDate;
                        BindSearchResultsGrid();
                    }
                    else
                    {
                        DateTime TodayDate = DateTime.Now;
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(TodayDate.AddDays(-30)));
                        //BindSearchResultsGrid();
                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchComplaints);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDomainDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.UA_Designations != null && _MdlUser.UA_Designations.IrrigationLevelID != null)
                Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            else
                Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);

        }

        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchComplaintCriteria];
            txtComplaintID.Text = (string)SearchCriteria[ComplaintIDKey];
            txtComplainantName.Text = (string)SearchCriteria[ComplainantNameKey];
            txtComplainantCell.Text = (string)SearchCriteria[ComplainantCellKey];
            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];
            string DivisionID = (string)SearchCriteria[DivisionIDKey];
            string ChannelID = (string)SearchCriteria[ChannelIDKey];
            string StatusID = (string)SearchCriteria[StatusIDKey];
            string DomainID = (string)SearchCriteria[DomainIDKey];
            string ComplaintSource = (string)SearchCriteria[ComplaintSourceKey];
            string ActionID = (string)SearchCriteria[ActionIDKey];
            string ComplaintTypeID = (string)SearchCriteria[ComplaintTypeIDKey];
            bool chkboxOtherThenPMIU = (bool)SearchCriteria[chkboxOtherThenPMIUStaffKey];

            if (DomainID != String.Empty)
            {
                ddlDomain.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDomain, DomainID);
            }

            if (DivisionID != string.Empty && ddlDivision.Enabled == true)
            {
                ddlDivision.ClearSelection();
                ddlDomain_SelectedIndexChanged(null, null);
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
            }

            if (ComplaintSource != String.Empty)
            {
                ddlComplaintSource.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlComplaintSource, ComplaintSource);

            }

            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }

            if (ActionID != String.Empty)
            {
                ddlAction.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlAction, StatusID);
            }

            if (ComplaintTypeID != String.Empty)
            {
                ddlComplaintType.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlComplaintType, StatusID);
            }

            if (chkboxOtherThenPMIU == true)
            {
                chkboxOtherThenPMIUStaff.Checked = true;
            }
            gvSearchComplaints.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindSearchResultsGrid();
        }
        protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users _MdlUser = SessionManagerFacade.UserInformation;
                long UserID = _MdlUser.ID;
                if (ddlDomain.SelectedItem.Value != string.Empty)
                {
                    long DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);

                    if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
                    {
                        Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);
                    }
                    else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID == null)
                    {
                        Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
                    }

                    else if (_MdlUser.DesignationID != null)
                    {
                        Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

                    }
                }
                else
                {
                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Insert(0, new ListItem("All", ""));
                    //ddlDivision.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDropdowns()
        {
            Dropdownlist.DDLComplaintSource(ddlComplaintSource, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.GetComplaintStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.GetComplaintType(ddlComplaintType, (int)Constants.DropDownFirstOption.All);

        }

        protected void btnComplaintsSearch_Click(object sender, EventArgs e)
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
                long SelectedStatusID = -1;
                long SelectedComplaintSourceID = -1;
                long SelectedActionID = -1;
                long ComplaintTypeID = -1;
                long SelectedDomainID = -1;
                bool SelectedOtherThenPMIUStaff = false;
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                string ComplaintNumber = txtComplaintID.Text.Trim();
                SearchCriteria.Add(ComplaintIDKey, ComplaintNumber);

                string ComplainantName = txtComplainantName.Text.Trim();
                SearchCriteria.Add(ComplainantNameKey, ComplainantName);

                string ComplainantCell = txtComplainantCell.Text.Trim();
                SearchCriteria.Add(ComplainantCellKey, ComplainantCell);


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

                if (ddlDomain.SelectedItem.Value != String.Empty)
                {
                    SelectedDomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
                }

                SearchCriteria.Add(DomainIDKey, ddlDomain.SelectedItem.Value);


                if (ddlComplaintSource.SelectedItem.Value != String.Empty)
                {
                    SelectedComplaintSourceID = Convert.ToInt64(ddlComplaintSource.SelectedItem.Value);
                }

                SearchCriteria.Add(ComplaintSourceKey, ddlComplaintSource.SelectedItem.Value);

                if (ddlStatus.SelectedItem.Value != String.Empty)
                {
                    SelectedStatusID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                }

                SearchCriteria.Add(StatusIDKey, ddlStatus.SelectedItem.Value);

                if (ddlAction.SelectedItem.Value != String.Empty)
                {
                    SelectedActionID = Convert.ToInt64(ddlAction.SelectedItem.Value);
                }

                SearchCriteria.Add(ActionIDKey, ddlAction.SelectedItem.Value);

                if (ddlComplaintType.SelectedItem.Value != String.Empty)
                {
                    ComplaintTypeID = Convert.ToInt64(ddlComplaintType.SelectedItem.Value);
                }

                SearchCriteria.Add(ComplaintTypeIDKey, ddlComplaintType.SelectedItem.Value);

                if (chkboxOtherThenPMIUStaff.Checked)
                {
                    SelectedOtherThenPMIUStaff = true;
                }
                SearchCriteria.Add(chkboxOtherThenPMIUStaffKey, SelectedOtherThenPMIUStaff);
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                var lstComplaintCases = new List<dynamic>();

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    IEnumerable<DataRow> ieComplaintCases = new ComplaintsManagementBLL().GetComplaintCases(mdlUser.ID, ComplaintNumber, ComplainantName, ComplainantCell, SelectedDomainID,
                    SelectedDivisionID, SelectedComplaintSourceID, SelectedStatusID, SelectedActionID, ComplaintTypeID, FromDate, ToDate,
                    (mdlUser.DesignationID == null ? null : mdlUser.UA_Designations.IrrigationLevelID), SelectedOtherThenPMIUStaff, (long)mdlUser.DesignationID);

                    lstComplaintCases = ieComplaintCases.Select(dataRow => new
                    {
                        ID = dataRow.Field<long>("ID"),
                        ComplaintDate = dataRow.Field<DateTime>("ComplaintDate"),
                        ComplainantName = dataRow.Field<string>("ComplainantName"),
                        ComplaintType = dataRow.Field<string>("ComplaintType"),
                        ChannelName = dataRow.Field<string>("ChannelName"),
                        // Status = dataRow.Field<string>("Status"),
                        DivisionName = dataRow.Field<string>("DivisionName"),
                        CircleName = dataRow.Field<string>("CircleName"),
                        ZoneName = dataRow.Field<string>("ZoneName"),
                        DomainName = dataRow.Field<string>("DomainName"),
                        ComplaintStatus = dataRow.Field<string>("ComplaintStatus"),
                        ComplaintNumber = dataRow.Field<string>("ComplaintNumber"),
                        Starred = dataRow.Field<bool?>("Starred") == null ? false : dataRow.Field<bool>("Starred"),
                        ComplaintSourceID = dataRow.Field<long>("ComplaintSourceID")
                        //  dataRow.Field<bool>("Starred")

                    }).ToList<dynamic>();

                }

                gvSearchComplaints.DataSource = lstComplaintCases;
                gvSearchComplaints.DataBind();

                lblTotalComplaints.Text = "Total Complaints Retrived: " + Convert.ToString(lstComplaintCases.Count());
                //lblTotalComplaints.Visible = true;

                SearchCriteria.Add(PageIndexKey, gvSearchComplaints.PageIndex);

                Session[SessionValues.SearchComplaintCriteria] = SearchCriteria;

                if (lstComplaintCases.Count() != 0)
                {
                    //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (mdlUser.DesignationID == (long)Constants.Designation.HelplineOperator || mdlUser.DesignationID == (long)Constants.Designation.DataEntryOperator)
                        btnBulkActivity.Visible = false;
                    else
                        btnBulkActivity.Visible = true;
                }
                else
                {
                    btnBulkActivity.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvSearchComplaints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblComplaintDate = (Label)e.Row.FindControl("lblComplaintDate");
                    LinkButton hlQuickAction = (LinkButton)e.Row.FindControl("hlQuickAction");
                    HyperLink hlFavorite = (HyperLink)e.Row.FindControl("hlFavorite");
                    LinkButton lbtnPrint = (LinkButton)e.Row.FindControl("lbtnPrint");
                    HyperLink hlComplaintActivity = (HyperLink)e.Row.FindControl("hlComplaintActivity");
                    Label lblFavorite = (Label)e.Row.FindControl("lblFavorite");
                    Label lblStatusID = (Label)e.Row.FindControl("lblStatus");
                    //HyperLink hlComplaintEdit = (HyperLink)e.Row.FindControl("hlComplaintEdit");


                    DateTime Date = DateTime.Parse(lblComplaintDate.Text);
                    lblComplaintDate.Text = Utility.GetFormattedDate(Date); //+ " " + Utility.GetFormattedTime(Date);
                    int LastRow = gvSearchComplaints.Rows.Count - 1;
                    int cell = gvSearchComplaints.Columns.Count - 1;
                    if (mdlUser.DesignationID == (long)Constants.Designation.HelplineOperator || mdlUser.DesignationID == (long)Constants.Designation.DataEntryOperator)
                    {
                        gvSearchComplaints.Columns[0].Visible = false;
                        //gvSearchComplaints.Columns[cell].Visible = false;
                        hlQuickAction.Visible = false;
                        hlFavorite.Visible = false;
                        lbtnPrint.Visible = false;
                        hlComplaintActivity.Visible = false;

                    }
                    if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        hlQuickAction.Visible = true;
                        lbtnPrint.Visible = true;

                    }
                    if (lblFavorite.Text == "True")
                    {
                        hlFavorite.CssClass = hlFavorite.CssClass.Replace("StarComplaint", "FillStar");
                    }
                    //if (lblStatusID.Text == "Resolved" && mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    //{
                    //    hlComplaintActivity.Enabled = false;
                    //}

                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchComplaints_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchComplaints.PageIndex = e.NewPageIndex;

                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool AddComplaintAsFavorite(long rowid)
        {
            bool IsSaved = false;
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
            // CM_ComplaintMarked CompMark = new CM_ComplaintMarked();
            try
            {
                //UA_Users mdlUser = SessionManagerFacade.UserInformation;

                //CompMark.ComplaintID = rowid;
                //CompMark.Starred = true;
                //CompMark.CreatedBy = Convert.ToInt32(mdlUser.ID);
                //CompMark.UserID = Convert.ToInt32(mdlUser.ID);
                //CompMark.UserDesigID = mdlUser.DesignationID;
                //CompMark.CreatedDate = DateTime.Now;
                IsSaved = bllComplaints.AddComplaintAsFavorite(rowid, Convert.ToInt64(mdlUser.ID));


            }
            catch (Exception ex)
            {
                //new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return IsSaved;
        }

        protected void btnBulkActivity_Click(object sender, EventArgs e)
        {
            try
            {
                ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
                List<long> Ids = new List<long>();
                foreach (GridViewRow gvrow in gvSearchComplaints.Rows)
                {
                    Label id = (Label)(gvrow.FindControl("ID"));
                    if (id.Text == "-1")
                    {
                        continue;
                    }
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    if (chk != null & chk.Checked)
                    {
                        Ids.Add(Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("ID")).Text));
                    }
                }
                List<CM_Complaint> lstOfComplaints = bllComplaints.GetComplaintInfoByID(Ids);
                lstBoxComplaints.DataValueField = "ComplaintNumber";
                lstBoxComplaints.DataSource = lstOfComplaints;
                lstBoxComplaints.DataBind();
                // bool Status = bllComplaints.GetComplaintInfoByID(lstofID);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BulkActivity", "$('#BulkActivity').modal();", true);
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (mdlUser.DesignationID != (long)Constants.Designation.ADM && mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                {
                    chkBoxResolved.Visible = false;
                }
                //   BindSearchGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void btnRemoveComplaints_Click(object sender, EventArgs e)
        {
            try
            {
                // lstBoxComplaints.Items.Remove(lstBoxComplaints.SelectedItem);
                //foreach (ListItem listItem in lstBoxComplaints.Items)
                //{
                //    lstBoxComplaints.Items.RemoveAt(listItem);
                //}
                for (int i = 0; i <= lstBoxComplaints.Items.Count; i++)
                {
                    lstBoxComplaints.Items.Remove(lstBoxComplaints.SelectedItem);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);

            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments CompComments = new CM_ComplaintComments();
                List<string> lstComplaintNumber = new List<string>();

                // Create DataTable

                DataSet DataSet = new DataSet();
                DataTable DataTable = DataSet.Tables.Add("Data");
                DataTable.Columns.Add("ComplaintID", typeof(long));
                DataTable.Columns.Add("Comments", typeof(string));
                DataTable.Columns.Add("CommentsDateTime", typeof(DateTime));
                DataTable.Columns.Add("Attachment", typeof(string));
                DataTable.Columns.Add("UserID", typeof(long));
                DataTable.Columns.Add("UserDesigID", typeof(long));
                DataTable.Columns.Add("CreatedDate", typeof(DateTime));
                DataTable.Columns.Add("CreatedBy", typeof(long));


                for (int i = 0; i < lstBoxComplaints.Items.Count; i++)
                {

                    lstComplaintNumber.Add(lstBoxComplaints.Items[i].Value);
                }

                List<CM_Complaint> lstOfComplaints = bllComplaints.GetComplaintInfoByID(lstComplaintNumber);
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                foreach (var item in lstOfComplaints)
                {
                    DataRow DataRow;
                    DataRow = DataTable.NewRow();
                    DataRow["ComplaintID"] = item.ID;
                    DataRow["Comments"] = txtComments.Text;
                    DataRow["CommentsDateTime"] = DateTime.Now;
                    DataRow["Attachment"] = FileName;
                    DataRow["UserID"] = mdlUser.ID;
                    DataRow["UserDesigID"] = mdlUser.DesignationID;
                    DataRow["CreatedDate"] = DateTime.Now;
                    DataRow["CreatedBy"] = Convert.ToInt32(mdlUser.ID);
                    DataTable.Rows.Add(DataRow);
                }
                bool StatusID = new ComplaintsManagementBLL().AddBulkData(DataTable);

                if (StatusID == true && chkBoxResolved.Checked)
                {
                    bool MarkAsResolved = bllComplaints.MarkAsCompalintsResolved(lstOfComplaints);
                }
                BindSearchResultsGrid();
                ResetControls();
                Master.ShowMessage("Your Action has been Completed.", SiteMaster.MessageType.Success);
                //Response.Redirect("~/Modules/ComplaintsManagement/SearchComplaints.aspx?ShowHistory=true");

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void hlQuickAction_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);
                Label hlComplaintNumber = (Label)gvrCurrent.FindControl("lblComplaintNo");
                Label lblComplaintID = (Label)gvrCurrent.FindControl("ID");
                lblQComplaintID.Text = hlComplaintNumber.Text;
                hdnComplaintID.Value = lblComplaintID.Text;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "QuickActivity", "$('#QuickActivity').modal();", true);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);

            }
        }


        protected void btnQuickSave_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string ComplaintNumber = lblQComplaintID.Text;
                CM_ComplaintComments CompComments = new CM_ComplaintComments();

                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.Complaints);

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }

                CompComments.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                CompComments.Comments = txtQuickComments.Text;
                CompComments.CommentsDateTime = DateTime.Now;
                CompComments.Attachment = FileName;
                CompComments.UserID = mdlUser.ID;
                CompComments.UserDesigID = mdlUser.DesignationID;
                CompComments.CreatedDate = DateTime.Now;
                CompComments.CreatedBy = Convert.ToInt32(mdlUser.ID);

                bool StatusID = new ComplaintsManagementBLL().AddQuickComplaintData(CompComments);

                if (StatusID == true && chkboxQResolve.Checked)
                {
                    bool ComplaintStatus = bllComplaints.CompalintResolvedByID(Convert.ToInt64(hdnComplaintID.Value));
                }
                BindSearchResultsGrid();
                ResetControls();
                Master.ShowMessage("Your Action has been Completed.", SiteMaster.MessageType.Success);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                long ID = Convert.ToInt64(gvSearchComplaints.DataKeys[gvrCurrent.RowIndex].Values[ComplaintIDIndex]);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string UserDesignation = string.Empty;

                if (mdlUser != null && mdlUser.DesignationID != null)
                {
                    UserDesignation = mdlUser.UA_Designations.Name;
                }
                else
                {
                    UserDesignation = mdlUser.UA_Roles.Name;
                }

                string UserName = string.Format("{0} {1}", mdlUser.FirstName, mdlUser.LastName);

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("ID", ID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                //ReportParameter = new ReportParameter("UserDesignation", UserDesignation);
                //mdlReportData.Parameters.Add(ReportParameter);

                //ReportParameter = new ReportParameter("UserName", UserName);
                //mdlReportData.Parameters.Add(ReportParameter);

                mdlReportData.Name = Constants.ComplaintInformation;
                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ResetControls()
        {

            txtQuickComments.Text = string.Empty;
            chkboxQResolve.Checked = false;
            txtComments.Text = string.Empty;
            chkBoxResolved.Checked = false;

        }
    }
}