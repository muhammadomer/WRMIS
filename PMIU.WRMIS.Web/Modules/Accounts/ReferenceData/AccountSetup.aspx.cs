
using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class AccountSetup : BasePage
    {
        ReferenceDataBLL ACBLL = new ReferenceDataBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
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

        protected void gvAccountSetup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAccountSetup.EditIndex = e.NewEditIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAccountSetup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAccountSetup.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindGrid()
        {
            List<object> lstAS = ACBLL.GetAccountSetups();
            Session["AcountStup"] = lstAS;
           // ViewState["QuationAmount"] = (from item in lstAS where Convert.ToString(item.GetType().GetProperty("ACRule").GetValue(item)).Trim().ToUpper() == "EXPENSE LIMIT FOR QUOTATIONS" select Convert.ToString(item.GetType().GetProperty("Amount").GetValue(item)));
            gvAccountSetup.DataSource = lstAS;
            gvAccountSetup.DataBind();
        }
        public bool IsAmountUpdate(long ID, Double Amount)
        {
            List<object> lstObject = Session["AcountStup"] as List<object>;
            object obj = (from item in lstObject where Convert.ToInt64(item.GetType().GetProperty("ID").GetValue(item)) == ID select item).FirstOrDefault();
            if (Convert.ToDouble(obj.GetType().GetProperty("Amount").GetValue(obj))==Amount)
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        protected void gvAccountSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lblRule = (Label)e.Row.FindControl("lblRule");
                    TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                    if (lblRule != null)
                    {
                        if (lblRule.Text.ToUpper().Trim()=="EXPENSE LIMIT FOR TAX" || lblRule.Text.ToUpper().Trim()=="EXPENSE LIMIT FOR QOTATIONS" || lblRule.Text.ToUpper().Trim()=="EXPENSE LIMIT FOR TENDERS")
                        {
                            if (txtAmount!=null)
                            {
                                txtAmount.CssClass = "form-control integerInput required col-md-4";
                                txtAmount.MaxLength = 7;    
                            }
                            
                        }
                        if (lblRule.Text.ToUpper().Trim()=="PER KM RATE FOR TA")
                        {
                            if (txtAmount!=null)
                            {
                                txtAmount.CssClass = "form-control decimal2PInput required col-md-4";
                                txtAmount.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + 1+ "','" + 999.99 + "');");
                                txtAmount.MaxLength = 6; 
                            }
                            
                        }
                        if (lblRule.Text.ToUpper().Trim() == "ANNUAL SANCTION POWER OF DDO") 
                        {
                            if (txtAmount!=null)
                            {
                                txtAmount.CssClass = "form-control integerInput required";
                                txtAmount.MaxLength = 9;    
                            }
                            
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAccountSetup_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                DataKey key = gvAccountSetup.DataKeys[e.RowIndex];
                long ModifiedBy= SessionManagerFacade.UserInformation.ID;
                long ASID = Convert.ToInt64(key.Values["ID"]);
                Double Amount = Convert.ToDouble(((TextBox)gvAccountSetup.Rows[RowIndex].FindControl("txtAmount")).Text);
                gvAccountSetup.EditIndex = -1;
                string ACRule = Convert.ToString(((Label)gvAccountSetup.Rows[RowIndex].FindControl("lblRule")).Text);
                if (ACRule.ToUpper() == "EXPENSE LIMIT FOR TENDERS")
                   {
                        string lblAmount = Convert.ToString(((Label)gvAccountSetup.Rows[RowIndex - 1].FindControl("lblAmount")).Text);
                       if (lblAmount.Contains(','))
                       {
                           string [] array = lblAmount.Split(',');
                           lblAmount = "";
                           foreach (string item in array)
                           {
                               lblAmount = lblAmount + item;
                           }
                       }
                       if (Convert.ToInt64(lblAmount) >= Amount)
                       {
                           Master.ShowMessage(Message.ExpenseLimitforTenders.Description, SiteMaster.MessageType.Success);
                           gvAccountSetup.EditIndex = RowIndex;
                           return;
                       }
                   }
                if (ACRule.ToUpper() == "EXPENSE LIMIT FOR QOTATIONS")
                {
                     string lblAmount = Convert.ToString(((Label)gvAccountSetup.Rows[RowIndex + 1].FindControl("lblAmount")).Text);
                       if (lblAmount.Contains(','))
                       {
                           string [] array = lblAmount.Split(',');
                           lblAmount = "";
                           foreach (string item in array)
                           {
                               lblAmount = lblAmount + item;
                           }
                       }
                       if (Convert.ToInt64(lblAmount) <= Amount)
                       {
                           Master.ShowMessage(Message.ExpenseLimitforTenders.Description, SiteMaster.MessageType.Success);
                           gvAccountSetup.EditIndex = RowIndex;
                           return;
                       }
                }
                if (IsAmountUpdate(ASID, Amount))
                {
                    ACBLL.UpdateAccountSetupsAmount(ASID, Amount, ModifiedBy, DateTime.Now);
                } 
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                //gvAccountSetup.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}