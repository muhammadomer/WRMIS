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
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using System.Data;
using PMIU.WRMIS.BLL.WaterLosses;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets
{
    public partial class SearchAssets : BasePage
    {
        AssetsWorkBLL BLLAsset = new AssetsWorkBLL();
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

        public const string LevelIDKey = "LevelID";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string OfficeIDKey = "OfficeID";
        public const string AssetTypeIDKey = "AssetTypeID";
        public const string CategoryIDKey = "CategoryID";
        public const string SubCategoryIDKey = "SubCategoryID";
        public const string FloodAssociationIDKey = "FloodAssociationID";
        public const string StatusIDKey = "StatusID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys

        #region View State keys
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    dropdownEnabledDisabled();
                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        SetControlsValues();
                    }
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long AssetsID = Utility.GetNumericValueFromQueryString("AssetsID", 0);
                    if (AssetsID > 0)
                    {
                        hdnAssetsID.Value = AssetsID.ToString();
                        //if (_IsSaved)
                        //{
                        //    Master.ShowMessage(Message.RecordSaved.Description);
                        //    _IsSaved = false;
                        //}
                        //BindSearchResultsGrid(Convert.ToInt64(hdnAssetsID.Value));
                    }
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.AccountOfficer)
                        ddlFAssociation.Enabled = false;

                    hlAddNew.Enabled = base.CanAdd;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region Dropdown Lists Binding

        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer)
            {
                // Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
                //WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                //List<object> lstData = new List<object>();
                //DropDownList ddlToLoad = null;
                //lstData = bll_waterLosses.GetAllZones();
                //ddlToLoad = ddlZone;

                //if (lstData.Count > 0)
                //    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
            }
            else if (mdlUser.DesignationID != null)
            {
                WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                List<object> lstData = new List<object>();
                DropDownList ddlToLoad = null;
                lstData = bll_waterLosses.GetAllZones();
                ddlToLoad = ddlZone;

                if (lstData.Count > 0)
                    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
            }
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer)
            {
                //  Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
                //WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                //List<object> lstData = new List<object>();
                //DropDownList ddlToLoad = null;
                //lstData = bll_waterLosses.GetCirclesByZoneID(Convert.ToInt64(_ZoneID));
                //ddlToLoad = ddlCircle;

                //if (lstData.Count > 0)
                //    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);

            }
            else if (mdlUser.DesignationID != null)
            {
                WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                List<object> lstData = new List<object>();
                DropDownList ddlToLoad = null;
                lstData = bll_waterLosses.GetCirclesByZoneID(Convert.ToInt64(_ZoneID));
                ddlToLoad = ddlCircle;

                if (lstData.Count > 0)
                    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
            }
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer)
            {
                //  Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, _CircleID, false, (int)Constants.DropDownFirstOption.All);
                //WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                //List<object> lstData = new List<object>();
                //DropDownList ddlToLoad = null;
                //lstData = bll_waterLosses.GetDivisionsByCircleID(Convert.ToInt64(_CircleID));
                //ddlToLoad = ddlDivision;

                //if (lstData.Count > 0)
                //    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
            }
            else if (mdlUser.DesignationID != null)
            {
                WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
                List<object> lstData = new List<object>();
                DropDownList ddlToLoad = null;
                lstData = bll_waterLosses.GetDivisionsByCircleID(Convert.ToInt64(_CircleID));
                ddlToLoad = ddlDivision;

                if (lstData.Count > 0)
                    Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
            }
        }

        private void BindDropdownlists()
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) && IsHeadQuarterDivision == false)
                {
                    Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, false, (int)Constants.DropDownFirstOption.NoOption);
                    ListItem removeItem = ddlLevel.Items.FindByValue("All");
                    ddlLevel.Items.Remove(removeItem);
                }
                else
                {
                    if (IsHeadQuarterDivision)
                    {
                        Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, IsHeadQuarterDivision, (int)Constants.DropDownFirstOption.All);
                    }
                    else
                    {
                        if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                            Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, false, (int)Constants.DropDownFirstOption.NoOption);
                        else
                            Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, false, (int)Constants.DropDownFirstOption.All);
                    }

                }
                if (string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                {
                    if (!(mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer))
                        BindUserLocation();
                }
                Dropdownlist.DDLAssetOffice(ddlOffice, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLAssetsType(ddlAssetType, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLAssetCategory(ddlCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLYesNo(ddlFAssociation, (int)Constants.DropDownFirstOption.All);
               // Dropdownlist.DDLAssetStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
                if ((long)SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.AccountOfficer)
                    Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
                else
                    Dropdownlist.DDLAssetInspectionAllStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
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

                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);

                }

                ddlDivision.SelectedIndex = 0;

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

                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);

                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Dropdown Lists Binding

        #region Set Page Title

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title
        #region SearchCriteria
        protected void SetControlsValues()
        {

            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                object currentObj = Session["CurrentControlsValues"] as object;
                if (currentObj != null)
                {
                    if (Convert.ToString(currentObj.GetType().GetProperty("LevelID").GetValue(currentObj)) != "")
                    {
                        ddlLevel.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("LevelID").GetValue(currentObj));
                    }



                    BindZoneDropdown();
                    // BindSetDDLZone();
                    if (Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj)) != "")
                    {
                        ddlZone.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj));

                        DDLEmptyCircleDivision();
                        long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                        BindCircleDropdown(ZoneID);
                    }


                    if (Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj)) != "")
                    {
                        ddlCircle.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj));
                        long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                        BindDivisionDropdown(CircleID);

                    }

                    if (Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj)) != "")
                        ddlDivision.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("OfficeID").GetValue(currentObj)) != "")
                        ddlOffice.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("OfficeID").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("AssetType").GetValue(currentObj)) != "")
                        ddlAssetType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("AssetType").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("CategID").GetValue(currentObj)) != "")
                    {
                        ddlCategory.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CategID").GetValue(currentObj));
                        Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ddlCategory.SelectedValue), false, (int)Constants.DropDownFirstOption.All);
                    }

                    if (Convert.ToString(currentObj.GetType().GetProperty("SubCategID").GetValue(currentObj)) != "")
                        ddlSubCategory.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("SubCategID").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("FAID").GetValue(currentObj)) != "")
                        ddlFAssociation.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("FAID").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("StatusId").GetValue(currentObj)) != "")
                        ddlStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("StatusId").GetValue(currentObj));

                    if (Convert.ToString(currentObj.GetType().GetProperty("AssetName").GetValue(currentObj)) != "")
                    {
                        txtAssetName.Text = Convert.ToString(currentObj.GetType().GetProperty("AssetName").GetValue(currentObj));
                    }

                    BindSearchResultsGrid(0);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void SaveSearchCriteriaInSession()
        {
            try
            {
                Session["CurrentControlsValues"] = null;
                object obj = new
                {
                    LevelID = ddlLevel.SelectedItem.Value,
                    ZoneID = ddlZone.SelectedItem.Value,
                    CircleID = ddlCircle.SelectedItem.Value,
                    DivisionID = ddlDivision.SelectedItem.Value,
                    OfficeID = ddlOffice.SelectedItem.Value,
                    AssetType = ddlAssetType.SelectedItem.Value,
                    CategID = ddlCategory.SelectedItem.Value,
                    SubCategID = ddlSubCategory.SelectedItem.Value,
                    FAID = ddlFAssociation.SelectedItem.Value,
                    StatusId = ddlStatus.SelectedItem.Value,
                    AssetName = txtAssetName.Text
                };
                Session["CurrentControlsValues"] = obj;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion SearchCriteria
        #region Gridview Method

        private void BindSearchResultsGrid(long _AssetsID)
        {
            try
            {
                SaveSearchCriteriaInSession();
                Hashtable SearchCriteria = new Hashtable();
                string SelectedLevelText = null;
                long? SelectedLevelID = null;
                long? SelectedIrrigationBoundaryID = null;
                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                long? SelectedOfficeID = null;
                string SelectedAssetType = null;
                long? SelectedCategoryID = null;
                long? SelectedSubCategoryID = null;
                bool? SelectedFloodAssociationID = null;
                long? SelectedStatusID = null;
                string AssetName = null;
                string SelectedStatus = null;

                ////////////////////////////////////////////////////////
                if (ddlLevel.SelectedItem.Value != String.Empty)
                {
                    SelectedLevelID = Convert.ToInt64(ddlLevel.SelectedItem.Value);
                    SelectedLevelText = ddlLevel.SelectedItem.Text;
                }
                SearchCriteria.Add(LevelIDKey, ddlLevel.SelectedItem.Value);

                if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Zonal))
                {
                    if (ddlZone.SelectedItem.Value != "")
                        SelectedIrrigationBoundaryID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Circle))
                {
                    if (ddlCircle.SelectedItem.Value != "")
                        SelectedIrrigationBoundaryID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Divisional))
                {
                    if (ddlDivision.SelectedItem.Value != "")
                        SelectedIrrigationBoundaryID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Office))
                {
                    if (ddlOffice.SelectedItem.Value != "")
                        SelectedIrrigationBoundaryID = Convert.ToInt64(ddlOffice.SelectedItem.Value);
                }
                else
                {
                    if (ddlZone.SelectedItem.Value != "")
                        SelectedIrrigationBoundaryID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }

                ////////////////////////////////////////////////////////

                if (ddlZone.SelectedItem.Value != String.Empty)
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty)
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlOffice.SelectedItem.Value != String.Empty)
                {
                    SelectedOfficeID = Convert.ToInt64(ddlOffice.SelectedItem.Value);
                }
                SearchCriteria.Add(OfficeIDKey, ddlOffice.SelectedItem.Value);

                if (ddlAssetType.SelectedItem.Value != String.Empty)
                {
                    SelectedAssetType = ddlAssetType.SelectedItem.Text;
                }
                SearchCriteria.Add(AssetTypeIDKey, ddlAssetType.SelectedItem.Value);

                if (ddlCategory.SelectedItem.Value != String.Empty)
                {
                    SelectedCategoryID = Convert.ToInt64(ddlCategory.SelectedItem.Value);
                }
                SearchCriteria.Add(CategoryIDKey, ddlCategory.SelectedItem.Value);

                if (ddlSubCategory.SelectedItem.Value != String.Empty)
                {
                    SelectedSubCategoryID = Convert.ToInt64(ddlSubCategory.SelectedItem.Value);
                }
                SearchCriteria.Add(SubCategoryIDKey, ddlSubCategory.SelectedItem.Value);

                if (ddlFAssociation.SelectedItem.Value != String.Empty)
                {
                    SelectedFloodAssociationID = Convert.ToBoolean(Convert.ToUInt16((ddlFAssociation.SelectedItem.Value)));
                }
                SearchCriteria.Add(FloodAssociationIDKey, ddlFAssociation.SelectedItem.Value);

                if (ddlStatus.SelectedItem.Value != String.Empty)
                {
                    SelectedStatusID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                }
                SearchCriteria.Add(StatusIDKey, ddlStatus.SelectedItem.Value);

                if (txtAssetName.Text != "")
                {
                    AssetName = txtAssetName.Text;
                }
                if (ddlStatus.SelectedItem.Value != String.Empty)
                {
                    SelectedStatus = Convert.ToString(ddlStatus.SelectedItem.Text);
                }

                var AssetsID = _AssetsID == 0 ? (long?)null : _AssetsID;

                IEnumerable<DataRow> IeAssets = new AssetsWorkBLL().GetAssetsSearch(SelectedLevelText, AssetsID, SelectedLevelID, SelectedIrrigationBoundaryID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedCategoryID, SelectedSubCategoryID, SelectedFloodAssociationID, SelectedAssetType, SelectedStatus, AssetName);
                var LstAssets = IeAssets.Select(dataRow => new
                {
                    AssetsID = dataRow.Field<long>("AssetsID"),
                    CategoryName = dataRow.Field<string>("CategoryName"),
                    SubCategoryName = dataRow.Field<string>("SubCategoryName"),
                    IrrigationLevelID = dataRow.Field<string>("IrrigationLevelID"),
                    IrrigationBoundryID = dataRow.Field<long>("IrrigationBoundryID"),
                    Location = dataRow.Field<string>("Location"),
                    AssetName = dataRow.Field<string>("AssetName"),
                    AssetType = dataRow.Field<string>("AssetType"),
                    FOAssociation = dataRow.Field<bool>("FOAssociation"),
                    STATUS = dataRow.Field<string>("STATUS"),
                    CreatedBy = dataRow.Field<long>("CreatedBy"),
                }).ToList();
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                if (IsHeadQuarterDivision)
                {
                    LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "OFFICE");
                }
                else
                {
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                    {
                        LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "ZONE");
                        LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "CIRCLE");
                        LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "DIVISION");
                    }
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SE) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ChiefIrrigation))
                    {
                        LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "OFFICE");
                    }
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SE))
                    {
                        LstAssets.RemoveAll(x => x.GetType().GetProperty("IrrigationLevelID").GetValue(x).ToString().ToUpper() == "ZONE");
                    }
                }

                gvAssets.DataSource = LstAssets;
                gvAssets.DataBind();
                SearchCriteria.Add(PageIndexKey, gvAssets.PageIndex);
                Session[SessionValues.SearchAssets] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssets_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvAssets.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssets.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long AssetID = Convert.ToInt64(((Label)gvAssets.Rows[e.RowIndex].FindControl("lblAssetsID")).Text);

                bool IsDeleted = new AssetsWorkBLL().DeleteAssetByID(AssetID);

                if (IsDeleted)
                {
                    BindSearchResultsGrid(0);
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Gridview Method
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

                            #endregion
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

                            #endregion
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

                            #endregion
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
            SaveSearchCriteriaInSession();
            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
        }

        public void BindSetDDLZone()
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

                            #endregion
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

                            #endregion
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

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ddlCategory.SelectedValue), false, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropdownEnabledDisabled();

        }
        #region "dropdownEnabledDisabled"
        public void dropdownEnabledDisabled()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Zonal))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = false;
                ddlDivision.Enabled = false;
                ddlOffice.Enabled = false;
                ddlCircle.ClearSelection();
                ddlDivision.ClearSelection();
                ddlOffice.ClearSelection();
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Circle))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = false;
                ddlOffice.Enabled = false;
                ddlDivision.ClearSelection();
                ddlOffice.ClearSelection();
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Divisional))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = true;
                ddlOffice.Enabled = false;
                ddlOffice.ClearSelection();
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Office))
            {
                ddlZone.Enabled = false;
                ddlCircle.Enabled = false;
                ddlDivision.Enabled = false;
                ddlOffice.Enabled = true;

                ddlZone.ClearSelection();
                ddlCircle.ClearSelection();
                ddlDivision.ClearSelection();
            }
            else
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = true;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                if (IsHeadQuarterDivision == true || mdlUser.DesignationID == (long)Constants.Designation.XEN || mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE)
                {
                    ddlOffice.Enabled = false;
                }
                else
                {
                    ddlOffice.Enabled = true;
                }

            }
        }
        #endregion

        protected void gvAssets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Inspection")
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                int AssetsID = Convert.ToInt32(gvAssets.DataKeys[row.RowIndex].Values[0]);
                string AssetType = Convert.ToString(gvAssets.DataKeys[row.RowIndex].Values[1]);
                if (AssetType == "Individual Item")
                {
                    Response.Redirect("InspectionAssetsIndividual.aspx?AssetsID=" + AssetsID);
                }
                else
                {
                    Response.Redirect("InspectionAssetsLot.aspx?AssetsID=" + AssetsID);
                }

            }
            else if (e.CommandName == "InspectionHistory")
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                int AssetsID = Convert.ToInt32(gvAssets.DataKeys[row.RowIndex].Values[0]);
                string AssetType = Convert.ToString(gvAssets.DataKeys[row.RowIndex].Values[1]);
                Response.Redirect("InspectionAssetsHistory.aspx?AssetsID=" + AssetsID + "&AssetType=" + AssetType);
            }
        }

        protected void gvAssets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvAssets.DataKeys[e.Row.RowIndex];
                    int AssetsID = Convert.ToInt32(key.Values["AssetsID"]);
                    string AssetStatus = Convert.ToString(key.Values["Status"]);
                    long CreatedBy = Convert.ToInt64(key.Values["CreatedBy"]);
                    #endregion

                    #region "Controls"

                    Button btnInspection = (Button)e.Row.FindControl("btnInspection");
                    Button btnInspectionHistory = (Button)e.Row.FindControl("btnInspectionHistory");
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                    Button btnDelete = (Button)e.Row.FindControl("ButtonDelete");
                    #endregion

                    if (base.CanEdit == false)
                    {
                        hlView.Visible = true;
                    }
                    else
                    {
                        hlView.Visible = false;
                    }
                    if (CreatedBy != (long)SessionManagerFacade.UserInformation.ID)
                    {
                        btnInspection.Enabled = false;
                    }

                    bool IsExist = BLLAsset.IsAssetInspectionExists(AssetsID);
                    if (IsExist)
                    {
                        btnDelete.Enabled = false;
                    }
                    if (AssetStatus == "Auctioned")
                    {
                        btnInspection.Enabled = false;
                        hlEdit.Visible = false;
                        btnDelete.Enabled = false;
                        if (base.CanView == true)
                            hlView.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}