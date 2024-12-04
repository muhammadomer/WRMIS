using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.BLL.WaterLosses;
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

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class BillDetail : BasePage
    {
        private Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        hdnIndustryID.Value = Request.QueryString["ID"];
                    }
                    lblIndustryName.Text = Session["IndustryName"].ToString();
                    lblIndustryNo.Text = Request.QueryString["ID"];
                    if (!string.IsNullOrEmpty(Session["EWDivision"].ToString()))
                    {
                        lblDivision.Text = Session["EWDivision"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Session["CWDivision"].ToString()))
                    {
                        lblDivision.Text = Session["CWDivision"].ToString();
                    }
                    CheckServiceType();
                    SetPageTitle();
                    LoadFinancialYearDDL();

                    if (!string.IsNullOrEmpty(Request.QueryString["finalize"]) || !string.IsNullOrEmpty(Request.QueryString["print"]))
                    {
                        if (Request.QueryString["ServiceType"] == "EFFLUENT")
                            rbEffluent.Checked = true;
                        else
                            rbCanal.Checked = true;
                        hdnBillID.Value = Request.QueryString["BillID"];
                    }
                    else
                    {
                        string ServiceType = null;
                        object bllGetLastBillID = null;
                        if (rbEffluent.Checked)
                            ServiceType = "EFFLUENT";
                        else
                            ServiceType = "CANAL";

                        if (ddlfinancialyear.Items.Count > 0)
                        {
                            bllGetLastBillID = bllEWC.GetLastECBillID(ServiceType, Convert.ToString(ddlfinancialyear.SelectedItem.Value), Convert.ToInt64(Request.QueryString["ID"]));
                            if (bllGetLastBillID != null)
                            {
                                string BillID = Convert.ToString(bllGetLastBillID.GetType().GetProperty("BillID").GetValue(bllGetLastBillID));
                                hdnBillID.Value = BillID;
                            }
                        }
                        // bllGetLastBillID = bllEWC.GetLastECBillID(ServiceType, Convert.ToString(ddlfinancialyear.SelectedItem.Value), Convert.ToInt64(Request.QueryString["ID"]));
                        //if (bllGetLastBillID != null)
                        //{
                        //    hdnBillID.Value = Convert.ToString(bllGetLastBillID.GetType().GetProperty("BillID").GetValue(bllGetLastBillID));

                        //}
                        //else
                        //{
                        //    bllGetLastBillID = bllEWC.GetLastECBillID("CANAL", Convert.ToString(ddlfinancialyear.SelectedItem.Value), Convert.ToInt64(Request.QueryString["ID"]));
                        //    if (bllGetLastBillID != null)
                        //    {
                        //        rbEffluent.Checked = false;
                        //        rbCanal.Checked = true;
                        //        hdnBillID.Value = Convert.ToString(bllGetLastBillID.GetType().GetProperty("BillID").GetValue(bllGetLastBillID));

                        //    }
                        //}
                    }
                    ddlfinancialyear.Enabled = false;
                    if (base.CanView == true)
                    {
                        ddlfinancialyear.Enabled = true;
                        GetBillDetail();

                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["finalize"]))
                    {
                        hlBack.NavigateUrl = string.Format("~/Modules/EWC/FinalizePrintBill.aspx?RestoreState=1");
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["print"]))
                    {
                        hlBack.NavigateUrl = string.Format("~/Modules/EWC/PrintBill.aspx?RestoreState=1");
                    }
                    else
                    {
                        hlBack.NavigateUrl = string.Format("~/Modules/EWC/Industry.aspx?RestoreState=1");
                    }

                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void CheckServiceType()
        {
            EC_Industry _Mdl = null;
            _Mdl = bllEWC.GetIndustry(Convert.ToInt64(hdnIndustryID.Value), string.Empty);
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
            }
            if (_Mdl.IsEffluentWater == true)
            {
                rbEffluent.Checked = true;
                rbEffluent.Enabled = true;
                rbCanal.Checked = false;

            }
        }
        protected void rbService_CheckedChanged(object sender, EventArgs e)
        {
            GetBillDetail();
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadFinancialYearDDL()
        {
            List<object> lstFinancialYear = bllEWC.Financial_GetList().Select(x => new { ID = x.FinancialYear, Name = x.FinancialYear }).Distinct().ToList<object>();
            Dropdownlist.DDLLoading(ddlfinancialyear, false, (int)Constants.DropDownFirstOption.NoOption, lstFinancialYear);

            if (!string.IsNullOrEmpty(Request.QueryString["Year"]))
            {
                if (ddlfinancialyear.Items.FindByValue(Request.QueryString["Year"].ToString()) != null)
                {
                    ddlfinancialyear.Items.FindByValue(Request.QueryString["Year"].ToString()).Selected = true;
                }
            }
        }
        private void GetBillDetail()
        {
            try
            {
                string ServiceType = null;
                string LastYearBillingDays = null;
                lblTotalBillText.Text = "Total Bill (Rs.)";
                if (rbEffluent.Checked)
                {
                    //pnlWaterBilling.GroupingText = "Effluent Water Billing Details";
                    ServiceType = "EFFLUENT";
                    lblChargesText.Text = "Effluent Charges (Rs.)";
                    lblRateText.Text = "Effluent Rate (Rs.)";
                    lblSanctionedDischargeText.Text = "Discharge (Cusec)";
                }
                else
                {
                    // pnlWaterBilling.GroupingText = "Canal Special Water Billing Details";
                    ServiceType = "CANAL";
                    lblChargesText.Text = "Canal Water Charges (Rs.)";
                    lblRateText.Text = "Canal Water Rate (Rs.)";
                    lblSanctionedDischargeText.Text = "Supply (Cusec)";
                }
                ClearControls();

                object bllGetLastBillID = null;
                if (rbCanal.Checked)
                    bllGetLastBillID = bllEWC.GetLastECBillID("CANAL", Convert.ToString(ddlfinancialyear.SelectedItem.Value), Convert.ToInt64(Request.QueryString["ID"]));
                else
                    bllGetLastBillID = bllEWC.GetLastECBillID("EFFLUENT", Convert.ToString(ddlfinancialyear.SelectedItem.Value), Convert.ToInt64(Request.QueryString["ID"]));

                if (bllGetLastBillID != null)
                {
                    string BillID = Convert.ToString(bllGetLastBillID.GetType().GetProperty("BillID").GetValue(bllGetLastBillID));
                    hdnBillID.Value = BillID;
                }

                DataSet DS = bllEWC.GetBillDetails(ServiceType, Convert.ToInt64(hdnBillID.Value));
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    pnlWaterBilling.Visible = true;
                    pnlBilldetail.Visible = true;
                    gv.DataSource = DS.Tables[0];
                    DataRow DR = DS.Tables[0].Rows[0];

                    //lblIndustryName.Text = DR["IndustryName"].ToString();
                    //lblIndustryNo.Text = DR["IndustryID"].ToString();
                    //lblDivision.Text = DR["Division"].ToString();
                    lblCharges.Text = Utility.GetRoundOffValue(DR["Rate"].ToString());

                    lblApplicableTax.Text = Utility.GetRoundOffValue(DR["ApplicableTax"].ToString() == string.Empty ? "0.00" : DR["ApplicableTax"].ToString());
                    lblAdjustment.Text = Utility.GetRoundOffValue(DR["Adjustment"].ToString() == string.Empty ? "0.00" : DR["Adjustment"].ToString());
                    lblTotalBill.Text = Utility.GetRoundOffValue(DR["TotalAmount"].ToString() == string.Empty ? "0.00" : DR["TotalAmount"].ToString());
                    lblArrears.Text = Utility.GetRoundOffValue(DR["Arrears"].ToString() == string.Empty ? "0.00" : DR["Arrears"].ToString());
                    if (lblArrears.Text == "")
                        lblArrears.Text = Utility.GetRoundOffValue("0");

                    lblAdvancePaid.Text = Utility.GetRoundOffValue(DR["LastBalance"].ToString());
                    if (lblAdvancePaid.Text == "")
                        lblAdvancePaid.Text = Utility.GetRoundOffValue("0");
                    lblAdvancePaid.Text = lblAdvancePaid.Text.Replace("-", "");

                    #region Calculate dynamic payable, after due date
                    //if (Convert.ToDouble(lblAdvancePaid.Text) >= Convert.ToDouble(DR["TotalAmount"].ToString()))
                    //{
                    //    lblPayablebeforeDueDate.Text = Utility.GetRoundOffValue("0");
                    //    lblPayableAfterDueDate.Text = Utility.GetRoundOffValue("0");
                    //}
                    //else
                    //{

                    //    string Payable = Convert.ToString(Convert.ToDouble(DR["TotalAmount"].ToString()) - Convert.ToDouble(lblAdvancePaid.Text));

                    //    if (Convert.ToDouble(lblAdvancePaid.Text) != 0)
                    //    {


                    //        if (Convert.ToDouble(Payable) > 0)
                    //        {
                    //            double SurchargeAmount = bllEWC.EC_GetCalculateSurcharge(Convert.ToDouble(Payable), ServiceType);
                    //            lblPayablebeforeDueDate.Text = Utility.GetRoundOffValue(Payable);
                    //            lblPayableAfterDueDate.Text = Utility.GetRoundOffValue(Convert.ToDouble(lblPayablebeforeDueDate.Text) + SurchargeAmount);
                    //        }
                    //        else
                    //        {
                    //            lblPayablebeforeDueDate.Text = Utility.GetRoundOffValue("0");
                    //            lblPayableAfterDueDate.Text = Utility.GetRoundOffValue("0");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblPayablebeforeDueDate.Text = Utility.GetRoundOffValue(DR["BillAmount"].ToString());
                    //        lblPayableAfterDueDate.Text = Utility.GetRoundOffValue(DR["BillAmountAfterDueDate"].ToString());

                    //    }
                    //}
                    #endregion
                    lblPayablebeforeDueDate.Text = Utility.GetRoundOffValue(DR["BillAmount"].ToString());
                    lblSrchg.Text = Utility.GetRoundOffValue(DR["SS"].ToString() == string.Empty ? "0.00" : DR["SS"].ToString());
                    lblPayableAfterDueDate.Text = Utility.GetRoundOffValue(DR["BillAmountAfterDueDate"].ToString());

                    lblBillNo.Text = DR["BillNo"].ToString();

                    lblBillIssueDate.Text = DR["BillIssueDate"].ToString();
                    lblBillDueDate.Text = DR["BillDueDate"].ToString();
                    lblSanctionedDischarge.Text = Utility.GetRoundOffValue(Convert.ToDouble(DR["SanctionedDischarge"].ToString()));
                    lblRate.Text = Utility.GetRoundOffValue(DR["Surcharge"].ToString());
                    LastYearBillingDays = DR["LastYearBillingDays"].ToString();
                    lblBillStatus.Text = DR["Status"].ToString().ToLower();
                    #region
                    //if (Convert.ToInt32(LastYearBillingDays) > 0)
                    //{
                    //   // double PWaterRate = bllEWC.WaterRate(ServiceType, 1, GetFYstartDate());
                    //   double LastSnctdDschrgSuply = bllEWC.EC_GetLastSnctdDschrgSuply(Convert.ToInt64(hdnIndustryID.Value), ServiceType, 0, 1, GetFYstartDate());

                    //   // lblRate.Text = Utility.GetRoundOffValue(Convert.ToString(Convert.ToDouble(lblRate.Text) + PWaterRate));
                    //    lblCharges.Text = Utility.GetRoundOffValue(Convert.ToString(Convert.ToDouble(lblCharges.Text) + LastSnctdDschrgSuply));
                    //   // DivPTotal.Visible = true;
                    //   // lblPtotalBill.Text = Utility.GetRoundOffValue(DR["PTotalAmount"].ToString());

                    //}
                    #endregion
                    gv.DataBind();

                }
                else
                {
                    pnlWaterBilling.Visible = false;
                    pnlBilldetail.Visible = false;
                    Master.ShowMessage("No record found.", SiteMaster.MessageType.Error);
                    return;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private DateTime GetFYstartDate()
        {
            DateTime nowDate = DateTime.Now;
            string strDate = "01-Jul-";
            if (nowDate.Month >= 1 && nowDate.Month <= 6)
                strDate = strDate + (DateTime.Today.Year - 1);
            else
                strDate = strDate + DateTime.Today.Year;

            return Convert.ToDateTime(strDate);
        }
        private void ClearControls()
        {
            //lblIndustryName.Text = "";
            //lblIndustryNo.Text = "";
            //lblDivision.Text = "";
            lblCharges.Text = "";

            lblApplicableTax.Text = "";
            lblAdjustment.Text = "";
            lblTotalBill.Text = "";
            lblArrears.Text = "";

            lblAdvancePaid.Text = "";
            lblPayablebeforeDueDate.Text = "";
            lblPayableAfterDueDate.Text = "";
            lblBillNo.Text = "";

            lblBillIssueDate.Text = "";
            lblBillDueDate.Text = "";
            lblSanctionedDischarge.Text = "";
            lblRate.Text = "";
            lblPtotalBill.Text = "";
        }
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv.PageIndex = e.NewPageIndex;
                GetBillDetail();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv.EditIndex = -1;
                GetBillDetail();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void rdServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbEffluent.Checked)
                rbCanal.Checked = false;
            if (rbCanal.Checked)
                rbEffluent.Checked = false;
            GetBillDetail();
        }

        protected void ddlfinancialyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBillDetail();
        }
    }
}