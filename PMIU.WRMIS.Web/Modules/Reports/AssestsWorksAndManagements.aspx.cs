using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class AssestsWorksAndManagements : System.Web.UI.Page
    {
        AssetsWorkBLL AWbll = new AssetsWorkBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    BindDDLlists();
                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
                    }
                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }
        private void LoadAllRegionDDByUser(long _UserID, long? _IrrigationLevelID)
        {
            if (_IrrigationLevelID == null)
                return;

            List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_IrrigationLevelID));

            int all = (int)Constants.DropDownFirstOption.All;
            int noOption = (int)Constants.DropDownFirstOption.NoOption;

            List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
            if (lstDivision.Count > 0) // Division
            {
                Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDivision, lstDivision, lstDivision.Count == 1 ? noOption : all);
            }

            List<CO_Circle> lstCircle = (List<CO_Circle>)lstData.ElementAt(2);
            if (lstCircle.Count > 0) // Circle
            {
                Dropdownlist.BindDropdownlist<List<CO_Circle>>(ddlCircle, lstCircle, lstCircle.Count == 1 ? noOption : all);
            }

            List<CO_Zone> lstZone = (List<CO_Zone>)lstData.ElementAt(3);
            if (lstZone.Count > 0) // Zone
            {
                Dropdownlist.BindDropdownlist<List<CO_Zone>>(ddlZone, lstZone, lstZone.Count == 1 ? noOption : all);
            }
        }

        public void dropdownEnabledDisabled()
        {
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
            }
        }

        #region "Dropdownlists Events
        private void BindDDLlists()
        {
            try
            {

                Dropdownlist.DDLIrrigationLevelForReports(ddlLevel, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLAssetOffice(ddlOffice, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLAssetStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLAssetCategory(ddlCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<object>>(ddlWorkType, AWbll.GetWorkType(), (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<object>>(ddlProgressStatus, AWbll.GetProgressStatus(), (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist(ddlYear, AWbll.GetAllYears(), (int)Constants.DropDownFirstOption.NoOption);
                Dropdownlist.BindDropdownlist(ddlFinancialYear, AWbll.GetAllYears(), (int)Constants.DropDownFirstOption.NoOption);
                Dropdownlist.BindDropdownlist<List<object>>(ddlProgressStatusw, AWbll.GetProgressStatus(), (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<object>>(ddlWorkTypeW, AWbll.GetWorkType(), (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                //Dropdownlist.DDLAssetCategory(ddlCategory, false, (int)Constants.DropDownFirstOption.All);
                DDLEmptyCircleDivisionSubDivision();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void DDLEmptyCircleDivisionSubDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
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
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
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
        #endregion

        #region "Load Reports Secondary Parameters"
        private void SecondaryParametersWorkProgress(bool show)
        {
            SPWorkProgress.Visible = show;
            divlevel.Visible = false;
            ddlZone.Enabled = true;
            ddlCircle.Enabled = true;
            ddlDivision.Enabled = true;
        }
        private void SecondaryParametersAssetsInspection(bool show)
        {
            SPAssetsInspection.Visible = show;
            rbIndividualAndLot.Visible = show;
            DivValueOfAttribute.Visible = false;
            DivWorkonAssets.Visible = false;


        }
        private void SecondaryParametersAssetsDetails(bool show)
        {
            SPAssetsInspection.Visible = show;
            rbnAssetsDetails.Visible = show;
            divlevel.Visible = true;
            rbIndividualAndLot.Visible = false;
            //rbAssetsDetails_SelectedIndexChanged(this, null);
        }
        #endregion

        #region GetSecondaryParameter
        private ReportData GetSecondaryParameterWorkProgress(ReportData rptData)
        {
            string yearID = ddlYear.SelectedItem.Text == string.Empty ? "0" : ddlYear.SelectedItem.Text;
            string WorkTypeID = ddlWorkType.SelectedItem.Value == string.Empty ? "0" : ddlWorkType.SelectedItem.Value;
            string ProgressStatusID = ddlProgressStatus.SelectedItem.Value == string.Empty ? "0" : ddlProgressStatus.SelectedItem.Value;


            ReportParameter reportParameter = new ReportParameter("FinancialYear", yearID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("WorkTypeID", WorkTypeID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ProgressStatusID", ProgressStatusID);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }

        private ReportData GetSecondaryParameterAssetsInspection(ReportData rptData)
        {

            string LevelID = ddlLevel.SelectedItem.Value == string.Empty ? "0" : ddlLevel.SelectedItem.Value;
            //string zoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            //string circleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            //string divisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string OfficeID = ddlOffice.SelectedItem.Value == string.Empty ? "0" : ddlOffice.SelectedItem.Value;
            string StatusID = ddlStatus.SelectedItem.Value == string.Empty ? "0" : ddlStatus.SelectedItem.Value;
            string CategoryID = ddlCategory.SelectedItem.Value == string.Empty ? "0" : ddlCategory.SelectedItem.Value;
            string SubCategoryID = ddlSubCategory.SelectedItem.Value == string.Empty ? "0" : ddlSubCategory.SelectedItem.Value;
            string IndividualORLotID = rdoIndividualLot.SelectedItem.Value == string.Empty ? "0" : rdoIndividualLot.SelectedItem.Value;

            ReportParameter reportParameter = new ReportParameter("LevelID", LevelID);
            rptData.Parameters.Add(reportParameter);
            if (LevelID == "1")
            {
                rptData.Parameters.Find(x=>x.Name=="CircleID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";
            }
            else if (LevelID == "2")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";
            }
            else if (LevelID == "3")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "CircleID").Values[0] = "0";
            }
            else if (LevelID == "4")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "CircleID").Values[0] = "0";                
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";    
            }
                

            //reportParameter = new ReportParameter("zoneID", zoneID);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("circleID", circleID);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("divisionID", divisionID);
            //rptData.Parameters.Add(reportParameter);            

            reportParameter = new ReportParameter("OfficeID", OfficeID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("StatusID", StatusID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("CategoryID", CategoryID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("SubCategoryID", SubCategoryID);
            rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("IndividualORLotID", IndividualORLotID);
            //rptData.Parameters.Add(reportParameter);
            //its ok see other code
            if (Convert.ToInt32(IndividualORLotID) == 1)
            {

                rptData.Name = ReportConstants.rptAMAssetInspectionIndividual;
            }
            if (Convert.ToInt32(IndividualORLotID) == 2)
            {
                rptData.Name = ReportConstants.rptAMAssetInspectionLot;
            }


            return rptData;
        }

        private ReportData GetSecondaryParameterAssetsDetails(ReportData rptData)
        {
            string LevelID = ddlLevel.SelectedItem.Value == string.Empty ? "0" : ddlLevel.SelectedItem.Value;
            //string zoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            //string circleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            //string divisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string OfficeID = ddlOffice.SelectedItem.Value == string.Empty ? "0" : ddlOffice.SelectedItem.Value;
            string StatusID = ddlStatus.SelectedItem.Value == string.Empty ? "0" : ddlStatus.SelectedItem.Value;
            string CategoryID = ddlCategory.SelectedItem.Value == string.Empty ? "0" : ddlCategory.SelectedItem.Value;
            string SubCategoryID = ddlSubCategory.SelectedItem.Value == string.Empty ? "0" : ddlSubCategory.SelectedItem.Value;
            string rbAssetsDetailsID = rbAssetsDetails.SelectedItem.Value == string.Empty ? "0" : rbAssetsDetails.SelectedItem.Value;

            #region Work On Assets
            string AssetNameID = ddlAssetName.SelectedItem.Value == string.Empty ? "0" : ddlAssetName.SelectedItem.Value;
            string FinancialYearID = ddlFinancialYear.SelectedItem.Value == string.Empty ? "0" : ddlFinancialYear.SelectedItem.Value;
            string WorkTypeID = ddlWorkTypeW.SelectedItem.Value == string.Empty ? "0" : ddlWorkTypeW.SelectedItem.Value;
            string ProgressStatusID = ddlProgressStatusw.SelectedItem.Value == string.Empty ? "0" : ddlProgressStatusw.SelectedItem.Value;
            #endregion

            #region Value Of Attribute
            string AttributeID = ddlAttribute.SelectedItem.Value == string.Empty ? "0" : ddlAttribute.SelectedItem.Value;
            string AttributevalueID = ddlAttributeValue.SelectedItem.Value == string.Empty ? "" : ddlAttributeValue.SelectedItem.Text;
            #endregion

            if (LevelID == "1")
            {
                rptData.Parameters.Find(x => x.Name == "CircleID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";
            }
            else if (LevelID == "2")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";
            }
            else if (LevelID == "3")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "CircleID").Values[0] = "0";
            }
            else if (LevelID == "4")
            {
                rptData.Parameters.Find(x => x.Name == "ZoneID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "CircleID").Values[0] = "0";
                rptData.Parameters.Find(x => x.Name == "DivisionID").Values[0] = "0";
            }

            ReportParameter reportParameter = new ReportParameter("LevelID", LevelID);
            rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("ZoneID", zoneID);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("CircleID", circleID);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("DivisionID", divisionID);
            //rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("OfficeID", OfficeID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("StatusID", StatusID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("CategoryID", CategoryID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("SubCategoryID", SubCategoryID);
            rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("IndividualORLotID", rbAssetsDetailsID);
            //rptData.Parameters.Add(reportParameter);
            //its ok see other code
            if (Convert.ToInt32(rbAssetsDetailsID) == 1)
            {
                DivWorkonAssets.Visible = true;
                DivValueOfAttribute.Visible = false;
                reportParameter = new ReportParameter("AssetItemID", AssetNameID);
                rptData.Parameters.Add(reportParameter);
                reportParameter = new ReportParameter("FinancialYearID", FinancialYearID);
                rptData.Parameters.Add(reportParameter);
                reportParameter = new ReportParameter("WorkTypeID", WorkTypeID);
                rptData.Parameters.Add(reportParameter);
                reportParameter = new ReportParameter("ProgressStatusID", ProgressStatusID);
                rptData.Parameters.Add(reportParameter);

                rptData.Name = ReportConstants.rptAMAssetWorksDetail;
            }
            if (Convert.ToInt32(rbAssetsDetailsID) == 2)
            {
                reportParameter = new ReportParameter("AttributeID", AttributeID);
                rptData.Parameters.Add(reportParameter);
                reportParameter = new ReportParameter("AttributeValueID", AttributevalueID);
                rptData.Parameters.Add(reportParameter);

                rptData.Name = ReportConstants.rptAMAssetAttributesDetail;
            }
            return rptData;
        }

        #endregion

        private ReportData GetPrimaryParameter()
        {
            string division = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string zone = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            string circle = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            //string year = ddlYear.SelectedItem.Value == string.Empty ? "0" : ddlYear.SelectedItem.Value;
            //string WorkType = ddlWorkType.SelectedItem.Value == string.Empty ? "0" : ddlWorkType.SelectedItem.Value;
            //string ProgressStatus = ddlProgressStatus.SelectedItem.Value == string.Empty ? "0" : ddlProgressStatus.SelectedItem.Value;

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("DivisionID", division);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ZoneID", zone);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("CircleID", circle);
            rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("Year", year);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("WorkType", WorkType);
            //rptData.Parameters.Add(reportParameter);
            //reportParameter = new ReportParameter("ProgressStatus", ProgressStatus);
            //rptData.Parameters.Add(reportParameter);
            return rptData;

        }
        private ReportData GetReportData(Int32 _ReportID)
        {

            ReportData rptData = GetPrimaryParameter();
            switch (_ReportID)
            {
                case (int)ReportConstants.AssestsAndWorksReports.WorkProgress:
                    rptData = GetSecondaryParameterWorkProgress(rptData);
                    rptData.Name = ReportConstants.rptAMAssetWorkProgress;
                    break;
                case (int)ReportConstants.AssestsAndWorksReports.AssestsInspections:
                    rptData = GetSecondaryParameterAssetsInspection(rptData);
                    break;
                case (int)ReportConstants.AssestsAndWorksReports.AssestsDetails:
                    rptData = GetSecondaryParameterAssetsDetails(rptData);
                    break;
                default:
                    break;
            }
            return rptData;

        }

        private void HideSecondaryParameters()
        {
            SecondaryParametersWorkProgress(false);
            iframestyle.Visible = false;
            divlevel.Visible = false;
        }
        private int GetReportID()
        {

            int reportID = 0;
            if (rbWorkProgress.Checked)
                reportID = (int)ReportConstants.AssestsAndWorksReports.WorkProgress;
            else if (rbAssetsInspections.Checked)
                reportID = (int)ReportConstants.AssestsAndWorksReports.AssestsInspections;
            else if (rbAssetsDetailsID.Checked)
                reportID = (int)ReportConstants.AssestsAndWorksReports.AssestsDetails;
            return reportID;


        }
        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            HideSecondaryParameters();
            iframestyle.Visible = false;
            int reportID = GetReportID();
            switch (reportID)
            {
                case (int)ReportConstants.AssestsAndWorksReports.WorkProgress:
                    SecondaryParametersWorkProgress(true);
                    SecondaryParametersAssetsInspection(false);
                    break;
                case (int)ReportConstants.AssestsAndWorksReports.AssestsInspections:
                    SecondaryParametersAssetsInspection(true);
                    rbnAssetsDetails.Visible = false;
                    divlevel.Visible = true;
                    break;
                case (int)ReportConstants.AssestsAndWorksReports.AssestsDetails:
                    SecondaryParametersAssetsDetails(true);
                    rbAssetsDetails_SelectedIndexChanged(this, null);
                    break;
                default:
                    break;
            }

        }
        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                Session[SessionValues.ReportData] = GetReportData(GetReportID());
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbAssetsDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbAssetsDetails.SelectedItem.Value != "")
                {
                    if (Convert.ToInt32(rbAssetsDetails.SelectedItem.Value) == 1)
                    {
                        DivWorkonAssets.Visible = true;
                        DivValueOfAttribute.Visible = false;
                    }
                    else if (Convert.ToInt32(rbAssetsDetails.SelectedItem.Value) == 2)
                    {
                        DivWorkonAssets.Visible = false;
                        DivValueOfAttribute.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAttribute.SelectedItem.Value != "")
                {
                    ddlAttributeValue.Items.Clear();
                    Dropdownlist.DDLAttributeValueForReports(ddlAttributeValue, Convert.ToInt64(ddlAttribute.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubCategory.SelectedItem.Value != "")
            {                
                long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                long? IrrigationBoundaryID = SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID;
                Dropdownlist.DDLAssetNameForReports(ddlAssetName, Convert.ToInt64(ddlSubCategory.SelectedItem.Value), IrrigationLevelID, IrrigationBoundaryID, (int)Constants.DropDownFirstOption.All);

                ddlAttribute.ClearSelection();
                ddlAttribute.Items.Clear();
                Dropdownlist.DDLAttributeForReports(ddlAttribute, Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (int)Constants.DropDownFirstOption.All);

                ddlAttributeValue.ClearSelection();
                ddlAttributeValue.Items.Clear();
                ddlAttributeValue.Items.Add(new ListItem("All", ""));
            }
        }
    }
}