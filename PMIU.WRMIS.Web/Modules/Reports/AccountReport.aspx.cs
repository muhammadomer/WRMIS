using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.UserAdministration;
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

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class AccountReport : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var selectList = Enumerable.Range(2008, (DateTime.Now.Year - 2008) + 1);
                    SetPageTitle();
                    BindDropdownlists();
                    LoadBudgetUtilization();
                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Events

        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLFinancialYear(ddlFinancialYear, null, false);
                ddlFinancialYear.SelectedIndex = 1;
                ddlObjectClassification.Enabled = false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #region DropDownIndexChanged
        protected void ddlTaxFor_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlTaxFor.SelectedIndex == 0)
            {
                RepairTypeCol.Visible = false;
            }

            if (ddlTaxFor.SelectedItem.Value == "1")
            {
                RepairTypeCol.Visible = true;

                Dropdownlist.DDLAAAssetType(ddlRepairType, false, (int)Constants.DropDownFirstOption.All);
            }
            
            if (ddlTaxFor.SelectedItem.Value == "2")
            {
                RepairTypeCol.Visible = false;
            }


        }
        protected void ddlAccountHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAccountHead.SelectedIndex != 0)
            {
                ddlObjectClassification.Enabled = true;
                Dropdownlist.DDLACObjectClassification(ddlObjectClassification, false, Convert.ToInt64(ddlAccountHead.SelectedItem.Value));
            }
            else
            {
                ddlObjectClassification.Enabled = false;
                Dropdownlist.DDLACObjectClassification(ddlObjectClassification, true);
            }
        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbPrintSanction.Checked)
            {
                if (ddlFinancialYear.SelectedIndex != 0 && ddlSanctionMonth.SelectedIndex != 0 && ddlSanctionOn.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, false, ddlFinancialYear.SelectedItem.Text, ddlSanctionMonth.SelectedItem.Text, ddlSanctionOn.SelectedItem.Value, (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, true, "", "", "", (int)Constants.DropDownFirstOption.Select);
                }
            }
        }

        protected void ddlSanctionMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbPrintSanction.Checked)
            {
                if (ddlFinancialYear.SelectedIndex != 0 && ddlSanctionMonth.SelectedIndex != 0 && ddlSanctionOn.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, false, ddlFinancialYear.SelectedItem.Text, ddlSanctionMonth.SelectedItem.Text, ddlSanctionOn.SelectedItem.Value, (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, true, "", "", "", (int)Constants.DropDownFirstOption.Select);
                }
            }
        }
        protected void ddlSanctionOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbPrintSanction.Checked)
            {
                if (ddlFinancialYear.SelectedIndex != 0 && ddlSanctionMonth.SelectedIndex != 0 && ddlSanctionOn.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, false, ddlFinancialYear.SelectedItem.Text, ddlSanctionMonth.SelectedItem.Text, ddlSanctionOn.SelectedItem.Value, (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.DDLSanctionNO(ddlSanctionNO, true, "", "", "", (int)Constants.DropDownFirstOption.Select);
                }
            }
        }
        #endregion

        #endregion

        #region RadioBtn

        protected void rbBudgetUtilization_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBudgetUtilization.Checked)
            {
                LoadBudgetUtilization();
            }
        }

        protected void rbTaxSheet_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTaxSheet.Checked)
            {
                LoadTaxSheet();
            }
        }

        protected void rbHeadWiseExpenditure_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHeadWiseExpenditure.Checked)
            {
                LoadHeadWisExpenditure();
            }
        }

        protected void rbHeadWiseBudgetUtilizationDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHeadWiseBudgetUtilizationDetails.Checked)
            {
                LoadHeadWiseBudgetUtilizationDetails();
            }
        }

        protected void rbPrintSanction_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPrintSanction.Checked)
            {
                LoadPrintSanction();
            }
        }

        #endregion

        #region Button
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string FinancialYear = ddlFinancialYear.SelectedItem.Value;
                string month = "";
                string TaxFor = "";
                string RepairType = "";
                string TaxOn = "";
                string AccountHead = "";
                string ObjectClassfication = "";
                string ObjectClassficationCode = "";
                string SanctionON = "";
                string SanctionNO = "";
                string SanctionStatus = "";

                if (rbBudgetUtilization.Checked)
                {
                    month = "";
                    month = ddlUptoMonth.SelectedIndex == 0 ? "" : ddlUptoMonth.SelectedItem.Text;
                }
                if (rbTaxSheet.Checked)
                {
                    month = "";
                    month = ddlMonthTax.SelectedIndex == 0 ? "" : ddlMonthTax.SelectedItem.Text;
                    TaxOn = ddlTaxOn.SelectedIndex == 0 ? "0" :  ddlTaxOn.SelectedItem.Value;

                    TaxFor = ddlTaxFor.SelectedIndex == 0 ? "0" : ddlTaxFor.SelectedItem.Value;
                    if (ddlTaxFor.SelectedIndex == 0 || ddlTaxFor.SelectedIndex == 2)
                    {
                        RepairType = "0";
                    }
                    else if (ddlTaxFor.SelectedIndex == 1)
                    {
                        RepairType = ddlRepairType.SelectedIndex == 0 ? "0" : ddlRepairType.SelectedItem.Value;
                    }

                }
                if (rbHeadWiseExpenditure.Checked)
                {
                    month = "";
                    month = ddlMonthHead.SelectedIndex == 0 ? "" : ddlMonthHead.SelectedItem.Text;
                    AccountHead = ddlAccountHead.SelectedItem.Value;
                    ObjectClassfication = ddlObjectClassification.SelectedItem.Value;
                }

                if (rbHeadWiseBudgetUtilizationDetails.Checked)
                {
                    ObjectClassficationCode = ddlObjectClassificationCode.SelectedItem.Value;
                }

                if (rbPrintSanction.Checked)
                {
                    month = "";
                    month = ddlSanctionMonth.SelectedIndex == 0 ? "" : ddlSanctionMonth.SelectedItem.Text;
                    SanctionON = ddlSanctionOn.SelectedItem.Value;
                    SanctionNO = ddlSanctionNO.SelectedItem.Value;
                    SanctionStatus = ddlSanctionStatus.SelectedItem.Value == "" ? "0" : ddlSanctionStatus.SelectedItem.Value;
                }

                ReportData rptData = new ReportData();

                ReportParameter reportParameter = new ReportParameter("Year", FinancialYear);
                rptData.Parameters.Add(reportParameter);

                #region BudgetUtilization Parameter


                if (rbBudgetUtilization.Checked)
                {
                    reportParameter = new ReportParameter("Month", month);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptACBudgetUtilization;
                }
                #endregion

                #region TaxSheet Parameter
                if (rbTaxSheet.Checked)
                {
                    reportParameter = new ReportParameter("Month", month);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("TaxForID", TaxFor);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("RepairTypeID", RepairType);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("TaxOnID", TaxOn);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptACTaxSheet;
                }
                #endregion

                #region HeadWiseExpenditure Parameter
                if (rbHeadWiseExpenditure.Checked)
                {
                    reportParameter = new ReportParameter("Month", month);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("AccountHeadID", AccountHead);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("ObjectClassificationID", ObjectClassfication);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptACHeadWiseExpenditure;
                }
                #endregion

                #region HeadWiseBudgetUtilizationDetails Parameter
                if (rbHeadWiseBudgetUtilizationDetails.Checked)
                {
                    ObjectClassficationCode = ddlObjectClassificationCode.SelectedItem.Value;
                    reportParameter = new ReportParameter("ObjectClasificationID", ObjectClassficationCode);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptACBudgetUtilizationDetails;
                }
                #endregion

                #region PrintSanction Parameter
                if (rbPrintSanction.Checked)
                {
                    reportParameter = new ReportParameter("Month", month);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("SanctionNo", SanctionNO);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("SanctionStatusID", SanctionStatus);
                    rptData.Parameters.Add(reportParameter);

                    if (SanctionON == "1")
                    {
                        rptData.Name = ReportConstants.rptACPrintSanction4Wheel;
                    }
                    else if (SanctionON == "2")
                    {
                        rptData.Name = ReportConstants.rptACPrintSanction2Wheel;
                    }

                }

                #endregion
                Session[SessionValues.ReportData] = rptData;
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #endregion

        #region Functions
        private void SetPageTitle()
        {
            //Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyDataOperationalReport);
            //Master.ModuleTitle = pageTitle.Item1;
            //Master.PageTitle = pageTitle.Item2;
            //Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadBudgetUtilization()
        {
            Dropdownlist.DDLMonthList(ddlUptoMonth, (int)Constants.DropDownFirstOption.All);
            TaxSheet.Visible = false;
            HeadWisExpenditure.Visible = false;
            HWBUDetails.Visible = false;
            BudgetUtilization.Visible = true;
            PrintSanction.Visible = false;

        }

        private void LoadTaxSheet()
        {
            Dropdownlist.DDLMonthList(ddlMonthTax, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLAccountTaxFor(ddlTaxFor, null, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLAccountTaxOn(ddlTaxOn, null, (int)Constants.DropDownFirstOption.All);
            RepairTypeCol.Visible = false;
            //Dropdownlist.DDLAAAssetType(ddlRepairType, true, (int)Constants.DropDownFirstOption.All);
            TaxSheet.Visible = true;
            HeadWisExpenditure.Visible = false;
            HWBUDetails.Visible = false;
            BudgetUtilization.Visible = false;
            PrintSanction.Visible = false;

        }

        private void LoadHeadWisExpenditure()
        {
            Dropdownlist.DDLMonthList(ddlMonthHead, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLAccountHeadList(ddlAccountHead, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLObjectClassificationCode(ddlObjectClassification, (int)Constants.DropDownFirstOption.Select);
            //object classification cLLL DDLAccountHeadList on index change

            TaxSheet.Visible = false;
            HeadWisExpenditure.Visible = true;
            BudgetUtilization.Visible = false;
            HWBUDetails.Visible = false;
            PrintSanction.Visible = false;
        }

        private void LoadHeadWiseBudgetUtilizationDetails()
        {
            Dropdownlist.DDLObjectClassificationCode(ddlObjectClassificationCode, (int)Constants.DropDownFirstOption.Select);

            HWBUDetails.Visible = true;
            TaxSheet.Visible = false;
            HeadWisExpenditure.Visible = false;
            BudgetUtilization.Visible = false;
            PrintSanction.Visible = false;
        }

        private void LoadPrintSanction()
        {
            Dropdownlist.DDLMonthList(ddlSanctionMonth, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLSanctionON(ddlSanctionOn, null, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLSanctionStatus(ddlSanctionStatus, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLSanctionNO(ddlSanctionNO, true, "", "", "", (int)Constants.DropDownFirstOption.Select);
            PrintSanction.Visible = true;
            HWBUDetails.Visible = false;
            TaxSheet.Visible = false;
            HeadWisExpenditure.Visible = false;
            BudgetUtilization.Visible = false;
        }

        #endregion












    }
}
