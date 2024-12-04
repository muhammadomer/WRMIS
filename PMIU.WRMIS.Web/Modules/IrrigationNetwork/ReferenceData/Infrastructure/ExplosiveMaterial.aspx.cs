using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
    public partial class ExplosiveMaterial : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string BreacingSectionID = Utility.GetStringValueFromQueryString("BreachingSectionID", "");
                    hdnBreachingSectionId.Value = BreacingSectionID;
                    FO_InfrastructureBreachingSection ObjBreachingSection = new InfrastructureBLL().GetBreachingSectioneByID(Convert.ToInt64(hdnBreachingSectionId.Value));
                    long ParentInfrastructureID = (long)ObjBreachingSection.ProtectionInfrastructureID;
                    InfrastructureDetail.InfrastructureID = Convert.ToInt64(ParentInfrastructureID);
                    hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/BreachingSection.aspx?BreacingSectionID={0}&InfrastructureID={1}", BreacingSectionID, ParentInfrastructureID);
                    LoadExplosiveMaterial(Convert.ToInt64(BreacingSectionID));


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelParentFeeder);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadExplosiveMaterial(long _BreacingSectionID)
        {
            try
            {
                LoadBreachingSectioneDetail(_BreacingSectionID);
                BindExplosiveMaterialGrid(_BreacingSectionID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindExplosiveMaterialGrid(long _BreacingSectionID)
        {
            try
            {
                List<object> lstExplosiveMaterial = new InfrastructureBLL().GetExplosivesMatetial(Convert.ToInt64(hdnBreachingSectionId.Value));
                gvExplosiveMaterial.DataSource = lstExplosiveMaterial;
                gvExplosiveMaterial.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadBreachingSectioneDetail(long _BreachingSectionID)
        {
            try
            {
                FO_InfrastructureBreachingSection ObjBreachingSection = new InfrastructureBLL().GetBreachingSectioneByID(_BreachingSectionID);
                lblReachRDStart.Text = Calculations.GetRDText(Convert.ToInt64(ObjBreachingSection.FromRD));
                lblReachRDEnd.Text = Calculations.GetRDText(Convert.ToInt64(ObjBreachingSection.ToRD));
                lblNoOfRows.Text = Convert.ToString(ObjBreachingSection.Rows);
                lblNoOfLines.Text = Convert.ToString(ObjBreachingSection.Liners);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetInfrastructureID(long _BreachingSectionID)
        {

        }

        #region ExplosiveMaterial Gridview Methods
        protected void gvExplosiveMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstExplosiveMaterial = new InfrastructureBLL().GetExplosivesMatetial(Convert.ToInt64(hdnBreachingSectionId.Value));
                    lstExplosiveMaterial.Add(new
                    {
                        ID = 0,
                        Custody = string.Empty,
                        CustodyName = string.Empty,
                        Quantity = string.Empty,
                        LocationDescription = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvExplosiveMaterial.PageIndex = gvExplosiveMaterial.PageCount;
                    gvExplosiveMaterial.DataSource = lstExplosiveMaterial;
                    gvExplosiveMaterial.DataBind();

                    gvExplosiveMaterial.EditIndex = gvExplosiveMaterial.Rows.Count - 1;
                    gvExplosiveMaterial.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExplosiveMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvExplosiveMaterial.DataKeys[e.RowIndex].Values[0]);

                bool isDeleted = new InfrastructureBLL().DeleteExplosivesMatetial(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    BindExplosiveMaterialGrid(Convert.ToInt64(hdnBreachingSectionId.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExplosiveMaterial_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvExplosiveMaterial.EditIndex = e.NewEditIndex;
                BindExplosiveMaterialGrid(Convert.ToInt64(hdnBreachingSectionId.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExplosiveMaterial_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                #region "Controls"
                GridViewRow Row = gvExplosiveMaterial.Rows[e.RowIndex];
                DropDownList ddlCustody = (DropDownList)Row.FindControl("ddlCustody");
                TextBox txtQuantity = (TextBox)Row.FindControl("txtQuantity");
                TextBox txtLocationDescription = (TextBox)Row.FindControl("txtLocationDescription");
                #endregion


                #region "Data Keys"

                DataKey key = gvExplosiveMaterial.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CustodyID = Convert.ToString(key.Values["Custody"]);
                //string CustodyName = Convert.ToString(key.Values["CustodyName"]);
                string QuantityID = Convert.ToString(key.Values["Quantity"]);
                string LocationDescriptionID = Convert.ToString(key.Values["LocationDescription"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                FO_BreachingSectionExplosives ObjExplosiveMaterial = new FO_BreachingSectionExplosives();
                ObjExplosiveMaterial.ID = Convert.ToInt64(ID);
                ObjExplosiveMaterial.BreachingSectionID = Convert.ToInt64(hdnBreachingSectionId.Value);
                if (ddlCustody != null)
                {

                    ObjExplosiveMaterial.ExplosiveCustodyID = Convert.ToInt16(ddlCustody.SelectedItem.Value);
                }
                if (txtQuantity != null)
                {
                    ObjExplosiveMaterial.Quantity = Convert.ToString(txtQuantity.Text);
                }
                if (txtLocationDescription != null)
                {
                    ObjExplosiveMaterial.LocationDescription = Convert.ToString(txtLocationDescription.Text);
                }
                if (ObjExplosiveMaterial.ID == 0)
                {

                    ObjExplosiveMaterial.CreatedDate = DateTime.Now;
                    ObjExplosiveMaterial.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                else
                {
                    ObjExplosiveMaterial.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjExplosiveMaterial.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjExplosiveMaterial.ModifiedDate = DateTime.Now;
                    ObjExplosiveMaterial.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }



                bool isSaved = new InfrastructureBLL().SaveExplosivesMatetial(ObjExplosiveMaterial);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ID) == 0)
                        gvExplosiveMaterial.PageIndex = 0;


                    gvExplosiveMaterial.EditIndex = -1;
                    BindExplosiveMaterialGrid(Convert.ToInt64(hdnBreachingSectionId.Value));

                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExplosiveMaterial_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvExplosiveMaterial.EditIndex = -1;
                BindExplosiveMaterialGrid(Convert.ToInt64(hdnBreachingSectionId.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }



        protected void gvExplosiveMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvExplosiveMaterial.EditIndex == e.Row.RowIndex)
                {
                    #region "Data Keys"
                    DataKey key = gvExplosiveMaterial.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values[0]);
                    string CustodyID = Convert.ToString(key.Values[1]);
                    string QuantityID = Convert.ToString(key.Values[3]);
                    string LocationDescriptionID = Convert.ToString(key.Values[4]);
                    string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                    string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                    #endregion

                    #region "Controls"
                    DropDownList ddlCustody = (DropDownList)e.Row.FindControl("ddlCustody");
                    TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                    TextBox txtLocationDescription = (TextBox)e.Row.FindControl("txtLocationDescription");
                    #endregion

                    if (ddlCustody != null)
                    {
                        Dropdownlist.DDLCustody(ddlCustody, (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlCustody, CustodyID);
                    }
                    if (txtQuantity != null)
                        txtQuantity.Text = QuantityID;
                    if (txtLocationDescription != null)
                        txtLocationDescription.Text = LocationDescriptionID;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvExplosiveMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvExplosiveMaterial.PageIndex = e.NewPageIndex;
                gvExplosiveMaterial.EditIndex = -1;
                BindExplosiveMaterialGrid(Convert.ToInt64(hdnBreachingSectionId.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion ExplosiveMaterial Gridview Methods

    }
}