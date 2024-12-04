using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ReferenceData
{
    public partial class KPISubCategories : BasePage
    {
        #region Page Constants

        public const string TotalCategory = "Total (Included KPISubCategories)";
        public const int MaxPositiveValue = 100;
        public const int MinNegativeValue = -100;
        public const int MinPositiveMaxNegativeValue = 0;

        #endregion

        #region Grid Data Key Index

        public const int KPISubCategoryIDIndex = 0;
        public const int PEIncludeIndex = 1;
        public const int BaseWeightIndex = 2;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    hdnCategID.Value = Convert.ToString(Utility.GetNumericValueFromQueryString("CatgID", 0));
                    SetPageTitle();
                    BindKPIDropdown();
                    SubCategory();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 16-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the KPI Dropdown
        /// Created On 16-09-2016
        /// </summary>
        private void BindKPIDropdown()
        {
            Dropdownlist.DDLKPICategories(ddlKPICategory);
            ddlKPICategory.SelectedValue = hdnCategID.Value;
        }

        protected void ddlKPICategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvKPISubCategories.PageIndex = 0;
                gvKPISubCategories.EditIndex = -1;

                if (ddlKPICategory.SelectedItem.Value != string.Empty)
                {
                    BindGrid();
                    pnlContent.Visible = true;
                }
                else
                {
                    pnlContent.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SubCategory()
        {
            try
            {
                gvKPISubCategories.PageIndex = 0;
                gvKPISubCategories.EditIndex = -1;

                if (ddlKPICategory.SelectedItem.Value != string.Empty)
                {
                    BindGrid();
                    pnlContent.Visible = true;
                }
                else
                {
                    pnlContent.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the grid.
        /// Created on 07-09-2016
        /// </summary>
        private void BindGrid()
        {
            long KPICategoryID = Convert.ToInt64(ddlKPICategory.SelectedItem.Value);

            List<PE_KPISubCategories> lstKPISubCategories = new ReferenceDataBLL().GetKPISubCategories(KPICategoryID);

            foreach (PE_KPISubCategories mdlKPISubCategories in lstKPISubCategories)
            {
                if (mdlKPISubCategories.PEInclude == false)
                {
                    mdlKPISubCategories.WeightDefault = 0;
                    mdlKPISubCategories.WeightCurrent = 0;
                }
            }

            lstKPISubCategories.Add(new PE_KPISubCategories
            {
                ID = 0,
                Name = TotalCategory,
                WeightDefault = lstKPISubCategories.Select(ksc => ksc.WeightDefault).Sum(),
                WeightCurrent = lstKPISubCategories.Select(ksc => ksc.WeightCurrent).Sum(),
                Source = string.Empty,
                Description = string.Empty,
                PEInclude = false
            });

            gvKPISubCategories.DataSource = lstKPISubCategories;
            gvKPISubCategories.DataBind();
        }

        protected void gvKPISubCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvKPISubCategories.EditIndex = e.NewEditIndex;

                BindGrid();

                gvKPISubCategories.Rows[e.NewEditIndex].FindControl("txtWeightCurrent").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPISubCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvKPISubCategories.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPISubCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");
                    Label lblWeightDefault = (Label)e.Row.FindControl("lblWeightDefault");
                    CheckBox cbPEInclude = (CheckBox)e.Row.FindControl("cbPEInclude");

                    if (lblCategoryName.Text.Trim().ToUpper() == TotalCategory.Trim().ToUpper())
                    {
                        Label lblWeightCurrent = (Label)e.Row.FindControl("lblWeightCurrent");
                        Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");

                        lblCategoryName.Font.Bold = true;
                        lblWeightDefault.Font.Bold = true;
                        lblWeightCurrent.Font.Bold = true;
                        lblDescription.Visible = false;
                        cbPEInclude.Visible = false;
                        btnEdit.Visible = false;
                    }
                    else
                    {
                        if (e.Row.RowIndex == gvKPISubCategories.EditIndex)
                        {
                            TextBox txtWeightCurrent = (TextBox)e.Row.FindControl("txtWeightCurrent");

                            if (cbPEInclude.Checked)
                            {
                                txtWeightCurrent.CssClass = "form-control decimalIntegerInput required";
                                txtWeightCurrent.Enabled = true;

                                double? BaseWeight = null;

                                if (gvKPISubCategories.DataKeys[e.Row.RowIndex].Values[BaseWeightIndex] != null)
                                {
                                    BaseWeight = Convert.ToDouble(gvKPISubCategories.DataKeys[e.Row.RowIndex].Values[BaseWeightIndex]);
                                }

                                double WeightDefault = Convert.ToDouble(lblWeightDefault.Text);

                                if (BaseWeight == null)
                                {
                                    if (WeightDefault > 0)
                                    {
                                        txtWeightCurrent.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinPositiveMaxNegativeValue + "','" + MaxPositiveValue + "');");
                                    }
                                    else
                                    {
                                        txtWeightCurrent.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinNegativeValue + "','" + MinPositiveMaxNegativeValue + "');");
                                    }
                                }
                                else
                                {
                                    if (WeightDefault > 0)
                                    {
                                        txtWeightCurrent.Attributes.Add("oninput", "javascript:ValueValidationMultiples(this,'" + MinPositiveMaxNegativeValue + "','" + MaxPositiveValue + "','" + BaseWeight + "');");
                                    }
                                    else
                                    {
                                        txtWeightCurrent.Attributes.Add("oninput", "javascript:ValueValidationMultiples(this,'" + MinNegativeValue + "','" + MinPositiveMaxNegativeValue + "','" + BaseWeight + "');");
                                    }
                                }
                            }
                            else
                            {
                                txtWeightCurrent.CssClass = "form-control";
                                txtWeightCurrent.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKPISubCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long KPICategoryID = Convert.ToInt64(ddlKPICategory.SelectedItem.Value);
                long KPISubCategoryID = Convert.ToInt64(gvKPISubCategories.DataKeys[RowIndex].Values[KPISubCategoryIDIndex]);
                double CurrentWeight = Convert.ToDouble(((TextBox)gvKPISubCategories.Rows[RowIndex].Cells[2].FindControl("txtWeightCurrent")).Text);
                string Description = ((TextBox)gvKPISubCategories.Rows[RowIndex].Cells[4].FindControl("txtDescription")).Text;
                bool PEIncludeCurrent = ((CheckBox)gvKPISubCategories.Rows[RowIndex].Cells[5].FindControl("cbPEInclude")).Checked;
                bool PEIncludeOld = Convert.ToBoolean(gvKPISubCategories.DataKeys[RowIndex].Values[PEIncludeIndex]);

                bool IsRecordSaved = false;

                using (TransactionScope TransactionScope = new TransactionScope())
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                    PE_KPISubCategories mdlKPISubCategories = bllReferenceData.GetKPISubCategoryByID(KPISubCategoryID);

                    if (PEIncludeOld)
                    {
                        mdlKPISubCategories.WeightCurrent = CurrentWeight;
                    }

                    mdlKPISubCategories.Description = Description.Trim();
                    mdlKPISubCategories.PEInclude = PEIncludeCurrent;
                    mdlKPISubCategories.ModifiedBy = mdlUser.ID;
                    mdlKPISubCategories.ModifiedDate = DateTime.Now;

                    IsRecordSaved = bllReferenceData.UpdateKPISubCategory(mdlKPISubCategories);

                    if (PEIncludeCurrent != PEIncludeOld)
                    {
                        List<PE_KPISubCategories> lstKPISubCategories = bllReferenceData.GetKPISubCategoriesByKPICategoryID(KPICategoryID);

                        bool PEIncludeParent = lstKPISubCategories.Any(ksc => ksc.PEInclude == true);

                        PE_KPICategories mdlKPICategories = bllReferenceData.GetKPICategoryByID(KPICategoryID);

                        if (PEIncludeParent != mdlKPICategories.PEInclude)
                        {
                            mdlKPICategories.PEInclude = PEIncludeParent;
                            mdlKPICategories.ModifiedBy = mdlUser.ID;
                            mdlKPICategories.ModifiedDate = DateTime.Now;

                            bllReferenceData.UpdateKPICategory(mdlKPICategories);
                        }
                    }

                    TransactionScope.Complete();
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    gvKPISubCategories.EditIndex = -1;
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

                    long KPICategoryID = Convert.ToInt64(ddlKPICategory.SelectedItem.Value);

                    PE_KPICategories mdlKPICategories = bllReferenceData.GetKPICategoryByID(KPICategoryID);

                    mdlKPICategories.PEInclude = true;
                    mdlKPICategories.ModifiedBy = mdlUser.ID;
                    mdlKPICategories.ModifiedDate = DateTime.Now;

                    bllReferenceData.UpdateKPICategory(mdlKPICategories);

                    List<PE_KPISubCategories> lstKPISubCategories = bllReferenceData.GetKPISubCategories(KPICategoryID);

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

                Master.ShowMessage(Message.KPISubCategoriesReset.Description, SiteMaster.MessageType.Success);

                gvKPISubCategories.EditIndex = -1;
                gvKPISubCategories.PageIndex = 0;

                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.KPISubCategoriesNotReset.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}