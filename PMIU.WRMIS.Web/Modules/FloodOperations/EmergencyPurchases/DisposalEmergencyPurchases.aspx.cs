using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class DisposalEmergencyPurchases : BasePage
    {
        public static int EPYear;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int EPWorkID = Utility.GetNumericValueFromQueryString("EPWorkID", 0);
                    Session["EPWorkID"] = "";
                    Session["EPWorkID"] = EPWorkID;
                    if (EPWorkID > 0)
                    {

                        hdnEPWorkID.Value = Convert.ToString(EPWorkID);
                        // hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/SearchEmergencyPurchases.aspx?EmergencyPurchaseID={0}", Session["EmergencyPurchaseID"].ToString());

                        HeaderMaterialDisposal_Show(EPWorkID);
                        Grid_disposal_works(EPWorkID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/WorksEmergencyPurchases.aspx?EmergencyPurchaseID={0}", Session["EmergencyPurchaseID"].ToString());
                    }

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
        private void Grid_disposal_works(int EPWorkID)
        {
            try
            {

                List<object> lstFo_disposal = new FloodOperationsBLL().GetF_EmergencyDisposalByID(EPWorkID);

                gv_disposal.DataSource = lstFo_disposal;
                gv_disposal.DataBind();
                gv_disposal.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void HeaderMaterialDisposal_Show(int EPworkID)
        {
            try
            {

                object lstF_MaterialDisposal = new FloodOperationsBLL().GetMaterialDisposal_Header_By_ID(EPworkID);


                if (lstF_MaterialDisposal != null)
                {

                    lbl_year.Text = Convert.ToString(lstF_MaterialDisposal.GetType().GetProperty("Year").GetValue(lstF_MaterialDisposal));
                    lbl_zone.Text = Convert.ToString(lstF_MaterialDisposal.GetType().GetProperty("ZoneName").GetValue(lstF_MaterialDisposal));
                    lbl_Circle.Text = Convert.ToString(lstF_MaterialDisposal.GetType().GetProperty("CircleName").GetValue(lstF_MaterialDisposal));
                    lblnatureofWork.Text = Convert.ToString(lstF_MaterialDisposal.GetType().GetProperty("NatureWorkName").GetValue(lstF_MaterialDisposal));
                    lblRD.Text = Convert.ToString(lstF_MaterialDisposal.GetType().GetProperty("RD").GetValue(lstF_MaterialDisposal));


                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gv_disposal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv_disposal.EditIndex = -1;
                Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_disposal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstFo_EmergencyDisposal = new FloodOperationsBLL().GetF_EmergencyDisposalByID(Convert.ToInt32(hdnEPWorkID.Value));

                    FO_MaterialDisposal mdlFO_EPWork = new FO_MaterialDisposal();

                    mdlFO_EPWork.ID = 0;
                    mdlFO_EPWork.DisposalDate = DateTime.Today;
                    mdlFO_EPWork.VehicleNumber = "";
                    mdlFO_EPWork.BuiltyNumber = "";

                    mdlFO_EPWork.QtyMaterial = 0;
                    mdlFO_EPWork.Unit = "";
                    mdlFO_EPWork.Cost = 0;
                    mdlFO_EPWork.CreatedBy = 0;
                    mdlFO_EPWork.CreatedDate = DateTime.Now;

                    lstFo_EmergencyDisposal.Add(mdlFO_EPWork);


                    gv_disposal.PageIndex = gv_disposal.PageCount;
                    gv_disposal.DataSource = lstFo_EmergencyDisposal;
                    gv_disposal.DataBind();

                    gv_disposal.EditIndex = gv_disposal.Rows.Count - 1;
                    gv_disposal.DataBind();

                    //  gv_disposal.Rows[gv_disposal.Rows.Count - 1].FindControl("txtDate").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;

                long FO_MaterialDisposal_ID = Convert.ToInt64(((Label)gv_disposal.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);

                TextBox txtDate = (TextBox)gv_disposal.Rows[RowIndex].Cells[1].FindControl("txtDate");
                TextBox txtVehicleNumber = (TextBox)gv_disposal.Rows[RowIndex].Cells[2].FindControl("txtVehicleNumber");
                TextBox txtBuiltyeNumber = (TextBox)gv_disposal.Rows[RowIndex].Cells[3].FindControl("txtBuiltyeNumber");
                TextBox txtQtyMaterial = (TextBox)gv_disposal.Rows[RowIndex].Cells[4].FindControl("txtQtyMaterial");

                TextBox txtUnit = (TextBox)gv_disposal.Rows[RowIndex].Cells[5].FindControl("txtUnit");
                TextBox txtCost = (TextBox)gv_disposal.Rows[RowIndex].Cells[6].FindControl("txtCost");

                DataKey key = gv_disposal.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                if (txtQtyMaterial.Text == "0")
                {
                    Master.ShowMessage("Quantity of Materials Should Be Greater Than Zero", SiteMaster.MessageType.Error);
                    return;
                }

                FO_MaterialDisposal mdlFO_MaterialDisposal = new FO_MaterialDisposal();

                mdlFO_MaterialDisposal.ID = FO_MaterialDisposal_ID;
                mdlFO_MaterialDisposal.EPWorkID = Convert.ToInt64(hdnEPWorkID.Value);
                mdlFO_MaterialDisposal.DisposalDate = Convert.ToDateTime(txtDate.Text);
                mdlFO_MaterialDisposal.VehicleNumber = txtVehicleNumber.Text;

                if (txtBuiltyeNumber.Text != null)
                    mdlFO_MaterialDisposal.BuiltyNumber = txtBuiltyeNumber.Text;

                mdlFO_MaterialDisposal.QtyMaterial = Convert.ToInt32(txtQtyMaterial.Text);
                mdlFO_MaterialDisposal.Unit = txtUnit.Text;
                mdlFO_MaterialDisposal.Cost = Convert.ToInt64(txtCost.Text);

                if (mdlFO_MaterialDisposal.ID == 0)
                {
                    mdlFO_MaterialDisposal.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFO_MaterialDisposal.CreatedDate = DateTime.Now;
                }
                else
                {
                    mdlFO_MaterialDisposal.CreatedBy = Convert.ToInt32(CreatedBy);
                    mdlFO_MaterialDisposal.CreatedDate = Convert.ToDateTime(CreatedDate);
                    mdlFO_MaterialDisposal.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFO_MaterialDisposal.ModifiedDate = DateTime.Now;
                }



                bool IsSave = new FloodOperationsBLL().SaveDisposalEmergency_Purchase(mdlFO_MaterialDisposal);



                if (IsSave)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (FO_MaterialDisposal_ID == 0)
                    {
                        gv_disposal.PageIndex = 0;
                    }

                    gv_disposal.EditIndex = -1;
                    Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));
                }


            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                bool CanAddEditEP = false;
                AttachmentDisposal.EPYear = EPYear;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                    btnAdd.Enabled = false;
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EPYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnAdd.Enabled = CanAddEditEP;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (gv_disposal.EditIndex == e.Row.RowIndex)
                    {

                        DataKey key = gv_disposal.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string EPWorkID = Convert.ToString(key.Values["EPWorkID"]);
                        string DisposalDate = Convert.ToString(key.Values["DisposalDate"]);
                        string VehicleNumber = Convert.ToString(key.Values["VehicleNumber"]);
                        string BuiltyNumber = Convert.ToString(key.Values["BuiltyNumber"]);

                        string QtyMaterial = Convert.ToString(key.Values["QtyMaterial"]);
                        string Unit = Convert.ToString(key.Values["Unit"]);
                        string Cost = Convert.ToString(key.Values["Cost"]);



                        TextBox txtDate = (TextBox)e.Row.FindControl("txtDate");
                        TextBox txtVehicleNumber = (TextBox)e.Row.FindControl("txtVehicleNumber");
                        TextBox txtBuiltyeNumber = (TextBox)e.Row.FindControl("txtBuiltyeNumber");
                        TextBox txtQtyMaterial = (TextBox)e.Row.FindControl("txtQtyMaterial");
                        TextBox txtUnit = (TextBox)e.Row.FindControl("txtUnit");
                        TextBox txtCost = (TextBox)e.Row.FindControl("txtCost");

                        //    HyperLink hlAttachments = (HyperLink)e.Row.FindControl("hlAttachments");

                        //   hlAttachments.NavigateUrl = "~/Modules/FloodOperations/EmergencyPurchases/AttachmentDisposal.aspx?MD_ID=" + ID.ToString();


                        //  hlAttachments.Enabled = false;


                        if (DisposalDate != null)
                        {
                            txtDate.Text = Convert.ToDateTime(DisposalDate).ToString("dd-MMM-yyyy");
                        }
                        if (VehicleNumber != null)
                        {
                            txtVehicleNumber.Text = VehicleNumber;
                        }
                        if (BuiltyNumber != null)
                        {
                            txtBuiltyeNumber.Text = BuiltyNumber;
                        }
                        if (QtyMaterial != null)
                        {
                            txtQtyMaterial.Text = QtyMaterial;
                        }
                        if (Unit != null)
                        {
                            txtUnit.Text = Unit;
                        }
                        if (Cost != null)
                        {
                            txtCost.Text = Cost;
                        }
                    }
                    #region User Role

                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EPYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnEdit.Enabled = CanAddEditEP;
                        btnDelete.Enabled = CanAddEditEP;
                    }
                    #endregion User Role
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    CanAdd = true;
                    //    CanEdit = true;
                    //    CanDelete = true;
                    //}
                    //else
                    //{
                    //    CanAdd = false;
                    //    CanEdit = false;
                    //    CanDelete = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_disposal.EditIndex = e.NewEditIndex;
                Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));

                //  gv_FloodFighting.Rows[e.NewEditIndex].FindControl("DisposalDate").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gv_disposal.DataKeys[e.RowIndex].Values[0]);
                if (!IsValidDelete(Convert.ToInt64(ID)))
                {
                    return;
                }
                bool IsDeleted = new FloodOperationsBLL().DeleteFo_MaterialDisposal(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_disposal.PageIndex = e.NewPageIndex;
                Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_disposal_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv_disposal.EditIndex = -1;
                Grid_disposal_works(Convert.ToInt32(hdnEPWorkID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private bool IsValidDelete(long ID)
        {
            FloodOperationsBLL bl = new FloodOperationsBLL();
            bool IsExist = bl.IsFo_MaterialDisposalIDExists(ID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }


    }
}