using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    public class DesignationRepository : Repository<UA_Designations>
    {
        WRMIS_Entities _context;

        public DesignationRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_Designations>();
            _context = context;
        }

        /// <summary>
        /// this function return designations by organization ID and (designations and organization) of designation reporting to another designation
        /// Created On: 23/12/2015
        /// </summary>
        /// <param name="_OrganizationID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDesignationsByOrganizatonID(long _OrganizationID)
        {
            List<dynamic> lstDesignation = (from d in context.UA_Designations
                                            join d2 in context.UA_Designations on d.ReportingToDesignationID equals d2.ID into d3
                                            from d4 in d3.DefaultIfEmpty()
                                            where d.OrganizationID == _OrganizationID || _OrganizationID == -1
                                            select new
                                            {
                                                designation = d,
                                                reportingToDesignation = d4.Name,
                                                reportingToDesignationID = d.ReportingToDesignationID,
                                                reportingToOrganization = d4.UA_Organization.Name,
                                                reportingToOrganizationID = d4.OrganizationID
                                            }

                ).OrderBy(d => d.designation.Name).ToList<dynamic>();
            return lstDesignation;
        }
    }
}

//join ib in context.CO_ChannelIrrigationBoundaries on c.ID equals ib.ChannelID into IBoundary
//                                             from irrigationBoundary in IBoundary.DefaultIfEmpty()

//List<dynamic> lstDesignation = (from d in context.UA_Designations
//                                            join d2 in context.UA_Designations on d.ReportingToDesignationID equals d2.ID
//                                            join o in context.UA_Organization on d2.OrganizationID equals o.ID
//                                            where d.OrganizationID == _OrganizationID || _OrganizationID == -1
//                                            select new
//                                            {
//                                                designation = d,
//                                                reportingToDesignation = d2.Name,
//                                                reportingToDesignationID = d2.ID,
//                                                reportingToOrganization = o.Name,
//                                                reportingToOrganizationID = o.ID
//                                            }

//                ).OrderBy(d => d.designation.Name).ToList<dynamic>();
//            return lstDesignation;