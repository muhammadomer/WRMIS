using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Modules.FEWS.FEWSClasses
{
    public class FewsXmlData
    {
        public string parameterId { get; set; }
        public string locationId { get; set; }
        public string stationName { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.TimeSpan> time { get; set; }
        public string value { get; set; }
        public Nullable<int> flag { get; set; }
        public int FID { get; set; }

        public string GetFilePath()
        {
            return ConfigurationSettings.AppSettings["BasePath"];
        }
    }
}