using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class AnnualScntdDschrgSuply : BasePage
    {

        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    pnlEffluent.Visible = false;
                    pnlCanal.Visible = false;
                    if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        hdfID.Value = Request.QueryString["ID"];
                    }
                    if (bllEWC.IsBillingTheService(Convert.ToInt64(hdfID.Value), Constants.ECWServiceType.EFFLUENT.ToString()))
                    {
                        pnlEffluent.Visible = true;
                        LoadEffluentChargesView();
                    }
                    if (bllEWC.IsBillingTheService(Convert.ToInt64(hdfID.Value), Constants.ECWServiceType.CANAL.ToString()))
                    {
                        pnlCanal.Visible = true;
                        LoadCanalChargesView();
                    }
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
        private void HideEffluentChargesView(bool _HideView)
        {
            if (_HideView)
            {
                lblSnctDate.Text = string.Empty;
                lblSnctNo.Text = string.Empty;
                lblSnctAuth.Text = string.Empty;
                lblSnctDschrg.Text = string.Empty;
                hlAtchmnt.NavigateUrl = string.Empty;

                lblSnctDate.Visible = false;
                lblSnctNo.Visible = false;
                lblSnctAuth.Visible = false;
                lblSnctDschrg.Visible = false;
                hlAtchmnt.Visible = false;
            }
            else
            {
                txtSnctDate.Text = string.Empty;
                txtNo.Text = string.Empty;
                txtAuth.Text = string.Empty;
                txtDschrg.Text = string.Empty;


                divSnctDate.Visible = false;
                txtNo.Visible = false;
                txtAuth.Visible = false;
                txtDschrg.Visible = false;
                fcUploadFile.Visible = false;
            }
        }
        private void LoadCanalChargesView()
        {
            gvCSW.DataSource = bllEWC.GetSantionedSupply(GetIndustryNo(), Constants.ECWServiceType.CANAL.ToString());
            gvCSW.DataBind();
        }

        private void LoadEffluentChargesView()
        {
            HideEffluentChargesView(false);
            EC_SanctionedDischargeSupply mdl = bllEWC.GetSanctionedDischarge(GetIndustryNo(), Constants.ECWServiceType.EFFLUENT.ToString());
            if (mdl != null)
            {
                hdfEffluentDschrgID.Value = "" + mdl.ID;
                lblSnctDate.Text = Utility.GetFormattedDate(mdl.SanctionedDate);
                lblSnctNo.Text = mdl.SanctionedNo;
                lblSnctAuth.Text = mdl.SanctionedAuthority;
                lblSnctDschrg.Text = mdl.SanctionedSupplyDischarge == null ? "" : "" + mdl.SanctionedSupplyDischarge;
                if (!string.IsNullOrEmpty(mdl.Attachment))
                {
                    hlAtchmnt.NavigateUrl = Utility.GetImageURL(Configuration.EffluentWaterCharges, mdl.Attachment);
                    hlAtchmnt.Visible = true;
                }

                lblSnctDate.Visible = true;
                lblSnctNo.Visible = true;
                lblSnctAuth.Visible = true;
                lblSnctDschrg.Visible = true;
                // hlAtchmnt.Visible = true;
            }
            //else LoadEffluentChargesForm();
        }
        private void LoadEffluentChargesForm()
        {
            txtSnctDate.Text = (string.IsNullOrEmpty(lblSnctDate.Text) ? Utility.GetFormattedDate(DateTime.Now) : lblSnctDate.Text);
            txtNo.Text = lblSnctNo.Text;
            txtAuth.Text = lblSnctAuth.Text;
            txtDschrg.Text = lblSnctDschrg.Text;

            divSnctDate.Visible = true;
            txtNo.Visible = true;
            txtAuth.Visible = true;
            txtDschrg.Visible = true;
            fcUploadFile.Visible = true;

            HideEffluentChargesView(true);

            btnChange.Text = "Update";
            btnHistry.Text = "Cancel";

        }
        private void UpdateEffluentWaterCharges()
        {
            try
            {
                if (bllEWC.IsLessThanExistingDate(Utility.GetParsedDate(txtSnctDate.Text), Convert.ToInt64(hdfEffluentDschrgID.Value)))
                {
                    Master.ShowMessage(Message.DateShouldBeGreaterThanLastValue.Description, SiteMaster.MessageType.Error);
                    return;
                }

                EC_SanctionedDischargeSupply mdlNew = new EC_SanctionedDischargeSupply();
                mdlNew.IndustryID = Convert.ToInt64(hdfID.Value);
                mdlNew.IsActive = true;
                mdlNew.SanctionedAuthority = txtAuth.Text;
                mdlNew.SanctionedDate = Utility.GetParsedDate(txtSnctDate.Text);
                mdlNew.SanctionedNo = txtNo.Text;
                if (!string.IsNullOrEmpty(txtDschrg.Text))
                    mdlNew.SanctionedSupplyDischarge = Convert.ToDouble(txtDschrg.Text);

                List<Tuple<string, string, string>> lstAttchmnt = fcUploadFile.UploadNow(Configuration.EffluentWaterCharges);
                if (lstAttchmnt != null && lstAttchmnt.Count() != 0)
                    mdlNew.Attachment = lstAttchmnt.ElementAt(0).Item3;

                mdlNew.ServiceType = Constants.ECWServiceType.EFFLUENT.ToString();
                mdlNew.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlNew.CreatedDate = DateTime.Now;

                long value = 0;
                if (!string.IsNullOrEmpty(hdfEffluentDschrgID.Value))
                    value = Convert.ToInt64(hdfEffluentDschrgID.Value);
                bllEWC.UpdateAnnualSanctionedDischarge(mdlNew, value);
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                LoadEffluentChargesView();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        private void UpdateCanaltWaterCharges()
        {
            try
            {
                if (bllEWC.IsLessThanExistingDate_CSW(Utility.GetParsedDate(txtSnctDate_CSW.Text), Convert.ToInt64(hdfID_CSW.Value)))
                {
                    Master.ShowMessage(Message.DateShouldBeGreaterThanLastValue.Description, SiteMaster.MessageType.Error);
                    return;
                }

                EC_SanctionedDischargeSupply mdlNew = new EC_SanctionedDischargeSupply();
                mdlNew.IndustryID = Convert.ToInt64(hdfID.Value);
                mdlNew.CanalSpecialWaterID = Convert.ToInt64(hdfID_CSW.Value);
                mdlNew.IsActive = true;
                mdlNew.SanctionedAuthority = txtSnctAuth_CSW.Text;
                mdlNew.SanctionedDate = Utility.GetParsedDate(txtSnctDate_CSW.Text);
                mdlNew.SanctionedNo = txtSnctNo_CSW.Text;
                if (!string.IsNullOrEmpty(txtSnctDschrg_CSW.Text))
                    mdlNew.SanctionedSupplyDischarge = Convert.ToDouble(txtSnctDschrg_CSW.Text);

                List<Tuple<string, string, string>> lstAttchmnt = fuc_CSW.UploadNow(Configuration.EffluentWaterCharges);
                if (lstAttchmnt != null && lstAttchmnt.Count() != 0)
                {
                    mdlNew.Attachment = lstAttchmnt.ElementAt(0).Item3;
                }
                else if (hlAtchmnt_CSW.Visible)
                {
                    mdlNew.Attachment = hlAtchmnt_CSW.ToolTip;
                }

                mdlNew.ServiceType = Constants.ECWServiceType.CANAL.ToString();
                mdlNew.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlNew.CreatedDate = DateTime.Now;

                long value = 0;
                if (!string.IsNullOrEmpty(hdf_RcrdID_CSW.Value))
                    value = Convert.ToInt64(hdf_RcrdID_CSW.Value);
                bllEWC.UpdateAnnualSanctionedDischarge(mdlNew, value);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.ID.Equals("btnChange")) // Effluent Waters
            {
                if (btn.Text.Equals("Change"))
                {
                    LoadEffluentChargesForm();
                   
                }
                else
                {
                    btnChange.Enabled = false;
                    UpdateEffluentWaterCharges();
                    btnChange.Text = "Change";
                    btnHistry.Text = "View History";
                    btnChange.Enabled = true; ;
                }
            }
            else if (btn.ID.Equals("btnHistry")) // Effluent Waters
            {
                if (btn.Text.Equals("Cancel"))
                {
                    LoadEffluentChargesView();
                    btnHistry.Text = "View History";
                    btnChange.Text = "Change";
                }
                else
                {
                    //TODO:
                    //Show history in a popup
                    lblTitle.Text = "Sanctioned Discharge History";
                    gv.DataSource = bllEWC.GetSanctionedDischargeEffulent(GetIndustryNo(), Constants.ECWServiceType.EFFLUENT.ToString());
                    gv.DataBind();
                    gv.Visible = true;
                    ShowHideHistoryDiv(true);
                    divAdd.Visible = false;
                    divHistoryCanal.Visible = false;
                    btnChangeCSW.Visible = false;
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append(@"<script type='text/javascript'>");
                    //sb.Append("$('#Add').modal('show');");
                    //sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowAddModalScript", sb.ToString(), false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);

                }
            }
            else if (btn.ID.Equals("btnChangeCSW"))
            {
                btnChangeCSW.Enabled = false;
                UpdateCanaltWaterCharges();
                ResetCanalSpecialWaterForm();
                LoadCanalChargesView();
                btnChangeCSW.Enabled = true;
            }
        }
        private void ShowHideHistoryDiv(bool _Show)
        {
            divHistory.Visible = _Show;
        }
        private long GetIndustryNo()
        {
            if (hdfID.Value == string.Empty)
                return 0;

            return Convert.ToInt64(hdfID.Value);
        }

        protected void gvCSW_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCSW.PageIndex = e.NewPageIndex;
                LoadCanalChargesView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCSW_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Change"))
            {
                //TODO:
                //Fill the field with the existing values
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                long id = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                hdfID_CSW.Value = id.ToString();
                hdf_RcrdID_CSW.Value = ((Label)row.FindControl("lblRecordID")).Text;
                divAdd.Visible = true;
                lblTitle.Text = ((Label)row.FindControl("lblName")).Text;

                txtSnctDate_CSW.Text = ((Label)row.FindControl("lblDate")).Text;
                txtSnctNo_CSW.Text = ((Label)row.FindControl("lblNo")).Text;
                txtSnctAuth_CSW.Text = ((Label)row.FindControl("lblAuth")).Text;
                txtSnctDschrg_CSW.Text = ((Label)row.FindControl("lblAmnt")).Text;

                HiddenField atchmnt = (HiddenField)row.FindControl("hdf_Atchmnt");
                if (!string.IsNullOrEmpty(atchmnt.Value))
                {
                    hlAtchmnt_CSW.ToolTip = atchmnt.Value.ToString();
                    hlAtchmnt_CSW.Visible = true;
                    hlAtchmnt_CSW.NavigateUrl = Utility.GetImageURL(Configuration.EffluentWaterCharges, atchmnt.Value.ToString().Trim());
                }
                ShowHideHistoryDiv(false);
                divHistoryCanal.Visible = false;
                btnChangeCSW.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }
            else if (e.CommandName.Equals("History"))
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                long id = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                long ChannelID = Convert.ToInt64(((Label)row.FindControl("lblChannelID")).Text);
                long DivisionID = Convert.ToInt64(((Label)row.FindControl("lblDivisionID")).Text);
                hdfID_CSW.Value = id.ToString();
                hdf_RcrdID_CSW.Value = ((Label)row.FindControl("lblRecordID")).Text;
                lblTitle.Text = ((Label)row.FindControl("lblName")).Text + " : Sanctioned Supply History";
                gvCanal.DataSource = bllEWC.GetSantionedSupplyCanal(GetIndustryNo(), Constants.ECWServiceType.CANAL.ToString(), id, DivisionID);
                gvCanal.DataBind();
                ShowHideHistoryDiv(false);
                divAdd.Visible = false;
                divHistoryCanal.Visible = true;
                btnChangeCSW.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }

        }

        private void ResetCanalSpecialWaterForm()
        {
            hdfID_CSW.Value = "0";
            hdf_RcrdID_CSW.Value = "0";
            lblTitle.Text = "";
            txtSnctDate_CSW.Text = "";
            txtSnctNo_CSW.Text = "";
            txtSnctAuth_CSW.Text = "";
            txtSnctDschrg_CSW.Text = "";
            hlAtchmnt_CSW.Visible = false;
            hlAtchmnt.NavigateUrl = null;
        }

        protected void gvCSW_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvCSW.EditIndex = -1;
                LoadCanalChargesView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvCanal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvCanal.DataKeys[e.Row.RowIndex];
                    string Attachment = Convert.ToString(key.Values["Attcmnt"]);
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