using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class Payments : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    EnableBillNo(false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIndName.Checked)
                EnableBillNo(false);
            else if (rbBillNo.Checked)
                EnableBillNo(true);

            if (rbCanal.Checked)
                LoadPaymentPanel(false);
            else if (rbEffluent.Checked)
                LoadPaymentPanel(true);
        }

        protected void rbService_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCanal.Checked)
                LoadPaymentPanel(false);
            else if (rbEffluent.Checked)
                LoadPaymentPanel(true);
        }

        private void EnableBillNo(bool _Flag)
        {
            pnlDetails.Visible = false;
            txtNo.Text = string.Empty;
        }
        private void LoadIndustryInfo()
        {

            EC_Industry _Mdl = null;
            if (rbIndName.Checked)
            {
             
                _Mdl = bllEWC.GetIndustry(Convert.ToInt64(txtNo.Text), string.Empty);
            }
            else
            {
                if (bllEWC.GetBill(txtNo.Text) != null)
                    _Mdl = bllEWC.GetBill(txtNo.Text).EC_Industry;
            }
            if (_Mdl == null)
            {
                pnlDetails.Visible = false;
                Master.ShowMessage("No record found.", SiteMaster.MessageType.Error);
                return;
            }
            pnlDetails.Visible = true;
            lblID.Text = _Mdl.ID.ToString();
            lblName.Text = _Mdl.IndustryName;
            lblType.Text = _Mdl.EC_IndustryType.Name;
            lblEffBlnc.Text = Utility.GetRoundOffValue(_Mdl.EWCurrentBalance == null ? 0 : _Mdl.EWCurrentBalance);
            if (lblEffBlnc.Text == "")
                lblEffBlnc.Text = "0";
            lblCnlBlnc.Text = Utility.GetRoundOffValue(_Mdl.CWCurrentBalance == null ? 0 : _Mdl.CWCurrentBalance);
            if (lblCnlBlnc.Text == "")
                lblCnlBlnc.Text = "0";

            List<object> lstBanks = bllEWC.Bank_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlBank, false, (int)Constants.DropDownFirstOption.Select, lstBanks);

            if (_Mdl.IsEffluentWater == true && _Mdl.IsCanalWater == true)
            {
            }
            else
            {
                rbEffluent.Checked = false;
                rbEffluent.Enabled = false;

                rbCanal.Checked = false;
                rbCanal.Enabled = false;
            }

            if (_Mdl.IsCanalWater == true)
            {
                rbCanal.Checked = true;
                rbCanal.Enabled = true;
                rbEffluent.Checked = false;

                LoadPaymentPanel(false);
            }


            if (_Mdl.IsEffluentWater == true)
            {
                rbEffluent.Checked = true;
                rbEffluent.Enabled = true;

                rbCanal.Checked = false;
                LoadPaymentPanel(true);
            }

            pnlDetails.Visible = true;

            Session["CurrentIndustry"] = _Mdl;
        }

        private void LoadPaymentPanel(bool _IsEffluent)
        {
            EC_Industry mdl = Session["CurrentIndustry"] as EC_Industry;
            txtDepositAmount.Text = txtChequeNo.Text = string.Empty;
            ddlBank.SelectedItem.Selected = false;
            ddlBank.Items.FindByValue(string.Empty).Selected = true;
            txtDepositDate.Text = Utility.GetFormattedDate(DateTime.Now);
            pnlPayment.Visible = true;
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                string btnName = ((Button)sender).ID;

                if (btnName.Equals("btnSubmit", StringComparison.OrdinalIgnoreCase))
                { 
                    EC_Payments mdl = new EC_Payments();

                    if (Utility.GetParsedDate(txtDepositDate.Text) > DateTime.Now)
                    {
                        Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    mdl.PaymentDate = Utility.GetParsedDate(txtDepositDate.Text);
                    mdl.IndustryID = Convert.ToInt64(lblID.Text);
                    mdl.ServiceType = rbEffluent.Checked ?
                        Constants.ECWServiceType.EFFLUENT.ToString() : Constants.ECWServiceType.CANAL.ToString();
                    if (rbBillNo.Checked)
                        mdl.BillID = bllEWC.GetBill(txtNo.Text.Trim()).ID;
                    else
                        mdl.BillID = bllEWC.GetIndustryBillID(mdl.IndustryID, mdl.ServiceType);

                    if( Convert.ToDouble(txtDepositAmount.Text.Trim()) <= 0)
                    { 
                        Master.ShowMessage("Enter a valid amount.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdl.PaymentAmount = Convert.ToDouble(txtDepositAmount.Text.Trim());
                    if (string.Equals(mdl.ServiceType, Constants.ECWServiceType.EFFLUENT.ToString()))
                        mdl.LastBalance = Convert.ToDouble(lblEffBlnc.Text);
                    else
                        mdl.LastBalance = Convert.ToDouble(lblCnlBlnc.Text);
                    mdl.PaymentReceivedBy = (int)SessionManagerFacade.UserInformation.ID;
                    mdl.ChequeNo = txtChequeNo.Text;
                    mdl.PaymentMode = "Cheque";
                    mdl.BankID = Convert.ToInt32(ddlBank.SelectedItem.Value);
                    mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    mdl.CreatedDate = DateTime.Now;

                    bllEWC.SaveBillPayment(mdl);
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
                else if (btnName.Equals("btnCancel", StringComparison.OrdinalIgnoreCase))
                {
                    pnlPayment.Visible = false;
                    rbEffluent.Checked = rbCanal.Checked = false;
                }
                if (txtNo.Text != "")
                    LoadIndustryInfo();

            }
            catch (Exception exp)
            {
                pnlDetails.Visible = false;
                Master.ShowMessage("No record found.", SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}