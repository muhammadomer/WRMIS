using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet
{
    // Enum used for pages links
    public enum PageNames
    {
        OutletVillages,
        Alteration,
        AlterationHistory,
        AddOutlet,
        EditOutlet,
        EditHistory
    }
    public partial class OutletView : BasePage
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
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        channelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        hdnChannelID.Value = Convert.ToString(channelID);
                        OutletChannelDetails.ChannelID = channelID;

                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}", channelID);
                        // Set Add new url
                        hlAdd.NavigateUrl = GetPageURL(PageNames.AddOutlet, "0", "0");

                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }

                        LoadOutletView(channelID);

                        hlAdd.Visible = base.CanAdd;
                    }
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Outlets);
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

                gvOutlet.VirtualItemCount = Convert.ToInt16(TotalRecords);

                gvDataCount = LstOutletData.Count;

                gvOutlet.DataSource = LstOutletData;
                gvOutlet.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnHtmlDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlButton btn = (HtmlButton)sender;

            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int i = Convert.ToInt32(row.RowIndex);
            Label lblOutlet = (Label)(gvOutlet.Rows[i].FindControl("lblOutletID"));

            long _OutletID = Convert.ToInt64(lblOutlet.Text);

            if (_OutletID != 0)
            {
                bool isDeleted = new IN_OutletBLL().DeleteOutletByID(_OutletID);

                BindOutletData(Convert.ToInt64(hdnChannelID.Value), 0);

                lblMessage.Text = Message.OutletDeleted.Description;
                lblMessage.Visible = true;
            }
        }
        public string GetPageURL(PageNames _PageName, string _OutletID, string _AlterationID)
        {
            string URL = "~/Modules/IrrigationNetwork/Outlet/";
            try
            {
                switch (_PageName)
                {
                    case PageNames.Alteration:
                        URL = URL + "OutletAlteration.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID + "&AlterationID=" + _AlterationID;
                        break;
                    case PageNames.AlterationHistory:
                        URL = URL + "OutletAlterationHistory.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID;
                        break;
                    case PageNames.OutletVillages:
                        URL = URL + "OutletVillage.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID + "&AlterationID=" + _AlterationID;
                        break;
                    case PageNames.AddOutlet:
                        URL = URL + "OutletAdd.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID + "&AlterationID=" + _AlterationID;
                        break;
                    case PageNames.EditOutlet:
                        URL = URL + "OutletAdd.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID + "&AlterationID=" + _AlterationID;
                        break;
                    //case PageNames.EditHistory:
                    //    URL = URL + "OutletAlterationHistory.aspx?ChannelID=" + hdnChannelID.Value + "&OutletID=" + _OutletID + "&ActionType=E";
                    //    break;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return URL;
        }
        protected void gvOutlet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOutlet.PageIndex = e.NewPageIndex;
            gvOutlet.EditIndex = -1;
            BindOutletData(Convert.ToInt64(hdnChannelID.Value), gvOutlet.PageIndex);
        }
        protected void gvOutlet_RowCreated(object sender, GridViewRowEventArgs e)
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
                    count = gvOutlet.PageSize;
                return Convert.ToInt32(count);
            }
            set
            {
                ViewState["gvDataCount"] = value;
            }
        }

    }
}