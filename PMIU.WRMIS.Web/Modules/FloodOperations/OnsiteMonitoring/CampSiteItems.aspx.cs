using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring
{
    public partial class CampSiteItems : BasePage
    {
        int FFPID, StructureID, StructureTypeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int OMCampSiteID = Utility.GetNumericValueFromQueryString("ID", 0);
                    if (OMCampSiteID > 0)
                    {


                        FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                        StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
                        StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);

                        lblYear.Text = Utility.GetStringValueFromQueryString("Year", "");
                        lblZone.Text = Utility.GetStringValueFromQueryString("Zone", "");
                        lblCircle.Text = Utility.GetStringValueFromQueryString("Circle", "");
                        lblDivision.Text = Utility.GetStringValueFromQueryString("Division", "");
                        lblInfrastructureName.Text = Utility.GetStringValueFromQueryString("InfrastructureName", "");

                        hdnOMCampSiteID.Value = Convert.ToString(OMCampSiteID);
                        LoadFFPDetail(OMCampSiteID);
                        LoadItemCategory();
                        if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
                        {
                            btnSave.Visible = true;
                        }
                        else
                        {
                            btnSave.Visible = false;
                        }
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/OnsiteMonitoring/ViewCampSite.aspx?FFPID=" + FFPID + "&StructureTypeID=" + StructureTypeID + "&StructureID=" + StructureID + "&Year=" + Utility.GetStringValueFromQueryString("Year", "") + "&infrastructureType=" + Utility.GetStringValueFromQueryString("infrastructureType", "") + "&InfrastructureName=" + lblInfrastructureName.Text + "&Zone=" + Utility.GetStringValueFromQueryString("Zone", "") + "&Circle=" + Utility.GetStringValueFromQueryString("Circle", "") + "&Division=" + Utility.GetStringValueFromQueryString("Division", ""));



                        //  Response.Redirect("CampSiteItems.aspx?ID=" + Convert.ToString(key["ID"]) + "&FFPID=" + Utility.GetNumericValueFromQueryString("FFPID", 0) + "&StructureTypeID=" + Utility.GetNumericValueFromQueryString("StructureTypeID", 0) + "&StructureID=" + Utility.GetNumericValueFromQueryString("StructureID", 0) + "&Year=" + Utility.GetStringValueFromQueryString("Year", "") + "&infrastructureType=" + Utility.GetStringValueFromQueryString("infrastructureType", "") + "&InfraName=" + Convert.ToString(key["InfraStructureName"]) + "&Zone=" + Utility.GetStringValueFromQueryString("Zone", "") + "&Circle=" + Utility.GetStringValueFromQueryString("Circle", "") + "&Division=" + Utility.GetStringValueFromQueryString("Division", ""));


                    }
                    //btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadItemCategory()
        {
            try
            {
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlItemCategory.SelectedItem.Value == "")
                {
                    gvItems.Visible = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    gvItems.Visible = true;
                    BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
                    gvItems.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadFFPDetail(long _CampSiteID)
        {
            try
            {

                DataSet DS = new OnsiteMonitoringBLL().GetOMDetail(null, _CampSiteID, null, null, null, null, null, null);
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    lblZone.Text = DR["Zone"].ToString();
                    lblCircle.Text = DR["Circle"].ToString();
                    lblDivision.Text = DR["Division"].ToString();
                    lblInfrastructureName.Text = DR["InfrastructureName"].ToString();
                    lblYear.Text = DR["Year"].ToString();
                    lblInfrastructureType.Text = DR["InfrastructureType"].ToString() == "Control Structure1" ? "Barrage/Headwork" : DR["InfrastructureType"].ToString();
                    lblRD.Text = Calculations.GetRDText(Convert.ToInt64(DR["RD"].ToString()));

                    if (DR["InfrastructureType"].ToString() == "Control Structure1")
                    {
                        lblRD.Visible = false;
                        RDText.Visible = false;
                    }

                    hdnDivisionID.Value = DR["DivisionID"].ToString();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //private void Header_Show(long FFPCampSiteID)
        //{
        //    try
        //    {

        //        DataSet DS = new OnsiteMonitoringBLL().GetFFPCampInfraStrucure(Convert.ToInt32(hdnFFPCampSiteID.Value));
        //        if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow DR = DS.Tables[0].Rows[0];
        //            lbl_infra_type.Text = DR["StructureType"].ToString();
        //            lbl_infrastructure.Text = DR["Infraname"].ToString();
        //            lbl_RD.Text = Convert.ToString(Calculations.GetRDText(Convert.ToInt64(DR["RD"].ToString())));
        //            lbl_Description.Text = DR["Description"].ToString();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        private void BindItemsGridView(long _ItemCategoryID)
        {
            try
            {
                //   _DivisionID, _Year, _CampsiteID, _Categoryid

                IEnumerable<DataRow> IeDivisionStoreItem = new OnsiteMonitoringBLL().OMCamSiteItems(Convert.ToInt64(hdnDivisionID.Value), Convert.ToInt32(lblYear.Text), Convert.ToInt64(hdnOMCampSiteID.Value), Convert.ToInt64(ddlItemCategory.SelectedValue));
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemId = dataRow.Field<long>("ItemID"),
                    OverallDivItemID = dataRow.Field<long?>("OverallDivItemID"),
                    OMID = dataRow.Field<long?>("OMID"),
                    OMCamsiteID = dataRow.Field<long?>("OMCamsiteID"),
                    QuantityApprovedFFP = dataRow.Field<Int32?>("QuantityApprovedFFP"),
                    OMQty = dataRow.Field<Int32?>("OMQty"),
                    CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                    CreatedBy = dataRow.Field<Int32?>("CreatedBy"),

                }).ToList();
                gvItems.DataSource = LstItem;
                gvItems.DataBind();
                if (gvItems.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void DisableQtyColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                TextBox txtOnsiteQuantity = (TextBox)r.FindControl("txtOnsiteQuantity");
                txtOnsiteQuantity.Enabled = false;

            }
        }
        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItems.PageIndex = e.NewPageIndex;
                BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    string OMID = Convert.ToString(key.Values["OMID"]);
                    UA_SystemParameters systemParameters = null;
                    int FFPYear = Utility.GetNumericValueFromQueryString("Year", 0);

                    #endregion

                    #region "Controls"

                    Label lbl_AvailableQty = (Label)e.Row.FindControl("lbl_AvailableQty");
                    TextBox txtOnsiteQuantity = (TextBox)e.Row.FindControl("txtOnsiteQuantity");

                    #endregion

                    if (OMID == "0")
                    {
                        if (txtOnsiteQuantity.Text == "0")
                        {
                            txtOnsiteQuantity.Text = "";
                        }
                    }

                    //if (CanView == true)
                    //{
                    //    txtOnsiteQuantity.Enabled = true;
                    //    e.Row.Cells[2].Enabled = true;
                    //}
                    //else
                    //{
                    //    e.Row.Cells[2].Enabled = false;
                    //}
                    e.Row.Cells[2].Enabled = false;

                    #region User Role

                    if (new FloodOperationsBLL().CanAddEditOnSiteMonitoring(FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        e.Row.Cells[2].Enabled = true;
                    }

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                    //string StartDate = systemParameters.ParameterValue + "-" + FFPYear; // 01-Jan

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                    //string EndDate = systemParameters.ParameterValue + "-" + FFPYear;

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        e.Row.Cells[2].Enabled = true;
                    //    }
                    //}

                    #endregion User Role

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
            bool IsSave = false;
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {

                    TextBox txtOnsiteQuantity = (TextBox)gvItems.Rows[m].FindControl("txtOnsiteQuantity");
                    //OverallDivItemID,ItemName,OMQty,OMID,CreatedDate,CreatedBy,OMCamsiteID
                    string OverallDivItemID = gvItems.DataKeys[m].Values[0].ToString();
                    string OMID = gvItems.DataKeys[m].Values[3].ToString();
                    string OMCamsiteID = gvItems.DataKeys[m].Values[6].ToString();
                    if (txtOnsiteQuantity.Text != "")
                    {

                        FO_OMCampSiteItems mdlOMCampSiteItem = new FO_OMCampSiteItems();

                        mdlOMCampSiteItem.ID = Convert.ToInt64(OMID);
                        mdlOMCampSiteItem.OMCampSiteID = Convert.ToInt64(OMCamsiteID);
                        mdlOMCampSiteItem.OverallDivItemID = Convert.ToInt64(OverallDivItemID);
                        if (txtOnsiteQuantity.Text == "")
                        {
                            txtOnsiteQuantity.Text = "0";
                        }
                        mdlOMCampSiteItem.OnSiteQty = Convert.ToInt32(txtOnsiteQuantity.Text);

                        if (mdlOMCampSiteItem.ID == 0)
                        {
                            mdlOMCampSiteItem.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            mdlOMCampSiteItem.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            string CreatedDate = gvItems.DataKeys[m].Values[4].ToString();
                            string CreatedBy = gvItems.DataKeys[m].Values[5].ToString();
                            mdlOMCampSiteItem.CreatedBy = Convert.ToInt32(CreatedBy);
                            mdlOMCampSiteItem.CreatedDate = Convert.ToDateTime(CreatedDate);
                            mdlOMCampSiteItem.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                            mdlOMCampSiteItem.ModifiedDate = DateTime.Now;
                        }

                        IsSave = new OnsiteMonitoringBLL().SaveOMCampSiteItems(mdlOMCampSiteItem);
                    }

                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
            try
            {
                if (IsSave == true)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedValue));
                    gvItems.Enabled = true;
                    btnSave.Enabled = true;

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