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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint
{
    public partial class AddJoint : BasePage
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
                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/joint/SearchJoint.aspx?FloodInspectionID={0}", FloodInspectionID);
                bool IsFloodInspectionExists = new FloodOperationsBLL().IsFloodInspectionParentExists(FloodInspectionID);
                if (IsFloodInspectionExists)
                {
                    LoadFloodInspectionDetail(FloodInspectionID);
                    if (Convert.ToUInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnSave.Enabled = false;
                    }
                }
                if (FloodInspectionID > 0)
                {
                    h3PageTitle.InnerText = "Edit Joint Flood Inspections";
                }
            }
        }

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
                //BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
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

            FloodInspection.InspectionCategoryID = 3;
            FloodInspection.InspectionStatusID = 1;

            return FloodInspection;
        }

        private void LoadFloodInspectionDetail(long _FloodInspectionID)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                FO_FloodInspection lstFloodInspectionDetail = new FloodInspectionsBLL().GetInspectionDetailsByID(Convert.ToInt64(hdnFloodInspectionID.Value));
                if (lstFloodInspectionDetail != null)
                {
                    string DivisionID = Convert.ToString(lstFloodInspectionDetail.DivisionID);
                    Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
                    hdnCreatedDate.Value = Convert.ToString(lstFloodInspectionDetail.CreatedDate);
                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(lstFloodInspectionDetail.InspectionDate)));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Save FloodInspection
                string strYear = DateTime.Parse(txtDate.Text).Year.ToString();
                if (DateTime.Now.Year == Convert.ToInt32(strYear))
                {
                    FO_FloodInspection FloodInspectionEntity = PrepareFloodInspectionEntity();

                    if (txtDate.Text != null && DateTime.Now < Convert.ToDateTime(txtDate.Text))
                    {
                        Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    bool isSaved = new FloodOperationsBLL().SaveFloodInspection(FloodInspectionEntity);

                    if (isSaved)
                    {
                        SearchJoint.IsSaved = true;
                        HttpContext.Current.Session.Add("FloodInspectionID", FloodInspectionEntity.ID);
                        Response.Redirect("SearchJoint.aspx?FloodInspectionID=" + FloodInspectionEntity.ID, false);
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