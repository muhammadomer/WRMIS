using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;
using System.Data;


namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class VillagesBenefittedSD : BasePage
    {
        #region Initialize
        List<object> ControlsValues = new List<object>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _ChannelID = Utility.GetNumericValueFromQueryString("ChannelID", 0);
                    long _SmallDamID = Utility.GetNumericValueFromQueryString("SmallDamID", 0);
                    hdnSmallDamID.Value = Convert.ToString(_SmallDamID);

                    long _UserID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

                    if (_ChannelID > 0)
                    {
                        if (boundryLvlID == null)
                        {
                            boundryLvlID = 0;
                        }
                        DamChannels._DAMID = _ChannelID;

                        long _VillageBenefittedID = Utility.GetNumericValueFromQueryString("VillagesBenID", 0);
                        hdnChannelID.Value = Convert.ToString(_ChannelID);
                        hdnVillageBenefittedID.Value = Convert.ToString(_VillageBenefittedID);

                        BindVillageTypeGrid();
                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/SmallDams/DamSearch/ChannelsSD.aspx?SmallDamID={0}", _SmallDamID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Events
        protected void gvVillages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVillages.PageIndex = e.NewPageIndex;
                gvVillages.EditIndex = -1;
                BindVillageTypeGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvVillages.EditIndex = -1;
                BindVillageTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddVillage")
                {
                    List<object> lstVillage = new SmallDamsBLL().GetVillageBenefitted(Convert.ToInt64(hdnChannelID.Value));

                    lstVillage.Add(new
                     {
                         ID = 0,
                         DivisionID = 0,
                         DivisionName = string.Empty,
                         DistrictID = 0,
                         DistrictName = string.Empty,
                         TehsilID = 0,
                         TehsilName = string.Empty,
                         VillageID = 0,
                         VillageName = string.Empty,
                         CreatedBy = 0,
                         CreatedDate = DateTime.Now
                     });
                    gvVillages.PageIndex = gvVillages.PageCount;
                    gvVillages.DataSource = lstVillage;
                    gvVillages.DataBind();

                    gvVillages.EditIndex = gvVillages.Rows.Count - 1;
                    gvVillages.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    #region "Data Keys"
                    DataKey key = gvVillages.DataKeys[e.Row.RowIndex];
                    string DivisionID = Convert.ToString(key.Values["DivisionID"]);
                    string DistrictID = Convert.ToString(key.Values["DistrictID"]);
                    string TehsilID = Convert.ToString(key.Values["TehsilID"]);
                    string VillageID = Convert.ToString(key.Values["VillageID"]);
                    #endregion

                    #region Control
                    DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDivision");
                    DropDownList ddlDistrict = (DropDownList)e.Row.FindControl("ddlDistrict");
                    DropDownList ddlTehsil = (DropDownList)e.Row.FindControl("ddlTehsil");
                    DropDownList ddlVillage = (DropDownList)e.Row.FindControl("ddlVillage");

                    #endregion
                    long _UserID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID; 

                    if (ddlDivision != null)
                    {
                        Dropdownlist.DDLSDDivisionsByUserAndDamID(ddlDivision, _UserID, Convert.ToInt64(hdnSmallDamID.Value), Convert.ToInt64(boundryLvlID));
                    }

                    if (gvVillages.EditIndex == e.Row.RowIndex)
                    {
                        //ID,DivisionID,DivisionName,DistrictID,DistrictName,TehsilID,TehsilName,VillageID,VillageName,CreatedBy,CreatedDate

                        if (ddlDivision != null)
                        {
                            Dropdownlist.DDLSDDivisionsByUserAndDamID(ddlDivision, _UserID, Convert.ToInt64(hdnSmallDamID.Value), Convert.ToInt64(boundryLvlID));
                            if (!string.IsNullOrEmpty(DivisionID))
                                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
                        }

                        if (ddlDistrict != null)
                        {
                            if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSDDistricts(ddlDistrict, true);
                            }
                            else
                            {
                                Dropdownlist.DDLSDDistricts(ddlDistrict, false, Convert.ToInt64(ddlDivision.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlDistrict, DistrictID);
                            }
                        }

                        if (ddlTehsil != null)
                        {
                            if (string.IsNullOrEmpty(ddlDistrict.SelectedItem.Value))
                                Dropdownlist.DDLSDTehsilByDistrictID(ddlTehsil, true);

                            else
                            {
                                Dropdownlist.DDLSDTehsilByDistrictID(ddlTehsil, false, Convert.ToInt64(ddlDistrict.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlTehsil, TehsilID);
                            }

                        }

                        if (ddlVillage != null)
                        {
                            if (string.IsNullOrEmpty(ddlTehsil.SelectedItem.Value))
                                Dropdownlist.DDLSDTehsilByDistrictID(ddlVillage, true);

                            else
                            {
                                Dropdownlist.DDLSDVillagesByTehsilID(ddlVillage, false, Convert.ToInt64(ddlTehsil.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlVillage, VillageID);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvVillages.DataKeys[e.RowIndex].Values["ID"]);
                bool IsDeleted = new SmallDamsBLL().DeleteVillageType(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindVillageTypeGrid();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvVillages.EditIndex = e.NewEditIndex;
                BindVillageTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //ID,DivisionID,DivisionName,DistrictID,DistrictName,TehsilID,TehsilName,VillageID,VillageName,CreatedBy,CreatedDate
                DataKey key = gvVillages.DataKeys[e.RowIndex];

                string ID = Convert.ToString(key.Values["ID"]);
                string VillageID = Convert.ToString(key.Values["VillageID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region Control
                GridViewRow row = gvVillages.Rows[e.RowIndex];
                DropDownList ddlVillage = (DropDownList)row.FindControl("ddlVillage");
                #endregion
                SD_Village dVillageType = new SD_Village();

                dVillageType.ID = Convert.ToInt64(ID);
                if (ID == "0")
                {
                    dVillageType.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dVillageType.CreatedDate = DateTime.Now;
                    dVillageType.ModifiedBy = null;
                    dVillageType.ModifiedDate = null;
                }
                else
                {
                    dVillageType.CreatedBy = Convert.ToInt32(CreatedBy);
                    dVillageType.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dVillageType.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dVillageType.ModifiedDate = DateTime.Now;
                }

                if (VillageID != "0")
                    dVillageType.VillageID = Convert.ToInt64(VillageID);
                else
                {
                    Int64 ddd = Convert.ToInt64(ddlVillage.SelectedItem.Value);
                    dVillageType.VillageID = Convert.ToInt64(ddlVillage.SelectedItem.Value);
                }
                if (hdnChannelID.Value != null)
                    dVillageType.SmallChannelID = Convert.ToInt64(hdnChannelID.Value);



                if (new SmallDamsBLL().IsVillageTypeUnique(dVillageType))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new SmallDamsBLL().SaveVillageType(dVillageType);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dVillageType.ID) == 0)
                        gvVillages.PageIndex = 0;

                    gvVillages.EditIndex = -1;
                    BindVillageTypeGrid();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;

                DropDownList ddlDistrict = (DropDownList)gvRow.FindControl("ddlDistrict");
                DropDownList ddlTehsil = (DropDownList)gvRow.FindControl("ddlTehsil");
                DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");

                //populate empty dropdown
                Dropdownlist.DDLSDDistricts(ddlDistrict, true);
                Dropdownlist.DDLSDTehsilByDistrictID(ddlTehsil, true);
                Dropdownlist.DDLSDVillagesByTehsilID(ddlVillage, true);

                if (gvRow != null)
                {
                    Dropdownlist.DDLSDDistricts(ddlDistrict, false, Convert.ToInt64(ddlDivision.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlDistrict = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDistrict.NamingContainer;


                DropDownList ddlTehsil = (DropDownList)gvRow.FindControl("ddlTehsil");
                DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");

                //populate empty dropdown
                Dropdownlist.DDLSDTehsilByDistrictID(ddlTehsil, true);
                Dropdownlist.DDLSDVillagesByTehsilID(ddlVillage, true);

                if (gvRow != null)
                {
                    Dropdownlist.DDLSDTehsilByDistrictID(ddlTehsil, false, Convert.ToInt64(ddlDistrict.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlTehsil = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlTehsil.NamingContainer;

                DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");

                //populate empty dropdown
                Dropdownlist.DDLSDVillagesByTehsilID(ddlVillage, true);

                if (gvRow != null)
                {
                    Dropdownlist.DDLSDVillagesByTehsilID(ddlVillage, false, Convert.ToInt64(ddlTehsil.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
        #region Funtions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindVillageTypeGrid()
        {
            try
            {
                List<object> lstVillageType = new SmallDamsBLL().GetVillageBenefitted(Convert.ToInt64(hdnChannelID.Value));
                gvVillages.DataSource = lstVillageType;
                gvVillages.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteVillage");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        #endregion

    }
}