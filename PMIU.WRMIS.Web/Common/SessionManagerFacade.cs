using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Web.Common
{
    /// <summary>
    ///  SessionManagerFacade provides a facade to the ASP.NET Session object.
    ///     All access to Session variables must be through this class.
    /// </summary>
    public static class SessionManagerFacade
    {
        #region "Private Constants"
        private const string userInformation = "UserInformation";
        private const string userAssociatedLocations = "UserAssociatedLocations";
        #endregion
        /// <summary>
        /// UserName is the current user.
        /// </summary>
        public static string UserName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
        /// <summary>
        /// UserInformation contains the information for the current logged in user.
        /// </summary>
        public static UA_Users UserInformation
        {
            get
            {
                return HttpContext.Current.Session[userInformation] == null ? new UA_Users() : (UA_Users)HttpContext.Current.Session[userInformation];
            }
            set { HttpContext.Current.Session[userInformation] = value; }
        }
        public static UA_AssociatedLocation UserAssociatedLocations
        {
            get
            {
                return HttpContext.Current.Session[userAssociatedLocations] == null ? new UA_AssociatedLocation() : (UA_AssociatedLocation)HttpContext.Current.Session[userAssociatedLocations];
            }
            set
            {
                HttpContext.Current.Session[userAssociatedLocations] = value;
            }
        }
    }
}