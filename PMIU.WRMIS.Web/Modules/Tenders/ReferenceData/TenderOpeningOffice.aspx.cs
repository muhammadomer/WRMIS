using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.Tenders;
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

namespace PMIU.WRMIS.Web.Modules.Tenders.ReferenceData
{
    public partial class TenderOpeningOffice : BasePage
    {
        List<TM_TenderOpeningOffice> lstTenderOpeningOffice = new List<TM_TenderOpeningOffice>();
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
            catch(Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstTenderOpeningOffice = new TenderManagementBLL().GetAllTenderOpeningOffices();
                    TM_TenderOpeningOffice mdlTenderOpeningOffice = new TM_TenderOpeningOffice();

                    mdlTenderOpeningOffice.ID = 0;
                    mdlTenderOpeningOffice.Name = "";
                    mdlTenderOpeningOffice.Address = "";
                    lstTenderOpeningOffice.Add(mdlTenderOpeningOffice);

                    gvTenderOpeningOffice.PageIndex = gvTenderOpeningOffice.PageCount;
                    gvTenderOpeningOffice.DataSource = lstTenderOpeningOffice;
                    gvTenderOpeningOffice.DataBind();

                    gvTenderOpeningOffice.EditIndex = gvTenderOpeningOffice.Rows.Count - 1;
                    gvTenderOpeningOffice.DataBind();
                    gvTenderOpeningOffice.Rows[gvTenderOpeningOffice.Rows.Count - 1].FindControl("txtOfficeName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTenderOpeningOffice.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long TenderOpeningOfficeID = Convert.ToInt32(((Label)gvTenderOpeningOffice.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string TenderOpeningOfficeName = ((TextBox)gvTenderOpeningOffice.Rows[RowIndex].Cells[1].FindControl("txtOfficeName")).Text.Trim();
                string Address = ((TextBox)gvTenderOpeningOffice.Rows[RowIndex].Cells[2].FindControl("txtAddress")).Text.Trim();

                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                TM_TenderOpeningOffice mdlSearchedTenderOpeningOffice = bllTenderManagement.GetTenderOpeningOfficeByName(TenderOpeningOfficeName);

                if (mdlSearchedTenderOpeningOffice != null && TenderOpeningOfficeID != mdlSearchedTenderOpeningOffice.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
                TM_TenderOpeningOffice mdlTenderOpeningOffice = new TM_TenderOpeningOffice();

                mdlTenderOpeningOffice.ID = TenderOpeningOfficeID;
                mdlTenderOpeningOffice.Name = TenderOpeningOfficeName;
                mdlTenderOpeningOffice.Address = Address;
                mdlTenderOpeningOffice.IsActive = true;

                bool IsRecordSaved = false;

                if (TenderOpeningOfficeID == 0)
                {
                    mdlTenderOpeningOffice.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlTenderOpeningOffice.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.AddTenderOpeningOffice(mdlTenderOpeningOffice);
                }
                else
                {
                    mdlTenderOpeningOffice.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlTenderOpeningOffice.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.UpdateTenderOpeningOffice(mdlTenderOpeningOffice);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (TenderOpeningOfficeID == 0)
                    {
                        gvTenderOpeningOffice.PageIndex = 0;
                    }
                    gvTenderOpeningOffice.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTenderOpeningOffice.EditIndex = e.NewEditIndex;
                BindGrid();
                gvTenderOpeningOffice.Rows[e.NewEditIndex].FindControl("txtOfficeName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int TenderOpeningOfficeID = Convert.ToInt32(((Label)gvTenderOpeningOffice.Rows[e.RowIndex].FindControl("lblID")).Text);
                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                bool IsExist = bllTenderManagement.IsTenderOpeningOfficeIDExists(TenderOpeningOfficeID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllTenderManagement.DeleteTenderOpeningOffice(TenderOpeningOfficeID);

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

        protected void gvTenderOpeningOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTenderOpeningOffice.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderOpeningOffice_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvTenderOpeningOffice.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            lstTenderOpeningOffice = new TenderManagementBLL().GetAllTenderOpeningOffices();
            gvTenderOpeningOffice.DataSource = lstTenderOpeningOffice;
            gvTenderOpeningOffice.DataBind();
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}