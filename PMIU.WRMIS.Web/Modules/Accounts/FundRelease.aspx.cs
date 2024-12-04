using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class FundRelease : BasePage
    {
        #region View State Constants

        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";

        #endregion

        #region Hash Table keys

        public const string FinancialYearKey = "FinancialYear";
        public const string PageIndexKey = "PageIndex";

        #endregion

        AccountsBLL bllAccounts = new AccountsBLL();
        List<dynamic> lstFundRelease = new List<dynamic>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();


                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.FundRelease] != null)
                            {
                                BindFinancialYearDropdown(ShowHistory);
                                BindHistoryData();
                                return;
                            }
                        }
                    }

                    BindFinancialYearDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindFinancialYearDropdown(bool _ShowHistory = false)
        {
            Dropdownlist.DDLFinancialYear(ddlFinancialYear);

            if (!_ShowHistory)
            {
                DateTime Now = DateTime.Now;

                if (Now.Month <= 6)
                {
                    ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year - 1, Now.ToString("yy"));
                }
                else
                {
                    Now = new DateTime(Now.Year + 1, 1, 1);
                    ddlFinancialYear.SelectedValue = string.Format("{0}-{1}", Now.Year, (Now.Year + 1).ToString().Substring(2));
                }

                ddlFinancialYear_SelectedIndexChanged(null, null);
            }
        }

        private void BindGrid()
        {
            string FinancialYear = ddlFinancialYear.SelectedItem.Value;
            lstFundRelease = bllAccounts.GetFundRelaseByFinancialYear(FinancialYear);
            gvFundRelease.DataSource = lstFundRelease;
            gvFundRelease.DataBind();

            Hashtable SearchCriteria = new Hashtable();
            SearchCriteria.Add(FinancialYearKey, FinancialYear);
            SearchCriteria.Add(PageIndexKey, gvFundRelease.PageIndex);
            Session[SessionValues.FundRelease] = SearchCriteria;
        }

        private void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.FundRelease];

            string FinancialYear = (string)SearchCriteria[FinancialYearKey];
            ddlFinancialYear.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlFinancialYear, FinancialYear);

            gvFundRelease.PageIndex = (int)SearchCriteria[PageIndexKey];

            ddlFinancialYear_SelectedIndexChanged(null, null);
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlFinancialYear.SelectedItem.Value == "")
                {
                    gvFundRelease.Visible = false;
                    ViewState[StartDate] = DateTime.Now;
                    ViewState[EndDate] = DateTime.Now;
                }
                else
                {
                    BindGrid();
                    gvFundRelease.Visible = true;

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

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    string FinancialYear = ddlFinancialYear.SelectedItem.Value;
                    lstFundRelease = bllAccounts.GetFundRelaseByFinancialYear(FinancialYear);

                    //AT_FundRelease mdlFundRelease = new AT_FundRelease();
                    //mdlFundRelease.ID = 0;
                    //mdlFundRelease.FinancialYear = "";
                    //mdlFundRelease.FundReleaseDate = DateTime.Today;
                    //mdlFundRelease.FundReleaseTypeID = 0;
                    //mdlFundRelease.Description = "";
                    dynamic a = new
                    {
                        ID = 0,
                        FundReleaseType = "",
                        FundReleaseTypeID = 0,
                        FundReleaseDate = "",
                        Description = ""
                    };
                    lstFundRelease.Add(a);

                    gvFundRelease.PageIndex = gvFundRelease.PageCount;
                    gvFundRelease.DataSource = lstFundRelease;
                    gvFundRelease.DataBind();

                    gvFundRelease.EditIndex = gvFundRelease.Rows.Count - 1;
                    gvFundRelease.DataBind();
                    gvFundRelease.Rows[gvFundRelease.Rows.Count - 1].FindControl("ddlFundReleaseType").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvFundRelease.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                string FinancialYear = ddlFinancialYear.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlFinancialYear.SelectedItem.Value);
                long FundReleaseID = Convert.ToInt64(((Label)gvFundRelease.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                //long FundReleaseTypeID = Convert.ToInt64(((Label)gvFundRelease.Rows[rowIndex].Cells[1].FindControl("lblFundReleaseTypeID")).Text);
                long FundReleaseTypeID = ((DropDownList)gvFundRelease.Rows[rowIndex].Cells[2].FindControl("ddlFundReleaseType")).SelectedValue == string.Empty ? -1 : Convert.ToInt64(((DropDownList)gvFundRelease.Rows[rowIndex].Cells[0].FindControl("ddlFundReleaseType")).SelectedValue);
                DateTime FundReleaseDate = Convert.ToDateTime(((TextBox)gvFundRelease.Rows[rowIndex].Cells[3].FindControl("txtFundReleaseDate")).Text.Trim());
                string Desc = ((TextBox)gvFundRelease.Rows[rowIndex].Cells[4].FindControl("txtDesc")).Text.Trim();

                AT_FundRelease mdlLatestFundRelase = bllAccounts.GetLatestReleaseByYear(FinancialYear, FundReleaseID);

                if (mdlLatestFundRelase != null)
                {
                    if (FundReleaseDate < mdlLatestFundRelase.FundReleaseDate)
                    {
                        Master.ShowMessage(Message.FundReleaseDatePreviouslyAdded.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (bllAccounts.IsFundReleaseUnique(FundReleaseTypeID, FinancialYear))
                {
                    Master.ShowMessage(Message.FundReleaseTypeUnique.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (gvFundRelease.Rows.Count != 1)
                {
                    if (Convert.ToString(ViewState["btnEdit"]) == "True" && FundReleaseID == 0)
                    {
                        Master.ShowMessage(Message.PreviousFundReleaseDetail.Description, SiteMaster.MessageType.Error);
                        return;

                    }
                }

                if (bllAccounts.IsFundRelease(FundReleaseID, FinancialYear, FundReleaseDate))
                {
                    Master.ShowMessage(Message.ReleaseDateUnique.Description, SiteMaster.MessageType.Error);
                    return;
                }

                AT_FundRelease mdlFundRelease = new AT_FundRelease();
                mdlFundRelease.ID = FundReleaseID;
                mdlFundRelease.FinancialYear = FinancialYear;
                mdlFundRelease.FundReleaseDate = FundReleaseDate;
                mdlFundRelease.FundReleaseTypeID = FundReleaseTypeID;
                mdlFundRelease.Description = Desc;

                bool isRecordSaved = false;

                if (FundReleaseID == 0)
                {
                    isRecordSaved = bllAccounts.AddFundRelease(mdlFundRelease);
                }
                else
                {
                    isRecordSaved = bllAccounts.UpdateFundRelease(mdlFundRelease);
                }
                if (isRecordSaved)
                {
                    if (FundReleaseID == 0)
                    {
                        gvFundRelease.PageIndex = 0;
                    }
                    gvFundRelease.EditIndex = -1;
                    BindGrid();
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvFundRelease.EditIndex = e.NewEditIndex;
                BindGrid();
                gvFundRelease.Rows[e.NewEditIndex].FindControl("ddlFundReleaseType").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long FundReleaseID = Convert.ToInt32(((Label)gvFundRelease.Rows[e.RowIndex].FindControl("lblID")).Text);

                bool IsDeleted = bllAccounts.DeleteFundRelease(FundReleaseID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFundRelease.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvFundRelease.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundRelease_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblFundReleaseDate = (Label)e.Row.FindControl("lblFundReleaseDate");
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (lblFundReleaseDate != null)
                        if (lblFundReleaseDate.Text != "")
                            lblFundReleaseDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblFundReleaseDate.Text));

                    if (gvFundRelease.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlFundReleaseType = (DropDownList)e.Row.FindControl("ddlFundReleaseType");

                        Dropdownlist.DDLFundReleaseType(ddlFundReleaseType);

                        Label lblFundReleaseTypeID = (Label)e.Row.FindControl("lblFundReleaseTypeID");
                        Dropdownlist.SetSelectedValue(ddlFundReleaseType, lblFundReleaseTypeID.Text);

                        TextBox txtFundReleaseDate = (TextBox)e.Row.FindControl("txtFundReleaseDate");
                        txtFundReleaseDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(txtFundReleaseDate.Text));
                    }

                    bool IsFundReleaseIDExist = bllAccounts.GetFundReleaseDetailsByFundReleaseID(Convert.ToInt64(lblID.Text));
                    if (IsFundReleaseIDExist)
                    {
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        ViewState["btnEdit"] = false;
                    }
                    else
                    {
                        ViewState["btnEdit"] = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}