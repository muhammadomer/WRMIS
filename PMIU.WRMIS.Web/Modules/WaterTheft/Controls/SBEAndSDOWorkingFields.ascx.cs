using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public partial class SBEAndSDOWorkingFields : System.Web.UI.UserControl
    {
        public long WTCaseID = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetCanalWires(WTCaseID);

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        public void GetCanalWires(long _WTCaseID)
        {
            try
            {
                List<object> lstCanalWires = new WaterTheftBLL().GetAllCanalWaireNo(_WTCaseID);

                txtSBECanaWireNo.Text = Convert.ToString(lstCanalWires.ElementAt(0).GetType().GetProperty("CanaWireNo").GetValue(lstCanalWires.ElementAt(0)));
                txtSBECanaWireDate.Text = Convert.ToString(lstCanalWires.ElementAt(0).GetType().GetProperty("CanalWireDate").GetValue(lstCanalWires.ElementAt(0)));
                txtClosingDate.Text = Convert.ToString(lstCanalWires.ElementAt(0).GetType().GetProperty("FixDate").GetValue(lstCanalWires.ElementAt(0)));

                if (lstCanalWires.Count() > 1)
                {
                    txtSDOCanalWireNo.Text = Convert.ToString(lstCanalWires.ElementAt(1).GetType().GetProperty("CanaWireNo").GetValue(lstCanalWires.ElementAt(1)));
                    txtSDOCanaWireDate.Text = Convert.ToString(lstCanalWires.ElementAt(1).GetType().GetProperty("CanalWireDate").GetValue(lstCanalWires.ElementAt(1)));
                    taComments.Value = Convert.ToString(lstCanalWires.ElementAt(1).GetType().GetProperty("Remarks").GetValue(lstCanalWires.ElementAt(1)));
                }

                txtSDOCanalWireNo.Enabled = false;
                txtSDOCanaWireDate.Enabled = false;
                spanDateID.Attributes.Add("class", "input-group-addon");
                taComments.Disabled = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

    }
}