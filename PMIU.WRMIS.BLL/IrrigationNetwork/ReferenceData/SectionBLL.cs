using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class SectionBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all sections with the given Sub Division ID.
        /// Created On 02-11-2015.
        /// </summary>
        /// <returns>List<CO_Section></returns>
        public List<CO_Section> GetSectionsBySubDivisionID(long _SubDivisionID, bool? _IsActive = null)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.GetSectionsBySubDivisionID(_SubDivisionID, _IsActive);
        }

        /// <summary>
        /// This function adds new section.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_Section"></param>
        /// <returns>bool</returns>
        public bool AddSection(CO_Section _Section)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.AddSection(_Section);
        }

        /// <summary>
        /// This function updates a section.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_Section"></param>
        /// <returns>bool</returns>
        public bool UpdateSection(CO_Section _Section)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.UpdateSection(_Section);
        }

        /// <summary>
        /// This function deletes a section with the provided ID.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>bool</returns>
        public bool DeleteSection(long _SectionID)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.DeleteSection(_SectionID);
        }

        /// <summary>
        /// This function checks in all related tables for given Section ID.
        /// Created On 02-11-2015.
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>bool</returns>
        public bool IsSectionIDExists(long _SectionID)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.IsSectionIDExists(_SectionID);
        }

        /// <summary>
        /// This function gets section by section name and Sub Division ID.
        /// Created On 03-11-2015.
        /// </summary>
        /// <param name="_SectionName"></param>
        /// <param name="_SubDivisionID"></param>
        /// <returns>CO_Section</returns>
        public CO_Section GetSectionByName(string _SectionName, long _SubDivisionID)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.GetSectionByName(_SectionName, _SubDivisionID);
        }

        /// <summary>
        /// This function return section based on the section id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>CO_Section</returns>
        public CO_Section GetByID(long _SectionID)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.GetByID(_SectionID);
        }

        /// <summary>
        /// This function returns data of only those section whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_FilteredSectionIDs"></param>
        /// <returns>List<CO_Section></returns>
        public List<CO_Section> GetFilteredSections(long _SubDivisionID, List<long> _FilteredSectionIDs)
        {
            SectionDAL dalSection = new SectionDAL();

            return dalSection.GetFilteredSections(_SubDivisionID, _FilteredSectionIDs);
        }
    }
}
