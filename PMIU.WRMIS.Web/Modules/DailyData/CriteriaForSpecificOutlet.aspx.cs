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
    public partial class CriteriaForSpecificOutlet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    ParentTable();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.CriteriaforSpecificOutlet);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void ParentTable()
        {
            try
            {
                long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/DailyData/CriteriaForSpecificOutlet.aspx");

                if (HasRights)
                {
                    if (Request.QueryString["History"] != null)
                    {
                        string QS = Request.QueryString["History"].ToString();
                        List<object> lstResult = new DailyDataBLL().OutletsDetail(Convert.ToInt64(Session[SessionValues.ChannelID]));
                        gvOutlets.DataSource = lstResult;
                        gvOutlets.DataBind();
                    }
                    else
                    {
                        Session[SessionValues.ChannelID] = Convert.ToInt64(Request.QueryString["ChannelID"]);
                    }

                    if (Session[SessionValues.ChannelID] != null)
                    {
                        object OutletInformation = new DailyDataBLL().ChannelParentInformation(Convert.ToInt64(Session[SessionValues.ChannelID]));
                        if (OutletInformation != null)
                        {
                            lblChnlName.Text = OutletInformation.GetType().GetProperty("ChannelName").GetValue(OutletInformation).ToString();
                            lblChnlType.Text = OutletInformation.GetType().GetProperty("ChannelType").GetValue(OutletInformation).ToString();
                            lblTotalRD.Text = OutletInformation.GetType().GetProperty("TotalRDs").GetValue(OutletInformation).ToString();
                            lblFlowType.Text = OutletInformation.GetType().GetProperty("FlowType").GetValue(OutletInformation).ToString();
                            lblCommandName.Text = OutletInformation.GetType().GetProperty("CommandName").GetValue(OutletInformation).ToString();
                            lblIMISCode.Text = Convert.ToString(OutletInformation.GetType().GetProperty("IMISCode").GetValue(OutletInformation));
                            lblSectionName.Text = Convert.ToString(OutletInformation.GetType().GetProperty("SectionName").GetValue(OutletInformation));
                        }
                    }
                }
                else
                {
                    Master.ShowMessage("Not allowed to view screen/Appropriate message", SiteMaster.MessageType.Error);
                    divMain.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            try
            {
                if (Session[SessionValues.ChannelID] != null)
                {
                    List<object> lstResult = new DailyDataBLL().OutletsDetail(Convert.ToInt64(Session[SessionValues.ChannelID]));
                    gvOutlets.DataSource = lstResult;
                    gvOutlets.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOutlets_PageIndexChanged(object sender, EventArgs e)
        {
            gvOutlets.EditIndex = -1;
            gvOutlets.DataSource = new DailyDataBLL().OutletsDetail(Convert.ToInt64(Session[SessionValues.ChannelID]));
            gvOutlets.DataBind();
        }

        protected void gvOutlets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOutlets.PageIndex = e.NewPageIndex;
            gvOutlets.DataSource = new DailyDataBLL().OutletsDetail(Convert.ToInt64(Session[SessionValues.ChannelID]));
            gvOutlets.DataBind();
        }

        protected void gvOutlets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    //HyperLink OutletPerformance = e.Row.FindControl("hlOutletPerformance") as HyperLink;
                    //OutletPerformance.Visible = false;

                    //HyperLink OutletPerformanceHist = e.Row.FindControl("hlOutletPerformanceHist") as HyperLink;
                    //OutletPerformanceHist.Visible = false;

                }
                catch (Exception exp)
                {
                    new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
                }
            }
        }
    }
}