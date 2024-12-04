using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure
{
    public partial class AddNew : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long ControlinfrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindControlInfrastructureDropdownlists();
                    ControlinfrastructureID = Utility.GetNumericValueFromQueryString("ControlInfrastructureID", 0);
                    txtYearofConstruction.Text = DateTime.Now.Year.ToString();
                    if (ControlinfrastructureID > 0)
                    {
                        hdnControlInfrastructureID.Value = Convert.ToString(ControlinfrastructureID);
                        LoadControlInfrastructureDetail(ControlinfrastructureID);
                    }
                }

                if (ControlinfrastructureID > 0)
                {
                    h3PageTitle.InnerText = "Edit Barrage / Headwork";
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ControlInfrastructureAddition);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CO_Station ControlinfrastructureEntity = PrepareControlInfrastructureEntity();
            try
            {
                bool isSaved = new ControlledInfrastructureBLL().SaveControlInfrastructure(ControlinfrastructureEntity);
                if (isSaved)
                {
                    Search.IsSaved = true;
                    HttpContext.Current.Session.Add("ControlInfrastructuresName", ControlinfrastructureEntity.Name);
                    Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Search.aspx?ControlInfrastructureID=" + ControlinfrastructureEntity.ID, false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                lblMessage.Text = ex.Message;
            }
        }
        private void BindControlInfrastructureDropdownlists()
        {
            try
            {
                Dropdownlist.DDLActiveControlInfrastructureType(ddlControlInfrastructureType);
                Dropdownlist.DDLProvince(ddlProvince);
                Dropdownlist.DDLRiver(ddlRiver);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadControlInfrastructureDetail(long _ControlInfrastructureID)
        {
            try
            {
                CO_Station bllIControlnfrastructure = new ControlledInfrastructureBLL().GetControlInfrastructureByID(_ControlInfrastructureID);
                if (bllIControlnfrastructure != null)
                {
                    Dropdownlist.SetSelectedValue(ddlControlInfrastructureType, Convert.ToString(bllIControlnfrastructure.StructureTypeID));
                    Dropdownlist.SetSelectedValue(ddlRiver, Convert.ToString(bllIControlnfrastructure.RiverID));
                    Dropdownlist.SetSelectedValue(ddlProvince, Convert.ToString(bllIControlnfrastructure.ProvinceID));
                    txtName.Text = bllIControlnfrastructure.Name;
                    txtYearofConstruction.Text = Convert.ToString(bllIControlnfrastructure.YearOfConstruction);
                    hdnCreatedDate.Value = Convert.ToString(bllIControlnfrastructure.CreatedDate);
                    rdolStatus.SelectedIndex = rdolStatus.Items.IndexOf(rdolStatus.Items.FindByValue(bllIControlnfrastructure.IsActive == true ? "1" : "0"));
                    txtDescription.Text = Convert.ToString(bllIControlnfrastructure.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private CO_Station PrepareControlInfrastructureEntity()
        {
            CO_Station Contolinfrastructure = new CO_Station();
            if (!string.IsNullOrEmpty(hdnControlInfrastructureID.Value))
                Contolinfrastructure.ID = Convert.ToInt64(hdnControlInfrastructureID.Value);

            if (Contolinfrastructure.ID == 0)
            {
                Contolinfrastructure.CreatedDate = DateTime.Now;
                Contolinfrastructure.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);

            }
            else
            {
                Contolinfrastructure.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                Contolinfrastructure.ModifiedDate = DateTime.Now;
                Contolinfrastructure.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                Contolinfrastructure.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }

            Contolinfrastructure.StructureTypeID = Convert.ToInt16(ddlControlInfrastructureType.SelectedValue);
            Contolinfrastructure.ProvinceID = Convert.ToInt16(ddlProvince.SelectedValue);
            Contolinfrastructure.RiverID = Convert.ToInt16(ddlRiver.SelectedValue);
            Contolinfrastructure.YearOfConstruction = Convert.ToInt16(ddlRiver.SelectedValue);
            Contolinfrastructure.IsActive = (rdolStatus.SelectedItem.Value == "1") ? true : false;

            if (!string.IsNullOrEmpty(txtName.Text))
                Contolinfrastructure.Name = Convert.ToString(txtName.Text);
            if (!string.IsNullOrEmpty(txtYearofConstruction.Text))
                Contolinfrastructure.YearOfConstruction = Convert.ToUInt16(txtYearofConstruction.Text);
            if (!string.IsNullOrEmpty(txtDescription.Text))
                Contolinfrastructure.Description = Convert.ToString(txtDescription.Text);

            return Contolinfrastructure;
        }

    }
}