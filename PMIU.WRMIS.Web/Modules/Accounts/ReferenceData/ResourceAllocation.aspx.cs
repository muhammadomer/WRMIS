using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class ResourceAllocation : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdown();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.ResourceAllocation] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Function
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private Int64 PMIUID()
        {
            Int64 SelectedADMID = 0;

            string _PMIUStaff = ddlPMIUStaff == null ? "" : Convert.ToString(ddlPMIUStaff.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlPMIUStaff.SelectedItem.Text));
            if (_PMIUStaff == "Field")
            {
                if (ddlADM.SelectedIndex != 0)
                {
                    SelectedADMID = Convert.ToInt64(ddlADM.SelectedItem.Value);
                }
            }
            else
            {
                SelectedADMID = -1;
            }

            return SelectedADMID;
        }

        private void BindResourceAllocationGrid()
        {
            try
            {
                //SaveHistoryValues();

                List<object> lstResourceAllocation = new ReferenceDataBLL().GetResourceAllocationSearch(PMIUID());
                gvResourceAllocation.DataSource = lstResourceAllocation;
                gvResourceAllocation.DataBind();
                gvResourceAllocation.Visible = true;

                List<dynamic> PreviousState = new List<dynamic>();
                PreviousState.Add(ddlPMIUStaff.SelectedItem.Value);
                PreviousState.Add(PMIUID());
                PreviousState.Add(gvResourceAllocation.PageIndex);
                Session[SessionValues.ResourceAllocation] = PreviousState;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void BindHistoryData()
        {
            ddlPMIUStaff.ClearSelection();
            ddlADM.ClearSelection();
            List<dynamic> PreviousState = (List<dynamic>)Session[SessionValues.ResourceAllocation];

            Dropdownlist.SetSelectedValue(ddlPMIUStaff, PreviousState[0]);
            ddlPMIUStaff_SelectedIndexChanged(null, null);
            if (PreviousState[1] == -1)
                Dropdownlist.SetSelectedValue(ddlADM, "");
            else
                Dropdownlist.SetSelectedValue(ddlADM, Convert.ToString(PreviousState[1]));
            gvResourceAllocation.PageIndex = PreviousState[2];
            List<object> lstResourceAllocation = new ReferenceDataBLL().GetResourceAllocationSearch(PreviousState[1]);
            gvResourceAllocation.DataSource = lstResourceAllocation;
            gvResourceAllocation.DataBind();
            gvResourceAllocation.Visible = true;
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteResourceAllocation");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        protected void SaveHistoryValues()
        {

            string _PMIUStaff = ddlPMIUStaff == null ? "" : Convert.ToString(ddlPMIUStaff.SelectedItem.Text == string.Empty ? "" : Convert.ToString(ddlPMIUStaff.SelectedItem.Text));
            string ADMID = "0";

            if (ddlADM.SelectedIndex != 0)
            {
                ADMID = ddlADM.SelectedItem.Value;
            }


            Session["ResourceAllocation"] = null;
            object obj = new
            {
                _PMIUStaff,
                ADMID
            };
            Session["ResourceAllocation"] = obj;
        }
        protected void HistoryValues()
        {
            object currentObj = Session["ResourceAllocation"] as object;
            if (currentObj != null)
            {

                if (currentObj.GetType().GetProperty("_PMIUStaff").GetValue(currentObj).ToString() == "Field")
                {
                    ddlPMIUStaff.SelectedIndex = 1;
                    Dropdownlist.DDLADM(ddlADM, false);
                    ddlADM.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("ADMID").GetValue(currentObj));
                }
                BindResourceAllocationGrid();
            }

        }

        private void BindDropdown()
        {
            if (ddlPMIUStaff.SelectedIndex == 0)
            {
                Dropdownlist.DDLADM(ddlADM, true);
                ddlADM.Enabled = false;
            }
        }
        #endregion

        #region Events

        #region Grid
        protected void gvResourceAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvResourceAllocation.PageIndex = e.NewPageIndex;
                gvResourceAllocation.EditIndex = -1;
                BindResourceAllocationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvResourceAllocation.EditIndex = -1;
                BindResourceAllocationGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddResourceAllocation")
                {

                    List<object> lstResourceAllocation = new ReferenceDataBLL().GetResourceAllocationSearch(PMIUID());
                    lstResourceAllocation.Add(new
                    {
                        ID = 0,
                        PMIUFieldOffice = string.Empty,
                        ADMUserID = 0,
                        StaffUserID = 0,
                        StaffUserName = string.Empty,
                        DesignationID = 0,
                        DesignationName = string.Empty,
                        EmailAddress = string.Empty,
                        ContactNumber = string.Empty,
                        BPS = string.Empty,
                        IsActive = true,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvResourceAllocation.PageIndex = gvResourceAllocation.PageCount;
                    gvResourceAllocation.DataSource = lstResourceAllocation;
                    gvResourceAllocation.DataBind();

                    gvResourceAllocation.EditIndex = gvResourceAllocation.Rows.Count - 1;
                    gvResourceAllocation.DataBind();


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    //dropdown populate here designation
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    if (gvResourceAllocation.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"
                        //ID,PMIUFieldOffice,ADMUserID,StaffUserID,StaffUserName,DesignationID,EmailAddress,ContactNumber,BPS,IsActive,CreatedBy,CreatedDate
                        DataKey key = gvResourceAllocation.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string PMIUFieldOffice = Convert.ToString(key.Values["PMIUFieldOffice"]);
                        string ADMUserID = Convert.ToString(key.Values["ADMUserID"]);
                        string StaffUserID = Convert.ToString(key.Values["StaffUserID"]);
                        string StaffUserName = Convert.ToString(key.Values["StaffUserName"]);
                        string DesignationID = Convert.ToString(key.Values["DesignationID"]);
                        string EmailAddress = Convert.ToString(key.Values["EmailAddress"]);
                        string ContactNumber = Convert.ToString(key.Values["ContactNumber"]);
                        string BPS = Convert.ToString(key.Values["BPS"]);
                        string IsActive = Convert.ToString(key.Values["IsActive"]);
                        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                        #endregion

                        #region "Controls"
                        //   GridViewRow row = gvResourceAllocation.Rows[e.Row.RowIndex];

                        TextBox txtEmailAddress = (TextBox)e.Row.FindControl("txtEmailAddress");
                        TextBox txtContactNumber = (TextBox)e.Row.FindControl("txtContactNumber");
                        Label lblEmailAddress = (Label)e.Row.FindControl("lbltxtEmailAddress");
                        Label lblContactNumber = (Label)e.Row.FindControl("lbltxtContactNumber");

                        TextBox txtBPS = (TextBox)e.Row.FindControl("txtBPS");
                        CheckBox chkIsActive = (CheckBox)e.Row.FindControl("chkActive");
                        DropDownList ddlDesignation = (DropDownList)e.Row.FindControl("ddlDesignation");
                        DropDownList ddlNameofStaff = (DropDownList)e.Row.FindControl("ddlNameofStaff");
                        TextBox txtNameofStaff = (TextBox)e.Row.FindControl("txtNameofStaff");
                        #endregion

                        if (ddlDesignation != null)
                        {
                            if (ddlPMIUStaff.SelectedIndex == 1)
                            {
                                Dropdownlist.DDLADMDesignation(ddlDesignation, 1); // 1 for field staff ,0 for office   
                            }
                            else if (ddlPMIUStaff.SelectedIndex == 2)
                            {
                                Dropdownlist.DDLADMDesignation(ddlDesignation, 0); // 1 for field staff ,0 for office   
                            }

                            if (!string.IsNullOrEmpty(DesignationID))
                            {
                                Dropdownlist.SetSelectedValue(ddlDesignation, DesignationID);
                            }
                        }


                        bool IsUserExist = new ReferenceDataBLL().IsUserExist(Convert.ToInt64(DesignationID));


                        if (!IsUserExist)
                        {
                            if (StaffUserName != "")
                                txtNameofStaff.Text = StaffUserName;
                            txtNameofStaff.Visible = true;
                            txtEmailAddress.Visible = true;
                            txtContactNumber.Visible = true;
                            ddlNameofStaff.Visible = false;

                            if (EmailAddress != "")
                            {
                                txtEmailAddress.Text = EmailAddress;
                                lblEmailAddress.Text = EmailAddress;
                                txtEmailAddress.Visible = true;
                            }
                            if (ContactNumber != "")
                            {
                                txtContactNumber.Text = ContactNumber;
                                lblContactNumber.Text = ContactNumber;
                                txtContactNumber.Visible = true;
                            }
                        }
                        else
                        {
                            if (ddlNameofStaff != null)
                            {
                                txtNameofStaff.Visible = false;
                                txtEmailAddress.Visible = false;
                                txtContactNumber.Visible = false;

                                ddlNameofStaff.Visible = true;
                                if (ddlDesignation.SelectedItem.Value != "")
                                    Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                                if (!string.IsNullOrEmpty(StaffUserID))
                                {
                                    Dropdownlist.SetSelectedValue(ddlNameofStaff, StaffUserID);

                                    if (StaffUserID != "0")
                                    {
                                        ddlNameofStaff.Enabled = true;
                                        ddlNameofStaff.Attributes.Add("required", "required");
                                        ddlNameofStaff.CssClass = "required form-control";
                                    }

                                }
                            }

                            if (EmailAddress != "")
                            {
                                txtEmailAddress.Text = EmailAddress;
                                lblEmailAddress.Text = EmailAddress;
                                lblEmailAddress.Visible = true;
                            }
                            if (ContactNumber != "")
                            {
                                txtContactNumber.Text = ContactNumber;
                                lblContactNumber.Text = ContactNumber;
                                lblContactNumber.Visible = true;
                            }
                        }



                        //if (ddlPMIUStaff.SelectedItem.Value == "Field")
                        //{
                        //    if (ddlDesignation.SelectedItem.Text.ToLower() == "driver" || ddlDesignation.SelectedItem.Text.ToLower() == "helper")
                        //    {
                        //        if (StaffUserName != "")
                        //            txtNameofStaff.Text = StaffUserName;
                        //        txtNameofStaff.Visible = true;
                        //        txtEmailAddress.Visible = true;
                        //        txtContactNumber.Visible = true;
                        //        ddlNameofStaff.Visible = false;

                        //        if (EmailAddress != "")
                        //        {
                        //            txtEmailAddress.Text = EmailAddress;
                        //            lblEmailAddress.Text = EmailAddress;
                        //            txtEmailAddress.Visible = true;
                        //        }
                        //        if (ContactNumber != "")
                        //        {
                        //            txtContactNumber.Text = ContactNumber;
                        //            lblContactNumber.Text = ContactNumber;
                        //            txtContactNumber.Visible = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (ddlNameofStaff != null)
                        //        {
                        //            txtNameofStaff.Visible = false;
                        //            txtEmailAddress.Visible = false;
                        //            txtContactNumber.Visible = false;

                        //            ddlNameofStaff.Visible = true;
                        //            if (ddlDesignation.SelectedItem.Value != "")
                        //                Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                        //            if (!string.IsNullOrEmpty(StaffUserID))
                        //            {
                        //                Dropdownlist.SetSelectedValue(ddlNameofStaff, StaffUserID);

                        //                if (StaffUserID != "0")
                        //                {
                        //                    ddlNameofStaff.Enabled = true;
                        //                    ddlNameofStaff.Attributes.Add("required", "required");
                        //                    ddlNameofStaff.CssClass = "required form-control";
                        //                }

                        //            }
                        //        }

                        //        if (EmailAddress != "")
                        //        {
                        //            txtEmailAddress.Text = EmailAddress;
                        //            lblEmailAddress.Text = EmailAddress;
                        //            lblEmailAddress.Visible = true;
                        //        }
                        //        if (ContactNumber != "")
                        //        {
                        //            txtContactNumber.Text = ContactNumber;
                        //            lblContactNumber.Text = ContactNumber;
                        //            lblContactNumber.Visible = true;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (ddlNameofStaff != null)
                        //    {
                        //        txtNameofStaff.Visible = false;
                        //        txtEmailAddress.Visible = false;
                        //        txtContactNumber.Visible = false;
                        //        ddlNameofStaff.Visible = true;
                        //        if (ddlDesignation.SelectedItem.Value != "")
                        //            Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                        //        if (!string.IsNullOrEmpty(StaffUserID))
                        //        {
                        //            Dropdownlist.SetSelectedValue(ddlNameofStaff, StaffUserID);
                        //            if (StaffUserID != "0")
                        //            {
                        //                ddlNameofStaff.Enabled = true;
                        //                ddlNameofStaff.Attributes.Add("required", "required");
                        //                ddlNameofStaff.CssClass = "required form-control";
                        //            }
                        //        }
                        //    }
                        //}



                        //if (EmailAddress != "")
                        //{
                        //    txtEmailAddress.Text = EmailAddress;
                        //    lblEmailAddress.Text = EmailAddress;
                        //    lblEmailAddress.Visible = true;
                        //}
                        //if (ContactNumber != "")
                        //{
                        //    txtContactNumber.Text = ContactNumber;
                        //    lblContactNumber.Text = ContactNumber;
                        //    lblContactNumber.Visible = true;
                        //}
                        if (BPS != "0")
                            txtBPS.Text = BPS;

                        if (IsActive.Equals("True"))
                            chkIsActive.Checked = true;
                        else
                            chkIsActive.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvResourceAllocation.DataKeys[e.RowIndex].Values[0]);
                bool IsExist = new ReferenceDataBLL().IsExistResourceAllocation(Convert.ToInt64(ID));
                if (!IsExist)
                {
                    bool IsDeleted = new ReferenceDataBLL().DeleteResourceAllocation(Convert.ToInt64(ID));
                    if (IsDeleted)
                    {
                        BindResourceAllocationGrid();
                        Master.ShowMessage(Message.RecordDeleted.Description);
                    }
                }
                else
                {
                    Master.ShowMessage("Selected record cannot be deleted as its relevant data exists.", SiteMaster.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvResourceAllocation.EditIndex = e.NewEditIndex;
                BindResourceAllocationGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvResourceAllocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //ID,PMIUFieldOffice,ADMUserID,StaffUserID,StaffUserName,DesignationID,EmailAddress,ContactNumber,BPS,IsActive,CreatedBy,CreatedDate
                DataKey key = gvResourceAllocation.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string PMIUFieldOffice = Convert.ToString(key.Values["PMIUFieldOffice"]);
                string ADMUserID = Convert.ToString(key.Values["ADMUserID"]);
                string StaffUserID = Convert.ToString(key.Values["StaffUserID"]);
                string StaffUserName = Convert.ToString(key.Values["StaffUserName"]);
                string DesignationID = Convert.ToString(key.Values["DesignationID"]);
                string EmailAddress = Convert.ToString(key.Values["EmailAddress"]);
                string ContactNumber = Convert.ToString(key.Values["ContactNumber"]);
                string BPS = Convert.ToString(key.Values["BPS"]);
                string IsActive = Convert.ToString(key.Values["IsActive"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow row = gvResourceAllocation.Rows[e.RowIndex];

                TextBox txtEmailAddress = (TextBox)row.FindControl("txtEmailAddress");
                TextBox txtContactNumber = (TextBox)row.FindControl("txtContactNumber");
                TextBox txtBPS = (TextBox)row.FindControl("txtBPS");
                CheckBox chkIsActive = (CheckBox)row.FindControl("chkActive");
                DropDownList ddlDesignation = (DropDownList)row.FindControl("ddlDesignation");
                DropDownList ddlNameofStaff = (DropDownList)row.FindControl("ddlNameofStaff");
                TextBox txtNameofStaff = (TextBox)row.FindControl("txtNameofStaff");

                #endregion

                AT_ResourceAllocation _ResourceAllocation = new AT_ResourceAllocation();

                _ResourceAllocation.ID = Convert.ToInt64(ID);
                if (ID == "0")
                {
                    _ResourceAllocation.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    _ResourceAllocation.CreatedDate = DateTime.Now;
                }
                else
                {
                    _ResourceAllocation.CreatedBy = Convert.ToInt32(CreatedBy);
                    _ResourceAllocation.CreatedDate = Convert.ToDateTime(CreatedDate);
                    _ResourceAllocation.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    _ResourceAllocation.ModifiedDate = DateTime.Now;
                }

                if (txtEmailAddress.Text != "")
                    _ResourceAllocation.EmailAddress = txtEmailAddress.Text;
                if (txtContactNumber.Text != "")
                    _ResourceAllocation.ContactNumber = txtContactNumber.Text;

                if (txtBPS.Text != "" && Convert.ToInt32(txtBPS.Text) > 0 && Convert.ToInt32(txtBPS.Text) < 23)
                {
                    _ResourceAllocation.BPS = Convert.ToInt32(txtBPS.Text);
                }
                else
                {
                    Master.ShowMessage("BPS must be between 1 to 22.", SiteMaster.MessageType.Error);
                }

                if (ddlDesignation.SelectedItem.Value != "")
                    _ResourceAllocation.DesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);

                if (ddlNameofStaff.Visible==true)
                {
                    _ResourceAllocation.StaffUserName = ddlNameofStaff.SelectedItem.Text;
                }
                else
                {
                    _ResourceAllocation.StaffUserName = txtNameofStaff.Text;
                }

                //if (ddlPMIUStaff.SelectedItem.Value == "Field")
                //{
                //    if (ddlDesignation.SelectedItem.Text.ToLower() == "driver" || ddlDesignation.SelectedItem.Text.ToLower() == "helper")
                //    {
                //        _ResourceAllocation.StaffUserName = txtNameofStaff.Text;
                //    }
                //    else
                //    {
                //        _ResourceAllocation.StaffUserName = ddlNameofStaff.SelectedItem.Text;
                //    }
                //}
                //else
                //{
                //    if (ddlNameofStaff.SelectedIndex != 0)
                //        _ResourceAllocation.StaffUserName = ddlNameofStaff.SelectedItem.Text;
                //}


                if (ddlPMIUStaff.SelectedItem.Value == "Field")
                {
                    _ResourceAllocation.PMIUFieldOffice = "F";
                    if (ddlADM.SelectedIndex != 0)
                    {
                        _ResourceAllocation.ADMUserID = Convert.ToInt64(ddlADM.SelectedItem.Value);
                    }

                    if (new ReferenceDataBLL().IsUserExits(Convert.ToInt64(ddlDesignation.SelectedItem.Value)))
                    {
                        _ResourceAllocation.StaffUserID = Convert.ToInt64(ddlNameofStaff.SelectedItem.Value);
                    }


                }
                else
                {
                    _ResourceAllocation.PMIUFieldOffice = "O";

                    if (new ReferenceDataBLL().IsUserExits(Convert.ToInt64(ddlDesignation.SelectedItem.Value)))
                    {
                        _ResourceAllocation.StaffUserID = Convert.ToInt64(ddlNameofStaff.SelectedItem.Value);
                    }
                }


                _ResourceAllocation.IsActive = chkIsActive.Checked;



                if (new ReferenceDataBLL().IsStaffUnique(_ResourceAllocation))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new ReferenceDataBLL().SaveResourceAllocation(_ResourceAllocation);


                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(_ResourceAllocation.ID) == 0)
                        gvResourceAllocation.PageIndex = 0;

                    gvResourceAllocation.EditIndex = -1;
                    BindResourceAllocationGrid();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region DropDown


        protected void ddlPMIUStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPMIUStaff.SelectedIndex == 0)
                {
                    Dropdownlist.DDLADM(ddlADM, true);
                    ddlADM.Enabled = false;
                    gvResourceAllocation.Visible = false;

                    ddlADM.Attributes.Add("required", "false");
                    ddlADM.CssClass = "form-control";
                }
                //field
                if (ddlPMIUStaff.SelectedIndex == 1)
                {
                    Dropdownlist.DDLADM(ddlADM, false);
                    ddlADM.Enabled = true;
                    gvResourceAllocation.Visible = false;

                    ddlADM.Attributes.Add("required", "true");
                    ddlADM.CssClass = "form-control required";
                }
                //Office
                if (ddlPMIUStaff.SelectedIndex == 2)
                {
                    Dropdownlist.DDLADM(ddlADM, false);
                    ddlADM.Enabled = false;
                    gvResourceAllocation.Visible = false;

                    ddlADM.Attributes.Add("required", "false");
                    ddlADM.CssClass = "form-control";
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlDesignation = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDesignation.NamingContainer;
                DropDownList ddlNameofStaff = (DropDownList)gvRow.FindControl("ddlNameofStaff");
                TextBox txtNameofStaff = (TextBox)gvRow.FindControl("txtNameofStaff");
                TextBox txtEmailAddress = (TextBox)gvRow.FindControl("txtEmailAddress");
                TextBox txtContactNumber = (TextBox)gvRow.FindControl("txtContactNumber");
                Label lbltxtEmailAddress = (Label)gvRow.FindControl("lbltxtEmailAddress");
                Label lbltxtContactNumber = (Label)gvRow.FindControl("lbltxtContactNumber");

                if (ddlPMIUStaff.SelectedItem.Value == "Field")
                {
                    if (ddlDesignation.SelectedItem.Text.ToLower() == "driver" || ddlDesignation.SelectedItem.Text.ToLower() == "helper")
                    {
                        ddlNameofStaff.Visible = false;

                        txtNameofStaff.Visible = true;
                        txtEmailAddress.Visible = true;
                        txtContactNumber.Visible = true;

                        txtNameofStaff.Text = "";
                        txtEmailAddress.Text = "";
                        txtContactNumber.Text = "";
                        lbltxtEmailAddress.Visible = false;
                        lbltxtContactNumber.Visible = false;

                    }
                    else
                    {
                        ddlNameofStaff.Visible = true;
                        ddlNameofStaff.Enabled = true;
                        txtNameofStaff.Visible = false;
                        txtEmailAddress.Visible = false;
                        txtContactNumber.Visible = false;
                        lbltxtContactNumber.Text = "";
                        lbltxtEmailAddress.Text = "";
                        Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, true);

                        if (gvRow != null)
                        {
                            if (ddlDesignation.SelectedItem.Value != "")
                            {
                                Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                                ddlNameofStaff.Attributes.Add("required", "required");
                                ddlNameofStaff.CssClass = "required form-control";
                            }

                        }

                        if (ddlDesignation.SelectedItem.Value == "")
                        {
                            lbltxtContactNumber.Text = "";
                            lbltxtEmailAddress.Text = "";
                            ddlNameofStaff.Enabled = false;
                            ddlNameofStaff.Attributes.Add("required", "false");
                            ddlNameofStaff.CssClass = "form-control";
                        }

                    }
                }
                else
                {
                    ReferenceDataBLL bllReferenceDate = new ReferenceDataBLL();
                    if (ddlDesignation.SelectedItem.Value != "")
                    {
                        List<object> obj = bllReferenceDate.GetNameofStaffByDesignationID(Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                        if (obj.Count == 0)
                        {
                            ddlNameofStaff.Visible = false;

                            txtNameofStaff.Visible = true;
                            txtEmailAddress.Visible = true;
                            txtContactNumber.Visible = true;

                            txtNameofStaff.Text = "";
                            txtEmailAddress.Text = "";
                            txtContactNumber.Text = "";
                            lbltxtEmailAddress.Visible = false;
                            lbltxtContactNumber.Visible = false;
                        }
                        else
                        {
                            ddlNameofStaff.Visible = true;
                            ddlNameofStaff.Enabled = true;
                            txtNameofStaff.Visible = false;
                            txtEmailAddress.Visible = false;
                            txtContactNumber.Visible = false;
                            lbltxtContactNumber.Text = "";
                            lbltxtEmailAddress.Text = "";
                            Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, true);

                            if (gvRow != null)
                            {
                                if (ddlDesignation.SelectedItem.Value != "")
                                {
                                    Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                                    ddlNameofStaff.Attributes.Add("required", "required");
                                    ddlNameofStaff.CssClass = "required form-control";
                                }

                            }

                            if (ddlDesignation.SelectedItem.Value == "")
                            {
                                lbltxtContactNumber.Text = "";
                                lbltxtEmailAddress.Text = "";
                                ddlNameofStaff.Enabled = false;
                                ddlNameofStaff.Attributes.Add("required", "false");
                                ddlNameofStaff.CssClass = "form-control";
                            }
                        }
                    }
                    else
                    {
                        lbltxtContactNumber.Text = "";
                        lbltxtEmailAddress.Text = "";
                        ddlNameofStaff.Enabled = false;
                        ddlNameofStaff.ClearSelection();
                        ddlNameofStaff.Attributes.Add("required", "false");
                        ddlNameofStaff.CssClass = "form-control";
                    }

                    //ddlNameofStaff.Visible = true;
                    //txtNameofStaff.Visible = false;
                    //txtEmailAddress.Visible = false;
                    //txtContactNumber.Visible = false;
                    //lbltxtContactNumber.Text = "";
                    //lbltxtEmailAddress.Text = "";
                    //Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, true);

                    //if (gvRow != null)
                    //{
                    //    if (ddlDesignation.SelectedItem.Value != "")
                    //    {
                    //        Dropdownlist.DDLACCNameofStaff(ddlNameofStaff, false, Convert.ToInt64(ddlDesignation.SelectedItem.Value));
                    //        ddlNameofStaff.Attributes.Add("required", "required");
                    //        ddlNameofStaff.CssClass = "required form-control";
                    //    }

                    //}

                    //if (ddlDesignation.SelectedItem.Value == "")
                    //{
                    //    lbltxtContactNumber.Text = "";
                    //    lbltxtEmailAddress.Text = "";
                    //    ddlNameofStaff.Enabled = false;
                    //    ddlNameofStaff.Attributes.Add("required", "false");
                    //    ddlNameofStaff.CssClass = "form-control";
                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlNameofStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlNameofStaff = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlNameofStaff.NamingContainer;

                TextBox txtNameofStaff = (TextBox)gvRow.FindControl("txtNameofStaff");
                TextBox txtEmailAddress = (TextBox)gvRow.FindControl("txtEmailAddress");
                TextBox txtContactNumber = (TextBox)gvRow.FindControl("txtContactNumber");
                Label lbltxtEmailAddress = (Label)gvRow.FindControl("lbltxtEmailAddress");
                Label lbltxtContactNumber = (Label)gvRow.FindControl("lbltxtContactNumber");

                txtNameofStaff.Visible = false;
                txtEmailAddress.Visible = false;
                txtContactNumber.Visible = false;
                lbltxtContactNumber.Visible = true;
                lbltxtEmailAddress.Visible = true;
                if (gvRow != null)
                {
                    if (ddlNameofStaff.SelectedIndex != 0)
                    {
                        object emailContact = new ReferenceDataBLL().GetEmailContact(Convert.ToInt64(ddlNameofStaff.SelectedItem.Value));
                        lbltxtEmailAddress.Text = Convert.ToString(emailContact.GetType().GetProperty("Email").GetValue(emailContact));
                        lbltxtContactNumber.Text = Convert.ToString(emailContact.GetType().GetProperty("MobilePhone").GetValue(emailContact));

                        txtEmailAddress.Text = Convert.ToString(emailContact.GetType().GetProperty("Email").GetValue(emailContact));
                        txtContactNumber.Text = Convert.ToString(emailContact.GetType().GetProperty("MobilePhone").GetValue(emailContact));
                    }
                    else
                    {
                        lbltxtEmailAddress.Text = "";
                        lbltxtContactNumber.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlPMIUStaff.SelectedItem.Value == "Field")
            {
                if (ddlADM.SelectedIndex == 0)
                {
                    Master.ShowMessage("Please select ADM.", SiteMaster.MessageType.Error);
                    return;
                }
            }

            BindResourceAllocationGrid();
        }

        #endregion

        #endregion



    }
}