using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class OfficeBLL : BaseBLL
    {
        /// <summary>
        /// this function returns list of all Offices
        /// Created On: 11-11-2015
        /// </summary>
        /// <returns>List<UA_Office></returns>
        public List<UA_Organization> GetAllOffices()
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.GetAllOffices();
        }

        /// <summary>
        /// this function adds Office into the database
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool AddOffice(UA_Organization _Office)
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.AddOffice(_Office);
        }


           /// <summary>
        /// this function return MAX ID
        /// Created On: 08-03-2017
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public long GetMaxIDofOffice()
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.GetMaxIDofOffice();
        }
        
        /// <summary>
        /// this function updates Office
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool Update(UA_Organization _Office)
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.Update(_Office);
        }

        /// <summary>
        /// this function deletes Office from Database
        /// Created On: 11-11-2015
        /// </summary>
        /// <param name="_Office"></param>
        /// <returns>bool</returns>
        public bool DeleteOffice(long _Office)
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.DeleteOffice(_Office);
        }

        /// <summary>
        /// this function returns Office By Name
        /// Created On:11-11-2015
        /// </summary>
        /// <param name="_OfficeName"></param>
        /// <returns>UA_Office</returns>
        public UA_Organization GetOfficeByName(string _OfficeName)
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.GetOfficeByName(_OfficeName);
        }

        /// <summary>
        /// this function checks that office Id exists in other tables or not?
        /// Created On:11-11-2015
        /// </summary>
        /// <param name="_OfficeID"></param>
        /// <returns>bool</returns>
        public bool IsOfficeIdExists(long _OfficeID)
        {
            OfficeDAL dalOffice = new OfficeDAL();
            return dalOffice.IsOfficeIdExists(_OfficeID);
        }
    }
}
