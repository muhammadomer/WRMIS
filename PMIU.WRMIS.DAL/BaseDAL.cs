using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL
{
    public class BaseDAL
    {
        public ContextDB db;
        public BaseDAL()
        {
            db = new ContextDB();
        }
    }
}
