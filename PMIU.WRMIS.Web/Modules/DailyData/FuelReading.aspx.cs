using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using System.Web.UI.HtmlControls;
using PMIU.WRMIS.BLL.DailyData;
using System.IO;
using PMIU.WRMIS.AppBlocks;
using System.Data;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class FuelReading : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtIncidentDate.Text = Utility.GetFormattedDate(DateTime.Now);
            }
        }
    }
}