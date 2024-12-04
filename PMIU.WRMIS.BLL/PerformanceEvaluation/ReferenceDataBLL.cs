using PMIU.WRMIS.DAL.DataAccess.PerformanceEvaluation;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.PerformanceEvaluation
{
    public class ReferenceDataBLL : BaseBLL
    {
        ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();

        /// <summary>
        /// This function returns all the Complexity Factors
        /// Created On 31-08-2016
        /// </summary>
        /// <returns>List<PE_ComplexityFactor></returns>
        public List<PE_ComplexityFactor> GetComplexityFactors()
        {
            return dalReferenceData.GetComplexityFactors();
        }

        /// <summary>
        /// This function returns Complexity Factor based on ID
        /// Craeted On 08-09-2016
        /// </summary>
        /// <param name="_ComplexityFactorID"></param>
        /// <returns>PE_ComplexityFactor</returns>
        public PE_ComplexityFactor GetComplexityFactorByID(long _ComplexityFactorID)
        {
            return dalReferenceData.GetComplexityFactorByID(_ComplexityFactorID);
        }

        /// <summary>
        /// This function updates a complexity factor.
        /// Created On 01-09-2016.
        /// </summary>
        /// <param name="_ComplexityFactor"></param>
        /// <returns>bool</returns>
        public bool UpdateComplexityFactor(PE_ComplexityFactor _ComplexityFactor)
        {
            return dalReferenceData.UpdateComplexityFactor(_ComplexityFactor);
        }

        /// <summary>
        /// This function returns all the KPI Categories
        /// Created On 07-09-2016
        /// </summary>
        /// <returns>List<PE_KPICategories></returns>
        public List<PE_KPICategories> GetKPICategories()
        {
            return dalReferenceData.GetKPICategories();
        }

        /// <summary>
        /// This function returns KPI Category based on ID
        /// Created On 08-09-2016
        /// </summary>
        /// <param name="_KPICategoryID"></param>
        /// <returns>PE_KPICategories</returns>
        public PE_KPICategories GetKPICategoryByID(long _KPICategoryID)
        {
            return dalReferenceData.GetKPICategoryByID(_KPICategoryID);
        }

        /// <summary>
        /// This function updates a KPI category.
        /// Created On 08-09-2016.
        /// </summary>
        /// <param name="_KPICategories"></param>
        /// <returns>bool</returns>
        public bool UpdateKPICategory(PE_KPICategories _KPICategories)
        {
            return dalReferenceData.UpdateKPICategory(_KPICategories);
        }

        /// <summary>
        /// This function returns all the KPI Sub Categories
        /// Created On 09-09-2016
        /// </summary>
        /// <param name="_KPICategoryID"></param>
        /// <returns>List<PE_KPISubCategories></returns>
        public List<PE_KPISubCategories> GetKPISubCategories(long? _KPICategoryID = null)
        {
            return dalReferenceData.GetKPISubCategories(_KPICategoryID);
        }

        /// <summary>
        /// This function returns KPI Sub Category based on ID
        /// Created On 09-09-2016
        /// </summary>
        /// <param name="_KPISubCategoryID"></param>
        /// <returns>PE_KPISubCategories</returns>
        public PE_KPISubCategories GetKPISubCategoryByID(long _KPISubCategoryID)
        {
            return dalReferenceData.GetKPISubCategoryByID(_KPISubCategoryID);
        }

        /// <summary>
        /// This function updates a KPI Sub category.
        /// Created On 09-09-2016.
        /// </summary>
        /// <param name="_KPISubCategories"></param>
        /// <returns>bool</returns>
        public bool UpdateKPISubCategory(PE_KPISubCategories _KPISubCategories)
        {
            return dalReferenceData.UpdateKPISubCategory(_KPISubCategories);
        }

        /// <summary>
        /// This function returns KPI Sub Categories based on KPICategoryID
        /// Created On 09-09-2016
        /// </summary>
        /// <param name="_KPICategoryID"></param>
        /// <returns>List<PE_KPISubCategories></returns>
        public List<PE_KPISubCategories> GetKPISubCategoriesByKPICategoryID(long _KPICategoryID)
        {
            return dalReferenceData.GetKPISubCategoriesByKPICategoryID(_KPICategoryID);
        }
    }
}
