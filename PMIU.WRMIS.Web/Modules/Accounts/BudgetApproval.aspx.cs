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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class BudgetApproval : BasePage
    {

        //List<AT_AccountsHead> lstAccountsHead = null;
        AccountsBLL bll = new AccountsBLL();
        #region View State Constants

        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    SetPageTitle();
                    OnLoadControlsVisibility();
                    BindFinancialYearDropdown();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void isApprovalBudget(bool isVisible)
        {
            tabs.Visible = true;

            if (isVisible)
            {
                btnRevised.Visible = false;
              //  btnBudgetReAppropriation.Visible = false;
                btnBudgetApproval.Visible = true;
                txtDate.Visible = false;
                date.Visible = false;
                txtAllocatedBudget.Visible = false;
                gvBudget.Visible = false;
                btnSave.Visible = false;
                dAllocatedBudget.Visible = false;
            }
            else
            {
                btnBudgetApproval.Visible = false;
                btnRevised.Visible = true;
                //btnBudgetReAppropriation.Visible = false;
                gvBudget.Columns[3].Visible = true;
                gvBudget.Columns[4].Visible = false;
                gvBudget.Columns[5].Visible = false;
                txtDate.Visible = true;
                date.Visible = false;
                dAllocatedBudget.Visible = false;
                txtDate.Enabled = false;
                date.Visible = true;
                gvBudget.Visible = true;
                btnSave.Visible = true;


            }
        }
        public void OnLoadControlsVisibility()
        {
            tabs.Visible = false;
            date.Visible = false;
            dAllocatedBudget.Visible = false;
            gvBudget.Visible = false;
            btnSave.Visible = false;
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindFinancialYearDropdown()
        {
            List<ListItem> lstYears = new List<ListItem>();

            DateTime Now = DateTime.Now;

            if (Now.Month <= 6)
            {
                for (int Year = Now.Year; Year > Now.Year - 15; Year--)
                {
                    DateTime CurrentYear = new DateTime(Year, 1, 1);

                    string FinancialYear = string.Format("{0}-{1}", Year - 1, CurrentYear.ToString("yy"));

                    lstYears.Add(new ListItem
                    {
                        Text = FinancialYear,
                        Value = FinancialYear
                    });
                }
            }
            else
            {
                for (int Year = Now.Year; Year > Now.Year - 15; Year--)
                {
                    DateTime NextYear = new DateTime(Year + 1, 1, 1);

                    string FinancialYear = string.Format("{0}-{1}", Year, NextYear.ToString("yy"));

                    lstYears.Add(new ListItem
                    {
                        Text = FinancialYear,
                        Value = FinancialYear
                    });
                }
            }

            Dropdownlist.BindDropdownlist(ddlFinancialYear, lstYears, (int)Constants.DropDownFirstOption.NoOption, "Text", "Value");
            if (Now.Month <= 6)
            {
                ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year - 1, Now.ToString("yy"));
            }
            else
            {
                ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year, (Now.Year + 1).ToString().Substring(2));
            }
            ddlFinancialYear_SelectedIndexChanged(null, null);
        }
        public void bindGrid(string FinancialYear)
        {


            List<object> lstBudgetApprove = new List<object>();
            lstBudgetApprove = bll.GetApprovedBudget(FinancialYear);
            gvBudget.DataSource = lstBudgetApprove;
            gvBudget.DataBind();
            if (Convert.ToBoolean(lstBudgetApprove[0].GetType().GetProperty("alreadyExist").GetValue(lstBudgetApprove[0])))
            {
                txtDate.Text = Convert.ToDateTime(lstBudgetApprove[0].GetType().GetProperty("BudgetDate").GetValue(lstBudgetApprove[0])).ToString("dd-MMM-yyyy");
                double ta = Convert.ToDouble(lstBudgetApprove[0].GetType().GetProperty("TotalApprovedBudget").GetValue(lstBudgetApprove[0]));
                Label lblamount = gvBudget.FooterRow.FindControl("lblTotalApproveBudget") as Label;
                lblamount.Text = Utility.GetRoundOffValueAccounts(ta);
                btnSave.Enabled=false;
                btnSave.CssClass+=" diabled";
                isApprovalBudget(false);
            }
            else
            {
                isApprovalBudget(true);
            }

        }
        protected void btnBudgetApproval_Click(object sender, EventArgs e)
        {
            ViewState["BudgetType"] = "APROVAL";
            #region "Visibilty of Controls"
            gvBudget.Visible = true;
            btnSave.CssClass = "btn btn-primary";
            btnSave.Enabled = true;
            btnSave.Visible = true;
            date.Visible = true;
            dAllocatedBudget.Visible = true;
            gvBudget.Columns[3].Visible = false;
            gvBudget.Columns[4].Visible = true;
            gvBudget.Columns[5].Visible = false;
            txtAllocatedBudget.Visible = true;
            txtDate.Visible = true;
            txtDate.Visible = true;
            txtAllocatedBudget.Enabled = true;
            txtAllocatedBudget.Text = string.Empty;
            txtDate.Enabled = true;
            lblAllocatedBudget.Text = "Allocated Budget";
            #endregion
        }

        protected void btnRevised_Click(object sender, EventArgs e)
        {
            ViewState["BudgetType"] = "REVISED";
            dAllocatedBudget.Visible = true;
            txtAllocatedBudget.Text = string.Empty;
            gvBudget.Columns[3].Visible = true;
            gvBudget.Columns[4].Visible = false;
            gvBudget.Columns[5].Visible = true;
            lblAllocatedBudget.Text = "Revised Total Budget (Rs)";
            dAllocatedBudget.Visible = true;
            txtAllocatedBudget.Visible = true;
            txtDate.Enabled = true;
            btnSave.CssClass = "btn btn-primary";
            btnSave.Enabled = true;
        }
        //protected void btnBudgetReAppropriation_Click(object sender, EventArgs e)
        //{
        //    ViewState["BudgetType"] = "REAPRO";
        //    dAllocatedBudget.Visible = false;
        //    gvBudget.Columns[3].Visible = true;
        //    gvBudget.Columns[4].Visible = false;
        //    gvBudget.Columns[5].Visible = true;
        //    txtDate.Enabled = true;
        //    ReAproDate.Value = txtDate.Text;
        //    btnSave.CssClass = "btn btn-primary";
        //    btnSave.Enabled = true;
        //    double OA= 0;
        //    ////long OCID = 0;
        //    foreach (GridViewRow row in gvBudget.Rows)
        //    {
        //        Label Oldamount = row.FindControl("lblApprovedBudget") as Label;
        //        TextBox txtNewValue = row.FindControl("txtNewValue") as TextBox;
        //        txtNewValue.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble((RemoveComma(Oldamount.Text))));
        //        OA = OA + Convert.ToDouble(RemoveComma(Oldamount.Text));
                
        //    }
        //    Label lblamount = gvBudget.FooterRow.FindControl("lblTotalNewValue") as Label;
        //    lblamount.Text =Utility.GetRoundOffValueAccounts(OA);
        //}
        protected void gvBudget_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            dAllocatedBudget.Visible = false;
            gvBudget.Columns[3].Visible = true;
            gvBudget.Columns[4].Visible = false;
        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FinancialYear = ddlFinancialYear.SelectedItem.Text;
           


                bindGrid(FinancialYear);
                int StartYear = Convert.ToInt32(ddlFinancialYear.SelectedValue.Split('-')[0]);
                ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, 7, StartYear);
                int EndYear = StartYear + 1;
                DateTime EndingDate = new DateTime(EndYear, 6, 30);
                DateTime Now = DateTime.Now;
                if (EndingDate > Now)
                {
                    ViewState[EndDate] = Now.ToString("dd-MM-yyyy");
                }
                else
                {
                    ViewState[EndDate] = string.Format("{0}-{1}-{2}", 30, 6, EndYear);
                }
               
           
        }
        public void AreYouSureToSave()
        {
                #region Budget Approve Date sould be in selected Financial Year
                int StartFinancialYear = Convert.ToInt32(ddlFinancialYear.SelectedItem.Text.ToString().Substring(0, 4));
                DateTime APDate = Convert.ToDateTime(txtDate.Text);
                //DateTime StartDateFY = new DateTime(StartFinancialYear, 7, 1);
                //DateTime EndDateFY = new DateTime(StartFinancialYear + 1, 6, 30);
                if (APDate > DateTime.Now)
                {
                    Master.ShowMessage(Message.ApproveDateShouldbetweenFinancialYear.Description, SiteMaster.MessageType.Error);
                    return;
                }
                #endregion
                #region "If date lie between Financial Year"
                double allocatedBudget = Convert.ToDouble(txtAllocatedBudget.Text);
                double tryToaddBudget = 0;
                long OCID = 0;
                foreach (GridViewRow row in gvBudget.Rows)
                {
                    TextBox amount = row.FindControl("txtApproveBudget") as TextBox;
                    tryToaddBudget = tryToaddBudget + Convert.ToDouble(amount.Text == "" ? "0" : amount.Text);
                }
                if (allocatedBudget == tryToaddBudget && tryToaddBudget > 0)
                {
                    List<AT_BudgetApprovel> lstba = new List<AT_BudgetApprovel>();
                    foreach (GridViewRow row in gvBudget.Rows)
                    {

                        AT_BudgetApprovel aProveBudget = new AT_BudgetApprovel();
                        TextBox amount = row.FindControl("txtApproveBudget") as TextBox;
                        Label ObjCID = row.FindControl("lblID") as Label;
                        tryToaddBudget = Convert.ToDouble(amount.Text);
                        OCID = Convert.ToInt64(ObjCID.Text);

                        aProveBudget.FinancialYear = ddlFinancialYear.SelectedItem.Text;
                        aProveBudget.BudgetDate = Convert.ToDateTime(txtDate.Text);
                        aProveBudget.BudgetType = "APROVAL";
                        aProveBudget.BudgetAmount = tryToaddBudget;
                        aProveBudget.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                        aProveBudget.CreatedDate = DateTime.Now;
                        aProveBudget.ObjectClassificationID = OCID;

                        lstba.Add(aProveBudget);
                    }
                    if (lstba.Count > 0)
                    {
                        if (bll.SaveApprovedBudget(lstba))
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            ddlFinancialYear_SelectedIndexChanged(null, null);
                        }
                    }
                }
                else
                {
                    Master.ShowMessage(Message.ApproveBudgetShouldEqualAllocatedBudget.Description, SiteMaster.MessageType.Error);
                }
                #endregion
            
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string bType = ViewState["BudgetType"] == null ? "" : ViewState["BudgetType"].ToString();
            #region APPROVAL

            if (bType == "APROVAL")
            {
                double Approval = 0;
                foreach (GridViewRow row in gvBudget.Rows)
                {
                    TextBox txtNewValue = row.FindControl("txtApproveBudget") as TextBox;
                    Approval = Approval + Convert.ToDouble(Utility.RemoveComma(txtNewValue.Text));
                }
                Label lblamount = gvBudget.FooterRow.FindControl("lblTotalOldValue") as Label;
                lblamount.Text = Approval.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#AreYouSureWantToSave').modal();", true);
            }
            #endregion
            //#region ReAppropriate
            //if (bType == "REAPRO")
            //{
            //    #region Budget Approve Date sould be in selected Financial Year
            //    int StartFinancialYear = Convert.ToInt32(ddlFinancialYear.SelectedItem.Text.ToString().Substring(0, 4));
            //    DateTime APDate = Convert.ToDateTime(txtDate.Text);
            //    //DateTime StartDateFY = new DateTime(StartFinancialYear, 7, 1);
            //    //DateTime EndDateFY = new DateTime(StartFinancialYear + 1, 6, 30);
            //    DateTime LastApproveDate = Convert.ToDateTime(ReAproDate.Value);
            //    if (APDate > DateTime.Now)
            //    {
            //        Master.ShowMessage(Message.ApproveDateShouldbetweenFinancialYear.Description, SiteMaster.MessageType.Error);
            //        return;
            //    }
            //    if (APDate < LastApproveDate)
            //    {
            //        Master.ShowMessage(Message.ReApropriateDateMustBGreaterThanLastReAppropriateDate.Description, SiteMaster.MessageType.Error);
            //        return;
            //    }
            //    #endregion
            //    double ReAproPriateBudget = 0;
            //    double OA = 0;
            //    double NA = 0;
            //    long APID = 0;
                
            //    foreach (GridViewRow row in gvBudget.Rows)
            //    {
            //        Label Oldamount = row.FindControl("lblApprovedBudget") as Label;
            //        TextBox NewValue = row.FindControl("txtNewValue") as TextBox;
            //        OA = OA + Convert.ToDouble(Oldamount.Text == string.Empty ? "0" : Oldamount.Text);
            //        NA = NA + Convert.ToDouble(NewValue.Text == string.Empty ? "0" : NewValue.Text);
            //    }
            //    Label lblamount = gvBudget.FooterRow.FindControl("lblTotalOldValue") as Label;
            //    Label lblTotalNewValue = gvBudget.FooterRow.FindControl("lblTotalNewValue") as Label;
            //    lblamount.Text = Utility.GetRoundOffValueAccounts(OA);
            //    lblTotalNewValue.Text = Utility.GetRoundOffValueAccounts(NA);

            //    if (OA == NA)
            //    {
            //        List<AT_BudgetApprovel> lstba = new List<AT_BudgetApprovel>();
            //        foreach (GridViewRow row in gvBudget.Rows)
            //        {
            //            AT_BudgetApprovel aProveBudget = new AT_BudgetApprovel();
            //            TextBox amount = row.FindControl("txtNewValue") as TextBox;
            //            Label ObjCID = row.FindControl("lblID") as Label;
            //            ReAproPriateBudget = Convert.ToDouble(amount.Text == string.Empty ? "0" : amount.Text);
            //            APID = Convert.ToInt64(ObjCID.Text);
            //            aProveBudget.ID = APID;
            //            aProveBudget.FinancialYear = ddlFinancialYear.SelectedItem.Text;
            //            aProveBudget.BudgetDate = Convert.ToDateTime(txtDate.Text);
            //            aProveBudget.BudgetType = bType;
            //            aProveBudget.BudgetAmount = ReAproPriateBudget;
            //            aProveBudget.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
            //            aProveBudget.ModifiedDate = DateTime.Now;
            //            lstba.Add(aProveBudget);
            //        }
            //        if (lstba.Count > 0)
            //        {
            //            if (bll.SaveApprovedBudget(lstba))
            //            {
            //                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            //                ddlFinancialYear_SelectedIndexChanged(null, null);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Master.ShowMessage(Message.SumofBudgetaryProvisionshouldbetosumofNewValue.Description, SiteMaster.MessageType.Error);
            //       // string amountF = string.Format("{0:#,##}", NA);
            //        //ClientScript.RegisterStartupScript(this.GetType(), "Confi", "$('.ReplaceMe').html(" + amountF + ");", true);
            //       // Label lblamount = gvBudget.FooterRow.FindControl("lblTotalApproveBudget") as Label;
            //        //lblamount.Text = amountF;
            //    }
            //}
            //#endregion
            #region Revised
            if (bType == "REVISED")
            {
                double RevisedAmount = 0;
                double OA = 0;
                double NA = 0;
                long APID = 0;
                foreach (GridViewRow row in gvBudget.Rows)
                {
                    Label Oldamount = row.FindControl("lblApprovedBudget") as Label;
                    TextBox NewValue = row.FindControl("txtNewValue") as TextBox;
                    OA = OA + Convert.ToDouble(Oldamount.Text == string.Empty ? "0" : Oldamount.Text);
                    NA = NA + Convert.ToDouble(NewValue.Text == string.Empty ? "0" : NewValue.Text);
                }
                Label lblamount = gvBudget.FooterRow.FindControl("lblTotalOldValue") as Label;
                Label lblTotalNewValue = gvBudget.FooterRow.FindControl("lblTotalNewValue") as Label;
                lblamount.Text = Utility.GetRoundOffValueAccounts(OA);
                lblTotalNewValue.Text = Utility.GetRoundOffValueAccounts(NA);
                DateTime APDate = Convert.ToDateTime(txtDate.Text);
                if (APDate > DateTime.Now)
                {
                    Master.ShowMessage(Message.ApproveDateShouldbetweenFinancialYear.Description, SiteMaster.MessageType.Error);
                    return;
                }
             double allocatedBudget=Convert.ToDouble(txtAllocatedBudget.Text);
                
                    if ((OA == NA || OA > NA || NA > OA) && (NA == allocatedBudget))
                    {
                        List<AT_BudgetApprovel> lstba = new List<AT_BudgetApprovel>();
                        foreach (GridViewRow row in gvBudget.Rows)
                        {
                            AT_BudgetApprovel aProveBudget = new AT_BudgetApprovel();
                            TextBox amount = row.FindControl("txtNewValue") as TextBox;
                            Label ObjCID = row.FindControl("lblID") as Label;
                            RevisedAmount = Convert.ToDouble(amount.Text == string.Empty ? "0" : amount.Text);
                            APID = Convert.ToInt64(ObjCID.Text);
                            aProveBudget.ID = APID;
                            aProveBudget.FinancialYear = ddlFinancialYear.SelectedItem.Text;
                            aProveBudget.BudgetDate = Convert.ToDateTime(txtDate.Text);
                            aProveBudget.BudgetType = bType;
                            aProveBudget.BudgetAmount = RevisedAmount;
                            aProveBudget.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                            aProveBudget.ModifiedDate = DateTime.Now;
                            lstba.Add(aProveBudget);
                        }
                        if (lstba.Count > 0)
                        {
                            if (bll.SaveApprovedBudget(lstba))
                            {
                                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                                ddlFinancialYear_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else
                    {
                        Master.ShowMessage(Message.RevisedTotalBudgetNotEqualNewValue.Description, SiteMaster.MessageType.Error);
                    }
            }
            #endregion
        }

        protected void btnSaveBudgetApproval_Click(object sender, EventArgs e)
        {
            AreYouSureToSave();
        }

        //protected void gvBudget_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string Amount = ((Label)e.Row.FindControl("lblApprovedBudget")).Text.ToString();
        //        TextBox txtNewValue = ((TextBox)e.Row.FindControl("txtNewValue"));
        //        txtNewValue.Text = RemoveComma(Amount);
        //    }
        //}
       
    }
}