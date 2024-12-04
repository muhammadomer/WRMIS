using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint
{
    public partial class InfrastructuresJoint : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                long FloodInspectionID = 0;
                if (!IsPostBack)
                {
                    SetPageTitle();
                    FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (FloodInspectionID > 0)
                    {
                        hdnFloodInspectionsID.Value = Convert.ToString(FloodInspectionID);
                        hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(FloodInspectionID)).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/joint/SearchJoint.aspx?FloodInspectionID={0}", FloodInspectionID);
                        JointInspectionDetail._FloodInspectionID = FloodInspectionID;
                        LoadInfrastructuresJointInformation(FloodInspectionID);
                    }


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #region Set PageTitle
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion

        #region LoadGrid
        private void LoadInfrastructuresJointInformation(long _FloodInspectionID)
        {
            try
            {
                List<object> lstInfrastructures = new FloodInspectionsBLL().GetInfrastructuresForJointInspection(_FloodInspectionID);
                gvInfrastructuresJoint.DataSource = lstInfrastructures;
                gvInfrastructuresJoint.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Infrastructures Joint Gridview Method


        protected void gvInfrastructuresJoint_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInfrastructuresJoint.PageIndex = e.NewPageIndex;
                gvInfrastructuresJoint.EditIndex = -1;
                LoadInfrastructuresJointInformation(Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvInfrastructuresJoint.EditIndex = -1;
                LoadInfrastructuresJointInformation(Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddInfrastructuresJoint")
                {
                    List<object> lstInfrastructuresJointInformation = new FloodInspectionsBLL().GetInfrastructuresForJointInspection(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    lstInfrastructuresJointInformation.Add(new
                    {
                        ID = 0,
                        StructureTypeID = string.Empty,
                        StructureID = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvInfrastructuresJoint.PageIndex = gvInfrastructuresJoint.PageCount;
                    gvInfrastructuresJoint.DataSource = lstInfrastructuresJointInformation;
                    gvInfrastructuresJoint.DataBind();

                    gvInfrastructuresJoint.EditIndex = gvInfrastructuresJoint.Rows.Count - 1;
                    gvInfrastructuresJoint.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvInfrastructuresJoint.DataKeys[e.RowIndex].Values[0]);

                bool isDeleted = new FloodInspectionsBLL().DeleteJointInspectionInfrastructure(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    LoadInfrastructuresJointInformation(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvInfrastructuresJoint.EditIndex = e.NewEditIndex;
                LoadInfrastructuresJointInformation(Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvInfrastructuresJoint.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow row = gvInfrastructuresJoint.Rows[e.RowIndex];
                DropDownList ddlInfrastructureType = (DropDownList)row.FindControl("ddlInfrastructureType");
                DropDownList ddlInfrastructureName = (DropDownList)row.FindControl("ddlInfrastructureName");

                #endregion

                FO_JInfrastructures ObjJInfrastructure = new FO_JInfrastructures();

                ObjJInfrastructure.ID = Convert.ToInt64(ID);
                ObjJInfrastructure.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (ddlInfrastructureType != null)
                    ObjJInfrastructure.StructureTypeID = Convert.ToInt16(ddlInfrastructureType.SelectedItem.Value);

                if (ddlInfrastructureName != null)
                    ObjJInfrastructure.StructureID = Convert.ToInt16(ddlInfrastructureName.SelectedItem.Value);


                if (ObjJInfrastructure.ID == 0)
                {
                    ObjJInfrastructure.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjJInfrastructure.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjJInfrastructure.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjJInfrastructure.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjJInfrastructure.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjJInfrastructure.ModifiedDate = DateTime.Now;
                }

                if (ObjJInfrastructure.ID == 0)
                {
                    if (new FloodInspectionsBLL().IsJIFInfrastructureExist(ObjJInfrastructure))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else
                {
                    if (new FloodInspectionsBLL().IsJIFInfrastructureExistUpdate(ObjJInfrastructure))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                }



                bool IsSave = new FloodInspectionsBLL().SaveJointInfrastructure(ObjJInfrastructure);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjJInfrastructure.ID) == 0)
                        gvInfrastructuresJoint.PageIndex = 0;

                    gvInfrastructuresJoint.EditIndex = -1;
                    LoadInfrastructuresJointInformation(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructuresJoint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                bool CanEditJoint = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAddInfrastructuresJoint = (Button)e.Row.FindControl("btnAddInfrastructuresJoint");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddInfrastructuresJoint.Enabled = false;
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 1) //For Draft
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                            btnAddInfrastructuresJoint.Enabled = CanEditJoint;
                        else
                            btnAddInfrastructuresJoint.Enabled = false;
                    }
                    else
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                            btnAddInfrastructuresJoint.Enabled = CanEditJoint;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region DataKey
                    DataKey key = gvInfrastructuresJoint.DataKeys[e.Row.RowIndex];
                    string InfrastructuresTypeID = Convert.ToString(key.Values["StructureTypeID"]);
                    string InfrastructuresNameID = Convert.ToString(key.Values["StructureID"]);

                    Label lblInfrastructureType = (Label)e.Row.FindControl("lblInfrastructureType");
                    Label lblInfrastructureName = (Label)e.Row.FindControl("lblInfrastructureName");
                    #endregion

                    Button btnEditInfrastructuresJoint = (Button)e.Row.FindControl("btnEditInfrastructuresJoint");
                    Button btnDeleteInfrastructuresJoint = (Button)e.Row.FindControl("btnDeleteInfrastructuresJoint");


                    if (InfrastructuresTypeID != null && InfrastructuresTypeID != "" && InfrastructuresNameID != null && InfrastructuresNameID != "")
                    {
                        object ObjInfrastructure = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(Convert.ToInt64(InfrastructuresTypeID), Convert.ToInt64(InfrastructuresNameID));
                        if (ObjInfrastructure != null)
                        {
                            if (lblInfrastructureType != null)
                                lblInfrastructureType.Text = Convert.ToString(Utility.GetDynamicPropertyValue(ObjInfrastructure, "InfrastructureTypeName")) == "Control Structure1"
                                    ? "Barrage/Headwork" : Convert.ToString(Utility.GetDynamicPropertyValue(ObjInfrastructure, "InfrastructureTypeName"));
                            if (lblInfrastructureName != null)
                                lblInfrastructureName.Text = Convert.ToString(Utility.GetDynamicPropertyValue(ObjInfrastructure, "InfrastructureName"));
                        }
                    }
                    if (gvInfrastructuresJoint.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlInfrastructureType = (DropDownList)e.Row.FindControl("ddlInfrastructureType");
                        DropDownList ddlInfrastructureName = (DropDownList)e.Row.FindControl("ddlInfrastructureName");
                        if (ddlInfrastructureType != null)
                        {
                            Dropdownlist.DDLInfrastructureType(ddlInfrastructureType);
                            if (!string.IsNullOrEmpty(Convert.ToString(InfrastructuresTypeID)))
                                Dropdownlist.SetSelectedValue(ddlInfrastructureType, Convert.ToString(InfrastructuresTypeID));
                        }
                        if (ddlInfrastructureType != null)
                        {
                            if (ddlInfrastructureType.SelectedItem.Value != "")
                            {
                                UA_Users _Users = SessionManagerFacade.UserInformation;
                                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                                if (InfrastructureTypeSelectedValue == 1)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                                else if (InfrastructureTypeSelectedValue == 2)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                                else if (InfrastructureTypeSelectedValue == 3)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(InfrastructuresNameID)))
                                Dropdownlist.SetSelectedValue(ddlInfrastructureName, Convert.ToString(InfrastructuresNameID));
                        }

                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditInfrastructuresJoint.Enabled = false;
                        btnDeleteInfrastructuresJoint.Enabled = false;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 1) //For Draft
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEditInfrastructuresJoint.Enabled = CanEditJoint;
                            btnDeleteInfrastructuresJoint.Enabled = CanEditJoint;
                        }
                        else
                        {
                            btnEditInfrastructuresJoint.Enabled = false;
                            btnDeleteInfrastructuresJoint.Enabled = false;

                        }
                    }
                    else
                    {
                        CanEditJoint = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value));
                        if (CanEditJoint)
                        {
                            btnEditInfrastructuresJoint.Enabled = CanEditJoint;
                            btnDeleteInfrastructuresJoint.Enabled = CanEditJoint;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {

            UA_Users _Users = SessionManagerFacade.UserInformation;

            DropDownList ddlListist = (DropDownList)sender;
            GridViewRow gvrow = (GridViewRow)ddlListist.NamingContainer;

            DropDownList ddlInfrastructureType = (DropDownList)gvrow.FindControl("ddlInfrastructureType");
            DropDownList ddlInfrastructureName = (DropDownList)gvrow.FindControl("ddlInfrastructureName");

            if (ddlInfrastructureType.SelectedItem.Value != "")
            {

                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
            }
            else
            {
                ddlInfrastructureName.Items.Clear();
            }

        }

        #endregion
    }
}