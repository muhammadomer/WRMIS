using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement.ReferenceData
{
    public partial class AdditionalAccessibility : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadOrgnization();
                    //EnableDisableControls(false);
                    btnSave.Visible = base.CanAdd;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AdditionalAccessibility);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadOrgnization()
        {
            try
            {
                List<UA_Organization> lstOrganization = new  ComplaintsManagementBLL().GetAllOrganization();
                Dropdownlist.BindDropdownlist<List<UA_Organization>>(ddlOrganization, lstOrganization);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlOrgnization_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ID = ddlOrganization.SelectedItem.Value;
                if (string.IsNullOrEmpty(ID))
                {
                    EnableDisableControls(false);
                    lstBoxDesignations.Items.Clear();
                }
                else
                {
                   // EnableDisableControls(true);

                    long OrganizationID = Convert.ToInt64(ID);
                    LoadUnAssignedDesignations(OrganizationID);
                    LoadAssignedDesignations(OrganizationID);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadUnAssignedDesignations(long _OrganizationID)
        {
            try
            {
                List<object> lstDesignation = new ComplaintsManagementBLL().GetUnAssignedDesignations(_OrganizationID);
                lstBoxDesignations.DataValueField = "ID";
                lstBoxDesignations.DataTextField = "Designation";
                lstBoxDesignations.DataSource = lstDesignation;
                lstBoxDesignations.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void EnableDisableControls(bool value)
        {
            lstBoxDesignations.Enabled = value;
            lstBoxAssignedDesignation.Enabled = value;
            btnAdd.Disabled = value;
            btnRemove.Disabled = value;
        }

        private void LoadAssignedDesignations(long _OrganizationID)
        {
            try
            {
                List<object> lstDesignation = new ComplaintsManagementBLL().GetAssignedDesignations(_OrganizationID);
                lstBoxAssignedDesignation.DataValueField = "ID";
                lstBoxAssignedDesignation.DataTextField = "Designation";
                lstBoxAssignedDesignation.DataSource = lstDesignation;
                lstBoxAssignedDesignation.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool SetAdditionalAccessibility(string _OrganizationID, List<string> _DesignationIDs)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            int _UserID = Convert.ToInt32(mdlUser.ID);
            bool IsSaved = false;
            if (_DesignationIDs != null && _DesignationIDs.Count > 0 && !string.IsNullOrEmpty(_OrganizationID) && _OrganizationID != "0")
                IsSaved = new ComplaintsManagementBLL().SetAdditionalAccessibility(Convert.ToInt64(_OrganizationID), _DesignationIDs.Select(long.Parse).ToList(), _UserID);
            else
                IsSaved = new ComplaintsManagementBLL().DeleteRowByOrganizationID(Convert.ToInt64(_OrganizationID));


            return IsSaved;
        }

    }
}