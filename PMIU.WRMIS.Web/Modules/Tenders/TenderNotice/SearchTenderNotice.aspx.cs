using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class SearchTenderNotice : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindDomainDropDown(mdlUser);
                    BindDivisionDropdown(mdlUser);
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);
                        if (IsSaved)
                        {
                             Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                           _IsSaved = false; // Reset flag after displaying message.
                        }
                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchTenderNotice] != null)
                            {
                                BindHistoryData();
                            }
                        }
                        
                    }
                    hlAdd.Visible = base.CanAdd;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderNotice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSubmissionDate = (Label)e.Row.FindControl("lblSubmissionDate");
                    Label lblOpeningDate = (Label)e.Row.FindControl("lblOpeningDate");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    HyperLink btnEdit = (HyperLink)e.Row.FindControl("btnEdit");
                    if (lblSubmissionDate.Text != "")
                    {
                        lblSubmissionDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblSubmissionDate.Text));
                    }

                    if (lblOpeningDate.Text != "")
                    {
                        lblOpeningDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblOpeningDate.Text));
                    }

                    if (Convert.ToDateTime(lblOpeningDate.Text) >= DateTime.Today)
                    {
                        lblStatus.Text = "Active";
                    }
                    else if (Convert.ToDateTime(lblOpeningDate.Text) < DateTime.Today)
                    {
                        lblStatus.Text = "Expired";
                    }

                    if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                    {
                        btnEdit.Enabled = false;
                    }
                    else
                    {
                        btnEdit.Enabled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
            }
            else if (_MdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

            }

        }
        protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                if (mdlUsers.DesignationID == (long)Constants.Designation.ChiefMonitoring)
                {
                    long SelectedDomainID = ddlDomain.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDomain.SelectedItem.Value);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, new TenderManagementBLL().GetDivisionByDomainID(SelectedDomainID));
                }
                
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                List<dynamic> SearchCriteria = new List<dynamic>();
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                if (DivisionID == -1 && mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                {
                    ddlDivision.SelectedIndex = 1;
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                long DomainID = ddlDomain.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDomain.SelectedItem.Value);
                long Status = ddlStatus.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlStatus.SelectedItem.Value);

                string TenderName = txtTenderNotice.Text;

                DateTime? SubmissionFrom = null;
                DateTime? SubmissionTo = null;

                DateTime? OpeningFrom = null;
                DateTime? OpeningTo = null;

                if (txtSubmissionFrom.Text != String.Empty)
                {
                    SubmissionFrom = DateTime.Parse(txtSubmissionFrom.Text);
                }

                if (txtSubmissionTo.Text != String.Empty)
                {
                    SubmissionTo = DateTime.Parse(txtSubmissionTo.Text);
                }

                if (txtOpeningFrom.Text != String.Empty)
                {
                    OpeningFrom = DateTime.Parse(txtOpeningFrom.Text);
                }

                if (txtOpeningTo.Text != String.Empty)
                {
                    OpeningTo = DateTime.Parse(txtOpeningTo.Text);
                }

                if ((SubmissionFrom != null && SubmissionTo != null && SubmissionFrom > SubmissionTo) || (OpeningFrom != null && OpeningTo != null && OpeningFrom > OpeningTo))
                {
                    Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                    gvTenderNotice.Visible = false;
                    return;
                }

                BindGrid(TenderName, DomainID, DivisionID, SubmissionFrom, SubmissionTo, OpeningFrom, OpeningTo, Status);

                SearchCriteria.Add(TenderName);
                SearchCriteria.Add(Status);
                SearchCriteria.Add(DomainID);
                SearchCriteria.Add(DivisionID);
                SearchCriteria.Add(SubmissionFrom);
                SearchCriteria.Add(SubmissionTo);
                SearchCriteria.Add(OpeningFrom);
                SearchCriteria.Add(OpeningTo);
                SearchCriteria.Add(gvTenderNotice.PageIndex);
                Session[SessionValues.SearchTenderNotice] = SearchCriteria;

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //private void BindDomainDropDown()
        //{
        //    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDomain, new TenderManagementBLL().GetDomains());
        //}

        private void BindDomainDropDown(UA_Users _MdlUser)
        {
            try
            {
                // Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDomain, new TenderManagementBLL().GetDomainsByUserID(_MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID));
                if (_MdlUser.UA_Designations.IrrigationLevelID != null)
                    Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
                else
                    Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindGrid(string _TenderNoticeName, long _DomainID, long _DivisionID, DateTime? _SubmissionDateFrom, DateTime? _SubmissionDateTo, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo, long _Status)
        {
            TenderManagementBLL bllTenders = new TenderManagementBLL();
            List<TM_TenderNotice> lstTenderNotice = bllTenders.GetAllTenderNotice(_TenderNoticeName, _DomainID, _DivisionID, _SubmissionDateFrom, _SubmissionDateTo, _OpeningDateFrom, _OpeningDateTo, _Status);
            gvTenderNotice.DataSource = lstTenderNotice;
            gvTenderNotice.DataBind();
            gvTenderNotice.Visible = true;
        }

        private void BindHistoryData()
        {
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.SearchTenderNotice];
            txtTenderNotice.Text = SearchCriteria[0];
            long Status = Convert.ToInt64(SearchCriteria[1]);
            if (Status != -1)
            {
                Dropdownlist.SetSelectedValue(ddlStatus, Status.ToString());
            }
            long DomainID = Convert.ToInt64(SearchCriteria[2]);
            long DivisionID = Convert.ToInt64(SearchCriteria[3]);
            if (DomainID != -1)
            {
                Dropdownlist.SetSelectedValue(ddlDomain, DomainID.ToString());
                if (DivisionID != -1)
                {
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, new TenderManagementBLL().GetDivisionByDomainID(DomainID));
                    Dropdownlist.SetSelectedValue(ddlDivision, DivisionID.ToString());
                }
            }
            txtSubmissionFrom.Text = SearchCriteria[4];
            txtSubmissionTo.Text = SearchCriteria[5];
            txtOpeningFrom.Text = SearchCriteria[6];
            txtOpeningTo.Text = SearchCriteria[7];
            gvTenderNotice.PageIndex = (int)SearchCriteria[8];
            BtnSearch_Click(null, null);
        }

        protected void gvTenderNotice_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvTenderNotice.EditIndex = -1;
                BtnSearch_Click(null, null);
                //BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderNotice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTenderNotice.PageIndex = e.NewPageIndex;
                BtnSearch_Click(null, null);
                //BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}