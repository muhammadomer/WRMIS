using PMIU.WRMIS.BLL.ScheduleInspection;
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

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.Reference_Data
{
    public partial class AddGeneralInspectionType : BasePage
    {
        List<SI_GeneralInspectionType> lstGeneralInspectionTypes = new List<SI_GeneralInspectionType>();
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

        protected void gvGeneralInspectionTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstGeneralInspectionTypes = new ScheduleInspectionBLL().GetAllGeneralInspectionTypes();
                    SI_GeneralInspectionType mdlGeneralInspectionType = new SI_GeneralInspectionType();

                    mdlGeneralInspectionType.ID = 0;
                    mdlGeneralInspectionType.Name = "";
                    mdlGeneralInspectionType.Description = "";
                    mdlGeneralInspectionType.IsActive = true;
                    lstGeneralInspectionTypes.Add(mdlGeneralInspectionType);

                    gvGeneralInspectionTypes.PageIndex = gvGeneralInspectionTypes.PageCount;
                    gvGeneralInspectionTypes.DataSource = lstGeneralInspectionTypes;
                    gvGeneralInspectionTypes.DataBind();

                    gvGeneralInspectionTypes.EditIndex = gvGeneralInspectionTypes.Rows.Count - 1;
                    gvGeneralInspectionTypes.DataBind();
                    gvGeneralInspectionTypes.Rows[gvGeneralInspectionTypes.Rows.Count - 1].FindControl("txtName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGeneralInspectionTypes.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;

                //Keys
                string CreatedBy = GetDataKeyValue(gvGeneralInspectionTypes, "CreatedBy", RowIndex);
                string CreatedDate = GetDataKeyValue(gvGeneralInspectionTypes, "CreatedDate", RowIndex);


                long GeneralInspectionTypeID = Convert.ToInt32(((Label)gvGeneralInspectionTypes.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string InspectionName = ((TextBox)gvGeneralInspectionTypes.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string Description = ((TextBox)gvGeneralInspectionTypes.Rows[RowIndex].Cells[2].FindControl("txtDescription")).Text.Trim();
                bool isActive = ((CheckBox)gvGeneralInspectionTypes.Rows[RowIndex].Cells[2].FindControl("cb_Active")).Checked;
                ScheduleInspectionBLL bllScheduleInspection = new ScheduleInspectionBLL();
                SI_GeneralInspectionType mdlSearchedInspectionType = bllScheduleInspection.GetGeneralInspectionTypeByName(InspectionName);

                if (mdlSearchedInspectionType != null && GeneralInspectionTypeID != mdlSearchedInspectionType.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
                SI_GeneralInspectionType mdlGeneralInspectiontype = new SI_GeneralInspectionType();

                mdlGeneralInspectiontype.ID = GeneralInspectionTypeID;
                mdlGeneralInspectiontype.Name = InspectionName;
                mdlGeneralInspectiontype.Description = Description;
                mdlGeneralInspectiontype.IsActive = isActive;

                bool IsRecordSaved = false;

                if (GeneralInspectionTypeID == 0)
                {
                    mdlGeneralInspectiontype.CreatedBy = Convert.ToInt64(mdlUser.ID);
                    mdlGeneralInspectiontype.CreatedDate = DateTime.Today;
                    mdlGeneralInspectiontype.ModifiedBy = Convert.ToInt64(mdlUser.ID);
                    mdlGeneralInspectiontype.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllScheduleInspection.AddGeneralInspectionType(mdlGeneralInspectiontype);
                }
                else
                {
                    mdlGeneralInspectiontype.CreatedBy = Convert.ToInt64(CreatedBy);
                    mdlGeneralInspectiontype.CreatedDate = Convert.ToDateTime(CreatedDate);
                    mdlGeneralInspectiontype.ModifiedBy = Convert.ToInt64(mdlUser.ID);
                    mdlGeneralInspectiontype.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllScheduleInspection.AddGeneralInspectionType(mdlGeneralInspectiontype);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (GeneralInspectionTypeID == 0)
                    {
                        gvGeneralInspectionTypes.PageIndex = 0;
                    }
                    gvGeneralInspectionTypes.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvGeneralInspectionTypes.EditIndex = e.NewEditIndex;
                BindGrid();
                gvGeneralInspectionTypes.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int GeneralInspectionTypeID = Convert.ToInt32(((Label)gvGeneralInspectionTypes.Rows[e.RowIndex].FindControl("lblID")).Text);
                ScheduleInspectionBLL bllScheduleInspection = new ScheduleInspectionBLL();
                bool IsExist = bllScheduleInspection.IsGeneralInspectionTypeAssociated(GeneralInspectionTypeID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllScheduleInspection.DeleteGeneralInspectionType(GeneralInspectionTypeID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGeneralInspectionTypes.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGeneralInspectionTypes_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvGeneralInspectionTypes.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindGrid()
        {
            lstGeneralInspectionTypes = new ScheduleInspectionBLL().GetAllGeneralInspectionTypes();
            gvGeneralInspectionTypes.DataSource = lstGeneralInspectionTypes;
            gvGeneralInspectionTypes.DataBind();
        }

        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddGeneralInspection);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}