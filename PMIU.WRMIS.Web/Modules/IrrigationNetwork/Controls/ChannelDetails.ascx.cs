using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls
{
    public partial class ChannelDetails : System.Web.UI.UserControl
    {
        public static long ChannelID { get; set; }
        public static long ChannelTotalRDs { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            try
            {
                long.TryParse(Convert.ToString(ChannelDetails.ChannelID), out channelID);
                LoadChannelDetail(channelID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadChannelDetail(long _ChannelID)
        {
            try
            {
                CO_Channel channel = new ChannelBLL().GetChannelByID(_ChannelID);
                lblChannelName.Text = channel.NAME;
                lblTotalRDs.Text = Calculations.GetRDText(channel.TotalRDs);
                hdnChannelTotalRDs.Value = Convert.ToString(channel.TotalRDs);
                lblGCAACRE.Text = Convert.ToString(channel.GCAAcre);
                lblCCAAcre.Text = Convert.ToString(channel.CCAAcre);

                IN_OutletBLL outLetBLL = new IN_OutletBLL();
                IMISCode._ChannelTypeID = Convert.ToInt64(channel.ChannelTypeID);
                lblChannelType.Text = outLetBLL.GetChannelTypeByID(channel.ChannelTypeID);
                lblFlowType.Text = outLetBLL.GetFlowType(channel.FlowTypeID);
                lblCommandName.Text = outLetBLL.GetCommandNameByID(channel.ComndTypeID);
                ChannelDetails.ChannelTotalRDs = Convert.ToInt64(channel.TotalRDs);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}