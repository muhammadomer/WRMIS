using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class DrainDAL : BaseDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function will save Drain data.
        /// Created On 23-09-2016
        /// <summary>
        public bool AddDrainData(FO_Drain _DrainObj)
        {
            bool Result = false;
            try
            {
                if (_DrainObj.ID == 0)
                {
                    db.Repository<FO_Drain>().Insert(_DrainObj);
                }
                else
                {
                    db.Repository<FO_Drain>().Update(_DrainObj);
                }
                
                db.Save();
                Result = true;
                
            }
            catch (Exception)
            {
               
                throw;
            }
            return Result;
        }

        /// <summary>
        /// This function fetch drain data by ID.
        /// Created On 23-09-2016
        /// <summary>
        
        public FO_Drain GetExistingRecord(long DrainID)
        {
            try
            {
                return db.Repository<FO_Drain>().GetAll().Where(x => x.ID == DrainID).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        /// <summary>
        /// This function delete drain data by ID.
        /// Created On 23-09-2016
        /// <summary>

        public bool DeleteDrainDataByID(long DrainID)
        {
            bool Result = false;
            try
            {
                db.Repository<FO_Drain>().Delete(DrainID);
                db.Save();
                Result = true;

            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }

        /// <summary>
        /// This function Gets Drain Data By ID.
        /// Created On 27-09-2016
        /// <summary>

        public dynamic GetDrainDataByID(long _DrainID)
        {

            try
            {
                return db.Repository<FO_Drain>().GetAll().Where(x => x.ID == _DrainID).FirstOrDefault();

            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// This function Gets Drain Search Criteria.
        /// Created On 26-09-2016
        /// <summary>

        public List<object> GetDrainSearchCriteria(long _DrainID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, string _Name)
        {
          
            try
            {
                return db.ExtRepositoryFor<DrainRepository>().GetDrainBySearchCriteria(_DrainID ,_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _Name);

            }
            catch (Exception)
            {

                throw;
            }
           
        }

        /// <summary>
        /// This function returns Drain Irrigation Boundaries.
        /// Created on: 28-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByDrainID(long _DrainID)
        {
            return db.ExtRepositoryFor<DrainRepository>().GetIrrigationBoundariesByDrainID(_DrainID, GetStructureTypeIDByDrainID(_DrainID));
        }

        /// <summary>
        /// This function returns Drain Administrative Boundaries.
        /// Created on: 28-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByDrainID(long _DrainID)
        {
           
            return db.ExtRepositoryFor<DrainRepository>().GetAdministrativeBoundariesByDrainID(_DrainID, GetStructureTypeIDByDrainID(_DrainID));
        }

        /// <summary>
        /// This function returns Drain Type ID.
        /// Created on: 28-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public long GetStructureTypeIDByDrainID(long _DrainID)
        {
            long? InfrastructureTypeID = null;
            InfrastructureTypeID = db.Repository<FO_Drain>().GetAll().Where(d => d.ID == _DrainID).Single().DrainTypeID;

            return (long)InfrastructureTypeID;
        }

        /// <summary>
        /// This function returns Circle List for drain domain.
        /// Created on: 28-09-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>List<object></returns>
        public List<dynamic> GetCirclesforDrainDomain(long _ZoneID)
        {
            return db.ExtRepositoryFor<DrainRepository>().GetCirclesforDrainDomain(_ZoneID);
        }


        /// <summary>
        /// This function returns List of Rivers.
        /// Created on: 29-09-2016
        /// </summary>
        /// <returns>List<object></returns>
        public List<dynamic> GetRiverNamesByStructureType()
        {
            return db.ExtRepositoryFor<DrainRepository>().GetRiverNamesByStructureType();
        }

        /// <summary>
        /// This function returns List of Drains against ZoneID.
        /// Created on: 29-09-2016
        /// </summary>
        /// <returns>List<object></returns>
        public List<dynamic> GetDrainsByID(long _ID, int Type)
        {

            return db.ExtRepositoryFor<DrainRepository>().GetDrainNamesByID(_ID, Type);
        }


        /// <summary>
        /// This function returns saves Drain outfall.
        /// Created on: 29-09-2016
        /// </summary>
        /// <returns>bool<object></returns>
        public bool SaveDrainOutfallData(FO_DrainOutfall DrainOutfallObj)
        {
            bool IsSaved = false;
            try
            {
                if (DrainOutfallObj.ID == 0)
                {
                    db.Repository<FO_DrainOutfall>().Insert(DrainOutfallObj);
                    
                    
                }
                else
                {
                    db.Repository<FO_DrainOutfall>().Update(DrainOutfallObj);   
                }
                IsSaved = true;
                db.Save();
            }
            catch (Exception)
            {

                throw;
            }
           
            return IsSaved;
        }

        /// <summary>
        /// This function Checks if dependency exists for Drain records.
        /// Created on: 29-09-2016
        /// </summary>
        /// <returns>bool<object></returns>
        public bool IsDrainDependencyExists(long _DrainID)
        {
            bool IsDependencyExists = false;
            try
            {
                bool IsStructureIrrigationBoundriesExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(x => x.StructureID == _DrainID);
                bool IsDrainOutfallExists = db.Repository<FO_DrainOutfall>().GetAll().Any(x=>x.DrainID == _DrainID);
                if (IsStructureIrrigationBoundriesExists || IsDrainOutfallExists)
                {
                    IsDependencyExists = true;
                }
                
            }
            catch (Exception)
            {

                throw;
            }

            return IsDependencyExists;
        }

        /// <summary>
        /// This function checks if duplicate outfall exists.
        /// Created on: 29-09-2016
        /// </summary>
        /// <returns>bool<object></returns>
        public bool IsOutfallRecordExists(long _DrainID)
        {
            bool IsRecordExists = false;
            try
            {
                bool IsDrainRecordExists = db.Repository<FO_DrainOutfall>().GetAll().Any(x => x.DrainID == _DrainID);

                IsRecordExists = IsDrainRecordExists;
                

            }
            catch (Exception)
            {

                throw;
            }

            return IsRecordExists;
        }

        /// <summary>
        /// This function outfall drain record.
        /// Created on: 29-09-2016
        /// </summary>
        public FO_DrainOutfall GetDrainOutfallByDrainID(long _DrainID)
        {
            try
            {
                FO_DrainOutfall OutfallData = db.Repository<FO_DrainOutfall>().Query().Get().Where(x => x.DrainID == _DrainID).FirstOrDefault();
                return OutfallData;
            }
            catch (Exception)
            {

                throw;
            }

           

        }

        /// <summary>
        /// This function returns selected IDs for Zone,Circle,Division.
        /// Created on: 29-09-2016
        /// </summary>
        public object GetSelectedDropdownsHeirarchyIDs(long? _SubDivisionID)
        {
            try
            {
                
                return db.ExtRepositoryFor<DrainRepository>().GetSelectedDropdownsHeirarchyIDs(_SubDivisionID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function check Section existance in Irrigation Boundaries
        /// Created on 04-10-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsSectionExists(FO_StructureIrrigationBoundaries _IrrigationBoundary)
        {
            bool qIsDivisionExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(i => i.SectionID == _IrrigationBoundary.SectionID
                       && i.StructureID == _IrrigationBoundary.StructureID
                       && i.StructureTypeID == _IrrigationBoundary.StructureTypeID.Value
                       && (i.ID != _IrrigationBoundary.ID || _IrrigationBoundary.ID == 0));
            return qIsDivisionExists;
        }

        
    }
}
