using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    //public delegate void DelegatePopulateData();
    public partial class ChannelSearch : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    this.Form.DefaultButton = btnChannelSearch.UniqueID;

                    BindDropdownlists();
                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        string channelID = Convert.ToString(Request.QueryString["ChannelID"]);
                        hdnChannelID.Value = channelID;

                        if (_IsSaved)
                        {
                            txtChannelName.Text = Convert.ToString(HttpContext.Current.Session["ChannelName"]);
                            BindChannelSearchGrid(Convert.ToInt64(channelID));

                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                    }
                    if (Session["SC_SearchCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["SC_SearchCriteria"]);
                        this.LoadSearchCriteria(searchCriteria["_ZoneID"], searchCriteria["_CircleID"], searchCriteria["_DivisionID"], searchCriteria["_SubDivisionID"], searchCriteria["_ComandName"]
                            , searchCriteria["_ChannelType"], searchCriteria["_FlowType"], searchCriteria["_ChannelName"], searchCriteria["_IMISCode"], searchCriteria["_ParentChannelID"]);

                        LoadSearchCriteria(searchCriteria);
                    }
                    hlAddNewChannel.Visible = base.CanAdd;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            //Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(searchCriteria["_ZoneID"]), (int)Constants.DropDownFirstOption.All);
            //Dropdownlist.DDLDivisions(ddlDivision, Convert.ToString(searchCriteria["_CircleID"]) == string.Empty ? true : false, Convert.ToInt64(searchCriteria["_CircleID"]), -1, (int)Constants.DropDownFirstOption.All);
            //Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(searchCriteria["_DivisionID"]), (int)Constants.DropDownFirstOption.All);
            // Populate search criteria fields
            Dropdownlist.SetSelectedValue(ddlZone, searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]));
            Dropdownlist.SetSelectedValue(ddlCircle, searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]));
            Dropdownlist.SetSelectedValue(ddlDivision, searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]));
            Dropdownlist.SetSelectedValue(ddlSubDivision, searchCriteria["_SubDivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_SubDivisionID"]));
            Dropdownlist.SetSelectedValue(ddlCommanName, searchCriteria["_ComandName"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ComandName"]));
            Dropdownlist.SetSelectedValue(ddlChannelType, searchCriteria["_ChannelType"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ChannelType"]));
            Dropdownlist.SetSelectedValue(ddlFlowType, searchCriteria["_FlowType"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_FlowType"]));
            Dropdownlist.SetSelectedValue(ddlParentChannel, Convert.ToString(searchCriteria["_ParentChannelID"]));
            txtChannelName.Text = Convert.ToString(searchCriteria["_ChannelName"]);
            txtIMISCode.Text = Convert.ToString(searchCriteria["_IMISCode"]);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelSearch);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region "Dropdownlists Events


        #region Old Location Level Binding Work

        // Location Level Binding Change Request
        //public void BindUserLocationOld()
        //{
        //    List<long> lstUserZone = new List<long>();
        //    List<long> lstUserCircle = new List<long>();
        //    List<long> lstUserDivision = new List<long>();
        //    List<long> lstUserSubDivision = new List<long>();
        //    List<long> lstUserSection = new List<long>();

        //    long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

        //    UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

        //    //ViewState.Add(UserIDKey, mdlUser.ID);

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

        //                    if (lstZone.Count() > 1)
        //                    {
        //                        ddlCircle.Items.Insert(0, new ListItem("All", ""));
        //                        Dropdownlist.DDLCircles(ddlCircle, true, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
        //                    }
        //                    else
        //                    {
        //                        ddlZone.SelectedValue = SelectedZoneID.ToString();
        //                        Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
        //                    }

        //                    Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
        //                    Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

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

        //                    if (lstCircle.Count() > 1)
        //                    {
        //                        ddlCircle.Items.Insert(0, new ListItem("All", ""));
        //                        Dropdownlist.DDLDivisions(ddlDivision, true, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);
        //                    }
        //                    else
        //                    {
        //                        ddlCircle.SelectedValue = SelectedCircleID.ToString();
        //                        Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);
        //                    }

        //                    Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

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

        //                    if (lstDivision.Count() > 1)
        //                    {
        //                        ddlDivision.Items.Insert(0, new ListItem("All", ""));
        //                        Dropdownlist.DDLSubDivisions(ddlSubDivision, true, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
        //                    }
        //                    else
        //                    {
        //                        ddlDivision.SelectedValue = SelectedDivisionID.ToString();
        //                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
        //                    }

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

        //                    if (lstSubDivision.Count() > 1)
        //                    {
        //                        ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
        //                    }
        //                    //else
        //                    //{
        //                    //    ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();
        //                    //}

        //                    // Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);

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

        //                    if (lstSubDivision.Count() > 1)
        //                        ddlSubDivision.Items.Insert(0, new ListItem("All", ""));

        //                    //List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);
        //                    //long SelectedSectionID = lstSection.FirstOrDefault().ID;
        //                    //ddlSection.DataSource = lstSection;
        //                    //ddlSection.DataTextField = "Name";
        //                    //ddlSection.DataValueField = "ID";
        //                    //ddlSection.DataBind();
        //                    //ddlSection.SelectedValue = SelectedSectionID.ToString();

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
        //            DDLEmptyCircleDivisionSubDivision();
        //        }
        //    }
        //    else
        //    {
        //        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        //        DDLEmptyCircleDivisionSubDivision();
        //    }

        //    ViewState.Add("UserZoneKey", lstUserZone);
        //    ViewState.Add("UserCircleKey", lstUserCircle);
        //    ViewState.Add("UserDivisionKey", lstUserDivision);
        //    ViewState.Add("UserSubDivisionKey", lstUserSubDivision);
        //    ViewState.Add("UserSectionKey", lstUserSection);
        //}
        //         

        #endregion



        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            //ViewState.Add(UserIDKey, mdlUser.ID);

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
                        Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                        Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                        Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLZones(ddlCircle, true, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLZones(ddlDivision, true, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLZones(ddlSubDivision, true, (int)Constants.DropDownFirstOption.All);
            }

            ViewState.Add("UserZoneKey", lstUserZone);
            ViewState.Add("UserCircleKey", lstUserCircle);
            ViewState.Add("UserDivisionKey", lstUserDivision);
            ViewState.Add("UserSubDivisionKey", lstUserSubDivision);
        }

        private void BindDropdownlists()
        {
            try
            {
                // Bind Zone dropdownlist 
                //Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);

                //DDLEmptyCircleDivisionSubDivision();

                BindUserLocation();

                // Bind Channel type dropdownlist
                Dropdownlist.DDLChannelTypes(ddlChannelType, (int)Constants.DropDownFirstOption.All);
                // Bind Flow type dropdownlist
                Dropdownlist.DDLFlowTypes(ddlFlowType, (int)Constants.DropDownFirstOption.All);
                // Bind Common name dropdownlist
                Dropdownlist.DDLCommandNames(ddlCommanName, (int)Constants.DropDownFirstOption.All);
                //Bind Parent Channel dropdownlist
                Dropdownlist.DDLParentChannels(ddlParentChannel, (int)Constants.DropDownFirstOption.All);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void DDLEmptyCircleDivisionSubDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
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
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);
                gvChannel.Visible = false;

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
                Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                //Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);

                List<long> lstUserDivisions = (List<long>)ViewState["UserDivisionKey"];
                List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(circleID, lstUserDivisions);

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
                else if (lstDivision.Count() == 1)
                {
                    SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                    List<long> lstUserSubdivisions = (List<long>)ViewState["UserSubDivisionKey"];
                    List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubdivisions);

                    ddlSubDivision.DataSource = lstSubDivision;
                    ddlSubDivision.DataTextField = "Name";
                    ddlSubDivision.DataValueField = "ID";
                    ddlSubDivision.DataBind();

                    if (lstSubDivision.Count() > 1)
                        ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                    else if (lstSubDivision.Count() == 1)
                        ddlSubDivision.SelectedValue = lstSubDivision.FirstOrDefault().ID.ToString(); //SelectedSubDivisionID.ToString();
                    else
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
                }

                gvChannel.Visible = false;
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
                //Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);


                List<long> lstUserSubdivisions = (List<long>)ViewState["UserSubDivisionKey"];
                List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(divisionID, lstUserSubdivisions);

                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "Name";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();

                if (lstSubDivision.Count() > 1)
                    ddlSubDivision.Items.Insert(0, new ListItem("All", ""));
                else if (lstSubDivision.Count() == 1)
                    ddlSubDivision.SelectedValue = lstSubDivision.FirstOrDefault().ID.ToString(); //SelectedSubDivisionID.ToString();
                else
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);

                gvChannel.Visible = false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
        private void BindChannelSearchGrid(long _ChannelID)
        {
            //long channelID = Convert.ToInt64(hdnChannelID.Value) == 0 ? 0 : Convert.ToInt64(hdnChannelID.Value);
            try
            {
                string[] ParentChannel = null;
                long ParentChannelID = -1;
                long StructureTypeID = -1;
                long ChannelID = _ChannelID;
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                long ComandName = ddlCommanName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCommanName.SelectedItem.Value);
                long ChannelType = ddlChannelType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannelType.SelectedItem.Value);
                long FlowType = ddlFlowType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlFlowType.SelectedItem.Value);
                string ChannelName = txtChannelName.Text.Trim();
                string IMISCode = txtIMISCode.Text.Trim();
                bool IsParentChannelSelected = ddlParentChannel.SelectedItem.Value == string.Empty ? false : true;

                if (IsParentChannelSelected)
                {
                    ParentChannel = ddlParentChannel.SelectedItem.Value.Split(';');
                    ParentChannelID = Convert.ToInt64(ParentChannel[0]);
                    StructureTypeID = Convert.ToInt64(ParentChannel[1]);
                }

                List<long> lstUserZones = (List<long>)ViewState["UserZoneKey"];
                List<long> lstUserCircles = (List<long>)ViewState["UserCircleKey"];
                List<long> lstUserDivisions = (List<long>)ViewState["UserDivisionKey"];
                List<long> lstUserSubdivisions = (List<long>)ViewState["UserSubDivisionKey"];

                List<object> lstChannelSearch = new ChannelBLL().GetChannelsBySearchCriteria(_ChannelID, ZoneID, CircleID, DivisionID, SubDivisionID, ComandName, ChannelType, FlowType, ChannelName, IMISCode, ParentChannelID, StructureTypeID, lstUserZones, lstUserCircles, lstUserDivisions, lstUserSubdivisions);
                gvChannel.DataSource = lstChannelSearch;
                gvChannel.DataBind();
                gvChannel.Visible = true;

                dynamic searchCriteria = new
                {
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _SubDivisionID = SubDivisionID,
                    _ComandName = ComandName,
                    _ChannelType = ChannelType,
                    _FlowType = FlowType,
                    _ChannelName = ChannelName,
                    _ParentChannelID = ddlParentChannel.SelectedItem.Value,
                    _IMISCode = IMISCode
                };

                Session["SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void btnChannelSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindChannelSearchGrid(0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long channelID = Convert.ToInt64(gvChannel.DataKeys[e.Row.RowIndex].Value);
                HyperLink hlGauges = (HyperLink)e.Row.FindControl("hlGauges");
                HyperLink hlParentChannelsChannelFeeders = (HyperLink)e.Row.FindControl("hlParentChannelsChannelFeeders");
                HyperLink hlOutlets = (HyperLink)e.Row.FindControl("hlOutlets");
                HyperLink hlReach = (HyperLink)e.Row.FindControl("hlReach");
                Button btnIMISCode = (Button)e.Row.FindControl("btnlIMIS");

                bool isIrrigationBoundariesExists = new ChannelBLL().IsChannelIrrigationBoundariesExists(channelID);
                bool IsChannelParentFeederExists = new ChannelBLL().IsChannelParentFeederExistsForIMIS(channelID);
                if (!isIrrigationBoundariesExists)
                {
                    hlGauges.CssClass += " disabled";
                    hlParentChannelsChannelFeeders.CssClass += " disabled";
                    hlOutlets.CssClass += " disabled";
                    hlReach.CssClass += " disabled";
                }
                if (IsChannelParentFeederExists)
                {
                    btnIMISCode.Enabled = true;
                }

            }
        }
        protected void gvChannel_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string channelID = Convert.ToString(gvChannel.DataKeys[e.RowIndex].Values["ChannelID"]);

            if (new ChannelBLL().IsChannelDependencyExists(Convert.ToInt64(channelID)))
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                return;
            }

            bool isDeleted = new ChannelBLL().DeleteChannel(Convert.ToInt64(channelID));
            if (isDeleted == true)
                BindChannelSearchGrid(Convert.ToInt64(hdnChannelID.Value));
        }
        protected void gvChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannel.PageIndex = e.NewPageIndex;

                BindChannelSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void LoadSearchCriteria(Int64 _ZoneID, Int64 _CircleID, Int64 _DivisionID, Int64 _SubDivisionID, Int64 _ComandName,
            Int64 _ChannelType, Int64 _FlowType, string _ChannelName, string _IMISCode, string _ParentChannelID)
        {
            string[] ParentChannel = null;
            long ParentChannelID = -1;
            long StructureTypeID = -1;

            try
            {
                bool IsParentChannelSelected = _ParentChannelID == string.Empty ? false : true;

                if (IsParentChannelSelected)
                {
                    ParentChannel = _ParentChannelID.Split(';');
                    ParentChannelID = Convert.ToInt64(ParentChannel[0]);
                    StructureTypeID = Convert.ToInt64(ParentChannel[1]);
                }

                List<long> lstUserZones = (List<long>)ViewState["UserZoneKey"];
                List<long> lstUserCircles = (List<long>)ViewState["UserCircleKey"];
                List<long> lstUserDivisions = (List<long>)ViewState["UserDivisionKey"];
                List<long> lstUserSubdivisions = (List<long>)ViewState["UserSubDivisionKey"];

                List<object> lstChannelSearch = new ChannelBLL().GetChannelsBySearchCriteria(0, _ZoneID, _CircleID, _DivisionID, _SubDivisionID, _ComandName, _ChannelType, _FlowType, _ChannelName.Trim(), _IMISCode.Trim(), ParentChannelID, StructureTypeID, lstUserZones, lstUserCircles, lstUserDivisions, lstUserSubdivisions);
                gvChannel.DataSource = lstChannelSearch;
                gvChannel.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "IMISCode")
                {
                    long ChannelID = Convert.ToInt64(e.CommandArgument);
                    bool IsChannelParentFeederExists = new ChannelBLL().IsChannelParentFeederExistsForIMIS(ChannelID);
                    if (IsChannelParentFeederExists)
                    {
                        Response.Redirect("IMISCode.aspx?ChannelID=" + ChannelID, false);
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}