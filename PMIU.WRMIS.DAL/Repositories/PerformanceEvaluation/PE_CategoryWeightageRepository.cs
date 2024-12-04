using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation
{
    public class PE_CategoryWeightageRepository: Repository<PE_CategoryWeightage>
    {
        public PE_CategoryWeightageRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<PE_CategoryWeightage>();

        }
    }

    
}