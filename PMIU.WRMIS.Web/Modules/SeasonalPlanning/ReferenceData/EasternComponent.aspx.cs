using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class EasternComponent : BasePage
    {
        #region Variables

        public decimal Sulemanki = 0;
        public decimal SulemankiLK = 0;
        public decimal BS1 = 0;
        public decimal BS1LK = 0;
        public decimal BS2 = 0;
        public decimal BS2LK = 0;
        public decimal Balloki = 0;
        public decimal BallokiLK = 0;
        public decimal UCC = 0;
        public decimal UCCLK = 0;
        public decimal MR = 0;
        public decimal MRLK = 0;
        public decimal QB = 0;
        public decimal QBLK = 0;
        public decimal Eastern = 0;
        public decimal EasternLK = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    BindSeasonDropDown();
                SetTitle();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindSeasonDropDown()
        {
            try
            {
                //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                //ddlSeason.DataTextField = "Name";
                //ddlSeason.DataValueField = "ID";
                //ddlSeason.DataBind();
                Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EasterComponent);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value != "" && ddlYear.SelectedItem.Value != "")
                {
                    long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                    string SYear = Convert.ToString(ddlYear.SelectedItem.Value);
                    SYear = SYear.Substring(0, 4);
                    BindGrid(SeasonID, SYear);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvEasternComponent.Visible = false;
                if (ddlSeason.SelectedItem.Value != "")
                {
                    ddlYear.Enabled = true;
                    ddlYear.ClearSelection();
                    ddlYear.DataSource = new SeasonalPlanningBLL().GetCalendarYears(Convert.ToInt64(ddlSeason.SelectedItem.Value));
                    ddlYear.DataTextField = "Year";
                    ddlYear.DataValueField = "Year";
                    ddlYear.DataBind();
                }
                else
                    ddlYear.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvEasternComponent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbTDailyID = (Label)e.Row.FindControl("lblID");
                    Label lbSulemanki = (Label)e.Row.FindControl("lblSulemanki");
                    Label lbBS1 = (Label)e.Row.FindControl("lblBS1");
                    Label lbBS2 = (Label)e.Row.FindControl("lblBS2");
                    Label lbBalloki = (Label)e.Row.FindControl("lblBalloki");
                    Label lbUCC = (Label)e.Row.FindControl("lblUCC");
                    Label lbMR = (Label)e.Row.FindControl("lblMR");
                    Label lbQB = (Label)e.Row.FindControl("lblQB");
                    Label lbEastern = (Label)e.Row.FindControl("lblEastern");

                    int TDailyID = Convert.ToInt32(lbTDailyID.Text);

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        if (TDailyID >= (int)Constants.SeasonDistribution.EKStart && TDailyID <= (int)Constants.SeasonDistribution.EKEnd)  // Represents Early Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                Sulemanki += Convert.ToDecimal(lbSulemanki.Text) * Constants.TDailyFactor;
                                BS1 += Convert.ToDecimal(lbBS1.Text) * Constants.TDailyFactor;
                                BS2 += Convert.ToDecimal(lbBS2.Text) * Constants.TDailyFactor;
                                Balloki += Convert.ToDecimal(lbBalloki.Text) * Constants.TDailyFactor;
                                UCC += Convert.ToDecimal(lbUCC.Text) * Constants.TDailyFactor;
                                MR += Convert.ToDecimal(lbMR.Text) * Constants.TDailyFactor;
                                QB += Convert.ToDecimal(lbQB.Text) * Constants.TDailyFactor;
                                Eastern += Convert.ToDecimal(lbEastern.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                Sulemanki += Convert.ToDecimal(lbSulemanki.Text);
                                BS1 += Convert.ToDecimal(lbBS1.Text);
                                BS2 += Convert.ToDecimal(lbBS2.Text);
                                Balloki += Convert.ToDecimal(lbBalloki.Text);
                                UCC += Convert.ToDecimal(lbUCC.Text);
                                MR += Convert.ToDecimal(lbMR.Text);
                                QB += Convert.ToDecimal(lbQB.Text);
                                Eastern += Convert.ToDecimal(lbEastern.Text);
                            }
                        }
                        else if (TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDailyID <= (int)Constants.SeasonDistribution.LKEnd)  // Represents Late Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily || TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                SulemankiLK += Convert.ToDecimal(lbSulemanki.Text) * Constants.TDailyFactor;
                                BS1LK += Convert.ToDecimal(lbBS1.Text) * Constants.TDailyFactor;
                                BS2LK += Convert.ToDecimal(lbBS2.Text) * Constants.TDailyFactor;
                                BallokiLK += Convert.ToDecimal(lbBalloki.Text) * Constants.TDailyFactor;
                                UCCLK += Convert.ToDecimal(lbUCC.Text) * Constants.TDailyFactor;
                                MRLK += Convert.ToDecimal(lbMR.Text) * Constants.TDailyFactor;
                                QBLK += Convert.ToDecimal(lbQB.Text) * Constants.TDailyFactor;
                                EasternLK += Convert.ToDecimal(lbEastern.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                SulemankiLK += Convert.ToDecimal(lbSulemanki.Text);
                                BS1LK += Convert.ToDecimal(lbBS1.Text);
                                BS2LK += Convert.ToDecimal(lbBS2.Text);
                                BallokiLK += Convert.ToDecimal(lbBalloki.Text);
                                UCCLK += Convert.ToDecimal(lbUCC.Text);
                                MRLK += Convert.ToDecimal(lbMR.Text);
                                QBLK += Convert.ToDecimal(lbQB.Text);
                                EasternLK += Convert.ToDecimal(lbEastern.Text);
                            }
                        }
                    }
                    else   // Rabi Case
                    {
                        if (TDailyID >= (int)Constants.SeasonDistribution.RabiStart && TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily || TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                || TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily || TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                            {
                                Sulemanki += Convert.ToDecimal(lbSulemanki.Text) * Constants.TDailyFactor;
                                BS1 += Convert.ToDecimal(lbBS1.Text) * Constants.TDailyFactor;
                                BS2 += Convert.ToDecimal(lbBS2.Text) * Constants.TDailyFactor;
                                Balloki += Convert.ToDecimal(lbBalloki.Text) * Constants.TDailyFactor;
                                UCC += Convert.ToDecimal(lbUCC.Text) * Constants.TDailyFactor;
                                MR += Convert.ToDecimal(lbMR.Text) * Constants.TDailyFactor;
                                QB += Convert.ToDecimal(lbQB.Text) * Constants.TDailyFactor;
                                Eastern += Convert.ToDecimal(lbEastern.Text) * Constants.TDailyFactor;
                            }
                            else if (TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    Sulemanki += Convert.ToDecimal(lbSulemanki.Text) * Constants.TDailyLeapYearTrue;
                                    BS1 += Convert.ToDecimal(lbBS1.Text) * Constants.TDailyLeapYearTrue;
                                    BS2 += Convert.ToDecimal(lbBS2.Text) * Constants.TDailyLeapYearTrue;
                                    Balloki += Convert.ToDecimal(lbBalloki.Text) * Constants.TDailyLeapYearTrue;
                                    UCC += Convert.ToDecimal(lbUCC.Text) * Constants.TDailyLeapYearTrue;
                                    MR += Convert.ToDecimal(lbMR.Text) * Constants.TDailyLeapYearTrue;
                                    QB += Convert.ToDecimal(lbQB.Text) * Constants.TDailyLeapYearTrue;
                                    Eastern += Convert.ToDecimal(lbEastern.Text) * Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    Sulemanki += Convert.ToDecimal(lbSulemanki.Text) * Constants.TDailyLeapYearFalse;
                                    BS1 += Convert.ToDecimal(lbBS1.Text) * Constants.TDailyLeapYearFalse;
                                    BS2 += Convert.ToDecimal(lbBS2.Text) * Constants.TDailyLeapYearFalse;
                                    Balloki += Convert.ToDecimal(lbBalloki.Text) * Constants.TDailyLeapYearFalse;
                                    UCC += Convert.ToDecimal(lbUCC.Text) * Constants.TDailyLeapYearFalse;
                                    MR += Convert.ToDecimal(lbMR.Text) * Constants.TDailyLeapYearFalse;
                                    QB += Convert.ToDecimal(lbQB.Text) * Constants.TDailyLeapYearFalse;
                                    Eastern += Convert.ToDecimal(lbEastern.Text) * Constants.TDailyLeapYearFalse;
                                }
                            }
                            else
                            {
                                Sulemanki += Convert.ToDecimal(lbSulemanki.Text);
                                BS1 += Convert.ToDecimal(lbBS1.Text);
                                BS2 += Convert.ToDecimal(lbBS2.Text);
                                Balloki += Convert.ToDecimal(lbBalloki.Text);
                                UCC += Convert.ToDecimal(lbUCC.Text);
                                MR += Convert.ToDecimal(lbMR.Text);
                                QB += Convert.ToDecimal(lbQB.Text);
                                Eastern += Convert.ToDecimal(lbEastern.Text);
                            }
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        Label lblSulemankiEK = (Label)e.Row.FindControl("lblSulemankiEK");
                        if (lblSulemankiEK != null)
                            lblSulemankiEK.Text = ConvertToThreeDecimalPlaces(Sulemanki * Constants.MAFFactor);

                        Label lblSulemankiLK = (Label)e.Row.FindControl("lblSulemankiLK");
                        if (lblSulemankiLK != null)
                            lblSulemankiLK.Text = ConvertToThreeDecimalPlaces(SulemankiLK * Constants.MAFFactor);

                        Label lblSulemankiTotal = (Label)e.Row.FindControl("lblSulemankiTotal");
                        if (lblSulemankiTotal != null)
                            lblSulemankiTotal.Text = ConvertToThreeDecimalPlaces((Sulemanki * Constants.MAFFactor) + (SulemankiLK * Constants.MAFFactor));


                        Label lblBS1EK = (Label)e.Row.FindControl("lblBS1EK");
                        if (lblBS1EK != null)
                            lblBS1EK.Text = ConvertToThreeDecimalPlaces(BS1 * Constants.MAFFactor);

                        Label lblBS1LK = (Label)e.Row.FindControl("lblBS1LK");
                        if (lblBS1LK != null)
                            lblBS1LK.Text = ConvertToThreeDecimalPlaces(BS1LK * Constants.MAFFactor);

                        Label lblBS1Total = (Label)e.Row.FindControl("lblBS1Total");
                        if (lblBS1Total != null)
                            lblBS1Total.Text = ConvertToThreeDecimalPlaces((BS1 * Constants.MAFFactor) + (BS1LK * Constants.MAFFactor));


                        Label lblBS2EK = (Label)e.Row.FindControl("lblBS2EK");
                        if (lblBS2EK != null)
                            lblBS2EK.Text = ConvertToThreeDecimalPlaces(BS2 * Constants.MAFFactor);

                        Label lblBS2LK = (Label)e.Row.FindControl("lblBS2LK");
                        if (lblBS2LK != null)
                            lblBS2LK.Text = ConvertToThreeDecimalPlaces(BS2LK * Constants.MAFFactor);

                        Label lblBS2Total = (Label)e.Row.FindControl("lblBS2Total");
                        if (lblBS2Total != null)
                            lblBS2Total.Text = ConvertToThreeDecimalPlaces((BS2 * Constants.MAFFactor) + (BS2LK * Constants.MAFFactor));


                        Label lblBallokiEK = (Label)e.Row.FindControl("lblBallokiEK");
                        if (lblBallokiEK != null)
                            lblBallokiEK.Text = ConvertToThreeDecimalPlaces(Balloki * Constants.MAFFactor);

                        Label lblBallokiLK = (Label)e.Row.FindControl("lblBallokiLK");
                        if (lblBallokiLK != null)
                            lblBallokiLK.Text = ConvertToThreeDecimalPlaces(BallokiLK * Constants.MAFFactor);

                        Label lblBallokiTotal = (Label)e.Row.FindControl("lblBallokiTotal");
                        if (lblBallokiTotal != null)
                            lblBallokiTotal.Text = ConvertToThreeDecimalPlaces((Balloki * Constants.MAFFactor) + (BallokiLK * Constants.MAFFactor));


                        Label lblUCCEK = (Label)e.Row.FindControl("lblUCCEK");
                        if (lblUCCEK != null)
                            lblUCCEK.Text = ConvertToThreeDecimalPlaces(UCC * Constants.MAFFactor);

                        Label lblUCCLK = (Label)e.Row.FindControl("lblUCCLK");
                        if (lblUCCLK != null)
                            lblUCCLK.Text = ConvertToThreeDecimalPlaces(UCCLK * Constants.MAFFactor);

                        Label lblUCCTotal = (Label)e.Row.FindControl("lblUCCTotal");
                        if (lblUCCTotal != null)
                            lblUCCTotal.Text = ConvertToThreeDecimalPlaces((UCC * Constants.MAFFactor) + (UCCLK * Constants.MAFFactor));


                        Label lblMREK = (Label)e.Row.FindControl("lblMREK");
                        if (lblMREK != null)
                            lblMREK.Text = ConvertToThreeDecimalPlaces(MR * Constants.MAFFactor);

                        Label lblMRLK = (Label)e.Row.FindControl("lblMRLK");
                        if (lblMRLK != null)
                            lblMRLK.Text = ConvertToThreeDecimalPlaces(MRLK * Constants.MAFFactor);

                        Label lblMRTotal = (Label)e.Row.FindControl("lblMRTotal");
                        if (lblMRTotal != null)
                            lblMRTotal.Text = ConvertToThreeDecimalPlaces((MR * Constants.MAFFactor) + (MRLK * Constants.MAFFactor));

                        Label lblQBEK = (Label)e.Row.FindControl("lblQBEK");
                        if (lblQBEK != null)
                            lblQBEK.Text = ConvertToThreeDecimalPlaces(QB * Constants.MAFFactor);

                        Label lblQBLK = (Label)e.Row.FindControl("lblQBLK");
                        if (lblQBLK != null)
                            lblQBLK.Text = ConvertToThreeDecimalPlaces(QBLK * Constants.MAFFactor);

                        Label lblQBTotal = (Label)e.Row.FindControl("lblQBTotal");
                        if (lblQBTotal != null)
                            lblQBTotal.Text = ConvertToThreeDecimalPlaces((QB * Constants.MAFFactor) + (QBLK * Constants.MAFFactor));


                        Label lblEasternEK = (Label)e.Row.FindControl("lblEasternEK");
                        if (lblEasternEK != null)
                            lblEasternEK.Text = ConvertToThreeDecimalPlaces(Eastern * Constants.MAFFactor);

                        Label lblEasternLK = (Label)e.Row.FindControl("lblEasternLK");
                        if (lblEasternLK != null)
                            lblEasternLK.Text = ConvertToThreeDecimalPlaces(EasternLK * Constants.MAFFactor);

                        Label lblEasternTotal = (Label)e.Row.FindControl("lblEasternTotal");
                        if (lblEasternTotal != null)
                            lblEasternTotal.Text = ConvertToThreeDecimalPlaces((Eastern * Constants.MAFFactor) + (EasternLK * Constants.MAFFactor));
                    }
                    else  //Rabi Case
                    {
                        Label lbEKPeriod = (Label)e.Row.FindControl("lblPeriodEK");
                        if (lbEKPeriod != null)
                            lbEKPeriod.Text = "Rabi (MAF)";

                        Label lbLKPeriod = (Label)e.Row.FindControl("lblPeriodLK");
                        if (lbLKPeriod != null)
                            lbLKPeriod.Visible = false;

                        Label lbTotalPeriod = (Label)e.Row.FindControl("lblPeriodTotal");
                        if (lbTotalPeriod != null)
                            lbTotalPeriod.Visible = false;

                        Label lblSulemankiEK = (Label)e.Row.FindControl("lblSulemankiEK");
                        if (lblSulemankiEK != null)
                            lblSulemankiEK.Text = ConvertToThreeDecimalPlaces(Sulemanki * Constants.MAFFactor);

                        Label lblBS1EK = (Label)e.Row.FindControl("lblBS1EK");
                        if (lblBS1EK != null)
                            lblBS1EK.Text = ConvertToThreeDecimalPlaces(BS1 * Constants.MAFFactor);

                        Label lblBS2EK = (Label)e.Row.FindControl("lblBS2EK");
                        if (lblBS2EK != null)
                            lblBS2EK.Text = ConvertToThreeDecimalPlaces(BS2 * Constants.MAFFactor);

                        Label lblBallokiEK = (Label)e.Row.FindControl("lblBallokiEK");
                        if (lblBallokiEK != null)
                            lblBallokiEK.Text = ConvertToThreeDecimalPlaces(Balloki * Constants.MAFFactor);

                        Label lblUCCEK = (Label)e.Row.FindControl("lblUCCEK");
                        if (lblUCCEK != null)
                            lblUCCEK.Text = ConvertToThreeDecimalPlaces(UCC * Constants.MAFFactor);

                        Label lblMREK = (Label)e.Row.FindControl("lblMREK");
                        if (lblMREK != null)
                            lblMREK.Text = ConvertToThreeDecimalPlaces(MR * Constants.MAFFactor);

                        Label lblQBEK = (Label)e.Row.FindControl("lblQBEK");
                        if (lblQBEK != null)
                            lblQBEK.Text = ConvertToThreeDecimalPlaces(QB * Constants.MAFFactor);

                        Label lblEasternEK = (Label)e.Row.FindControl("lblEasternEK");
                        if (lblEasternEK != null)
                            lblEasternEK.Text = ConvertToThreeDecimalPlaces(Eastern * Constants.MAFFactor);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid(long _Season, string _Year)
        {
            try
            {
                gvEasternComponent.Visible = true;
                gvEasternComponent.DataSource = new SeasonalPlanningBLL().GetTDailyDataForEasternComponent(_Season, Convert.ToInt32(_Year));
                gvEasternComponent.DataBind();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public String ConvertToThreeDecimalPlaces(String _Value)
        {
            return String.Format("{0:0.000}", Convert.ToDouble(_Value));
        }

        public String ConvertToTwoDecimalPlaces(String _Value)
        {
            return String.Format("{0:0.00}", Convert.ToDouble(_Value));
        }

        public String ConvertToThreeDecimalPlaces(Decimal _Value)
        {
            return String.Format("{0:0.000}", _Value);
        }

    }
}