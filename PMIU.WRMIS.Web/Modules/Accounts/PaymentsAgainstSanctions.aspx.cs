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
    public partial class PaymentsAgainstSanctions : BasePage
    {
        AccountsBLL bllAcounts = new AccountsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    btnBack.NavigateUrl = "~/Modules/Accounts/SearchCheque.aspx?ShowHistory=true";
                    txtCashDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    txtChequeDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //Dropdownlist.DDLExpenseType(ddlSanctionType);
                    BindGrid();
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
            long SanctionTypeID = -1;//ddlSanctionType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSanctionType.SelectedItem.Value);
            List<AT_Sanction> lstSanction = bllAcounts.GetAllSanctions(SanctionTypeID);
            gvPayments.DataSource = lstSanction;
            gvPayments.DataBind();
        }

        protected void ddlSanctionType_SelectedIndexChanged(object sender, EventArgs e)
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

        //private void SaveData()
        //{
        //    DateTime CashDate = Convert.ToDateTime(txtCashDate.Text);
        //    DateTime ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
        //    if (CashDate < ChequeDate)
        //    {
        //        Master.ShowMessage(Message.CashDateCanNotBeLessThanChequeDate.Description, SiteMaster.MessageType.Error);
        //        return;
        //    }

        //    AT_PaymentDetails mdlPaymentDetails = new AT_PaymentDetails();

        //    List<Tuple<string, string, string>> lstNameofFiles = fupAttachBill.UploadNow(Configuration.Accounts);

        //    string FileName = null;

        //    if (lstNameofFiles.Count > 0)
        //    {
        //        FileName = lstNameofFiles[0].Item3.ToString();
        //    }

        //    mdlPaymentDetails.ChequeNo = txtChequeNo.Text;
        //    mdlPaymentDetails.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
        //    mdlPaymentDetails.ChequeAmount = Convert.ToDouble(txtChequeAmount.Text);
        //    mdlPaymentDetails.CashDate = Convert.ToDateTime(txtCashDate.Text);
        //    mdlPaymentDetails.ChequeAttachment = FileName;

        //    bllAcounts.AddPaymentDetails(mdlPaymentDetails);

        //    mdlPaymentDetails = null;

        //    mdlPaymentDetails = bllAcounts.GetLatestPaymentDetail();

        //    AT_SanctionPayment mdlSanctionPayment = new AT_SanctionPayment();

        //    foreach (TableRow Row in gvPayments.Rows)
        //    {
        //        Label lblID = (Label)Row.FindControl("lblID");
        //        CheckBox chk = (CheckBox)Row.FindControl("chk");

        //        if (chk.Checked)
        //        {
        //            mdlSanctionPayment.PaymentDetailID = mdlPaymentDetails.ID;
        //            mdlSanctionPayment.SanctionID = Convert.ToInt64(lblID.Text);
        //            bllAcounts.AddSanctionPayment(mdlSanctionPayment);


        //            AT_Sanction mdlSanction = new AT_Sanction();
        //            mdlSanction = bllAcounts.GetSanctionByID(Convert.ToInt64(lblID.Text));
        //            mdlSanction.SanctionStatusID = (long)Constants.SanctionStatus.PaymentReleased;
        //            bllAcounts.UpdateSanction(mdlSanction);
        //            bllAcounts.SetSanctionExpenseStatus(mdlSanction.ID);
        //        }
        //    }
        //    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
        //}


        private void SaveData()
        {
            DateTime CashDate = Convert.ToDateTime(txtCashDate.Text);
            DateTime ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
            bool SanctionExist = false;
            if (CashDate < ChequeDate)
            {
                Master.ShowMessage(Message.CashDateCanNotBeLessThanChequeDate.Description, SiteMaster.MessageType.Error);
                return;
            }

            AT_PaymentDetails mdlPaymentDetails = new AT_PaymentDetails();

            List<Tuple<string, string, string>> lstNameofFiles = fupAttachBill.UploadNow(Configuration.Accounts);

            string FileName = null;

            if (lstNameofFiles.Count > 0)
            {
                FileName = lstNameofFiles[0].Item3.ToString();
            }

            mdlPaymentDetails.ChequeNo = txtChequeNo.Text;
            mdlPaymentDetails.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
            mdlPaymentDetails.ChequeAmount = Convert.ToDouble(txtChequeAmount.Text);
            mdlPaymentDetails.CashDate = Convert.ToDateTime(txtCashDate.Text);
            mdlPaymentDetails.ChequeAttachment = FileName;

            bllAcounts.AddPaymentDetails(mdlPaymentDetails);

            mdlPaymentDetails = null;

            mdlPaymentDetails = bllAcounts.GetLatestPaymentDetail();

            AT_SanctionPayment mdlSanctionPayment = new AT_SanctionPayment();

            foreach (TableRow Row in gvPayments.Rows)
            {
                Label lblID = (Label)Row.FindControl("lblID");
                CheckBox chk = (CheckBox)Row.FindControl("chk");

                if (chk.Checked)
                {
                    SanctionExist = true;
                    mdlSanctionPayment.PaymentDetailID = mdlPaymentDetails.ID;
                    mdlSanctionPayment.SanctionID = Convert.ToInt64(lblID.Text);
                    bllAcounts.AddSanctionPayment(mdlSanctionPayment);


                    AT_Sanction mdlSanction = new AT_Sanction();
                    mdlSanction = bllAcounts.GetSanctionByID(Convert.ToInt64(lblID.Text));
                    mdlSanction.SanctionStatusID = (long)Constants.SanctionStatus.PaymentReleased;
                    bllAcounts.UpdateSanction(mdlSanction);
                    bllAcounts.SetSanctionExpenseStatus(mdlSanction.ID);

                    chk.Checked = false;
                }
            }

            if (!SanctionExist)
            {
                bllAcounts.DeleteCheque(mdlPaymentDetails.ID);
                Master.ShowMessage(Message.AtleastOneSanctionMustBeAssociatedWithTheCheque.Description, SiteMaster.MessageType.Error);
            }
            else
            {
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                txtChequeAmount.Text = "";
                txtChequeNo.Text = "";
                txtCashDate.Text = "";
                txtChequeDate.Text = "";

                BindGrid();
                //ddlSanctionType.ClearSelection();
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSanctionType = (Label)e.Row.FindControl("lblSanctionType");
                    Label lblSanctionTypeName = (Label)e.Row.FindControl("lblSanctionTypeName");
                    Label lblSanctionedAmount = (Label)e.Row.FindControl("lblSanctionedAmount");

                    if (lblSanctionedAmount.Text != string.Empty)
                    {
                        lblSanctionedAmount.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(lblSanctionedAmount.Text));
                    }

                    if (lblSanctionType.Text == string.Empty)
                    {
                        lblSanctionType.Text = lblSanctionTypeName.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}