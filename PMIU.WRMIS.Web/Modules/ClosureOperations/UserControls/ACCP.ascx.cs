using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.DAL.DataAccess.ClosureOperations;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls
{
    public partial class ACCP : System.Web.UI.UserControl
    {
        private long ACCP_ID = 0;

        public long ACCPID { get { return ACCP_ID; } set { ACCP_ID = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetLabels(ACCP_ID);
        }


        private void SetLabels(long acpID)
        {
            CW_AnnualClosureProgram accp = new ClosureOperationsBLL().GetACCP_ByID(acpID);
            if (accp != null && accp.ID > 0)
            {
                lblACCP.Text = accp.Title;
                lblACCPYear.Text = accp.Year;
            }
        }
    }
}