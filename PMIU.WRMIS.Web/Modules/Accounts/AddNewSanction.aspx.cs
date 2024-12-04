using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class AddNewSanction : BasePage
    {
        #region declare Global Variables
        AccountsBLL bll = new AccountsBLL();
        string VendroType = "";
        string Mode = "";
        string OperationAssetName = "";

        const string RepairMaintainence = "RM";
        const string POLReceipts = "POL";
        const string TADA = "TA";
        const string NewPurchase = "NP";
        const string OtherExpense = "OE";


        #endregion
        #region Page Load and bind DLL
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    ResetControls();
                }
                if (ddlFinancialYear != null && !string.IsNullOrEmpty(ddlFinancialYear.SelectedItem.Value))
                {
                    string FinancialYear = ddlFinancialYear.SelectedItem.Text;
                    //FY.FinancialYear = FinancialYear;
                    //FY.Visible = true;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindDll()
        {
            bindSanctionType();
            bindSanctionOn();
            BindFinancialYearDropdown();
            BindMonthDropdown(ddlMonth);
        }
        #endregion
        #region Bind all DDL
        private void bindSanctionType()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
            {
                Dropdownlist.DDLExpenseType(ddlSanctionType, null, false, (int)Constants.DropDownFirstOption.NoOption);
            }
            else
            {
                Dropdownlist.DDLExpenseType(ddlSanctionType, "O", false, (int)Constants.DropDownFirstOption.NoOption);
            }
            ddlSanctionType.SelectedValue = "1";
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
            }
            else
            {
                ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year, (Now.Year + 1).ToString().Substring(2));
            }
        }

        /// <summary>
        /// This function binds the twelve months of year to months dropdown
        /// Created On 07-04-2017
        /// </summary>
        private void BindMonthDropdown(DropDownList ddl)
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
            Dropdownlist.BindDropdownlist(ddl, lstMonths, (int)Constants.DropDownFirstOption.All, "Text", "Value");
            ddl.SelectedValue = Now.ToString("MMMM");
        }

        public void bindSanctionOn()
        {
            Dropdownlist.DDLAccountAssetsType(ddlSanctionOn, (int)Constants.DropDownFirstOption.All);
        }

        #endregion
        #region Set Page Title
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        #endregion
        #region Bind Grid  and Grid Visibility
        public void BindGrid_RM(List<object> LstOfObjects)
        {

            gvRepairMaintenance.DataSource = LstOfObjects;
            gvRepairMaintenance.DataBind();
            if (LstOfObjects == null || LstOfObjects.Count == 0)
            {
                gvRepairMaintenance.Columns[2].Visible = false;
            }
            else
            {
                gvRepairMaintenance.Columns[2].Visible = true;

            }
            GridViewVisibilty("RM");

        }
        public void BindGrid_POL(List<object> LstOfObjects)
        {
            gvPolRecipts.DataSource = LstOfObjects;
            gvPolRecipts.DataBind();
            if (LstOfObjects == null || LstOfObjects.Count == 0)
            {
                gvPolRecipts.Columns[1].Visible = false;

            }
            else
            {
                gvPolRecipts.Columns[1].Visible = true;
            }
            GridViewVisibilty("POL");
        }
        public void BindGrid_TADA(List<object> LstOfObjects)
        {
            gvTADA.DataSource = LstOfObjects;
            gvTADA.DataBind();
            if (LstOfObjects == null || LstOfObjects.Count == 0)
            {
                gvTADA.Columns[1].Visible = false;

            }
            else
            {
                gvTADA.Columns[1].Visible = true;
            }
            GridViewVisibilty("TADA");
        }
        public void BindGrid_NP(List<object> LstOfObjects)
        {
            gvNewPurchase.DataSource = LstOfObjects;
            gvNewPurchase.DataBind();
            if (LstOfObjects == null || LstOfObjects.Count == 0)
            {
                gvNewPurchase.Columns[1].Visible = false;

            }
            else
            {
                gvNewPurchase.Columns[1].Visible = true;


            }
            GridViewVisibilty("NP");
        }
        public void BindGrid_OE(List<object> LstOfObjects)
        {
            gvOtherExpense.DataSource = LstOfObjects;
            gvOtherExpense.DataBind();
            if (LstOfObjects == null || LstOfObjects.Count == 0)
            {
                gvOtherExpense.Columns[1].Visible = false;
            }
            else
            {
                gvOtherExpense.Columns[1].Visible = true;
            }
            GridViewVisibilty("OE");
        }

        public void GridViewVisibilty(string gridName)
        {
            switch (gridName)
            {
                case "RM":
                    GridRepairMaintenance.Visible = true;
                    GridPolRecipts.Visible = false;
                    GridTADA.Visible = false;
                    GridNewPurchase.Visible = false;

                    GridOtherExpense.Visible = false;
                    break;
                case "POL":
                    GridRepairMaintenance.Visible = false;
                    GridPolRecipts.Visible = true;
                    GridTADA.Visible = false;
                    GridNewPurchase.Visible = false;

                    GridOtherExpense.Visible = false;
                    break;
                case "TADA":
                    GridRepairMaintenance.Visible = false;
                    GridPolRecipts.Visible = false;
                    GridTADA.Visible = true;
                    GridNewPurchase.Visible = false;

                    GridOtherExpense.Visible = false;
                    break;
                case "NP":
                    GridRepairMaintenance.Visible = false;
                    GridPolRecipts.Visible = false;
                    GridTADA.Visible = false;
                    GridNewPurchase.Visible = true;
                    GridOtherExpense.Visible = false;

                    break;
                case "OE":
                    GridRepairMaintenance.Visible = false;
                    GridPolRecipts.Visible = false;
                    GridTADA.Visible = false;
                    GridNewPurchase.Visible = false;
                    GridOtherExpense.Visible = true;

                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Button Search and Sanction Type SelectedIndexChanged
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["BtnPressed"] = true;
                string FinancialYear = ddlFinancialYear.SelectedItem.Text;
                string Month = ddlMonth.SelectedItem.Value != string.Empty ? ddlMonth.SelectedItem.Value : null;
                long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedValue);
                string AssetTypeID = ddlSanctionOn.SelectedItem.Text;
                long AccountOfficer = SessionManagerFacade.UserInformation.ID;
                List<object> lstRepairMaintenance = new List<object>();
                if (ExpenseTypeID == 1)
                {
                    lstRepairMaintenance = bll.RMGridData(FinancialYear, Month, ExpenseTypeID, AssetTypeID, AccountOfficer);
                    BindGrid_RM(lstRepairMaintenance);
                }
                if (ExpenseTypeID == 2)
                {
                    lstRepairMaintenance = bll.POLGridData(FinancialYear, Month, ExpenseTypeID, AssetTypeID, AccountOfficer);
                    BindGrid_POL(lstRepairMaintenance);
                }
                if (ExpenseTypeID == 3)
                {
                    lstRepairMaintenance = bll.TADAGridData(FinancialYear, Month, ExpenseTypeID, AccountOfficer);
                    BindGrid_TADA(lstRepairMaintenance);
                }
                if (ExpenseTypeID == 4)
                {
                    lstRepairMaintenance = bll.NPGridData(FinancialYear, Month, ExpenseTypeID, AccountOfficer);
                    BindGrid_NP(lstRepairMaintenance);
                }
                if (ExpenseTypeID == 5)
                {
                    lstRepairMaintenance = bll.OEGridData(FinancialYear, Month, ExpenseTypeID, AccountOfficer);
                    BindGrid_OE(lstRepairMaintenance);
                }
                if (lstRepairMaintenance.Count > 0)
                {
                    btnSave.Visible = true;
                    btnSave.Enabled = false;
                    btnSave.CssClass += " disabled";
                    //string gridShortName = ExpenseTypeID == 1 ? "RM" : ExpenseTypeID == 2 ? "POL" : ExpenseTypeID == 3 ? "TADA" : ExpenseTypeID == 4 ? "NP" : ExpenseTypeID == 5 ? "OE" : "";
                    GridView grid = (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance ? gvRepairMaintenance : ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts ? gvPolRecipts : ExpenseTypeID == (long)Constants.ExpenseType.TADA ? gvTADA : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense ? gvOtherExpense : null);
                    grid.Visible = true;
                    pnlTotalClaim.Visible = true;
                }
                else
                {
                    btnSave.Visible = false;
                    pnlTotalClaim.Visible = false;
                }
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
                #region Sanction on Disable or Enable
                string SactionType = ddlSanctionType.SelectedItem.Text;
                bindSanctionOn();
                if (SactionType.ToUpper().Trim() == "REPAIR & MAINTENANCE".Trim())
                    ddlSanctionOn.Enabled = true;
                else
                    ddlSanctionOn.Enabled = false;


                #endregion
                HideAllPanels();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllPanels();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllPanels();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSanctionOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllPanels();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Show Tax Sheet and Print Sanction
        protected void PrintSanction_Click(object sender, EventArgs e)
        {

        }
        protected void ShowTaxSheet_Click(object sender, EventArgs e)
        {

        }

        #endregion
        #region gvRepairMaintenance Events


        protected void gvRepairMaintenance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Mode = e.CommandName;
                #region Rejected
                if (e.CommandName == "Rejected")
                {
                    GridViewRow rejectedRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ViewState["RejectedRowIndex"] = rejectedRow.RowIndex;
                    txtRejectionReason.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#RejectionPopUp').modal();", true);
                    txtRejectionReason.Attributes.Add("required", "required");
                    txtRejectionReason.CssClass = "required form-control";
                    ViewState["RejectedAssetName"] = e.CommandArgument;
                }
                #endregion
                #region Verify
                //if (e.CommandName == "Verify")
                //{

                //    List<object> lstRM = new List<object>();

                //    foreach (GridViewRow row in gvRepairMaintenance.Rows)
                //    {

                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblName = row.FindControl("lblName") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblAssetName = row.FindControl("lblAssetName") as Label;
                //        Label lblAssetType = row.FindControl("lblAssetType") as Label;
                //        Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
                //        Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
                //        Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
                //        Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
                //        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                //        Label lblVendorType = row.FindControl("lblVendorType") as Label;
                //        DropDownList ddlVendorType = row.FindControl("ddlVendorType") as DropDownList;
                //        Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
                //        Button btnEdit = row.FindControl("btnEdit") as Button;
                //        Button btnReject = row.FindControl("btnReject") as Button;
                //        Button btnVerify = row.FindControl("btnVerify") as Button;
                //        Button btnReConsider = row.FindControl("btnReConsider") as Button;

                //        if (lblAssetName.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                //        {
                //            OperationAssetName = lblAssetName.Text.ToString();
                //        }
                //        lstRM.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    NameOfStaff = lblName.Text,
                //                    Designation = lblDesignation.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    AssetName = lblAssetName.Text,
                //                    AssetType = lblAssetType.Text,
                //                    PurchaseItems = lblPurchaseItems.Text,
                //                    RepairItems = lblRepairItems.Text,
                //                    TotalClaim = "",//lblGrandTotal.Text,
                //                    SanctionStatus = lblSanctionedStatus.Text,
                //                    VendorType = ddlVendorType == null ? lblVendorType.Text : ddlVendorType.SelectedItem.Text,
                //                    ObjectClassification = lblObjectClassification.Text
                //                }
                //           );
                //    }
                //    BindGrid_RM(lstRM);
                //}
                #endregion
                #region Re-Consider
                if (e.CommandName == "ReConsider")
                {
                    List<object> lstRM = new List<object>();
                    foreach (GridViewRow row in gvRepairMaintenance.Rows)
                    {
                        Label ID = row.FindControl("lblID") as Label;
                        Label lblName = row.FindControl("lblName") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                        Label lblAssetName = row.FindControl("lblAssetName") as Label;
                        Label lblAssetType = row.FindControl("lblAssetType") as Label;
                        Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
                        Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
                        Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
                        Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                        Label lblVendorType = row.FindControl("lblVendorType") as Label;
                        DropDownList ddlVendorType = row.FindControl("lblVendorType") as DropDownList;
                        Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
                        if (lblAssetName.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                        {
                            OperationAssetName = lblAssetName.Text.ToString();
                        }
                        lstRM.Add
                            (
                                new
                                {
                                    ID = ID.Text,
                                    NameOfStaff = lblName.Text,
                                    Designation = lblDesignation.Text,
                                    BillNo = lblBillNo.Text,
                                    BillDate = lblBillDate.Text,
                                    AssetName = lblAssetName.Text,
                                    AssetType = lblAssetType.Text,
                                    PurchaseItems = lblPurchaseItems.Text,
                                    RepairItems = lblRepairItems.Text,
                                    TotalClaim = "",//lblGrandTotal.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    VendorType = ddlVendorType == null ? lblVendorType.Text : ddlVendorType.SelectedItem.Text,
                                    ObjectClassification = lblObjectClassification.Text
                                }
                           );
                    }
                    BindGrid_RM(lstRM);
                }
                #endregion
                #region Save
                if (e.CommandName == "Save")
                {
                    List<object> lstRM = new List<object>();
                    foreach (GridViewRow row in gvRepairMaintenance.Rows)
                    {
                        Label ID = row.FindControl("lblID") as Label;
                        Label lblName = row.FindControl("lblName") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                        Label lblAssetName = row.FindControl("lblAssetName") as Label;
                        Label lblAssetType = row.FindControl("lblAssetType") as Label;
                        Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
                        Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
                        Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
                        Label lblGrandTotal = row.FindControl("GrandTotal") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                        Label lblVendorType = row.FindControl("lblVendorType") as Label;
                        DropDownList ddlVendorType = row.FindControl("ddlVendorType") as DropDownList;
                        Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
                        lstRM.Add
                            (
                                new
                                {
                                    ID = ID.Text,
                                    NameOfStaff = lblName.Text,
                                    Designation = lblDesignation.Text,
                                    BillNo = lblBillNo.Text,
                                    BillDate = lblBillDate.Text,
                                    AssetName = lblAssetName.Text,
                                    AssetType = lblAssetType.Text,
                                    PurchaseItems = lblPurchaseItems.Text,
                                    RepairItems = lblTotalClaim.Text,
                                    TotalClaim = lblGrandTotal == null ? "" : lblGrandTotal.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    VendorType = ddlVendorType == null ? lblVendorType.Text : ddlVendorType.SelectedItem.Text,
                                    ObjectClassification = lblObjectClassification.Text
                                }
                           );
                    }
                    gvRepairMaintenance.EditIndex = -1;
                    BindGrid_RM(lstRM);
                }
                #endregion
                #region Edit Row
                //if (e.CommandName == "Edit")
                //{

                //    GridViewRow abc = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                //    Label lbl = abc.FindControl("lblVendorType") as Label;
                //    VendroType = lbl.Text;
                //}

                #endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRepairMaintenance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                List<object> lstRM = new List<object>();
                foreach (GridViewRow row in gvRepairMaintenance.Rows)
                {
                    Label ID = row.FindControl("lblID") as Label;
                    Label lblName = row.FindControl("lblName") as Label;
                    Label lblDesignation = row.FindControl("lblDesignation") as Label;
                    Label lblBillNo = row.FindControl("lblBillNo") as Label;
                    Label lblBillDate = row.FindControl("lblBillDate") as Label;
                    Label lblAssetName = row.FindControl("lblAssetName") as Label;
                    Label lblAssetType = row.FindControl("lblAssetType") as Label;
                    Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
                    Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
                    Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
                    //Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
                    Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                    Label lblVendorType = row.FindControl("lblVendorType") as Label;
                    Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
                    //DropDownList ddlDropDown = row.FindControl("ddlVendorType") as DropDownList;
                    //ddlDropDown.SelectedItem.Value = lblVendorType.Text == "Filer" ? "1" : "2";
                    lstRM.Add
                        (
                            new
                            {
                                ID = ID.Text,
                                NameOfStaff = lblName.Text,
                                Designation = lblDesignation.Text,
                                BillNo = lblBillNo.Text,
                                BillDate = lblBillDate.Text,
                                AssetName = lblAssetName.Text,
                                AssetType = lblAssetType.Text,
                                PurchaseItems = lblPurchaseItems.Text,
                                RepairItems = lblTotalClaim.Text,
                                TotalClaim = "",//lblGrandTotal.Text,
                                SanctionStatus = lblSanctionedStatus.Text,
                                VendorType = lblVendorType.Text,
                                ObjectClassification = lblObjectClassification.Text
                            }
                       );
                }
                gvRepairMaintenance.EditIndex = e.NewEditIndex;
                gvRepairMaintenance.DataSource = lstRM;
                gvRepairMaintenance.DataBind();
                DropDownList ddl = (DropDownList)gvRepairMaintenance.Rows[e.NewEditIndex].FindControl("ddlVendorType");
                ddl.SelectedValue = VendroType == "Filer" ? "1" : "2";

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRepairMaintenance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                List<object> lstRM = new List<object>();
                foreach (GridViewRow row in gvRepairMaintenance.Rows)
                {
                    Label ID = row.FindControl("lblID") as Label;
                    Label lblName = row.FindControl("lblName") as Label;
                    Label lblDesignation = row.FindControl("lblDesignation") as Label;
                    Label lblBillNo = row.FindControl("lblBillNo") as Label;
                    Label lblBillDate = row.FindControl("lblBillDate") as Label;
                    Label lblAssetName = row.FindControl("lblAssetName") as Label;
                    Label lblAssetType = row.FindControl("lblAssetType") as Label;
                    Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
                    Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
                    Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
                    Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                    Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
                    DropDownList ddlVendorType = row.FindControl("ddlVendorType") as DropDownList;
                    Label lblVendorType = row.FindControl("lblVendorType") as Label;
                    lstRM.Add
                        (
                            new
                            {
                                ID = ID.Text,
                                NameOfStaff = lblName.Text,
                                Designation = lblDesignation.Text,
                                BillNo = lblBillNo.Text,
                                BillDate = lblBillDate.Text,
                                AssetName = lblAssetName.Text,
                                AssetType = lblAssetType.Text,
                                PurchaseItems = lblPurchaseItems.Text,
                                RepairItems = lblTotalClaim.Text,
                                TotalClaim = "",
                                SanctionStatus = lblSanctionedStatus.Text,
                                VendorType = ddlVendorType == null ? lblVendorType.Text : ddlVendorType.SelectedItem.Text,
                                ObjectClassification = lblObjectClassification.Text
                            }
                       );
                }
                gvRepairMaintenance.EditIndex = -1;
                gvRepairMaintenance.DataSource = lstRM;
                gvRepairMaintenance.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRepairMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (Mode == "Verify")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblAssetName")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Sanctioned";

                        }
                    }
                    if (Mode == "Rejected")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblAssetName")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Rejected";
                        }
                    }
                    if (Mode == "ReConsider")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblAssetName")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Not Sanctioned";
                        }
                    }
                    // LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                    LinkButton btnReject = e.Row.FindControl("btnReject") as LinkButton;
                    //LinkButton btnVerify = e.Row.FindControl("btnVerify") as LinkButton;
                    LinkButton btnReConsider = e.Row.FindControl("btnReConsider") as LinkButton;
                    Label lblStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    if (lblStatus.Text.ToString().Trim() == "Rejected" || lblStatus.Text.ToString().Trim() == "Sanctioned")
                    {
                        //   btnEdit.Visible = false;
                        btnReject.Visible = false;
                        // btnVerify.Visible = false;
                        btnReConsider.Visible = true;
                    }
                    if (lblStatus.Text.ToString().Trim() == "ReConsider" || lblStatus.Text.ToString().Trim() == "Not Sanctioned")
                    {
                        //  btnEdit.Visible = true;
                        btnReject.Visible = true;
                        //btnVerify.Visible = true;
                        btnReConsider.Visible = false;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region gvPolRecipts Events
        protected void gvPolRecipts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Mode = e.CommandName;
                List<object> lstPOL = new List<object>();
                #region Rejected
                if (e.CommandName == "Rejected")
                {



                    GridViewRow rejectedRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ViewState["RejectedRowIndex"] = rejectedRow.RowIndex;
                    txtRejectionReason.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#RejectionPopUp').modal();", true);
                    txtRejectionReason.Attributes.Add("required", "required");
                    txtRejectionReason.CssClass = "required form-control";
                    ViewState["RejectedAssetName"] = e.CommandArgument;
                }
                #endregion
                #region Verify
                if (e.CommandName == "Verify")
                {
                    foreach (GridViewRow row in gvPolRecipts.Rows)
                    {

                        Label ID = row.FindControl("lblID") as Label;
                        Label lblName = row.FindControl("lblName") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblMeterReading = row.FindControl("lblMeterReading") as Label;
                        Label lblPOLReceiptNo = row.FindControl("lblPOLReceiptNo") as Label;
                        Label lblPOLDate = row.FindControl("lblPOLDate") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                        Label lblAmountRs = row.FindControl("lblAmountRs") as Label;
                        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                        {
                            OperationAssetName = lblBillNo.Text.ToString();
                        }
                        lstPOL.Add
                            (
                                new
                                {
                                    ID = ID.Text,
                                    NameOfStaff = lblName.Text,
                                    Designation = lblDesignation.Text,
                                    BillNo = lblBillNo.Text,
                                    MeterReading = lblMeterReading.Text,
                                    POLReceiptNo = lblPOLReceiptNo.Text,
                                    POLDatetime = lblPOLDate.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    AmountRs = lblAmountRs.Text,
                                }
                           );
                    }
                    BindGrid_POL(lstPOL);
                }
                #endregion
                #region Re-Consider
                if (e.CommandName == "ReConsider")
                {

                    foreach (GridViewRow row in gvPolRecipts.Rows)
                    {
                        Label ID = row.FindControl("lblID") as Label;
                        Label lblName = row.FindControl("lblName") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblMeterReading = row.FindControl("lblMeterReading") as Label;
                        Label lblPOLReceiptNo = row.FindControl("lblPOLReceiptNo") as Label;
                        Label lblPOLDate = row.FindControl("lblPOLDate") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                        Label lblAmountRs = row.FindControl("lblAmountRs") as Label;
                        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                        {
                            OperationAssetName = lblBillNo.Text.ToString();
                        }
                        lstPOL.Add
                            (
                                new
                                {
                                    ID = ID.Text,
                                    NameOfStaff = lblName.Text,
                                    Designation = lblDesignation.Text,
                                    BillNo = lblBillNo.Text,
                                    MeterReading = lblMeterReading.Text,
                                    POLReceiptNo = lblPOLReceiptNo.Text,
                                    POLDatetime = lblPOLDate.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    AmountRs = lblAmountRs.Text,
                                }
                           );
                    }
                    BindGrid_POL(lstPOL);
                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }


        protected void gvPolRecipts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Mode == "Verify")
                    {
                        string Name = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (Name.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Sanctioned";

                        }
                    }
                    if (Mode == "Rejected")
                    {
                        long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                        if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblName")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                    }
                    if (Mode == "ReConsider")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Not Sanctioned";
                        }
                    }
                    LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                    LinkButton btnReject = e.Row.FindControl("btnReject") as LinkButton;
                    //LinkButton btnVerify = e.Row.FindControl("btnVerify") as LinkButton;
                    LinkButton btnReConsider = e.Row.FindControl("btnReConsider") as LinkButton;
                    Label lblStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    if (lblStatus.Text.ToString().Trim() == "Rejected" || lblStatus.Text.ToString().Trim() == "Sanctioned")
                    {
                        btnEdit.Visible = false;
                        btnReject.Visible = false;
                        //btnVerify.Visible = false;
                        btnReConsider.Visible = true;
                    }
                    if (lblStatus.Text.ToString().Trim() == "ReConsider" || lblStatus.Text.ToString().Trim() == "Not Sanctioned")
                    {
                        btnEdit.Visible = true;
                        btnReject.Visible = true;
                        //btnVerify.Visible = true;
                        btnReConsider.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region gvTADA Events


        protected void gvTADA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Rejected
                Mode = e.CommandName;
                List<object> lstTADA = new List<object>();
                if (e.CommandName == "Rejected")
                {
                    GridViewRow rejectedRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ViewState["RejectedRowIndex"] = rejectedRow.RowIndex;
                    txtRejectionReason.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#RejectionPopUp').modal();", true);
                    txtRejectionReason.Attributes.Add("required", "required");
                    txtRejectionReason.CssClass = "required form-control";
                    ViewState["RejectedAssetName"] = e.CommandArgument;
                }
                #endregion
                #region Verify
                if (e.CommandName == "Verify")
                {
                    foreach (GridViewRow row in gvTADA.Rows)
                    {

                        Label ID = row.FindControl("lblID") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblNameOfStaff = row.FindControl("lblNameOfStaff") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblHalfDailiesOrdinary = row.FindControl("lblHalfDailiesOrdinary") as Label;
                        Label lblFullDailiesOrdinary = row.FindControl("lblFullDailiesOrdinary") as Label;
                        Label lblFullDailiesSpecial = row.FindControl("lblFullDailiesSpecial") as Label;
                        Label lblHalfDailiesSpecial = row.FindControl("lblHalfDailiesSpecial") as Label;
                        Label lblTotalKMPublicTransport = row.FindControl("lblTotalKMPublicTransport") as Label;
                        Label lblTotalKMIrrigationVehicle = row.FindControl("lblTotalKMIrrigationVehicle") as Label;
                        Label lblMiscExpenses = row.FindControl("lblMiscExpenses") as Label;
                        Label lblTotalTADA = row.FindControl("lblTotalTADA") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;

                        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                        {
                            OperationAssetName = lblBillNo.Text.ToString();
                        }
                        lstTADA.Add
                            (
                                new
                                {
                                    ID = ID.Text,

                                    Designation = lblDesignation.Text,
                                    NameOfStaff = lblNameOfStaff.Text,
                                    BillNo = lblBillNo.Text,
                                    HalfDailiesOrdinary = lblHalfDailiesOrdinary.Text,
                                    FullDailiesOrdinary = lblFullDailiesOrdinary.Text,
                                    FullDailiesSpecial = lblFullDailiesSpecial.Text,
                                    HalfDailiesSpecial = lblHalfDailiesSpecial.Text,
                                    TotalKMPublicTransport = lblTotalKMPublicTransport.Text,
                                    TotalKMIrrigationVehicle = lblTotalKMIrrigationVehicle.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    MiscExpenses = lblMiscExpenses.Text,
                                    TotalTADA = lblTotalTADA.Text,
                                }
                           );
                    }
                    BindGrid_TADA(lstTADA);
                }
                #endregion
                #region Re-Consider
                if (e.CommandName == "ReConsider")
                {

                    foreach (GridViewRow row in gvTADA.Rows)
                    {
                        Label ID = row.FindControl("lblID") as Label;
                        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                        Label lblNameOfStaff = row.FindControl("lblNameOfStaff") as Label;
                        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                        Label lblHalfDailiesOrdinary = row.FindControl("lblHalfDailiesOrdinary") as Label;
                        Label lblFullDailiesOrdinary = row.FindControl("lblFullDailiesOrdinary") as Label;
                        Label lblFullDailiesSpecial = row.FindControl("lblFullDailiesSpecial") as Label;
                        Label lblHalfDailiesSpecial = row.FindControl("lblHalfDailiesSpecial") as Label;
                        Label lblTotalKMPublicTransport = row.FindControl("lblTotalKMPublicTransport") as Label;
                        Label lblTotalKMIrrigationVehicle = row.FindControl("lblTotalKMIrrigationVehicle") as Label;
                        Label lblMiscExpenses = row.FindControl("lblMiscExpenses") as Label;
                        Label lblTotalTADA = row.FindControl("lblTotalTADA") as Label;
                        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;

                        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                        {
                            OperationAssetName = lblBillNo.Text.ToString();
                        }
                        lstTADA.Add
                            (
                                new
                                {
                                    ID = ID.Text,

                                    Designation = lblDesignation.Text,
                                    NameOfStaff = lblNameOfStaff.Text,
                                    BillNo = lblBillNo.Text,
                                    HalfDailiesOrdinary = lblHalfDailiesOrdinary.Text,
                                    FullDailiesOrdinary = lblFullDailiesOrdinary.Text,
                                    FullDailiesSpecial = lblFullDailiesSpecial.Text,
                                    HalfDailiesSpecial = lblHalfDailiesSpecial.Text,
                                    TotalKMPublicTransport = lblTotalKMPublicTransport.Text,
                                    TotalKMIrrigationVehicle = lblTotalKMIrrigationVehicle.Text,
                                    SanctionStatus = lblSanctionedStatus.Text,
                                    MiscExpenses = lblMiscExpenses.Text,
                                    TotalTADA = lblTotalTADA.Text,
                                }
                           );
                    }
                    BindGrid_TADA(lstTADA);
                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvTADA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Mode == "Verify")
                    {
                        string Name = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (Name.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Sanctioned";

                        }
                    }
                    if (Mode == "Rejected")
                    {
                        long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                        if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblName")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                    }
                    if (Mode == "ReConsider")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Not Sanctioned";
                        }
                    }
                    LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                    LinkButton btnReject = e.Row.FindControl("btnReject") as LinkButton;
                    LinkButton btnVerify = e.Row.FindControl("btnVerify") as LinkButton;
                    LinkButton btnReConsider = e.Row.FindControl("btnReConsider") as LinkButton;
                    Label lblStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    if (lblStatus.Text.ToString().Trim() == "Rejected" || lblStatus.Text.ToString().Trim() == "Sanctioned")
                    {
                        btnEdit.Visible = false;
                        btnReject.Visible = false;
                        btnVerify.Visible = false;
                        btnReConsider.Visible = true;
                    }
                    if (lblStatus.Text.ToString().Trim() == "ReConsider" || lblStatus.Text.ToString().Trim() == "Not Sanctioned")
                    {
                        btnEdit.Visible = true;
                        btnReject.Visible = true;
                        btnVerify.Visible = true;
                        btnReConsider.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region gvNewPurchas Events


        protected void gvNewPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Rejected
                Mode = e.CommandName;
                List<object> lstNewPurchase = new List<object>();
                if (e.CommandName == "Rejected")
                {
                    GridViewRow rejectedRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ViewState["RejectedRowIndex"] = rejectedRow.RowIndex;
                    txtRejectionReason.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#RejectionPopUp').modal();", true);
                    txtRejectionReason.Attributes.Add("required", "required");
                    txtRejectionReason.CssClass = "required form-control";
                    ViewState["RejectedAssetName"] = e.CommandArgument;
                }
                #endregion
                #region Commented Code
                //#region Verify
                //if (e.CommandName == "Verify")
                //{
                //    foreach (GridViewRow row in gvNewPurchase.Rows)
                //    {

                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblNameOfStaff = row.FindControl("lblNameOfStaff") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblPurchasedItemName = row.FindControl("lblPurchasedItemName") as Label;
                //        Label lblPurchaseAmount = row.FindControl("lblPurchaseAmount") as Label;
                //        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;

                //        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                //        {
                //            OperationAssetName = lblBillNo.Text.ToString();
                //        }
                //        lstNewPurchase.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    Designation = lblDesignation.Text,
                //                    NameOfStaff = lblNameOfStaff.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    PurchaseItemName = lblPurchasedItemName.Text,
                //                    PurchaseItemAmount = lblPurchaseAmount.Text,
                //                    SanctionStatus = lblSanctionedStatus.Text,
                //                }
                //           );
                //    }
                //    BindGrid_NP(lstNewPurchase);
                //}
                //#endregion
                //#region Re-Consider
                //if (e.CommandName == "ReConsider")
                //{

                //    foreach (GridViewRow row in gvNewPurchase.Rows)
                //    {

                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblNameOfStaff = row.FindControl("lblNameOfStaff") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblPurchasedItemName = row.FindControl("lblPurchasedItemName") as Label;
                //        Label lblPurchaseAmount = row.FindControl("lblPurchaseAmount") as Label;
                //        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;

                //        if (lblBillNo.Text.ToString().ToUpper().Trim() == e.CommandArgument.ToString().Trim().ToUpper())
                //        {
                //            OperationAssetName = lblBillNo.Text.ToString();
                //        }
                //        lstNewPurchase.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    Designation = lblDesignation.Text,
                //                    NameOfStaff = lblNameOfStaff.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    PurchaseItemName = lblPurchasedItemName.Text,
                //                    PurchaseItemAmount = lblPurchaseAmount.Text,
                //                    SanctionStatus = lblSanctionedStatus.Text,
                //                }
                //           );
                //    }
                //    BindGrid_NP(lstNewPurchase);
                //}
                //#endregion
                #endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvNewPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Mode == "Verify")
                    {
                        string Name = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (Name.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Sanctioned";

                        }
                    }
                    if (Mode == "Rejected")
                    {
                        long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                        if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblName")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                        if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
                        {
                            string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                            if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                            {
                                Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                                lblSanctionedStatus.Text = "Rejected";
                            }
                        }
                    }
                    if (Mode == "ReConsider")
                    {
                        string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                        {
                            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                            lblSanctionedStatus.Text = "Not Sanctioned";
                        }
                    }
                    //  LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                    LinkButton btnReject = e.Row.FindControl("btnReject") as LinkButton;
                    //LinkButton btnVerify = e.Row.FindControl("btnVerify") as LinkButton;
                    LinkButton btnReConsider = e.Row.FindControl("btnReConsider") as LinkButton;
                    Label lblStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    if (lblStatus.Text.ToString().Trim() == "Rejected" || lblStatus.Text.ToString().Trim() == "Sanctioned")
                    {
                        // btnEdit.Visible = false;
                        btnReject.Visible = false;
                        // btnVerify.Visible = false;
                        btnReConsider.Visible = true;
                    }
                    if (lblStatus.Text.ToString().Trim() == "ReConsider" || lblStatus.Text.ToString().Trim() == "Not Sanctioned")
                    {
                        //btnEdit.Visible = true;
                        btnReject.Visible = true;
                        // btnVerify.Visible = true;
                        btnReConsider.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Save and Reject Sanction
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#PrePairSanctionedPopUp').modal();", true);
                ddlObjectClassification.Attributes.Add("required", "ture");
                ddlSanctionedMonth.CssClass = "required form-control";
                ddlSanctionedMonth.Attributes.Add("required", "ture");
                txtRejectionReason.Attributes.Remove("required");

                BindMonthDropdown(ddlSanctionedMonth);

                List<AT_ObjectClassification> exsanctioned = bll.GetAllObjectClassifications();
                List<object> lst = (from item in exsanctioned where item.ID > 0 select new { ID = item.ID, Name = item.AccountsCode + "  (" + item.ObjectClassification + ")" }).ToList<object>();
                Dropdownlist.BindDropdownlist<List<object>>(ddlObjectClassification, lst, (int)Constants.DropDownFirstOption.NoOption);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void btnRejectYes_Click(object sender, EventArgs e)
        {
            try
            {
                #region Intialization of variables
                Mode = "Rejected";
                int rowIndex = Convert.ToInt32(ViewState["RejectedRowIndex"]);
                ViewState["RejectionReason"] = txtRejectionReason.Text;
                long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                GridView grid = (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance ? gvRepairMaintenance : ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts ? gvPolRecipts : ExpenseTypeID == (long)Constants.ExpenseType.TADA ? gvTADA : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense ? gvOtherExpense : null);
                List<object> lstRM = new List<object>();
                #endregion
                #region
                GridViewRow row = grid.Rows[rowIndex];
                Label lblRejectionReason = row.FindControl("lblRejenctionReason") as Label;
                Label lblSnctioned = (Label)row.FindControl("lblSanctionedStatus");
                LinkButton btnReject = (LinkButton)row.FindControl("btnReject");
                CheckBox chkVerify = row.FindControl("chkVerify") as CheckBox;
                lblRejectionReason.Text = txtRejectionReason.Text;
                chkVerify.Enabled = false;
                btnReject.Enabled = false;
                btnReject.CssClass += " disabled";
                lblSnctioned.Text = "Rejected";
                btnSave.Enabled = true;
                btnSave.CssClass = "btn btn-primary";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSaveSanctioned_Click(object sender, EventArgs e)
        {
            try
            {

                #region Is Sancioned Amounmt  is valid
                string FinancialYear = Utility.GetCurrentFinancialYear();
                double SanctionedAmount = Convert.ToDouble(Utility.RemoveComma(lblTotalsanctionAmount.Text));
                long ObjectClassificationID = Convert.ToInt64(ddlObjectClassification.SelectedValue);
                bool IsSanctionedValid = bll.IsSanctionedAmountExceed(FinancialYear, SanctionedAmount, ObjectClassificationID);
                if (!IsSanctionedValid)
                {
                    string message = "Sanctioned amount is exceeding funds release for this financial year.";
                    Master.ShowMessage(message, SiteMaster.MessageType.Error);
                    return;
                }


                #endregion

                #region Intialization of variables
                List<AT_Sanction> lstRM = new List<AT_Sanction>();
                long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                bool isRecordSaved = false;
                string SanctionType = ddlSanctionType.SelectedValue == "1" ? "RM" : ddlSanctionType.SelectedValue == "2" ? "POL" : ddlSanctionType.SelectedValue == "3" ? "TA" : ddlSanctionType.SelectedValue == "4" ? "NP" : ddlSanctionType.SelectedValue == "5" ? "OE" : "";
                List<AT_ExpenseSanction> exsec = new List<AT_ExpenseSanction>();
                GridView grid = (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance ? gvRepairMaintenance : ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts ? gvPolRecipts : ExpenseTypeID == (long)Constants.ExpenseType.TADA ? gvTADA : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense ? gvOtherExpense : null);
                foreach (GridViewRow row in grid.Rows)
                {
                    Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
                    Label lblID = row.FindControl("lblID") as Label;
                    DropDownList ddlVendorType = row.FindControl("ddlVendorType") as DropDownList;
                    Label lblAssetTypeID = row.FindControl("lblAssetTypeID") as Label;
                    if (lblSanctionedStatus.Text.ToUpper() == "REJECTED" || lblSanctionedStatus.Text.ToUpper() == "SANCTIONED")
                    {
                        AT_ExpenseSanction es = new AT_ExpenseSanction();
                        es.AT_MonthlyExpenses = new AT_MonthlyExpenses();
                        es.AT_MonthlyExpenses.ID = Convert.ToInt64(lblID.Text);
                        es.MonthlyExpenseID = Convert.ToInt64(lblID.Text);
                        es.AT_MonthlyExpenses.ReasonOfRejection = lblSanctionedStatus.Text.Trim().ToUpper() == "REJECTED" ? Convert.ToString(ViewState["RejectionReason"]) : "";
                        if (ddlVendorType != null && !string.IsNullOrEmpty(ddlVendorType.SelectedValue))
                        {
                            es.VendorType = ddlVendorType.SelectedItem.Text;
                        }
                        es.Status = lblSanctionedStatus.Text.ToUpper() == "REJECTED" ? "4" : "2";
                        es.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                        es.CreatedDate = DateTime.Now;
                        es.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                        es.ModifiedDate = DateTime.Now;
                        exsec.Add(es);
                    }
                }
                AT_Sanction obj = new AT_Sanction();
                //if (Convert.ToInt64(ddlSanctionOn.SelectedValue == "" ? "0" : ddlSanctionOn.SelectedValue) > 0)
                if (ddlSanctionOn.SelectedValue != "")
                {
                    obj.AssetTypeID = Convert.ToInt64(ddlSanctionOn.SelectedValue);
                }
                if (!string.IsNullOrEmpty(lblTotalsanctionAmount.Text))
                {
                    obj.SanctionAmount = Convert.ToDouble(Utility.RemoveComma(lblTotalsanctionAmount.Text));
                }
                obj.ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedValue);
                obj.FinancialYear = ddlFinancialYear.SelectedItem.Text;
                obj.SanctionStatusID = (long)Constants.SanctionStatus.Sanctioned;// 2;
                obj.Month = ddlSanctionedMonth.SelectedItem.Text;
                obj.ObjectClassificationID = Convert.ToInt64(ddlObjectClassification.SelectedValue);
                obj.IsActive = true;
                obj.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                obj.ModifiedDate = DateTime.Now;
                obj.SanctionNo = GenerateBillNo();
                lstRM.Add(obj);
                isRecordSaved = bll.SaveSanctioned(lstRM, exsec);
                #endregion


                #region Save Reacord
                if (isRecordSaved)
                {


                    grid.Visible = false;
                    btnSave.Visible = false;
                    pnlTotalClaim.Visible = false;
                    ResetControls();

                    string SaveMessage = "A new Sanction containing verified bill(s) has been prepared with Sanction No  " + "<b>" + obj.SanctionNo + "</b>";
                    lblRejectionReason.Text = SaveMessage;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#SanctionedCreate').modal();", true);

                }
                else
                {
                }




                #endregion

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
                #endregion
        #region commented Code
        //#region POL Receipts
        //if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
        //{
        //    GridViewRow row = gvPolRecipts.Rows[rowIndex];
        //    Label lblRejectionReason = row.FindControl("lblRejenctionReason") as Label;
        //    Label lblSnctioned = (Label)row.FindControl("lblSanctionedStatus");
        //    LinkButton btnReject = (LinkButton)row.FindControl("btnReject");
        //    CheckBox chkVerify = row.FindControl("chkVerify") as CheckBox;
        //    lblRejectionReason.Text = txtRejectionReason.Text;
        //    chkVerify.Enabled = false;
        //    btnReject.Enabled = false;
        //    btnReject.CssClass += "disabled";
        //    lblSnctioned.Text = "Rejected";
        //}
        //#endregion
        //#region TADA
        //if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
        //{
        //    GridViewRow row = gvTADA.Rows[rowIndex];
        //    Label lblRejectionReason = row.FindControl("lblRejenctionReason") as Label;
        //    Label lblSnctioned = (Label)row.FindControl("lblSanctionedStatus");
        //    LinkButton btnReject = (LinkButton)row.FindControl("btnReject");
        //    CheckBox chkVerify = row.FindControl("chkVerify") as CheckBox;
        //    lblRejectionReason.Text = txtRejectionReason.Text;
        //    chkVerify.Enabled = false;
        //    btnReject.Enabled = false;
        //    btnReject.CssClass += "disabled";
        //    lblSnctioned.Text = "Rejected";
        //}
        //#endregion
        //#region New Purchase
        //if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
        //{
        //    foreach (GridViewRow row in gvNewPurchase.Rows)
        //    {

        //        Label ID = row.FindControl("lblID") as Label;
        //        Label lblNameOfStaff = row.FindControl("lblNameOfStaff") as Label;
        //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
        //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
        //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
        //        Label lblPurchasedItemName = row.FindControl("lblPurchasedItemName") as Label;
        //        Label lblPurchaseAmount = row.FindControl("lblPurchaseAmount") as Label;
        //        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;

        //        if (lblBillNo.Text.ToString().ToUpper().Trim() == ViewState["RejectedAssetName"].ToString().Trim().ToUpper())
        //        {
        //            OperationAssetName = lblBillNo.Text.ToString();
        //        }
        //        lstRM.Add
        //            (
        //                new
        //                {
        //                    ID = ID.Text,
        //                    Designation = lblDesignation.Text,
        //                    NameOfStaff = lblNameOfStaff.Text,
        //                    BillNo = lblBillNo.Text,
        //                    BillDate = lblBillDate.Text,
        //                    PurchaseItemName = lblPurchasedItemName.Text,
        //                    PurchaseItemAmount = lblPurchaseAmount.Text,
        //                    SanctionStatus = lblSanctionedStatus.Text,
        //                }
        //           );
        //    }
        //    BindGrid_NP(lstRM);
        //}
        //#endregion
        //#region Other Expense
        //if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
        //{
        //    foreach (GridViewRow row in gvRepairMaintenance.Rows)
        //    {
        //        Label ID = row.FindControl("lblID") as Label;
        //        Label lblName = row.FindControl("lblName") as Label;
        //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
        //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
        //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
        //        Label lblAssetName = row.FindControl("lblAssetName") as Label;
        //        Label lblAssetType = row.FindControl("lblAssetType") as Label;
        //        Label lblPurchaseItems = row.FindControl("lblPurchaseItems") as Label;
        //        Label lblRepairItems = row.FindControl("lblRepairItems") as Label;
        //        Label lblTotalClaim = row.FindControl("lblTotalClaim") as Label;
        //        Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
        //        Label lblSanctionedStatus = row.FindControl("lblSanctionedStatus") as Label;
        //        Label lblVendorType = row.FindControl("lblVendorType") as Label;
        //        Label lblObjectClassification = row.FindControl("lblObjectClassification") as Label;
        //        if (lblAssetName.Text.ToString().ToUpper().Trim() == ViewState["RejectedAssetName"].ToString().Trim().ToUpper())
        //        {
        //            OperationAssetName = lblAssetName.Text.ToString();
        //        }
        //        lstRM.Add
        //            (
        //                new
        //                {
        //                    ID = ID.Text,
        //                    NameOfStaff = lblName.Text,
        //                    Designation = lblDesignation.Text,
        //                    BillNo = lblBillNo.Text,
        //                    BillDate = lblBillDate.Text,
        //                    AssetName = lblAssetName.Text,
        //                    AssetType = lblAssetType.Text,
        //                    PurchaseItems = lblPurchaseItems.Text,
        //                    RepairItems = lblRepairItems.Text,
        //                    TotalClaim = "",//lblGrandTotal.Text,
        //                    SanctionStatus = lblSanctionedStatus.Text,
        //                    VendorType = lblVendorType.Text,
        //                    ObjectClassification = lblObjectClassification.Text
        //                }
        //           );
        //    }
        //    BindGrid_RM(lstRM);

        //}
        //#endregion}
        #endregion

        #endregion
        #region gvOtherExpense Events



        protected void gvOtherExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (Mode == "Verify")
                    //{
                    //    string Name = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                    //    if (Name.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                    //    {
                    //        Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    //        lblSanctionedStatus.Text = "Sanctioned";

                    //    }
                    //}
                    //if (Mode == "Rejected")
                    //{
                    //    long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                    //    if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                    //    {
                    //        string AssetName = ((Label)e.Row.FindControl("lblName")).Text.ToString();
                    //        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                    //        {
                    //            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    //            lblSanctionedStatus.Text = "Rejected";
                    //        }
                    //    }
                    //    if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                    //    {
                    //        string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                    //        if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                    //        {
                    //            Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    //            lblSanctionedStatus.Text = "Rejected";
                    //        }
                    //    }
                    //}
                    //if (Mode == "ReConsider")
                    //{
                    //    string AssetName = ((Label)e.Row.FindControl("lblBillNo")).Text.ToString();
                    //    if (AssetName.ToUpper().Trim() == OperationAssetName.ToUpper().Trim())
                    //    {
                    //        Label lblSanctionedStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    //        lblSanctionedStatus.Text = "Not Sanctioned";
                    //    }
                    //}
                    LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                    LinkButton btnReject = e.Row.FindControl("btnReject") as LinkButton;
                    LinkButton btnVerify = e.Row.FindControl("btnVerify") as LinkButton;
                    LinkButton btnReConsider = e.Row.FindControl("btnReConsider") as LinkButton;
                    Label lblStatus = e.Row.FindControl("lblSanctionedStatus") as Label;
                    Label lblBillDate = e.Row.FindControl("lblBillDate") as Label;
                    lblBillDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblBillDate.Text));
                    if (lblStatus.Text.ToString().Trim() == "Rejected" || lblStatus.Text.ToString().Trim() == "Sanctioned")
                    {
                        btnEdit.Visible = false;
                        btnReject.Visible = false;
                        btnVerify.Visible = false;
                        btnReConsider.Visible = true;
                    }
                    if (lblStatus.Text.ToString().Trim() == "ReConsider" || lblStatus.Text.ToString().Trim() == "Not Sanctioned")
                    {
                        btnEdit.Visible = true;
                        btnReject.Visible = true;
                        btnVerify.Visible = true;
                        btnReConsider.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOtherExpense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Mode = e.CommandName;
                #region Rejected
                if (e.CommandName == "Rejected")
                {
                    txtRejectionReason.Text = string.Empty;
                    GridViewRow rejectedRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ViewState["RejectedRowIndex"] = rejectedRow.RowIndex;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ChangeStatus", "$('#RejectionPopUp').modal();", true);
                    txtRejectionReason.Attributes.Add("required", "required");
                    txtRejectionReason.CssClass = "required form-control";
                    ViewState["RejectedAssetName"] = e.CommandArgument;
                }
                #endregion
                //#region Verify
                //if (e.CommandName == "Verify")
                //{

                //    List<object> lstOE = new List<object>();

                //    foreach (GridViewRow row in gvOtherExpense.Rows)
                //    {

                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblName = row.FindControl("lblName") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblExpenseName = row.FindControl("lblExpenseName") as Label;
                //        Label lblAmountRs = row.FindControl("lblAmountRs") as Label;

                //        Button btnEdit = row.FindControl("btnEdit") as Button;
                //        Button btnReject = row.FindControl("btnReject") as Button;
                //        Button btnVerify = row.FindControl("btnVerify") as Button;
                //        Button btnReConsider = row.FindControl("btnReConsider") as Button;

                //        lstOE.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    NameOfStaff = lblName.Text,
                //                    Designation = lblDesignation.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    ExpenseName = lblExpenseName,
                //                    AmountRs = lblAmountRs
                //                }
                //           );
                //    }
                //    BindGrid_OE(lstOE);
                //}
                //#endregion
                //#region Re-Consider
                //if (e.CommandName == "ReConsider")
                //{
                //    List<object> lstOE = new List<object>();
                //    foreach (GridViewRow row in gvOtherExpense.Rows)
                //    {
                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblName = row.FindControl("lblName") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblExpenseName = row.FindControl("lblExpenseName") as Label;
                //        Label lblAmountRs = row.FindControl("lblAmountRs") as Label;

                //        Button btnEdit = row.FindControl("btnEdit") as Button;
                //        Button btnReject = row.FindControl("btnReject") as Button;
                //        Button btnVerify = row.FindControl("btnVerify") as Button;
                //        Button btnReConsider = row.FindControl("btnReConsider") as Button;
                //        lstOE.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    NameOfStaff = lblName.Text,
                //                    Designation = lblDesignation.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    ExpenseName = lblExpenseName,
                //                    AmountRs = lblAmountRs
                //                }
                //           );
                //    }
                //    BindGrid_OE(lstOE);
                //}
                //#endregion
                //#region Save
                //if (e.CommandName == "Save")
                //{
                //    List<object> lstOE = new List<object>();
                //    foreach (GridViewRow row in gvOtherExpense.Rows)
                //    {
                //        Label ID = row.FindControl("lblID") as Label;
                //        Label lblName = row.FindControl("lblName") as Label;
                //        Label lblDesignation = row.FindControl("lblDesignation") as Label;
                //        Label lblBillNo = row.FindControl("lblBillNo") as Label;
                //        Label lblBillDate = row.FindControl("lblBillDate") as Label;
                //        Label lblExpenseName = row.FindControl("lblExpenseName") as Label;
                //        Label lblAmountRs = row.FindControl("lblAmountRs") as Label;

                //        Button btnEdit = row.FindControl("btnEdit") as Button;
                //        Button btnReject = row.FindControl("btnReject") as Button;
                //        Button btnVerify = row.FindControl("btnVerify") as Button;
                //        Button btnReConsider = row.FindControl("btnReConsider") as Button;
                //        lstOE.Add
                //            (
                //                new
                //                {
                //                    ID = ID.Text,
                //                    NameOfStaff = lblName.Text,
                //                    Designation = lblDesignation.Text,
                //                    BillNo = lblBillNo.Text,
                //                    BillDate = lblBillDate.Text,
                //                    ExpenseName = lblExpenseName,
                //                    AmountRs = lblAmountRs
                //                }
                //           );
                //    }
                //    gvOtherExpense.EditIndex = -1;
                //    BindGrid_OE(lstOE);
                //}
                //#endregion
                //#region Edit Row
                //if (e.CommandName == "Edit")
                //{

                //    GridViewRow abc = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                //    Label lbl = abc.FindControl("lblVendorType") as Label;
                //    VendroType = lbl.Text;
                //}

                //#endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Miscellaneous
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                txtRejectionReason.Attributes.Remove("required");
                double amount = 0;
                long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                GridView grid = (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance ? gvRepairMaintenance : ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts ? gvPolRecipts : ExpenseTypeID == (long)Constants.ExpenseType.TADA ? gvTADA : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense ? gvOtherExpense : null);
                foreach (GridViewRow row in grid.Rows)
                {
                    LinkButton btn = (LinkButton)row.FindControl("btnReject");
                    Label lblSnctioned = (Label)row.FindControl("lblSanctionedStatus");
                    if (chk.Checked)
                    {
                        if (lblSnctioned.Text != "Rejected")
                        {
                            CheckBox chkVerify = row.FindControl("chkVerify") as CheckBox;
                            Label lblamount = row.FindControl("lblTotalClaim") as Label;
                            double claimAmount = Convert.ToDouble(Utility.RemoveComma(lblamount.Text));
                            amount = amount + claimAmount;
                            chkVerify.Checked = true;
                            lblSnctioned.Text = "Sanctioned";
                            btn.Enabled = false;
                            btn.CssClass += " disabled";
                        }

                    }
                    else
                    {
                        if (lblSnctioned.Text != "Rejected")
                        {
                            CheckBox chkVerify = row.FindControl("chkVerify") as CheckBox;
                            chkVerify.Checked = false;
                            btn.Enabled = true;
                            btn.CssClass = "btn btn-danger btn_32 reject";
                            lblSnctioned.Text = "Not Sanctioned";
                        }

                    }
                }
                if (chk.Checked || (chk.Enabled == false))
                {
                    lblTotalsanctionAmount.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(amount));
                    btnSave.Enabled = true;
                    btnSave.CssClass = "btn btn-primary";
                }
                else
                {
                    lblTotalsanctionAmount.Text = string.Empty;
                    btnSave.Enabled = false;
                    btnSave.CssClass += " disabled";
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void chkVerify_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                GridViewRow row = chk.NamingContainer as GridViewRow;
                Label lblSnctioned = (Label)row.FindControl("lblSanctionedStatus");
                LinkButton btn = (LinkButton)row.FindControl("btnReject");
                Label lblamount = row.FindControl("lblTotalClaim") as Label;
                txtRejectionReason.Attributes.Remove("required");
                btn.Enabled = !chk.Checked;

                if (chk.Checked)
                {
                    double amount = Convert.ToDouble(Utility.RemoveComma(lblamount.Text)) + Convert.ToDouble(Utility.RemoveComma(lblTotalsanctionAmount.Text == "" ? "0" : lblTotalsanctionAmount.Text));
                    lblTotalsanctionAmount.Text = PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(amount));
                    lblSnctioned.Text = "Sanctioned";
                    btn.CssClass += " disabled";
                }
                else
                {
                    long amount = Convert.ToInt64(Utility.RemoveComma(lblTotalsanctionAmount.Text)) - Convert.ToInt64(Utility.RemoveComma(lblamount.Text));
                    lblTotalsanctionAmount.Text = amount.ToString() == "0" ? "" : amount.ToString();
                    lblSnctioned.Text = "Not Sanctioned";
                    btn.CssClass = "btn btn-danger btn_32 reject";
                }


                #region
                long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
                GridView grid = (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance ? gvRepairMaintenance : ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts ? gvPolRecipts : ExpenseTypeID == (long)Constants.ExpenseType.TADA ? gvTADA : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase ? gvNewPurchase : ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense ? gvOtherExpense : null);
                bool flag = false;

                foreach (GridViewRow rows in grid.Rows)
                {
                    CheckBox CheckBox = (CheckBox)rows.FindControl("chkVerify");
                    if (CheckBox.Checked || (CheckBox.Enabled == false))
                    {
                        flag = true;

                        break;
                    }
                    else
                    {
                        flag = false;


                    }
                }
                if (flag)
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "btn btn-primary";
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass += " disabled";
                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void ResetControls()
        {
            lblTotalsanctionAmount.Text = string.Empty;
            BindDll();
        }
        private string GenerateBillNo()
        {
            long ExpenseTypeID = Convert.ToInt64(ddlSanctionType.SelectedValue);

            int Month = DateTime.ParseExact(ddlSanctionedMonth.SelectedValue, "MMMM", CultureInfo.InvariantCulture).Month;
            string[] years = ddlFinancialYear.SelectedValue.Split('-');

            int Year = Convert.ToInt32(years[0]);

            if (Month < 7)
            {
                Year = Year + 1;
            }

            DateTime BillDate = new DateTime(Year, Month, 1);

            string Prefix = string.Format("San.{0}{1}-", BillDate.ToString("yy"), BillDate.Month.ToString("00"));
            string BillNo = string.Empty;

            if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
            {
                BillNo = string.Format("{0}{1}-", Prefix, RepairMaintainence);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
            {
                BillNo = string.Format("{0}{1}-", Prefix, POLReceipts);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
            {
                BillNo = string.Format("{0}{1}-", Prefix, TADA);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
            {
                BillNo = string.Format("{0}{1}-", Prefix, NewPurchase);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
            {
                BillNo = string.Format("{0}{1}-", Prefix, OtherExpense);
            }

            string OldBillNo = new AccountsBLL().GetSanctionNumber(BillNo);

            int Index = 1;

            if (OldBillNo == null)
            {
                BillNo = string.Format("{0}{1}", BillNo, Index.ToString("000"));
            }
            else
            {
                string[] parts = OldBillNo.Split('-');
                Index = Convert.ToInt32(parts[2]);
                Index++;
                BillNo = string.Format("{0}{1}", BillNo, Index.ToString("000"));
            }

            return BillNo;
        }
        public void HideAllPanels()
        {
            GridRepairMaintenance.Visible = false;
            GridPolRecipts.Visible = false;
            GridTADA.Visible = false;
            GridNewPurchase.Visible = false;
            pnlTotalClaim.Visible = false;
            GridOtherExpense.Visible = false;
            btnSave.Visible = false;

        }
        #endregion




    }
}