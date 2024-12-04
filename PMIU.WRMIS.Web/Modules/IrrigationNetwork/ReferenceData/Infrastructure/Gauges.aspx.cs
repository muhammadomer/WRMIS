using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
    public partial class Gauges : BasePage
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            long ProtectioninfrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    ProtectioninfrastructureID = Utility.GetNumericValueFromQueryString("InfrastructureID", 0);

                    if (ProtectioninfrastructureID > 0)
                    {
                        hdnProtectionInfrastructureID.Value = Convert.ToString(ProtectioninfrastructureID);
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID={0}", ProtectioninfrastructureID);
                        LoadGaugeInformation(ProtectioninfrastructureID);
                        InfrastructureDetail.InfrastructureID = Convert.ToInt64(ProtectioninfrastructureID);
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

        private void LoadGaugeInformation(long _InfrastructureID)
        {
            try
            {
                List<object> lstGauges = new InfrastructureBLL().GetGaugesByProtectionInfrastructureID(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
                LoadGaugeInformation(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
                LoadGaugeInformation(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
                    List<object> lstGaugesInformation = new InfrastructureBLL().GetGaugesByProtectionInfrastructureID(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
                    lstGaugesInformation.Add(new
                    {
                        ID = 0,
                        GaugesTypeID = string.Empty,
                        GaugesTypeName = string.Empty,
                        GaugeRDTotal = string.Empty,
                        GaugeRD = string.Empty,
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

                bool isDeleted = new InfrastructureBLL().DeleteGaugesByID(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    LoadGaugeInformation(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
                LoadGaugeInformation(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
                #region Data Key
                DataKey key = gvGauges.DataKeys[e.RowIndex];
                string GaugeInformationID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion Data Key
                #region "Controls"
                GridViewRow row = gvGauges.Rows[e.RowIndex];
                DropDownList ddlGaugeType = (DropDownList)row.FindControl("ddlGaugeType");
                TextBox txtGaugeRDLeft = (TextBox)row.FindControl("txtGaugeRDLeft");
                TextBox txtGaugeRDRight = (TextBox)row.FindControl("txtGaugeRDRight");

                long infrastructureTypeID = Utility.GetNumericValueFromQueryString("InfrastructureTypeID", 0);
                #endregion

                FO_FloodGauge ObjGauge = new FO_FloodGauge();

                ObjGauge.ID = Convert.ToInt64(GaugeInformationID);
                ObjGauge.StructureID = Convert.ToInt64(hdnProtectionInfrastructureID.Value);
                ObjGauge.StructureTypeID = infrastructureTypeID;

                if (ddlGaugeType != null && !string.IsNullOrEmpty(ddlGaugeType.SelectedItem.Value))
                    ObjGauge.GaugeTypeID = Convert.ToInt64(ddlGaugeType.SelectedItem.Value);

                if (txtGaugeRDLeft != null & txtGaugeRDRight != null)
                    ObjGauge.GaugeRD = Calculations.CalculateTotalRDs(txtGaugeRDLeft.Text, txtGaugeRDRight.Text);

                if (new InfrastructureBLL().IsFloodGaugeExists(ObjGauge))
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
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

                bool isSaved = new InfrastructureBLL().SaveProtectionInfrastructureGauges(ObjGauge);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(GaugeInformationID) == 0)
                        gvGauges.PageIndex = 0;

                    gvGauges.EditIndex = -1;
                    LoadGaugeInformation(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
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
            string GaugeLeftRD = string.Empty;
            string GaugeRightRD = string.Empty;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvGauges.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"
                        DataKey key = gvGauges.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string GaugesTypeID = Convert.ToString(key.Values["GaugesTypeID"]);
                        //   string GaugesTypeName = Convert.ToString(key.Values["GaugesTypeName"]);
                        string GaugeRD = Convert.ToString(key.Values["GaugeRDTotal"]);
                        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                        #endregion

                        #region "Controls"
                        DropDownList ddlGaugeType = (DropDownList)e.Row.FindControl("ddlGaugeType");
                        TextBox txtGaugeRDLeft = (TextBox)e.Row.FindControl("txtGaugeRDLeft");
                        TextBox txtGaugeRDRight = (TextBox)e.Row.FindControl("txtGaugeRDRight");
                        #endregion

                        if (ddlGaugeType != null)
                        {
                            Dropdownlist.DDLGaugeTypesForFloodbund(ddlGaugeType);
                            if (!string.IsNullOrEmpty(GaugesTypeID))
                                Dropdownlist.SetSelectedValue(ddlGaugeType, GaugesTypeID);
                        }
                        if (!string.IsNullOrEmpty(GaugeRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(GaugeRD));
                            GaugeLeftRD = tupleFromRD.Item1;
                            GaugeRightRD = tupleFromRD.Item2;
                        }

                        if (txtGaugeRDLeft != null)
                            txtGaugeRDLeft.Text = GaugeLeftRD;
                        if (txtGaugeRDRight != null)
                            txtGaugeRDRight.Text = GaugeRightRD;
                    }

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