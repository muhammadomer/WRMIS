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
    public partial class EditTDailyEntitlements : BasePage
    {
        #region Screen Constants

        public const int PercentageRoundDigit = 2;
        public const int MAFRoundDigit = 3;

        #endregion

        string MaskedValue = "";
        double MAF = 0;
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
        /// This function gets the Rabi Data to fill the screen.
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_ScenarioID"></param>
        /// <param name="_CommandName"></param>
        /// <param name="_CurrentYear"></param>
        private void FillRabiData(string _CommandName, int _CurrentYear, long _CommandID)
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            //IEnumerable<DataRow> ieRabiShare = bllEntitlementDelivery.GetRabiShare(_CurrentYear, _ScenarioID);

            ED_ProvincialEntitlement mdlProvincialEntitlement = bllEntitlementDelivery.GetProvincialEntitlement(_CommandID, _CurrentYear, (long)Constants.Seasons.Rabi, Constants.PunjabProvinceID);

            lblScenario.Text = mdlProvincialEntitlement.SP_PlanScenario.Scenario;
            //List<dynamic> lstRabiShare = ieRabiShare.Select(dataRow => new
            //{
            //    TDailyID = dataRow.Field<short>("TDailyID"),
            //    ShortName = dataRow.Field<string>("ShortName"),
            //    Cusecs = dataRow.Field<double>("PunjabAS")
            //}).ToList<dynamic>();

            int NextYear = _CurrentYear + 1;

            double RabiMAF = mdlProvincialEntitlement.RabiMAF.Value;

            lblMainDesc.Text = string.Format("{0} Entitlement for {1} {2}-{3}", _CommandName, "Rabi", _CurrentYear, NextYear);

            List<dynamic> lstCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.Rabi, _CommandID);

            double Average7782 = Utility.GetMAF(lstCusecs, NextYear);
            double Average7782Rounded = Math.Round(Average7782, MAFRoundDigit);
            lbl7782Average.Text = String.Format("{0:0.000}", Average7782Rounded);

            lblEntitlementText.Text = string.Format("Entitlement for {0} {1}-{2} (MAF):", "Rabi", _CurrentYear, NextYear);
            double RabiMAFRounded = Math.Round(RabiMAF, MAFRoundDigit);
            lblEntitlement.Text = String.Format("{0:0.000}", RabiMAFRounded);

            double PercentChange = ((RabiMAFRounded / Average7782Rounded) * 100) - 100;
            double PercentChangeRounded = Math.Round(PercentChange, PercentageRoundDigit);
            lblPercentChange.Text = String.Format("{0:0.00}", PercentChangeRounded);

            List<dynamic> lstPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.Rabi, _CommandID);

            double Para2 = Utility.GetMAF(lstPara2Cs, NextYear);
            double Para2Rounded = Math.Round(Para2, MAFRoundDigit);
            lblPara2.Text = String.Format("{0:0.000}", Para2Rounded);

            pnlRabiHeader.Visible = true;

            lblMainDesc.Visible = true;

            dvScenario.Visible = true;
            lblScenario.Visible = true;
        }

        /// <summary>
        /// This function gets the Kharif Data to fill the screen.
        /// Created On 31-01-2017
        /// </summary>
        /// <param name="_ScenarioID"></param>
        /// <param name="_CommandName"></param>
        /// <param name="_CurrentYear"></param>
        /// <param name="_CommandID"></param>
        private void FillKharifData(string _CommandName, int _CurrentYear, long _CommandID)
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            //IEnumerable<DataRow> ieKharifShare = bllEntitlementDelivery.GetKharifShare(_CurrentYear, _ScenarioID);

            //List<dynamic> lstEarlyKharifShare = ieKharifShare.Select(dataRow => new
            //{
            //    TDailyID = dataRow.Field<short>("TDailyID"),
            //    ShortName = dataRow.Field<string>("shortName"),
            //    Cusecs = dataRow.Field<double>("PunjabIndusShare")
            //}).Where(dr => dr.TDailyID <= 7).ToList<dynamic>();            

            ED_ProvincialEntitlement mdlProvincialEntitlement = bllEntitlementDelivery.GetProvincialEntitlement(_CommandID, _CurrentYear, (long)Constants.Seasons.Kharif, Constants.PunjabProvinceID);

            lblScenario.Text = mdlProvincialEntitlement.SP_PlanScenario.Scenario;

            lblMainDesc.Text = string.Format("{0} Entitlement for {1} {2}", _CommandName, "Kharif", _CurrentYear);

            double EKMAF = mdlProvincialEntitlement.EKMAF.Value;

            List<dynamic> lstEKCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.EarlyKharif, _CommandID);

            double AverageEK7782 = Utility.GetMAF(lstEKCusecs, _CurrentYear);
            double AverageEK7782Rounded = Math.Round(AverageEK7782, MAFRoundDigit);
            lbl7782EKAverage.Text = String.Format("{0:0.000}", AverageEK7782Rounded);

            lblEKEntitlementText.Text = string.Format("Entitlement for {0} {1} (MAF):", "Early Kharif", _CurrentYear);
            double EKMAFRounded = Math.Round(EKMAF, MAFRoundDigit);
            lblEKEntitlement.Text = String.Format("{0:0.000}", EKMAFRounded);

            double EKPercentChange = ((EKMAFRounded / AverageEK7782Rounded) * 100) - 100;
            lblEKPercentChange.Text = (Math.Round(EKPercentChange, PercentageRoundDigit)).ToString();

            List<dynamic> lstEKPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.EarlyKharif, _CommandID);

            double EKPara2 = Utility.GetMAF(lstEKPara2Cs, _CurrentYear);
            double EKPara2Rounded = Math.Round(EKPara2, MAFRoundDigit);
            lblEKPara2.Text = String.Format("{0:0.000}", EKPara2Rounded);

            //List<dynamic> lstLateKharifShare = ieKharifShare.Select(dataRow => new
            //{
            //    TDailyID = dataRow.Field<short>("TDailyID"),
            //    ShortName = dataRow.Field<string>("shortName"),
            //    Cusecs = dataRow.Field<double>("PunjabIndusShare")
            //}).Where(dr => dr.TDailyID > 7).ToList<dynamic>();

            double LKMAF = mdlProvincialEntitlement.LKMAF.Value;

            List<dynamic> lstLKCusecs = bllEntitlementDelivery.Get7782AverageCommandSeasonalCusecs((long)Constants.Seasons.LateKharif, _CommandID);

            double AverageLK7782 = Utility.GetMAF(lstLKCusecs, _CurrentYear);
            double AverageLK7782Rounded = Math.Round(AverageLK7782, MAFRoundDigit);
            lbl7782LKAverage.Text = String.Format("{0:0.000}", AverageLK7782Rounded);

            lblLKEntitlementText.Text = string.Format("Entitlement for {0} {1} (MAF):", "Late Kharif", _CurrentYear);
            double LKMAFRounded = Math.Round(LKMAF, MAFRoundDigit);
            lblLKEntitlement.Text = String.Format("{0:0.000}", LKMAFRounded);

            double LKPercentChange = ((LKMAFRounded / AverageLK7782Rounded) * 100) - 100;
            lblLKPercentChange.Text = (Math.Round(LKPercentChange, PercentageRoundDigit)).ToString();

            List<dynamic> lstLKPara2Cs = bllEntitlementDelivery.GetPara2AverageCommandSeasonalMAF((long)Constants.Seasons.LateKharif, _CommandID);

            double LKPara2 = Utility.GetMAF(lstLKPara2Cs, _CurrentYear);
            double LKPara2Rounded = Math.Round(LKPara2, MAFRoundDigit);
            lblLKPara2.Text = String.Format("{0:0.000}", LKPara2Rounded);

            pnlKharifHeader.Visible = true;

            lblMainDesc.Visible = true;

            dvScenario.Visible = true;
            lblScenario.Visible = true;
        }


        /// <summary>
        /// This function converts the Cusecs value to MAF
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_LstTDailyCusecs"></param>
        /// <param name="_Year"></param>
        /// <returns>double</returns>
        private double GetMAF(List<dynamic> _LstTDailyCusecs, int _Year)
        {
            double TotalCusecs = 0;

            foreach (dynamic TDailyCusecs in _LstTDailyCusecs)
            {
                int TDailyID = TDailyCusecs.TDailyID;
                string ShortName = TDailyCusecs.ShortName;

                string MonthName = ShortName.Remove(ShortName.Length - 1);
                int Month = DateTime.Parse(string.Format("{0} {1},{2}", 1, MonthName, _Year)).Month;

                if (TDailyID % 3 == 0)
                {
                    int DaysInMonth = DateTime.DaysInMonth(_Year, Month);

                    if (DaysInMonth == 31)
                    {
                        TotalCusecs = TotalCusecs + (TDailyCusecs.Cusecs * Constants.TDailyConversion);
                    }
                    else if (DaysInMonth == 30)
                    {
                        TotalCusecs = TotalCusecs + TDailyCusecs.Cusecs;
                    }
                    else if (DaysInMonth == 29)
                    {
                        TotalCusecs = TotalCusecs + (TDailyCusecs.Cusecs * Constants.LeapYearTrue);
                    }
                    else if (DaysInMonth == 28)
                    {
                        TotalCusecs = TotalCusecs + (TDailyCusecs.Cusecs * Constants.LeapYearFalse);
                    }
                }
                else
                {
                    TotalCusecs = TotalCusecs + TDailyCusecs.Cusecs;
                }
            }

            return TotalCusecs * Constants.MAFConversion;
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
                if (ddlCommand.SelectedItem.Value != string.Empty)
                {
                    DateTime Now = DateTime.Now;

                    //int ScenarioID = Int32.Parse(ddlScenario.SelectedValue);
                    ListItem liCommand = ddlCommand.SelectedItem;
                    string CommandName = liCommand.Text;
                    long CommandID = Int64.Parse(liCommand.Value);

                    EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

                    //// Kharif Entitlement days are between 11 March and 30 April
                    //if (new DateTime(Year, 3, 11) <= Now && new DateTime(Year, 4, 30) >= Now)
                    //{
                    //    FillKharifData(CommandName, Now.Year, CommandID);
                    //}
                    //// Rabi Entitlement days are between 11 September and 31 October
                    //else if (new DateTime(Year, 9, 11) <= Now && new DateTime(Year, 10, 31) >= Now)
                    //{

                    //    FillRabiData(CommandName, Year, CommandID);
                    //}
                    //else
                    //{
                    //    FillRabiData(CommandName, Year, CommandID);
                    //}






                    //// Kharif Entitlement days are between 11 March and 30 April
                    //if (new DateTime(Now.Year, 3, 11) <= Now && new DateTime(Now.Year, 4, 30) >= Now)
                    //{
                    //    FillKharifData( CommandName, Now.Year, CommandID);
                    //BindGrid(Now.Year, (long)Constants.Seasons.Kharif, CommandID);
                    //}
                    //// Rabi Entitlement days are between 11 September and 31 October
                    //else if (new DateTime(Now.Year, 9, 11) <= Now && new DateTime(Now.Year, 10, 31) >= Now)
                    //{
                    //    FillRabiData( CommandName, Now.Year, CommandID);
                    //BindGrid(Now.Year, (long)Constants.Seasons.Rabi, CommandID);
                    //}
                    //else
                    //{
                    //    FillRabiData( CommandName, Now.Year, CommandID);
                    //BindGrid(Now.Year, (long)Constants.Seasons.Rabi, CommandID);
                    //}

                    // Kharif
                    if (new DateTime(Now.Year, 3, 11) <= Now && new DateTime(Now.Year, 9, 10) >= Now)
                    {
                        FillKharifData(CommandName, Now.Year, CommandID);
                        BindGrid(Now.Year, (long)Constants.Seasons.Kharif, CommandID);
                    }
                    else
                    {
                        if (new DateTime(Now.Year, 3, 10) >= Now)
                        {
                            FillRabiData(CommandName, Now.Year - 1, CommandID);
                            BindGrid(Now.Year - 1, (long)Constants.Seasons.Rabi, CommandID);
                        }
                        else
                        {
                            FillRabiData(CommandName, Now.Year, CommandID);
                            BindGrid(Now.Year, (long)Constants.Seasons.Rabi, CommandID);
                        }
                    }


                    btnSave.Visible = true;
                }
                else
                {
                    gvTenDailyEntitlementsIndus.Visible = false;
                    btnSave.Visible = false;
                    lblMainDesc.Visible = false;
                    pnlRabiHeader.Visible = false;
                    pnlKharifHeader.Visible = false;
                    lblScenario.Visible = false;
                    dvScenario.Visible = false;
                }

            }
            catch (Exception exp)
            {
                gvTenDailyEntitlementsIndus.Visible = false;
                btnSave.Visible = false;
                lblMainDesc.Visible = false;
                pnlRabiHeader.Visible = false;
                pnlKharifHeader.Visible = false;
                lblScenario.Visible = false;
                dvScenario.Visible = false;

                Master.ShowMessage(Message.NoDataExists.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }



        protected void gvTenDailyEntitlementsIndus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    Label lblTenDaily = (Label)e.Row.FindControl("lblTenDaily");

                    TextBox txtHD = (TextBox)e.Row.FindControl("txtHD");
                    Label lblHD = (Label)e.Row.FindControl("lblHD");

                    TextBox txtSC = (TextBox)e.Row.FindControl("txtSC");
                    Label lblSC = (Label)e.Row.FindControl("lblSC");

                    TextBox txtRC = (TextBox)e.Row.FindControl("txtRC");
                    Label lblRC = (Label)e.Row.FindControl("lblRC");

                    TextBox txtPML = (TextBox)e.Row.FindControl("txtPML");
                    Label lblPML = (Label)e.Row.FindControl("lblPML");

                    TextBox txtAC = (TextBox)e.Row.FindControl("txtAC");
                    Label lblAC = (Label)e.Row.FindControl("lblAC");

                    TextBox txtTCMLU = (TextBox)e.Row.FindControl("txtTCMLU");
                    Label lblTCMLU = (Label)e.Row.FindControl("lblTCMLU");

                    TextBox txtPCL = (TextBox)e.Row.FindControl("txtPCL");
                    Label lblPCL = (Label)e.Row.FindControl("lblPCL");

                    TextBox txtMC = (TextBox)e.Row.FindControl("txtMC");
                    Label lblMC = (Label)e.Row.FindControl("lblMC");

                    TextBox txtLBC = (TextBox)e.Row.FindControl("txtLBC");
                    Label lblLBC = (Label)e.Row.FindControl("lblLBC");

                    TextBox txtMGC = (TextBox)e.Row.FindControl("txtMGC");
                    Label lblMGC = (Label)e.Row.FindControl("lblMGC");

                    TextBox txtDGKC = (TextBox)e.Row.FindControl("txtDGKC");
                    Label lblDGKC = (Label)e.Row.FindControl("lblDGKC");

                    TextBox txtCRBC = (TextBox)e.Row.FindControl("txtCRBC");
                    Label lblCRBC = (Label)e.Row.FindControl("lblCRBC");

                    TextBox txtGTC = (TextBox)e.Row.FindControl("txtGTC");
                    Label lblGTC = (Label)e.Row.FindControl("lblGTC");

                    Label txtTotal = (Label)e.Row.FindControl("txtTotal");


                    if (lblTenDaily.Text.Trim().ToUpper() == "DESIGN DIS.")
                    {
                        lblTenDaily.CssClass = "control-label text-bold";

                        txtHD.Visible = false;
                        lblHD.Visible = true;

                        txtSC.Visible = false;
                        lblSC.Visible = true;

                        txtRC.Visible = false;
                        lblRC.Visible = true;

                        txtPML.Visible = false;
                        lblPML.Visible = true;

                        txtAC.Visible = false;
                        lblAC.Visible = true;

                        txtTCMLU.Visible = false;
                        lblTCMLU.Visible = true;

                        txtPCL.Visible = false;
                        lblPCL.Visible = true;

                        txtMC.Visible = false;
                        lblMC.Visible = true;

                        txtLBC.Visible = false;
                        lblLBC.Visible = true;

                        txtMGC.Visible = false;
                        lblMGC.Visible = true;

                        txtDGKC.Visible = false;
                        lblDGKC.Visible = true;

                        txtCRBC.Visible = false;
                        lblCRBC.Visible = true;

                        txtGTC.Visible = false;
                        lblGTC.Visible = true;
                    }
                    else
                    {


                        if (txtTotal.Text != string.Empty)
                        {
                            MAF = Convert.ToDouble(txtTotal.Text);
                            MAF = Math.Round(MAF, 3);
                            MaskedValue = String.Format("{0:0.000}", MAF);
                            txtTotal.Text = MaskedValue;
                        }

                        if (lblTenDaily.Text.ToUpper().Trim() == "TOTAL (MAF)" || lblTenDaily.Text.ToUpper().Trim() == "KHARIF (MAF)" || lblTenDaily.Text == "77-82 (MAF)" || lblTenDaily.Text.ToUpper().Trim() == "DESIGN DIS." || lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") || lblTenDaily.Text.ToUpper().Trim().Contains("E.K") || lblTenDaily.Text.ToUpper().Trim().Contains("L.K"))
                        {
                            lblTenDaily.CssClass = "control-label text-bold";

                            txtHD.Visible = false;
                            lblHD.Visible = true;

                            MAF = Convert.ToDouble(lblHD.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblHD.Text = MaskedValue;

                            txtSC.Visible = false;
                            lblSC.Visible = true;

                            MAF = Convert.ToDouble(lblSC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblSC.Text = MaskedValue;

                            txtRC.Visible = false;
                            lblRC.Visible = true;

                            MAF = Convert.ToDouble(lblRC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblRC.Text = MaskedValue;

                            txtPML.Visible = false;
                            lblPML.Visible = true;

                            MAF = Convert.ToDouble(lblPML.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblPML.Text = MaskedValue;

                            txtAC.Visible = false;
                            lblAC.Visible = true;

                            MAF = Convert.ToDouble(lblAC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblAC.Text = MaskedValue;

                            txtTCMLU.Visible = false;
                            lblTCMLU.Visible = true;

                            MAF = Convert.ToDouble(lblTCMLU.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblTCMLU.Text = MaskedValue;

                            txtPCL.Visible = false;
                            lblPCL.Visible = true;

                            MAF = Convert.ToDouble(lblPCL.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblPCL.Text = MaskedValue;

                            txtMC.Visible = false;
                            lblMC.Visible = true;

                            MAF = Convert.ToDouble(lblMC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblMC.Text = MaskedValue;

                            txtLBC.Visible = false;
                            lblLBC.Visible = true;

                            MAF = Convert.ToDouble(lblLBC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblLBC.Text = MaskedValue;

                            txtMGC.Visible = false;
                            lblMGC.Visible = true;

                            MAF = Convert.ToDouble(lblMGC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblMGC.Text = MaskedValue;

                            txtDGKC.Visible = false;
                            lblDGKC.Visible = true;

                            MAF = Convert.ToDouble(lblDGKC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblDGKC.Text = MaskedValue;

                            txtCRBC.Visible = false;
                            lblCRBC.Visible = true;

                            MAF = Convert.ToDouble(lblCRBC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblCRBC.Text = MaskedValue;

                            txtGTC.Visible = false;
                            lblGTC.Visible = true;

                            MAF = Convert.ToDouble(lblGTC.Text);
                            MAF = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? Math.Round(MAF, 2) : Math.Round(MAF, 3);
                            MaskedValue = lblTenDaily.Text.ToUpper().Trim().Contains("SHORTAGE") ? String.Format("{0:0.00}", MAF) : String.Format("{0:0.000}", MAF);
                            lblGTC.Text = MaskedValue;

                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {

                    string[] arr = (string[])ViewState["ColumnNames"];
                    e.Row.Cells[0].Text = arr[0];
                    e.Row.Cells[1].Text = arr[2];
                    e.Row.Cells[2].Text = arr[4];
                    e.Row.Cells[3].Text = arr[6];
                    e.Row.Cells[4].Text = arr[8];
                    e.Row.Cells[5].Text = arr[10];
                    e.Row.Cells[6].Text = arr[12];
                    e.Row.Cells[7].Text = arr[14];
                    e.Row.Cells[8].Text = arr[16];
                    e.Row.Cells[9].Text = arr[18];
                    e.Row.Cells[10].Text = arr[20];
                    e.Row.Cells[11].Text = arr[22];
                    e.Row.Cells[12].Text = arr[24];
                    e.Row.Cells[13].Text = arr[26];
                    e.Row.Cells[14].Text = arr[28];
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        public void BindGrid(long _Year, long _Season, long _Command)
        {
            EntitlementDeliveryBLL bllEntitlements = new EntitlementDeliveryBLL();
            long CommandID = ddlCommand.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCommand.SelectedItem.Value);

            DataTable dtTenDailies = bllEntitlements.GetTenDailyBySeasonYearCommand(_Year, _Season, _Command);

            string[] columnNames = (from dc in dtTenDailies.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            ViewState["ColumnNames"] = columnNames;

            gvTenDailyEntitlementsIndus.DataSource = dtTenDailies;
            gvTenDailyEntitlementsIndus.DataBind();

            gvTenDailyEntitlementsIndus.Visible = true;
        }

        public void ManageKharifSeassonColumn(GridViewRow gvr, Double MAF, string ControlName)
        {
            #region Early Kharif
            if (gvr.RowIndex < 8)
            {
                Label EarlyKharif = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 7].FindControl(ControlName);
                double TotalEarlyKharif = Double.Parse(EarlyKharif.Text);
                TotalEarlyKharif = TotalEarlyKharif + MAF;
                TotalEarlyKharif = Math.Round(TotalEarlyKharif, 3);
                EarlyKharif.Text = String.Format("{0:0.000}", TotalEarlyKharif);
                TotalEarlyKharif = TotalEarlyKharif + MAF;
                Label EK7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 4].FindControl(ControlName);
                Label lblShortageEarlyKharif = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl(ControlName);
                double shortageEarlyKharif = new EntitlementDeliveryBLL().GetShortage(TotalEarlyKharif, Convert.ToDouble(EK7782.Text));
                shortageEarlyKharif = Math.Round(shortageEarlyKharif, 2);
                lblShortageEarlyKharif.Text = String.Format("{0:0.00}", shortageEarlyKharif.ToString());
            }
            #endregion
            #region Late Kharif
            else
            {
                Label LateKharif = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 6].FindControl(ControlName);
                double TotalLateKharif = Double.Parse(LateKharif.Text);
                LateKharif.Text = String.Format("{0:0.000}", (TotalLateKharif + MAF).ToString());
                TotalLateKharif = TotalLateKharif + MAF;
                Label LK7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl(ControlName);
                Label lblShortageLateKharif = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl(ControlName);
                double shortageLateKharif = new EntitlementDeliveryBLL().GetShortage(TotalLateKharif, Convert.ToDouble(LK7782.Text));
                shortageLateKharif = Math.Round(shortageLateKharif, 2);
                lblShortageLateKharif.Text = String.Format("{0:0.00}", shortageLateKharif.ToString());
            }
            #endregion
        }
        protected void txtHD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtHD = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtHD.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;

                Label lblHD = (Label)gvr.FindControl("lblHD");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtHD.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtHD.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblHD.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;

                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblHD") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblHD");


                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");


                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);
                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblHD.Text = txtHD.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblHD");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblHD");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                if (IsKharifSeasson)
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblHD");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        protected void txtSC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtSC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtSC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblSC = (Label)gvr.FindControl("lblSC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtSC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtSC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblSC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblSC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblSC");

                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblSC.Text = txtSC.Text;



                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblSC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblSC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblSC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtRC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtRC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtRC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblRC = (Label)gvr.FindControl("lblRC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtRC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtRC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblRC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblRC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblRC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblRC.Text = txtRC.Text;


                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblRC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblRC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblRC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtPML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtPML = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtPML.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblPML = (Label)gvr.FindControl("lblPML");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtPML.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtPML.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblPML.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblPML") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblPML");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblPML.Text = txtPML.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblPML");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblPML");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblPML");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtAC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtAC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtAC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblAC = (Label)gvr.FindControl("lblAC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtAC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtAC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblAC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblAC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblAC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblAC.Text = txtAC.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblAC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblAC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblAC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtTCMLU_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtTCMLU = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtTCMLU.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblTCMLU = (Label)gvr.FindControl("lblTCMLU");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtTCMLU.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtTCMLU.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblTCMLU.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblTCMLU") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblTCMLU");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblTCMLU.Text = txtTCMLU.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblTCMLU");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblTCMLU");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblTCMLU");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtPCL_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtPCL = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtPCL.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblPCL = (Label)gvr.FindControl("lblPCL");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtPCL.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtPCL.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblPCL.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblPCL") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblPCL");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblPCL.Text = txtPCL.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblPCL");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblPCL");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblPCL");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtMC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtMC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtMC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblMC = (Label)gvr.FindControl("lblMC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtMC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtMC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblMC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblMC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblMC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblMC.Text = txtMC.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblMC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblMC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblMC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtLBC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtLBC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtLBC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblLBC = (Label)gvr.FindControl("lblLBC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtLBC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtLBC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblLBC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblLBC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblLBC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);


                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblLBC.Text = txtLBC.Text;


                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblLBC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblLBC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblLBC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtMGC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtMGC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtMGC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblMGC = (Label)gvr.FindControl("lblMGC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtMGC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtMGC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblMGC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblMGC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblMGC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);


                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblMGC.Text = txtMGC.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblMGC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblMGC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblMGC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtDGKC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtDGKC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtDGKC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblDGKC = (Label)gvr.FindControl("lblDGKC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtDGKC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtDGKC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblDGKC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblDGKC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblDGKC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblDGKC.Text = txtDGKC.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblDGKC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblDGKC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblDGKC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCRBC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtCRBC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtCRBC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblCRBC = (Label)gvr.FindControl("lblCRBC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtCRBC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtCRBC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblCRBC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblCRBC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblCRBC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblCRBC.Text = txtCRBC.Text;

                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblCRBC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblCRBC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblCRBC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtGTC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();

                TextBox txtGTC = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtGTC.NamingContainer;
                GridView grid = (GridView)gvr.NamingContainer;
                Label lblGTC = (Label)gvr.FindControl("lblGTC");
                HiddenField hdnTenDailyID = (HiddenField)gvr.FindControl("hdnTenDailyID");

                double NearestHundred = Convert.ToDouble(txtGTC.Text);
                NearestHundred = Math.Round(NearestHundred / 100) * 100;
                txtGTC.Text = NearestHundred.ToString();

                double OldValue = Double.Parse(lblGTC.Text);
                //double NewValue = Double.Parse(txtHD.Text);
                double NewValue = NearestHundred;

                double ValueDelta = NewValue - OldValue;
                bool IsKharifSeasson = grid.Rows.Count > 22 ? true : false;
                Label lblHDLast = !IsKharifSeasson ? (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 3].FindControl("lblGTC") : (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 5].FindControl("lblGTC");
                Label txtTotal = (Label)gvr.Cells[gvTenDailyEntitlementsIndus.Columns.Count - 1].FindControl("txtTotal");

                double TotalChannelEntitlementHorizontal = Double.Parse(lblHDLast.Text);
                double TotalChannelEntitlementVertical = Double.Parse(txtTotal.Text);

                double MAF = bllEntitlement.GetMAFByCusecs(Convert.ToInt64(hdnTenDailyID.Value), ValueDelta);
                MAF = Math.Round(MAF, 3);

                lblHDLast.Text = (TotalChannelEntitlementHorizontal + MAF).ToString();
                txtTotal.Text = (TotalChannelEntitlementVertical + MAF).ToString();
                lblGTC.Text = txtGTC.Text;


                if (!IsKharifSeasson)
                {
                    Label lbl7782 = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 2].FindControl("lblGTC");
                    Label lblShortage = (Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].FindControl("lblGTC");
                    double shortage = bllEntitlement.GetShortage(Convert.ToDouble(lblHDLast.Text), Convert.ToDouble(lbl7782.Text));
                    shortage = Math.Round(shortage, 2);
                    String.Format("{0:0.00}", shortage);
                    lblShortage.Text = shortage.ToString();
                }
                else
                {
                    ManageKharifSeassonColumn(gvr, MAF, "lblGTC");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlement = new EntitlementDeliveryBLL();
                List<long> lstSeasonIDs = new List<long>();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                DateTime Now = DateTime.Now;
                int Year = Now.Year;
                if (new DateTime(Now.Year, 3, 10) >= Now)
                {
                    Year = Year - 1;
                }


                //for (int i = 1; i < gvTenDailyEntitlementsIndus.Rows[0].Cells.Count - 1; i++)
                //{
                //    string HeaderName = gvTenDailyEntitlementsIndus.HeaderRow.Cells[i].Text;
                //    long ChannelEntitlementID = Convert.ToInt64(((HiddenField)gvTenDailyEntitlementsIndus.Rows[0].Cells[i].Controls[5]).Value);
                //    double MAFofChannel = Convert.ToDouble(((Label)gvTenDailyEntitlementsIndus.Rows[gvTenDailyEntitlementsIndus.Rows.Count - 1].Cells[i].Controls[3]).Text);

                //    if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 9)
                //    {
                //        lstSeasonIDs.Add(3);
                //        lstSeasonIDs.Add(4);
                //    }
                //    else
                //    {
                //        lstSeasonIDs.Add(1);
                //    }

                //    ED_ChannelEntitlement mdlChannelEntitlement = bllEntitlement.GetChannelEntitlementByID(ChannelEntitlementID);

                //    MAFFromSeasonal = bllEntitlement.GetMAFFromSeasonalEntitlement(Year, lstSeasonIDs, mdlChannelEntitlement.CommandChannelID);

                //    if (MAFofChannel > MAFFromSeasonal)
                //    {
                //        Master.ShowMessage("Entitlement of " + HeaderName + " is greater, Please Enter Lower Value", SiteMaster.MessageType.Error);
                //        return;
                //    }
                //}

                for (int i = 0; i < gvTenDailyEntitlementsIndus.Rows.Count - 1; i++)
                {
                    if (((HiddenField)gvTenDailyEntitlementsIndus.Rows[i].Cells[0].Controls[3]).Value != string.Empty)
                    {
                        long TenDailyID = Convert.ToInt64(((HiddenField)gvTenDailyEntitlementsIndus.Rows[i].Cells[0].Controls[3]).Value);
                        string TenDailies = ((Label)gvTenDailyEntitlementsIndus.Rows[i].Cells[0].Controls[1]).Text;

                        for (int j = 1; j < gvTenDailyEntitlementsIndus.Rows[i].Cells.Count - 1; j++)
                        {
                            ED_ChannelEntitlement mdlChannelEntitlement = new ED_ChannelEntitlement();
                            long ChannelEntitlementID = Convert.ToInt64(((HiddenField)gvTenDailyEntitlementsIndus.Rows[i].Cells[j].Controls[5]).Value);
                            double CusecValue = Convert.ToDouble(((TextBox)gvTenDailyEntitlementsIndus.Rows[i].Cells[j].Controls[1]).Text);
                            double MAF = bllEntitlement.GetMAFByCusecs(TenDailyID, CusecValue);

                            mdlChannelEntitlement.ID = ChannelEntitlementID;
                            mdlChannelEntitlement.CsEntitlement = CusecValue;
                            mdlChannelEntitlement.MAFEntitlement = MAF;
                            //mdlChannelEntitlement.PercentageEntitlement = 0;
                            mdlChannelEntitlement.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                            mdlChannelEntitlement.ModifiedDate = DateTime.Today;

                            bllEntitlement.UpdateChannelEntitlement(mdlChannelEntitlement);
                        }
                    }
                }

                //EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

                //for (int i = 0; i < gvTenDailyEntitlementsIndus.Rows.Count - 1; i++)
                //{
                //    string TenDailies = ((Label)gvTenDailyEntitlementsIndus.Rows[i].Cells[0].Controls[1]).Text;

                //    for (int j = 1; j < gvTenDailyEntitlementsIndus.Rows[i].Cells.Count - 1; j++)
                //    {
                //        long ChannelEntitlementID = Convert.ToInt64(((HiddenField)gvTenDailyEntitlementsIndus.Rows[i].Cells[j].Controls[5]).Value);
                //        double CusecValue = Convert.ToDouble(((TextBox)gvTenDailyEntitlementsIndus.Rows[i].Cells[j].Controls[1]).Text);
                //        double Sum = 0;

                //        ED_ChannelEntitlement mdlChannelEntitlement = bllEntitlement.GetChannelEntitlementByID(ChannelEntitlementID);

                //        if (TenDailies == "Apr1" || TenDailies == "Apr2" || TenDailies == "Apr3" || TenDailies == "May1" || TenDailies == "May2" || TenDailies == "May3" || TenDailies == "Jun1")
                //        {
                //            Sum = bllEntitlement.GetSumOfEKharifTenDailies(Year, (long)Constants.Seasons.EarlyKharif, mdlChannelEntitlement.CommandChannelID);
                //        }
                //        else if (TenDailies == "Jun2" || TenDailies == "Jun3" || TenDailies == "Jul1" || TenDailies == "Jul2" || TenDailies == "Jul3" || TenDailies == "Aug1" || TenDailies == "Aug2" || TenDailies == "Aug3" || TenDailies == "Sep1" || TenDailies == "Sep2" || TenDailies == "Sep3")
                //        {
                //            Sum = bllEntitlement.GetSumOfLKharifTenDailies(Year, (long)Constants.Seasons.LateKharif, mdlChannelEntitlement.CommandChannelID);
                //        }
                //        else
                //        {
                //            Sum = bllEntitlement.GetSumOfRabiTenDailies(Year, (long)Constants.Seasons.Rabi, mdlChannelEntitlement.CommandChannelID);
                //        }

                //        double Percentage = (CusecValue / Sum) * 100;

                //        ED_ChannelEntitlement mdl = new ED_ChannelEntitlement();

                //        mdl.ID = ChannelEntitlementID;
                //        mdl.PercentageEntitlement = Percentage;

                //        bllEntitlement.UpdateChannelEntitlementPercentage(mdlChannelEntitlement);

                //    }

                //}

                short SeasonID = 0;

                if (Now.Month >= 4 && Now.Month <= 9)
                {
                    SeasonID = (short)Constants.Seasons.Kharif;
                }
                else
                {
                    SeasonID = (short)Constants.Seasons.Rabi;
                }

                bllEntitlement.UpdatePercentageAndGenerateEntitlements(Convert.ToInt64(ddlCommand.SelectedItem.Value), Convert.ToInt16(Year), SeasonID, mdlUser.ID);

                ddlCommand.ClearSelection();
                ddlCommand.SelectedIndex = 0;

                ddlCommand_SelectedIndexChanged(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}