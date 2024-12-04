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
using System.Web.UI;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary
{
    public partial class AddDivisionSummary : BasePage
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
                    long DivisionSummaryID = Utility.GetNumericValueFromQueryString("DivisionSummaryID", 0);
                    SetPageTitle();
                    BindDropdownlists();
                    hdnDivisionSummaryID.Value = Convert.ToString(DivisionSummaryID);
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionSummary/SearchDivisionSummary.aspx?DivisionSummaryID={0}", DivisionSummaryID);
                    LoadDivisionSummayDetail();
                    if (DivisionSummaryID > 0)
                    {
                        h3PageTitle.InnerText = "Edit Division Summary Basic Information";
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item1 + " - Division Summary Basic Information";
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdownlists()
        {
            try
            {
                //BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

                // Bind year dropdownlist

                Dropdownlist.DDLYear(ddlYear, false, 0, 2011, (int)Constants.DropDownFirstOption.Select);
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
                if (DateTime.Now.Year == Convert.ToInt32(ddlYear.SelectedValue))
                {
                    FO_DivisionSummary divisionSummaryEntity = PrepareDivisionSummaryEntity();
                    if (new FloodOperationsBLL().IsDivisionSummaryAlreadyExists(divisionSummaryEntity))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    bool isSaved = new FloodOperationsBLL().SaveDivisionSummary(divisionSummaryEntity);
                    if (isSaved)
                    {
                        SearchDivisionSummary.IsSaved = true;
                        HttpContext.Current.Session.Add("DivisionSummaryID", divisionSummaryEntity.ID);
                        Response.Redirect("SearchDivisionSummary.aspx?DivisionSummaryID=" + divisionSummaryEntity.ID,
                            false);
                    }
                }
                else
                {
                    Master.ShowMessage("Current Year Division Summary can only be added", SiteMaster.MessageType.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                //lblMessage.Text = ex.Message;
            }
        }

        private FO_DivisionSummary PrepareDivisionSummaryEntity()
        {
            FO_DivisionSummary divisionSummary = new FO_DivisionSummary();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnDivisionSummaryID.Value))
                divisionSummary.ID = Convert.ToInt64(hdnDivisionSummaryID.Value);

            if (divisionSummary.ID == 0)
            {
                divisionSummary.CreatedDate = DateTime.Now;
                divisionSummary.CreatedBy = Convert.ToInt32(mdlUser.ID);
            }
            else
            {
                divisionSummary.CreatedBy = Convert.ToInt32(mdlUser.ID);
                divisionSummary.CreatedDate = DateTime.Now;
                divisionSummary.ModifiedDate = DateTime.Now;
                divisionSummary.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            divisionSummary.DivisionID = Convert.ToInt16(ddlDivision.SelectedValue);
            divisionSummary.Year = Convert.ToInt16(ddlYear.SelectedValue);
            divisionSummary.Status = "Draft";

            return divisionSummary;
        }

        private void LoadDivisionSummayDetail()
        {
            FO_DivisionSummary ObjDS = new FloodOperationsBLL().GetDivisionSummaryID(Convert.ToInt64(hdnDivisionSummaryID.Value));
            if (ObjDS != null)
            {
                Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjDS.DivisionID));
                Dropdownlist.SetSelectedValue(ddlYear, Convert.ToString(ObjDS.Year));
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