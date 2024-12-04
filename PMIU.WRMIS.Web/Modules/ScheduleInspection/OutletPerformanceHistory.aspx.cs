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
using System.Globalization;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class OutletPerformanceHistory : BasePage
    {

        #region ViewState
        public string OutletID = "OutletID";
        public string FromDate_VS = "FromDate";
        public string ToDate_VS = "ToDate";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                    bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/ScheduleInspection/OutletPerformanceHistory.aspx");

                    if (HasRights)
                    {
                        ViewState[OutletID] = Convert.ToInt64(Request.QueryString[OutletID]);
                        ParentInformation();
                        txtFromDate.Text = Utility.GetFormattedDate(DateTime.Today.AddDays(-1));
                        txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                        hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/CriteriaForSpecificOutlet.aspx?ChannelID={0}", Convert.ToString(hdnChannelID.Value));
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
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SIOutletPerformanceHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void ParentInformation()
        {
            try
            {
                long ChannelID = new DailyDataBLL().GetChannelID(Convert.ToInt64(ViewState[OutletID]));
                object parentInfo = new DailyDataBLL().OutletHistoryInformation(ChannelID, Convert.ToInt64(ViewState[OutletID]));

                if (parentInfo != null)
                {
                    lblChnlName.Text = Convert.ToString(parentInfo.GetType().GetProperty("ChannelName").GetValue(parentInfo));
                    lblChnlType.Text = Convert.ToString(parentInfo.GetType().GetProperty("ChannelType").GetValue(parentInfo));
                    lblTotalRD.Text = Convert.ToString(parentInfo.GetType().GetProperty("TotalRDs").GetValue(parentInfo));
                    lblflowtype.Text = Convert.ToString(parentInfo.GetType().GetProperty("FlowType").GetValue(parentInfo));
                    lblCommandName.Text = Convert.ToString(parentInfo.GetType().GetProperty("CommandName").GetValue(parentInfo));
                    lblOutletRD.Text = Convert.ToString(parentInfo.GetType().GetProperty("OutletRDs").GetValue(parentInfo));
                    lblOutletSide.Text = Convert.ToString(parentInfo.GetType().GetProperty("Outletside").GetValue(parentInfo));
                    lblDistrict.Text = Convert.ToString(parentInfo.GetType().GetProperty("DistrictName").GetValue(parentInfo));
                    lblTehsil.Text = Convert.ToString(parentInfo.GetType().GetProperty("TehsilName").GetValue(parentInfo));
                    lblPoliceStation.Text = Convert.ToString(parentInfo.GetType().GetProperty("PoliceStation").GetValue(parentInfo));
                    lblVillage.Text = Convert.ToString(parentInfo.GetType().GetProperty("VillageName").GetValue(parentInfo));
                    lblIMIS.Text = Convert.ToString(parentInfo.GetType().GetProperty("IMISCode").GetValue(parentInfo));
                    hdnChannelID.Value = Convert.ToString(parentInfo.GetType().GetProperty("ChannelID").GetValue(parentInfo));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnShowHistory_Click(object sender, EventArgs e)
        {
            try
            {
                bool Valid = true;
                DateTime? FromDate = new DateTime();
                DateTime? ToDate = new DateTime();

                if (txtFromDate.Text != "")
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                else
                    FromDate = null;

                if (txtToDate.Text != "")
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                else
                    ToDate = null;

                if (FromDate != null && ToDate != null)
                {
                    if (FromDate > ToDate)
                    {
                        Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                        Valid = false;
                        gvOutletshistory.Visible = false;
                    }
                }
                ViewState[FromDate_VS] = FromDate;
                ViewState[ToDate_VS] = ToDate;
                if (Valid)
                {
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), FromDate, ToDate);
                    gvOutletshistory.DataBind();
                    gvOutletshistory.Visible = true;
                    Master.HideMessageInstantly();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutletshistory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOutletshistory.EditIndex = -1;
                if (ViewState[FromDate_VS] == null && ViewState[ToDate_VS] != null)
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), null, Convert.ToDateTime(ViewState[ToDate_VS]));
                else if (ViewState[FromDate_VS] != null && ViewState[ToDate_VS] == null)
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), Convert.ToDateTime(ViewState[FromDate_VS]), null);
                else
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), null, null);

                gvOutletshistory.DataBind();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutletshistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletshistory.PageIndex = e.NewPageIndex;
                //gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState["OutletID"]), Convert.ToDateTime(ViewState["FromDate"]), Convert.ToDateTime(ViewState["ToDate"]));  //33389

                if (ViewState[FromDate_VS] == null && ViewState[ToDate_VS] != null)
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), null, Convert.ToDateTime(ViewState[ToDate_VS]));
                else if (ViewState["FromDate"] != null && ViewState["ToDate"] == null)
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), Convert.ToDateTime(ViewState[FromDate_VS]), null);
                else
                    gvOutletshistory.DataSource = new DailyDataBLL().GetHistory(Convert.ToInt64(ViewState[OutletID]), null, null);

                gvOutletshistory.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}