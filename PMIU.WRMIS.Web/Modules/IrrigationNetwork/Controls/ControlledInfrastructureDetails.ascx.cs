using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PMIU.WRMIS.Model;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls
{
    public partial class ControlledInfrastructureDetails : System.Web.UI.UserControl
    {
        public static long ID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            long ControlInfrastructureID = 0;
            try
            {
                long.TryParse(Convert.ToString(ControlledInfrastructureDetails.ID), out ControlInfrastructureID);
                LoadControlInfrastructureDetail(ControlInfrastructureID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region ControlInfrastructureDetails
        private void LoadControlInfrastructureDetail(long _ControlInfrastructureID)
        {
            try
            {
                CO_Station Controlinfrastructure = new ControlledInfrastructureBLL().GetControlInfrastructureByID(_ControlInfrastructureID);
                lblName.Text = Convert.ToString(Controlinfrastructure.Name);
                lblStatus.Text = Controlinfrastructure.IsActive == true ? "Active" : "InActive";

                long ControlInfrastructureTypeID = (long)Controlinfrastructure.StructureTypeID;
                CO_StructureType bllStructureType = new ControlledInfrastructureBLL().GetControlInfrastructureTypeByID(ControlInfrastructureTypeID);

                lblType.Text = Convert.ToString(bllStructureType.Name);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion InfrastructureDetails

    }
}