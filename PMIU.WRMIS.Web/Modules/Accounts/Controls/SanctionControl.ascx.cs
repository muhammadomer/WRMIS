using System;
using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.Accounts.ReferenceData;

namespace PMIU.WRMIS.Web.Modules.Accounts.Controls
{
    public partial class SanctionControl : System.Web.UI.UserControl
    {

        public long _BudgetApprovalID;
        public string _FinancialYear;
        public long _TotalGridAmount;


        public string FinancialYear { get { return _FinancialYear; } set { _FinancialYear = value; } }
        public long BudgetApprovalID { get { return _BudgetApprovalID; } set { _BudgetApprovalID = value; } }
        public long TotalGridAmount { get { return _TotalGridAmount; } set { _TotalGridAmount = value; } }



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_BudgetApprovalID != null)
                {
                    LoadData(FinancialYear);
                    
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }




        private void LoadData(string FinancialYear)//long _BudgetApprovalID, long TotalGridAmount)
        {
            try
            {
                //long BudgetApproval = 0;
                //long CurrentReleaseBudget = 0;
                //long AmountAvailable = 0;
                //long ExpenditureUptoPreviousBill = 0;
                //long BalanceAfterThisBill = 0;
                //long ThisBill = 0;

                object _Data = new AccountsBLL().GetApprovalBudget_ReleaseAmountByFinancialYear(FinancialYear);
                if (_Data != null)
                {
                    lblBudgetProvision.Text = Utility.GetDynamicPropertyValue(_Data, "BA");
                    lblAmountReleased.Text = Utility.GetDynamicPropertyValue(_Data, "CBR");
                    lblBalanceAvailable.Text = Convert.ToString(Convert.ToInt64(lblBudgetProvision.Text) - Convert.ToInt64(lblAmountReleased.Text));
                    //lblExpenditureUptpPerviousBill.Text = Utility.GetDynamicPropertyValue(_Data, "BPS");
                    //lblBalanceAfterThisBill.Text = Convert.ToString(Convert.ToInt64(lblBalanceAvailable.Text) - TotalGridAmount);
                    //lblThisBill.Text = Convert.ToString(TotalGridAmount);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}