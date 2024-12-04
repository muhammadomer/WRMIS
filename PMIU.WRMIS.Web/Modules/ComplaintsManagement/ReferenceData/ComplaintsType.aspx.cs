using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement.ReferenceData
{
    public partial class ComplaintsType : BasePage
    {
        List<CM_ComplaintType> lstComplaintType = new List<CM_ComplaintType>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstComplaintType = new ComplaintsManagementBLL().GetAllComplaintsType();
                    CM_ComplaintType mdlComplaintsType = new CM_ComplaintType();

                    mdlComplaintsType.ID = 0;
                    mdlComplaintsType.Name = "";
                    mdlComplaintsType.ResponseTime = null;
                    mdlComplaintsType.Description = "";
                    lstComplaintType.Add(mdlComplaintsType);

                    gvComplaintsType.PageIndex = gvComplaintsType.PageCount;
                    gvComplaintsType.DataSource = lstComplaintType;
                    gvComplaintsType.DataBind();

                    gvComplaintsType.EditIndex = gvComplaintsType.Rows.Count - 1;
                    gvComplaintsType.DataBind();
                    gvComplaintsType.Rows[gvComplaintsType.Rows.Count - 1].FindControl("txtComplaintType").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvComplaintsType.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long ComplaintTypeID = Convert.ToInt32(((Label)gvComplaintsType.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string ComplaintType = ((TextBox)gvComplaintsType.Rows[RowIndex].Cells[1].FindControl("txtComplaintType")).Text.Trim();
                short? ResponseTime;
                if (((TextBox)gvComplaintsType.Rows[RowIndex].Cells[2].FindControl("txtResponseTime")).Text.Trim() == "")
                {
                    ResponseTime = null;
                }
                else
                {
                    ResponseTime = Convert.ToInt16(((TextBox)gvComplaintsType.Rows[RowIndex].Cells[2].FindControl("txtResponseTime")).Text.Trim());
                }
                string ComplaintTypeDescription = ((TextBox)gvComplaintsType.Rows[RowIndex].Cells[3].FindControl("txtComplaintDescription")).Text.Trim();

                ComplaintsManagementBLL bllComplaintsType = new ComplaintsManagementBLL();
                CM_ComplaintType mdlSearchedComplaintType = bllComplaintsType.GetComplaintTypeByName(ComplaintType);

                if (mdlSearchedComplaintType != null && ComplaintTypeID != mdlSearchedComplaintType.ID)
                {
                    Master.ShowMessage(Message.ComplaintsTypeExists.Description, SiteMaster.MessageType.Error);
                    return;
                }
                CM_ComplaintType mdlComplaintType = new CM_ComplaintType();

                mdlComplaintType.ID = ComplaintTypeID;
                mdlComplaintType.Name = ComplaintType;
                mdlComplaintType.ResponseTime = ResponseTime;
                mdlComplaintType.Description = ComplaintTypeDescription;

                bool IsRecordSaved = false;

                if (ComplaintTypeID == 0)
                {
                    long MaxID = bllComplaintsType.GetMaxIDofCompalintType();
                    mdlComplaintType.ID = MaxID + 1;
                    mdlComplaintType.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlComplaintType.CreatedDate = DateTime.Now;
                    mdlComplaintType.IsActive = true;
                    IsRecordSaved = bllComplaintsType.AddComplaintType(mdlComplaintType);
                }
                else
                {
                    mdlComplaintType.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlComplaintType.ModifiedDate = DateTime.Now;
                    IsRecordSaved = bllComplaintsType.UpdateComplaintType(mdlComplaintType);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (ComplaintTypeID == 0)
                    {
                        gvComplaintsType.PageIndex = 0;
                    }
                    gvComplaintsType.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvComplaintsType.EditIndex = e.NewEditIndex;
                BindGrid();
                gvComplaintsType.Rows[e.NewEditIndex].FindControl("txtComplaintType").Focus();

                //for (int i = 1; i < gvComplaintsType.Rows[e.NewEditIndex].Cells.Count; i++)
                //{
                //    if (ComplaintTypeID == (long)Constants.ComplaintType.WaterTheft || ComplaintTypeID == (long)Constants.ComplaintType.RotationalProgram || ComplaintTypeID == (long)Constants.ComplaintType.ShortTail || ComplaintTypeID == (long)Constants.ComplaintType.DryTail || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotPainted || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotPainted)
                //    {
                //        ((TextBox)gvComplaintsType.Rows[e.NewEditIndex].Cells[1].Controls[0]).Attributes.Add("readonly", "true");
                //    } 
                //}
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ComplaintTypeID = Convert.ToInt32(((Label)gvComplaintsType.Rows[e.RowIndex].FindControl("lblID")).Text);
                ComplaintsManagementBLL bllComplaintsManagement = new ComplaintsManagementBLL();
                bool IsExist = bllComplaintsManagement.IsComplaintTypeIDExists(ComplaintTypeID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllComplaintsManagement.DeleteComplaintType(ComplaintTypeID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvComplaintsType.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplaintsType_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvComplaintsType.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function binds Complaint Type to the grid
        /// Created on 06/09/2016
        /// </summary>
        private void BindGrid()
        {
            lstComplaintType = new ComplaintsManagementBLL().GetAllComplaintsType();
            gvComplaintsType.DataSource = lstComplaintType;
            gvComplaintsType.DataBind();
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 06/09/2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ComplaintsType);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvComplaintsType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int ComplaintTypeID = Convert.ToInt32(((Label)e.Row.FindControl("lblID")).Text);
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    ComplaintsManagementBLL bllComplaintsManagement = new ComplaintsManagementBLL();
                    bool IsValidDelete = false;
                    IsValidDelete = bllComplaintsManagement.IsComplaintTypeIDExists(ComplaintTypeID);

                    if (IsValidDelete || ComplaintTypeID == (long)Constants.ComplaintType.WaterTheft || ComplaintTypeID == (long)Constants.ComplaintType.RotationalProgram || ComplaintTypeID == (long)Constants.ComplaintType.ShortTail || ComplaintTypeID == (long)Constants.ComplaintType.DryTail || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotPainted || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotPainted)
                    {
                        if (btnDelete != null)
                            btnDelete.Enabled = false;
                    }


                    if (gvComplaintsType.EditIndex == e.Row.RowIndex)
                    {
                        TextBox txtComplaintType = (TextBox)e.Row.FindControl("txtComplaintType");
                        Label elblComplaintType = (Label)e.Row.FindControl("elblComplaintType");

                        if (ComplaintTypeID == (long)Constants.ComplaintType.WaterTheft || ComplaintTypeID == (long)Constants.ComplaintType.RotationalProgram || ComplaintTypeID == (long)Constants.ComplaintType.ShortTail || ComplaintTypeID == (long)Constants.ComplaintType.DryTail || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.HeadGaugeNotPainted || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotFixed || ComplaintTypeID == (long)Constants.ComplaintType.TailGaugeNotPainted)
                        {
                            txtComplaintType.Visible = false;
                            elblComplaintType.Visible = true;
                        }
                    }
                }



            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}