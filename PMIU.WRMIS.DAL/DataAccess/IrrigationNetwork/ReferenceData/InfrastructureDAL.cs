using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class InfrastructureDAL : BaseDAL
    {

        /// <summary>
        /// This function return all infrastructes type.
        /// Created On 29-08-2016
        /// </summary>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetAllInfrastructureType()
        {
            return db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && (d.Name == "Bund / Guide Bund" || d.Name == "Spur / Stud" || d.Name == "Wall")).ToList();
        }

        /// <summary>
        /// This function return all active infrastructes type.
        /// Created On 29-08-2016
        /// </summary>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetAllActiveInfrastructureType()
        {
            return db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && (d.Name == "Bund / Guide Bund" || d.Name == "Spur / Stud" || d.Name == "Wall")).ToList();
        }


        public List<object> GetProtectionInfrastructureBySearchCriteria(long _InfrastructureID
            , long _ZoneID
            , long _CircleID
            , long _DivisionID
            , Int16 _InfrastructureTypeID
            , string _InfrastructureName
            , Int64 _InfrastructureStatus)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetInfrastructureBySearchCriteria(_InfrastructureID
                , _ZoneID
                , _CircleID
                , _DivisionID
                , _InfrastructureTypeID
                , _InfrastructureName
                , _InfrastructureStatus);
        }

        /// <summary>
        /// This method return Protection Infrastructure details
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>FO_ProtectionInfrastructure</returns>
        public FO_ProtectionInfrastructure GetInfrastructureByID(long _InfrastructureID)
        {
            FO_ProtectionInfrastructure qInfrastructure = db.Repository<FO_ProtectionInfrastructure>().GetAll().Where(s => s.ID == _InfrastructureID).FirstOrDefault();
            return qInfrastructure;
        }


        /// <summary>
        /// This function retun ProtectionInfrastructure addition success along with message
        /// Created on 02-Sep-2016
        /// </summary>
        /// <param name="_Infrastructure"></param>
        /// <returns></returns>
        public bool SaveInfrastructure(FO_ProtectionInfrastructure _Infrastructure)
        {
            bool isSaved = false;

            if (_Infrastructure.ID == 0)
            {
                db.Repository<FO_ProtectionInfrastructure>().Insert(_Infrastructure);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_ProtectionInfrastructure>().Update(_Infrastructure);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Protection Infrastructure with the provided ID.
        /// Created On 05-09-2016.
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructure(long _InfrastructureID)
        {
            db.Repository<FO_ProtectionInfrastructure>().Delete(_InfrastructureID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function checks in all related tables for given Infrastructure ID.
        /// Created On 05-09-2016.
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>bool</returns>
        public bool IsInfrastructureIDExists(long _InfrastructureID)
        {

            long InfrastructureTypeID = 0;

            InfrastructureTypeID = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Source.Equals("Protection Infrastructure")).FirstOrDefault().ID;

            bool qIsExists = db.Repository<FO_InfrastructureParent>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_InfrastructureBreachingSection>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            }

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            }

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            }

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_InfrastructureRepresentative>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            }

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_InfrastructureStoneStock>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            }

            return qIsExists;
        }

        /// <summary>
        /// This method return Protection Infrastructure Type Information Using ID
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>CO_StructureType</returns>
        public CO_StructureType GetInfrastructureTypeByID(long _ID)
        {
            CO_StructureType qStructureType = db.Repository<CO_StructureType>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qStructureType;
        }

        #region "Infrastructure Physical Location"
        /// <summary>
        /// This function return Infrastructure Irrigation Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();

            return repInfrastructureRepository.GetIrrigationBoundariesByInfrastructureID(_InfrastructureID, GetStructureTypeIDByInfrastructureID(_InfrastructureID));
        }

        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public long GetStructureTypeIDByInfrastructureID(long _InfrastructureID)
        {
            long? InfrastructureTypeID = null;
            InfrastructureTypeID = db.Repository<FO_ProtectionInfrastructure>().GetAll().Where(d => d.ID == _InfrastructureID).Single().InfrastructureTypeID;

            return (long)InfrastructureTypeID;
        }

        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsDivisionExists(FO_StructureIrrigationBoundaries _IrrigationBoundary)
        {
            bool qIsDivisionExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(i => i.DivisionID == _IrrigationBoundary.DivisionID
                       && i.StructureID == _IrrigationBoundary.StructureID
                       && i.StructureTypeID == _IrrigationBoundary.StructureTypeID.Value
                       && (i.ID != _IrrigationBoundary.ID || _IrrigationBoundary.ID == 0));
            return qIsDivisionExists;
        }

        ///// <summary>
        ///// This function check SectionRD existance in Irrigation Boundaries
        ///// Created on 15-09-2016
        ///// </summary>
        ///// <param name="_IrrigationBoundary"></param>
        ///// <returns>bool</returns>
        //public bool IsSectionRDsExists(FO_InfrastructureIrrigationBoundaries _IrrigationBoundary)
        //{
        //  bool qIsSectionRDsExists = db.Repository<FO_InfrastructureIrrigationBoundaries>().GetAll().Any(i => i.SectionRD == _IrrigationBoundary.SectionRD
        //              && i.ProtectionInfrastructureID == _IrrigationBoundary.ProtectionInfrastructureID
        //              && (i.ID != _IrrigationBoundary.ID || _IrrigationBoundary.ID == 0));

        //  return qIsSectionRDsExists;

        //}

        public bool SaveIrrigationBoundaries(FO_StructureIrrigationBoundaries _IrrigationBoundaries)
        {
            if (_IrrigationBoundaries.ID == 0)
                db.Repository<FO_StructureIrrigationBoundaries>().Insert(_IrrigationBoundaries);
            else
                db.Repository<FO_StructureIrrigationBoundaries>().Update(_IrrigationBoundaries);

            db.Save();
            return true;

        }
        public bool DeleteIrrigationBoundaries(long _IrrigationBoundariesID)
        {
            db.Repository<FO_StructureIrrigationBoundaries>().Delete(_IrrigationBoundariesID);
            db.Save();
            return true;
        }
        /// <summary>
        /// This function returns InfrastructureID Administrative Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetAdministrativeBoundariesByInfrastructureID(_InfrastructureID, GetStructureTypeIDByInfrastructureID(_InfrastructureID));
        }
        /// <summary>
        /// This function returns Infrastructure Divisions
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetDivisionsByInfrastructureID(_InfrastructureID, GetStructureTypeIDByInfrastructureID(_InfrastructureID));

        }
        /// <summary>
        /// This function returns Infrastructure Districts
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetDistrictsByInfrastructureID(_InfrastructureID, GetStructureTypeIDByInfrastructureID(_InfrastructureID));

        }
        /// <summary>
        /// This function check Village existance in Admin Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_AdminBoundary"></param>
        /// <returns>bool</returns>
        public bool IsVillageExists(FO_StructureAdminBoundaries _AdminBoundary)
        {

            bool qIsVillageExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(i => i.StructureID == _AdminBoundary.StructureID.Value
                 && i.StructureTypeID == _AdminBoundary.StructureTypeID.Value
                 && i.VillageID == _AdminBoundary.VillageID.Value
                 && i.PoliceStationID == _AdminBoundary.PoliceStationID.Value
                //&& i.ChannelSide.Contains(_AdminBoundary.ChannelSide)
                 && (i.ID != _AdminBoundary.ID || _AdminBoundary.ID == 0));

            return qIsVillageExists;

        }
        public bool SaveAdministrativeBoundaries(FO_StructureAdminBoundaries _AdministrativeBoundaries)
        {
            if (_AdministrativeBoundaries.ID == 0)
                db.Repository<FO_StructureAdminBoundaries>().Insert(_AdministrativeBoundaries);
            else
                db.Repository<FO_StructureAdminBoundaries>().Update(_AdministrativeBoundaries);

            db.Save();
            return true;
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            db.Repository<FO_StructureAdminBoundaries>().Delete(_AdministrativeBoundaries);
            db.Save();
            return true;
        }
        public FO_StructureIrrigationBoundaries GetIrrigationBoundaryByID(long _IrrigationBoundariesID)
        {
            FO_StructureIrrigationBoundaries irrigationBoundary = (from i in db.Repository<FO_StructureIrrigationBoundaries>().Query().Get()
                                                                   where i.ID == _IrrigationBoundariesID
                                                                   select i).FirstOrDefault();

            return irrigationBoundary;
        }
        public bool IsIrrigationBoundariesDependencyExists(long _InfrastructureID)
        {
            bool isDependanceExists = false;
            try
            {
                //// Check Irrigation boundary exists in Gauge Information
                //bool isReachExists = db.Repository<CO_ChannelReach>().GetAll().Any(g => g.ChannelID == _ChannelID);
                //// Check Irrigation boundary exists in Channel parent feeder
                //bool isParentFeederExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(g => g.ChannelID == _ChannelID);
                //// Check Irrigation boundary exists in Channel outlets
                //bool isOutletExists = db.Repository<CO_ChannelOutlets>().GetAll().Any(g => g.ChannelID == _ChannelID);

                //if (isReachExists == true || isParentFeederExists == true || isOutletExists == true)
                //  isDependanceExists = true;

                return isDependanceExists;
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.Business);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// This function return List of all active protection infrastructure 
        /// </summary>
        /// <typeparam name="FO_ProtectionInfrastructure"></typeparam>
        /// <returns></returns>
        /// <created>tom,9/16/2016</created>
        /// <changed>tom,9/16/2016</changed>
        public List<FO_ProtectionInfrastructure> GetAllActiveProtectionInfrastructure()
        {
            return db.Repository<FO_ProtectionInfrastructure>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public bool SaveInfrastructureParent(FO_InfrastructureParent _InfrastructureParent)
        {

            db.Repository<FO_InfrastructureParent>().Insert(_InfrastructureParent);
            db.Save();
            return true;
        }
        public bool UpdateInfrastructureParent(FO_InfrastructureParent _InfrastructureParent)
        {
            FO_InfrastructureParent UpdateParent = db.Repository<FO_InfrastructureParent>().FindById(_InfrastructureParent.ID);
            UpdateParent.ProtectionInfrastructureID = _InfrastructureParent.ProtectionInfrastructureID;
            UpdateParent.StructureTypeID = _InfrastructureParent.StructureTypeID;
            UpdateParent.StructureID = _InfrastructureParent.StructureID;
            UpdateParent.DivisionID = _InfrastructureParent.DivisionID;
            UpdateParent.OfftakeRD = _InfrastructureParent.OfftakeRD;
            UpdateParent.CreatedDate = DateTime.Now;
            UpdateParent.CreatedBy = _InfrastructureParent.CreatedBy;
            UpdateParent.ModifiedDate = DateTime.Now;
            UpdateParent.ModifiedBy = _InfrastructureParent.ModifiedBy;
            db.Repository<FO_InfrastructureParent>().Update(UpdateParent);
            db.Save();

            return true;
        }

        public bool IsInfrastructureParentExists(long _InfrastructureID)
        {
            bool qIsExists = db.Repository<FO_InfrastructureParent>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            return qIsExists;
        }
        public FO_InfrastructureParent GetInfrastructureParentByID(long _InfrastructureID)
        {

            FO_InfrastructureParent qInfrastructure = db.Repository<FO_InfrastructureParent>().GetAll().Where(s => s.ProtectionInfrastructureID == _InfrastructureID).FirstOrDefault();
            return qInfrastructure;
        }


        #region "Infrastructure Stone Stock"

        /// <summary>
        /// This method return Protection Infrastructure Stone stock by _InfrastructureID
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetStoneStockByInfrastructureID(long _InfrastructureID)
        {

            List<object> lstStoneStock = null;
            lstStoneStock = (from i in db.Repository<FO_InfrastructureStoneStock>().Query().Get()
                             where i.ProtectionInfrastructureID == _InfrastructureID
                             select new
                             {
                                 ID = i.ID,
                                 ProtectionInfrastructureID = i.ProtectionInfrastructureID,
                                 FromRD = i.FromRD,
                                 ToRD = i.ToRD,
                                 SanctionedLimit = i.SanctionedLimit,
                                 CreatedBy = i.CreatedBy,
                                 CreatedDate = i.CreatedDate,
                             }).ToList()
                             .Select(i => new
                                {
                                    ID = i.ID,
                                    ProtectionInfrastructureID = i.ProtectionInfrastructureID,
                                    FromRDTotal = i.FromRD,
                                    ToRDTotal = i.ToRD,
                                    FromRD = Calculations.GetRDText(Convert.ToInt64(i.FromRD)),
                                    ToRD = Calculations.GetRDText(Convert.ToInt64(i.ToRD)),
                                    SanctionedLimit = i.SanctionedLimit,
                                    CreatedBy = i.CreatedBy,
                                    CreatedDate = i.CreatedDate,
                                }).ToList<object>();

            return lstStoneStock;
        }

        /// <summary>
        /// This function check FromRD ToRD existance in Stone Stock
        /// Created on 21-09-2016
        /// </summary>
        /// <param name="_InfrastructureStoneStock"></param>
        /// <returns>bool</returns>
        public bool IsStoneStockFromRDToRDExits(FO_InfrastructureStoneStock _InfrastructureStoneStock)
        {
            bool qIsStoneStockFromRDToRDExits = db.Repository<FO_InfrastructureStoneStock>().GetAll().Any(i => i.FromRD == _InfrastructureStoneStock.FromRD
                       && i.ToRD == _InfrastructureStoneStock.ToRD
                       && (i.ID != _InfrastructureStoneStock.ID || _InfrastructureStoneStock.ID == 0));
            return qIsStoneStockFromRDToRDExits;
        }

        /// <summary>
        /// This function retun Protection Infrastructure Stone stock addition success along with message
        /// Created on 21-Sep-2016
        /// </summary>
        /// <param name="_InfrastructureStoneStock"></param>
        /// <returns>bool</returns>
        public bool SaveInfrastructureStoneStock(FO_InfrastructureStoneStock _InfrastructureStoneStock)
        {
            bool isSaved = false;

            if (_InfrastructureStoneStock.ID == 0)
            {
                db.Repository<FO_InfrastructureStoneStock>().Insert(_InfrastructureStoneStock);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_InfrastructureStoneStock>().Update(_InfrastructureStoneStock);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Protection Infrastructure Stone stock with the provided ID.
        /// Created On 21-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructureStoneStockByID(long _ID)
        {
            db.Repository<FO_InfrastructureStoneStock>().Delete(_ID);
            db.Save();

            return true;
        }

        #endregion

        #region "Infrastructure Public Representative"

        /// <summary>
        /// This method return Protection Infrastructure Representative by _InfrastructureID
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetRepresentativeByInfrastructureID(long _InfrastructureID)
        {
            List<object> lstRepresentative = null;
            lstRepresentative = (from i in db.Repository<FO_InfrastructureRepresentative>().Query().Get()
                                 where i.ProtectionInfrastructureID == _InfrastructureID
                                 select new
                                 {
                                     ID = i.ID,
                                     ProtectionInfrastructureID = i.ProtectionInfrastructureID,
                                     Name = i.Name,
                                     Details = i.Details,
                                     ContactNumber = i.ContactNumber,
                                     CreatedBy = i.CreatedBy,
                                     CreatedDate = i.CreatedDate,
                                 }).ToList()
                             .Select(i => new
                             {
                                 ID = i.ID,
                                 ProtectionInfrastructureID = i.ProtectionInfrastructureID,
                                 Name = i.Name,
                                 Details = i.Details,
                                 ContactNumber = i.ContactNumber,
                                 CreatedBy = i.CreatedBy,
                                 CreatedDate = i.CreatedDate,
                             }).ToList<object>();

            return lstRepresentative;
        }

        ///// <summary>
        ///// This function check FromRD ToRD existance in Stone Stock
        ///// Created on 21-09-2016
        ///// </summary>
        ///// <param name="_InfrastructureStoneStock"></param>
        ///// <returns>bool</returns>
        //public bool IsStoneStockFromRDToRDExits(FO_InfrastructureStoneStock _InfrastructureStoneStock)
        //{
        //  InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
        //  return dalInfrastructure.IsStoneStockFromRDToRDExits(_InfrastructureStoneStock);
        //}

        /// <summary>
        /// This function retun Protection Infrastructure Representative addition success along with message
        /// Created on 21-Sep-2016
        /// </summary>
        /// <param name="_InfrastructureRepresentative"></param>
        /// <returns>bool</returns>
        public bool SaveInfrastructureRepresentative(FO_InfrastructureRepresentative _InfrastructureRepresentative)
        {
            bool isSaved = false;

            if (_InfrastructureRepresentative.ID == 0)
            {
                db.Repository<FO_InfrastructureRepresentative>().Insert(_InfrastructureRepresentative);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_InfrastructureRepresentative>().Update(_InfrastructureRepresentative);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Protection Infrastructure Stone stock with the provided ID.
        /// Created On 21-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructureRepresentative(long _ID)
        {
            db.Repository<FO_InfrastructureRepresentative>().Delete(_ID);
            db.Save();

            return true;
        }

        #endregion

        #region "Define Infrastructure BreachingSection"

        public List<object> GetBreachingSection(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetInfrastructureBreachingSection(_InfrastructureID);
        }

        public bool SaveInfrastructureBreachingSection(FO_InfrastructureBreachingSection _BreachingSection)
        {
            bool isSaved = false;

            if (_BreachingSection.ID == 0)
                db.Repository<FO_InfrastructureBreachingSection>().Insert(_BreachingSection);
            else
                db.Repository<FO_InfrastructureBreachingSection>().Update(_BreachingSection);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        /// <summary>
        /// This function delete Infrastructur Breaching Section
        /// Created on 21-09-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public bool DeleteInfrastructureBreachingSection(long _ID)
        {
            db.Repository<FO_InfrastructureBreachingSection>().Delete(_ID);
            db.Save();

            return true;
        }
        /// <summary>
        /// This method return Breaching Sectione details
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>FO_InfrastructureBreachingSection</returns>
        public FO_InfrastructureBreachingSection GetBreachingSectioneByID(long _BreachingSectionID)
        {
            FO_InfrastructureBreachingSection qBreachingSectione = db.Repository<FO_InfrastructureBreachingSection>().GetAll().Where(s => s.ID == _BreachingSectionID).FirstOrDefault();
            return qBreachingSectione;
        }

        public List<object> GetExplosivesMatetial(long _BreachingSectioneID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetBreachingSectionExplosives(_BreachingSectioneID);
        }
        /// <summary>
        /// This function Save Infrastructur Breaching Section Explosive Detail
        /// Created on 22-09-2016
        /// </summary>
        /// <returns
        public bool SaveExplosivesMatetial(FO_BreachingSectionExplosives _ExplosivesMatetial)
        {
            bool isSaved = false;

            if (_ExplosivesMatetial.ID == 0)
                db.Repository<FO_BreachingSectionExplosives>().Insert(_ExplosivesMatetial);
            else
                db.Repository<FO_BreachingSectionExplosives>().Update(_ExplosivesMatetial);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        /// <summary>
        /// This function delete Infrastructur Breaching Section Explosive Detail
        /// Created on 22-09-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public bool DeleteExplosivesMatetial(long _ID)
        {
            db.Repository<FO_BreachingSectionExplosives>().Delete(_ID);
            db.Save();

            return true;
        }
        #endregion "Define Infrastructure BreachingSection"

        public List<CO_Division> DDLDivisionsForDFAndIrrigation(long _CircleID)
        {
            List<CO_Division> lstDivision = db.Repository<CO_Division>().GetAll().Where(d => (d.CircleID == _CircleID || _CircleID == -1) &&
                (d.DomainID == 1 || d.DomainID == 4)).OrderBy(d => d.Name).ToList<CO_Division>();

            return lstDivision;
        }

        /// <summary>
        /// This function returns Infrastructure Parent by InfrastructureID 
        /// Created on: 26-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public object GetInfrastructureParentNameByID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetInfrastructureParentNameByID(_InfrastructureID);
        }

        public bool IsInfrastructureExists(FO_ProtectionInfrastructure _Infrastructure)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.IsInfrastructureExists(_Infrastructure);
        }

        public List<FO_ExplosivesCustody> GetInfrastructureCustody()
        {
            return db.Repository<FO_ExplosivesCustody>().GetAll().ToList<FO_ExplosivesCustody>();
        }

        /// <summary>
        /// This function returns Structure Districts
        /// Created on: 24-02-2017
        /// </summary>
        /// <param name="_StructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByStructureID(long _StructureID, bool? _IsActive = null)
        {
            InfrastructureRepository repInfrastructureRepository = db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetDistrictsByStructureID(_StructureID, _IsActive);

        }

        public bool DeleteGaugesByID(long _ID)
        {
            db.Repository<FO_FloodGauge>().Delete(_ID);
            db.Save();
            return true;
        }
        public bool SaveProtectionInfrastructureGauges(FO_FloodGauge _StructureGauge)
        {
            bool isSaved = false;

            if (_StructureGauge.ID == 0)
                db.Repository<FO_FloodGauge>().Insert(_StructureGauge);
            else
                db.Repository<FO_FloodGauge>().Update(_StructureGauge);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public List<object> GetGaugesByProtectionInfrastructureID(long ID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetGaugesByProtectionInfrastructureID(ID);
        }
        public List<FO_FloodGaugeType> GetGaugeTypesFloodBund()
        {
            List<FO_FloodGaugeType> lstGaugeTypes = db.Repository<FO_FloodGaugeType>().GetAll().OrderBy(z => z.Name).ToList<FO_FloodGaugeType>();

            return lstGaugeTypes;
        }
        public bool IsFloodGaugeExists(FO_FloodGauge _FloodGauge)
        {
            bool isFloodGaugeExists = db.Repository<FO_FloodGauge>().GetAll().Any(i => i.GaugeTypeID == _FloodGauge.GaugeTypeID && i.GaugeRD == _FloodGauge.GaugeRD
                             && (i.ID != _FloodGauge.ID || _FloodGauge.ID == 0));
            return isFloodGaugeExists;
        }

    }
}
