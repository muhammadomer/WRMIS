using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class RiversAndCanalsDischarge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // MapSiteDischarge();

            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void MapSiteDischarge(DateTime siteDate)
        {
            //Convert.ToDateTime("2017-08-23")
            DateTime readingDate = siteDate;
            List<dynamic> lstgss = new List<dynamic>();
            lstgss = new DailyDataBLL().GetAllGaugeSlipSiteDateForDiagram(readingDate);
            if (lstgss.Count == 0)
            {
                ResetLablesValues();
                return;
            }
            string siteValue = "";
            string Discharge_, Indent_, AFSQ_, dailyGauge_;
            int Discharge_Lcc, Indent_Lcc, AFSQ_Lcc;
            Discharge_Lcc = 0;
            Indent_Lcc = 0;
            AFSQ_Lcc = 0;
            Discharge_ = "";
            dailyGauge_ = "";
            foreach (object item in lstgss)
            {

                int siteID = Convert.ToInt32(item.GetType().GetProperty("ID").GetValue(item));
                int Discharge = Convert.ToInt32(item.GetType().GetProperty("DailyDischarge").GetValue(item));
                int Indent = Convert.ToInt32(item.GetType().GetProperty("DailyIndent").GetValue(item));
                int AFSQ = Convert.ToInt32(item.GetType().GetProperty("AFSQ").GetValue(item));
                double dailyGauge = Convert.ToDouble(item.GetType().GetProperty("DailyGauge").GetValue(item));

                if (siteID == 63 || siteID == 68)
                {
                    Discharge_Lcc = Discharge_Lcc + Discharge;
                    Indent_Lcc = Indent_Lcc + Indent;
                    AFSQ_Lcc = AFSQ_Lcc + AFSQ;
                }


                Discharge_ = string.Format("{0:#,##0}", Discharge);
                Indent_ = string.Format("{0:#,##0}", Indent);
                AFSQ_ = string.Format("{0:#,##0}", AFSQ);
                dailyGauge_ = string.Format("{0:#,##0.00}", dailyGauge);
                if (siteID == 159)
                {

                }
                if (IsThreePair(Convert.ToString(siteID)))
                {
                    Discharge_ = Discharge_ == "0" ? "NIL" : Discharge_;
                    Indent_ = Indent_ == "0" ? "NIL" : Indent_;
                    AFSQ_ = AFSQ_ == "0" ? "NIL" : AFSQ_;
                    siteValue = Discharge_ + "  (" + Indent_ + ")" + "(" + AFSQ_ + ")";
                }
                else
                {
                    if (siteID == 1 || siteID == 26)
                    {
                        siteValue = dailyGauge_;
                    }
                    else
                    {
                        siteValue = Discharge_;
                    }

                }
                MatchSite(siteID, siteValue);
            }
            //Lcc Total Count
            Discharge_ = string.Format("{0:#,##0}", Discharge_Lcc);
            Indent_ = string.Format("{0:#,##0}", Indent_Lcc);
            AFSQ_ = string.Format("{0:#,##0}", AFSQ_Lcc);
            siteValue = Discharge_ + "  (" + Indent_ + ")" + "(" + AFSQ_ + ")";
            MatchSite(0, siteValue);
        }
        public bool IsThreePair(string ID)
        {
            string value = "99,77,115,153,119,111,94,15,16,156,154,151,113,12,21,22,159,85,86,87,34,39,40,48,45,79,74,75,97,98,128,129,132,127,161,126,100,102,106,108,109,93,124,122,67,68,158,63,52,114,54,58,57";
            string[] siteIDArray = value.Split(',');
            if (siteIDArray.Contains(ID))
            {
                return true;
            }
            return false;
        }
        public void MatchSite(int siteID, string siteValue)
        {
            switch (siteID)
            {
                //Lcc_ Total
                case 0:
                    lcc_total.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 1:
                    tarbela.Text = siteValue == "0" ? "NIL" : siteValue + " ft";
                    break;
                case 2:
                    tarbela_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 3:
                    tarbela_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 10:
                    kalabagh_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 11:
                    kalabagh_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 12:
                    thal.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 13:
                    chashma_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 14:
                    chashma_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 15:
                    cbrc_head.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 16:
                    cbrc_pb_rd.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;

                case 19:
                    taunsa_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 20:
                    taunsa_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 21:
                    tplink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 22:
                    muzaffargarh.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 24:
                    guddu_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 25:
                    guddu_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 26:
                    mangla.Text = siteValue == "0" ? "NIL" : siteValue + " ft";
                    break;
                case 28:
                    mangla_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 29:
                    mangla_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 35:
                    ujc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 39:
                    rpc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 40:
                    ujc_int.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 42:
                    rasul_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 43:
                    rasul_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 45:
                    lower_jehlum.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 48:
                    rqlink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 50:
                    marala_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 51:
                    marala_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 52:
                    ucc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 54:
                    mll.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 57:
                    mrlink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 58:
                    mrint.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 61:
                    khanki_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 62:
                    khanki_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 63:
                    lcc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 65:
                    qadirabad_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 66:
                    qadirabad_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 67:
                    qblink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 68:
                    lcc_feeder.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 72:
                    trimmu_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 73:
                    trimmu_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 74:
                    havali.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 75:
                    havali_int.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 77:
                    rangpur.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 79:
                    tslink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 80:
                    kotri_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 81:
                    kotri_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 83:
                    punjnad_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 84:
                    punjnad_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 85:
                    punjnad.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 86:
                    abbasia.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 87:
                    abbasia_link.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 91:
                    balloki_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 92:
                    balloki_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 93:
                    lbdc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 94:
                    bslink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 95:
                    sidhnal_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 96:
                    sidhnal_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 98:
                    sidhnal.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 99:
                    upakpattan.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 100:
                    sadiqia.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 102:
                    fardwah.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 103:
                    islam_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 104:
                    islam_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 105:
                    ubc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 106:
                    qaim.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                // Kabul ID 107
                case 107:
                    kabul.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 108:
                    mplink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 109:
                    pilink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 111:
                    bslink_2.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 113:
                    greater_thal.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 114:
                    brbdlink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 119:
                    udc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 122:
                    bslink_1.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 124:
                    ldc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 127:
                    rd161.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 97:
                    smblink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 128:
                    pakpattan.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 129:
                    lmailsi.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 132:
                    smlink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 133:
                    sukkur_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 134:
                    sukkur_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 151:
                    cjlink.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 153:
                    cbdc.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 154:
                    dgkhan.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 156:
                    kachhi.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 158:
                    uccint.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 159:
                    rtp.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 161:
                    lalsohara.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 162:
                    skardu.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 163:
                    bunji.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 164:
                    bisham.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 8:
                    sulemanki_top.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                case 9:
                    sulemanki_bot.Text = siteValue == "0" ? "NIL" : siteValue;
                    break;
                default:
                    break;
            }
        }
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            DateTime siteDate = Convert.ToDateTime(txtFromDate.Text);
            MapSiteDischarge(siteDate);
            DiagramSec.Visible = true;
            dated.Text = ":   " + Utility.GetFormattedDate(siteDate);
        }

        public void ResetLablesValues()
        {
            skardu.Text = "NIL";
            bunji.Text = "NIL";
            bisham.Text = "NIL";
            tarbela.Text = "NIL";
            tarbela_top.Text = "NIL";
            tarbela_bot.Text = "NIL";
            kabul.Text = "NIL";
            kalabagh_top.Text = "NIL";
            kalabagh_bot.Text = "NIL";
            chashma_top.Text = "NIL";
            chashma_bot.Text = "NIL";
            cbrc_head.Text = "NIL (NIL)(NIL)";
            cbrc_pb_rd.Text = "NIL (NIL)(NIL)";
            taunsa_top.Text = "NIL";
            taunsa_bot.Text = "NIL";
            kachhi.Text = "NIL (NIL)(NIL)";
            dgkhan.Text = "NIL (NIL)(NIL)";
            guddu_top.Text = "NIL";
            guddu_bot.Text = "NIL";
            sukkur_top.Text = "NIL";
            sukkur_bot.Text = "NIL";
            kotri_top.Text = "NIL";
            kotri_bot.Text = "NIL";
            thal.Text = "NIL (NIL)(NIL)";
            cjlink.Text = "NIL (NIL)(NIL)";
            greater_thal.Text = "NIL (NIL)(NIL)";
            tplink.Text = "NIL (NIL)(NIL)";
            muzaffargarh.Text = "NIL (NIL)(NIL)";
            mangla.Text = "NIL";
            mangla_top.Text = "NIL";
            mangla_bot.Text = "NIL";
            rasul_top.Text = "NIL";
            rasul_bot.Text = "NIL";
            trimmu_top.Text = "NIL";
            trimmu_bot.Text = "NIL";
            rangpur.Text = "NIL (NIL)(NIL)";
            rtp.Text = "NIL (NIL)(NIL)";
            punjnad_top.Text = "NIL";
            punjnad_bot.Text = "NIL";
            punjnad.Text = "NIL (NIL)(NIL)";
            abbasia.Text = "NIL (NIL)(NIL)";
            abbasia_link.Text = "NIL (NIL)(NIL)";
            ujc.Text = "NIL (NIL)(NIL)";
            rpc.Text = "NIL (NIL)(NIL)";
            ujc_int.Text = "NIL (NIL)(NIL)";
            rqlink.Text = "NIL (NIL)(NIL)";
            lower_jehlum.Text = "NIL (NIL)(NIL)";
            tslink.Text = "NIL (NIL)(NIL)";
            havali.Text = "NIL (NIL)(NIL)";
            havali_int.Text = "NIL (NIL)(NIL)";
            marala_top.Text = "NIL";
            marala_bot.Text = "NIL";
            khanki_top.Text = "NIL";
            khanki_bot.Text = "NIL";
            qadirabad_top.Text = "NIL";
            qadirabad_bot.Text = "NIL";
            sidhnal_top.Text = "NIL";
            sidhnal_bot.Text = "NIL";
            sidhnal.Text = "NIL (NIL)(NIL)";
            pakpattan.Text = "NIL (NIL)(NIL)";
            lmailsi.Text = "NIL (NIL)(NIL)";
            smlink.Text = "NIL (NIL)(NIL)";
            rd161.Text = "NIL (NIL)(NIL)";
            lalsohara.Text = "NIL (NIL)(NIL)";
            smblink.Text = "NIL (NIL)(NIL)";
            mrlink.Text = "NIL (NIL)(NIL)";
            mrint.Text = "NIL (NIL)(NIL)";
            brbdlink.Text = "NIL (NIL)(NIL)";
            ucc.Text = "NIL (NIL)(NIL)";
            mll.Text = "NIL (NIL)(NIL)";
            uccint.Text = "NIL (NIL)(NIL)";
            lcc.Text = "NIL (NIL)(NIL)";
            qblink.Text = "NIL (NIL)(NIL)";
            lcc_total.Text = "NIL (NIL)(NIL)";
            lcc_feeder.Text = "NIL (NIL)(NIL)";
            balloki_top.Text = "NIL";
            balloki_bot.Text = "NIL";
            lbdc.Text = "NIL (NIL)(NIL)";
            mplink.Text = "NIL (NIL)(NIL)";
            pilink.Text = "NIL (NIL)(NIL)";
            qaim.Text = "NIL (NIL)(NIL)";
            ubc.Text = "NIL (NIL)(NIL)";
            islam_top.Text = "NIL";
            islam_bot.Text = "NIL";
            cbdc.Text = "NIL (NIL)(NIL)";
            brbd_int.Text = "NIL (NIL)(NIL)";
            udc.Text = "NIL (NIL)(NIL)";
            bslink.Text = "NIL (NIL)(NIL)";
            bslink_2.Text = "NIL (NIL)(NIL)";
            bslink_1.Text = "NIL (NIL)(NIL)";
            ldc.Text = "NIL (NIL)(NIL)";
            upakpattan.Text = "NIL (NIL)(NIL)";
            fardwah.Text = "NIL (NIL)(NIL)";
            sadiqia.Text = "NIL (NIL)(NIL)";
            sulemanki_top.Text = "NIL";
            sulemanki_bot.Text = "NIL";
        }

    }
}