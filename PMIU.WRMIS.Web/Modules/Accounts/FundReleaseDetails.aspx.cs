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
    public partial class FundReleaseDetails : BasePage
    {
        AccountsBLL bllAccounts = new AccountsBLL();
        long FundReleaseID = 0;
        bool IsFundReleaseIDExist = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    hlBack.NavigateUrl = "~/Modules/Accounts/FundRelease.aspx?ShowHistory=true";
                    BindHeaderGrid();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        private void BindHeaderGrid()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["FundReleaseID"]))
            {
                FundReleaseID = Convert.ToInt64(Request.QueryString["FundReleaseID"]);
                AT_FundRelease mdlFundRelease = bllAccounts.GetFundReleaseDetailsByID(FundReleaseID);

                lblCutType.Text = mdlFundRelease.AT_FundReleaseTypes.TypeName;
                lblCutDate.Text = Utility.GetFormattedDate(mdlFundRelease.FundReleaseDate);
                lblFinancialYear.Text = mdlFundRelease.FinancialYear;

                IsFundReleaseIDExist = bllAccounts.GetFundReleaseDetailsByFundReleaseID(FundReleaseID);
                if (IsFundReleaseIDExist)
                {
                    btnSave.Visible = false;
                }
            }
        }
        private void BindGrid()
        {
            string FinancialYear = "";

            if (!string.IsNullOrEmpty(Request.QueryString["FinancialYear"]))
            {
                FinancialYear = Convert.ToString(Request.QueryString["FinancialYear"]);
            }
            List<object> lstBudget = bllAccounts.GetFundReleaseDetails(FinancialYear, FundReleaseID);
            gvFundReleaseDetails.DataSource = lstBudget;
            gvFundReleaseDetails.DataBind();
            foreach (TableRow Row in gvFundReleaseDetails.Rows)
            {
                TextBox txtCurrentRelease = (TextBox)Row.FindControl("txtCurrentRelease");
                if (!lblCutType.Text.ToUpper().Equals("REAPPROPRIATION") && !lblCutType.Text.ToUpper().Equals("SUPPLEMENTARY GRANT"))
                {

                    txtCurrentRelease.CssClass = "integerInput form-control";
                }
                else
                {
                    txtCurrentRelease.CssClass = "NegativeintegerInput form-control";
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

        protected void gvFundReleaseDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (lblCutType.Text.ToUpper().Equals("REAPPROPRIATION"))
                    {
                        e.Row.Cells[8].Text = "Re-appropriation Amount (Rs.)";
                    }
                    if (lblCutType.Text.ToUpper().Equals("SUPPLEMENTARY GRANT"))
                    {
                        e.Row.Cells[8].Text = "Supplementary Grant (Rs.)";
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblApprovedBudget = (Label)e.Row.FindControl("lblApprovedBudget");
                    Label lblPreviouslyReleasedAmount = (Label)e.Row.FindControl("lblPreviouslyReleasedAmount");
                    Label lblPreviousBalance = (Label)e.Row.FindControl("lblPreviousBalance");
                    Label lblBalanceAmount = (Label)e.Row.FindControl("lblBalanceAmount");
                    Label lblCurrentReleaseAmount = (Label)e.Row.FindControl("lblCurrentReleaseAmount");
                    TextBox txtCurrentRelease = (TextBox)e.Row.FindControl("txtCurrentRelease");


                    if (lblPreviouslyReleasedAmount.Text == "")
                    {
                        lblPreviouslyReleasedAmount.Text = "0";
                    }

                    long ApprovedBudget = Convert.ToInt64(lblApprovedBudget.Text);
                    if (lblPreviouslyReleasedAmount.Text != "")
                    {
                        long PreviouslyReleasedAmount = Convert.ToInt64(lblPreviouslyReleasedAmount.Text);
                        lblPreviousBalance.Text = Convert.ToString(ApprovedBudget - PreviouslyReleasedAmount);   
                       
                    }
                    long PreviousBalance = Convert.ToInt64(lblPreviousBalance.Text);

                    lblBalanceAmount.Text = lblPreviousBalance.Text;

                    double? lblApprovedBudgetNullable = lblApprovedBudget.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblApprovedBudget.Text);
                    double? lblPreviouslyReleasedAmountNullable = lblPreviouslyReleasedAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPreviouslyReleasedAmount.Text);
                    double? lblPreviousBalanceNullable = lblPreviousBalance.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPreviousBalance.Text);
                    double? lblBalanceAmountNullable = lblBalanceAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblBalanceAmount.Text);

                    lblApprovedBudget.Text = Utility.GetRoundOffValueAccounts(lblApprovedBudgetNullable);
                    lblPreviouslyReleasedAmount.Text = Utility.GetRoundOffValueAccounts(lblPreviouslyReleasedAmountNullable);
                    lblPreviousBalance.Text = Utility.GetRoundOffValueAccounts(lblPreviousBalanceNullable);
                    lblBalanceAmount.Text = Utility.GetRoundOffValueAccounts(lblBalanceAmountNullable);
                    if (IsFundReleaseIDExist)
                    {
                        double? lblCurrentReleaseAmountNullable = lblCurrentReleaseAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblCurrentReleaseAmount.Text);
                        lblCurrentReleaseAmount.Text = Utility.GetRoundOffValueAccounts(lblCurrentReleaseAmountNullable);
                        lblCurrentReleaseAmount.Visible = true;
                        txtCurrentRelease.Visible = false;
                        gvFundReleaseDetails.Columns[7].Visible= false;
                    }
                    else
                    {
                        lblCurrentReleaseAmount.Visible = false;
                        txtCurrentRelease.Visible = true;
                        if (!lblCutType.Text.ToUpper().Equals("REAPPROPRIATION") && !lblCutType.Text.ToUpper().Equals("SUPPLEMENTARY GRANT"))
                        {
                            txtCurrentRelease.Attributes.Add("onblur", "callFunctions(this,'" + PreviousBalance + "','" + lblBalanceAmount.ClientID + "'); AddCommas(this);");
                        }
                    }

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


                if (!string.IsNullOrEmpty(Request.QueryString["FundReleaseID"]))
                {
                    FundReleaseID = Convert.ToInt64(Request.QueryString["FundReleaseID"]);
                }
                bool isRepropriation = false;
                bool isSupplymentaryGrant = false;
                bool isTxtReApropriation = false;
                if (lblCutType.Text.ToUpper().Equals("REAPPROPRIATION"))
                {
                    isRepropriation = true;
                    long TotalRA = 0;
                    foreach (TableRow Row in gvFundReleaseDetails.Rows)
                    {
                        TextBox txtCurrentRelease = (TextBox)Row.FindControl("txtCurrentRelease");
                        TotalRA = TotalRA + Convert.ToInt64(Utility.RemoveComma(txtCurrentRelease.Text));
                    }
                    if (TotalRA!=0)
                    {
                         Master.ShowMessage(Message.ReAppropriationAmountConflict.Description, SiteMaster.MessageType.Error);
                         return;
                    }
                }
                //long TotalBudgetoryProvision = 0;
                //long TotalReleaseAmout = 0;
                List<AT_BudgetApprovel> lstBudgetApproval = new List<AT_BudgetApprovel>();
                List<AT_FundReleaseDetails> lsFundReleaeDetail = new List<AT_FundReleaseDetails>();
                foreach (TableRow Row in gvFundReleaseDetails.Rows)
                {
                    Label lblBudgetApprovelID = (Label)Row.FindControl("lblID");
                    Label lblObjectsClassificationID = (Label)Row.FindControl("lblObjectsClassificationID");
                    TextBox txtCurrentRelease = (TextBox)Row.FindControl("txtCurrentRelease");
                    Label lblApprovedBudget = (Label)Row.FindControl("lblApprovedBudget");
                    if (txtCurrentRelease.Text != "")
                    {
                        AT_FundReleaseDetails mdlFundReleaseDetails = new AT_FundReleaseDetails();
                        AT_BudgetApprovel ba = new AT_BudgetApprovel();


                        if (lblCutType.Text.ToUpper().Equals("REAPPROPRIATION"))
                        {
                            ba.ID = Convert.ToInt64(lblBudgetApprovelID.Text);
                            ba.BudgetDate = Convert.ToDateTime(lblCutDate.Text);
                            int txtCurrentnumber;
                            bool result = Int32.TryParse(Utility.RemoveComma(txtCurrentRelease.Text), out txtCurrentnumber);
                            if (!result)
                            {
                                ba.BudgetAmount = Convert.ToInt64(Utility.RemoveComma(lblApprovedBudget.Text));
                            }
                            else
                            {
                                ba.BudgetAmount = Convert.ToInt64(Utility.RemoveComma(lblApprovedBudget.Text)) + (Convert.ToInt64(Utility.RemoveComma(txtCurrentRelease.Text)));
                            }
                            if (ba.BudgetAmount==0 || ba.BudgetAmount < 0)
                            {
                                Master.ShowMessage(Message.BudgetAmountCanNotbeZeroOrLessThanZero.Description, SiteMaster.MessageType.Error);
                                return;
                            }
                            ba.BudgetType = "REAPRO";
                            lstBudgetApproval.Add(ba);
                        }
                        if (lblCutType.Text.ToUpper().Equals("SUPPLEMENTARY GRANT"))
                        {
                            ba.ID = Convert.ToInt64(lblBudgetApprovelID.Text);
                            ba.BudgetDate = Convert.ToDateTime(lblCutDate.Text);
                            if (string.IsNullOrEmpty(txtCurrentRelease.Text))
                            {
                                ba.BudgetAmount = Convert.ToInt64(Utility.RemoveComma(lblApprovedBudget.Text));
                            }
                            else
                            {
                                ba.BudgetAmount = Convert.ToInt64(Utility.RemoveComma(lblApprovedBudget.Text)) + (Convert.ToInt64(Utility.RemoveComma(txtCurrentRelease.Text)));        
                            }
                            
                            if (ba.BudgetAmount == 0 || ba.BudgetAmount < 0)
                            {
                                Master.ShowMessage(Message.BudgetAmountCanNotbeZeroOrLessThanZero.Description, SiteMaster.MessageType.Error);
                                return;
                            }
                            ba.BudgetType = "SUPPLY";
                            lstBudgetApproval.Add(ba);
                            isSupplymentaryGrant = true;
                        }

                        mdlFundReleaseDetails.ObjectClassificationID = Convert.ToInt64(lblObjectsClassificationID.Text);
                        mdlFundReleaseDetails.BudgetApprovelID = Convert.ToInt64(lblBudgetApprovelID.Text);
                        mdlFundReleaseDetails.CurrentReleaseAmount = Convert.ToDouble(txtCurrentRelease.Text);
                        mdlFundReleaseDetails.FundReleaseID = FundReleaseID;
                        lsFundReleaeDetail.Add(mdlFundReleaseDetails);
                        if (txtCurrentRelease.Visible && !isTxtReApropriation)
                            isTxtReApropriation = true;
	                                       
                    }
                }
                if ((isRepropriation || isSupplymentaryGrant) && (isTxtReApropriation))
                {
                    bllAccounts.SaveApprovedBudget(lstBudgetApproval);
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                   
                   
                }
                if (lsFundReleaeDetail!=null && lsFundReleaeDetail.Count>0)
                {
                    foreach (AT_FundReleaseDetails item in lsFundReleaeDetail)
                    {
                        bllAccounts.AddFundReleaseDetails(item);
                    }
                    btnSave.Enabled = false;
                    btnSave.Visible = false;
                    IsFundReleaseIDExist = bllAccounts.GetFundReleaseDetailsByFundReleaseID(FundReleaseID);
                    BindGrid();
                }
                else
                {
                    Master.ShowMessage(Message.txtBudgetAmountNotBEmpty.Description, SiteMaster.MessageType.Warning);
                    return;
                }
                
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}