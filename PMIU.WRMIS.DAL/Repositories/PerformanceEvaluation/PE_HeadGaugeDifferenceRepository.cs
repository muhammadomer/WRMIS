using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation
{
    class PE_HeadGaugeDifferenceRepository : Repository<PE_HeadGaugeDifference>
    {
        public PE_HeadGaugeDifferenceRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<PE_HeadGaugeDifference>();
        }
    }
}
