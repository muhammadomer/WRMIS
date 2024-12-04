using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls
{
    public partial class ACCPXChannels : System.Web.UI.UserControl
    {
        private long ACCP_ID = 0;
        private long Channel_ID = 0;

        public long ACCPID { get { return ACCP_ID; } set { ACCP_ID = value; } }
        public long ChannelID { get { return Channel_ID; } set { Channel_ID = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetLabels(ACCP_ID, Channel_ID);
        }


        private void SetLabels(long acpID=0,long channelID=0)
        {
            CW_AnnualClosureProgram accp = new ClosureOperationsBLL().GetACCP_ByID(acpID);
            CO_Channel chnl = new ChannelBLL().GetChannelByID(channelID);


            if(acpID>0 && channelID>0)
            {
                if (accp != null && accp.ID > 0)
                {
                    lblACCP.Text = "Annual Canal Closure Programme   " + accp.Year;
                    lblACCPYear.Text = accp.Year;
                    lblChannelName.Text = chnl.NAME;
                    lblCommandName.Text=Convert.ToInt64(Constants.Commands.IndusCommand)==chnl.ComndTypeID ? "Terbela Command" : "Mangla Command";
                }
            }

            
        }
    }
}