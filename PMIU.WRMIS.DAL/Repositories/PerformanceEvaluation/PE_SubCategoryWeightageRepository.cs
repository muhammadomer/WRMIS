using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation
{
    public class PE_SubCategoryWeightageRepository : Repository<PE_SubCategoryWeightage>
    {
        public PE_SubCategoryWeightageRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<PE_SubCategoryWeightage>();

        }
    }

    
}
