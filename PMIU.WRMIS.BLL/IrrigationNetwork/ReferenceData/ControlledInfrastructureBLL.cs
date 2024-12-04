using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class ControlledInfrastructureBLL
    {
        ControlledInfrastructureDAL dalControlInfrastructure = new ControlledInfrastructureDAL();
        public List<object> GetControlledInfrastructureBySearch(long _ControlInfrastructureID
           , long _ZoneID
           , long _CircleID
           , long _DivisionID
            , string _ControlInfrastructureName
          , Int64 _ControlInfrastructureStatus)
        {

            return dalControlInfrastructure.GetControlledInfrastructureBySearch(_ControlInfrastructureID, _ZoneID, _CircleID, _DivisionID, _ControlInfrastructureName, _ControlInfrastructureStatus);
        }
        public List<CO_StructureType> GetAllActiveControlInfrastructureType()
        {
            return dalControlInfrastructure.GetAllActiveControlInfrastructureType();
        }
        public CO_Station GetControlInfrastructureByID(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.GetControlInfrastructureByID(_ControlInfrastructureID);

        }
        public bool SaveControlInfrastructure(CO_Station _ControlInfrastructure)
        {
            return dalControlInfrastructure.SaveControlInfrastructure(_ControlInfrastructure);
        }
        public bool IsControlInfrastructureDependencyExists(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.IsControlInfrastructureDependencyExists(_ControlInfrastructureID);
        }
        public bool DeleteControlInfrastructure(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.DeleteControlInfrastructure(_ControlInfrastructureID);
        }
        public CO_StructureType GetControlInfrastructureTypeByID(long _ID)
        {
            return dalControlInfrastructure.GetControlInfrastructureTypeByID(_ID);
        }
        public List<object> GetIrrigationBoundariesByControlInfrastructureID(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.GetIrrigationBoundariesByControlInfrastructureID(_ControlInfrastructureID);
        }
        /// <summary>
        /// This function check Division existance in Irrigation Boundaries
        /// Created on 03-10-2016
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public long GetStructureTypeIDByControlInfrastructureID(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.GetStructureTypeIDByControlInfrastructureID(_ControlInfrastructureID);
        }
        public bool SaveIrrigationBoundaries(FO_StructureIrrigationBoundaries _IrrigationBoundaries)
        {
            return dalControlInfrastructure.SaveIrrigationBoundaries(_IrrigationBoundaries);
        }
        public bool DeleteIrrigationBoundaries(long _IrrigationBoundariesID)
        {
            return dalControlInfrastructure.DeleteIrrigationBoundaries(_IrrigationBoundariesID);
        }
        public bool IsDivisionExists(FO_StructureIrrigationBoundaries _IrrigationBoundary)
        {
            return dalControlInfrastructure.IsDivisionExists(_IrrigationBoundary);
        }
        public List<object> GetAdministrativeBoundariesByControlInfrastructureID(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.GetAdministrativeBoundariesByControlInfrastructureID(_ControlInfrastructureID);
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            return dalControlInfrastructure.DeleteAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        public bool SaveAdministrativeBoundaries(FO_StructureAdminBoundaries _AdministrativeBoundaries)
        {
            return dalControlInfrastructure.SaveAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        /// <summary>
        /// This function check Village existance in Admin Boundaries
        /// Created on 04-10-2016
        /// </summary>
        /// <param name="_AdminBoundary"></param>
        /// <returns>bool</returns>
        public bool IsVillageExists(FO_StructureAdminBoundaries _AdminBoundary)
        {
            return dalControlInfrastructure.IsVillageExists(_AdminBoundary);
        }
        public List<object> GetGaugesByID(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.GetGaugesByID(_ControlInfrastructureID);
        }
        public bool DeleteGaugesByControlInfrastructure(long _ID)
        {
            return dalControlInfrastructure.DeleteGaugesByControlInfrastructure(_ID);
        }
        public bool SaveControlInfrastructureGauges(CO_StructureGauge _StructureGauge)
        {
            return dalControlInfrastructure.SaveControlInfrastructureGauges(_StructureGauge);
        }
        public bool IsRecordExists(long _ControlInfrastructureID)
        {
            return dalControlInfrastructure.IsRecordExists(_ControlInfrastructureID);
        }
        public bool SaveStructureTechPara(CO_StructureTechPara _StructureTechPara)
        {
            return dalControlInfrastructure.SaveStructureTechPara(_StructureTechPara);
        }
        public CO_StructureTechPara GetStructureTechParaeByID(long _StructureTechParaID)
        {
            return dalControlInfrastructure.GetStructureTechParaeByID(_StructureTechParaID);

        }
    }
}
