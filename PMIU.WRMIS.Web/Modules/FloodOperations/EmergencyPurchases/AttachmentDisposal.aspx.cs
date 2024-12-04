using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class AttachmentDisposal : BasePage
    {
        public static int EPYear;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int MaterialDisposalID = Utility.GetNumericValueFromQueryString("ID", 0);
                    if (MaterialDisposalID > 0)
                    {
                        hdnMaterialDisposalID.Value = Convert.ToString(MaterialDisposalID);

                        //hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/SearchEmergencyPurchases.aspx?EmergencyPurchaseID={0}", Session["EmergencyPurchaseID"].ToString());
                        HeaderMaterialDisposal_Show(MaterialDisposalID);
                        GridMD_attachment(MaterialDisposalID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/DisposalEmergencyPurchases.aspx?EPWorkID={0}", Session["EPWorkID"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void GridMD_attachment(int MaterialDisposalID)
        {
            try
            {

                List<object> lstDisposalAttachment = new FloodOperationsBLL().GetF_EmergencyDisposal_Attachment_ID(MaterialDisposalID);

                gv_M_Disposal_Attachment.DataSource = lstDisposalAttachment;
                gv_M_Disposal_Attachment.DataBind();
                gv_M_Disposal_Attachment.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void HeaderMaterialDisposal_Show(int MaterialDisposalID)
        {
            try
            {

                object lstF_MaterialDisposal_Attach = new FloodOperationsBLL().GetF_MaterialDisposal_Attachement_Header_By_ID(MaterialDisposalID);


                if (lstF_MaterialDisposal_Attach != null)
                {

                    string disposalDate = Convert.ToString(lstF_MaterialDisposal_Attach.GetType().GetProperty("DisposalDate").GetValue(lstF_MaterialDisposal_Attach));
                    lbl_VehicleNumber.Text = Convert.ToString(lstF_MaterialDisposal_Attach.GetType().GetProperty("VehicleNumber").GetValue(lstF_MaterialDisposal_Attach));
                    lbl_BuiltyNumber.Text = Convert.ToString(lstF_MaterialDisposal_Attach.GetType().GetProperty("BuiltyNumber").GetValue(lstF_MaterialDisposal_Attach));
                    lbl_Date.Text = Convert.ToDateTime(disposalDate).ToString("dd-MMM-yyyy");


                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gv_M_Disposal_Attachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv_M_Disposal_Attachment.EditIndex = -1;
                GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_M_Disposal_Attachment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstF_EmergencyDisposal_Attachment = new FloodOperationsBLL().GetF_EmergencyDisposal_Attachment_ID(Convert.ToInt32(hdnMaterialDisposalID.Value));

                    FO_MaterialDisposalAttachment mdlFO_MD_Attachment = new FO_MaterialDisposalAttachment();

                    mdlFO_MD_Attachment.ID = 0;
                    mdlFO_MD_Attachment.MaterialDisposalID = 0;
                    mdlFO_MD_Attachment.FileName = "";
                    mdlFO_MD_Attachment.FileURL = "";

                    mdlFO_MD_Attachment.CreatedBy = 0;
                    mdlFO_MD_Attachment.CreatedDate = DateTime.Now;
                    lstF_EmergencyDisposal_Attachment.Add(mdlFO_MD_Attachment);


                    gv_M_Disposal_Attachment.PageIndex = gv_M_Disposal_Attachment.PageCount;
                    gv_M_Disposal_Attachment.DataSource = lstF_EmergencyDisposal_Attachment;
                    gv_M_Disposal_Attachment.DataBind();

                    gv_M_Disposal_Attachment.EditIndex = gv_M_Disposal_Attachment.Rows.Count - 1;
                    gv_M_Disposal_Attachment.DataBind();



                }
                if (e.CommandName == "image")
                {
                    GridViewRow gvrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gv_M_Disposal_Attachment.DataKeys[gvrow.RowIndex];
                    //     string URL = Convert.ToString(key["ImageURL"]).Substring(Convert.ToString(key["ImageURL"]).LastIndexOf('_') + 1);
                    string URL = Convert.ToString(key["FileURL"]);
                    imgSDImage.ImageUrl = Utility.GetImageURL(Configuration.FloodOperations, URL);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#viewimage').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScript", sb.ToString(), false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_M_Disposal_Attachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsSave = false;
                DataKey key = gv_M_Disposal_Attachment.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                GridViewRow row = gv_M_Disposal_Attachment.Rows[e.RowIndex];
                HyperLink hypFileName = (HyperLink)row.FindControl("hypFileName");
                FileUpload FileUpload = (FileUpload)row.FindControl("fileupdloadID");



                FO_MaterialDisposalAttachment ObjModel = new FO_MaterialDisposalAttachment();

                string fileName = "";


                if (FileUpload.HasFile)
                {
                    string file_ext = System.IO.Path.GetExtension(FileUpload.FileName).ToUpper();
                    if (file_ext == ".JPG" || file_ext == ".JPEG" || file_ext == ".PNG" || file_ext == ".GIF")
                    {
                        string guidID = Guid.NewGuid().ToString();
                        fileName = guidID + "_FO_MD_" + hdnMaterialDisposalID.Value + "_" + System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);

                        string path = Utility.GetImagePath(Configuration.FloodOperations);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string finalpath = path + "\\\\" + fileName;
                        FileUpload.PostedFile.SaveAs(finalpath);

                        ObjModel.ID = Convert.ToInt64(ID);
                        ObjModel.MaterialDisposalID = Convert.ToInt64(hdnMaterialDisposalID.Value);
                        ObjModel.FileName = FileUpload.FileName;

                        if (ObjModel.ID == 0)
                        {
                            ObjModel.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            ObjModel.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            ObjModel.CreatedBy = Convert.ToInt32(CreatedBy);
                            ObjModel.CreatedDate = Convert.ToDateTime(CreatedDate);
                            ObjModel.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                            ObjModel.ModifiedDate = DateTime.Now;
                        }
                        ObjModel.FileURL = fileName;
                        IsSave = new FloodOperationsBLL().SaveDM_Attachment(ObjModel);
                    }
                    else
                    {
                        Master.ShowMessage("Only Image Upload", SiteMaster.MessageType.Error);
                        GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
                        return;
                    }
                }

                if (IsSave == true)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjModel.ID) == 0)
                    {
                        gv_M_Disposal_Attachment.PageIndex = 0;
                    }

                    gv_M_Disposal_Attachment.EditIndex = -1;
                    GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_M_Disposal_Attachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_SystemParameters systemParameters = null;
                bool CanAddEditEP = false;
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                //string StartDate = systemParameters.ParameterValue + "-" + EPYear; // 01-Jan
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                //string EndDate = systemParameters.ParameterValue + "-" + EPYear;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                    btnAdd.Enabled = false;
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EPYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnAdd.Enabled = CanAddEditEP;
                    }
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnAdd.Enabled = true;
                    //    }
                    //}

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DataKey key = gv_M_Disposal_Attachment.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string MaterialDisposalID = Convert.ToString(key.Values["MaterialDisposalID"]);
                    //string FileName = Convert.ToString(key.Values["FileName"]);
                    string FileURL = Convert.ToString(key.Values["FileURL"]);

                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    LinkButton lnkimage = (LinkButton)e.Row.FindControl("lnkimage");
                    FileUpload fileupdloadID = (FileUpload)e.Row.FindControl("fileupdloadID");

                    #region User Role

                    btnEdit.Enabled = false;
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnEdit.Enabled = true;
                    //    }
                    //}
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EPYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnEdit.Enabled = CanAddEditEP;
                    }

                    #endregion User Role



                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    CanAdd = true;
                    //    CanEdit = true;
                    //    CanDelete = true;
                    //}
                    //else
                    //{
                    //    CanAdd = false;
                    //    CanEdit = false;
                    //    CanDelete = false;
                    //}

                    //if (e.Row.RowState.Equals(DataControlRowState.Edit))
                    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                    {
                        Button btnSave = (Button)e.Row.FindControl("btnSave");
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
                    }
                    string URL = FileURL.Substring(FileURL.LastIndexOf('_') + 1);
                    lnkimage.Text = URL;


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_M_Disposal_Attachment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_M_Disposal_Attachment.EditIndex = e.NewEditIndex;
                GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_M_Disposal_Attachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gv_M_Disposal_Attachment.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodOperationsBLL().DeleteFo_MaterialDisposal_Attachement(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_M_Disposal_Attachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_M_Disposal_Attachment.PageIndex = e.NewPageIndex;

                GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_M_Disposal_Attachment_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv_M_Disposal_Attachment.EditIndex = -1;

                GridMD_attachment(Convert.ToInt32(hdnMaterialDisposalID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}