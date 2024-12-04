using PMIU.WRMIS.BLL.FloodOperations;
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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class AddIndependent : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        protected void Page_Load(object sender, EventArgs e)
        {
            int FloodInspectionID = 0;
            if (!IsPostBack)
            {
                SetPageTitle();
                BindDropdownlists();
                FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                hdnFloodInspectionID.Value = Convert.ToString(FloodInspectionID);
                hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(FloodInspectionID)).ToString();
                txtDate.Attributes.Add("style", "display:visible;");
                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                //   hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", FloodInspectionID);
                bool IsFloodInspectionExists = new FloodOperationsBLL().IsFloodInspectionParentExists(FloodInspectionID);
                if (IsFloodInspectionExists)
                {
                    LoadFloodInspectionByType(FloodInspectionID);
                    if (Convert.ToUInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnSave.Enabled = false;
                        ddlDivision.Enabled = false;
                        ddlInspectionType.Enabled = false;
                        ddlInfrastructureType.Enabled = false;
                        ddlInfrastructureName.Enabled = false;
                        txtDate.ReadOnly = true;
                    }
                }
                if (FloodInspectionID > 0)
                {
                    h3PageTitle.InnerText = "Edit Independent Flood Inspections";
                }
            }
        }

        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLActiveInspectionType(ddlInspectionType);
                BindInspectionTypeDropDown();
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType);
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                //BindUserLocation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInspectionTypeDropDown()
        {
            try
            {
                Int16 InspectionTypeToAdd = 0;

                InspectionTypeToAdd = new FloodOperationsBLL().CanAddFloodInspections();

                //UA_SystemParameters systemParameters = null;
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("PreFloodInspection", "StartDate");// 01-Apr
                //string PreStartDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("PreFloodInspection", "EndDate");// 14-Jun
                //string PreEndDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
                //if (DateTime.Now >= Convert.ToDateTime(PreStartDate) && DateTime.Now <= Convert.ToDateTime(PreEndDate))
                if(InspectionTypeToAdd == 1)
                {
                    //Dropdownlist.DDLPrePostInspectionType(ddlInspectionType, 1, false);
                    Dropdownlist.SetSelectedValue(ddlInspectionType, "1");
                }

                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("PostFloodInspection", "StartDate"); // 16-Oct
                //string PostStartDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("PostFloodInspection", "EndDate");// 31-Dec
                //string PostEndDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
                //if (DateTime.Now >= Convert.ToDateTime(PostStartDate) && DateTime.Now <= Convert.ToDateTime(PostEndDate))
                else if(InspectionTypeToAdd == 2)
                {
                    //Dropdownlist.DDLPrePostInspectionType(ddlInspectionType, 2, false);
                    Dropdownlist.SetSelectedValue(ddlInspectionType, "2");
                }
                else
                {
                    Master.ShowMessage("Can't add inspection on this Date", SiteMaster.MessageType.Error);
                    return;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strYear = DateTime.Parse(txtDate.Text).Year.ToString();
                if (DateTime.Now.Year == Convert.ToInt32(strYear))
                {
                    long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                    long InfrastructureNameSelectedValue = Convert.ToInt64(ddlInfrastructureName.SelectedItem.Value);
                    object bllGetStructureTypeIDByvalue = null;

                    #region FloodInspectionDetail

                    FO_FloodInspectionDetail FloodInspectionDetail = new FO_FloodInspectionDetail();

                    if (InfrastructureTypeSelectedValue == 1)
                    {
                        bllGetStructureTypeIDByvalue =
                            new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1,
                                InfrastructureNameSelectedValue);
                        long ProtectionInfrastructureID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("ProtectionInfrastructureID")
                                    .GetValue(bllGetStructureTypeIDByvalue));
                        long InfrastructureTypeID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("InfrastructureTypeID")
                                    .GetValue(bllGetStructureTypeIDByvalue));
                        //string InfrastructureName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureName").GetValue(bllGetStructureTypeIDByvalue));
                        //string InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

                        FloodInspectionDetail.StructureTypeID = InfrastructureTypeID;
                        FloodInspectionDetail.StructureID = ProtectionInfrastructureID;
                    }
                    else if (InfrastructureTypeSelectedValue == 2)
                    {
                        bllGetStructureTypeIDByvalue =
                            new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2,
                                InfrastructureNameSelectedValue);
                        long StationID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("StationID")
                                    .GetValue(bllGetStructureTypeIDByvalue));
                        long InfrastructureTypeID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("InfrastructureTypeID")
                                    .GetValue(bllGetStructureTypeIDByvalue));

                        FloodInspectionDetail.StructureTypeID = InfrastructureTypeID;
                        FloodInspectionDetail.StructureID = StationID;
                    }
                    else if (InfrastructureTypeSelectedValue == 3)
                    {
                        bllGetStructureTypeIDByvalue =
                            new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3,
                                InfrastructureNameSelectedValue);
                        long DrainID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("DrainID")
                                    .GetValue(bllGetStructureTypeIDByvalue));
                        long InfrastructureTypeID =
                            Convert.ToInt64(
                                bllGetStructureTypeIDByvalue.GetType()
                                    .GetProperty("InfrastructureTypeID")
                                    .GetValue(bllGetStructureTypeIDByvalue));

                        FloodInspectionDetail.StructureTypeID = InfrastructureTypeID;
                        FloodInspectionDetail.StructureID = DrainID;
                    }

                    FloodInspectionDetail.InspectionTypeID = Convert.ToInt16(ddlInspectionType.SelectedItem.Value);

                    #endregion FloodInspectionDetail

                    #region Save FloodInspection

                    FO_FloodInspection FloodInspectionEntity = PrepareFloodInspectionEntity();

                    if (new FloodOperationsBLL().IsFloodInspectionDataAlreadyExists(FloodInspectionEntity,
                        FloodInspectionDetail))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    //if (txtDate.Text != null && DateTime.Now < Convert.ToDateTime(txtDate.Text))
                    //{
                    //    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                    //    return;
                    //}
                    bool isSaved = new FloodOperationsBLL().SaveFloodInspection(FloodInspectionEntity);

                    if (isSaved)
                    {
                        SearchIndependent.IsSaved = true;
                        HttpContext.Current.Session.Add("FloodInspectionID", FloodInspectionEntity.ID);
                        //  Response.Redirect("SearchIndependent.aspx?FloodInspectionID=" + FloodInspectionEntity.ID, false);
                    }

                    #endregion Save FloodInspection

                    #region Save FloodInspectionDetail

                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    FloodInspectionDetail.FloodInspectionID = FloodInspectionEntity.ID;

                    FO_FloodInspectionDetail FloodDetailByID =
                        new FloodOperationsBLL().IsFloodInspectionDetailaAlreadyExists(FloodInspectionEntity.ID);
                    if (FloodDetailByID != null)
                        FloodInspectionDetail.ID = FloodDetailByID.ID;

                    if (FloodInspectionDetail.ID == 0)
                    {
                        FloodInspectionDetail.CreatedDate = DateTime.Now;
                        FloodInspectionDetail.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    }
                    else
                    {
                        FloodInspectionDetail.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                        FloodInspectionDetail.ModifiedDate = DateTime.Now;
                        FloodInspectionDetail.CreatedBy = Convert.ToInt32(mdlUser.ID);
                        FloodInspectionDetail.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    }

                    bool isDetailSaved = new FloodOperationsBLL().SaveFloodInspectionDetail(FloodInspectionDetail);
                    if (isDetailSaved)
                    {
                        SearchIndependent.IsSaved = true;
                        HttpContext.Current.Session.Add("FloodInspectionDetailID", FloodInspectionDetail.ID);
                        Response.Redirect(
                            "SearchIndependent.aspx?FloodInspectionID=" + FloodInspectionDetail.FloodInspectionID, false);
                    }

                    #endregion Save FloodInspectionDetail
                }
                else
                {
                    Master.ShowMessage("Can't add inspection on this Date", SiteMaster.MessageType.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users _Users = SessionManagerFacade.UserInformation;

            if (ddlInfrastructureType.SelectedItem.Value != "")
            {
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
            }
        }

        private void LoadFloodInspectionByType(long _FloodInspectionID)
        {
            try
            {
                string InfrastructureType = string.Empty;
                object lstInfrastructureType = new FloodOperationsBLL().GetInfrastructureTypeByID(_FloodInspectionID);

                if (lstInfrastructureType != null)
                {
                    InfrastructureType = Convert.ToString(lstInfrastructureType.GetType().GetProperty("InfrastructureType").GetValue(lstInfrastructureType));
                    var CreatedDate = Convert.ToString(lstInfrastructureType.GetType().GetProperty("CreatedDate").GetValue(lstInfrastructureType));
                    var InspectionDate = Convert.ToString(lstInfrastructureType.GetType().GetProperty("InspectionDate").GetValue(lstInfrastructureType));
                    hdnCreatedDate.Value = Convert.ToString(CreatedDate);

                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(InspectionDate)));
                }
                LoadFloodInspectionDetail(InfrastructureType, _FloodInspectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadFloodInspectionDetail(string _InfrastructureType, long _FloodInspectionID)
        {
            try
            {
                //  object bllFloodInspection = new FloodOperationsBLL().GetFloodInspectionsByID(_InfrastructureType, _FloodInspectionID);
                UA_Users _Users = SessionManagerFacade.UserInformation;

                FO_GetFloodInspectionsDetailByID_Result2 lstFloodInspectionDetail = new FloodOperationsBLL().GetFloodInspectionsDetail(_InfrastructureType, _FloodInspectionID);

                if (lstFloodInspectionDetail != null)
                {
                    long StructurID = Convert.ToInt64(lstFloodInspectionDetail.StructureID);

                    long InspectionID = Convert.ToInt64(lstFloodInspectionDetail.InspectionID);
                    long StructureTypeID = Convert.ToInt64(lstFloodInspectionDetail.StructureTypeID);
                    string DivisionID = Convert.ToString(lstFloodInspectionDetail.DivisionID);
                    string InspectionDate = Convert.ToString(lstFloodInspectionDetail.InspectionDate);
                    string InfrastructureName = Convert.ToString(lstFloodInspectionDetail.InfrastructureName);
                    string InfrastructureType = Convert.ToString(lstFloodInspectionDetail.InfrastructureType);

                    string inspectionType = Convert.ToString(lstFloodInspectionDetail.InspectionTypeID);

                    if (_InfrastructureType.Equals("Protection Infrastructure"))
                    {
                        Dropdownlist.SetSelectedText(ddlInfrastructureType, "Protection Infrastructure");
                        Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                    }
                    else if (_InfrastructureType.Equals("Control Structure1"))
                    {
                        Dropdownlist.SetSelectedText(ddlInfrastructureType, "Barrage/Headwork");
                        Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                    }
                    else if (_InfrastructureType.Equals("Drain"))
                    {
                        Dropdownlist.SetSelectedText(ddlInfrastructureType, "Drain");
                        Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
                    }
                    Dropdownlist.SetSelectedText(ddlInfrastructureName, InfrastructureName);
                    Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
                    Dropdownlist.SetSelectedValue(ddlInspectionType, inspectionType);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private FO_FloodInspection PrepareFloodInspectionEntity()
        {
            FO_FloodInspection FloodInspection = new FO_FloodInspection();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnFloodInspectionID.Value))
                FloodInspection.ID = Convert.ToInt64(hdnFloodInspectionID.Value);

            if (FloodInspection.ID == 0)
            {
                FloodInspection.CreatedDate = DateTime.Now;
                FloodInspection.CreatedBy = Convert.ToInt32(mdlUser.ID);
            }
            else
            {
                FloodInspection.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                FloodInspection.ModifiedDate = DateTime.Now;
                FloodInspection.CreatedBy = Convert.ToInt32(mdlUser.ID);
                FloodInspection.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            FloodInspection.DivisionID = Convert.ToInt16(ddlDivision.SelectedValue);

            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                FloodInspection.InspectionDate = Convert.ToDateTime(txtDate.Text);
                FloodInspection.Year = Convert.ToDateTime(txtDate.Text).Year.ToString();
            }

            //   FloodInspection.InspectionCategoryID = Convert.ToInt16(ddlInspectionType.SelectedValue);
            FloodInspection.InspectionCategoryID = 1;
            FloodInspection.InspectionStatusID = 1;

            return FloodInspection;
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
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
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
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

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

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
                        Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
            }
            ViewState.Add(UserDivisionKey, lstUserDivision);
        }
    }
}