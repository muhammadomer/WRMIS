using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.DAL.Repositories;

namespace PMIU.WRMIS.DAL.IrrigationNetwork.ReferenceData
{
    public class ZoneRepository : Repository<CO_Zone>
    {
        WRMIS_Entities _context;

        public ZoneRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CO_Zone>();
            _context = context;

        }

        public CO_Zone GetUserByID(long ID)
        {
            CO_Zone z = (from dbu in context.CO_Zone
                         where dbu.ID == ID
                         select dbu).FirstOrDefault();
            return z;
        }
    }
}
