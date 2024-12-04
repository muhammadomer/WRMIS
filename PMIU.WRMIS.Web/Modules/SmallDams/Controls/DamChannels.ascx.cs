using System;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.SmallDams.DamSearch;

namespace PMIU.WRMIS.Web.Modules.SmallDams.Controls
{
    public partial class DamChannels : System.Web.UI.UserControl
    {
        public static long? _DAMID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_DAMID!=null)
                {
                    LoadDamChannels(_DAMID);
                    _DAMID = null;
                }
                
                
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadDamChannels(Int64? _ID)
        {
            try
            {
                object _DamChannels = new SmallDamsBLL().GetDamChannels(_ID);
                if (_DamChannels != null)
                {



                    ChannelText.Text = Utility.GetDynamicPropertyValue(_DamChannels, "Channel");
                    ChannelCapacityText.Text = Utility.GetDynamicPropertyValue(_DamChannels, "ChannelCapacity");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}