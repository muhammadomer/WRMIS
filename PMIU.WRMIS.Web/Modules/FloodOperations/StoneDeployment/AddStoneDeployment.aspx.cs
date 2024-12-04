using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.StoneDeployment
{
    public partial class AddStoneDeployment : BasePage
    {

        double TotalCost = 0.0;
        double TotalQuantity = 0.0;
        int FFPStonePositionID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    FFPStonePositionID = Utility.GetNumericValueFromQueryString("FFPStonePositionID", 0);
                    if (FFPStonePositionID > 0)
                    {

                        hdnFFPStonePositionID.Value = Convert.ToString(FFPStonePositionID);
                        AttachmentSD._FFPSPID = FFPStonePositionID;
                        //   hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/WorksEmergencyPurchases.aspx?EPWorkID={0}", EPWorkID);



                        //HeaderMaterialDisposal_Show(FFPStonePositionID);
                         BindGrid(FFPStonePositionID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/StoneDeployment/SearchStoneDeployment.aspx?FFPStonePositionID={0}", FFPStonePositionID);
                    }
                    Header_Show();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStoneDeployment.EditIndex = -1;
                BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;

                long StoneDeploymentID = Convert.ToInt64(((Label)gvStoneDeployment.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                DateTime Date = Convert.ToDateTime(((TextBox)gvStoneDeployment.Rows[RowIndex].Cells[1].FindControl("txtDate")).Text);
                string VehicleNumber = ((TextBox)gvStoneDeployment.Rows[RowIndex].Cells[2].FindControl("txtVehicleNumber")).Text.Trim();
                string BuiltyeNumber = ((TextBox)gvStoneDeployment.Rows[RowIndex].Cells[3].FindControl("txtBuiltyeNumber")).Text.Trim();
                string QtyMaterial = ((TextBox)gvStoneDeployment.Rows[RowIndex].Cells[4].FindControl("txtQtyMaterial")).Text.Trim();
                string Cost = ((TextBox)gvStoneDeployment.Rows[RowIndex].Cells[5].FindControl("txtCost")).Text.Trim();


                FO_StoneDeployment mdlStoneDeployment = new FO_StoneDeployment();

                mdlStoneDeployment.ID = StoneDeploymentID;
                mdlStoneDeployment.FFPStonePositionID = Convert.ToInt32(hdnFFPStonePositionID.Value);

                Int64? MaxStoneDeploymentID = null;
                int? PreviousDisposedQty = null;

                if (mdlStoneDeployment.FFPStonePositionID != null && mdlStoneDeployment.FFPStonePositionID > 0)
                {
                    DataSet DS = new FloodFightingPlanBLL().GetBalancedByFFPStonePositionID(mdlStoneDeployment.FFPStonePositionID, mdlStoneDeployment.ID);

                    foreach (DataRow DR in DS.Tables[0].Rows)
                    {
                        if (DR["StoneDeploymentID"] != null && DR["StoneDeploymentID"].ToString() != "")
                        {
                            MaxStoneDeploymentID = Convert.ToInt64(DR["StoneDeploymentID"].ToString());
                            PreviousDisposedQty = Convert.ToInt32(DR["DisposedQty"].ToString());
                        }
                        else
                        { PreviousDisposedQty = 0; }
                        mdlStoneDeployment.Balance = PreviousDisposedQty;
                    }
                }

                //   mdlStoneDeployment.DisposedDate = DateTime.Now;
                mdlStoneDeployment.DisposedDate = Date;
                mdlStoneDeployment.VehicleNumber = VehicleNumber;
                mdlStoneDeployment.BuiltyNo = BuiltyeNumber;
                mdlStoneDeployment.QtyOfStoneDisposed = Convert.ToInt32(QtyMaterial);
                mdlStoneDeployment.Cost = Convert.ToInt32(Cost);

                mdlStoneDeployment.Balance = mdlStoneDeployment.Balance + mdlStoneDeployment.QtyOfStoneDisposed;

                bool IsRecordSaved = false;

                if (StoneDeploymentID == 0)
                {
                    mdlStoneDeployment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlStoneDeployment.CreatedDate = DateTime.Today;
                    IsRecordSaved = new FloodFightingPlanBLL().AddStoneDeployment(mdlStoneDeployment);
                }
                else
                {
                    mdlStoneDeployment.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlStoneDeployment.ModifiedDate = DateTime.Today;
                    IsRecordSaved = new FloodFightingPlanBLL().UpdateStoneDeployment(mdlStoneDeployment);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (StoneDeploymentID == 0)
                    {
                        gvStoneDeployment.PageIndex = 0;
                    }
                    gvStoneDeployment.EditIndex = -1;
                    BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
                }
            }

            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStoneDeployment.EditIndex = e.NewEditIndex;
                BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                long StoneDeploymentID = Convert.ToInt64(((Label)gvStoneDeployment.Rows[e.RowIndex].FindControl("lblID")).Text);
                string ID = Convert.ToString(gvStoneDeployment.DataKeys[e.RowIndex].Values[0]);
                if (!IsValidDelete(Convert.ToInt64(ID)))
                {
                    return;
                }
                bool IsDeleted = new FloodFightingPlanBLL().DeleteStoneDeployment(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
                else
                {
                    BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
                    Master.ShowMessage(Message.RecordNotDeleted.Description);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStoneDeployment.PageIndex = e.NewPageIndex;
                BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                FloodFightingPlanBLL bllFloodFightingPlan = new FloodFightingPlanBLL();
                int Year = bllFloodFightingPlan.GetYearByStonePositioID(FFPStonePositionID);
                Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    btnAdd.Enabled = false;

                    if (new FloodOperationsBLL().CanAddEditStoneDeployment(Year, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnAdd.Enabled = true;
                    }

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) && DateTime.Now.Year == Year)
                    //{
                    //    btnAdd.Enabled = true;   
                    //}
                    //else
                    //{
                    //    btnAdd.Enabled = false;
                    //}

                    //if (Year != DateTime.Now.Year)
                    //{
                    //    Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                    //    btnAdd.Enabled = false;
                    //}
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    if (new FloodOperationsBLL().CanAddEditStoneDeployment(Year, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                    }

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) && DateTime.Now.Year == Year)
                    //{
                    //    btnEdit.Enabled = true;
                    //    btnDelete.Enabled = true;
                    //}
                    //else
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnDelete.Enabled = false;
                    //}
                    //if (Year != DateTime.Now.Year)
                    //{

                    //    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    //    Button btnDelete = (Button)e.Row.FindControl("btnDelete");


                    //    btnEdit.Enabled = false;
                    //    btnDelete.Enabled = false;
                    //}
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalCost = (Label)e.Row.FindControl("lblTotalCost");
                    Label lblTotalQuantity = (Label)e.Row.FindControl("lblTotalQuantity");

                    lblTotalCost.Text = Convert.ToString(string.Format("{0:#,##0.##}", TotalCost));
                    lblTotalQuantity.Text = Convert.ToString(TotalQuantity);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStoneDeployment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<FO_StoneDeployment> lstStoneDeployment = new FloodFightingPlanBLL().GetStoneDeploymentByStonePositionID(Convert.ToInt32(hdnFFPStonePositionID.Value));

                    TotalCost = lstStoneDeployment.Sum(x => Convert.ToDouble(x.Cost));
                    TotalQuantity = lstStoneDeployment.Sum(x => Convert.ToDouble(x.QtyOfStoneDisposed));

                    FO_StoneDeployment mdlStoneDeployment = new FO_StoneDeployment();

                    mdlStoneDeployment.ID = 0;
                    mdlStoneDeployment.DisposedDate = DateTime.Today;
                    mdlStoneDeployment.VehicleNumber = "";
                    mdlStoneDeployment.BuiltyNo = "";
                    mdlStoneDeployment.QtyOfStoneDisposed = 0;
                    mdlStoneDeployment.Cost = 0;


                    lstStoneDeployment.Add(mdlStoneDeployment);


                    gvStoneDeployment.PageIndex = gvStoneDeployment.PageCount;
                    gvStoneDeployment.DataSource = lstStoneDeployment;
                    gvStoneDeployment.DataBind();
                    gvStoneDeployment.EditIndex = gvStoneDeployment.Rows.Count - 1;
                    gvStoneDeployment.DataBind();
                    //  gvStoneDeployment.Rows[gvStoneDeployment.Rows.Count - 1].FindControl("txtDate").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid(int FFPStonePositionID)
        {
            List<FO_StoneDeployment> lstStoneDeployment = new FloodFightingPlanBLL().GetStoneDeploymentByStonePositionID(FFPStonePositionID);

            TotalCost = lstStoneDeployment.Sum(x => Convert.ToDouble(x.Cost));
            TotalQuantity = lstStoneDeployment.Sum(x => Convert.ToDouble(x.QtyOfStoneDisposed));

            gvStoneDeployment.DataSource = lstStoneDeployment;
            gvStoneDeployment.DataBind();
            gvStoneDeployment.Visible = true;


        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvStoneDeployment_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvStoneDeployment.EditIndex = -1;
                BindGrid(Convert.ToInt32(hdnFFPStonePositionID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private bool IsValidDelete(long ID)
        {
            FloodFightingPlanBLL bllFloodFightingPlan = new FloodFightingPlanBLL();
            bool IsExist = bllFloodFightingPlan.IsStoneDeploymentIDExists(ID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        private void Header_Show()
        {


            DataSet DS = new FloodFightingPlanBLL().GetSDAddHeader(Convert.ToInt32(hdnFFPStonePositionID.Value), null, null, null, null, null, null);
            if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                DataRow DR = DS.Tables[0].Rows[0];
                lblInfrastructureType.Text = DR["InfrastructureType"].ToString();
                lblInfrastructureName.Text = DR["InfrastructureName"].ToString();
                lblRD.Text = Convert.ToString(Calculations.GetRDText(Convert.ToInt64(DR["RD"].ToString())));
                lblQtyApp.Text = DR["RequiredQty"].ToString();

                if (DR["InfrastructureType"].ToString() == "Control Structure1")
                {
                    lblRDLabel.Visible = false;
                    lblRD.Visible = false;
                }


            }
        }
    }
}