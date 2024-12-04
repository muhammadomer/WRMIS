using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation
{
    class PE_PMIUChannelDataRepository : Repository<PE_PMIUChannelData>
    {
        public PE_PMIUChannelDataRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<PE_PMIUChannelData>();
        }
    }
}
