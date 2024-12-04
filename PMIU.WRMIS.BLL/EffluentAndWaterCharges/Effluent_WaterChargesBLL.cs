using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.EffluentAndWaterCharges;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.EffluentAndWaterCharges
{
    public class Effluent_WaterChargesBLL : BaseBLL
    {
        private Effluent_WaterChargesDAL dal_EWC = new Effluent_WaterChargesDAL();

        #region Billing
        public long? GetIndustryBillID(long? _IndustryID, string _ServiceType)
        {
            return dal_EWC.GetIndustryBillID(_IndustryID, _ServiceType);
        }
        public bool IndustryBillGenerated(long _IndustryID, string _ServiceType)
        {
            return dal_EWC.IndustryBillGenerated(_IndustryID, _ServiceType);
        }
        public void SaveBillPayment(EC_Payments _Mdl)
        {
            dal_EWC.SaveBillPayment(_Mdl);
        }
        public EC_Bills GetBill(string _BillNo)
        {
            return dal_EWC.GetBill(_BillNo);
        }
        public bool IndustryBillExists(string _FinancialYear, string _ServiceType)
        {
            return dal_EWC.IndustryBillExists(_FinancialYear, _ServiceType);
        }
        /// <summary>
        /// this function add Bill in database
        /// created on: 29/03/2017
        /// </summary>
        /// <param name="_BillParameters"></param>
        /// <returns>bool</returns>
        public bool AddBillParameters(EC_GenerateBillParameters _BillParameters, string Service, string FinancialYear, DateTime FYStrtDate,
            DateTime FYEndDate, DateTime LFYEndDate, long UserID, string BillNoPrefix, string BillingPrd
            , DateTime IssueDate, DateTime DueDate, int BillingDays)
        {
            return dal_EWC.AddBillParameters(_BillParameters, Service, FinancialYear, FYStrtDate,
             FYEndDate, LFYEndDate, UserID, BillNoPrefix, BillingPrd
            , IssueDate, DueDate, BillingDays);
        }
        public bool AddAdjustmentFinalizeBill(long BillID, double AdjValue, bool AddInto, int PaymentType, double AdjAmount, string AdjReason, long UserID)
        {
            return dal_EWC.AddAdjustmentFinalizeBill(BillID, AdjValue, AddInto, PaymentType, AdjAmount, AdjReason, UserID);
        }
        public bool FinalizeIndustryBills(string BillID, string ServiceType, long userid)
        {
            return dal_EWC.FinalizeIndustryBills(BillID, ServiceType, userid);
        }
        public double BillingFactorSum(string _Source, long _BillID)
        {
            return dal_EWC.BillingFactorSum(_Source, _BillID);
        }

        /// <summary>
        /// This Function Return Outlets By Division ID and ChannelID
        /// Created On: 3/4/2017
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_ChannelOutlets></returns>
        public List<CO_ChannelOutlets> GetOutletsByDivisionIDandChannelID(long _DivisionID, long _ChannelID)
        {
            return dal_EWC.GetOutletsByDivisionIDandChannelID(_DivisionID, _ChannelID);
        }

        /// <summary>
        /// this function add Canal Special Waters in database
        /// created on: 3/04/2017
        /// </summary>
        /// <param name="_CanalSpecialWater"></param>
        /// <returns>bool</returns>
        public bool AddCanalSpecialWater(EC_CanalSpecialWater _CanalSpecialWater)
        {
            return dal_EWC.AddCanalSpecialWater(_CanalSpecialWater);
        }

        //public void GenerateBill(EC_GenerateBillParameters _BillParameters)
        //{
        //    AddBillParameters(_BillParameters);
        //}
        #endregion

        #region Annual Discharge Charges
        public bool IsLessThanExistingDate_CSW(DateTime _NewSanctionDate, long _CurrentChargesID)
        {
            return dal_EWC.IsLessThanExistingDate_CSW(_NewSanctionDate, _CurrentChargesID);
        }
        public bool IsLessThanExistingDate(DateTime _NewSanctionDate, long _CurrentChargesID)
        {
            return dal_EWC.IsLessThanExistingDate(_NewSanctionDate, _CurrentChargesID);
        }
        public void UpdateAnnualSanctionedDischarge(EC_SanctionedDischargeSupply _Mdl, long _OldID)
        {
            dal_EWC.UpdateAnnualSanctionedDischarge(_Mdl, _OldID);
        }
        public EC_SanctionedDischargeSupply GetSanctionedDischarge(long _IndustryNo, string _ServiceType)
        {
            return dal_EWC.GetSanctionedDischarge(_IndustryNo, _ServiceType);
        }
        public List<object> GetSanctionedDischargeEffulent(long _IndustryNo, string _ServiceType)
        {
            return dal_EWC.GetSanctionedDischargeEffulent(_IndustryNo, _ServiceType);
        }
        public List<object> GetSantionedSupply(long _IndustryNo, string _ServiceType)
        {
            return dal_EWC.GetSantionedSupply(_IndustryNo, _ServiceType);
        }
        //GetSantionedSupplyCanal
        public List<object> GetSantionedSupplyCanal(long _IndustryNo, string _ServiceType, long _channelID, long _DivisionID)
        {
            return dal_EWC.GetSantionedSupplyCanal(_IndustryNo, _ServiceType, _channelID, _DivisionID);
        }
        #endregion

        #region Industry
        public void UpdateCSWRecord(EC_CanalSpecialWater _Mdl)
        {
            dal_EWC.UpdateCSWRecord(_Mdl);
        }
        public EC_CanalSpecialWater GetCSWRecord(long _ID)
        {
            return db.Repository<EC_CanalSpecialWater>().FindById(_ID);
        }
        public bool DeleteCSWRecord(long _ID)
        {

            //db.Repository<EC_CanalSpecialWater>().Delete(
            //    db.Repository<EC_CanalSpecialWater>().FindById(_ID)
            //    );
            //db.Save();
            return dal_EWC.DeleteCSWRecord(_ID);

        }
        public EC_CanalSpecialWater CSWDuplicationExists(long _DivisionID, long _ChannelID, int? _RD, long? _OutletID)
        {
            return dal_EWC.CSWDuplicationExists(_DivisionID, _ChannelID, _RD, _OutletID);
        }

        public void SaveIndustryCSWDetails(EC_CanalSpecialWater _CanalSpecialWaters)
        {
            dal_EWC.SaveIndustryCSWDetails(_CanalSpecialWaters);
        }
        public long GetIndustryCSWDivision(long _IndustryID)
        {
            return dal_EWC.GetIndustryCSWDivision(_IndustryID);
        }
        public void UpdateIndustryEffluentWaterDetail(EC_EffuentWaterDetails _EffluentWaters)
        {
            dal_EWC.UpdateIndustryEffluentWaterDetail(_EffluentWaters);
        }
        public void UpdateIndustry(EC_Industry _Industry)
        {
            dal_EWC.UpdateIndustry(_Industry);
        }
        public List<object> GetIndustryCSWDetails(long _IndustryID)
        {
            return dal_EWC.GetIndustryCSWDetails(_IndustryID);
        }
        public EC_EffuentWaterDetails GetIndustryEffuentWaterDetails(long _IndustryID)
        {
            return dal_EWC.GetIndustryEffuentWaterDetails(_IndustryID);
        }
        public EC_Industry GetIndustry(long _IndustryID, string _IndustryName)
        {
            return dal_EWC.GetIndustry(_IndustryID, _IndustryName);
        }
        public bool IsBillingTheService(long _IndustryNo, string _ServiceType)
        {
            return dal_EWC.IsBillingTheService(_IndustryNo, _ServiceType);
        }

        public List<object> GetIndustries()
        {
            return dal_EWC.GetIndustries();
        }
        public bool IsDrainRDWithinDivision(long _DivisionID, long _DrainID, int _RD)
        {
            return dal_EWC.IsDrainRDWithinDivision(_DivisionID, _DrainID, _RD);
        }
        //public bool DoesIndustryExists(long _IndustryID, string _IndustryName, long _IndustryType)
        //{
        //    return dal_EWC.DoesIndustryExists(_IndustryID, _IndustryName, _IndustryType);
        //}
        public void SaveIndustry(EC_Industry _NewIndustry, EC_EffuentWaterDetails _EffulentWaters, List<EC_CanalSpecialWater> _CanalSpecialWaters)
        {
            dal_EWC.SaveIndustry(_NewIndustry, _EffulentWaters, _CanalSpecialWaters);
        }
        public long GetDomainByInfrastructureID(long _InfrastructureID)
        {
            return dal_EWC.GetDomainByInfrastructureID(_InfrastructureID);
        }
        public List<object> GetDivisionsByDomain(long _DomainID)
        {
            return dal_EWC.GetDivisionsByDomain(_DomainID);
        }

        public List<object> GetDivisions()
        {
            return dal_EWC.GetDivisions();
        }

        public string GetIndustryNo()
        {
            return dal_EWC.GetIndustryNo();
        }
        public List<object> GetChannelDrainByDivision(long _DivisionID, bool _GetChannel)
        {
            return dal_EWC.GetChannelDrainByDivision(_DivisionID, _GetChannel);
        }

        public bool IsChannelRDWithinDivision(long _DivisionID, long _ChannelID, int _RD)
        {
            return dal_EWC.IsChannelRDWithinDivision(_DivisionID, _ChannelID, _RD);
        }
        public IEnumerable<DataRow> GetIndustrySearch(string _Level, long? _IndustryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _IndustryTypeID, string _IndustryStatus, string _IndustryName)
        {
            return dal_EWC.GetIndustrySearch(_Level, _IndustryID, _ZoneID, _CircleID, _DivisionID, _IndustryTypeID, _IndustryStatus, _IndustryName);
        }
        public IEnumerable<DataRow> GetFinalizeBillPrint(string _ServiceType, string _FinancialYear, long? _IndustryTypeID, long? _ZoneID, long? _CircleID, long? _DivisionID, string _IndustryName, string _IndustryNo)
        {
            return dal_EWC.GetFinalizeBillPrint(_ServiceType, _FinancialYear, _IndustryTypeID, _ZoneID, _CircleID, _DivisionID, _IndustryName, _IndustryNo);
        }
        public IEnumerable<DataRow> BillPrint(string _ServiceType, string _FinancialYear, long? _IndustryTypeID, long? _ZoneID, long? _CircleID, long? _DivisionID , string _IndustryName , string _IndustryNo)
        {
            return dal_EWC.BillPrint(_ServiceType, _FinancialYear, _IndustryTypeID, _ZoneID, _CircleID, _DivisionID, _IndustryName, _IndustryNo);
        }
        public DataSet GetBillDetails(string _ServiceType,  long? _BillID)
        {
            return dal_EWC.GetBillDetails(_ServiceType, _BillID);
        }
        public object GetLastECBillID(string ServiceType, string FinancialYear, long industryID)
        {
            return dal_EWC.GetLastECBillID(ServiceType, FinancialYear, industryID);
        }
        public double WaterRate(string _service, int _getLastYear, DateTime _FYStart)
        {
            return dal_EWC.WaterRate(_service, _getLastYear, _FYStart);
        }
        public double EC_GetLastSnctdDschrgSuply(long _IndustryID, string _service, long _CSWID, int _getLastYear, DateTime _FYStart)
        {
            return dal_EWC.EC_GetLastSnctdDschrgSuply(_IndustryID, _service, _CSWID, _getLastYear, _FYStart);
        }
        public double EC_GetCalculateSurcharge(double _amount, string _service)
        {
            return dal_EWC.EC_GetCalculateSurcharge(_amount, _service);
        }
        public bool DeleteIndustryByID(long _IndustryID)
        {
            return dal_EWC.DeleteIndustryByID(_IndustryID);
        }
        #endregion

        #region Refrence Data
        public bool BillSetup_AddNew(EC_PrintBillSertup _Mdl)
        {
            return dal_EWC.BillSetup_AddNew(_Mdl);
        }
        public EC_PrintBillSertup BillSetup_Get()
        {
            return dal_EWC.BillSetup_Get();
        }
        public bool Surcharges_AddNew(EC_SurchargeAmount _Mdl)
        {
            return dal_EWC.Surcharges_AddNew(_Mdl);
        }
        public List<object> Surcharges_GetList(string _Source)
        {
            return dal_EWC.Surcharges_GetList(_Source);
        }
        public object Surcharges_GetLatestRateDetails(string _Source)
        {
            return dal_EWC.Surcharges_GetLatestRateDetails(_Source);
        }
        public List<object> GetTaxType()
        {
            return dal_EWC.TaxType_GetList();
        }
        public object Operations_Taxes(int _OperationType, EC_ApplicableTaxes _Data)
        {
            object result = null;
            switch (_OperationType)
            {
                case Constants.CHECK_ASSOCIATION:
                    result = dal_EWC.Taxes_AssiciationExists(_Data.ID);
                    break;

                case Constants.CRUD_CREATE:
                    dal_EWC.Taxes_AddNew(_Data);
                    result = true;
                    break;

                case Constants.CRUD_DELETE:
                    dal_EWC.Taxes_Delete(_Data.ID);
                    result = true;
                    break;

                case Constants.CRUD_READ:
                    result = dal_EWC.Taxes_Get(_Data.ApplicableTax, null, _Data.Source);
                    break;
                case Constants.CRUD_UPDATE:
                    dal_EWC.Taxes_Update(_Data);
                    result = true;
                    break;
                default:
                    break;
            }

            return result;
        }
        public List<object> Taxes_GetList(string _Source)
        {
            return dal_EWC.Taxes_GetList(_Source);
        }
        public bool RatePerCusecs_AddNew(EC_WaterCharges _Mdl)
        {
            return dal_EWC.RatePerCusecs_AddNew(_Mdl);
        }
        public object RatePerCusecs_GetLatestRateDetails(string _Source)
        {
            return dal_EWC.RatePerCusecs_GetLatestRateDetails(_Source);
        }
        public List<object> RatePerCusecs_GetList(string _Source)
        {
            return dal_EWC.RatePerCusecs_GetList(_Source);
        }

        public object Operations_Bank(int _OperationType, EC_Bank _Data)
        {
            object result = null;
            switch (_OperationType)
            {
                case Constants.CHECK_ASSOCIATION:
                    result = dal_EWC.Bank_AssiciationExists(_Data.ID);
                    break;

                case Constants.CRUD_CREATE:
                    dal_EWC.Bank_AddNew(_Data);
                    result = true;
                    break;

                case Constants.CRUD_DELETE:
                    dal_EWC.Bank_Delete(_Data.ID);
                    result = true;
                    break;

                case Constants.CRUD_READ:
                    result = dal_EWC.Bank_Get(_Data.Name, null);
                    break;
                case Constants.CRUD_UPDATE:
                    dal_EWC.Bank_Update(_Data);
                    result = true;
                    break;
                default:
                    break;
            }

            return result;
        }
        public List<EC_Bank> Bank_GetList()
        {
            return dal_EWC.Bank_GetList();
        }

        public object Operations_DSSources(int _OperationType, EC_DischargeSupplySource _Data)
        {
            object result = null;
            switch (_OperationType)
            {
                case Constants.CHECK_ASSOCIATION:
                    result = dal_EWC.DSSource_AssiciationExists(_Data.ID);
                    break;

                case Constants.CRUD_CREATE:
                    dal_EWC.DSSource_AddNew(_Data);
                    result = true;
                    break;

                case Constants.CRUD_DELETE:
                    dal_EWC.DSSource_Delete(_Data.ID);
                    result = true;
                    break;

                case Constants.CRUD_READ:
                    result = dal_EWC.DSSource_Get(_Data.Name, null);
                    break;
                case Constants.CRUD_UPDATE:
                    dal_EWC.DSSource_Update(_Data);
                    result = true;
                    break;
                default:
                    break;
            }

            return result;
        }
        public List<EC_DischargeSupplySource> DSSource_GetList()
        {
            return dal_EWC.DSSource_GetList();
        }

        public object Operations_IndustryType(int _OperationType, EC_IndustryType _Data)
        {
            object result = null;
            switch (_OperationType)
            {
                case Constants.CHECK_ASSOCIATION:
                    result = dal_EWC.IndustryType_AssiciationExists(_Data.ID);
                    break;

                case Constants.CRUD_CREATE:
                    dal_EWC.IndustryType_AddNew(_Data);
                    result = true;
                    break;

                case Constants.CRUD_DELETE:
                    dal_EWC.IndustryType_Delete(_Data.ID);
                    result = true;
                    break;

                case Constants.CRUD_READ:
                    result = dal_EWC.IndustryType_Get(_Data.Name, null);
                    break;
                case Constants.CRUD_UPDATE:
                    dal_EWC.IndustryType_Update(_Data);
                    result = true;
                    break;
                default:
                    break;
            }

            return result;
        }
        public List<EC_IndustryType> IndustryType_GetList()
        {
            return dal_EWC.IndustryType_GetList();
        }
        public List<EC_Bills> Financial_GetList()
        {
            return dal_EWC.Financial_GetList();
        }
        #endregion
    }
}
