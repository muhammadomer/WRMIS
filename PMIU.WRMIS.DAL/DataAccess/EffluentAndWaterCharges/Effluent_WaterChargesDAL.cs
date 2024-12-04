using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.ClosureOperations;
using PMIU.WRMIS.DAL.Repositories.WaterLosses;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories.EffluentAndWaterCharges;

namespace PMIU.WRMIS.DAL.DataAccess.EffluentAndWaterCharges
{
    public class Effluent_WaterChargesDAL
    {
        ContextDB db = new ContextDB();

        #region Billing
        public long? GetIndustryBillID(long? _IndustryID, string _ServiceType)
        {
            int count = db.Repository<EC_Bills>().Query().Get().Where(x => x.IndustryID == _IndustryID && x.ServiceType.Equals(_ServiceType)).Count();

            if (count > 0)
            {
                return db.Repository<EC_Bills>().Query().Get()
                    .Where(x => x.IndustryID == _IndustryID && x.ServiceType.Equals(_ServiceType)).Max(x => x.ID);
            }
            return null;
        }

        public bool IndustryBillGenerated(long _IndustryID, string _ServiceType)
        {
            int count = db.Repository<EC_Bills>().Query().Get().Where(x => x.IndustryID == _IndustryID && x.ServiceType.Equals(_ServiceType)).Count();

            if (count > 0)
                return true;
            return false;
        }
        public void SaveBillPayment(EC_Payments _Mdl)
        {
            db.Repository<EC_Payments>().Insert(_Mdl);
            EC_Industry mdl = db.Repository<EC_Industry>().FindById(_Mdl.IndustryID);
            if (string.Equals(_Mdl.ServiceType, Constants.ECWServiceType.EFFLUENT.ToString()))
                mdl.EWCurrentBalance = mdl.EWCurrentBalance - (double)_Mdl.PaymentAmount;
            else
                mdl.CWCurrentBalance = mdl.CWCurrentBalance - (double)_Mdl.PaymentAmount;
            db.Repository<EC_Industry>().Update(mdl);
            db.Save();
        }
        public EC_Bills GetBill(string _BillNo)
        {
            return db.Repository<EC_Bills>().Query().Get()
                .Where(x => x.BillNo.Equals(_BillNo, StringComparison.OrdinalIgnoreCase) && x.Status == "FINALIZED")
                .FirstOrDefault();
        }
        public bool IndustryBillExists(string _FinancialYear, string _ServiceType)
        {
            int count = db.Repository<EC_Bills>().Query().Get().Where(x => x.FinancialYear.Equals(_FinancialYear) && x.ServiceType.Equals(_ServiceType)).Count();

            if (count > 0)
                return true;
            return false;
        }
        public void GenerateBill(Constants.ECWServiceType _ServiceType, string _FinancialYear, DateTime _IssueDate, DateTime _DueDate, bool _IncludeTaxes, bool? AddAjustmnts, string _AdjustmntType, double? AdjustmntAmount, string _AdjustmntReason)
        {
            //Tuple<long, int, int> industryBilling = new Tuple<long, int, int>();

            List<EC_Industry> lstIndustries = new List<EC_Industry>();
            if (_ServiceType == Constants.ECWServiceType.EFFLUENT)
            {
                lstIndustries = db.Repository<EC_Industry>().Query().Get()
                .Where(x => x.IsEffluentWater == true).ToList();
            }

            int FYstartyear = Convert.ToInt32(_FinancialYear.Substring(5)) - 1;
            bool isLeapYear = true;
            //TODO:
            // logic to calculate is current is a leap Year or not

            List<IndustryBill> lstBill = new List<IndustryBill>();
            IndustryBill bill = new IndustryBill();
            foreach (EC_Industry item in lstIndustries)
            {
                bill = new IndustryBill();
                //TODO bill no :
                bill.billingPrd = "1-July-" + FYstartyear + " to 30-June-" + (FYstartyear + 1);
                bill.FY = _FinancialYear;
                bill.indusryid = item.ID;
                bill.issuedate = _IssueDate;
                bill.duedate = _DueDate;



            }
        }

        public class IndustryBill
        {
            public string BillNo { get; set; }
            public string billingPrd;
            public string FY;
            public long indusryid;
            public DateTime issuedate;
            public DateTime duedate;
            public double payable;
            public double payableAfterDueDate;
            public double surcharge;
            public double lastbalance;
            public int billingdays;
            public int lastyearbillingdays;
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
            db.Repository<EC_GenerateBillParameters>().Insert(_BillParameters);
            db.Save();
            long BillParamID = _BillParameters.ID;

            db.ExecuteStoredProcedureDataSet("EC_GenerateIndustryBill", Service, FinancialYear, FYStrtDate, FYEndDate, LFYEndDate, BillParamID, UserID, BillNoPrefix
                , BillingPrd, IssueDate, DueDate, BillingDays);
            return true;
        }
        public bool AddAdjustmentFinalizeBill(long BillID, double AdjValue, bool AddInto, int PaymentType, double AdjAmount, string AdjReason, long UserID)
        {
            db.ExecuteStoredProcedureDataSet("EC_AddSpecialAdjustmentToBill", BillID, AdjValue, AddInto, PaymentType, AdjAmount, AdjReason, UserID);
            return true;
        }
        public bool FinalizeIndustryBills(string BillID, string ServiceType, long userid)
        {
            db.ExecuteStoredProcedureDataSet("EC_FinalizeIndustryBills", BillID, ServiceType, userid);
            return true;
        }
        public double BillingFactorSum(string _Source, long _BillID)
        {
            double myAmountSum;
            return myAmountSum = (double)db.Repository<EC_BillFactor>().Query().Get().Where(x => x.FactorCategory.Equals(_Source) && x.BillID == _BillID).Sum(x => x.Amount);
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
            List<long> lstSection = db.Repository<CO_Section>().GetAll().Where(x => x.CO_SubDivision.CO_Division.ID == _DivisionID).Select(x => x.ID).ToList();

            List<CO_ChannelIrrigationBoundaries> lstChannelIrrigationBoundaries = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Where(x => lstSection.Contains(x.SectionID.Value) && x.ChannelID == _ChannelID).ToList();

            List<CO_ChannelOutlets> lstOutlets = new List<CO_ChannelOutlets>();
            List<CO_ChannelOutlets> Outlets = new List<CO_ChannelOutlets>();

            for (int i = 0; i < lstChannelIrrigationBoundaries.Count(); i++)
            {
                int SectionRD = lstChannelIrrigationBoundaries[i].SectionRD;
                int SectionToRD = (int)lstChannelIrrigationBoundaries[i].SectionToRD;
                Outlets = db.Repository<CO_ChannelOutlets>().GetAll().Where(x => x.ChannelID == _ChannelID && (x.OutletRD >= SectionRD && x.OutletRD <= SectionToRD)).ToList();

                //var result = list1.Concat(list2).OrderBy(x => x.Elevation).ToList();
                //lstOutlets = lstOutlets.Union(lstOutlets).ToList();
                lstOutlets = lstOutlets.Union(Outlets).ToList();
            }

            lstOutlets = lstOutlets.OrderBy(x => x.OutletRD).ToList();
            return lstOutlets;
        }

        /// <summary>
        /// this function add Canal Special Waters in database
        /// created on: 3/04/2017
        /// </summary>
        /// <param name="_CanalSpecialWater"></param>
        /// <returns>bool</returns>
        public bool AddCanalSpecialWater(EC_CanalSpecialWater _CanalSpecialWater)
        {
            db.Repository<EC_CanalSpecialWater>().Insert(_CanalSpecialWater);
            db.Save();
            return true;
        }

        #endregion

        #region Annual Discharge Charges
        public bool IsLessThanExistingDate_CSW(DateTime _NewSanctionDate, long _CurrentChargesID)
        {
            EC_SanctionedDischargeSupply mdl = db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                 .Where(x => x.CanalSpecialWaterID == _CurrentChargesID)
                 .OrderByDescending(x => x.SanctionedDate)
                 .FirstOrDefault();
            if (mdl != null)
            {
                DateTime? oldDate = mdl.SanctionedDate;
                if (oldDate != null)
                {
                    if (Convert.ToDateTime(oldDate) > _NewSanctionDate)
                        return true;
                }
            }
            return false;
        }

        public bool IsLessThanExistingDate(DateTime _NewSanctionDate, long _CurrentChargesID)
        {
            EC_SanctionedDischargeSupply mdl = db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                .Where(x => x.ID == _CurrentChargesID).FirstOrDefault();
            if (mdl != null)
            {
                DateTime? oldDate = mdl.SanctionedDate;
                if (oldDate != null)
                {
                    if (Convert.ToDateTime(oldDate) > _NewSanctionDate)
                        return true;
                }
            }
            return false;
        }
        public void UpdateAnnualSanctionedDischarge(EC_SanctionedDischargeSupply _Mdl, long _OldID)
        {
            if (_OldID > 0)
            {
                EC_SanctionedDischargeSupply mdlOld = db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                  .Where(x => x.ID == _OldID).FirstOrDefault();
                if (mdlOld != null)
                    mdlOld.IsActive = false;
                db.Repository<EC_SanctionedDischargeSupply>().Update(mdlOld);
            }

            db.Repository<EC_SanctionedDischargeSupply>().Insert(_Mdl);
            db.Save();
        }
        public EC_SanctionedDischargeSupply GetSanctionedDischarge(long _IndustryNo, string _ServiceType)
        {
            return db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                .Where(x => x.IsActive == true && x.IndustryID == _IndustryNo
                    && x.ServiceType.Equals(_ServiceType, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
        }
        //RatePerCusecs_GetList
        public List<object> GetSanctionedDischargeEffulent(long _IndustryNo, string _ServiceType)
        {
            return db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                .Where(x => x.ServiceType.Equals(_ServiceType) && x.IndustryID == _IndustryNo).OrderByDescending(x => x.SanctionedDate).ToList()
                .Select(x => new
                {
                    ID = x.ID,
                    SanctionedDate = Utility.GetFormattedDate(x.SanctionedDate),
                    SanctionedNo = x.SanctionedNo,
                    SanctionedAuthority = x.SanctionedAuthority,
                    SanctionedSupplyDischarge = x.SanctionedSupplyDischarge,
                    Attachment = x.Attachment

                }).ToList<object>();

        }
        public List<object> GetSantionedSupplyCanal(long _IndustryNo, string _ServiceType, long _channelID, long _DivisionID)
        {
            return db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                   .Where(x => x.IndustryID == _IndustryNo
                       && x.ServiceType == _ServiceType
                       && x.CanalSpecialWaterID == _channelID).ToList().Select(x => new
                       {
                           ID = x.ID,
                           recordID = x.ID,
                           SnctDate = Utility.GetFormattedDate(x.SanctionedDate),
                           SnctNo = x.SanctionedNo,
                           SnctAuth = x.SanctionedAuthority,
                           SnctSuply = x.SanctionedSupplyDischarge,
                           Attcmnt = x.Attachment
                       }).OrderByDescending(x => x.ID).ToList<object>();

        }
        public List<object> GetSantionedSupply(long _IndustryNo, string _ServiceType)
        {
            List<EC_CanalSpecialWater> lstCSW = db.Repository<EC_CanalSpecialWater>().Query().Get()
                  .Where(x => x.IndustryID == _IndustryNo).ToList();

            List<object> lst = new List<object>();

            if (lstCSW == null && lstCSW.Count == 0)
                return lst;
            List<EC_SanctionedDischargeSupply> lstData = db.Repository<EC_SanctionedDischargeSupply>().Query().Get()
                   .Where(x => x.IsActive == true && x.IndustryID == _IndustryNo
                       && x.ServiceType.Equals(_ServiceType, StringComparison.OrdinalIgnoreCase) && x.IsActive == true).ToList();


            if (lstCSW != null && lstCSW.Count > 0)
            {
                foreach (var item in lstCSW)
                {
                    if (lstData == null || lstData.Count == 0)
                    {
                        lst.Add(new
                        {
                            ID = item.ID,
                            recordID = 0,
                            Name = item.SupplyFrom,
                            ChannelID = item.ChannelID,
                            DivisionID = item.DivisionID,
                            Type = item.ChannelOutletID != null ? "Outlet" : "RD",
                            SnctDate = "",
                            SnctNo = "",
                            SnctAuth = "",
                            SnctSuply = "",
                            Attcmnt = ""
                        });
                    }
                    else
                    {
                        List<EC_SanctionedDischargeSupply> lstTemp = lstData.Where(x => x.CanalSpecialWaterID == item.ID).ToList();
                        if (lstTemp.Count() > 0)
                        {
                            EC_SanctionedDischargeSupply mdl = lstTemp.ElementAt(0);
                            lst.Add(new
                            {
                                ID = item.ID,
                                recordID = mdl.ID,
                                Name = item.SupplyFrom,
                                ChannelID = item.ChannelID,
                                DivisionID = item.DivisionID,
                                Type = item.ChannelOutletID != null ? "Outlet" : "RD",
                                SnctDate = Utility.GetFormattedDate(mdl.SanctionedDate),
                                SnctNo = mdl.SanctionedNo,
                                SnctAuth = mdl.SanctionedAuthority,
                                SnctSuply = mdl.SanctionedSupplyDischarge,
                                Attcmnt = mdl.Attachment
                            });
                        }
                        else
                        {
                            lst.Add(new
                            {
                                ID = item.ID,
                                recordID = 0,
                                Name = item.SupplyFrom,
                                ChannelID = item.ChannelID,
                                DivisionID = item.DivisionID,
                                Type = item.ChannelOutletID != null ? "Outlet" : "RD",
                                SnctDate = "",
                                SnctNo = "",
                                SnctAuth = "",
                                SnctSuply = "",
                                Attcmnt = ""
                            });
                        }
                    }
                }
            }
            return lst;
        }
        #endregion

        #region Industry
        public void UpdateCSWRecord(EC_CanalSpecialWater _Mdl)
        {

            EC_CanalSpecialWater mdl = db.Repository<EC_CanalSpecialWater>().FindById(_Mdl.ID);

            mdl.DivisionID = _Mdl.DivisionID;
            mdl.ChannelID = _Mdl.ChannelID;
            mdl.SupplyFrom = _Mdl.SupplyFrom;
            mdl.ChannelOutletID = _Mdl.ChannelOutletID;
            mdl.RD = _Mdl.RD;
            mdl.Side = _Mdl.Side;
            mdl.SupplySourceID = _Mdl.SupplySourceID;
            mdl.InstallationDate = _Mdl.InstallationDate;
            mdl.InstallationCost = _Mdl.InstallationCost;
            mdl.AgreementSignedOn = _Mdl.AgreementSignedOn;
            mdl.AgreementEndDate = _Mdl.AgreementEndDate;
            mdl.AgreementParties = _Mdl.AgreementParties;
            mdl.InActive = _Mdl.InActive;
            mdl.FromDate = _Mdl.FromDate;
            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = _Mdl.ModifiedDate;

            db.Repository<EC_CanalSpecialWater>().Update(mdl);
            db.Save();
        }
        public EC_CanalSpecialWater CSWDuplicationExists(long _DivisionID, long _ChannelID, int? _RD, long? _OutletID)
        {
            var q = db.Repository<EC_CanalSpecialWater>().Query().Get()
                .Where(x => x.DivisionID == _DivisionID && x.ChannelID == _ChannelID &&
                    (_RD == null || x.RD == _RD) && (_OutletID == null || x.ChannelOutletID == _OutletID))
                .ToList();
            if (q == null || q.Count() <= 0)
                return null;

            return q.OrderByDescending(x => x.ID).ToList().ElementAt(0);
        }
        public void SaveIndustryCSWDetails(EC_CanalSpecialWater _CanalSpecialWaters)
        {
            EC_Industry mdl = db.Repository<EC_Industry>().FindById(_CanalSpecialWaters.IndustryID);
            mdl.IsCanalWater = true;
            db.Repository<EC_Industry>().Update(mdl);
            db.Repository<EC_CanalSpecialWater>().Insert(_CanalSpecialWaters);
            db.Save();
        }
        public long GetIndustryCSWDivision(long _IndustryID)
        {
            List<EC_CanalSpecialWater> lst = db.Repository<EC_CanalSpecialWater>().Query().Get().Where(x => x.IndustryID == _IndustryID).ToList();

            if (lst.Count() > 0)
                return lst.FirstOrDefault().DivisionID;
            else
                return -1;
        }

        public List<object> GetIndustryCSWDetails(long _IndustryID)
        {
            List<EC_CanalSpecialWater> lst = db.Repository<EC_CanalSpecialWater>().Query().Get()
                 .Where(x => x.IndustryID == _IndustryID).ToList();

            return lst.Select(x => new
                {
                    ID = x.ID,
                    Channel = GetChannelName((long)x.ChannelID),
                    SForm = (x.ChannelOutletID == null ? "RD" : "Outlet"),
                    RO = x.SupplyFrom,
                    Date = Utility.GetFormattedDate(x.InstallationDate),
                    InActive = x.InActive == true ? "In-active" : "Active"
                })
                .ToList<object>();
        }


        private string GetChannelName(long _ID)
        {
            var q = db.Repository<CO_Channel>().Query().Get().Where(x => x.ID == _ID).SingleOrDefault();
            if (q == null)
                return string.Empty;

            return q.NAME;
        }

        public void UpdateIndustryEffluentWaterDetail(EC_EffuentWaterDetails _EffluentWaters)
        {
            if (!IsBillingTheService((long)_EffluentWaters.IndustryID, Constants.ECWServiceType.EFFLUENT.ToString()))
            {
                EC_Industry mdl = db.Repository<EC_Industry>().FindById(_EffluentWaters.IndustryID);
                mdl.IsEffluentWater = true;
                db.Repository<EC_Industry>().Update(mdl);
                db.Repository<EC_EffuentWaterDetails>().Insert(_EffluentWaters);
            }
            else
            {
                EC_EffuentWaterDetails mdl = db.Repository<EC_EffuentWaterDetails>().FindById(_EffluentWaters.ID);

                mdl.DivisionID = _EffluentWaters.DivisionID;
                mdl.InfrastructureTypeID = _EffluentWaters.InfrastructureTypeID;
                mdl.StructureID = _EffluentWaters.StructureID;
                mdl.RD = _EffluentWaters.RD;
                mdl.Side = _EffluentWaters.Side;
                mdl.DischargeSourceID = _EffluentWaters.DischargeSourceID;
                mdl.InstallationDate = _EffluentWaters.InstallationDate;
                mdl.InstallationCost = _EffluentWaters.InstallationCost;
                mdl.AgreementSignedOn = _EffluentWaters.AgreementSignedOn;
                mdl.AgreementEndDate = _EffluentWaters.AgreementEndDate;
                mdl.AgreementParties = _EffluentWaters.AgreementParties;
                mdl.ModifiedDate = _EffluentWaters.ModifiedDate;
                mdl.ModifiedBy = _EffluentWaters.ModifiedBy;
                mdl.InActive = _EffluentWaters.InActive;
                mdl.FromDate = _EffluentWaters.FromDate;
                db.Repository<EC_EffuentWaterDetails>().Update(mdl);
            }
            db.Save();
        }
        public void UpdateIndustry(EC_Industry _Industry)
        {
            EC_Industry mdl = db.Repository<EC_Industry>().FindById(_Industry.ID);
            mdl.IndustryName = _Industry.IndustryName;
            mdl.IndustryStatus = _Industry.IndustryStatus;
            mdl.IndustryTypeID = _Industry.IndustryTypeID;
            mdl.NTNNo = _Industry.NTNNo;
            mdl.ServiceType = _Industry.ServiceType;
            mdl.Address = _Industry.Address;
            mdl.PhoneNo = _Industry.PhoneNo;
            mdl.Fax = _Industry.Fax;
            mdl.Email = _Industry.Email;
            mdl.GisX = _Industry.GisX;
            mdl.GisY = _Industry.GisY;
            mdl.WTPlantExist = _Industry.WTPlantExist;
            mdl.WTPlantCondition = _Industry.WTPlantCondition;
            mdl.ContactPersonName = _Industry.ContactPersonName;
            mdl.CPCellNo = _Industry.CPCellNo;
            mdl.CPCNIC = _Industry.CPCNIC;
            mdl.CPEmail = _Industry.CPEmail;
            mdl.ModifiedBy = _Industry.ModifiedBy;
            mdl.ModifiedDate = _Industry.ModifiedDate;
            mdl.IsEffluentWater = _Industry.IsEffluentWater;
            mdl.IsCanalWater = _Industry.IsCanalWater;
            db.Repository<EC_Industry>().Update(mdl);
            db.Save();
        }
        public EC_EffuentWaterDetails GetIndustryEffuentWaterDetails(long _IndustryID)
        {
            return db
                .Repository<EC_EffuentWaterDetails>().Query().Get()
                .Where(x => x.IndustryID == _IndustryID)
                .SingleOrDefault();
        }
        public EC_Industry GetIndustry(long _IndustryID, string _IndustryName)
        {
            if (_IndustryID == 0 && _IndustryName == string.Empty)
                return null;

            return db
                .Repository<EC_Industry>().Query().Get()
                .Where(x =>
                    (_IndustryID == 0 || x.ID == _IndustryID)
                    &&
                    (_IndustryName == string.Empty || x.IndustryName.Equals(_IndustryName, StringComparison.OrdinalIgnoreCase))
                )
                .SingleOrDefault();
        }
        public bool IsBillingTheService(long _IndustryNo, string _ServiceType)
        {
            bool? value = false;
            if (_ServiceType.Equals("EFFLUENT"))
            {

                EC_Industry mdl = db.Repository<EC_Industry>().Query().Get().Where(x => x.ID == _IndustryNo)
               .SingleOrDefault();

                if (mdl != null)
                    value = mdl.IsEffluentWater;
            }
            else
            {
                EC_Industry mdl = db.Repository<EC_Industry>().Query().Get().Where(x => x.ID == _IndustryNo)
                .SingleOrDefault();

                if (mdl != null)
                    value = mdl.IsCanalWater;
            }

            if (value != null)
                return (bool)value;

            return false;
        }
        public List<object> GetIndustries()
        {
            List<object> lstIndustries = db.Repository<EC_Industry>().Query().Get().ToList()
                .Select(x => new
                {
                    No = x.ID,
                    TypeID = x.IndustryTypeID,
                    Name = x.IndustryName,
                    Status = x.IndustryStatus
                })
                .ToList()
                .Select(x => new
                {
                    No = x.No,
                    TypeID = x.TypeID,
                    Type = IndustryType_Get("", Convert.ToInt64(x.TypeID)).Name,
                    Name = x.Name,
                    Status = x.Status
                })
                .OrderBy(x => x.Name)
                .ToList<object>();

            return (lstIndustries == null ? new List<object>() : lstIndustries);
        }
        //public bool DoesIndustryExists(long _IndustryID, string _IndustryName, long _IndustryType)
        //{
        //    EC_Industry mdl = db.Repository<EC_Industry>().Query().Get()
        //         .Where(x => x.IndustryName.Equals(_IndustryName, StringComparison.OrdinalIgnoreCase)
        //             && x.IndustryTypeID == _IndustryType)
        //         .FirstOrDefault();

        //    if (mdl != null && mdl.ID != _IndustryID)
        //        return true;

        //    return false;
        //}
        public void SaveIndustry(EC_Industry _NewIndustry, EC_EffuentWaterDetails _EffulentWaters, List<EC_CanalSpecialWater> _CanalSpecialWaters)
        {

            if (_EffulentWaters != null)
            {
                _EffulentWaters.InActive = false;
                db.Repository<EC_EffuentWaterDetails>().Insert(_EffulentWaters);
                _NewIndustry.EWBillingStart = DateTime.Now;
            }

            if (_CanalSpecialWaters != null)
            {
                foreach (EC_CanalSpecialWater item in _CanalSpecialWaters)
                {
                    item.InActive = false;
                    item.IndustryID = _NewIndustry.ID;
                    db.Repository<EC_CanalSpecialWater>().Insert(item);
                }

                _NewIndustry.CWBillingStart = DateTime.Now;
            }
            db.Repository<EC_Industry>().Insert(_NewIndustry);

            db.Save();
        }
        public long GetDomainByInfrastructureID(long _InfrastructureID)
        {
            return (long)db.Repository<CO_StructureType>().Query().Get()
                .Where(x => x.ID == _InfrastructureID).FirstOrDefault().DomainID;
        }
        public string GetIndustryNo()
        {
            if (db.Repository<EC_Industry>().GetAll().Count() > 0)
                return "" + (db.Repository<EC_Industry>().Query().Get().Max(x => x.ID) + 1);
            else
                return "" + 1;
        }

        public List<object> GetDivisionsByDomain(long _DomainID)
        {
            return db.Repository<CO_Division>().Query().Get().Where(x => x.DomainID == _DomainID)
                 .ToList().Select(x => new { ID = x.ID, Name = x.Name }).OrderBy(x => x.Name)
                 .ToList<object>();
        }

        public List<object> GetDivisions()
        {
            return db.ExtRepositoryFor<WaterLossesRepository>().GetDivisions();
        }

        public List<object> GetChannelDrainByDivision(long _DivisionID, bool _GetChannel)
        {
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
            if (_GetChannel)
                return repWaterLosses.GetChannelsByDivision(_DivisionID);
            else
                return repWaterLosses.GetDrainsByDivision(_DivisionID);
        }
        public bool IsChannelRDWithinDivision(long _DivisionID, long _ChannelID, int _RD)
        {
            ClosureOperationsRepository repCO = db.ExtRepositoryFor<ClosureOperationsRepository>();
            int? startRD = repCO.GetStartRDRangeByDivision(_DivisionID, _ChannelID);
            int? endRD = repCO.GetEndRDRangeByDivision(_DivisionID, _ChannelID);

            if (startRD == null || endRD == null)
                return false;

            if (_RD >= startRD && _RD <= endRD)
                return true;

            return false; ;
        }
        public bool IsDrainRDWithinDivision(long _DivisionID, long _DrainID, int _RD)
        {
            ClosureOperationsRepository repCO = db.ExtRepositoryFor<ClosureOperationsRepository>();
            int? startRD = repCO.GetDrainStartRDRangeByDivision(_DivisionID, _DrainID);
            int? endRD = repCO.GetDrainEndRDRangeByDivision(_DivisionID, _DrainID);

            /* This code has been commented so that migrated data without SectionID and RDs
                     * do not cause exception or block the flow */

            /* Begin */

            // if (startRD == null || endRD == null)
            //return false;

            /* End */

            /* This code has been added in its place. */

            /* Begin */

            if (startRD == null || endRD == null)
                return true;

            /* End */

            if (_RD >= startRD && _RD <= endRD)
                return true;

            return false;
        }
        public IEnumerable<DataRow> GetIndustrySearch(string _Level, long? _IndustryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _IndustryTypeID, string _IndustryStatus, string _IndustryName)
        {
            return db.ExecuteDataSet("Proc_EC_SearchIndustry", _Level, _IndustryID, _ZoneID, _CircleID, _DivisionID, _IndustryTypeID, _IndustryStatus, _IndustryName);
        }
        public IEnumerable<DataRow> GetFinalizeBillPrint(string _ServiceType, string _FinancialYear, long? _IndustryTypeID, long? _ZoneID, long? _CircleID, long? _DivisionID, string _IndustryName, string _IndustryNo)
        {
            return db.ExecuteDataSet("Proc_EC_FinalizePrintBill", _ServiceType, _FinancialYear, _IndustryTypeID, _ZoneID, _CircleID, _DivisionID, _IndustryName, _IndustryNo);
        }
        public IEnumerable<DataRow> BillPrint(string _ServiceType, string _FinancialYear, long? _IndustryTypeID, long? _ZoneID, long? _CircleID, long? _DivisionID, string _IndustryName, string _IndustryNo)
        {
            return db.ExecuteDataSet("Proc_EC_PrintBill", _ServiceType, _FinancialYear, _IndustryTypeID, _ZoneID, _CircleID, _DivisionID, _IndustryName, _IndustryNo);
        }
        public DataSet GetBillDetails(string _ServiceType, long? _BillID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_EC_BillDetails", _ServiceType, _BillID);
        }
        public object GetLastECBillID(string ServiceType, string FinancialYear, long industryID)
        {
            Effluent_WaterChargesRepositry repEff = this.db.ExtRepositoryFor<Effluent_WaterChargesRepositry>();
            return repEff.GetLastECBillID(ServiceType, FinancialYear, industryID);
        }
        public double WaterRate(string _service, int _getLastYear, DateTime _FYStart)
        {
            return db.ExecuteScalarDouble("EC_Call_Function_GetWaterRate", _service, _getLastYear, _FYStart);
        }
        public double EC_GetLastSnctdDschrgSuply(long _IndustryID, string _service, long _CSWID, int _getLastYear, DateTime _FYStart)
        {
            return db.ExecuteScalarDouble("EC_Call_Function_GetSnctdDschrgSuply", _IndustryID, _service, _CSWID, _getLastYear, _FYStart);
        }
        public double EC_GetCalculateSurcharge(double _amount, string _service)
        {
            return db.ExecuteScalarDouble("EC_Call_Function_ECCalculateSurcharge", _amount, _service);
        }
        public bool DeleteIndustryByID(long _IndustryID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_EC_DeleteIndustry", _IndustryID);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }
            return IsExists;
        }

        public bool DeleteCSWRecord(long _ID)
        {
            bool Result = false;
            long Count = db.Repository<EC_SanctionedDischargeSupply>().GetAll().Where(q => q.CanalSpecialWaterID == _ID).ToList().Count();
            if (Count == 0)
            {
                db.Repository<EC_CanalSpecialWater>().Delete(_ID);
                db.Save();
                Result = true;
            }
            return Result;
        }

        #endregion

        #region Refrence Data
        public bool BillSetup_AddNew(EC_PrintBillSertup _Mdl)
        {
            EC_PrintBillSertup mdl = BillSetup_Get();//TODO:Mdl.Source);
            if (mdl != null)
            {
                mdl.EffluentText1 = _Mdl.EffluentText1;
                mdl.EffluentText2 = _Mdl.EffluentText2;

                mdl.CanalText1 = _Mdl.CanalText1;
                mdl.CanalText2 = _Mdl.CanalText2;

                mdl.HelpLineNo = _Mdl.HelpLineNo;

                mdl.ModifiedBy = _Mdl.ModifiedBy;
                mdl.ModifiedDate = _Mdl.ModifiedDate;

                db.Repository<EC_PrintBillSertup>().Update(mdl);
            }
            else
            {
                _Mdl.CreatedBy = (int)_Mdl.ModifiedBy;
                _Mdl.CreatedDate = (DateTime)_Mdl.ModifiedDate;
                db.Repository<EC_PrintBillSertup>().Insert(_Mdl);
            }
            db.Save();

            return true;
        }
        public EC_PrintBillSertup BillSetup_Get()
        {
            return db.Repository<EC_PrintBillSertup>().Query().Get().FirstOrDefault();
        }

        public List<object> TaxType_GetList()
        {
            return db.Repository<EC_PaymentType>().Query().Get().Where(x => x.IsActive == true).ToList()
                .Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
        }

        public string TaxType_Get(int? _ID)
        {
            if (_ID == null)
                return ""
                    ;
            return db.Repository<EC_PaymentType>().FindById(_ID).Name;
        }
        #region Surchrge Amount

        public bool Surcharges_AddNew(EC_SurchargeAmount _Mdl)
        {
            db.Repository<EC_SurchargeAmount>().Insert(_Mdl);
            db.Save();

            return true;
        }

        public object Surcharges_GetLatestRateDetails(string _Source)
        {
            List<EC_SurchargeAmount> lst = db.Repository<EC_SurchargeAmount>().Query().Get()
                .Where(x => x.Source.Equals(_Source)).OrderByDescending(x => x.ID).ToList();
            if (lst != null && lst.Count() > 0)
            {
                return lst.
                    Select(x => new
                    {
                        Amount = x.Amount, //Utility.GetRoundOffValue(x.Amount),
                        Date = Utility.GetFormattedDate(x.Date) +
                            " " + Utility.GetFormattedTime(Convert.ToDateTime(x.Date)),
                        Type = TaxType_Get(x.PaymentTypeID),
                        Attachment = x.Attachment,
                        AttachmentName = x.Attachment

                    }
                    ).ToList().ElementAt(0);
            }

            return null;
        }

        public List<object> Surcharges_GetList(string _Source)
        {
            return db.Repository<EC_SurchargeAmount>().Query().Get()
                .Where(x => x.Source.Equals(_Source)).OrderByDescending(x => x.Date).ToList()
                .Select(x => new
                {
                    ID = x.ID,
                    Date = Utility.GetFormattedDate(x.Date)
                        + " " + Utility.GetFormattedTime(Convert.ToDateTime(x.Date)),
                    Amount = Utility.GetRoundOffValue(x.Amount),
                    Type = TaxType_Get(x.PaymentTypeID),
                    Remarks = x.Description,
                    Attachment = x.Attachment

                }).ToList<object>();
        }
        #endregion

        #region Applicable Taxes
        public bool Taxes_Update(EC_ApplicableTaxes _Mdl)
        {
            EC_ApplicableTaxes mdl = db.Repository<EC_ApplicableTaxes>().FindById(_Mdl.ID);
            mdl.ApplicableTax = _Mdl.ApplicableTax;
            mdl.Description = _Mdl.Description;
            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Mdl.IsActive;
            mdl.Source = _Mdl.Source;
            mdl.Amount = _Mdl.Amount;
            mdl.PaymentTypeID = _Mdl.PaymentTypeID;
            db.Repository<EC_ApplicableTaxes>().Update(mdl);
            db.Save();

            return true;
        }
        public EC_ApplicableTaxes Taxes_Get(string _Name, long? _ID, string _Source)
        {
            return db.Repository<EC_ApplicableTaxes>().Query().Get()
                .Where(x => x.Source.Equals(_Source)
                    &&
                    (string.IsNullOrEmpty(_Name) || x.ApplicableTax.ToUpper().Equals(_Name.ToUpper()))
                    &&
                    (_ID == null || x.ID == _ID)
                    )
                    .FirstOrDefault();
        }
        public bool Taxes_Delete(long _ID)
        {
            db.Repository<EC_ApplicableTaxes>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool Taxes_AddNew(EC_ApplicableTaxes _MdlNew)
        {
            db.Repository<EC_ApplicableTaxes>().Insert(_MdlNew);
            db.Save();
            return true;
        }
        public bool Taxes_AssiciationExists(long _ID)
        {
            //TODO:
            //Check industry type association with Industry and any other table which has such possibilty
            //List<CW_ClosureWork> lstWork = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.WorkTypeID == _ID).ToList();
            //if (lstWork != null && lstWork.Count() > 0)
            //    return true;

            return false;
        }

        public List<object> Taxes_GetList(string _Source)
        {
            return db.Repository<EC_ApplicableTaxes>().Query().Get()
                .Where(x => x.Source.Equals(_Source)).ToList()
                .Select(x => new
                {
                    ID = x.ID,
                    Tax = x.ApplicableTax,
                    TypeID = x.PaymentTypeID,
                    Type = TaxType_Get(x.PaymentTypeID),
                    Amount = x.Amount,
                    AmountS = Utility.GetRoundOffValue(x.Amount),
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToList<object>();
        }
        #endregion

        #region Rate Pe Cusecs

        public bool RatePerCusecs_AddNew(EC_WaterCharges _Mdl)
        {
            db.Repository<EC_WaterCharges>().Insert(_Mdl);
            db.Save();

            return true;
        }

        public object RatePerCusecs_GetLatestRateDetails(string _Source)
        {
            List<EC_WaterCharges> lst = db.Repository<EC_WaterCharges>().Query().Get()
                .Where(x => x.Source.Equals(_Source)).OrderByDescending(x => x.ID).ToList();
            if (lst != null && lst.Count() > 0)
            {
                return lst.
                    Select(x => new
                    {
                        Rate = Utility.GetRoundOffValue(x.Charges),
                        Date = Utility.GetFormattedDate(x.ChargesDate) +
                            " " + Utility.GetFormattedTime(x.ChargesDate),
                        RateNum = x.Charges,
                        Attachment = x.Attachment,
                        AttachmentName = x.Attachment
                    }
                    ).ToList().ElementAt(0);
            }

            return null;
        }

        public List<object> RatePerCusecs_GetList(string _Source)
        {
            return db.Repository<EC_WaterCharges>().Query().Get()
                .Where(x => x.Source.Equals(_Source)).OrderByDescending(x => x.ChargesDate).ToList()
                .Select(x => new
                {
                    ID = x.ID,
                    ChargesDate = Utility.GetFormattedDate(x.ChargesDate) + " " + Utility.GetFormattedTime(x.ChargesDate),
                    Charges = Utility.GetRoundOffValue(x.Charges),
                    Remarks = x.Description,
                    Attachment = x.Attachment
                }).ToList<object>();

        }
        #endregion

        #region Bank
        public bool Bank_Update(EC_Bank _Mdl)
        {
            EC_Bank mdl = db.Repository<EC_Bank>().FindById(_Mdl.ID);
            mdl.Name = _Mdl.Name;
            mdl.Description = _Mdl.Description;
            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Mdl.IsActive;
            db.Repository<EC_Bank>().Update(mdl);
            db.Save();

            return true;
        }
        public EC_Bank Bank_Get(string _Name, long? _ID)
        {
            return db.Repository<EC_Bank>().Query().Get()
                .Where(x =>
                    (string.IsNullOrEmpty(_Name) || x.Name.ToUpper().Equals(_Name.ToUpper()))
                    &&
                    (_ID == null || x.ID == _ID)
                    )
                    .FirstOrDefault();
        }
        public bool Bank_Delete(long _ID)
        {
            db.Repository<EC_Bank>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool Bank_AddNew(EC_Bank _MdlNew)
        {
            db.Repository<EC_Bank>().Insert(_MdlNew);
            db.Save();
            return true;
        }
        public bool Bank_AssiciationExists(long _ID)
        {
            //TODO:
            //Check industry type association with Industry and any other table which has such possibilty
            //List<CW_ClosureWork> lstWork = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.WorkTypeID == _ID).ToList();
            //if (lstWork != null && lstWork.Count() > 0)
            //    return true;

            return false;
        }
        public List<EC_Bank> Bank_GetList()
        {
            return db.Repository<EC_Bank>().Query().Get().ToList();
        }
        #endregion

        #region Discharge Supply Sources
        public bool DSSource_Update(EC_DischargeSupplySource _Mdl)
        {
            EC_DischargeSupplySource mdl = db.Repository<EC_DischargeSupplySource>().FindById(_Mdl.ID);
            mdl.Name = _Mdl.Name;
            mdl.Description = _Mdl.Description;
            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Mdl.IsActive;
            mdl.Source = "BOTH";
            db.Repository<EC_DischargeSupplySource>().Update(mdl);
            db.Save();

            return true;
        }
        public EC_DischargeSupplySource DSSource_Get(string _Name, long? _ID)
        {
            return db.Repository<EC_DischargeSupplySource>().Query().Get()
                .Where(x =>
                    (string.IsNullOrEmpty(_Name) || x.Name.ToUpper().Equals(_Name.ToUpper()))
                    &&
                    (_ID == null || x.ID == _ID)
                    )
                    .FirstOrDefault();
        }
        public bool DSSource_Delete(long _ID)
        {
            db.Repository<EC_DischargeSupplySource>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool DSSource_AddNew(EC_DischargeSupplySource _MdlNew)
        {
            _MdlNew.Source = "BOTH";
            db.Repository<EC_DischargeSupplySource>().Insert(_MdlNew);
            db.Save();
            return true;
        }
        public bool DSSource_AssiciationExists(long _ID)
        {
            //TODO:
            //Check industry type association with Industry and any other table which has such possibilty

            bool qIsExists = db.Repository<EC_EffuentWaterDetails>().GetAll().Any(s => s.DischargeSourceID == _ID);
            if (!qIsExists)
            {
                qIsExists = db.Repository<EC_CanalSpecialWater>().GetAll().Any(s => s.SupplySourceID == _ID);
            }
            return qIsExists;
        }
        public List<EC_DischargeSupplySource> DSSource_GetList()
        {
            return db.Repository<EC_DischargeSupplySource>().Query().Get().ToList();
        }
        #endregion

        #region Industry Type
        public bool IndustryType_Update(EC_IndustryType _Mdl)
        {
            EC_IndustryType mdl = db.Repository<EC_IndustryType>().FindById(_Mdl.ID);
            mdl.Name = _Mdl.Name;
            mdl.Description = _Mdl.Description;
            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Mdl.IsActive;
            db.Repository<EC_IndustryType>().Update(mdl);
            db.Save();

            return true;
        }
        public EC_IndustryType IndustryType_Get(string _Name, long? _ID)
        {
            return db.Repository<EC_IndustryType>().Query().Get()
                .Where(x =>
                    (string.IsNullOrEmpty(_Name) || x.Name.ToUpper().Equals(_Name.ToUpper()))
                    &&
                    (_ID == null || x.ID == _ID)
                    )
                    .FirstOrDefault();
        }
        public bool IndustryType_Delete(long _ID)
        {
            db.Repository<EC_IndustryType>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IndustryType_AddNew(EC_IndustryType _MdlNew)
        {
            db.Repository<EC_IndustryType>().Insert(_MdlNew);
            db.Save();
            return true;
        }

        public bool IndustryType_AssiciationExists(long _ID)
        {
            //TODO:
            //Check industry type association with Industry and any other table which has such possibilty
            List<EC_Industry> lstIndustry = db.Repository<EC_Industry>().Query().Get().Where(x => x.IndustryTypeID == _ID).ToList();
            if (lstIndustry != null && lstIndustry.Count() > 0)
                return true;

            return false;
        }
        public List<EC_IndustryType> IndustryType_GetList()
        {
            return db.Repository<EC_IndustryType>().Query().Get().ToList();
        }
        public List<EC_Bills> Financial_GetList()
        {
            return db.Repository<EC_Bills>().Query().Get().Distinct().OrderByDescending(x => x.FinancialYear).ToList();
        }
        #endregion
        #endregion
    }
}
