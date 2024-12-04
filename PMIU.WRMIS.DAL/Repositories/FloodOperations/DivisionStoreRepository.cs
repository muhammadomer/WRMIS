using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.FloodOperations
{
    class DivisionStoreRepository : Repository<FO_FloodFightingPlan>
    {
        public DivisionStoreRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_FloodFightingPlan>();
        }

        public List<FO_DSEntryType> GetDSStockType(string _Source)
        {
            List<FO_DSEntryType> lstStock = null;

            lstStock = (from DSStockType in context.FO_DSEntryType
                       where DSStockType.Source == _Source
                                          select DSStockType).ToList();

            return lstStock;
        }


    }
}
