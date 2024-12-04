using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure
{
    public partial class Gauges : BasePage
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            long ControlinfrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    ControlinfrastructureID = Utility.GetNumericValueFromQueryString("ControlInfrastructureID", 0);

                    if (ControlinfrastructureID > 0)
                    {
                        hdnControlInfrastructureID.Value = Convert.ToString(ControlinfrastructureID);
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Search.aspx?ControlInfrastructureID={0}", ControlinfrastructureID);
                        LoadGaugeInformation(ControlinfrastructureID);
                        ControlledInfrastructureDetails.ID = Convert.ToInt64(ControlinfrastructureID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion Page Load

        #region Set PageTitle
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ControlInfrastructureGauges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set PageTitle

        private void LoadGaugeInformation(long _ControlinfrastructureID)
        {
            try
            {
                List<object> lstGauges = new ControlledInfrastructureBLL().GetGaugesByID(Convert.ToInt64(hdnControlInfrastructureID.Value));
                gvGauges.DataSource = lstGauges;
                gvGauges.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }



        #region Gauges Gridview Method
        protected void gvGauges_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGauges.PageIndex = e.NewPageIndex;
                gvGauges.EditIndex = -1;
                LoadGaugeInformation(Convert.ToInt64(hdnControlInfrastructureID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGauges.EditIndex = -1;
                LoadGaugeInformation(Convert.ToInt64(hdnControlInfrastructureID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddGaugesInformation")
                {
                    List<object> lstGaugesInformation = new ControlledInfrastructureBLL().GetGaugesByID(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    lstGaugesInformation.Add(new
                    {
                        ID = 0,
                        GaugesTypeID = string.Empty,
                        GaugesTypeName = string.Empty,
                        NoOfGauges = string.Empty,
                        UpstreamDownstream = string.Empty,
                        Side = string.Empty,
                        Remarks = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvGauges.PageIndex = gvGauges.PageCount;
                    gvGauges.DataSource = lstGaugesInformation;
                    gvGauges.DataBind();

                    gvGauges.EditIndex = gvGauges.Rows.Count - 1;
                    gvGauges.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvGauges.DataKeys[e.RowIndex].Values[0]);

                bool isDeleted = new ControlledInfrastructureBLL().DeleteGaugesByControlInfrastructure(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    LoadGaugeInformation(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvGauges.EditIndex = e.NewEditIndex;
                LoadGaugeInformation(Convert.ToInt64(hdnControlInfrastructureID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                //string GaugeInformationID = Convert.ToString(gvGauges.DataKeys[e.RowIndex].Values["ID"]);


                #region Data Key
                DataKey key = gvGauges.DataKeys[e.RowIndex];
                string GaugeInformationID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion Data Key
                #region "Controls"
                GridViewRow row = gvGauges.Rows[e.RowIndex];
                DropDownList ddlGaugeType = (DropDownList)row.FindControl("ddlGaugeType");
                DropDownList ddlUpstreamDownstream = (DropDownList)row.FindControl("ddlUpstreamDownstream");
                DropDownList ddlSide = (DropDownList)row.FindControl("ddlSide");

                TextBox txtNoOfGauges = (TextBox)row.FindControl("txtNoOfGauges");
                TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                #endregion

                CO_StructureGauge ObjGauge = new CO_StructureGauge();

                ObjGauge.ID = Convert.ToInt64(GaugeInformationID);
                ObjGauge.StationID = Convert.ToInt64(hdnControlInfrastructureID.Value);

                if (ddlGaugeType != null && !string.IsNullOrEmpty(ddlGaugeType.SelectedItem.Value))
                    ObjGauge.GaugeTypeID = Convert.ToInt64(ddlGaugeType.SelectedItem.Value);

                if (ddlUpstreamDownstream != null)
                    ObjGauge.UsDs = Convert.ToString(ddlUpstreamDownstream.SelectedItem.Value);

                if (ddlSide != null)
                    ObjGauge.Side = Convert.ToString(ddlSide.SelectedItem.Value);

                if (!string.IsNullOrEmpty(txtNoOfGauges.Text))
                {
                    ObjGauge.NoOfGauges = Convert.ToInt16(txtNoOfGauges.Text);
                }
                if (txtRemarks != null)
                {
                    ObjGauge.Remarks = Convert.ToString(txtRemarks.Text);
                }

                if (ObjGauge.ID == 0)
                {

                    ObjGauge.CreatedDate = DateTime.Now;
                    ObjGauge.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                else
                {
                    ObjGauge.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjGauge.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjGauge.ModifiedDate = DateTime.Now;
                    ObjGauge.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }

                bool isSaved = new ControlledInfrastructureBLL().SaveControlInfrastructureGauges(ObjGauge);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(GaugeInformationID) == 0)
                        gvGauges.PageIndex = 0;

                    gvGauges.EditIndex = -1;
                    LoadGaugeInformation(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGauges_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvGauges.EditIndex == e.Row.RowIndex)
                {
                    #region "Data Keys"
                    DataKey key = gvGauges.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values["ID"]);
                    string GaugesTypeID = Convert.ToString(key.Values["GaugesTypeID"]);
                    //   string GaugesTypeName = Convert.ToString(key.Values["GaugesTypeName"]);
                    string NoOfGauges = Convert.ToString(key.Values["NoOfGauges"]);
                    string UpstreamDownstreamID = Convert.ToString(key.Values["UpstreamDownstream"]);
                    string SideID = Convert.ToString(key.Values["Side"]);
                    string Remarks = Convert.ToString(key.Values["Remarks"]);
                    string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                    string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                    #endregion

                    #region "Controls"
                    DropDownList ddlGaugeType = (DropDownList)e.Row.FindControl("ddlGaugeType");
                    DropDownList ddlUpstreamDownstream = (DropDownList)e.Row.FindControl("ddlUpstreamDownstream");
                    DropDownList ddlSide = (DropDownList)e.Row.FindControl("ddlSide");

                    TextBox txtNoOfGauges = (TextBox)e.Row.FindControl("txtNoOfGauges");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                    #endregion

                    if (ddlGaugeType != null)
                    {
                        Dropdownlist.DDLGaugeTypes(ddlGaugeType);

                        if (!string.IsNullOrEmpty(GaugesTypeID))
                            Dropdownlist.SetSelectedValue(ddlGaugeType, GaugesTypeID);
                    }
                    if (ddlSide != null)
                    {
                        Dropdownlist.DDLGaugesSide(ddlSide);

                        if (!string.IsNullOrEmpty(SideID))
                            Dropdownlist.SetSelectedText(ddlSide, SideID);
                    }
                    if (ddlUpstreamDownstream != null)
                    {
                        Dropdownlist.DDLUStreamDStream(ddlUpstreamDownstream);

                        if (!string.IsNullOrEmpty(UpstreamDownstreamID))
                            Dropdownlist.SetSelectedText(ddlUpstreamDownstream, UpstreamDownstreamID);
                    }

                    if (txtNoOfGauges != null)
                        txtNoOfGauges.Text = Convert.ToString(NoOfGauges);

                    if (txtRemarks != null)
                        txtRemarks.Text = Convert.ToString(Remarks);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Gauges Gridview Method
    }
}