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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental
{
    public partial class AddDepartmental : BasePage
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
                long _FloodInspectionID = 0;
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    _FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    hdnFloodInspectionID.Value = Convert.ToString(_FloodInspectionID);
                    hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(_FloodInspectionID)).ToString();
                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?FloodInspectionID={0}", _FloodInspectionID);

                    bool IsFloodInspectionExists = new FloodOperationsBLL().IsFloodInspectionParentExists(_FloodInspectionID);
                    if (IsFloodInspectionExists)
                    {
                        LoadFloodInspectionByType(_FloodInspectionID);
                    }
                    if (_FloodInspectionID > 0)
                    {
                        h3PageTitle.InnerText = "Edit Departmental Flood Inspections";
                    }
                }
            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #region Function
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdownlists()
        {
            try
            {
                // BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadFloodInspectionByType(long _FloodInspectionID)
        {
            try
            {
                string InfrastructureType = string.Empty;
                object lstDepartmental = new FloodOperationsBLL().GetDepartmentalByID(_FloodInspectionID);

                string DivisionID = Convert.ToString(lstDepartmental.GetType().GetProperty("DivisionID").GetValue(lstDepartmental));
                var InspectionDate = Convert.ToString(lstDepartmental.GetType().GetProperty("InspectionDate").GetValue(lstDepartmental));
                var CreatedDate = Convert.ToString(lstDepartmental.GetType().GetProperty("CreatedDate").GetValue(lstDepartmental));
                hdnCreatedDate.Value = Convert.ToString(CreatedDate);

                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(InspectionDate)));
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private FO_FloodInspection PrepareFloodDepartmentalEntity()
        {

            FO_FloodInspection floodInspection = new FO_FloodInspection();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnFloodInspectionID.Value))
                floodInspection.ID = Convert.ToInt64(hdnFloodInspectionID.Value);

            if (floodInspection.ID == 0)
            {
                floodInspection.CreatedDate = DateTime.Now;
                floodInspection.CreatedBy = Convert.ToInt32(mdlUser.ID);
            }
            else
            {
                floodInspection.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                floodInspection.ModifiedDate = DateTime.Now;
                floodInspection.CreatedBy = Convert.ToInt32(mdlUser.ID);
                floodInspection.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            floodInspection.DivisionID = Convert.ToInt16(ddlDivision.SelectedValue);
            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                floodInspection.InspectionDate = Convert.ToDateTime(txtDate.Text);
                floodInspection.Year = Convert.ToDateTime(txtDate.Text).Year.ToString();
            }

            floodInspection.InspectionCategoryID = 2;
            floodInspection.InspectionStatusID = 1;

            return floodInspection;
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

                            #endregion
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
        #endregion

        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // object bllGetStructureTypeIDByvalue = null;
                #region Save FloodInspection
                string strYear = DateTime.Parse(txtDate.Text).Year.ToString();
                if (DateTime.Now.Year == Convert.ToInt32(strYear))
                {
                    FO_FloodInspection floodInspectionEntity = PrepareFloodDepartmentalEntity();
                    if (txtDate.Text != null && DateTime.Now < Convert.ToDateTime(txtDate.Text))
                    {
                        Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    bool isSaved = new FloodOperationsBLL().SaveFloodInspection(floodInspectionEntity);
                    if (isSaved)
                    {
                        if (Convert.ToInt64(hdnFloodInspectionID.Value) > 0)
                        {
                            Master.ShowMessage("Record update successfully.", SiteMaster.MessageType.Error);
                        }
                        else
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Error);
                        }

                        SearchDepartmental.IsSaved = true;
                        HttpContext.Current.Session.Add("FloodInspectionID", floodInspectionEntity.ID);
                        Response.Redirect("SearchDepartmental.aspx?FloodInspectionID=" + floodInspectionEntity.ID, false);
                    }
                }
                else
                {
                    Master.ShowMessage("Currrent Year inspection can only be added", SiteMaster.MessageType.Error);
                    return;
                }

                #endregion Save FloodInspection
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}