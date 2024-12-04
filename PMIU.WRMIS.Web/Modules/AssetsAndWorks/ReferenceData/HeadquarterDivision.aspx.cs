using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.WaterLosses;
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

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData
{
    public partial class HeadquarterDivision : BasePage
    {
        AssetsWorkBLL balAW = new AssetsWorkBLL();
        WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    bindGvHeadquarterDivision();
                    btnSave.Enabled = base.CanAdd;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void bindGvHeadquarterDivision()
        {
            List<object> allZone = bll_waterLosses.GetAllZones();

            gvHeadquarterDivision.DataSource = allZone;
            gvHeadquarterDivision.DataBind();
        }
        protected void gvHeadquarterDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHeadquarterDivision.PageIndex = e.NewPageIndex;
                bindGvHeadquarterDivision();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvHeadquarterDivision_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvHeadquarterDivision.EditIndex = -1;
                bindGvHeadquarterDivision();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvHeadquarterDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlHeadquarterDivision = (DropDownList)e.Row.FindControl("ddlHeadquarterDivision");
                    Label ZoneID = (Label)e.Row.FindControl("lblZoneID");
                    List<object> lstObject = new List<object>();
                    if (ZoneID != null && !string.IsNullOrEmpty(ZoneID.Text))
                    {
                        lstObject = balAW.GetDivisionzByZoneId(Convert.ToInt32(ZoneID.Text));
                        Dropdownlist.BindDropdownlist<List<object>>(ddlHeadquarterDivision, lstObject);
                        string DivisionId = "";
                        object lstHQ = new AssetsWorkBLL().GetHeadQDivisionByID(Convert.ToInt64(ZoneID.Text));
                        if (lstHQ != null)
                        {
                            Dropdownlist.SetSelectedValue(ddlHeadquarterDivision, Convert.ToString(lstHQ.GetType().GetProperty("DivisionID").GetValue(lstHQ)));
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssestsAndWorks);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSave = false;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                for (int m = 0; m < gvHeadquarterDivision.Rows.Count; m++)
                {

                    string CreatedBy = "";
                    string CreatedDate = "";
                    string HQID = "0";
                    DropDownList ddlHeadquarterDivision = (DropDownList)gvHeadquarterDivision.Rows[m].FindControl("ddlHeadquarterDivision");
                    string ZoneID = gvHeadquarterDivision.DataKeys[m].Values[0].ToString();
                    if (ddlHeadquarterDivision.SelectedItem.Value != "")
                    {
                        object lstHQ = new AssetsWorkBLL().GetHeadQDivisionByID(Convert.ToInt64(ZoneID));
                        if (lstHQ != null)
                        {
                            CreatedBy = Convert.ToString(lstHQ.GetType().GetProperty("CreatedBy").GetValue(lstHQ));
                            CreatedDate = Convert.ToString(lstHQ.GetType().GetProperty("CreatedDate").GetValue(lstHQ));
                            HQID = Convert.ToString(lstHQ.GetType().GetProperty("HQID").GetValue(lstHQ));

                        }

                        AM_HeadquarterDivision mdl = new AM_HeadquarterDivision();

                        mdl.ID = Convert.ToInt64(HQID);
                        mdl.ZoneID = Convert.ToInt64(ZoneID);
                        mdl.DivisionID = Convert.ToInt64(ddlHeadquarterDivision.SelectedValue);

                        if (mdl.ID == 0)
                        {
                            mdl.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            mdl.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            mdl.CreatedBy = Convert.ToInt32(CreatedBy);
                            mdl.CreatedDate = Convert.ToDateTime(CreatedDate);
                            mdl.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                            mdl.ModifiedDate = DateTime.Now;
                        }

                        isSave = new AssetsWorkBLL().SaveHQDivision(mdl);
                    }
                    else
                    {
                        if (Convert.ToInt64(HQID) != 0)
                        {
                            bool IsDeleted = new AssetsWorkBLL().DeleteAssetsAttribute(Convert.ToInt64(HQID));
                        }
                    }
                }
                if(isSave)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    bindGvHeadquarterDivision();
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}

