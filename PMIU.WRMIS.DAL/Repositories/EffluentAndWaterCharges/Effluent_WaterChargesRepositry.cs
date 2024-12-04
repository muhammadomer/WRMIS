using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.EffluentAndWaterCharges
{
    class Effluent_WaterChargesRepositry : Repository<EC_Bills>
    {
        public Effluent_WaterChargesRepositry(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<EC_Bills>();

        }
        public object GetLastECBillID(string ServiceType, string FinancialYear, long industryID)
        {
            object ECBIlls = (from u in context.EC_Bills
                                   where u.ServiceType == ServiceType
                                   && u.FinancialYear == FinancialYear
                                   && u.IndustryID == industryID 
                                   orderby u.ID descending
                                   select new
                                   {
                                       BillID = u.ID

                                   }).FirstOrDefault();
            return ECBIlls;
        }
    }
}
