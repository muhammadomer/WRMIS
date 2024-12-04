using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.BLL.WaterTheft;
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

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class GenerateBill : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtBillIssueDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    List<dynamic> lst = new List<dynamic>();
                    //TODO:
                    //Financial Logic to be applied
                    lst.Add(GetFinancialYear());
                    ddlFinancialYear.DataSource = lst;
                    ddlFinancialYear.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private DateTime GetLastFYEndDate()
        {
            DateTime nowDate = DateTime.Now;

            if (nowDate.Month >= 1 && nowDate.Month <= 6)
            { //dd-MMM-yyyy
                return Convert.ToDateTime("30-Jun-" + (DateTime.Today.Year - 1));
            }

            return Convert.ToDateTime("30-June-" + (DateTime.Today.Year));
        }

        private DateTime GetFYendDate()
        {
            DateTime nowDate = DateTime.Now;
            string strDate = "30-Jun-";
            if (nowDate.Month >= 1 && nowDate.Month <= 6) 
                strDate = strDate + DateTime.Today.Year; 
            else
                strDate = strDate + (DateTime.Today.Year + 1);

            return Convert.ToDateTime(strDate);
        }

        private DateTime GetFYstartDate()
        {
            DateTime nowDate = DateTime.Now; 
            string strDate = "01-Jul-";
            if (nowDate.Month >= 1 && nowDate.Month <= 6)
                strDate = strDate + (DateTime.Today.Year -1 );
            else
             strDate = strDate +  DateTime.Today.Year ;

            return Convert.ToDateTime(strDate);
        }
         
        private string GetFinancialYear()
        {
            DateTime nowDate = DateTime.Now;

            if (nowDate.Month >= 1 && nowDate.Month <= 6)
            {
                return "" + (DateTime.Today.Year - 1) + "-" + (DateTime.Today.Year);
            }

            return DateTime.Today.Year + "-" + (DateTime.Today.Year + 1);
        }
        protected void btnBillGeneration_Click(object sender, EventArgs e)
        {
            try
            {
               
                EC_GenerateBillParameters mdlBillParameters = new EC_GenerateBillParameters();
                Effluent_WaterChargesBLL bllEffluents = new Effluent_WaterChargesBLL();

                string ServiceType = "";
                string FinancalYear = "";
                DateTime BillIssueDate = DateTime.Today;
                DateTime BillDueDate = DateTime.Today;
                bool ApplicableTax = false;
                string AdjustmentAddSub = "";
                string AdjustmentType = "";
                double AdjsutmentAmount = 0;
                string AdjustmentReason = null;

                if (rbCanalSpecialWater.Checked)
                {
                    ServiceType = Constants.ECWServiceType.CANAL.ToString();
                }
                else
                {
                    ServiceType = Constants.ECWServiceType.EFFLUENT.ToString();
                }

                FinancalYear = ddlFinancialYear.SelectedItem.Value;
                if (bllEWC.IndustryBillExists(FinancalYear, ServiceType))
                {
                    if (ServiceType.Equals("CANAL"))
                        Master.ShowMessage("Canal Special Water bills have already been generated for this financial year.", SiteMaster.MessageType.Success);
                    else
                        Master.ShowMessage("Effluent Water bills have already been generated for this financial year.", SiteMaster.MessageType.Success);

                     
                    return;
                }
                BillIssueDate = Convert.ToDateTime(txtBillIssueDate.Text);
                BillDueDate = Convert.ToDateTime(txtBillDueDate.Text);

                if (cbApplicableTaxes.Checked)
                {
                    ApplicableTax = true;
                }
                else
                {
                    ApplicableTax = false;
                }

                if (!string.IsNullOrEmpty(ddlAdjustment1.SelectedItem.Value))
                    AdjustmentAddSub = ddlAdjustment1.SelectedItem.Value;
                if (!string.IsNullOrEmpty(ddlAdjustment2.SelectedItem.Value))
                    AdjustmentType = ddlAdjustment2.SelectedItem.Value;

                if (!string.IsNullOrEmpty(txtAdjustment.Text))
                    AdjsutmentAmount = Convert.ToDouble(txtAdjustment.Text);
                AdjustmentReason = txtReason.Text;

                mdlBillParameters.ServiceType = ServiceType;
                mdlBillParameters.FinancialYear = FinancalYear;
                mdlBillParameters.BillIssueDate = BillIssueDate;
                mdlBillParameters.BillDueDate = BillDueDate;
                mdlBillParameters.IncludApplicableTaxes = ApplicableTax;
                mdlBillParameters.AdjustmentAddSub = AdjustmentAddSub;
                mdlBillParameters.AdjustmentType = AdjustmentType;
                mdlBillParameters.AdjustmentAmount = AdjsutmentAmount;
                mdlBillParameters.AdjustmentReason = AdjustmentReason;
                mdlBillParameters.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;

                bllEffluents.AddBillParameters 
                   (mdlBillParameters, ServiceType, GetFinancialYear()
                   , GetFYstartDate(), GetFYendDate(), GetLastFYEndDate(), SessionManagerFacade.UserInformation.ID
                   , GetBillingPerfix(mdlBillParameters.ServiceType)
                   , "From 01 July " + (GetFYendDate().Year -1) + " to 30 June " + GetFYendDate().Year
                   , BillIssueDate, BillDueDate, 365);

                if (ServiceType.Equals("CANAL"))
                    Master.ShowMessage("Canal Special Water bills have been generated successfully.", SiteMaster.MessageType.Success);
                else
                    Master.ShowMessage("Effluent Water bills have been generated successfully.", SiteMaster.MessageType.Success);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        private string GetFinancialYearCropped()
        {
            DateTime nowDate = DateTime.Now;

            if (nowDate.Month >= 1 && nowDate.Month <= 6)
            {
                return "" + (DateTime.Today.Year - 1) + "-" + (DateTime.Today.Year).ToString().Substring(2);
            }

            return DateTime.Today.Year + "-" + (DateTime.Today.Year + 1);
        }

        private string GetBillingPerfix(string _ServiceType)
        {
            if (_ServiceType.Equals("CANAL")) 
                return "CW-" + GetFinancialYearCropped() + "-"; 

            return "EW-" + GetFinancialYearCropped() + "-";
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}