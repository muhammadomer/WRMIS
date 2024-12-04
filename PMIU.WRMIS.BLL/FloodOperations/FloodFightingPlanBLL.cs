using PMIU.WRMIS.DAL.DataAccess.FloodOperations;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.BLL.FloodOperations
{
    public class FloodFightingPlanBLL : BaseBLL
    {
        FloodFightingPlanDAL dalFloodFightingPlan = new FloodFightingPlanDAL();

        #region Flood Fighting Plan

        public bool IsFightingPlanAlreadyExists(FO_FloodFightingPlan _FloodFightingPlan)
        {
            FloodFightingPlanDAL dalFloodFightingPlan = new FloodFightingPlanDAL();
            return dalFloodFightingPlan.IsFightingPlanAlreadyExists(_FloodFightingPlan);
        }

        public bool SaveFloodFightingPlan(FO_FloodFightingPlan _FloodFightingPlan)
        {
            FloodFightingPlanDAL dalFloodFightingPlan = new FloodFightingPlanDAL();
            return dalFloodFightingPlan.SaveFloodFightingPlan(_FloodFightingPlan);
        }
        public List<object> GetInfrastructures_CampSiteBy_FFPID(long FFPID)
        {
            return dalFloodFightingPlan.GetInfrastructures_CampSiteBy_FFPID(FFPID);
        }
        public object GetFFPDivisionID(long FFPID)
        {
            return dalFloodFightingPlan.GetFFPDivisionID(FFPID);
        }
        public string GetFFPStatus(long _FFPID)
        {
            return dalFloodFightingPlan.GetFFPStatus(_FFPID);
        }
        public bool DeleteFFPCampSites(long _ID)
        {
            return dalFloodFightingPlan.DeleteFFPCampSites(_ID);
        }
        public bool IsFo_FFPCampSite_IDExists(long _ID)
        {
            return dalFloodFightingPlan.IsFo_FFPCampSite_IDExists(_ID);
        }
        public bool IsFFPCampSits_Unique(FO_FFPCampSites _ObjModel)
        {
            return dalFloodFightingPlan.IsFFPCampSits_Unique(_ObjModel);
        }
        public bool IsFFPCampSitsWithoutRD_Unique(FO_FFPCampSites _ObjModel)
        {
            return dalFloodFightingPlan.IsFFPCampSitsWithoutRD_Unique(_ObjModel);
        }
        public bool IsFFPCampSits_UniqueByID(FO_FFPCampSites _ObjModel)
        {
            return dalFloodFightingPlan.IsFFPCampSits_UniqueByID(_ObjModel);
        }
        public bool IsFFPCampSitsWithoutRD_UniqueByID(FO_FFPCampSites _ObjModel)
        {
            return dalFloodFightingPlan.IsFFPCampSitsWithoutRD_UniqueByID(_ObjModel);
        }
        public bool SaveFFPCampSites(FO_FFPCampSites _ObjModel)
        {
            return dalFloodFightingPlan.SaveFFPCampSites(_ObjModel);
        }

        public long SaveFO_FFPCampSiteItems(long _ItemID, long _OMCampSiteID, int _OnSiteQty, int _UserID, long _OverallDivItemID)
        {
            return dalFloodFightingPlan.SaveFO_FFPCampSiteItems(_ItemID, _OMCampSiteID, _OnSiteQty, _UserID, _OverallDivItemID);

        }

        public long SaveFO_EPItems(long _ItemID, long EPItemID, int _PurchasedQty, int _UserID, int _CurrentQty, long EmergencyPurchaseID)
        {
            return dalFloodFightingPlan.SaveFO_EPItems(_ItemID, EPItemID, _PurchasedQty, _UserID, _CurrentQty, EmergencyPurchaseID);

        }

        public bool IsFFPStonePosition_Unique(FO_FFPStonePosition _ObjModel)
        {
            return dalFloodFightingPlan.IsFFPStonePosition_Unique(_ObjModel);
        }

        public bool SaveFFPStonePosition(FO_FFPStonePosition _ObjModel)
        {
            return dalFloodFightingPlan.SaveFFPStonePosition(_ObjModel);
        }
        public IEnumerable<DataRow> GetFFPItemsQty(long? _DivisionID, long? _FFPID, long? _Categoryid, int _year, long? _StructureTypeID, long? _StructureID)
        {
            return dalFloodFightingPlan.GetFFPItemsQty(_DivisionID, _FFPID, _Categoryid, _year, _StructureTypeID, _StructureID);
        }
        public bool DeleteFFP(long _ID)
        {
            return dalFloodFightingPlan.DeleteFFP(_ID);
        }
        public DataSet GetFFPCampInfraStrucure(long? FFPID)
        {
            return new FloodFightingPlanDAL().GetFFPCampInfraStrucure(FFPID);
        }
        public DataSet GetSDAttache(long? FFPStonePositionID, long? SDID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return new FloodFightingPlanDAL().GetSDAttache(FFPStonePositionID, SDID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }
        public DataSet GetSDAddHeader(long? FFPStonePositionID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return new FloodFightingPlanDAL().GetSDAddHeader(FFPStonePositionID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }
        public bool IsFFPDependencyExists(long _FFPID)
        {
            return dalFloodFightingPlan.IsFFPDependencyExists(_FFPID);
        }

        public IEnumerable<DataRow> GetFFPSearch(long? _FFPID, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _FFPYear, string _Status)
        {
            return dalFloodFightingPlan.GetFFPSearch(_FFPID, _ZoneID, _CircleID, _DivisionID, _FFPYear, _Status);
        }


        public dynamic GetFFPDetails(long? _FFPID, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _FFPYear, string _Status)
        {
            return dalFloodFightingPlan.GetFFPDetails(_FFPID, _ZoneID, _CircleID, _DivisionID, _FFPYear, _Status);
        }

        public IEnumerable<DataRow> GetFFPArrangement(long _FFPID)
        {
            return dalFloodFightingPlan.GetFFPArrangement(_FFPID);
        }
        public List<FO_FFPArrangements> GetAllArangements()
        {
            return dalFloodFightingPlan.GetAllArangements();
        }

        public List<FO_FFPArrangements> GetAllArangementsByFFPID(long _FFPID)
        {
            return dalFloodFightingPlan.GetAllArangementsByFFPID(_FFPID);
        }


        public List<FO_FFPArrangementType> GetAllArrangementType()
        {
            return dalFloodFightingPlan.GetAllArrangementType();
        }

        public bool AddArrangements(FO_FFPArrangements _Arrangements)
        {
            return dalFloodFightingPlan.AddArrangements(_Arrangements);
        }

        public bool UpdateArrangements(FO_FFPArrangements _Arrangements)
        {
            return dalFloodFightingPlan.UpdateArrangements(_Arrangements);
        }

        public bool DeleteArrangements(long _Arrangements)
        {
            return dalFloodFightingPlan.DeleteArrangements(_Arrangements);
        }

        public FO_FloodFightingPlan GetFFPID(long _ID)
        {
            return dalFloodFightingPlan.GetFFPID(_ID);
        }

        public bool CheckFFPStatusByID(long _FFPID)
        {
            return dalFloodFightingPlan.CheckFFPStatusByID(_FFPID);
        }
        public IEnumerable<DataRow> GetFO_FFPCampSitesByIDs(string _InfrastructureType, long? _FFPID)
        {

            return dalFloodFightingPlan.GetFO_FFPCampSitesByIDs(_InfrastructureType, _FFPID);
        }
        public IEnumerable<DataRow> GetFO_GetOverallDivItems(long _DivisionID, int _Year, Int16 _CategID)
        {
            return dalFloodFightingPlan.GetFO_GetOverallDivItems(_DivisionID, _Year, _CategID);
        }
        public List<object> GetFO_SD_Attachment_ID(long SDID)
        {
            return dalFloodFightingPlan.GetFO_SD_Attachment_ID(SDID);
        }
        public bool SaveSD_Attachment(FO_SDImages _Obj)
        {
            return dalFloodFightingPlan.SaveSD_Attachment(_Obj);
        }
        public bool DeleteFo_SD_Attachement(long _ID)
        {
            return dalFloodFightingPlan.DeleteFo_SD_Attachement(_ID);
        }
        #endregion
        public List<FO_StoneDeployment> GetStoneDeploymentByStonePositionID(int _StonePositionID)
        {
            return dalFloodFightingPlan.GetStoneDeploymentByStonePositionID(_StonePositionID);
        }


        public List<object> GetStoneDeploymentByStonePositionID(long _StonePositionID)
        {
            return dalFloodFightingPlan.GetStoneDeploymentByStonePositionID(_StonePositionID);
        }

        public DataSet GetBalancedByFFPStonePositionID(long? _FFPStonePositionID, long? StoneDeploymentID)
        {
            return dalFloodFightingPlan.GetBalancedByFFPStonePositionID(_FFPStonePositionID, StoneDeploymentID);
        }

        public bool AddStoneDeployment(FO_StoneDeployment _StoneDeployment)
        {
            return dalFloodFightingPlan.AddStoneDeployment(_StoneDeployment);
        }


        public List<object> SearchStoneDeployment(long? _FFPStonePositionID, long? _StoneDeploymentID, string _InfrastructureType, string _InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _Year, int _UserID)
        {
            return dalFloodFightingPlan.SearchStoneDeployment(_FFPStonePositionID, _StoneDeploymentID, _InfrastructureType, _InfrastructureName, _ZoneID, _CircleID, _DivisionID, _Year, _UserID);
        }


        public bool UpdateStoneDeployment(FO_StoneDeployment _StoneDeployment)
        {
            return dalFloodFightingPlan.UpdateStoneDeployment(_StoneDeployment);
        }

        public bool DeleteStoneDeployment(long _ID)
        {
            return dalFloodFightingPlan.DeleteStoneDeployment(_ID);
        }

        public bool IsStoneDeploymentIDExists(long _ID)
        {
            return dalFloodFightingPlan.IsStoneDeploymentIDExists(_ID);
        }

        public bool IsStoneDeploymentExists(long _ID)
        {
            return dalFloodFightingPlan.IsStoneDeploymentExists(_ID);
        }


        public IEnumerable<DataRow> GetSearchStoneDeployment(long? _FFPStonePositionID, long? _StoneDeploymentID, string _InfrastructureType, string _InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _Year)
        {
            return dalFloodFightingPlan.GetSearchStoneDeployment(_FFPStonePositionID, _StoneDeploymentID, _InfrastructureType, _InfrastructureName, _ZoneID, _CircleID, _DivisionID, _Year);
        }

        public int GetYearByStonePositioID(long _StonePositioID)
        {
            return dalFloodFightingPlan.GetYearByStonePositioID(_StonePositioID);
        }


        public long AddStoneDeployment(int _StoneDeploymentID, int _StonePosID, string _VehicleNumber, string _BuiltyNumber, string _Quantity, string _Cost, List<string> lstNameofFiles, int _UserID, int? _Balance)
        {
            return dalFloodFightingPlan.AddStoneDeployment(_StoneDeploymentID, _StonePosID, _VehicleNumber, _BuiltyNumber, _Quantity, _Cost, lstNameofFiles, _UserID, _Balance);

        }


        public int FO_SDGetBalancedByFFPStonePositionID(int _FFPStonePositionID, int _StoneDeploymentID)
        {

            Int64? MaxStoneDeploymentID = null;
            int? PreviousDisposedQty = null;
            int? Balance = 0;

            if (_FFPStonePositionID > 0)
            {
                DataSet DS = new DataSet();

                //if (_StoneDeploymentID == 0)
                //    DS = dalFloodFightingPlan.GetBalancedByFFPStonePositionID(_FFPStonePositionID, null);
                //else
                DS = dalFloodFightingPlan.GetBalancedByFFPStonePositionID(_FFPStonePositionID, _StoneDeploymentID);

                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    if (DR["StoneDeploymentID"] != null && DR["StoneDeploymentID"].ToString() != "")
                    {
                        MaxStoneDeploymentID = Convert.ToInt64(DR["StoneDeploymentID"].ToString());
                        PreviousDisposedQty = Convert.ToInt32(DR["DisposedQty"].ToString());
                    }
                    else
                    { PreviousDisposedQty = 0; }
                    Balance = PreviousDisposedQty;
                }
            }

            return Convert.ToInt32(Balance);
            //return dalFloodFightingPlan.FO_SDGetBalancedByFFPStonePositionID(_FFPStonePositionID, _StoneDeploymentID);
        }


        public List<object> GetAttachmentStoneDeploymentByID(long SDID)
        {
            return dalFloodFightingPlan.GetAttachmentStoneDeploymentByID(SDID);
        }


        public object GetStoneDeploymentByID(long _ID)
        {

            return dalFloodFightingPlan.GetStoneDeploymentByID(_ID);
        }

        public bool DeleteStoneDeploymentAttachmentBySDID(long _ID)
        {
            return dalFloodFightingPlan.DeleteStoneDeploymentAttachmentBySDID(_ID);
        }



        //public DataSet GetFFPItemsQty(long FFPCampSiteID)
        //{
        //    return dalFloodFightingPlan.GetFFPItemsQty(FFPCampSiteID);
        //}
        public List<object> GetItemsQtyList(long FFPCampSiteID, int year, long divisionID, long itemCatID)
        {
            return dalFloodFightingPlan.GetItemsQtyList(FFPCampSiteID, year, divisionID, itemCatID);
        }


        public IEnumerable<DataRow> GetFFPGetStonePositionByID(long? _FloodFightingPlanID)
        {
            return dalFloodFightingPlan.GetFFPGetStonePositionByID(_FloodFightingPlanID);
        }


        public List<object> GetItemsQtyEPList(long EPPurchaseID)
        {
            return dalFloodFightingPlan.GetItemsQtyEPList(EPPurchaseID);
        }

        public object GetEPItemsByID(long _EPItemID)
        {
            return dalFloodFightingPlan.GetEPItemsByID(_EPItemID);
        }

        public bool DeleteFFPStonePosition(long _ID)
        {
            return dalFloodFightingPlan.DeleteFFPStonePosition(_ID);
        }
        public long OverallDivItemsInsertion(long _OverallDivItemID, int _Year, long? _DivisionID,
                                          long? _ItemCategoryID, long? _ItemSubcategoryID, long? _StructureTypeID, long? _StructureID, long? _PreMBStatusID,
                                          long? _FloodInspectionDetailID, int? _PostAvailableQty, int? _PostRequiredQty,
                                          long? _CS_CampSiteID, int? _CS_RequiredQty, int? _OD_AdditionalQty, int _CreatedBy, int _ModifiedBy, long _ODIID)
        {
            return dalFloodFightingPlan.OverallDivItemsInsertion(_OverallDivItemID, _Year, _DivisionID,
                                          _ItemCategoryID, _ItemSubcategoryID, _StructureTypeID, _StructureID, _PreMBStatusID,
                                           _FloodInspectionDetailID, _PostAvailableQty, _PostRequiredQty,
                                           _CS_CampSiteID, _CS_RequiredQty, _OD_AdditionalQty, _CreatedBy, _ModifiedBy, _ODIID);
        }
        public long OverallDivItemsUpdation(int _Year, long _DivisionID, long _ItemCategoryID, long _ItemSubcategoryID, int _AdditionalQty, long _UserID)
        {
            return dalFloodFightingPlan.OverallDivItemsUpdation(_Year, _DivisionID, _ItemCategoryID, _ItemSubcategoryID, _AdditionalQty, _UserID);
        }
        public bool IsFFPArrangementsExist(long _FFPID, Int16 _ArrangementType)
        {
            return dalFloodFightingPlan.IsFFPArrangementsExist(_FFPID, _ArrangementType);
        }
        public bool IsFFPArrangementsExistOnUpdate(long _FFPID, Int16 _ArrangementType, long _ArrangementID)
        {
            return dalFloodFightingPlan.IsFFPArrangementsExistOnUpdate(_FFPID, _ArrangementType, _ArrangementID);
        }

        public UA_SystemParameters SystemParameterValue(string _PKey, string _PType)
        {
            return dalFloodFightingPlan.SystemParameterValue(_PKey, _PType);
        }

        public IEnumerable<DataRow> GetFFPLastYearRestorationWork(long? _FFPDivisionID, int _FFPYear)
        {
            return dalFloodFightingPlan.GetFFPLastYearRestorationWork(_FFPDivisionID, _FFPYear);
        }

        #region RoleRights
        public bool CanAddEditFFP(int _Year, long _DesignationID, string _InspectionStatus)
        {
            UA_SystemParameters systemParameters = null;
            string startDate = string.Empty;
            string endDate = string.Empty;
            bool returnVal = false;

            systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodFightingPlan", "StartDate");// 01-Jan
            startDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodFightingPlan", "EndDate"); // 31-Mar
            endDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if (_InspectionStatus == "Published")
                {
                    returnVal = false;
                }
                else if ((DateTime.Now >= Convert.ToDateTime(startDate) && DateTime.Now <= Convert.ToDateTime(endDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }
            else if (_DesignationID == Convert.ToInt64(Constants.Designation.DF))
            {
                if ((DateTime.Now.Year == _Year || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO")))) && _InspectionStatus == "Published")
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        public bool CanAddFFP(int _Year, long _DesignationID)
        {
            UA_SystemParameters systemParameters = null;
            DateTime now = DateTime.Now;
            DateTime AddFFPdate = Convert.ToDateTime(now.ToString("dd") + "-" + now.ToString("MMM") + "-" + _Year);
            string AddFFPdate1 = now.ToString("dd") + "-" + now.ToString("MMM") + "-" + _Year;
            string startDate = string.Empty;
            string endDate = string.Empty;
            bool returnVal = false;

            systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodFightingPlan", "StartDate");// 01-Jan
            startDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodFightingPlan", "EndDate"); // 31-Mar
            endDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if ((AddFFPdate >= Convert.ToDateTime(startDate) && AddFFPdate <= Convert.ToDateTime(endDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        #endregion  RoleRights

        #region Notification

        public FO_GetFloodFightingPlanNotifyData_Result GetFloodFightingPlanNotifyData(long _FloodFightingPlanID)
        {
            return dalFloodFightingPlan.GetFloodFightingPlanNotifyData(_FloodFightingPlanID);
        }

        #endregion  Notification

    }
}
