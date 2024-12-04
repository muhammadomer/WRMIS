using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
    public partial class InfrastructureAddUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long infrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    //SetPageTitle();
                    BindInfrastructureDropdownlists();
                    infrastructureID = Utility.GetNumericValueFromQueryString("InfrastructureID", 0);
                    if (infrastructureID > 0)
                    {
                        hdnInfrastructureID.Value = Convert.ToString(infrastructureID);
                        LoadInfrastructureDetail(infrastructureID);
                    }
                }

                if (infrastructureID > 0)
                {
                    h3PageTitle.InnerText = "Edit Protection Infrastructure";
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }
        private void LoadInfrastructureDetail(long _InfrastructureID)
        {
            try
            {
                FO_ProtectionInfrastructure bllInfrastructure = new InfrastructureBLL().GetInfrastructureByID(_InfrastructureID);
                if (bllInfrastructure != null)
                {
                    Dropdownlist.SetSelectedValue(ddlInfrastructureType, Convert.ToString(bllInfrastructure.InfrastructureTypeID));
                    txtName.Text = bllInfrastructure.InfrastructureName;
                    txtTotalLength.Text = Convert.ToString(bllInfrastructure.TotalLength);
                    txtInitialCost.Text = Convert.ToString(bllInfrastructure.InitialCost);
                    txtDesignedTopWidth.Text = Convert.ToString(bllInfrastructure.DesignedTopWidth);
                    txtDesignedFreeBoard.Text = Convert.ToString(bllInfrastructure.DesignedFreeBoard);
                    txtCountrySideSlope1.Text = Convert.ToString(bllInfrastructure.CountrySideSlope1);
                    txtCountrySideSlope2.Text = Convert.ToString(bllInfrastructure.CountrySideSlope2);
                    txtDesignedRiverSideSlope1.Text = Convert.ToString(bllInfrastructure.RiverSideSlope1);
                    txtDesignedRiverSideSlope2.Text = Convert.ToString(bllInfrastructure.RiverSideSlope2);
                    hdnInfrastructureID.Value = Convert.ToString(_InfrastructureID);// Convert.ToString(bllInfrastructure.ID);
                    hdnInfrastructureCreatedDate.Value = Convert.ToString(bllInfrastructure.CreatedDate);
                    rdolInfrastructureStatus.SelectedIndex = rdolInfrastructureStatus.Items.IndexOf(rdolInfrastructureStatus.Items.FindByValue(bllInfrastructure.IsActive == true ? "1" : "0"));
                    txtDescription.Text = Convert.ToString(bllInfrastructure.Description);

                    hdnCurrentTotalRD.Value = Convert.ToString(bllInfrastructure.TotalLength);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //private void SetPageTitle()
        //{
        //  Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelAddition);

        //  Master.ModuleTitle = pageTitle.Item1;
        //  if (string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
        //  {
        //    Master.NavigationBar = pageTitle.Item3;
        //    Master.PageTitle = pageTitle.Item2;
        //    pageTitleID.InnerText = pageTitle.Item2;
        //  }
        //  else
        //  {
        //    Master.NavigationBar = "Edit Channel";
        //    Master.PageTitle = "Edit Channel";
        //    pageTitleID.InnerText = "Edit Channel";
        //  }
        //}
        private void BindInfrastructureDropdownlists()
        {
            try
            {
                // Bind Infrastructure type dropdownlist
                Dropdownlist.DDLActiveInfrastructureType(ddlInfrastructureType);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            FO_ProtectionInfrastructure infrastructureEntity = PrepareInfrastructureEntity();
            try
            {
                if (Convert.ToInt64(hdnInfrastructureID.Value) != 0 && Convert.ToInt64(hdnCurrentTotalRD.Value) > 0 && infrastructureEntity.TotalLength != Convert.ToInt64(hdnCurrentTotalRD.Value))
                {
                    Master.ShowMessage("Total RD can not be changed.", SiteMaster.MessageType.Error);
                }
                else if (new InfrastructureBLL().IsInfrastructureExists(infrastructureEntity))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                }
                else if (txtCountrySideSlope1.Text != "" && txtCountrySideSlope2.Text == "")
                {
                    txtCountrySideSlope2.CssClass = "integerInput form-control required";
                    txtCountrySideSlope2.Attributes.Add("required", "required");

                    txtCountrySideSlope1.CssClass = "integerInput form-control";
                    txtCountrySideSlope1.Attributes.Remove("required");

                    return;
                }
                else if (txtCountrySideSlope2.Text != "" && txtCountrySideSlope1.Text == "")
                {
                    txtCountrySideSlope1.CssClass = "integerInput form-control required";
                    txtCountrySideSlope1.Attributes.Add("required", "required");

                    txtCountrySideSlope2.CssClass = "integerInput form-control";
                    txtCountrySideSlope2.Attributes.Remove("required");
                    return;
                }
                else if (txtDesignedRiverSideSlope1.Text != "" && txtDesignedRiverSideSlope2.Text == "")
                {
                    txtDesignedRiverSideSlope2.CssClass = "integerInput form-control required";
                    txtDesignedRiverSideSlope2.Attributes.Add("required", "required");

                    txtDesignedRiverSideSlope1.CssClass = "integerInput form-control";
                    txtDesignedRiverSideSlope1.Attributes.Remove("required");
                    return;

                }
                else if (txtDesignedRiverSideSlope2.Text != "" && txtDesignedRiverSideSlope1.Text == "")
                {
                    txtDesignedRiverSideSlope1.CssClass = "integerInput form-control required";
                    txtDesignedRiverSideSlope1.Attributes.Add("required", "required");

                    txtDesignedRiverSideSlope2.CssClass = "integerInput form-control";
                    txtDesignedRiverSideSlope2.Attributes.Remove("required");
                    return;
                }
                else
                {
                    //    txtCountrySideSlope1.CssClass = "integerInput form-control";
                    //    txtCountrySideSlope1.Attributes.Remove("required");
                    //    txtCountrySideSlope2.CssClass = "integerInput form-control";
                    //    txtCountrySideSlope2.Attributes.Remove("required");
                    //    txtDesignedRiverSideSlope1.CssClass = "integerInput form-control";
                    //    txtDesignedRiverSideSlope1.Attributes.Remove("required");
                    //    txtDesignedRiverSideSlope2.CssClass = "integerInput form-control";
                    //    txtDesignedRiverSideSlope2.Attributes.Remove("required");

                    bool isSaved = new InfrastructureBLL().SaveInfrastructure(infrastructureEntity);
                    if (isSaved)
                    {
                        InfrastructureSearch.IsSaved = true;
                        HttpContext.Current.Session.Add("InfrastructureName", infrastructureEntity.InfrastructureName);
                        Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID=" + infrastructureEntity.ID, false);
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                lblMessage.Text = ex.Message;
            }
        }

        private FO_ProtectionInfrastructure PrepareInfrastructureEntity()
        {
            FO_ProtectionInfrastructure infrastructure = new FO_ProtectionInfrastructure();
            if (!string.IsNullOrEmpty(hdnInfrastructureID.Value))
                infrastructure.ID = Convert.ToInt64(hdnInfrastructureID.Value);

            if (infrastructure.ID == 0)
            {
                infrastructure.CreatedDate = DateTime.Now;
            }
            else
            {
                infrastructure.CreatedDate = Convert.ToDateTime(hdnInfrastructureCreatedDate.Value);
                infrastructure.ModifiedDate = DateTime.Now;
            }

            infrastructure.InfrastructureTypeID = Convert.ToInt16(ddlInfrastructureType.SelectedValue);
            infrastructure.InfrastructureName = txtName.Text;
            infrastructure.TotalLength = Convert.ToInt64(txtTotalLength.Text);

            if (!string.IsNullOrEmpty(txtInitialCost.Text))
                infrastructure.InitialCost = Convert.ToInt64(txtInitialCost.Text);

            infrastructure.DesignedTopWidth = Convert.ToInt64(txtDesignedTopWidth.Text);

            infrastructure.DesignedFreeBoard = Convert.ToInt64(txtDesignedFreeBoard.Text);

            if (!string.IsNullOrEmpty(txtCountrySideSlope1.Text))
                infrastructure.CountrySideSlope1 = txtCountrySideSlope1.Text; //Convert.ToInt16(txtCountrySideSlope1.Text);

            if (!string.IsNullOrEmpty(txtCountrySideSlope2.Text))
                infrastructure.CountrySideSlope2 = txtCountrySideSlope2.Text; // Convert.ToInt16(txtCountrySideSlope2.Text);

            if (!string.IsNullOrEmpty(txtDesignedRiverSideSlope1.Text))
                infrastructure.RiverSideSlope1 = txtDesignedRiverSideSlope1.Text; //Convert.ToInt16(txtDesignedRiverSideSlope1.Text);

            if (!string.IsNullOrEmpty(txtDesignedRiverSideSlope2.Text))
                infrastructure.RiverSideSlope2 = txtDesignedRiverSideSlope2.Text; //Convert.ToInt16(txtDesignedRiverSideSlope2.Text);

            infrastructure.IsActive = (rdolInfrastructureStatus.SelectedItem.Value == "1") ? true : false;

            if (!string.IsNullOrEmpty(txtDescription.Text))
                infrastructure.Description = txtDescription.Text.ToString();

            return infrastructure;
        }


    }
}