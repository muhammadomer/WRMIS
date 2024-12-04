using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class TaxRates : BasePage
    {
        #region Grid Data Key Index

        public const int TaxRateIDIndex = 0;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 30-03-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds data to the grid
        /// Created On 30-03-2017
        /// </summary>
        private void BindGrid()
        {
            string TransactionType = ddlTransactionType.SelectedItem.Value;
            string VendorType = ddlVendorType.SelectedItem.Value;

            if (TransactionType.Trim() != string.Empty && VendorType.Trim() != string.Empty)
            {
                List<AT_TaxRate> lstTaxRates = new ReferenceDataBLL().GetTaxData(TransactionType, VendorType);

                gvTaxRates.DataSource = lstTaxRates;
                gvTaxRates.DataBind();

                gvTaxRates.EditIndex = -1;
                gvTaxRates.Visible = true;
            }
            else
            {
                gvTaxRates.Visible = false;
            }
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlVendorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTaxRates_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTaxRates.EditIndex = e.NewEditIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTaxRates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTaxRates.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTaxRates_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long TaxRateID = Convert.ToInt64(gvTaxRates.DataKeys[RowIndex].Values[TaxRateIDIndex]);
                string Percentage = ((TextBox)gvTaxRates.Rows[RowIndex].Cells[1].FindControl("txtTaxRatePercentage")).Text;

                ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                AT_TaxRate mdlTaxRate = bllReferenceData.GetTaxDataByID(TaxRateID);

                mdlTaxRate.TaxRateInPercentage = Percentage == string.Empty ? null : (double?)Convert.ToDouble(Percentage);

                bllReferenceData.UpdateTaxData(mdlTaxRate);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                gvTaxRates.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}