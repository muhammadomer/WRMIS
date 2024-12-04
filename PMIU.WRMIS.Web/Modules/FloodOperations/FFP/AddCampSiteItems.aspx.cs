using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;
using PMIU.WRMIS.BLL.FloodOperations;
namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class AddCampSiteItems : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int FFPCampSiteID = Utility.GetNumericValueFromQueryString("ID", 0);
                    if (FFPCampSiteID > 0)
                    {

                        hdnFFPCampSiteID.Value = Convert.ToString(FFPCampSiteID);
                        Header_Show(FFPCampSiteID);
                        LoadItemCategory();
                        //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                        //{
                        //    btnSave.Visible = true;
                        //}
                        //else
                        //{
                        //    btnSave.Visible = false;
                        //}
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/AddCampSites.aspx?FFPID={0}", Session["FFPID"].ToString());


                    }
                    btnSave.Enabled = false;
                }

                hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/AddCampSites.aspx?FFPID={0}", Session["FFPID"].ToString());
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
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void Header_Show(long FFPCampSiteID)
        {
            try
            {

                DataSet DS = new FloodFightingPlanBLL().GetFFPCampInfraStrucure(Convert.ToInt32(hdnFFPCampSiteID.Value));
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    lbl_infra_type.Text = DR["StructureType"].ToString();
                    lbl_infrastructure.Text = DR["Infraname"].ToString();

                    if (DR["Source"].ToString() == "Control Structure1")
                    {
                        lbl_RD.Visible = false;
                        Label4.Visible = false;
                    }
                    else
                    {
                        lbl_RD.Text = Convert.ToString(Calculations.GetRDText(Convert.ToInt64(DR["RD"].ToString())));
                    }

                    lbl_Description.Text = DR["Description"].ToString();

                    hdnDivisionID.Value = DR["DivisionID"].ToString();
                    hdnyear.Value = DR["Year"].ToString();
                    hdnStructureTypeID.Value = DR["StructureTypeID"].ToString();
                    hdnStructureID.Value = DR["StructureID"].ToString();
                    hdnStatus.Value = DR["Status"].ToString();

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindItemsGridView(long _ItemCategoryID)
        {
            try
            {
                bool CanAddEditFFP = false;
                UA_SystemParameters systemParameters = null;
                
                IEnumerable<DataRow> IeDivisionStoreItem = new FloodFightingPlanBLL().GetFFPItemsQty(Convert.ToInt64(hdnDivisionID.Value), Convert.ToInt32(hdnFFPCampSiteID.Value), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(hdnyear.Value), Convert.ToInt64(hdnStructureTypeID.Value), Convert.ToInt64(hdnStructureID.Value));
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemId = dataRow.Field<long>("ItemId"),
                    OveralID = dataRow.Field<long?>("OveralID"),
                    RequiredQty = dataRow.Field<Int32?>("RequiredQty"),
                    DivisionQty = dataRow.Field<Int32?>("DivisionQty"),

                }).ToList();
                gvItems.DataSource = LstItem;
                gvItems.DataBind();
                gvItems.Enabled = true;
                if (gvItems.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
                if (hdnStatus.Value == "Published")
                {
                    btnSave.Enabled = false;
                    DisableQtyColumn(gvItems);
                    //if (hdnStatus.Value == "Published" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == Convert.ToInt32(hdnyear.Value))
                    //{
                    //    btnSave.Enabled = true;
                    //    EnableQtyColumn(gvItems);
                    //    btnSave.Visible = true;
                    //}

                }
                if (hdnStatus.Value == "Draft")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(Convert.ToInt32(hdnyear.Value), SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                    if (CanAddEditFFP)
                    {
                        btnSave.Enabled = CanAddEditFFP;
                        EnableQtyColumn(gvItems);
                        btnSave.Visible = CanAddEditFFP;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        DisableQtyColumn(gvItems);
                    }
                }
                else if (Convert.ToString(hdnStatus.Value) == "Published")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(Convert.ToInt32(hdnyear.Value), SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                    if (CanAddEditFFP)
                    {
                        btnSave.Enabled = CanAddEditFFP;
                        EnableQtyColumn(gvItems);
                        btnSave.Visible = CanAddEditFFP;
                    }
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
                TextBox txt_Qty = (TextBox)r.FindControl("txt_Qty");
                txt_Qty.Enabled = false;

            }
        }
        private void EnableQtyColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                TextBox txt_Qty = (TextBox)r.FindControl("txt_Qty");
                txt_Qty.Enabled = true;

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
                    string ID = Convert.ToString(key.Values["OveralID"]);

                    #endregion

                    #region "Controls"

                    Label lbl_AvailableQty = (Label)e.Row.FindControl("lbl_AvailableQty");
                    TextBox txt_Qty = (TextBox)e.Row.FindControl("txt_Qty");

                    #endregion

                    if (ID == "0")
                    {
                        if (txt_Qty.Text == "0")
                        {
                            txt_Qty.Text = "";
                        }
                    }

                    if (CanView == true)
                    {
                        txt_Qty.Enabled = true;
                        e.Row.Cells[2].Enabled = true;
                    }
                    else
                    {
                        e.Row.Cells[2].Enabled = false;
                    }


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
            FloodFightingPlanBLL bllFFPBLL = new FloodFightingPlanBLL();
            bool IsSave = false;
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {
                    string OveralID = "0";
                    string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                    string ExistReceivedQty = gvItems.DataKeys[m].Values[2].ToString();
                    OveralID = Convert.ToString(gvItems.DataKeys[m].Values[3].ToString());
                    TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Qty");

                    if (Convert.ToInt32(OveralID) != 0)
                    {
                        if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        {
                            bllFFPBLL.OverallDivItemsInsertion(
                            Convert.ToInt64(OveralID), Convert.ToInt32(hdnyear.Value),
                            Convert.ToInt64(hdnDivisionID.Value), Convert.ToInt64(ddlItemCategory.SelectedValue),
                            Convert.ToInt64(ItemID), Convert.ToInt64(hdnStructureTypeID.Value),
                            Convert.ToInt64(hdnStructureID.Value), null,
                            null, null,
                            null, Convert.ToInt64(hdnFFPCampSiteID.Value),
                            Convert.ToInt32(txt_Received_Qty.Text), null,
                            Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID),
                            0);
                            IsSave = true;
                        }
                    }
                    else
                    {
                        //if (txt_Received_Qty.Text != "" && txt_Received_Qty.Text != "0")
                        if (txt_Received_Qty.Text != "")
                        {
                            bllFFPBLL.OverallDivItemsInsertion(
                                                       0, Convert.ToInt32(hdnyear.Value),
                                                       Convert.ToInt64(hdnDivisionID.Value), Convert.ToInt64(ddlItemCategory.SelectedValue),
                                                       Convert.ToInt64(ItemID), Convert.ToInt64(hdnStructureTypeID.Value),
                                                       Convert.ToInt64(hdnStructureID.Value), null,
                                                       null, null,
                                                       null, Convert.ToInt64(hdnFFPCampSiteID.Value),
                                                       Convert.ToInt32(txt_Received_Qty.Text), null,
                                                       Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID),
                                                       0);
                            IsSave = true;
                        }
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