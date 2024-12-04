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
    public partial class SearchSanction : BasePage
    {
        public string GridDisplay = "block";
        AccountsBLL bllAccounts = new AccountsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindExpenseTypeDropdown();
                    Dropdownlist.DDLAssetType(ddlSanctionOn, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLFinancialYear(ddlFinancialYear, null, false, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLMonthList(ddlMonth, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLSanctionStatus(ddlSanctionStatus, (int)Constants.DropDownFirstOption.All);

                    DateTime Now = DateTime.Now;
                    Dropdownlist.SetSelectedValue(ddlMonth, Now.ToString("MMMM"));
                    //ddlSanctionStatus.Items.Insert(0, new ListItem("Select", ""));

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
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindExpenseTypeDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
            {
                Dropdownlist.DDLExpenseType(ddlSanctionType, null, false, (int)Constants.DropDownFirstOption.All);
            }
            else
            {
                Dropdownlist.DDLExpenseType(ddlSanctionType, "O", false, (int)Constants.DropDownFirstOption.All);
            }

            ddlSanctionType.Items.Add(new ListItem("Misc.", "0"));

            Dropdownlist.SetSelectedValue(ddlSanctionType, ((long)Constants.ExpenseType.RepairMaintainance).ToString());
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindGrid()
        {
            string SanctionNo = txtSanctionNo.Text;
            long SanctionType = ddlSanctionType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
            long SanctionedOn = ddlSanctionOn.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionOn.SelectedItem.Value);
            string FinancialYear = ddlFinancialYear.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Value);
            string Month = ddlMonth.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlMonth.SelectedItem.Value);
            long SanctionStatus = ddlSanctionStatus.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionStatus.SelectedItem.Value);

            List<AT_Sanction> lstSanction = bllAccounts.GetSanctions(SanctionNo, SanctionType, SanctionedOn, FinancialYear, Month, SanctionStatus);
            gvSanctions.DataSource = lstSanction;
            gvSanctions.DataBind();
        }


        protected void gvSanctions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvSanctions.EditIndex != e.Row.RowIndex)
                    {
                        Label lblStatusDate = (Label)e.Row.FindControl("lblStatusDate");
                        HyperLink btnTaxSheet = (HyperLink)e.Row.FindControl("btnTaxSheet");
                        Label lblSanctionTypeID = (Label)e.Row.FindControl("lblSanctionTypeID");
                        Label lblSanctionType = (Label)e.Row.FindControl("lblSanctionType");
                        Label lblSanctionTypeName = (Label)e.Row.FindControl("lblSanctionTypeName");
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                        Label lblSanctionStatusID = (Label)e.Row.FindControl("lblSanctionStatusID");
                        Label lblSanctionAmount = (Label)e.Row.FindControl("lblSanctionAmount");

                        Label lblAssetType = (Label)e.Row.FindControl("lblAssetType");
                        Label lblExpenseType = (Label)e.Row.FindControl("lblExpenseType");

                        LinkButton lbtnChangeStatus = (LinkButton)e.Row.FindControl("btnChangeStatus");

                        if (lblSanctionTypeID.Text != "")
                        {
                            btnEdit.Visible = false;
                            btnDelete.Visible = false;

                            if (Convert.ToInt64(lblSanctionTypeID.Text) == Convert.ToInt64(Constants.ExpenseType.RepairMaintainance))
                            {
                                if (lblAssetType.Text == "")
                                {
                                    lblSanctionType.Text = lblExpenseType.Text;
                                }
                                else
                                {
                                    lblSanctionType.Text = lblExpenseType.Text + " (" + lblAssetType.Text + ")";
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt64(lblSanctionStatusID.Text) != (long)Constants.SanctionStatus.Sanctioned)
                            {
                                btnEdit.Visible = false;
                                btnDelete.Visible = false;
                            }
                        }



                        long SanctionTypeID = 0;

                        if (string.IsNullOrEmpty(lblSanctionTypeID.Text))
                            lblSanctionType.Text = lblSanctionTypeName.Text;


                        if (!string.IsNullOrEmpty(lblStatusDate.Text))
                            lblStatusDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblStatusDate.Text));

                        if (lblSanctionTypeID != null)
                            if (!string.IsNullOrEmpty(lblSanctionTypeID.Text))
                                SanctionTypeID = Convert.ToInt64(lblSanctionTypeID.Text);

                        if (SanctionTypeID == Convert.ToInt64(Constants.ExpenseType.RepairMaintainance) || SanctionTypeID == Convert.ToInt64(Constants.ExpenseType.NewPurchase))
                        {
                            btnTaxSheet.Visible = true;
                        }
                        else
                        {
                            btnTaxSheet.Visible = false;
                        }

                        if (lblSanctionAmount.Text.Trim() != string.Empty && lblSanctionAmount.Text.Trim() != "0")
                        {
                            double SanctionAmount = Convert.ToDouble(lblSanctionAmount.Text.Trim());

                            lblSanctionAmount.Text = Utility.GetRoundOffValueAccounts(SanctionAmount);
                        }

                        if (Convert.ToInt64(lblSanctionStatusID.Text) == (long)Constants.SanctionStatus.PaymentReleased)
                        {
                            lbtnChangeStatus.Enabled = false;
                            lbtnChangeStatus.CssClass = "btn btn-primary btn_24 change_status disabled";
                        }
                        else
                        {
                            lbtnChangeStatus.Enabled = true;
                            lbtnChangeStatus.CssClass = "btn btn-primary btn_24 change_status";
                        }
                    }

                    if (gvSanctions.EditIndex == e.Row.RowIndex)
                    {
                        Label lblStatusDate = (Label)e.Row.FindControl("lblStatusDate");
                        Label lblEditMonth = (Label)e.Row.FindControl("lblEditMonth");
                        Label lblObjectClassification = (Label)e.Row.FindControl("lblObjectClassification");

                        if (!string.IsNullOrEmpty(lblStatusDate.Text))
                            lblStatusDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblStatusDate.Text));

                        DropDownList ddlgvMonth = (DropDownList)e.Row.FindControl("ddlgvMonth");
                        DropDownList ddlgvObjectClassification = (DropDownList)e.Row.FindControl("ddlgvObjectClassification");

                        Dropdownlist.DDLMonthList(ddlgvMonth);
                        Dropdownlist.DDLObjectClassification(ddlgvObjectClassification);

                        Dropdownlist.SetSelectedValue(ddlgvMonth, lblEditMonth.Text);
                        Dropdownlist.SetSelectedValue(ddlgvObjectClassification, lblObjectClassification.Text);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSanctions.PageIndex = e.NewPageIndex;
                gvSanctions.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnChangeStatus = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)btnChangeStatus.NamingContainer;

                Label lblID = (Label)gvRow.FindControl("lblID");
                Label lblSanctionStatusID = (Label)gvRow.FindControl("lblSanctionStatusID");
                Label lblTokenNumber = (Label)gvRow.FindControl("lblTokenNumber");

                hdnLabel.Text = lblID.Text;
                Dropdownlist.DDLSanctionStatus(ddlModalSanctionStatus);
                Dropdownlist.SetSelectedValue(ddlModalSanctionStatus, lblSanctionStatusID.Text);
                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                txtTokenNumber.Text = lblTokenNumber.Text;

                dvTokenNumber.Attributes.Remove("style");

                if (Convert.ToInt64(lblSanctionStatusID.Text) == (long)Constants.SanctionStatus.SenttoAGOffice)
                {
                    dvTokenNumber.Attributes.Add("style","display:block;");
                }
                else
                {
                    dvTokenNumber.Attributes.Add("style", "display:none;");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#ChangeStatus').modal();", true);

                ddlModalSanctionStatus.Attributes.Add("required", "required");
                ddlModalSanctionStatus.CssClass = "required form-control";

                txtDate.Attributes.Add("required", "required");
                txtDate.CssClass = "disabled-future-date-picker form-control required";
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
                long SanctionStatus = Convert.ToInt64(ddlModalSanctionStatus.SelectedItem.Value);
                DateTime StatusDate = Convert.ToDateTime(txtDate.Text);
                string TokenNumber = txtTokenNumber.Text;
                AT_Sanction mdlSanction = new AT_Sanction();

                mdlSanction.SanctionStatusID = SanctionStatus;
                mdlSanction.SanctionStatusDate = StatusDate;
                mdlSanction.ID = Convert.ToInt64(hdnLabel.Text);
                mdlSanction.TokenNumber = (TokenNumber.Trim() == string.Empty ? (int?)null : Convert.ToInt32(TokenNumber.Trim()));
                mdlSanction.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlSanction.ModifiedDate = DateTime.Now;

                bllAccounts.UpdateSanctionOnSearchSanction(mdlSanction);

                bllAccounts.UpdateSanctionStatusInMonthlyExpense(Convert.ToInt64(hdnLabel.Text), SanctionStatus, (int)SessionManagerFacade.UserInformation.ID);

                BindGrid();

                ddlModalSanctionStatus.Attributes.Remove("required");
                ddlModalSanctionStatus.CssClass = "form-control";

                txtDate.Attributes.Remove("required");
                txtDate.CssClass = "date-picker form-control";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    string SanctionNo = txtSanctionNo.Text;
                    long SanctionType = ddlSanctionType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                    long SanctionedOn = ddlSanctionOn.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionOn.SelectedItem.Value);
                    string FinancialYear = ddlFinancialYear.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Value);
                    string Month = ddlMonth.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlMonth.SelectedItem.Value);
                    long SanctionStatus = ddlSanctionStatus.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionStatus.SelectedItem.Value);

                    List<AT_Sanction> lstSanction = bllAccounts.GetSanctions(SanctionNo, SanctionType, SanctionedOn, FinancialYear, Month, SanctionStatus);
                    AT_Sanction mdlSanction = new AT_Sanction();
                    mdlSanction.ID = 0;
                    mdlSanction.Month = "";
                    mdlSanction.SanctionNo = "";
                    mdlSanction.SanctionTypeName = "";
                    mdlSanction.ObjectClassificationID = 0;
                    mdlSanction.SanctionAmount = null;
                    mdlSanction.SanctionStatusID = (long?)Constants.SanctionStatus.Sanctioned;
                    mdlSanction.SanctionStatusDate = DateTime.Today;
                    lstSanction.Add(mdlSanction);


                    gvSanctions.PageIndex = gvSanctions.PageCount;
                    gvSanctions.DataSource = lstSanction;
                    gvSanctions.DataBind();

                    gvSanctions.EditIndex = gvSanctions.Rows.Count - 1;
                    gvSanctions.DataBind();
                    gvSanctions.Rows[gvSanctions.Rows.Count - 1].FindControl("ddlgvMonth").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                DateTime Now = DateTime.Now;
                int Year = Now.Year;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                long SanctionID = Convert.ToInt32(((Label)gvSanctions.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                string Month = ((DropDownList)gvSanctions.Rows[rowIndex].Cells[0].FindControl("ddlgvMonth")).SelectedItem.Text;
                string SanctionNo = ((TextBox)gvSanctions.Rows[rowIndex].Cells[1].FindControl("txtgvSanctionNo")).Text.Trim();
                string SanctionType = ((TextBox)gvSanctions.Rows[rowIndex].Cells[2].FindControl("txtSanctionType")).Text.Trim();
                long ObjectClassification = Convert.ToInt64(((DropDownList)gvSanctions.Rows[rowIndex].Cells[3].FindControl("ddlgvObjectClassification")).SelectedItem.Value);
                double SanctionAmount = Convert.ToDouble(((TextBox)gvSanctions.Rows[rowIndex].Cells[4].FindControl("txtSanctionAmount")).Text.Trim());
                string SanctionStatus = ((Label)gvSanctions.Rows[rowIndex].Cells[5].FindControl("txtSanctionStatus")).Text.Trim();
                DateTime SanctionStatusDate = Convert.ToDateTime(((Label)gvSanctions.Rows[rowIndex].Cells[6].FindControl("lblStatusDate")).Text.Trim());


                #region Is Sancioned Amounmt  is valid
                string finanacialYear = ddlFinancialYear.SelectedItem.Text;
                bool IsSanctionedValid = bllAccounts.IsSanctionedAmountExceed(finanacialYear, SanctionAmount, ObjectClassification);
                if (!IsSanctionedValid)
                {
                    string message = "Sanctioned amount is exceeding funds release for this financial year.";
                    Master.ShowMessage(message, SiteMaster.MessageType.Error);
                    return;
                }


                #endregion

                AT_Sanction mdlSanction = new AT_Sanction();
                mdlSanction.ID = SanctionID;
                mdlSanction.Month = Month;
                mdlSanction.SanctionNo = SanctionNo;
                mdlSanction.SanctionTypeName = "Misc. – " + SanctionType;
                mdlSanction.ObjectClassificationID = ObjectClassification;
                mdlSanction.SanctionAmount = SanctionAmount;
                mdlSanction.SanctionStatusID = (long)Constants.SanctionStatus.Sanctioned;
                mdlSanction.SanctionStatusDate = SanctionStatusDate;

                bool IsSanctionNoExist = bllAccounts.IsSanctionNoExist(SanctionNo);

                if (IsSanctionNoExist && SanctionID == 0)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (Month.ToUpper().Trim() == "JANUARY" || Month.ToUpper().Trim() == "FEBRUARY" || Month.ToUpper().Trim() == "MARCH" || Month.ToUpper().Trim() == "APRIL" || Month.ToUpper().Trim() == "MAY" || Month.ToUpper().Trim() == "JUNE")
                {
                    DateTime CurrentYear = new DateTime(Year, 1, 1);
                    mdlSanction.FinancialYear = string.Format("{0}-{1}", Year - 1, CurrentYear.ToString("yy"));
                }
                else
                {
                    DateTime NextYear = new DateTime(Year + 1, 1, 1);
                    mdlSanction.FinancialYear = string.Format("{0}-{1}", Year, NextYear.ToString("yy"));
                }

                if (SanctionID == 0)
                {
                    bllAccounts.AddSanction(mdlSanction);
                }
                else
                {
                    mdlSanction.ModifiedBy = (int?)mdlUser.ID;
                    mdlSanction.ModifiedDate = DateTime.Now;
                    bllAccounts.UpdateSanction(mdlSanction);
                }

                if (SanctionID == 0)
                {
                    gvSanctions.PageIndex = 0;
                }

                gvSanctions.EditIndex = -1;
                BindGrid();
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvSanctions.EditIndex = e.NewEditIndex;
                BindGrid();
                gvSanctions.Rows[e.NewEditIndex].FindControl("ddlgvMonth").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSanctions.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSanctionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridDisplay = "none";
                if (Convert.ToInt64(ddlSanctionType.SelectedItem.Value) == (long)Constants.ExpenseType.RepairMaintainance)
                {
                    ddlSanctionOn.Enabled = true;
                }
                else
                {
                    ddlSanctionOn.ClearSelection();
                    ddlSanctionOn.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSanctions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int SanctionID = Convert.ToInt32(((Label)gvSanctions.Rows[e.RowIndex].FindControl("lblID")).Text);
                bllAccounts.DeleteSanction(SanctionID);
                Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#ChangeStatus').modal(hide);", true);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}