using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class SearchCriteriaForOutletPerformance : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    long LoggedUser = Convert.ToInt64(Session[SessionValues.UserID]);
                    bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/DailyData/AddOutletPerformanceData.aspx");

                    if (HasRights)
                    {
                        BindCommandDropDown(Convert.ToString(Session[SessionValues.CommandID]));
                        BindChannelTypeDropDown(Convert.ToString(Session[SessionValues.ChanneltypeID]));
                        BindFlowTypeDropDown(Convert.ToString(Session[SessionValues.FlowTypeID]));
                        BindChannelNameDropDown(Convert.ToString(Session[SessionValues.ChannelNameID]));

                        if (Request.QueryString["History"] != null)
                        {
                            string QS = Request.QueryString["History"].ToString();
                            gvChannels.DataSource = new DailyDataBLL().GetSearchResult(LoggedUser, Convert.ToInt64(Session[SessionValues.CommandID]), Convert.ToInt64(Session[SessionValues.ChanneltypeID]), Convert.ToInt64(Session[SessionValues.FlowTypeID]), Convert.ToInt64(Session[SessionValues.ChannelNameID]));
                            gvChannels.DataBind();
                        }
                    }
                    else
                    {
                        Master.ShowMessage("Not allowed to view screen/Appropriate message", SiteMaster.MessageType.Error);
                        divMain.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchVriteriaForOutletPerformance);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void BindCommandDropDown(string _CommandID)
        {
            ddlCommandName.DataSource = new DailyDataBLL().GetAllChannelCommands();
            ddlCommandName.DataTextField = "Name";
            ddlCommandName.DataValueField = "ID";
            ddlCommandName.DataBind();
            ddlCommandName.Items.Insert(0, new ListItem("All", ""));
            if (_CommandID != "" && _CommandID != "-1")
                ddlCommandName.Items.FindByValue(_CommandID).Selected = true;
        }

        public void BindChannelTypeDropDown(string _ChannelID)
        {
            ddlChannelType.DataSource = new DailyDataBLL().GetAllChannelTypes();
            ddlChannelType.DataTextField = "Name";
            ddlChannelType.DataValueField = "ID";
            ddlChannelType.DataBind();
            ddlChannelType.Items.Insert(0, new ListItem("All", ""));
            if (_ChannelID != "" && _ChannelID != "-1")
                ddlChannelType.Items.FindByValue(_ChannelID).Selected = true;
        }

        public void BindFlowTypeDropDown(string _FlowID)
        {
            ddlFlowType.DataSource = new DailyDataBLL().GetAllChannelFlowTypes();
            ddlFlowType.DataTextField = "Name";
            ddlFlowType.DataValueField = "ID";
            ddlFlowType.DataBind();
            ddlFlowType.Items.Insert(0, new ListItem("All", ""));
            if (_FlowID != "" && _FlowID != "-1")
                ddlFlowType.Items.FindByValue(_FlowID).Selected = true;
        }

        public void BindChannelNameDropDown(string _ChannelID)
        {
            try
            {
                ddlChannelName.DataSource = new DailyDataBLL().GetUserChannles(Convert.ToInt64(Session[SessionValues.UserID]));
                ddlChannelName.DataTextField = "Name";
                ddlChannelName.DataValueField = "ID";
                ddlChannelName.DataBind();
                ddlChannelName.Items.Insert(0, new ListItem("All", ""));
                if (_ChannelID != "" && _ChannelID != "-1")
                    ddlChannelName.Items.FindByValue(_ChannelID).Selected = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string CommandName = ddlCommandName.SelectedItem.Value.ToString();
                long CommandID = -1;

                if (CommandName != "")
                {
                    CommandID = Convert.ToInt64(ddlCommandName.SelectedItem.Value);
                }

                string Channeltype = ddlChannelType.SelectedItem.Value.ToString();
                long ChanneltypeID = -1;

                if (Channeltype != "")
                {
                    ChanneltypeID = Convert.ToInt64(ddlChannelType.SelectedItem.Value);
                }

                string FlowType = ddlFlowType.SelectedItem.Value.ToString();
                long FlowTypeID = -1;

                if (FlowType != "")
                {
                    FlowTypeID = Convert.ToInt64(ddlFlowType.SelectedItem.Value);
                }

                string ChannelName = ddlChannelName.SelectedItem.Value.ToString();
                long ChannelNameID = -1;

                if (ChannelName != "")
                {
                    ChannelNameID = Convert.ToInt64(ddlChannelName.SelectedItem.Value);
                }

                Session[SessionValues.CommandID] = CommandID;
                Session[SessionValues.ChanneltypeID] = ChanneltypeID;
                Session[SessionValues.FlowTypeID] = FlowTypeID;
                Session[SessionValues.ChannelNameID] = ChannelNameID;

                gvChannels.DataSource = new DailyDataBLL().GetSearchResult(Convert.ToInt64(Session[SessionValues.UserID]), CommandID, ChanneltypeID, FlowTypeID, ChannelNameID);
                gvChannels.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvChannels_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //gvChannels.EditIndex = -1;
                //gvChannels.DataSource = new DailyDataBLL().GetSearchResult(Convert.ToInt64(ViewState["UserID"]), Convert.ToInt64(Session["CommandID"]), Convert.ToInt64(Session["ChanneltypeID"]), Convert.ToInt64(Session["FlowTypeID"]), Convert.ToInt64(Session["ChannelNameID"]));
                //gvChannels.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvChannels_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannels.PageIndex = e.NewPageIndex;
                gvChannels.DataSource = new DailyDataBLL().GetSearchResult(Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(Session[SessionValues.CommandID]), Convert.ToInt64(Session[SessionValues.ChanneltypeID]), Convert.ToInt64(Session[SessionValues.FlowTypeID]), Convert.ToInt64(Session[SessionValues.ChannelNameID]));
                gvChannels.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvChannels_RowEditing(object sender, GridViewEditEventArgs e)
        {
            long ChannelID = Convert.ToInt32(((Label)gvChannels.Rows[e.NewEditIndex].Cells[0].FindControl("lblID")).Text);
            Response.Redirect("SearchCriteriaForOutletPerformance.aspx?ChannelID=" + ChannelID);
        }
    }
}