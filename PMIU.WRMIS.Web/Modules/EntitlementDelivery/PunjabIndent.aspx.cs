using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class PunjabIndent : System.Web.UI.Page
    {
        long PunjabIndentID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Convert.ToInt32(Request.QueryString["save"])==1)
                {
                    txtFromDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(Request.QueryString["fd"]));
                    txtToDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(Request.QueryString["td"]));
                    bindPunjabIndentGrid();
                    gvPunjabIndent.Visible = true;
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    bindPunjabIndentGrid();
                    gvPunjabIndent.Visible = true;
                }
                
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bindPunjabIndentGrid();
            // gvPunjabIndent.Visible = true;
            RemoveValidation();
        }
        public void bindPunjabIndentGrid()
        {
            DateTime _ToDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime _FromDate = Convert.ToDateTime(txtToDate.Text);
            List<dynamic> lstPunjabIndent = new EntitlementDeliveryBLL().GetAllPunjabIndent(_ToDate, _FromDate);
            Session["pnjbIndent"] = lstPunjabIndent;
            gvPunjabIndent.DataSource = lstPunjabIndent;
            gvPunjabIndent.DataBind();
            gvPunjabIndent.Visible = true;
            //PunjabIndentUP.Update();
        }
        #region Grid View Events Punjab Indent
        protected void gvPunjabIndent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //ID,FromDate,ToDate,Thal,CJLinkAtHead,GreaterThal,BelowChashmaBarrage,CRBC,Mangla,Remarks
                if (e.CommandName == "AddPI")
                {
                    List<object> lstACCPOrder = Session["pnjbIndent"] as List<object>;
                    lstACCPOrder.Add(
                    new
                    {
                        ID = 0,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now,
                        Thal = string.Empty,
                        CJLinkAtHead = string.Empty,
                        GreaterThal = string.Empty,
                        BelowChashmaBarrage = string.Empty,
                        CRBC = string.Empty,
                        Mangla = string.Empty,
                        Remarks = string.Empty
                    });

                    gvPunjabIndent.PageIndex = gvPunjabIndent.PageCount;
                    gvPunjabIndent.DataSource = lstACCPOrder;
                    gvPunjabIndent.DataBind();

                    gvPunjabIndent.EditIndex = gvPunjabIndent.Rows.Count - 1;
                    gvPunjabIndent.DataBind();
                }
                #region Edit
                if (e.CommandName == "Edit")
                {
                    GridView gv = sender as GridView;
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    long PID = Convert.ToInt64(GetDataKeyValue(gv, "ID", rowIndex));
                    Response.Redirect("~/Modules/EntitlementDelivery/AddPunjabIndent.aspx?mode=e&PID="+PID+"");

                    //#region "Datakeys"
                    //long PID = Convert.ToInt64(GetDataKeyValue(gv, "ID", rowIndex));
                    //string FromIndentDate = GetDataKeyValue(gv, "FromDate", rowIndex);
                    //string ToIndentDate = GetDataKeyValue(gv, "ToDate", rowIndex);
                    //string Thal = GetDataKeyValue(gv, "Thal", rowIndex);
                    //string CJ = GetDataKeyValue(gv, "CJLinkAtHead", rowIndex);
                    //string GTC = GetDataKeyValue(gv, "GreaterThal", rowIndex);
                    //string ChashmaDS = GetDataKeyValue(gv, "BelowChashmaBarrage", rowIndex);
                    //string CRBCPunjab = GetDataKeyValue(gv, "CRBC", rowIndex);
                    //string Mangla = GetDataKeyValue(gv, "Mangla", rowIndex);
                    //string Remarks = GetDataKeyValue(gv, "Remarks", rowIndex);
                    //#endregion
                    //#region RestControls
                    //ResetControls();
                    //#endregion
                    //#region Binding Controls
                    //txtBelowChashmaBarrage.Text = ChashmaDS;
                    //txtCJ.Text = CJ;
                    //txtCRBC.Text = CRBCPunjab;
                    //txtGreaterThal.Text = GTC;
                    //txtThal.Text = Thal;
                    //txtMangla.Text = Mangla;
                    //txtFromDateAdd.Text = Utility.GetFormattedDate(Convert.ToDateTime(FromIndentDate));
                    //txtToDateAdd.Text = Utility.GetFormattedDate(Convert.ToDateTime(ToIndentDate));
                    //txtRemarks.Text = Remarks;
                    //hdrText.Text = "Update Punjab Indent";
                    ////btnUpdatePunjabIndent.Visible = true;
                    ////btnSavePunjabIndent.Visible = false;
                    //hfPunjabIndentID.Value = Convert.ToString(PID);
                    ///// PunjabIndentID = PID;
                    //#endregion
                    //AddValidation();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#MpAddUpdatePunjabIndent').modal();", true);
                }
                #endregion
                #region Delete
                if (e.CommandName == "Delete")
                {
                    try
                    {
                        GridView gv = sender as GridView;
                        int rowIndex = int.Parse(e.CommandArgument.ToString());
                        long PunjabIndentID = Convert.ToInt64(GetDataKeyValue(gv, "ID", rowIndex));
                        bool isRecordExists = new EntitlementDeliveryBLL().IsPunjabIndentExist(PunjabIndentID);

                        if (isRecordExists)
                        {
                            bool isDeleted = new EntitlementDeliveryBLL().DeletePunjabIndent(PunjabIndentID);
                            if (isDeleted)
                            {
                                Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                                bindPunjabIndentGrid();
                            }
                            else
                                Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    catch (Exception exp)
                    {
                        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                    }

                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvPunjabIndent_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvPunjabIndent_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            #region "Data Keys"
            DataKey key = gvPunjabIndent.DataKeys[e.RowIndex];
            string ID = Convert.ToString(key.Values["ID"]);
            PunjabIndentID = Convert.ToInt64(ID);
            #endregion
            #region "Controls"
            GridViewRow row = gvPunjabIndent.Rows[e.RowIndex];
            TextBox gvtxtFromDate = (TextBox)row.FindControl("gvtxtFromDate");
            TextBox gvtxtToDate = (TextBox)row.FindControl("gvtxtToDate");
            TextBox gvtxtThal = (TextBox)row.FindControl("gvtxtThal");
            TextBox gvtxtCJ = (TextBox)row.FindControl("gvtxtCJ");
            TextBox gvtxtGreaterThal = (TextBox)row.FindControl("gvtxtGreaterThal");
            TextBox gvtxtBelowChashmaBarrage = (TextBox)row.FindControl("gvtxtBelowChashmaBarrage");
            TextBox gvtxtCRBC = (TextBox)row.FindControl("gvtxtCRBC");
            TextBox gvtxtMangla = (TextBox)row.FindControl("gvtxtMangla");
            TextBox gvtxtRemarks = (TextBox)row.FindControl("gvtxtRemarks");
            #endregion

            #region Prepair Enitity
            IW_PunjabIndent pi = new IW_PunjabIndent();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            pi.ID = PunjabIndentID;
            pi.FromIndentDate = Convert.ToDateTime(gvtxtFromDate.Text);
            pi.ToIndentDate = Convert.ToDateTime(gvtxtToDate.Text);
            pi.Thal = Convert.ToDouble(gvtxtThal.Text);
            pi.CJ = Convert.ToDouble(gvtxtCJ.Text);
            pi.CRBCPunjab = Convert.ToDouble(gvtxtCRBC.Text);
            pi.ChashmaDS = Convert.ToDouble(gvtxtBelowChashmaBarrage.Text);
            pi.GTC = Convert.ToDouble(gvtxtGreaterThal.Text);
            pi.Mangla = Convert.ToDouble(gvtxtMangla.Text);
            pi.Remarks = gvtxtRemarks.Text;
            if (pi.ID > 0)
            {
                pi.ModifiedBy = (int)mdlUser.ID;
                pi.ModifiedDate = DateTime.Now;
            }
            else
            {
                pi.CreatedBy = (int)mdlUser.ID;
                pi.CreatedDate = DateTime.Now;

            }
            #endregion

            #region Save  Date
            if (pi.ToIndentDate > DateTime.Now && pi.FromIndentDate > DateTime.Now)
            {
                if (pi.FromIndentDate > pi.ToIndentDate)
                {
                    new EntitlementDeliveryBLL().AddUpdatePunjabIndent(pi);
                    ResetControls();
                    gvPunjabIndent.Visible = true;
                    bindPunjabIndentGrid();
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    txtFromDate.Text = Utility.GetFormattedDate(pi.FromIndentDate);
                    txtToDate.Text = Utility.GetFormattedDate(pi.ToIndentDate);
                    bindPunjabIndentGrid();
                    //UpdatePanelInspectionArea.Update();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#MpAddUpdatePunjabIndent').modal('hide');", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "EnableDesignParameter(" + txtFromDate.ID + ");", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "EnableDesignParameter(" + txtToDate.ID + ");", true);    
                }
                else
                {
                    Master.ShowMessage(Message.FromDateMustbGreaterThan.Description, SiteMaster.MessageType.Error);
                }

            }
            else
            {
                Master.ShowMessage(Message.RecordNotSavedInPerviousDate.Description, SiteMaster.MessageType.Error);
            }
            #endregion
        }
        public void BindGaugeInspectionGridView()
        {
            try
            {
                //GridView gvPunjabIndent = (GridView)UpdatePanelSchduleInspection.FindControl("gvPunjabIndent");
                //gvPunjabIndent.DataSource = new EntitlementDeliveryBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.GaugeReading);
                //gvPunjabIndent.DataBind();
                //UpdatePanelSchduleInspection.Update();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvPunjabIndent_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{
            //    RemoveAllpopupValidation();
            //    gvPunjabIndent.EditIndex = e.NewEditIndex;
            //    BindGaugeInspectionGridView();

            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        #endregion
        #region Utility Methods
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private void ResetControls()
        {
            txtBelowChashmaBarrage.Text = string.Empty;
            txtCRBC.Text = string.Empty;
            txtFromDateAdd.Text = string.Empty;
            txtToDateAdd.Text = string.Empty;
            txtGreaterThal.Text = string.Empty;
            txtCJ.Text = string.Empty;
            txtMangla.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtThal.Text = string.Empty;

        }

        protected void AddValidation()
        {
            #region Validation GaugeInspec
            txtFromDateAdd.CssClass = "form-control required date-picker";
            txtFromDateAdd.Attributes.Add("required", "true");

            txtToDateAdd.CssClass = "form-control required date-picker";
            txtToDateAdd.Attributes.Add("required", "true");

            txtThal.CssClass = "form-control required";
            txtThal.Attributes.Add("required", "true");

            txtCJ.CssClass = "form-control required";
            txtCJ.Attributes.Add("required", "true");

            txtBelowChashmaBarrage.CssClass = "form-control required";
            txtBelowChashmaBarrage.Attributes.Add("required", "true");

            txtCRBC.CssClass = "form-control required";
            txtCRBC.Attributes.Add("required", "true");

            txtGreaterThal.CssClass = "form-control required";
            txtGreaterThal.Attributes.Add("required", "true");

            txtMangla.CssClass = "form-control required";
            txtMangla.Attributes.Add("required", "true");

            #endregion
        }
        protected void RemoveValidation()
        {
            #region Validation GaugeInspec
            txtFromDateAdd.CssClass = "form-control date-picker";
            txtFromDateAdd.Attributes.Remove("required");

            txtToDateAdd.CssClass = "form-control required date-picker";
            txtToDateAdd.Attributes.Remove("required");

            txtThal.CssClass = "form-control";
            txtThal.Attributes.Remove("required");

            txtCJ.CssClass = "form-control";
            txtCJ.Attributes.Remove("required");

            txtBelowChashmaBarrage.CssClass = "form-control";
            txtBelowChashmaBarrage.Attributes.Remove("required");

            txtCRBC.CssClass = "form-control";
            txtCRBC.Attributes.Remove("required");

            txtGreaterThal.CssClass = "form-control";
            txtGreaterThal.Attributes.Remove("required");

            txtMangla.CssClass = "form-control";
            txtMangla.Attributes.Remove("required");

            #endregion
        }
        #endregion

        protected void btnSavePunjabIndent_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveValidation();
                PunjabIndentID = hfPunjabIndentID.Value != string.Empty ? Convert.ToInt64(hfPunjabIndentID.Value) : 0;
                IW_PunjabIndent pi = AddPunjabIndentEntity(PunjabIndentID);


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }

        }

        private IW_PunjabIndent AddPunjabIndentEntity(long PunjabIndentID = 0)
        {
            IW_PunjabIndent pi = new IW_PunjabIndent();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            pi.ID = PunjabIndentID;
            pi.FromIndentDate = Convert.ToDateTime(txtFromDateAdd.Text);
            pi.ToIndentDate = Convert.ToDateTime(txtToDateAdd.Text);
            pi.Thal = Convert.ToDouble(txtThal.Text);
            pi.CJ = Convert.ToDouble(txtCJ.Text);
            pi.CRBCPunjab = Convert.ToDouble(txtCRBC.Text);
            pi.ChashmaDS = Convert.ToDouble(txtBelowChashmaBarrage.Text);
            pi.GTC = Convert.ToDouble(txtGreaterThal.Text);
            pi.Mangla = Convert.ToDouble(txtMangla.Text);
            pi.Remarks = txtRemarks.Text;
            if (pi.ID > 0)
            {
                pi.ModifiedBy = (int)mdlUser.ID;
                pi.ModifiedDate = DateTime.Now;
            }
            else
            {
                pi.CreatedBy = (int)mdlUser.ID;
                pi.CreatedDate = DateTime.Now;

            }
            return pi;
        }

        //protected void btnAddPI_Click(object sender, EventArgs e)
        //{
        //    //ResetControls();
        //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#MpAddUpdatePunjabIndent').modal('toggle');", true);
        ////    //AddValidation();
        ////       protected void gvPunjabIndent_RowCommand(object sender, GridViewCommandEventArgs e)
        ////{
        ////    try
        ////    {
        ////        //ID,FromDate,ToDate,Thal,CJLinkAtHead,GreaterThal,BelowChashmaBarrage,CRBC,Mangla,Remarks
        ////        if (e.CommandName == "AddPI")
        //    /////

        //}




    }
}