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
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class AddFFP : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    SetPageTitle();
                    BindDropdownlists();
                    hdnFFPID.Value = Convert.ToString(FFPID);
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/SearchFFP.aspx?FFPID={0}", FFPID);
                    LoadFFPDetail();
                    if (FFPID > 0)
                    {
                        h3PageTitle.InnerText = "Edit Flood Fighting Plan";
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item1 + " - Flood Fighting Plan Basic Information";
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdownlists()
        {
            try
            {
                // BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

                // Bind year dropdownlist

                Dropdownlist.DDLYear(ddlYear, false, 0, 2011, (int)Constants.DropDownFirstOption.Select);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == String.Empty)
                {
                    ddlYear.SelectedIndex = 0;
                    //ddlYear.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool CanAddFFP = false;

                if (DateTime.Now.Year == Convert.ToInt32(ddlYear.SelectedValue) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    CanAddFFP = new FloodFightingPlanBLL().CanAddFFP(Convert.ToInt32(ddlYear.SelectedValue), SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddFFP)
                    {
                        FO_FloodFightingPlan FloodFightingEntity = PrepareFloodFightingPlanEntity();
                        if (new FloodFightingPlanBLL().IsFightingPlanAlreadyExists(FloodFightingEntity))
                        {
                            Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        bool isSaved = new FloodFightingPlanBLL().SaveFloodFightingPlan(FloodFightingEntity);
                        if (isSaved)
                        {
                            SearchFFP.IsSaved = true;
                            HttpContext.Current.Session.Add("FFPID", FloodFightingEntity.ID);
                            Response.Redirect("SearchFFP.aspx?FFPID=" + FloodFightingEntity.ID, false);
                        }
                    }
                    else
                    {
                        Master.ShowMessage("Can not add out of date FFP", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else
                {
                    Master.ShowMessage("Can not add out of date FFP", SiteMaster.MessageType.Error);
                    return;
                }
                //try
                //{
                //if (new FloodFightingPlanBLL().IsFightingPlanAlreadyExists(FloodFightingEntity))
                //{
                //    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                //    return;
                //}

                //bool isSaved = new FloodFightingPlanBLL().SaveFloodFightingPlan(FloodFightingEntity);
                //if (isSaved)
                //{
                //    SearchFFP.IsSaved = true;
                //    HttpContext.Current.Session.Add("FFPID", FloodFightingEntity.ID);
                //    Response.Redirect("SearchFFP.aspx?FFPID=" + FloodFightingEntity.ID, false);
                //}
                //}
                //catch (Exception ex)
                //{
                //    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                //    //lblMessage.Text = ex.Message;
                //}
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private FO_FloodFightingPlan PrepareFloodFightingPlanEntity()
        {
            FO_FloodFightingPlan mdlFloodFightingPlan = new FO_FloodFightingPlan();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnFFPID.Value))
                mdlFloodFightingPlan.ID = Convert.ToInt64(hdnFFPID.Value);

            if (mdlFloodFightingPlan.ID == 0)
            {
                mdlFloodFightingPlan.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlFloodFightingPlan.CreatedDate = DateTime.Now;
            }
            else
            {
                mdlFloodFightingPlan.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlFloodFightingPlan.CreatedDate = DateTime.Now;
                mdlFloodFightingPlan.ModifiedDate = DateTime.Now;
                mdlFloodFightingPlan.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            mdlFloodFightingPlan.DivisionID = Convert.ToInt64(ddlDivision.SelectedValue);
            mdlFloodFightingPlan.Year = Convert.ToInt32(ddlYear.SelectedValue);
            mdlFloodFightingPlan.Status = "Draft";

            return mdlFloodFightingPlan;
        }

        private void LoadFFPDetail()
        {
            FO_FloodFightingPlan ObjDS = new FloodFightingPlanBLL().GetFFPID(Convert.ToInt64(hdnFFPID.Value));
            if (ObjDS != null)
            {
                Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjDS.DivisionID));
                Dropdownlist.SetSelectedValue(ddlYear, Convert.ToString(ObjDS.Year));
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