using PMIU.WRMIS.DAL;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class OfficeDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function returns list of all Offices
        /// Created On: 11-11-2015
        /// </summary>
        /// <returns>List<UA_Office></returns>
        public List<UA_Organization> GetAllOffices()
        {
            List<UA_Organization> lstOffice = db.Repository<UA_Organization>().GetAll().OrderBy(n => n.Name).ToList<UA_Organization>();
            return lstOffice;
        }

        /// <summary>
        /// this function adds Office into the database
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool AddOffice(UA_Organization _Office)
        {
            db.Repository<UA_Organization>().Insert(_Office);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return max id
        /// Created On: 08-03-2017
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public long GetMaxIDofOffice()
        {
            long MaxID  = db.Repository<UA_Organization>().GetAll().Max(x => x.ID);
            return MaxID;
        }

        /// <summary>
        /// this function updates Office
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool Update(UA_Organization _Office)
        {
            UA_Organization mdlOffice = db.Repository<UA_Organization>().FindById(_Office.ID);
            mdlOffice.Name = _Office.Name;
            mdlOffice.Description = _Office.Description;

            db.Repository<UA_Organization>().Update(mdlOffice);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes Office from Database
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool DeleteOffice(long _Office)
        {
            db.Repository<UA_Organization>().Delete(_Office);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function returns Office By Name
        /// Created On:11-11-2015
        /// </summary>
        /// <param name="_OfficeName"></param>
        /// <returns>UA_Office</returns>
        public UA_Organization GetOfficeByName(string _OfficeName)
        {
            UA_Organization mdlOffice = db.Repository<UA_Organization>().GetAll().Where(s => s.Name.Trim().ToLower() == _OfficeName.Trim().ToLower()).FirstOrDefault();
            return mdlOffice;
        }

        /// <summary>
        /// this function checks that office Id exists in other tables or not?
        /// Created On:11-11-2015
        /// </summary>
        /// <param name="_OfficeID"></param>
        /// <returns>bool</returns>
        public bool IsOfficeIdExists(long _OfficeID)
        {
            bool qDesignation = db.Repository<UA_Designations>().GetAll().Any(c => c.OrganizationID == _OfficeID);
            return qDesignation;
        }
    }
}
