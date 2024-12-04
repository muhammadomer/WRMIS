using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class Offices : BasePage
    {
        List<UA_Organization> lstOffice = new List<UA_Organization>();
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

        protected void gvOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstOffice = new OfficeBLL().GetAllOffices();
                    UA_Organization mdlOffice = new UA_Organization();

                    mdlOffice.ID = 0;
                    mdlOffice.Name = "";
                    mdlOffice.Description = "";
                    lstOffice.Add(mdlOffice);

                    gvOffice.PageIndex = gvOffice.PageCount;
                    gvOffice.DataSource = lstOffice;
                    gvOffice.DataBind();

                    gvOffice.EditIndex = gvOffice.Rows.Count - 1;
                    gvOffice.DataBind();
                    gvOffice.Rows[gvOffice.Rows.Count - 1].FindControl("txtOfficeName").Focus();
                }
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOffice.EditIndex = -1;
                BindGrid();
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long OfficeID = Convert.ToInt32(((Label)gvOffice.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string OfficeName = ((TextBox)gvOffice.Rows[RowIndex].Cells[1].FindControl("txtOfficeName")).Text.Trim();
                string OfficeDescription = ((TextBox)gvOffice.Rows[RowIndex].Cells[2].FindControl("txtOfficeDesc")).Text.Trim();
                if (!IsValidAddEdit(OfficeID, OfficeName))
                {
                    return;
                }
                UA_Organization mdlOrganization = new UA_Organization();
                mdlOrganization.ID = OfficeID;
                mdlOrganization.Name = OfficeName;
                mdlOrganization.Description = OfficeDescription;

                OfficeBLL bllOffice = new OfficeBLL();

                bool IsRecordSaved = false;
                if (OfficeID == 0)
                {
                    long MaxID = bllOffice.GetMaxIDofOffice();
                    mdlOrganization.ID = MaxID + 1;
                    IsRecordSaved = bllOffice.AddOffice(mdlOrganization);
                }
                else
                {
                    IsRecordSaved = bllOffice.Update(mdlOrganization);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (OfficeID == 0)
                    {
                        gvOffice.PageIndex = 0;
                    }
                    
                    gvOffice.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception Exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOffice.EditIndex = e.NewEditIndex;
                BindGrid();
                gvOffice.Rows[e.NewEditIndex].FindControl("txtOfficeName").Focus();
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long OfficeID = Convert.ToInt32(((Label)gvOffice.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(OfficeID))
                {
                    return;
                }

                OfficeBLL bllOffice = new OfficeBLL();

                bool IsDeleted = bllOffice.DeleteOffice(OfficeID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception Exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOffice.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 11-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Office);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds Offices to the Grid
        /// Created On: 11-11-2015
        /// </summary>
        private void BindGrid()
        {
            lstOffice = new OfficeBLL().GetAllOffices();
            gvOffice.DataSource = lstOffice;
            gvOffice.DataBind();
        }

        /// <summary>
        /// this function checks weather data is valid for add/edit operation.
        /// Created On:11-11-2015
        /// </summary>
        /// <param name="_OfficeID"></param>
        /// <param name="_OfficeName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _OfficeID, string _OfficeName)
        {
            OfficeBLL bllOffice = new OfficeBLL();

            UA_Organization mdlSearchedOffice = bllOffice.GetOfficeByName(_OfficeName);

            if (mdlSearchedOffice != null && _OfficeID != mdlSearchedOffice.ID)
            {
                Master.ShowMessage(Message.OfficeNameExists.Description, SiteMaster.MessageType.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// this function checks that weather data is valid for delete opeation or not.
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_OfficeID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _OfficeID)
        {
            OfficeBLL bllOffice = new OfficeBLL();
            bool IsExist = bllOffice.IsOfficeIdExists(_OfficeID);

            if (IsExist)
            {
                Master.ShowMessage(Message.OfficeReferenceExists.Description, SiteMaster.MessageType.Error);
                return false;
            }

            return true;
        }

        protected void gvOffice_PageIndexChanged(object sender, EventArgs e)
        {
            gvOffice.EditIndex = -1;
            BindGrid();
        }

        protected void gvOffice_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAdd = (Button)e.Row.FindControl("btnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
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