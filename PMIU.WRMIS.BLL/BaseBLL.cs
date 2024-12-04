using PMIU.WRMIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL
{
    public class BaseBLL
    {
        public ContextDB db;

        public BaseBLL()
        {
            db = new ContextDB();
        }
    }
}
