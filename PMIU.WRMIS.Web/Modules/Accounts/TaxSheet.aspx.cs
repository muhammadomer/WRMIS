using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class TaxSheet : BasePage
    {
        double SanctionAmount = 0;
        AccountsBLL bllAccounts = new AccountsBLL();
        double? TotalAmountSum = 0;
        double? PurchaseItemSum = 0;
        double? RepairItemSum = 0;
        double? GSTSum = 0;
        double? IncomeTaxOnPurchaseSum = 0;
        double? IncomeTaxOnServiceSum = 0;
        double? PSTSum = 0;
        double? NetAmountSum = 0;
        double? TotalTaxSum = 0;
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
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTaxSheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblPurchaseAmount = (Label)e.Row.FindControl("lblPurchaseAmount");
                    Label lblRepairAmount = (Label)e.Row.FindControl("lblRepairAmount");
                    Label lblTotalBill = (Label)e.Row.FindControl("lblTotalBill");
                    Label lblGST = (Label)e.Row.FindControl("lblGST");
                    Label lblIncomeTaxPurchase = (Label)e.Row.FindControl("lblIncomeTaxPurchase");
                    Label lblIncomeTaxService = (Label)e.Row.FindControl("lblIncomeTaxService");
                    Label lblPSTService = (Label)e.Row.FindControl("lblPSTService");
                    Label lblNetAmount = (Label)e.Row.FindControl("lblNetAmount");
                    Label lblVendorType = (Label)e.Row.FindControl("lblVendorTpye");
                    Label lblTotalTax = (Label)e.Row.FindControl("lblTotalTax");

                    double? lblTotalBillNullable = lblTotalBill.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblTotalBill.Text);
                    double? lblPurchaseAmountNullable = lblPurchaseAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPurchaseAmount.Text);
                    double? lblRepairAmountNullable = lblRepairAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblRepairAmount.Text);

                    lblTotalBill.Text = Utility.GetRoundOffValueAccounts(lblTotalBillNullable);
                    lblPurchaseAmount.Text = Utility.GetRoundOffValueAccounts(lblPurchaseAmountNullable);
                    lblRepairAmount.Text = Utility.GetRoundOffValueAccounts(lblRepairAmountNullable);


                    double ExpenseLimitForTax = bllAccounts.GetExpenseLimitForTax();
                    /****************************Purchase Variables****************************/
                    double? GSTPurchase = 0;
                    double? IncomeTaxPurchase = 0;
                    double? PSTPurchase = 0;
                    double? OtherTaxPurchase = 0;

                    /****************************Repair Variables****************************/
                    double? GSTRepair = 0;
                    double? IncomeTaxRepair = 0;
                    double? PSTRepair = 0;
                    double? OtherTaxRepair = 0;

                    /****************************Total Variables****************************/
                    double TotalAmount = 0;
                    double? TotalTax = 0;

                    if (lblVendorType.Text.Trim().ToUpper() == "FILER")
                    {
                        if (!string.IsNullOrEmpty(lblPurchaseAmount.Text) && ExpenseLimitForTax <= Convert.ToDouble(lblTotalBill.Text))
                        {
                            double PurchaseAmount = Convert.ToDouble(lblPurchaseAmount.Text);

                            /**********GST Calculation***********/
                            double? SalesTaxFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.SalesTaxFilerPurchase);
                            GSTPurchase = SalesTaxFilerPurchase * PurchaseAmount / 100;
                            GSTPurchase = Math.Round(GSTPurchase.Value);
                            lblGST.Text = Utility.GetRoundOffValueAccounts(GSTPurchase);

                            /**********Income Tax On Purchase Calculation***********/
                            double? IncomeTaxFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.IncomeTaxFilerPurchase);
                            IncomeTaxPurchase = IncomeTaxFilerPurchase * PurchaseAmount / 100;
                            IncomeTaxPurchase = Math.Round(IncomeTaxPurchase.Value);
                            lblIncomeTaxPurchase.Text = Utility.GetRoundOffValueAccounts(IncomeTaxPurchase);

                            /***********PST Tax On Purchase************/
                            double? PSTTaxFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.PunjabSalesTaxFilerPurchase);
                            PSTPurchase = PSTTaxFilerPurchase * PurchaseAmount / 100;
                            PSTPurchase = Math.Round(PSTPurchase.Value);

                            /***********Other Tax On Purchase************/
                            double? OtherTaxFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.OtherTaxesFilerPurchase);
                            OtherTaxPurchase = OtherTaxFilerPurchase * PurchaseAmount / 100;
                            OtherTaxPurchase = Math.Round(OtherTaxPurchase.Value);
                        }

                        if (!string.IsNullOrEmpty(lblRepairAmount.Text) && ExpenseLimitForTax <= Convert.ToDouble(lblTotalBill.Text))
                        {
                            double RepairAmount = Convert.ToDouble(lblRepairAmount.Text);

                            /**********Income Tax On Repair Calculation***********/
                            double? IncomeTaxFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.IncomeTaxFilerRepair);
                            IncomeTaxRepair = IncomeTaxFilerRepair * RepairAmount / 100;
                            IncomeTaxRepair = Math.Round(IncomeTaxRepair.Value);
                            lblIncomeTaxService.Text = Utility.GetRoundOffValueAccounts(IncomeTaxRepair);

                            /**********PST Tax On Repair Calculation***********/
                            double? PunjabSalesTaxFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.PunjabSalesTaxFilerRepair);
                            PSTRepair = PunjabSalesTaxFilerRepair * RepairAmount / 100;
                            PSTRepair = Math.Round(PSTRepair.Value);
                            lblPSTService.Text = Utility.GetRoundOffValueAccounts(PSTRepair);

                            /**********GST Tax On Repair Calculation***********/
                            double? SalesTaxFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.SalesTaxFilerRepair);
                            GSTRepair = SalesTaxFilerRepair * RepairAmount / 100;
                            GSTRepair = Math.Round(GSTRepair.Value);

                            /**********Other Tax On Repair Calculation***********/
                            double? OtherTaxFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.OtherTaxesFilerRepair);
                            OtherTaxRepair = OtherTaxFilerRepair * RepairAmount / 100;
                            OtherTaxRepair = Math.Round(OtherTaxRepair.Value);
                        }

                        if (!string.IsNullOrEmpty(lblTotalBill.Text))
                            TotalAmount = Convert.ToDouble(lblTotalBill.Text);

                        /***************Total Tax Calculation******************/
                        TotalTax = (GSTPurchase + IncomeTaxPurchase + PSTPurchase + OtherTaxPurchase + IncomeTaxRepair + PSTRepair + GSTRepair + OtherTaxRepair);
                        lblTotalTax.Text = Utility.GetRoundOffValueAccounts(TotalTax); //TotalTax.ToString();


                        /**********Net Amount Calculation***********/
                        lblNetAmount.Text = Utility.GetRoundOffValueAccounts((TotalAmount - TotalTax));

                        /**********Footer Calculation***********/
                        if (!string.IsNullOrEmpty(lblTotalBill.Text))
                            TotalAmountSum = TotalAmountSum + Convert.ToDouble(lblTotalBill.Text);

                        if (!string.IsNullOrEmpty(lblPurchaseAmount.Text))
                            PurchaseItemSum = PurchaseItemSum + Convert.ToDouble(lblPurchaseAmount.Text);

                        if (!string.IsNullOrEmpty(lblRepairAmount.Text))
                            RepairItemSum = RepairItemSum + Convert.ToDouble(lblRepairAmount.Text);

                        if (!string.IsNullOrEmpty(lblGST.Text))
                            GSTSum = GSTSum + Convert.ToDouble(lblGST.Text);

                        if (!string.IsNullOrEmpty(lblIncomeTaxPurchase.Text))
                            IncomeTaxOnPurchaseSum = IncomeTaxOnPurchaseSum + Convert.ToDouble(lblIncomeTaxPurchase.Text);

                        if (!string.IsNullOrEmpty(lblIncomeTaxService.Text))
                            IncomeTaxOnServiceSum = IncomeTaxOnServiceSum + Convert.ToDouble(lblIncomeTaxService.Text);

                        if (!string.IsNullOrEmpty(lblPSTService.Text))
                            PSTSum = PSTSum + Convert.ToDouble(lblPSTService.Text);

                        if (!string.IsNullOrEmpty(lblNetAmount.Text))
                            NetAmountSum = NetAmountSum + Convert.ToDouble(lblNetAmount.Text);

                        if (!string.IsNullOrEmpty(lblTotalTax.Text))
                            TotalTaxSum = TotalTaxSum + Convert.ToDouble(lblTotalTax.Text);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lblPurchaseAmount.Text) && ExpenseLimitForTax <= Convert.ToDouble(lblTotalBill.Text))
                        {
                            double PurchaseAmount = Convert.ToDouble(lblPurchaseAmount.Text);

                            /**********GST Calculation***********/
                            double? SalesTaxNonFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.SalesTaxNonFilerPurchase);
                            GSTPurchase = SalesTaxNonFilerPurchase * PurchaseAmount / 100;
                            GSTPurchase = Math.Round(GSTPurchase.Value);
                            lblGST.Text = Utility.GetRoundOffValueAccounts(GSTPurchase);

                            /**********Income Tax On Purchase Calculation***********/
                            double? IncomeTaxNonFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.IncomeTaxNonFilerPurchase);
                            IncomeTaxPurchase = IncomeTaxNonFilerPurchase * PurchaseAmount / 100;
                            IncomeTaxPurchase = Math.Round(IncomeTaxPurchase.Value);
                            lblIncomeTaxPurchase.Text = Utility.GetRoundOffValueAccounts(IncomeTaxPurchase);

                            /***********PST Tax On Purchase************/
                            double? PSTTaxNonFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.PunjabSalesTaxNonFilerPurchase);
                            PSTPurchase = PSTTaxNonFilerPurchase * PurchaseAmount / 100;
                            PSTPurchase = Math.Round(PSTPurchase.Value);

                            /***********Other Tax On Purchase************/
                            double? OtherTaxNonFilerPurchase = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.OtherTaxesNonFilerPurchase);
                            OtherTaxPurchase = OtherTaxNonFilerPurchase * PurchaseAmount / 100;
                            OtherTaxPurchase = Math.Round(OtherTaxPurchase.Value);
                        }

                        if (!string.IsNullOrEmpty(lblRepairAmount.Text) && ExpenseLimitForTax <= Convert.ToDouble(lblTotalBill.Text))
                        {
                            double RepairAmount = Convert.ToDouble(lblRepairAmount.Text);

                            /**********Income Tax On Repair Calculation***********/
                            double? IncomeTaxNonFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.IncomeTaxNonFilerRepair);
                            IncomeTaxRepair = IncomeTaxNonFilerRepair * RepairAmount / 100;
                            IncomeTaxRepair = Math.Round(IncomeTaxRepair.Value);
                            lblIncomeTaxService.Text = Utility.GetRoundOffValueAccounts(IncomeTaxRepair);

                            /**********PST Tax On Repair Calculation***********/
                            double? PunjabSalesTaxNonFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.PunjabSalesTaxNonFilerRepair);
                            PSTRepair = PunjabSalesTaxNonFilerRepair * RepairAmount / 100;
                            PSTRepair = Math.Round(PSTRepair.Value);
                            lblPSTService.Text = Utility.GetRoundOffValueAccounts(PSTRepair);

                            /**********GST Tax On Repair Calculation***********/
                            double? SalesTaxNonFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.SalesTaxNonFilerRepair);
                            GSTRepair = SalesTaxNonFilerRepair * RepairAmount / 100;
                            GSTRepair = Math.Round(GSTRepair.Value);

                            /**********Other Tax On Repair Calculation***********/
                            double? OtherTaxNonFilerRepair = bllAccounts.GetTaxRateByID((long)Constants.TaxRates.OtherTaxesNonFilerRepair);
                            OtherTaxRepair = OtherTaxNonFilerRepair * RepairAmount / 100;
                            OtherTaxRepair = Math.Round(OtherTaxRepair.Value);

                        }

                        if (!string.IsNullOrEmpty(lblTotalBill.Text))
                            TotalAmount = Convert.ToDouble(lblTotalBill.Text);

                        /***************Total Tax Calculation******************/
                        TotalTax = (GSTPurchase + IncomeTaxPurchase + PSTPurchase + OtherTaxPurchase + IncomeTaxRepair + PSTRepair + GSTRepair + OtherTaxRepair);
                        lblTotalTax.Text = TotalTax.ToString();


                        /**********Net Amount Calculation***********/
                        lblNetAmount.Text = Utility.GetRoundOffValueAccounts((TotalAmount - TotalTax));

                        /**********Footer Calculation***********/
                        if (!string.IsNullOrEmpty(lblTotalBill.Text))
                            TotalAmountSum = TotalAmountSum + Convert.ToDouble(lblTotalBill.Text);

                        if (!string.IsNullOrEmpty(lblPurchaseAmount.Text))
                            PurchaseItemSum = PurchaseItemSum + Convert.ToDouble(lblPurchaseAmount.Text);

                        if (!string.IsNullOrEmpty(lblRepairAmount.Text))
                            RepairItemSum = RepairItemSum + Convert.ToDouble(lblRepairAmount.Text);

                        if (!string.IsNullOrEmpty(lblGST.Text))
                            GSTSum = GSTSum + Convert.ToDouble(lblGST.Text);

                        if (!string.IsNullOrEmpty(lblIncomeTaxPurchase.Text))
                            IncomeTaxOnPurchaseSum = IncomeTaxOnPurchaseSum + Convert.ToDouble(lblIncomeTaxPurchase.Text);

                        if (!string.IsNullOrEmpty(lblIncomeTaxService.Text))
                            IncomeTaxOnServiceSum = IncomeTaxOnServiceSum + Convert.ToDouble(lblIncomeTaxService.Text);

                        if (!string.IsNullOrEmpty(lblPSTService.Text))
                            PSTSum = PSTSum + Convert.ToDouble(lblPSTService.Text);

                        if (!string.IsNullOrEmpty(lblNetAmount.Text))
                            NetAmountSum = NetAmountSum + Convert.ToDouble(lblNetAmount.Text);

                        if (!string.IsNullOrEmpty(lblTotalTax.Text))
                            TotalTaxSum = TotalTaxSum + Convert.ToDouble(lblTotalTax.Text);
                    }

                    if (lblGST.Text.Trim() == string.Empty)
                    {
                        lblGST.Text = "0";
                    }

                    if (lblIncomeTaxPurchase.Text.Trim() == string.Empty)
                    {
                        lblIncomeTaxPurchase.Text = "0";
                    }

                    if (lblIncomeTaxService.Text.Trim() == string.Empty)
                    {
                        lblIncomeTaxService.Text = "0";
                    }

                    if (lblPSTService.Text.Trim() == string.Empty)
                    {
                        lblPSTService.Text = "0";
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblFtrTotalBill = (Label)e.Row.FindControl("lblFtrTotalBill");
                    Label lblFtrPurchaseItemAmount = (Label)e.Row.FindControl("lblFtrPurchaseItemAmount");
                    Label lblFtrRepairItemAmount = (Label)e.Row.FindControl("lblFtrRepairItemAmount");
                    Label lblFtrGST = (Label)e.Row.FindControl("lblFtrGST");
                    Label lblFtrIncomeTaxPurchase = (Label)e.Row.FindControl("lblFtrIncomeTaxPurchase");
                    Label lblFtrIncomeTaxService = (Label)e.Row.FindControl("lblFtrIncomeTaxService");
                    Label lblFtrPSTService = (Label)e.Row.FindControl("lblFtrPSTService");
                    Label lblFtrNetAmount = (Label)e.Row.FindControl("lblFtrNetAmount");
                    Label lblFtrTotalTax = (Label)e.Row.FindControl("lblFtrTotalTax");

                    lblFtrTotalBill.Text = Utility.GetRoundOffValueAccounts(TotalAmountSum);
                    lblFtrPurchaseItemAmount.Text = Utility.GetRoundOffValueAccounts(PurchaseItemSum);
                    lblFtrRepairItemAmount.Text = Utility.GetRoundOffValueAccounts(RepairItemSum);
                    lblFtrGST.Text = Utility.GetRoundOffValueAccounts(GSTSum);
                    lblFtrIncomeTaxPurchase.Text = Utility.GetRoundOffValueAccounts(IncomeTaxOnPurchaseSum);
                    lblFtrIncomeTaxService.Text = Utility.GetRoundOffValueAccounts(IncomeTaxOnServiceSum);
                    lblFtrPSTService.Text = Utility.GetRoundOffValueAccounts(PSTSum);
                    lblFtrNetAmount.Text = Utility.GetRoundOffValueAccounts(NetAmountSum);
                    lblFtrTotalTax.Text = Utility.GetRoundOffValueAccounts(TotalTaxSum);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["SanctionID"]))
            {
                long SanctionID = Convert.ToInt64(Request.QueryString["SanctionID"]);
                List<dynamic> lstTaxSheet = bllAccounts.GetTaxSheet(SanctionID);
                gvTaxSheet.DataSource = lstTaxSheet;
                gvTaxSheet.DataBind();

                AT_Sanction mdlSanction = bllAccounts.GetSanctionByID(SanctionID);
                //lblMonth.Text = mdlSanction.Month + " " + mdlSanction.FinancialYear;
                lblSanctionType.Text = mdlSanction.AT_ExpenseType.ExpenseType;
                lblSanctionAmount.Text = Utility.GetRoundOffValueAccounts(mdlSanction.SanctionAmount);

                if (mdlSanction.Month.ToUpper().Trim() == "JANUARY" || mdlSanction.Month.ToUpper().Trim() == "FEBRUARY" || mdlSanction.Month.ToUpper().Trim() == "MARCH" || mdlSanction.Month.ToUpper().Trim() == "APRIL" || mdlSanction.Month.ToUpper().Trim() == "MAY" || mdlSanction.Month.ToUpper().Trim() == "JUNE")
                {
                    int year = Convert.ToInt32(mdlSanction.FinancialYear.Split('-')[0]);
                    DateTime NextYear = new DateTime(year + 1, 1, 1);
                    string CY = NextYear.ToString("yyyy");

                    lblMonth.Text = mdlSanction.Month + " " + CY;
                }
                else
                {
                    int year = Convert.ToInt32(mdlSanction.FinancialYear.Split('-')[0]);
                    DateTime CurrentYear = new DateTime(year, 1, 1);
                    string NY = CurrentYear.ToString("yyyy");

                    lblMonth.Text = mdlSanction.Month + " " + NY;
                }
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["SanctionID"]))
                {
                    lbtnPrint.Enabled = true;

                    long SanctionID = Convert.ToInt64(Request.QueryString["SanctionID"]);

                    ReportData mdlReportData = new ReportData();

                    ReportParameter ReportParameter = new ReportParameter("SnactionID", SanctionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.TaxSheetReport;

                    Session[SessionValues.ReportData] = mdlReportData;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
                }
                else
                {
                    lbtnPrint.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}