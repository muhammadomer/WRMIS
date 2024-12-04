using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.Controls
{
    public partial class AddWorks : System.Web.UI.UserControl
    {
        public static string TenderNotice { get; set; }
        public static string TenderDomain { get; set; }
        public static string TenderDivision { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblTenderNotice.Text = TenderNotice;
                    lblTenderDomain.Text = TenderDomain;
                    lblTenderDivision.Text = TenderDivision;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}