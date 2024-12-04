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
    public partial class InfrastructureDetail : System.Web.UI.UserControl
    {
        public static long InfrastructureID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            long ParentInfrastructureID = 0;
            try
            {
                long.TryParse(Convert.ToString(InfrastructureDetail.InfrastructureID), out ParentInfrastructureID);
                LoadInfrastructureDetail(ParentInfrastructureID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #region InfrastructureDetails
        private void LoadInfrastructureDetail(long _InfrastructureID)
        {
            try
            {
                FO_ProtectionInfrastructure infrastructure = new InfrastructureBLL().GetInfrastructureByID(_InfrastructureID);
                lblName.Text = Convert.ToString(infrastructure.InfrastructureName);
                lblStatus.Text = infrastructure.IsActive == true ? "Active" : "InActive";

                long InfrastructureTypeID = (long)infrastructure.InfrastructureTypeID;
                CO_StructureType bllInfrastructureType = new InfrastructureBLL().GetInfrastructureTypeByID(InfrastructureTypeID);

                lblType.Text = Convert.ToString(bllInfrastructureType.Name);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion InfrastructureDetails
    }
}