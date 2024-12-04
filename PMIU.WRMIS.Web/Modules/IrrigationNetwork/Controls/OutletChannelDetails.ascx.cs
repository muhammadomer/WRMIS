using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.AppBlocks;
namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls
{
    public partial class OutletChannelDetails : System.Web.UI.UserControl
    {
        public static long ChannelID { get; set; }
        public static long ChannelTotalRDs { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            try
            {
                long.TryParse(Convert.ToString(OutletChannelDetails.ChannelID), out channelID);
                LoadChannelDetails(channelID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadChannelDetails(long _ChannelID)
        {
            try
            {
                CO_Channel channel = new ChannelBLL().GetChannelByID(_ChannelID);
                lblChannelName.Text = channel.NAME;
                lblTotalRD.Text = Calculations.GetRDText(channel.TotalRDs);
                hdnChannelTotalRDs.Value = Convert.ToString(channel.TotalRDs);
                lblIMISCode.Text = Convert.ToString(channel.IMISCode);

                IN_OutletBLL outLetBLL = new IN_OutletBLL();
                lblChannelType.Text = outLetBLL.GetChannelTypeByID(channel.ChannelTypeID);
                lblFlowType.Text = outLetBLL.GetFlowType(channel.FlowTypeID);
                lblCommandName.Text = outLetBLL.GetCommandNameByID(channel.ComndTypeID);
                OutletChannelDetails.ChannelTotalRDs = Convert.ToInt64(channel.TotalRDs);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}