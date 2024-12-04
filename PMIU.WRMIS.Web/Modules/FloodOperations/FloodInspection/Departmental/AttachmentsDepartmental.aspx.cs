using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental
{
    public partial class AttachmentsDepartmental : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);

                    if (_FloodInspectionID > 0)
                    {
                        DepartmentalInspectionDetail1.FloodInspectionIDProp = _FloodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(_FloodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?FloodInspectionID={0}", _FloodInspectionID);
                        BindAttachmentsGrid(_FloodInspectionID);
                    }
                    //  hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?ShowHistory=true");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindAttachmentsGrid(long _FloodInspectionID)
        {
            try
            {
                bool CanEditDep = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                List<object> lstAttachments = new FloodInspectionsBLL().GetAttachmentsByFloodInspectionID(_FloodInspectionID);

                gvAttachmentsDFI.DataSource = lstAttachments;
                gvAttachmentsDFI.DataBind();
                gvAttachmentsDFI.Visible = true;
                Button btn = (Button)gvAttachmentsDFI.HeaderRow.FindControl("btnAddAttachments");
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    btn.Enabled = false;
                    DisableEditDeleteColumn(gvAttachmentsDFI);
                }
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 1)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 1);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvAttachmentsDFI);
                    }
                    else
                    {
                        btn.Enabled = false;
                        DisableEditDeleteColumn(gvAttachmentsDFI);

                    }
                }
                else if (Convert.ToInt32(hdnInspectionStatus.Value) == 1)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 2);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvAttachmentsDFI);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDeleteAttachments = (Button)_e.Row.FindControl("btnDeleteAttachments");
            if (btnDeleteAttachments != null)
            {
                btnDeleteAttachments.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditAttachments = (Button)r.FindControl("btnEditAttachments");
                btnEditAttachments.Enabled = false;

                //Button btnDeleteAttachments = (Button)r.FindControl("btnDeleteAttachments");
                //btnDeleteAttachments.Enabled = false;
            }
        }
        private void EnableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditAttachments = (Button)r.FindControl("btnEditAttachments");
                btnEditAttachments.Enabled = true;

                //Button btnDeleteAttachments = (Button)r.FindControl("btnDeleteAttachments");
                //btnDeleteAttachments.Enabled = false;
            }
        }

        #endregion

        #region Events
        protected void gvAttachmentsDFI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAttachmentsDFI.PageIndex = e.NewPageIndex;
                gvAttachmentsDFI.EditIndex = -1;
                BindAttachmentsGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAttachmentsDFI.EditIndex = -1;
                BindAttachmentsGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddAttachments")
                {
                    if (gvAttachmentsDFI.Rows.Count < 5)
                    {
                        List<object> lstAttachments = new FloodInspectionsBLL().GetAttachmentsByFloodInspectionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                        lstAttachments.Add(new
                        {
                            ID = 0,
                            FileName = string.Empty,
                            CreatedBy = 0,
                            CreatedDate = DateTime.Now
                        });

                        gvAttachmentsDFI.PageIndex = gvAttachmentsDFI.PageCount;
                        gvAttachmentsDFI.DataSource = lstAttachments;
                        gvAttachmentsDFI.DataBind();

                        gvAttachmentsDFI.EditIndex = gvAttachmentsDFI.Rows.Count - 1;
                        gvAttachmentsDFI.DataBind();
                    }
                    else
                    {
                        Master.ShowMessage("More than five attachments are not allowed.", SiteMaster.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // AddDeletionConfirmMessage(e);

                    #region "Controls"
                    FileUpload file = (FileUpload)e.Row.FindControl("FileUpload2");
                    file.Attributes.Add("OnChange", "CheckFileType(this)");
                    HyperLink hlFileName = (HyperLink)e.Row.FindControl("hlFileName");
                    #endregion

                    file.Enabled = false;

                    #region "Data Keys"
                    DataKey key = gvAttachmentsDFI.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string FileName = Convert.ToString(key.Values["FileName"]);
                    #endregion
                    string resurl = Utility.GetImageURL(Configuration.FloodOperations, FileName);

                    hlFileName.NavigateUrl = resurl;

                    if (gvAttachmentsDFI.EditIndex == e.Row.RowIndex)
                    {
                        file.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvAttachmentsDFI.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteAttachments(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    GridViewRow row = gvAttachmentsDFI.Rows[e.RowIndex];
                    HyperLink hlFileName = (HyperLink)row.FindControl("hlFileName");
                    string resurl = Utility.GetImagePath(Configuration.FloodOperations);
                    string finalPath = resurl + "\\\\" + hlFileName.Text;
                    if (System.IO.File.Exists(finalPath))
                    {
                        System.IO.File.Delete(finalPath);
                    }
                    BindAttachmentsGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAttachmentsDFI.EditIndex = e.NewEditIndex;
                BindAttachmentsGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAttachmentsDFI_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"
                DataKey key = gvAttachmentsDFI.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvAttachmentsDFI.Rows[e.RowIndex];
                HyperLink hlFileName = (HyperLink)row.FindControl("hlFileName");
                FileUpload FileUploaded = (FileUpload)row.FindControl("FileUpload2");
                #endregion

                FO_DAttachments dAttachments = new FO_DAttachments();

                string fileName = "";

                if (FileUploaded.HasFile)
                {
                    // string guidID = Guid.NewGuid().ToString();
                    //fileName = guidID + "_FO_DI_" + hdnFloodInspectionsID.Value + "_" + System.IO.Path.GetFileName(FileUploaded.PostedFile.FileName);
                    fileName = "FO_DI_" + hdnFloodInspectionsID.Value + "_" + System.IO.Path.GetFileName(FileUploaded.PostedFile.FileName);

                    string respath = Utility.GetImagePath(Configuration.FloodOperations);
                    string finalpath = respath + "\\\\" + fileName;
                    FileUploaded.PostedFile.SaveAs(finalpath);

                    dAttachments.ID = Convert.ToInt64(ID);
                    dAttachments.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                    if (dAttachments.ID == 0)
                    {
                        dAttachments.CreatedBy = Convert.ToInt32(mdlUser.ID);
                        dAttachments.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        dAttachments.CreatedBy = Convert.ToInt32(CreatedBy);
                        dAttachments.CreatedDate = Convert.ToDateTime(CreatedDate);
                        dAttachments.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                        dAttachments.ModifiedDate = DateTime.Now;
                    }
                    dAttachments.FileURL = fileName;
                }

                bool IsSave = new FloodInspectionsBLL().SaveAttachments(dAttachments);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dAttachments.ID) == 0)
                        gvAttachmentsDFI.PageIndex = 0;

                    gvAttachmentsDFI.EditIndex = -1;
                    BindAttachmentsGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}