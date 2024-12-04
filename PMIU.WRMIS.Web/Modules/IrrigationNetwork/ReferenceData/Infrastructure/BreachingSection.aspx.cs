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
    public partial class BreachingSection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string ParentInfrastructureID = Utility.GetStringValueFromQueryString("InfrastructureID", "");
                    hdnProtectioninfrastructure.Value = ParentInfrastructureID;
                    InfrastructureDetail.InfrastructureID = Convert.ToInt64(ParentInfrastructureID);
                    hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID={0}", ParentInfrastructureID);
                    BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
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

        #region BreachingSection Gridview Methods

        private void BindBreachingSectionGrid(long _InfrastructureID)
        {
            try
            {
                List<object> lstBreachingSection = new InfrastructureBLL().GetBreachingSection(Convert.ToInt64(hdnProtectioninfrastructure.Value));
                gvBreachingSection.DataSource = lstBreachingSection;
                gvBreachingSection.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvBreachingSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBreachingSection.PageIndex = e.NewPageIndex;
                gvBreachingSection.EditIndex = -1;
                BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBreachingSection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvBreachingSection.EditIndex = -1;
                BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvBreachingSection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddBreachingSection")
                {
                    List<object> lstBreachingSection = new InfrastructureBLL().GetBreachingSection(Convert.ToInt64(hdnProtectioninfrastructure.Value));
                    lstBreachingSection.Add(new
                    {
                        ID = 0,
                        StartingRDTotal = string.Empty,
                        EndingRDTotal = string.Empty,
                        Rows = string.Empty,
                        Liners = string.Empty,
                        StartingRD = string.Empty,
                        EndingRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvBreachingSection.PageIndex = gvBreachingSection.PageCount;
                    gvBreachingSection.DataSource = lstBreachingSection;
                    gvBreachingSection.DataBind();

                    gvBreachingSection.EditIndex = gvBreachingSection.Rows.Count - 1;
                    gvBreachingSection.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBreachingSection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvBreachingSection.DataKeys[e.RowIndex].Values[0]);

                bool isDeleted = new InfrastructureBLL().DeleteBreachingSection(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBreachingSection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvBreachingSection.EditIndex = e.NewEditIndex;
                BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBreachingSection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                #region "Controls"
                GridViewRow Row = gvBreachingSection.Rows[e.RowIndex];
                TextBox txtStartingRDLeft = (TextBox)Row.FindControl("txtStartingRDLeft");
                TextBox txtStartingRDRight = (TextBox)Row.FindControl("txtStartingRDRight");

                TextBox txtEndingRDLeft = (TextBox)Row.FindControl("txtEndingRDLeft");
                TextBox txtEndingRDRight = (TextBox)Row.FindControl("txtEndingRDRight");
                TextBox txtRows = (TextBox)Row.FindControl("txtNoOfRows");
                TextBox txtLiners = (TextBox)Row.FindControl("txtNoOfLiners");
                Button btnExplosiveDetail = (Button)Row.FindControl("btnExplosiveDetail");
                #endregion

                DataKey key = gvBreachingSection.DataKeys[e.RowIndex];
                string BreachingSectionID = Convert.ToString(key.Values[0]);
                string EndingRDTotal = Convert.ToString(key.Values[2]);
                string CreatedBy = Convert.ToString(key.Values[7]);
                string CreatedDate = Convert.ToString(key.Values[8]);


                FO_InfrastructureBreachingSection ObjBreachingSection = new FO_InfrastructureBreachingSection();

                ObjBreachingSection.ID = Convert.ToInt64(BreachingSectionID);
                ObjBreachingSection.ProtectionInfrastructureID = Convert.ToInt64(hdnProtectioninfrastructure.Value);

                if (txtStartingRDLeft != null && txtStartingRDRight != null)
                    ObjBreachingSection.FromRD = Calculations.CalculateTotalRDs(txtStartingRDLeft.Text, txtStartingRDRight.Text);

                if (txtEndingRDLeft != null && txtEndingRDRight != null)
                    ObjBreachingSection.ToRD = Calculations.CalculateTotalRDs(txtEndingRDLeft.Text, txtEndingRDRight.Text);

                if (txtRows != null)
                {
                    ObjBreachingSection.Rows = Convert.ToInt32(txtRows.Text);
                }
                if (txtLiners != null)
                {
                    ObjBreachingSection.Liners = Convert.ToInt32(txtLiners.Text);
                }

                if (ObjBreachingSection.ID == 0)
                {

                    ObjBreachingSection.CreatedDate = DateTime.Now;
                    ObjBreachingSection.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                else
                {
                    ObjBreachingSection.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjBreachingSection.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjBreachingSection.ModifiedDate = DateTime.Now;
                    ObjBreachingSection.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }


                if (ObjBreachingSection.FromRD >= ObjBreachingSection.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }


                bool isSaved = new InfrastructureBLL().SaveBreachingSection(ObjBreachingSection);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(BreachingSectionID) == 0)
                        gvBreachingSection.PageIndex = 0;


                    gvBreachingSection.EditIndex = -1;
                    BindBreachingSectionGrid(Convert.ToInt64(hdnProtectioninfrastructure.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBreachingSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string StartingRDLeft = string.Empty;
            string StartingRDRight = string.Empty;
            string EndingRDLeft = string.Empty;
            string EndingRDRight = string.Empty;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvBreachingSection.EditIndex == e.Row.RowIndex)
                {
                    #region "Data Keys"
                    DataKey key = gvBreachingSection.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values[0]);
                    string StartingRD = Convert.ToString(key.Values[1]);
                    string EndingRD = Convert.ToString(key.Values[2]);
                    string Rows = Convert.ToString(key.Values[3]);
                    string Liners = Convert.ToString(key.Values[4]);
                    #endregion

                    #region "Controls"
                    TextBox txtStartingRDLeft = (TextBox)e.Row.FindControl("txtStartingRDLeft");
                    TextBox txtStartingRDRight = (TextBox)e.Row.FindControl("txtStartingRDRight");

                    TextBox txtEndingRDLeft = (TextBox)e.Row.FindControl("txtEndingRDLeft");
                    TextBox txtEndingRDRight = (TextBox)e.Row.FindControl("txtEndingRDRight");
                    TextBox txtRows = (TextBox)e.Row.FindControl("txtNoOfRows");
                    TextBox txtLiners = (TextBox)e.Row.FindControl("txtNoOfLiners");

                    Label lblEditStartingRD = (Label)e.Row.FindControl("lblEditStartingRD");
                    Panel pnlStartingRD = (Panel)e.Row.FindControl("pnlStartingRD");
                    Label lblEditEndingRD = (Label)e.Row.FindControl("lblEditEndingRD");
                    Panel pnlEndingRD = (Panel)e.Row.FindControl("pnlEndingRD");
                    HyperLink hlExplosiveDetail = (HyperLink)e.Row.FindControl("hlExplosiveDetail");

                    #endregion

                    if (hlExplosiveDetail != null && String.IsNullOrEmpty(ID) == false && ID == "0")
                    {
                        hlExplosiveDetail.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(StartingRD) && lblEditStartingRD != null)
                        lblEditStartingRD.Text = Calculations.GetRDText(Convert.ToInt64(StartingRD));


                    if (!string.IsNullOrEmpty(StartingRD))
                    {
                        Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(StartingRD));
                        StartingRDLeft = tupleRD.Item1;
                        StartingRDRight = tupleRD.Item2;
                    }
                    if (txtStartingRDLeft != null)
                        txtStartingRDLeft.Text = StartingRDLeft;
                    if (txtStartingRDRight != null)
                        txtStartingRDRight.Text = StartingRDRight;


                    if (!string.IsNullOrEmpty(EndingRD))
                    {
                        Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(EndingRD));
                        EndingRDLeft = tupleRD.Item1;
                        EndingRDRight = tupleRD.Item2;
                    }
                    if (txtEndingRDLeft != null)
                        txtEndingRDLeft.Text = EndingRDLeft;
                    if (txtEndingRDRight != null)
                        txtEndingRDRight.Text = EndingRDRight;

                    if (txtRows != null)
                        txtRows.Text = Rows;

                    if (txtLiners != null)
                        txtLiners.Text = Rows;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion BreachingSection Gridview Methods




    }

}