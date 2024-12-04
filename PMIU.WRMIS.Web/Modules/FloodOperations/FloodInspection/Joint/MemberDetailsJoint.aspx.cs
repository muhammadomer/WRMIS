using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint
{
    public partial class MemberDetailsJoint : BasePage
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
                        LoadMemberDetailsJointInformation(FloodInspectionID);
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

        #region Load Grid
        private void LoadMemberDetailsJointInformation(long _FloodInspectionID)
        {
            try
            {
                List<object> lstJointMemberDetails = new FloodInspectionsBLL().GetJointMemberDetails(_FloodInspectionID);

                gvMemberDetailsJointInspection.DataSource = lstJointMemberDetails;
                gvMemberDetailsJointInspection.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion Load Grid

        #region MemberDetails JointInspection GridView Method
        protected void gvMemberDetailsJointInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvMemberDetailsJointInspection.EditIndex = -1;
                LoadMemberDetailsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvMemberDetailsJointInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddMemberDetailsJointInspection")
                {
                    List<object> lstJointMembers = new FloodInspectionsBLL().GetJointMemberDetails(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    lstJointMembers.Add(new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Department = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvMemberDetailsJointInspection.PageIndex = gvMemberDetailsJointInspection.PageCount;
                    gvMemberDetailsJointInspection.DataSource = lstJointMembers;
                    gvMemberDetailsJointInspection.DataBind();

                    gvMemberDetailsJointInspection.EditIndex = gvMemberDetailsJointInspection.Rows.Count - 1;
                    gvMemberDetailsJointInspection.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMemberDetailsJointInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                bool CanEditJoint = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAddRD = (Button)e.Row.FindControl("btnAddMemberDetailsJointInspection");
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
                    Button btnEdit = (Button)e.Row.FindControl("btnEditMemberDetailsJointInspection");
                    Button btnDelete = (Button)e.Row.FindControl("btnDeleteMemberDetailsJointInspection");

                    if (gvMemberDetailsJointInspection.EditIndex == e.Row.RowIndex)
                    {
                        //Button btnEdit = (Button)e.Row.FindControl("btnEditMemberDetailsJointInspection");
                        //Button btnDelete = (Button)e.Row.FindControl("btnDeleteMemberDetailsJointInspection");
                        //if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        //{
                        //    btnEdit.Enabled = false;
                        //    btnDelete.Enabled = false;
                        //}

                        #region "Data Keys"

                        DataKey key = gvMemberDetailsJointInspection.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string Name = Convert.ToString(key.Values["Name"]);
                        string Department = Convert.ToString(key.Values["Department"]);

                        #endregion

                        #region "Controls"

                        Label lblName = (Label)e.Row.FindControl("llblName");
                        Label lblDepartment = (Label)e.Row.FindControl("lblDepartment");
                        TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                        TextBox txtDepartment = (TextBox)e.Row.FindControl("txtDepartment");


                        #endregion

                        if (lblName != null)
                        {
                            lblName.Text = Name;
                        }
                        if (lblDepartment != null)
                        {
                            lblDepartment.Text = Department;
                        }

                        if (txtName != null)
                        {
                            txtName.Text = Name;
                        }
                        if (txtDepartment != null)
                        {
                            txtDepartment.Text = Department;
                        }

                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 1) //For Draft
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEdit.Enabled = CanEditJoint;
                            btnDelete.Enabled = CanEditJoint;
                        }
                        else
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;

                        }
                    }
                    else
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEdit.Enabled = CanEditJoint;
                            btnDelete.Enabled = CanEditJoint;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMemberDetailsJointInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvMemberDetailsJointInspection.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new FloodInspectionsBLL().DeleteJointMemberDetails(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    LoadMemberDetailsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMemberDetailsJointInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvMemberDetailsJointInspection.EditIndex = e.NewEditIndex;
                LoadMemberDetailsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMemberDetailsJointInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvMemberDetailsJointInspection.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow row = gvMemberDetailsJointInspection.Rows[e.RowIndex];
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDepartment = (TextBox)row.FindControl("txtDepartment");

                #endregion

                FO_JMemberDetails ObjJointMemberDetails = new FO_JMemberDetails();

                ObjJointMemberDetails.ID = Convert.ToInt64(ID);
                ObjJointMemberDetails.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (txtName != null)
                    ObjJointMemberDetails.Name = txtName.Text;

                if (txtDepartment != null)
                    ObjJointMemberDetails.Department = txtDepartment.Text;


                if (ObjJointMemberDetails.ID == 0)
                {
                    ObjJointMemberDetails.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjJointMemberDetails.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjJointMemberDetails.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjJointMemberDetails.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjJointMemberDetails.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjJointMemberDetails.ModifiedDate = DateTime.Now;
                }
                if (new FloodInspectionsBLL().IsJointMemberDetailsExist(ObjJointMemberDetails))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new FloodInspectionsBLL().SaveJointMemberDetails(ObjJointMemberDetails);
                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjJointMemberDetails.ID) == 0)
                        gvMemberDetailsJointInspection.PageIndex = 0;

                    gvMemberDetailsJointInspection.EditIndex = -1;
                    LoadMemberDetailsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMemberDetailsJointInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMemberDetailsJointInspection.PageIndex = e.NewPageIndex;
                gvMemberDetailsJointInspection.EditIndex = -1;
                LoadMemberDetailsJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion MemberDetails JointInspection GridView Method
    }
}