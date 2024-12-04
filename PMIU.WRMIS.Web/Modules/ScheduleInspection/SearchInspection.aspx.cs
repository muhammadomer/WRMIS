using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class SearchInspection : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindUserLocation();
                    if (_IsSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        _IsSaved = false; // Reset flag after displaying message.
                    }
                    if (Session["SearchInspectionCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["SearchInspectionCriteria"]);
                        LoadSearchCriteria(searchCriteria);
                    }
                    else
                    {
                        DateTime CurrentDate = DateTime.Now;
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-30)));
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(+30)));
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["From"]))
                    {
                        string from = Convert.ToString(Request.QueryString["From"]);
                        if (from.Equals("OutletChecking"))
                        {
                            ddlInspectionType.SelectedValue = Convert.ToString(Constants.InspectionType.OutletChecking);
                        }
                        BtnSearch_Click(null, null);
                    }
                    if (true)
                    {
                        
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region DropdownListsBinding

        //private void BindDropdownlists()
        //{
        //    try
        //    {
        //        BindInspectionCategoryDropDown();
        //        BindInspectionTypeDropDown();
        //        UA_AssociatedLocation userLocation = SessionManagerFacade.UserAssociatedLocations;
        //        switch (SessionManagerFacade.UserInformation.DesignationID)
        //        {
        //            case (long)Constants.Designation.SDO:
        //                if (userLocation != null && (long)Constants.IrrigationLevelID.SubDivision == userLocation.IrrigationLevelID)
        //                {
        //                    LoadAllDropdownlistsData(new SubDivisionBLL().GetByID(userLocation.IrrigationBoundryID.Value).DivisionID.Value);
        //                    Dropdownlist.SetSelectedValue(ddlSubDivision, Convert.ToString(userLocation.IrrigationBoundryID.Value));
        //                }
        //                break;
        //            case (long)Constants.Designation.XEN:
        //            case (long)Constants.Designation.MA:
        //            case (long)Constants.Designation.ADM:
        //                if (userLocation != null && (long)Constants.IrrigationLevelID.Division == userLocation.IrrigationLevelID)
        //                    LoadAllDropdownlistsData(userLocation.IrrigationBoundryID.Value);
        //                break;
        //            case (long)Constants.Designation.DataAnalyst:
        //            case (long)Constants.Designation.ChiefMonitoring:
        //                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //                DDLEmptyCircleDivisionSubDivision();
        //                break;
        //            case (long)Constants.Designation.ChiefIrrigation:
        //                if (userLocation != null && (long)Constants.IrrigationLevelID.Zone == userLocation.IrrigationLevelID)
        //                {
        //                    // Bind Zone dropdownlist 
        //                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //                    Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(userLocation.IrrigationBoundryID.Value));
        //                    Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //                    DDLEmptyDivisionSubDivision();
        //                }
        //                break;
        //            case (long)Constants.Designation.SE:
        //                if (userLocation != null && (long)Constants.IrrigationLevelID.Circle == userLocation.IrrigationLevelID)
        //                {
        //                    CO_Circle circle = new CircleBLL().GetByID(userLocation.IrrigationBoundryID.Value);
        //                    // Bind Zone dropdownlist 
        //                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //                    Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
        //                    Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //                    Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
        //                    Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value), -1, (int)Constants.DropDownFirstOption.All);
        //                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, -1, (int)Constants.DropDownFirstOption.All);
        //                }
        //                break;
        //            case (long)Constants.Designation.DeputyDirector:


        //                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //                Dropdownlist.DDLCircles(ddlCircle, false, -1, (int)Constants.DropDownFirstOption.All);
        //                Dropdownlist.DDLDivisions(ddlDivision, false, -1, -1, (int)Constants.DropDownFirstOption.All);
        //                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, -1, (int)Constants.DropDownFirstOption.All);

        //                break;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }

        //}


        #region Previous Location binding Work

        //public void BindUserLocation()
        //{
        //    List<long> lstUserZone = new List<long>();
        //    List<long> lstUserCircle = new List<long>();
        //    List<long> lstUserDivision = new List<long>();
        //    List<long> lstUserSubDivision = new List<long>();
        //    List<long> lstUserSection = new List<long>();
        //    BindInspectionCategoryDropDown();
        //    BindInspectionTypeDropDown();
        //    DropDownsDisabledModeCheck();
        //    long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

        //    UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

        //    ViewState.Add(UserIDKey, mdlUser.ID);

        //    if (mdlUser.RoleID != Constants.AdministratorRoleID)
        //    {
        //        if (mdlUser.UA_Designations.IrrigationLevelID != null)
        //        {
        //            List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

        //            if (lstAssociatedLocation.Count() > 0)
        //            {
        //                if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
        //                {
        //                    #region Zone Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    ddlZone.DataSource = lstZone;
        //                    ddlZone.DataTextField = "Name";
        //                    ddlZone.DataValueField = "ID";
        //                    ddlZone.DataBind();
        //                    ddlZone.SelectedValue = SelectedZoneID.ToString();

        //                    Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

        //                    #endregion
        //                }
        //                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
        //                {
        //                    #region Circle Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

        //                        CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                        lstUserZone.Add(mdlCircle.ZoneID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    ddlZone.DataSource = lstZone;
        //                    ddlZone.DataTextField = "Name";
        //                    ddlZone.DataValueField = "ID";
        //                    ddlZone.DataBind();
        //                    ddlZone.SelectedValue = SelectedZoneID.ToString();

        //                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

        //                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;

        //                    ddlCircle.DataSource = lstCircle;
        //                    ddlCircle.DataTextField = "Name";
        //                    ddlCircle.DataValueField = "ID";
        //                    ddlCircle.DataBind();
        //                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

        //                    Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

        //                    #endregion
        //                }
        //                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
        //                {
        //                    #region Division Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

        //                        CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                        lstUserCircle.Add((long)mdlDivision.CircleID);
        //                        lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    ddlZone.DataSource = lstZone;
        //                    ddlZone.DataTextField = "Name";
        //                    ddlZone.DataValueField = "ID";
        //                    ddlZone.DataBind();
        //                    ddlZone.SelectedValue = SelectedZoneID.ToString();

        //                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

        //                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;

        //                    ddlCircle.DataSource = lstCircle;
        //                    ddlCircle.DataTextField = "Name";
        //                    ddlCircle.DataValueField = "ID";
        //                    ddlCircle.DataBind();
        //                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

        //                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

        //                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

        //                    ddlDivision.DataSource = lstDivision;
        //                    ddlDivision.DataTextField = "Name";
        //                    ddlDivision.DataValueField = "ID";
        //                    ddlDivision.DataBind();
        //                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

        //                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

        //                    #endregion
        //                }
        //                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
        //                {
        //                    #region Sub Division Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserSubDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

        //                        CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                        lstUserDivision.Add((long)mdlSubDivision.DivisionID);
        //                        lstUserCircle.Add((long)mdlSubDivision.CO_Division.CircleID);
        //                        lstUserZone.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    ddlZone.DataSource = lstZone;
        //                    ddlZone.DataTextField = "Name";
        //                    ddlZone.DataValueField = "ID";
        //                    ddlZone.DataBind();
        //                    ddlZone.SelectedValue = SelectedZoneID.ToString();

        //                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

        //                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;

        //                    ddlCircle.DataSource = lstCircle;
        //                    ddlCircle.DataTextField = "Name";
        //                    ddlCircle.DataValueField = "ID";
        //                    ddlCircle.DataBind();
        //                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

        //                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

        //                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

        //                    ddlDivision.DataSource = lstDivision;
        //                    ddlDivision.DataTextField = "Name";
        //                    ddlDivision.DataValueField = "ID";
        //                    ddlDivision.DataBind();
        //                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

        //                    List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

        //                    long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

        //                    ddlSubDivision.DataSource = lstSubDivision;
        //                    ddlSubDivision.DataTextField = "Name";
        //                    ddlSubDivision.DataValueField = "ID";
        //                    ddlSubDivision.DataBind();
        //                    ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();



        //                    #endregion
        //                }
        //                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
        //                {
        //                    #region Section Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserSection.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

        //                        CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                        lstUserSubDivision.Add((long)mdlSection.SubDivID);
        //                        lstUserDivision.Add((long)mdlSection.CO_SubDivision.DivisionID);
        //                        lstUserCircle.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
        //                        lstUserZone.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    ddlZone.DataSource = lstZone;
        //                    ddlZone.DataTextField = "Name";
        //                    ddlZone.DataValueField = "ID";
        //                    ddlZone.DataBind();
        //                    ddlZone.SelectedValue = SelectedZoneID.ToString();

        //                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

        //                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;

        //                    ddlCircle.DataSource = lstCircle;
        //                    ddlCircle.DataTextField = "Name";
        //                    ddlCircle.DataValueField = "ID";
        //                    ddlCircle.DataBind();
        //                    ddlCircle.SelectedValue = SelectedCircleID.ToString();

        //                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

        //                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

        //                    ddlDivision.DataSource = lstDivision;
        //                    ddlDivision.DataTextField = "Name";
        //                    ddlDivision.DataValueField = "ID";
        //                    ddlDivision.DataBind();
        //                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

        //                    List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

        //                    long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

        //                    ddlSubDivision.DataSource = lstSubDivision;
        //                    ddlSubDivision.DataTextField = "Name";
        //                    ddlSubDivision.DataValueField = "ID";
        //                    ddlSubDivision.DataBind();
        //                    ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();



        //                    #endregion
        //                }
        //            }
        //            else
        //            {
        //                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //            }
        //        }
        //        else
        //        {
        //            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //        }
        //    }
        //    else
        //    {
        //        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //    }

        //    ViewState.Add(UserCircleKey, lstUserCircle);
        //    ViewState.Add(UserDivisionKey, lstUserDivision);
        //    ViewState.Add(UserSubDivisionKey, lstUserSubDivision);
        //    ViewState.Add(UserSectionKey, lstUserSection);
        //}

        #endregion

        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();
            BindInspectionCategoryDropDown();
            BindInspectionTypeDropDown();
            DropDownsDisabledModeCheck();

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

        //private void LoadAllDropdownlistsData(long _DivisionID)
        //{
        //    try
        //    {
        //        CO_Division division = new DivisionBLL().GetByID(_DivisionID);
        //        CO_Circle circle = new CircleBLL().GetByID(division.CircleID.Value);
        //        // Bind Zone dropdownlist 
        //        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
        //        //ListItem zoneItem = ddlZone.Items.FindByValue(Convert.ToString(circle.ZoneID));
        //        //ListItem firstZ = ddlZone.Items[0];
        //        //ddlZone.Items.Clear();
        //        //ddlZone.Items.Add(firstZ);
        //        //ddlZone.Items.Add(zoneItem);
        //        Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //        //ListItem CircleItem = ddlCircle.Items.FindByValue(Convert.ToString(circle.ID));
        //        //ListItem firstC = ddlCircle.Items[0];
        //        //ddlCircle.Items.Clear();
        //        //ddlCircle.Items.Add(firstC);
        //        //ddlCircle.Items.Add(CircleItem);
        //        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
        //        Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value), -1, (int)Constants.DropDownFirstOption.Select);
        //        //ListItem DivisionItem = ddlDivision.Items.FindByValue(Convert.ToString(division.ID));
        //        //ListItem firstD = ddlDivision.Items[0];
        //        //ddlDivision.Items.Clear();
        //        //ddlDivision.Items.Add(firstD);
        //        //ddlDivision.Items.Add(DivisionItem);
        //        Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(division.ID));
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //private void DDLEmptyCircleDivisionSubDivision()
        //{
        //    try
        //    {
        //        // Bind empty circle dropdownlist
        //        Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
        //        // Bind empty division dropdownlist
        //        Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
        //        // Bind empty sub division dropdownlist
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //private void DDLEmptyDivisionSubDivision()
        //{
        //    try
        //    {
        //        // Bind empty division dropdownlist
        //        Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        //        // Bind empty sub division dropdownlist
        //        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
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

                        long SelectedCircleID = -1;// lstCircle.FirstOrDefault().ID;

                        ddlCircle.DataSource = lstCircle;
                        ddlCircle.DataTextField = "Name";
                        ddlCircle.DataValueField = "ID";
                        ddlCircle.DataBind();

                        if (lstCircle.Count() > 1)
                        {
                            ddlCircle.Items.Insert(0, new ListItem("All", ""));
                            ddlDivision.Items.Insert(0, new ListItem("All", ""));
                            ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                        }
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
                                {
                                    ddlDivision.Items.Insert(0, new ListItem("All", ""));
                                    ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                                }
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

                        long SelectedDivisionID = -1;// lstDivision.FirstOrDefault().ID;

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
                                    long SelectedSubDivisionID = -1;//lstSubDivision.FirstOrDefault().ID;

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

                        long SelectedSubDivisionID = -1; //lstSubDivision.FirstOrDefault().ID;

                        ddlSubDivision.DataSource = lstSubDivision;
                        ddlSubDivision.DataTextField = "Name";
                        ddlSubDivision.DataValueField = "ID";
                        ddlSubDivision.DataBind();

                        if (lstSubDivision.Count() > 1)
                            ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                        else if (lstSubDivision.Count() == 1)
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

        private void BindInspectionCategoryDropDown()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlInspectionCategory, CommonLists.GetInspectionCategories(), (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInspectionTypeDropDown()
        {
            try
            {
                ddlInspectionType.DataSource = CommonLists.GetInspectionTypes();
                ddlInspectionType.DataTextField = "Name";
                ddlInspectionType.DataValueField = "ID";
                ddlInspectionType.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }




        #endregion

        #region GaugeGrid
        protected void gvGuage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGuage.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGauge_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvGauge_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView GaugeGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-3";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Area";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Gauge (ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Discharge (Cusec)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Complaint";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    GaugeGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Discharge-BedLevel
        protected void gvDischargeTableBedLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDischargeTableBedLevel.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDischargeTableBedLevel_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvDischargeTableBedLevel_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView DischargeBedGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Area";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Value of Exponent(n)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Mean Depth (D in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    DischargeBedGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Discharge-CrestLevel
        protected void gvDischargeTableCrestlvl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDischargeTableCrestlvl.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDischargeTableCrestlvl_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvDischargeTableCrestlvl_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView DischargeCrestGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Area";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Breadth of Fall(B in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Head above Crest  (H in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    DischargeCrestGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region OutletPerformance Grid
        protected void gvOutletPerformance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletPerformance.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvOutletPerformance_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView OutletPerformanceGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Outlet";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Head above Crest (H in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Working Head (wh in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    OutletPerformanceGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region OutletHistory Grid
        protected void gvOutletAlterationHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletAlterationHistory.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletAlterationHistory_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvOutletAlterationHistory_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView OutletAlterationGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-3";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Outlet";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Height/Orifice (Y in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Working Head(ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    OutletAlterationGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Tender Grid
        protected void gvTenderMonitoring_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTenderMonitoring.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTenderMonitoring_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.Header)
                //{
                //e.Row.Cells[1].Visible = false;
                //e.Row.Cells[3].Visible = false;
                //e.Row.Cells[4].Visible = false;
                //e.Row.Cells[5].Visible = false;
                //e.Row.Cells[6].Visible = false;
                //e.Row.Cells[7].Visible = false;
                //e.Row.Cells[8].Visible = false;
                //e.Row.Cells[9].Visible = false;

                //}
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvTenderMonitoring.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string TenderNoticeID = Convert.ToString(key.Values[1]);
                        string TenderWorkID = Convert.ToString(key.Values[2]);
                        string WorkSourceID = Convert.ToString(key.Values[3]);
                        var hyperlink = e.Row.FindControl("hlTenderMonitoring") as HyperLink;
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM)
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorkID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                        }
                        else if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorkID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvTenderMonitoring_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                //if (e.Row.RowType == DataControlRowType.Header)
                //{
                //    GridView TenderGrid = (GridView)sender;

                //    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                //    TableHeaderCell HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Inspection";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                //    HeaderCell.ColumnSpan = 2;
                //    HeaderCell.CssClass = "col-md-2";
                //    HeaderCell.Style["text-align"] = "center";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);




                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Division";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-2";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);


                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Tender Notice";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-2";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);

                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Work";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-2";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);

                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Opening Date";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-2";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);

                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Remarks";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-3";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);


                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "Complaint";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-1";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);

                //    HeaderCell = new TableHeaderCell();
                //    HeaderCell.Text = "";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //    HeaderCell.RowSpan = 2;
                //    HeaderCell.CssClass = "col-md-1";
                //    HeaderCell.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);

                //    TenderGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                //}

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Closure Operations

        protected void gvClosureOperations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvClosureOperations.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvClosureOperations.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string WorkSourceID = Convert.ToString(key.Values[1]);
                        string WorkSource = Convert.ToString(key.Values[2]);
                        string DivisionID = Convert.ToString(key.Values[3]);
                        string RefMonitoringID = Convert.ToString(key.Values[5]);
                        string CWPID = Convert.ToString(key.Values[6]);
                        var hyperlink = e.Row.FindControl("hlClosureOperations") as HyperLink;
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                        {
                            if (string.IsNullOrEmpty(RefMonitoringID))
                            {
                                hyperlink.NavigateUrl = string.Format("/../../Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(WorkSourceID) + "&CWPID=" + Convert.ToInt64(CWPID) + "&ScheduleDetailID=" + Convert.ToInt64(ScheduleDetailID));
                            }
                            else
                            {
                                hyperlink.NavigateUrl = string.Format("/../../Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(WorkSourceID) + "&CWPID=" + Convert.ToInt64(CWPID) + "&RefMonitoringID=" + Convert.ToInt64(RefMonitoringID));
                            }

                        }

                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvClosureOperations_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {



            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region General Inspections

        protected void gvGeneralInspections_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGeneralInspections.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvGeneralInspections.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values[0]);
                        string ScheduleDetailID = Convert.ToString(key.Values[1]);
                        var hyperlink = e.Row.FindControl("hlGeneralInspections") as HyperLink;
                        //if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                        //{
                        if (string.IsNullOrEmpty(ScheduleDetailID))
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddGeneralInspections.aspx?GID=" + Convert.ToInt64(ID) + "&Source=I");
                        }
                        else
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddGeneralInspections.aspx?ScheduleDetailID=" + Convert.ToInt64(ScheduleDetailID) + "&IsViewMode=true&Source=I");
                        }

                        //}

                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }


        #endregion
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGridView()
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

                List<long> lstUserZone = (List<long>)ViewState[UserZoneKey];
                List<long> lstUserCircle = (List<long>)ViewState[UserCircleKey];
                List<long> lstUserDiv = (List<long>)ViewState[UserDivisionKey];
                List<long> lstUserSubDiv = (List<long>)ViewState[UserSubDivisionKey];

                long InspectionCategory = ddlInspectionCategory.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlInspectionCategory.SelectedItem.Value);
                long InspectionType = ddlInspectionType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlInspectionType.SelectedItem.Value);
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                switch (InspectionType)
                {
                    case 1:
                        List<dynamic> lstGaugeInspectionReadings = new ScheduleInspectionBLL().GetGaugeInspectionBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvGuage.DataSource = lstGaugeInspectionReadings;
                        gvGuage.DataBind();
                        gvGuage.Visible = true;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 2:

                        List<dynamic> lstDischargeTableBedLevelReadings = new ScheduleInspectionBLL().GetDischargeTableBedLevelBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvDischargeTableBedLevel.DataSource = lstDischargeTableBedLevelReadings;
                        gvDischargeTableBedLevel.DataBind();
                        gvDischargeTableBedLevel.Visible = true;
                        gvGuage.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 3:

                        List<dynamic> lstDischargeTableCrestLevelReadings = new ScheduleInspectionBLL().GetDischargeTableCrestLevelBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvDischargeTableCrestlvl.DataSource = lstDischargeTableCrestLevelReadings;
                        gvDischargeTableCrestlvl.DataBind();
                        gvDischargeTableCrestlvl.Visible = true;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 4:

                        List<dynamic> lstOutletPerformanceReadings = new ScheduleInspectionBLL().GetOutletPerformanceBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvOutletPerformance.DataSource = lstOutletPerformanceReadings;
                        gvOutletPerformance.DataBind();
                        gvOutletPerformance.Visible = true;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 5:
                        List<dynamic> lstOutletAlterationReadings = new ScheduleInspectionBLL().GetOutletAlterationHistoryBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvOutletAlterationHistory.DataSource = lstOutletAlterationReadings;
                        gvOutletAlterationHistory.DataBind();
                        gvOutletAlterationHistory.Visible = true;
                        gvOutletPerformance.Visible = false;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 6:
                        List<dynamic> lstTenderMonitoring = new ScheduleInspectionBLL().GetTenderMonitoringBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvTenderMonitoring.DataSource = lstTenderMonitoring;
                        gvTenderMonitoring.DataBind();
                        gvTenderMonitoring.Visible = true;
                        gvOutletAlterationHistory.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 7:
                        List<dynamic> lstClosureOperations = new ScheduleInspectionBLL().GetClosureOperationsBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvClosureOperations.DataSource = lstClosureOperations;
                        gvClosureOperations.DataBind();
                        gvClosureOperations.Visible = true;
                        gvTenderMonitoring.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvGeneralInspections.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 8:
                        List<dynamic> lstGeneralInspections = new ScheduleInspectionBLL().GetGeneralInspectionsBySearchCriteria(InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID);
                        gvGeneralInspections.DataSource = lstGeneralInspections;
                        gvGeneralInspections.DataBind();
                        gvGeneralInspections.Visible = true;
                        gvClosureOperations.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvOutletChecking.Visible = false;
                        break;
                    case 9:
                        List<dynamic> lstOutletChecking = new ScheduleInspectionBLL().GetOutletCheckingBySearchCriteria(ZoneID, CircleID, DivisionID, SubDivisionID, InspectionCategory, fromDate, toDate, mdlUser.ID, mdlUser.DesignationID, lstUserZone, lstUserCircle, lstUserDiv, lstUserSubDiv);
                        gvOutletChecking.DataSource = lstOutletChecking;
                        gvOutletChecking.DataBind();
                        gvOutletChecking.Visible = true;
                        gvGeneralInspections.Visible = false;
                        gvClosureOperations.Visible = false;
                        gvTenderMonitoring.Visible = false;
                        gvOutletAlterationHistory.Visible = false;
                        gvOutletPerformance.Visible = false;
                        gvGuage.Visible = false;
                        gvDischargeTableBedLevel.Visible = false;
                        gvDischargeTableCrestlvl.Visible = false;
                        gvOutletPerformance.Visible = false;
                        break;
                    default:
                        break;
                }
                dynamic searchCriteria = new
                {
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _SubDivisionID = SubDivisionID,
                    _InspectionCategory = InspectionCategory,
                    _FromDate = fromDate,
                    _ToDate = toDate,
                    _InspectionType = InspectionType
                };

                Session["SearchInspectionCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            //string _Zone = searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]);
            //string _Circel = searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]);
            //string _DivisionID = searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]);
            //string _SubDivisionID = searchCriteria["_SubDivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_SubDivisionID"]);

            //Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            //Dropdownlist.SetSelectedValue(ddlZone, _Zone);
            //ListItem zoneItem = ddlZone.Items.FindByValue(_Zone);
            //ddlZone.Items.Clear();
            //ddlZone.Items.Add(zoneItem);

            //Dropdownlist.DDLCircles(ddlCircle, false, _Zone == string.Empty ? -1 : Convert.ToInt64(_Zone), (int)Constants.DropDownFirstOption.All);

            //ListItem CircleItem = ddlCircle.Items.FindByValue(_Circel);
            //ddlCircle.Items.Clear();
            //ddlCircle.Items.Add(CircleItem);
            //Dropdownlist.SetSelectedValue(ddlCircle, _Circel);

            //Dropdownlist.DDLDivisions(ddlDivision, false, _Circel == string.Empty ? -1 : Convert.ToInt64(_Circel), -1, (int)Constants.DropDownFirstOption.All);


            //ListItem DivisionItem = ddlDivision.Items.FindByValue(_DivisionID);
            //ddlDivision.Items.Clear();
            //ddlDivision.Items.Add(DivisionItem);
            //Dropdownlist.SetSelectedValue(ddlDivision, _DivisionID);

            //Dropdownlist.DDLSubDivisions(ddlSubDivision, false, _DivisionID == string.Empty ? -1 : Convert.ToInt64(_DivisionID), (int)Constants.DropDownFirstOption.All);

            //Dropdownlist.SetSelectedValue(ddlSubDivision, _SubDivisionID);


            SetDropDownsForSearchCriteria(searchCriteria);
            BindInspectionCategoryDropDown();
            BindInspectionTypeDropDown();


            // Populate search criteria fields

            Dropdownlist.SetSelectedValue(ddlInspectionCategory, searchCriteria["_InspectionCategory"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_InspectionCategory"]));
            Dropdownlist.SetSelectedValue(ddlInspectionType, searchCriteria["_InspectionType"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_InspectionType"]));
            txtFromDate.Text = Convert.ToString(searchCriteria["_FromDate"]);
            txtToDate.Text = Convert.ToString(searchCriteria["_ToDate"]);
            DropDownsDisabledModeCheck();
            //Binding grid

            BindGridView();

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

        private void DropDownsDisabledModeCheck()
        {
            try
            {
                if (ddlInspectionType.SelectedValue == "6" || ddlInspectionType.SelectedValue == "7")
                {
                    ddlInspectionCategory.Enabled = false;
                    ddlSubDivision.Enabled = false;
                }
                else if (ddlInspectionType.SelectedValue == "8")
                {
                    ddlInspectionCategory.Enabled = true;
                    ddlZone.Enabled = false;
                    ddlCircle.Enabled = false;
                    ddlSubDivision.Enabled = false;
                    ddlDivision.Enabled = false;
                }
                else
                {
                    ddlInspectionCategory.Enabled = true;
                    ddlSubDivision.Enabled = true;
                    ddlZone.Enabled = true;
                    ddlCircle.Enabled = true;
                    ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInspectionType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownsDisabledModeCheck();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
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
        #region gvOutletChecking
        protected void gvOutletChecking_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView OutletCheckingGrid = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Inspection";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);




                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Division";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Channel";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Outlet";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Head above Crest (H in ft)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    OutletCheckingGrid.Controls[0].Controls.AddAt(0, HeaderRow);




                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletChecking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletChecking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletChecking.PageIndex = e.NewPageIndex;

                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

    }
}