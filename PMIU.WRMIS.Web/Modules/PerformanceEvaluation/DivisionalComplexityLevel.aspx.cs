using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class DivisionalComplexityLevel : BasePage
    {
        #region Grid Data Key Index

        public const int DivisionalComplexityIDIndex = 0;
        public const int DivisionIDIndex = 1;
        public const int ComplexityIDIndex = 2;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindZoneDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 21-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 21-09-2016
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 21-09-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == string.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    ddlCircle.Enabled = true;
                }

                gvDivisionalComplexity.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    gvDivisionalComplexity.Visible = false;
                }
                else
                {
                    gvDivisionalComplexity.EditIndex = -1;
                    gvDivisionalComplexity.PageIndex = 0;

                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds divisions to the grid based on the selected Circle ID.        
        /// Created on 22-09-2016
        /// </summary>        
        private void BindGrid()
        {
            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
            List<dynamic> lstDivisionalComplexities = new PerformanceEvaluationBLL().GetDivisionalComplexityLevel(CircleID, Constants.IrrigationDomainID);

            if (lstDivisionalComplexities.Count != 0)
            {
                gvDivisionalComplexity.DataSource = lstDivisionalComplexities;
                gvDivisionalComplexity.DataBind();

                gvDivisionalComplexity.Visible = true;
            }
            else
            {
                Master.ShowMessage(Message.IrrigationDivisionNotAvailable.Description, SiteMaster.MessageType.Error);
                gvDivisionalComplexity.Visible = false;
            }
        }

        protected void gvDivisionalComplexity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDivisionalComplexity.PageIndex = e.NewPageIndex;
                gvDivisionalComplexity.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionalComplexity_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDivisionalComplexity.EditIndex = e.NewEditIndex;

                BindGrid();

                gvDivisionalComplexity.Rows[e.NewEditIndex].FindControl("ddlComplexityLevel").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionalComplexity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDivisionalComplexity.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionalComplexity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvDivisionalComplexity.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlComplexityLevel = (DropDownList)e.Row.FindControl("ddlComplexityLevel");

                        Dropdownlist.DDLComplexityFactor(ddlComplexityLevel, (int)Constants.DropDownFirstOption.NoOption, "ComplexityLevel");

                        long ComplexityLevelID = Convert.ToInt64(gvDivisionalComplexity.DataKeys[e.Row.RowIndex].Values[ComplexityIDIndex]);

                        Dropdownlist.SetSelectedValue(ddlComplexityLevel, ComplexityLevelID.ToString());
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlComplexityLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlComplexityLevel = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlComplexityLevel.NamingContainer;

                Label lblMultiplicationFactor = (Label)gvRow.FindControl("lblMultiplicationFactor");

                long ComplexityFactorID = Convert.ToInt64(ddlComplexityLevel.SelectedItem.Value);

                PE_ComplexityFactor mdlComplexityFactor = new ReferenceDataBLL().GetComplexityFactorByID(ComplexityFactorID);

                lblMultiplicationFactor.Text = mdlComplexityFactor.MultiplicationFactor.ToString();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionalComplexity_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ID = Convert.ToInt64(gvDivisionalComplexity.DataKeys[RowIndex].Values[DivisionalComplexityIDIndex]);
                long DivisionID = Convert.ToInt64(gvDivisionalComplexity.DataKeys[RowIndex].Values[DivisionIDIndex]);
                long ComplexityLevelID = Convert.ToInt64(((DropDownList)gvDivisionalComplexity.Rows[RowIndex].Cells[1].FindControl("ddlComplexityLevel")).SelectedItem.Value);
                string Remarks = ((TextBox)gvDivisionalComplexity.Rows[RowIndex].Cells[3].FindControl("txtRemarks")).Text;

                PerformanceEvaluationBLL bllPerformanceEvaluation = new PerformanceEvaluationBLL();

                PE_DivisionComplexityLevel mdlDivisionComplexityLevel = null;

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                DateTime Now = DateTime.Now;

                bool IsRecordSaved = false;

                if (ID == 0)
                {
                    mdlDivisionComplexityLevel = new PE_DivisionComplexityLevel
                    {
                        DivisionID = DivisionID,
                        ComplexityFactorID = ComplexityLevelID,
                        Remarks = Remarks,
                        CreatedDate = Now,
                        ModifiedDate = Now,
                        CreatedBy = mdlUsers.ID,
                        ModifiedBy = mdlUsers.ID
                    };

                    IsRecordSaved = bllPerformanceEvaluation.AddDivisionComplexityLevel(mdlDivisionComplexityLevel);
                }
                else
                {
                    mdlDivisionComplexityLevel = bllPerformanceEvaluation.GetDivisionComplexityLevelByID(ID);

                    mdlDivisionComplexityLevel.ComplexityFactorID = ComplexityLevelID;
                    mdlDivisionComplexityLevel.Remarks = Remarks;
                    mdlDivisionComplexityLevel.ModifiedDate = Now;
                    mdlDivisionComplexityLevel.ModifiedBy = mdlUsers.ID;

                    IsRecordSaved = bllPerformanceEvaluation.UpdateDivisionComplexityLevel(mdlDivisionComplexityLevel);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.DivisionalComplexitySaved.Description, SiteMaster.MessageType.Success);

                    gvDivisionalComplexity.EditIndex = -1;
                    BindGrid();
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.DivisionalComplexityNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}