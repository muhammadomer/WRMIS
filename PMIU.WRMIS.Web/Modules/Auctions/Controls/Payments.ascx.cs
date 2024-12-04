using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions.Controls
{
    public partial class Payments : System.Web.UI.UserControl
    {
        public static string AuctionNoticeName { get; set; }
        public static string OpeningDate { get; set; }
        public static string SubmissionDate { get; set; }
        public static string Category { get; set; }
        public static string SubCategory { get; set; }
        public static string AssetName { get; set; }
        public static string EarnestMoney { get; set; }
        public static string BidderRate { get; set; }
        public static string TokenMoney { get; set; }
        public static string TotalTokenMoneyPaid { get; set; }
        public static string TotalAmountPaid { get; set; }
        public static string BalanceAmount { get; set; }
        public static string BalanceSubmissionDate { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblAuctionNotice.Text = AuctionNoticeName;
                    lblOpeningDate.Text = OpeningDate;
                    lblSubmissiondate.Text = SubmissionDate;
                    lblCategory.Text = Category;
                    lblSubCategory.Text = SubCategory;
                    lblAssetName.Text = AssetName;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblBidderRate.Text = BidderRate;
                    lblTokenMoney.Text = TokenMoney;
                    lblTotalTokenMoneyPaid.Text = TotalTokenMoneyPaid;
                    lblTotalAmountPaid.Text = TotalAmountPaid;
                    lblBalanceAmount.Text = BalanceAmount;
                    lblBalanceSubmissionDate.Text = BalanceSubmissionDate;
                 
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}