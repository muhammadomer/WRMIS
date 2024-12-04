using PMIU.WRMIS.BLL.CO_ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class ListUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetAllZones()
        {
            CO_RefrenceDataBLL bllReferenceData = new CO_RefrenceDataBLL();

            List<CO_Zone> lstZones = bllReferenceData.GetAllZones();


        }
    }
}