using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;


namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class DrainBLL : BaseBLL
    {
        DrainDAL dalDrain = new DrainDAL();
        /// <summary>
        /// This function will save drain data.
        /// Created On 23-09-2016.
        /// <summary>
        public bool AddDrainData(FO_Drain _ObjDrain)
        {
            try
            {
                return dalDrain.AddDrainData(_ObjDrain);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        /// <summary>
        /// This function will Fetch drain data by ID.
        /// Created On 23-09-2016.
        /// <summary>

        public FO_Drain GetExisitngRecord(long DrainID)
        {
            try
            {
                return dalDrain.GetExistingRecord(DrainID);
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
            try
            {
                return dalDrain.DeleteDrainDataByID(DrainID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function gets drain data by ID.
        /// Created On 27-09-2016
        /// <summary>
        public dynamic GetDrainDataByID(long _DrainID)
        {
            try
            {
                return dalDrain.GetDrainDataByID(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function gets drain search criteria.
        /// Created On 26-09-2016
        /// <summary>
        public List<object> GetDrainSearchCriteria(long _DrainID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, string _DrainName)
        {
            try
            {
                return dalDrain.GetDrainSearchCriteria(_DrainID, _ZoneID, _CircleID, _DivisionID, _SubDivisionID, _DrainName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function gets Irrigation Boundries by Drain ID.
        /// Created On 28-09-2016
        /// <summary>
        public List<object> GetIrrigationBoundariesByDrainID(long _DrainID)
        {
            try
            {
                return dalDrain.GetIrrigationBoundariesByDrainID(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }
           
        }


        /// <summary>
        /// This function gets Administrative Boundries by Drain ID.
        /// Created On 28-09-2016
        /// <summary>
        public List<object> GetAdministrativeBoundriesByDrainID(long _DrainID)
        {
            try
            {
                return dalDrain.GetAdministrativeBoundariesByDrainID(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// This function gets Circle List for Drain Domain.
        /// Created On 28-09-2016
        /// <summary>
        public List<dynamic> GetCircleForDrainDomain(long _ZoneID)
        {
            try
            {
                return dalDrain.GetCirclesforDrainDomain(_ZoneID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function gets strycture type against drain ID.
        /// Created On 28-09-2016
        /// <summary>
        public long GetDrainTypeByDrainID(long _DrainID)
        {
            try
            {
                return dalDrain.GetStructureTypeIDByDrainID(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function gets Rivers Names.
        /// Created On 28-09-2016
        /// <summary>
        public List<dynamic> GetRiversNameByStructureType()
        {
            try
            {
               
                return dalDrain.GetRiverNamesByStructureType();
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function gets Drains data.
        /// Created On 28-09-2016
        /// <summary>
        public List<dynamic> GetDrainsByID(long _ID, int Type)
        {
            try
            {
              
                return dalDrain.GetDrainsByID(_ID, Type);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function saves outfall drain data.
        /// Created On 28-09-2016
        /// <summary>
        public bool SaveDrainOutfallData(FO_DrainOutfall DrainOutfallData)
        {
            try
            {
                
                return dalDrain.SaveDrainOutfallData(DrainOutfallData);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function checks if dependency exists for Drain.
        /// Created On 28-09-2016
        /// <summary>
        public bool IsDrainDependencyExists(long _DrainID)
        {
            try
            {
                
                return dalDrain.IsDrainDependencyExists(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function checks for duplicate Drain Record.
        /// Created On 28-09-2016
        /// <summary>
        public bool IsDrainRecordExists(long _DrainID)
        {
            try
            {
                
                return dalDrain.IsOutfallRecordExists(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        /// <summary>
        /// This function gets Exisiting outfall record.
        /// Created On 28-09-2016
        /// <summary>
        public FO_DrainOutfall GetOutfallExistingRecordByDrainID(long _DrainID)
        {
            try
            {
                
                return dalDrain.GetDrainOutfallByDrainID(_DrainID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function gets Zone,Circle,Division selectd values.
        /// Created On 28-09-2016
        /// <summary>
        public object GetSelectedDropdownsHeirarchyIDs(long? _SubDivisionID)
        {
            try
            {
               
                return dalDrain.GetSelectedDropdownsHeirarchyIDs(_SubDivisionID);
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
            try
            {
                return dalDrain.IsSectionExists(_IrrigationBoundary);
            }
            catch (Exception)
            {

                throw;
            }
            
           
        }       


    }
}
