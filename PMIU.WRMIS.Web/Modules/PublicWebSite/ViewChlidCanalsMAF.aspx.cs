using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class ViewChlidCanalsMAF : System.Web.UI.Page
    {
        public string ChannelName = "";
        long CommandID = 0;
        long MainChannelID = 0;
        long SeasonID = 0;
        long Year = 0;
        List<long> lstSeasonIDs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ChannelID"] != null)
                    MainChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);

                if (Request.QueryString["SeasonID"] != null)
                    SeasonID = Convert.ToInt64(Request.QueryString["SeasonID"]);

                if (Request.QueryString["Year"] != null)
                    Year = Convert.ToInt64(Request.QueryString["Year"]);

                if (Request.QueryString["CommandID"] != null)
                    CommandID = Convert.ToInt64(Request.QueryString["CommandID"]);

                if (Request.QueryString["Channel"] != null)
                    ChannelName = Convert.ToString(Request.QueryString["Channel"]);

                lstSeasonIDs = new List<long>();
                lstSeasonIDs = GetSeasonIDs(SeasonID);
                BindGrid(MainChannelID, lstSeasonIDs, Year, CommandID, ChannelName.Trim().ToLower());
            }
        }

        public void BindGrid(long _MainChannelID, List<long> _lstSeasonID, long _Year, long _CommandID, string _Name)
        {
            List<dynamic> lstChanneles = new EntitlementDeliveryBLL().GetChildChannelByMainChannel(_MainChannelID, _lstSeasonID, _Year, _CommandID);
            gvChildChannels.DataSource = lstChanneles;
            gvChildChannels.DataBind();

            if (lstChanneles.Count() == 1)  // when command channel is also main channel
            {
                //long ChannelID = 0;
                //string Name = "";
                //dynamic obj = lstChanneles.FirstOrDefault();
                //ChannelID = Convert.ToInt64(obj.GetType().GetProperty("ChannelID").GetValue(obj));
                //Name = Convert.ToString(obj.GetType().GetProperty("ChannelName").GetValue(obj));
                //Name = Name.Trim().ToLower();

                //if (_Name == Name)
                //{
                //    List<dynamic> lstChannels = new EntitlementDeliveryBLL().GetChildChannelFromParentFeederByChannelID(ChannelID, lstSeasonIDs, Year, CommandID);
                //    gvChildChannels.DataSource = lstChannels;
                //    gvChildChannels.DataBind();
                //}

                long ChannelID = 0;
                dynamic obj = lstChanneles.FirstOrDefault();
                ChannelID = Convert.ToInt64(obj.GetType().GetProperty("ChannelID").GetValue(obj));
                long MainChannelID = new WaterLossesBLL().GetCommandChannelID(_MainChannelID);
                //if (_Name == Name)
                if (MainChannelID == ChannelID)
                {
                    List<dynamic> lstChannels = new EntitlementDeliveryBLL().GetChildChannelFromParentFeederByChannelID(ChannelID, lstSeasonIDs, Year, CommandID);
                    gvChildChannels.DataSource = lstChannels;
                    gvChildChannels.DataBind();
                }
            }
        }

        protected void gvChildChannels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MoreChild")
            {
                GridViewRow gvrow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                ViewState["RejectedRowIndex"] = gvrow.RowIndex;
                long ChannelID = Convert.ToInt64(GetDataKeyValue(gvChildChannels, "ChannelID", gvrow.RowIndex));

                long SeasonID = Convert.ToInt64(GetDataKeyValue(gvChildChannels, "Season", gvrow.RowIndex));
                long Year = Convert.ToInt64(GetDataKeyValue(gvChildChannels, "Year", gvrow.RowIndex));
                long CommandID = Convert.ToInt64(GetDataKeyValue(gvChildChannels, "CommandID", gvrow.RowIndex));
                lstSeasonIDs = new List<long>();
                lstSeasonIDs = GetSeasonIDs(SeasonID);
                List<dynamic> lstChanneles = new EntitlementDeliveryBLL().GetChildChannelFromParentFeederByChannelID(ChannelID, lstSeasonIDs, Year, CommandID);
                gvChildChannels.DataSource = lstChanneles;
                gvChildChannels.DataBind();
            }

        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        public List<long> GetSeasonIDs(long SeasonID)
        {
            List<long> lstSeasonIDs = new List<long>(); ;
            if (SeasonID == 1)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
            }
            else if (SeasonID == 2)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
            }
            return lstSeasonIDs;
        }
    }
}