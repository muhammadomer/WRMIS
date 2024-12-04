using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class PaymentsAgainstSanctionsView : BasePage
    {
        AccountsBLL bllAcounts = new AccountsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                long _PaymentDetailID = 0;
                if (!IsPostBack)
                {
                    SetPageTitle();
                    _PaymentDetailID = Utility.GetNumericValueFromQueryString("PaymentDetailID", 0);
                    hlBack.NavigateUrl = string.Format("~/Modules/Accounts/SearchCheque.aspx?ShowHistory=true", _PaymentDetailID);
                    LoadPaymentDetail(_PaymentDetailID);
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

        private void LoadPaymentDetail(long _PaymentDetailID)
        {
            try
            {
                DataSet DS = new AccountsBLL().GetPaymentDetailsByID(_PaymentDetailID);

                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    txtChequeNo.Text = DR["ChequeNo"].ToString();
                    txtChequeDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(DR["ChequeDate"]));
                    txtChequeAmount.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(DR["ChequeAmount"].ToString()));
                    txtCashDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(DR["CashDate"]));

                    string FileName = DR["ChequeAttachment"].ToString();

                    if (FileName != string.Empty)
                    {
                        string Attachment = new System.IO.FileInfo(FileName).Name;
                        List<string> lstName = new List<string> { Attachment };

                        fupBillAttachment.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        fupBillAttachment.Size = 1;
                        fupBillAttachment.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
                    }
                }

                if (DS.Tables.Count > 0)
                    gvPayments.DataSource = DS.Tables[1];

                gvPayments.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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