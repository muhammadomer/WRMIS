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
    public partial class Contractors : BasePage
    {
        List<TM_Contractors> lstContractors = new List<TM_Contractors>();
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

        protected void gvContractors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstContractors = new TenderManagementBLL().GetAllContractors();
                    TM_Contractors mdlContractor = new TM_Contractors();

                    mdlContractor.ID = 0;
                    mdlContractor.CompanyName = "";
                    mdlContractor.ContactPerson = "";
                    mdlContractor.ContactNo = "";
                    mdlContractor.Address = "";
                    lstContractors.Add(mdlContractor);

                    gvContractors.PageIndex = gvContractors.PageCount;
                    gvContractors.DataSource = lstContractors;
                    gvContractors.DataBind();

                    gvContractors.EditIndex = gvContractors.Rows.Count - 1;
                    gvContractors.DataBind();
                    gvContractors.Rows[gvContractors.Rows.Count - 1].FindControl("txtContractorName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvContractors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvContractors.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvContractors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long ContractorID = Convert.ToInt32(((Label)gvContractors.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string CompanyName = ((TextBox)gvContractors.Rows[RowIndex].Cells[1].FindControl("txtContractorName")).Text.Trim();
                string ContactPersonName = ((TextBox)gvContractors.Rows[RowIndex].Cells[2].FindControl("txtContactPersonName")).Text.Trim();
                string ContactNo = ((TextBox)gvContractors.Rows[RowIndex].Cells[3].FindControl("txtContactNo")).Text.Trim();
                string Address = ((TextBox)gvContractors.Rows[RowIndex].Cells[4].FindControl("txtAddress")).Text.Trim();

                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                TM_Contractors mdlSearchedContractors = bllTenderManagement.GetContractorByName(CompanyName);

                if (mdlSearchedContractors != null && ContractorID != mdlSearchedContractors.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }

                mdlSearchedContractors = bllTenderManagement.GetContractorByContactNumber(ContactNo);

                if (mdlSearchedContractors != null && ContractorID != mdlSearchedContractors.ID)
                {
                    Master.ShowMessage(Message.ContactNoExists.Description, SiteMaster.MessageType.Error);
                    return;
                }

                TM_Contractors mdlContractor = new TM_Contractors();

                mdlContractor.ID = ContractorID;
                mdlContractor.CompanyName = CompanyName;
                mdlContractor.ContactPerson = ContactPersonName;
                mdlContractor.ContactNo = ContactNo;
                mdlContractor.Address = Address;
                mdlContractor.IsActive = true;

                bool IsRecordSaved = false;

                if (ContractorID == 0)
                {
                    mdlContractor.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlContractor.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.AddContractor(mdlContractor);
                }
                else
                {
                    mdlContractor.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlContractor.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.UpdateContractor(mdlContractor);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (ContractorID == 0)
                    {
                        gvContractors.PageIndex = 0;
                    }
                    gvContractors.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvContractors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvContractors.EditIndex = e.NewEditIndex;
                BindGrid();
                gvContractors.Rows[e.NewEditIndex].FindControl("txtContractorName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvContractors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ContractorID = Convert.ToInt32(((Label)gvContractors.Rows[e.RowIndex].FindControl("lblID")).Text);
                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                bool IsExist = bllTenderManagement.IsContractorExist(ContractorID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllTenderManagement.DeleteContractor(ContractorID);

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

        protected void gvContractors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvContractors.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvContractors_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvContractors.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            lstContractors = new TenderManagementBLL().GetAllContractors();
            gvContractors.DataSource = lstContractors;
            gvContractors.DataBind();
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