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
    public partial class AddPayment : BasePage
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
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        List<dynamic> lstAwardedAssets = new AuctionBLL().GetAuctionItemsForPayments(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        gvPayment.DataSource = lstAwardedAssets;
                        gvPayment.DataBind();

                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvPayment_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string ID = GetDataKeyValue(gvPayment, "ID", e.Row.RowIndex);
                    string EarnestMoney = GetDataKeyValue(gvPayment, "EarnestMoney", e.Row.RowIndex);
                    string TokenMoney = GetDataKeyValue(gvPayment, "TokenMoney", e.Row.RowIndex);
                    string EarnestMoneySubmitted = GetDataKeyValue(gvPayment, "EarnestMoneySubmitted", e.Row.RowIndex);
                    string TokenMoneySubmitted = GetDataKeyValue(gvPayment, "TokenMoneySubmitted", e.Row.RowIndex);
                    string BidderRate = GetDataKeyValue(gvPayment, "BidderRate", e.Row.RowIndex);
                    Label lblBalanceAmount = (Label)e.Row.FindControl("lblBalanceAmount");
                    double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ID));
                    double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ID));
                    if (TotalTokenMoneySubmitted == null)
                    {
                        TotalTokenMoneySubmitted = 0;
                    }
                    if (TotalRemainingAmountSubmitted == null)
                    {
                        TotalRemainingAmountSubmitted = 0;
                    }

                    string Amount = Convert.ToString(Convert.ToDouble(BidderRate) - Convert.ToDouble(EarnestMoneySubmitted) - TotalTokenMoneySubmitted - TotalRemainingAmountSubmitted);
                   // Amount = string.Format("{0:n0}", Amount);
                    lblBalanceAmount.Text = string.Format("{0:#,##0}", double.Parse(Amount));
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPayment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                long AuctionPriceID = Convert.ToInt64(((Label)gvRow.FindControl("ID")).Text);
                if (e.CommandName == "Payments")
                {
                    Response.RedirectPermanent(String.Format("AddRemainingPayment.aspx?AuctionNoticeID={0}&AuctionPriceID={1}", Convert.ToInt64(hdnAuctionNoticeID.Value),AuctionPriceID));
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