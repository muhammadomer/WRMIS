using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class Cast
    {
        public static long ToLong(object value)
        {
            return value == null ? 0 : Convert.ToInt64(value);
        }
        public static double ToDouble(object value)
        {
            return value == null ? 0 : Convert.ToDouble(value);
        }

        public static int ToInt(object value)
        {
            return value == null ? 0 : Convert.ToInt32(value);
        }
    }
}
