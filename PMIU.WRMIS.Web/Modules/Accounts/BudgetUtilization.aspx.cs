using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class BudgetUtilization : BasePage
    {

        #region Initialize

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //btnSave.Enabled = base.CanAdd;
                    SetPageTitle();
                    BindDropdown();
                }
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Events

        protected void gvBudgetUtilization_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region Key
                    //ID,AccountHeadID,ObjectClassificationID,AccountsCode,ObjectClassification,BudetoryProvision,AmountReleased,PreviousExpenses,CurrentExpense,ExpenseDate,RemainingBalance,CreatedDate,CreatedBy

                    DataKey key = gvBudgetUtilization.DataKeys[e.Row.RowIndex];

                    string ID = Convert.ToString(key.Values["ID"]);
                    string AccountCode = Convert.ToString(key.Values["AccountCode"]);
                    string ObjectClassification = Convert.ToString(key.Values["ObjectClassification"]);
                    string BudetoryProvision = Convert.ToString(key.Values["BudetoryProvision"]);
                    string AmountReleased = Convert.ToString(key.Values["AmountReleased"]);
                    string PreviousExpenses = Convert.ToString(key.Values["PreviousExpenses"]);
                    string CurrentExpense = Convert.ToString(key.Values["CurrentExpense"]);
                    string ExpenseDate = Convert.ToString(key.Values["ExpenseDate"]);
                    string RemainingBalance = Convert.ToString(key.Values["RemainingBalance"]);
                    string ObjectClassificationID = Convert.ToString(key.Values["ObjectClassificationID"]);
                    #endregion
                    #region Control
                    Label lblAccountCode = (Label)e.Row.FindControl("lblAccountCode");
                    Label lblObjectClassification = (Label)e.Row.FindControl("lblObjectClassification");
                    Label lblBudetoryProvision = (Label)e.Row.FindControl("lblBudetoryProvision");
                    Label lblAmountReleased = (Label)e.Row.FindControl("lblAmountReleased");
                    Label lblPreviousExpenses = (Label)e.Row.FindControl("lblPreviousExpenses");
                    TextBox txtCurrentExpense = (TextBox)e.Row.FindControl("txtCurrentExpense");
                    TextBox txtExpenseDate = (TextBox)e.Row.FindControl("txtExpenseDate");
                    Label lblRemainingBalance = (Label)e.Row.FindControl("lblRemainingBalance");
                    Label lblObjectClassificationID = (Label)e.Row.FindControl("lblObjectClassificationID");
                    #endregion

                    if (ObjectClassificationID != "")
                        lblObjectClassificationID.Text = ObjectClassificationID;

                    if (AccountCode != "")
                        lblAccountCode.Text = AccountCode;

                    if (ObjectClassification != "")
                        lblObjectClassification.Text = ObjectClassification;

                    if (BudetoryProvision != "")
                        lblBudetoryProvision.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudetoryProvision));

                    if (AmountReleased != "")
                        lblAmountReleased.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(AmountReleased));

                    if (PreviousExpenses != "")
                        lblPreviousExpenses.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(PreviousExpenses));

                    if (RemainingBalance != "")
                        lblRemainingBalance.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(RemainingBalance));

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBudgetUtilization_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvBudgetUtilization.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBudgetUtilization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBudgetUtilization.PageIndex = e.NewPageIndex;
                gvBudgetUtilization.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            UA_Users mdlUsers = SessionManagerFacade.UserInformation;

            bool saveResult = false;

            foreach (GridViewRow Row in gvBudgetUtilization.Rows)
            {
                DataKey key = gvBudgetUtilization.DataKeys[Row.RowIndex];
                AT_BudgetUtilization budgetUtilization = new AT_BudgetUtilization();

                budgetUtilization.CreatedBy = Convert.ToInt32(mdlUsers.ID);
                budgetUtilization.CreatedDate = DateTime.Now;
                budgetUtilization.ModifiedBy = Convert.ToInt32(mdlUsers.ID);
                budgetUtilization.ModifiedDate = DateTime.Now;

                budgetUtilization.FinancialYear = ddlFinancialYear.SelectedItem.Text;
                budgetUtilization.Month = ddlMonth.SelectedItem.Text;
                budgetUtilization.AccountHeadID = Convert.ToInt64(key.Values["AccountHeadID"]);

                Label lblObjectClassificationID = (Label)Row.FindControl("lblObjectClassificationID");
                budgetUtilization.ObjectClassificationID = Convert.ToInt64(lblObjectClassificationID.Text);

                TextBox txtCurrentExpense = (TextBox)Row.FindControl("txtCurrentExpense");
                if (txtCurrentExpense.Text != "")
                    budgetUtilization.CurrentExpense = Convert.ToDouble(txtCurrentExpense.Text);

                TextBox txtExpenseDate = (TextBox)Row.FindControl("txtExpenseDate");
                if (txtExpenseDate.Text != "")
                    budgetUtilization.ExpenseDate = Convert.ToDateTime(txtExpenseDate.Text);

                TextBox txtDescription = (TextBox)Row.FindControl("txtDescription");
                budgetUtilization.Description = txtDescription.Text;

                Label lblBudetoryProvision = (Label)Row.FindControl("lblBudetoryProvision");

                if (!string.IsNullOrEmpty(txtCurrentExpense.Text.Trim()) && !string.IsNullOrEmpty(txtExpenseDate.Text.Trim()) && !string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    if (lblBudetoryProvision.Text.Trim() != string.Empty)
                    {
                        bool res = new ReferenceDataBLL().SaveBudgetUtilization(budgetUtilization);

                        if (res)
                        {
                            saveResult = true;
                        }
                        else
                        {
                            saveResult = false;
                        }
                    }
                }
            }

            if (saveResult)
            {
                Master.ShowMessage(Message.ValidRecordsSaved.Description, SiteMaster.MessageType.Success);
                BindGrid();
            }
            else
            {
                Master.ShowMessage(Message.SomeRecordsNotSaved.Description, SiteMaster.MessageType.Error);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();

        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
        }

        protected void ddlAccountHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
        }

        #endregion

        #region Function

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdown()
        {
            Dropdownlist.DDLFinancialYear(ddlFinancialYear);
            Dropdownlist.DDLMonthList(ddlMonth);
            


            //Change 03-Aug-2017 by ATEEQ, Lead by Wahab Sab, co-ordination with Shehzad Sab
            // Change Accout head ddl to Object Classifcation ddl whtih specif formate which mentioned PMIU
            // One line commented
            //Dropdownlist.DDLAccountHeadList(ddlAccountHead);

            List<AT_ObjectClassification> exsanctioned = new AccountsBLL().GetAllObjectClassifications();
            List<object> lst = (from item in exsanctioned where item.ID > 0 select new { ID = item.ID, Name = item.AccountsCode + "  (" + item.ObjectClassification + ")" }).ToList<object>();
            Dropdownlist.BindDropdownlist<List<object>>(ddlObjectClassification, lst, (int)Constants.DropDownFirstOption.All);







            DateTime Now = DateTime.Now;
            Dropdownlist.SetSelectedValue(ddlMonth, Now.ToString("MMMM"));

            string FinancialYear = string.Empty;

            if (Now.Month <= 6)
            {
                FinancialYear = string.Format("{0}-{1}", Now.Year - 1, Now.ToString("yy"));
            }
            else
            {
                DateTime NextYear = new DateTime(Now.Year + 1, 1, 1);

                FinancialYear = string.Format("{0}-{1}", Now.Year, NextYear.ToString("yy"));
            }

            Dropdownlist.SetSelectedValue(ddlFinancialYear, FinancialYear);
        }

        public void BindGrid()
        {

            string _FinancialYear = "";
            string _Month = "";
            long _ObjectClassificationID = 0;

            _FinancialYear = ddlFinancialYear.SelectedItem.Value;
            _Month = ddlMonth.SelectedItem.Text;
            if (ddlObjectClassification.SelectedIndex != 0)
            {
                _ObjectClassificationID = Convert.ToInt64(ddlObjectClassification.SelectedItem.Value);
                List<AT_GetBudgetUtilizationList_Result> BudgetList = new ReferenceDataBLL().GetBudgetUtilizationList(_FinancialYear, _Month, _ObjectClassificationID);
                List<object> lstBudgetUtilization = new AccountsBLL().GetBudgetUtilizationGridData(_FinancialYear, _Month, _ObjectClassificationID);
                gvBUShowCase.DataSource = lstBudgetUtilization;
                gvBUShowCase.DataBind();
                gvBUShowCase.Visible = true;
                gvBudgetUtilization.Visible = false;


                if (BudgetList != null && BudgetList.Count > 0)
                {
                    lblAccountCode.Text = BudgetList[0].AccountsCode;
                    lblObjectClassification.Text = ddlObjectClassification.SelectedItem.Text;
                    lblBudgetaryProvision.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudgetList[0].BudetoryProvision));
                    lblAmountReleased.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudgetList[0].AmountReleased));
                    lblRemainingAmount.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudgetList[0].RemainingBalance));
                    lblTotalExpenses.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudgetList[0].TotalSanctionAmount));
                    lblTotalExpenses.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(BudgetList[0].TotalSanctionAmount));
                    tableSelection.Visible = true;
                    
                }
            }
            else
            {
                _ObjectClassificationID = 0;
                tableSelection.Visible = false;
                List<AT_GetBudgetUtilizationList_Result> BudgetList = new ReferenceDataBLL().GetBudgetUtilizationList(_FinancialYear, _Month, _ObjectClassificationID);
                gvBudgetUtilization.DataSource = BudgetList;
                gvBudgetUtilization.DataBind();
                gvBudgetUtilization.Visible = true;
                gvBUShowCase.Visible = false;
            }
            
            

            //if (BudgetList.Count != 0)
            //{
            //    btnSave.Visible = true;
            //}
            //else
            //{
            //    btnSave.Visible = false;
            //}
        }

        private void EmptyGrid()
        {
            if (gvBudgetUtilization.Visible == true)
            {
               // btnSave.Visible = false;
                gvBudgetUtilization.DataSource = null;
                gvBudgetUtilization.DataBind();
                gvBudgetUtilization.Visible = false;
                
            }
            if (gvBUShowCase.Visible==true)
            {
                tableSelection.Visible = false;
                gvBUShowCase.Visible = false;
            }

        }

        #endregion

        //protected void txtDescription_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtDescription = (TextBox)sender;
        //        GridViewRow gvr = (GridViewRow)txtDescription.NamingContainer;

        //        TextBox txtCurrentExpense = (TextBox)gvr.FindControl("txtCurrentExpense");
        //        TextBox txtExpenseDate = (TextBox)gvr.FindControl("txtExpenseDate");

        //        SetRequiredFields(txtCurrentExpense, txtExpenseDate, txtDescription);

        //        if (!string.IsNullOrEmpty(txtDescription.Text))
        //        {
        //         //   btnSave.Focus();
        //        }
        //        else
        //        {
        //            txtDescription.Focus();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void txtExpenseDate_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtExpenseDate = (TextBox)sender;
        //        GridViewRow gvr = (GridViewRow)txtExpenseDate.NamingContainer;

        //        TextBox txtCurrentExpense = (TextBox)gvr.FindControl("txtCurrentExpense");
        //        TextBox txtDescription = (TextBox)gvr.FindControl("txtDescription");

        //        SetRequiredFields(txtCurrentExpense, txtExpenseDate, txtDescription);

        //        if (!string.IsNullOrEmpty(txtExpenseDate.Text))
        //        {
        //            txtDescription.Focus();
        //        }
        //        else
        //        {
        //            txtExpenseDate.Focus();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void txtCurrentExpense_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtCurrentExpense = (TextBox)sender;
        //        GridViewRow gvr = (GridViewRow)txtCurrentExpense.NamingContainer;

        //        TextBox txtExpenseDate = (TextBox)gvr.FindControl("txtExpenseDate");
        //        TextBox txtDescription = (TextBox)gvr.FindControl("txtDescription");
        //        Label lblBudetoryProvision = (Label)gvr.FindControl("lblBudetoryProvision");
        //        Label lblPreviousExpenses = (Label)gvr.FindControl("lblPreviousExpenses");

        //        if (!string.IsNullOrEmpty(txtCurrentExpense.Text))
        //        {
        //            double CurrentExpense = Convert.ToDouble(txtCurrentExpense.Text.Trim());

        //            if (CurrentExpense < 1 || CurrentExpense > 9999999999)
        //            {
        //                txtCurrentExpense.Text = string.Empty;
        //                Master.ShowMessage(Message.CurrentExpenseValidValue.Description, SiteMaster.MessageType.Error);
        //            }
        //            else
        //            {
        //                if (lblBudetoryProvision.Text.Trim() == string.Empty)
        //                {
        //                    txtCurrentExpense.Text = string.Empty;
        //                    Master.ShowMessage(Message.NoBudgetoryProvision.Description, SiteMaster.MessageType.Error);
        //                }
        //                else
        //                {
        //                    double BudgetoryProvision = Convert.ToDouble(lblBudetoryProvision.Text);
        //                    double PreviousExpenses = (lblPreviousExpenses.Text.Trim() != string.Empty ? Convert.ToDouble(lblPreviousExpenses.Text) : 0);

        //                    if (CurrentExpense + PreviousExpenses > BudgetoryProvision)
        //                    {
        //                        txtCurrentExpense.Text = string.Empty;
        //                        Master.ShowMessage(Message.SumGreaterThanBudgetoryProvision.Description, SiteMaster.MessageType.Error);
        //                    }
        //                }
        //            }
        //        }

        //        SetRequiredFields(txtCurrentExpense, txtExpenseDate, txtDescription);

        //        if (!string.IsNullOrEmpty(txtCurrentExpense.Text))
        //        {
        //            txtExpenseDate.Focus();
        //        }
        //        else
        //        {
        //            txtCurrentExpense.Focus();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        private void SetRequiredFields(TextBox _TxtCurrentExpense, TextBox _TxtExpenseDate, TextBox _TxtDescription)
        {
            if (_TxtCurrentExpense.Text.Trim() == string.Empty && _TxtExpenseDate.Text.Trim() == string.Empty && _TxtDescription.Text.Trim() == string.Empty)
            {
                _TxtCurrentExpense.Attributes.Remove("required");
                _TxtCurrentExpense.CssClass = "form-control decimal2PInput";

                _TxtExpenseDate.Attributes.Remove("required");
                _TxtExpenseDate.CssClass = "form-control date-picker";

                _TxtDescription.Attributes.Remove("required");
                _TxtDescription.CssClass = "form-control";
            }
            else
            {
                _TxtCurrentExpense.Attributes.Add("required", "required");
                _TxtCurrentExpense.CssClass = "form-control decimal2PInput required";

                _TxtExpenseDate.Attributes.Add("required", "required");
                _TxtExpenseDate.CssClass = "form-control date-picker required";

                _TxtDescription.Attributes.Add("required", "required");
                _TxtDescription.CssClass = "required form-control";
            }
        }
    }
}