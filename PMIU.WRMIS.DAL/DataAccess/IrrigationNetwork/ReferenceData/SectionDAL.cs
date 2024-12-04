using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class SectionDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all sections with the given Sub Division ID.
        /// Created On 02-11-2015.
        /// </summary>
        /// <returns>List<CO_Section></returns>
        public List<CO_Section> GetSectionsBySubDivisionID(long _SubDivisionID, bool? _IsActive = null)
        {
            List<CO_Section> lstSection = db.Repository<CO_Section>().GetAll().Where(s => s.SubDivID == _SubDivisionID && (s.IsActive == _IsActive || _IsActive == null)).OrderBy(s => s.Name).ToList<CO_Section>();

            return lstSection;
        }

        /// <summary>
        /// This function adds new section.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_Section"></param>
        /// <returns>bool</returns>
        public bool AddSection(CO_Section _Section)
        {
            db.Repository<CO_Section>().Insert(_Section);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a section.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_Section"></param>
        /// <returns>bool</returns>
        public bool UpdateSection(CO_Section _Section)
        {
            CO_Section mdlSection = db.Repository<CO_Section>().FindById(_Section.ID);

            mdlSection.Name = _Section.Name;
            mdlSection.Description = _Section.Description;

            db.Repository<CO_Section>().Update(mdlSection);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a section with the provided ID.
        /// Created On 19-10-2015.
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>bool</returns>
        public bool DeleteSection(long _SectionID)
        {
            db.Repository<CO_Section>().Delete(_SectionID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function checks in all related tables for given Section ID.
        /// Created On 02-11-2015.
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>bool</returns>
        public bool IsSectionIDExists(long _SectionID)
        {
            bool qIsExists = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Any(c => c.SectionID == _SectionID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_ChannelGauge>().GetAll().Any(c => c.SectionID == _SectionID);
            }

            return qIsExists;
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
            CO_Section mdlSection = db.Repository<CO_Section>().GetAll().Where(s => s.Name.Replace(" ", "").ToUpper() == _SectionName.Replace(" ", "").ToUpper() &&
                s.SubDivID == _SubDivisionID).FirstOrDefault();

            return mdlSection;
        }

        /// <summary>
        /// This function return section based on the section id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>CO_Section</returns>
        public CO_Section GetByID(long _SectionID)
        {
            CO_Section mdlSection = db.Repository<CO_Section>().GetAll().Where(s => s.ID == _SectionID).FirstOrDefault();

            return mdlSection;
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
            List<CO_Section> lstSection = db.Repository<CO_Section>().GetAll().Where(s => s.SubDivID == _SubDivisionID && _FilteredSectionIDs.Contains(s.ID)).ToList<CO_Section>();

            return lstSection;
        }
    }
}
