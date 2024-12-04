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
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class CriteriaForSpecificOutlet : BasePage
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
            long channelID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        channelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        hdnChannelID.Value = Convert.ToString(channelID);
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        ParentTable();
                        LoadOutletView(channelID);
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SICriteriaforSpecificOutlet);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadOutletView(long _ChannelID)
        {
            try
            {
                BindOutletData(_ChannelID, 0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindOutletData(long _ChannelID, int _PageIndex)
        {
            try
            {
                List<dynamic> LstOutletData = new IN_OutletBLL().GetOutletHistory(_ChannelID, (_PageIndex + 1), 10);
                dynamic outletHistoryData = LstOutletData[0];
                string TotalRecords = Convert.ToString(outletHistoryData.GetType().GetProperty("TotalRecords").GetValue(outletHistoryData, null));

                gvOutlets.VirtualItemCount = Convert.ToInt16(TotalRecords);

                gvDataCount = LstOutletData.Count;

                gvOutlets.DataSource = LstOutletData;
                gvOutlets.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void ParentTable()
        {
            try
            {
                if (Convert.ToInt64(hdnChannelID.Value) != 0)
                {
                    object OutletInformation = new DailyDataBLL().ChannelParentInformation(Convert.ToInt64(hdnChannelID.Value));
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
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutlets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOutlets.PageIndex = e.NewPageIndex;
            gvOutlets.EditIndex = -1;
            BindOutletData(Convert.ToInt64(hdnChannelID.Value), gvOutlets.PageIndex);
        }
        protected void gvOutlets_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                if (e.Row.RowIndex > this.gvDataCount)
                    e.Row.Visible = false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private int gvDataCount
        {
            get
            {
                object count = ViewState["gvDataCount"];
                if (count == null)
                    count = gvOutlets.PageSize;
                return Convert.ToInt32(count);
            }
            set
            {
                ViewState["gvDataCount"] = value;
            }
        }
    }
}