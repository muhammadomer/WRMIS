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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint
{
    public partial class AttachmentsJoint : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
                    long FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (FloodInspectionID > 0)
                    {
                        hdnFloodInspectionsID.Value = Convert.ToString(FloodInspectionID);
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(FloodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/joint/SearchJoint.aspx?FloodInspectionID={0}", FloodInspectionID);
                        JointInspectionDetail._FloodInspectionID = FloodInspectionID;
                        LoadAttachmentsJointInformation(FloodInspectionID);
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region SetPageTitle
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        #endregion

        #region Load Attachments JointInspection Grid
        private void LoadAttachmentsJointInformation(long _FloodInspectionID)
        {
            try
            {
                List<object> lstJointAttachmentsDetails = new FloodInspectionsBLL().GetJointAttachmentsDetails(_FloodInspectionID);

                gvAttachmentsJointInspection.DataSource = lstJointAttachmentsDetails;
                gvAttachmentsJointInspection.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion

        #region Attachments JointInspection GridView Method
        protected void gvAttachmentsJointInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAttachmentsJointInspection.EditIndex = -1;
                LoadAttachmentsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvAttachmentsJointInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddAttachmentsJointInspection")

                    if (gvAttachmentsJointInspection.Rows.Count < 5)
                    {
                        List<object> lstJointMembers = new FloodInspectionsBLL().GetJointAttachmentsDetails(Convert.ToInt64(hdnFloodInspectionsID.Value));
                        lstJointMembers.Add(new
                        {
                            ID = 0,
                            FileURL = string.Empty,
                            Department = string.Empty,
                            CreatedBy = 0,
                            CreatedDate = DateTime.Now
                        });

                        gvAttachmentsJointInspection.PageIndex = gvAttachmentsJointInspection.PageCount;
                        gvAttachmentsJointInspection.DataSource = lstJointMembers;
                        gvAttachmentsJointInspection.DataBind();

                        gvAttachmentsJointInspection.EditIndex = gvAttachmentsJointInspection.Rows.Count - 1;
                        gvAttachmentsJointInspection.DataBind();
                    }
                    else
                    {
                        Master.ShowMessage("More than five attachments are not allowed.", SiteMaster.MessageType.Error);
                    }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsJointInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                bool CanEditJoint = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAddRD = (Button)e.Row.FindControl("btnAddAttachmentsJointInspection");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddRD.Enabled = false;
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 1) //For Draft
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                            btnAddRD.Enabled = CanEditJoint;
                        else
                            btnAddRD.Enabled = false;
                    }
                    else
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                            btnAddRD.Enabled = CanEditJoint;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEditAttachmentsJointInspection");
                    //  Button btnDelete = (Button)e.Row.FindControl("btnDeleteAttachmentsJointInspection");

                    #region "Controls"
                    HyperLink hypFileName = (HyperLink)e.Row.FindControl("hypFileName");
                    // Button btnFileUpload = (Button)e.Row.FindControl("btnFileUpload");
                    FileUpload selectedFile = (FileUpload)e.Row.FindControl("selectedFile");

                    #endregion

                    selectedFile.Enabled = false;

                    #region "Data Keys"
                    DataKey key = gvAttachmentsJointInspection.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string FileURL = Convert.ToString(key.Values["FileURL"]);

                    #endregion
                    string URL = Utility.GetImageURL(Configuration.FloodOperations, FileURL);
                    hypFileName.NavigateUrl = URL;

                    if (gvAttachmentsJointInspection.EditIndex == e.Row.RowIndex)
                    {
                        selectedFile.Enabled = true;
                    }

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEdit.Enabled = false;
                        // btnDelete.Enabled = false;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 1) //For Draft
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEdit.Enabled = CanEditJoint;
                            //btnDelete.Enabled = CanEditJoint;
                        }
                        else
                        {
                            btnEdit.Enabled = false;
                            //btnDelete.Enabled = false;

                        }
                    }
                    else
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEdit.Enabled = CanEditJoint;
                            //btnDelete.Enabled = CanEditJoint;
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsJointInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvAttachmentsJointInspection.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteJointAttachments(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    GridViewRow row = gvAttachmentsJointInspection.Rows[e.RowIndex];
                    HyperLink FileName = (HyperLink)row.FindControl("hypFileName");
                    string resurl = Utility.GetImagePath(Configuration.FloodOperations);
                    string finalPath = resurl + "\\\\" + FileName.Text;
                    if (System.IO.File.Exists(finalPath))
                    {
                        System.IO.File.Delete(finalPath);
                    }
                    LoadAttachmentsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsJointInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAttachmentsJointInspection.EditIndex = e.NewEditIndex;
                LoadAttachmentsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsJointInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvAttachmentsJointInspection.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow row = gvAttachmentsJointInspection.Rows[e.RowIndex];
                HyperLink hypFileName = (HyperLink)row.FindControl("hypFileName");
                FileUpload FileUpload = (FileUpload)row.FindControl("selectedFile");
                Button btnFileUpload = (Button)row.FindControl("btnFileUpload");
                #endregion

                FO_JAttachments ObjModel = new FO_JAttachments();

                string fileName = "";


                if (FileUpload.HasFile)
                {
                    // string guidID = Guid.NewGuid().ToString();
                    fileName = "FO_JI_" + hdnFloodInspectionsID.Value + "_" + System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);

                    string path = Utility.GetImagePath(Configuration.FloodOperations);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string finalpath = path + "\\\\" + fileName;
                    FileUpload.PostedFile.SaveAs(finalpath);

                    ObjModel.ID = Convert.ToInt64(ID);
                    ObjModel.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

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
                }

                bool IsSave = new FloodInspectionsBLL().SaveJointAttachments(ObjModel);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjModel.ID) == 0)
                        gvAttachmentsJointInspection.PageIndex = 0;

                    gvAttachmentsJointInspection.EditIndex = -1;
                    LoadAttachmentsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsJointInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAttachmentsJointInspection.PageIndex = e.NewPageIndex;
                gvAttachmentsJointInspection.EditIndex = -1;
                LoadAttachmentsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Attachments JointInspection GridView Method

    }

}