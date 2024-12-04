using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls
{
    public partial class GaugeInspection : System.Web.UI.UserControl
    {
        public static string Title { get; set; }
        public static string Status { get; set; }
        public static string PreparedBy { get; set; }
        public static string FromDate { get; set; }
        public static string ToDate { get; set; }
        public static string ChannelName { get; set; }
        public static string InspectionArea { get; set; }
        public static string InspectedBy { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblScheduleTitle.Text = Title;
                    lblStatus.Text = Status;
                    lblPreparedBy.Text = PreparedBy;
                    lblFromDate.Text = FromDate;
                    lblToDate.Text = ToDate;
                    lblChannelName.Text = ChannelName;
                    lblInspectionArea.Text = InspectionArea;
                    lblInspectedBy.Text = InspectedBy;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}