using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.FloodBundGauges
{
    public partial class AddStructure : BasePage
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            long StructureNallahHillTorentID = 0;
            if (!Page.IsPostBack)
            {
                SetPageTitle();
                BindDropDown();
                //BindTehsilDropdown();
                //BindVillageDropdown();
                StructureNallahHillTorentID = Utility.GetNumericValueFromQueryString("StructureNalaHillTorantID", 0);
                LoadStructureDetails(StructureNallahHillTorentID);
                if (StructureNallahHillTorentID > 0)
                {
                    hdnStructureNallahHillTorentID.Value = Convert.ToString(StructureNallahHillTorentID);
                    h3PageTitle.InnerText = "Edit Structure";
                }
                hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/ReferenceData/FloodBundGauges/SearchStructure.aspx";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region BindDropdown

        private void BindDropDown()
        {
            BindStructureTypeFloodBund();
            BindZoneDropdown();
        }

        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisionsForDFAndIrrigation(ddlDivision, false, _CircleID, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindDisrictDropdown()
        {
            Dropdownlist.DDLDistricts(ddlDistrict, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindTehsilDropdown()
        {
            Dropdownlist.DDLTehsils(ddlTehsil, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindVillageDropdown()
        {
            Dropdownlist.DDLVillages(ddlVillage, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindStructureTypeFloodBund()
        {
            Dropdownlist.DDLStructureTypeFloodBund(ddlStructureType, (int)Constants.DropDownFirstOption.Select);
        }

        #endregion BindDropdown

        #region Dropdown Event

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    ddlDivision.SelectedIndex = 0;
                    ddlDistrict.SelectedIndex = 0;
                    ddlTehsil.SelectedIndex = 0;
                    ddlVillage.SelectedIndex = 0;
                    // ddlCircle.Enabled = true;
                }
                //ddlDivision.SelectedIndex = 0;
                //ddlDivision.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    // ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    //  ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDivision.SelectedItem.Value == String.Empty)
            {
                ddlDistrict.SelectedIndex = 0;
                // ddlDistrict.Enabled = false;
            }
            else
            {
                BindDisrictDropdown();
                // ddlDistrict.Enabled = true;
            }
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDistrict.SelectedItem.Value == String.Empty)
                {
                    ddlTehsil.SelectedIndex = 0;
                    // ddlDivision.Enabled = false;
                }
                else
                {
                    BindTehsilDropdown();
                    //  ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTehsil.SelectedItem.Value == String.Empty)
                {
                    ddlVillage.SelectedIndex = 0;
                    // ddlDivision.Enabled = false;
                }
                else
                {
                    BindVillageDropdown();
                    //  ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlDistrict.SelectedItem.Value == String.Empty)
        //        {
        //            ddlTehsil.SelectedIndex = 0;
        //            ddlTehsil.Enabled = false;
        //        }
        //        else
        //        {
        //            BindTehsilDropdown();
        //            ddlTehsil.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlTehsil.SelectedItem.Value == String.Empty)
        //        {
        //            ddlVillage.SelectedIndex = 0;
        //            ddlVillage.Enabled = false;
        //        }
        //        else
        //        {
        //            BindVillageDropdown();
        //            ddlVillage.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        #endregion Dropdown Event


        private void LoadStructureDetails(long _StructureNalaHillTorantID)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                FO_StructureNalaHillTorant FoStructureNalaHillTorant = new FloodOperationsBLL().GetStructureNalaHillTorantByID(_StructureNalaHillTorantID);
                if (FoStructureNalaHillTorant != null)
                {
                    if (FoStructureNalaHillTorant.StructureType.Equals("Nallah"))
                    {
                        Dropdownlist.SetSelectedText(ddlStructureType, "Nallah");
                    }
                    else if (FoStructureNalaHillTorant.StructureType.Equals("Hill Torrent"))
                    {
                        Dropdownlist.SetSelectedText(ddlStructureType, "Hill Torrent");
                    }
                    txtStructureName.Text = FoStructureNalaHillTorant.StructureName;
                    rdolStatus.SelectedIndex = rdolStatus.Items.IndexOf(rdolStatus.Items.FindByValue(FoStructureNalaHillTorant.IsActive == true ? "1" : "0"));

                    CO_Circle GetCirlceByDivisionID = new FloodOperationsBLL().GetCirlceByDivisionID(FoStructureNalaHillTorant.DivisionID);
                    BindCircleDropdown(GetCirlceByDivisionID.ZoneID);
                    Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(GetCirlceByDivisionID.ID));
                    ddlCircle.Enabled = true;


                    CO_Zone GetZoneByCirlceID = new FloodOperationsBLL().GetZoneByCirlceID(GetCirlceByDivisionID.ZoneID);
                    Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(GetZoneByCirlceID.ID));
                    ddlZone.Enabled = true;

                    BindDivisionDropdown(GetCirlceByDivisionID.ID);
                    Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(FoStructureNalaHillTorant.DivisionID));
                    ddlDivision.Enabled = true;

                    BindDisrictDropdown();
                    Dropdownlist.SetSelectedValue(ddlDistrict, Convert.ToString(FoStructureNalaHillTorant.DistrictID));
                    BindTehsilDropdown();
                    Dropdownlist.SetSelectedValue(ddlTehsil, Convert.ToString(FoStructureNalaHillTorant.TehsilID));
                    BindVillageDropdown();
                    Dropdownlist.SetSelectedValue(ddlVillage, Convert.ToString(FoStructureNalaHillTorant.VillageID));
                    txtDesignedDischarge.Text = Convert.ToString(FoStructureNalaHillTorant.DesignedDischarge);

                    //txtName.Text = bllIControlnfrastructure.Name;
                    //txtYearofConstruction.Text = Convert.ToString(bllIControlnfrastructure.YearOfConstruction);
                    //hdnCreatedDate.Value = Convert.ToString(bllIControlnfrastructure.CreatedDate);
                    //txtDescription.Text = Convert.ToString(bllIControlnfrastructure.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            FloodOperationsBLL objFloodOperationsBll = new FloodOperationsBLL();
            bool IsSave = false;
            try
            {
                if (Convert.ToInt32(hdnStructureNallahHillTorentID.Value) != 0)
                {
                    if (new FloodOperationsBLL().IsStructureNalaHillTorantExists(Convert.ToInt32(hdnStructureNallahHillTorentID.Value), Convert.ToInt64(ddlDivision.SelectedItem.Value), Convert.ToString(ddlStructureType.SelectedItem.Text), Convert.ToString(txtStructureName.Text).Trim()))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    objFloodOperationsBll.StructureNallahHillTorantInsertion(Convert.ToInt32(hdnStructureNallahHillTorentID.Value), Convert.ToString(ddlStructureType.SelectedItem.Text),
                        Convert.ToString(txtStructureName.Text).Trim(), Convert.ToInt64(ddlDivision.SelectedItem.Value), ddlDistrict.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlDistrict.SelectedItem.Value), ddlTehsil.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlTehsil.SelectedItem.Value),
                        ddlVillage.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlVillage.SelectedItem.Value), Convert.ToDouble(txtDesignedDischarge.Text), (rdolStatus.SelectedItem.Value == "1") ? true : false, Convert.ToInt32(mdlUser.ID),
                       Convert.ToInt32(mdlUser.ID), 0);
                    IsSave = true;

                }
                else
                {
                    if (new FloodOperationsBLL().IsStructureNalaHillTorantExists(Convert.ToInt32(hdnStructureNallahHillTorentID.Value), Convert.ToInt64(ddlDivision.SelectedItem.Value), Convert.ToString(ddlStructureType.SelectedItem.Text), Convert.ToString(txtStructureName.Text).Trim()))
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    objFloodOperationsBll.StructureNallahHillTorantInsertion(0, Convert.ToString(ddlStructureType.SelectedItem.Text),
                        Convert.ToString(txtStructureName.Text).Trim(), Convert.ToInt64(ddlDivision.SelectedItem.Value), ddlDistrict.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlDistrict.SelectedItem.Value), ddlTehsil.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlTehsil.SelectedItem.Value),
                        ddlVillage.SelectedItem.Value == "" ? (long?)null : Convert.ToInt64(ddlVillage.SelectedItem.Value), Convert.ToDouble(txtDesignedDischarge.Text == "" ? null : txtDesignedDischarge.Text), (rdolStatus.SelectedItem.Value == "1") ? true : false, Convert.ToInt32(mdlUser.ID),
                       Convert.ToInt32(mdlUser.ID), 0);
                    IsSave = true;
                }
                if (IsSave)
                {
                    SearchStructure.IsSaved = IsSave;
                    // Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    //HttpContext.Current.Session.Add("StructureNallahHillTorentID", Convert.ToInt32(hdnStructureNallahHillTorentID.Value));
                    Response.Redirect("SearchStructure.aspx", false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }


}