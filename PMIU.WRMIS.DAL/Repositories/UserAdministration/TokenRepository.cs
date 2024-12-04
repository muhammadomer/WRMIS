using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Database;

namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    class TokenRepository : Repository<UA_Tokens>
    {
        public TokenRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_Tokens>();
        }
    }
}