using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class DivisionItemFFP : BasePage
    {
        private long _DivisionID;
        private int _Year;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    long _FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    if (_FFPID > 0)
                    {
                        FFPDetail._FFPID = _FFPID;
                        hdnFFPStatus.Value = new FloodFightingPlanBLL().GetFFPStatus(_FFPID).ToString();
                        SetPageTitle();
                        LoadItemCategory();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/SearchFFP.aspx?FFPID={0}", _FFPID);
                    }
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }
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
                Dropdownlist.DDLItemCategory(ddlItemCategory, false);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region gvItems Grid for FO_Items

        protected void gvItems_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView ProductGrid = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Item Name";
                HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Available Quantity in Division Store";
                HeaderCell.HorizontalAlign = HorizontalAlign.Right;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Quantity Required in Division";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 8; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                // e.Row.Cells[2].Visible = false;
            }
        }

        protected void gvItems_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvItems.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItems.PageIndex = e.NewPageIndex;
                BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion gvItems Grid for FO_Items


        #region gvItemsAsset Grid for FO_AssetItems

        protected void gvItemsAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Visible = false;
                //e.Row.Cells[1].Visible = false;
                // e.Row.Cells[2].Visible = false;
            }
        }

        protected void gvItemsAsset_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvItemsAsset.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvItemsAsset_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItemsAsset.PageIndex = e.NewPageIndex;
                BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion gvItemsAsset Grid for FO_AssetItems

        private void GetYearDivision(long _FFPID)
        {
            try
            {
                object _FFP = new FloodFightingPlanBLL().GetFFPDetails(_FFPID, null, null, null, null, null);
                if (_FFP != null)
                {
                    _DivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_FFP, "FFPDivisionID"));
                    _Year = Convert.ToInt32(Utility.GetDynamicPropertyValue(_FFP, "FFPYear"));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInfrastructuresGrid(long _FFPID)
        {
            try
            {
                GetYearDivision(Utility.GetNumericValueFromQueryString("FFPID", 0));

                if (Convert.ToInt16(ddlItemCategory.SelectedValue) == 4)
                {
                    IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFO_GetOverallDivItems(_DivisionID, _Year, Convert.ToInt16(ddlItemCategory.SelectedValue));
                    var LstFFP = IeFFPCam.Select(dataRow => new
                    {
                        ItemSubCategoryName = dataRow.Field<string>("ItemSubCategoryName"),
                        ItemSubCategoryID = dataRow.Field<long>("ItemSubCategoryID"),
                        AvailableQuantity = dataRow.Field<int>("AvailableQuantity"),
                        AdditionalQty = dataRow.Field<Int32>("AdditionalQty"),
                    }).ToList();
                    gvItemsAsset.DataSource = LstFFP;
                    gvItemsAsset.DataBind();
                    gvItemsAsset.Visible = true;
                    if (gvItemsAsset.Rows.Count > 0)
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                    }
                    gvItemsAsset.Visible = true;
                    gvItems.Visible = false;
                }
                else
                {
                    IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFO_GetOverallDivItems(_DivisionID, _Year, Convert.ToInt16(ddlItemCategory.SelectedValue));
                    var LstFFP = IeFFPCam.Select(dataRow => new
                    {
                        Item = dataRow.Field<string>("Item"),
                        ItemSubcategoryID = dataRow.Field<long>("ItemSubcategoryID"),
                        AvailableQuantityinDivisionStore = dataRow.Field<int>("AvailableQuantityinDivisionStore"),
                        TotalCampSiteQty = dataRow.Field<Int32>("TotalCampSiteQty"),
                        TotalInfrastructureQty = dataRow.Field<Int32>("TotalInfrastructureQty"),
                        AdditionalQty = dataRow.Field<Int32>("AdditionalQty"),
                        TotalQuantity = dataRow.Field<Int32>("TotalQuantity"),
                    }).ToList();
                    gvItems.DataSource = LstFFP;
                    gvItems.DataBind();
                    gvItems.Visible = true;
                    if (gvItems.Rows.Count > 0)
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                    }
                    gvItemsAsset.Visible = false;
                    gvItems.Visible = true;
                }
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
                bool CanAddEditFFP = false;
                UA_SystemParameters systemParameters = null;
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);

                if (ddlItemCategory.SelectedItem.Value == "")
                {
                    gvItems.Visible = false;
                    gvItemsAsset.Visible = false;
                    btnSave.Enabled = false;
                }
                else
                {

                    ShowHideGrid(true);

                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        gvItemsAsset.Visible = true;
                    }
                    else
                    {
                        gvItems.Visible = true;
                    }

                    //gvItems.Enabled = true;
                    //gvItems.Visible = true;
                    //gvItemsAsset.Visible = true;
                    BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    btnSave.Visible = true;
                    //    gvItems.Enabled = true;
                    //}
                    //else
                    //{
                    //    btnSave.Visible = false;
                    //    gvItems.Enabled = false;
                    //}
                    if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                    {
                        btnSave.Visible = false;
                        //gvItems.Enabled = false;
                        //gvItemsAsset.Enabled = false;
                        ShowHideGrid(false);
                    }
                    if (Convert.ToString(hdnFFPStatus.Value) == "Draft")
                    {
                        CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                        if (CanAddEditFFP)
                        {
                            btnSave.Visible = CanAddEditFFP;
                            //gvItems.Enabled = CanAddEditFFP;
                            //gvItemsAsset.Enabled = CanAddEditFFP;
                            ShowHideGrid(CanAddEditFFP);
                        }
                        else
                        {
                            btnSave.Visible = false;
                            //gvItems.Enabled = false;
                            //gvItemsAsset.Enabled = false;
                            ShowHideGrid(CanAddEditFFP);
                        }
                    }
                    else if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                    {
                        CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                        if (CanAddEditFFP)
                        {
                            btnSave.Visible = CanAddEditFFP;
                            //gvItems.Enabled = CanAddEditFFP;
                            //gvItemsAsset.Enabled = CanAddEditFFP;
                            ShowHideGrid(CanAddEditFFP);
                        }
                    }
                    //if (Convert.ToString(hdnFFPStatus.Value) == "Published" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                    //{
                    //    btnSave.Visible = true;
                    //    gvItems.Enabled = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ShowHideGrid(bool boolVal)
        {
          if(Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
          {
              gvItemsAsset.Enabled = boolVal;
          }
          else
          {
              gvItems.Enabled = boolVal;
          }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            GetYearDivision(Utility.GetNumericValueFromQueryString("FFPID", 0));
            FloodFightingPlanBLL bllFloodFightingPlanBLL = new FloodFightingPlanBLL();
            bool IsSave = false;

            if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
            {
                for (int m = 0; m < gvItemsAsset.Rows.Count; m++)
                {
                    try
                    {
                        string ItemID = gvItemsAsset.DataKeys[m].Values[0].ToString();
                        string AdditionalQty = gvItemsAsset.DataKeys[m].Values[2].ToString();
                        TextBox txt_Qty = (TextBox)gvItemsAsset.Rows[m].FindControl("txt_Qty");

                        if (txt_Qty.Text != "" && Convert.ToInt32(txt_Qty.Text) != Convert.ToInt32(AdditionalQty))
                        {
                            bllFloodFightingPlanBLL.OverallDivItemsUpdation(_Year, _DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(txt_Qty.Text), (long)Session[SessionValues.UserID]);
                            IsSave = true;
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                    }
                }
            }
            else
            {
                for (int m = 0; m < gvItems.Rows.Count; m++)
                {
                    try
                    {
                        string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                        string AdditionalQty = gvItems.DataKeys[m].Values[2].ToString();
                        TextBox txt_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Qty");

                        if (txt_Qty.Text != "" && Convert.ToInt32(txt_Qty.Text) != Convert.ToInt32(AdditionalQty))
                        {
                            bllFloodFightingPlanBLL.OverallDivItemsUpdation(_Year, _DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(txt_Qty.Text), (long)Session[SessionValues.UserID]);
                            IsSave = true;
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                    }
                }
            }

            try
            {
                if (IsSave == true)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindInfrastructuresGrid(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));

                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                        gvItemsAsset.Visible = true;
                    else
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