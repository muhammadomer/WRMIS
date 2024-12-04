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
using PMIU.WRMIS.BLL.ClosureOperations;
 

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ReferenceData
{
    public delegate void DelegatePopulateData();
    public partial class TechnicalSanctionUnits : BasePage
    {
        //Data Members 
        Dictionary<string, object> unit_data = new Dictionary<string, object>();
        List<CW_TechnicalSanctionUnit> lstUnits = new List<CW_TechnicalSanctionUnit>();

        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (!IsPostBack) {
                    SetPageTitle();
                    BindGrid();
                } 
            }
            catch (Exception exp) {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvUnits_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstUnits = new ClosureOperationsBLL().GetAllUnit();
                    CW_TechnicalSanctionUnit mdlUnit = new CW_TechnicalSanctionUnit();

                    mdlUnit.ID = 0;
                    mdlUnit.Name = "";
                    mdlUnit.Description = "";
                    mdlUnit.IsActive = true;
                    lstUnits.Add(mdlUnit);
                     
                    gvUnits.PageIndex = gvUnits.PageCount;
                    gvUnits.DataSource = lstUnits;
                    gvUnits.DataBind();

                    gvUnits.EditIndex = gvUnits.Rows.Count - 1;
                    gvUnits.DataBind();
                    gvUnits.Rows[gvUnits.Rows.Count - 1].FindControl("txtName").Focus(); 
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUnits.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvUnits_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            try { 
                ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
                long unitID = Convert.ToInt32(((Label)gvUnits.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (bllCO.UnitAssociationExists(unitID)) {// check for unit associated with other modules 
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error); 
                    return;
                }

                unit_data.Clear();      
                unit_data.Add("ID", unitID);

                if ((bool)bllCO.TechnicalSanctionUnit_Operations(Constants.CRUD_DELETE, unit_data))
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp) {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvUnits_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            try {  
                ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
                GridViewRow row =  gvUnits.Rows[e.RowIndex] ;
                long unitID = Convert.ToInt32(((Label)row.FindControl("lblID")).Text);
                string unitName = ((TextBox)row.FindControl("txtName")).Text.Trim();
                bool isActive = ((CheckBox)row.FindControl("cb_Active")).Checked;
                unit_data.Clear();
                unit_data.Add("Name" , unitName);
                CW_TechnicalSanctionUnit mdlSearchedReasonForChange =
                    (CW_TechnicalSanctionUnit)bllCO.TechnicalSanctionUnit_Operations(Constants.CRUD_READ, unit_data);
              
                if (mdlSearchedReasonForChange != null && unitID != mdlSearchedReasonForChange.ID) {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                string unitDesc = ((TextBox)row.Cells[2].FindControl("txtDesc")).Text.Trim();
                unit_data.Clear();
                unit_data.Add("ID", unitID);
                unit_data.Add("Name", unitName);
                unit_data.Add("Description", unitDesc);
                unit_data.Add("IsActive", isActive);
                unit_data.Add("UserID", SessionManagerFacade.UserInformation.ID);

                bool status = false; 
                if (unitID == 0) {
                    status = (bool)bllCO.TechnicalSanctionUnit_Operations(Constants.CRUD_CREATE, unit_data);
                }
                else {
                    status = (bool)bllCO.TechnicalSanctionUnit_Operations(Constants.CRUD_UPDATE, unit_data);
                }

                if (status) {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (unitID == 0) {
                        gvUnits.PageIndex = 0;
                    }
                    gvUnits.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvUnits_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            try {
                gvUnits.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp) {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try {
                gvUnits.EditIndex = e.NewEditIndex;
                BindGrid();
                gvUnits.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp) {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_PageIndexChanged(object sender, EventArgs e) {
            try {
                gvUnits.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp) {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowCreated(object sender, GridViewRowEventArgs e) {
            try {
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
            catch (Exception exp) {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.TechnicalSanctionUnits);
           // Tuple<string, string, string> pageTitleAsset = base.SetPageTitle(PageName.AssetsCategory);
          //  Master.ModuleTitle = pageTitle.Item1 + '/' + pageTitleAsset.Item1;
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;

        }
        private void BindGrid()
        {
            lstUnits = new ClosureOperationsBLL().GetAllUnit();
            gvUnits.DataSource = lstUnits;
            gvUnits.DataBind();
        } 
    }
}