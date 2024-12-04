using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PMIU.WRMIS.Web.Common
{
    public class CustomSSRSCredentials : IReportServerCredentials
    {
        private string _SSRSUserName;
        private string _SSRSPassWord;
        private string _DomainName;

        public CustomSSRSCredentials(string UserName, string PassWord, string DomainName)
        {
            _SSRSUserName = UserName;
            _SSRSPassWord = PassWord;
            _DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(_SSRSUserName, _SSRSPassWord, _DomainName); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = _SSRSUserName;
            password = _SSRSPassWord;
            authority = null;
            return false;
        }
    }
}