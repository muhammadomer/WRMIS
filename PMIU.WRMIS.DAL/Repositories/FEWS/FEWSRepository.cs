using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.FEWS
{
    class FEWSRepository : Repository<FW_Ref_Classifications>
    {
        public FEWSRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FW_Ref_Classifications>();
        }
    }
}
