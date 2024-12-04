using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class ViewBidderList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindGridView(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionDetailData(long _AuctionNoticeID)
        {
            try
            {
                AC_AuctionNotice mdlAuctionData = new AuctionBLL().GetAuctionDetailsByID(_AuctionNoticeID);
                string AuctionNotice = mdlAuctionData.AuctionTitle;
                string OpeningDate = Utility.GetFormattedDate(mdlAuctionData.OpeningDate);
                string SubmissionDate = Utility.GetFormattedDate(mdlAuctionData.SubmissionDate);

                Auctions.Controls.AuctionNotice.AuctionNoticeName = AuctionNotice;
                Auctions.Controls.AuctionNotice.OpeningDate = OpeningDate;
                Auctions.Controls.AuctionNotice.SubmissionDate = SubmissionDate;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetBidders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string Attachment = GetDataKeyValue(gvAssetBidders, "Attachment", e.Row.RowIndex);
                    string EarnestMoney = GetDataKeyValue(gvAssetBidders, "EarnestMoney", e.Row.RowIndex);
                    string EarnestMoneySubmitted = GetDataKeyValue(gvAssetBidders, "EarnestMoneySubmitted", e.Row.RowIndex);
                    HyperLink hlImage = (HyperLink)e.Row.FindControl("hlImage");
                    Label lblEarnestMoney = (Label)e.Row.FindControl("lblEarnestMoney");
                    Label lblEarnestMoneySubmitted = (Label)e.Row.FindControl("lblEarnestMoneySubmitted");
                    hlImage.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, Attachment);
                    //hlImage.Text = Attachment.Substring(Attachment.LastIndexOf('_') + 1);
                    hlImage.Attributes["FullName"] = Attachment;
                    lblEarnestMoney.Text = string.Format("{0:#,##0}", double.Parse(EarnestMoney));
                    lblEarnestMoneySubmitted.Text = string.Format("{0:#,##0}", double.Parse(EarnestMoneySubmitted));
                }
               
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private void BindGridView(long _AuctionNoticeID)
        {
            try
            {
                List<dynamic> lstAuctionAssets = new AuctionBLL().GetAssetsforBidderEarnestMoneyView(Convert.ToInt64(hdnAuctionNoticeID.Value));
                gvAssetBidders.DataSource = lstAuctionAssets;
                gvAssetBidders.DataBind();

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Auctions);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}