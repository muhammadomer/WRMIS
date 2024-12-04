using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class EntitlementOnDeliveries : BasePage
    {
        #region GridIndex

        public const int PercentageFiveYrRabiIndex = 0;
        public const int PercentageTenYrRabiIndex = 1;
        public const int Percentage7782RabiIndex = 2;
        public const int RabiCommandChannelIDIndex = 3;
        public const int SeasonalAverageIDIndex = 4;

        public const int PercentageFiveYrEKIndex = 0;
        public const int PercentageTenYrEKIndex = 1;
        public const int Percentage7782EKIndex = 2;
        public const int PercentageFiveYrLKIndex = 3;
        public const int PercentageTenYrLKIndex = 4;
        public const int Percentage7782LKIndex = 5;
        public const int KharifCommandChannelIDIndex = 6;
        public const int EKSeasonalAverageIDIndex = 7;
        public const int LKSeasonalAverageIDIndex = 8;

        #endregion

        #region Screen Constants

        public const int PercentageRoundDigit = 2;
        public const int MAFRoundDigit = 3;

        #endregion

        #region ViewState Keys

        public const string ProvincialEntitlementIDKey = "ProvincialEntitlementID";

        #endregion

        public double TotalMAF5y = 0, TotalMAF10y = 0, TotalMAF7782 = 0, TotalChannelEntitlement = 0, TotalYearlyEntitlemt = 0;
        public double TotalEKMAF5y = 0, TotalLKMAF5y = 0, TotalEKMAF10y = 0, TotalLKMAF10y = 0, TotalEKMAF7782 = 0,
            TotalLKMAF7782 = 0, TotalEKMAFYear = 0, TotalLKMAFYear = 0, TotalEKChannelEntitlement = 0, TotalLKChannelEntitlement;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindCommandDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 24-01-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function sets the command names in the dropdown
        /// Created On 24-01-2017
        /// </summary>
        private void BindCommandDropdown()
        {
            Dropdownlist.DDLCommandNames(ddlCommand);
        }

        protected void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCommand.SelectedValue != string.Empty)
                {
                    dvScenario.Visible = true;
                    ddlScenario.SelectedIndex = 0;
                }
                else
                {
                    dvScenario.Visible = false;
                }

                pnlGrid.Visible = false;
                gvRabiAverage.Visible = false;
                gvKharifAverage.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlScenario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRabiAverage.Visible = false;
                gvKharifAverage.Visible = false;
                pnlRabiHeader.Visible = false;
                pnlKharifHeader.Visible = false;

                if (ddlScenario.SelectedValue != string.Empty)
                {
                    DateTime Now = DateTime.Now;
                    int ScenarioID = Int32.Parse(ddlScenario.SelectedValue);
                    string CommandName = ddlCommand.SelectedItem.Text;
                    long CommandID = Int64.Parse(ddlCommand.SelectedValue);
                    string Scenario = ddlScenario.SelectedItem.Text;

                    //EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

                    //// Kharif Entitlement days are between 11 March and 30 April
                    //if (new DateTime(Now.Year, 3, 11) <= Now && new DateTime(Now.Year, 4, 30) >= Now)
                    //{
                    //    FillKharifData(ScenarioID, CommandName, Now.Year, CommandID, Scenario);
                    //}
                    //// Rabi Entitlement days are between 11 September and 31 October
                    //else if (new DateTime(Now.Year, 9, 11) <= Now && new DateTime(Now.Year, 10, 31) >= Now)
                    //{
                    //    FillRabiData(ScenarioID, CommandName, Now.Year, CommandID, Scenario);
                    //}
                    //else
                    //{
                    //    FillRabiData(ScenarioID, CommandName, Now.Year, CommandID, Scenario);
                    //}

                    // Kharif
                    if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) <= Now && new DateTime(Now.Year, Constants.KharifEntitlementEndMonth, Constants.KharifEntitlementEndDay) >= Now)
                    {
                        FillKharifData(ScenarioID, CommandName, Now.Year, CommandID, Scenario);
                    }
                    else
                    {
                        if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) > Now)
                        {
                            FillRabiData(ScenarioID, CommandName, Now.Year - 1, CommandID, Scenario);
                        }
                        else
                        {
                            FillRabiData(ScenarioID, CommandName, Now.Year, CommandID, Scenario);
                        }
                    }

                    pnlGrid.Visible = true;
                }
                else
                {
                    pnlGrid.Visible = false;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function gets the Rabi Data to fill the screen.
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_ScenarioID"></param>
        /// <param name="_CommandName"></param>
        /// <param name="_CurrentYear"></param>
        private void FillRabiData(int _ScenarioID, string _CommandName, int _CurrentYear, long _CommandID, string _Scenario)
        {
            //_CurrentYear = 2014;

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            ED_ProvincialEntitlement mdlProvincialEntitlement = bllEntitlementDelivery.GetProvincialEntitlement(_CommandID, _CurrentYear, (long)Constants.Seasons.Rabi, (long)Constants.PunjabProvinceID);

            double RabiMAF = 0;
            int NextYear = _CurrentYear + 1;

            if (_CommandID == (long)Constants.Commands.IndusCommand)
            {
                IEnumerable<DataRow> ieIndusRabiShare = bllEntitlementDelivery.GetRabiShare(_CurrentYear, _ScenarioID);

                List<dynamic> lstIndusRabiShare = ieIndusRabiShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("Indus")
                }).ToList<dynamic>();

                RabiMAF = Utility.GetMAF(lstIndusRabiShare, NextYear);
            }
            else
            {
                IEnumerable<DataRow> ieJCRabiShare = bllEntitlementDelivery.GetRabiShare(_CurrentYear, _ScenarioID);

                List<dynamic> lstJCRabiShare = ieJCRabiShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("J-C")
                }).ToList<dynamic>();

                RabiMAF = Utility.GetMAF(lstJCRabiShare, NextYear);
            }

            if (mdlProvincialEntitlement == null)
            {
                litTitle.Text = "Add Entitlement on Deliveries";
                ViewState.Add(ProvincialEntitlementIDKey, 0);

                pnlEdit.Visible = false;
            }
            else
            {
                litTitle.Text = "Edit Entitlement on Deliveries";
                ViewState.Add(ProvincialEntitlementIDKey, mdlProvincialEntitlement.ID);

                dynamic mdlEditData = bllEntitlementDelivery.GetEditInformation(_CurrentYear, (long)Constants.Seasons.Rabi, _CommandID);

                string SelectedYear = mdlEditData.GetType().GetProperty("SelectedYear").GetValue(mdlEditData, null);
                string SelectedAvg = mdlEditData.GetType().GetProperty("SelectedAvg").GetValue(mdlEditData, null);
                string ESource = mdlEditData.GetType().GetProperty("ESource").GetValue(mdlEditData, null);
                ViewState["SelectedAvg"] = SelectedAvg;
                ViewState["SelectedYear"] = SelectedYear;
                ViewState["ESource"] = ESource;
                ViewState["Scenario"] = mdlProvincialEntitlement.SP_PlanScenario.Scenario;
                DateTime? Date = mdlEditData.GetType().GetProperty("Date").GetValue(mdlEditData, null);
                string UserName = mdlEditData.GetType().GetProperty("UserName").GetValue(mdlEditData, null);



                string ModifiedDate = Utility.GetFormattedDate(Date);

                lblEdit.Text = string.Format("{0} Entitlement for {1} {2}-{3} on {4} ({5}) basis using {8} had been generated on {6} by {7}",
                    _CommandName, "Rabi", _CurrentYear, NextYear, SelectedAvg, mdlProvincialEntitlement.SP_PlanScenario.Scenario, ModifiedDate, UserName, ESource);

                pnlEdit.Visible = true;

                if (mdlProvincialEntitlement != null && mdlProvincialEntitlement.SP_PlanScenario.Scenario == ddlScenario.SelectedItem.Text)
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }

            lblMainDesc.Text = string.Format("{0} Entitlement for {1} {2}-{3}", _CommandName, "Rabi", _CurrentYear, NextYear);

            List<dynamic> lstCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.Rabi, _CommandID);

            double Average7782 = Utility.GetMAF(lstCusecs, NextYear);
            double Average7782Rounded = Math.Round(Average7782, MAFRoundDigit);
            lbl7782Average.Text = String.Format("{0:0.000}", Average7782Rounded);

            lblEntitlementText.Text = string.Format("Entitlement for {0} {1}-{2} (MAF):", "Rabi", _CurrentYear, NextYear);
            double RabiMAFRounded = Math.Round(RabiMAF, MAFRoundDigit);
            lblEntitlement.Text = String.Format("{0:0.000}", RabiMAFRounded);

            double PercentChange = ((RabiMAF / Average7782) * 100) - 100;
            double PercentChangeRounded = Math.Round(PercentChange, PercentageRoundDigit);
            lblPercentChange.Text = String.Format("{0:0.000}", PercentChangeRounded);

            List<dynamic> lstPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.Rabi, _CommandID);

            double Para2 = Utility.GetMAF(lstPara2Cs, NextYear);
            double Para2Rounded = Math.Round(Para2, MAFRoundDigit);
            lblPara2.Text = String.Format("{0:0.000}", Para2Rounded);

            pnlRabiHeader.Visible = true;

            BindRabiGrid(_CommandID, _CurrentYear);
        }

        /// <summary>
        /// This function gets the Kharif Data to fill the screen.
        /// Created On 31-01-2017
        /// </summary>
        /// <param name="_ScenarioID"></param>
        /// <param name="_CommandName"></param>
        /// <param name="_CurrentYear"></param>
        /// <param name="_CommandID"></param>
        private void FillKharifData(int _ScenarioID, string _CommandName, int _CurrentYear, long _CommandID, string _Scenario)
        {
            //_CurrentYear = 2016;

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            ED_ProvincialEntitlement mdlProvincialEntitlement = bllEntitlementDelivery.GetProvincialEntitlement(_CommandID, _CurrentYear, (long)Constants.Seasons.Kharif, (long)Constants.PunjabProvinceID);

            double EKMAF = 0;
            double LKMAF = 0;

            if (_CommandID == (long)Constants.Commands.IndusCommand)
            {
                IEnumerable<DataRow> ieIndusKharifShare = bllEntitlementDelivery.GetKharifShare(_CurrentYear, _ScenarioID);

                List<dynamic> lstIndusEarlyKharifShare = ieIndusKharifShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("Indus")
                }).Where(dr => dr.TDailyID <= Constants.Jun1TDailyID).ToList<dynamic>();

                EKMAF = Utility.GetMAF(lstIndusEarlyKharifShare, _CurrentYear);

                List<dynamic> lstIndusLateKharifShare = ieIndusKharifShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("Indus")
                }).Where(dr => dr.TDailyID >= Constants.Jun2TDailyID).ToList<dynamic>();

                LKMAF = Utility.GetMAF(lstIndusLateKharifShare, _CurrentYear);
            }
            else
            {
                IEnumerable<DataRow> ieJCKharifShare = bllEntitlementDelivery.GetKharifShare(_CurrentYear, _ScenarioID);

                List<dynamic> lstJCEarlyKharifShare = ieJCKharifShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("J-C")
                }).Where(dr => dr.TDailyID <= Constants.Jun1TDailyID).ToList<dynamic>();

                EKMAF = Utility.GetMAF(lstJCEarlyKharifShare, _CurrentYear);

                List<dynamic> lstJCLateKharifShare = ieJCKharifShare.Select(dataRow => new
                {
                    TDailyID = dataRow.Field<short>("TDailyID"),
                    ShortName = dataRow.Field<string>("shortName"),
                    Cusecs = dataRow.Field<double>("J-C")
                }).Where(dr => dr.TDailyID >= Constants.Jun2TDailyID).ToList<dynamic>();

                LKMAF = Utility.GetMAF(lstJCLateKharifShare, _CurrentYear);
            }

            if (mdlProvincialEntitlement == null)
            {
                litTitle.Text = "Add Entitlement on Deliveries";
                ViewState.Add(ProvincialEntitlementIDKey, 0);

                pnlEdit.Visible = false;
            }
            else
            {
                litTitle.Text = "Edit Entitlement on Deliveries";
                ViewState.Add(ProvincialEntitlementIDKey, mdlProvincialEntitlement.ID);

                dynamic mdlEKEditData = bllEntitlementDelivery.GetEditInformation(_CurrentYear, (long)Constants.Seasons.EarlyKharif, _CommandID);

                string EKSelectedAvg = mdlEKEditData.GetType().GetProperty("SelectedAvg").GetValue(mdlEKEditData, null);
                string EKSelectedYear = mdlEKEditData.GetType().GetProperty("SelectedYear").GetValue(mdlEKEditData, null);
                DateTime? Date = mdlEKEditData.GetType().GetProperty("Date").GetValue(mdlEKEditData, null);
                string UserName = mdlEKEditData.GetType().GetProperty("UserName").GetValue(mdlEKEditData, null);

                dynamic mdlLKEditData = bllEntitlementDelivery.GetEditInformation(_CurrentYear, (long)Constants.Seasons.LateKharif, _CommandID);

                string LKSelectedAvg = mdlLKEditData.GetType().GetProperty("SelectedAvg").GetValue(mdlLKEditData, null);
                string LKSelectedYear = mdlLKEditData.GetType().GetProperty("SelectedYear").GetValue(mdlLKEditData, null);
                string ESource = mdlLKEditData.GetType().GetProperty("ESource").GetValue(mdlLKEditData, null);

                string ModifiedDate = Utility.GetFormattedDate(Date);

                ViewState["EKSelectedAvg"] = EKSelectedAvg;
                ViewState["EKSelectedYear"] = EKSelectedYear;
                ViewState["LKSelectedAvg"] = LKSelectedAvg;
                ViewState["LKSelectedYear"] = LKSelectedYear;
                ViewState["ESource"] = ESource;
                ViewState["Scenario"] = mdlProvincialEntitlement.SP_PlanScenario.Scenario;



                lblEdit.Text = string.Format("{0} Entitlement for {1} {2} on {3} basis and {4} {5} on {6} ({7}) basis using {10} had been generated on {8} by {9}",
                    _CommandName, "Early Kharif", _CurrentYear, EKSelectedAvg, "Late Kharif", _CurrentYear, LKSelectedAvg, mdlProvincialEntitlement.SP_PlanScenario.Scenario,
                    ModifiedDate, UserName, ESource);

                pnlEdit.Visible = true;

                if (mdlProvincialEntitlement.SP_PlanScenario.Scenario == ddlScenario.SelectedItem.Text)
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }

            lblMainDesc.Text = string.Format("{0} Entitlement for {1} {2}", _CommandName, "Kharif", _CurrentYear);

            List<dynamic> lstEKCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.EarlyKharif, _CommandID);

            double AverageEK7782 = Utility.GetMAF(lstEKCusecs, _CurrentYear);
            double AverageEK7782Rounded = Math.Round(AverageEK7782, MAFRoundDigit);
            lbl7782EKAverage.Text = String.Format("{0:0.000}", AverageEK7782Rounded);

            lblEKEntitlementText.Text = string.Format("Entitlement for {0} {1} (MAF):", "Early Kharif", _CurrentYear);
            double EKMAFRounded = Math.Round(EKMAF, MAFRoundDigit);
            lblEKEntitlement.Text = String.Format("{0:0.000}", EKMAFRounded);

            double EKPercentChange = ((EKMAF / AverageEK7782) * 100) - 100;
            lblEKPercentChange.Text = (Math.Round(EKPercentChange, PercentageRoundDigit)).ToString();

            List<dynamic> lstEKPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.EarlyKharif, _CommandID);

            double EKPara2 = Utility.GetMAF(lstEKPara2Cs, _CurrentYear);
            double EKPara2Rounded = Math.Round(EKPara2, MAFRoundDigit);
            lblEKPara2.Text = String.Format("{0:0.000}", EKPara2Rounded);

            List<dynamic> lstLKCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.LateKharif, _CommandID);

            double AverageLK7782 = Utility.GetMAF(lstLKCusecs, _CurrentYear);
            double AverageLK7782Rounded = Math.Round(AverageLK7782, MAFRoundDigit);
            lbl7782LKAverage.Text = String.Format("{0:0.000}", AverageLK7782Rounded);

            lblLKEntitlementText.Text = string.Format("Entitlement for {0} {1} (MAF):", "Late Kharif", _CurrentYear);
            double LKMAFRounded = Math.Round(LKMAF, MAFRoundDigit);
            lblLKEntitlement.Text = String.Format("{0:0.000}", LKMAFRounded);

            double LKPercentChange = ((LKMAF / AverageLK7782) * 100) - 100;
            lblLKPercentChange.Text = (Math.Round(LKPercentChange, PercentageRoundDigit)).ToString();

            List<dynamic> lstLKPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.LateKharif, _CommandID);

            double LKPara2 = Utility.GetMAF(lstLKPara2Cs, _CurrentYear);
            double LKPara2Rounded = Math.Round(LKPara2, MAFRoundDigit);
            lblLKPara2.Text = String.Format("{0:0.000}", LKPara2Rounded);

            pnlKharifHeader.Visible = true;

            BindKharifGrid(_CommandID, _CurrentYear);
        }

        protected void gvRabiAverage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblMAF5y = (Label)e.Row.FindControl("lblMAF5y");
                    double MAF5y = Double.Parse(lblMAF5y.Text);
                    MAF5y = Math.Round(MAF5y, MAFRoundDigit);
                    lblMAF5y.Text = String.Format("{0:0.000}", MAF5y);
                    TotalMAF5y = TotalMAF5y + MAF5y;

                    Label lblMAF10y = (Label)e.Row.FindControl("lblMAF10y");
                    double MAF10y = Double.Parse(lblMAF10y.Text);
                    MAF10y = Math.Round(MAF10y, MAFRoundDigit);
                    lblMAF10y.Text = String.Format("{0:0.000}", MAF10y);
                    TotalMAF10y = TotalMAF10y + MAF10y;

                    Label lblMAF7782 = (Label)e.Row.FindControl("lblMAF7782");
                    double MAF7782 = Double.Parse(lblMAF7782.Text);
                    MAF7782 = Math.Round(MAF7782, MAFRoundDigit);
                    lblMAF7782.Text = String.Format("{0:0.000}", MAF7782);
                    TotalMAF7782 = TotalMAF7782 + MAF7782;

                    Label lblSYear = (Label)e.Row.FindControl("lblSYearMAF");
                    double SYearMAF = Double.Parse(lblSYear.Text);
                    SYearMAF = Math.Round(SYearMAF, MAFRoundDigit);
                    lblSYear.Text = String.Format("{0:0.000}", SYearMAF);
                    TotalYearlyEntitlemt = TotalYearlyEntitlemt + SYearMAF;

                    TextBox txtChannelEntitlement = (TextBox)e.Row.FindControl("txtChannelEntitlement");
                    double ChannelEntitlement = Double.Parse(txtChannelEntitlement.Text);
                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);
                    TotalChannelEntitlement = TotalChannelEntitlement + ChannelEntitlement;
                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalMAF5y = (Label)e.Row.FindControl("lblTotalMAF5y");
                    lblTotalMAF5y.Text = String.Format("{0:0.000}", TotalMAF5y);

                    Label lblTotalMAF10y = (Label)e.Row.FindControl("lblTotalMAF10y");
                    lblTotalMAF10y.Text = String.Format("{0:0.000}", TotalMAF10y);

                    Label lblTotalMAF7782 = (Label)e.Row.FindControl("lblTotalMAF7782");
                    lblTotalMAF7782.Text = String.Format("{0:0.000}", TotalMAF7782);

                    Label lblTotalChannelEntitlement = (Label)e.Row.FindControl("lblTotalChannelEntitlement");
                    lblTotalChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);

                    Label lblSYearTotalMAF = (Label)e.Row.FindControl("lblSYearTotalMAF");
                    lblSYearTotalMAF.Text = String.Format("{0:0.000}", TotalYearlyEntitlemt);
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    DropDownList ddlEntitlementYear = (DropDownList)e.Row.FindControl("ddlSelectedYear");

                    DateTime Now = DateTime.Now;

                    int Year = Now.Year;

                    if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) > Now)
                    {
                        Year = Year - 1;
                    }

                    Dropdownlist.DDLDeliveriesYear(ddlEntitlementYear, (int)Constants.Seasons.Rabi, Year, (int)Constants.DropDownFirstOption.NoOption);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKharifAverage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblEKMAF5y = (Label)e.Row.FindControl("lblEKMAF5y");
                    double EKMAF5y = Double.Parse(lblEKMAF5y.Text);
                    EKMAF5y = Math.Round(EKMAF5y, MAFRoundDigit);
                    lblEKMAF5y.Text = String.Format("{0:0.000}", EKMAF5y);
                    TotalEKMAF5y = TotalEKMAF5y + EKMAF5y;

                    Label lblLKMAF5y = (Label)e.Row.FindControl("lblLKMAF5y");
                    double LKMAF5y = Double.Parse(lblLKMAF5y.Text);
                    LKMAF5y = Math.Round(LKMAF5y, MAFRoundDigit);
                    lblLKMAF5y.Text = String.Format("{0:0.000}", LKMAF5y);
                    TotalLKMAF5y = TotalLKMAF5y + LKMAF5y;

                    Label lblEKMAF10y = (Label)e.Row.FindControl("lblEKMAF10y");
                    double EKMAF10y = Double.Parse(lblEKMAF10y.Text);
                    EKMAF10y = Math.Round(EKMAF10y, MAFRoundDigit);
                    lblEKMAF10y.Text = String.Format("{0:0.000}", EKMAF10y);
                    TotalEKMAF10y = TotalEKMAF10y + EKMAF10y;

                    Label lblLKMAF10y = (Label)e.Row.FindControl("lblLKMAF10y");
                    double LKMAF10y = Double.Parse(lblLKMAF10y.Text);
                    LKMAF10y = Math.Round(LKMAF10y, MAFRoundDigit);
                    lblLKMAF10y.Text = String.Format("{0:0.000}", LKMAF10y);
                    TotalLKMAF10y = TotalLKMAF10y + LKMAF10y;

                    Label lblEKMAF7782 = (Label)e.Row.FindControl("lblEKMAF7782");
                    double EKMAF7782 = Double.Parse(lblEKMAF7782.Text);
                    EKMAF7782 = Math.Round(EKMAF7782, MAFRoundDigit);
                    lblEKMAF7782.Text = String.Format("{0:0.000}", EKMAF7782);
                    TotalEKMAF7782 = TotalEKMAF7782 + EKMAF7782;

                    Label lblLKMAF7782 = (Label)e.Row.FindControl("lblLKMAF7782");
                    double LKMAF7782 = Double.Parse(lblLKMAF7782.Text);
                    LKMAF7782 = Math.Round(LKMAF7782, MAFRoundDigit);
                    lblLKMAF7782.Text = String.Format("{0:0.000}", LKMAF7782);
                    TotalLKMAF7782 = TotalLKMAF7782 + LKMAF7782;

                    Label lblEKYearMAF = (Label)e.Row.FindControl("lblEKYearMAF");
                    double EKYearMAF = Double.Parse(lblEKYearMAF.Text);
                    EKYearMAF = Math.Round(EKYearMAF, MAFRoundDigit);
                    lblEKYearMAF.Text = String.Format("{0:0.000}", EKYearMAF);
                    TotalEKMAFYear = TotalEKMAFYear + EKYearMAF;

                    Label lblLKYearMAF = (Label)e.Row.FindControl("lblLKYearMAF");
                    double LKYearMAF = Double.Parse(lblLKMAF7782.Text);
                    LKYearMAF = Math.Round(LKYearMAF, MAFRoundDigit);
                    lblLKYearMAF.Text = String.Format("{0:0.000}", LKYearMAF);
                    TotalLKMAFYear = TotalLKMAFYear + LKYearMAF;

                    TextBox txtEKChannelEntitlement = (TextBox)e.Row.FindControl("txtEKChannelEntitlement");
                    double EKChannelEntitlement = Double.Parse(txtEKChannelEntitlement.Text);
                    EKChannelEntitlement = Math.Round(EKChannelEntitlement, MAFRoundDigit);
                    TotalEKChannelEntitlement = TotalEKChannelEntitlement + EKChannelEntitlement;
                    txtEKChannelEntitlement.Text = EKChannelEntitlement.ToString();

                    TextBox txtLKChannelEntitlement = (TextBox)e.Row.FindControl("txtLKChannelEntitlement");
                    double LKChannelEntitlement = Double.Parse(txtLKChannelEntitlement.Text);
                    LKChannelEntitlement = Math.Round(LKChannelEntitlement, MAFRoundDigit);
                    TotalLKChannelEntitlement = TotalLKChannelEntitlement + LKChannelEntitlement;
                    txtLKChannelEntitlement.Text = LKChannelEntitlement.ToString();
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalEKMAF5y = (Label)e.Row.FindControl("lblTotalEKMAF5y");
                    lblTotalEKMAF5y.Text = String.Format("{0:0.000}", TotalEKMAF5y);

                    Label lblTotalLKMAF5y = (Label)e.Row.FindControl("lblTotalLKMAF5y");
                    lblTotalLKMAF5y.Text = String.Format("{0:0.000}", TotalLKMAF5y);

                    Label lblTotalEKMAF10y = (Label)e.Row.FindControl("lblTotalEKMAF10y");
                    lblTotalEKMAF10y.Text = String.Format("{0:0.000}", TotalEKMAF10y);

                    Label lblTotalLKMAF10y = (Label)e.Row.FindControl("lblTotalLKMAF10y");
                    lblTotalLKMAF10y.Text = String.Format("{0:0.000}", TotalLKMAF10y);

                    Label lblTotalEKMAF7782 = (Label)e.Row.FindControl("lblTotalEKMAF7782");
                    lblTotalEKMAF7782.Text = String.Format("{0:0.000}", TotalEKMAF7782);

                    Label lblTotalLKMAF7782 = (Label)e.Row.FindControl("lblTotalLKMAF7782");
                    lblTotalLKMAF7782.Text = String.Format("{0:0.000}", TotalLKMAF7782);

                    Label lblEKYearTotalMAF = (Label)e.Row.FindControl("lblEKYearTotalMAF");
                    lblEKYearTotalMAF.Text = String.Format("{0:0.000}", TotalEKMAFYear);

                    Label lblLKYearTotalMAF = (Label)e.Row.FindControl("lblLKYearTotalMAF");
                    lblLKYearTotalMAF.Text = String.Format("{0:0.000}", TotalLKMAFYear);

                    Label lblTotalEKChannelEntitlement = (Label)e.Row.FindControl("lblTotalEKChannelEntitlement");
                    lblTotalEKChannelEntitlement.Text = String.Format("{0:0.000}", TotalEKChannelEntitlement);

                    Label lblTotalLKChannelEntitlement = (Label)e.Row.FindControl("lblTotalLKChannelEntitlement");
                    lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalLKChannelEntitlement);
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    DropDownList ddlEKEntitlementYear = (DropDownList)e.Row.FindControl("ddlEKSelectedYear");
                    DropDownList ddlLKEntitlementYear = (DropDownList)e.Row.FindControl("ddlLKSelectedYear");

                    DateTime Now = DateTime.Now;
                    int Year = Now.Year;

                    Dropdownlist.DDLDeliveriesYear(ddlEKEntitlementYear, (int)Constants.Seasons.Kharif, Year, (int)Constants.DropDownFirstOption.NoOption);
                    Dropdownlist.DDLDeliveriesYear(ddlLKEntitlementYear, (int)Constants.Seasons.Kharif, Year, (int)Constants.DropDownFirstOption.NoOption);

                    ViewState["btnPrintVisibleEK"] = ddlEKEntitlementYear.SelectedItem.Text;
                    ViewState["btnPrintVisibleLK"] = ddlLKEntitlementYear.SelectedItem.Text;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the Rabi grid.
        /// Created On 06-02-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_CurrentYear"></param>
        private void BindRabiGrid(long _CommandID, int _CurrentYear)
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            int NextYear = _CurrentYear + 1;

            double ProvincialRabiEntitlement = Double.Parse(lblEntitlement.Text);

            List<double> lstProvincialEntitlements = new List<double> { ProvincialRabiEntitlement };

            List<dynamic> lstAveragedData = bllEntitlementDelivery.GetAverageDataDeliveries(_CommandID, _CurrentYear, (long)Constants.Seasons.Rabi, lstProvincialEntitlements);

            if (lstAveragedData == null || lstAveragedData.Count() == 0)
            {
                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                lstAveragedData = bllEntitlementDelivery.SaveRetrieveRabiAverageDataDeliveries(_CommandID, _CurrentYear, mdlUsers.ID, lstProvincialEntitlements);
            }

            if (Convert.ToString(ViewState["SelectedAvg"]) == "5 year" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["SelectedAvg"]) == "10 year" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["SelectedAvg"]) == "1977-1982" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["SelectedAvg"]) == Convert.ToString(ViewState["SelectedYear"]) && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }

            gvRabiAverage.DataSource = lstAveragedData;
            gvRabiAverage.DataBind();

            string SelectedAvg = lstAveragedData[0].GetType().GetProperty("SelectedAvg").GetValue(lstAveragedData[0], null);
            string SelectedYear = Convert.ToString(lstAveragedData[0].GetType().GetProperty("SelectedYear").GetValue(lstAveragedData[0], null));

            DropDownList ddl = (DropDownList)gvRabiAverage.HeaderRow.FindControl("ddlSelectedYear");

            if (!string.IsNullOrEmpty(SelectedYear))
            {
                if (ddl != null)
                {
                    ddl.ClearSelection();
                    if (ddl.Items.FindByValue(SelectedYear) != null)
                        ddl.Items.FindByValue(SelectedYear).Selected = true;
                }
            }
            else
            {
                if (ddl != null)
                {
                    ddl.SelectedIndex = 0;
                }
            }

            if (SelectedAvg != null)
            {

                RadioButton rb5y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb5y");
                RadioButton rb10y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb10y");
                RadioButton rb7782 = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb7782");
                RadioButton rbYear = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rbSYear");

                if (SelectedAvg == "10y")
                {
                    rb5y.Checked = false;
                    rb10y.Checked = true;

                    rb7782.Checked = false;
                    rbYear.Checked = false;
                }
                else if (SelectedAvg == "7782")
                {
                    rbYear.Checked = false;
                    rb5y.Checked = false;
                    rb10y.Checked = false;
                    rb7782.Checked = true;
                }
                else if (SelectedAvg == "5y")
                {
                    rbYear.Checked = false;
                    rb5y.Checked = true;
                    rb10y.Checked = false;
                    rb7782.Checked = false;
                }
                else
                {
                    rbYear.Checked = true;
                    rb5y.Checked = false;
                    rb10y.Checked = false;
                    rb7782.Checked = false;
                }
            }

            Label lblHEntitlement = (Label)gvRabiAverage.HeaderRow.FindControl("lblHEntitlement");
            lblHEntitlement.Text = string.Format("{0} {1}-{2}", "Rabi", _CurrentYear, NextYear);

            gvRabiAverage.Visible = true;
        }

        /// <summary>
        /// This function binds the Kharif grid.
        /// Created On 07-02-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_CurrentYear"></param>
        private void BindKharifGrid(long _CommandID, int _CurrentYear)
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            double ProvincialEKEntitlement = Double.Parse(lblEKEntitlement.Text);
            double ProvincialLKEntitlement = Double.Parse(lblLKEntitlement.Text);

            List<double> lstProvincialEntitlements = new List<double> { ProvincialEKEntitlement, ProvincialLKEntitlement };

            List<dynamic> lstAveragedData = bllEntitlementDelivery.GetAverageDataDeliveries(_CommandID, _CurrentYear, (long)Constants.Seasons.Kharif, lstProvincialEntitlements);

            if (lstAveragedData == null || lstAveragedData.Count() == 0)
            {
                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                lstAveragedData = bllEntitlementDelivery.SaveRetrieveKharifAverageDataNew(_CommandID, _CurrentYear, mdlUsers.ID, lstProvincialEntitlements);
            }


            if (Convert.ToString(ViewState["EKSelectedAvg"]) == "5 year" && Convert.ToString(ViewState["LKSelectedAvg"]) == "5 year" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["EKSelectedAvg"]) == "10 year" && Convert.ToString(ViewState["LKSelectedAvg"]) == "10 year" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["EKSelectedAvg"]) == "1977-1982" && Convert.ToString(ViewState["LKSelectedAvg"]) == "1977-1982" && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else if (Convert.ToString(ViewState["EKSelectedAvg"]) == Convert.ToString(ViewState["EKSelectedYear"]) && Convert.ToString(ViewState["LKSelectedAvg"]) == Convert.ToString(ViewState["LKSelectedYear"]) && Convert.ToString(ViewState["ESource"]) == "Deliveries" && Convert.ToString(ViewState["Scenario"]) == ddlScenario.SelectedItem.Text)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }

            gvKharifAverage.DataSource = lstAveragedData;
            gvKharifAverage.DataBind();

            string EKSelectedAvg = lstAveragedData[0].GetType().GetProperty("EKSelectedAvg").GetValue(lstAveragedData[0], null);

            if (EKSelectedAvg != null)
            {
                RadioButton rbEK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK5y");
                RadioButton rbEK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK10y");
                RadioButton rbEK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK7782");
                RadioButton rbEKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEKYear");

                if (EKSelectedAvg == "10y")
                {
                    rbEK5y.Checked = false;
                    rbEK10y.Checked = true;
                    rbEK7782.Checked = false;
                    rbEKYear.Checked = false;
                }
                else if (EKSelectedAvg == "7782")
                {
                    rbEK5y.Checked = false;
                    rbEK10y.Checked = false;
                    rbEK7782.Checked = true;
                    rbEKYear.Checked = false;
                }
                else if (EKSelectedAvg == "5y")
                {
                    rbEK5y.Checked = true;
                    rbEK10y.Checked = false;
                    rbEK7782.Checked = false;
                    rbEKYear.Checked = false;
                }
                else
                {
                    rbEKYear.Checked = true;
                    rbEK5y.Checked = false;
                    rbEK10y.Checked = false;
                    rbEK7782.Checked = false;
                }
            }

            string EKYear = Convert.ToString(lstAveragedData[0].GetType().GetProperty("SelectedYearEK").GetValue(lstAveragedData[0], null));

            DropDownList ddlEK = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlEKSelectedYear");

            if (!string.IsNullOrEmpty(EKYear))
            {
                if (ddlEK != null)
                {
                    ddlEK.ClearSelection();
                    if (ddlEK.Items.FindByValue(EKYear) != null)
                        ddlEK.Items.FindByValue(EKYear).Selected = true;
                }
            }
            else
            {
                if (ddlEK != null)
                {
                    ddlEK.SelectedIndex = 0;
                }
            }

            Label lblEKHEntitlement = (Label)gvKharifAverage.HeaderRow.FindControl("lblEKHEntitlement");
            lblEKHEntitlement.Text = string.Format("{0} {1}", "Early Kharif", _CurrentYear);

            string LKSelectedAvg = lstAveragedData[0].GetType().GetProperty("LKSelectedAvg").GetValue(lstAveragedData[0], null);

            if (LKSelectedAvg != null)
            {
                RadioButton rbLK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK5y");
                RadioButton rbLK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK10y");
                RadioButton rbLK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK7782");
                RadioButton rbLKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLKYear");

                if (LKSelectedAvg == "10y")
                {
                    rbLK5y.Checked = false;
                    rbLK10y.Checked = true;
                    rbLK7782.Checked = false;
                    rbLKYear.Checked = false;
                }
                else if (LKSelectedAvg == "7782")
                {
                    rbLK5y.Checked = false;
                    rbLK10y.Checked = false;
                    rbLK7782.Checked = true;
                    rbLKYear.Checked = false;
                }
                else if (LKSelectedAvg == "5y")
                {
                    rbLKYear.Checked = false;
                    rbLK5y.Checked = true;
                    rbLK10y.Checked = false;
                    rbLK7782.Checked = false;
                }
                else
                {
                    rbLKYear.Checked = true;
                    rbLK5y.Checked = false;
                    rbLK10y.Checked = false;
                    rbLK7782.Checked = false;
                }
            }

            string LKYear = Convert.ToString(lstAveragedData[0].GetType().GetProperty("SelectedYearLK").GetValue(lstAveragedData[0], null));

            DropDownList ddlLK = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlLKSelectedYear");

            if (!string.IsNullOrEmpty(LKYear))
            {
                if (ddlLK != null)
                {
                    ddlLK.ClearSelection();
                    if (ddlLK.Items.FindByValue(LKYear) != null)
                        ddlLK.Items.FindByValue(LKYear).Selected = true;
                }
            }
            else
            {
                if (ddlLK != null)
                {
                    ddlLK.SelectedIndex = 0;
                }
            }

            Label lblLKHEntitlement = (Label)gvKharifAverage.HeaderRow.FindControl("lblLKHEntitlement");
            lblLKHEntitlement.Text = string.Format("{0} {1}", "Late Kharif", _CurrentYear);
            gvKharifAverage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvRabiAverage.Visible)
                {
                    SaveRabiData();
                }
                else
                {
                    SaveKharifData();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rb5y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvRabiAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnChannelEntitlement");

                    double PercentageFiveYr = Double.Parse(gvRabiAverage.DataKeys[gvr.RowIndex].Values[PercentageFiveYrRabiIndex].ToString());
                    double ProvincialEntitlement = Double.Parse(lblEntitlement.Text);
                    double ChannelEntitlement = (PercentageFiveYr * ProvincialEntitlement) / 100;

                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + ChannelEntitlement;

                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
                lblTotalChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                //RadioButton rb10y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb10y");
                RadioButton rb5y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb5y");
                if (ViewState["SelectedAvg"].ToString() == "5 year" && rb5y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rb10y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvRabiAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnChannelEntitlement");

                    double PercentageTenYr = Double.Parse(gvRabiAverage.DataKeys[gvr.RowIndex].Values[PercentageTenYrRabiIndex].ToString());
                    double ProvincialEntitlement = Double.Parse(lblEntitlement.Text);
                    double ChannelEntitlement = (PercentageTenYr * ProvincialEntitlement) / 100;

                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + ChannelEntitlement;

                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
                lblTotalChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rb10y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb10y");
                if (ViewState["SelectedAvg"].ToString() == "10 year" && rb10y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rb7782_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvRabiAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnChannelEntitlement");

                    double Percentage7782 = Double.Parse(gvRabiAverage.DataKeys[gvr.RowIndex].Values[Percentage7782RabiIndex].ToString());
                    double ProvincialEntitlement = Double.Parse(lblEntitlement.Text);
                    double ChannelEntitlement = (Percentage7782 * ProvincialEntitlement) / 100;

                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + ChannelEntitlement;

                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
                lblTotalChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rb7782 = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb7782");
                if (ViewState["SelectedAvg"].ToString() == "1977-1982" && rb7782.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtChannelEntitlement_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtChannelEntitlement = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtChannelEntitlement.NamingContainer;
                HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnChannelEntitlement");

                double OldValue = Double.Parse(hdnChannelEntitlement.Value);
                double NewValue = Double.Parse(txtChannelEntitlement.Text);

                double ValueDelta = NewValue - OldValue;

                Label lblTotalChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
                double TotalChannelEntitlement = Double.Parse(lblTotalChannelEntitlement.Text);

                lblTotalChannelEntitlement.Text = (TotalChannelEntitlement + ValueDelta).ToString();
                hdnChannelEntitlement.Value = txtChannelEntitlement.Text;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtEKChannelEntitlement_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtEKChannelEntitlement = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtEKChannelEntitlement.NamingContainer;
                HiddenField hdnEKChannelEntitlement = (HiddenField)gvr.FindControl("hdnEKChannelEntitlement");

                double OldValue = Double.Parse(hdnEKChannelEntitlement.Value);
                double NewValue = Double.Parse(txtEKChannelEntitlement.Text);

                double ValueDelta = NewValue - OldValue;

                Label lblTotalEKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
                double TotalChannelEntitlement = Double.Parse(lblTotalEKChannelEntitlement.Text);

                lblTotalEKChannelEntitlement.Text = (TotalChannelEntitlement + ValueDelta).ToString();
                hdnEKChannelEntitlement.Value = txtEKChannelEntitlement.Text;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtLKChannelEntitlement_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtLKChannelEntitlement = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtLKChannelEntitlement.NamingContainer;
                HiddenField hdnLKChannelEntitlement = (HiddenField)gvr.FindControl("hdnLKChannelEntitlement");

                double OldValue = Double.Parse(hdnLKChannelEntitlement.Value);
                double NewValue = Double.Parse(txtLKChannelEntitlement.Text);

                double ValueDelta = NewValue - OldValue;

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
                double TotalChannelEntitlement = Double.Parse(lblTotalLKChannelEntitlement.Text);

                lblTotalLKChannelEntitlement.Text = (TotalChannelEntitlement + ValueDelta).ToString();
                hdnLKChannelEntitlement.Value = txtLKChannelEntitlement.Text;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbEK5y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtEKChannelEntitlement = (TextBox)gvr.FindControl("txtEKChannelEntitlement");
                    HiddenField hdnEKChannelEntitlement = (HiddenField)gvr.FindControl("hdnEKChannelEntitlement");

                    double EKPercentageFiveYr = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[PercentageFiveYrEKIndex].ToString());
                    double EKProvincialEntitlement = Double.Parse(lblEKEntitlement.Text);
                    double EKChannelEntitlement = (EKPercentageFiveYr * EKProvincialEntitlement) / 100;

                    EKChannelEntitlement = Math.Round(EKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + EKChannelEntitlement;

                    txtEKChannelEntitlement.Text = EKChannelEntitlement.ToString();
                    hdnEKChannelEntitlement.Value = EKChannelEntitlement.ToString();
                }

                Label lblTotalEKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
                lblTotalEKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK5y");
                RadioButton rbLK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK5y");
                if (ViewState["EKSelectedAvg"].ToString() == "5 year" && ViewState["LKSelectedAvg"].ToString() == "5 year" && rbEK5y.Checked && rbLK5y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbLK5y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtLKChannelEntitlement = (TextBox)gvr.FindControl("txtLKChannelEntitlement");
                    HiddenField hdnLKChannelEntitlement = (HiddenField)gvr.FindControl("hdnLKChannelEntitlement");

                    double LKPercentageFiveYr = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[PercentageFiveYrLKIndex].ToString());
                    double LKProvincialEntitlement = Double.Parse(lblLKEntitlement.Text);
                    double LKChannelEntitlement = Math.Round((LKPercentageFiveYr * LKProvincialEntitlement) / 100, MAFRoundDigit);

                    LKChannelEntitlement = Math.Round(LKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + LKChannelEntitlement;

                    txtLKChannelEntitlement.Text = LKChannelEntitlement.ToString();
                    hdnLKChannelEntitlement.Value = LKChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK5y");
                RadioButton rbLK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK5y");
                if (ViewState["EKSelectedAvg"].ToString() == "5 year" && ViewState["LKSelectedAvg"].ToString() == "5 year" && rbEK5y.Checked && rbLK5y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbEK10y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtEKChannelEntitlement = (TextBox)gvr.FindControl("txtEKChannelEntitlement");
                    HiddenField hdnEKChannelEntitlement = (HiddenField)gvr.FindControl("hdnEKChannelEntitlement");

                    double EKPercentageTenYr = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[PercentageTenYrEKIndex].ToString());
                    double EKProvincialEntitlement = Double.Parse(lblEKEntitlement.Text);
                    double EKChannelEntitlement = Math.Round((EKPercentageTenYr * EKProvincialEntitlement) / 100, MAFRoundDigit);

                    EKChannelEntitlement = Math.Round(EKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + EKChannelEntitlement;

                    txtEKChannelEntitlement.Text = EKChannelEntitlement.ToString();
                    hdnEKChannelEntitlement.Value = EKChannelEntitlement.ToString();
                }

                Label lblTotalEKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
                lblTotalEKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK10y");
                RadioButton rbLK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK10y");
                if (ViewState["EKSelectedAvg"].ToString() == "10 year" && ViewState["LKSelectedAvg"].ToString() == "10 year" && rbEK10y.Checked && rbLK10y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbLK10y_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtLKChannelEntitlement = (TextBox)gvr.FindControl("txtLKChannelEntitlement");
                    HiddenField hdnLKChannelEntitlement = (HiddenField)gvr.FindControl("hdnLKChannelEntitlement");

                    double LKPercentageTenYr = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[PercentageTenYrLKIndex].ToString());
                    double LKProvincialEntitlement = Double.Parse(lblLKEntitlement.Text);
                    double LKChannelEntitlement = Math.Round((LKPercentageTenYr * LKProvincialEntitlement) / 100, MAFRoundDigit);

                    LKChannelEntitlement = Math.Round(LKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + LKChannelEntitlement;

                    txtLKChannelEntitlement.Text = LKChannelEntitlement.ToString();
                    hdnLKChannelEntitlement.Value = LKChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK10y");
                RadioButton rbLK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK10y");
                if (ViewState["EKSelectedAvg"].ToString() == "10 year" && ViewState["LKSelectedAvg"].ToString() == "10 year" && rbEK10y.Checked && rbLK10y.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbEK7782_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtEKChannelEntitlement = (TextBox)gvr.FindControl("txtEKChannelEntitlement");
                    HiddenField hdnEKChannelEntitlement = (HiddenField)gvr.FindControl("hdnEKChannelEntitlement");

                    double EKPercentage7782 = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[Percentage7782EKIndex].ToString());
                    double EKProvincialEntitlement = Double.Parse(lblEKEntitlement.Text);
                    double EKChannelEntitlement = Math.Round((EKPercentage7782 * EKProvincialEntitlement) / 100, MAFRoundDigit);

                    EKChannelEntitlement = Math.Round(EKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + EKChannelEntitlement;

                    txtEKChannelEntitlement.Text = EKChannelEntitlement.ToString();
                    hdnEKChannelEntitlement.Value = EKChannelEntitlement.ToString();
                }

                Label lblTotalEKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
                lblTotalEKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK7782");
                RadioButton rbLK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK7782");
                if (ViewState["EKSelectedAvg"].ToString() == "1977-1982" && ViewState["LKSelectedAvg"].ToString() == "1977-1982" && rbLK7782.Checked && rbEK7782.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbLK7782_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double TotalChannelEntitlement = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtLKChannelEntitlement = (TextBox)gvr.FindControl("txtLKChannelEntitlement");
                    HiddenField hdnLKChannelEntitlement = (HiddenField)gvr.FindControl("hdnLKChannelEntitlement");

                    double LKPercentage7782 = Double.Parse(gvKharifAverage.DataKeys[gvr.RowIndex].Values[Percentage7782LKIndex].ToString());
                    double LKProvincialEntitlement = Double.Parse(lblLKEntitlement.Text);
                    double LKChannelEntitlement = Math.Round((LKPercentage7782 * LKProvincialEntitlement) / 100, MAFRoundDigit);

                    LKChannelEntitlement = Math.Round(LKChannelEntitlement, MAFRoundDigit);

                    TotalChannelEntitlement = TotalChannelEntitlement + LKChannelEntitlement;

                    txtLKChannelEntitlement.Text = LKChannelEntitlement.ToString();
                    hdnLKChannelEntitlement.Value = LKChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalChannelEntitlement);
                RadioButton rbEK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK7782");
                RadioButton rbLK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK7782");
                if (ViewState["EKSelectedAvg"].ToString() == "1977-1982"
                    && ViewState["LKSelectedAvg"].ToString() == "1977-1982"
                    && rbLK7782.Checked && rbEK7782.Checked
                    && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function saves the Rabi provincial and seasonal data.
        /// Created On 08-02-2017
        /// </summary>
        private void SaveRabiData()
        {
            double RabiProvincialEntitlement = Double.Parse(lblEntitlement.Text);

            Label lblTotalChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
            double GeneratedEntitlement = Double.Parse(lblTotalChannelEntitlement.Text);

            //if (RabiProvincialEntitlement < GeneratedEntitlement)
            //{
            //    Master.ShowMessage(Message.RabiShareGreater.Description, SiteMaster.MessageType.Error);
            //    return;
            //}

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            DateTime Now = DateTime.Now;

            int Year = Now.Year;
            //Year = 2014;

            if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) > Now)
            {
                Year = Year - 1;
            }

            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            long CommandID = Int64.Parse(ddlCommand.SelectedValue);

            SP_PlanScenario mdlPlanScenario = bllEntitlementDelivery.GetPlanScenario((long)Constants.Seasons.Rabi, Year, ddlScenario.SelectedItem.Text);

            ED_ProvincialEntitlement mdlProvincialEntitlement = new ED_ProvincialEntitlement
            {
                ID = Convert.ToInt64(ViewState[ProvincialEntitlementIDKey]),
                ChannelComndTypeID = CommandID,
                PlanDraftID = mdlPlanScenario.PlanDraftID,
                PlanScenarioID = mdlPlanScenario.ID,
                Year = Convert.ToInt16(Year),
                SeasonID = (short)Constants.Seasons.Rabi,
                ProvinceID = Constants.PunjabProvinceID,
                RabiMAF = RabiProvincialEntitlement,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = Convert.ToInt32(mdlUser.ID),
                ModifiedBy = Convert.ToInt32(mdlUser.ID)
            };

            if (mdlProvincialEntitlement.ID == 0)
            {
                bllEntitlementDelivery.AddProvincialEntitlement(mdlProvincialEntitlement);

                ViewState[ProvincialEntitlementIDKey] = mdlProvincialEntitlement.ID;
            }
            else
            {
                bllEntitlementDelivery.UpdateProvincialEntitlement(mdlProvincialEntitlement);
            }

            List<long> lstSeasonalAverageIDs = new List<long>();

            List<ED_SeasonalEntitlement> lstSeasonalEntitlement = new List<ED_SeasonalEntitlement>();

            foreach (GridViewRow gvr in gvRabiAverage.Rows)
            {
                long SeasonalAverageID = Convert.ToInt64(gvRabiAverage.DataKeys[gvr.RowIndex].Values[SeasonalAverageIDIndex]);
                lstSeasonalAverageIDs.Add(SeasonalAverageID);

                long CommandChannelID = Convert.ToInt64(gvRabiAverage.DataKeys[gvr.RowIndex].Values[RabiCommandChannelIDIndex]);

                ED_SeasonalEntitlement mdlSeasonalEntitlement = bllEntitlementDelivery.GetCommandSeasonalEntitlement(CommandChannelID, Year, (long)Constants.Seasons.Rabi);

                if (mdlSeasonalEntitlement != null && mdlSeasonalEntitlement.IsApproved == true)
                {
                    Master.ShowMessage(Message.EntitlementAlreadySaved.Description, SiteMaster.MessageType.Error);
                    return;
                }

                TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtChannelEntitlement");
                double ChannelEntitlement = Double.Parse(txtChannelEntitlement.Text);

                if (mdlSeasonalEntitlement == null)
                {
                    mdlSeasonalEntitlement = new ED_SeasonalEntitlement
                    {
                        CommandChannelID = CommandChannelID,
                        Year = Convert.ToInt16(Year),
                        SeasonID = (short)Constants.Seasons.Rabi,
                        PercentageEntitlement = ((ChannelEntitlement / GeneratedEntitlement) * 100),
                        MAFEntitlement = ChannelEntitlement,
                        IsApproved = false,
                        EntitlementSource = "Deliveries",
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(mdlUser.ID),
                        ModifiedBy = Convert.ToInt32(mdlUser.ID)
                    };

                    lstSeasonalEntitlement.Add(mdlSeasonalEntitlement);
                }
                else
                {
                    mdlSeasonalEntitlement.PercentageEntitlement = ((ChannelEntitlement / GeneratedEntitlement) * 100);
                    mdlSeasonalEntitlement.MAFEntitlement = ChannelEntitlement;
                    mdlSeasonalEntitlement.EntitlementSource = "Deliveries";
                    mdlSeasonalEntitlement.ModifiedDate = DateTime.Now;
                    mdlSeasonalEntitlement.ModifiedBy = Convert.ToInt32(mdlUser.ID);

                    bllEntitlementDelivery.UpdateSeasonalEntitlement(mdlSeasonalEntitlement);
                }
            }

            if (lstSeasonalEntitlement.Count != 0)
            {
                bllEntitlementDelivery.AddSeasonalEntitlements(lstSeasonalEntitlement);
            }

            string SelectedAverage = "5y";

            RadioButton rb10y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb10y");
            RadioButton rb7782 = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb7782");
            RadioButton rbYear = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rbSYear");

            if (rb10y.Checked)
            {
                SelectedAverage = "10y";
            }
            else if (rb7782.Checked)
            {
                SelectedAverage = "7782";
            }
            else if (rbYear.Checked)
            {
                SelectedAverage = "sy";
            }

            DropDownList ddlSelectedYear = (DropDownList)gvRabiAverage.HeaderRow.FindControl("ddlSelectedYear");

            short SelectedYear = Convert.ToInt16(ddlSelectedYear.SelectedValue);

            bllEntitlementDelivery.UpdateDeliveriesRabiAverageSelected(lstSeasonalAverageIDs, SelectedAverage, SelectedYear);

            bllEntitlementDelivery.GenerateEntitlements(Convert.ToInt16(Year), (short)Constants.Seasons.Rabi, CommandID, mdlUser.ID);

            litTitle.Text = "Entitlement on Deliveries";

            ddlCommand.ClearSelection();
            ddlCommand.SelectedIndex = 0;

            ddlCommand_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// This function saves the Kharif provincial and seasonal data.
        /// Created On 08-02-2017
        /// </summary>
        private void SaveKharifData()
        {
            double EKProvincialEntitlement = Double.Parse(lblEKEntitlement.Text);

            Label lblTotalEKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
            double EKGeneratedEntitlement = Double.Parse(lblTotalEKChannelEntitlement.Text);

            double LKProvincialEntitlement = Double.Parse(lblLKEntitlement.Text);

            Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
            double LKGeneratedEntitlement = Double.Parse(lblTotalLKChannelEntitlement.Text);

            //if (EKProvincialEntitlement < EKGeneratedEntitlement && LKProvincialEntitlement >= LKGeneratedEntitlement)
            //{
            //    Master.ShowMessage(Message.EKShareGreater.Description, SiteMaster.MessageType.Error);
            //    return;
            //}
            //else if (LKProvincialEntitlement < LKGeneratedEntitlement && EKProvincialEntitlement >= EKGeneratedEntitlement)
            //{
            //    Master.ShowMessage(Message.LKShareGreater.Description, SiteMaster.MessageType.Error);
            //    return;
            //}
            //else if (EKProvincialEntitlement < EKGeneratedEntitlement && LKProvincialEntitlement < LKGeneratedEntitlement)
            //{
            //    Master.ShowMessage(Message.KharifShareGreater.Description, SiteMaster.MessageType.Error);
            //    return;
            //}

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            DateTime Now = DateTime.Now;

            int Year = Now.Year;
            //Year = 2016;

            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            long CommandID = Int64.Parse(ddlCommand.SelectedValue);

            SP_PlanScenario mdlPlanScenario = bllEntitlementDelivery.GetPlanScenario((long)Constants.Seasons.Kharif, Year, ddlScenario.SelectedItem.Text);

            ED_ProvincialEntitlement mdlProvincialEntitlement = new ED_ProvincialEntitlement
            {
                ID = Convert.ToInt64(ViewState[ProvincialEntitlementIDKey]),
                ChannelComndTypeID = CommandID,
                PlanDraftID = mdlPlanScenario.PlanDraftID,
                PlanScenarioID = mdlPlanScenario.ID,
                Year = Convert.ToInt16(Year),
                SeasonID = (short)Constants.Seasons.Kharif,
                ProvinceID = Constants.PunjabProvinceID,
                EKMAF = EKProvincialEntitlement,
                LKMAF = LKProvincialEntitlement,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = Convert.ToInt32(mdlUser.ID),
                ModifiedBy = Convert.ToInt32(mdlUser.ID)
            };

            if (mdlProvincialEntitlement.ID == 0)
            {
                bllEntitlementDelivery.AddProvincialEntitlement(mdlProvincialEntitlement);

                ViewState[ProvincialEntitlementIDKey] = mdlProvincialEntitlement.ID;
            }
            else
            {
                bllEntitlementDelivery.UpdateProvincialEntitlement(mdlProvincialEntitlement);
            }

            List<long> lstSeasonalAverageIDs = new List<long>();

            List<ED_SeasonalEntitlement> lstSeasonalEntitlement = new List<ED_SeasonalEntitlement>();

            foreach (GridViewRow gvr in gvKharifAverage.Rows)
            {
                long EKSeasonalAverageID = Convert.ToInt64(gvKharifAverage.DataKeys[gvr.RowIndex].Values[EKSeasonalAverageIDIndex]);
                lstSeasonalAverageIDs.Add(EKSeasonalAverageID);

                long LKSeasonalAverageID = Convert.ToInt64(gvKharifAverage.DataKeys[gvr.RowIndex].Values[LKSeasonalAverageIDIndex]);
                lstSeasonalAverageIDs.Add(LKSeasonalAverageID);

                long CommandChannelID = Convert.ToInt64(gvKharifAverage.DataKeys[gvr.RowIndex].Values[KharifCommandChannelIDIndex]);

                ED_SeasonalEntitlement mdlEKSeasonalEntitlement = bllEntitlementDelivery.GetCommandSeasonalEntitlement(CommandChannelID, Year, (long)Constants.Seasons.EarlyKharif);

                if (mdlEKSeasonalEntitlement != null && mdlEKSeasonalEntitlement.IsApproved == true)
                {
                    Master.ShowMessage(Message.EntitlementAlreadySaved.Description, SiteMaster.MessageType.Error);
                    return;
                }

                ED_SeasonalEntitlement mdlLKSeasonalEntitlement = bllEntitlementDelivery.GetCommandSeasonalEntitlement(CommandChannelID, Year, (long)Constants.Seasons.LateKharif);

                if (mdlLKSeasonalEntitlement != null && mdlLKSeasonalEntitlement.IsApproved == true)
                {
                    Master.ShowMessage(Message.EntitlementAlreadySaved.Description, SiteMaster.MessageType.Error);
                    return;
                }

                TextBox txtEKChannelEntitlement = (TextBox)gvr.FindControl("txtEKChannelEntitlement");
                double EKChannelEntitlement = Double.Parse(txtEKChannelEntitlement.Text);

                TextBox txtLKChannelEntitlement = (TextBox)gvr.FindControl("txtLKChannelEntitlement");
                double LKChannelEntitlement = Double.Parse(txtLKChannelEntitlement.Text);

                if (mdlEKSeasonalEntitlement == null)
                {
                    mdlEKSeasonalEntitlement = new ED_SeasonalEntitlement
                    {
                        CommandChannelID = CommandChannelID,
                        Year = Convert.ToInt16(Year),
                        SeasonID = (short)Constants.Seasons.EarlyKharif,
                        PercentageEntitlement = ((EKChannelEntitlement / EKGeneratedEntitlement) * 100),
                        MAFEntitlement = EKChannelEntitlement,
                        EntitlementSource = "Deliveries",
                        IsApproved = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(mdlUser.ID),
                        ModifiedBy = Convert.ToInt32(mdlUser.ID)
                    };

                    lstSeasonalEntitlement.Add(mdlEKSeasonalEntitlement);
                }
                else
                {
                    mdlEKSeasonalEntitlement.PercentageEntitlement = ((EKChannelEntitlement / EKGeneratedEntitlement) * 100);
                    mdlEKSeasonalEntitlement.MAFEntitlement = EKChannelEntitlement;
                    mdlEKSeasonalEntitlement.EntitlementSource = "Deliveries";
                    mdlEKSeasonalEntitlement.ModifiedDate = DateTime.Now;
                    mdlEKSeasonalEntitlement.ModifiedBy = Convert.ToInt32(mdlUser.ID);

                    bllEntitlementDelivery.UpdateSeasonalEntitlement(mdlEKSeasonalEntitlement);
                }

                if (mdlLKSeasonalEntitlement == null)
                {
                    mdlLKSeasonalEntitlement = new ED_SeasonalEntitlement
                    {
                        CommandChannelID = CommandChannelID,
                        Year = Convert.ToInt16(Year),
                        SeasonID = (short)Constants.Seasons.LateKharif,
                        PercentageEntitlement = ((LKChannelEntitlement / LKGeneratedEntitlement) * 100),
                        MAFEntitlement = LKChannelEntitlement,
                        EntitlementSource = "Deliveries",
                        IsApproved = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(mdlUser.ID),
                        ModifiedBy = Convert.ToInt32(mdlUser.ID)
                    };

                    lstSeasonalEntitlement.Add(mdlLKSeasonalEntitlement);
                }
                else
                {
                    mdlLKSeasonalEntitlement.PercentageEntitlement = ((LKChannelEntitlement / LKGeneratedEntitlement) * 100);
                    mdlLKSeasonalEntitlement.MAFEntitlement = LKChannelEntitlement;
                    mdlLKSeasonalEntitlement.EntitlementSource = "Deliveries";
                    mdlLKSeasonalEntitlement.ModifiedDate = DateTime.Now;
                    mdlLKSeasonalEntitlement.ModifiedBy = Convert.ToInt32(mdlUser.ID);

                    bllEntitlementDelivery.UpdateSeasonalEntitlement(mdlLKSeasonalEntitlement);
                }
            }

            if (lstSeasonalEntitlement.Count != 0)
            {
                lstSeasonalEntitlement = lstSeasonalEntitlement.OrderBy(se => se.SeasonID).ThenBy(se => se.CommandChannelID).ToList();

                bllEntitlementDelivery.AddSeasonalEntitlements(lstSeasonalEntitlement);
            }

            string EKSelectedAverage = "5y";
            string LKSelectedAverage = "5y";

            RadioButton rbEK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK10y");
            RadioButton rbLK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK10y");
            RadioButton rbEK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK7782");
            RadioButton rbLK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK7782");
            RadioButton rbSYearEK = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEKYear");
            RadioButton rbSYearLK = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLKYear");

            if (rbEK10y.Checked)
            {
                EKSelectedAverage = "10y";
            }
            else if (rbEK7782.Checked)
            {
                EKSelectedAverage = "7782";
            }
            else if (rbSYearEK.Checked)
            {
                EKSelectedAverage = "sy";
            }

            if (rbLK10y.Checked)
            {
                LKSelectedAverage = "10y";
            }
            else if (rbLK7782.Checked)
            {
                LKSelectedAverage = "7782";
            }
            else if (rbSYearLK.Checked)
            {
                EKSelectedAverage = "sy";
            }

            DropDownList ddlSelectedYearEK = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlEKSelectedYear");
            short SelectedYearEK = Convert.ToInt16(ddlSelectedYearEK.SelectedValue);
            DropDownList ddlSelectedYearLK = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlLKSelectedYear");
            short SelectedYearLK = Convert.ToInt16(ddlSelectedYearLK.SelectedValue);

            bllEntitlementDelivery.UpdateKharifAverageSelectedDeliveries(lstSeasonalAverageIDs, EKSelectedAverage, LKSelectedAverage, SelectedYearEK, SelectedYearLK);

            bllEntitlementDelivery.GenerateEntitlements(Convert.ToInt16(Year), (short)Constants.Seasons.Kharif, CommandID, mdlUser.ID);

            litTitle.Text = "Entitlement on Deliveries";

            ddlCommand.ClearSelection();
            ddlCommand.SelectedIndex = 0;

            ddlCommand_SelectedIndexChanged(null, null);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                int CommandType = Convert.ToInt32(ddlCommand.SelectedItem.Value);
                int Year = System.DateTime.Now.Year;
                int Month = System.DateTime.Now.Month;

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("CommandType", CommandType.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                string Scenario = ddlScenario.SelectedItem.Text;
                ReportParameter = new ReportParameter("Scenario", Scenario);
                mdlReportData.Parameters.Add(ReportParameter);

                string EKCol = "";

                if (gvKharifAverage.Visible)
                {
                    RadioButton rbEK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK5y");
                    RadioButton rbEK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK10y");
                    RadioButton rbEK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEK7782");
                    RadioButton rbEKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEKYear");
                    DropDownList ddlEKSelectedYear = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlEKSelectedYear");

                    ReportParameter = new ReportParameter("Year", Year.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    string LKCol = "";

                    if (rbEK5y.Checked)
                    {
                        EKCol = "5-Year Average";
                    }
                    else if (rbEK10y.Checked)
                    {
                        EKCol = "10-Year Average";
                    }
                    else if (rbEK7782.Checked)
                    {
                        EKCol = "1977-1982 Average";
                    }
                    else if (rbEKYear.Checked)
                    {
                        EKCol = "Year" + Convert.ToString(ddlEKSelectedYear.SelectedValue);
                    }

                    RadioButton rbLK5y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK5y");
                    RadioButton rbLK10y = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK10y");
                    RadioButton rbLK7782 = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLK7782");
                    RadioButton rbLKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLKYear");
                    DropDownList ddlLKSelectedYear = (DropDownList)gvKharifAverage.HeaderRow.FindControl("ddlLKSelectedYear");

                    if (rbLK5y.Checked)
                    {
                        LKCol = "5-Year Average";
                    }
                    else if (rbLK10y.Checked)
                    {
                        LKCol = "10-Year Average";
                    }
                    else if (rbLK7782.Checked)
                    {
                        LKCol = "1977-1982 Average";
                    }
                    else if (rbLKYear.Checked)
                    {
                        EKCol = "Year" + Convert.ToString(ddlLKSelectedYear.SelectedValue);
                    }

                    ReportParameter = new ReportParameter("EKCol", EKCol.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("LKCol", LKCol.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.EntitlementAndDeliveriesKharif;
                }
                else
                {
                    RadioButton rb5y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb5y");
                    RadioButton rb10y = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb10y");
                    RadioButton rb7782 = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rb7782");
                    RadioButton rbSYear = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rbSYear");
                    DropDownList ddlSelectedYear = (DropDownList)gvRabiAverage.HeaderRow.FindControl("ddlSelectedYear");

                    if (new DateTime(DateTime.Now.Year, Constants.KharifEntitlementStartMonth,
                            Constants.KharifEntitlementStartDay) > DateTime.Now)
                    {
                        ReportParameter = new ReportParameter("Year", (Year - 1).ToString());
                    }
                    else
                    {
                        ReportParameter = new ReportParameter("Year", Year.ToString());
                    }
                    mdlReportData.Parameters.Add(ReportParameter);
                    if (rb5y.Checked)
                    {
                        EKCol = "5-Year Average";
                    }
                    else if (rb10y.Checked)
                    {
                        EKCol = "10-Year Average";
                    }
                    else if (rb7782.Checked)
                    {
                        EKCol = "1977-1982 Average";
                    }
                    else if (rbSYear.Checked)
                    {
                        DateTime Yearplus = new DateTime(int.Parse(ddlSelectedYear.SelectedValue), 1, 1).AddYears(1);
                        EKCol = "Year " + Convert.ToInt64(ddlSelectedYear.SelectedValue) + "-" + Yearplus.ToString("yyyy");
                    }
                    ReportParameter = new ReportParameter("EKCol", EKCol.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.EntitlementAndDeliveriesRabi;
                }

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbSYear_CheckedChanged(object sender, EventArgs e)
        {
            RabiCheckFunctionality();
        }

        public void RabiCheckFunctionality()
        {
            try
            {
                double TotalYearlyEntitlemt = 0;

                foreach (GridViewRow gvr in gvRabiAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnChannelEntitlement");

                    Label Percentage = (Label)gvr.FindControl("lblSYearPer");
                    double ProvincialEntitlement = Double.Parse(lblEntitlement.Text);

                    double ChannelEntitlement = Math.Round((Double.Parse(Percentage.Text) * ProvincialEntitlement) / 100, MAFRoundDigit);
                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);
                    TotalYearlyEntitlemt = TotalYearlyEntitlemt + ChannelEntitlement;
                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvRabiAverage.FooterRow.FindControl("lblTotalChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalYearlyEntitlemt);

                RadioButton rbYear = (RadioButton)gvRabiAverage.HeaderRow.FindControl("rbSYear");
                if (Convert.ToString(ViewState["SelectedAvg"]) == Convert.ToString(ViewState["SelectedYear"]) && rbYear.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {

                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSelectedYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long CommandID = Convert.ToInt64(ddlCommand.SelectedValue);
                DropDownList ddl = (DropDownList)sender;

                double TotalMAF = 0;

                List<dynamic> lstValues = new EntitlementDeliveryBLL().GetSelectedYearDeliveries(Convert.ToInt64(ddl.SelectedValue), (long)Constants.Seasons.Rabi, CommandID);

                foreach (GridViewRow row in gvRabiAverage.Rows)
                {
                    Label lblMAF = (Label)row.FindControl("lblSYearMAF");
                    Label lblPer = (Label)row.FindControl("lblSYearPer");

                    int CommandChannelID = Convert.ToInt32(gvRabiAverage.DataKeys[row.RowIndex].Values[RabiCommandChannelIDIndex].ToString());

                    object obj;

                    if (lstValues != null && lstValues.Count > 0)
                    {
                        obj = lstValues.Find(x => Convert.ToInt32(x.GetType().GetProperty("CommandChannelID").GetValue(x)) == CommandChannelID);

                        if (obj != null)
                        {
                            double MAF = Convert.ToDouble(obj.GetType().GetProperty("SelectedYearMAF").GetValue(obj));
                            MAF = Math.Round(MAF, MAFRoundDigit);
                            lblPer.Text = obj.GetType().GetProperty("SelectedYearPercentage").GetValue(obj).ToString();
                            lblMAF.Text = String.Format("{0:0.000}", MAF);
                            TotalMAF = TotalMAF + MAF;
                        }
                        else
                        {
                            lblMAF.Text = "0.000";
                            lblPer.Text = "0";
                        }
                    }
                    else
                    {
                        lblMAF.Text = "0.000";
                        lblPer.Text = "0";
                    }
                }

                Label FooterRow = (Label)(gvRabiAverage.FooterRow.FindControl("lblSYearTotalMAF"));

                if (TotalMAF > 0)
                {
                    FooterRow.Text = String.Format("{0:0.000}", Math.Round(TotalMAF, MAFRoundDigit));
                }
                else
                {
                    FooterRow.Text = "0.000";
                }

                if (((RadioButton)gvRabiAverage.HeaderRow.FindControl("rbSYear")).Checked)
                {
                    rbSYear_CheckedChanged(null, null);
                }
                btnPrint.Visible = ddl.SelectedItem.Text == Convert.ToString(ViewState["SelectedYear"]).Substring(5);




            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbEKYear_CheckedChanged(object sender, EventArgs e)
        {
            EKCheckFunctionality();
        }

        protected void ddlEKSelectedYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem liCommand = ddlCommand.SelectedItem;
                string CommandName = liCommand.Text;
                long CommandID = Int64.Parse(liCommand.Value);
                DropDownList ddl = (DropDownList)sender;

                double MAF = 0;

                List<dynamic> lstValues = new EntitlementDeliveryBLL().GetSelectedYearDeliveries(Convert.ToInt64(ddl.SelectedValue), (long)Constants.Seasons.EarlyKharif, CommandID);

                foreach (GridViewRow row in gvKharifAverage.Rows)
                {
                    Label lblMAF = (Label)row.FindControl("lblEKYearMAF");
                    Label lblPer = (Label)row.FindControl("lblEKYearPer");

                    if (lstValues != null && lstValues.Count() > 0)
                    {
                        long ChannelID = Convert.ToInt64(gvKharifAverage.DataKeys[row.RowIndex].Values[KharifCommandChannelIDIndex].ToString());
                        object Value = lstValues.Where(q => (q.GetType().GetProperty("CommandChannelID").GetValue(q)) == ChannelID).FirstOrDefault();

                        if (Value != null)
                        {

                            Double MAFValue = Math.Round(Convert.ToDouble(Value.GetType().GetProperty("SelectedYearMAF").GetValue(Value)), MAFRoundDigit);
                            lblMAF.Text = String.Format("{0:0.000}", MAFValue);
                            lblPer.Text = Convert.ToString(Value.GetType().GetProperty("SelectedYearPercentage").GetValue(Value));
                            MAF = MAF + Convert.ToDouble(lblMAF.Text);
                        }
                        else
                        {
                            lblMAF.Text = "0.000";
                            lblPer.Text = "0";
                            MAF = MAF + 0;
                        }
                    }
                    else
                    {
                        lblMAF.Text = "0.000";
                        lblPer.Text = "0";
                        MAF = MAF + 0;
                    }

                    //if (row.RowIndex < lstValues.Count())
                    //{
                    //    lblMAF.Text = String.Format("{0:0.000}", (Math.Round((lstValues[row.RowIndex].GetType().GetProperty("SelectedYearMAF").GetValue(lstValues[row.RowIndex])), MAFRoundDigit)));
                    //    lblPer.Text = Convert.ToString(lstValues[row.RowIndex].GetType().GetProperty("SelectedYearPercentage").GetValue(lstValues[row.RowIndex]));
                    //    MAF = MAF + Convert.ToDouble(String.Format("{0:0.000}", (Math.Round((lstValues[row.RowIndex].GetType().GetProperty("SelectedYearMAF").GetValue(lstValues[row.RowIndex])), MAFRoundDigit))));
                    //}
                    //else
                    //{
                    //    lblMAF.Text = "0.000";
                    //    lblPer.Text = "0";
                    //    MAF = MAF + 0;
                    //}
                }
                ViewState["btnPrintVisibleEK"] = Convert.ToString(ddl.SelectedItem.Text);
                Label FooterRow = (Label)(gvKharifAverage.FooterRow.FindControl("lblEKYearTotalMAF"));
                FooterRow.Text = String.Format("{0:0.000}", Math.Round(MAF, MAFRoundDigit));

                CheckBox cb = (CheckBox)gvKharifAverage.HeaderRow.FindControl("rbEKYear");
                if (cb.Checked)
                    EKCheckFunctionality();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EKCheckFunctionality()
        {
            try
            {
                double TotalYearlyEntitlemt = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtEKChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnEKChannelEntitlement");
                    //double PercentageYearlyEntitlemen = Double.Parse(gvRabiAverage.DataKeys[gvr.RowIndex].Values[PercentageYearlyEntitlementIndex].ToString());
                    Label Percentage = (Label)gvr.FindControl("lblEKYearPer");
                    double ProvincialEntitlement = Double.Parse(lblEKEntitlement.Text);
                    //double ChannelEntitlement = Math.Round((PercentageYearlyEntitlemen * ProvincialEntitlement) / 100, MAFRoundDigit);
                    double ChannelEntitlement = Math.Round((Double.Parse(Percentage.Text) * ProvincialEntitlement) / 100, MAFRoundDigit);
                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);
                    TotalYearlyEntitlemt = TotalYearlyEntitlemt + ChannelEntitlement;
                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalEKChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalYearlyEntitlemt);
                RadioButton rbEKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEKYear");
                RadioButton rbLKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLKYear");
                if (Convert.ToString(ViewState["EKSelectedAvg"]) == Convert.ToString(ViewState["EKSelectedYear"])
                    && Convert.ToString(ViewState["LKSelectedAvg"]) == Convert.ToString(ViewState["LKSelectedYear"])
                    && rbEKYear.Checked && rbLKYear.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = Convert.ToString(ViewState["btnPrintVisibleEK"]) == Convert.ToString(ViewState["EKSelectedYear"]).Substring(5)
                                       && Convert.ToString(ViewState["btnPrintVisibleLK"]) == Convert.ToString(ViewState["LKSelectedYear"]).Substring(5);
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbLKYear_CheckedChanged(object sender, EventArgs e)
        {
            LKCheckFunctionality();
        }

        public void LKCheckFunctionality()
        {
            try
            {
                double TotalYearlyEntitlemt = 0;

                foreach (GridViewRow gvr in gvKharifAverage.Rows)
                {
                    TextBox txtChannelEntitlement = (TextBox)gvr.FindControl("txtLKChannelEntitlement");
                    HiddenField hdnChannelEntitlement = (HiddenField)gvr.FindControl("hdnLKChannelEntitlement");
                    //double PercentageYearlyEntitlemen = Double.Parse(gvRabiAverage.DataKeys[gvr.RowIndex].Values[PercentageYearlyEntitlementIndex].ToString());
                    Label Percentage = (Label)gvr.FindControl("lblLKYearPer");
                    double ProvincialEntitlement = Double.Parse(lblLKEntitlement.Text);
                    //double ChannelEntitlement = Math.Round((PercentageYearlyEntitlemen * ProvincialEntitlement) / 100, MAFRoundDigit);
                    double ChannelEntitlement = Math.Round((Double.Parse(Percentage.Text) * ProvincialEntitlement) / 100, MAFRoundDigit);
                    ChannelEntitlement = Math.Round(ChannelEntitlement, MAFRoundDigit);
                    TotalYearlyEntitlemt = TotalYearlyEntitlemt + ChannelEntitlement;
                    txtChannelEntitlement.Text = ChannelEntitlement.ToString();
                    hdnChannelEntitlement.Value = ChannelEntitlement.ToString();
                }

                Label lblTotalLKChannelEntitlement = (Label)gvKharifAverage.FooterRow.FindControl("lblTotalLKChannelEntitlement");
                lblTotalLKChannelEntitlement.Text = String.Format("{0:0.000}", TotalYearlyEntitlemt);
                RadioButton rbEKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbEKYear");
                RadioButton rbLKYear = (RadioButton)gvKharifAverage.HeaderRow.FindControl("rbLKYear");
                if (Convert.ToString(ViewState["EKSelectedAvg"]) == Convert.ToString(ViewState["EKSelectedYear"]) &&
                    Convert.ToString(ViewState["LKSelectedAvg"]) == Convert.ToString(ViewState["LKSelectedYear"]) &&
                    rbEKYear.Checked && rbLKYear.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {
                    btnPrint.Visible = Convert.ToString(ViewState["btnPrintVisibleEK"]) == Convert.ToString(ViewState["EKSelectedYear"]).Substring(5)
                                       && Convert.ToString(ViewState["btnPrintVisibleLK"]) == Convert.ToString(ViewState["LKSelectedYear"]).Substring(5);
                }
                else
                {
                    btnPrint.Visible = false;
                }









                if (Convert.ToString(ViewState["LKSelectedAvg"]) == Convert.ToString(ViewState["LKSelectedYear"]) && rbLKYear.Checked && Convert.ToString(ViewState["ESource"]) == "Deliveries")
                {

                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlLKSelectedYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem liCommand = ddlCommand.SelectedItem;
                string CommandName = liCommand.Text;
                long CommandID = Int64.Parse(liCommand.Value);
                DropDownList ddl = (DropDownList)sender;
                double MAF = 0;

                List<dynamic> lstValues = new EntitlementDeliveryBLL().GetSelectedYearDeliveries(Convert.ToInt64(ddl.SelectedValue), (long)Constants.Seasons.LateKharif, CommandID);

                foreach (GridViewRow row in gvKharifAverage.Rows)
                {
                    Label lblMAF = (Label)row.FindControl("lblLKYearMAF");
                    Label lblPer = (Label)row.FindControl("lblLKYearPer");

                    if (lstValues != null && lstValues.Count() > 0)
                    {
                        long ChannelID = Convert.ToInt64(gvKharifAverage.DataKeys[row.RowIndex].Values[KharifCommandChannelIDIndex].ToString());
                        dynamic Value = lstValues.Where(q => q.CommandChannelID == ChannelID).FirstOrDefault();

                        if (Value != null)
                        {
                            Double MAFValue = Math.Round((Value.GetType().GetProperty("SelectedYearMAF").GetValue(Value)), MAFRoundDigit);
                            lblMAF.Text = String.Format("{0:0.000}", MAFValue);
                            lblPer.Text = Convert.ToString(Value.GetType().GetProperty("SelectedYearPercentage").GetValue(Value));
                            MAF = MAF + Convert.ToDouble(String.Format("{0:0.000}", lblMAF.Text));
                        }
                        else
                        {
                            lblMAF.Text = "0.000";
                            lblPer.Text = "0";
                            MAF = MAF + 0;
                        }
                    }
                    else
                    {
                        lblMAF.Text = "0.000";
                        lblPer.Text = "0";
                        MAF = MAF + 0;
                    }

                    //if (row.RowIndex < lstValues.Count())
                    //{
                    //    lblMAF.Text = String.Format("{0:0.000}", (Math.Round((lstValues[row.RowIndex].GetType().GetProperty("SelectedYearMAF").GetValue(lstValues[row.RowIndex])), MAFRoundDigit)));
                    //    lblPer.Text = Convert.ToString(lstValues[row.RowIndex].GetType().GetProperty("SelectedYearPercentage").GetValue(lstValues[row.RowIndex]));
                    //    MAF = MAF + Convert.ToDouble(String.Format("{0:0.000}", (Math.Round((lstValues[row.RowIndex].GetType().GetProperty("SelectedYearMAF").GetValue(lstValues[row.RowIndex])), MAFRoundDigit))));
                    //}
                    //else
                    //{
                    //    lblMAF.Text = "0.000";
                    //    lblPer.Text = "0";
                    //    MAF = MAF + 0;
                    //}
                }

                ViewState["btnPrintVisibleLK"] = Convert.ToString(ddl.SelectedItem.Text);
                Label FooterRow = (Label)(gvKharifAverage.FooterRow.FindControl("lblLKYearTotalMAF"));
                FooterRow.Text = String.Format("{0:0.000}", Math.Round(MAF, MAFRoundDigit));//Convert.ToString(Math.Round(MAF, MAFRoundDigit));

                CheckBox cb = (CheckBox)gvKharifAverage.HeaderRow.FindControl("rbLKYear");
                if (cb.Checked)
                    LKCheckFunctionality();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}