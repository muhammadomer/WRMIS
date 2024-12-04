using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Database;

namespace PMIU.WRMIS.DAL.Repositories
{
    public class UsersRepository : Repository<User>
    {
        WRMIS_Entities _context;

        public UsersRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<User>();
            _context = context;

        }

        public User GetUserByID(long ID)
        {
            User u = (from dbu in context.Users
                     where dbu.ID == ID
                     select dbu).FirstOrDefault();
            return u;
        }
    }
}
