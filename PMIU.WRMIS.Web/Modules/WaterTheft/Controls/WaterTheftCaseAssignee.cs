using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public static class WaterTheftCaseAssignee
    {
        public static long WaterTheftID { get; set; }
        public static string OffenceSite { get; set; }
        public static long CaseStatusID { get; set; }
        public static long AssignedToDesignationID { get; set; }
        public static long AssignedByDesignationID { get; set; }
    }
}