
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
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
using Microsoft.Reporting.WebForms;

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class FinalizePrintBill : BasePage
    {
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;
        private Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    //long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    //if (userID > 0 && boundryLvlID != null) // Irrigation Staff
                    //{
                    //    LoadAllRegionDDByUser(userID, boundryLvlID);
                    //}
                    //else
                    LoadDDL(ZONE, 0);
                    LoadIndustryTypeDDL();
                    LoadFinancialYearDDL();
                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        SetControlsValues();
                        LoadGrid();
                    }

                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void DisableDD(DropDownList DDL)
        {
            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("All", ""));
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList sndr = (DropDownList)sender;
            string strValue = sndr.SelectedItem.Value;
            //if (string.IsNullOrEmpty(strValue))
            //    return;

            if (sndr.ID.Equals("ddlZone"))
            {
                DisableDD(ddlCircle);
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlCircle"))
            {
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(DIVISION, Convert.ToInt64(strValue));
            }

            SaveSearchCriteriaInSession();

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadAllRegionDDByUser(long _UserID, long? _BoundryLevelID)
        {
            try
            {
                if (_BoundryLevelID == null)
                    return;
                int selectOption = (int)Constants.DropDownFirstOption.All;
                List<object> lstData = new WaterLossesBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_BoundryLevelID));
                List<object> lstChild = (List<object>)lstData.ElementAt(0);
                lstChild = (List<object>)lstData.ElementAt(1);
                if (lstChild.Count > 0) // Division
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    Dropdownlist.DDLLoading(ddlDiv, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                    {
                        ddlDiv.SelectedIndex = 1;
                    }
                }

                lstChild = (List<object>)lstData.ElementAt(2);
                if (lstChild.Count > 0) // Circle
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    Dropdownlist.DDLLoading(ddlCircle, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                        ddlCircle.SelectedIndex = 1;
                }

                lstChild = (List<object>)lstData.ElementAt(3);
                if (lstChild.Count > 0) // Zone
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    Dropdownlist.DDLLoading(ddlZone, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                        ddlZone.SelectedIndex = 1;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadDDL(int _DDLType, long _SearchID)
        {
            WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
            List<object> lstData = new List<object>();
            DropDownList ddlToLoad = null;

            switch (_DDLType)
            {
                case ZONE: // Zone
                    lstData = bll_waterLosses.GetAllZones();
                    ddlToLoad = ddlZone;
                    break;

                case CIRCLE: // Circle
                    lstData = bll_waterLosses.GetCirclesByZoneID(_SearchID);
                    ddlToLoad = ddlCircle;
                    break;

                case DIVISION: // Division
                    lstData = bll_waterLosses.GetDivisionsByCircleID(_SearchID);
                    ddlToLoad = ddlDiv;
                    break;

                default:
                    break;
            }
            if (lstData.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
                ddlToLoad.Enabled = true;
            }
            if (lstData.Count == 1)
            {
                ddlToLoad.SelectedIndex = 1;
                ddlToLoad.Items.RemoveAt(0);
            }
        }
        private void LoadIndustryTypeDDL()
        {
            List<object> lstIndustryType = bllEWC.IndustryType_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.All, lstIndustryType);
        }
        private void LoadFinancialYearDDL()
        {
            List<object> lstFinancialYear = bllEWC.Financial_GetList().Select(x => new { ID = x.FinancialYear, Name = x.FinancialYear }).Distinct().ToList<object>();
            Dropdownlist.DDLLoading(ddlfinancialyear, false, (int)Constants.DropDownFirstOption.NoOption, lstFinancialYear);
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                // SaveSearchCriteriaInSession();
                LoadGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void SetControlsValues()
        {
            object currentObj = Session["CurrentControlsValues"] as object;
            if (currentObj != null)
            {
                rdServiceType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ServiceType").GetValue(currentObj));
                ddlfinancialyear.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("financialyear").GetValue(currentObj));
                ddlType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("Type").GetValue(currentObj));
                ddlZone.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj));
                if (!string.IsNullOrWhiteSpace(ddlZone.SelectedValue))
                    LoadDDL(2, Convert.ToInt64(ddlZone.SelectedValue));
                ddlCircle.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj));
                if (!string.IsNullOrWhiteSpace(ddlCircle.SelectedValue))
                    LoadDDL(3, Convert.ToInt64(ddlCircle.SelectedValue));
                ddlDiv.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                txtIndName.Text = Convert.ToString(currentObj.GetType().GetProperty("IndName").GetValue(currentObj));
                txtIndNo.Text = Convert.ToString(currentObj.GetType().GetProperty("IndNo").GetValue(currentObj));
                LoadGrid();
            }

        }
        protected void LoadGrid()
        {
            try
            {

                SaveSearchCriteriaInSession();
                Hashtable SearchCriteria = new Hashtable();
                string SelectedServiceType = null;
                string SelectedFinancialYear = null;
                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                long? SelectedIndustryType = null;

                if (ddlZone.SelectedItem.Value != String.Empty)
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }

                if (ddlCircle.SelectedItem.Value != String.Empty)
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }

                if (ddlDiv.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDiv.SelectedItem.Value);
                }
                if (ddlType.SelectedItem.Value != String.Empty)
                {
                    SelectedIndustryType = Convert.ToInt64(ddlType.SelectedItem.Value);
                }
                if (ddlfinancialyear.SelectedItem.Value != String.Empty)
                {
                    SelectedFinancialYear = Convert.ToString(ddlfinancialyear.SelectedItem.Value);
                }

                if (rdServiceType.SelectedItem.Value == "1")
                {
                    SelectedServiceType = "EFFLUENT";
                }
                else
                {
                    SelectedServiceType = "CANAL";
                }
                string indName = null, indNo = null;
                if (txtIndName.Text.ToString().Trim() != string.Empty)
                    indName = txtIndName.Text.ToString().Trim();
                if (txtIndNo.Text.ToString().Trim() != string.Empty)
                    indNo = txtIndNo.Text.ToString().Trim();

                //GetFinalizeBillPrint(string _ServiceType, string _FinancialYear, long? _IndustryTypeID, long? _ZoneID, long? _CircleID, long? _DivisionID)
                IEnumerable<DataRow> IeBill = new Effluent_WaterChargesBLL().GetFinalizeBillPrint(SelectedServiceType, SelectedFinancialYear, SelectedIndustryType, SelectedZoneID, SelectedCircleID, SelectedDivisionID, indName, indNo);
                var LstBill = IeBill.Select(dataRow => new
                {
                    IndustryID = dataRow.Field<long>("IndustryID"),
                    BillID = dataRow.Field<long>("BillID"),
                    IndustryName = dataRow.Field<string>("IndustryName"),
                    Division = dataRow.Field<string>("Division"),
                    BillNo = dataRow.Field<string>("BillNo"),
                    Surcharge = dataRow.Field<double>("Surcharge"),
                    ApplicableTax = dataRow.Field<double>("ApplicableTax"),
                    Arrears = dataRow.Field<double>("Arrears"),
                    Adjustment = dataRow.Field<double>("Adjustment"),

                    LastBalance = dataRow.Field<double>("LastBalance"),
                    BillAmount = dataRow.Field<double>("BillAmount"),
                    BillAmountAfterDueDate = dataRow.Field<double>("BillAmountAfterDueDate"),
                    TotalAmount = dataRow.Field<double>("TotalAmount"),
                    Status = dataRow.Field<string>("Status"),
                    ServiceType = dataRow.Field<string>("ServiceType"),
                    SurchargeRate = dataRow.Field<double>("SurchargeRate"),
                }).ToList();
                gv.DataSource = LstBill;
                gv.DataBind();
                gv.Visible = true;
                if (gv.Rows.Count > 0)
                {
                    finalizebillDiv.Visible = true;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void SaveSearchCriteriaInSession()
        {
            Session["CurrentControlsValues"] = null;
            object obj = new
            {
                ServiceType = rdServiceType.SelectedItem.Value,
                financialyear = ddlfinancialyear.SelectedItem.Value,
                ZoneID = ddlZone.SelectedItem.Value,
                CircleID = ddlCircle.SelectedItem.Value,
                DivisionID = ddlDiv.SelectedItem.Value,
                Type = ddlType.SelectedItem.Value,
                IndName = txtIndName.Text.ToString(),
                IndNo = txtIndNo.Text.ToString(),
            };
            Session["CurrentControlsValues"] = obj;
        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv.PageIndex = e.NewPageIndex;
                LoadGrid();
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
                LoadGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Adjustment"))
            {

                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gv.DataKeys[row.RowIndex];
                hdnIndustryID.Value = Convert.ToString(key["IndustryID"]);
                hdnBillID.Value = Convert.ToString(key["BillID"]);
                divAdd.Visible = true;
                btnSave.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);


            }
            if (e.CommandName.Equals("FinalizeBill"))
            {

                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gv.DataKeys[row.RowIndex];
                hdnIndustryID.Value = Convert.ToString(key["IndustryID"]);
                hdnBillID.Value = Convert.ToString(key["BillID"]);

                bllEWC.FinalizeIndustryBills(Convert.ToString(hdnBillID.Value), Convert.ToString(key["ServiceType"]), (long)SessionManagerFacade.UserInformation.ID);
                LoadGrid();
                Master.ShowMessage("Bill has been finalized", SiteMaster.MessageType.Success);

            }
            if (e.CommandName.Equals("Print"))
            {
                string servicetype = "";
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gv.DataKeys[row.RowIndex];
                servicetype = Convert.ToString(key["ServiceType"]);
                ReportData mdlReportData = new ReportData();
                ReportParameter ReportParameter = new ReportParameter("BillID", Convert.ToString(key["BillID"]));
                mdlReportData.Parameters.Add(ReportParameter);
                if (servicetype == "EFFLUENT")
                {
                    mdlReportData.Name = Constants.EffluentWaterBill;
                }
                else
                {
                    mdlReportData.Name = Constants.CanalWaterBill;
                }

                // Set the ReportData in Session with specific Key
                Session[SessionValues.ReportData] = mdlReportData;
                string ReportViwerurl = "../" + Constants.ReportsUrl;
                // Open the report printable in new tab
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
            }
            if (e.CommandName.Equals("BillingDetail"))
            {
                Session["IndustryName"] = "";
                Session["EWDivision"] = "";
                Session["CWDivision"] = "";
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                DataKey key = gv.DataKeys[row.RowIndex];
                Session["IndustryName"] = Convert.ToString(key["IndustryName"]);
                Session["EWDivision"] = Convert.ToString(key["Division"]);
                Response.Redirect("~/Modules/EWC/BillDetail.aspx?finalize=true&ID=" + Convert.ToString(key["IndustryID"]) + "&BillID=" + Convert.ToString(key["BillID"]) + "&ServiceType=" + Convert.ToString(key["ServiceType"]) + "&Year=" + (ddlfinancialyear.SelectedItem == null ? "" : ddlfinancialyear.SelectedItem.Value), false); //2017-2018", false); //+ ddlfinancialyear.SelectedItem == null ? "" : ddlfinancialyear.SelectedItem.Value, false);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool adjustmenttype = false;
            double Amount;
            if (ddlAdjustment1.SelectedItem.Text == "+")
            {
                adjustmenttype = true;
            }
            else
            {
                adjustmenttype = false;
            }
            if (ddlAdjustment2.SelectedItem.Value == "2")
            {
                Amount = (bllEWC.BillingFactorSum("Charges", Convert.ToInt64(hdnBillID.Value)) * Convert.ToDouble(txtAdjustment.Text)) / 100;
            }
            else
            {
                Amount = Convert.ToDouble(txtAdjustment.Text);
            }

            bllEWC.AddAdjustmentFinalizeBill(Convert.ToInt64(hdnBillID.Value), Convert.ToDouble(txtAdjustment.Text), adjustmenttype, Convert.ToInt32(ddlAdjustment2.SelectedValue), Amount, txtReason.Text, SessionManagerFacade.UserInformation.ID);
            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            ResetControlAfterSave();
            LoadGrid();
        }
        private void ResetControlAfterSave()
        {

            hdnIndustryID.Value = "0";
            hdnBillID.Value = "0";
            txtAdjustment.Text = "";
            txtReason.Text = "";
            ddlAdjustment2.ClearSelection();

        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int count = 0;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    count++;
                    DataKey key = gv.DataKeys[e.Row.RowIndex];
                    string Status = Convert.ToString(key.Values["Status"]);
                    Button btnAdjustment = (Button)e.Row.FindControl("btnAdjustment");
                    Button btnFinalizeBill = (Button)e.Row.FindControl("btnFinalizeBill");
                    Button btnPrint = (Button)e.Row.FindControl("btnPrint");
                    if (Status == "FINALIZED")
                    {
                        btnAdjustment.Enabled = false;
                        btnFinalizeBill.Enabled = false;
                        if (gv.Rows.Count != count)
                        {
                            btnfinalizebill.Enabled = false;
                            //
                            btnPrint.Enabled = false;
                        }

                    }
                    else
                    {
                        btnfinalizebill.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnfinalizebill_Click(object sender, EventArgs e)
        {
            try
            {
                string ServiceType = "";
                string BillIDs = "0";
                for (int m = 0; m < gv.Rows.Count; m++)
                {
                    string status = gv.DataKeys[m].Values["Status"].ToString();
                    if (status != "FINALIZED")
                    {
                        CheckBox cb = (CheckBox)gv.Rows[m].FindControl("cb");
                        if (cb != null && cb.Checked)
                            BillIDs = gv.DataKeys[m].Values["BillID"].ToString() + ',' + BillIDs;
                        //if (BillIDs == "")
                        //{
                        //    BillIDs = BillIDs + gv.DataKeys[m].Values["BillID"].ToString();

                        //}
                        //else
                        //{

                        //    BillIDs = BillIDs + "," + gv.DataKeys[m].Values["BillID"].ToString();

                        //}
                    }


                }
                if (rdServiceType.SelectedItem.Value == "1")
                {
                    ServiceType = "EFFLUENT";
                }
                else
                {
                    ServiceType = "CANAL";
                }
                bllEWC.FinalizeIndustryBills(BillIDs, ServiceType, (long)SessionManagerFacade.UserInformation.ID);
                LoadGrid();
                Master.ShowMessage("All Bills have been finalized", SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rdServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv.Visible = false;
            finalizebillDiv.Visible = false;
        }

        protected void btnAllprint_Click(object sender, EventArgs e)
        {
            try
            {

                string BillIDs = "";
                for (int m = 0; m < gv.Rows.Count; m++)
                {
                    string status = gv.DataKeys[m].Values["Status"].ToString();

                    //  BillIDs = gv.DataKeys[m].Values["BillID"].ToString() + ',' + BillIDs;
                    if (BillIDs == "")
                    {
                        BillIDs = BillIDs + gv.DataKeys[m].Values["BillID"].ToString();

                    }
                    else
                    {

                        BillIDs = BillIDs + "," + gv.DataKeys[m].Values["BillID"].ToString();

                    }



                }

                ReportData mdlReportData = new ReportData();
                ReportParameter ReportParameter = new ReportParameter("BillID", BillIDs);
                mdlReportData.Parameters.Add(ReportParameter);

                if (rdServiceType.SelectedItem.Value == "1")
                {

                    mdlReportData.Name = Constants.EffluentWaterBill;
                }
                else
                {

                    mdlReportData.Name = Constants.CanalWaterBill;
                }

                // Set the ReportData in Session with specific Key
                Session[SessionValues.ReportData] = mdlReportData;
                string ReportViwerurl = "../" + Constants.ReportsUrl;
                // Open the report printable in new tab
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}