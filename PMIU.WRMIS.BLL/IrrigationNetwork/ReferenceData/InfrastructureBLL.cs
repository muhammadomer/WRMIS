using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class InfrastructureBLL
    {
        /// <summary>
        /// This function returns list of all active Infrastructure type.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_InfrastructureType>()</returns>
        public List<CO_StructureType> GetAllActiveInfrastructureType()
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();

            return dalInfrastructure.GetAllActiveInfrastructureType();
        }

        /// <summary>
        /// This function returns list of all Infrastructure.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_InfrastructureType>()</returns>
        public List<CO_StructureType> GetAllInfrastructureType()
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();

            return dalInfrastructure.GetAllInfrastructureType();
        }

        public List<object> GetProtectionInfrastructureBySearchCriteria(long _InfrastructureID
            , long _ZoneID
            , long _CircleID
            , long _DivisionID
            , Int16 _InfrastructureTypeID
            , string _InfrastructureName
            , Int64 _InfrastructureStatus)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetProtectionInfrastructureBySearchCriteria(_InfrastructureID
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
        /// <param name="_InfrastructureID"></param>
        /// <returns>FO_ProtectionInfrastructure</returns>
        public FO_ProtectionInfrastructure GetInfrastructureByID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetInfrastructureByID(_InfrastructureID);
        }

        /// <summary>
        /// This function retun Infrastructure addition success along with message
        /// Created on 2-09-2016
        /// </summary>
        /// <param name="_Infrastructure"></param>
        /// <returns></returns>
        public bool SaveInfrastructure(FO_ProtectionInfrastructure _Infrastructure)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveInfrastructure(_Infrastructure);
        }

        /// <summary>
        /// This function deletes a sub division with the provided ID.
        /// Created On 05-09-2016.
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructure(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();

            return dalInfrastructure.DeleteInfrastructure(_InfrastructureID);
        }

        /// <summary>
        /// This function checks in all related tables for given Infrastructure ID.
        /// Created On 05-09-2016.
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        public bool IsInfrastructureIDExists(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();

            return dalInfrastructure.IsInfrastructureIDExists(_InfrastructureID);
        }

        /// <summary>
        /// This method return Protection Infrastructure Type Information Using ID
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>CO_StructureType</returns>
        public CO_StructureType GetInfrastructureTypeByID(long _ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetInfrastructureTypeByID(_ID);
        }

        #region "Infrastructure Physical Location"
        /// <summary>
        /// This function return Infrastructure Irrigation Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetIrrigationBoundariesByInfrastructureID(_InfrastructureID);
        }
        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public long GetStructureTypeIDByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetStructureTypeIDByInfrastructureID(_InfrastructureID);
        }
        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsDivisionExists(FO_StructureIrrigationBoundaries _IrrigationBoundary)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsDivisionExists(_IrrigationBoundary);
        }
        ///// <summary>
        ///// This function check SectionRD existance in Irrigation Boundaries
        ///// Created on 15-09-2016
        ///// </summary>
        ///// <param name="_IrrigationBoundary"></param>
        ///// <returns>bool</returns>
        //public bool IsSectionRDsExists(FO_InfrastructureIrrigationBoundaries _IrrigationBoundary)
        //{
        //  InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
        //  return dalInfrastructure.IsSectionRDsExists(_IrrigationBoundary);
        //}
        public bool SaveIrrigationBoundaries(FO_StructureIrrigationBoundaries _IrrigationBoundaries)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveIrrigationBoundaries(_IrrigationBoundaries);
        }
        public bool DeleteIrrigationBoundaries(long _IrrigationBoundariesID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DeleteIrrigationBoundaries(_IrrigationBoundariesID);
        }
        /// <summary>
        /// This function returns Infrastructure Administrative Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetAdministrativeBoundariesByInfrastructureID(_InfrastructureID);
        }
        /// <summary>
        /// This function returns Infrastructure Divisions
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetDivisionsByInfrastructureID(_InfrastructureID);
        }
        /// <summary>
        /// This function returns Channel Districts
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetDistrictsByInfrastructureID(_InfrastructureID);

        }
        /// <summary>
        /// This function check Village existance in Admin Boundaries
        /// Created on 15-09-2016
        /// </summary>
        /// <param name="_AdminBoundary"></param>
        /// <returns>bool</returns>
        public bool IsVillageExists(FO_StructureAdminBoundaries _AdminBoundary)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsVillageExists(_AdminBoundary);
        }
        public bool SaveAdministrativeBoundaries(FO_StructureAdminBoundaries _AdministrativeBoundaries)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DeleteAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        public FO_StructureIrrigationBoundaries GetIrrigationBoundaryByID(long _IrrigationBoundariesID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetIrrigationBoundaryByID(_IrrigationBoundariesID);
        }
        public bool IsIrrigationBoundariesDependencyExists(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsIrrigationBoundariesDependencyExists(_InfrastructureID);
        }

        ///// <summary>
        ///// This function update the IsCalcualted bit in Channel table to calculate Gauges
        ///// Created on: 03-02-2016
        ///// </summary>
        ///// <param name="_ChannelID"></param>
        ///// <param name="_IsCalculated"></param>
        ///// <returns>bool</returns>
        //public bool UpdateIsCalculated(long _ChannelID, bool _IsCalculated = false)
        //{
        //  ChannelDAL dalChannel = new ChannelDAL();
        //  return dalChannel.UpdateIsCalculated(_ChannelID, _IsCalculated);
        //}
        #endregion

        /// <summary>
        /// This method will save Protection Infrastructure Parent, IF successfuly inserted return true
        /// </summary>
        /// <param name="FO_InfrastructureParent"></param>
        /// <returns>Bool</returns>
        public bool SaveInfrastructureParent(FO_InfrastructureParent _InfrastructureParent)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveInfrastructureParent(_InfrastructureParent);
        }
        public bool UpdateInfrastructureParent(FO_InfrastructureParent _InfrastructureParent)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.UpdateInfrastructureParent(_InfrastructureParent);
        }

        /// <summary>
        /// This function returns list of all active Infrastructure type.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_InfrastructureType>()</returns>
        public List<CO_StructureType> GetAllInfrastructureParent()
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();

            return dalInfrastructure.GetAllActiveInfrastructureType();
        }


        #region "Infrastructure Stone Stock"

        /// <summary>
        /// This method return Protection Infrastructure Stone stock by _InfrastructureID
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetStoneStockByInfrastructureID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetStoneStockByInfrastructureID(_InfrastructureID);
        }

        /// <summary>
        /// This function check FromRD ToRD existance in Stone Stock
        /// Created on 21-09-2016
        /// </summary>
        /// <param name="_InfrastructureStoneStock"></param>
        /// <returns>bool</returns>
        public bool IsStoneStockFromRDToRDExits(FO_InfrastructureStoneStock _InfrastructureStoneStock)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsStoneStockFromRDToRDExits(_InfrastructureStoneStock);
        }

        /// <summary>
        /// This function retun Protection Infrastructure Stone stock addition success along with message
        /// Created on 21-Sep-2016
        /// </summary>
        /// <param name="_InfrastructureStoneStock"></param>
        /// <returns>bool</returns>
        public bool SaveInfrastructureStoneStock(FO_InfrastructureStoneStock _InfrastructureStoneStock)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveInfrastructureStoneStock(_InfrastructureStoneStock);
        }

        /// <summary>
        /// This function deletes a Protection Infrastructure Stone stock with the provided ID.
        /// Created On 21-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructureStoneStockByID(long _ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DeleteInfrastructureStoneStockByID(_ID);
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
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetRepresentativeByInfrastructureID(_InfrastructureID);
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
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveInfrastructureRepresentative(_InfrastructureRepresentative);
        }

        /// <summary>
        /// This function deletes a Protection Infrastructure Stone stock with the provided ID.
        /// Created On 21-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteInfrastructureRepresentative(long _ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DeleteInfrastructureRepresentative(_ID);
        }
        public bool IsInfrastructureParent(long _ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsInfrastructureParentExists(_ID);
        }
        public FO_InfrastructureParent GetParentByID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetInfrastructureParentByID(_InfrastructureID);
        }
        #endregion

        #region "Define Infrastructure BreachingSection"
        public List<object> GetBreachingSection(long _InfrastructureID)
        {
            InfrastructureDAL dalBreachingSection = new InfrastructureDAL();
            return dalBreachingSection.GetBreachingSection(_InfrastructureID);
        }
        public bool SaveBreachingSection(FO_InfrastructureBreachingSection _BreachingSection)
        {
            InfrastructureDAL dalBreachingSection = new InfrastructureDAL();
            return dalBreachingSection.SaveInfrastructureBreachingSection(_BreachingSection);
        }

        public bool DeleteBreachingSection(long _ID)
        {
            InfrastructureDAL dalBreachingSection = new InfrastructureDAL();
            return dalBreachingSection.DeleteInfrastructureBreachingSection(_ID);
        }

        public FO_InfrastructureBreachingSection GetBreachingSectioneByID(long _BreachingSectionID)
        {
            InfrastructureDAL dalBreachingSection = new InfrastructureDAL();
            return dalBreachingSection.GetBreachingSectioneByID(_BreachingSectionID);
        }


        /// <summary>
        /// /// This function return List of all active protection infrastructure 
        /// </summary>
        /// <typeparam name="FO_ProtectionInfrastructure"></typeparam>
        /// <returns></returns>
        /// <created>9/16/2016</created>
        /// <changed>9/16/2016</changed>
        public List<FO_ProtectionInfrastructure> GetActiveProtectionInfrastructure()
        {
            InfrastructureDAL dalactiveInfrastructure = new InfrastructureDAL();

            return dalactiveInfrastructure.GetAllActiveProtectionInfrastructure();
        }
        public List<object> GetExplosivesMatetial(long _BreachingSectionID)
        {
            InfrastructureDAL dalExplosivesMatetial = new InfrastructureDAL();
            return dalExplosivesMatetial.GetExplosivesMatetial(_BreachingSectionID);
        }
        public bool SaveExplosivesMatetial(FO_BreachingSectionExplosives _ExplosivesMatetial)
        {
            InfrastructureDAL dalExplosivesMatetial = new InfrastructureDAL();
            return dalExplosivesMatetial.SaveExplosivesMatetial(_ExplosivesMatetial);
        }
        public bool DeleteExplosivesMatetial(long _ID)
        {
            InfrastructureDAL dalExplosivesMatetial = new InfrastructureDAL();
            return dalExplosivesMatetial.DeleteExplosivesMatetial(_ID);
        }

        #endregion "Define Infrastructure BreachingSection"

        /// <summary>
        /// This function returns Infrastructure Parent by InfrastructureID 
        /// Created on: 26-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public object GetInfrastructureParentNameByID(long _InfrastructureID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetInfrastructureParentNameByID(_InfrastructureID);
        }

        public List<CO_Division> DDLDivisionsForDFAndIrrigation(long _CircleID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DDLDivisionsForDFAndIrrigation(_CircleID);
        }
        public List<FO_ExplosivesCustody> GetInfrastructureCustody()
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetInfrastructureCustody();
        }

        public bool IsInfrastructureExists(FO_ProtectionInfrastructure _Infrastructure)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsInfrastructureExists(_Infrastructure);
        }

        /// <summary>
        /// This function returns StructureID 
        /// Created on: 24-03-2017
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByStructureID(long _StructureID, bool? _IsActive = null)
        {
            return new InfrastructureDAL().GetDistrictsByStructureID(_StructureID, _IsActive);

        }

        public bool DeleteGaugesByID(long _ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.DeleteGaugesByID(_ID);
        }

        public bool SaveProtectionInfrastructureGauges(FO_FloodGauge _StructureGauge)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.SaveProtectionInfrastructureGauges(_StructureGauge);
        }

        public List<object> GetGaugesByProtectionInfrastructureID(long ID)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetGaugesByProtectionInfrastructureID(ID);
        }

        public List<FO_FloodGaugeType> GetGaugeTypesFloodBund()
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.GetGaugeTypesFloodBund();
        }

        public bool IsFloodGaugeExists(FO_FloodGauge _FloodGauge)
        {
            InfrastructureDAL dalInfrastructure = new InfrastructureDAL();
            return dalInfrastructure.IsFloodGaugeExists(_FloodGauge);
        }
    }
}
