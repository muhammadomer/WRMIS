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
    public partial class KPICategories : BasePage
    {
        #region Page Constants

        public const string TotalCategory = "Total (Included KPICategories)";
        public const string FieldValue = "Field";
        public const string PMIUValue = "PMIU";

        #endregion

        #region Grid Data Key Index

        public const int KPICategoryIDIndex = 0;
        public const int PEIncludeIndex = 1;

        #endregion

        #region Global variables

        List<PE_KPISubCategories> lstKPISubCategories = null;

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
        /// Created on 06-09-2016
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
        /// Created on 07-09-2016
        /// </summary>
        private void BindGrid()
        {
            ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

            lstKPISubCategories = bllReferenceData.GetKPISubCategories();

            List<PE_KPICategories> lstKPICategories = bllReferenceData.GetKPICategories();

            lstKPICategories.Add(new PE_KPICategories
            {
                ID = 0,
                Name = TotalCategory,
                Description = string.Empty,
                PEInclude = false
            });

            gvKPICategories.DataSource = lstKPICategories;
            gvKPICategories.DataBind();
        }

        protected void gvKPICategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvKPICategories.EditIndex = e.NewEditIndex;

                BindGrid();

                gvKPICategories.Rows[e.NewEditIndex].FindControl("txtDescription").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPICategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvKPICategories.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPICategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");
                    Label lblWeightFieldDefault = (Label)e.Row.FindControl("lblWeightFieldDefault");
                    Label lblWeightFieldCurrent = (Label)e.Row.FindControl("lblWeightFieldCurrent");
                    Label lblWeightPMIUDefault = (Label)e.Row.FindControl("lblWeightPMIUDefault");
                    Label lblWeightPMIUCurrent = (Label)e.Row.FindControl("lblWeightPMIUCurrent");

                    if (lblCategoryName.Text.Trim().ToUpper() == TotalCategory.Trim().ToUpper())
                    {
                        Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                        CheckBox cbPEInclude = (CheckBox)e.Row.FindControl("cbPEInclude");
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        HyperLink hlKpisubcat = (HyperLink)e.Row.FindControl("hlKpisubcat");

                        lblCategoryName.Font.Bold = true;

                        double TotalFieldDefault = lstKPISubCategories.Where(ksc => ksc.Source == FieldValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightDefault.Value);

                        lblWeightFieldDefault.Text = TotalFieldDefault.ToString();
                        lblWeightFieldDefault.Font.Bold = true;

                        double TotalFieldCurrent = lstKPISubCategories.Where(ksc => ksc.Source == FieldValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightCurrent.Value);

                        lblWeightFieldCurrent.Text = TotalFieldCurrent.ToString();
                        lblWeightFieldCurrent.Font.Bold = true;

                        double TotalPMIUDefault = lstKPISubCategories.Where(ksc => ksc.Source == PMIUValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightDefault.Value);

                        lblWeightPMIUDefault.Text = TotalPMIUDefault.ToString();
                        lblWeightPMIUDefault.Font.Bold = true;

                        double TotalPMIUCurrent = lstKPISubCategories.Where(ksc => ksc.Source == PMIUValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightCurrent.Value);

                        lblWeightPMIUCurrent.Text = TotalPMIUCurrent.ToString();
                        lblWeightPMIUCurrent.Font.Bold = true;

                        lblDescription.Visible = false;
                        cbPEInclude.Visible = false;
                        btnEdit.Visible = false;
                        hlKpisubcat.Visible = false;
                    }
                    else
                    {
                        int RowIndex = e.Row.RowIndex;

                        long KPICategoryID = Convert.ToInt64(gvKPICategories.DataKeys[RowIndex].Values[KPICategoryIDIndex]);

                        if (lstKPISubCategories.Any(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == FieldValue))
                        {
                            double WeightFieldDefault = lstKPISubCategories.Where(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == FieldValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightDefault.Value);

                            lblWeightFieldDefault.Text = WeightFieldDefault.ToString();

                            double WeightFieldCurrent = lstKPISubCategories.Where(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == FieldValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightCurrent.Value);

                            lblWeightFieldCurrent.Text = WeightFieldCurrent.ToString();
                        }

                        if (lstKPISubCategories.Any(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == PMIUValue))
                        {
                            double WeightPMIUDefault = lstKPISubCategories.Where(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == PMIUValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightDefault.Value);

                            lblWeightPMIUDefault.Text = WeightPMIUDefault.ToString();

                            double WeightPMIUCurrent = lstKPISubCategories.Where(ksc => ksc.KPICategoryID == KPICategoryID && ksc.Source == PMIUValue && ksc.PEInclude == true).Sum(ksc => ksc.WeightCurrent.Value);

                            lblWeightPMIUCurrent.Text = WeightPMIUCurrent.ToString();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPICategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long KPICategoryID = Convert.ToInt64(gvKPICategories.DataKeys[RowIndex].Values[KPICategoryIDIndex]);
                string Description = ((TextBox)gvKPICategories.Rows[RowIndex].Cells[5].FindControl("txtDescription")).Text;
                bool PEIncludeCurrent = ((CheckBox)gvKPICategories.Rows[RowIndex].Cells[6].FindControl("cbPEInclude")).Checked;
                bool PEIncludeOld = Convert.ToBoolean(gvKPICategories.DataKeys[RowIndex].Values[PEIncludeIndex]);

                bool IsRecordSaved = false;

                using (TransactionScope TransactionScope = new TransactionScope())
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                    PE_KPICategories mdlKPICategories = bllReferenceData.GetKPICategoryByID(KPICategoryID);

                    mdlKPICategories.Description = Description.Trim();
                    mdlKPICategories.PEInclude = PEIncludeCurrent;
                    mdlKPICategories.ModifiedBy = mdlUser.ID;
                    mdlKPICategories.ModifiedDate = DateTime.Now;

                    IsRecordSaved = bllReferenceData.UpdateKPICategory(mdlKPICategories);

                    if (PEIncludeCurrent != PEIncludeOld)
                    {
                        List<PE_KPISubCategories> lstKPISubCategories = bllReferenceData.GetKPISubCategoriesByKPICategoryID(KPICategoryID);

                        foreach (PE_KPISubCategories mdlKPISubCategories in lstKPISubCategories)
                        {
                            mdlKPISubCategories.PEInclude = PEIncludeCurrent;
                            mdlKPISubCategories.ModifiedBy = mdlUser.ID;
                            mdlKPISubCategories.ModifiedDate = DateTime.Now;

                            bllReferenceData.UpdateKPISubCategory(mdlKPISubCategories);
                        }
                    }

                    TransactionScope.Complete();
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    gvKPICategories.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
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

                    List<PE_KPICategories> lstKPICategories = bllReferenceData.GetKPICategories();

                    foreach (PE_KPICategories mdlKPICategories in lstKPICategories)
                    {
                        mdlKPICategories.PEInclude = true;
                        mdlKPICategories.ModifiedBy = mdlUser.ID;
                        mdlKPICategories.ModifiedDate = DateTime.Now;

                        bllReferenceData.UpdateKPICategory(mdlKPICategories);
                    }

                    lstKPISubCategories = bllReferenceData.GetKPISubCategories();

                    foreach (PE_KPISubCategories mdlKPISubCategories in lstKPISubCategories)
                    {
                        mdlKPISubCategories.WeightCurrent = mdlKPISubCategories.WeightDefault;
                        mdlKPISubCategories.PEInclude = true;
                        mdlKPISubCategories.ModifiedBy = mdlUser.ID;
                        mdlKPISubCategories.ModifiedDate = DateTime.Now;

                        bllReferenceData.UpdateKPISubCategory(mdlKPISubCategories);
                    }

                    TransactionScope.Complete();
                }

                Master.ShowMessage(Message.KPICategoriesReset.Description, SiteMaster.MessageType.Success);

                gvKPICategories.EditIndex = -1;
                gvKPICategories.PageIndex = 0;

                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.KPICategoriesNotReset.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}