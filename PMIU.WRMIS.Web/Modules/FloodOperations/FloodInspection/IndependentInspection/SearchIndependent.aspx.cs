using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Antlr.Runtime;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class SearchIndependent : BasePage
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

        #region Hash Table Keys

        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string InfrastructureTypeIDKey = "InfrastructureTypeID";
        public const string InfrastructureNameIDKey = "InfrastructureNameID";
        public const string InspectionTypeIDKey = "InspectionTypeID";
        public const string StatusIDKey = "StatusID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys

        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        #region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (FloodInspectionID > 0)
                    {
                        hdnFloodInspectionID.Value = FloodInspectionID.ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    }
                    //if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    //{
                    //    bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                    //    if (ShowHistory)
                    //    {
                    //        if (Session[SessionValues.SearchFloodInspection] != null)
                    //        {
                    //            BindHistoryData();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                    //        //txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //        //BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    //    }
                    //}
                    //else
                    //{
                    //    BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    //    //txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                    //    //txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //}
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion PageLoad

        #region Dropdown Lists Binding

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

        private void BindDropdownlists()
        {
            try
            {
                //BindZoneDropdown();
                //DDLEmptyCircleDivision();
                BindUserLocation();
                BindInfrastructureTypeDropdown();
                BindInspectionTypeDropDown();

                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    divStatus.Attributes.Add("style", "display:visible");
                    BindInspectionStatusDropDown();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //  ddlCircle.Enabled = false;
                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    //  ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                //  ddlDivision.Enabled = false;

                // gvSearchIndependent.Visible = false;
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
                    //   ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    //  ddlDivision.Enabled = true;
                }
                //   gvSearchIndependent.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users _Users = SessionManagerFacade.UserInformation;

            if (ddlInfrastructureType.SelectedItem.Value != "")
            {
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1, (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2, (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3, (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindInspectionStatusDropDown()
        {
            try
            {
                Dropdownlist.DDLActiveInspectionStatus(ddlStatus, false, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInspectionTypeDropDown()
        {
            try
            {
                Dropdownlist.DDLActiveInspectionType(ddlInspectionType, false, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInfrastructureTypeDropdown()
        {
            try
            {
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Dropdown Lists Binding

        #region Set Page Title

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title

        #region Gridview Method

        private void BindSearchResultsGrid(long _FloodInspectionID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                string SelectedInfrastyructureTypeID = null;
                string SelectedInfrastyructureName = null;
                long? SelectedInspectionTypeID = null;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                long? SelectedStatusID = null;

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }
                if (ddlZone.SelectedItem.Value != String.Empty && ddlZone.SelectedItem.Value != "0")
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty && ddlCircle.SelectedItem.Value != "0")
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty && ddlDivision.SelectedItem.Value != "0")
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlInfrastructureType.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureTypeID = Convert.ToString(ddlInfrastructureType.SelectedItem.Text) == "Barrage/Headwork" ? "Control Structure1" : Convert.ToString(ddlInfrastructureType.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureTypeIDKey, SelectedInfrastyructureTypeID);

                if (ddlInfrastructureName.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureName = Convert.ToString(ddlInfrastructureName.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureNameIDKey, ddlInfrastructureName.SelectedItem.Text);

                if (ddlStatus.SelectedItem.Value != String.Empty && ddlStatus.SelectedItem.Value != "0")
                {
                    SelectedStatusID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                }

                SearchCriteria.Add(StatusIDKey, ddlStatus.SelectedItem.Value);

                if (ddlInspectionType.SelectedItem.Value != String.Empty && ddlInspectionType.SelectedItem.Value != "0")
                {
                    SelectedInspectionTypeID = Convert.ToInt64(ddlInspectionType.SelectedItem.Value);
                }

                SearchCriteria.Add(InspectionTypeIDKey, ddlInspectionType.SelectedItem.Value);

                var ID = _FloodInspectionID == 0 ? (long?)null : _FloodInspectionID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> ieFloodInapection = new FloodOperationsBLL().GetFloodInspectionSearch(SelectedInfrastyructureTypeID, ID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedInspectionTypeID, SelectedStatusID, SelectedInfrastyructureName, FromDate, ToDate, mdlUser.ID);

                var lstFloodInspection = ieFloodInapection.Select(dataRow => new
                {
                    FloodInspectionID = dataRow.Field<long>("FloodInspectionID"),
                    InspectionDate = dataRow.Field<DateTime>("InspectionDate"),
                    InspectionTypeID = dataRow.Field<Int16>("InspectionTypeID"),
                    InspectionType = dataRow.Field<string>("InspectionType"),
                    InfrastructureStatusID = dataRow.Field<Int16>("InfrastructureStatusID"),
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    InfrastructureStatus = dataRow.Field<string>("InfrastructureStatus"),
                    InspectionYear = dataRow.Field<int>("InspectionYear"),
                    DivisionID = dataRow.Field<Int64>("DivisionID"),
                    StructureTypeID = dataRow.Field<Int64>("StructureTypeID"),
                    StructureID = dataRow.Field<Int64>("StructureID"),
                }).ToList();

                gvSearchIndependent.DataSource = lstFloodInspection;
                gvSearchIndependent.DataBind();

                SearchCriteria.Add(PageIndexKey, gvSearchIndependent.PageIndex);

                Session[SessionValues.SearchFloodInspection] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchFloodInspection];
            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];
            string ZoneID = (string)SearchCriteria[ZoneIDKey];
            string CircleID = (string)SearchCriteria[CircleIDKey];
            string DivisionID = (string)SearchCriteria[DivisionIDKey];
            string InfrastructureTypeID = (string)SearchCriteria[InfrastructureTypeIDKey];
            string StatusID = (string)SearchCriteria[StatusIDKey];
            string InfrastructureNameID = (string)SearchCriteria[InfrastructureNameIDKey];
            string InspectionTypeID = (string)SearchCriteria[InspectionTypeIDKey];

            if (ZoneID != String.Empty)
            {
                ddlZone.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID);
            }
            if (CircleID != String.Empty)
            {
                ddlCircle.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlCircle, CircleID);
            }
            if (DivisionID != string.Empty && ddlDivision.Enabled == true)
            {
                ddlDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
            }
            if (InfrastructureTypeID != String.Empty)
            {
                ddlInfrastructureType.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlInfrastructureType, InfrastructureTypeID);
            }

            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }

            if (InfrastructureNameID != String.Empty)
            {
                ddlInfrastructureName.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlInfrastructureName, InfrastructureNameID);
            }

            if (InspectionTypeID != String.Empty)
            {
                ddlInspectionType.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlInspectionType, InspectionTypeID);
            }

            gvSearchIndependent.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
            //gvSearchIndependent.Visible = true;
        }

        protected void gvSearchIndependent_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSearchIndependent.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchIndependent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchIndependent.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchIndependent_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long FloodInspectionID = Convert.ToInt64(((Label)gvSearchIndependent.Rows[e.RowIndex].FindControl("lblFloodInspectionID")).Text);

                if (new FloodOperationsBLL().IsFloodInspectionDependencyExists(Convert.ToInt64(FloodInspectionID)))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }


                //bool IsDeleted = new FloodOperationsBLL().DeleteFloodInspection(FloodInspectionID);  Previous Delete Funtion 
                bool IsDeleted = new FloodOperationsBLL().DeleteInspectionByFloodInspectionID(FloodInspectionID);  // Make new delete Functin after the discussin with Wajahat bhai 

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchIndependent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool CanEditInspection = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region "Data Keys"

                DataKey key = gvSearchIndependent.DataKeys[e.Row.RowIndex];
                long thisFloodInspectionID = Convert.ToInt64(key.Values["FloodInspectionID"]);
                short thisInspectionTypeID = Convert.ToInt16(key.Values["InspectionTypeID"]);
                short InfrastructureStatusID = Convert.ToInt16(key.Values["InfrastructureStatusID"]);
                int _InspectionYear = Convert.ToInt16(key.Values["InspectionYear"]);
                string thisInfrastructureType = Convert.ToString(key.Values["InfrastructureType"]);
                string thisInfrastructureStatusID = Convert.ToString(key.Values["InfrastructureStatusID"]);

                #endregion "Data Keys"

                HyperLink hlBreachingSection = (HyperLink)e.Row.FindControl("hlBreachingSection");
                HyperLink hlGeneralCondition = (HyperLink)e.Row.FindControl("hlGeneralCondition");
                HyperLink hlStonePosition = (HyperLink)e.Row.FindControl("hlStonePosition");
                HyperLink hlRDwisecondition = (HyperLink)e.Row.FindControl("hlRDwisecondition");
                HyperLink hlMBStatus = (HyperLink)e.Row.FindControl("hlMBStatus");
                HyperLink hlProblems = (HyperLink)e.Row.FindControl("hlProblems");
                HyperLink hlPrintPreview = (HyperLink)e.Row.FindControl("hlPrintPreview");
                Button hlEdit = (Button)e.Row.FindControl("hlEdit");
                Button hlPublish = (Button)e.Row.FindControl("hlPublish");
                //LinkButton linkButtonDelete = (LinkButton)e.Row.FindControl("linkButtonDelete");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                hlProblems.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/ProblemForFI.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InfrastructureType=" + Server.UrlEncode(thisInfrastructureType) + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]);

                string StatusValue = ((Label)e.Row.FindControl("lblFloodInspectionStatus")).Text;

                //linkButtonDelete.Attributes.Add("style", "display:none;");
                //hlEdit.Attributes.Add("style", "display:none;");
                //linkButtonDelete.Enabled = false;
                btnDelete.Enabled = false;
                hlEdit.Enabled = false;
                //if (StatusValue.Equals("Draft"))
                //{
                //    // e.row.cells[5].text = string.empty;
                //    //e.row.cells[5].visible = false;

                //    //linkButtonDelete.Attributes.Add("style", "display:visible;");
                //    //linkButtonDelete.Enabled = true;
                //    btnDelete.Enabled = true;
                //    //hlEdit.Attributes.Add("style", "display:visible;");
                //    hlEdit.Enabled = true;
                //    CanDelete = true;
                //    hlPublish.Enabled = true;
                //}

                if (thisInspectionTypeID == 1)
                {
                    CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, InfrastructureStatusID, 1);
                    if (CanEditInspection)
                    {
                        hlEdit.Enabled = CanEditInspection;
                        hlPublish.Enabled = CanEditInspection;
                        btnDelete.Enabled = CanEditInspection;
                    }
                }
                else if (thisInspectionTypeID == 2)
                {
                    CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, InfrastructureStatusID, 2);
                    if (CanEditInspection)
                    {
                        hlEdit.Enabled = CanEditInspection;
                        btnDelete.Enabled = CanEditInspection;
                        hlPublish.Enabled = CanEditInspection;
                    }
                }
                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    gvSearchIndependent.Columns[5].Visible = true;
                }

                if (thisInfrastructureType == "Protection Infrastructure")
                {
                    hlGeneralCondition.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/GCProtectionInfrastructure.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]);

                    if (thisInspectionTypeID == 1)
                    {
                        hlBreachingSection.Visible = true;
                    }
                    else if (thisInspectionTypeID == 2)
                    {
                        hlBreachingSection.Visible = true;
                        hlStonePosition.Visible = true;
                    }
                }
                else if (thisInfrastructureType == "Control Structure1")
                {
                    hlGeneralCondition.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/GCBarrageHW.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]); ;

                    if (thisInspectionTypeID == 1)
                    {
                        //hlRDwisecondition.Enabled = false;
                    }
                    else if (thisInspectionTypeID == 2)
                    {
                        //hlRDwisecondition.Enabled = false;
                        hlStonePosition.Visible = true;
                    }
                }
                else if (thisInfrastructureType == "Drain")
                {
                    hlGeneralCondition.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/GCDrain.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]); ;

                    if (thisInspectionTypeID == 1)
                    {
                        //hlRDwisecondition.Enabled = true;
                    }
                    else if (thisInspectionTypeID == 2)
                    {
                        //hlRDwisecondition.Enabled = true;
                        hlStonePosition.Visible = true;
                    }
                }

                if (thisInspectionTypeID == 1)
                {
                    hlMBStatus.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/MeasuringBookStatusPreFlood.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]);
                }
                else if (thisInspectionTypeID == 2)
                {
                    hlMBStatus.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/MeasuringBookStatusPostFlood.aspx?FloodInspectionID=" + thisFloodInspectionID.ToString() + "&InspectionYear=" + Convert.ToInt32(key.Values["InspectionYear"]) + "&InspectionTypeID=" + Convert.ToInt16(key.Values["InspectionTypeID"]);
                }

                //if (ddlInfrastructureType.SelectedItem.Value == "1" && ddlInspectionType.SelectedItem.Value == "1")
                //{
                //    hlBreachingSection.Visible = true;
                //}
                //else if (ddlInfrastructureType.SelectedItem.Value == "1" && ddlInspectionType.SelectedItem.Value == "2")
                //{
                //    hlBreachingSection.Visible = true;
                //    hlStonePosition.Visible = true;
                //}
                //else if (ddlInfrastructureType.SelectedItem.Value == "2" && ddlInspectionType.SelectedItem.Value == "1")
                //{
                //    hlRDwisecondition.Visible = false;
                //}
                //else if (ddlInfrastructureType.SelectedItem.Value == "2" && ddlInspectionType.SelectedItem.Value == "2")
                //{
                //    hlRDwisecondition.Visible = false;
                //    hlStonePosition.Visible = true;
                //}
                //else if (ddlInfrastructureType.SelectedItem.Value == "3" && ddlInspectionType.SelectedItem.Value == "1")
                //{
                //    hlRDwisecondition.Visible = true;
                //}
                //else if (ddlInfrastructureType.SelectedItem.Value == "3" && ddlInspectionType.SelectedItem.Value == "2")
                //{
                //    hlRDwisecondition.Visible = true;
                //    hlStonePosition.Visible = true;
                //}

                if (thisInfrastructureType == "Control Structure1")
                {
                    hlRDwisecondition.Visible = false;
                }
            }
        }

        protected void gvSearchIndependent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvSearchIndependent.DataKeys[gvrow.RowIndex];
                int InfrastructureTypeID = 0;
                long StructureID = Convert.ToInt32(key["StructureID"]);
                long DivisionID = Convert.ToInt64(key["DivisionID"]);
                string InfrastructureType = Convert.ToString(key["InfrastructureType"]);
                int InspectionYear = Convert.ToInt32(key["InspectionYear"]);
                int InspectionTypeID = Convert.ToInt16(key["InspectionTypeID"]);
                if (InfrastructureType == "Protection Infrastructure")
                    InfrastructureTypeID = 1;
                else if (InfrastructureType == "Control Structure1")
                    InfrastructureTypeID = 2;
                else if (InfrastructureType == "Drain")
                    InfrastructureTypeID = 3;

                if (e.CommandName == "Published")
                {
                    long FloodInspectionID = Convert.ToInt64(e.CommandArgument);
                    bool IsSave = new FloodInspectionsBLL().CheckInspectionStatusByID(FloodInspectionID);
                    if (IsSave)
                    {
                        BindSearchResultsGrid(0);
                    }
                    Master.ShowMessage("Published Successfully", SiteMaster.MessageType.Success);
                    return;
                }
                else if (e.CommandName == "EditIndependentInspection")
                {
                    long FloodInspectionID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddIndependent.aspx?FloodInspectionID=" + FloodInspectionID);
                }
                else if (e.CommandName.Equals("PrintReport"))
                {
                    ReportData mdlReportData = new ReportData();
                    ReportParameter ReportParameter = new ReportParameter("Year", Convert.ToString(InspectionYear));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("DivisionID", Convert.ToString(DivisionID));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("StructureTypeID", Convert.ToString(InfrastructureTypeID));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("StructureID", Convert.ToString(StructureID));
                    mdlReportData.Parameters.Add(ReportParameter);
                    ReportParameter = new ReportParameter("InspectionTypeID", Convert.ToString(InspectionTypeID));
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.IndependentInspectionReport;
                    // Set the ReportData in Session with specific Key
                    Session[SessionValues.ReportData] = mdlReportData;
                    string ReportViwerurl = "../../" + Constants.ReportsUrl;
                    // Open the report printable in new tab
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Gridview Method

        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid(0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}