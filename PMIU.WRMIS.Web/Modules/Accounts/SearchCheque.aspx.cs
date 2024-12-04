using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class SearchCheque : BasePage
    {
        #region Hash Table keys

        public const string ChequeNoKey = "ChequeNo";
        public const string DateFromKey = "DateFrom";
        public const string DateToKey = "DateTo";
        public const string AmountFromKey = "AmountFrom";
        public const string AmountToKey = "AmountTo";
        public const string PageIndexKey = "PageIndex";

        #endregion

        public string GridDisplay = "block";

        AccountsBLL bllAccounts = new AccountsBLL();
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
                            if (Session[SessionValues.SearchChequeCriteria] != null)
                            {
                                BindHistoryData();
                                return;
                            }
                        }
                    }

                    DateTime ToDate = DateTime.Now;
                    DateTime FromDate = ToDate.AddDays(-30);
                    txtCheckDateFrom.Text = Utility.GetFormattedDate(FromDate);
                    txtCheckDateTo.Text = Utility.GetFormattedDate(ToDate);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
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
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                string ChequeNo = txtChequeNo.Text == string.Empty ? null : txtChequeNo.Text;
                string ChequeFromDate = null;
                string ChequeToDate = null;
                double? ChequeFromAmount = null;
                double? ChequeToAmount = null;

                if (txtCheckDateFrom.Text != "")
                {
                    ChequeFromDate = Convert.ToDateTime(txtCheckDateFrom.Text).ToString();
                }

                if (txtCheckDateTo.Text != "")
                {
                    ChequeToDate = Convert.ToDateTime(txtCheckDateTo.Text).ToString();
                }

                if (txtAmountBetweenFrom.Text != "")
                {
                    ChequeFromAmount = Convert.ToDouble(txtAmountBetweenFrom.Text);
                }

                if (txtAmountBetweenAnd.Text != "")
                {
                    ChequeToAmount = Convert.ToDouble(txtAmountBetweenAnd.Text);
                }

                SearchCriteria.Add(ChequeNoKey, txtChequeNo.Text);
                SearchCriteria.Add(DateFromKey, txtCheckDateFrom.Text);
                SearchCriteria.Add(DateToKey, txtCheckDateTo.Text);
                SearchCriteria.Add(AmountFromKey, txtAmountBetweenFrom.Text);
                SearchCriteria.Add(AmountToKey, txtAmountBetweenAnd.Text);

                DataSet DS = bllAccounts.SearchCheque(ChequeNo, ChequeFromDate, ChequeToDate, ChequeFromAmount, ChequeToAmount);

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvSearchCheques.DataSource = DS.Tables[0];
                }
                else
                {
                    gvSearchCheques.DataSource = new DataTable();
                }

                gvSearchCheques.DataBind();

                SearchCriteria.Add(PageIndexKey, gvSearchCheques.PageIndex);

                Session[SessionValues.SearchChequeCriteria] = SearchCriteria;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvSearchCheques_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lblChequeDate = (Label)e.Row.FindControl("lblChequeDate");
                    Label lblCashDate = (Label)e.Row.FindControl("lblCashDate");
                    Label lblPaymentDate = (Label)e.Row.FindControl("lblPaymentDate");
                    HyperLink btnTaxSheet = (HyperLink)e.Row.FindControl("btnTaxSheet");
                    Label lblChequeAmount = (Label)e.Row.FindControl("lblChequeAmount");
                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");
                    WebFormsTest.FileUploadControl fupBillAttachment = (WebFormsTest.FileUploadControl)e.Row.FindControl("fupBillAttachment");

                    DataKey key = gvSearchCheques.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string FileName = Convert.ToString(key.Values["ChequeAttachment"]);

                    if (!string.IsNullOrEmpty(lblChequeDate.Text))
                        lblChequeDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblChequeDate.Text));

                    if (!string.IsNullOrEmpty(lblCashDate.Text))
                        lblCashDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblCashDate.Text));

                    if (!string.IsNullOrEmpty(lblPaymentDate.Text))
                    {
                        lblPaymentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblPaymentDate.Text));
                    }

                    if (lblChequeAmount != null)
                        if (!string.IsNullOrEmpty(lblChequeAmount.Text))
                            lblChequeAmount.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(lblChequeAmount.Text));

                    if (FileName != string.Empty)
                    {
                        string Attachment = new System.IO.FileInfo(FileName).Name;
                        List<string> lstName = new List<string> { Attachment };

                        fupBillAttachment.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        fupBillAttachment.Size = 1;
                        fupBillAttachment.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
                    }

                    hlDetail.NavigateUrl = "~/Modules/Accounts/PaymentsAgainstSanctionsView.aspx?PaymentDetailID=" + ID.ToString();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchCheques_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchCheques.PageIndex = e.NewPageIndex;
                gvSearchCheques.EditIndex = -1;
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

        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchChequeCriteria];

            txtChequeNo.Text = (string)SearchCriteria[ChequeNoKey];
            txtCheckDateFrom.Text = (string)SearchCriteria[DateFromKey];
            txtCheckDateTo.Text = (string)SearchCriteria[DateToKey];
            txtAmountBetweenFrom.Text = (string)SearchCriteria[AmountFromKey];
            txtAmountBetweenAnd.Text = (string)SearchCriteria[AmountToKey];

            gvSearchCheques.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindGrid();
        }

        protected void lbtnPrintAcquittenceRoll_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                Label lblChequeNoGrid = (Label)gvrCurrent.FindControl("lblChequeNoGrid");

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("ChequeNo", lblChequeNoGrid.Text);
                mdlReportData.Parameters.Add(ReportParameter);

                mdlReportData.Name = Constants.AcquaintanceRollReport;

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnAddPaymentDate_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnAddPaymentDate = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lbtnAddPaymentDate.NamingContainer;

                Label lblID = (Label)gvRow.FindControl("lblID");
                Label lblPaymentDate = (Label)gvRow.FindControl("lblPaymentDate");

                hdnChequeID.Value = lblID.Text;
                txtPaymentDate.Text = lblPaymentDate.Text;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddPaymentDate", "$('#AddPaymentDate').modal();", true);
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

                AT_PaymentDetails mdlPaymentDetails = new AT_PaymentDetails();

                mdlPaymentDetails.ID = Convert.ToInt64(hdnChequeID.Value);
                mdlPaymentDetails.PaymentDate = (txtPaymentDate.Text.Trim() == string.Empty ? (DateTime?)null : Convert.ToDateTime(txtPaymentDate.Text));
                mdlPaymentDetails.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlPaymentDetails.ModifiedDate = DateTime.Now;

                bllAccounts.UpdateChequePaymentDate(mdlPaymentDetails);

                BindGrid();

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddPaymentDate", "$('#AddPaymentDate').modal(hide);", true);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddPaymentDate", "$('#AddPaymentDate').modal(hide);", true);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}