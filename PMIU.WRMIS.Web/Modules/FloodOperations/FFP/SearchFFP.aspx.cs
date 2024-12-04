using PMIU.WRMIS.Web.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using Microsoft.Reporting.WebForms;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class SearchFFP : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }

        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        #endregion

        #region Hash Table Keys

        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string YearDateKey = "Year";
        public const string StatusKey = "Status";
        public const string PageIndexKey = "PageIndex";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropDownList();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);

                    if (FFPID > 0)
                    {
                        hdnFFPID.Value = FFPID.ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindFFPSearchGrid(Convert.ToInt64(hdnFFPID.Value));
                    }
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    hlAddNewFFP.Visible = true;
                    //}
                    //else
                    //{
                    //    hlAddNewFFP.Visible = false;
                    //}
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Bind DropDown List

        private void BindDropDownList()
        {

            //  BindZoneDropdown();
            //  DDLEmptyCircleDivision();
            BindUserLocation();
            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
            {
                BindStatusDropDown();
            }
            Dropdownlist.DDLYear(ddlYear, false, 0, 2011, (int)Constants.DropDownFirstOption.All);
        }

        private void BindStatusDropDown()
        {
            try
            {
                Dropdownlist.DDLActiveInspectionStatus(ddlStatus, false, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
        }
        private void BindCircleDropdown(long _ZoneID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);

            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, _ZoneID, false, (int)Constants.DropDownFirstOption.All);

            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);

            }
        }
        private void BindDivisionDropdown(long _CircleID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    // ddlCircle.Enabled = false;
                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    // ddlCircle.Enabled = true;
                }
                ddlDivision.SelectedIndex = 0;
                //  ddlDivision.Enabled = false;
                // gvFFP.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    //  ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    // ddlDivision.Enabled = true;
                }
                // gvFFP.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Bind DropDown List

        #region Gridview  Events
        private void BindFFPSearchGrid(long _FFPID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                string SelectedStatusID = null;

                if (ddlZone.SelectedItem.Value != String.Empty)
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty)
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlYear.SelectedItem.Value != String.Empty)
                {
                    SelectedYear = Convert.ToInt32(ddlYear.SelectedItem.Value);
                }
                SearchCriteria.Add(YearDateKey, ddlYear.SelectedItem.Value);

                if (ddlStatus.SelectedItem.Text != "All")
                {
                    SelectedStatusID = Convert.ToString(ddlStatus.SelectedItem.Text);
                }
                SearchCriteria.Add(StatusKey, SelectedStatusID);

                //if (ddlStatus.SelectedItem.Value != String.Empty)
                //{
                //    SelectedStatusID = Convert.ToBoolean(Convert.ToUInt16((ddlStatus.SelectedItem.Value)));

                //}

                //SearchCriteria.Add(StatusKey, ddlStatus.SelectedItem.Value);

                var FFPID = _FFPID == 0 ? (long?)null : _FFPID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                IEnumerable<DataRow> IeFFP = new FloodFightingPlanBLL().GetFFPSearch(FFPID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear, SelectedStatusID);
                var LstFFP = IeFFP.Select(dataRow => new
                {
                    FFPID = dataRow.Field<long>("FFPID"),
                    FFPYear = dataRow.Field<int>("FFPYear"),
                    FFPZone = dataRow.Field<string>("FFPZone"),
                    FFPCircle = dataRow.Field<string>("FFPCircle"),
                    FFPDivision = dataRow.Field<string>("FFPDivision"),
                    FFPDivisionID = dataRow.Field<long>("FFPDivisionID"),
                    FFPStatus = dataRow.Field<string>("FFPStatus"),

                }).ToList();

                gvFFP.DataSource = LstFFP;
                gvFFP.DataBind();
                SearchCriteria.Add(PageIndexKey, gvFFP.PageIndex);
                Session[SessionValues.SearchFFP] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvFFP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFFP.PageIndex = e.NewPageIndex;
                BindFFPSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvFFP_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long FFPID = Convert.ToInt64(((Label)gvFFP.Rows[e.RowIndex].FindControl("lblFFPID")).Text);
                if (new FloodFightingPlanBLL().IsFFPDependencyExists(FFPID))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsDeleted = new FloodFightingPlanBLL().DeleteFFP(FFPID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindFFPSearchGrid(0);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvFFP_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvFFP.EditIndex = -1;
                BindFFPSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool CanAddEditFFP = false;
            UA_SystemParameters systemParameters = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Button hlEdit = (Button)e.Row.FindControl("hlEdit");
                Button hlPublish = (Button)e.Row.FindControl("btnPublish");
                Button btnDelete = (Button)e.Row.FindControl("linkButtonDelete");

                string StatusValue = ((Label)e.Row.FindControl("lblFFPStatus")).Text;
                int FFPYear = Convert.ToInt32(((Label)e.Row.FindControl("Year")).Text);

                btnDelete.Enabled = false;
                hlEdit.Enabled = false;

                if (StatusValue == "Draft")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                    if (CanAddEditFFP)
                    {
                        btnDelete.Enabled = CanAddEditFFP;
                        hlEdit.Enabled = CanAddEditFFP;
                        CanDelete = CanAddEditFFP;
                        hlPublish.Enabled = CanAddEditFFP;
                    }
                }
                else if (StatusValue == "Published")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                    if (CanAddEditFFP)
                    {
                        hlEdit.Enabled = CanAddEditFFP;
                        //btnDelete.Enabled = CanAddEditFFP;
                    }
                }
                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    gvFFP.Columns[5].Visible = true;
                }
            }
        }
        protected void gvFFP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvFFP.DataKeys[gvrow.RowIndex];
                int Year = Convert.ToInt32(key["FFPYear"]);
                long DivisionID = Convert.ToInt64(key["FFPDivisionID"]);
                if (e.CommandName == "FFPStatus")
                {
                    long FFPID = Convert.ToInt64(e.CommandArgument);
                    bool IsSave = new FloodFightingPlanBLL().CheckFFPStatusByID(FFPID);
                    if (IsSave)
                    {
                        new FloodOperationsBLL().SendNotifiactions(FFPID, (long)Session[SessionValues.UserID], (long)NotificationEventConstants.FloodOperations.FloodFightingPlan);
                        BindFFPSearchGrid(0);

                    }
                    Master.ShowMessage("Published Successfully", SiteMaster.MessageType.Success);
                    return;
                }
                else if (e.CommandName == "EditFFP")
                {
                    long FFPID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddFFP.aspx?FFPID=" + FFPID);
                }
                else if (e.CommandName.Equals("PrintReport"))
                {
                    ReportData mdlReportData = new ReportData();
                    ReportParameter ReportParameter = new ReportParameter("DivisionID", Convert.ToString(DivisionID));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("Year", Convert.ToString(Year));
                    mdlReportData.Parameters.Add(ReportParameter);
                    mdlReportData.Name = Constants.FloodFightingPlanReport;
                    // Set the ReportData in Session with specific Key
                    Session[SessionValues.ReportData] = mdlReportData;
                    string ReportViwerurl = "../" + Constants.ReportsUrl;
                    // Open the report printable in new tab
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion


        #region Button Event

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindFFPSearchGrid(0);
                gvFFP.Visible = true;
            }
            catch (Exception ex)
            {
                gvFFP.Visible = false;
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Button Event

        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            ViewState.Add(UserIDKey, mdlUser.ID);

            if (mdlUser.RoleID != Constants.AdministratorRoleID)
            {
                if (mdlUser.UA_Designations.IrrigationLevelID != null)
                {
                    List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            #region Zone Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            #region Circle Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserZone.Add(mdlCircle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserCircle.Add((long)mdlDivision.CircleID);
                                lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            #endregion
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
        }
        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

    }
}