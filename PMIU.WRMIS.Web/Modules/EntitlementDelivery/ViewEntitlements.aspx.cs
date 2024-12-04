using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class ViewEntitlements : BasePage
    {
        List<double> lstAccumulative = new List<double>();
        double MAF = 0;
        double Cusecs = 0;
        List<double> lstEntitlement = new List<double>();
        List<double> lstEntitlementCSForShortage = new List<double>();
        List<double> lstDeliveriesCSForShortage = new List<double>();
        List<double> lstDeliveriesCS = new List<double>();
        List<double> lstDifference = new List<double>();
        List<double> lstDeliveriesMAF = new List<double>();
        double AccumulativeDeliveries = 0;
        double BalanceEntitlement = 0;
        string MaskedValue = "";

        char ParentChild = 'P';
        long CommandID = 0;

        EntitlementDeliveryBLL bllEntdel = new EntitlementDeliveryBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
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
                    ParentChild = Convert.ToChar(Request.QueryString["ParentChild"]);

                    Label lblTenDailyID = (Label)e.Row.FindControl("lblTenDailyID");
                    Label lblEntitlementCs = (Label)e.Row.FindControl("lblEntitlementCs");
                    Label lblDeliveriesCs = (Label)e.Row.FindControl("lblDeliveriesCs");
                    Label lblDifferenceCs = (Label)e.Row.FindControl("lblDifferenceCs");
                    Label lblDeliveriesMAF = (Label)e.Row.FindControl("lblDeliveriesMAF");
                    Label lblAccumulativeDeliveriesMAF = (Label)e.Row.FindControl("lblAccumulativeDeliveriesMAF");
                    Label lblBalanceEntitlementMAF = (Label)e.Row.FindControl("lblBalanceEntitlementMAF");
                    Label lblShortage = (Label)e.Row.FindControl("lblShortage");                    

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

                        if (ParentChild == 'P')
                        {
                            Cusecs = Math.Round(Cusecs / 100) * 100;
                        }
                        else
                        {
                            Cusecs = Math.Round(Cusecs);
                        }

                        //MAF = Math.Round(MAF / 100) * 100;
                        lblEntitlementCs.Text = Convert.ToString(Cusecs);

                        lstEntitlementCSForShortage.Add(Cusecs);

                        //MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Convert.ToDouble(lblEntitlementCs.Text), ParentChild);
                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[0]);
                        //MAF = Math.Round(MAF, 3);

                        lstEntitlement.Add(MAF);
                    }

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        Cusecs = Convert.ToDouble(lblDeliveriesCs.Text);

                        if (ParentChild == 'P')
                        {
                            Cusecs = Math.Round(Cusecs / 100) * 100;
                        }
                        else
                        {
                            Cusecs = Math.Round(Cusecs);
                        }

                        lblDeliveriesCs.Text = Convert.ToString(Cusecs);

                        lstDeliveriesCSForShortage.Add(Cusecs);

                        //MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Convert.ToDouble(lblDeliveriesCs.Text), ParentChild);
                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[1]);
                        MAF = Math.Round(MAF, 3);

                        lstDeliveriesCS.Add(MAF);
                    }

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        double Difference = Convert.ToDouble(lblDeliveriesCs.Text) - Convert.ToDouble(lblEntitlementCs.Text);
                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        if (ParentChild == 'P')
                        {
                            Difference = Math.Round(Difference / 100) * 100;
                        }
                        else
                        {
                            Difference = Math.Round(Difference);
                        }

                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Difference, ParentChild);
                        MAF = Math.Round(MAF, 3);

                        lstDifference.Add(MAF);
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

                    if (lblDeliveriesCs.Text.Trim() != string.Empty && lblEntitlementCs.Text.Trim() != string.Empty && Convert.ToDouble(lblEntitlementCs.Text.Trim()) != 0)
                    {
                        double Entitlement = Convert.ToDouble(lblEntitlementCs.Text.Trim());
                        double Deliveries = Convert.ToDouble(lblDeliveriesCs.Text.Trim());

                        double Ratio = Deliveries / Entitlement;

                        //double Shortage = Math.Round((Ratio * 100), 2) - 100;
                        double Shortage = Math.Round((Ratio * 100) - 100, 2);

                        lblShortage.Text = String.Format("{0:0.0}", Shortage); //Shortage.ToString();
                    }
                    else
                    {
                        if (lblDeliveriesCs.Text.Trim() != string.Empty)
                        {
                            lblShortage.Text = "0";
                        }
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
                    Label ftrShortageEntitlement = (Label)e.Row.FindControl("ftrShortageEntitlement");


                    //ftrEntitlement.Text = Convert.ToString(lstEntitlement.Sum());
                    MAF = lstEntitlement.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrEntitlement.Text = MaskedValue;

                    //ftrDeliveriesCS.Text = Convert.ToString(lstDeliveriesCS.Sum());
                    MAF = lstDeliveriesCS.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesCS.Text = MaskedValue;

                    //ftrDifference.Text = Convert.ToString(lstDifference.Sum());
                    MAF = lstDifference.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDifference.Text = MaskedValue;

                    //ftrDeliveriesMAF.Text = Convert.ToString(lstDeliveriesMAF.Sum());
                    MAF = lstDeliveriesMAF.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesMAF.Text = MaskedValue;

                    //ftrAccumulativeDeliveries.Text = AccumulativeDeliveries.ToString();
                    MAF = AccumulativeDeliveries;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrAccumulativeDeliveries.Text = MaskedValue;

                    //ftrBalanceEntitlement.Text = BalanceEntitlement.ToString();
                    MAF = BalanceEntitlement;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrBalanceEntitlement.Text = MaskedValue;

                    //Shortage Entitlement w.r.t (%)
                    //ftrShortageEntitlement.Text = Math.Round((((lstDeliveriesCS.Sum() / lstEntitlement.Sum()) * 100) - 100), 2).ToString();
                    ftrShortageEntitlement.Text = String.Format("{0:0.0}", Math.Round((((lstDeliveriesCSForShortage.Sum() / lstEntitlementCSForShortage.Sum()) * 100) - 100), 2), 2);

                    if (ParentChild == 'P')
                    {
                        lblDeleveriesChange.Text = String.Format("{0:0.000}", Math.Round(((lstDeliveriesCS.Sum() / Convert.ToDouble(lbl7782Average.Text)) * 100) - 100, 3));
                    }

                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ParentChild == 'C')
                    {
                        e.Row.Cells[5].Text = "Deliveries (1000 AF)";
                        e.Row.Cells[6].Text = "Accumulative Deliveries (1000 AF)";
                        e.Row.Cells[7].Text = "Balance Entitlement (1000 AF)";
                    }
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
            ParentChild = Convert.ToChar(Request.QueryString["ParentChild"]);
            CommandID = Convert.ToInt64(Request.QueryString["CommandID"]);


            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            if (SeasonID == (long)Constants.Seasons.Rabi)
            {
                long IncrementedYEarYear = Year + 1;
                //CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                if (ParentChild == 'P')
                {
                    ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);//(chnl.ID);

                    lblEntitlementText.Text = "Entitlement for Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF):";
                    lblMainDesc.Text = "Entitlement and Actual Distribution of " + mdlCommandChannel.ChannelName + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF)"; //chnl.NAME + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF)";

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
                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year));

                    List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
                    double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                    Average7782 = Math.Round(Average7782, 3);
                    MaskedValue = String.Format("{0:0.000}", Average7782);
                    lbl7782Average.Text = MaskedValue;

                    double PercentChange = ((Convert.ToDouble(lblEntitlement.Text) / Average7782) * 100) - 100;
                    lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();
                    //lblDeleveriesChange.Text = 
                    //if (CommandID == 1)
                    //{
                    //    double Para2 = bllEntitlementDelivery.GetPara2ChannelAverageMAF(lstSeasonIDs, chnl.ID);
                    //    lblPara2.Text = (Math.Round(Para2, 3)).ToString();
                    //    lblPara2Text.Text = "Punjab Para(2) Rabi Share (MAF):";
                    //}
                    //else
                    //{
                    //    lblPara2.Visible = false;
                    //    lblPara2Text.Visible = false;
                    //}
                }
                else
                {
                    CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);
                    lblEntitlementText.Text = "Entitlement for Rabi" + " " + Year + "-" + IncrementedYEarYear + " (1000 AF):";
                    lblMainDesc.Text = "Entitlement and Actual Distribution of " + chnl.NAME + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (1000 AF)";
                    CO_ChannelGauge mdlChannelGauge = bllEntitlementDelivery.GetChannelGaugeByChannelID(ChannelID);//(chnl.ID);
                    lblDesignDischarge.Text = Convert.ToString(mdlChannelGauge.DesignDischarge);

                    MAF = bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);

                    lblEntitlement.Text = MaskedValue; //Convert.ToString(bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year));
                    lbl7782AverageText.Visible = false;
                    lblPercentChangeText.Visible = false;
                    lblDeleveriesChangeText.Visible = false;
                    //lblPara2.Visible = false;
                    //lblPara2Text.Visible = false;
                }

            }
            else if (SeasonID == (long)Constants.Seasons.Kharif)
            {
                //CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                if (ParentChild == 'P')
                {
                    ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);//(chnl.ID);

                    lblMainDesc.Text = "Entitlement and Actual Distribution of " + mdlCommandChannel.ChannelName + " Kharif" + " " + Year + " (MAF)";
                    lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (MAF):";

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
                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year));

                    List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
                    double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                    Average7782 = Math.Round(Average7782, 3);
                    MaskedValue = String.Format("{0:0.000}", Average7782);
                    lbl7782Average.Text = MaskedValue;

                    double PercentChange = ((MAF / Average7782) * 100) - 100;
                    lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();

                    //if (CommandID == 1)
                    //{
                    //    double Para2 = bllEntitlementDelivery.GetPara2ChannelAverageMAF(lstSeasonIDs, chnl.ID);
                    //    lblPara2.Text = (Math.Round(Para2, 3)).ToString();
                    //    lblPara2Text.Text = "Punjab Para(2) Kharif Share (MAF):";
                    //}
                    //else
                    //{
                    //    lblPara2.Visible = false;
                    //    lblPara2Text.Visible = false;
                    //}
                }
                else
                {
                    CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                    lblMainDesc.Text = "Entitlement and Actual Distribution of " + chnl.NAME + " Kharif" + " " + Year + " (1000 AF)";
                    lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (1000 AF):";
                    CO_ChannelGauge mdlChannelGauge = bllEntitlementDelivery.GetChannelGaugeByChannelID(chnl.ID);
                    lblDesignDischarge.Text = Convert.ToString(mdlChannelGauge.DesignDischarge);

                    MAF = bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);

                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year));
                    lbl7782AverageText.Visible = false;
                    lblPercentChangeText.Visible = false;
                    lblDeleveriesChangeText.Visible = false;
                    //lblPara2.Visible = false;
                    //lblPara2Text.Visible = false;
                }
            }

            if (ParentChild == 'P')
            {
                List<dynamic> lstEntitlements = bllEntitlementDelivery.ViewEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);
                gvEntitlements.DataSource = lstEntitlements;
                gvEntitlements.DataBind();
            }
            else
            {
                List<dynamic> lstEntitlements = bllEntitlementDelivery.ViewChildEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);
                gvEntitlements.DataSource = lstEntitlements;
                gvEntitlements.DataBind();
            }

        }
    }
}