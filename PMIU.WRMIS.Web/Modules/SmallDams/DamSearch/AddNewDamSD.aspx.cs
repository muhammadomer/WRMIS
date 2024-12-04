using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.SmallDams;

namespace PMIU.WRMIS.Web.Modules.SmallDams
{
    public partial class AddNewDamSD : BasePage
    {

        #region  Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                long _DamID = 0;
                if (!IsPostBack)
                {
                    _DamID = Utility.GetNumericValueFromQueryString("DamID", 0);
                    hdnDamTypeID.Value = _DamID.ToString();
                    SetPageTitle();
                    InitialBind();
                    if (_DamID > 0)
                    {
                        BindFormDate(_DamID);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        void InitialBind()
        {
            //ddlSubDivision.Enabled = false;
            //ddlTehsil.Enabled = false;
            //ddlNearestVillage.Enabled = false;

            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
            if (boundryLvlID == null)
            {
                boundryLvlID = 0;
            }
            if (userID > 0)
            {

                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
            }
            EmptyDropdown();

            Dropdownlist.DDLDamType(ddlDamType);
        }

        private void EmptyDropdown()
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);

            Dropdownlist.DDLSDDistricts(ddlDistrict, true, -1, (int)Constants.DropDownFirstOption.Select);

            Dropdownlist.DDLTehsils(ddlTehsil, true, -1, (int)Constants.DropDownFirstOption.Select);

            Dropdownlist.DDLSDVillagesByTehsilID(ddlNearestVillage, true, -1, (int)Constants.DropDownFirstOption.Select);

        }


        SD_SmallDam PrepareDamTypeEntity()
        {
            SD_SmallDam smallDam = new SD_SmallDam();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (Convert.ToInt64(hdnDamTypeID.Value) == 0)
            {
                smallDam.ID = 0;
                smallDam.ModifiedBy = null;
                smallDam.ModifiedDate = null;
            }
            else
            {
                smallDam.ID = Convert.ToInt64(hdnDamTypeID.Value);
                smallDam.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                smallDam.ModifiedDate = System.DateTime.Now;
            }

            if (txtDamName.Text != "")
                smallDam.Name = txtDamName.Text;
            
            if (Convert.ToInt16(ddlDamType.SelectedItem.Value) != 0)
            smallDam.DamTypeID = Convert.ToInt16(ddlDamType.SelectedItem.Value);
            
            if (txtCostofProject.Text != "")
            smallDam.Cost = Convert.ToInt64(txtCostofProject.Text);
            
            if (txtYearofCompletion.Text != "")
            smallDam.CompletionYear = txtYearofCompletion.Text;
            
            if (rdolStatus.SelectedItem.Value == "1")
                smallDam.IsActive = true;
            else
                smallDam.IsActive = false;
            
            if (txtDescription.Text != "")
            smallDam.Description = txtDescription.Text;
            
            if (ddlSubDivision.SelectedIndex != 0)
            smallDam.SubDivID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
            
            if (ddlNearestVillage.SelectedIndex != 0)
            smallDam.VillageID = Convert.ToInt64(ddlNearestVillage.SelectedItem.Value);
            
            if (txtPassingbyimportantroad.Text != "")
            smallDam.Road = txtPassingbyimportantroad.Text;
            
            if (txtLocation.Text != "")
            smallDam.Location = txtLocation.Text;
            
            if (txtStreamNullah.Text != "")
            smallDam.StreamNullah = txtStreamNullah.Text;
            
            if (txtNA.Text != "")
            smallDam.NA = txtNA.Text;
            
            if (txtPP.Text != "")
            smallDam.PP = txtPP.Text;
            
            if (txtUCC.Text != "")
            smallDam.UC = txtUCC.Text;

            smallDam.CreatedBy = Convert.ToInt32(mdlUser.ID);
            smallDam.CreatedDate = System.DateTime.Now;

            return smallDam;
        }

        void ReadOnlyMode()
        {
            txtDamName.Enabled = false;
            txtCostofProject.Enabled = false;
            txtDescription.Enabled = false;
            txtLocation.Enabled = false;
            txtNA.Enabled = false;
            txtPassingbyimportantroad.Enabled = false;
            txtPP.Enabled = false;
            txtStreamNullah.Enabled = false;
            txtUCC.Enabled = false;
            txtYearofCompletion.Enabled = false;
            ddlDamType.Enabled = false;
            ddlDistrict.Enabled = false;
            ddlDivision.Enabled = false;
            ddlNearestVillage.Enabled = false;
            ddlSubDivision.Enabled = false;
            ddlTehsil.Enabled = false;
            rdolStatus.Enabled = false;
        }

        void BindFormDate(Int64 DamID)
        {
            SmallDamsBLL smallDamBll = new SmallDamsBLL();
            object smallDam = smallDamBll.GetSmallDamByID(DamID);

            if (smallDam != null)
            {

                Dropdownlist.DDLDamType(ddlDamType);
                ddlDamType.SelectedIndex = ddlDamType.Items.IndexOf(ddlDamType.Items.FindByValue(smallDam.GetType().GetProperty("DamTypeID").GetValue(smallDam).ToString()));

                long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                if (boundryLvlID == null)
                {
                    boundryLvlID = 0;
                }
                if (userID > 0)
                {

                    Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                }
                ddlDivision.SelectedIndex = ddlDivision.Items.IndexOf(ddlDivision.Items.FindByValue(smallDam.GetType().GetProperty("DivisionID").GetValue(smallDam).ToString()));

                ddlSubDivision.Enabled = true;
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                ddlSubDivision.SelectedIndex = ddlSubDivision.Items.IndexOf(ddlSubDivision.Items.FindByValue(Convert.ToString(smallDam.GetType().GetProperty("SubDivID").GetValue(smallDam))));

                Dropdownlist.DDLDistricts(ddlDistrict);
                ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByValue(Convert.ToString(smallDam.GetType().GetProperty("DistrictID").GetValue(smallDam))));

                Dropdownlist.DDLTehsils(ddlTehsil, false, Convert.ToInt64(ddlDistrict.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                ddlTehsil.Enabled = true;
                ddlTehsil.SelectedIndex = ddlTehsil.Items.IndexOf(ddlTehsil.Items.FindByValue(Convert.ToString(smallDam.GetType().GetProperty("TehsilID").GetValue(smallDam))));

                Dropdownlist.DDLVillagesByTehsilID(ddlNearestVillage, Convert.ToInt64(ddlTehsil.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                ddlNearestVillage.Enabled = true;
                ddlNearestVillage.SelectedIndex = ddlNearestVillage.Items.IndexOf(ddlNearestVillage.Items.FindByValue(Convert.ToString(smallDam.GetType().GetProperty("VillageID").GetValue(smallDam))));



                hdnDamTypeID.Value = Convert.ToString(smallDam.GetType().GetProperty("ID").GetValue(smallDam));
                txtDamName.Text = Convert.ToString(smallDam.GetType().GetProperty("Name").GetValue(smallDam));

                txtCostofProject.Text = Convert.ToString(smallDam.GetType().GetProperty("Cost").GetValue(smallDam));
                txtYearofCompletion.Text = Convert.ToString(smallDam.GetType().GetProperty("CompletionYear").GetValue(smallDam));

                if (Convert.ToBoolean(smallDam.GetType().GetProperty("IsActive").GetValue(smallDam)))
                {
                    rdolStatus.SelectedIndex = 0;
                }
                else
                {
                    rdolStatus.SelectedIndex = 1;
                }
                //rdolStatus.SelectedItem                

                txtDescription.Text = Convert.ToString(smallDam.GetType().GetProperty("Description").GetValue(smallDam));

                txtPassingbyimportantroad.Text = Convert.ToString(smallDam.GetType().GetProperty("Road").GetValue(smallDam));
                txtLocation.Text = Convert.ToString(smallDam.GetType().GetProperty("Location").GetValue(smallDam));
                txtStreamNullah.Text = Convert.ToString(smallDam.GetType().GetProperty("StreamNullah").GetValue(smallDam));
                txtNA.Text = Convert.ToString(smallDam.GetType().GetProperty("NA").GetValue(smallDam));
                txtPP.Text = Convert.ToString(smallDam.GetType().GetProperty("PP").GetValue(smallDam));
                txtUCC.Text = Convert.ToString(smallDam.GetType().GetProperty("UC").GetValue(smallDam));
                MainHeading.InnerHtml = "Edit Dam";
            }

        }
        #endregion

        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                #region Save
                SD_SmallDam DamTypeEntity = PrepareDamTypeEntity();
                if (new SmallDamsBLL().ISDamExist(DamTypeEntity))
                {
                    Master.ShowMessage("Dam name can not be same in same Sub Division.", SiteMaster.MessageType.Error);
                    return;
                }

                bool isSaved = new SmallDamsBLL().SaveSmallDam(DamTypeEntity);
                if (isSaved)
                {
                    if (Convert.ToInt64(hdnDamTypeID.Value) > 0)
                    {
                        Master.ShowMessage("Record update successfully.", SiteMaster.MessageType.Success);

                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    ReadOnlyMode();
                    //HttpContext.Current.Session.Add("FloodInspectionID", DamTypeEntity.ID);
                    Response.Redirect("SearchSD.aspx?ShowHistroy=true", false);
                }

                #endregion
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlDivision.SelectedIndex == 0)
            {
                EmptyDropdown();
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.DDLSDDistricts(ddlDistrict, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
            //ddlSubDivision.Enabled = true;
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDistrict.SelectedIndex == 0)
            {
                Dropdownlist.DDLTehsils(ddlTehsil, true, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLTehsils(ddlTehsil, false, Convert.ToInt64(ddlDistrict.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
            //ddlTehsil.Enabled = true;
        }
        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTehsil.SelectedIndex == 0)
            {
                Dropdownlist.DDLSDVillagesByTehsilID(ddlNearestVillage, true, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLSDVillagesByTehsilID(ddlNearestVillage, false, Convert.ToInt64(ddlTehsil.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }

            //ddlNearestVillage.Enabled = true;
        }
        #endregion













    }
}