using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class ViewWorkTenderDetails : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys
        List<object> ControlsValues = new List<object>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                long TenderNoticeID = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                        TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                    }
                    // hdnF_ID.Value = Request.QueryString["CWID"];
                    BindDropdown();
                    string ID = Request.QueryString["CWID"];
                    hdnWordSourceID.Value = ID;
                    hdnTenderNoticeID.Value = Convert.ToString(TenderNoticeID);
                    LaodClosureWorkData(Convert.ToInt64(Request.QueryString["CWID"]), TenderNoticeID);
                    bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                    //    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    //if (IsAwarded == true)
                    //{
                    //    btnAddOpOffice.Visible = false;
                    //}


                }
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewWorkTenderDetails);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LaodClosureWorkData(long _WorkSourceID, long _TenderNoticeID)
        {
            dynamic lstData = new TenderManagementBLL().GetWorktypeIDandSourceByWorkID(_WorkSourceID);

            string Source = Utility.GetDynamicPropertyValue(lstData, "Source");
            string WorkTypeID = Utility.GetDynamicPropertyValue(lstData, "WorkTypeID");
            string Office = Utility.GetDynamicPropertyValue(lstData, "Office");
            string OfficeID = Utility.GetDynamicPropertyValue(lstData, "OfficeID");
            long OfficeLocationID = Convert.ToInt64(OfficeID);
            long ID = Convert.ToInt64(WorkTypeID);
            lblOfficeLocated.Text = Office;

            if (Office == "Zone")
            {
                dynamic OfficeLocation = new TenderManagementBLL().GetOfficeLocationByZoneID(OfficeLocationID, Convert.ToInt64(hdnWordSourceID.Value));
                if (OfficeLocation != null)
                {
                    string ZoneOfficeName = Utility.GetDynamicPropertyValue(OfficeLocation, "ZoneOfficeName");
                    lblOfficeAddress.Text = ZoneOfficeName;
                }

            }
            else if (Office == "Circle")
            {
                dynamic OfficeLocation = new TenderManagementBLL().GetOfficeLocationByCircleID(OfficeLocationID, Convert.ToInt64(hdnWordSourceID.Value));
                if (OfficeLocation != null)
                {
                    string CircleOfficeName = Utility.GetDynamicPropertyValue(OfficeLocation, "CircleOfficeName");
                    lblOfficeAddress.Text = CircleOfficeName;
                }

            }
            else if (Office == "Division")
            {
                dynamic OfficeLocation = new TenderManagementBLL().GetOfficeLocationByDivisionID(OfficeLocationID, Convert.ToInt64(hdnWordSourceID.Value));
                if (OfficeLocation != null)
                {
                    string DivisionOfficeName = Utility.GetDynamicPropertyValue(OfficeLocation, "DivisionOfficeName");
                    lblOfficeAddress.Text = DivisionOfficeName;
                }

            }
            else
            {
                dynamic OfficeLocation = new TenderManagementBLL().GetOfficeLocationByOtherID(OfficeLocationID, Convert.ToInt64(hdnWordSourceID.Value));
                if (OfficeLocation != null)
                {
                    string OtherOfficeName = Utility.GetDynamicPropertyValue(OfficeLocation, "OtherOfficeName");
                    lblOfficeAddress.Text = OtherOfficeName;
                }
            }


            if (Source == "CLOSURE")
            {

                if (ID == (long)Constants.CWWorkType.Desilting)
                {
                    Desilting.Visible = true;
                    dynamic lstDesiltingInfo = new TenderManagementBLL().GetDesiltingByWorkID(_WorkSourceID);

                    string TenderNotice = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "WorkName");

                    string ChannelName = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "ChannelName");
                    string Structure = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "HeadWork");
                    if (ChannelName != null)
                        lblDesiltingChannel.Text = ChannelName;
                    if (Structure != null)
                        lblStructure.Text = Structure;

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(Utility.GetDynamicPropertyValue(lstDesiltingInfo, "CompletionPeriodFlag"));
                    }

                    string StartDate = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "Description");
                    if (Description == "")
                    {
                        Description = "N/A";
                    }

                    string FromRD = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "FromRD");
                    string ToRD = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "ToRD");
                    string SiltRemoved = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "SiltRemoved");
                    string DistrictName = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "DistrictName");
                    string TehsilName = Utility.GetDynamicPropertyValue(lstDesiltingInfo, "TehsilName");


                    lblDesiltingFromRD.Text = FromRD;
                    lblDesiltingToRD.Text = ToRD;
                    lblCilt.Text = SiltRemoved;
                    lblDesiltingDistrict.Text = DistrictName;
                    lblDesiltingTehsil.Text = TehsilName;


                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }
                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                }
                else if (ID == (long)Constants.CWWorkType.ElectricalMechanical)
                {
                    div_EM.Visible = true;
                    //Electrical/Mechanical Information
                    dynamic lstElecMechInfo = new TenderManagementBLL().GetElecMechInfoByWorkID(_WorkSourceID);

                    string TenderNotice = Utility.GetDynamicPropertyValue(lstElecMechInfo, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(lstElecMechInfo, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(lstElecMechInfo, "WorkName");

                    string ChannelName = Utility.GetDynamicPropertyValue(lstElecMechInfo, "ChannelName");
                    string Structure = Utility.GetDynamicPropertyValue(lstElecMechInfo, "HeadWork");
                    if (ChannelName != null)
                        lblEMChannel.Text = ChannelName;
                    if (Structure != null)
                        lblStructure.Text = Structure;

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(lstElecMechInfo, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(lstElecMechInfo, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(lstElecMechInfo, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(lstElecMechInfo, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(lstElecMechInfo, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }
                    string StartDate = Utility.GetDynamicPropertyValue(lstElecMechInfo, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(lstElecMechInfo, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(lstElecMechInfo, "SanctionNo");


                    string SanctionDate = Utility.GetDynamicPropertyValue(lstElecMechInfo, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(lstElecMechInfo, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(lstElecMechInfo, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(lstElecMechInfo, "Description");

                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                }
                else if (ID == (long)Constants.CWWorkType.BuildingWorks)
                {
                    div_BW.Visible = true;
                    lblBWType.Text = "";
                    //Building Works
                    dynamic lstBuildingWorksInfo = new TenderManagementBLL().GetBuildingWorksInfoByWorkID(_WorkSourceID);


                    string TenderNotice = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }
                    string StartDate = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));

                    string EndDate = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "Description");

                    string BWOffice = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "BWOffice");
                    string BWResidence = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "BWResidence");
                    string BWRestHouse = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "BWRestHouse");
                    string BWGRHut = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "BWGRHut");
                    string BWOthers = Utility.GetDynamicPropertyValue(lstBuildingWorksInfo, "BWOthers");


                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                    if (BWOffice == "True")
                        lblBWType.Text = "Offices,";
                    if (BWResidence == "True")
                        lblBWType.Text += "Residences,";
                    if (BWRestHouse == "True")
                        lblBWType.Text += "Rest House,";
                    if (BWGRHut == "True")
                        lblBWType.Text += "Gauge Reader Hut,";
                    if (BWOthers == "True")
                        lblBWType.Text += "Others";
                }
                else if (ID == (long)Constants.CWWorkType.OilingGreasingPainting)
                {
                    div_OGP.Visible = true;
                    lblOGPType.Text = "";
                    dynamic OilGrePaintingData = new TenderManagementBLL().GetOilGrePaintingInfoByWorkID(_WorkSourceID);

                    string TenderNotice = Utility.GetDynamicPropertyValue(OilGrePaintingData, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(OilGrePaintingData, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(OilGrePaintingData, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(OilGrePaintingData, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(OilGrePaintingData, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(OilGrePaintingData, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(OilGrePaintingData, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(OilGrePaintingData, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }
                    string StartDate = Utility.GetDynamicPropertyValue(OilGrePaintingData, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(OilGrePaintingData, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(OilGrePaintingData, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(OilGrePaintingData, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(OilGrePaintingData, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(OilGrePaintingData, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(OilGrePaintingData, "Description");

                    string GaugePainting = Utility.GetDynamicPropertyValue(OilGrePaintingData, "GaugePainting");
                    string GaugeFixing = Utility.GetDynamicPropertyValue(OilGrePaintingData, "GaugeFixing");
                    string OilGreasePaint = Utility.GetDynamicPropertyValue(OilGrePaintingData, "OilGreasePaint");
                    string OPothers = Utility.GetDynamicPropertyValue(OilGrePaintingData, "OPothers");
                    string SubDivisionName = Utility.GetDynamicPropertyValue(OilGrePaintingData, "SubDivisionName");
                    string SectionName = Utility.GetDynamicPropertyValue(OilGrePaintingData, "SectionName");

                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = EstimatedCost;
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                    //     lblOGPType.Text = GaugePainting + "," + GaugeFixing + "," + OilGreasePaint + "," + OPothers;


                    if (GaugePainting == "True")
                        lblOGPType.Text = "Gauge Painting,";
                    if (GaugeFixing == "True")
                        lblOGPType.Text += "Gauge Fixing,";
                    if (OilGreasePaint == "True")
                        lblOGPType.Text += "Gate Oiling/Greasing/Painting,";
                    if (OPothers == "True")
                        lblOGPType.Text += "Other";



                    lblOGPSubDivision.Text = SubDivisionName;
                    lblOGPSection.Text = SectionName;

                }
                else if (ID == (long)Constants.CWWorkType.OutletRepairing)
                {
                    div_OR.Visible = true;
                    dynamic OutletRepairingData = new TenderManagementBLL().GetOutletRepairingInfoByWorkID(_WorkSourceID);


                    string TenderNotice = Utility.GetDynamicPropertyValue(OutletRepairingData, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(OutletRepairingData, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(OutletRepairingData, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(OutletRepairingData, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(OutletRepairingData, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(OutletRepairingData, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(OutletRepairingData, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(OutletRepairingData, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }

                    string StartDate = Utility.GetDynamicPropertyValue(OutletRepairingData, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(OutletRepairingData, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(OutletRepairingData, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(OutletRepairingData, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(OutletRepairingData, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(OutletRepairingData, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(OutletRepairingData, "Description");

                    string SubDivisionName = Utility.GetDynamicPropertyValue(OutletRepairingData, "SubDivisionName");
                    string ChannelName = Utility.GetDynamicPropertyValue(OutletRepairingData, "ChannelName");
                    string SectionName = Utility.GetDynamicPropertyValue(OutletRepairingData, "SectionName");

                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                    lblORSubDivision.Text = SubDivisionName;
                    lblORSection.Text = SectionName;
                    lblORChannel.Text = ChannelName;

                }
                else if (ID == (long)Constants.CWWorkType.ChannelStructureWork)
                {
                    div_CSW.Visible = true;
                    //Channel Structure Work
                    dynamic ChannelStructWorkData = new TenderManagementBLL().GetChannelStructWorkInfoByWorkID(_WorkSourceID);

                    string TenderNotice = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }
                    string StartDate = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "EarnestMoney");
                    string EarnestMoneyType = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "EarnestMoneyType");

                    string TenderFees = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "Description");

                    string ChannelName = Utility.GetDynamicPropertyValue(ChannelStructWorkData, "ChannelName");


                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney + "(" + EarnestMoneyType + ")";
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                    lblCSWChannel.Text = ChannelName;


                }
                else
                {
                    div_Other.Visible = true;
                    //Other Work
                    dynamic OtherWorkData = new TenderManagementBLL().GetOtherWorkInfoByWorkID(_WorkSourceID);


                    string TenderNotice = Utility.GetDynamicPropertyValue(OtherWorkData, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(OtherWorkData, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(OtherWorkData, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(OtherWorkData, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(OtherWorkData, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(OtherWorkData, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(OtherWorkData, "CompletionPeriodUnit");
                    string CF = Utility.GetDynamicPropertyValue(OtherWorkData, "CompletionPeriodFlag");
                    bool? CompletionPeriodFlag = null;
                    if (!string.IsNullOrEmpty(CF))
                    {
                        CompletionPeriodFlag = Convert.ToBoolean(CF);
                    }
                    string StartDate = Utility.GetDynamicPropertyValue(OtherWorkData, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(OtherWorkData, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(OtherWorkData, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(OtherWorkData, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(OtherWorkData, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(OtherWorkData, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(OtherWorkData, "Description");

                    string SubDivisionName = Utility.GetDynamicPropertyValue(OtherWorkData, "SubDivisionName");
                    string SectionName = Utility.GetDynamicPropertyValue(OtherWorkData, "SectionName");

                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    if (CompletionPeriodFlag == true)
                    {
                        lblCompPeriod.Text = "Closure Period";
                    }
                    else if (CompletionPeriodFlag == false || CompletionPeriodFlag == null)
                    {
                        lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    }

                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;

                    lblOWSubDivision.Text = SubDivisionName;
                    lblOWSection.Text = SectionName;
                }
            }
            else
            {
                //Asset
                dynamic OtherWorkData = new TenderManagementBLL().GetAssetWorkInfoByWorkID(_WorkSourceID, _TenderNoticeID);

                if (OtherWorkData != null)
                {

                    string TenderNotice = Utility.GetDynamicPropertyValue(OtherWorkData, "TenderNotice");
                    string WorkType = Utility.GetDynamicPropertyValue(OtherWorkData, "WorkType");
                    string WorkName = Utility.GetDynamicPropertyValue(OtherWorkData, "WorkName");

                    lblTenderNotice.Text = TenderNotice;
                    lblWorkType.Text = WorkType;
                    lblWorkName.Text = WorkName;


                    string FundingSource = Utility.GetDynamicPropertyValue(OtherWorkData, "FundingSource");
                    string EstimatedCost = Utility.GetDynamicPropertyValue(OtherWorkData, "EstimatedCost");
                    string CompletionPeriod = Utility.GetDynamicPropertyValue(OtherWorkData, "CompletionPeriod");
                    string CompletionPeriodUnit = Utility.GetDynamicPropertyValue(OtherWorkData, "CompletionPeriodUnit");

                    string StartDate = Utility.GetDynamicPropertyValue(OtherWorkData, "StartDate");
                    if (StartDate != null)
                        StartDate = Utility.GetFormattedDate(Convert.ToDateTime(StartDate));
                    string EndDate = Utility.GetDynamicPropertyValue(OtherWorkData, "EndDate");
                    if (EndDate != null)
                        EndDate = Utility.GetFormattedDate(Convert.ToDateTime(EndDate));
                    string SanctionNo = Utility.GetDynamicPropertyValue(OtherWorkData, "SanctionNo");

                    string SanctionDate = Utility.GetDynamicPropertyValue(OtherWorkData, "SanctionDate");
                    if (SanctionDate != null)
                        SanctionDate = Utility.GetFormattedDate(Convert.ToDateTime(SanctionDate));
                    string EarnestMoney = Utility.GetDynamicPropertyValue(OtherWorkData, "EarnestMoney");
                    string TenderFees = Utility.GetDynamicPropertyValue(OtherWorkData, "TenderFees");
                    string Description = Utility.GetDynamicPropertyValue(OtherWorkData, "Description");



                    lblFundingSource.Text = FundingSource;
                    lblCost.Text = string.Format("{0:#,##0.00}", double.Parse(EstimatedCost));
                    lblCompPeriod.Text = CompletionPeriod + " " + CompletionPeriodUnit;
                    lblStratDate.Text = StartDate;
                    lblEndDate.Text = EndDate;
                    lblSanctionNo.Text = SanctionNo;
                    lblSanctionDate.Text = SanctionDate;
                    lblEarnestMoney.Text = EarnestMoney;
                    lblTenderFee.Text = string.Format("{0:#,##0.00}", double.Parse(TenderFees));
                    lblDescription.Text = Description;
                }

            }


            if (_WorkSourceID > 0)
            {
                AssetsWorkBLL AWbll = new AssetsWorkBLL();
                string AssetType = new AuctionBLL().GetAssetTypeByAssetWorkID(_WorkSourceID);
                if (!string.IsNullOrEmpty(AssetType))
                {
                    if (AssetType.ToUpper() == "ASSET")
                    {
                        PnlAssets.Visible = true;
                        PnlFlood.Visible = false;
                        PnlInfrastructure.Visible = false;
                        List<object> Lstdetail = AWbll.GetAssetWorkDetailByWorkIDForTender(_WorkSourceID);
                        gvWork.DataSource = Lstdetail;
                        gvWork.DataBind();

                    }
                    else if (AssetType.ToUpper() == "FLOOD")
                    {
                        PnlAssets.Visible = false;
                        PnlFlood.Visible = true;
                        PnlInfrastructure.Visible = false;
                        List<object> Lstdetail = AWbll.GetFloodWorkDetailByWorkID(null, _WorkSourceID);
                        gvFlood.DataSource = Lstdetail;
                        gvFlood.DataBind();

                    }
                    else if (AssetType.ToUpper() == "INFRASTRUCTURE")
                    {
                        PnlAssets.Visible = false;
                        PnlFlood.Visible = false;
                        PnlInfrastructure.Visible = true;
                        List<object> Lstdetail = AWbll.GetInfraWorkDetailByWorkID(_WorkSourceID);
                        List<Infrastructure> LstInfra = new List<Infrastructure>();
                        foreach (var item in Lstdetail)
                        {
                            Infrastructure mdlInfra = new Infrastructure();
                            mdlInfra.StructureTypeName = new TenderManagementBLL().GetStructureTypeNameByID(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "StructureType")));
                            mdlInfra.StructureNameText = Utility.GetDynamicPropertyValue(item, "StructureNameText");
                            LstInfra.Add(mdlInfra);
                        }
                        gv_Infra.DataSource = LstInfra;
                        gv_Infra.DataBind();
                    }
                }


            }



        }


        private void BindDropdown()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlOpeningOffice, CommonLists.GetTenderOffices());
        }

        protected void ddlOpeningOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlOpeningOffice.SelectedItem.Value;
            if (value == "Z")
            {
                lblOfficeLocation.InnerText = "Zone";
                //Dropdownlist.DDLZones(ddlOfficeLocated);
                long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                Dropdownlist.DDLZoneByCirclelID(ddlOfficeLocated, Convert.ToInt64(CircleID), (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (value == "C")
            {
                lblOfficeLocation.InnerText = "Circle";
                Dropdownlist.DDLCirlceByDivisionlID(ddlOfficeLocated, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), (int)Constants.DropDownFirstOption.NoOption);
                //Dropdownlist.DDLALLCircles(ddlOfficeLocated);
            }
            else if (value == "D")
            {
                lblOfficeLocation.InnerText = "Division";
                //Dropdownlist.DDLDivisions(ddlOfficeLocated);
                Dropdownlist.DDLWorksDivision(ddlOfficeLocated, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), (int)Constants.DropDownFirstOption.NoOption);
                
            }
            else
            {
                lblOfficeLocation.InnerText = "Office";
                Dropdownlist.DDLALLOtherTenderOffices(ddlOfficeLocated);
            }
        }
        protected void btnAddOpOffice_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddOpeningOffices", "$('#AddOpeningOffices').modal();", true);

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
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string Office = ddlOpeningOffice.SelectedItem.Text;
                long OfficeID = Convert.ToInt64(ddlOfficeLocated.SelectedItem.Value);

                bool IsRecordUpdated = new TenderManagementBLL().UpdateOpeningOfficeByWorkSourceID(Convert.ToInt64(hdnWordSourceID.Value), Office, OfficeID);
                LaodClosureWorkData(Convert.ToInt64(hdnWordSourceID.Value), Convert.ToInt64(hdnTenderNoticeID.Value));
                Master.ShowMessage("Your Action has been Completed.", SiteMaster.MessageType.Success);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public class Infrastructure
        {
            public string StructureTypeName { get; set; }
            public string StructureNameText { get; set; }

        }
        public void BindUserLocationZone()
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
                        #region Zone Level Bindings

                        foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                        {
                            lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                        }

                        List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                        long SelectedZoneID = lstZone.FirstOrDefault().ID;

                        ddlOfficeLocated.DataSource = lstZone;
                        ddlOfficeLocated.DataTextField = "Name";
                        ddlOfficeLocated.DataValueField = "ID";
                        ddlOfficeLocated.DataBind();
                        ddlOfficeLocated.SelectedValue = SelectedZoneID.ToString();
                        #endregion

                    }

                }
            }

        }
    }
}