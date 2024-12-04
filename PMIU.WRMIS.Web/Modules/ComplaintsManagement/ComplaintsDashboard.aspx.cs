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
    public partial class ComplaintsDashboard : BasePage
    {

        #region Hash Table Keys
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string PageIndexKey = "PageIndex";
        public const string StatusIDKey = "StatusID";
        #endregion
        public const int ComplaintIDIndex = 0;
        public static List<long> SuperUserComplaints = new List<long>() { (long)Constants.Designation.ChiefMonitoring, (long)Constants.Designation.DirectorGauges, 
            (long)Constants.Designation.DeputyDirectorHelpline, (long)Constants.Designation.DataEntryOperator, (long)Constants.Designation.HelplineOperator };
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindDropDown();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchComplaintCriteria] != null)
                            {
                                //  BindHistoryData();
                            }
                        }
                        else
                        {
                            DateTime CurrentDate = DateTime.Now;
                            txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                            txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-30)));
                            GetComplaintStatusForGraph();
                            ddlShowComplaints.SelectedIndex = 2;
                            //BindSearchResultsGrid();
                            barchars.Visible = true;
                            // CompTypeChart.Visible = false;
                            hdnComplaintTitle.Value = "";

                        }
                    }
                    else
                    {
                        DateTime TodayDate = DateTime.Now;
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(TodayDate.AddDays(-30)));
                        GetComplaintStatusForGraph();
                        ddlShowComplaints.SelectedIndex = 2;
                        // BindSearchResultsGrid();
                        barchars.Visible = true;
                        // CompTypeChart.Visible = false;
                        hdnComplaintTitle.Value = "";
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> SearchResults(string FromDate, string ToDate, string CompTypeID)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintCount = null;
            long ID = Convert.ToInt64(CompTypeID);
            try
            {
                lstComplaintCount = GetComplaintTypeGraph(ID, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintCount;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetComplaintStatusResults(string FromDate, string ToDate)
        {
            List<object> lstComplaintCount = null;
            try
            {
                lstComplaintCount = GetComplaintStatusGraph(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintCount;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetComplaintResultsBySource(string FromDate, string ToDate)
        {
            List<object> lstComplaintCount = null;
            try
            {
                lstComplaintCount = GetComplaintGraphBySource(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintCount;
        }
        public static List<object> GetComplaintTypeGraph(long _ComplaintType, string _FromDate, string _ToDate)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintsCount = new List<object>();
            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (_FromDate != "")
                    FromDate = Utility.GetParsedDate(_FromDate);

                if (_ToDate != "")
                    ToDate = Utility.GetParsedDate(_ToDate);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsCount = bllComplaint.GetComplaintTypeCounts(mdlUser.ID, FromDate, ToDate, _ComplaintType, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsCount = bllComplaint.GetAllComplaintTypeCounts(mdlUser.ID, FromDate, ToDate, _ComplaintType);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintsCount;
        }

        private void BindDropDown()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlShowComplaints, CommonLists.GetComplaintTypeAndSource());
            Dropdownlist.GetComplaintType(ddlComplaintType, (int)Constants.DropDownFirstOption.Select);
            //Dropdownlist.GetComplaintStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
        }
        protected void ddlShowComplaints_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ComplaintTypeorSource = Convert.ToString(ddlShowComplaints.SelectedItem.Value);
            //BindSearchResultsGrid();
            GetComplaintStatusForGraph();
            ddlComplaintType.SelectedIndex = 0;
            if (ComplaintTypeorSource == "2")
            {
                barchars.Visible = true;
                CompTypeChart.Visible = false;
                ddlComplaintType.Visible = false;
                lblComplaintType.Visible = false;
                hdnComplaintName.Value = "";
            }
            else if (ComplaintTypeorSource == "1")
            {
                ddlComplaintType.Visible = true;
                lblComplaintType.Visible = true;
                barchars.Visible = false;
                CompTypeChart.Visible = true;
            }
            else
            {
                barchars.Visible = false;
                ddlComplaintType.Visible = false;
                lblComplaintType.Visible = false;
            }
        }
        public List<object> GetComplaintTypeForGraph(long _ComplaintTypeorSource)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintsCount = new List<object>();
            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsCount = bllComplaint.GetComplaintTypeCounts(mdlUser.ID, FromDate, ToDate, _ComplaintTypeorSource, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsCount = bllComplaint.GetAllComplaintTypeCounts(mdlUser.ID, FromDate, ToDate, _ComplaintTypeorSource);
                    }
                }
                //String ComplaintName = "";

                for (int i = 0; i < lstComplaintsCount.Count; i++)
                {
                    String Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "ComplaintStatusID");
                    hdnComplaintName.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Name");
                    hdnComplaintTypeId.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "ComplaintTypeID");

                    if (Value == "1")
                    {
                        CompTypeInBox.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                    }
                    else if (Value == "2")
                    {
                        CompTypeInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                    }
                    else
                    {
                        CompTypeResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintsCount;
        }

        public static List<object> GetComplaintStatusGraph(string _FromDate, string _ToDate)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintsCount = new List<object>();

            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (_FromDate != "")
                    FromDate = Utility.GetParsedDate(_FromDate);

                if (_ToDate != "")
                    ToDate = Utility.GetParsedDate(_ToDate);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsCount = bllComplaint.GetStatusCounts(mdlUser.ID, FromDate, ToDate, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsCount = bllComplaint.GetStatusCount(mdlUser.ID, FromDate, ToDate);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintsCount;
        }
        public List<object> GetComplaintStatusForGraph()
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintsCount = new List<object>();
            List<object> lstComplaintsBySource = new List<object>();
            long TotalNumberOfCompSource = 0;
            long TotalNumberOfComplaints = 0;
            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsCount = bllComplaint.GetStatusCounts(mdlUser.ID, FromDate, ToDate, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsCount = bllComplaint.GetStatusCount(mdlUser.ID, FromDate, ToDate);
                    }
                }

                for (int i = 0; i < lstComplaintsCount.Count; i++)
                {
                    String Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Value");

                    if (Value == "1")
                    {
                        InBox.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                        TotalNumberOfComplaints += Convert.ToInt64(InBox.Value);
                    }
                    else if (Value == "2")
                    {
                        InProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                        TotalNumberOfComplaints += Convert.ToInt64(InProgress.Value);
                    }
                    else
                    {
                        Resolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsCount[i], "Count");
                        TotalNumberOfComplaints += Convert.ToInt64(Resolved.Value);
                    }

                }

                lblStatuCount.Text = Convert.ToString(TotalNumberOfComplaints);

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsBySource = bllComplaint.GetComplaintBySourceCounts(mdlUser.ID, FromDate, ToDate, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsBySource = bllComplaint.GetAllComplaintBySourceCounts(mdlUser.ID, FromDate, ToDate);
                    }
                }

                for (int i = 0; i < lstComplaintsBySource.Count; i++)
                {
                    String ComplaintSource = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "ComplaintSourceID");
                    String ComplaintStatus = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "ComplaintStatusID");

                    if (ComplaintSource == "1" && ComplaintStatus == "1")
                    {
                        hdnDefaultSourceInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnDefaultSourceInbox.Value);
                    }
                    else if (ComplaintSource == "1" && ComplaintStatus == "2")
                    {
                        hdnDefaultSourceInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnDefaultSourceInProgress.Value);
                    }
                    else if (ComplaintSource == "1" && ComplaintStatus == "3")
                    {
                        hdnDefaultSourceResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnDefaultSourceResolved.Value);
                    }


                    else if (ComplaintSource == "2" && ComplaintStatus == "1")
                    {
                        hdnChiefMinisterInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChiefMinisterInbox.Value);
                    }
                    else if (ComplaintSource == "2" && ComplaintStatus == "2")
                    {
                        hdnChiefMinisterInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChiefMinisterInProgress.Value);
                    }
                    else if (ComplaintSource == "2" && ComplaintStatus == "3")
                    {
                        hdnChiefMinisterResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChiefMinisterResolved.Value);
                    }


                    else if (ComplaintSource == "3" && ComplaintStatus == "1")
                    {
                        hdnChieftSecretaryInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChieftSecretaryInbox.Value);
                    }
                    else if (ComplaintSource == "3" && ComplaintStatus == "2")
                    {
                        hdnChieftSecretaryInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChieftSecretaryInProgress.Value);
                    }
                    else if (ComplaintSource == "3" && ComplaintStatus == "3")
                    {
                        hdnChieftSecretaryResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnChieftSecretaryResolved.Value);
                    }


                    else if (ComplaintSource == "4" && ComplaintStatus == "1")
                    {
                        hdnSecretaryIrrigationInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnSecretaryIrrigationInbox.Value);
                    }
                    else if (ComplaintSource == "4" && ComplaintStatus == "2")
                    {
                        hdnSecretaryIrrigationInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnSecretaryIrrigationInProgress.Value);
                    }
                    else if (ComplaintSource == "4" && ComplaintStatus == "3")
                    {
                        hdnSecretaryIrrigationResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnSecretaryIrrigationResolved.Value);
                    }


                    else if (ComplaintSource == "5" && ComplaintStatus == "1")
                    {
                        hdnTehsilProgrammeInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnTehsilProgrammeInbox.Value);
                    }
                    else if (ComplaintSource == "5" && ComplaintStatus == "2")
                    {
                        hdnTehsilProgrammeInProgress.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnTehsilProgrammeInProgress.Value);
                    }
                    else if (ComplaintSource == "5" && ComplaintStatus == "3")
                    {
                        hdnTehsilProgrammeResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnTehsilProgrammeResolved.Value);
                    }


                    else if (ComplaintSource == "6" && ComplaintStatus == "1")
                    {
                        hdnAutomaticInbox.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnAutomaticInbox.Value);
                    }
                    else if (ComplaintSource == "6" && ComplaintStatus == "2")
                    {
                        hdnAutomaticInprogess.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnAutomaticInprogess.Value);
                    }
                    else if (ComplaintSource == "6" && ComplaintStatus == "3")
                    {
                        hdnAutomaticResolved.Value = Utility.GetDynamicPropertyValue(lstComplaintsBySource[i], "Count");
                        TotalNumberOfCompSource += Convert.ToInt64(hdnAutomaticResolved.Value);
                    }
                }

                lblSourceComplaints.Text = Convert.ToString(TotalNumberOfCompSource);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintsCount;
        }

        public static List<object> GetComplaintGraphBySource(string _FromDate, string _ToDate)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            List<object> lstComplaintsBySource = new List<object>();
            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (_FromDate != "")
                    FromDate = Utility.GetParsedDate(_FromDate);

                if (_ToDate != "")
                    ToDate = Utility.GetParsedDate(_ToDate);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    if (!SuperUserComplaints.Contains(mdlUser.DesignationID.Value) && mdlUser.UA_Designations.IrrigationLevelID != null) //mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        lstComplaintsBySource = bllComplaint.GetComplaintBySourceCounts(mdlUser.ID, FromDate, ToDate, mdlUser.DesignationID);
                    }
                    else
                    {
                        lstComplaintsBySource = bllComplaint.GetAllComplaintBySourceCounts(mdlUser.ID, FromDate, ToDate);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComplaintsBySource;
        }
        protected void btnComplaintsSearch_Click(object sender, EventArgs e)
        {
            try
            {
                hdnComplaintTitle.Value = "Complaint By Status";
                String ComplaintType = Convert.ToString(ddlShowComplaints.SelectedItem.Value);
                //   BindSearchResultsGrid();
                String ComplaintTypeorSource = Convert.ToString(ddlComplaintType.SelectedItem.Value);
                if (ComplaintTypeorSource != String.Empty)
                {
                    long ID = Convert.ToInt64(ComplaintTypeorSource);
                    GetComplaintTypeForGraph(ID);
                    CompTypeChart.Visible = true;
                    ddlComplaintType.Visible = true;
                    lblComplaintType.Visible = true;
                }
                else
                {
                    barchars.Visible = true;
                    CompTypeChart.Visible = false;
                    ddlComplaintType.Visible = false;
                    lblComplaintType.Visible = false;
                }
                GetComplaintStatusForGraph();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAdvanceSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SearchComplaints.aspx", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}