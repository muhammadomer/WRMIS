using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class InflowForecasting : BasePage
    {
        #region  Variables
        int Season = 1;
        decimal EK = 0;
        decimal LK = 0;
        decimal Rabi = 0;
        int Count = 0;
        static long DraftID;
        static long JMScenarioIDMax;
        static long JMScenarioIDMin;
        static long JMScenarioIDLikely;
        static long CMScenarioIDMax;
        static long CMScenarioIDMin;
        static long CMScenarioIDLikely;
        static long ITScenarioIDMax;
        static long ITScenarioIDMin;
        static long ITScenarioIDLikely;
        static long KNScenarioIDMax;
        static long KNScenarioIDMin;
        static long KNScenarioIDLikely;
        List<SP_ForecastScenario> lstProbabilities;
        #endregion

        #region ViewState

        public string AddView = "AddView";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
                    || (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningApril)
                    || (DateTime.Now.Month >= (int)Constants.PlanningMonthsAndDays.KharifPlanningApril && DateTime.Now.Month <= 8)
                    || (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay)
                    )
                    Season = 2;

                if (!IsPostBack)
                {
                    SetTitle();
                    hName.InnerText = "Inflow Forecasting";
                    if (Season == (int)Constants.Seasons.Rabi)
                    {
                        RBSRM.Visible = false;
                        RBSelected.Visible = false;
                    }

                    if (!Utility.PlanningDaysLimit())
                        btnAdd.Visible = false;
                    else
                        btnAdd.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (RBStatistical.Checked == true)
                {
                    hName.InnerText = "Statistical Inflow Forecasting";
                    divScenarioSelection.Visible = false;
                    bool Result = SetDefaultDraftName();
                    if (Result)
                    {
                        ViewState[AddView] = "Add";
                        divStep1.Visible = true;
                        txtName.CssClass = txtName.CssClass.Replace("form-control", "form-control required");
                        txtName.Attributes.Add("required", "required");
                        lblPeriod.Text = PlanningPeriodMsg();
                        object objMaf = null;
                        if (Season == (int)Constants.Seasons.Kharif)
                            objMaf = new SeasonalPlanningBLL().GetPrevoidSeasonMAF((int)Constants.Seasons.Rabi);
                        else
                            objMaf = new SeasonalPlanningBLL().GetPrevoidSeasonMAF((int)Constants.Seasons.Kharif);
                        lblJM.Text = Convert.ToString(objMaf.GetType().GetProperty("JM").GetValue(objMaf));
                        lblCM.Text = Convert.ToString(objMaf.GetType().GetProperty("CM").GetValue(objMaf));
                        lblIT.Text = Convert.ToString(objMaf.GetType().GetProperty("IT").GetValue(objMaf));
                        lblKN.Text = Convert.ToString(objMaf.GetType().GetProperty("KN").GetValue(objMaf));

                        lblStep2JM.Text = Convert.ToString(objMaf.GetType().GetProperty("JM").GetValue(objMaf));
                        lblStep2CM.Text = Convert.ToString(objMaf.GetType().GetProperty("CM").GetValue(objMaf));
                        lblStep2IT.Text = Convert.ToString(objMaf.GetType().GetProperty("IT").GetValue(objMaf));
                        lblStep2KN.Text = Convert.ToString(objMaf.GetType().GetProperty("KN").GetValue(objMaf));

                        //lblKharifJM.Text = Convert.ToString(objMaf.GetType().GetProperty("JM").GetValue(objMaf));
                        //lblKharifCM.Text = Convert.ToString(objMaf.GetType().GetProperty("CM").GetValue(objMaf));
                        //lblKharifIT.Text = Convert.ToString(objMaf.GetType().GetProperty("IT").GetValue(objMaf));
                        //lblKharifKN.Text = Convert.ToString(objMaf.GetType().GetProperty("KN").GetValue(objMaf));

                        //lblLKJM.Text = Convert.ToString(objMaf.GetType().GetProperty("JM").GetValue(objMaf));
                        //lblLKCM.Text = Convert.ToString(objMaf.GetType().GetProperty("CM").GetValue(objMaf));
                        //lblLKIT.Text = Convert.ToString(objMaf.GetType().GetProperty("IT").GetValue(objMaf));
                        //lblLKKN.Text = Convert.ToString(objMaf.GetType().GetProperty("KN").GetValue(objMaf));
                    }
                    else
                    {
                        BindViewGrid();
                        divView.Visible = true;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                    }
                }
                else if (RBSRM.Checked == true)
                {
                    Response.RedirectPermanent("SRMInflowForecasting.aspx?From=Add");
                }
                else if (RBSelected.Checked == true)
                {
                    Response.RedirectPermanent("SelectedInflowForecasting.aspx?From=Add");
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool SetDefaultDraftName()
        {
            string DraftName = "";
            string Description = "Inflow Forecast Draft for ";
            bool Result = true;
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    DraftName = new SeasonalPlanningBLL().GetDraftCountName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                        divView.Visible = true;
                        BindViewGrid();
                    }
                    else
                        txtName.Text = DraftName + Description + "Kharif " + DateTime.Now.Year.ToString();
                }
                else
                {
                    DraftName = new SeasonalPlanningBLL().GetDraftCountName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                        divView.Visible = true;
                        BindViewGrid();
                    }
                    else
                    {
                        if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay)
                            || DateTime.Now.Month < (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch)
                            txtName.Text = DraftName + Description + "Rabi " + (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                        else
                            txtName.Text = DraftName + Description + "Rabi " + DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public String PlanningPeriodMsg()
        {
            String Message = "";
            try
            {
                if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay))
                {
                    Message = "Rabi " + (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString() + " Volume in MAF (Inflows from 1st Oct to Mar ";
                    if (DateTime.Now.Day > 20)
                        Message = Message + "20)";
                    else
                        Message = Message + "10)";
                }
                else if (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningApril)
                {
                    Message = "Rabi " + (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString() + " Volume in MAF (Inflows from 1st Oct to ";
                    Message = Message + "Mar 31)";
                }
                else if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay))
                {
                    Message = "Kharif " + (DateTime.Now.Year).ToString() + " Volume in MAF (Inflows from 1st Apr to Sep ";
                    if (DateTime.Now.Day > 20)
                        Message = Message + "20)";
                    else
                        Message = Message + "10)";
                }
                else if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober))
                {
                    Message = "Kharif " + (DateTime.Now.Year).ToString() + " Volume in MAF (Inflows from 1st Apr to ";
                    Message = Message + "Sep 30)";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Message;
        }

        protected void btnVariation_Click(object sender, EventArgs e)
        {
            try
            {
                divStep1.Visible = false;
                divStep2.Visible = true;

                double MAF = Convert.ToDouble(lblStep2KN.Text);
                double Variation = Convert.ToDouble(txtKNStartVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF - (MAF * Variation);
                    lblKNStartVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2KN.Text);
                Variation = Convert.ToDouble(txtKNEndVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF + (MAF * Variation);
                    lblKNEndVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2IT.Text);
                Variation = Convert.ToDouble(txtITStartVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF - (MAF * Variation);
                    lblITStartVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2IT.Text);
                Variation = Convert.ToDouble(txtITEndVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF + (MAF * Variation);
                    lblITEndVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2CM.Text);
                Variation = Convert.ToDouble(txtCMStartVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF - (MAF * Variation);
                    lblCMStartVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2CM.Text);
                Variation = Convert.ToDouble(txtCMEndVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF + (MAF * Variation);
                    lblCMEndVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2JM.Text);
                Variation = Convert.ToDouble(txtJMStartVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF - (MAF * Variation);
                    lblJMStartVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

                MAF = Convert.ToDouble(lblStep2JM.Text);
                Variation = Convert.ToDouble(txtJMEndVariation.Text);
                if (Variation > 0)
                {
                    Variation = Variation / 100;
                    MAF = MAF + (MAF * Variation);
                    lblJMEndVariationFinal.Text = ConvertToThreeDecimalPlaces(MAF);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnMatchInflows_Click(object sender, EventArgs e)
        {
            try
            {
                decimal EKVariation = Convert.ToInt32(txtJMStartVariation.Text);
                decimal MAF = Convert.ToDecimal(lblStep2JM.Text);
                decimal JMStartVariation = MAF - (MAF * (EKVariation / 100));

                decimal LKVariation = Convert.ToInt32(txtJMEndVariation.Text);
                decimal JMEndVariation = MAF + (MAF * (LKVariation / 100));

                EKVariation = Convert.ToInt32(txtCMStartVariation.Text);
                MAF = Convert.ToDecimal(lblStep2CM.Text);
                decimal CMStartVariation = MAF - (MAF * (EKVariation / 100));

                LKVariation = Convert.ToInt32(txtCMEndVariation.Text);
                decimal CMEndVariation = MAF + (MAF * (LKVariation / 100));

                EKVariation = Convert.ToInt32(txtITStartVariation.Text);
                MAF = Convert.ToDecimal(lblStep2IT.Text);
                decimal ITStartVariation = MAF - (MAF * (EKVariation / 100));

                LKVariation = Convert.ToInt32(txtITEndVariation.Text);
                decimal ITEndVariation = MAF + (MAF * (LKVariation / 100));

                EKVariation = Convert.ToInt32(txtKNStartVariation.Text);
                MAF = Convert.ToDecimal(lblStep2KN.Text);
                decimal KNStartVariation = MAF - (MAF * (EKVariation / 100));

                LKVariation = Convert.ToInt32(txtKNEndVariation.Text);
                decimal KNEndVariation = MAF + (MAF * (LKVariation / 100));

                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = true;
                Count = 0;
                EK = 0;
                LK = 0;
                Rabi = 0;
                gvJhelumAtMangla.DataSource = new SeasonalPlanningBLL().GetMatchingYears((int)Constants.RimStationsIDs.JhelumATMangla, JMStartVariation, JMEndVariation);
                gvJhelumAtMangla.DataBind();

                //aaa



                Count = 0;
                EK = 0;
                LK = 0;
                Rabi = 0;
                gvIndusAtTarbela.DataSource = new SeasonalPlanningBLL().GetMatchingYears((int)Constants.RimStationsIDs.IndusAtTarbela, ITStartVariation, ITEndVariation);
                gvIndusAtTarbela.DataBind();

                Count = 0;
                EK = 0;
                LK = 0;
                Rabi = 0;
                gvChenabAtMarala.DataSource = new SeasonalPlanningBLL().GetMatchingYears((int)Constants.RimStationsIDs.ChenabAtMarala, CMStartVariation, CMEndVariation);
                gvChenabAtMarala.DataBind();

                Count = 0;
                EK = 0;
                LK = 0;
                Rabi = 0;
                gvKabulAtNowshera.DataSource = new SeasonalPlanningBLL().GetMatchingYears((int)Constants.RimStationsIDs.KabulAtNowshera, KNStartVariation, KNEndVariation);
                gvKabulAtNowshera.DataBind();

                if ((int)Constants.Seasons.Rabi == Utility.GetCurrentSeasonForView())
                {
                    gvJhelumAtMangla.HeaderRow.Cells[3].Text = "Kharif(MAF)";
                    gvIndusAtTarbela.HeaderRow.Cells[3].Text = "Kharif(MAF)";
                    gvChenabAtMarala.HeaderRow.Cells[3].Text = "Kharif(MAF)";
                    gvKabulAtNowshera.HeaderRow.Cells[3].Text = "Kharif(MAF)";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void cbJM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                decimal Value = 0;
                decimal ValueLK = 0;
                foreach (GridViewRow row in gvJhelumAtMangla.Rows)
                {
                    CheckBox lblChkBox = (CheckBox)row.FindControl("cbJM");
                    if (lblChkBox.Checked == true)
                    {
                        if (Season == 2)
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblEKJM");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);

                            lblValue = (Label)row.FindControl("lblLKJM");
                            if (lblValue.Text != "")
                                ValueLK += Convert.ToDecimal(lblValue.Text);
                        }
                        else
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblRabiJM");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);
                        }
                    }
                }

                if (Season == 2)
                {
                    Label FooterRow = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblKharif"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = String.Format("{0:0.000}", Convert.ToDouble(Value / count));
                        else
                            FooterRow.Text = "0.000";
                    }

                    FooterRow = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblLK"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = String.Format("{0:0.000}", Convert.ToDouble(ValueLK / count));
                        else
                            FooterRow.Text = "0.000";
                    }
                }
                else
                {
                    Label FooterRow = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblRabi"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = String.Format("{0:0.000}", Convert.ToDouble(Value / count));
                        else
                            FooterRow.Text = "0.000";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void cbIT_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                decimal Value = 0;
                decimal ValueLK = 0;
                foreach (GridViewRow row in gvIndusAtTarbela.Rows)
                {
                    CheckBox lblChkBox = (CheckBox)row.FindControl("cbIT");
                    if (lblChkBox.Checked == true)
                    {
                        if (Season == 2)
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblEKIT");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);

                            lblValue = (Label)row.FindControl("lblLKIT");
                            if (lblValue.Text != "")
                                ValueLK += Convert.ToDecimal(lblValue.Text);
                        }
                        else
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblRabiIT");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);
                        }
                    }
                }

                if (Season == 2)
                {
                    Label FooterRow = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblKharif"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }

                    FooterRow = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblLK"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((ValueLK / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }
                else
                {
                    Label FooterRow = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblRabi"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void cbCM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                decimal Value = 0;
                decimal ValueLK = 0;
                foreach (GridViewRow row in gvChenabAtMarala.Rows)
                {
                    CheckBox lblChkBox = (CheckBox)row.FindControl("cbCM");
                    if (lblChkBox.Checked == true)
                    {
                        if (Season == 2)
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblEKCM");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);

                            lblValue = (Label)row.FindControl("lblLKCM");
                            if (lblValue.Text != "")
                                ValueLK += Convert.ToDecimal(lblValue.Text);
                        }
                        else
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblRabiCM");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);
                        }
                    }
                }

                if (Season == 2)
                {
                    Label FooterRow = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblKharif"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }

                    FooterRow = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblLK"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((ValueLK / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }
                else
                {
                    Label FooterRow = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblRabi"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void cbKN_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                decimal Value = 0;
                decimal ValueLK = 0;
                foreach (GridViewRow row in gvKabulAtNowshera.Rows)
                {
                    CheckBox lblChkBox = (CheckBox)row.FindControl("cbKN");
                    if (lblChkBox.Checked == true)
                    {
                        if (Season == 2)
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblEKKN");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);

                            lblValue = (Label)row.FindControl("lblLKKN");
                            if (lblValue.Text != "")
                                ValueLK += Convert.ToDecimal(lblValue.Text);
                        }
                        else
                        {
                            count++;
                            Label lblValue = (Label)row.FindControl("lblRabiKN");
                            if (lblValue.Text != "")
                                Value += Convert.ToDecimal(lblValue.Text);
                        }
                    }
                }

                if (Season == 2)
                {
                    Label FooterRow = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblKharif"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }

                    FooterRow = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblLK"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((ValueLK / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }
                else
                {
                    Label FooterRow = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblRabi"));
                    if (FooterRow != null)
                    {
                        if (count > 0)
                            FooterRow.Text = ConvertToThreeDecimalPlaces((Value / count).ToString());
                        else
                            FooterRow.Text = "0.000";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelumAtMangla_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Season == (int)Constants.Seasons.Kharif)
                    {
                        Label lblJMEK = (Label)e.Row.FindControl("lblEKJM");
                        Label lblJMLK = (Label)e.Row.FindControl("lblLKJM");
                        if (lblJMEK != null)
                            EK += Convert.ToDecimal(lblJMEK.Text);
                        if (lblJMLK != null)
                            LK += Convert.ToDecimal(lblJMLK.Text);
                        Count += 1;
                    }
                    else
                    {
                        Label lblJMRabi = (Label)e.Row.FindControl("lblRabiJM");
                        Label lblJMEK = (Label)e.Row.FindControl("lblEKJM");

                        if (lblJMRabi != null)
                            Rabi += Convert.ToDecimal(lblJMRabi.Text);
                        //if (lblJMLK != null)
                        //    lblJMLK.Visible = false;
                        Count += 1;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (Season == (int)Constants.Seasons.Kharif)
                    {
                        Label lblJMEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblJMLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            lblJMEK.Text = String.Format("{0:0.000}", Convert.ToDouble(EK / Count));
                            lblJMLK.Text = String.Format("{0:0.000}", Convert.ToDouble(LK / Count));
                        }
                        else
                        {
                            lblJMEK.Text = "0.000";
                            lblJMLK.Text = "0.000";
                        }
                    }
                    else
                    {
                        Label lblJMRabi = (Label)e.Row.FindControl("lblRabi");
                        Label lblJMEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblJMLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            if (lblJMEK != null)
                                lblJMEK.Visible = false;
                            if (lblJMEK != null)
                                lblJMLK.Visible = false;
                            if (lblJMRabi != null)
                                lblJMRabi.Text = String.Format("{0:0.000}", Convert.ToDouble(Rabi / Count));
                            lblJMRabi.Visible = true;
                        }
                        else
                            lblJMRabi.Text = "0.000";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndusAtTarbela_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblEKIT");
                        Label lblLK = (Label)e.Row.FindControl("lblLKIT");
                        if (lblEK != null)
                            EK += Convert.ToDecimal(lblEK.Text);
                        if (lblLK != null)
                            LK += Convert.ToDecimal(lblLK.Text);
                        Count += 1;
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabiIT");
                        Label lblLK = (Label)e.Row.FindControl("lblEKIT");

                        if (lblRabi != null)
                            Rabi += Convert.ToDecimal(lblRabi.Text);
                        //if (lblLK != null)
                        //    lblLK.Visible = false;
                        Count += 1;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            lblEK.Text = String.Format("{0:0.000}", Convert.ToDouble(EK / Count));
                            lblLK.Text = String.Format("{0:0.000}", Convert.ToDouble(LK / Count));
                        }
                        else
                        {
                            lblEK.Text = "0.000";
                            lblLK.Text = "0.000";
                        }
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabi");
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            if (lblEK != null)
                                lblEK.Visible = false;
                            if (lblEK != null)
                                lblLK.Visible = false;
                            if (lblRabi != null)
                                lblRabi.Text = String.Format("{0:0.000}", Convert.ToDouble(Rabi / Count));

                        }
                        else
                            lblRabi.Text = "0.000";
                        lblRabi.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChenabAtMarala_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblEKCM");
                        Label lblLK = (Label)e.Row.FindControl("lblLKCM");
                        if (lblEK != null)
                            EK += Convert.ToDecimal(lblEK.Text);
                        if (lblLK != null)
                            LK += Convert.ToDecimal(lblLK.Text);
                        Count += 1;
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabiCM");
                        Label lblLK = (Label)e.Row.FindControl("lblEKCM");

                        if (lblRabi != null)
                            Rabi += Convert.ToDecimal(lblRabi.Text);
                        //if (lblLK != null)
                        //    lblLK.Visible = false;
                        Count += 1;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            lblEK.Text = String.Format("{0:0.000}", Convert.ToDouble(EK / Count));
                            lblLK.Text = String.Format("{0:0.000}", Convert.ToDouble(LK / Count));
                        }
                        else
                        {
                            lblEK.Text = "0.000";
                            lblLK.Text = "0.000";
                        }
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabi");
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            if (lblEK != null)
                                lblEK.Visible = false;
                            if (lblEK != null)
                                lblLK.Visible = false;
                            if (lblRabi != null)
                                lblRabi.Text = String.Format("{0:0.000}", Convert.ToDouble(Rabi / Count));
                        }
                        else
                            lblRabi.Text = "0.000";
                        lblRabi.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKabulAtNowshera_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblEKKN");
                        Label lblLK = (Label)e.Row.FindControl("lblLKKN");
                        if (lblEK != null)
                            EK += Convert.ToDecimal(lblEK.Text);
                        if (lblLK != null)
                            LK += Convert.ToDecimal(lblLK.Text);
                        Count += 1;
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabiKN");
                        Label lblEK = (Label)e.Row.FindControl("lblEKKN");

                        if (lblRabi != null)
                            Rabi += Convert.ToDecimal(lblRabi.Text);
                        //if (lblLK != null)
                        //    lblLK.Visible = false;
                        Count += 1;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (Season == 2)
                    {
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            lblEK.Text = String.Format("{0:0.000}", Convert.ToDouble(EK / Count));
                            lblLK.Text = String.Format("{0:0.000}", Convert.ToDouble(LK / Count));
                        }
                        else
                        {
                            lblEK.Text = "0.000";
                            lblLK.Text = "0.000";
                        }
                    }
                    else
                    {
                        Label lblRabi = (Label)e.Row.FindControl("lblRabi");
                        Label lblEK = (Label)e.Row.FindControl("lblKharif");
                        Label lblLK = (Label)e.Row.FindControl("lblLK");
                        if (Count > 0)
                        {
                            if (lblEK != null)
                                lblEK.Visible = false;
                            if (lblEK != null)
                                lblLK.Visible = false;
                            if (lblRabi != null)
                                lblRabi.Text = String.Format("{0:0.000}", Convert.ToDouble(Rabi / Count));
                        }
                        else
                            lblRabi.Text = "0.000";
                        lblRabi.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnForecastProb_Click(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    decimal EK = 0;
                    decimal LK = 0;
                    Label EKFooter = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblKharif"));
                    lblKharifJM.Text = EKFooter.Text;
                    if (EKFooter != null)
                    {
                        EK = Convert.ToDecimal(EKFooter.Text);
                        lblMLJM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(EK, (int)Constants.RimStationsIDs.JhelumATMangla, Season, (int)Constants.SeasonDistribution.EKEnd));
                        if (lblMLJM.Text != "")
                        {
                            lblMaxJM.Text = ((Convert.ToInt32(lblMLJM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLJM.Text) - 10).ToString();
                            lblMinJM.Text = ((Convert.ToInt32(lblMLJM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLJM.Text) + 10).ToString();
                        }
                    }

                    EKFooter = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblKharif"));
                    lblKharifIT.Text = EKFooter.Text;
                    if (EKFooter != null)
                    {
                        EK = Convert.ToDecimal(EKFooter.Text);
                        lblMLIT.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(EK, (int)Constants.RimStationsIDs.IndusAtTarbela, Season, (int)Constants.SeasonDistribution.EKEnd));
                        if (lblMLIT.Text != "")
                        {
                            lblMaxIT.Text = ((Convert.ToInt32(lblMLIT.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLIT.Text) - 10).ToString();
                            lblMinIT.Text = ((Convert.ToInt32(lblMLIT.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLIT.Text) + 10).ToString();
                        }
                    }

                    EKFooter = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblKharif"));
                    lblKharifCM.Text = EKFooter.Text;
                    if (EKFooter != null)
                    {
                        EK = Convert.ToDecimal(EKFooter.Text);
                        lblMLCM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(EK, (int)Constants.RimStationsIDs.ChenabAtMarala, Season, (int)Constants.SeasonDistribution.EKEnd));
                        if (lblMLCM.Text != "")
                        {
                            lblMaxCM.Text = ((Convert.ToInt32(lblMLCM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLCM.Text) - 10).ToString();
                            lblMinCM.Text = ((Convert.ToInt32(lblMLCM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLCM.Text) + 10).ToString();
                        }
                    }

                    EKFooter = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblKharif"));
                    lblKharifKN.Text = EKFooter.Text;
                    if (EKFooter != null)
                    {
                        EK = Convert.ToDecimal(EKFooter.Text);
                        lblMLKN.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(EK, (int)Constants.RimStationsIDs.KabulAtNowshera, Season, (int)Constants.SeasonDistribution.EKEnd));
                        if (lblMLKN.Text != "")
                        {
                            lblMaxKN.Text = ((Convert.ToInt32(lblMLKN.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLKN.Text) - 10).ToString();
                            lblMinKN.Text = ((Convert.ToInt32(lblMLKN.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLKN.Text) + 10).ToString();
                        }
                    }

                    Label LKFooter = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblLK"));
                    lblLKJM.Text = LKFooter.Text;
                    if (LKFooter != null)
                    {
                        LK = Convert.ToDecimal(LKFooter.Text);
                        lblLKMLJM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(LK, (int)Constants.RimStationsIDs.JhelumATMangla, Season, (int)Constants.SeasonDistribution.LKEnd));
                        if (lblLKMLJM.Text != "")
                        {
                            lblLKMaxJM.Text = ((Convert.ToInt32(lblLKMLJM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblLKMLJM.Text) - 10).ToString();
                            lblLKMinJM.Text = ((Convert.ToInt32(lblLKMLJM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblLKMLJM.Text) + 10).ToString();

                        }
                    }

                    LKFooter = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblLK"));
                    lblLKIT.Text = LKFooter.Text;
                    if (LKFooter != null)
                    {
                        LK = Convert.ToDecimal(LKFooter.Text);
                        lblLKMLIT.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(LK, (int)Constants.RimStationsIDs.IndusAtTarbela, Season, (int)Constants.SeasonDistribution.LKEnd));
                        if (lblLKMLIT.Text != "")
                        {
                            lblLKMaxIT.Text = ((Convert.ToInt32(lblLKMLIT.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblLKMLIT.Text) - 10).ToString();
                            lblLKMinIT.Text = ((Convert.ToInt32(lblLKMLIT.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblLKMLIT.Text) + 10).ToString();
                        }
                    }

                    LKFooter = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblLK"));
                    lblLKCM.Text = LKFooter.Text;
                    if (LKFooter != null)
                    {
                        LK = Convert.ToDecimal(LKFooter.Text);
                        lblLKMLCM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(LK, (int)Constants.RimStationsIDs.ChenabAtMarala, Season, (int)Constants.SeasonDistribution.LKEnd));
                        if (lblLKMLCM.Text != "")
                        {
                            lblLKMaxCM.Text = ((Convert.ToInt32(lblLKMLCM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblLKMLCM.Text) - 10).ToString();
                            lblLKMinCM.Text = ((Convert.ToInt32(lblLKMLCM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblLKMLCM.Text) + 10).ToString();
                        }
                    }

                    LKFooter = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblLK"));
                    lblLKKN.Text = LKFooter.Text;
                    if (LKFooter != null)
                    {
                        LK = Convert.ToDecimal(LKFooter.Text);
                        lblLKMLKN.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(LK, (int)Constants.RimStationsIDs.KabulAtNowshera, Season, (int)Constants.SeasonDistribution.LKEnd));
                        if (lblLKMLKN.Text != "")
                        {
                            lblLKMaxKN.Text = ((Convert.ToInt32(lblLKMLKN.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblLKMLKN.Text) - 10).ToString();
                            lblLKMinKN.Text = ((Convert.ToInt32(lblLKMLKN.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblLKMLKN.Text) + 10).ToString();
                        }
                    }
                }
                else
                {
                    hSeasonRabi.Visible = true;
                    hSeasonKharif.Visible = false;
                    divLK.Visible = false;
                    Step4lblKharif.Text = "Rabi Average (MAF)";
                    Label RabiFooter = (Label)(gvJhelumAtMangla.FooterRow.FindControl("lblRabi"));
                    lblKharifJM.Text = RabiFooter.Text;

                    if (RabiFooter != null)
                    {
                        Rabi = Convert.ToDecimal(RabiFooter.Text);
                        lblMLJM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(Rabi, (int)Constants.RimStationsIDs.JhelumATMangla, Season, (int)Constants.SeasonDistribution.RabiEnd));
                        if (lblMLJM.Text != "")
                        {
                            lblMaxJM.Text = ((Convert.ToInt32(lblMLJM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLJM.Text) - 10).ToString();
                            lblMinJM.Text = ((Convert.ToInt32(lblMLJM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLJM.Text) + 10).ToString();
                        }
                    }

                    RabiFooter = (Label)(gvIndusAtTarbela.FooterRow.FindControl("lblRabi"));
                    lblKharifIT.Text = RabiFooter.Text;
                    if (RabiFooter != null)
                    {
                        Rabi = Convert.ToDecimal(RabiFooter.Text);
                        lblMLIT.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(Rabi, (int)Constants.RimStationsIDs.IndusAtTarbela, Season, (int)Constants.SeasonDistribution.RabiEnd));
                        if (lblMLIT.Text != "")
                        {
                            lblMaxIT.Text = ((Convert.ToInt32(lblMLIT.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLIT.Text) - 10).ToString();
                            lblMinIT.Text = ((Convert.ToInt32(lblMLIT.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLIT.Text) + 10).ToString();
                        }
                    }

                    RabiFooter = (Label)(gvChenabAtMarala.FooterRow.FindControl("lblRabi"));
                    lblKharifCM.Text = RabiFooter.Text;
                    if (RabiFooter != null)
                    {
                        Rabi = Convert.ToDecimal(RabiFooter.Text);
                        lblMLCM.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(Rabi, (int)Constants.RimStationsIDs.ChenabAtMarala, Season, (int)Constants.SeasonDistribution.RabiEnd));
                        if (lblMLCM.Text != "")
                        {
                            lblMaxCM.Text = ((Convert.ToInt32(lblMLCM.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLCM.Text) - 10).ToString();
                            lblMinCM.Text = ((Convert.ToInt32(lblMLCM.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLCM.Text) + 10).ToString();
                        }
                    }

                    RabiFooter = (Label)(gvKabulAtNowshera.FooterRow.FindControl("lblRabi"));
                    lblKharifKN.Text = RabiFooter.Text;
                    if (RabiFooter != null)
                    {
                        Rabi = Convert.ToDecimal(RabiFooter.Text);
                        lblMLKN.Text = Convert.ToString(new SeasonalPlanningBLL().ForecastProbability(Rabi, (int)Constants.RimStationsIDs.KabulAtNowshera, Season, (int)Constants.SeasonDistribution.RabiEnd));
                        if (lblMLKN.Text != "")
                        {
                            lblMaxKN.Text = ((Convert.ToInt32(lblMLKN.Text) - 10) < 0 ? 0 : Convert.ToInt32(lblMLKN.Text) - 10).ToString();
                            lblMinKN.Text = ((Convert.ToInt32(lblMLKN.Text) + 10) > 100 ? 100 : Convert.ToInt32(lblMLKN.Text) + 10).ToString();
                        }
                    }
                }

                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnForecast_Click(object sender, EventArgs e)
        {
            try
            {
                int? JM = null;
                int? JMLK = null;
                int? CM = null;
                int? CMLK = null;
                int? IT = null;
                int? ITLK = null;
                int? KN = null;
                int? KNLK = null;
                decimal Result;

                if (Season == (int)Constants.Seasons.Kharif)
                {
                    // JM = Convert.ToInt32(lblMLJM.Text) - Convert.ToInt32(txtMaxJM.Text);
                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                    if (JM < 0)
                        JM = 0;
                    JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text);
                    if (JMLK < 0)
                        JMLK = 0;
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                    if (CM < 0)
                        CM = 0;
                    CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text);
                    if (CMLK < 0)
                        CMLK = 0;
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                    if (IT < 0)
                        IT = 0;
                    ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text);
                    if (ITLK < 0)
                        ITLK = 0;
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);
                    if (KN < 0)
                        KN = 0;
                    KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text);
                    if (KNLK < 0)
                        KNLK = 0;
                    gvMax.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvMax.DataBind();

                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                    JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                    CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                    ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);
                    KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text);
                    gvMin.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvMin.DataBind();

                    JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                    if (JM < 0)
                        JM = 0;

                    Result = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                    JM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    JMLK = (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text));
                    if (JMLK < 0)
                        JMLK = 0;

                    Result = ((int)JMLK + (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text))) / 2;
                    JMLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                    if (CM < 0)
                        CM = 0;

                    Result = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                    CM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    CMLK = (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text));
                    if (CMLK < 0)
                        CMLK = 0;

                    Result = ((int)CMLK + (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text))) / 2;
                    CMLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));

                    Result = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                    IT = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    ITLK = (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text));

                    Result = ((int)ITLK + (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text))) / 2;
                    ITLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                    Result = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                    KN = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    KNLK = (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text));
                    Result = ((int)KNLK + (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text))) / 2;
                    KNLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    gvLikely.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvLikely.DataBind();
                }
                else
                {
                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                    if (JM < 0)
                        JM = 0;
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                    if (CM < 0)
                        CM = 0;
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                    if (IT < 0)
                        IT = 0;
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);
                    if (KN < 0)
                        KN = 0;
                    gvMax.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvMax.DataBind();

                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);
                    gvMin.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvMin.DataBind();

                    JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                    if (JM < 0)
                        JM = 0;

                    Result = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                    JM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                    if (CM < 0)
                        CM = 0;
                    Result = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                    CM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));
                    if (IT < 0)
                        IT = 0;
                    Result = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                    IT = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                    Result = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                    KN = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                    gvLikely.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, true);
                    gvLikely.DataBind();
                }
                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStepforecast.Visible = true;
                btnSave.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMax_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    int? JM = null;
                    int? JMLK = null;
                    int? CM = null;
                    int? CMLK = null;
                    int? IT = null;
                    int? ITLK = null;
                    int? KN = null;
                    int? KNLK = null;

                    if (Season == (int)Constants.Seasons.Kharif)
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                            if (JM < 0)
                                JM = 0;
                            JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text);
                            if (JMLK < 0)
                                JMLK = 0;
                            CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                            if (CM < 0)
                                CM = 0;
                            CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text);
                            if (CMLK < 0)
                                CMLK = 0;
                            IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                            if (IT < 0)
                                IT = 0;
                            ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text);
                            if (ITLK < 0)
                                ITLK = 0;
                            KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);
                            if (KN < 0)
                                KN = 0;
                            KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text);
                            if (KNLK < 0)
                                KNLK = 0;
                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                            JMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                            CMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                            ITLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                            KNLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);
                        }

                        Label Header = (Label)e.Row.FindControl("lblJMKahrifPer");
                        Header.Text = "EK %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Text = "LK %:" + JMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "EK %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Text = "LK %:" + CMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "EK %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Text = "LK %:" + ITLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "EK %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Text = "LK %:" + KNLK.ToString();
                    }
                    else
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                            CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                            IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                            KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);
                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Maximum").FirstOrDefault().RabiPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Maximum").FirstOrDefault().RabiPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Maximum").FirstOrDefault().RabiPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Maximum").FirstOrDefault().RabiPercent);
                        }

                        Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        Header.Text = "Rabi %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "Rabi %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "Rabi %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "Rabi %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblPeriod");
                    if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)" || lbl.Text == "Rabi(MAF)")
                    {
                        e.Row.Font.Bold = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int? JM = null;
                int? JMLK = null;
                int? CM = null;
                int? CMLK = null;
                int? IT = null;
                int? ITLK = null;
                int? KN = null;
                int? KNLK = null;

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Season == (int)Constants.Seasons.Kharif)
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                            JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text);
                            CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                            CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text);
                            IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                            ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text);
                            KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);
                            KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text);

                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                            JMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                            CMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                            ITLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                            KNLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);
                        }

                        Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        Header.Text = "EK %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Text = "LK %:" + JMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "EK %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Text = "LK %:" + CMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "EK %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Text = "LK %:" + ITLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "EK %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Text = "LK %:" + KNLK.ToString();
                    }
                    else
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                            CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                            IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                            KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);
                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Minimum").FirstOrDefault().RabiPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Minimum").FirstOrDefault().RabiPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Minimum").FirstOrDefault().RabiPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Minimum").FirstOrDefault().RabiPercent);
                        }

                        Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        Header.Text = "Rabi %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "Rabi %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "Rabi %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "Rabi %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblPeriod");
                    if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)" || lbl.Text == "Rabi(MAF)")
                    {
                        e.Row.Font.Bold = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLikely_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int? JM = null;
                int? JMLK = null;
                int? CM = null;
                int? CMLK = null;
                int? IT = null;
                int? ITLK = null;
                int? KN = null;
                int? KNLK = null;
                decimal Result;

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Season == (int)Constants.Seasons.Kharif)
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            //JM = Convert.ToInt32(lblMLJM.Text);
                            //JMLK = Convert.ToInt32(lblLKMLJM.Text);
                            //CM = Convert.ToInt32(lblMLCM.Text);
                            //CMLK = Convert.ToInt32(lblLKMLCM.Text);
                            //IT = Convert.ToInt32(lblMLIT.Text);
                            //ITLK = Convert.ToInt32(lblLKMLIT.Text);
                            //KN = Convert.ToInt32(lblMLKN.Text);
                            //KNLK = Convert.ToInt32(lblLKMLKN.Text);


                            JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                            if (JM < 0)
                                JM = 0;

                            Result = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                            JM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            JMLK = (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text));
                            if (JMLK < 0)
                                JMLK = 0;

                            Result = ((int)JMLK + (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text))) / 2;
                            JMLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                            if (CM < 0)
                                CM = 0;

                            Result = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                            CM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            CMLK = (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text));
                            if (CMLK < 0)
                                CMLK = 0;

                            Result = ((int)CMLK + (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text))) / 2;
                            CMLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));
                            if (IT < 0)
                                IT = 0;

                            Result = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                            IT = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            ITLK = (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text));
                            if (ITLK < 0)
                                ITLK = 0;

                            Result = ((int)ITLK + (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text))) / 2;
                            ITLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                            if (KN < 0)
                                KN = 0;
                            Result = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                            KN = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            KNLK = (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text));
                            if (KNLK < 0)
                                KNLK = 0;
                            Result = ((int)KNLK + (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text))) / 2;
                            KNLK = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);
                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                            JMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                            CMLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                            ITLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                            KNLK = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);
                        }

                        Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        Header.Text = "EK %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Text = "LK %:" + JMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "EK %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Text = "LK %:" + CMLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "EK %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Text = "LK %:" + ITLK.ToString();

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "EK %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Text = "LK %:" + KNLK.ToString();
                    }
                    else
                    {
                        if (ViewState[AddView] != null && ViewState[AddView].ToString() == "Add")
                        {
                            //JM = Convert.ToInt32(lblMLJM.Text);
                            //CM = Convert.ToInt32(lblMLCM.Text);
                            //IT = Convert.ToInt32(lblMLIT.Text);
                            //KN = Convert.ToInt32(lblMLKN.Text);

                            JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                            if (JM < 0)
                                JM = 0;

                            Result = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                            JM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                            if (CM < 0)
                                CM = 0;
                            Result = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                            CM = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));
                            if (IT < 0)
                                IT = 0;
                            Result = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                            IT = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);

                            KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                            Result = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                            KN = Convert.ToInt32(Math.Ceiling(Result / 5) * 5);
                        }
                        else
                        {
                            JM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Likely").FirstOrDefault().RabiPercent);

                            CM = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Likely").FirstOrDefault().RabiPercent);

                            IT = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Likely").FirstOrDefault().RabiPercent);

                            KN = (lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Likely").FirstOrDefault().RabiPercent);
                        }

                        Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        Header.Text = "Rabi %:" + JM.ToString();
                        Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        Header.Text = "Rabi %:" + CM.ToString();
                        Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        Header.Text = "Rabi %:" + IT.ToString();
                        Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        Header.Visible = false;

                        Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        Header.Text = "Rabi %:" + KN.ToString();
                        Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        Header.Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblPeriod");
                    if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)" || lbl.Text == "Rabi(MAF)")
                    {
                        e.Row.Font.Bold = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int? JM = null;
                int? JMLK = null;
                int? CM = null;
                int? CMLK = null;
                int? IT = null;
                int? ITLK = null;
                int? KN = null;
                int? KNLK = null;
                bool Result = false;
                SP_ForecastScenario ObjSave;

                SP_ForecastDraft Objsave = new SP_ForecastDraft();
                Objsave.Description = txtName.Text;
                Objsave.DraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                Objsave.SeasonID = (short)Season;
                Objsave.Year = (short)DateTime.Now.Year;
                Objsave.CreatedDate = DateTime.Now;
                Objsave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                Objsave.ModifiedDate = DateTime.Now;
                Objsave.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                DraftID = new SeasonalPlanningBLL().SaveStatisticalDraftBasicInfo(Objsave);

                if (Season == (int)Constants.Seasons.Kharif)
                {
                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                    JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                    CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                    ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);
                    KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)JM;
                    ObjSave.LkPercent = (short)JMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)CM;
                    ObjSave.LkPercent = (short)CMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)IT;
                    ObjSave.LkPercent = (short)ITLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)KN;
                    ObjSave.LkPercent = (short)KNLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDMax, CMScenarioIDMax, ITScenarioIDMax, KNScenarioIDMax, Convert.ToInt32(Session[SessionValues.UserID]));

                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                    JMLK = Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                    CMLK = Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                    ITLK = Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);
                    KNLK = Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)JM;
                    ObjSave.LkPercent = (short)JMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)CM;
                    ObjSave.LkPercent = (short)CMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)IT;
                    ObjSave.LkPercent = (short)ITLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)KN;
                    ObjSave.LkPercent = (short)KNLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDMin, CMScenarioIDMin, ITScenarioIDMin, KNScenarioIDMin, Convert.ToInt32(Session[SessionValues.UserID]));

                    //JM = Convert.ToInt32(lblMLJM.Text);
                    //JMLK = Convert.ToInt32(lblLKMLJM.Text);
                    //CM = Convert.ToInt32(lblMLCM.Text);
                    //CMLK = Convert.ToInt32(lblLKMLCM.Text);
                    //IT = Convert.ToInt32(lblMLIT.Text);
                    //ITLK = Convert.ToInt32(lblLKMLIT.Text);
                    //KN = Convert.ToInt32(lblMLKN.Text);
                    //KNLK = Convert.ToInt32(lblLKMLKN.Text);

                    JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                    if (JM < 0)
                        JM = 0;

                    JM = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                    JM = Convert.ToInt32(Math.Ceiling((double)JM / 5) * 5);

                    JMLK = (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMaxJM.Text));
                    if (JMLK < 0)
                        JMLK = 0;

                    JMLK = ((int)JMLK + (Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text))) / 2;
                    JMLK = Convert.ToInt32(Math.Ceiling((double)JMLK / 5) * 5);

                    CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                    if (CM < 0)
                        CM = 0;

                    CM = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                    CM = Convert.ToInt32(Math.Ceiling((double)CM / 5) * 5);

                    CMLK = (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMaxCM.Text));
                    if (CMLK < 0)
                        CMLK = 0;

                    CMLK = ((int)CMLK + (Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text))) / 2;
                    CMLK = Convert.ToInt32(Math.Ceiling((double)CMLK / 5) * 5);

                    IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));
                    if (IT < 0)
                        IT = 0;

                    IT = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                    IT = Convert.ToInt32(Math.Ceiling((double)IT / 5) * 5);

                    ITLK = (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMaxIT.Text));
                    if (ITLK < 0)
                        ITLK = 0;

                    ITLK = ((int)ITLK + (Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text))) / 2;
                    ITLK = Convert.ToInt32(Math.Ceiling((double)ITLK / 5) * 5);

                    KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                    if (KN < 0)
                        KN = 0;
                    KN = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                    KN = Convert.ToInt32(Math.Ceiling((double)KN / 5) * 5);

                    KNLK = (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMaxKN.Text));
                    if (KNLK < 0)
                        KNLK = 0;
                    KNLK = ((int)KNLK + (Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text))) / 2;
                    KNLK = Convert.ToInt32(Math.Ceiling((double)KNLK / 5) * 5);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)JM;
                    ObjSave.LkPercent = (short)JMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)CM;
                    ObjSave.LkPercent = (short)CMLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)IT;
                    ObjSave.LkPercent = (short)ITLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    ObjSave.EkPercent = (short)KN;
                    ObjSave.LkPercent = (short)KNLK;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    Result = new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDLikely, CMScenarioIDLikely, ITScenarioIDLikely, KNScenarioIDLikely, Convert.ToInt32(Session[SessionValues.UserID]));
                }
                else
                {
                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = (short)JM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = (short)CM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = (short)KN;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = (short)IT;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDMax, CMScenarioIDMax, ITScenarioIDMax, KNScenarioIDMax, Convert.ToInt32(Session[SessionValues.UserID]));

                    JM = Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text);
                    CM = Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text);
                    IT = Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text);
                    KN = Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = (short)JM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = (short)CM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = (short)KN;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = (short)IT;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDMin, CMScenarioIDMin, ITScenarioIDMin, KNScenarioIDMin, Convert.ToInt32(Session[SessionValues.UserID]));

                    //JM = Convert.ToInt32(lblMLJM.Text);
                    //CM = Convert.ToInt32(lblMLCM.Text);
                    //IT = Convert.ToInt32(lblMLIT.Text);
                    //KN = Convert.ToInt32(lblMLKN.Text);

                    JM = (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMaxJM.Text));
                    if (JM < 0)
                        JM = 0;

                    JM = ((int)JM + (Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text))) / 2;
                    JM = Convert.ToInt32(Math.Ceiling((double)JM / 5) * 5);

                    CM = (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMaxCM.Text));
                    if (CM < 0)
                        CM = 0;
                    CM = ((int)CM + (Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text))) / 2;
                    CM = Convert.ToInt32(Math.Ceiling((double)CM / 5) * 5);

                    IT = (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMaxIT.Text));
                    if (IT < 0)
                        IT = 0;
                    IT = ((int)IT + (Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text))) / 2;
                    IT = Convert.ToInt32(Math.Ceiling((double)IT / 5) * 5);

                    KN = (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMaxKN.Text));
                    KN = ((int)KN + (Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text))) / 2;
                    KN = Convert.ToInt32(Math.Ceiling((double)KN / 5) * 5);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = (short)JM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = (short)CM;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = (short)KN;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = (short)IT;
                    ObjSave.EkPercent = null;
                    ObjSave.LkPercent = null;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    Result = new SeasonalPlanningBLL().SaveForecastedValues(Season, JM, JMLK, CM, CMLK, IT, ITLK, KN, KNLK, JMScenarioIDLikely, CMScenarioIDLikely, ITScenarioIDLikely, KNScenarioIDLikely, Convert.ToInt32(Session[SessionValues.UserID]));
                }
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    BindViewGrid();
                    divView.Visible = true;
                    divStep1.Visible = false;
                    divStep2.Visible = false;
                    divStep3.Visible = false;
                    divStep4.Visible = false;
                    divStepforecast.Visible = false;
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackViewDiv_Click(object sender, EventArgs e)
        {
            try
            {
                divScenarioSelection.Visible = true;
                divView.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (RBStatistical.Checked == true)
                {
                    divStep1.Visible = false;
                    divScenarioSelection.Visible = false;
                    divView.Visible = true;
                    BindViewGrid();
                }
                else if (RBSRM.Checked == true)
                {
                    Response.RedirectPermanent("SRMInflowForecasting.aspx?From=View");
                }
                else if (RBSelected.Checked == true)
                {
                    Response.RedirectPermanent("SelectedInflowForecasting.aspx?From=View");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int RecordID = Convert.ToInt32(((Label)gvView.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new SeasonalPlanningBLL().DeleteDraft(RecordID);
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    BindViewGrid();
                }
                else
                    Master.ShowMessage(Message.NotallowedToDelete.Description, SiteMaster.MessageType.Error);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int RowID = e.NewEditIndex;
                long RecordID = Convert.ToInt64(((Label)gvView.Rows[RowID].Cells[0].FindControl("lblID")).Text);
                lstProbabilities = new SeasonalPlanningBLL().GetSavedProbabilities(RecordID);
                gvMax.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Maximum");
                gvMax.DataBind();
                // SetFooterForMaximumScenario();

                gvMin.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Minimum");
                gvMin.DataBind();
                // SetFooterForMinimumScenario();

                gvLikely.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Likely");
                gvLikely.DataBind();
                //  SetFooterForLikelyScenario();

                btnSave.Visible = false;
                btnBackForecast.Visible = false;
                btnBackView.Visible = true;
                header.InnerText = "Forecasted Scenarios";
                divView.Visible = false;
                divStepforecast.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindViewGrid()
        {
            try
            {
                gvView.DataSource = new SeasonalPlanningBLL().GetDraftsInformation();
                gvView.DataBind();
                ViewState[AddView] = "View";
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMaximumScenario()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    GridViewRow row = gvMax.Rows[6] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;
                    Label Footer = gvMax.FooterRow.FindControl("lblJMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblcM") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblcMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    row = gvMax.Rows[17] as GridViewRow;
                    Volume = row.FindControl("lblJM") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvMax.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblCM") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }


                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }
                }
                else
                {
                    Label Footer = gvMax.FooterRow.FindControl("lblJMKharif") as Label;
                    Label FooterNameEK = gvMax.FooterRow.FindControl("lblKharif") as Label;
                    Label FooterNameLK = gvMax.FooterRow.FindControl("lblLK") as Label;
                    Label FooterNameTotal = gvMax.FooterRow.FindControl("lblTotal") as Label;

                    if (FooterNameEK != null)
                        FooterNameEK.Text = "Rabi (MAF)";

                    if (FooterNameLK != null)
                        FooterNameLK.Visible = false;

                    if (FooterNameTotal != null)
                        FooterNameTotal.Visible = false;

                    GridViewRow row = gvMax.Rows[17] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;

                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblcM") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblcMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMax.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Footer = gvMax.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvMax.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMax.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMax.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMax.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvMax.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMinimumScenario()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    GridViewRow row = gvMin.Rows[6] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;
                    Label Footer = gvMin.FooterRow.FindControl("lblJMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblCM") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblCMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    row = gvMin.Rows[17] as GridViewRow;
                    Volume = row.FindControl("lblJM") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvMin.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblCM") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }
                }
                else
                {
                    Label Footer = gvMin.FooterRow.FindControl("lblJMKharif") as Label;
                    Label FooterNameEK = gvMin.FooterRow.FindControl("lblKharif") as Label;
                    Label FooterNameLK = gvMin.FooterRow.FindControl("lblLK") as Label;
                    Label FooterNameTotal = gvMin.FooterRow.FindControl("lblTotal") as Label;

                    if (FooterNameEK != null)
                        FooterNameEK.Text = "Rabi (MAF)";

                    if (FooterNameLK != null)
                        FooterNameLK.Visible = false;

                    if (FooterNameTotal != null)
                        FooterNameTotal.Visible = false;

                    GridViewRow row = gvMin.Rows[17] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;

                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblcM") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblcMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvMin.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Footer = gvMin.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvMin.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMin.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMin.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvMin.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvMin.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForLikelyScenario()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    GridViewRow row = gvLikely.Rows[6] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;
                    Label Footer = gvLikely.FooterRow.FindControl("lblJMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblCM") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblCMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;


                    row = gvLikely.Rows[17] as GridViewRow;
                    Volume = row.FindControl("lblJM") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvLikely.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblCM") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Text = Volume.Text;
                        FooterTotal.Text = Volume.Text;
                    }
                }
                else
                {
                    Label Footer = gvLikely.FooterRow.FindControl("lblJMKharif") as Label;
                    Label FooterNameEK = gvLikely.FooterRow.FindControl("lblKharif") as Label;
                    Label FooterNameLK = gvLikely.FooterRow.FindControl("lblLK") as Label;
                    Label FooterNameTotal = gvLikely.FooterRow.FindControl("lblTotal") as Label;

                    if (FooterNameEK != null)
                        FooterNameEK.Text = "Rabi (MAF)";

                    if (FooterNameLK != null)
                        FooterNameLK.Visible = false;

                    if (FooterNameTotal != null)
                        FooterNameTotal.Visible = false;

                    GridViewRow row = gvLikely.Rows[17] as GridViewRow;
                    Label Volume = row.FindControl("lblJM") as Label;

                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblcM") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblcMKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblIT") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblITKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Volume = row.FindControl("lblKN") as Label;
                    Footer = gvLikely.FooterRow.FindControl("lblKNKharif") as Label;
                    if (Volume != null && Footer != null)
                        Footer.Text = Volume.Text;

                    Footer = gvLikely.FooterRow.FindControl("lblJMLK") as Label;
                    Label FooterTotal = gvMin.FooterRow.FindControl("lblJMTotal") as Label;
                    if (Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvLikely.FooterRow.FindControl("lblCMLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblCMTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvLikely.FooterRow.FindControl("lblITLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblITTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }

                    Footer = gvLikely.FooterRow.FindControl("lblKNLK") as Label;
                    FooterTotal = gvLikely.FooterRow.FindControl("lblKNTotal") as Label;
                    if (Volume != null && Footer != null && FooterTotal != null)
                    {
                        Footer.Visible = false;
                        FooterTotal.Visible = false;
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelumAtMangla_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (Season == (int)Constants.Seasons.Rabi)
                    if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
           || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober)
                        e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndusAtTarbela_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (Season == (int)Constants.Seasons.Rabi)
                    if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
           || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober)
                        e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChenabAtMarala_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (Season == (int)Constants.Seasons.Rabi)
                    if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
           || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober)
                        e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKabulAtNowshera_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (Season == (int)Constants.Seasons.Rabi)
                    if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
           || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober)
                        e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackStep1_Click(object sender, EventArgs e)
        {
            try
            {
                divScenarioSelection.Visible = true;
                divStep1.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackPlaceVariation_Click(object sender, EventArgs e)
        {
            try
            {
                divStep1.Visible = true;
                divStep2.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackMatchInflows_Click(object sender, EventArgs e)
        {
            try
            {
                divStep3.Visible = false;
                divStep2.Visible = true;

                double JM = Convert.ToDouble(lblStep2JM.Text);
                double Limit = Convert.ToInt32(txtJMStartVariation.Text);
                lblJMStartVariationFinal.Text = ConvertToThreeDecimalPlaces(JM - (JM * (Limit / 100)));
                Limit = Convert.ToInt32(txtJMEndVariation.Text);
                lblJMEndVariationFinal.Text = ConvertToThreeDecimalPlaces(JM + (JM * (Limit / 100)));

                double CM = Convert.ToDouble(lblStep2CM.Text);
                Limit = Convert.ToInt32(txtCMStartVariation.Text);
                lblCMStartVariationFinal.Text = ConvertToThreeDecimalPlaces(CM - (CM * (Limit / 100)));
                Limit = Convert.ToInt32(txtCMEndVariation.Text);
                lblCMEndVariationFinal.Text = ConvertToThreeDecimalPlaces(CM + (CM * (Limit / 100)));

                double IT = Convert.ToDouble(lblStep2IT.Text);
                Limit = Convert.ToInt32(txtITStartVariation.Text);
                lblITStartVariationFinal.Text = ConvertToThreeDecimalPlaces(IT - (IT * (Limit / 100)));
                Limit = Convert.ToInt32(txtITEndVariation.Text);
                lblITEndVariationFinal.Text = ConvertToThreeDecimalPlaces(IT + (IT * (Limit / 100)));

                double KN = Convert.ToDouble(lblStep2KN.Text);
                Limit = Convert.ToInt32(txtKNStartVariation.Text);
                lblKNStartVariationFinal.Text = ConvertToThreeDecimalPlaces(KN - (KN * (Limit / 100)));
                Limit = Convert.ToInt32(txtKNEndVariation.Text);
                lblKNEndVariationFinal.Text = ConvertToThreeDecimalPlaces(KN + (KN * (Limit / 100)));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackStep4_Click(object sender, EventArgs e)
        {
            try
            {
                divStep4.Visible = false;
                divStep3.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackForecast_Click(object sender, EventArgs e)
        {
            try
            {
                divStepforecast.Visible = false;
                divStep4.Visible = true;

                int? JM = null;
                int? JMLK = null;
                int? CM = null;
                int? CMLK = null;
                int? IT = null;
                int? ITLK = null;
                int? KN = null;
                int? KNLK = null;

                JM = Convert.ToInt32(lblMLJM.Text) - Convert.ToInt32(txtMaxJM.Text);
                if (JM < 0)
                    JM = 0;
                lblMaxJM.Text = JM.ToString();

                JMLK = Convert.ToInt32(lblLKMLJM.Text) - Convert.ToInt32(txtLKMaxJM.Text);
                if (JMLK < 0)
                    JMLK = 0;
                lblLKMaxJM.Text = JMLK.ToString();

                CM = Convert.ToInt32(lblMLCM.Text) - Convert.ToInt32(txtMaxCM.Text);
                if (CM < 0)
                    CM = 0;
                lblMaxCM.Text = CM.ToString();

                CMLK = Convert.ToInt32(lblLKMLCM.Text) - Convert.ToInt32(txtLKMaxCM.Text);
                if (CMLK < 0)
                    CMLK = 0;
                lblLKMaxCM.Text = CMLK.ToString();

                IT = Convert.ToInt32(lblMLIT.Text) - Convert.ToInt32(txtMaxIT.Text);
                if (IT < 0)
                    IT = 0;
                lblMaxIT.Text = IT.ToString();

                ITLK = Convert.ToInt32(lblLKMLIT.Text) - Convert.ToInt32(txtLKMaxIT.Text);
                if (ITLK < 0)
                    ITLK = 0;
                lblLKMaxIT.Text = ITLK.ToString();

                KN = Convert.ToInt32(lblMLKN.Text) - Convert.ToInt32(txtMaxKN.Text);
                if (KN < 0)
                    KN = 0;
                lblMaxKN.Text = KN.ToString();

                KNLK = Convert.ToInt32(lblLKMLKN.Text) - Convert.ToInt32(txtLKMaxKN.Text);
                if (KNLK < 0)
                    KNLK = 0;
                lblLKMaxKN.Text = KNLK.ToString();

                if (Season == (int)Constants.Seasons.Kharif)
                {
                    lblMinJM.Text = Convert.ToString(Convert.ToInt32(lblMLJM.Text) + Convert.ToInt32(txtMinJM.Text));
                    lblLKMinJM.Text = Convert.ToString(Convert.ToInt32(lblLKMLJM.Text) + Convert.ToInt32(txtLKMinJM.Text));
                    lblMinCM.Text = Convert.ToString(Convert.ToInt32(lblMLCM.Text) + Convert.ToInt32(txtMinCM.Text));
                    lblLKMinCM.Text = Convert.ToString(Convert.ToInt32(lblLKMLCM.Text) + Convert.ToInt32(txtLKMinCM.Text));
                    lblMinIT.Text = Convert.ToString(Convert.ToInt32(lblMLIT.Text) + Convert.ToInt32(txtMinIT.Text));
                    lblLKMinIT.Text = Convert.ToString(Convert.ToInt32(lblLKMLIT.Text) + Convert.ToInt32(txtLKMinIT.Text));
                    lblMinKN.Text = Convert.ToString(Convert.ToInt32(lblMLKN.Text) + Convert.ToInt32(txtMinKN.Text));
                    lblLKMinKN.Text = Convert.ToString(Convert.ToInt32(lblLKMLKN.Text) + Convert.ToInt32(txtLKMinKN.Text));
                }
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

        public String ConvertToThreeDecimalPlaces(Double? _Value)
        {
            return String.Format("{0:0.000}", _Value);
        }

        protected void btnBackView_Click(object sender, EventArgs e)
        {
            try
            {
                divStepforecast.Visible = false;
                divView.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}