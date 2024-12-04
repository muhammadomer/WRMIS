﻿using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.SYS
{
    public class SYSLogsRepository : Repository<SYS_Logs>
    {
        WRMIS_Entities _context;

        public SYSLogsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<SYS_Logs>();
            _context = context;
        }
    }
}