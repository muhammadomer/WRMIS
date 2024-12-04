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
    public partial class DailyRate : BasePage
    {
        #region Grid Data Key Index

        public const int DailyRateIDIndex = 0;

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
        /// Created on 30-03-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvDailyRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDailyRate.EditIndex = e.NewEditIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDailyRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDailyRate.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDailyRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long DailyRateID = Convert.ToInt64(gvDailyRate.DataKeys[RowIndex].Values[DailyRateIDIndex]);
                string OrdinaryRateString = ((TextBox)gvDailyRate.Rows[RowIndex].FindControl("txtOrdinaryRate")).Text;
                string SpecialRateString = ((TextBox)gvDailyRate.Rows[RowIndex].FindControl("txtSpecialRate")).Text;
                int BPS = Convert.ToInt32(((Label)gvDailyRate.Rows[RowIndex].FindControl("lblBPS")).Text);

                OrdinaryRateString = OrdinaryRateString.Replace(",", "");
                SpecialRateString = SpecialRateString.Replace(",", "");

                int OrdinaryRate = Convert.ToInt32(OrdinaryRateString);
                int SpecialRate = Convert.ToInt32(SpecialRateString);

                if (SpecialRate < OrdinaryRate)
                {
                    Master.ShowMessage(Message.SpecialRateCannotBeLess.Description, SiteMaster.MessageType.Error);
                    return;
                }

                ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                AT_DailyRate LowerDailyRate = bllReferenceData.GetBPSRate(BPS - 1);

                if (LowerDailyRate != null && OrdinaryRate < LowerDailyRate.OrdinaryRate)
                {
                    Master.ShowMessage(Message.OrdinaryRateLessThanPrevious.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (LowerDailyRate != null && SpecialRate < LowerDailyRate.SpecialRate)
                {
                    Master.ShowMessage(Message.SpecialRateLessThanPrevious.Description, SiteMaster.MessageType.Error);
                    return;
                }

                AT_DailyRate mdlDailyRate = bllReferenceData.GetDailyRateDateByID(DailyRateID);

                mdlDailyRate.OrdinaryRate = OrdinaryRate;
                mdlDailyRate.SpecialRate = SpecialRate;

                bllReferenceData.UpdateDailyRateData(mdlDailyRate);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                gvDailyRate.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds data to the grid
        /// Created On 31-03-2017
        /// </summary>
        private void BindGrid()
        {
            List<AT_DailyRate> lstDailyRate = new ReferenceDataBLL().GetDailyRates();

            gvDailyRate.DataSource = lstDailyRate;
            gvDailyRate.DataBind();

        }

        protected void gvDailyRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDailyRate.PageIndex = e.NewPageIndex;
                gvDailyRate.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}