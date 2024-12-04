using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class ViewEntitlement : System.Web.UI.Page
    {
        List<double> lstAccumulative = new List<double>();
        double MAF = 0;
        double Cusecs = 0;
        List<double> lstEntitlement = new List<double>();
        List<double> lstDeliveriesCS = new List<double>();
        List<double> lstDifference = new List<double>();
        List<double> lstDeliveriesMAF = new List<double>();
        double AccumulativeDeliveries = 0;
        double BalanceEntitlement = 0;
        string MaskedValue = "";

        long CommandID = 0;

        EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEntitlements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    long ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                    long SeasonID = Convert.ToInt64(Request.QueryString["SeasonID"]);
                    long Year = Convert.ToInt64(Request.QueryString["Year"]);

                    Label lblTenDailyID = (Label)e.Row.FindControl("lblTenDailyID");
                    Label lblEntitlementCs = (Label)e.Row.FindControl("lblEntitlementCs");
                    Label lblDeliveriesCs = (Label)e.Row.FindControl("lblDeliveriesCs");
                    Label lblDifferenceCs = (Label)e.Row.FindControl("lblDifferenceCs");
                    Label lblDeliveriesMAF = (Label)e.Row.FindControl("lblDeliveriesMAF");
                    Label lblAccumulativeDeliveriesMAF = (Label)e.Row.FindControl("lblAccumulativeDeliveriesMAF");
                    Label lblBalanceEntitlementMAF = (Label)e.Row.FindControl("lblBalanceEntitlementMAF");

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        double Difference = Convert.ToDouble(lblDeliveriesCs.Text) - Convert.ToDouble(lblEntitlementCs.Text);
                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        Difference = Math.Round(Difference / 100) * 100;

                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        MAF = bllEntitlementDelivery.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Difference, 'P');
                        MAF = Math.Round(MAF, 3);

                        lstDifference.Add(MAF);
                    }

                    if (lblDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblDeliveriesMAF.Text);
                        MAF = Math.Round(MAF, 3);

                        lstAccumulative.Add(MAF);

                        lblAccumulativeDeliveriesMAF.Text = Convert.ToString(lstAccumulative.Sum());

                        if (e.Row.RowIndex == 0)
                        {
                            if (lblEntitlement.Text != string.Empty)
                                lblBalanceEntitlementMAF.Text = Convert.ToString(Convert.ToDouble(lblEntitlement.Text) - Convert.ToDouble(lblDeliveriesMAF.Text));
                        }
                        else
                        {
                            double MAFofChannel = Convert.ToDouble(((Label)gvEntitlements.Rows[gvEntitlements.Rows.Count - 1].Cells[7].Controls[1]).Text);
                            lblBalanceEntitlementMAF.Text = (MAFofChannel - Convert.ToDouble(lblDeliveriesMAF.Text)).ToString();
                        }
                    }

                    if (lblEntitlementCs.Text != string.Empty)
                    {
                        Cusecs = Convert.ToDouble(lblEntitlementCs.Text);

                        Cusecs = Math.Round(Cusecs / 100) * 100;

                        lblEntitlementCs.Text = Convert.ToString(Cusecs);

                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[0]);
                        MAF = Math.Round(MAF, 3);

                        lstEntitlement.Add(MAF);
                    }

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        Cusecs = Convert.ToDouble(lblDeliveriesCs.Text);

                        Cusecs = Math.Round(Cusecs / 100) * 100;

                        lblDeliveriesCs.Text = Convert.ToString(Cusecs);

                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[1]);
                        MAF = Math.Round(MAF, 3);

                        lstDeliveriesCS.Add(MAF);
                    }

                    if (lblDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblDeliveriesMAF.Text);

                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblDeliveriesMAF.Text = MaskedValue;

                        lstDeliveriesMAF.Add(MAF);
                    }

                    if (lblAccumulativeDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblAccumulativeDeliveriesMAF.Text);
                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblAccumulativeDeliveriesMAF.Text = MaskedValue;

                        AccumulativeDeliveries = MAF;
                    }

                    if (lblBalanceEntitlementMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblBalanceEntitlementMAF.Text);
                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblBalanceEntitlementMAF.Text = MaskedValue;

                        BalanceEntitlement = MAF;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label ftrEntitlement = (Label)e.Row.FindControl("ftrEntitlement");
                    Label ftrDeliveriesCS = (Label)e.Row.FindControl("ftrDeliveriesCS");
                    Label ftrDifference = (Label)e.Row.FindControl("ftrDifference");
                    Label ftrDeliveriesMAF = (Label)e.Row.FindControl("ftrDeliveriesMAF");
                    Label ftrAccumulativeDeliveries = (Label)e.Row.FindControl("ftrAccumulativeDeliveries");
                    Label ftrBalanceEntitlement = (Label)e.Row.FindControl("ftrBalanceEntitlement");

                    MAF = lstEntitlement.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrEntitlement.Text = MaskedValue;

                    MAF = lstDeliveriesCS.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesCS.Text = MaskedValue;

                    MAF = lstDifference.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDifference.Text = MaskedValue;

                    MAF = lstDeliveriesMAF.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesMAF.Text = MaskedValue;

                    MAF = AccumulativeDeliveries;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrAccumulativeDeliveries.Text = MaskedValue;

                    MAF = BalanceEntitlement;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrBalanceEntitlement.Text = MaskedValue;
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            long ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
            long SeasonID = Convert.ToInt64(Request.QueryString["SeasonID"]);
            long Year = Convert.ToInt64(Request.QueryString["Year"]);
            CommandID = Convert.ToInt64(Request.QueryString["CommandID"]);

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            if (SeasonID == (long)Constants.Seasons.Rabi)
            {
                long IncrementedYEarYear = Year + 1;
                //CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);//(chnl.ID);

                lblEntitlementText.Text = "Entitlement for Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF):";
                lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + mdlCommandChannel.ChannelName + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF)";

                if (mdlCommandChannel != null)
                {
                    lblDesignDischarge.Text = Convert.ToString(mdlCommandChannel.DesignDischargeRabi);
                }
                else
                {
                    lblDesignDischarge.Text = "0";
                }

                MAF = bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                MAF = Math.Round(MAF, 3);
                MaskedValue = String.Format("{0:0.000}", MAF);
                lblEntitlement.Text = MaskedValue;

                List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
                double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                Average7782 = Math.Round(Average7782, 3);
                MaskedValue = String.Format("{0:0.000}", Average7782);
                lbl7782Average.Text = MaskedValue;

                double PercentChange = 0;

                if (Average7782 != 0)
                {
                    PercentChange = ((Convert.ToDouble(lblEntitlement.Text) / Average7782) * 100) - 100;
                }

                lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();

                double Para2 = bllEntitlementDelivery.GetPara2ChannelAverageMAF(lstSeasonIDs, ChannelID);
                lblPara2.Text = (Math.Round(Para2, 3)).ToString();
                lblPara2Text.Text = "Punjab Para(2) Rabi Share (MAF):";
            }
            else if (SeasonID == (long)Constants.Seasons.Kharif)
            {
                //CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);//(chnl.ID);

                lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + mdlCommandChannel.ChannelName + " Kharif" + " " + Year + " (MAF)";
                lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (MAF):";

                //CO_ChannelGauge mdlChannelGauge = bllEntitlementDelivery.GetChannelGaugeByChannelID(chnl.ID);

                if (mdlCommandChannel != null)
                {
                    lblDesignDischarge.Text = Convert.ToString(mdlCommandChannel.DesignDischarge);
                }
                else
                {
                    lblDesignDischarge.Text = "0";
                }

                MAF = bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                MAF = Math.Round(MAF, 3);
                MaskedValue = String.Format("{0:0.000}", MAF);
                lblEntitlement.Text = MaskedValue;

                List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
                double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                Average7782 = Math.Round(Average7782, 3);
                MaskedValue = String.Format("{0:0.000}", Average7782);
                lbl7782Average.Text = MaskedValue;

                double PercentChange = ((MAF / Average7782) * 100) - 100;
                lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();

                lblPara2.Visible = false;
                lblPara2Text.Visible = false;
            }

            List<dynamic> lstEntitlements = bllEntitlementDelivery.ViewEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);
            gvEntitlements.DataSource = lstEntitlements;
            gvEntitlements.DataBind();
        }
    }
}