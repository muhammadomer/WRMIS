using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring
{
    public partial class ViewCampSite : BasePage
    {
        List<FO_FFPCampSites> lstOMCampSites = new List<FO_FFPCampSites>();
        int FFPID, StructureID, StructureTypeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    ShowHeader();

                    //FFPID = 10012;
                    //StructureTypeID = 11;
                    //StructureID = 19;
                    //  OMDetail._CampSiteID = FFPID;
                    BindGrid();
                    SetPageTitle();
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/OnsiteMonitoring/SearchOnsiteMonitoring.aspx?FFPID={0}", FFPID);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCampSite.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvCampSite.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //DropDownList ddlFundingType = (DropDownList)e.Row.FindControl("ddlFundingType");
                    OnsiteMonitoringBLL bllOnsiteMonitoring = new OnsiteMonitoringBLL();

                    //   long CampSiteID = Utility.GetNumericValueFromQueryString("CampSiteID", 0);
                    DataKey key = gvCampSite.DataKeys[e.Row.RowIndex];
                    UA_SystemParameters systemParameters = null;
                    long CampSiteID = Convert.ToInt64(Convert.ToString(key.Values["ID"]));
                    int FFPYear = Utility.GetNumericValueFromQueryString("Year", 0);
                    Label lblQuantityEntered = (Label)e.Row.FindControl("lblQuantityEntered");
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");


                    FO_OMCampSites mdlCampSite = bllOnsiteMonitoring.GetOMCampSiteByFFPCampSite(CampSiteID);



                    //  DataKey key = gvCampSite.DataKeys[e.Row.RowIndex];
                    string RD = Convert.ToString(key.Values["RD"]);
                    Label lblRD = (Label)e.Row.FindControl("lblRD");
                    if (lblRD.Text != "")
                    {
                        lblRD.Text = Calculations.GetRDText(Convert.ToInt64(RD));

                    }
                    if (Utility.GetStringValueFromQueryString("InfrastructureType", "") == "Control Structure1")
                    {
                        lblRD.Visible = false;
                    }

                    if (gvCampSite.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlCampSiteAvailable = (DropDownList)e.Row.FindControl("ddlCampSiteAvailable");
                        //Label lblHdnLevel = (Label)e.Row.FindControl("lblHdnLevel");


                        ddlCampSiteAvailable.ClearSelection();
                        if (mdlCampSite.IsAvailable == true)
                        {
                            Dropdownlist.SetSelectedValue(ddlCampSiteAvailable, "true");
                        }
                        else if (mdlCampSite.IsAvailable == false)
                        {
                            Dropdownlist.SetSelectedValue(ddlCampSiteAvailable, "false");
                        }



                    }
                    //  if (lblQuantityEntered.Text != "")
                    //  { 
                    LinkButton hlItems = (LinkButton)e.Row.FindControl("hlItems");
                    if (mdlCampSite != null && mdlCampSite.IsAvailable == true)
                    {
                        lblQuantityEntered.Text = "Yes";
                        hlItems.Enabled = true;
                    }
                    else
                    {
                        lblQuantityEntered.Text = "No";
                        hlItems.Enabled = false;
                    }


                    #region User Role
                    btnEdit.Enabled = false;

                    if (new FloodOperationsBLL().CanAddEditOnSiteMonitoring(FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnEdit.Enabled = true;
                    }

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                    //string StartDate = systemParameters.ParameterValue + "-" + FFPYear; // 01-Jan

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                    //string EndDate = systemParameters.ParameterValue + "-" + FFPYear;

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnEdit.Enabled = true;
                    //    }
                    //}

                    #endregion User Role



                    // }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCampSite.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long OMCampSiteID = 0;
                DataKey key = gvCampSite.DataKeys[e.RowIndex];
                long CampSiteID = Convert.ToInt64(Convert.ToString(key.Values["ID"]));
                //   long CampSiteID = Utility.GetNumericValueFromQueryString("CampSiteID", 0);
                int RowIndex = e.RowIndex;
                //long CampSiteID = Convert.ToInt32(((Label)gvCampSite.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                bool CampSiteAvailable = ((DropDownList)gvCampSite.Rows[RowIndex].Cells[3].FindControl("ddlCampSiteAvailable")).SelectedValue == string.Empty ? false : Convert.ToBoolean(((DropDownList)gvCampSite.Rows[RowIndex].Cells[3].FindControl("ddlCampSiteAvailable")).SelectedItem.Value);
                //  HyperLink HlItems = (HyperLink)gvCampSite.Rows[RowIndex].Cells[4].FindControl("hlItems");
                //   HlItems.Enabled = false;

                OnsiteMonitoringBLL bllOnSiteMonitoring = new OnsiteMonitoringBLL();

                FO_OMCampSites mdlCampSiteForAdd = new FO_OMCampSites();

                FO_OMCampSites mdlCampSite = bllOnSiteMonitoring.GetOMCampSiteByFFPCampSite(CampSiteID);

                bool IsRecordSaved = false;

                if (mdlCampSite != null)
                {
                    mdlCampSiteForAdd.ID = mdlCampSite.ID;
                    mdlCampSiteForAdd.MonitoringDate = mdlCampSite.MonitoringDate;
                    mdlCampSiteForAdd.CampSiteID = mdlCampSite.CampSiteID;
                    mdlCampSiteForAdd.IsAvailable = CampSiteAvailable;

                    mdlCampSiteForAdd.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCampSiteForAdd.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllOnSiteMonitoring.UpdateCampSite(mdlCampSiteForAdd);
                }
                else
                {
                    mdlCampSiteForAdd.ID = OMCampSiteID;
                    mdlCampSiteForAdd.MonitoringDate = DateTime.Now;
                    mdlCampSiteForAdd.CampSiteID = CampSiteID;
                    mdlCampSiteForAdd.IsAvailable = CampSiteAvailable;

                    mdlCampSiteForAdd.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCampSiteForAdd.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllOnSiteMonitoring.AddCampSite(mdlCampSiteForAdd);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (CampSiteID == 0)
                    {
                        gvCampSite.PageIndex = 0;
                    }
                    gvCampSite.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCampSite.EditIndex = e.NewEditIndex;
                BindGrid();
                HyperLink HlItems = (HyperLink)gvCampSite.Rows[e.NewEditIndex].Cells[4].FindControl("hlItems");
                HlItems.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
            StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
            StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
            //FFPID = 10012;
            //StructureTypeID = 11;
            //StructureID = 19;
            lstOMCampSites = new OnsiteMonitoringBLL().GetFFPCampSiteByFFPCampSiteID(FFPID, StructureID, StructureTypeID);
            gvCampSite.DataSource = lstOMCampSites;
            gvCampSite.DataBind();
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void ShowHeader()
        {
            FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
            StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
            StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);

            lblYear.Text = Utility.GetStringValueFromQueryString("Year", "");
            lblZone.Text = Utility.GetStringValueFromQueryString("Zone", "");
            lblCircle.Text = Utility.GetStringValueFromQueryString("Circle", "");
            lblDivision.Text = Utility.GetStringValueFromQueryString("Division", "");
            lblinfraname.Text = Utility.GetStringValueFromQueryString("InfrastructureName", "");

        }

        protected void gvCampSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvCampSite.DataKeys[row.RowIndex];
                    Response.Redirect("CampSiteItems.aspx?ID=" + Convert.ToString(key["ID"]) + "&FFPID=" + Utility.GetNumericValueFromQueryString("FFPID", 0) + "&StructureTypeID=" + Utility.GetNumericValueFromQueryString("StructureTypeID", 0) + "&StructureID=" + Utility.GetNumericValueFromQueryString("StructureID", 0) + "&Year=" + Utility.GetStringValueFromQueryString("Year", "") + "&infrastructureType=" + Utility.GetStringValueFromQueryString("infrastructureType", "") + "&InfrastructureName=" + Convert.ToString(key["InfraStructureName"]) + "&Zone=" + Utility.GetStringValueFromQueryString("Zone", "") + "&Circle=" + Utility.GetStringValueFromQueryString("Circle", "") + "&Division=" + Utility.GetStringValueFromQueryString("Division", ""));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}