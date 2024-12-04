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
    public partial class AddAssets : BasePage
    {
        #region View State keys
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int AssetID = 0;
                int ViewAssetID = 0;
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    dropdownEnabledDisabled();
                    AssetID = Utility.GetNumericValueFromQueryString("AssetsID", 0);
                    ViewAssetID = Utility.GetNumericValueFromQueryString("View", 0);

                    if (ViewAssetID != 0)
                    {
                        hdnAssetID.Value = Convert.ToString(ViewAssetID);
                    }
                    else
                    {
                        hdnAssetID.Value = Convert.ToString(AssetID);
                    }


                    hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/SearchAssets.aspx?RestoreState=1");
                    //Edit
                    if (ViewAssetID != 0)
                    {
                        LoadViewAssetByID(ViewAssetID);
                        if (ViewAssetID > 0)
                        {
                            h3PageTitle.InnerText = "View Asset";
                        }
                    }
                    else
                    {
                        LoadAssetByID(AssetID);
                        if (AssetID > 0)
                        {
                            h3PageTitle.InnerText = "Edit Asset";
                        }
                    }

                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.AccountOfficer)
                        rdAssociatFlood.Enabled = false;

                    btnSave.Enabled = base.CanAdd;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }
        #region Set Page Title
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title
        #region Dropdown Lists Binding

        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
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

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.Select);
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

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.Select);
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
                        Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, IsHeadQuarterDivision, (int)Constants.DropDownFirstOption.Select);
                    }
                    else
                    {
                        if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                            Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, false, (int)Constants.DropDownFirstOption.NoOption);
                        else
                            Dropdownlist.DDLAssetIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, false, (int)Constants.DropDownFirstOption.Select);
                    }
                }
                if (Utility.GetNumericValueFromQueryString("AssetsID", 0) == 0)
                {
                    if (!(mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer))
                        BindUserLocation();
                }

                Dropdownlist.DDLAssetOffice(ddlOffice, false, (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.DDLAssetCategory(ddlCategory, false, (int)Constants.DropDownFirstOption.Select);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.Select);

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

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.Select);

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
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
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
        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.Select);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.Select);
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
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategory.SelectedItem.Value == "")
                {
                    gvAssetAttribute.Visible = false;
                    DivAssetDetail.Visible = false;
                }
                else
                {
                    Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ddlCategory.SelectedValue), false, (int)Constants.DropDownFirstOption.Select);
                    gvAssetAttribute.Visible = false;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion Dropdown Lists Binding
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

                ddlZone.CssClass = "form-control required";
                ddlZone.Attributes.Add("required", "required");

                ddlCircle.CssClass = "form-control";
                ddlCircle.Attributes.Remove("required");

                ddlDivision.CssClass = "form-control";
                ddlDivision.Attributes.Remove("required");

                ddlOffice.CssClass = "form-control";
                ddlOffice.Attributes.Remove("required");
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Circle))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = false;
                ddlOffice.Enabled = false;
                ddlDivision.ClearSelection();

                ddlZone.CssClass = "form-control required";
                ddlZone.Attributes.Add("required", "required");

                ddlCircle.CssClass = "form-control required";
                ddlCircle.Attributes.Add("required", "required");

                ddlDivision.CssClass = "form-control";
                ddlDivision.Attributes.Remove("required");

                ddlOffice.CssClass = "form-control";
                ddlOffice.Attributes.Remove("required");

            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Divisional))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = true;
                ddlOffice.Enabled = false;

                ddlZone.CssClass = "form-control required";
                ddlZone.Attributes.Add("required", "required");

                ddlCircle.CssClass = "form-control required";
                ddlCircle.Attributes.Add("required", "required");

                ddlDivision.CssClass = "form-control required";
                ddlDivision.Attributes.Add("required", "required");

                ddlOffice.CssClass = "form-control";
                ddlOffice.Attributes.Remove("required");

            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Office))
            {
                ddlZone.Enabled = false;
                ddlCircle.Enabled = false;
                ddlDivision.Enabled = false;
                ddlOffice.Enabled = true;

                ddlZone.CssClass = "form-control";
                ddlZone.Attributes.Remove("required");

                ddlCircle.CssClass = "form-control";
                ddlCircle.Attributes.Remove("required");

                ddlDivision.CssClass = "form-control";
                ddlDivision.Attributes.Remove("required");

                ddlOffice.CssClass = "form-control required";
                ddlOffice.Attributes.Add("required", "required");
            }
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
        #endregion
        #region AlreadyAdded
        private void LoadAssetByID(long _AssetsID)
        {
            try
            {
                string irrigationlevel = "";
                UA_Users _Users = SessionManagerFacade.UserInformation;
                AM_AssetItems ObjAssetItems = new AssetsWorkBLL().GetAssetID(_AssetsID);
                if (ObjAssetItems != null)
                {
                    txtAssetName.Text = Convert.ToString(ObjAssetItems.AssetName);
                    hdnCreatedDate.Value = Convert.ToString(ObjAssetItems.CreatedDate);
                    hdnCreatedBy.Value = Convert.ToString(ObjAssetItems.CreatedBy);
                    AM_SubCategory ObjCatID = new AssetsWorkBLL().GetcatID(ObjAssetItems.SubCategoryID);
                    Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ObjCatID.CategoryID), false, (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlSubCategory, Convert.ToString(ObjAssetItems.SubCategoryID));
                    Dropdownlist.SetSelectedValue(ddlCategory, Convert.ToString(ObjCatID.CategoryID));

                    //   Dropdownlist.SetSelectedValue(ddlLevel, Convert.ToString(ObjAssetItems.IrrigationLevelID));
                    //DDLAssetViewIrrigationLevel
                    Dropdownlist.DDLAssetViewIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, (int)Constants.DropDownFirstOption.Select);
                    ddlLevel.SelectedValue = Convert.ToString(ObjAssetItems.IrrigationLevelID);
                    txtEstimatedvalue.Text = Convert.ToString(ObjAssetItems.EstimatedValue);
                    string FOBit = Convert.ToString(ObjAssetItems.FOBit) == true.ToString() ? "1" : "0";
                    rdAssociatFlood.SelectedValue = FOBit;
                    if (rdAssociatFlood.SelectedValue == "1")
                    {
                        rdAssetType.Enabled = false;
                    }
                    if (Convert.ToString(ObjAssetItems.AssetType) == "Lot")
                    {
                        rdAssetType.SelectedValue = "1";
                        txtQty.Text = Convert.ToString(ObjAssetItems.LotQuantity);
                        txtUnits.Text = Convert.ToString(ObjAssetItems.Units);
                        div_QtyUnits.Visible = true;
                    }
                    else
                    {
                        rdAssetType.SelectedValue = "2";
                    }

                    irrigationlevel = Convert.ToString(ObjAssetItems.IrrigationLevelID);
                    if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "1")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Zonal);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "2")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Circle);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "3")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Divisional);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "6")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Office);
                    }

                    if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Zonal))
                    {
                        Dropdownlist.DDLAddAssetZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Circle))
                    {
                        Dropdownlist.DDLAddAssetCircles(ddlCircle, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                        Dropdownlist.DDLZoneByCirclelID(ddlZone, Convert.ToInt64(ddlCircle.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ddlZone.SelectedValue));
                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Divisional))
                    {
                        Dropdownlist.DDLAddAssetDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                        Dropdownlist.DDLCirlceByDivisionlID(ddlCircle, Convert.ToInt64(ddlDivision.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(ddlCircle.SelectedValue));

                        Dropdownlist.DDLZoneByCirclelID(ddlZone, Convert.ToInt64(ddlCircle.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ddlZone.SelectedValue));
                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Office))
                    {
                        Dropdownlist.SetSelectedValue(ddlOffice, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                    }



                    LoadAssetDetail();
                   
                    ddlCategory.Enabled = false;
                    ddlSubCategory.Enabled = false;
                    rdAssetType.Enabled = false;
                    rdAssociatFlood.Enabled = false;
                    //physical location
                    ddlLevel.Enabled = false;
                    ddlZone.Enabled = false;
                    ddlCircle.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlOffice.Enabled = false;
                    //required remove
                    ddlCategory.CssClass = ddlCategory.CssClass.Replace("required", "");
                    ddlSubCategory.CssClass = ddlCategory.CssClass.Replace("required", "");
                    ddlLevel.CssClass = ddlCategory.CssClass.Replace("required", "");

                    ddlZone.CssClass = ddlZone.CssClass.Replace("required", "");
                    ddlCircle.CssClass = ddlCircle.CssClass.Replace("required", "");
                    ddlDivision.CssClass = ddlDivision.CssClass.Replace("required", "");
                    ddlOffice.CssClass = ddlOffice.CssClass.Replace("required", "");
                    // Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                    // Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjAssetItems.DivisionID));

                }
            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void LoadViewAssetByID(long _AssetsID)
        {
            try
            {
                string irrigationlevel = "";
                UA_Users _Users = SessionManagerFacade.UserInformation;
                AM_AssetItems ObjAssetItems = new AssetsWorkBLL().GetAssetID(_AssetsID);
                if (ObjAssetItems != null)
                {
                    txtAssetName.Text = Convert.ToString(ObjAssetItems.AssetName);
                    hdnCreatedDate.Value = Convert.ToString(ObjAssetItems.CreatedDate);
                    hdnCreatedBy.Value = Convert.ToString(ObjAssetItems.CreatedBy);
                    AM_SubCategory ObjCatID = new AssetsWorkBLL().GetcatID(ObjAssetItems.SubCategoryID);
                    Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ObjCatID.CategoryID), false, (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlSubCategory, Convert.ToString(ObjAssetItems.SubCategoryID));
                    Dropdownlist.SetSelectedValue(ddlCategory, Convert.ToString(ObjCatID.CategoryID));

                    //   Dropdownlist.SetSelectedValue(ddlLevel, Convert.ToString(ObjAssetItems.IrrigationLevelID));
                    //DDLAssetViewIrrigationLevel
                    Dropdownlist.DDLAssetViewIrrigationLevel(ddlLevel, SessionManagerFacade.UserInformation.UA_Designations.ID, (int)Constants.DropDownFirstOption.Select);
                    ddlLevel.SelectedValue = Convert.ToString(ObjAssetItems.IrrigationLevelID);
                    txtEstimatedvalue.Text = Convert.ToString(ObjAssetItems.EstimatedValue);
                    string FOBit = Convert.ToString(ObjAssetItems.FOBit) == true.ToString() ? "1" : "0";
                    rdAssociatFlood.SelectedValue = FOBit;
                    if (rdAssociatFlood.SelectedValue == "1")
                    {
                        rdAssetType.Enabled = false;
                    }
                    if (Convert.ToString(ObjAssetItems.AssetType) == "Lot")
                    {
                        rdAssetType.SelectedValue = "1";
                        txtQty.Text = Convert.ToString(ObjAssetItems.LotQuantity);
                        txtUnits.Text = Convert.ToString(ObjAssetItems.Units);
                        div_QtyUnits.Visible = true;
                    }
                    else
                    {
                        rdAssetType.SelectedValue = "2";
                    }

                    irrigationlevel = Convert.ToString(ObjAssetItems.IrrigationLevelID);
                    if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "1")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Zonal);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "2")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Circle);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "3")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Divisional);
                    }
                    else if (Convert.ToString(ObjAssetItems.IrrigationLevelID) == "6")
                    {
                        irrigationlevel = Convert.ToString(Constants.AssetsLevel.Office);
                    }

                    if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Zonal))
                    {
                        Dropdownlist.DDLAddAssetZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Circle))
                    {
                        Dropdownlist.DDLAddAssetCircles(ddlCircle, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                        Dropdownlist.DDLZoneByCirclelID(ddlZone, Convert.ToInt64(ddlCircle.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ddlZone.SelectedValue));

                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Divisional))
                    {
                        Dropdownlist.DDLAddAssetDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                        Dropdownlist.DDLCirlceByDivisionlID(ddlCircle, Convert.ToInt64(ddlDivision.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(ddlCircle.SelectedValue));

                        Dropdownlist.DDLZoneByCirclelID(ddlZone, Convert.ToInt64(ddlCircle.SelectedValue), (int)Constants.DropDownFirstOption.NoOption);
                        Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(ddlZone.SelectedValue));
                    }
                    else if (irrigationlevel == Convert.ToString(Constants.AssetsLevel.Office))
                    {
                        Dropdownlist.SetSelectedValue(ddlOffice, Convert.ToString(ObjAssetItems.IrrigationBoundryID));
                    }



                    LoadAssetDetail();
                    ddlCategory.Enabled = false;
                    ddlSubCategory.Enabled = false;
                    rdAssetType.Enabled = false;
                    rdAssociatFlood.Enabled = false;
                    //physical location
                    ddlLevel.Enabled = false;
                    ddlZone.Enabled = false;
                    ddlCircle.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlOffice.Enabled = false;
                    //required remove
                    ddlCategory.CssClass = ddlCategory.CssClass.Replace("required", "");
                    ddlSubCategory.CssClass = ddlCategory.CssClass.Replace("required", "");
                    ddlLevel.CssClass = ddlCategory.CssClass.Replace("required", "");

                    ddlZone.CssClass = ddlZone.CssClass.Replace("required", "");
                    ddlCircle.CssClass = ddlCircle.CssClass.Replace("required", "");
                    ddlDivision.CssClass = ddlDivision.CssClass.Replace("required", "");
                    ddlOffice.CssClass = ddlOffice.CssClass.Replace("required", "");

                    txtAssetName.Enabled = false;
                    txtEstimatedvalue.Enabled = false;
                    txtQty.Enabled = false;
                    txtUnits.Enabled = false;
                    gvAssetAttribute.Enabled = false;

                    txtAssetName.CssClass = txtAssetName.CssClass.Replace("required", "");
                    txtQty.CssClass = txtQty.CssClass.Replace("required", "");
                    btnSave.Visible = false;
                    // Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                    // Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjAssetItems.DivisionID));

                }
            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion AlreadyAdded

        #region AssetDetail
        private void LoadAssetDetail()
        {
            try
            {
                IEnumerable<DataRow> IeAssetDetail = new AssetsWorkBLL().GetAm_AssetAtrtributeDetail(Convert.ToInt32(hdnAssetID.Value), Convert.ToInt32(ddlSubCategory.SelectedValue));
                var LstItem = IeAssetDetail.Select(dataRow => new
                {
                    //  AssetAttributeID,AttributeID,AttributeName,AttributeDataType,AttributeValue,CreatedBy,CreatedDate
                    AssetAttributeID = dataRow.Field<long?>("AssetAttributeID"),
                    AttributeID = dataRow.Field<long>("AttributeID"),
                    AttributeName = dataRow.Field<string>("AttributeName"),
                    AttributeDataType = dataRow.Field<string>("AttributeDataType"),
                    AttributeValue = dataRow.Field<string>("AttributeValue"),
                    CreatedBy = dataRow.Field<Int32?>("CreatedBy"),
                    CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),


                }).ToList();
                gvAssetAttribute.DataSource = LstItem;
                gvAssetAttribute.DataBind();

                if (gvAssetAttribute.Rows.Count > 0)
                {
                    DivAssetDetail.Visible = true;
                    gvAssetAttribute.Visible = true;
                }
                else
                {
                    DivAssetDetail.Visible = false;
                    gvAssetAttribute.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvAssetAttribute.DataKeys[e.Row.RowIndex];
                    string AttributeValue = Convert.ToString(key.Values["AttributeValue"]);
                    string AttributeDataType = Convert.ToString(key.Values["AttributeDataType"]);
                    #endregion

                    #region "Controls"
                    TextBox txtAttributeValue = (TextBox)e.Row.FindControl("txtAttributeValue");

                    #endregion

                    txtAttributeValue.Text = AttributeValue;
                    if (AttributeDataType == "Text")
                    {
                        txtAttributeValue.CssClass = "form-control";
                        //txtAttributeValue.Attributes.Add("required", "required");
                    }
                    else if (AttributeDataType == "Numeric")
                    {
                        txtAttributeValue.CssClass = "integerInput form-control";
                        //txtAttributeValue.Attributes.Add("required", "required");
                    }
                    else if (AttributeDataType == "Date")
                    {
                        txtAttributeValue.CssClass = "form-control date-picker";
                        //txtAttributeValue.Attributes.Add("required", "required");
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion AssetDetail
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubCategory.SelectedItem.Value == "")
                {
                    gvAssetAttribute.Visible = false;
                    DivAssetDetail.Visible = false;
                }
                else
                {
                    LoadAssetDetail();
                    if (new AssetsWorkBLL().isSubCatgFlood(Convert.ToInt64(ddlSubCategory.SelectedItem.Value)))
                    {
                        rdAssociatFlood.Enabled = false;
                        rdAssociatFlood.SelectedValue = "0";
                    }
                    else
                    {
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.AccountOfficer)
                            rdAssociatFlood.Enabled = false;
                        else
                            rdAssociatFlood.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private AM_AssetItems PrepareAssetItemEntity()
        {

            AM_AssetItems ObjAssetItem = new AM_AssetItems();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnAssetID.Value))
                ObjAssetItem.ID = Convert.ToInt64(hdnAssetID.Value);

            if (ObjAssetItem.ID == 0)
            {
                ObjAssetItem.CreatedDate = DateTime.Now;
                ObjAssetItem.CreatedBy = Convert.ToInt32(mdlUser.ID);

            }
            else
            {
                ObjAssetItem.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                ObjAssetItem.ModifiedDate = DateTime.Now;
                ObjAssetItem.CreatedBy = Convert.ToInt32(hdnCreatedBy.Value);
                ObjAssetItem.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }
            ObjAssetItem.AssetName = txtAssetName.Text.Trim();
            ObjAssetItem.SubCategoryID = Convert.ToInt64(ddlSubCategory.SelectedValue);

            if (txtEstimatedvalue.Text != "")
            {
                ObjAssetItem.EstimatedValue = Convert.ToDouble(txtEstimatedvalue.Text);
            }
            ObjAssetItem.FOBit = (rdAssociatFlood.SelectedItem.Value == "1") ? true : false;// Convert.ToBoolean(rdAssociatFlood.SelectedValue);
            ObjAssetItem.AssetType = rdAssetType.SelectedItem.Text;
            if (rdAssetType.SelectedItem.Text == "Lot")
            {
                if (txtQty.Text != "")
                {
                    ObjAssetItem.LotQuantity = Convert.ToInt32(txtQty.Text);
                }
                if (txtUnits.Text != "")
                {
                    ObjAssetItem.Units = txtUnits.Text.Trim();
                }
            }

            ObjAssetItem.IrrigationLevelID = Convert.ToInt64(ddlLevel.SelectedValue);

            if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Zonal))
            {
                ObjAssetItem.IrrigationBoundryID = Convert.ToInt64(ddlZone.SelectedValue);
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Circle))
            {
                ObjAssetItem.IrrigationBoundryID = Convert.ToInt64(ddlCircle.SelectedValue);
            }
            else if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Divisional))
            {
                ObjAssetItem.IrrigationBoundryID = Convert.ToInt64(ddlDivision.SelectedValue);
            }
            else //if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Office))
            {
                ObjAssetItem.IrrigationBoundryID = Convert.ToInt64(ddlOffice.SelectedValue);
            }
            if (rdAssociatFlood.SelectedValue == "0" && rdAssetType.SelectedItem.Text == "Lot")
            {
                ObjAssetItem.AssetAvailableQuantity = Convert.ToInt32(txtQty.Text);
            }
            else
            {
                ObjAssetItem.AssetAvailableQuantity = 1;
            }
            ObjAssetItem.IsAvailable = true;
            ObjAssetItem.IsActive = true;
            ObjAssetItem.IsAuctioned = false;

            return ObjAssetItem;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsSaveDetail = false;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Office) && ddlOffice.SelectedValue == "")
                {
                    Master.ShowMessage("Office field Required", SiteMaster.MessageType.Error);
                    return;
                }
                if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Zonal) && ddlZone.SelectedValue == "")
                {
                    Master.ShowMessage("Zone field Required", SiteMaster.MessageType.Error);
                    return;
                }
                if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Circle) && ddlCircle.SelectedValue == "")
                {
                    Master.ShowMessage("Circle field Required", SiteMaster.MessageType.Error);
                    return;
                }
                if (ddlLevel.SelectedItem.Text == Convert.ToString(Constants.AssetsLevel.Divisional) && ddlDivision.SelectedValue == "")
                {
                    Master.ShowMessage("Division field Required", SiteMaster.MessageType.Error);
                    return;
                }
                //if (gvAssetAttribute.Rows.Count==0)
                //{
                //    Master.ShowMessage("Asset Attributes Required", SiteMaster.MessageType.Error);
                //    return;
                //}
                if (txtEstimatedvalue.Text != "")
                {
                    if (Convert.ToInt64(txtEstimatedvalue.Text) == 0)
                    {
                        Master.ShowMessage("Estimated value should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                if (txtQty.Text != "")
                {
                    if (Convert.ToInt64(txtQty.Text) == 0)
                    {
                        Master.ShowMessage("Quantity should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                AM_AssetItems AssetItemEntity = PrepareAssetItemEntity();
                if (new AssetsWorkBLL().IsAssetsExist(AssetItemEntity))
                {
                    Master.ShowMessage(Message.AlreadyExistAsset.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //else
                //{
                bool isSaved = new AssetsWorkBLL().SaveAssets(AssetItemEntity);
                if (isSaved)
                {
                    ////////////////////////////////////Asset detail///////////////////////////////

                    for (int m = 0; m < gvAssetAttribute.Rows.Count; m++)
                    {
                        try
                        {

                            TextBox txtAttributeValue = (TextBox)gvAssetAttribute.Rows[m].FindControl("txtAttributeValue");
                            string AssetAttributeID = gvAssetAttribute.DataKeys[m].Values[0].ToString();
                            string AttributeID = gvAssetAttribute.DataKeys[m].Values[1].ToString();
                            string AttributeName = gvAssetAttribute.DataKeys[m].Values[2].ToString();
                            //   string AttributeValue = gvAssetAttribute.DataKeys[m].Values[4].ToString();
                            if (txtAttributeValue.Text != "")
                            {


                                string CreatedBy = gvAssetAttribute.DataKeys[m].Values[5].ToString();
                                string CreatedDate = gvAssetAttribute.DataKeys[m].Values[6].ToString();

                                AM_AssetAttributes mdlAssetAttribute = new AM_AssetAttributes();

                                mdlAssetAttribute.ID = Convert.ToInt64(AssetAttributeID);
                                mdlAssetAttribute.AssetItemID = Convert.ToInt64(AssetItemEntity.ID);
                                mdlAssetAttribute.AttributeID = Convert.ToInt64(AttributeID);
                                mdlAssetAttribute.AttributeValue = txtAttributeValue.Text.Trim();

                                if (mdlAssetAttribute.ID == 0)
                                {
                                    mdlAssetAttribute.CreatedBy = Convert.ToInt32(mdlUser.ID);
                                    mdlAssetAttribute.CreatedDate = DateTime.Now;
                                }
                                else
                                {
                                    mdlAssetAttribute.CreatedBy = Convert.ToInt32(CreatedBy);
                                    mdlAssetAttribute.CreatedDate = Convert.ToDateTime(CreatedDate);
                                    mdlAssetAttribute.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                                    mdlAssetAttribute.ModifiedDate = DateTime.Now;
                                }

                                IsSaveDetail = new AssetsWorkBLL().SaveAssetsAttributeDetail(mdlAssetAttribute);
                            }
                            else
                            {
                                if (Convert.ToInt64(AssetAttributeID) != 0)
                                {
                                    bool IsDeleted = new AssetsWorkBLL().DeleteAssetsAttribute(Convert.ToInt64(AssetAttributeID));
                                }
                            }

                        }
                        catch (Exception exp)
                        {
                            Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                            new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                        }
                    }
                    //////////////////////////////////////////////////////////////////
                    try
                    {
                        if (isSaved == true)
                        {
                            SearchAssets.IsSaved = true;
                            HttpContext.Current.Session.Add("AssetsID", AssetItemEntity.ID);
                            Response.Redirect("SearchAssets.aspx?RestoreState=1", false);
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                    }


                }
                //  }

            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void rdAssociatFlood_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdAssociatFlood.SelectedValue == "1")
                {
                    rdAssetType.SelectedValue = "2";
                    rdAssetType.Enabled = false;
                    div_QtyUnits.Visible = false;
                }
                else
                {
                    rdAssetType.SelectedValue = "1";
                    rdAssetType.Enabled = true;
                    div_QtyUnits.Visible = true;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void rdAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdAssetType.SelectedValue == "1")
                {
                    div_QtyUnits.Visible = true;
                }
                else
                {
                    div_QtyUnits.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }



    }
}