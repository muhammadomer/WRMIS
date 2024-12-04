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
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls
{
    public partial class ChannelOutletAlterationDetails : System.Web.UI.UserControl
    {
        CO_ChannelOutlets OutletData = new CO_ChannelOutlets();
        CO_ChannelOutletsLocation OutletLoc = new CO_ChannelOutletsLocation();
        IN_OutletBLL OutletBLL = new IN_OutletBLL();
        public static long ChannelID { get; set; }
        public static long OutletID { get; set; }
        public static long ChannelTotalRDs { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            long outletID = 0;
            try
            {
                long.TryParse(Convert.ToString(ChannelOutletAlterationDetails.ChannelID), out channelID);
                long.TryParse(Convert.ToString(ChannelOutletAlterationDetails.OutletID), out outletID);
                LoadChannelDetails(channelID, outletID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadChannelDetails(long _ChannelID, long _OutletID)
        {
            try
            {
                LoadChannelData(_ChannelID);
                LoadOutletData(_ChannelID, _OutletID);
                LoadLocationData(_ChannelID, _OutletID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadChannelData(long _ChannelID)
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
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadOutletData(long _ChannelID, long _OutletID)
        {
            try
            {
                dynamic channelSide = null;

                OutletData = OutletBLL.GetOutletByOutletID(_OutletID); 
                OutletAlteration._OutletRD = OutletData.OutletRD;
                OutletAlteration._ChannelSide = OutletData.ChannelSide;
                lblOutletRD.Text = Calculations.GetRDText(OutletData.OutletRD);
                channelSide = CommonLists.GetOutletSides(OutletData.ChannelSide)[0];

                if (channelSide != null)
                    lblOutletSide.Text = Convert.ToString(channelSide.GetType().GetProperty("Name").GetValue(channelSide, null));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadLocationData(long _ChannelID, long _OutletID)
        {
            try
            {
                // Get Outlet-Location record for the given Outlet ID
                OutletLoc = OutletBLL.GetVLGIDbyOutletID(_OutletID);

                if (OutletLoc != null)
                {
                    // Get Village Name from Village Table by Village ID stored in Locations Table
                    CO_Village mdlVillage = OutletBLL.GetVillageNameByID(OutletLoc.VillageID);

                    string VillageName = mdlVillage.Name;
                    lblVillage.Text = VillageName;

                    // Get Police Station Name from Police Station Table by PS ID stored in Village Table
                    string PoliceStationName = OutletBLL.GetPStationNameByID(mdlVillage.PoliceStationID);
                    lblPoliceStation.Text = PoliceStationName;

                    // Get Tehsil by ID stored in Village entity
                    CO_Tehsil mdlTehsil = OutletBLL.GetTehsilNameByID(mdlVillage.TehsilID);
                    lblTehsil.Text = mdlTehsil.Name;

                    // Get Districts by ID stored in Tehsil entity
                    CO_District mdlDistrict = OutletBLL.GetDistrictNameByID(mdlTehsil.DistrictID);
                    lblDistrict.Text = mdlDistrict.Name;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}