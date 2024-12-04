using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
  public partial class InfrastructureInformation : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      long infrastructureID = 0;
      try
      {
        //SetPageTitle();
        if (!string.IsNullOrEmpty(Request.QueryString["InfrastructureID"]))
        {
          infrastructureID = Convert.ToInt64(Request.QueryString["InfrastructureID"]);
          hdnInfrastructureID.Value = Convert.ToString(infrastructureID);
          LoadInfrastructureDetail(infrastructureID);
        }

      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }

    }

    private void LoadInfrastructureDetail(long _InfrastructureID)
    {
      try
      {
        FO_ProtectionInfrastructure bllInfrastructure = new InfrastructureBLL().GetInfrastructureByID(_InfrastructureID);

        if (bllInfrastructure != null)
        {
          long InfrastructureTypeID = (long)bllInfrastructure.InfrastructureTypeID;

          CO_StructureType bllInfrastructureType = new InfrastructureBLL().GetInfrastructureTypeByID(InfrastructureTypeID);
          lblTypeVal.Text = bllInfrastructureType.Name;

          lblNameValue.Text = bllInfrastructure.InfrastructureName;
          lblTotalLengthVal.Text = Convert.ToString(bllInfrastructure.TotalLength);

          if (!string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.InitialCost)))
            lblInitialCostVal.Text = Convert.ToString(bllInfrastructure.InitialCost);

          lblDesignedTopWidthVal.Text = Convert.ToString(bllInfrastructure.DesignedTopWidth);
          lblDesignedFreeBoardVal.Text = Convert.ToString(bllInfrastructure.DesignedFreeBoard);

          if (!string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.CountrySideSlope1)))
            lblCountrySideSlopeVal.Text = Convert.ToString(bllInfrastructure.CountrySideSlope1);

          if (!string.IsNullOrEmpty(lblCountrySideSlopeVal.Text) && !string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.CountrySideSlope2)))
            lblCountrySideSlopeVal.Text = lblCountrySideSlopeVal.Text + " : " + Convert.ToString(bllInfrastructure.CountrySideSlope2);


          if (!string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.RiverSideSlope1)))
            lblDesignedRiverSideSlopeVal.Text = Convert.ToString(bllInfrastructure.RiverSideSlope1);

          if (!string.IsNullOrEmpty(lblDesignedRiverSideSlopeVal.Text) && !string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.RiverSideSlope2)))
            lblDesignedRiverSideSlopeVal.Text = lblDesignedRiverSideSlopeVal.Text + " : " + Convert.ToString(bllInfrastructure.RiverSideSlope2);

          hdnInfrastructureID.Value = Convert.ToString(_InfrastructureID);
          hdnInfrastructureCreatedDate.Value = Convert.ToString(bllInfrastructure.CreatedDate);
          lblInfrastructureStatusVal.Text = bllInfrastructure.IsActive == true ? "Active" : "InActive";

          if (!string.IsNullOrEmpty(Convert.ToString(bllInfrastructure.Description)))
            lblInfrastructureDescriptionVal.Text = bllInfrastructure.Description;

        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }

  }
}