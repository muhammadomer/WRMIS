using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;


namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls
{
    public partial class DrainDetails : System.Web.UI.UserControl
    {
        public static string DrainName { get; set; }
        public static string TotalLength { get; set; }
        public static string CatchmentArea { get; set; }
        public static string MajorBuildupArea { get; set; }
        public static string DrainStatus { get; set; }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblDrainName.Text = DrainName;
                    lblCatchmentArea.Text = CatchmentArea;
                    lblTotalLength.Text = TotalLength;
                    lblMajorBuildUpArea.Text = MajorBuildupArea;
                    lblDrainStatus.Text = DrainStatus;
                    
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}