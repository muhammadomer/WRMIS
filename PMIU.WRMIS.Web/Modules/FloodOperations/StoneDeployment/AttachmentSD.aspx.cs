using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.StoneDeployment
{
    public partial class AttachmentSD : BasePage
    {
        public static long _FFPSPID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                if (!IsPostBack)
                {

                    SetPageTitle();
                    int SDID = Utility.GetNumericValueFromQueryString("ID", 0);
                    //  SDID = 5;
                    if (SDID > 0)
                    {
                        hdnSDID.Value = Convert.ToString(SDID);
                        Header_Show(SDID);
                        Gridattachment(SDID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/StoneDeployment/AddStoneDeployment.aspx?FFPStonePositionID={0}", _FFPSPID);

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
        private void Header_Show(long SDID)
        {
            try
            {

                DataSet DS = new FloodFightingPlanBLL().GetSDAttache(null, Convert.ToInt32(hdnSDID.Value), null, null, null, null, null, null);
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    lbl_Division.Text = DR["Division"].ToString();
                    lbl_infra_type.Text = DR["InfrastructureType"].ToString();
                    lbl_infrastructure.Text = DR["InfrastructureName"].ToString();
                    lbl_RD.Text = Convert.ToString(Calculations.GetRDText(Convert.ToInt64(DR["RD"].ToString())));
                    lbl_qtyApp.Text = DR["RequiredQty"].ToString();
                    lbl_qtyDispos.Text = DR["QtyOfStoneDisposed"].ToString();

                    if (DR["DisposedDate"].ToString() != null && DR["DisposedDate"].ToString() != "")
                        lbl_disposDate.Text = Convert.ToDateTime(DR["DisposedDate"].ToString()).ToString("dd-MMM-yyyy");
                    lbl_vehicleNumber.Text = DR["VehicleNumber"].ToString();
                    lbl_builtyNumber.Text = DR["BuiltyNo"].ToString();

                    if (lbl_infra_type.Text.Equals("Control Structure1"))
                    {
                        lbl_RD.Visible = false;
                        lbl_D.Visible = false;
                    }

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void Gridattachment(int SDID)
        {
            try
            {

                List<object> lstDisposalAttachment = new FloodFightingPlanBLL().GetFO_SD_Attachment_ID(SDID);

                gv_Attachment.DataSource = lstDisposalAttachment;
                gv_Attachment.DataBind();
                gv_Attachment.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gv_Attachment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    if (gv_Attachment.Rows.Count < 5)
                    {
                        List<object> lstF_SD_Attachment = new FloodFightingPlanBLL().GetFO_SD_Attachment_ID(Convert.ToInt32(hdnSDID.Value));

                        FO_SDImages mdlFO_SD_Attachment = new FO_SDImages();

                        mdlFO_SD_Attachment.ID = 0;
                        mdlFO_SD_Attachment.SDID = 0;
                        mdlFO_SD_Attachment.ImageURL = "";

                        mdlFO_SD_Attachment.CreatedBy = 0;
                        mdlFO_SD_Attachment.CreatedDate = DateTime.Now;
                        lstF_SD_Attachment.Add(mdlFO_SD_Attachment);


                        gv_Attachment.PageIndex = gv_Attachment.PageCount;
                        gv_Attachment.DataSource = lstF_SD_Attachment;
                        gv_Attachment.DataBind();

                        gv_Attachment.EditIndex = gv_Attachment.Rows.Count - 1;
                        gv_Attachment.DataBind();
                    }
                    else
                    {
                        Master.ShowMessage("More than five attachments are not allowed.");
                    }

                }
                if (e.CommandName == "image")
                {
                    GridViewRow gvrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gv_Attachment.DataKeys[gvrow.RowIndex];
                    //     string URL = Convert.ToString(key["ImageURL"]).Substring(Convert.ToString(key["ImageURL"]).LastIndexOf('_') + 1);
                    string URL = Convert.ToString(key["ImageURL"]);
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
        protected void gv_Attachment_PreRender(object sender, EventArgs e)
        {
            try
            {
                Gridattachment(Convert.ToInt32(hdnSDID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                bool IsSave = false;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataKey key = gv_Attachment.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                GridViewRow row = gv_Attachment.Rows[e.RowIndex];
                HyperLink hypFileName = (HyperLink)row.FindControl("hypFileName");
                FileUpload FileUpload = (FileUpload)row.FindControl("fileupdloadID");

                FO_SDImages ObjModel = new FO_SDImages();

                string fileName = "";


                if (FileUpload.HasFile)
                {
                    string file_ext = System.IO.Path.GetExtension(FileUpload.FileName).ToUpper();
                    if (file_ext == ".JPG" || file_ext == ".JPEG" || file_ext == ".PNG" || file_ext == ".GIF")
                    {

                        string guidID = Guid.NewGuid().ToString();
                        fileName = guidID + "FO_SD_" + hdnSDID.Value + "_" + System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);

                        string path = Utility.GetImagePath(Configuration.FloodOperations);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string finalpath = path + "\\\\" + fileName;
                        FileUpload.PostedFile.SaveAs(finalpath);

                        ObjModel.ID = Convert.ToInt64(ID);
                        ObjModel.SDID = Convert.ToInt64(hdnSDID.Value);

                        if (ObjModel.ID == 0)
                        {
                            ObjModel.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            ObjModel.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            ObjModel.CreatedBy = Convert.ToInt32(CreatedBy);
                            ObjModel.CreatedDate = Convert.ToDateTime(CreatedDate);
                        }
                        ObjModel.ImageName = FileUpload.FileName;
                        ObjModel.ImageURL = fileName;
                        IsSave = new FloodFightingPlanBLL().SaveSD_Attachment(ObjModel);
                    }
                    else
                    {
                        Master.ShowMessage("Only Image Upload", SiteMaster.MessageType.Error);
                        Gridattachment(Convert.ToInt32(hdnSDID.Value));
                        return;
                    }
                }
                if (IsSave == true)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjModel.ID) == 0)
                    {
                        gv_Attachment.PageIndex = 0;
                    }

                    gv_Attachment.EditIndex = -1;
                    Gridattachment(Convert.ToInt32(hdnSDID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                int Year = new FloodFightingPlanBLL().GetYearByStonePositioID(_FFPSPID);

                if (e.Row.RowType == DataControlRowType.Header)
                {

                    btnAdd.Enabled = false;

                    if (new FloodOperationsBLL().CanAddEditStoneDeployment(Year, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnAdd.Enabled = true;
                    }
                
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) && DateTime.Now.Year == Year)
                    //{
                    //    btnAdd.Enabled = true;
                    //}
                    //else
                    //{
                    //    btnAdd.Enabled = false;
                    //}
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gv_Attachment.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string SDID = Convert.ToString(key.Values["SDID"]);
                    string ImageURL = Convert.ToString(key.Values["ImageURL"]);
                    LinkButton lnkimage = (LinkButton)e.Row.FindControl("lnkimage");
                    FileUpload fileupdloadID = (FileUpload)e.Row.FindControl("fileupdloadID");
                    // if (e.Row.RowState.Equals(DataControlRowState.Edit) || e.Row.RowState.Equals(DataControlRowState.Alternate))
                    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                    {
                        Button btnSave = (Button)e.Row.FindControl("btnSave");
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
                    }
                    string URL = ImageURL.Substring(ImageURL.LastIndexOf('_') + 1);
                    lnkimage.Text = URL;


                    btnEdit.Enabled = false;
                    
                    if (new FloodOperationsBLL().CanAddEditStoneDeployment(Year, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnEdit.Enabled = true;
                    }

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) && DateTime.Now.Year == Year)
                    //{
                    //    btnEdit.Enabled = true;

                    //    //CanAdd = true;
                    //    //CanEdit = true;
                    //    //CanDelete = true;
                    //}
                    //else
                    //{
                    //    btnEdit.Enabled = false;
                    //    //CanAdd = false;
                    //    //CanEdit = false;
                    //    //CanDelete = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_Attachment.EditIndex = e.NewEditIndex;
                Gridattachment(Convert.ToInt32(hdnSDID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gv_Attachment.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodFightingPlanBLL().DeleteFo_SD_Attachement(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    Gridattachment(Convert.ToInt32(hdnSDID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_Attachment.PageIndex = e.NewPageIndex;

                Gridattachment(Convert.ToInt32(hdnSDID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv_Attachment.EditIndex = -1;

                Gridattachment(Convert.ToInt32(hdnSDID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_Attachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv_Attachment.EditIndex = -1;
                Gridattachment(Convert.ToInt32(hdnSDID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}