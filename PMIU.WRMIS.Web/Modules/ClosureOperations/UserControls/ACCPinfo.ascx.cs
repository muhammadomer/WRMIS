using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls
{
    public partial class ACCPinfo : System.Web.UI.UserControl
    {
        public  string Title { get; set; }
        public  string Year { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                lblTitle.Text = Title; lblYear.Text = Year;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        } 
    }
}