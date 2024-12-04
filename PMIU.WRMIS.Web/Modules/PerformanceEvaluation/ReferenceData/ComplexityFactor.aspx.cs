using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ReferenceData
{
    public partial class ComplexityFactor : BasePage
    {
        #region Grid Data Key Index

        public const int ComplexityFactorIDIndex = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 31-08-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the grid.
        /// Created on 31-08-2016
        /// </summary>
        private void BindGrid()
        {
            List<PE_ComplexityFactor> lstComplexityFactor = new ReferenceDataBLL().GetComplexityFactors();

            gvComplexityFactor.DataSource = lstComplexityFactor;
            gvComplexityFactor.DataBind();
        }

        protected void gvComplexityFactor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvComplexityFactor.EditIndex = e.NewEditIndex;

                BindGrid();

                gvComplexityFactor.Rows[e.NewEditIndex].FindControl("txtComplexityFactor").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplexityFactor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvComplexityFactor.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplexityFactor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ComplexityFactorID = Convert.ToInt64(gvComplexityFactor.DataKeys[RowIndex].Values[ComplexityFactorIDIndex]);
                double ComplexityFactor = Convert.ToDouble(((TextBox)gvComplexityFactor.Rows[RowIndex].Cells[1].FindControl("txtComplexityFactor")).Text);
                string ComplexityLevel = ((Label)gvComplexityFactor.Rows[RowIndex].Cells[0].FindControl("lblComplexityLevel")).Text;

                double LowerComplexity = 0.99;
                double UpperComplexity = 100.01;
                int LowerIndex = RowIndex - 1;
                int UpperIndex = RowIndex + 1;
                string LowerComplexityLevel = string.Empty;
                string UpperComplexityLevel = string.Empty;
                string ErrorMessage = string.Empty;

                if (LowerIndex >= 0)
                {
                    LowerComplexity = Convert.ToDouble(((Label)gvComplexityFactor.Rows[LowerIndex].Cells[1].FindControl("lblComplexityFactor")).Text);
                    LowerComplexityLevel = ((Label)gvComplexityFactor.Rows[LowerIndex].Cells[0].FindControl("lblComplexityLevel")).Text;
                }

                if (UpperIndex <= gvComplexityFactor.Rows.Count - 1)
                {
                    UpperComplexity = Convert.ToDouble(((Label)gvComplexityFactor.Rows[UpperIndex].Cells[1].FindControl("lblComplexityFactor")).Text);
                    UpperComplexityLevel = ((Label)gvComplexityFactor.Rows[UpperIndex].Cells[0].FindControl("lblComplexityLevel")).Text;
                }

                if (ComplexityFactor <= LowerComplexity)
                {
                    ErrorMessage = string.Format("The Complexity Factor of {0} cannot be less than or equal to {1}", ComplexityLevel, LowerComplexityLevel);
                    Master.ShowMessage(ErrorMessage, SiteMaster.MessageType.Error);
                    return;
                }

                if (ComplexityFactor >= UpperComplexity)
                {
                    ErrorMessage = string.Format("The Complexity Factor of {0} cannot be greater than or equal to {1}", ComplexityLevel, UpperComplexityLevel);
                    Master.ShowMessage(ErrorMessage, SiteMaster.MessageType.Error);
                    return;
                }

                double MultiplicationFactor = Math.Round(ComplexityFactor / 10, 2);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                PE_ComplexityFactor mdlComplexityFactor = bllReferenceData.GetComplexityFactorByID(ComplexityFactorID);

                mdlComplexityFactor.ComplexityFactor = ComplexityFactor;
                mdlComplexityFactor.MultiplicationFactor = MultiplicationFactor;
                mdlComplexityFactor.ModifiedBy = mdlUser.ID;
                mdlComplexityFactor.ModifiedDate = DateTime.Now;

                bool IsRecordSaved = bllReferenceData.UpdateComplexityFactor(mdlComplexityFactor);

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    gvComplexityFactor.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComplexityFactor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblComplexityLevel = (Label)e.Row.FindControl("lblComplexityLevel");

                    if (lblComplexityLevel.Text.Trim().ToUpper() == Constants.NormalComplexityLevelName.Trim().ToUpper())
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        btnEdit.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope TransactionScope = new TransactionScope())
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                    List<PE_ComplexityFactor> lstComplexityFactor = bllReferenceData.GetComplexityFactors();

                    foreach (PE_ComplexityFactor mdlComplexityFactor in lstComplexityFactor)
                    {
                        mdlComplexityFactor.ComplexityFactor = mdlComplexityFactor.ComplexityFactorDefault;
                        mdlComplexityFactor.MultiplicationFactor = mdlComplexityFactor.MultiplicationFactorDefault;
                        mdlComplexityFactor.ModifiedBy = mdlUser.ID;
                        mdlComplexityFactor.ModifiedDate = DateTime.Now;

                        bllReferenceData.UpdateComplexityFactor(mdlComplexityFactor);
                    }

                    TransactionScope.Complete();
                }

                Master.ShowMessage(Message.ComplexityFactorReset.Description, SiteMaster.MessageType.Success);

                gvComplexityFactor.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.ComplexityFactorNotReset.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}