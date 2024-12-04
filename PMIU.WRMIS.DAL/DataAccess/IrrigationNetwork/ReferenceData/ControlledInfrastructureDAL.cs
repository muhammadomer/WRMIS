using System;
using System.Collections.Generic;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class ControlledInfrastructureDAL : BaseDAL
    {
        public List<object> GetControlledInfrastructureBySearch(long _ControlInfrastructureID
           , long _ZoneID
           , long _CircleID
           , long _DivisionID
            , string _ControlInfrastructureName
          , Int64 _ControlInfrastructureStatus)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetControlledInfrastructureBySearchCriteria(_ControlInfrastructureID, _ZoneID, _CircleID, _DivisionID, _ControlInfrastructureName, _ControlInfrastructureStatus);

        }
        public List<CO_StructureType> GetAllActiveControlInfrastructureType()
        {
            return db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && (d.Source == "Control Structure1")).ToList();
        }
        /// <summary>
        /// This method return Control Infrastructure details
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>FO_ProtectionInfrastructure</returns>
        public CO_Station GetControlInfrastructureByID(long _ControlInfrastructureID)
        {
            CO_Station qControlInfrastructure = db.Repository<CO_Station>().GetAll().Where(s => s.ID == _ControlInfrastructureID).FirstOrDefault();
            return qControlInfrastructure;
        }
        public bool SaveControlInfrastructure(CO_Station _ControlInfrastructure)
        {
            bool isSaved = false;

            if (_ControlInfrastructure.ID == 0)
                db.Repository<CO_Station>().Insert(_ControlInfrastructure);
            else
                db.Repository<CO_Station>().Update(_ControlInfrastructure);

            db.Save();
            isSaved = true;
            return isSaved;
        }
        public bool IsControlInfrastructureDependencyExists(long _ControlInfrastructureID)
        {
            long ControlInfrastructureTypeID = 0;
            //ControlInfrastructureTypeID = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.ID == 1 || d.ID == 2 || d.ID == 3 || d.ID == 4 || d.ID == 5).SingleOrDefault().ID;
            ControlInfrastructureTypeID = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Source.Equals("Control Structure1")).FirstOrDefault().ID;

            //bool qIsExists = db.Repository<CO_Station>().GetAll().Any(s => s.ID == _ControlInfrastructureID);
            //if (!qIsExists)
            //{
            bool qIsExists = db.Repository<CO_StructureTechPara>().GetAll().Any(s => s.ID == _ControlInfrastructureID);
            //}
            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_StructureGauge>().GetAll().Any(s => s.ID == _ControlInfrastructureID);
            }
            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(s => s.StructureTypeID == ControlInfrastructureTypeID && s.StructureID == _ControlInfrastructureID);
            }

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(s => s.StructureTypeID == ControlInfrastructureTypeID && s.StructureID == _ControlInfrastructureID);
            }
            return qIsExists;
        }
        public bool DeleteControlInfrastructure(long _ControlInfrastructureID)
        {
            db.Repository<CO_Station>().Delete(_ControlInfrastructureID);
            db.Save();
            return true;
        }
        public CO_StructureType GetControlInfrastructureTypeByID(long _ID)
        {
            CO_StructureType qStructureType = db.Repository<CO_StructureType>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qStructureType;
        }
        /// <summary>
        /// This function return Control Infrastructure Irrigation Boundaries
        /// Created on: 03-10-2016
        /// </summary>
        /// <param name="_ControlInfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByControlInfrastructureID(long _ControlInfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();

            return repInfrastructureRepository.GetIrrigationBoundariesByInfrastructureID(_ControlInfrastructureID, GetStructureTypeIDByControlInfrastructureID(_ControlInfrastructureID));
        }
        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 03-10-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public long GetStructureTypeIDByControlInfrastructureID(long _ControlInfrastructureID)
        {
            long? InfrastructureTypeID = null;
            InfrastructureTypeID = db.Repository<CO_Station>().GetAll().Where(d => d.ID == _ControlInfrastructureID).Single().StructureTypeID;

            return (long)InfrastructureTypeID;
        }
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
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 03-10-2016
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
        /// <summary>
        /// This function returns InfrastructureID Administrative Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByControlInfrastructureID(long _ControlInfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetAdministrativeBoundariesByInfrastructureID(_ControlInfrastructureID, GetStructureTypeIDByControlInfrastructureID(_ControlInfrastructureID));
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            db.Repository<FO_StructureAdminBoundaries>().Delete(_AdministrativeBoundaries);
            db.Save();
            return true;
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
        public bool IsVillageExists(FO_StructureAdminBoundaries _AdminBoundary)
        {

            bool qIsVillageExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(i => i.StructureID == _AdminBoundary.StructureID.Value
                 && i.StructureTypeID == _AdminBoundary.StructureTypeID.Value
                 && i.VillageID == _AdminBoundary.VillageID.Value
                 && i.PoliceStationID == _AdminBoundary.PoliceStationID.Value
                 && (i.ID != _AdminBoundary.ID || _AdminBoundary.ID == 0));

            return qIsVillageExists;

        }

        public List<object> GetGaugesByID(long _InfrastructureID)
        {
            InfrastructureRepository repInfrastructureRepository = this.db.ExtRepositoryFor<InfrastructureRepository>();
            return repInfrastructureRepository.GetGaugesByControlInfrastructureID(_InfrastructureID);
        }
        /// <summary>
        /// This function delete Control Infrastructur Gauges
        /// Created on 04-10-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public bool DeleteGaugesByControlInfrastructure(long _ID)
        {
            db.Repository<CO_StructureGauge>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool SaveControlInfrastructureGauges(CO_StructureGauge _StructureGauge)
        {
            bool isSaved = false;

            if (_StructureGauge.ID == 0)
                db.Repository<CO_StructureGauge>().Insert(_StructureGauge);
            else
                db.Repository<CO_StructureGauge>().Update(_StructureGauge);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        /// <summary>
        /// This function checks if duplicate StructureTechPara exists.
        /// Created on: 05-10-2016
        /// </summary>
        /// <returns>bool<object></returns>
        public bool IsRecordExists(long _ControlInfrastructureID)
        {
            bool IsRecordExists = false;

            bool IsDrainRecordExists = db.Repository<CO_StructureTechPara>().GetAll().Any(x => x.StationID == _ControlInfrastructureID);
            IsRecordExists = IsDrainRecordExists;
            return IsRecordExists;
        }
        /// <summary>
        /// This function retun StructureTechPara addition success along with message
        /// Created on 05-10-2016
        /// </summary>
        /// <param name="_Channel"></param>
        /// <returns></returns>
        public bool SaveStructureTechPara(CO_StructureTechPara _StructureTechPara)
        {
            bool isSaved = false;

            if (_StructureTechPara.ID == 0)
            {
                db.Repository<CO_StructureTechPara>().Insert(_StructureTechPara);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<CO_StructureTechPara>().Update(_StructureTechPara);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        public CO_StructureTechPara GetStructureTechParaeByID(long _StructureTechParaID)
        {
            CO_StructureTechPara qControlInfrastructure = db.Repository<CO_StructureTechPara>().GetAll().Where(s => s.StationID == _StructureTechParaID).FirstOrDefault();
            return qControlInfrastructure;
        }


    }
}
