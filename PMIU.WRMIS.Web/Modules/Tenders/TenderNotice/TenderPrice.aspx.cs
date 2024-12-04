using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class TenderPrice : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        #region ViewState Constants

        string TenderPriceList = "TenderPriceList";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    btnSave.Visible = base.CanAdd;
                    UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                    //Page.Form.Attributes.Add("enctype", "multipart/form-data");
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hdnWorkID.Value = Request.QueryString["TenderWorkID"];
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnWorkID.Value));
                        hlBackToWork.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        hdnTenderNoticeID.Value = Convert.ToString(TenderNoticeID);

                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value);
                        BindTenderPriceDepositGridData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        //  BindTenderPriceDepositGridData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        BindCallDepositData(Convert.ToInt64(hdnWorkID.Value), 0);
                        BindDropDown();
                        txtValue.Enabled = false;
                        if (base.CanView)
                            anchCommittee.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchContractor.HRef = string.Format("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));


                        if (mdlUsers.DesignationID == (long)Constants.Designation.XEN || mdlUsers.DesignationID == (long)Constants.Designation.CE || mdlUsers.DesignationID == (long)Constants.Designation.SE)
                        {
                            anchReport.Disabled = true;
                            anchReport.Attributes.Add("style", "color:black");
                        }

                        else
                        {
                            anchReport.Disabled = false;
                            anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        }

                        //if (base.CanView)
                        //    anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchstatement.HRef = string.Format("~/Modules/Tenders/TenderNotice/ViewComparativeStatement.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchTenderPrice.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));

                        bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnWorkID.Value));
                        //    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                        if (IsAwarded == true)
                        {
                            btnSave.Visible = false;
                            ddlEstimate.Enabled = false;
                            txtValue.Enabled = false;
                        }
                        //else if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                        //{
                        //    btnSave.Visible = false;
                        //}
                        //else
                        //{
                        //    btnSave.Visible = true;
                        //}


                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropDown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            Dropdownlist.BindDropdownlist<List<object>>(ddlEstimate, CommonLists.GetTSType());
            Dropdownlist.BindDropdownlist<List<object>>(ddlCompanyName, new TenderManagementBLL().GetCompanyNameByWorkID(Convert.ToInt64(hdnWorkID.Value)));
        }

        private void BindTenderPriceDepositGridData(long _WorkID, long _WorkSourceID)
        {
            try
            {
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _WorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");

                List<object> lstTenderWorkData = new TenderManagementBLL().GetWorkItemsforViewByWorKID(_WorkID);
                gvTenderPrice.DataSource = lstTenderWorkData;
                gvTenderPrice.DataBind();


                List<object> lstCallDepositData = new TenderManagementBLL().GetCallDepositDataforViewByWorKID(_WorkID);
                if (lstCallDepositData.Count > 0)
                {
                    List<TenderPriceModel> lstTender = new List<TenderPriceModel>();
                    foreach (var item in lstCallDepositData)
                    {
                        TenderPriceModel MDLtENDER = new TenderPriceModel();
                        MDLtENDER.ID = Convert.ToInt32(Utility.GetDynamicPropertyValue(item, "ID"));
                        MDLtENDER.TenerWContractorID = Convert.ToInt32(Utility.GetDynamicPropertyValue(item, "TenerWContractorID"));
                        MDLtENDER.CDRNO = Utility.GetDynamicPropertyValue(item, "CDRNO");
                        MDLtENDER.BankDetail = Utility.GetDynamicPropertyValue(item, "BankDetail");
                        MDLtENDER.Attachment = Utility.GetDynamicPropertyValue(item, "Attachment");
                        MDLtENDER.Amount = Utility.GetDynamicPropertyValue(item, "Amount");
                        lstTender.Add(MDLtENDER);



                    }
                    ViewState[TenderPriceList] = lstTender;
                }
                else
                {
                    ViewState[TenderPriceList] = new List<TenderPriceModel>();
                }
                gvCallDepositDetail.DataSource = lstCallDepositData;
                gvCallDepositDetail.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindTenderPriceData(long _WorkID, long _WorkSourceID, long _CompanyID)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _WorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");

                List<object> lstTenderWorkData = new TenderManagementBLL().GetWorkItemsDetailsByWorKID(_WorkID, _CompanyID, Convert.ToInt64(hdnTenderNoticeID.Value));

                if (lstTenderWorkData.Count > 0)
                {
                    string Value = Utility.GetDynamicPropertyValue(lstTenderWorkData[0], "EstimateType");
                    string EstimatePercentage = Utility.GetDynamicPropertyValue(lstTenderWorkData[0], "EstimatePercentage");
                    lblTotal.Text = Utility.GetDynamicPropertyValue(lstTenderWorkData[0], "TotalItemAmount");
                    if (Value != "")
                        //ddlEstimate.SelectedItem.Text = Value;
                        ddlEstimate.ClearSelection();
                    //    ddlEstimate.Items.FindByText(Value).Selected = true;
                    Dropdownlist.SetSelectedText(ddlEstimate, Value);
                    if (EstimatePercentage != "")
                    {

                        if (Value.Replace(" ", "").Trim().ToUpper() == "ITEMRATE")
                        {
                            txtValue.Text = "";
                        }
                        else
                        {
                            txtValue.Text = EstimatePercentage;
                        }
                    }
                    if (mdlUser.DesignationID == (long)Constants.Designation.ChiefMonitoring && (Value.ToUpper().Trim() == "Above T.S" || Value == "Below T.S"))
                        txtValue.Enabled = true;
                    else
                        txtValue.Enabled = false;
                    //for (int i = 0; i < lstTenderWorkData.Count; i++)
                    //{
                    //    string Value = Utility.GetDynamicPropertyValue(lstTenderWorkData[i], "EstimateType");
                    //    string EstimatePercentage = Utility.GetDynamicPropertyValue(lstTenderWorkData[i], "EstimatePercentage");
                    //    if (Value != "")
                    //        //ddlEstimate.SelectedItem.Text = Value;
                    //        ddlEstimate.Items.FindByText(Value).Selected = true;
                    //    if (EstimatePercentage != "")
                    //        txtValue.Text = EstimatePercentage;
                    //}
                }
                if (lstTenderWorkData.Count == 0)
                {
                    BindTenderPriceDepositGridData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                    ddlEstimate.SelectedIndex = -1;
                    txtValue.Text = "";
                    lblTotal.Text = "";
                }

                if (lstTenderWorkData.Count > 0)
                {
                    gvTenderPrice.DataSource = lstTenderWorkData;
                    gvTenderPrice.DataBind();
                }

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvTenderPrice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtContractorRate = (TextBox)e.Row.FindControl("txtContractorRate");
                    Label lblEstimateType = (Label)e.Row.FindControl("lblEstimateType");

                    TextBox txtSancQuan = (TextBox)e.Row.FindControl("txtSancQuan");
                    TextBox txtTSRate = (TextBox)e.Row.FindControl("txtTSRate");
                    TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");


                    txtTSRate.Text = string.Format("{0:#,##0.00}", double.Parse(txtTSRate.Text));

                    if (txtContractorRate.Text != "")
                        txtContractorRate.Text = string.Format("{0:#,##0.00}", double.Parse(txtContractorRate.Text));

                    txtSancQuan.Text = txtSancQuan.Text; //string.Format("{0:#,##0.00}", double.Parse(txtSancQuan.Text));
                    if (txtAmount.Text != "")
                        txtAmount.Text = string.Format("{0:#,##0.00}", double.Parse(txtAmount.Text));

                    //txtContractorRate.Attributes.Add("OnTextChanged", "Calculate_AmountG");

                    //if (txtContractorRate.Text == "")
                    //{
                    //    txtContractorRate.Attributes.Add("disabled", "disabled");
                    //}

                    if (txtContractorRate.Text != "" && mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                    {
                        btnSave.Visible = false;
                    }

                    bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnWorkID.Value));
                    //    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    if (IsAwarded != true && txtContractorRate.Text == "")
                    {
                        btnSave.Visible = true;
                    }

                    //else
                    //{
                    //    btnSave.Visible = true;
                    //}

                    //if (txtContractorRate.Text != "")
                    //{
                    //    btnSave.Visible = false;  
                    //}


                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void ddlEstimate_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            string value = ddlEstimate.SelectedItem.Value;
            if (value == "1")
            {
                lblTotal.Text = "";
                txtValue.Enabled = false;
                txtValue.Text = "";
                txtValue.CssClass = "form-control Percentage";
                txtValue.Attributes.Remove("required");
                double TotalAsPerTS = 0.0;
                for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                {
                    //   Label SRate = (Label)gvTenderPrice.Rows[i].FindControl("lblTSRate");
                    TextBox SRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtTSRate");
                    TextBox SQuantity = (TextBox)gvTenderPrice.Rows[i].FindControl("txtSancQuan");
                    TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                    TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                    ContractorRate.Text = SRate.Text;
                    //   Amount.Text = Convert.ToString(Convert.ToInt32(SRate.Text.Replace(",", "")) * Convert.ToInt32(SQuantity.Text.Replace(",", "")));
                    Amount.Text = Convert.ToString(Convert.ToDouble(SRate.Text) * Convert.ToDouble(SQuantity.Text));

                    TotalAsPerTS = TotalAsPerTS + Convert.ToDouble(Amount.Text);

                    ContractorRate.Text = string.Format("{0:#,##0.00}", double.Parse(ContractorRate.Text));
                    Amount.Text = string.Format("{0:#,##0.00}", double.Parse(Amount.Text));
                    ContractorRate.Enabled = false;

                }
                lblTotal.Text = Convert.ToString(TotalAsPerTS);
                lblTotal.Text = string.Format("{0:#,##0.00}", double.Parse(lblTotal.Text));

            }
            else if (value == "2" || value == "3")
            {
                lblTotal.Text = "";
                for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                {
                    TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                    TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                    ContractorRate.Text = "";
                    Amount.Text = "";
                    ContractorRate.Enabled = false;
                }
                txtValue.Text = "";
                txtValue.Enabled = true;
                txtValue.CssClass = "form-control Percentage required";
                txtValue.Attributes.Add("required", "required");
                //foreach (GridViewRow gvrow in gvTenderPrice.Rows)
                //{
                //    TextBox txtContractorRate = (TextBox)(gvrow.FindControl("txtContractorRate"));
                //    txtContractorRate.Attributes.Add("disabled", "disabled");
                //}
            }
            else if (value == "4")
            {
                lblTotal.Text = "";
                for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                {
                    TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                    TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                    ContractorRate.Text = "";
                    Amount.Text = "";
                    ContractorRate.Enabled = true;
                    ContractorRate.Attributes.Add("required", "required");
                }
                txtValue.Enabled = false;
                txtValue.Text = "";
                txtValue.CssClass = "form-control Percentage";
                txtValue.Attributes.Remove("required");
            }
            //else if (value == "4")
            //{
            //    foreach (GridViewRow gvrow in gvTenderPrice.Rows)
            //    {
            //        TextBox txtContractorRate = (TextBox)(gvrow.FindControl("txtContractorRate"));
            //        txtContractorRate.Attributes.Remove("disabled");
            //    }

            //}
            //else
            //{
            //    txtValue.Enabled = false;
            //    foreach (GridViewRow gvrow in gvTenderPrice.Rows)
            //    {
            //        TextBox txtContractorRate = (TextBox)(gvrow.FindControl("txtContractorRate"));
            //        txtContractorRate.Attributes.Add("disabled", "disabled");
            //    }
            //}
            //BindTenderPriceData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
        }

        protected void Calculate_Amount(object sender, EventArgs e)
        {
            try
            {
                if (txtValue.Text != "")
                {
                    double NegValue = Convert.ToDouble(txtValue.Text);
                    if (NegValue < 0)
                    {
                        Master.ShowMessage("Negative value not allowed", SiteMaster.MessageType.Error);
                        return;
                    }
                    string value = ddlEstimate.SelectedItem.Value;
                    if (value == "2")
                    {
                        double TotalAbove = 0.0;
                        for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                        {
                            //Label SRate = (Label)gvTenderPrice.Rows[i].FindControl("lblTSRate");
                            TextBox SRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtTSRate");
                            var AbvPer = (Convert.ToDouble(SRate.Text.Replace(",", "")) / 100) * Convert.ToDouble(txtValue.Text);
                            TextBox SQuantity = (TextBox)gvTenderPrice.Rows[i].FindControl("txtSancQuan");
                            TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                            TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                            var NRate = Convert.ToDouble(SRate.Text.Replace(",", "")) + AbvPer;
                            ContractorRate.Text = Convert.ToString(NRate);
                            //       Amount.Text = Convert.ToString(NRate * Convert.ToInt32(SQuantity.Text.Replace(",", "")));
                            Amount.Text = Convert.ToString(NRate * Convert.ToDouble(SQuantity.Text));
                            TotalAbove = TotalAbove + Convert.ToDouble(Amount.Text);
                            ContractorRate.Text = string.Format("{0:#,##0.00}", double.Parse(ContractorRate.Text));
                            Amount.Text = string.Format("{0:#,##0.00}", double.Parse(Amount.Text));
                        }
                        lblTotal.Text = Convert.ToString(TotalAbove);
                        lblTotal.Text = string.Format("{0:#,##0.00}", double.Parse(lblTotal.Text));

                    }
                    else if (value == "3")
                    {
                        double TotalBelow = 0.0;
                        for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                        {
                            //  Label SRate = (Label)gvTenderPrice.Rows[i].FindControl("lblTSRate");
                            TextBox SRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtTSRate");
                            var AbvPer = (Convert.ToDouble(SRate.Text.Replace(",", "")) / 100) * Convert.ToDouble(txtValue.Text);
                            TextBox SQuantity = (TextBox)gvTenderPrice.Rows[i].FindControl("txtSancQuan");
                            TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                            TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                            var NRate = Convert.ToDouble(SRate.Text.Replace(",", "")) - AbvPer;
                            ContractorRate.Text = Convert.ToString(NRate);
                            // Amount.Text = Convert.ToString(NRate * Convert.ToInt32(SQuantity.Text.Replace(",", "")));
                            Amount.Text = Convert.ToString(NRate * Convert.ToDouble(SQuantity.Text));
                            TotalBelow = TotalBelow + Convert.ToDouble(Amount.Text);
                            ContractorRate.Text = string.Format("{0:#,##0.00}", double.Parse(ContractorRate.Text));
                            Amount.Text = string.Format("{0:#,##0.00}", double.Parse(Amount.Text));
                        }
                        lblTotal.Text = Convert.ToString(TotalBelow);
                        lblTotal.Text = string.Format("{0:#,##0.00}", double.Parse(lblTotal.Text));

                    }
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void Calculate_AmountG(object sender, EventArgs e)
        {
            try
            {
                double TotalItemRate = 0.0;
                for (int i = 0; i < gvTenderPrice.Rows.Count; i++)
                {
                    TextBox SQuantity = (TextBox)gvTenderPrice.Rows[i].FindControl("txtSancQuan");
                    TextBox ContractorRate = (TextBox)gvTenderPrice.Rows[i].FindControl("txtContractorRate");
                    TextBox Amount = (TextBox)gvTenderPrice.Rows[i].FindControl("txtAmount");
                    if (ContractorRate.Text != "")
                    {
                        //   Amount.Text = Convert.ToString(Convert.ToInt32(ContractorRate.Text) * Convert.ToInt32(SQuantity.Text.Replace(",", "")));
                        Amount.Text = Convert.ToString(Convert.ToDouble(ContractorRate.Text) * Convert.ToDouble(SQuantity.Text));
                        TotalItemRate = TotalItemRate + Convert.ToDouble(Amount.Text);
                    }

                }
                lblTotal.Text = Convert.ToString(TotalItemRate);
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void BindCallDepositData(long _WorkID, long _CompanyID)
        {
            try
            {
                List<TenderPriceModel> lstTender = new List<TenderPriceModel>();
                List<object> lstCallDepositData = new TenderManagementBLL().GetCallDepositDataByWorKID(_WorkID, _CompanyID);
                if (lstCallDepositData.Count > 0)
                {

                    foreach (var item in lstCallDepositData)
                    {
                        TenderPriceModel MDLtENDER = new TenderPriceModel();
                        MDLtENDER.ID = Convert.ToInt32(Utility.GetDynamicPropertyValue(item, "ID"));
                        MDLtENDER.TenerWContractorID = Convert.ToInt32(Utility.GetDynamicPropertyValue(item, "TenerWContractorID"));
                        MDLtENDER.CDRNO = Utility.GetDynamicPropertyValue(item, "CDRNO");
                        MDLtENDER.BankDetail = Utility.GetDynamicPropertyValue(item, "BankDetail");
                        MDLtENDER.Attachment = Utility.GetDynamicPropertyValue(item, "Attachment");
                        MDLtENDER.Amount = Utility.GetDynamicPropertyValue(item, "Amount");
                        lstTender.Add(MDLtENDER);



                    }
                    ViewState[TenderPriceList] = lstTender;
                }
                else
                {
                    ViewState[TenderPriceList] = new List<TenderPriceModel>();
                }
                gvCallDepositDetail.DataSource = lstTender;
                gvCallDepositDetail.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvCallDepositDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long CompanyID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
                if (e.CommandName == "AddBankDepositGrid")
                {
                    List<object> lstCallDepositData = new TenderManagementBLL().GetCallDepositDataByWorKID(Convert.ToInt64(hdnWorkID.Value), CompanyID);

                    lstCallDepositData.Add(
                    new
                    {
                        ID = 0,
                        CDRNO = string.Empty,
                        BankDetail = string.Empty,
                        Amount = string.Empty,
                        Attachment = string.Empty

                    });

                    gvCallDepositDetail.PageIndex = gvCallDepositDetail.PageCount;
                    gvCallDepositDetail.DataSource = lstCallDepositData;
                    gvCallDepositDetail.DataBind();

                    gvCallDepositDetail.EditIndex = gvCallDepositDetail.Rows.Count - 1;
                    gvCallDepositDetail.DataBind();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "DepositAmount();", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript1", "CalculationItemRate();", true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void AddRow_Grid(object sender, EventArgs e)
        {
            try
            {

                int rowIndex = 0;
                if (ViewState[TenderPriceList] != null)
                {
                    List<TenderPriceModel> lstPriceCallDepositSource = (List<TenderPriceModel>)ViewState[TenderPriceList];
                    List<TenderPriceModel> lstNew = new List<TenderPriceModel>();
                    if (lstPriceCallDepositSource.Count > 0)
                    {
                        for (int i = 0; i < lstPriceCallDepositSource.Count; i++)
                        {
                            Label lblID = (Label)gvCallDepositDetail.Rows[rowIndex].Cells[0].FindControl("ID");
                            TextBox CallDepositNumber = (TextBox)gvCallDepositDetail.Rows[rowIndex].Cells[0].FindControl("txtCDRNo");
                            TextBox BankDetail = (TextBox)gvCallDepositDetail.Rows[rowIndex].Cells[1].FindControl("txtBankDetail");
                            TextBox Amount = (TextBox)gvCallDepositDetail.Rows[rowIndex].Cells[0].FindControl("txtDepositAmount");
                            Label lblAttachment = (Label)gvCallDepositDetail.Rows[rowIndex].Cells[0].FindControl("lblAttachment");
                            if (string.IsNullOrEmpty(lblAttachment.Text))
                            {
                                FileUploadControl FileUploadReceipt = (FileUploadControl)gvCallDepositDetail.Rows[rowIndex].Cells[0].FindControl("FileUploadControl");
                                List<Tuple<string, string, string>> lstNameofFiles = FileUploadReceipt.UploadNow(Configuration.TenderManagement);
                                if (lstNameofFiles.Count > 0)
                                {
                                    lblAttachment.Text = lstNameofFiles[0].Item3;
                                }
                                
                            }
                            //TextBox AdvertisementDate = (TextBox)gvCallDepositDetail.Rows[rowIndex].Cells[1].FindControl("txtAdvertisementDate"); Attachment = AdvertisementDate.Text
                            lstNew.Add(new TenderPriceModel { ID = Convert.ToInt32(lblID.Text), CDRNO = CallDepositNumber.Text, BankDetail = BankDetail.Text, Amount = Amount.Text, Attachment = lblAttachment.Text });
                            rowIndex++;
                        }
                        lstNew.Add(new TenderPriceModel { ID = 0, CDRNO = "", BankDetail = "", Amount = "", Attachment = "" });
                        ViewState[TenderPriceList] = lstNew;
                        gvCallDepositDetail.DataSource = lstNew;
                        gvCallDepositDetail.DataBind();

                    }
                    else
                    {
                        List<TenderPriceModel> lstAdvertisementSourceNew = new List<TenderPriceModel>();
                        lstAdvertisementSourceNew.Insert(0, GetNewTenderPriceCDR());
                        ViewState[TenderPriceList] = lstAdvertisementSourceNew;
                        gvCallDepositDetail.DataSource = lstAdvertisementSourceNew;
                        gvCallDepositDetail.DataBind();
                    }

                }
                else
                {
                    List<TenderPriceModel> lstTenderPrice = (List<TenderPriceModel>)ViewState[TenderPriceList];
                    lstTenderPrice.Insert(lstTenderPrice.Count, GetNewTenderPriceCDR());

                    ViewState[TenderPriceList] = lstTenderPrice;
                    gvCallDepositDetail.DataSource = lstTenderPrice;
                    gvCallDepositDetail.DataBind();
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript3", "DepositAmount();", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript4", "CalculationItemRate();", true);

            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        //protected void gvCallDepositDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow row = gvCallDepositDetail.Rows[e.RowIndex];
        //        Label lblID = (Label)row.FindControl("lblID");
        //        TextBox txtMemberName = (TextBox)row.FindControl("txtMemberName");
        //        TextBox txtDesignation = (TextBox)row.FindControl("txtDesignation");
        //        TextBox txtMemberID = (TextBox)row.FindControl("txtMemberID");

        //        //TM_CommitteeMembers mdlCommitteeMembers = new TM_CommitteeMembers();
        //        //TM_TenderCommitteeMembers mdlTenderCommitteeMembers = new TM_TenderCommitteeMembers();
        //        //if (txtMemberID.Text == "-1")
        //        //{
        //        //    mdlCommitteeMembers.ID = Convert.ToInt32(lblID.Text);
        //        //    mdlCommitteeMembers.Name = txtMemberName.Text;
        //        //    mdlCommitteeMembers.Designation = txtDesignation.Text;
        //        //    mdlCommitteeMembers.IsActive = true;
        //        //    mdlCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
        //        //    mdlCommitteeMembers.CreatedDate = DateTime.Now;
        //        //    long ID = new TenderManagementBLL().SaveCommitteeMember(mdlCommitteeMembers);

        //        //    if (ID > 0 && lblID.Text == "0")
        //        //    {
        //        //        mdlTenderCommitteeMembers.CommitteeMembersID = ID;
        //        //        mdlTenderCommitteeMembers.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
        //        //        mdlTenderCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
        //        //        mdlTenderCommitteeMembers.CreatedDate = DateTime.Now;
        //        //        bool IsSaved = new TenderManagementBLL().SaveTenderCommitteeMember(mdlTenderCommitteeMembers);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    mdlTenderCommitteeMembers.CommitteeMembersID = Convert.ToInt32(txtMemberID.Text);
        //        //    mdlTenderCommitteeMembers.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
        //        //    mdlTenderCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
        //        //    mdlTenderCommitteeMembers.CreatedDate = DateTime.Now;
        //        //    bool IsSaved = new TenderManagementBLL().SaveTenderCommitteeMember(mdlTenderCommitteeMembers);
        //        //}
        //        gvCallDepositDetail.EditIndex = -1;
        //        //BindCallDepositData(Convert.ToInt64(hdnWorkID.Value));
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvCallDepositDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvCallDepositDetail.EditIndex = -1;
        //        //  BindCallDepositData(Convert.ToInt64(hdnWorkID.Value));
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}


        private TenderPriceModel GetNewTenderPriceCDR()
        {
            TenderPriceModel mdlTenderPrice = new TenderPriceModel
            {
                ID = 0,
                CDRNO = string.Empty,
                TenerWContractorID = 0,
                BankDetail = string.Empty,
                Amount = string.Empty,
                Attachment = string.Empty
            };
            return mdlTenderPrice;
        }

        [Serializable]
        public class TenderPriceModel
        {
            public int ID { get; set; }
            public string CDRNO { get; set; }
            public long TenerWContractorID { get; set; }
            public string BankDetail { get; set; }
            public string Amount { get; set; }
            public string Attachment { get; set; }

        }

        protected void gvCallDepositDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (ViewState[TenderPriceList] != null)
                {
                    List<TenderPriceModel> lstAdvertisementSource = (List<TenderPriceModel>)ViewState[TenderPriceList];
                    if (lstAdvertisementSource.Count > 0)
                    {

                        lstAdvertisementSource.RemoveAt(e.RowIndex);

                        //  gvCallDepositDetail.DataSource = new TenderManagementBLL().DeleteCallDepositByTenderWorkContractorID(Convert.ToInt64(lstAdvertisementSource.ElementAtOrDefault(e.RowIndex).ID));
                        ViewState[TenderPriceList] = lstAdvertisementSource;
                        gvCallDepositDetail.DataSource = lstAdvertisementSource;
                        gvCallDepositDetail.DataBind();

                    }
                }



            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvCallDepositDetail.Rows.Count == 0)
                {
                    Master.ShowMessage(Message.TM_EnterDCR.Description, SiteMaster.MessageType.Error);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Calculation();", true);
                    return;
                }
                TenderManagementBLL TenderMgmtBLL = new TenderManagementBLL();
                List<TM_TenderWorksContractors> lstTenderWorkContractor = new List<TM_TenderWorksContractors>();

                //List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);
                //TenderNoticeModel.TenderNoticeFile = lstNameofFiles[0].Item3;
                List<TM_TenderPrice> lstNew = new List<TM_TenderPrice>();
                List<TM_TenderPriceCDR> lstTenderPriceCDR = new List<TM_TenderPriceCDR>();

                foreach (GridViewRow gvrow in gvTenderPrice.Rows)
                {
                    TM_TenderPrice mdlTenderPrice = new TM_TenderPrice();
                    TM_TenderWorksContractors mdlTenderWorkContractor = new TM_TenderWorksContractors();
                    TextBox txtContractorRate = (TextBox)gvrow.FindControl("txtContractorRate");
                    //Label ContractorID = (Label)gvrow.FindControl("lblContractorID");
                    long ContractorID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
                    Label ID = (Label)gvrow.FindControl("ID");
                    Label TPID = (Label)gvrow.FindControl("lblTPID");
                    TextBox txtSancQuan = (TextBox)gvrow.FindControl("txtSancQuan");
                    // Label lblTSRate = (Label)gvrow.FindControl("lblTSRate");
                    TextBox lblTSRate = (TextBox)gvrow.FindControl("txtTSRate");

                    mdlTenderPrice.WorkItemID = Convert.ToInt64(ID.Text);
                    mdlTenderPrice.TWContractorID = ContractorID;
                    if (TPID.Text != "")
                        mdlTenderPrice.ID = Convert.ToInt64(TPID.Text);

                    mdlTenderPrice.ContractorRate = Convert.ToDouble(txtContractorRate.Text);
                    //   mdlTenderPrice.ID = Convert.ToInt64(ID.Text);
                    //if (ddlEstimate.SelectedItem.Value == "1")
                    //{
                    //    mdlTenderPrice.ContractorRate = Convert.ToDouble(lblTSRate.Text);
                    //}
                    //else if (ddlEstimate.SelectedItem.Value == "2" || ddlEstimate.SelectedItem.Value == "3")
                    //{
                    //    //mdlTenderPrice.ContractorRate = Convert.ToDouble(txtValue.Text) / 100 * Convert.ToDouble(txtSancQuan.Text);
                    //}
                    //else
                    //{
                    //    mdlTenderPrice.ContractorRate = Convert.ToDouble(txtContractorRate.Text);
                    //}


                    mdlTenderPrice.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdlTenderPrice.CreatedDate = DateTime.Now;
                    lstNew.Add(mdlTenderPrice);

                    mdlTenderWorkContractor.TP_EstimateType = ddlEstimate.SelectedItem.Text;

                    //if (ddlEstimate.SelectedItem.Value == "1")
                    //{
                    //    mdlTenderWorkContractor.TP_EstimatePercentage = Convert.ToDouble(lblTSRate.Text);
                    //}

                    if (ddlEstimate.SelectedItem.Value == "2" || ddlEstimate.SelectedItem.Value == "3")
                    {
                        mdlTenderWorkContractor.TP_EstimatePercentage = Convert.ToDouble(txtValue.Text);
                    }
                    //else
                    //{
                    //    mdlTenderWorkContractor.TP_EstimatePercentage = Convert.ToDouble(txtContractorRate.Text);
                    //}

                    mdlTenderWorkContractor.TenderPriced = true;
                    mdlTenderWorkContractor.ContractorsID = ContractorID;
                    mdlTenderWorkContractor.TenderWorksID = Convert.ToInt64(hdnWorkID.Value);
                    mdlTenderWorkContractor.TenderWorkAmount = Convert.ToDouble(lblTotal.Text);
                    lstTenderWorkContractor.Add(mdlTenderWorkContractor);

                }



                foreach (GridViewRow gvrow in gvCallDepositDetail.Rows)
                {
                    TM_TenderPriceCDR mdlTenderPriceCDR = new TM_TenderPriceCDR();

                    TextBox CDRNo = (TextBox)gvrow.FindControl("txtCDRNo");
                    TextBox BankDetail = (TextBox)gvrow.FindControl("txtBankDetail");
                    TextBox Amount = (TextBox)gvrow.FindControl("txtDepositAmount");
                    Label ID = (Label)gvrow.FindControl("ID");
                    Label lblAttachmentVal = (Label)gvrow.FindControl("lblAttachmentVal");
                    int rowindex = gvrow.RowIndex;
                    if (!string.IsNullOrEmpty(ID.Text))
                    {
                        mdlTenderPriceCDR.ID = Convert.ToInt64(ID.Text);
                    }

                    if (mdlTenderPriceCDR.ID > 0)
                    {
                        mdlTenderPriceCDR.Attachment = lblAttachmentVal.Text;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lblAttachmentVal.Text))
                        {
                            mdlTenderPriceCDR.Attachment = lblAttachmentVal.Text;
                        }
                        else
                        {
                            FileUploadControl FileUploadReceipt = (FileUploadControl)gvrow.FindControl("FileUploadControl");
                            List<Tuple<string, string, string>> lstNameofFiles = FileUploadReceipt.UploadNow(Configuration.TenderManagement, "ctrl", rowindex);
                            mdlTenderPriceCDR.Attachment = lstNameofFiles[0].Item3;
                        }
                       
                    }



                    Label TWContractorID = (Label)gvrow.FindControl("lblTenerWContractorID");


                    mdlTenderPriceCDR.TWContractorID = Convert.ToInt64(TWContractorID.Text);
                    mdlTenderPriceCDR.CDRNo = CDRNo.Text;
                    mdlTenderPriceCDR.BankDetail = BankDetail.Text;
                    mdlTenderPriceCDR.Amount = Convert.ToDouble(Amount.Text);

                    mdlTenderPriceCDR.CreatedDate = DateTime.Now;
                    mdlTenderPriceCDR.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    lstTenderPriceCDR.Add(mdlTenderPriceCDR);

                }
                long Id = 0;
                using (TransactionScope transaction = new TransactionScope())
                {

                    for (int i = 0; i < lstTenderWorkContractor.Count; i++)
                    {
                        Id = TenderMgmtBLL.UpdateTenderWorkContractorDataByID(lstTenderWorkContractor.ElementAt(i));
                    }

                    for (int i = 0; i < lstNew.Count; i++)
                    {
                        lstNew.ElementAt(i).TWContractorID = Id;
                        TenderMgmtBLL.AddTenderPriceValue(lstNew.ElementAt(i));
                    }

                    TenderMgmtBLL.DeleteCallDepositByTenderWorkContractorID(Id);

                    for (int i = 0; i < lstTenderPriceCDR.Count; i++)
                    {
                        if (lstTenderPriceCDR.ElementAt(i).TWContractorID == 0)
                        {
                            lstTenderPriceCDR.ElementAt(i).TWContractorID = Id;
                        }

                        TenderMgmtBLL.AddUpdateTenderPriceCDR(lstTenderPriceCDR.ElementAt(i));
                    }

                    transaction.Complete();
                }

                PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                _event.Parameters.Add("TenderNoticeID", Convert.ToInt64(hdnTenderNoticeID.Value));
                _event.Parameters.Add("TenderWorkID", Convert.ToInt64(hdnWorkID.Value));
                _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.AddComparativeStatement, (int)SessionManagerFacade.UserInformation.ID);


                ///  Works.AddADMReport.IsSaved = true;
                Response.Redirect("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value), false);
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            long CompanyID = 0;
            if (ddlCompanyName.SelectedItem.Value != "")
            {
                CompanyID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
            }
            BindTenderPriceData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value), CompanyID);
            BindCallDepositData(Convert.ToInt64(hdnWorkID.Value), CompanyID);
        }

        protected void gvCallDepositDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnWorkID.Value));
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtCDRNo = (TextBox)(TextBox)e.Row.FindControl("txtCDRNo");
                    TextBox BankDetail = (TextBox)e.Row.FindControl("txtBankDetail");
                    TextBox txtDepositAmount = (TextBox)e.Row.FindControl("txtDepositAmount");
                    if (gvCallDepositDetail.EditIndex == e.Row.RowIndex)
                    {
                        txtDepositAmount.Text = Convert.ToString(string.Format("{0:#,##0.00}", double.Parse(txtDepositAmount.Text)));
                    }
                    // Button btnAddBankDepositGrid = (Button)e.Row.FindControl("btnAddBankDepositGrid");
                    //  HyperLink hlImage = (HyperLink)e.Row.FindControl("hlImage");
                    HtmlGenericControl div = e.Row.FindControl("FileUploadDiv") as HtmlGenericControl;
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button lbtnDeleteAdvertisement = (Button)e.Row.FindControl("lbtnDeleteAdvertisement");
                    
                    Label lblAttachment = (Label)e.Row.FindControl("lblAttachment");
                    Label ID = (Label)e.Row.FindControl("ID");
                    //hlAttachmentName.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, hlAttachmentName.Text);
                    //hlAttachmentName.Text = hlAttachmentName.Text.Substring(hlAttachmentName.Text.LastIndexOf('_') + 1);

                    string AttachmentPath = lblAttachment.Text;
                    List<string> lstName = new List<string>();
                    lstName.Add(AttachmentPath);
                    WebFormsTest.FileUploadControl FileUploadControl1 = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl1");
                    FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    FileUploadControl1.Size = lstName.Count;

                   
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                         Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript" + e.Row.RowIndex, "RemoveFormRequired(" + e.Row.RowIndex + ");", true);
                         FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.TenderManagement, lstName);
                    }

                    GridViewRow header = gvCallDepositDetail.HeaderRow;
                    if (header != null)
                    {
                        Button btnAddBankDepositGrid = header.FindControl("btnAddBankDepositGrid") as Button;
                        //btnAddBankDepositGrid != null && mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring || 
                        if (IsAwarded == true || (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring && ID.Text != "0"))
                        {
                            btnAddBankDepositGrid.CssClass += " disabled";


                        }
                    }

                    //    hlImage.Visible = true;
                    if (mdlUser.DesignationID == (long)Constants.Designation.ChiefMonitoring)
                    {
                        div.Visible = false;
                        txtCDRNo.Enabled = false;
                        BankDetail.Enabled = false;
                        txtDepositAmount.Enabled = false;
                    }
                  
                    if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                    {
                        btnEdit.Visible = false;
                        btnEdit.CssClass += " disabled";
                        //lbtnDeleteAdvertisement.CssClass += " disabled";
                    }

                    else
                        btnEdit.Visible = true;

                    if (IsAwarded == true)
                    {
                        btnEdit.Visible = false;
                        btnEdit.CssClass += " disabled";
                        lbtnDeleteAdvertisement.CssClass += " disabled";
                    }
                    //txtCDRNo.Attributes.Remove("required");
                    //BankDetail.Attributes.Remove("required");
                    //txtDepositAmount.Attributes.Remove("required");

                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCallDepositDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                long CompanyID = 0;
                if (ddlCompanyName.SelectedItem.Value != "")
                {
                    CompanyID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
                }

                gvCallDepositDetail.EditIndex = e.NewEditIndex;
                BindCallDepositData(Convert.ToInt64(hdnWorkID.Value), CompanyID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCallDepositDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                long CompanyID = 0;
                if (ddlCompanyName.SelectedItem.Value != "")
                {
                    CompanyID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
                }

                gvCallDepositDetail.EditIndex = -1;
                BindCallDepositData(Convert.ToInt64(hdnWorkID.Value), CompanyID);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCallDepositDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                GridViewRow row = gvCallDepositDetail.Rows[e.RowIndex];
                TextBox txtCDRNo = (TextBox)row.FindControl("txtCDNo");
                TextBox txtBankDetail = (TextBox)row.FindControl("txtDetail");
                TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

                string CDRID = Convert.ToString(gvCallDepositDetail.DataKeys[e.RowIndex].Values[0]);

                TM_TenderPriceCDR mdlTenderCDR = new TM_TenderPriceCDR();

                mdlTenderCDR.ID = Convert.ToInt64(CDRID);

                mdlTenderCDR.CDRNo = txtCDRNo.Text;
                mdlTenderCDR.BankDetail = txtBankDetail.Text;
                mdlTenderCDR.Amount = Convert.ToDouble(txtAmount.Text);

                HyperLink hlAttachmentName = (HyperLink)row.FindControl("hlBankRecieptLnk");
                var FileName = hlAttachmentName.Attributes["FullName"];
                FileUploadControl FileUploadControl = (FileUploadControl)row.FindControl("FileUpload");
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);

                if (lstNameofFiles.Count == 0)
                {
                    mdlTenderCDR.Attachment = FileName;
                }
                else
                {
                    mdlTenderCDR.Attachment = lstNameofFiles[0].Item3;
                }


                bool IsSaved = new TenderManagementBLL().UpdateTenderPriceCDR(mdlTenderCDR);
                if (IsSaved)
                {
                    long CompanyID = 0;
                    if (ddlCompanyName.SelectedItem.Value != "")
                    {
                        CompanyID = Convert.ToInt64(ddlCompanyName.SelectedItem.Value);
                    }

                    gvCallDepositDetail.EditIndex = -1;
                    BindCallDepositData(Convert.ToInt64(hdnWorkID.Value), CompanyID);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}