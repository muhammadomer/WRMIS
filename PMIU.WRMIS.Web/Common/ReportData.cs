using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Common
{
    public class ReportData
    {
        public string Name = string.Empty;
        public List<ReportParameter> Parameters = new List<ReportParameter>();
    }
}