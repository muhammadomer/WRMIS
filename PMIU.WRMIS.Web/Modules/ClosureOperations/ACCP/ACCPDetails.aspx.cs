using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP
{
    public partial class ACCPDetails : BasePage
    {



        #region Intializaion of Global variables
        Dictionary<string, object> dd_ACCP = new Dictionary<string, object>();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        List<object> lstOfObj = new List<object>();
        UA_RoleRights mdlRoleRights;
        #endregion
        #region pageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                mdlRoleRights = Master.GetPageRoleRights();

                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ACCPID"]))
                    {
                        LoadACCPDetails(Convert.ToInt64(Request.QueryString["ACCPID"]));
                        ACCPID.ACCPID = Convert.ToInt64(Request.QueryString["ACCPID"]);
                    }
                    DisplayAccordian(3);

                }
                hlBack.NavigateUrl = string.Format("~/Modules/ClosureOperations/ACCP/AnnualCanalClosureProgram.aspx?CFCH=Detail");
                hfAccpID.Value = Request.QueryString["ACCPID"];
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DetailsAnnualCanalClosureProgram);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        protected void bindgvMC(List<object> lstOfMC)
        {

            gvMC.DataSource = lstOfMC;
            gvMC.DataBind();
        }
        protected void bindgvTC(List<object> lstOfTC)
        {

            gvTC.DataSource = lstOfTC;
            gvTC.DataBind();
        }
        private void LoadACCPDetails(long _ACCPID)
        {
            List<object> lstOfTCMC = new List<object>();
            lstOfTCMC = bllCO.GetTCandMCByACCPID(_ACCPID);
            Session["ListOfTCandMCObjs"] = lstOfTCMC;
            if (lstOfTCMC != null && lstOfTCMC.Count > 0)
            {
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOfTCMC));
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOfTCMC));
            }
            else
            {
                bindgvMC(lstOfTCMC);
                bindgvTC(lstOfTCMC);
            }
        }

        private void DisplayAccordian(int AccordianIndex)
        {
            List<object> lstOfTCMC = new List<object>();
            lstOfTCMC = Session["ListOfTCandMCObjs"] as List<object>;
            if (AccordianIndex == 0)
            {
                iconManglaCommand.Attributes["class"] = "fa fa-chevron-up";
                divMC.Attributes["style"] = "display: Block;";
                divMC.Attributes["disabled"] = "display: Block;";


                iconTerbelaCommand.Attributes["class"] = "fa fa-chevron-down";
                divTBC.Attributes["style"] = "display: none;";
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOfTCMC));
            }
            else if (AccordianIndex == 1)
            {
                iconTerbelaCommand.Attributes["class"] = "fa fa-chevron-up";
                divTBC.Attributes["style"] = "display: Block;";

                iconManglaCommand.Attributes["class"] = "fa fa-chevron-down";
                divMC.Attributes["style"] = "display: none;";
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOfTCMC));
            }
            else
            {
                iconManglaCommand.Attributes["class"] = "fa fa-chevron-up";
                divMC.Attributes["style"] = "display: Block;";
                iconTerbelaCommand.Attributes["class"] = "fa fa-chevron-up";
                divTBC.Attributes["style"] = "display: Block;";
            }

        }
        private List<object> FilterListOfDetialsByCommandType(int commandType, List<object> lstOfObject)
        {
            List<object> lstObject = new List<object>();
            lstObject = (from obj in lstOfObject where Convert.ToInt32(obj.GetType().GetProperty("ChannelCmdType").GetValue(obj)) == commandType select obj).ToList();
            return lstObject;

        }

        private void BindDdl(DropDownList ddl, List<object> lstOfChannels)
        {
            ddl.DataSource = lstOfChannels;
            ddl.DataTextField = "ChannelName";
            ddl.DataValueField = "ChannelID";
            ddl.DataBind();
        }
        #endregion
        #region Events gv TC
        protected void gvTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTC.PageIndex = e.NewPageIndex;
                gvTC.EditIndex = -1;
                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOTCandMC));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTC.EditIndex = -1;
                DisplayAccordian(1);
                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOTCandMC));
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOTCandMC));


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {



                if (e.CommandName == "AddACCPDetail")
                {
                    List<object> lstAccpDetial = Session["ListOfTCandMCObjs"] as List<object>;
                    List<object> lstData = (from item in lstAccpDetial where Convert.ToInt64(item.GetType().GetProperty("ChannelCmdType").GetValue(item)) == Convert.ToInt64(Constants.Commands.IndusCommand) select item).ToList();
                    List<object> lstTCChnls = new List<object>();
                    lstTCChnls.Add(
                    new
                    {
                        ID = 0,
                        FromDate = Utility.GetFormattedDate( DateTime.Now),
                        ToDate = string.Empty,
                        MainCanalName = string.Empty,
                        ChannelCmdType = Convert.ToInt64(Constants.Commands.IndusCommand),
                        ACCPID = hfAccpID.Value,
                        MainCanalID = string.Empty
                    });

                    foreach (var item in lstData)
                        lstTCChnls.Add(item);

                    gvTC.PageIndex = gvTC.PageCount;
                    gvTC.DataSource = lstTCChnls;
                    gvTC.DataBind();
                    gvTC.EditIndex = 0;// gvTC.Rows.Count - 1;
                    gvTC.DataBind();
                    DisplayAccordian(1);

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvTC.EditIndex == e.Row.RowIndex)
                    {

                        DropDownList ddlTCChannel = (DropDownList)e.Row.FindControl("ddlTCChannel");

                        List<object> lstChannels = bllCO.GetChannelsByChannelTypeandChannelCommandType(Convert.ToInt32(Constants.ChannelType.MainCanal), Convert.ToInt32(Constants.Commands.IndusCommand));
                        lstChannels.Sort((obj1, obj2) => Convert.ToString(obj1.GetType().GetProperty("ChannelName").GetValue(obj1)).CompareTo(Convert.ToString(obj2.GetType().GetProperty("ChannelName").GetValue(obj2))));
                        BindDdl(ddlTCChannel, lstChannels);
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvTC.DataKeys[e.RowIndex].Values[0]);
                long hfACCPID = Convert.ToInt64(hfAccpID.Value);
                dd_ACCP.Clear();
                dd_ACCP.Add("ID", ID);
                List<object> isDependencyExist = bllCO.GetExcludedChannels(Convert.ToInt64(ID));
                if (isDependencyExist != null && isDependencyExist.Count > 0)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool isDeleted = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_DELETE, dd_ACCP);
                if (isDeleted)
                {
                    LoadACCPDetails(hfACCPID);
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
                DisplayAccordian(1);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label labID = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblTCID");
                Label lblCommandType = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblCommandType");
                Label lblMainCanalID = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblMainCanalID");
                Label lblTCFromDate = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblTCFromDate");
                Label lblTCToDate = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblTCToDate");
                Label lblTCMainCanalName = (Label)gvTC.Rows[e.NewEditIndex].FindControl("lblTCMainCanalName");
                gvTC.EditIndex = e.NewEditIndex;
                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOTCandMC));
                DropDownList ddlTCMainCanal = (DropDownList)gvTC.Rows[e.NewEditIndex].FindControl("ddlTCChannel");
                TextBox txtFromDate = (TextBox)gvTC.Rows[e.NewEditIndex].FindControl("txtTCFromDate");
                TextBox txtToDate = (TextBox)gvTC.Rows[e.NewEditIndex].FindControl("txtTcToDate");
                lstOfObj = new List<object>();
                lstOfObj = bllCO.GetChannelsByChannelTypeandChannelCommandType(Convert.ToInt32(Constants.ChannelType.MainCanal), Convert.ToInt32(Constants.Commands.IndusCommand));
                lstOfObj.Sort((obj1, obj2) => Convert.ToString(obj1.GetType().GetProperty("ChannelName").GetValue(obj1)).CompareTo(Convert.ToString(obj2.GetType().GetProperty("ChannelName").GetValue(obj2))));
                BindDdl(ddlTCMainCanal, lstOfObj);
                object selectedChannelID = (from item in lstOfObj where Convert.ToString(item.GetType().GetProperty("ChannelName").GetValue(item)).Trim() == lblTCMainCanalName.Text.Trim() select item).FirstOrDefault();
                ddlTCMainCanal.SelectedValue = Convert.ToString(selectedChannelID.GetType().GetProperty("ChannelID").GetValue(selectedChannelID));
                txtFromDate.Text = lblTCFromDate.Text;
                txtToDate.Text = lblTCToDate.Text;

                DisplayAccordian(1);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"
                DataKey key = gvTC.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string ChannelCmdType = Convert.ToString(key.Values["ChannelCmdType"]);
                string ACCPID = Convert.ToString(key.Values["ACCPID"]);
                string MainCanalID = Convert.ToString(key.Values["MainCanalID"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvTC.Rows[e.RowIndex];
                DropDownList ddlTCMainCanal = (DropDownList)row.FindControl("ddlTCChannel");
                TextBox txtFromDate = (TextBox)row.FindControl("txtTCFromDate");
                TextBox txtToDate = (TextBox)row.FindControl("txtTcToDate");
                #endregion

                string fromDate = txtFromDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();
                long channelID = Convert.ToInt64(ddlTCMainCanal.SelectedValue);
                long AccpID = Convert.ToInt64(ACCPID);
                long ChannelCommandType = Convert.ToInt64(ChannelCmdType);
                bool isSaved = false;
                 
                string[] year = bllCO.GetACCP_ByID(AccpID).Year.Split('-'); ;
                DateTime fromStartDate = Convert.ToDateTime("01-Jul-" + year[0]);
                DateTime toEndDate = Convert.ToDateTime("30-Jun-" + year[1]);

                if (Utility.GetParsedDate(fromDate) < fromStartDate)
                {
                    Master.ShowMessage("From Date cannot be less than " + Utility.GetFormattedDate(fromStartDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(toDate) < fromStartDate)
                {
                    Master.ShowMessage("To Date cannot be less than " + Utility.GetFormattedDate(fromStartDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(fromDate) > toEndDate)
                {
                    Master.ShowMessage("From Date cannot be greater than " + Utility.GetFormattedDate(toEndDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(toDate) > toEndDate)
                {
                    Master.ShowMessage("To Date cannot be greater than " + Utility.GetFormattedDate(toEndDate), SiteMaster.MessageType.Error);
                    return;
                } 

                dd_ACCP.Clear();
                dd_ACCP.Add("FromDate", fromDate);
                dd_ACCP.Add("ToDate", toDate);
                dd_ACCP.Add("ChannelID", channelID);
                dd_ACCP.Add("ACPID", AccpID);
                dd_ACCP.Add("CommandType", ChannelCommandType);
                if (Convert.ToDateTime(fromDate) > Convert.ToDateTime(toDate))
                {
                    Master.ShowMessage(Message.TODateCannotBeLessFromDate.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (ID == "0")
                {
                    if (Session["ListOfTCandMCObjs"] == null)
                    {
                        Session["ListOfTCandMCObjs"] = bllCO.GetTCandMCByACCPID(AccpID);
                    }
                    List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                    object obj = (from ob in lstOTCandMC where Convert.ToInt64(ob.GetType().GetProperty("ChannelID").GetValue(ob)) == channelID select ob).FirstOrDefault();

                    if (obj == null)
                    {
                        dd_ACCP.Add("CreatedBy", mdlUser.ID);
                        isSaved = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_CREATE, dd_ACCP);
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueMainCanal.Description, SiteMaster.MessageType.Error);

                        return;
                    }
                }
                else
                {
                    if (Convert.ToInt64(MainCanalID) != channelID)
                    {
                        if (bllCO.AssociationExists(Convert.ToInt64(ID)))
                        {
                            Master.ShowMessage("Main canal cannot be changed, its associated exclude channels exists.", SiteMaster.MessageType.Error);
                            return;
                        }

                    }

                    dd_ACCP.Add("ID", ID);
                    dd_ACCP.Add("CreatedBy", mdlUser.ID);
                    if (Session["ListOfTCandMCObjs"] == null)
                    {
                        Session["ListOfTCandMCObjs"] = bllCO.GetTCandMCByACCPID(AccpID);
                    }
                    List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                    lstOTCandMC.Remove(lstOTCandMC.Find(x => Convert.ToInt64(x.GetType().GetProperty("MainCanalID").GetValue(x)) == Convert.ToInt64(MainCanalID)));
                    object obj = (from ob in lstOTCandMC where Convert.ToInt64(ob.GetType().GetProperty("ChannelID").GetValue(ob)) == channelID select ob).FirstOrDefault();
                    if (obj == null)
                    {
                        isSaved = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_UPDATE, dd_ACCP);
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueMainCanal.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (Convert.ToInt64(ID) == 0)
                    gvTC.PageIndex = 0;
                gvTC.EditIndex = -1;
                LoadACCPDetails(Convert.ToInt64(hfAccpID.Value));
                Master.ShowMessage(Message.RecordSaved.Description);
                DisplayAccordian(1);


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTC_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (mdlRoleRights != null)
                {

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAddACCPOrder = (Button)e.Row.FindControl("btnAddACCPOrder");
                        if (btnAddACCPOrder != null)
                        {
                            btnAddACCPOrder.Enabled = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnDeleteACCDetail = (Button)e.Row.FindControl("btnDeleteACCDetail");
                        Button btnEditACCDetail = (Button)e.Row.FindControl("btnEditACCDetail");
                        HyperLink btnExclude = (HyperLink)e.Row.FindControl("btnExclude");
                        if (btnDeleteACCDetail != null)
                        {
                            btnDeleteACCDetail.Enabled = (bool)mdlRoleRights.BDelete;
                        }
                        if (btnEditACCDetail != null)
                        {
                            btnEditACCDetail.Enabled = (bool)mdlRoleRights.BEdit;
                        }
                        if (btnExclude != null)
                        {
                            btnExclude.Enabled = (bool)mdlRoleRights.BEdit && (bool)mdlRoleRights.BAdd;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        #endregion
        #region Events gv MC

        protected void gvMC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMC.PageIndex = e.NewPageIndex;
                gvMC.EditIndex = -1;
                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOTCandMC));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvMC.EditIndex = -1;
                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                DisplayAccordian(0);
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOTCandMC));
                bindgvTC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.IndusCommand), lstOTCandMC));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {



                if (e.CommandName == "AddACCPDetail")
                {
                    List<object> lstAccpDetial = Session["ListOfTCandMCObjs"] as List<object>;
                    List<object> lstData = (from item in lstAccpDetial where Convert.ToInt64(item.GetType().GetProperty("ChannelCmdType").GetValue(item)) == Convert.ToInt64(Constants.Commands.JhelumChenabCommand) select item).ToList();
                    List<object> lstMJChnls = new List<object>();
                    lstMJChnls.Add(
                       new
                       {
                           ID = 0,
                           FromDate = Utility.GetFormattedDate(DateTime.Now),
                           ToDate = string.Empty,
                           MainCanalName = string.Empty,
                           ChannelCmdType = Convert.ToInt64(Constants.Commands.JhelumChenabCommand),
                           ACCPID = hfAccpID.Value,
                           MainCanalID = string.Empty
                       }); 

                     foreach (var d in lstData)
                         lstMJChnls.Add(d);

                    gvMC.PageIndex = gvMC.PageCount;
                    gvMC.DataSource = lstMJChnls;
                    gvMC.DataBind();
                    gvMC.EditIndex = 0;// gvMC.Rows.Count - 1;
                    gvMC.DataBind();
                    DisplayAccordian(0);

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvMC.EditIndex == e.Row.RowIndex)
                    {

                        DropDownList ddlMCChannel = (DropDownList)e.Row.FindControl("ddlMCChannel");

                        List<object> lstChannels = bllCO.GetChannelsByChannelTypeandChannelCommandType(Convert.ToInt32(Constants.ChannelType.MainCanal), Convert.ToInt32(Constants.Commands.JhelumChenabCommand));
                        lstChannels.Sort((obj1, obj2) => Convert.ToString(obj1.GetType().GetProperty("ChannelName").GetValue(obj1)).CompareTo(Convert.ToString(obj2.GetType().GetProperty("ChannelName").GetValue(obj2))));
                        BindDdl(ddlMCChannel, lstChannels);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                string ID = Convert.ToString(gvMC.DataKeys[e.RowIndex].Values[0]);
                long hfACCPID = Convert.ToInt64(hfAccpID.Value);
                dd_ACCP.Clear();
                dd_ACCP.Add("ID", ID);
                List<object> isDependencyExist = bllCO.GetExcludedChannels(Convert.ToInt64(ID));
                if (isDependencyExist != null && isDependencyExist.Count > 0)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool isDeleted = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_DELETE, dd_ACCP);
                if (isDeleted)
                {
                    LoadACCPDetails(hfACCPID);
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
                DisplayAccordian(0);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label labID = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCID");
                Label lblCommandType = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCCommandType");
                Label lblMainCanalID = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCMainCanalID");
                Label lblMCFromDate = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCFromDate");
                Label lblMCToDate = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCToDate");
                Label lblMCMainCanalName = (Label)gvMC.Rows[e.NewEditIndex].FindControl("lblMCMainCanalName");

                gvMC.EditIndex = e.NewEditIndex;

                List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                bindgvMC(FilterListOfDetialsByCommandType(Convert.ToInt32(Constants.Commands.JhelumChenabCommand), lstOTCandMC));
                DropDownList ddlTCMainCanal = (DropDownList)gvMC.Rows[e.NewEditIndex].FindControl("ddlMCChannel");
                TextBox txtFromDate = (TextBox)gvMC.Rows[e.NewEditIndex].FindControl("txtMCFromDate");
                TextBox txtToDate = (TextBox)gvMC.Rows[e.NewEditIndex].FindControl("txtMcToDate");

                lstOfObj = new List<object>();
                lstOfObj = bllCO.GetChannelsByChannelTypeandChannelCommandType(Convert.ToInt32(Constants.ChannelType.MainCanal), Convert.ToInt32(Constants.Commands.JhelumChenabCommand));
                lstOfObj.Sort((obj1, obj2) => Convert.ToString(obj1.GetType().GetProperty("ChannelName").GetValue(obj1)).CompareTo(Convert.ToString(obj2.GetType().GetProperty("ChannelName").GetValue(obj2))));
                BindDdl(ddlTCMainCanal, lstOfObj);
                object selectedChannelID = (from item in lstOfObj where Convert.ToString(item.GetType().GetProperty("ChannelName").GetValue(item)).Trim() == lblMCMainCanalName.Text.Trim() select item).FirstOrDefault();
                ddlTCMainCanal.SelectedValue = Convert.ToString(selectedChannelID.GetType().GetProperty("ChannelID").GetValue(selectedChannelID));
                txtFromDate.Text = lblMCFromDate.Text;
                txtToDate.Text = lblMCToDate.Text;

                DisplayAccordian(0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"
                DataKey key = gvMC.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string ChannelCmdType = Convert.ToString(key.Values["ChannelCmdType"]);
                string ACCPID = Convert.ToString(key.Values["ACCPID"]);
                string MainCanalID = Convert.ToString(key.Values["MainCanalID"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvMC.Rows[e.RowIndex];
                DropDownList ddlTCMainCanal = (DropDownList)row.FindControl("ddlMCChannel");
                TextBox txtFromDate = (TextBox)row.FindControl("txtMCFromDate");
                TextBox txtToDate = (TextBox)row.FindControl("txtMcToDate");
                #endregion

                string fromDate = txtFromDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();
                
                long AccpID = Convert.ToInt64(ACCPID);
                string [] year = bllCO.GetACCP_ByID(AccpID).Year.Split('-'); ;

                DateTime fromStartDate = Convert.ToDateTime("01-Jul-" + year[0]);
                DateTime toEndDate = Convert.ToDateTime("30-Jun-" + year[1]);

                if (Utility.GetParsedDate(fromDate) < fromStartDate)
                {
                    Master.ShowMessage("From Date cannot be less than " + Utility.GetFormattedDate(fromStartDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(toDate) < fromStartDate)
                {
                    Master.ShowMessage("To Date cannot be less than " + Utility.GetFormattedDate(fromStartDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(fromDate) > toEndDate)
                {
                    Master.ShowMessage("From Date cannot be greater than " + Utility.GetFormattedDate(toEndDate), SiteMaster.MessageType.Error);
                    return;
                }
                if (Utility.GetParsedDate(toDate) > toEndDate)
                {
                    Master.ShowMessage("To Date cannot be greater than " + Utility.GetFormattedDate(toEndDate), SiteMaster.MessageType.Error);
                    return;
                } 
                

                long channelID = Convert.ToInt64(ddlTCMainCanal.SelectedValue);
                long ChannelCommandType = Convert.ToInt64(ChannelCmdType);
                bool isSaved = false;
                dd_ACCP.Clear();
                dd_ACCP.Add("FromDate", fromDate);
                dd_ACCP.Add("ToDate", toDate);
                dd_ACCP.Add("ChannelID", channelID);
                dd_ACCP.Add("ACPID", AccpID);
                dd_ACCP.Add("CommandType", ChannelCommandType);
                if (Convert.ToDateTime(fromDate) > Convert.ToDateTime(toDate))
                {
                    Master.ShowMessage(Message.TODateCannotBeLessFromDate.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (ID == "0")
                {
                    if (Session["ListOfTCandMCObjs"] == null)
                    {
                        Session["ListOfTCandMCObjs"] = bllCO.GetTCandMCByACCPID(AccpID);
                    }
                    List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                    object obj = (from ob in lstOTCandMC where Convert.ToInt64(ob.GetType().GetProperty("ChannelID").GetValue(ob)) == channelID select ob).FirstOrDefault();
                    if (obj == null)
                    {
                        dd_ACCP.Add("CreatedBy", mdlUser.ID);
                        isSaved = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_CREATE, dd_ACCP);
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueMainCanal.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else
                {

                    if (Convert.ToInt64 (MainCanalID) != channelID)
                    { 
                        if (bllCO.AssociationExists(Convert.ToInt64(ID)))
                        {
                            Master.ShowMessage("Main canal cannot be changed, its associated exclude channels exists.", SiteMaster.MessageType.Error);
                            return;
                        }

                    }

                    dd_ACCP.Add("ID", ID);
                    dd_ACCP.Add("CreatedBy", mdlUser.ID);
                    if (Session["ListOfTCandMCObjs"] == null)
                    {
                        Session["ListOfTCandMCObjs"] = bllCO.GetTCandMCByACCPID(AccpID);
                    }
                    List<object> lstOTCandMC = Session["ListOfTCandMCObjs"] as List<object>;
                    lstOTCandMC.Remove(lstOTCandMC.Find(x => Convert.ToInt64(x.GetType().GetProperty("MainCanalID").GetValue(x)) == Convert.ToInt64(MainCanalID)));
                    object obj = (from ob in lstOTCandMC where Convert.ToInt64(ob.GetType().GetProperty("ChannelID").GetValue(ob)) == channelID select ob).FirstOrDefault();
                    if (obj == null)
                    {
                        isSaved = (bool)bllCO.ACCPDetail_Operations(Constants.CRUD_UPDATE, dd_ACCP);
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueMainCanal.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                }
                if (Convert.ToInt64(ID) == 0)
                    gvMC.PageIndex = 0;
                gvMC.EditIndex = -1;
                LoadACCPDetails(Convert.ToInt64(hfAccpID.Value));
                Master.ShowMessage(Message.RecordSaved.Description);
                DisplayAccordian(0);




            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMC_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (mdlRoleRights != null)
                {

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAddACCPOrder = (Button)e.Row.FindControl("btnAddACCPMCDetail");
                        if (btnAddACCPOrder != null)
                        {
                            btnAddACCPOrder.Enabled = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnMCEdit = (Button)e.Row.FindControl("btnMCEdit");
                        Button btnMCDelete = (Button)e.Row.FindControl("btnMCDelete");
                        HyperLink btnMCExclude = (HyperLink)e.Row.FindControl("btnMCExclude");
                        if (btnMCDelete != null)
                        {
                            btnMCDelete.Enabled = (bool)mdlRoleRights.BDelete;
                        }
                        if (btnMCEdit != null)
                        {
                            btnMCEdit.Enabled = (bool)mdlRoleRights.BEdit;
                        }
                        if (btnMCExclude != null)
                        {
                            btnMCExclude.Enabled = (bool)mdlRoleRights.BEdit && (bool)mdlRoleRights.BEdit;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}