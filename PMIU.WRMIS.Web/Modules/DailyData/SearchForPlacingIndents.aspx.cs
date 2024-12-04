using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class SearchForPlacingIndents : BasePage
    {
        List<CO_Channel> lstChannel = new List<CO_Channel>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    IndentsBLL bllIndents = new IndentsBLL();
                    long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                    UA_AssociatedLocation mdlAssociatedLocation = bllIndents.GetSubDivisionByUserID(LoggedUser,-1);
                    if (mdlAssociatedLocation != null)
                    {
                        ViewState["SubDivisionID"] = mdlAssociatedLocation.IrrigationBoundryID;
                    }
                    BindDropdownlists();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchIndentCriteria] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                }
                catch (Exception Exp)
                {
                    new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
        }

        protected void btnSearchChannel_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function binds data to the grid on the basis of selected criteria and without Criteria
        /// Created On: 7/12/2015
        /// </summary>
        private void BindGrid()
        {
            List<long> SearchCriteria = new List<long>();
            long SubDivisionID = Convert.ToInt64(ViewState["SubDivisionID"]);
            long ChannelName = ddlChannelName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannelName.SelectedItem.Value);
            long FlowType = ddlFlowType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlFlowType.SelectedItem.Value);
            long CommandName = ddlCommandName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCommandName.SelectedItem.Value);
            long ChannelType = ddlChannelType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannelType.SelectedItem.Value);

            gvPlacingIndents.DataSource = new IndentsBLL().GetAllChannels(SubDivisionID, CommandName, ChannelType, FlowType, ChannelName);
            gvPlacingIndents.DataBind();

            SearchCriteria.Add(CommandName);
            SearchCriteria.Add(ChannelType);
            SearchCriteria.Add(FlowType);
            SearchCriteria.Add(ChannelName);
            SearchCriteria.Add(gvPlacingIndents.PageIndex);
            Session[SessionValues.SearchIndentCriteria] = SearchCriteria;
        }

        /// <summary>
        /// this function binds all the dropdowns.
        /// Created On: 7/12/2015
        /// </summary>
        private void BindDropdownlists()
        {
            // Bind Channel type dropdownlist
            Dropdownlist.DDLChannelTypes(ddlChannelType);
            // Bind Flow type dropdownlist
            Dropdownlist.DDLFlowTypes(ddlFlowType);
            //// Bind Common name dropdownlist
            Dropdownlist.DDLCommandNames(ddlCommandName);
            ////Bind Channel name dropdownlist
            BindChannelNameDropdown();
        }

        protected void gvPlacingIndents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPlacingIndents.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function binds Channel Names to the dropdown.
        /// Created On: 7/12/2015
        /// </summary>
        public void BindChannelNameDropdown()
        {
            //long SubDivisionID = Convert.ToInt64(ViewState["SubDivisionID"]);
            //long DivisionID = -1;
            //List<CO_Channel> lstChannelName = new IndentsBLL().GetChannelsNameBySubDivisionID(SubDivisionID, DivisionID);
            //ddlChannelName.DataSource = lstChannelName;
            //ddlChannelName.DataTextField = "NAME";
            //ddlChannelName.DataValueField = "ID";
            //ddlChannelName.DataBind();
            //ddlChannelName.Items.Insert(0, new ListItem("Select", ""));
        }

        protected void gvPlacingIndents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTotalRd = (Label)e.Row.FindControl("lblTotalRD");
                    Label lblIndentPlacementDate = (Label)e.Row.FindControl("lblIndentPlacementDate");
                    if (lblIndentPlacementDate.Text != "")
                        lblIndentPlacementDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblIndentPlacementDate.Text));

                    if (lblTotalRd.Text != String.Empty)
                    {
                        double TotalRd = Convert.ToDouble(lblTotalRd.Text);
                        lblTotalRd.Text = Calculations.GetRDText(TotalRd);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 7/12/2015 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchPlacingIndents);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindHistoryData()
        {
            List<long> SearchCriteria = (List<long>)Session[SessionValues.SearchIndentCriteria];
            Dropdownlist.SetSelectedValue(ddlCommandName, Convert.ToString(SearchCriteria[0]));
            Dropdownlist.SetSelectedValue(ddlChannelType, Convert.ToString(SearchCriteria[1]));
            Dropdownlist.SetSelectedValue(ddlFlowType, Convert.ToString(SearchCriteria[2]));
            Dropdownlist.SetSelectedValue(ddlChannelName, Convert.ToString(SearchCriteria[3]));
            gvPlacingIndents.PageIndex = (int)SearchCriteria[4];
            BindGrid();
        }
    }
}