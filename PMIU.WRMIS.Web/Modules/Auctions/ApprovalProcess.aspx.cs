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
    public partial class ApprovalProcess : BasePage
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
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        List<dynamic> lstAwardedAssets = new AuctionBLL().GetAuctionItemsForPayments(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        gvApprovalProcess.DataSource = lstAwardedAssets;
                        gvApprovalProcess.DataBind();

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

        protected void gvApprovalProcess_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string ID = GetDataKeyValue(gvApprovalProcess, "ID", e.Row.RowIndex);
                    string EarnestMoney = GetDataKeyValue(gvApprovalProcess, "EarnestMoney", e.Row.RowIndex);
                    string TokenMoney = GetDataKeyValue(gvApprovalProcess, "TokenMoney", e.Row.RowIndex);
                    string EarnestMoneySubmitted = GetDataKeyValue(gvApprovalProcess, "EarnestMoneySubmitted", e.Row.RowIndex);
                    string TokenMoneySubmitted = GetDataKeyValue(gvApprovalProcess, "TokenMoneySubmitted", e.Row.RowIndex);
                    string BidderRate = GetDataKeyValue(gvApprovalProcess, "BidderRate", e.Row.RowIndex);
                    Label lblTotalMoneyPaid = (Label)e.Row.FindControl("lblTotalMoneyPaid");

                    string Status = GetDataKeyValue(gvApprovalProcess, "Status", e.Row.RowIndex);
                    if (Status.ToUpper() == "DELIVERED" || Status.ToUpper() == "CANCELLED")
                    {
                        Button btnDelivery = (Button)e.Row.FindControl("btnDelivery");
                       // btnDelivery.Visible = false;
                    }


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

                    string Amount = Convert.ToString(Convert.ToDouble(EarnestMoneySubmitted) + TotalTokenMoneySubmitted + TotalRemainingAmountSubmitted);
                    // Amount = string.Format("{0:n0}", Amount);
                    lblTotalMoneyPaid.Text = string.Format("{0:#,##0}", double.Parse(Amount));
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvApprovalProcess_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                long AuctionPriceID = Convert.ToInt64(((Label)gvRow.FindControl("ID")).Text);
                if (e.CommandName == "Delivery")
                {
                    Response.RedirectPermanent(String.Format("DeliveryDetails.aspx?AuctionNoticeID={0}&AuctionPriceID={1}", Convert.ToInt64(hdnAuctionNoticeID.Value), AuctionPriceID));
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