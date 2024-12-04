using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using System.Web.Services;
using System.Web.Script.Services;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class AssociateLocationToUser : BasePage
    {

        #region ViewState

        public string Rights = "Rights";

        public string UserID_VS = "UserID";

        public string UserLevel_VS = "UserLevel";

        public string LevelID = "LevelID";

        public string RecordID = "RecordID";

        public static long? DesignationID;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                SetTitle();

                if (Session["IsSave"] != null)
                {
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
        }

        //summary  
        // Bind data for initial display of screen 
        public void BindData()
        {
            try
            {
                long LoggedUser = (long)HttpContext.Current.Session[SessionValues.UserID];
                string UserRole = new UserAdministrationBLL().GetRoleName(LoggedUser);
                if (UserRole != "Administrator")
                {
                    bool HasRights = Convert.ToBoolean(new UserAdministrationBLL().HasRights(LoggedUser));

                    if (!HasRights)
                    {
                        ViewState[Rights] = HasRights;
                        Master.ShowMessage(Message.NotAllowedToAdd.Description, SiteMaster.MessageType.Error);
                    }
                }

                long UserID = -1;
                if (ViewState[UserID_VS] != null)
                {
                    UserID = Convert.ToInt64(ViewState[UserID_VS]);
                }
                else
                {
                    UserID = Convert.ToInt64(Request.QueryString[UserID_VS]);
                    ViewState[UserID_VS] = UserID;
                }

                UA_IrrigationLevel UserLevel = null;
                object UserDetail = new UserAdministrationBLL().GetUserDetail(UserID);
                if (UserDetail != null)
                {
                    lblFullName.Text = UserDetail.GetType().GetProperty("FullName").GetValue(UserDetail).ToString();
                    lblUserName.Text = UserDetail.GetType().GetProperty("UserName").GetValue(UserDetail).ToString();
                    lblCellNumber.Text = UserDetail.GetType().GetProperty("Mobile").GetValue(UserDetail).ToString();
                    lblDesignation.Text =
                        UserDetail.GetType().GetProperty("Designation").GetValue(UserDetail).ToString();
                    string right = UserDetail.GetType().GetProperty("AuthorityRights").GetValue(UserDetail).ToString();
                    if (UserDetail.GetType().GetProperty("DesignationID").GetValue(UserDetail) != null)
                        DesignationID =
                            Convert.ToInt64(UserDetail.GetType().GetProperty("DesignationID").GetValue(UserDetail));
                }

                UserLevel = new UserAdministrationBLL().GetUserLevel(UserID);
                ViewState[UserLevel_VS] = UserLevel.Name;
                ViewState[LevelID] = UserLevel.ID;

                if (UserLevel != null || (ViewState[UserLevel_VS] != null))
                {
                    if (ViewState[UserLevel_VS] != null)
                    {
                        UserLevel.Name = ViewState[UserLevel_VS].ToString();
                    }

                    List<object> lstLevels =
                        new UserAdministrationBLL().GetAssignedLevelsList(UserID, UserLevel.ID, UserLevel.Name);
                    gvLocation.DataSource = lstLevels;
                    gvLocation.DataBind();
                }
                else
                {
                    Master.ShowMessage(Message.NoIrrigationLevel.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }

        }

        #region previously designed screen

        //protected void btnAssign_Click(object sender, EventArgs e)
        //{
        //    if (ViewState["UserLevel"] != null)
        //    {
        //        Boolean result = true;
        //        string UserLevel = ViewState["UserLevel"].ToString();

        //        if (UserLevel == "Zone")
        //        {
        //            string ZoneName = ddlZone.SelectedItem.Text.ToString();
        //            if (ZoneName != "Select")
        //            {
        //                long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
        //                foreach (ListItem itm in lstBoxAssigned.Items)
        //                {
        //                    if (itm.Text == ZoneName)
        //                    {
        //                        result = false;
        //                        Master.ShowMessage(Message.ZoneAlreadyAssigned.Description, SiteMaster.MessageType.Error);
        //                        //lblMessage.Text = Message.ZoneAlreadyAssigned.Description;
        //                        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                        //lblMessage.Visible = true;
        //                    }
        //                }

        //                if (result)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Text = ZoneName;
        //                    item.Value = ZoneID.ToString();
        //                    lstBoxAssigned.Items.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);

        //                //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //                //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                //lblMessage.Visible = true;
        //            }
        //        }
        //        else if (UserLevel == "Circle")
        //        {
        //            string CircleName = ddlCircle.SelectedItem.Text.ToString();

        //            if (CircleName != "Select")
        //            {

        //                long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);


        //                foreach (ListItem itm in lstBoxAssigned.Items)
        //                {
        //                    if (itm.Text == CircleName)
        //                    {
        //                        result = false;
        //                        Master.ShowMessage(Message.CircleAlreadyAssigned.Description, SiteMaster.MessageType.Error);
        //                        //lblMessage.Text = Message.CircleAlreadyAssigned.Description;
        //                        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                        //lblMessage.Visible = true;
        //                    }
        //                }

        //                if (result)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Value = CircleID.ToString();
        //                    item.Text = CircleName;
        //                    lstBoxAssigned.Items.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
        //                //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //                //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                //lblMessage.Visible = true;
        //            }
        //        }
        //        else if (UserLevel == "Division")
        //        {
        //            string DivisionName = ddlDivision.SelectedItem.Text.ToString();

        //            if (DivisionName != "Select")
        //            {
        //                long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
        //                foreach (ListItem itm in lstBoxAssigned.Items)
        //                {
        //                    if (itm.Text == DivisionName)
        //                    {
        //                        result = false;
        //                        Master.ShowMessage(Message.DivisionAlreadyAssigned.Description, SiteMaster.MessageType.Error);
        //                        //lblMessage.Text = Message.DivisionAlreadyAssigned.Description;
        //                        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                        //lblMessage.Visible = true;
        //                    }
        //                }

        //                if (result)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Value = DivisionID.ToString();
        //                    item.Text = DivisionName;
        //                    lstBoxAssigned.Items.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
        //                //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //                //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                //lblMessage.Visible = true;
        //            }
        //        }
        //        else if (UserLevel == "Sub Division")
        //        {
        //            string SubDivisionName = ddlSubDivision.SelectedItem.Text;

        //            if (SubDivisionName != "Select")
        //            {
        //                long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);


        //                foreach (ListItem itm in lstBoxAssigned.Items)
        //                {
        //                    if (itm.Text == SubDivisionName)
        //                    {
        //                        result = false;

        //                        Master.ShowMessage(Message.SubDivisionAlreadyAssigned.Description, SiteMaster.MessageType.Error);
        //                        //lblMessage.Text = Message.SubDivisionAlreadyAssigned.Description;
        //                        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                        //lblMessage.Visible = true;
        //                    }
        //                }

        //                if (result)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Text = SubDivisionName;
        //                    item.Value = SubDivisionID.ToString();
        //                    lstBoxAssigned.Items.Add(item);
        //                }
        //            }
        //            else
        //            {

        //                Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
        //                //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //                //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                //lblMessage.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            string SectionName = ddlSection.SelectedItem.Text.ToString();

        //            if (SectionName != "Select")
        //            {
        //                long SectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);
        //                foreach (ListItem itm in lstBoxAssigned.Items)
        //                {
        //                    if (itm.Text == SectionName)
        //                    {
        //                        result = false;
        //                        Master.ShowMessage(Message.SectionAlreadyAssigned.Description, SiteMaster.MessageType.Error);
        //                        //lblMessage.Text = Message.SectionAlreadyAssigned.Description;
        //                        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                        //lblMessage.Visible = true;
        //                    }
        //                }

        //                if (result)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Text = SectionName;
        //                    item.Value = SectionID.ToString();
        //                    lstBoxAssigned.Items.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
        //                //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //                //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                //lblMessage.Visible = true;
        //            }
        //        }
        //    }
        //}

        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    bool NoSelection = true;

        //    for (int i = 0; i < lstBoxAssigned.Items.Count; i++)
        //    {
        //        if (lstBoxAssigned.Items[i].Selected)
        //        {
        //            NoSelection = false;
        //            lstBoxAssigned.Items.Remove(lstBoxAssigned.Items[i]);
        //        }
        //    }

        //    if (NoSelection)
        //    {
        //        Master.ShowMessage(Message.RemovalSelect.Description, SiteMaster.MessageType.Error);
        //        //lblMessage.Text = Message.RemovalSelect.Description;
        //        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //        //lblMessage.Visible = true;
        //    }

        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    Boolean result = false;
        //    if (ViewState["UserLevel"] != null)
        //    {
        //        string UserLevel = ViewState["UserLevel"].ToString();

        //        if (UserLevel == "Zone")
        //        {
        //            string selection = ddlZone.SelectedItem.Text.ToString();
        //            if (selection != "Select")
        //                result = true;
        //        }
        //        else if (UserLevel == "Circle")
        //        {
        //            string selection = ddlCircle.SelectedItem.Text.ToString();
        //            if (selection != "Select")
        //                result = true;
        //        }
        //        else if (UserLevel == "Division")
        //        {
        //            string selection = ddlDivision.SelectedItem.Text.ToString();
        //            if (selection != "Select")
        //                result = true;
        //        }
        //        else if (UserLevel == "Sub Division")
        //        {
        //            string selection = ddlSubDivision.SelectedItem.Text.ToString();
        //            if (selection != "Select")
        //                result = true;
        //        }
        //        else if (UserLevel == "Section")
        //        {
        //            string selection = ddlSection.SelectedItem.Text.ToString();
        //            if (selection != "Select")
        //                result = true;
        //        }
        //    }

        //    if (result)
        //    {
        //        long UserID = -1;
        //        long LevelID = -1;

        //        if (ViewState["UserID"] != null)
        //        {
        //            UserID = Convert.ToInt64(ViewState["UserID"]);
        //            if (ViewState["LevelID"] != null)
        //            {
        //                LevelID = Convert.ToInt64(ViewState["LevelID"]);
        //                List<long> lstBoundryID = new List<long>();
        //                long BoundryID = -1;

        //                foreach (ListItem lstitem in lstBoxAssigned.Items)
        //                {
        //                    BoundryID = new long();
        //                    BoundryID = Convert.ToInt64(lstitem.Value);
        //                    lstBoundryID.Add(BoundryID);
        //                }

        //                if (lstBoundryID.Count() > 0)
        //                {
        //                    new UserAdministrationBLL().AddUserLocation(UserID, LevelID, lstBoundryID);

        //                    Master.ShowMessage(Message.LocationAssigned.Description, SiteMaster.MessageType.Success);
        //                    //lblMessage.Text = Message.LocationAssigned.Description;
        //                    //lblMessage.ForeColor = System.Drawing.Color.Green;
        //                    //lblMessage.Visible = true;
        //                }
        //                else
        //                {
        //                    Master.ShowMessage(Message.OneLocationMustBeAssigned.Description, SiteMaster.MessageType.Error);
        //                    //lblMessage.Text = Message.OneLocationMustBeAssigned.Description;
        //                    //lblMessage.ForeColor = System.Drawing.Color.Red;
        //                    //lblMessage.Visible = true;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
        //        //lblMessage.Text = Message.SelectionTillLeafLevel.Description;
        //        //lblMessage.ForeColor = System.Drawing.Color.Red;
        //        //lblMessage.Visible = true;
        //    }
        //}

        //protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    if (ViewState["UserLevel"] != null)
        //    {
        //        string LevelName = ViewState["UserLevel"].ToString();

        //        if (LevelName == "Circle")
        //        {
        //            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
        //        }
        //        else if (LevelName == "Division")
        //        {
        //            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true);
        //        }
        //        else if (LevelName == "Sub Division")
        //        {
        //            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
        //        }
        //        else if (LevelName == "Section")
        //        {
        //            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true);
        //        }
        //    }
        //}

        //protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ViewState["UserLevel"] != null)
        //    {
        //        string LevelName = ViewState["UserLevel"].ToString();

        //        if (LevelName == "Division")
        //        {
        //            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, false, CircleID);
        //        }
        //        else if (LevelName == "Sub Division")
        //        {
        //            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, false, CircleID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true);

        //        }
        //        else if (LevelName == "Section")
        //        {
        //            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, false, CircleID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
        //        }
        //    }
        //}

        //protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ViewState["UserLevel"] != null)
        //    {
        //        string LevelName = ViewState["UserLevel"].ToString();

        //        if (LevelName == "Sub Division")
        //        {
        //            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID);
        //        }
        //        else if (LevelName == "Section")
        //        {
        //            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true);
        //        }
        //    }
        //}

        //protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ViewState["UserLevel"] != null)
        //    {
        //        string LevelName = ViewState["UserLevel"].ToString();

        //        if (LevelName == "Section")
        //        {
        //            long SubDivsnID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
        //            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, false, SubDivsnID);
        //        }
        //    }
        //}

        #endregion

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.LocationToUser);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// rework starts here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        protected void gvLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    hlBack.Visible = false;
                    List<object> lstLevels = new UserAdministrationBLL().GetAssignedLevelsList(
                        Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]),
                        ViewState[UserLevel_VS].ToString());

                    object newRow = new
                    {
                        ID = -1,
                        zoneName = "",
                        zoneID = "",
                        circleName = "",
                        circleID = "",
                        divisionName = "",
                        divisionID = "",
                        subDivisionName = "",
                        subDivisionID = "",
                        sectionName = "",
                        sectioID = ""
                    };

                    ViewState[RecordID] = -1;
                    lstLevels.Add(newRow);
                    gvLocation.DataSource = lstLevels;
                    gvLocation.PageIndex = gvLocation.PageCount;
                    gvLocation.DataBind();

                    gvLocation.EditIndex = gvLocation.Rows.Count - 1;
                    gvLocation.DataBind();
                }
                else if (e.CommandName == "MultiAdd")
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    //if ((mdlUser.DesignationID == null && lblDesignation.Text.Trim() == "Chief Irrigation") // admin user
                    //    || (mdlUser.DesignationID == (long)Constants.Designation.Secretary))
                    if (lblDesignation.Text.Trim() == "Chief Irrigation")
                    {
                        MultipleSlection.Visible = true;
                        lblListleft.Text = "Zones";
                        lblListright.Text = "Selected Zones";
                        lstLeft.DataSource = new ZoneBLL().GetAllZones(true);
                        lstLeft.DataTextField = "Name";
                        lstLeft.DataValueField = "ID";
                        lstLeft.DataBind();
                    }
                    else
                        BindUserLocation(); //Dropdownlist.DDLZones(ddlMZone, false, (int)Constants.DropDownFirstOption.Select);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#AddMultipal').modal();", true);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }



        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

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

                            ddlMZone.DataSource = lstZone;
                            ddlMZone.DataTextField = "Name";
                            ddlMZone.DataValueField = "ID";
                            ddlMZone.DataBind();
                            ddlMZone.SelectedValue = SelectedZoneID.ToString();

                            if (lblDesignation.Text.Trim() == "SE")
                            {
                                MultipleSlection.Visible = true;
                                lblListleft.Text = "Circle";
                                lblListright.Text = "Selected Circle";
                                List<CO_Circle> lstCircle = new CircleBLL().GetCirclesByZoneID(SelectedZoneID);
                                lstLeft.DataSource = lstCircle;
                                lstLeft.DataTextField = "Name";
                                lstLeft.DataValueField = "ID";
                                lstLeft.DataBind();
                            }
                            else
                                Dropdownlist.DDLCircles(ddlMCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

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

                            ddlMZone.DataSource = lstZone;
                            ddlMZone.DataTextField = "Name";
                            ddlMZone.DataValueField = "ID";
                            ddlMZone.DataBind();
                            ddlMZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlMCircle.DataSource = lstCircle;
                            ddlMCircle.DataTextField = "Name";
                            ddlMCircle.DataValueField = "ID";
                            ddlMCircle.DataBind();
                            ddlMCircle.SelectedValue = SelectedCircleID.ToString();

                            if (lblDesignation.Text.Trim() == "XEN" || lblDesignation.Text.Trim() == "MA" || lblDesignation.Text.Trim() == "ADM")
                            {
                                MultipleSlection.Visible = true;
                                lblListleft.Text = "Division";
                                lblListright.Text = "Selected Division";
                                List<CO_Division> lstDivisions = new DivisionBLL().GetDivisionsByCircleIDAndDomainID(SelectedCircleID);
                                lstLeft.DataSource = lstDivisions;
                                lstLeft.DataTextField = "Name";
                                lstLeft.DataValueField = "ID";
                                lstLeft.DataBind();
                            }
                            else
                                Dropdownlist.DDLDivisions(ddlMDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

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

                            ddlMZone.DataSource = lstZone;
                            ddlMZone.DataTextField = "Name";
                            ddlMZone.DataValueField = "ID";
                            ddlMZone.DataBind();
                            ddlMZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlMCircle.DataSource = lstCircle;
                            ddlMCircle.DataTextField = "Name";
                            ddlMCircle.DataValueField = "ID";
                            ddlMCircle.DataBind();
                            ddlMCircle.SelectedValue = SelectedCircleID.ToString();

                            if (lblDesignation.Text.Trim() == "MA" || lblDesignation.Text.Trim() == "ADM")
                            {
                                MultipleSlection.Visible = true;
                                ddlMDivision.Visible = false;
                                ddlMSubDivision.Visible = false;
                                lblDivlbl.Visible = false;
                                lblSubdivlbl.Visible = false;
                                lblListleft.Text = "Division";
                                lblListright.Text = "Selected Division";
                                List<CO_Division> lstDivisionn = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);
                                //List<CO_SubDivision> lstSubDivisions = new SubDivisionBLL().GetSubDivisionsByDivisionID(SelectedDivisionID);
                                //lstLeft.DataSource = lstSubDivisions;
                                lstLeft.DataSource = lstDivisionn;
                                lstLeft.DataTextField = "Name";
                                lstLeft.DataValueField = "ID";
                                lstLeft.DataBind();
                            }
                            else
                            {
                                List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);
                                long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                                ddlMDivision.DataSource = lstDivision;
                                ddlMDivision.DataTextField = "Name";
                                ddlMDivision.DataValueField = "ID";
                                ddlMDivision.DataBind();
                                ddlMDivision.SelectedValue = SelectedDivisionID.ToString();

                                if (lblDesignation.Text.Trim() == "SDO") //|| lblDesignation.Text.Trim() == "MA" || lblDesignation.Text.Trim() == "ADM")
                                {
                                    ddlMSubDivision.Visible = false;
                                    lblSubdivlbl.Visible = false;
                                    MultipleSlection.Visible = true;
                                    lblListleft.Text = "Sub Division";
                                    lblListright.Text = "Selected Sub Division";
                                    List<CO_SubDivision> lstSubDivisions = new SubDivisionBLL().GetSubDivisionsByDivisionID(SelectedDivisionID);
                                    lstLeft.DataSource = lstSubDivisions;
                                    lstLeft.DataTextField = "Name";
                                    lstLeft.DataValueField = "ID";
                                    lstLeft.DataBind();
                                }
                                else
                                    Dropdownlist.DDLSubDivisions(ddlMSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);
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

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlMZone.DataSource = lstZone;
                            ddlMZone.DataTextField = "Name";
                            ddlMZone.DataValueField = "ID";
                            ddlMZone.DataBind();
                            ddlMZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlMCircle.DataSource = lstCircle;
                            ddlMCircle.DataTextField = "Name";
                            ddlMCircle.DataValueField = "ID";
                            ddlMCircle.DataBind();
                            ddlMCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlMDivision.DataSource = lstDivision;
                            ddlMDivision.DataTextField = "Name";
                            ddlMDivision.DataValueField = "ID";
                            ddlMDivision.DataBind();
                            ddlMDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlMSubDivision.DataSource = lstSubDivision;
                            ddlMSubDivision.DataTextField = "Name";
                            ddlMSubDivision.DataValueField = "ID";
                            ddlMSubDivision.DataBind();
                            ddlMSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                            if (lblDesignation.Text.Trim() == "SBE" || lblDesignation.Text.Trim() == "Gauge Reader" || lblDesignation.Text.Trim() == "Ziladaar")
                            {
                                MultipleSlection.Visible = true;
                                lblListleft.Text = "Section";
                                lblListright.Text = "Selected Section";
                                List<CO_Section> lstSections = new SectionBLL().GetSectionsBySubDivisionID(SelectedSubDivisionID);
                                lstLeft.DataSource = lstSections;
                                lstLeft.DataTextField = "Name";
                                lstLeft.DataValueField = "ID";
                                lstLeft.DataBind();
                            }

                            // Dropdownlist.DDLSections(ddlMSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);

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

                            ddlMZone.DataSource = lstZone;
                            ddlMZone.DataTextField = "Name";
                            ddlMZone.DataValueField = "ID";
                            ddlMZone.DataBind();
                            ddlMZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlMCircle.DataSource = lstCircle;
                            ddlMCircle.DataTextField = "Name";
                            ddlMCircle.DataValueField = "ID";
                            ddlMCircle.DataBind();
                            ddlMCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlMDivision.DataSource = lstDivision;
                            ddlMDivision.DataTextField = "Name";
                            ddlMDivision.DataValueField = "ID";
                            ddlMDivision.DataBind();
                            ddlMDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlMSubDivision.DataSource = lstSubDivision;
                            ddlMSubDivision.DataTextField = "Name";
                            ddlMSubDivision.DataValueField = "ID";
                            ddlMSubDivision.DataBind();
                            ddlMSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                            List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                            long SelectedSectionID = lstSection.FirstOrDefault().ID;

                            //ddlMSection.DataSource = lstSection;
                            //ddlMSection.DataTextField = "Name";
                            //ddlMSection.DataValueField = "ID";
                            //ddlMSection.DataBind();
                            //ddlMSection.SelectedValue = SelectedSectionID.ToString();

                            #endregion
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLZones(ddlMZone, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlMZone, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlMZone, false, (int)Constants.DropDownFirstOption.All);
            }
        }

        protected void gvLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvLocation.EditIndex = -1;
                BindData();
                hlBack.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLocation.EditIndex = e.NewEditIndex;
            BindData();
            Master.HideMessageInstantly();
            hlBack.Visible = false;
        }

        protected void gvLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtn = e.Row.FindControl("lbtnAdd") as LinkButton;
                        lbtn.Visible = false;

                        LinkButton lMbtn = e.Row.FindControl("lbMulSlec") as LinkButton;
                        lMbtn.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvLocation.EditIndex == e.Row.RowIndex)
                    {
                        Label LabelID = e.Row.FindControl("lblID") as Label;
                        long RecordID = Convert.ToInt64(LabelID.Text);
                        long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];
                        UA_Users mdlUser = new UserBLL().GetUserByID(UserID);
                        List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);
                        List<long> lstZoneIDs = new List<long>();
                        List<long> lstCircleIDs = new List<long>();
                        List<long> lstDivisionIDs = new List<long>();
                        List<long> lstSubdivIDs = new List<long>();
                        List<long> lstSectionIDs = new List<long>();

                        if (RecordID == -1) // add new scenario
                        {
                            DropDownList ddlZone = e.Row.FindControl("ddlZone") as DropDownList;
                            DropDownList ddCircle = e.Row.FindControl("ddlCircle") as DropDownList;
                            DropDownList ddDivision = e.Row.FindControl("ddlDivision") as DropDownList;
                            DropDownList ddSubDivision = e.Row.FindControl("ddlSubDivision") as DropDownList;
                            DropDownList ddSection = e.Row.FindControl("ddlSection") as DropDownList;
                            //ddCircle.Enabled = false;
                            //ddDivision.Enabled = false;
                            //ddSubDivision.Enabled = false;
                            //ddSection.Enabled = false;

                            ddCircle.CssClass = "form-control";
                            ddDivision.CssClass = "form-control";
                            ddSubDivision.CssClass = "form-control";
                            ddSection.CssClass = "form-control";

                            if (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID != null) //if (mdlUser.RoleID != Constants.AdministratorRoleID)
                            {
                                if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                {
                                    #region Zone Level Binding
                                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                        lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                    long SelectedZoneID = lstZone.FirstOrDefault().ID;
                                    ddlZone.DataSource = lstZone;
                                    ddlZone.DataTextField = "Name";
                                    ddlZone.DataValueField = "ID";
                                    ddlZone.DataBind();
                                    ddlZone.SelectedValue = SelectedZoneID.ToString();

                                    Dropdownlist.DDLCircles(ddCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
                                    #endregion
                                }
                                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                                {
                                    #region Circle Level Bindings

                                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                    {
                                        lstCircleIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                        CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                        lstZoneIDs.Add(mdlCircle.ZoneID);
                                    }

                                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                    long SelectedZoneID = lstZone.FirstOrDefault().ID;
                                    ddlZone.DataSource = lstZone;
                                    ddlZone.DataTextField = "Name";
                                    ddlZone.DataValueField = "ID";
                                    ddlZone.DataBind();
                                    ddlZone.SelectedValue = SelectedZoneID.ToString();

                                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddCircle.DataSource = lstCircle;
                                    ddCircle.DataTextField = "Name";
                                    ddCircle.DataValueField = "ID";
                                    ddCircle.DataBind();
                                    ddCircle.SelectedValue = SelectedCircleID.ToString();

                                    Dropdownlist.DDLDivisions(ddDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                                    #endregion
                                }
                                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                                {
                                    #region Division Level Bindings

                                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                    {
                                        lstDivisionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                        CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                        lstCircleIDs.Add((long)mdlDivision.CircleID);
                                        lstZoneIDs.Add(mdlDivision.CO_Circle.ZoneID);
                                    }

                                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                    long SelectedZoneID = lstZone.FirstOrDefault().ID;
                                    ddlZone.DataSource = lstZone;
                                    ddlZone.DataTextField = "Name";
                                    ddlZone.DataValueField = "ID";
                                    ddlZone.DataBind();
                                    ddlZone.SelectedValue = SelectedZoneID.ToString();

                                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddCircle.DataSource = lstCircle;
                                    ddCircle.DataTextField = "Name";
                                    ddCircle.DataValueField = "ID";
                                    ddCircle.DataBind();
                                    ddCircle.SelectedValue = SelectedCircleID.ToString();

                                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                    ddDivision.DataSource = lstDivision;
                                    ddDivision.DataTextField = "Name";
                                    ddDivision.DataValueField = "ID";
                                    ddDivision.DataBind();
                                    ddDivision.SelectedValue = SelectedDivisionID.ToString();

                                    if (lblDesignation.Text == "ADM" || lblDesignation.Text == "MA")
                                        ddSubDivision.Enabled = false;
                                    else
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.Select);

                                    #endregion
                                }
                                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                                {
                                    #region Sub Division Level Bindings

                                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                    {
                                        lstSubdivIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                        CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                        lstDivisionIDs.Add((long)mdlSubDivision.DivisionID);
                                        lstCircleIDs.Add((long)mdlSubDivision.CO_Division.CircleID);
                                        lstZoneIDs.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                                    }

                                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                    long SelectedZoneID = lstZone.FirstOrDefault().ID;
                                    ddlZone.DataSource = lstZone;
                                    ddlZone.DataTextField = "Name";
                                    ddlZone.DataValueField = "ID";
                                    ddlZone.DataBind();
                                    ddlZone.SelectedValue = SelectedZoneID.ToString();

                                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddCircle.DataSource = lstCircle;
                                    ddCircle.DataTextField = "Name";
                                    ddCircle.DataValueField = "ID";
                                    ddCircle.DataBind();
                                    ddCircle.SelectedValue = SelectedCircleID.ToString();

                                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                    ddDivision.DataSource = lstDivision;
                                    ddDivision.DataTextField = "Name";
                                    ddDivision.DataValueField = "ID";
                                    ddDivision.DataBind();
                                    ddDivision.SelectedValue = SelectedDivisionID.ToString();

                                    List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstSubdivIDs);
                                    long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                                    ddSubDivision.DataSource = lstSubDivision;
                                    ddSubDivision.DataTextField = "Name";
                                    ddSubDivision.DataValueField = "ID";
                                    ddSubDivision.DataBind();
                                    ddSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                    Dropdownlist.DDLSections(ddSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);

                                    #endregion
                                }
                                else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                                {
                                    #region Section Level Bindings

                                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                    {
                                        lstSectionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                        CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                        lstSubdivIDs.Add((long)mdlSection.SubDivID);
                                        lstDivisionIDs.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                        lstCircleIDs.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                        lstZoneIDs.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
                                    }

                                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                    long SelectedZoneID = lstZone.FirstOrDefault().ID;
                                    ddlZone.DataSource = lstZone;
                                    ddlZone.DataTextField = "Name";
                                    ddlZone.DataValueField = "ID";
                                    ddlZone.DataBind();
                                    ddlZone.SelectedValue = SelectedZoneID.ToString();

                                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                    ddCircle.DataSource = lstCircle;
                                    ddCircle.DataTextField = "Name";
                                    ddCircle.DataValueField = "ID";
                                    ddCircle.DataBind();
                                    ddCircle.SelectedValue = SelectedCircleID.ToString();

                                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;
                                    ddDivision.DataSource = lstDivision;
                                    ddDivision.DataTextField = "Name";
                                    ddDivision.DataValueField = "ID";
                                    ddDivision.DataBind();
                                    ddDivision.SelectedValue = SelectedDivisionID.ToString();

                                    List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstSubdivIDs);
                                    long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                                    ddSubDivision.DataSource = lstSubDivision;
                                    ddSubDivision.DataTextField = "Name";
                                    ddSubDivision.DataValueField = "ID";
                                    ddSubDivision.DataBind();
                                    ddSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                    List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstSectionIDs);
                                    long SelectedSectionID = lstSection.FirstOrDefault().ID;
                                    ddSection.DataSource = lstSection;
                                    ddSection.DataTextField = "Name";
                                    ddSection.DataValueField = "ID";
                                    ddSection.DataBind();
                                    ddSection.SelectedValue = SelectedSectionID.ToString();

                                    #endregion
                                }
                            }
                            else
                            {
                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddlZone);
                                ddCircle.Enabled = false;
                                ddDivision.Enabled = false;
                                ddSubDivision.Enabled = false;
                                ddSection.Enabled = false;
                            }

                            ddlZone.Enabled = true;
                        }
                        else // edit scenario
                        {
                            if (ViewState[UserLevel_VS] != null)
                            {
                                LabelID = e.Row.FindControl("lblID") as Label;
                                RecordID = Convert.ToInt64(LabelID.Text);

                                DropDownList ddZone = e.Row.FindControl("ddlZone") as DropDownList;
                                DropDownList ddCircle = e.Row.FindControl("ddlCircle") as DropDownList;
                                DropDownList ddDivison = e.Row.FindControl("ddlDivision") as DropDownList;
                                DropDownList ddSubDivision = e.Row.FindControl("ddlSubDivision") as DropDownList;
                                DropDownList ddSection = e.Row.FindControl("ddlSection") as DropDownList;

                                if (ViewState[UserLevel_VS].ToString() == "Section")
                                {
                                    object RecordDetail =
                                        new UserAdministrationBLL().GetUserDetail(RecordID,
                                            ViewState[UserLevel_VS].ToString());
                                    string ZoneName = RecordDetail.GetType().GetProperty("zoneName")
                                        .GetValue(RecordDetail).ToString();
                                    long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID")
                                        .GetValue(RecordDetail));

                                    string CircleName = RecordDetail.GetType().GetProperty("circleName")
                                        .GetValue(RecordDetail).ToString();
                                    long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID")
                                        .GetValue(RecordDetail));

                                    string DivisionName = RecordDetail.GetType().GetProperty("divisionName")
                                        .GetValue(RecordDetail).ToString();
                                    long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID")
                                        .GetValue(RecordDetail));

                                    string SubDivisionName = RecordDetail.GetType().GetProperty("subDivisionName")
                                        .GetValue(RecordDetail).ToString();
                                    long SubDivisionID = Convert.ToInt64(RecordDetail.GetType()
                                        .GetProperty("subDivisionID").GetValue(RecordDetail));

                                    string SectionName = RecordDetail.GetType().GetProperty("sectionName")
                                        .GetValue(RecordDetail).ToString();
                                    long SectionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("sectioID")
                                        .GetValue(RecordDetail));

                                    if ((mdlUser.UA_Designations == null) || (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID == null)) // if (mdlUser.RoleID == Constants.AdministratorRoleID) 
                                    {
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                        ddZone.Enabled = true;
                                        ddZone.ClearSelection();
                                        ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID,
                                            (int)Constants.DropDownFirstOption.Select);
                                        ddCircle.Enabled = true;
                                        ddCircle.ClearSelection();
                                        ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID,
                                            -1, (int)Constants.DropDownFirstOption.Select);
                                        ddDivison.Enabled = true;
                                        ddDivison.ClearSelection();
                                        ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false,
                                            DivisionID, (int)Constants.DropDownFirstOption.Select);
                                        ddSubDivision.Enabled = true;
                                        ddSubDivision.ClearSelection();
                                        ddSubDivision.Items.FindByValue(SubDivisionID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false,
                                            SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                        ddSection.Enabled = true;
                                        ddSection.ClearSelection();
                                        ddSection.Items.FindByValue(SectionID.ToString()).Selected = true;
                                    }
                                    else
                                    {
                                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                                        {
                                            #region Section
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstSectionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstSubdivIDs.Add((long)mdlSection.SubDivID);
                                                lstDivisionIDs.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                                lstCircleIDs.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                                lstZoneIDs.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            if (ddDivison.Items.FindByValue(SelectedDivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstSubdivIDs);
                                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;
                                            ddSubDivision.DataSource = lstSubDivision;
                                            ddSubDivision.DataTextField = "Name";
                                            ddSubDivision.DataValueField = "ID";
                                            ddSubDivision.DataBind();
                                            if (ddSubDivision.Items.FindByValue(SelectedSubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                            List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstSectionIDs);
                                            long SelectedSectionID = lstSection.FirstOrDefault().ID;
                                            ddSection.DataSource = lstSection;
                                            ddSection.DataTextField = "Name";
                                            ddSection.DataValueField = "ID";
                                            ddSection.DataBind();
                                            if (ddSection.Items.FindByValue(SelectedSectionID.ToString()) != null)
                                                ddSection.SelectedValue = SelectedSectionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                                        {
                                            #region Sub Division Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstSubdivIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstDivisionIDs.Add((long)mdlSubDivision.DivisionID);
                                                lstCircleIDs.Add((long)mdlSubDivision.CO_Division.CircleID);
                                                lstZoneIDs.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstSubdivIDs);
                                            long SelectedSubDivisionID = SubDivisionID;
                                            ddSubDivision.DataSource = lstSubDivision;
                                            ddSubDivision.DataTextField = "Name";
                                            ddSubDivision.DataValueField = "ID";
                                            ddSubDivision.DataBind();
                                            ddSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSection.Items.FindByValue(SectionID.ToString()) != null)
                                                ddSection.SelectedValue = SectionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                                        {
                                            #region Division Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstDivisionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstCircleIDs.Add((long)mdlDivision.CircleID);
                                                lstZoneIDs.Add(mdlDivision.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            if (ddDivison.Items.FindByValue(SelectedDivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByText(SubDivisionName.ToString()) != null)
                                            {
                                                ddSubDivision.ClearSelection();
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();
                                            }

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSection.Items.FindByText(SectionName.ToString()) != null)
                                            {
                                                ddSection.SelectedValue = SectionID.ToString();
                                            }

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                                        {
                                            #region Circle Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstCircleIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstZoneIDs.Add(mdlCircle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByValue(SubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSection.Items.FindByValue(SectionID.ToString()) != null)
                                                ddSection.SelectedValue = SectionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                        {
                                            #region Zone Level Binding
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                                lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddCircle.Items.FindByValue(CircleID.ToString()) != null)
                                                ddCircle.SelectedValue = CircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByValue(SubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSection.Items.FindByValue(SectionID.ToString()) != null)
                                                ddSection.SelectedValue = SectionID.ToString();

                                            #endregion
                                        }
                                    }
                                }
                                else if (ViewState[UserLevel_VS].ToString() == "Sub Division")
                                {
                                    object RecordDetail =
                                        new UserAdministrationBLL().GetUserDetail(RecordID,
                                            ViewState[UserLevel_VS].ToString());
                                    string ZoneName = RecordDetail.GetType().GetProperty("zoneName")
                                        .GetValue(RecordDetail).ToString();
                                    long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID")
                                        .GetValue(RecordDetail));

                                    string CircleName = RecordDetail.GetType().GetProperty("circleName")
                                        .GetValue(RecordDetail).ToString();
                                    long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID")
                                        .GetValue(RecordDetail));

                                    string DivisionName = RecordDetail.GetType().GetProperty("divisionName")
                                        .GetValue(RecordDetail).ToString();
                                    long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID")
                                        .GetValue(RecordDetail));

                                    string SubDivisionName = RecordDetail.GetType().GetProperty("subDivisionName")
                                        .GetValue(RecordDetail).ToString();
                                    long SubDivisionID = Convert.ToInt64(RecordDetail.GetType()
                                        .GetProperty("subDivisionID").GetValue(RecordDetail));

                                    ddSection.CssClass = "form-control";
                                    ddSection.Enabled = false;

                                    if ((mdlUser.UA_Designations == null) || (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID == null)) //if (mdlUser.RoleID == Constants.AdministratorRoleID)
                                    {
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                        ddZone.Enabled = true;
                                        ddZone.ClearSelection();
                                        ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID,
                                            (int)Constants.DropDownFirstOption.Select);
                                        ddCircle.Enabled = true;
                                        ddCircle.ClearSelection();
                                        ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID,
                                            -1, (int)Constants.DropDownFirstOption.Select);
                                        ddDivison.Enabled = true;
                                        ddDivison.ClearSelection();
                                        ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false,
                                            DivisionID, (int)Constants.DropDownFirstOption.Select);
                                        ddSubDivision.Enabled = true;
                                        ddSubDivision.ClearSelection();
                                        ddSubDivision.Items.FindByValue(SubDivisionID.ToString()).Selected = true;
                                        ddSection.Enabled = false;
                                    }
                                    else
                                    {
                                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                                        {
                                            #region Sub Division Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstSubdivIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstDivisionIDs.Add((long)mdlSubDivision.DivisionID);
                                                lstCircleIDs.Add((long)mdlSubDivision.CO_Division.CircleID);
                                                lstZoneIDs.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstSubdivIDs);
                                            long SelectedSubDivisionID = SubDivisionID;
                                            ddSubDivision.DataSource = lstSubDivision;
                                            ddSubDivision.DataTextField = "Name";
                                            ddSubDivision.DataValueField = "ID";
                                            ddSubDivision.DataBind();
                                            ddSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                                        {
                                            #region Division Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstDivisionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstCircleIDs.Add((long)mdlDivision.CircleID);
                                                lstZoneIDs.Add(mdlDivision.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            if (ddDivison.Items.FindByValue(SelectedDivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByValue(SubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                                        {
                                            #region Circle Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstCircleIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstZoneIDs.Add(mdlCircle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByValue(SubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                        {
                                            #region Zone Level Binding
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                                lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddCircle.Items.FindByValue(CircleID.ToString()) != null)
                                                ddCircle.SelectedValue = CircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddSubDivision.Items.FindByValue(SubDivisionID.ToString()) != null)
                                                ddSubDivision.SelectedValue = SubDivisionID.ToString();

                                            #endregion
                                        }
                                    }
                                }
                                else if (ViewState[UserLevel_VS].ToString() == "Division")
                                {
                                    object RecordDetail =
                                        new UserAdministrationBLL().GetUserDetail(RecordID,
                                            ViewState[UserLevel_VS].ToString());
                                    string ZoneName = RecordDetail.GetType().GetProperty("zoneName")
                                        .GetValue(RecordDetail).ToString();
                                    long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID")
                                        .GetValue(RecordDetail));

                                    string CircleName = RecordDetail.GetType().GetProperty("circleName")
                                        .GetValue(RecordDetail).ToString();
                                    long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID")
                                        .GetValue(RecordDetail));

                                    string DivisionName = RecordDetail.GetType().GetProperty("divisionName")
                                        .GetValue(RecordDetail).ToString();
                                    long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID")
                                        .GetValue(RecordDetail));

                                    ddSubDivision.Enabled = false;
                                    ddSection.Enabled = false;

                                    ddSubDivision.CssClass = "form-control";
                                    ddSection.CssClass = "form-control";

                                    if ((mdlUser.UA_Designations == null) || (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID == null)) //if (mdlUser.RoleID == Constants.AdministratorRoleID)
                                    {
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                        ddZone.Enabled = true;
                                        ddZone.ClearSelection();
                                        ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID,
                                            (int)Constants.DropDownFirstOption.Select);
                                        ddCircle.Enabled = true;
                                        ddCircle.ClearSelection();
                                        ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID,
                                            -1, (int)Constants.DropDownFirstOption.Select);
                                        ddDivison.Enabled = true;
                                        ddDivison.ClearSelection();
                                        ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;
                                    }
                                    else
                                    {
                                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                                        {
                                            #region Division Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstDivisionIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstCircleIDs.Add((long)mdlDivision.CircleID);
                                                lstZoneIDs.Add(mdlDivision.CO_Circle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = CircleID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstDivisionIDs);
                                            long SelectedDivisionID = DivisionID;
                                            ddDivison.DataSource = lstDivision;
                                            ddDivison.DataTextField = "Name";
                                            ddDivison.DataValueField = "ID";
                                            ddDivison.DataBind();
                                            if (ddDivison.Items.FindByValue(SelectedDivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = SelectedDivisionID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                                        {
                                            #region Circle Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstCircleIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstZoneIDs.Add(mdlCircle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();
                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                        {
                                            #region Zone Level Binding
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                                lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddCircle.Items.FindByValue(CircleID.ToString()) != null)
                                                ddCircle.SelectedValue = CircleID.ToString();


                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                            if (ddDivison.Items.FindByValue(DivisionID.ToString()) != null)
                                                ddDivison.SelectedValue = DivisionID.ToString();

                                            #endregion
                                        }
                                    }
                                }
                                else if (ViewState[UserLevel_VS].ToString() == "Circle")
                                {
                                    object RecordDetail =
                                        new UserAdministrationBLL().GetUserDetail(RecordID,
                                            ViewState[UserLevel_VS].ToString());
                                    string ZoneName = RecordDetail.GetType().GetProperty("zoneName")
                                        .GetValue(RecordDetail).ToString();
                                    long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID")
                                        .GetValue(RecordDetail));

                                    string CircleName = RecordDetail.GetType().GetProperty("circleName")
                                        .GetValue(RecordDetail).ToString();
                                    long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID")
                                        .GetValue(RecordDetail));

                                    ddDivison.Enabled = false;
                                    ddSubDivision.Enabled = false;
                                    ddSection.Enabled = false;

                                    ddDivison.CssClass = "form-control";
                                    ddSubDivision.CssClass = "form-control";
                                    ddSection.CssClass = "form-control";

                                    if ((mdlUser.UA_Designations == null) || (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID == null)) //if (mdlUser.RoleID == Constants.AdministratorRoleID)
                                    {
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                        ddZone.Enabled = true;
                                        ddZone.ClearSelection();
                                        ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID,
                                            (int)Constants.DropDownFirstOption.Select);
                                        ddCircle.Enabled = true;
                                        ddCircle.ClearSelection();
                                        ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;
                                    }
                                    else
                                    {
                                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                                        {
                                            #region Circle Level Bindings

                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                            {
                                                lstCircleIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                                lstZoneIDs.Add(mdlCircle.ZoneID);
                                            }

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstCircleIDs);
                                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;
                                            ddCircle.DataSource = lstCircle;
                                            ddCircle.DataTextField = "Name";
                                            ddCircle.DataValueField = "ID";
                                            ddCircle.DataBind();
                                            if (ddCircle.Items.FindByValue(SelectedCircleID.ToString()) != null)
                                                ddCircle.SelectedValue = SelectedCircleID.ToString();

                                            #endregion
                                        }
                                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                        {
                                            #region Zone Level Binding
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                                lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();

                                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                            if (ddCircle.Items.FindByValue(CircleID.ToString()) != null)
                                                ddCircle.SelectedValue = CircleID.ToString();
                                            #endregion
                                        }
                                    }

                                }
                                else if (ViewState[UserLevel_VS].ToString() == "Zone")
                                {
                                    object RecordDetail =
                                        new UserAdministrationBLL().GetUserDetail(RecordID,
                                            ViewState[UserLevel_VS].ToString());
                                    string ZoneName = RecordDetail.GetType().GetProperty("zoneName")
                                        .GetValue(RecordDetail).ToString();
                                    long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID")
                                        .GetValue(RecordDetail));

                                    ddCircle.Enabled = false;
                                    ddDivison.Enabled = false;
                                    ddSubDivision.Enabled = false;
                                    ddSection.Enabled = false;

                                    ddCircle.CssClass = "form-control";
                                    ddDivison.CssClass = "form-control";
                                    ddSubDivision.CssClass = "form-control";
                                    ddSection.CssClass = "form-control";

                                    if ((mdlUser.UA_Designations == null) || (mdlUser.UA_Designations != null && mdlUser.UA_Designations.IrrigationLevelID == null)) //if (mdlUser.RoleID == Constants.AdministratorRoleID)
                                    {
                                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                        ddZone.Enabled = true;
                                        ddZone.ClearSelection();
                                        ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;
                                    }
                                    else
                                    {
                                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                                        {
                                            #region Zone Level Binding
                                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                                                lstZoneIDs.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstZoneIDs);
                                            long SelectedZoneID = ZoneID;
                                            ddZone.DataSource = lstZone;
                                            ddZone.DataTextField = "Name";
                                            ddZone.DataValueField = "ID";
                                            ddZone.DataBind();
                                            if (ddZone.Items.FindByValue(SelectedZoneID.ToString()) != null)
                                                ddZone.SelectedValue = SelectedZoneID.ToString();
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                        lbtnEdit.Visible = false;

                        Button lbtnDel = e.Row.FindControl("lbtnDelete") as Button;
                        lbtnDel.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (ViewState[UserLevel_VS] != null)
                {
                    int rowIndex = e.RowIndex;
                    long recordID =
                        Convert.ToInt64(((Label)gvLocation.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                    bool LeafSelect = false;
                    bool saved = false;
                    bool LocationAssigned = false;

                    if (recordID == -1)
                    {
                        if (ViewState[UserLevel_VS].ToString() == "Zone")
                        {
                            string ZoneName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone"))
                                .Text;
                            if (ZoneName != "Select" && ZoneName != "")
                            {
                                long ZoneID =
                                    Convert.ToInt64(
                                        ((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone"))
                                        .Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(
                                    Convert.ToInt64(ViewState[LevelID]), ZoneID, Convert.ToInt64(ViewState[UserID_VS]));
                                if (!LocationAssigned)
                                {
                                    saved = new UserAdministrationBLL().AssignLocation(
                                        Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]),
                                        ZoneID, DesignationID);
                                    if (saved)
                                        gvLocation.PageIndex = 0;
                                }
                            }
                        }
                        else if (ViewState[UserLevel_VS].ToString() == "Circle")
                        {
                            string CircleName =
                                ((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text;
                            if (CircleName != "Select" && CircleName != "")
                            {
                                long CircleID =
                                    Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[2]
                                        .FindControl("ddlCircle")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(
                                    Convert.ToInt64(ViewState[LevelID]), CircleID,
                                    Convert.ToInt64(ViewState[UserID_VS]));
                                if (!LocationAssigned)
                                {
                                    saved = new UserAdministrationBLL().AssignLocation(
                                        Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]),
                                        CircleID, DesignationID);
                                    if (saved)
                                        gvLocation.PageIndex = 0;
                                }
                            }
                        }
                        else if (ViewState[UserLevel_VS].ToString() == "Division")
                        {
                            string DivName =
                                ((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text;
                            if (DivName != "Select" && DivName != "")
                            {
                                long DivisionID =
                                    Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[3]
                                        .FindControl("ddlDivision")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(
                                    Convert.ToInt64(ViewState[LevelID]), DivisionID,
                                    Convert.ToInt64(ViewState[UserID_VS]));
                                if (!LocationAssigned)
                                {
                                    saved = new UserAdministrationBLL().AssignLocation(
                                        Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]),
                                        DivisionID, DesignationID);
                                    if (saved)
                                        gvLocation.PageIndex = 0;
                                }
                            }
                        }
                        else if (ViewState[UserLevel_VS].ToString() == "Sub Division")
                        {
                            string SubDiv =
                                ((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision")).Text;
                            if (SubDiv != "Select" && SubDiv != "")
                            {
                                long SubDivisionID =
                                    Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[4]
                                        .FindControl("ddlSubDivision")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(
                                    Convert.ToInt64(ViewState[LevelID]), SubDivisionID,
                                    Convert.ToInt64(ViewState[UserID_VS]));
                                if (!LocationAssigned)
                                {
                                    saved = new UserAdministrationBLL().AssignLocation(
                                        Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]),
                                        SubDivisionID, DesignationID);
                                    if (saved)
                                        gvLocation.PageIndex = 0;
                                }
                            }
                        }
                        else if (ViewState[UserLevel_VS].ToString() == "Section")
                        {
                            string SecName =
                                ((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text;
                            if (SecName != "Select" && SecName != "")
                            {
                                long SectionID =
                                    Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[5]
                                        .FindControl("ddlSection")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(
                                    Convert.ToInt64(ViewState[LevelID]), SectionID,
                                    Convert.ToInt64(ViewState[UserID_VS]));
                                if (!LocationAssigned)
                                {
                                    saved = new UserAdministrationBLL().AssignLocation(Convert.ToInt64(ViewState[UserID_VS]), Convert.ToInt64(ViewState[LevelID]), SectionID, DesignationID);
                                    if (saved)
                                        gvLocation.PageIndex = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool Result =
                            new UserAdministrationBLL().AssociationExistAgainstLocation(
                                Convert.ToInt64(ViewState[UserID_VS]));
                        if (!Result)
                        {
                            if (ViewState[UserLevel_VS].ToString() == "Zone")
                            {
                                string ZoneName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text;
                                if (ZoneName != "Select" && ZoneName != "")
                                {
                                    long ZoneID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text);
                                    LeafSelect = true;
                                    LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(Convert.ToInt64(ViewState[LevelID]), ZoneID, Convert.ToInt64(ViewState[UserID_VS]), recordID);
                                    if (!LocationAssigned)
                                        saved = new UserAdministrationBLL().UpdateLocation(recordID, ZoneID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                                }
                            }
                            else if (ViewState[UserLevel_VS].ToString() == "Circle")
                            {
                                string CircleName =
                                    ((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text;
                                if (CircleName != "Select" && CircleName != "")
                                {
                                    long CircleID = Convert.ToInt64(
                                        ((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle"))
                                        .Text);
                                    LeafSelect = true;
                                    LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(
                                        Convert.ToInt64(ViewState[LevelID]), CircleID,
                                        Convert.ToInt64(ViewState[UserID_VS]), recordID);
                                    if (!LocationAssigned)
                                        saved = new UserAdministrationBLL().UpdateLocation(recordID, CircleID,
                                            Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                                }
                            }
                            else if (ViewState[UserLevel_VS].ToString() == "Division")
                            {
                                string DivName =
                                    ((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text;
                                if (DivName != "Select" && DivName != "")
                                {
                                    long DivisionID = Convert.ToInt64(
                                        ((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision"))
                                        .Text);
                                    LeafSelect = true;
                                    LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(
                                        Convert.ToInt64(ViewState[LevelID]), DivisionID,
                                        Convert.ToInt64(ViewState[UserID_VS]), recordID);
                                    if (!LocationAssigned)
                                        saved = new UserAdministrationBLL().UpdateLocation(recordID, DivisionID,
                                            Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                                }
                            }
                            else if (ViewState[UserLevel_VS].ToString() == "Sub Division")
                            {
                                string SubDiv =
                                    ((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision"))
                                    .Text;
                                if (SubDiv != "Select" && SubDiv != "")
                                {
                                    long SubDivisionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex]
                                        .Cells[4].FindControl("ddlSubDivision")).Text);
                                    LeafSelect = true;
                                    LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(
                                        Convert.ToInt64(ViewState[LevelID]), SubDivisionID,
                                        Convert.ToInt64(ViewState[UserID_VS]), recordID);
                                    if (!LocationAssigned)
                                        saved = new UserAdministrationBLL().UpdateLocation(recordID, SubDivisionID,
                                            Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                                }
                            }
                            else if (ViewState[UserLevel_VS].ToString() == "Section")
                            {
                                string SecName =
                                    ((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text;
                                if (SecName != "Select" && SecName != "")
                                {
                                    long SectionID = Convert.ToInt64(
                                        ((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection"))
                                        .Text);
                                    LeafSelect = true;
                                    LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(
                                        Convert.ToInt64(ViewState[LevelID]), SectionID,
                                        Convert.ToInt64(ViewState[UserID_VS]), recordID);
                                    if (!LocationAssigned)
                                        saved = new UserAdministrationBLL().UpdateLocation(recordID, SectionID,
                                            Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                                }
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.RecordAssociationsNotEdited.Description,
                                SiteMaster.MessageType.Error);
                            LeafSelect = true;
                        }
                    }

                    if (saved)
                    {
                        gvLocation.EditIndex = -1;
                        BindData();
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                        hlBack.Visible = true;
                    }

                    if (LocationAssigned)
                    {
                        Master.ShowMessage(Message.OneLocationMustBeAssigned.Description, SiteMaster.MessageType.Error);
                    }

                    if (!LeafSelect)
                    {
                        Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[UserLevel_VS] != null)
                {
                    if (ViewState[UserLevel_VS].ToString() == "Zone")
                    {

                    }
                    else
                    {
                        DropDownList ddlZon = (DropDownList)sender;
                        GridViewRow gvRow = (GridViewRow)ddlZon.NamingContainer;
                        DropDownList ddlZone = (DropDownList)gvRow.FindControl("ddlZone");

                        if (ddlZone.SelectedItem.Text != "Select" && ddlZone.SelectedItem.Text != "")
                        {
                            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                            string ZoneName = ddlZone.SelectedItem.Text;

                            DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlCircle.Enabled = true;
                            ddlCircle.CssClass = "form-control required";

                            MultipleSlection.Visible = true;
                            bindDropdownByDesignation(ZoneID);
                            lblListleft.Text = "Circle";
                            lblListright.Text = "Selected Circle";

                            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlDivision.Enabled = false;

                            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSubDivision.Enabled = false;

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                        else
                        {
                            DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlCircle.Enabled = false;

                            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlDivision.Enabled = false;

                            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSubDivision.Enabled = false;

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[UserLevel_VS] != null)
                {
                    if (ViewState[UserLevel_VS].ToString() != "Circle")
                    {
                        DropDownList ddlCir = (DropDownList)sender;
                        GridViewRow gvRow = (GridViewRow)ddlCir.NamingContainer;
                        DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");

                        if (ddlCircle.SelectedItem.Value != "Select" && ddlCircle.SelectedItem.Value != "")
                        {
                            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                            string CircleName = ddlCircle.SelectedItem.Text;

                            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, false, CircleID, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlDivision.Enabled = true;
                            ddlDivision.CssClass = "form-control required";

                            MultipleSlection.Visible = true;
                            bindDropdownByDesignation(CircleID);
                            lblListleft.Text = "Division";
                            lblListright.Text = "Selected Division";


                            if (lblDesignation.Text.Trim() == "MA" || lblDesignation.Text.Trim() == "ADM")
                            {



                            }
                            else
                            {
                                DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1,
                                    (int)Constants.DropDownFirstOption.Select);
                                ddlSubDivision.Enabled = false;

                                DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                    (int)Constants.DropDownFirstOption.Select);
                                ddlSection.Enabled = false;
                            }
                        }
                        else
                        {
                            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlDivision.Enabled = false;

                            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSubDivision.Enabled = false;

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[UserLevel_VS] != null)
                {
                    if (ViewState[UserLevel_VS].ToString() != "Division")
                    {
                        DropDownList ddlDiv = (DropDownList)sender;
                        GridViewRow gvRow = (GridViewRow)ddlDiv.NamingContainer;
                        DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");

                        if (ddlDivision.SelectedItem.Value != "Select" && ddlDivision.SelectedItem.Value != "")
                        {
                            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                            string DivisionName = ddlDivision.SelectedItem.Text;

                            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                            ddlSubDivision.Enabled = true;
                            ddlSubDivision.CssClass = "form-control required";
                            MultipleSlection.Visible = true;
                            bindDropdownByDesignation(DivisionID);
                            lblListleft.Text = "Sub Division";
                            lblListright.Text = "Selected Sub Division";

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                        else
                        {
                            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSubDivision.Enabled = false;

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[UserLevel_VS] != null)
                {
                    if (ViewState[UserLevel_VS].ToString() != "Sub Division")
                    {
                        DropDownList ddlSubDiv = (DropDownList)sender;
                        GridViewRow gvRow = (GridViewRow)ddlSubDiv.NamingContainer;
                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");

                        if (ddlSubDivision.SelectedItem.Value != "Select" && ddlSubDivision.SelectedItem.Value != "")
                        {
                            long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                            string SubDivisionName = ddlSubDivision.SelectedItem.Text;

                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, false, SubDivisionID,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = true;
                            ddlSection.CssClass = "form-control required";
                            MultipleSlection.Visible = true;
                            bindDropdownByDesignation(SubDivisionID);
                            lblListleft.Text = "Section";
                            lblListright.Text = "Selected Section";
                        }
                        else
                        {
                            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1,
                                (int)Constants.DropDownFirstOption.Select);
                            ddlSection.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvLocation.EditIndex = -1;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLocation.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvLocation.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result =
                    new UserAdministrationBLL().AssociationExistAgainstLocation(Convert.ToInt64(ViewState[UserID_VS]));
                if (!Result)
                {
                    new UserAdministrationBLL().DeleteLocation(recordID,
                        Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindData();
                }
                else
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");
                        LinkButton btnMAdd = (LinkButton)e.Row.FindControl("lbMulSlec");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                            btnMAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("lbtnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        public static int test(string ID)
        {
            return 0;
        }


        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string leftSelectedItems = Request.Form[lstLeft.UniqueID];
        //    lstLeft.Items.Clear();
        //    if (!string.IsNullOrEmpty(leftSelectedItems))
        //    {
        //        foreach (string item in leftSelectedItems.Split(','))
        //        {
        //            lstLeft.Items.Add(item);
        //        }
        //    }
        //    string rightSelectedItems = Request.Form[lstRight.UniqueID];
        //    lstRight.Items.Clear();
        //    if (!string.IsNullOrEmpty(rightSelectedItems))
        //    {
        //        foreach (string item in rightSelectedItems.Split(','))
        //        {
        //            lstRight.Items.Add(item);
        //        }
        //    }
        //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Left ListBox Items: " + leftSelectedItems + "\\nRight ListBox Items: " + rightSelectedItems + "');", true);
        //}


        #region Multiple Selection
        private void bindDropdownByDesignation(long _SelectedID)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                //if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                //{

                //}
                //else 
                if (mdlUser.DesignationID == (long)Constants.Designation.SE)
                {
                    List<CO_Circle> lstCircle = new CircleBLL().GetCirclesByZoneID(_SelectedID);
                    lstLeft.DataSource = lstCircle;
                    lstLeft.DataTextField = "Name";
                    lstLeft.DataValueField = "ID";
                    lstLeft.DataBind();
                }
                else if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                {
                    List<CO_Division> lstDivisions = new DivisionBLL().GetDivisionsByCircleIDAndDomainID(_SelectedID);
                    lstLeft.DataSource = lstDivisions;
                    lstLeft.DataTextField = "Name";
                    lstLeft.DataValueField = "ID";
                    lstLeft.DataBind();

                }
                else if (mdlUser.DesignationID == (long)Constants.Designation.SDO)
                {
                    List<CO_SubDivision> lstSubDivisions = new SubDivisionBLL().GetSubDivisionsByDivisionID(_SelectedID);
                    lstLeft.DataSource = lstSubDivisions;
                    lstLeft.DataTextField = "Name";
                    lstLeft.DataValueField = "ID";
                    lstLeft.DataBind();
                }
                else if (mdlUser.DesignationID == (long)Constants.Designation.SBE)
                {
                    List<CO_Section> lstSections = new SectionBLL().GetSectionsBySubDivisionID(_SelectedID);
                    lstLeft.DataSource = lstSections;
                    lstLeft.DataTextField = "Name";
                    lstLeft.DataValueField = "ID";
                    lstLeft.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlMZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (ViewState[UserLevel_VS] != null)
                {
                    //if (ViewState[UserLevel_VS].ToString() == "Zone")
                    //{

                    //}
                    //else
                    //{
                    if (ddlMZone.SelectedItem.Text != "Select" && ddlMZone.SelectedItem.Text != "")
                    {
                        long ZoneID = Convert.ToInt64(ddlMZone.SelectedItem.Value);
                        string ZoneName = ddlMZone.SelectedItem.Text;
                        //if (mdlUser.DesignationID == (long)Constants.Designation.SE)
                        //if ((mdlUser.DesignationID == null && lblDesignation.Text.Trim() == "SE") // admin user
                        //|| (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation))
                        if (lblDesignation.Text.Trim() == "SE")
                        {
                            MultipleSlection.Visible = true;
                            lblListleft.Text = "Circle";
                            lblListright.Text = "Selected Circle";
                            List<CO_Circle> lstCircle = new CircleBLL().GetCirclesByZoneID(ZoneID);
                            lstLeft.DataSource = lstCircle;
                            lstLeft.DataTextField = "Name";
                            lstLeft.DataValueField = "ID";
                            lstLeft.DataBind();
                        }
                        else
                        {
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlMCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                            ddlMCircle.Enabled = true;
                            // ddlMCircle.CssClass = "form-control required";
                        }
                        //  bindDropdownByDesignation(ZoneID);

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlMDivision, true, -1, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMDivision.Enabled = false;

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMSubDivision.Enabled = false;

                        //  PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,(int)Constants.DropDownFirstOption.Select);
                        // ddlMSection.Enabled = false;
                    }
                    else
                    {
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlMCircle, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMCircle.Enabled = false;

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlMDivision, true, -1, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMDivision.Enabled = false;

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMSubDivision.Enabled = false;

                        //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,
                        //    (int)Constants.DropDownFirstOption.Select);
                        //ddlMSection.Enabled = false;
                    }
                    //  }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (ViewState[UserLevel_VS] != null)
                {
                    //if (ViewState[UserLevel_VS].ToString() == "Zone")
                    //{

                    //}
                    //else
                    //{
                    if (ddlMCircle.SelectedItem.Text != "Select" && ddlMCircle.SelectedItem.Text != "")
                    {
                        long CircleID = Convert.ToInt64(ddlMCircle.SelectedItem.Value);
                        string CircleName = ddlMCircle.SelectedItem.Text;
                        //if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                        //if ((mdlUser.DesignationID == null && lblDesignation.Text.Trim() == "XEN" || (lblDesignation.Text.Trim() == "MA") || (lblDesignation.Text.Trim() == "ADM")) // admin user
                        //    || ((mdlUser.DesignationID == (long)Constants.Designation.SE) || (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirector)))
                        if (lblDesignation.Text.Trim() == "XEN" || lblDesignation.Text.Trim() == "MA" || lblDesignation.Text.Trim() == "ADM")
                        {
                            MultipleSlection.Visible = true;
                            lblListleft.Text = "Division";
                            lblListright.Text = "Selected Division";
                            List<CO_Division> lstDivisions = new DivisionBLL().GetDivisionsByCircleIDAndDomainID(CircleID);
                            lstLeft.DataSource = lstDivisions;
                            lstLeft.DataTextField = "Name";
                            lstLeft.DataValueField = "ID";
                            lstLeft.DataBind();
                        }
                        else
                        {

                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlMDivision, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                            ddlMDivision.Enabled = true;
                            //ddlMDivision.CssClass = "form-control required";
                        }
                        //  bindDropdownByDesignation(ZoneID);

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMSubDivision.Enabled = false;

                        //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,
                        //    (int)Constants.DropDownFirstOption.Select);
                        //ddlMSection.Enabled = false;
                    }
                    else
                    {

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlMDivision, true, -1, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMDivision.Enabled = false;

                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMSubDivision.Enabled = false;

                        //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,
                        //    (int)Constants.DropDownFirstOption.Select);
                        //ddlMSection.Enabled = false;
                    }
                    //}
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (ViewState[UserLevel_VS] != null)
                {
                    if (ddlMDivision.SelectedItem.Value != "Select" && ddlMDivision.SelectedItem.Value != "")
                    {
                        long DivisionID = Convert.ToInt64(ddlMDivision.SelectedItem.Value);
                        string DivisionName = ddlMDivision.SelectedItem.Text;

                        //if (mdlUser.DesignationID == (long)Constants.Designation.SDO)
                        //if ((mdlUser.DesignationID == null && lblDesignation.Text.Trim() == "SDO")  // admin user
                        //    || (mdlUser.DesignationID == (long)Constants.Designation.XEN))
                        if (lblDesignation.Text.Trim() == "SDO")
                        {
                            MultipleSlection.Visible = true;
                            lblListleft.Text = "Sub Division";
                            lblListright.Text = "Selected Sub Division";
                            List<CO_SubDivision> lstSubDivisions = new SubDivisionBLL().GetSubDivisionsByDivisionID(DivisionID);
                            lstLeft.DataSource = lstSubDivisions;
                            lstLeft.DataTextField = "Name";
                            lstLeft.DataValueField = "ID";
                            lstLeft.DataBind();
                        }
                        else
                        {
                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                            ddlMSubDivision.Enabled = true;
                        }
                        //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,
                        //    (int)Constants.DropDownFirstOption.Select);
                        //ddlMSection.Enabled = false;
                    }
                    else
                    {
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlMSubDivision, true, -1,
                            (int)Constants.DropDownFirstOption.Select);
                        ddlMSubDivision.Enabled = false;

                        //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1,
                        //    (int)Constants.DropDownFirstOption.Select);
                        //ddlMSection.Enabled = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (ViewState[UserLevel_VS] != null)
                {
                    //if (ViewState[UserLevel_VS].ToString() == "Sub Division") //!= "Sub Division")
                    //{
                    if (ddlMSubDivision.SelectedItem.Value != "Select" && ddlMSubDivision.SelectedItem.Value != "")
                    {
                        long SubDivisionID = Convert.ToInt64(ddlMSubDivision.SelectedItem.Value);
                        string SubDivisionName = ddlMSubDivision.SelectedItem.Text;
                        //if ((mdlUser.DesignationID == null && lblDesignation.Text.Trim() == "SBE") // admin user
                        //    || (mdlUser.DesignationID == (long)Constants.Designation.SDO))
                        if (lblDesignation.Text.Trim() == "SBE" || lblDesignation.Text.Trim() == "Ziladaar" || lblDesignation.Text.Trim() == "Gauge Reader")
                        {
                            MultipleSlection.Visible = true;
                            lblListleft.Text = "Section";
                            lblListright.Text = "Selected Section";
                            List<CO_Section> lstSections = new SectionBLL().GetSectionsBySubDivisionID(SubDivisionID);
                            lstLeft.DataSource = lstSections;
                            lstLeft.DataTextField = "Name";
                            lstLeft.DataValueField = "ID";
                            lstLeft.DataBind();
                        }
                        //else
                        //{
                        //    PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                        //    ddlMSection.Enabled = true;
                        //    // ddlMSection.CssClass = "form-control required";
                        //}
                    }
                    //else
                    //{
                    //    PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlMSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                    //    ddlMSection.Enabled = false;
                    //}
                    //}
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState[UserLevel_VS] != null)
            {
                List<UA_AssociatedLocation> lstAssociatedLocation = new List<UA_AssociatedLocation>();
                UA_AssociatedLocation ObjAssociatedLocation = new UA_AssociatedLocation();
                bool LeafSelect = false;
                bool saved = false;
                bool LocationAssigned = false;
                long recordID = 0;
                //foreach (GridViewRow row in gvLocation.Rows)
                //{
                //    recordID = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                //}
                //if (recordID == -1)
                //{
                string rightSelectedItems = Request.Form[lstRight.UniqueID];
                List<long> lstRightIDs = new List<long>();
                lstRight.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstRight.Items.Add(item);
                        lstRightIDs.Add(Convert.ToInt64(item));
                    }
                }
                if (lstRight.Items.Count > 0)
                {
                    for (int i = 0; i < lstRight.Items.Count; i++)
                    {
                        ObjAssociatedLocation = new UA_AssociatedLocation();
                        ObjAssociatedLocation.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                        ObjAssociatedLocation.IrrigationLevelID = Convert.ToInt64(ViewState[LevelID]);
                        ObjAssociatedLocation.IrrigationBoundryID = Convert.ToInt64(lstRight.Items[i].Value);
                        ObjAssociatedLocation.DesignationID = DesignationID;
                        ObjAssociatedLocation.IsActive = true;
                        ObjAssociatedLocation.ModifiedDate = DateTime.Now;
                        ObjAssociatedLocation.ModifiedBy = Convert.ToInt64(ViewState[UserID_VS]);
                        ObjAssociatedLocation.CreatedDate = DateTime.Now;
                        ObjAssociatedLocation.CreatedBy = Convert.ToInt64(ViewState[UserID_VS]);
                        lstAssociatedLocation.Add(ObjAssociatedLocation);
                    }
                    if (ViewState[UserLevel_VS].ToString() == "Zone")
                    {

                    }
                    else if (ViewState[UserLevel_VS].ToString() == "Circle")
                    {
                        LeafSelect = true;
                        LocationAssigned = new UserAdministrationBLL().CheckMultipleLocationAlreadyAssigned(Convert.ToInt64(ViewState[LevelID]), lstRightIDs, Convert.ToInt64(ViewState[UserID_VS]), recordID);
                        if (!LocationAssigned)
                        {
                            saved = new UserAdministrationBLL().SaveLocation(lstAssociatedLocation);
                        }
                    }
                    else if (ViewState[UserLevel_VS].ToString() == "Division")
                    {
                        LeafSelect = true;
                        LocationAssigned = new UserAdministrationBLL().CheckMultipleLocationAlreadyAssigned(Convert.ToInt64(ViewState[LevelID]), lstRightIDs, Convert.ToInt64(ViewState[UserID_VS]), recordID);
                        if (!LocationAssigned)
                        {
                            saved = new UserAdministrationBLL().SaveLocation(lstAssociatedLocation);
                        }
                    }
                    else if (ViewState[UserLevel_VS].ToString() == "Sub Division")
                    {
                        LeafSelect = true;
                        LocationAssigned = new UserAdministrationBLL().CheckMultipleLocationAlreadyAssigned(Convert.ToInt64(ViewState[LevelID]), lstRightIDs, Convert.ToInt64(ViewState[UserID_VS]), recordID);
                        if (!LocationAssigned)
                        {
                            saved = new UserAdministrationBLL().SaveLocation(lstAssociatedLocation);
                        }
                    }
                    else if (ViewState[UserLevel_VS].ToString() == "Section")
                    {
                        LeafSelect = true;
                        LocationAssigned = new UserAdministrationBLL().CheckMultipleLocationAlreadyAssigned(Convert.ToInt64(ViewState[LevelID]), lstRightIDs, Convert.ToInt64(ViewState[UserID_VS]), recordID);
                        if (!LocationAssigned)
                        {
                            saved = new UserAdministrationBLL().SaveLocation(lstAssociatedLocation);
                        }
                    }
                }
                //}
                if (saved)
                {

                    Session["IsSave"] = "Yes";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal();", true);
                    lstLeft.Items.Clear();
                    lstRight.Items.Clear();
                    gvLocation.EditIndex = -1;
                    BindData();
                    hlBack.Visible = true;
                    Response.Redirect(Request.RawUrl);
                }


                if (LocationAssigned)
                {
                    lstRight.Items.Clear();
                    Master.ShowMessage(Message.OneLocationMustBeAssigned.Description, SiteMaster.MessageType.Error);
                }
                if (!LeafSelect)
                {
                    Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
                }
            }
        }
        //protected void btnClose_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ddlMZone.Items.Clear();//ClearSelection();
        //        ddlMCircle.Items.Clear();//ClearSelection();
        //        ddlMDivision.Items.Clear();//ClearSelection();
        //        ddlMSubDivision.Items.Clear();//ClearSelection();
        //        // ddlMSection.Items.Clear();//ClearSelection();
        //        lstLeft.Items.Clear();
        //        lstRight.Items.Clear();
        //        MultipleSlection.Visible = false;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        #endregion

    }
}
