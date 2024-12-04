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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class IStonePosition : BasePage
    {
        #region Initialize

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int floodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (floodInspectionID > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = floodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                        LoadIGCProtectionInfrastructure(floodInspectionID);
                        BindStonePostionGrid(floodInspectionID);
                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Functions

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindStonePostionGrid(int _FloodInspectionID)
        {
            try
            {

                //List<object> lstStonePostion = new FloodInspectionsBLL().GetIStonePositionByFloodInspectionID(_FloodInspectionID);

                //gvStonePosition.DataSource = lstStonePostion;
                //gvStonePosition.DataBind();
                //gvStonePosition.Visible = true;
                IEnumerable<DataRow> IeFloodInspectionStonePosition = new FloodInspectionsBLL().FO_LoadStonePosition(_FloodInspectionID);
                var LsFloodInspectionStonePosition = IeFloodInspectionStonePosition.Select(dataRow => new
                {
                    IStonePositionID = dataRow.Field<long>("IStonePositionID"),
                    RD = Calculations.GetRDText(Convert.ToInt32(dataRow.Field<int>("RD"))),
                    TotalRD = Convert.ToInt32(dataRow.Field<int>("RD")),
                    BeforeFloodQty = dataRow.Field<Int32>("BeforeFloodQty"),
                    AvailableQty = dataRow.Field<Int32>("AvailableQty"),
                    ConsumedQty = dataRow.Field<Int32>("ConsumedQty"),
                    CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                    CreatedBy = dataRow.Field<Int32?>("CreatedBy"),

                }).ToList();
                gvStonePosition.DataSource = LsFloodInspectionStonePosition;
                gvStonePosition.DataBind();
                gvStonePosition.Visible = true;






            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadIGCProtectionInfrastructure(long _FloodInspectionID)
        {
            try
            {
                FO_IGCProtectionInfrastructure iGCProtectionInfrastructure = new FloodInspectionsBLL().GetIGCProtectionInfrastructureByInspectionID(_FloodInspectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }


        }



        #endregion

        #region Event
        protected void gvStonePosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStonePosition.PageIndex = e.NewPageIndex;
                gvStonePosition.EditIndex = -1;
                BindStonePostionGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStonePosition.EditIndex = -1;
                BindStonePostionGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvStonePosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvStonePosition.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteProblemFI(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindStonePostionGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStonePosition.EditIndex = e.NewEditIndex;
                BindStonePostionGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string LeftRD = string.Empty;
                string RightRD = string.Empty;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvStonePosition.Rows[e.RowIndex];

                #region "Data Keys"

                DataKey key = gvStonePosition.DataKeys[e.RowIndex];

                string ID = Convert.ToString(key.Values["IStonePositionID"]);
                string RD = Convert.ToString(key.Values["TotalRD"]);
                string BeforeFloodQty = Convert.ToString(key.Values["BeforeFloodQty"]);
                string AvailableQty = Convert.ToString(key.Values["AvailableQty"]);
                string QuantityConsumed = Convert.ToString(key.Values["ConsumedQty"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvStonePosition.Rows[e.RowIndex];

                Label lblRD = (Label)Row.FindControl("lblRD");
                Label lblQuantityRegistered = (Label)Row.FindControl("lblQuantityRegistered");
                TextBox txtQuantityAvalibletxtToRDLeft = (TextBox)Row.FindControl("txtQuantityAvalible");

                #endregion


                FO_IStonePosition _ObjModel = new FO_IStonePosition();

                _ObjModel.ID = Convert.ToInt64(ID);
                _ObjModel.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (!string.IsNullOrEmpty(RD))
                {
                    //Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt32(RD));
                    //LeftRD = tupleFromRD.Item1;
                    //RightRD = tupleFromRD.Item2;
                    //_ObjModel.RD = Calculations.CalculateTotalRDs(LeftRD, RightRD);
                    _ObjModel.RD = Convert.ToInt32(RD);

                }


                //if (lblQuantityRegistered != null)
                _ObjModel.BeforeFloodQty = Convert.ToInt32(BeforeFloodQty);

                //if (txtQuantityAvalibletxtToRDLeft != null)
                _ObjModel.AvailableQty = Convert.ToInt32(txtQuantityAvalibletxtToRDLeft.Text);

                if (_ObjModel.ID == 0)
                {
                    _ObjModel.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    _ObjModel.CreatedDate = DateTime.Now;
                }
                else
                {
                    _ObjModel.CreatedBy = Convert.ToInt32(CreatedBy);
                    _ObjModel.CreatedDate = Convert.ToDateTime(CreatedDate);
                    _ObjModel.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    _ObjModel.ModifiedDate = DateTime.Now;
                }


                bool IsSave = new FloodInspectionsBLL().SaveIStonePosition(_ObjModel);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(_ObjModel.ID) == 0)
                        gvStonePosition.PageIndex = 0;

                    gvStonePosition.EditIndex = -1;
                    BindStonePostionGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
                bool CanEdit = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // GridViewRow Row = gvStonePosition.Rows[e.Row.RowIndex];
                    //  TextBox txtQuantityAvalibletxtToRDLeft = (TextBox)Row.FindControl("txtQuantityAvalible");
                    //  Button btnbtnEditStonePos = (Button)Row.FindControl("btnEditStonePos");
                    //  TextBox txtQuantityAvalibletxtToRDLeft = (TextBox)e.Row.FindControl("txtQuantityAvalible");
                    Button btnbtnEditStonePos = (Button)e.Row.FindControl("btnEditStonePos");
                    //if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    //{

                    //    btnbtnEditStonePos.Enabled = false;
                    //      txtQuantityAvalibletxtToRDLeft.Enabled = false;
                    //}
                    if (gvStonePosition.EditIndex == e.Row.RowIndex)
                    {
                        GridViewRow Row = gvStonePosition.Rows[e.Row.RowIndex];
                        #region "Data Keys"

                        DataKey key = gvStonePosition.DataKeys[e.Row.RowIndex];

                        string ID = Convert.ToString(key.Values["IStonePositionID"]);
                        string RD = Convert.ToString(key.Values["RD"]);
                        string QuantityRegistered = Convert.ToString(key.Values["BeforeFloodQty"]);
                        string AvailableQty = Convert.ToString(key.Values["AvailableQty"]);
                        string QuantityConsumed = Convert.ToString(key.Values["ConsumedQty"]);
                        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                        #endregion

                        #region "Controls"
                        TextBox txtQuantityAvalibletxtToRDLeft = (TextBox)Row.FindControl("txtQuantityAvalible");

                        Label lblRD = (Label)Row.FindControl("lblRD");
                        Label lblQuantityRegistered = (Label)Row.FindControl("lblQuantityRegistered");

                        Label lblQuantityConsumed = (Label)Row.FindControl("lblQuantityConsumed");

                        #endregion


                        if (lblRD != null)
                            lblRD.Text = RD;

                        if (lblQuantityRegistered != null)
                            lblQuantityRegistered.Text = QuantityRegistered;

                        if (txtQuantityAvalibletxtToRDLeft != null)
                            txtQuantityAvalibletxtToRDLeft.Text = AvailableQty;

                        lblQuantityConsumed.Text = (Convert.ToInt32(QuantityRegistered) - Convert.ToInt32(AvailableQty)).ToString();

                        if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        {
                            btnbtnEditStonePos.Enabled = false;
                            txtQuantityAvalibletxtToRDLeft.Enabled = false;
                        }
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnbtnEditStonePos.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnbtnEditStonePos.Enabled = CanEdit;
                        else
                            btnbtnEditStonePos.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnbtnEditStonePos.Enabled = CanEdit;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //protected void TextChangedEvent(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //        int index = gvRow.RowIndex;
        //        string QtyReg = (gvStonePosition.Rows[index].FindControl("lblQuantityRegistered") as Label).Text;
        //        string QtyAva = (gvStonePosition.Rows[index].FindControl("txtQuantityAvalible") as TextBox).Text;

        //        if (QtyAva != "")
        //        {
        //            (gvStonePosition.Rows[index].FindControl("lblQuantityConsumed") as Label).Text = (Convert.ToInt32(QtyReg) - Convert.ToInt32(QtyAva)).ToString();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        #endregion

    }
}