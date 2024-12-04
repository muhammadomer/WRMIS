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
using System.Collections;
namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class SearchSD : BasePage
    {
        #region Initialize
        //long LoggedUser = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    SetPageTitle();

                    //LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                    InitialBind();

                }
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion
        #region Events

        #region Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindSearchResultsGrid();
        }
        #endregion

        #region Grid
        protected void gvDamSearch_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDamSearch.EditIndex = -1;
                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDamSearch.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditDam")
                {
                    long DamID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddNewDamSD.aspx?DamID=" + DamID);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Key
                //ID,Division,DamName,DamType

                DataKey key = gvDamSearch.DataKeys[e.Row.RowIndex];
                string ID = key.Values["ID"].ToString();
                string Division = key.Values["Division"].ToString();
                string DamName = key.Values["DamName"].ToString();
                string DamType = key.Values["DamType"].ToString();
                #endregion
                #region Control
                Label lblID = (Label)e.Row.FindControl("lblID");
                Label lblDivision = (Label)e.Row.FindControl("lblDivision");
                Label lblDamName = (Label)e.Row.FindControl("lblDamName");
                Label lblDamType = (Label)e.Row.FindControl("lblDamType");
                #endregion
                if (ID != null)
                {
                    lblID.Text = ID;
                }
                else
                {
                    lblID.Text = "";
                }
                if (Division != null)
                {
                    lblDivision.Text = Division;
                }
                else
                {
                    lblDivision.Text = "";
                }
                if (DamName != null)
                {
                    lblDamName.Text = DamName;
                }
                else
                {
                    lblDamName.Text = "";
                }
                if (DamType != null)
                {
                    lblDamType.Text = DamType;
                }
                else
                {
                    lblDamType.Text = "";
                }




            }
        }

        protected void gvDamSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long SmallDamID = Convert.ToInt64(gvDamSearch.DataKeys[e.RowIndex].Values["ID"]);

                if (new SmallDamsBLL().IsSmallDamDependencyExists(Convert.ToInt64(SmallDamID)))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new SmallDamsBLL().DeleteSmallDam(SmallDamID);
                if (IsDeleted)
                {
                    Dropdownlist.DDLDamName(ddlDamName, false, -1, (int)Constants.DropDownFirstOption.Select);
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindSearchResultsGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region DropDown
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlDivision.SelectedIndex == 0)
            {
                EmptyDropdown();
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }

        }
        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubDivision.SelectedIndex == 0)
            {
                Dropdownlist.DDLDamName(ddlDamName, false, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLDamName(ddlDamName, false, Convert.ToInt64(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
        }
        #endregion


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
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
            if (boundryLvlID == null || boundryLvlID == 1) // for admin and Chief 
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
            if (!string.IsNullOrEmpty(Request.QueryString["ShowHistroy"]))
            {
                HistoryValues();
            }
        }

        private void BindSearchResultsGrid()
        {
            try
            {

                Int64? SelectedDivisionID = null;
                Int64? SelectedSubDivisionID = null;
                Int64? SelectedDamID = null;
                if (ddlDivision.SelectedIndex != 0)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedDivisionID = 0;
                }
                if (ddlSubDivision.SelectedIndex != 0)
                {
                    SelectedSubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedSubDivisionID = 0;
                }
                if (ddlDamName.SelectedIndex != 0)
                {
                    SelectedDamID = Convert.ToInt64(ddlDamName.SelectedItem.Value);
                }
                else
                {
                    SelectedDamID = 0;
                }

                SaveHistoryValues();
                long _UserID = SessionManagerFacade.UserAssociatedLocations.UserID;
                long? _IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

                if (_IrrigationLevelID == null || _IrrigationLevelID == 1) // for admin and Chief
                    _IrrigationLevelID = 0;

                List<object> lstDamType = new SmallDamsBLL().GetDamTypeSearch(SelectedDamID, SelectedDivisionID, SelectedSubDivisionID, _UserID, _IrrigationLevelID);
                gvDamSearch.DataSource = lstDamType;
                gvDamSearch.DataBind();
                gvDamSearch.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void EmptyDropdown()
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLDamName(ddlDamName, true, -1, (int)Constants.DropDownFirstOption.Select);
        }


        protected void SaveHistoryValues()
        {
            string DivisionID = "0";
            string SubDivisionID = "0";
            string DamNameID = "0";

            if (ddlDivision.SelectedIndex != 0)
            {
                DivisionID = ddlDivision.SelectedIndex.ToString();
            }
            if (ddlSubDivision.SelectedIndex != 0)
            {
                SubDivisionID = ddlSubDivision.SelectedIndex.ToString();
            }
            if (ddlDamName.SelectedIndex != 0)
            {
                DamNameID = ddlDamName.SelectedIndex.ToString();
            }

            Session["SearchValuesSD"] = null;
            object obj = new
            {
                DivisionID,
                SubDivisionID,
                DamNameID

            };
            Session["SearchValuesSD"] = obj;
        }
        protected void HistoryValues()
        {
            object currentObj = Session["SearchValuesSD"] as object;
            if (currentObj != null)
            {


                ddlDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                if (ddlDivision.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                ddlSubDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("SubDivisionID").GetValue(currentObj));

                Dropdownlist.DDLDamName(ddlDamName, false, -1, (int)Constants.DropDownFirstOption.Select);
                ddlDamName.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DamNameID").GetValue(currentObj));

                BindSearchResultsGrid();
            }

        }
        #endregion

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            SaveHistoryValues();
            Response.Redirect("AddNewDamSD.aspx", false);
        }








    }
}