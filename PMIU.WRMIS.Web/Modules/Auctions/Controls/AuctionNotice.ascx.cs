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
    public partial class AuctionNotice : System.Web.UI.UserControl
    {
        public static string AuctionNoticeName { get; set; }
        public static string OpeningDate { get; set; }
        public static string SubmissionDate { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblAuctionNotice.Text = AuctionNoticeName;
                    lblOpeningDate.Text = OpeningDate;
                    lblSubmissiondate.Text = SubmissionDate;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}