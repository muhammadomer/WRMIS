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
    public partial class SearchMonthlyExpenses : BasePage
    {
        AccountsBLL bll = new AccountsBLL();
        bool isExpenseSubmitted = false;
        bool isExpenseSubmittedByAccountOfficer = false;
        string _FinancialYear = "";
        string _Month = "";
        long _ExpenseMadeby = 0;
        string _PMIUStaff = "";
        int _PageIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (SessionManagerFacade.UserInformation.DesignationID != Convert.ToInt64(Constants.Designation.ADM))
                    {
                        ddlExpenseMadeBy.Enabled = false;
                        ddlPMIUStaff_SelectedIndexChanged(null, null);
                        divPMIUNExpenseMadeBy.Visible = true;
                    }
                    else
                    {
                        divPMIUNExpenseMadeBy.Visible = false;
                    }
                    BindMonthDropdown();
                    BindFinancialYearDropdown();
                    if (Convert.ToBoolean(Request.QueryString["LoadHistory"]))
                    {
                        Dictionary<string, object> dd_SearchAccount = new Dictionary<string, object>();
                        dd_SearchAccount = Session["SME_SC_SearchCriteria"] as Dictionary<string, object>;
                        _FinancialYear = Convert.ToString(dd_SearchAccount["_FinancialYear"]);
                        _Month = Convert.ToString(dd_SearchAccount["_Month"]);
                        _ExpenseMadeby = Convert.ToInt64(dd_SearchAccount["_ExpenseMadeby"]);
                        _PMIUStaff = Convert.ToString(dd_SearchAccount["_PMIUStaff"]);
                        ddlExpenseMadeBy.Enabled = true;
                        ddlFinancialYear.SelectedValue = _FinancialYear;
                        ddlMonth.SelectedValue = _Month;
                        ddlPMIUStaff.SelectedValue = _PMIUStaff;
                        ddlPMIUStaff_SelectedIndexChanged(null, null);
                        ddlExpenseMadeBy.SelectedValue = _ExpenseMadeby == 0 ? "" : "" + _ExpenseMadeby;
                        gvMonthlyExpense.Visible = true;
                        IsSubmitExpenseVisible();
                        gvMonthlyExpense.PageIndex = Convert.ToInt32(dd_SearchAccount["_PageIndex"]);
                        btnSearch_Click(null, null);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["RecordSaved"]))
                    {
                        string BillNo = Utility.GetStringValueFromQueryString("BillNo", "");

                        string message = "A new Expense has been added with Expense No  " + "<b>" + BillNo + "</b>";
                        lblRecordSaved.Text = message;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#ExpenseSaved').modal();", true);
                       // Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    }
                }
                
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void IsSubmitExpenseVisible()
        {
            if (divPMIUNExpenseMadeBy.Visible)
            {
                if (ddlPMIUStaff.SelectedItem.Value.ToUpper() == "O")
                {
                    divBtnSubmitExpense.Visible = true;
                }
                else
                {
                    divBtnSubmitExpense.Visible = true;
                }
            }
            else
            {
                divBtnSubmitExpense.Visible = true;
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 07-04-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds then financial year dropdown
        /// Created On 07-04-2017
        /// </summary>
        private void BindFinancialYearDropdown()
        {
            Dropdownlist.DDLFinancialYear(ddlFinancialYear);
            DateTime Now = DateTime.Now;
            if (Now.Month <= 6)
            {

                ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year - 1, Now.ToString("yy"));
                //ddlFinancialYear.SelectedValue = Now.AddYears(-1)+"-"+ Now.Year.ToString().Substring(2);
            }
            else
            {
                ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year, (Now.Year + 1).ToString().Substring(2));


                //ddlFinancialYear.SelectedValue = Now.AddYears(1) + "-" + Now.AddYears(2).ToString().Substring(2);
            }

        }

        /// <summary>
        /// This function binds the twelve months of year to months dropdown
        /// Created On 07-04-2017
        /// </summary>
        private void BindMonthDropdown()
        {
            List<ListItem> lstMonths = new List<ListItem>();

            for (int Month = 1; Month <= 12; Month++)
            {
                DateTime FirstDay = Convert.ToDateTime(string.Format("{0}-{1}-{2}", Month, 1, DateTime.Now.Year));

                lstMonths.Add(new ListItem
                {
                    Text = FirstDay.ToString("MMMM"),
                    Value = FirstDay.ToString("MMMM")
                });
            }

            DateTime Now = DateTime.Now;
            Dropdownlist.BindDropdownlist(ddlMonth, lstMonths, (int)Constants.DropDownFirstOption.Select, "Text", "Value");
            ddlMonth.SelectedValue = Now.ToString("MMMM");
        }


        private void BindExpensesMadeByDDL(char c)
        {
            if (c == 'O')
            {
                Dropdownlist.DDLOfficeADMusers(ddlExpenseMadeBy, false, (int)Constants.DropDownFirstOption.All, "Name", "ID", "O");
                ddlExpenseMadeBy.Attributes.Remove("required");
               
                ddlExpenseMadeBy.CssClass = "form-control";

            }
            else if (c == 'F')
            {
                Dropdownlist.DDLOfficeADMusers(ddlExpenseMadeBy, false, (int)Constants.DropDownFirstOption.All, "Name", "ID", "F");
                ddlExpenseMadeBy.Attributes.Remove("required");
                ddlExpenseMadeBy.CssClass = "form-control";
            }
            else if (c == 'A')
            {
                Dropdownlist.DDLADM(ddlExpenseMadeBy, false, (int)Constants.DropDownFirstOption.Select);
                ddlExpenseMadeBy.Attributes.Add("required", "required");
                ddlExpenseMadeBy.CssClass = "required form-control";


            }

        }


        protected void gvMonthlyExpense_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMonthlyExpense.PageIndex = e.NewPageIndex;
                Dictionary<string, object> dd_SearchAccount = new Dictionary<string, object>();
                dd_SearchAccount = Session["SME_SC_SearchCriteria"] as Dictionary<string, object>;
                dd_SearchAccount.Remove("_PageIndex");
                dd_SearchAccount.Add("_PageIndex", e.NewPageIndex);
                btnSearch_Click(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid(Dictionary<string, object> dd_SearchAccount)
        {
                //gvMonthlyExpense.Visible = true;
                //_FinancialYear = ddlFinancialYear == null ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Text));
                //_Month = ddlMonth == null ? "" : Convert.ToString(ddlMonth.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlMonth.SelectedItem.Text));
                //_ExpenseMadeby = divPMIUNExpenseMadeBy.Visible == false ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value));
                //_PMIUStaff = divPMIUNExpenseMadeBy.Visible == false ? "F" : Convert.ToString(ddlPMIUStaff.SelectedItem.Value);
                //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                //if (mdlUser.DesignationID == (long)Constants.Designation.ADM && _ExpenseMadeby == 0)
                //{
                //    _ExpenseMadeby = mdlUser.ID;
                //}
                //dd_SearchAccount.Clear();
                //dd_SearchAccount.Add("_FinancialYear", _FinancialYear);
                //dd_SearchAccount.Add("_Month", _Month);
                //dd_SearchAccount.Add("_ExpenseMadeby", _ExpenseMadeby);
                //dd_SearchAccount.Add("_PMIUStaff", _PMIUStaff);
                //dd_SearchAccount.Add("_PageIndex", 0);
                //Session["SME_SC_SearchCriteria"] = dd_SearchAccount;
            
            List<long> ResourceAllocationID = new List<long>();
            if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
            {
                if (ddlPMIUStaff.SelectedValue == "F")
                {
                    List<object> lstME = bll.SearchMonthlyExpnses(dd_SearchAccount, "AccountOfficerADM", SessionManagerFacade.UserInformation.ID);
                    gvMonthlyExpense.DataSource = lstME;
                    int AccountOfficerID = (int)SessionManagerFacade.UserInformation.ID;
                    ResourceAllocationID = (from rid in lstME select Convert.ToInt64(rid.GetType().GetProperty("ResourceAllocationID").GetValue(rid))).ToList();
                    ResourceAllocationID = ResourceAllocationID.Distinct().ToList();
                    isExpenseSubmittedByAccountOfficer = bll.IsExpenseSubmited(ddlFinancialYear.SelectedValue, ddlMonth.SelectedValue, ResourceAllocationID, AccountOfficerID);
                }
                else
                {
                    List<object> lstME = bll.SearchMonthlyExpnses(dd_SearchAccount, "AccountOfficer");
                    int AccountOfficerID = (int)SessionManagerFacade.UserInformation.ID;
                    ResourceAllocationID = (from rid in lstME select Convert.ToInt64(rid.GetType().GetProperty("ResourceAllocationID").GetValue(rid))).ToList();
                    ResourceAllocationID = ResourceAllocationID.Distinct().ToList();
                    isExpenseSubmittedByAccountOfficer = bll.IsExpenseSubmited(ddlFinancialYear.SelectedValue, ddlMonth.SelectedValue, ResourceAllocationID, AccountOfficerID);
                    gvMonthlyExpense.DataSource = lstME;
                }

                gvMonthlyExpense.Columns[7].Visible = true;

            }
            else
            {
                List<object> lstME = bll.SearchMonthlyExpnses(dd_SearchAccount);
                ResourceAllocationID = (from rid in lstME select Convert.ToInt64(rid.GetType().GetProperty("ResourceAllocationID").GetValue(rid))).ToList();
                ResourceAllocationID = ResourceAllocationID.Distinct().ToList();
                isExpenseSubmitted = bll.IsExpenseSubmited(ddlFinancialYear.SelectedValue, ddlMonth.SelectedValue, ResourceAllocationID);
                gvMonthlyExpense.DataSource = lstME;
                gvMonthlyExpense.Columns[7].Visible = false;
            }
            gvMonthlyExpense.PageIndex = Convert.ToInt32(dd_SearchAccount["_PageIndex"]);
            gvMonthlyExpense.Visible = true;
            gvMonthlyExpense.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dd_SearchAccount = new Dictionary<string, object>();
           
            _FinancialYear = ddlFinancialYear == null ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Text));
            _Month = ddlMonth == null ? "" : Convert.ToString(ddlMonth.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlMonth.SelectedItem.Text));
            _ExpenseMadeby = divPMIUNExpenseMadeBy.Visible == false ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value));
            _PMIUStaff = divPMIUNExpenseMadeBy.Visible == false ? "F" : Convert.ToString(ddlPMIUStaff.SelectedItem.Value);
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (mdlUser.DesignationID == (long)Constants.Designation.ADM && _ExpenseMadeby == 0)
            {
                _ExpenseMadeby = mdlUser.ID;
            }
            dd_SearchAccount.Clear();
            dd_SearchAccount.Add("_FinancialYear", _FinancialYear);
            dd_SearchAccount.Add("_Month", _Month);
            dd_SearchAccount.Add("_ExpenseMadeby", _ExpenseMadeby);
            dd_SearchAccount.Add("_PMIUStaff", _PMIUStaff);
            dd_SearchAccount.Add("_PageIndex", gvMonthlyExpense.PageIndex);
            Session["SME_SC_SearchCriteria"] = dd_SearchAccount;
            BindGrid(dd_SearchAccount);
            IsSubmitExpenseVisible();
        }

        protected void ddlPMIUStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMonthlyExpense.Visible = false;
            gvMonthlyExpense.PageIndex = 0;
            divBtnSubmitExpense.Visible = false;
            if (ddlPMIUStaff.SelectedItem.Value.ToUpper() == "F")
            {

                ddlExpenseMadeBy.Enabled = true;
                if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                {
                    BindExpensesMadeByDDL('A');
                    divBtnSubmitExpense.Visible = false;
                }
                else
                {
                    BindExpensesMadeByDDL('F');
                }
            }
            else if (ddlPMIUStaff.SelectedItem.Value.ToUpper() == "O")
            {
                BindExpensesMadeByDDL('O');
                ddlExpenseMadeBy.Enabled = true;
                if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                {
                    //divBtnSubmitExpense.Visible = true;
                    ddlExpenseMadeBy.Enabled = false;
                }
                else
                {
                    divBtnSubmitExpense.Visible = false;
                }
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlExpenseMadeBy, null, (int)Constants.DropDownFirstOption.All);
                ddlExpenseMadeBy.Enabled = false;
            }
        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMonthlyExpense.Visible = false;
            divBtnSubmitExpense.Visible = false;
            gvMonthlyExpense.PageIndex = 0;
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMonthlyExpense.Visible = false;
            divBtnSubmitExpense.Visible = false;
            gvMonthlyExpense.PageIndex = 0;
        }

        protected void ddlExpenseMadeBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMonthlyExpense.Visible = false;
            divBtnSubmitExpense.Visible = false;
            gvMonthlyExpense.PageIndex = 0;
        }

        protected void btnSubmitExpenses_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMonthlyExpense.Rows)
            {
                Label lblResourceAllocationID = row.FindControl("lblResourceAllocationID") as Label;
                long _ExpenseMadeby = 0;
                string _PMIUStaff = "";
                _ExpenseMadeby = divPMIUNExpenseMadeBy.Visible == false ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlExpenseMadeBy.SelectedItem.Value));
                _PMIUStaff = divPMIUNExpenseMadeBy.Visible == false ? "F" : Convert.ToString(ddlPMIUStaff.SelectedItem.Value);
                if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                {
                    bll.UpdateMonthlyExpenseStatus(Convert.ToInt64(lblResourceAllocationID.Text),
                                                              ddlFinancialYear.SelectedItem.Text,
                                                                      ddlMonth.SelectedItem.Text,
                                                                                      _PMIUStaff,
                                                                                  _ExpenseMadeby,
                                                                                   SessionManagerFacade.UserInformation.ID);
                }
                else
                {

                    bll.UpdateMonthlyExpenseStatus(Convert.ToInt64(lblResourceAllocationID.Text),
                                                              ddlFinancialYear.SelectedItem.Text,
                                                                      ddlMonth.SelectedItem.Text,
                                                                                      _PMIUStaff,
                                                                                  _ExpenseMadeby,
                                                                                   SessionManagerFacade.UserInformation.ID, "ADM");

                }
            }
            if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
            {
                Master.ShowMessage(Message.AccountOfficerSubmitExpenses.Description, SiteMaster.MessageType.Success);
            }
            else
            {
                Master.ShowMessage(Message.ExpenseSubmitToAccountOffice.Description, SiteMaster.MessageType.Success);
            }

            btnSearch_Click(null, null);
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnAdd = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lbtnAdd.NamingContainer;

                Label lblResourceAllocationID = (Label)gvRow.FindControl("lblResourceAllocationID");
                Label lblTotalClaim = (Label)gvRow.FindControl("lblTotalClaim");

                hdnUrl.Value = string.Format("FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}", ddlFinancialYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, lblResourceAllocationID.Text, lblTotalClaim.Text);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                {
                    Dropdownlist.DDLExpenseType(ddlExpenseType, null, false, (int)Constants.DropDownFirstOption.NoOption);
                }
                else
                {
                    Dropdownlist.DDLExpenseType(ddlExpenseType, "O", false, (int)Constants.DropDownFirstOption.NoOption);
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ExpenseType", "$('#dvExpenseType').modal();", true);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            long ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value);
            string Url = hdnUrl.Value;
            string RedirectionUrl = string.Empty;

            if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
            {
                RedirectionUrl = string.Format("{0}?{1}", "RepairMaintainanceExpense.aspx", Url);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
            {
                RedirectionUrl = string.Format("{0}?{1}", "POLExpense.aspx", Url);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
            {
                RedirectionUrl = string.Format("{0}?{1}", "TADAExpense.aspx", Url);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
            {
                RedirectionUrl = string.Format("{0}?{1}", "NewPurchaseExpense.aspx", Url);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
            {
                RedirectionUrl = string.Format("{0}?{1}", "OtherExpense.aspx", Url);
            }

            Response.Redirect(RedirectionUrl, false);
        }

        protected void gvMonthlyExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkBtn = e.Row.FindControl("lbtnAdd") as LinkButton;
                if (SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                {
                    if (ddlPMIUStaff.SelectedValue == "F")
                    {

                        if (isExpenseSubmittedByAccountOfficer)
                        {
                            lnkBtn.Enabled = false;
                            lnkBtn.CssClass += " disabled";
                            btnSubmitExpenses.Enabled = false;
                        }
                        else
                        {
                            lnkBtn.Enabled = true;
                            lnkBtn.CssClass += " btn btn-success btn_32 plus";
                            btnSubmitExpenses.Enabled = true;
                        }
                    }
                    else
                    {
                        if (isExpenseSubmittedByAccountOfficer)
                        {
                            lnkBtn.Enabled = false;
                            lnkBtn.CssClass += " disabled";
                            btnSubmitExpenses.Enabled = false;
                        }
                        else
                        {
                            lnkBtn.Enabled = true;
                            lnkBtn.CssClass += " btn btn-success btn_32 plus";
                            btnSubmitExpenses.Enabled = true;
                        }
                    }
                }
                else
                {
                    if (isExpenseSubmitted)
                    {
                        lnkBtn.Enabled = false;
                        lnkBtn.CssClass += " disabled";
                        btnSubmitExpenses.Enabled = false;
                    }
                    else
                    {
                        lnkBtn.Enabled = true;
                        lnkBtn.CssClass += " btn btn-success btn_32 plus";
                        btnSubmitExpenses.Enabled = true;
                    }
                }

            }
        }
    }
}