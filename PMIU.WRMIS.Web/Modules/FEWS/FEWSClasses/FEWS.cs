using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Modules.FEWS.FEWSClasses
{
    public class FEWS
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Decimal Value { get; set; }
        public int Flag { get; set; }
    }
}