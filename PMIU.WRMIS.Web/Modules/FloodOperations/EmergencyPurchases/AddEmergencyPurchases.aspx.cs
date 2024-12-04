using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Web.Common.Controls;



namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class AddEmergencyPurchases : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int EmergencyPurchasesID = 0;
            if (!IsPostBack)
            {
                SetPageTitle();
                BindDropdownlists();
                EmergencyPurchasesID = Utility.GetNumericValueFromQueryString("EmergencyPurchaseID", 0);
                hdnEmergencyPurchasesID.Value = Convert.ToString(EmergencyPurchasesID);
                txtCurrentyear.Text = DateTime.Now.Year.ToString();
                hlBack.NavigateUrl =
                    string.Format(
                        "~/Modules/FloodOperations/EmergencyPurchases/SearchEmergencyPurchases.aspx?EmergencyPurchaseID={0}",
                        EmergencyPurchasesID);
                LoadEmergencyPurchaseByID(EmergencyPurchasesID);
                if (EmergencyPurchasesID > 0)
                {
                    h3PageTitle.InnerText = "Edit Emergency Purchases";
                }
            }





        }

        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLYesNo(ddlCampSite);
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType);
                //BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID,
                    (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID,
                    (int)Constants.DropDownFirstOption.Select);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CanAddEditEP = false;
            try
            {
                if (DateTime.Now.Year == Convert.ToInt32(txtCurrentyear.Text))
                {
                    CanAddEditEP =
                        new FloodOperationsBLL().CanAddEditEmergencyPurchase(Convert.ToInt32(txtCurrentyear.Text),
                            SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        FO_EmergencyPurchase EmergencyPurchaseEntity = PrepareEmergencyPurchaseEntity();
                        //if (txtRDLeft.Text != "" && txtRDRight.Text != "")
                        //{
                        //    if (hdnStructureType.Value != "Control Structure1")
                        //    {
                        //        Tuple<string, string> tupleTotalRD = Calculations.GetRDValues(Convert.ToInt64(hdnTotalRD.Value));
                        //        if (!(Convert.ToInt64(tupleTotalRD.Item1) <= EmergencyPurchaseEntity.RD && Convert.ToInt64(tupleTotalRD.Item2) >= EmergencyPurchaseEntity.RD))
                        //        {
                        //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                        //            return;
                        //        }

                        //    }
                        //}

                        if (Convert.ToInt64(hdnEmergencyPurchasesID.Value) != 0 &&
                            Convert.ToInt64(hdnCurrentTotalRD.Value) > 0 &&
                            EmergencyPurchaseEntity.RD != Convert.ToInt64(hdnCurrentTotalRD.Value))
                        {
                            Master.ShowMessage("Total RD can not be changed.", SiteMaster.MessageType.Error);
                        }
                        else if (new FloodOperationsBLL().IsEmergencyPurchasesExist(EmergencyPurchaseEntity))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        }
                        else
                        {
                            bool isSaved = new FloodOperationsBLL().SaveEmergencyPurchases(EmergencyPurchaseEntity);
                            if (isSaved)
                            {
                                SearchEmergencyPurchases.IsSaved = true;
                                HttpContext.Current.Session.Add("EmergencyPurchaseID", EmergencyPurchaseEntity.ID);
                                Response.Redirect(
                                    "SearchEmergencyPurchases.aspx?EmergencyPurchaseID=" + EmergencyPurchaseEntity.ID,
                                    false);
                            }
                        }
                    }
                    else
                    {
                        Master.ShowMessage("Can not added Emergency Purchase at this Month",
                            SiteMaster.MessageType.Error);
                        return;
                    }

                }
                else
                {
                    Master.ShowMessage("Can not added Emergency Purchase of Privious Year", SiteMaster.MessageType.Error);
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
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                    Dropdownlist.DDLYesNo(ddlCampSite);
                    txtRDLeft.Enabled = true;
                    txtRDRight.Enabled = true;
                }
                else if (InfrastructureTypeSelectedValue == 2)
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                    Dropdownlist.DDLYesNo(ddlCampSite);
                    txtRDLeft.Enabled = false;
                    txtRDRight.Enabled = false;
                }
                else if (InfrastructureTypeSelectedValue == 3)
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
                    Dropdownlist.DDLYesNo(ddlCampSite);
                    txtRDLeft.Enabled = true;
                    txtRDRight.Enabled = true;
                }
            }
        }

        private void LoadEmergencyPurchaseByID(long _EmergencyPurchaseID)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                string InfrastructureType = string.Empty;
                string InfrastructureName = string.Empty;
                object bllGetStructureTypeIDByvalue = null;
                object lstInfrastructureType =
                    new FloodOperationsBLL().GetInfrastructureTypeByEmergencyPurchaseID(_EmergencyPurchaseID);
                FO_EmergencyPurchase ObjEmergencyPurchase =
                    new FloodOperationsBLL().GetEmergencyPurchaseByID(_EmergencyPurchaseID);
                if (ObjEmergencyPurchase != null)
                {
                    Tuple<string, string> tuple = Calculations.GetRDValues(Convert.ToInt64(ObjEmergencyPurchase.RD));
                    txtRDLeft.Text = tuple.Item1;
                    txtRDRight.Text = tuple.Item2;
                    hdnCurrentTotalRD.Value = Convert.ToString(ObjEmergencyPurchase.RD);
                    txtCurrentyear.Text = Convert.ToString(ObjEmergencyPurchase.Year);
                    Dropdownlist.DDLYesNo(ddlCampSite);
                    string Compvalue = Convert.ToString(ObjEmergencyPurchase.IsCampSite) == true.ToString() ? "1" : "0";
                    long selctedInfrastructureName = Convert.ToInt64(ObjEmergencyPurchase.StructureID);
                    Dropdownlist.SetSelectedValue(ddlCampSite, Compvalue);
                    Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID,
                        (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID,
                        (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(ObjEmergencyPurchase.DivisionID));

                    if (lstInfrastructureType != null)
                    {
                        InfrastructureType =
                            Convert.ToString(
                                lstInfrastructureType.GetType()
                                    .GetProperty("InfrastructureType")
                                    .GetValue(lstInfrastructureType));
                        var CreatedDate =
                            Convert.ToString(
                                lstInfrastructureType.GetType()
                                    .GetProperty("CreatedDate")
                                    .GetValue(lstInfrastructureType));
                        hdnCreatedDate.Value = Convert.ToString(CreatedDate);
                        if (InfrastructureType.Equals("Protection Infrastructure"))
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructureType, "Protection Infrastructure");
                            bllGetStructureTypeIDByvalue =
                                new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1,
                                    selctedInfrastructureName);
                            InfrastructureName =
                                Convert.ToString(
                                    bllGetStructureTypeIDByvalue.GetType()
                                        .GetProperty("InfrastructureName")
                                        .GetValue(bllGetStructureTypeIDByvalue));
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1);
                            Dropdownlist.SetSelectedText(ddlInfrastructureName, InfrastructureName);

                        }
                        else if (InfrastructureType.Equals("Control Structure1"))
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructureType, "Barrage/Headwork");
                            bllGetStructureTypeIDByvalue =
                                new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2,
                                    selctedInfrastructureName);
                            InfrastructureName =
                                Convert.ToString(
                                    bllGetStructureTypeIDByvalue.GetType()
                                        .GetProperty("InfrastructureName")
                                        .GetValue(bllGetStructureTypeIDByvalue));
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2);
                            Dropdownlist.SetSelectedText(ddlInfrastructureName, InfrastructureName);

                        }
                        else if (InfrastructureType.Equals("Drain"))
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructureType, "Drain");
                            bllGetStructureTypeIDByvalue =
                                new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3,
                                    selctedInfrastructureName);
                            InfrastructureName =
                                Convert.ToString(
                                    bllGetStructureTypeIDByvalue.GetType()
                                        .GetProperty("InfrastructureName")
                                        .GetValue(bllGetStructureTypeIDByvalue));
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3);
                            Dropdownlist.SetSelectedText(ddlInfrastructureName, InfrastructureName);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private FO_EmergencyPurchase PrepareEmergencyPurchaseEntity()
        {
            hdnTotalRD.Value = "0";
            hdnStructureType.Value = "0";
            long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
            long InfrastructureNameSelectedValue = Convert.ToInt64(ddlInfrastructureName.SelectedItem.Value);
            object bllGetStructureTypeIDByvalue = null;

            FO_EmergencyPurchase ObjEmergencyPurchase = new FO_EmergencyPurchase();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (!string.IsNullOrEmpty(hdnEmergencyPurchasesID.Value))
                ObjEmergencyPurchase.ID = Convert.ToInt64(hdnEmergencyPurchasesID.Value);

            if (ObjEmergencyPurchase.ID == 0)
            {
                ObjEmergencyPurchase.CreatedDate = DateTime.Now;
                ObjEmergencyPurchase.CreatedBy = Convert.ToInt32(mdlUser.ID);

            }
            else
            {
                ObjEmergencyPurchase.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                ObjEmergencyPurchase.ModifiedDate = DateTime.Now;
                ObjEmergencyPurchase.CreatedBy = Convert.ToInt32(mdlUser.ID);
                ObjEmergencyPurchase.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            ObjEmergencyPurchase.DivisionID = Convert.ToInt16(ddlDivision.SelectedValue);
            ObjEmergencyPurchase.Year = Convert.ToInt16(txtCurrentyear.Text);
            // Convert.ToInt16(DateTime.Now.Year.ToString());

            //ObjEmergencyPurchase.Year=Convert.ToInt16(ddl)
            if (InfrastructureTypeSelectedValue == 1)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1,
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
                hdnTotalRD.Value =
                    Convert.ToString(
                        bllGetStructureTypeIDByvalue.GetType()
                            .GetProperty("TotalLength")
                            .GetValue(bllGetStructureTypeIDByvalue));

                ObjEmergencyPurchase.StructureTypeID = InfrastructureTypeID;
                ObjEmergencyPurchase.StructureID = ProtectionInfrastructureID;
            }
            else if (InfrastructureTypeSelectedValue == 2)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2,
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
                hdnStructureType.Value = "Control Structure1";

                ObjEmergencyPurchase.StructureTypeID = InfrastructureTypeID;
                ObjEmergencyPurchase.StructureID = StationID;
            }
            else if (InfrastructureTypeSelectedValue == 3)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3,
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
                hdnTotalRD.Value =
                    Convert.ToString(
                        bllGetStructureTypeIDByvalue.GetType()
                            .GetProperty("TotalLength")
                            .GetValue(bllGetStructureTypeIDByvalue));

                ObjEmergencyPurchase.StructureTypeID = InfrastructureTypeID;
                ObjEmergencyPurchase.StructureID = DrainID;
            }

            //ObjEmergencyPurchase.StructureTypeID = Convert.ToInt64(ddlInfrastructureType.SelectedValue);
            //ObjEmergencyPurchase.StructureID = Convert.ToInt64(ddlInfrastructureName.SelectedValue);
            if (ddlCampSite.SelectedItem.Value != string.Empty)
            {
                ObjEmergencyPurchase.IsCampSite = Convert.ToBoolean(Convert.ToInt16(ddlCampSite.SelectedItem.Value));
            }
            if (txtRDLeft.Text != "" && txtRDRight.Text != "")
            {
                ObjEmergencyPurchase.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);
            }




            return ObjEmergencyPurchase;
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
                    List<UA_AssociatedLocation> lstAssociatedLocation =
                        new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision =
                                    new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserCircle.Add((long)mdlDivision.CircleID);
                                lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID,
                                lstUserDivision);

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

        protected void ddlCampSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCampSite.SelectedItem.Value != "")
                {
                    if ((ddlCampSite.SelectedItem.Text == "Yes" || ddlCampSite.SelectedItem.Text == "No") && Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value) != 2)
                    {
                        txtRDLeft.Enabled = true;
                        txtRDRight.Enabled = true;
                    }
                    else
                    {
                        txtRDLeft.Enabled = false;
                        txtRDRight.Enabled = false;

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