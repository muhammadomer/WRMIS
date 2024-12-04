using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using System.Linq;
using PMIU.WRMIS.BLL.UserAdministration;




namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{

    public partial class SearchSchedule : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserZoneKey = "UserZone";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        public const string UserSubDivisionKey = "UserSubDivision";
        public const string UserSectionKey = "UserSection";

        #endregion

        #region Hash Table keys

        public const string UserNameKey = "UserName";
        public const string NameKey = "Name";
        public const string StatusIDKey = "StatusID";
        public const string RoleIDKey = "RoleID";
        public const string OrganizationIDKey = "OrganizationID";
        public const string DesignationIDKey = "DesignationID";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string SubDivisionIDKey = "SubDivisionID";
        public const string SectionIDKey = "SectionID";
        public const string PageIndexKey = "PageIndex";

        #endregion
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

        private static string _Message = "";
        public static string ActionMessage
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (SessionManagerFacade.UserInformation.RoleID == 8 || SessionManagerFacade.UserInformation.RoleID == 12) // Hide Prepare Schedule button in case of SE, DD
                        hlNewSchedule.Visible = false;

                    if (_IsSaved)
                    {
                        if (_Message == "")
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        }
                        else
                        {
                            Master.ShowMessage(_Message, SiteMaster.MessageType.Success);
                        }
                        _Message = string.Empty;
                        _IsSaved = false; // Reset flag after displaying message.
                    }
                    BindUserLocation();
                    if (Session["SS_SC_SearchCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["SS_SC_SearchCriteria"]);

                        this.LoadSearchCriteria(Convert.ToInt64(searchCriteria["_ZoneID"]), Convert.ToInt64(searchCriteria["_CircleID"]), Convert.ToInt64(searchCriteria["_DivisionID"]),
              Convert.ToInt64(searchCriteria["_SubDivisionID"]), Convert.ToInt64(searchCriteria["_StatusID"]), Convert.ToBoolean(searchCriteria["_ApprovedCheck"]), Convert.ToString(searchCriteria["_fromDate"]), Convert.ToString(searchCriteria["_toDate"]));
                        BindScreenControls();
                        LoadSearchCriteria(searchCriteria);
                    }
                    else
                    {
                        dynamic Value = 1;
                        //BindUserLocation();
                        BindScreenControls();
                        Dropdownlist.DDLScheduleInspectionStatus(ddlStatus, false, 3);
                        //BindDropdownlists(false, Value);
                        DateTime CurrentDate = DateTime.Now;
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(+30)));
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchChriteria();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvSISearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ScheduleID = Convert.ToInt64(((Label)gvSISearch.Rows[e.RowIndex].FindControl("ScheduleID")).Text);
                long StatusID = Convert.ToInt64(((Label)gvSISearch.Rows[e.RowIndex].FindControl("ScheduleStatusID")).Text);
                
                if (StatusID == (long)Constants.SIScheduleStatus.Prepared)
                {
                    bool isDeleted = new ScheduleInspectionBLL().DeleteSchedule(Convert.ToInt64(ScheduleID));
                    if (isDeleted == true)
                        Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    SearchChriteria();                    
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);                    
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void gvSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSISearch.PageIndex = e.NewPageIndex;

                SearchChriteria();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        #region DropdownListsBinding

        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

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

                            long SelectedZoneID = -1;// lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();

                            if (lstZone.Count() > 1)
                            {
                                ddlZone.Items.Insert(0, new ListItem("All", ""));
                                //Dropdownlist.DDLCircles(ddlCircle, true, SelectedZoneID, (int)Constants.DropDownFirstOption.All);                                
                                Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                            }
                            else
                            {
                                SelectedZoneID = lstZone.FirstOrDefault().ID;
                                ddlZone.SelectedValue = SelectedZoneID.ToString();
                                Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
                            }

                            //Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
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

                            long SelectedZoneID = -1;// lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();

                            if (lstZone.Count() > 1)
                            {
                                ddlZone.Items.Insert(0, new ListItem("All", ""));
                                Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                            }
                            else
                            {
                                SelectedZoneID = lstZone.FirstOrDefault().ID;
                                ddlZone.SelectedValue = SelectedZoneID.ToString();

                                List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                                long SelectedCircleID = -1; //lstCircle.FirstOrDefault().ID;

                                ddlCircle.DataSource = lstCircle;
                                ddlCircle.DataTextField = "Name";
                                ddlCircle.DataValueField = "ID";
                                ddlCircle.DataBind();


                                if (lstCircle.Count() > 1)
                                {
                                    ddlCircle.Items.Insert(0, new ListItem("All", ""));
                                    //Dropdownlist.DDLDivisions(ddlDivision, true, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);                                    
                                    Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                    Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                                }
                                else
                                {
                                    ddlCircle.SelectedValue = SelectedCircleID.ToString();
                                    SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);
                                }
                                //Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            }

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

                            long SelectedZoneID = -1; //lstZone.FirstOrDefault().ID;
                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();

                            if (lstZone.Count() > 1)
                            {
                                ddlZone.Items.Insert(0, new ListItem("All", ""));
                                Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                            }
                            else
                            {
                                SelectedZoneID = lstZone.FirstOrDefault().ID;
                                ddlZone.SelectedValue = SelectedZoneID.ToString();

                                List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                                long SelectedCircleID = -1; //lstCircle.FirstOrDefault().ID;

                                ddlCircle.DataSource = lstCircle;
                                ddlCircle.DataTextField = "Name";
                                ddlCircle.DataValueField = "ID";
                                ddlCircle.DataBind();

                                if (lstCircle.Count() > 1)
                                {
                                    ddlCircle.Items.Insert(0, new ListItem("All", ""));
                                    Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                    Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                                }
                                else
                                {
                                    SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

                                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                                    long SelectedDivisionID = -1; //lstDivision.FirstOrDefault().ID;

                                    ddlDivision.DataSource = lstDivision;
                                    ddlDivision.DataTextField = "Name";
                                    ddlDivision.DataValueField = "ID";
                                    ddlDivision.DataBind();

                                    if (lstDivision.Count() > 1)
                                    {
                                        ddlDivision.Items.Insert(0, new ListItem("All", ""));
                                        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                                    }
                                    else
                                    {
                                        SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                        ddlDivision.SelectedValue = SelectedDivisionID.ToString();
                                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                                    }
                                    //Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                                }
                            }

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            #region Sub Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSubDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserDivision.Add((long)mdlSubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = -1;// lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();

                            if (lstZone.Count() > 1)
                            {
                                ddlZone.Items.Insert(0, new ListItem("All", ""));
                                Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                            }
                            else
                            {
                                SelectedZoneID = lstZone.FirstOrDefault().ID;
                                ddlZone.SelectedValue = SelectedZoneID.ToString();

                                List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                                long SelectedCircleID = -1; //lstCircle.FirstOrDefault().ID;

                                ddlCircle.DataSource = lstCircle;
                                ddlCircle.DataTextField = "Name";
                                ddlCircle.DataValueField = "ID";
                                ddlCircle.DataBind();

                                if (lstCircle.Count() > 1)
                                {
                                    ddlCircle.Items.Insert(0, new ListItem("All", ""));
                                    Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                                    Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                                }
                                else
                                {
                                    SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

                                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                                    long SelectedDivisionID = -1;// lstDivision.FirstOrDefault().ID;

                                    ddlDivision.DataSource = lstDivision;
                                    ddlDivision.DataTextField = "Name";
                                    ddlDivision.DataValueField = "ID";
                                    ddlDivision.DataBind();

                                    if (lstDivision.Count() > 1)
                                    {
                                        ddlCircle.Items.Insert(0, new ListItem("All", ""));
                                        Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                                    }
                                    else
                                    {
                                        SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                        ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                                        List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                                        long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                                        ddlSubDivision.DataSource = lstSubDivision;
                                        ddlSubDivision.DataTextField = "Name";
                                        ddlSubDivision.DataValueField = "ID";
                                        ddlSubDivision.DataBind();

                                        if (lstSubDivision.Count() > 1)
                                            ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                                        else
                                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                        {
                            #region Section Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSection.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserSubDivision.Add((long)mdlSection.SubDivID);
                                lstUserDivision.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
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

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlSubDivision.DataSource = lstSubDivision;
                            ddlSubDivision.DataTextField = "Name";
                            ddlSubDivision.DataValueField = "ID";
                            ddlSubDivision.DataBind();
                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();



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

            ViewState.Add(UserZoneKey, lstUserZone);
            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
            ViewState.Add(UserSubDivisionKey, lstUserSubDivision);
            ViewState.Add(UserSectionKey, lstUserSection);
        }

        private void BindDropdownlists(dynamic SearchCriteria)
        {
            try
            {
                Dropdownlist.DDLScheduleInspectionStatus(ddlStatus, false, 3);
                UA_AssociatedLocation userLocation = SessionManagerFacade.UserAssociatedLocations;
                switch (SessionManagerFacade.UserInformation.DesignationID)
                {
                    case (long)Constants.Designation.SDO:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.SubDivision == userLocation.IrrigationLevelID)
                        {

                            // LoadAllDropdownlistsForSessionData(new SubDivisionBLL().GetByID(userLocation.IrrigationBoundryID.Value).DivisionID.Value, SearchCriteria);
                            SetDropDownsForSearchCriteria(SearchCriteria);
                        }
                        break;
                    case (long)Constants.Designation.XEN:
                    case (long)Constants.Designation.MA:
                    case (long)Constants.Designation.ADM:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Division == userLocation.IrrigationLevelID)
                            SetDropDownsForSearchCriteria(SearchCriteria);
                        //LoadAllDropdownlistsForSessionData(userLocation.IrrigationBoundryID.Value, SearchCriteria);


                        break;
                    case (long)Constants.Designation.DataAnalyst:
                    case (long)Constants.Designation.ChiefMonitoring:
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                        DDLEmptyCircleDivisionSubDivision();
                        break;
                    case (long)Constants.Designation.ChiefIrrigation:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Zone == userLocation.IrrigationLevelID)
                        {
                            // Bind Zone dropdownlist 
                            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(userLocation.IrrigationBoundryID.Value));
                            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
                            DDLEmptyDivisionSubDivision();
                        }
                        break;
                    case (long)Constants.Designation.SE:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Circle == userLocation.IrrigationLevelID)
                        {
                            CO_Circle circle = new CircleBLL().GetByID(userLocation.IrrigationBoundryID.Value);
                            // Bind Zone dropdownlist 
                            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
                            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
                            Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
                            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value), -1, (int)Constants.DropDownFirstOption.All);
                            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, -1, (int)Constants.DropDownFirstOption.All);
                        }
                        break;

                    case (long)Constants.Designation.DeputyDirector:


                        // Bind Zone dropdownlist 
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                        Dropdownlist.DDLCircles(ddlCircle, false, -1, (int)Constants.DropDownFirstOption.All);
                        Dropdownlist.DDLDivisions(ddlDivision, false, -1, -1, (int)Constants.DropDownFirstOption.All);
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, -1, (int)Constants.DropDownFirstOption.All);

                        break;
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        //private void LoadAllDropdownlistsData(long _DivisionID)
        //{
        //    try
        //    {
        //        CO_Division division = new DivisionBLL().GetByID(_DivisionID);
        //        CO_Circle circle = new CircleBLL().GetByID(division.CircleID.Value);
        //        // Bind Zone dropdownlist 
        //        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
        //        ListItem zoneItem = ddlZone.Items.FindByValue(Convert.ToString(circle.ZoneID));
        //        ListItem firstZ = ddlZone.Items[0];
        //        ddlZone.Items.Clear();
        //        ddlZone.Items.Add(firstZ);
        //        ddlZone.Items.Add(zoneItem);
        //        Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //        ListItem CircleItem = ddlCircle.Items.FindByValue(Convert.ToString(circle.ID));
        //        ListItem firstC = ddlCircle.Items[0];
        //        ddlCircle.Items.Clear();
        //        ddlCircle.Items.Add(firstC);
        //        ddlCircle.Items.Add(CircleItem);
        //        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
        //        Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value), -1, (int)Constants.DropDownFirstOption.All);
        //        ListItem DivisionItem = ddlDivision.Items.FindByValue(Convert.ToString(division.ID));
        //        ListItem firstD = ddlDivision.Items[0];
        //        ddlDivision.Items.Clear();
        //        ddlDivision.Items.Add(firstD);
        //        ddlDivision.Items.Add(DivisionItem);
        //        Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(division.ID));
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        private void LoadAllDropdownlistsForSessionData(long _DivisionID, dynamic SearchCriteria)
        {
            try
            {
                CO_Division division = new DivisionBLL().GetByID(_DivisionID);
                CO_Circle circle = new CircleBLL().GetByID(division.CircleID.Value);
                // Bind Zone dropdownlist 
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(SearchCriteria["_ZoneID"]));
                ListItem zoneItem = ddlZone.Items.FindByValue(Convert.ToString(SearchCriteria["_ZoneID"]));
                ddlZone.Items.Clear();
                ddlZone.Items.Add(zoneItem);

                Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(SearchCriteria["_ZoneID"]), (int)Constants.DropDownFirstOption.All);

                ListItem CircleItem = ddlCircle.Items.FindByValue(Convert.ToString(SearchCriteria["_CircleID"]));
                ddlCircle.Items.Clear();
                ddlCircle.Items.Add(CircleItem);
                Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(SearchCriteria["_CircleID"]));

                Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(SearchCriteria["_CircleID"]), -1, (int)Constants.DropDownFirstOption.All);


                ListItem DivisionItem = ddlDivision.Items.FindByValue(Convert.ToString(SearchCriteria["_DivisionID"]));
                if (DivisionItem != null)
                {
                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Add(DivisionItem);
                    Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(SearchCriteria["_DivisionID"]));
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(SearchCriteria["_DivisionID"]), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.SetSelectedValue(ddlSubDivision, Convert.ToString(SearchCriteria["_SubDivisionID"]));
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SetDropDownsForSearchCriteria(dynamic SearchCriteria)
        {
            try
            {
                List<long> lstUserZone = (List<long>)ViewState[UserZoneKey];
                List<long> lstUserCircle = (List<long>)ViewState[UserCircleKey];
                List<long> lstUserDiv = (List<long>)ViewState[UserDivisionKey];
                List<long> lstUserSubDiv = (List<long>)ViewState[UserSubDivisionKey];

                List<CO_Zone> lstzones = new ScheduleInspectionBLL().GetUserZones(lstUserZone);
                ddlZone.DataSource = lstzones;
                ddlZone.DataTextField = "Name";
                ddlZone.DataValueField = "ID";
                ddlZone.DataBind();

                if (lstUserZone.Count() > 0)
                {
                    if (lstUserZone.Count() > 1)
                        ddlZone.Items.Insert(0, new ListItem("All", ""));

                    ddlZone.SelectedValue = Convert.ToString(SearchCriteria["_ZoneID"]);
                }
                else if (lstUserZone.Count() == 0)
                    ddlZone.Items.Insert(0, new ListItem("All", ""));

                // List<CO_Circle> lstCircles = new ScheduleInspectionBLL().GetUserCircles(lstUserCircle);
                List<CO_Circle> lstCircles = new CircleBLL().GetFilteredCircles(Convert.ToInt64(SearchCriteria["_ZoneID"]), lstUserCircle);
                ddlCircle.DataSource = lstCircles;
                ddlCircle.DataTextField = "Name";
                ddlCircle.DataValueField = "ID";
                ddlCircle.DataBind();

                if (lstCircles.Count() > 0)
                {
                    if (lstCircles.Count() > 1)
                        ddlCircle.Items.Insert(0, new ListItem("All", ""));
                    ddlCircle.SelectedValue = Convert.ToString(SearchCriteria["_CircleID"]);
                }
                else if (lstCircles.Count() == 0)
                    ddlCircle.Items.Insert(0, new ListItem("All", ""));

                //List<CO_Division> lstDivisions = new ScheduleInspectionBLL().GetUserDivision(lstUserDiv);
                List<CO_Division> lstDivisions = new DivisionBLL().GetFilteredDivisions(Convert.ToInt64(SearchCriteria["_CircleID"]), lstUserDiv);
                ddlDivision.DataSource = lstDivisions;
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataValueField = "ID";
                ddlDivision.DataBind();

                if (lstDivisions.Count() > 0)
                {
                    if (lstDivisions.Count() > 1)
                        ddlDivision.Items.Insert(0, new ListItem("All", ""));
                    ddlDivision.SelectedValue = Convert.ToString(SearchCriteria["_DivisionID"]);
                }
                else if (lstDivisions.Count() == 0)
                    ddlDivision.Items.Insert(0, new ListItem("All", ""));

                //List<CO_SubDivision> lstSubDiv = new ScheduleInspectionBLL().GetUserSubDivision(lstUserSubDiv);
                List<CO_SubDivision> lstSubDiv = new SubDivisionBLL().GetFilteredSubDivisions(Convert.ToInt64(SearchCriteria["_DivisionID"]), lstUserSubDiv);
                ddlSubDivision.DataSource = lstSubDiv;
                ddlSubDivision.DataTextField = "Name";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();

                if (lstSubDiv.Count() > 0)
                {
                    if (lstSubDiv.Count() > 1)
                        ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                    ddlSubDivision.SelectedValue = Convert.ToString(SearchCriteria["_SubDivisionID"]);
                }
                else if (lstSubDiv.Count() == 0)
                    ddlSubDivision.Items.Insert(0, new ListItem("All", ""));

            }
            catch (Exception)
            {

                throw;
            }
        }


        private void DDLEmptyCircleDivisionSubDivision()
        {
            try
            {
                // Bind empty circle dropdownlist
                Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
                // Bind empty division dropdownlist
                Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                // Bind empty sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void DDLEmptyDivisionSubDivision()
        {
            try
            {
                // Bind empty division dropdownlist
                Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
                // Bind empty sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetUserCurrentLocation(long _From, long _ID)
        {

            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

            UA_Users mdlUser = new UserBLL().GetUserByID((long)HttpContext.Current.Session[SessionValues.UserID]);

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

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                if (_From == (long)Constants.IrrigationLevelID.Circle)
                                {

                                    CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);

                                    lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                                    lstUserCircle.Add((long)mdlDivision.CircleID);
                                    lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                                }
                            }

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            #region Sub Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSubDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserDivision.Add((long)mdlSubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                        {
                            #region Section Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSection.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserSubDivision.Add((long)mdlSection.SubDivID);
                                lstUserDivision.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
                            }
                            #endregion
                        }
                    }
                }
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
            ViewState.Add(UserSubDivisionKey, lstUserSubDivision);
            ViewState.Add(UserSectionKey, lstUserSection);
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlZone.SelectedValue != String.Empty)
                {
                    List<long> lstUserCircle = (List<long>)ViewState[UserCircleKey];

                    long SelectedZoneID = Convert.ToInt64(ddlZone.SelectedValue);

                    if (lstUserCircle.Count() > 0)
                    {
                        List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                        ddlCircle.DataSource = lstCircle;
                        ddlCircle.DataTextField = "Name";
                        ddlCircle.DataValueField = "ID";
                        ddlCircle.DataBind();
                        long SelectedCircleID = -1;// lstCircle.FirstOrDefault().ID;

                        if (lstCircle.Count() > 1)
                            ddlCircle.Items.Insert(0, new ListItem("All", ""));
                        else
                        {
                            SelectedCircleID = lstCircle.FirstOrDefault().ID;
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<long> lstUserDivision = (List<long>)ViewState[UserDivisionKey];

                            if (lstUserDivision.Count() > 0)
                            {
                                List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                                long SelectedDivisionID = -1;// lstDivision.FirstOrDefault().ID;

                                ddlDivision.DataSource = lstDivision;
                                ddlDivision.DataTextField = "Name";
                                ddlDivision.DataValueField = "ID";
                                ddlDivision.DataBind();

                                if (lstDivision.Count() > 1)
                                    ddlDivision.Items.Insert(0, new ListItem("All", ""));
                                else
                                {
                                    SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                                    List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                                    if (lstUserSubDivision.Count() > 0)
                                    {
                                        List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                                        long SelectedSubDivisionID = -1;// lstSubDivision.FirstOrDefault().ID;

                                        ddlSubDivision.DataSource = lstSubDivision;
                                        ddlSubDivision.DataTextField = "Name";
                                        ddlSubDivision.DataValueField = "ID";
                                        ddlSubDivision.DataBind();

                                        if (lstSubDivision.Count() > 1)
                                            ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                                        else
                                        {
                                            SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();
                                        }
                                    }
                                    else
                                    {
                                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                                    }
                                }
                            }
                            else
                            {
                                Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);
                                ddlSubDivision.Items.Clear();
                                ddlSubDivision.Items.Insert(0, BlankEntry);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
                        ddlDivision.Items.Clear();
                        ddlDivision.Items.Insert(0, BlankEntry);
                        ddlSubDivision.Items.Clear();
                        ddlSubDivision.Items.Insert(0, BlankEntry);
                    }
                }
                else
                {
                    ddlCircle.Items.Clear();
                    ddlCircle.Items.Insert(0, BlankEntry);

                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Insert(0, BlankEntry);

                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);

                    //ViewState.Add(UserCircleKey, null);
                    //ViewState.Add(UserDivisionKey, null);
                    //ViewState.Add(UserSubDivisionKey, null);
                    //ViewState.Add(UserSectionKey, null);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlCircle.SelectedValue != String.Empty)
                {
                    List<long> lstUserDivision = (List<long>)ViewState[UserDivisionKey];

                    long SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedValue);

                    if (lstUserDivision.Count() > 0)
                    {
                        List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                        long SelectedDivisionID = -1; //lstDivision.FirstOrDefault().ID;

                        ddlDivision.DataSource = lstDivision;
                        ddlDivision.DataTextField = "Name";
                        ddlDivision.DataValueField = "ID";
                        ddlDivision.DataBind();

                        if (lstDivision.Count() > 1)
                        {
                            ddlDivision.Items.Insert(0, new ListItem("All", ""));
                            Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                        }
                        else
                        {
                            SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                            if (lstUserDivision.Count() > 0)
                            {
                                List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                                if (lstSubDivision.Count() > 0)
                                {
                                    ddlSubDivision.DataSource = lstSubDivision;
                                    ddlSubDivision.DataTextField = "Name";
                                    ddlSubDivision.DataValueField = "ID";
                                    ddlSubDivision.DataBind();

                                    if (lstSubDivision.Count() > 1)
                                        ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                                    else
                                    {
                                        long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                                        ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();
                                    }
                                }
                                else
                                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                            }
                            else
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                        ddlSubDivision.Items.Clear();
                        ddlSubDivision.Items.Insert(0, BlankEntry);
                    }
                }
                else
                {
                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Insert(0, BlankEntry);

                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);

                    //ViewState.Add(UserCircleKey, null);
                    //ViewState.Add(UserDivisionKey, null);
                    //ViewState.Add(UserSubDivisionKey, null);
                    //ViewState.Add(UserSectionKey, null);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlDivision.SelectedValue != String.Empty)
                {
                    List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                    long SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedValue);

                    if (lstUserSubDivision.Count() > 0)
                    {
                        List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                        ddlSubDivision.DataSource = lstSubDivision;
                        ddlSubDivision.DataTextField = "Name";
                        ddlSubDivision.DataValueField = "ID";
                        ddlSubDivision.DataBind();

                        if (lstSubDivision.Count() > 1)
                            ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                        else if (lstSubDivision.Count() == 1)
                        {
                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);


                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Reset Circle,Division, Sub Division dropdownlists
        //        DDLEmptyCircleDivisionSubDivision();
        //        // Bind Circel dropdownlist based on Zone
        //        string zoneID = ddlZone.SelectedItem.Value;
        //        Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);

        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Reset division dropdownlist
        //        Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
        //        // Reset sub division dropdownlist
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        //        int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
        //        // Bind Division dropdownlist based on Circle 
        //        Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Reset sub division dropdownlist
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        //        // Bind Sub Division dropdownlist based on Division 
        //        long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        #endregion

        /// <summary>
        /// This function binds the screen controls
        /// Created on 14-07-2016
        /// </summary>
        /// <param name="_PreparedByID"></param>
        /// <param name="_StatusID"></param>
        private void BindScreenControls()
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (mdlUser.DesignationID == (long)Constants.Designation.MA || mdlUser.DesignationID == (long)Constants.Designation.SDO)
                {
                    CheckBoxApproval.Enabled = false;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        protected void gvSISearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        var PrepairedBy = e.Row.FindControl("lblPrepairedBy") as Label;
                        var PrepairedByDesignationID = e.Row.FindControl("lblPrepairedByDesignationID") as Label;
                        var ScheduleStatus = e.Row.FindControl("ScheduleStatusID") as Label;
                        var lblHasDependants = e.Row.FindControl("HasDependants") as Label;
                        var StatusID = Convert.ToInt64(ScheduleStatus.Text);
                        bool HasDependants = Convert.ToBoolean(lblHasDependants.Text);
                        var hyperlinkEdit = e.Row.FindControl("gvEdit") as HyperLink;
                        var hyperlinkNotes = e.Row.FindControl("gvINotes") as HyperLink;
                        LinkButton lnk2 = (LinkButton)e.Row.FindControl("linkButtonDelete");

                        if (mdlUser.DesignationID == (long)Constants.Designation.MA || mdlUser.DesignationID == (long)Constants.Designation.SDO)
                        {
                            if (StatusID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                hyperlinkNotes.CssClass += " disabled";
                            }
                            else if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                            {
                                hyperlinkNotes.CssClass += " disabled";
                                hyperlinkEdit.CssClass += " disabled";
                                lnk2.CssClass += " disabled";
                            }
                            else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                hyperlinkEdit.CssClass += " disabled";
                                lnk2.CssClass += " disabled";
                            }
                            else if (StatusID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                hyperlinkNotes.CssClass += " disabled";
                                hyperlinkEdit.CssClass += " disabled";
                                lnk2.CssClass += " disabled";
                            }
                        }
                        else if (mdlUser.DesignationID == (long)Constants.Designation.ADM || mdlUser.DesignationID == (long)Constants.Designation.XEN)
                        {
                            if (mdlUser.ID == Convert.ToInt64(PrepairedBy.Text))
                            {
                                if (StatusID == (long)Constants.SIScheduleStatus.Prepared)
                                {
                                    hyperlinkNotes.CssClass += " disabled";
                                }
                                else if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                {
                                    hyperlinkNotes.CssClass += " disabled";
                                    hyperlinkEdit.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                                else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                                {
                                    hyperlinkEdit.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                                else if (StatusID == (long)Constants.SIScheduleStatus.Rejected)
                                {
                                    hyperlinkNotes.CssClass += " disabled";
                                    hyperlinkEdit.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                            }
                            else
                            {
                                if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                {
                                    hyperlinkNotes.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                                else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                                {
                                    hyperlinkEdit.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                                else if (StatusID == (long)Constants.SIScheduleStatus.Rejected)
                                {
                                    hyperlinkNotes.CssClass += " disabled";
                                    hyperlinkEdit.CssClass += " disabled";
                                    lnk2.CssClass += " disabled";
                                }
                            }
                        }
                        else if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirector)
                        {
                            if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirector)
                            {
                                if (mdlUser.ID != Convert.ToInt64(PrepairedBy.Text))
                                {
                                    if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                    {
                                        hyperlinkNotes.CssClass += " disabled";

                                    }
                                    else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                                    {
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";
                                    }
                                    else if (StatusID == (long)Constants.SIScheduleStatus.Rejected)
                                    {
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";
                                    }
                                }
                            }
                        }
                        else if (mdlUser.DesignationID == (long)Constants.Designation.SE)
                        {
                            if (mdlUser.ID != Convert.ToInt64(PrepairedBy.Text))
                            {
                                if (Convert.ToInt64(PrepairedByDesignationID.Text) == (long)Constants.Designation.SDO)
                                {
                                    if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                    {
                                        hyperlinkNotes.CssClass += " disabled";
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";

                                    }
                                    else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                                    {
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";
                                    }
                                }
                                else
                                {
                                    if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                    {
                                        hyperlinkNotes.CssClass += " disabled";

                                    }
                                    else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                                    {
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";
                                    }
                                    else if (StatusID == (long)Constants.SIScheduleStatus.Rejected)
                                    {
                                        hyperlinkEdit.CssClass += " disabled";
                                        lnk2.CssClass += " disabled";
                                    }
                                }
                            }
                        }

                        if (StatusID == (long)Constants.SIScheduleStatus.Prepared && !lnk2.CssClass.Contains("disabled"))
                        {
                            if(HasDependants)
                            {
                                lnk2.OnClientClick = "return confirm('This schedule has dependant records. Are you sure you want to delete it?');";
                            }
                            else
                            {
                                lnk2.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                            }
                        }
                        else
                        {
                            lnk2.OnClientClick = "";
                        }
                    }

                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        private void LoadSearchCriteria(dynamic searchCriteria)
        {

            BindDropdownlists(searchCriteria);
            Dropdownlist.SetSelectedValue(ddlStatus, searchCriteria["_StatusID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_StatusID"]));
            txtFromDate.Text = Convert.ToString(searchCriteria["_fromDate"]);
            txtToDate.Text = Convert.ToString(searchCriteria["_toDate"]);
            CheckBoxApproval.Checked = Convert.ToBoolean(searchCriteria["_ApprovedCheck"]);
        }

        private void SearchChriteria()
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                    DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                    if (FromDate.Date > ToDate.Date)
                    {
                        Master.ShowMessage(Message.FromDateCannotBeGreaterToDate.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                List<long> lstUserDiv = (List<long>)ViewState[UserDivisionKey];
                List<long> lstUserSubDiv = (List<long>)ViewState[UserSubDivisionKey];

                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                long StatusID = ddlStatus.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlStatus.SelectedItem.Value);
                bool ApprovedCheck = CheckBoxApproval.Checked ? true : false;
                List<dynamic> lstScheduleSearch = new ScheduleInspectionBLL().GetSchedulesBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, StatusID, ApprovedCheck, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserDiv, lstUserSubDiv);
                gvSISearch.DataSource = lstScheduleSearch;
                gvSISearch.DataBind();


                dynamic searchCriteria = new
       {
           _ZoneID = ZoneID,
           _CircleID = CircleID,
           _DivisionID = DivisionID,
           _SubDivisionID = SubDivisionID,
           _StatusID = StatusID,
           _ApprovedCheck = ApprovedCheck,
           _fromDate = fromDate,
           _toDate = toDate
       };

                Session["SS_SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ActionOnSchedule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void LoadSearchCriteria(Int64 _ZoneID, Int64 _CircleID, Int64 _DivisionID, Int64 _SubDivisionID, Int64 _StatusID, bool _ApprovedCheck, string _fromDate, string _toDate)
        {

            try
            {
                List<long> lstUserDiv = (List<long>)ViewState[UserDivisionKey];
                List<long> lstUserSubDiv = (List<long>)ViewState[UserSubDivisionKey];

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                List<object> lstProtectionInfrastructureSearch = new ScheduleInspectionBLL().GetSchedulesBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _StatusID, _ApprovedCheck, _fromDate, _toDate, mdlUser.ID, mdlUser.DesignationID, lstUserDiv, lstUserSubDiv);

                gvSISearch.DataSource = lstProtectionInfrastructureSearch;
                gvSISearch.DataBind();

                gvSISearch.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}