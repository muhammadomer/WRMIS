using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Services.Models
{
    public class ServiceResponse
    {
        public int StatusCode;
        public string StatusMessage;
        public object Data;
        public object Data1=null;
    }
}