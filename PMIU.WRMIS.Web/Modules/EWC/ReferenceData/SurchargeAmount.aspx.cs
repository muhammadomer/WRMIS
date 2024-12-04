using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EWC.ReferenceData
{
    public partial class SurchargeAmount : BasePage
    {
        //Data Members  
        List<EC_SurchargeAmount> lstData = new List<EC_SurchargeAmount>();
        Effluent_WaterChargesBLL bll_EWC = new Effluent_WaterChargesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                if (!IsPostBack)
                {
                    SetPageTitle();
                    Reset();
                    UpdateInfo();
                }
                FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.RemoveValidation);
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
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

        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = true;
                ShowHideHistoryDiv(false);
                ShowHideAddDiv(true);

                string buttonId = ((Button)sender).ID;

                if (buttonId.Equals("btnChange")) //Effluent Waters 
                {
                    lblTitle.Text = "Effluent Water";
                    txtAmount.Text = lblAmnt.Text;
                    ddlType.SelectedValue = ddlTypeID.Value;
                    hlAtchmntAdd.ToolTip = hdnAttachmentEffulent.Value;
                    if (Convert.ToString(hdnAttachmentEffulent.Value) != "")
                        hlAtchmntAdd.Visible = true;
                    else
                        hlAtchmntAdd.Visible = false;
                    hlAtchmntAdd.NavigateUrl = Utility.GetImageURL("EffluentWaterCharges", hdnAttachmentEffulent.Value);

                }
                else if (buttonId.Equals("btnChangeCanal")) // Canal Special Waters
                {
                    lblTitle.Text = "Canal Special Water";
                    txtAmount.Text = lblAmntC.Text;
                    ddlType.SelectedValue = ddlTypeIDC.Value;
                    hlAtchmntAdd.ToolTip = hdnAttachmentCanal.Value;
                     if (Convert.ToString(hdnAttachmentCanal.Value) != "")
                        hlAtchmntAdd.Visible = true;
                    else
                        hlAtchmntAdd.Visible = false;
                    hlAtchmntAdd.NavigateUrl = Utility.GetImageURL("EffluentWaterCharges", hdnAttachmentCanal.Value + "");
                }

                Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.NoOption, bll_EWC.GetTaxType());


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Add').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowAddModalScript", sb.ToString(), false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnHistry_Click(object sender, EventArgs e)
        {
            try
            {
                string buttonId = ((Button)sender).ID;
                btnSave.Visible = false;

                if (buttonId.Equals("btnHistry")) //Effluent Waters 
                {
                    lblTitle.Text = "Effluent Water";
                    gv.DataSource = bll_EWC.Surcharges_GetList(Constants.ECWServiceType.EFFLUENT.ToString());
                }
                else if (buttonId.Equals("btnHstoryCanal")) // Canal Special Waters
                {
                    lblTitle.Text = "Canal Special Water";
                    gv.DataSource = bll_EWC.Surcharges_GetList(Constants.ECWServiceType.CANAL.ToString());
                }

                gv.DataBind();
                gv.Visible = true;
                ShowHideHistoryDiv(true);
                ShowHideAddDiv(false);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Add').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowAddModalScript", sb.ToString(), false);
            }
            catch (Exception exp)
            {
                ShowHideHistoryDiv(true);
                ShowHideAddDiv(false);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EC_SurchargeAmount mdl = new EC_SurchargeAmount();

                if (lblTitle.Text.Equals("Effluent Water")) //Effluent Waters 
                {
                    if (!string.IsNullOrEmpty(lblAmnt.Text))
                    {
                        if (Convert.ToInt32(lblAmnt.Text) == Convert.ToInt32(txtAmount.Text) &&  (ddlType.SelectedItem.Value==ddlTypeID.Value))
                            return;
                    }
                    mdl.Source = Constants.ECWServiceType.EFFLUENT.ToString();
                }
                else if (lblTitle.Text.Equals("Canal Special Water")) // Canal Special Waters
                {
                    if (!string.IsNullOrEmpty(lblAmntC.Text))
                    {
                        if (Convert.ToInt32(lblAmntC.Text) == Convert.ToInt32(txtAmount.Text) && (ddlType.SelectedItem.Value == ddlTypeIDC.Value))
                            return;
                    }

                    mdl.Source = Constants.ECWServiceType.CANAL.ToString();
                }

                mdl.Amount = Convert.ToInt32(txtAmount.Text);
                mdl.Date = DateTime.Now;
                mdl.Description = txtRmrks.Text.Trim();
                List<Tuple<string, string, string>> lstAttchmnt = FileUploadControl1.UploadNow(Configuration.EffluentWaterCharges);

                if (lstAttchmnt != null && lstAttchmnt.Count() != 0)
                {
                    mdl.Attachment = lstAttchmnt.ElementAt(0).Item3;
                }
                else if (hlAtchmntAdd.Visible)
                {
                    mdl.Attachment = hlAtchmntAdd.ToolTip;
                }
                mdl.PaymentTypeID = Convert.ToInt32(ddlType.SelectedItem.Value);
                mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.CreatedDate = DateTime.Now;
                mdl.IsActive = true;

                if (bll_EWC.Surcharges_AddNew(mdl))
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Add').modal('hide');");
                sb.Append("$('.modal-backdrop').remove();");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CloseModalScript", sb.ToString(), false);
                Reset();
                UpdateInfo();

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnCncl_Click(object sender, EventArgs e)
        {
            ShowHideAddDiv(false);
        }

        private void ShowHideAddDiv(bool _Show)
        {
            txtAmount.Text = "";
            txtRmrks.Text = "";
            divAdd.Visible = _Show;
        }
        private void ShowHideHistoryDiv(bool _Show)
        {
            divHistory.Visible = _Show;
        }
        private void UpdateInfo()
        {
            object objDetail = bll_EWC.Surcharges_GetLatestRateDetails(Constants.ECWServiceType.EFFLUENT.ToString());
            if (objDetail != null)
            {
                Type propertiesType = objDetail.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(objDetail, null);
                    if (prop.ToString().Contains("Amount"))
                        lblAmnt.Text = propValue + "";

                    if (prop.ToString().Contains("Date"))
                        lblDate.Text = propValue + "";

                    if (prop.ToString().Contains("Type"))
                        lblType.Text = propValue + "";
                    if (prop.ToString().Contains("Attachment"))
                        hlAtchmntEffluent.NavigateUrl = Utility.GetImageURL("EffluentWaterCharges", propValue + "");

                    if (prop.ToString().Contains("AttachmentName"))
                    {
                        hdnAttachmentEffulent.Value = propValue + "";
                        if (!string.IsNullOrEmpty(hdnAttachmentEffulent.Value))
                            hlAtchmntEffluent.Visible = true;
                        else
                            hlAtchmntEffluent.Visible = false;
                    }
                    if (lblType.Text.Contains("Fix"))
                    {
                        ddlTypeID.Value = "1";
                    }
                    else 
                    {
                        ddlTypeID.Value = "2";
                    }
                }
            }

            objDetail = bll_EWC.Surcharges_GetLatestRateDetails(Constants.ECWServiceType.CANAL.ToString());
            if (objDetail != null)
            {
                Type propertiesType = objDetail.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(objDetail, null);
                    if (prop.ToString().Contains("Amount"))
                        lblAmntC.Text = propValue + "";

                    if (prop.ToString().Contains("Date"))
                        lblDateC.Text = propValue + "";

                    if (prop.ToString().Contains("Type"))
                        lblTypeC.Text = propValue + "";
                    if (prop.ToString().Contains("Attachment"))
                        hlAtchmntCanal.NavigateUrl = Utility.GetImageURL("EffluentWaterCharges", propValue + "");

                    if (prop.ToString().Contains("AttachmentName"))
                    {
                        hdnAttachmentCanal.Value = propValue + "";
                        if (!string.IsNullOrEmpty(hdnAttachmentCanal.Value))
                            hlAtchmntCanal.Visible = true;
                        else
                            hlAtchmntCanal.Visible = false;
                    }

                }
                if (lblTypeC.Text.Contains("Fix"))
                    {
                        ddlTypeIDC.Value = "1";
                    }
                else
                    {
                        ddlTypeIDC.Value = "2";
                    }
                
            }

        }
        private void Reset()
        {
            lblAmnt.Text = ""; lblType.Text = ""; lblDate.Text = "";
            lblAmntC.Text = ""; lblTypeC.Text = ""; lblDateC.Text = "";
            gv.DataSource = null; gv.DataBind(); gv.Visible = false;
            ShowHideAddDiv(false);
        }
        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gv.DataKeys[e.Row.RowIndex];
                    string Attachment = Convert.ToString(key.Values["Attachment"]);
                    #endregion

                    #region "Controls"
                    HyperLink hlImage = (HyperLink)e.Row.FindControl("hlImage");
                    #endregion
                    if (Attachment != "")
                        hlImage.Visible = true;
                    else
                        hlImage.Visible = false;
                   


                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}