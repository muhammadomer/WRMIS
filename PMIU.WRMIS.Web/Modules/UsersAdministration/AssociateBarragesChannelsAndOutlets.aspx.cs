using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Web.Common;


namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{

    public partial class AssociateBarragesChannelsAndOutlets : BasePage
    {
        #region ViewState

        public string Rights = "Rights";
        public string UserID_VS = "UserID";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindGrid();
                    if (Session["IsSave"] != null)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //this function binds the initial screen to display
        public void BindGrid()
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
                    UserID = Convert.ToInt64(Request.QueryString["UserID"]);
                    ViewState[UserID_VS] = UserID;
                }

                object UserBasicInfo = new UserAdministrationBLL().UserBasicInformation(UserID);

                if (UserBasicInfo != null)
                {
                    lblDesignation.Text = UserBasicInfo.GetType().GetProperty("Designation").GetValue(UserBasicInfo).ToString();
                    lblUserName.Text = UserBasicInfo.GetType().GetProperty("UserName").GetValue(UserBasicInfo).ToString();
                    lblFullName.Text = UserBasicInfo.GetType().GetProperty("FullName").GetValue(UserBasicInfo).ToString();
                    hBarrages.Visible = false;
                    hChannels.Visible = false;
                    hOutlets.Visible = false;
                }

                List<object> UserInfo = new UserAdministrationBLL().UserInfo(UserID);
                if (UserInfo != null && UserInfo.Count() > 0)
                {
                    string right = UserInfo.FirstOrDefault().GetType().GetProperty("Rights").GetValue(UserInfo.FirstOrDefault()).ToString();
                    string LevelName = UserInfo[0].GetType().GetProperty("IrrLvlName").GetValue(UserInfo[0]).ToString();
                    lblUserName.Text = UserInfo[0].GetType().GetProperty("UserName").GetValue(UserInfo[0]).ToString();
                    lblFullName.Text = UserInfo[0].GetType().GetProperty("FullName").GetValue(UserInfo[0]).ToString();
                    lblDesignation.Text = UserInfo[0].GetType().GetProperty("DesigName").GetValue(UserInfo[0]).ToString();
                    List<string> lstLevel = new List<string>();

                    if (LevelName == "Section")
                    {
                        lstLevel = new UserAdministrationBLL().SectionUser(UserInfo);
                        tdSection.Visible = true;
                        thSection.Visible = true;
                        lblSection.Text = lstLevel[4].ToString();

                        tdSubDivision.Visible = true;
                        thSubDivision.Visible = true;
                        lblSubDivision.Text = lstLevel[3].ToString();

                        tdDivision.Visible = true;
                        thDivision.Visible = true;
                        lblDivision.Text = lstLevel[2].ToString();

                        tdCircle.Visible = true;
                        thCircle.Visible = true;
                        lblCircle.Text = lstLevel[1].ToString();

                        tdZone.Visible = true;
                        thZone.Visible = true;
                        lblZone.Text = lstLevel[0].ToString();
                    }
                    else if (LevelName == "Sub Division")
                    {
                        lstLevel = new UserAdministrationBLL().SubDivisionUser(UserInfo);

                        tdSubDivision.Visible = true;
                        thSubDivision.Visible = true;
                        lblSubDivision.Text = lstLevel[3].ToString();

                        tdDivision.Visible = true;
                        thDivision.Visible = true;
                        lblDivision.Text = lstLevel[2].ToString();

                        tdCircle.Visible = true;
                        thCircle.Visible = true;
                        lblCircle.Text = lstLevel[1].ToString();

                        tdZone.Visible = true;
                        thZone.Visible = true;
                        lblZone.Text = lstLevel[0].ToString();
                    }
                    else if (LevelName == "Division")
                    {
                        lstLevel = new UserAdministrationBLL().DivisionUser(UserInfo);

                        tdDivision.Visible = true;
                        thDivision.Visible = true;
                        lblDivision.Text = lstLevel[2].ToString();

                        tdCircle.Visible = true;
                        thCircle.Visible = true;
                        lblCircle.Text = lstLevel[1].ToString();

                        tdZone.Visible = true;
                        thZone.Visible = true;
                        lblZone.Text = lstLevel[0].ToString();
                    }
                    else if (LevelName == "Circle")
                    {
                        lstLevel = new UserAdministrationBLL().CircleUser(UserInfo);

                        tdCircle.Visible = true;
                        thCircle.Visible = true;
                        lblCircle.Text = lstLevel[1].ToString();

                        tdZone.Visible = true;
                        thZone.Visible = true;
                        lblZone.Text = lstLevel[0].ToString();
                    }
                    else if (LevelName == "Zone")
                    {
                        lstLevel = new UserAdministrationBLL().ZoneUser(UserInfo);

                        tdZone.Visible = true;
                        thZone.Visible = true;
                        lblZone.Text = lstLevel[0].ToString();
                    }

                    gvBarrages.DataSource = new UserAdministrationBLL().GetExistingBarrageAssociation(Convert.ToInt64(ViewState[UserID_VS]));
                    gvBarrages.EditIndex = gvBarrages.EditIndex;
                    gvBarrages.DataBind();

                    gvChannels.DataSource = new UserAdministrationBLL().GetExisitngChannelAssociation((Convert.ToInt64(ViewState[UserID_VS])));
                    gvChannels.EditIndex = gvChannels.EditIndex;
                    gvChannels.DataBind();

                    gvOutlets.DataSource = new UserAdministrationBLL().GetExistingOutletAssociations(Convert.ToInt64(ViewState[UserID_VS]));
                    gvOutlets.EditIndex = gvOutlets.EditIndex;
                    gvOutlets.DataBind();
                }
                else
                {
                    lblNotAllowed.Text = "This user has not been assigned any location. Please assign the location to associate Barrages, Channels, Outlets to this user.";
                    lblNotAllowed.Visible = true;
                    lblNotAllowed.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        # region barrages

        public void BarrageUpdate()
        {
            gvBarrages.DataSource = new UserAdministrationBLL().GetExistingBarrageAssociation(Convert.ToInt64(ViewState[UserID_VS]));
            gvBarrages.EditIndex = gvBarrages.EditIndex;
            gvBarrages.DataBind();
        }

        // this function generates a new row to add record
        protected void gvBarrages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                btnback.Visible = false;
                List<object> lstBarrages = new UserAdministrationBLL().GetExistingBarrageAssociation(Convert.ToInt64(ViewState[UserID_VS]));
                object objBarrage = new
                {
                    ID = -1,
                    Name = "",
                    StationSite = ""
                };

                lstBarrages.Add(objBarrage);
                gvBarrages.PageIndex = gvBarrages.PageCount;
                gvBarrages.DataSource = lstBarrages;
                gvBarrages.DataBind();

                gvBarrages.EditIndex = gvBarrages.Rows.Count - 1;
                gvBarrages.DataBind();
            }
        }

        protected void gvBarrages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvBarrages.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                long barrageID = Convert.ToInt64(((DropDownList)gvBarrages.Rows[rowIndex].Cells[1].FindControl("ddlBarrage")).Text);
                string siteName = ((DropDownList)gvBarrages.Rows[rowIndex].Cells[2].FindControl("ddlSites")).Text.ToString();

                if (barrageID != -1)
                {
                    if (siteName != "-1")
                    {
                        if (siteName == "1")   // 1 shows downstream is selected
                            siteName = "D";
                        else
                            siteName = "U";

                        bool saveResult = false;

                        if (recordID == -1)
                        {
                            UA_AssociatedStations objSave = new UA_AssociatedStations();
                            objSave.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                            objSave.StractureTypeID = 1;
                            objSave.StationID = barrageID;
                            objSave.StationSite = siteName;
                            saveResult = new UserAdministrationBLL().SaveData(objSave);
                            if (saveResult)
                                gvBarrages.PageIndex = 0;
                        }
                        else
                        {
                            UA_AssociatedStations objUpdate = new UA_AssociatedStations();
                            objUpdate.ID = recordID;
                            objUpdate.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                            objUpdate.StractureTypeID = 1;
                            objUpdate.StationID = barrageID;
                            objUpdate.StationSite = siteName;
                            new UserAdministrationBLL().UpdateData(objUpdate);
                            saveResult = true;
                        }

                        if (saveResult)
                        {
                            Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                            gvBarrages.EditIndex = -1;
                            BarrageUpdate();

                            if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                            {
                                btnback.Visible = true;
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        }
                    }
                    else
                    {
                        // Master.ShowMessage(Message.SelectSite.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    //  Master.ShowMessage(Message.SelectBarrage.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvBarrages.EditIndex = -1;
                BarrageUpdate();
                if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                {
                    btnback.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtn = e.Row.FindControl("lbtnAdd") as LinkButton;
                        lbtn.Visible = false;

                        LinkButton lMbtn = e.Row.FindControl("lbMul") as LinkButton;
                        lMbtn.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvBarrages.EditIndex == e.Row.RowIndex)
                    {
                        Label lblID = e.Row.FindControl("lblID") as Label;
                        long RecordID = Convert.ToInt64(lblID.Text);

                        if (RecordID == -1)  // add new case 
                        {
                            DropDownList ddlBarrage = e.Row.FindControl("ddlBarrage") as DropDownList;
                            List<CO_Station> lstBarrages = new UserAdministrationBLL().GetAllBarrages();
                            CO_Station objSelect = new CO_Station();
                            objSelect.ID = -1;
                            objSelect.Name = "Select";
                            lstBarrages.Insert(0, objSelect);

                            ddlBarrage.DataSource = lstBarrages;
                            ddlBarrage.DataTextField = "Name";
                            ddlBarrage.DataValueField = "ID";
                            ddlBarrage.DataBind();

                            DropDownList ddlSites = e.Row.FindControl("ddlSites") as DropDownList;
                            ddlSites.Items.Insert(0, new ListItem("Select", "-1"));
                            ddlSites.Items.Insert(1, new ListItem(Constants.UpStream, "0"));
                            ddlSites.Items.Insert(2, new ListItem(Constants.DownStream, "1"));
                        }
                        else
                        {
                            object record = new UserAdministrationBLL().GetBarrageAssociationsRecord(RecordID);
                            string Name = record.GetType().GetProperty("Name").GetValue(record).ToString();
                            string Site = record.GetType().GetProperty("StationSite").GetValue(record).ToString();

                            DropDownList ddlBarrage = e.Row.FindControl("ddlBarrage") as DropDownList;
                            ddlBarrage.DataSource = new UserAdministrationBLL().GetAllBarrages();
                            ddlBarrage.DataTextField = "Name";
                            ddlBarrage.DataValueField = "ID";
                            ddlBarrage.DataBind();

                            ddlBarrage.ClearSelection();
                            ddlBarrage.Items.FindByText(Name).Selected = true;

                            DropDownList ddlSites = e.Row.FindControl("ddlSites") as DropDownList;
                            ddlSites.Items.Insert(0, new ListItem("Select", "-1"));
                            ddlSites.Items.Insert(1, new ListItem(Constants.UpStream, "0"));
                            ddlSites.Items.Insert(2, new ListItem(Constants.DownStream, "1"));

                            ddlSites.ClearSelection();
                            ddlSites.Items.FindByText(Site).Selected = true;
                        }
                    }

                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                        lbtnEdit.Visible = false;

                        LinkButton lbtnDel = e.Row.FindControl("lbtnDelete") as LinkButton;
                        lbtnDel.Visible = false;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int recordID = Convert.ToInt32(((Label)gvBarrages.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
            new UserAdministrationBLL().DeleteData(recordID);
            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
            BarrageUpdate();
        }

        protected void gvBarrages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBarrages.EditIndex = e.NewEditIndex;
            BarrageUpdate();
            btnback.Visible = false;
        }

        protected void gvBarrages_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvBarrages.EditIndex = -1;
                BarrageUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBarrages.PageIndex = e.NewPageIndex;
                BarrageUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Channel

        public void ChannelUpdate()
        {
            gvChannels.DataSource = new UserAdministrationBLL().GetExisitngChannelAssociation((Convert.ToInt64(ViewState[UserID_VS])));
            gvChannels.EditIndex = gvChannels.EditIndex;
            gvChannels.DataBind();
        }

        protected void gvChannels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    btnback.Visible = false;
                    List<object> lstChannels = new UserAdministrationBLL().GetExisitngChannelAssociation(Convert.ToInt64(ViewState[UserID_VS]));
                    object objChannel = new
                    {
                        ID = -1,
                        Name = "",
                        GuageRD = ""
                    };
                    lstChannels.Add(objChannel);
                    gvChannels.DataSource = lstChannels;
                    gvChannels.PageIndex = gvChannels.PageCount;
                    gvChannels.DataBind();

                    gvChannels.EditIndex = gvChannels.Rows.Count - 1;
                    gvChannels.DataBind();
                }
                if (e.CommandName == "MultiAdd")
                {

                    List<object> lstObject = new List<object>();
                    lstObject = new UserAdministrationBLL().GetUserCahnnels(Convert.ToInt64(ViewState[UserID_VS]));
                    ddlChannelCopy.DataSource = lstObject;
                    ddlChannelCopy.DataTextField = "Name";
                    ddlChannelCopy.DataValueField = "ID";
                    ddlChannelCopy.DataBind();
                    ddlChannelCopy.CssClass = "form-control required";
                    ddlChannelCopy.Attributes.Add("required", "true");
                    lstSelectedChannelRDs.Attributes.Add("required", "required");
                    lstChannelRDs.Items.Clear();
                    lstSelectedChannelRDs.Items.Clear();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script123", "$('#AddMultipalChannelRDs').modal();", true);
                }
                //MultiAdd
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvChannels.EditIndex = -1;
                ChannelUpdate();

                if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                {
                    btnback.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnAdd = e.Row.FindControl("lbtnAdd") as LinkButton;
                        lbtnAdd.Visible = false;

                        LinkButton lMbtnAdd = e.Row.FindControl("lbMul") as LinkButton;
                        lMbtnAdd.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvChannels.EditIndex == e.Row.RowIndex)
                    {
                        Label lblID = e.Row.FindControl("lblChnlID") as Label;
                        long RecordID = Convert.ToInt64(lblID.Text);

                        if (RecordID == -1)
                        {
                            DropDownList ddlBarrage = e.Row.FindControl("ddlChannel") as DropDownList;
                            ddlBarrage.DataSource = new UserAdministrationBLL().GetUserCahnnels(Convert.ToInt64(ViewState[UserID_VS]));
                            ddlBarrage.DataTextField = "Name";
                            ddlBarrage.DataValueField = "ID";
                            ddlBarrage.DataBind();
                        }
                        else
                        {
                            object record = new UserAdministrationBLL().GetChannelAssociationsRecord(RecordID);
                            string ChannelName = record.GetType().GetProperty("Name").GetValue(record).ToString();
                            long ChannelID = Convert.ToInt64(record.GetType().GetProperty("ChnlID").GetValue(record));
                            string RD = record.GetType().GetProperty("GuageRD").GetValue(record).ToString();

                            DropDownList ddlBarrage = e.Row.FindControl("ddlChannel") as DropDownList;
                            ddlBarrage.DataSource = new UserAdministrationBLL().GetUserCahnnels(Convert.ToInt64(ViewState[UserID_VS]));
                            ddlBarrage.DataTextField = "Name";
                            ddlBarrage.DataValueField = "ID";
                            ddlBarrage.DataBind();
                            ddlBarrage.ClearSelection();
                            ddlBarrage.Items.FindByText(ChannelName).Selected = true;

                            DropDownList ddlRD = e.Row.FindControl("ddlRD") as DropDownList;
                            ddlRD.DataSource = new UserAdministrationBLL().GetChannelRDs(ChannelID);
                            ddlRD.DataTextField = "Name";
                            ddlRD.DataValueField = "ID";
                            ddlRD.DataBind();
                            ddlRD.ClearSelection();
                            ddlRD.Items.FindByText(RD).Selected = true;
                        }
                    }

                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                        lbtnEdit.Visible = false;

                        LinkButton lbtnDel = e.Row.FindControl("lbtnDelete") as LinkButton;
                        lbtnDel.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int recordID = Convert.ToInt32(((Label)gvChannels.Rows[rowIndex].Cells[0].FindControl("lblChnlID")).Text);
                new UserAdministrationBLL().DeleteData(recordID);
                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                ChannelUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvChannels.EditIndex = e.NewEditIndex;
                ChannelUpdate();
                btnback.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlChannel = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlChannel.NamingContainer;
                DropDownList ddlRD = (DropDownList)gvRow.FindControl("ddlRD");
                long ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                string ChannelName = ddlChannel.SelectedItem.Text;
                ddlRD.DataSource = new UserAdministrationBLL().GetChannelRDs(ChannelID);
                ddlRD.DataTextField = "Name";
                ddlRD.DataValueField = "ID";
                ddlRD.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvChannels.Rows[rowIndex].Cells[0].FindControl("lblChnlID")).Text);

                string ChnlName = ((DropDownList)gvChannels.Rows[rowIndex].Cells[1].FindControl("ddlChannel")).Text.ToString();
                if (ChnlName != "")
                {
                    long ChannelID = Convert.ToInt64(((DropDownList)gvChannels.Rows[rowIndex].Cells[1].FindControl("ddlChannel")).Text);
                    string RD = ((DropDownList)gvChannels.Rows[rowIndex].Cells[2].FindControl("ddlRD")).Text;
                    if (RD != "")
                    {
                        long RDID = Convert.ToInt64(((DropDownList)gvChannels.Rows[rowIndex].Cells[2].FindControl("ddlRD")).Text);
                        bool saveResult = false;

                        if (recordID == -1)
                        {
                            UA_AssociatedStations objSave = new UA_AssociatedStations();
                            objSave.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                            objSave.StractureTypeID = 6;
                            objSave.StationID = ChannelID;
                            objSave.StationSite = "G";
                            objSave.GaugeOutlet = RDID;
                            saveResult = new UserAdministrationBLL().SaveData(objSave);
                            if (saveResult)
                                gvChannels.PageIndex = 0;
                        }
                        else
                        {
                            UA_AssociatedStations objUpdate = new UA_AssociatedStations();
                            objUpdate.ID = recordID;
                            objUpdate.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                            objUpdate.StractureTypeID = 6;
                            objUpdate.StationID = ChannelID;
                            objUpdate.StationSite = "G";
                            objUpdate.GaugeOutlet = RDID;
                            saveResult = new UserAdministrationBLL().UpdateData(objUpdate);
                        }

                        if (saveResult)
                        {
                            Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                            gvChannels.EditIndex = -1;

                            if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                            {
                                btnback.Visible = true;
                            }
                            ChannelUpdate();
                        }
                        else
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        }
                    }
                    else
                    {
                        //Master.ShowMessage(Message.FieldRequired.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    //Master.ShowMessage(Message.FieldRequired.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvChannels.EditIndex = -1;
                ChannelUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannels.PageIndex = e.NewPageIndex;
                ChannelUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Outlets

        public void OutletUpdate()
        {
            gvOutlets.DataSource = new UserAdministrationBLL().GetExistingOutletAssociations(Convert.ToInt64(ViewState[UserID_VS]));
            gvOutlets.EditIndex = gvOutlets.EditIndex;
            gvOutlets.DataBind();
        }

        protected void gvOutlets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    btnback.Visible = false;
                    List<object> lstOutlets = new UserAdministrationBLL().GetExistingOutletAssociations(Convert.ToInt64(ViewState[UserID_VS]));
                    object objChannel = new
                    {
                        ID = -1,
                        Name = "",
                        Outlet = ""
                    };
                    lstOutlets.Add(objChannel);
                    gvOutlets.DataSource = lstOutlets;
                    gvOutlets.PageIndex = gvOutlets.PageCount;
                    gvOutlets.DataBind();

                    gvOutlets.EditIndex = gvOutlets.Rows.Count - 1;
                    gvOutlets.DataBind();
                }
                if (e.CommandName == "MultiAdd")
                {
                    List<object> lstObject = new List<object>();
                    lstObject = new UserAdministrationBLL().GetUserCahnnels(Convert.ToInt64(ViewState[UserID_VS]));
                    ddlOutletChannelCopy.DataSource = lstObject;
                    ddlOutletChannelCopy.DataTextField = "Name";
                    ddlOutletChannelCopy.DataValueField = "ID";
                    ddlOutletChannelCopy.DataBind();

                    ddlOutletChannelCopy.CssClass = "form-control required";
                    ddlOutletChannelCopy.Attributes.Add("required", "true");
                    lstSelectedOutlet.Attributes.Add("required", "required");

                    lstOutLets.Items.Clear();
                    lstSelectedOutlet.Items.Clear();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#AddMultipaleOutLetPopUp').modal();", true);
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOutlets.EditIndex = -1;
                OutletUpdate();

                if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                {
                    btnback.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int recordID = Convert.ToInt32(((Label)gvOutlets.Rows[rowIndex].Cells[0].FindControl("lblOutletlID")).Text);
                new UserAdministrationBLL().DeleteData(recordID);
                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                OutletUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOutlets.EditIndex = e.NewEditIndex;
                OutletUpdate();
                btnback.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnAdd = e.Row.FindControl("lbtnAdd") as LinkButton;
                        lbtnAdd.Visible = false;

                        LinkButton lMbtnAdd = e.Row.FindControl("lbMulSlec") as LinkButton;
                        lMbtnAdd.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvOutlets.EditIndex == e.Row.RowIndex)
                    {
                        Label lblRecord = e.Row.FindControl("lblOutletlID") as Label;
                        long RecordID = Convert.ToInt64(lblRecord.Text);

                        DropDownList ddlBarrage = e.Row.FindControl("ddlOutletChannel") as DropDownList;

                        List<object> lstObject = new List<object>();
                        lstObject = new UserAdministrationBLL().GetUserCahnnels(Convert.ToInt64(ViewState[UserID_VS]));
                        ddlBarrage.DataSource = lstObject;
                        ddlBarrage.DataTextField = "Name";
                        ddlBarrage.DataValueField = "ID";
                        ddlBarrage.DataBind();




                        if (RecordID != -1)
                        {
                            object SelectedOutlet = new UserAdministrationBLL().GetOutletAssociationsRecord(RecordID);
                            string Outlet = SelectedOutlet.GetType().GetProperty("Outlet").GetValue(SelectedOutlet).ToString();
                            string ChnlName = SelectedOutlet.GetType().GetProperty("Name").GetValue(SelectedOutlet).ToString();
                            long ChnlID = Convert.ToInt64(SelectedOutlet.GetType().GetProperty("ChnlID").GetValue(SelectedOutlet));

                            ddlBarrage.ClearSelection();
                            ddlBarrage.Items.FindByText(ChnlName).Selected = true;

                            DropDownList ddOutlet = e.Row.FindControl("ddlOutlet") as DropDownList;
                            ddOutlet.DataSource = new UserAdministrationBLL().GetChannelOutlets(ChnlID);
                            ddOutlet.DataTextField = "Name";
                            ddOutlet.DataValueField = "ID";
                            ddOutlet.DataBind();
                            ddOutlet.ClearSelection();
                            ddOutlet.Items.FindByText(Outlet).Selected = true;
                        }
                    }

                    if (ViewState[Rights] != null && ViewState[Rights].ToString() == "False")
                    {
                        LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                        lbtnEdit.Visible = false;
                        LinkButton lbtnDel = e.Row.FindControl("lbtnDelete") as LinkButton;
                        lbtnDel.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvOutlets.Rows[rowIndex].Cells[0].FindControl("lblOutletlID")).Text);
                long ChannelID = Convert.ToInt64(((DropDownList)gvOutlets.Rows[rowIndex].Cells[1].FindControl("ddlOutletChannel")).Text);
                string ROutlet = ((DropDownList)gvOutlets.Rows[rowIndex].Cells[2].FindControl("ddlOutlet")).Text;

                if (ROutlet != "")
                {
                    long OutletID = Convert.ToInt64(((DropDownList)gvOutlets.Rows[rowIndex].Cells[2].FindControl("ddlOutlet")).Text);
                    bool saveResult = false;

                    if (recordID == -1)
                    {
                        UA_AssociatedStations objSave = new UA_AssociatedStations();
                        objSave.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                        objSave.StractureTypeID = 6;
                        objSave.StationID = ChannelID;
                        objSave.StationSite = "O";
                        objSave.GaugeOutlet = OutletID;
                        saveResult = new UserAdministrationBLL().SaveData(objSave);
                        if (saveResult)
                            gvOutlets.PageIndex = 0;
                    }
                    else
                    {
                        UA_AssociatedStations objUpdate = new UA_AssociatedStations();
                        objUpdate.ID = recordID;
                        objUpdate.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                        objUpdate.StractureTypeID = 6;
                        objUpdate.StationID = ChannelID;
                        objUpdate.StationSite = "O";
                        objUpdate.GaugeOutlet = OutletID;
                        saveResult = new UserAdministrationBLL().UpdateData(objUpdate);
                    }

                    if (saveResult)
                    {
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                        gvOutlets.EditIndex = -1;
                        if (gvBarrages.EditIndex == -1 && gvOutlets.EditIndex == -1 && gvChannels.EditIndex == -1)
                        {
                            btnback.Visible = true;
                        }
                        OutletUpdate();
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    //Master.ShowMessage(Message.SelectOutlet.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlOutletChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlOutletChannel = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlOutletChannel.NamingContainer;
                DropDownList ddlOutlet = (DropDownList)gvRow.FindControl("ddlOutlet");
                long ChannelID = Convert.ToInt64(ddlOutletChannel.SelectedItem.Value);
                string ChannelName = ddlOutletChannel.SelectedItem.Text;
                ddlOutlet.DataSource = new UserAdministrationBLL().GetChannelOutlets(ChannelID);
                ddlOutlet.DataTextField = "Name";
                ddlOutlet.DataValueField = "ID";
                ddlOutlet.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOutlets.EditIndex = -1;
                OutletUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutlets.PageIndex = e.NewPageIndex;
                OutletUpdate();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchUser.aspx?ShowHistory=true");
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssociateBarrageChannelOutlets);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvBarrages_RowCreated(object sender, GridViewRowEventArgs e)
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
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

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
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

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
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutlets_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

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
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlOutletChannelCopy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlOutletChannelCopy = (DropDownList)sender;
                if (ddlOutletChannelCopy.SelectedItem.Value != "")
                {
                    long ChannelID = Convert.ToInt64(ddlOutletChannelCopy.SelectedItem.Value);
                    string ChannelName = ddlOutletChannelCopy.SelectedItem.Text;
                    lstOutLets.DataSource = new UserAdministrationBLL().GetChannelOutletsWhichAlreadyNotSaved(ChannelID, Convert.ToInt64(ViewState[UserID_VS]));
                    lstOutLets.DataTextField = "Name";
                    lstOutLets.DataValueField = "ID";
                    lstOutLets.DataBind();
                }
                else
                {
                    lstOutLets.Items.Clear();
                    lstSelectedOutlet.Items.Clear();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSaveOutlet_Click(object sender, EventArgs e)
        {

            string rightSelectedItems = Request.Form[lstSelectedOutlet.UniqueID];
            List<long> lstSelectedOutLets = new List<long>();
            lstSelectedOutlet.Items.Clear();
            if (!string.IsNullOrEmpty(rightSelectedItems))
            {
                foreach (string item in rightSelectedItems.Split(','))
                {
                    lstSelectedOutLets.Add(Convert.ToInt64(item));
                }
            }
            long ChannelID = Convert.ToInt64(ddlOutletChannelCopy.SelectedValue);

            bool saveResult = false;
            List<UA_AssociatedStations> lstAssStations = new List<UA_AssociatedStations>();
            foreach (long OutletID in lstSelectedOutLets)
            {
                UA_AssociatedStations objSave = new UA_AssociatedStations();
                objSave.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                objSave.StractureTypeID = 6;
                objSave.StationID = ChannelID;
                objSave.StationSite = "O";
                objSave.GaugeOutlet = OutletID;
                saveResult = new UserAdministrationBLL().SaveData(objSave);
            }
            if (saveResult)
            {
                 Session["IsSave"] = "Yes";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal();", true);
            lstOutLets.ClearSelection();
            lstSelectedOutlet.ClearSelection();
            Response.Redirect(Request.RawUrl);
        }

        protected void ddlChannelCopy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlOutletChannelCopy = (DropDownList)sender;
                if (ddlOutletChannelCopy.SelectedItem.Value != "")
                {
                    long ChannelID = Convert.ToInt64(ddlOutletChannelCopy.SelectedItem.Value);
                    string ChannelName = ddlOutletChannelCopy.SelectedItem.Text;
                    lstChannelRDs.DataSource = new UserAdministrationBLL().GetChannelRDsWhichAlreadyNotSaved(ChannelID, Convert.ToInt64(ViewState[UserID_VS]));
                    lstChannelRDs.DataTextField = "Name";
                    lstChannelRDs.DataValueField = "ID";
                    lstChannelRDs.DataBind();
                }
                else
                {
                    lstChannelRDs.Items.Clear();
                    lstSelectedChannelRDs.Items.Clear();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSaveChannelRDs_Click(object sender, EventArgs e)
        {
            string rightSelectedItems = Request.Form[lstSelectedChannelRDs.UniqueID];
            List<long> lstSelectedRDs = new List<long>();
            lstSelectedChannelRDs.Items.Clear();
            if (!string.IsNullOrEmpty(rightSelectedItems))
            {
                foreach (string item in rightSelectedItems.Split(','))
                {
                    lstSelectedRDs.Add(Convert.ToInt64(item));
                }
            }
            long ChannelID = Convert.ToInt64(ddlChannelCopy.SelectedValue);

            bool saveResult = false;
            List<UA_AssociatedStations> lstAssStations = new List<UA_AssociatedStations>();
            foreach (long RDid in lstSelectedRDs)
            {
                UA_AssociatedStations objSave = new UA_AssociatedStations();
                objSave.UserID = Convert.ToInt64(ViewState[UserID_VS]);
                objSave.StractureTypeID = 6;
                objSave.StationID = ChannelID;
                objSave.StationSite = "G";
                objSave.GaugeOutlet = RDid;
                saveResult = new UserAdministrationBLL().SaveData(objSave);
            }
            if (saveResult)
            {
                Session["IsSave"] = "Yes";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModalRDs();", true);
            lstOutLets.ClearSelection();
            lstSelectedOutlet.ClearSelection();
            Response.Redirect(Request.RawUrl);
        }
    }
}