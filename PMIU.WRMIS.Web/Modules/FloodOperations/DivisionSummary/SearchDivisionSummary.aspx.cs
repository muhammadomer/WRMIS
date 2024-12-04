using PMIU.WRMIS.BLL.FloodOperations;
using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary
{
    public partial class SearchDivisionSummary : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        private List<CO_SubDivision> lstSubDivision = new List<CO_SubDivision>();

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
                    SetPageTitle();
                    // BindZoneDropdown();
                    BindUserLocation();
                    // DDLEmptyCircleDivision();
                    // Bind year dropdownlist
                    Dropdownlist.DDLYear(ddlYear, false, 0, 2011, (int)Constants.DropDownFirstOption.All);
                    //BindActiveDivisionSummaryTypeDropdown();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long divisionSummaryID = Utility.GetNumericValueFromQueryString("DivisionSummaryID", 0);
                    if (divisionSummaryID > 0)
                    {
                        hdnDivisionSummaryID.Value = divisionSummaryID.ToString();
                        if (_IsSaved)
                        {
                            //txtDivisionSummaryName.Text = Convert.ToString(HttpContext.Current.Session["DivisionSummaryName"]);
                            //rdolDivisionSummaryStatus.SelectedIndex = 2;
                            Master.ShowMessage(Message.RecordSaved.Description);
                            BindDivisionSummarySearchGrid(divisionSummaryID);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                    }
                    if (Session["SDS_SC_SearchCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["SDS_SC_SearchCriteria"]);

                        if (Convert.ToInt64(searchCriteria["_DivisionSummaryID"]) == 0 || Convert.ToInt64(searchCriteria["_DivisionSummaryID"]) == -1)
                        {
                            if (Convert.ToInt64(hdnDivisionSummaryID.Value) != 0 && Convert.ToInt64(hdnDivisionSummaryID.Value) != -1)
                            {
                                divisionSummaryID = Convert.ToInt64(hdnDivisionSummaryID.Value);
                            }
                        }
                        else
                        {
                            divisionSummaryID = Convert.ToInt64(searchCriteria["_DivisionSummaryID"]);
                        }

                        if (divisionSummaryID > 0)
                        {
                            searchCriteria = null;

                            searchCriteria = new
                            {
                                _DivisionSummaryID = divisionSummaryID,
                                _ZoneID = -1,
                                _CircleID = -1,
                                _DivisionID = -1,
                                _Year = -1,
                                _DivisionSummaryStatus = ""
                            };

                            Session["SDS_SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
                            searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["SDS_SC_SearchCriteria"]);
                        }

                        this.LoadSearchCriteria(divisionSummaryID, Convert.ToInt64(searchCriteria["_ZoneID"]), Convert.ToInt64(searchCriteria["_CircleID"]), Convert.ToInt64(searchCriteria["_DivisionID"]), Convert.ToInt64(searchCriteria["_Year"]), Convert.ToString(searchCriteria["_DivisionSummaryStatus"]));
                        LoadSearchCriteria(searchCriteria);
                    }
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    {
                        hlAddNewDivisionSummary.Visible = true;
                    }
                    else
                    {
                        hlAddNewDivisionSummary.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }

        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 01-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _DivisionSummaryID)
        {
            FloodOperationsBLL bllFloodOperations = new FloodOperationsBLL();
            bool IsExist = bllFloodOperations.IsDivisionSummaryIDExists(_DivisionSummaryID);

            //To do Wajahat
            ////if (!IsExist)
            ////{
            ////  long ZoneIrrigationLevelID = 4;

            ////  IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _DivisionSummaryID);
            ////}

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        ///// <summary>
        ///// This function check whether data is valid for add/edit operation.
        ///// Created on 29-10-2015
        ///// </summary>
        ///// <param name="_DivisionID"></param>
        ///// <param name="_SubDivisionID"></param>
        ///// <param name="_SubDivisionName"></param>
        ///// <returns>bool</returns>
        //private bool IsValidAddEdit(long _DivisionID, long _SubDivisionID, string _SubDivisionName)
        //{
        //  SubDivisionBLL bllSubDivision = new SubDivisionBLL();

        //  CO_SubDivision mdlSearchedSubDivision = bllSubDivision.GetSubDivisionByName(_SubDivisionName, _DivisionID);

        //  if (mdlSearchedSubDivision != null && _SubDivisionID != mdlSearchedSubDivision.ID)
        //  {
        //    Master.ShowMessage(Message.SubDivisionNameExists.Description, SiteMaster.MessageType.Error);

        //    return false;
        //  }

        //  return true;
        //}

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //   ddlCircle.Enabled = false;
                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    // ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                // ddlDivision.Enabled = false;

                //gvDivisionSummary.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    // ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    //  ddlDivision.Enabled = true;
                }

                // gvDivisionSummary.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            //BindZoneDropdown();
            //  BindCircleDropdown(Convert.ToInt64(searchCriteria["_ZoneID"]));
            //    BindDivisionDropdown(Convert.ToInt64(searchCriteria["_CircleID"]));
            Dropdownlist.DDLYear(ddlYear, false, 0, 2011, (int)Constants.DropDownFirstOption.All);
            //Dropdownlist.DDLActiveInfrastructureType(ddlInfrastructureType);
            // Populate search criteria fields
            Dropdownlist.SetSelectedValue(ddlZone, searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]));
            Dropdownlist.SetSelectedValue(ddlCircle, searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]));
            Dropdownlist.SetSelectedValue(ddlDivision, searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]));
            Dropdownlist.SetSelectedValue(ddlYear, searchCriteria["_Year"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_Year"]));

            if (ddlCircle.SelectedIndex > 0)
                ddlCircle.Enabled = true;

            if (ddlDivision.SelectedIndex > 0)
                ddlDivision.Enabled = true;
            //Dropdownlist.SetSelectedValue(ddlDivisionSummaryType, searchCriteria["_DivisionSummaryTypeID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionSummaryTypeID"]));
            //txtDivisionSummaryName.Text = Convert.ToString(searchCriteria["_DivisionSummaryName"]);
            //rdolDivisionSummaryStatus.SelectedIndex = rdolDivisionSummaryStatus.Items.IndexOf(rdolDivisionSummaryStatus.Items.FindByValue(Convert.ToString(searchCriteria["_DivisionSummaryStatus"])));
        }

        private void BindDivisionSummarySearchGrid(long _DivisionSummaryID)
        {
            try
            {
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long Year = ddlYear.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlYear.SelectedItem.Value);

                string DivisionSummaryStatus = "";

                //if (rdolDivisionSummaryStatus.SelectedItem != null && rdolDivisionSummaryStatus.SelectedItem.Value != "")
                //{
                //    DivisionSummaryStatus = rdolDivisionSummaryStatus.SelectedItem.Value;
                //}

                //List<object> lstDivisionSummarySearch = new FloodOperationsBLL().GetDivisionSummaryBySearchCriteria(_DivisionSummaryID
                //, ZoneID
                //, CircleID
                //, DivisionID
                //, Year
                //, DivisionSummaryStatus);
                List<object> lstDivisionSummarySearch = new FloodOperationsBLL().GetDivisionSummaryBySearchCriteria(_DivisionSummaryID
                , ZoneID
                , CircleID
                , DivisionID
                , Year
                , "");

                gvDivisionSummary.DataSource = lstDivisionSummarySearch;
                gvDivisionSummary.DataBind();

                dynamic searchCriteria = new
                {
                    _DivisionSummaryID = _DivisionSummaryID,
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _Year = Year,
                    _DivisionSummaryStatus = DivisionSummaryStatus
                };

                Session["SDS_SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
                gvDivisionSummary.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //private void BindDivisionSummarySearchGrid()
        //{
        //  try
        //  {
        //    long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
        //    long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
        //    long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);

        //    Int16 DivisionSummaryTypeID = -1;
        //    if (ddlDivisionSummaryType.SelectedItem.Value != string.Empty)
        //    {
        //      DivisionSummaryTypeID = Convert.ToInt16(ddlDivisionSummaryType.SelectedItem.Value);
        //    }

        //    string DivisionSummaryName = txtDivisionSummaryName.Text.ToString().Trim();
        //    int DivisionSummaryStatus = 0;

        //    if (rdolDivisionSummaryStatus.SelectedItem != null && rdolDivisionSummaryStatus.SelectedItem.Value != "")
        //    {
        //      DivisionSummaryStatus = int.Parse(rdolDivisionSummaryStatus.SelectedItem.Value);
        //    }

        //    List<object> lstDivisionSummarySearch = new DivisionSummaryBLL().GetDivisionSummaryBySearchCriteria(0
        //    , ZoneID
        //    , CircleID
        //    , DivisionID
        //    , DivisionSummaryTypeID
        //    , DivisionSummaryName
        //    , DivisionSummaryStatus);

        //    gvDivisionSummary.DataSource = lstDivisionSummarySearch;
        //    gvDivisionSummary.DataBind();

        //    dynamic searchCriteria = new
        //    {
        //      _ZoneID = ZoneID,
        //      _CircleID = CircleID,
        //      _DivisionID = DivisionID,
        //      _DivisionSummaryTypeID = DivisionSummaryTypeID,
        //      _DivisionSummaryName = DivisionSummaryName,
        //      _DivisionSummaryStatus = DivisionSummaryStatus
        //    };

        //    Session["SDS_SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
        //    gvDivisionSummary.Visible = true;
        //  }
        //  catch (Exception ex)
        //  {
        //    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //  }

        //}

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindDivisionSummarySearchGrid(0);
                gvDivisionSummary.Visible = true;
            }
            catch (Exception ex)
            {
                gvDivisionSummary.Visible = false;
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDivisionSummary.PageIndex = e.NewPageIndex;

                BindDivisionSummarySearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDivisionSummary.EditIndex = -1;

                BindDivisionSummarySearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long DivisionSummaryID = Convert.ToInt64(((Label)gvDivisionSummary.Rows[e.RowIndex].FindControl("lblDivisionSummaryID")).Text);

                if (!IsValidDelete(DivisionSummaryID))
                {
                    return;
                }

                FloodOperationsBLL bllFloodOperations = new FloodOperationsBLL();

                bool IsDeleted = bllFloodOperations.DeleteDivisionSummary(DivisionSummaryID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                    BindDivisionSummarySearchGrid(0);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDivisionSummary.EditIndex = -1;

                BindDivisionSummarySearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadSearchCriteria(Int64 _DivisionSummaryID, Int64 _ZoneID, Int64 _CircleID, Int64 _DivisionID, Int64 _Year, string _DivisionSummaryStatus)
        {
            try
            {
                List<object> lstDivisionSummarySearch;

                lstDivisionSummarySearch = new FloodOperationsBLL().GetDivisionSummaryBySearchCriteria(_DivisionSummaryID
                , _ZoneID
                , _CircleID
                , _DivisionID
                , _Year
                , _DivisionSummaryStatus);

                gvDivisionSummary.DataSource = lstDivisionSummarySearch;
                gvDivisionSummary.DataBind();

                gvDivisionSummary.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvDivisionSummary.DataKeys[gvrow.RowIndex];
                int Year = Convert.ToInt32(key["Year"]);
                long DivisionID = Convert.ToInt64(key["DivisionID"]);

                if (e.CommandName == "Published")
                {
                    long DivisionSummaryID = Convert.ToInt64(e.CommandArgument);
                    bool IsSave = new FloodOperationsBLL().CheckDivisionSummaryStatusByID(DivisionSummaryID);
                    if (IsSave)
                    {
                        BindDivisionSummarySearchGrid(0);
                    }
                }
                else if (e.CommandName == "EditDivisionSummary")
                {
                    long DivisionSummaryID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddDivisionSummary.aspx?DivisionSummaryID=" + DivisionSummaryID);
                }
                else if (e.CommandName.Equals("PrintReport"))
                {
                    ReportData mdlReportData = new ReportData();
                    ReportParameter ReportParameter = new ReportParameter("DivisionID", Convert.ToString(DivisionID));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("Year", Convert.ToString(Year));
                    mdlReportData.Parameters.Add(ReportParameter);
                    mdlReportData.Name = Constants.DivisionSummaryReport;
                    // Set the ReportData in Session with specific Key
                    Session[SessionValues.ReportData] = mdlReportData;
                    string ReportViwerurl = "../" + Constants.ReportsUrl;
                    // Open the report printable in new tab
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvDivisionSummary.DataKeys[e.Row.RowIndex];
                int DivisionSummaryYear = Convert.ToInt32(key.Values["Year"]);

                Button hlEdit = (Button)e.Row.FindControl("hlEdit");
                Button hlPublish = (Button)e.Row.FindControl("btnPublish");
                Button btnDelete = (Button)e.Row.FindControl("btnButtonDelete");

                string StatusValue = ((Label)e.Row.FindControl("lblDivisionSummaryStatus")).Text;

                btnDelete.Enabled = false;
                hlEdit.Enabled = false;
                if (DateTime.Now.Year == DivisionSummaryYear && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF))
                {
                    btnDelete.Enabled = true;
                    hlEdit.Enabled = true;
                    //    CanDelete = true;
                }
                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    //  gvDivisionSummary.Columns[5].Visible = true;
                }
            }
        }

        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            ViewState.Add(UserIDKey, mdlUser.ID);

            if (mdlUser.RoleID != Constants.AdministratorRoleID)
            {
                if (mdlUser.UA_Designations.IrrigationLevelID != null)
                {
                    List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            #region Zone Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

                            #endregion Zone Level Bindings
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            #region Circle Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserZone.Add(mdlCircle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            #endregion Circle Level Bindings
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserCircle.Add((long)mdlDivision.CircleID);
                                lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            #endregion Division Level Bindings
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
        }

        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
        }
    }
}