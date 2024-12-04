using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class ShareDistribution : BasePage
    {
        #region Variables

        public decimal Bal = 0;
        public decimal BalLK = 0;
        public decimal KPK = 0;
        public decimal KPKLK = 0;
        public decimal HistPunj = 0;
        public decimal HistPunjLK = 0;
        public decimal HistSindh = 0;
        public decimal HistSindhLK = 0;
        public decimal ParaPunjab = 0;
        public decimal ParaPunjabLK = 0;
        public decimal ParaSindh = 0;
        public decimal ParaSindhLK = 0;

        #endregion

        #region ViewStates

        public String TDailyCalID = "TDailyCalID";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindSeasonDropDown();
                    SetTitle();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindSeasonDropDown()
        {
            try
            {
                //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                //ddlSeason.DataTextField = "Name";
                //ddlSeason.DataValueField = "ID";
                //ddlSeason.DataBind();
                Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());
                gvShare.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value != "")
                {
                    BindGrid();
                    gvShare.Visible = true;
                }
                else
                    gvShare.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            try
            {
                gvShare.DataSource = new SeasonalPlanningBLL().GetShareDistribution(Convert.ToInt64(ddlSeason.SelectedItem.Value));
                gvShare.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ShareDistribution);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvShare_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvShare.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvShare.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowID = e.RowIndex;
                long RecordID = Convert.ToInt64(((Label)gvShare.Rows[RowID].Cells[0].FindControl("lblId")).Text);
                decimal Balochistan = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[2].FindControl("txtBalochistan")).Text);
                decimal KPK = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[3].FindControl("txtKPK")).Text);
                decimal HistPunj = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[4].FindControl("txtHistPunj")).Text);
                decimal HistSindh = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[5].FindControl("txtHistSindh")).Text);
                decimal? ParaPunj = null;
                decimal? paraSindh = null;
                if (((TextBox)gvShare.Rows[RowID].Cells[6].FindControl("txtParaPunj")).Text != "")
                    ParaPunj = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[6].FindControl("txtParaPunj")).Text);
                if (((TextBox)gvShare.Rows[RowID].Cells[7].FindControl("txtParaSindh")).Text != "")
                    paraSindh = Convert.ToDecimal(((TextBox)gvShare.Rows[RowID].Cells[7].FindControl("txtParaSindh")).Text);

                bool Result = new SeasonalPlanningBLL().UpdateShareDistribution(RecordID, Balochistan, KPK, HistPunj, HistSindh, ParaPunj, paraSindh, Convert.ToInt64(Session[SessionValues.UserID]));
                if (Result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    gvShare.EditIndex = -1;
                    gvShare.PageIndex = 1;
                    BindGrid();
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvShare.EditIndex = e.NewEditIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvShare.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ((ddlSeason.SelectedItem.Value != "Select") && (Convert.ToInt64(ddlSeason.SelectedItem.Value) == (int)Constants.Seasons.Rabi))
                    {
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbTDailyID = (Label)e.Row.FindControl("lblTDailyID");
                    Label lbBal = (Label)e.Row.FindControl("lblBalochistan");
                    Label lbKPK = (Label)e.Row.FindControl("lblKPK");
                    Label lbHistPunj = (Label)e.Row.FindControl("lblHistPunj");
                    Label lbHistSindh = (Label)e.Row.FindControl("lblHistSindh");
                    Label lbParaPunj = (Label)e.Row.FindControl("lblParaPunj");
                    Label lbParaSindh = (Label)e.Row.FindControl("lblParasindh");

                    int TDailyID = Convert.ToInt32(lbTDailyID.Text);

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        if (TDailyID >= (int)Constants.SeasonDistribution.EKStart && TDailyID <= (int)Constants.SeasonDistribution.EKEnd)  // Represents Early Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                Bal += Convert.ToDecimal(lbBal.Text) * Constants.TDailyFactor;
                                KPK += Convert.ToDecimal(lbKPK.Text) * Constants.TDailyFactor;
                                HistPunj += Convert.ToDecimal(lbHistPunj.Text) * Constants.TDailyFactor;
                                HistSindh += Convert.ToDecimal(lbHistSindh.Text) * Constants.TDailyFactor;
                                ParaPunjab += Convert.ToDecimal(lbParaPunj.Text) * Constants.TDailyFactor;
                                ParaSindh += Convert.ToDecimal(lbParaSindh.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                Bal += Convert.ToDecimal(lbBal.Text);
                                KPK += Convert.ToDecimal(lbKPK.Text);
                                HistPunj += Convert.ToDecimal(lbHistPunj.Text);
                                HistSindh += Convert.ToDecimal(lbHistSindh.Text);
                                ParaPunjab += Convert.ToDecimal(lbParaPunj.Text);
                                ParaSindh += Convert.ToDecimal(lbParaSindh.Text);
                            }
                        }
                        else if (TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDailyID <= (int)Constants.SeasonDistribution.LKEnd)  // Represents Late Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily || TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                BalLK += Convert.ToDecimal(lbBal.Text) * Constants.TDailyFactor;
                                KPKLK += Convert.ToDecimal(lbKPK.Text) * Constants.TDailyFactor;
                                HistPunjLK += Convert.ToDecimal(lbHistPunj.Text) * Constants.TDailyFactor;
                                HistSindhLK += Convert.ToDecimal(lbHistSindh.Text) * Constants.TDailyFactor;
                                ParaPunjabLK += Convert.ToDecimal(lbParaPunj.Text) * Constants.TDailyFactor;
                                ParaSindhLK += Convert.ToDecimal(lbParaSindh.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                BalLK += Convert.ToDecimal(lbBal.Text);
                                KPKLK += Convert.ToDecimal(lbKPK.Text);
                                HistPunjLK += Convert.ToDecimal(lbHistPunj.Text);
                                HistSindhLK += Convert.ToDecimal(lbHistSindh.Text);
                                ParaPunjabLK += Convert.ToDecimal(lbParaPunj.Text);
                                ParaSindhLK += Convert.ToDecimal(lbParaSindh.Text);
                            }
                        }
                    }
                    else   // Rabi Case
                    {
                        if (TDailyID >= (int)Constants.SeasonDistribution.RabiStart && TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily || TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                || TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily || TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                            {
                                Bal += Convert.ToDecimal(lbBal.Text) * Constants.TDailyFactor;
                                KPK += Convert.ToDecimal(lbKPK.Text) * Constants.TDailyFactor;
                                HistPunj += Convert.ToDecimal(lbHistPunj.Text) * Constants.TDailyFactor;
                                HistSindh += Convert.ToDecimal(lbHistSindh.Text) * Constants.TDailyFactor;
                            }
                            else if (TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    Bal += Convert.ToDecimal(lbBal.Text) * Constants.TDailyLeapYearTrue;
                                    KPK += Convert.ToDecimal(lbKPK.Text) * Constants.TDailyLeapYearTrue;
                                    HistPunj += Convert.ToDecimal(lbHistPunj.Text) * Constants.TDailyLeapYearTrue;
                                    HistSindh += Convert.ToDecimal(lbHistSindh.Text) * Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    Bal += Convert.ToDecimal(lbBal.Text) * Constants.TDailyLeapYearFalse;
                                    KPK += Convert.ToDecimal(lbKPK.Text) * Constants.TDailyLeapYearFalse;
                                    HistPunj += Convert.ToDecimal(lbHistPunj.Text) * Constants.TDailyLeapYearFalse;
                                    HistSindh += Convert.ToDecimal(lbHistSindh.Text) * Constants.TDailyLeapYearFalse;
                                }
                            }
                            else
                            {
                                Bal += Convert.ToDecimal(lbBal.Text);
                                KPK += Convert.ToDecimal(lbKPK.Text);
                                HistPunj += Convert.ToDecimal(lbHistPunj.Text);
                                HistSindh += Convert.ToDecimal(lbHistSindh.Text);
                            }
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        Label lbEKBal = (Label)e.Row.FindControl("lblBalEK");
                        if (lbEKBal != null)
                            lbEKBal.Text = String.Format("{0:0.000}", Convert.ToDouble(Bal * Constants.MAFFactor));

                        Label lbLKBal = (Label)e.Row.FindControl("lblBalLK");
                        if (lbLKBal != null)
                            lbLKBal.Text = String.Format("{0:0.000}", Convert.ToDouble(BalLK * Constants.MAFFactor));

                        Label lbTotalBal = (Label)e.Row.FindControl("lblBalTotal");
                        if (lbTotalBal != null)
                            lbTotalBal.Text = String.Format("{0:0.000}", Convert.ToDouble((Bal * Constants.MAFFactor) + (BalLK * Constants.MAFFactor))); 
                        
                        Label lbEKKPK = (Label)e.Row.FindControl("lblKPKEK");
                        if (lbEKKPK != null)
                            lbEKKPK.Text = String.Format("{0:0.000}", Convert.ToDouble(KPK * Constants.MAFFactor));  

                        Label lbLKKPK = (Label)e.Row.FindControl("lblKPKLK");
                        if (lbLKKPK != null)
                            lbLKKPK.Text = String.Format("{0:0.000}", Convert.ToDouble(KPKLK * Constants.MAFFactor));  

                        Label lbTotalKPK = (Label)e.Row.FindControl("lblKPKTotal");
                        if (lbTotalKPK != null)
                            lbTotalKPK.Text = String.Format("{0:0.000}", Convert.ToDouble((KPK * Constants.MAFFactor) + (KPKLK * Constants.MAFFactor))); 

                        Label lbEKHistPunj = (Label)e.Row.FindControl("lblHistPunjEK");
                        if (lbEKHistPunj != null)
                            lbEKHistPunj.Text = String.Format("{0:0.000}", Convert.ToDouble(HistPunj * Constants.MAFFactor));  

                        Label lbLKHistPunj = (Label)e.Row.FindControl("lblHistPunjLK");
                        if (lbLKHistPunj != null)
                            lbLKHistPunj.Text = String.Format("{0:0.000}", Convert.ToDouble(HistPunjLK * Constants.MAFFactor)); 

                        Label lbTotalHistPunj = (Label)e.Row.FindControl("lblHistPunjTotal");
                        if (lbTotalHistPunj != null)
                            lbTotalHistPunj.Text = String.Format("{0:0.000}", Convert.ToDouble((HistPunj * Constants.MAFFactor) + (HistPunjLK * Constants.MAFFactor))); 

                        Label lbEKHistSindh = (Label)e.Row.FindControl("lblHistSindhEK");
                        if (lbEKHistSindh != null)
                            lbEKHistSindh.Text = String.Format("{0:0.000}", Convert.ToDouble(HistSindh * Constants.MAFFactor)); 

                        Label lbLKHistsindh = (Label)e.Row.FindControl("lblHistSindhLK");
                        if (lbLKHistsindh != null)
                            lbLKHistsindh.Text = String.Format("{0:0.000}", Convert.ToDouble(HistSindhLK * Constants.MAFFactor)); 

                        Label lbTotalHistsindh = (Label)e.Row.FindControl("lblHistSindhTotal");
                        if (lbTotalHistsindh != null)
                            lbTotalHistsindh.Text = String.Format("{0:0.000}", Convert.ToDouble((HistSindh * Constants.MAFFactor) + (HistSindhLK * Constants.MAFFactor))); 
                        
                        Label lbEKParaPunj = (Label)e.Row.FindControl("lblParaPunjEK");
                        if (lbEKParaPunj != null)
                            lbEKParaPunj.Text = String.Format("{0:0.000}", Convert.ToDouble(ParaPunjab * Constants.MAFFactor)); 

                        Label lbLKParaPunj = (Label)e.Row.FindControl("lblParaPunjLK");
                        if (lbLKParaPunj != null)
                            lbLKParaPunj.Text = String.Format("{0:0.000}", Convert.ToDouble(ParaPunjabLK * Constants.MAFFactor)); 

                        Label lbTotalParaPunj = (Label)e.Row.FindControl("lblParaPunjTotal");
                        if (lbTotalParaPunj != null)
                            lbTotalParaPunj.Text = String.Format("{0:0.000}", Convert.ToDouble((ParaPunjab * Constants.MAFFactor) + (ParaPunjabLK * Constants.MAFFactor)));
                        
                        Label lbEKParaSindh = (Label)e.Row.FindControl("lblParaSindhEK");
                        if (lbEKParaSindh != null)
                            lbEKParaSindh.Text = String.Format("{0:0.000}", Convert.ToDouble(ParaSindh * Constants.MAFFactor)); 

                        Label lbLKParasindh = (Label)e.Row.FindControl("lblParaSindhLK");
                        if (lbLKParasindh != null)
                            lbLKParasindh.Text = String.Format("{0:0.000}", Convert.ToDouble(ParaSindhLK * Constants.MAFFactor)); 

                        Label lbTotalParaSindh = (Label)e.Row.FindControl("lblParaSindhTotal");
                        if (lbTotalParaSindh != null)
                            lbTotalParaSindh.Text = String.Format("{0:0.000}", Convert.ToDouble((ParaSindh * Constants.MAFFactor) + (ParaSindhLK * Constants.MAFFactor))); 
                    }
                    else  //Rabi Case
                    {
                        Label lbEKPeriod = (Label)e.Row.FindControl("lblPeriodEK");
                        if (lbEKPeriod != null)
                            lbEKPeriod.Text = "Rabi (MAF)";

                        Label lbLKPeriod = (Label)e.Row.FindControl("lblPeriodLK");
                        if (lbLKPeriod != null)
                            lbLKPeriod.Visible = false;

                        Label lbTotalPeriod = (Label)e.Row.FindControl("lblPeriodTotal");
                        if (lbTotalPeriod != null)
                            lbTotalPeriod.Visible = false;

                        Label lbEKBal = (Label)e.Row.FindControl("lblBalEK");
                        if (lbEKBal != null)
                            lbEKBal.Text = String.Format("{0:0.000}", Convert.ToDouble(Bal * Constants.MAFFactor)); 

                        Label lbLKBal = (Label)e.Row.FindControl("lblBalLK");
                        if (lbLKBal != null)
                            lbLKBal.Visible = false;

                        Label lbEKKPK = (Label)e.Row.FindControl("lblKPKEK");
                        if (lbEKKPK != null)
                            lbEKKPK.Text = String.Format("{0:0.000}", Convert.ToDouble(KPK * Constants.MAFFactor)); 

                        Label lbLKKPK = (Label)e.Row.FindControl("lblKPKLK");
                        if (lbLKKPK != null)
                            lbLKKPK.Visible = false;

                        Label lbEKHistPunj = (Label)e.Row.FindControl("lblHistPunjEK");
                        if (lbEKHistPunj != null)
                            lbEKHistPunj.Text = String.Format("{0:0.000}", Convert.ToDouble(HistPunj * Constants.MAFFactor)); 

                        Label lbLKHistPunj = (Label)e.Row.FindControl("lblHistPunjLK");
                        if (lbLKHistPunj != null)
                            lbLKHistPunj.Visible = false;
                        
                        Label lbEKHistSindh = (Label)e.Row.FindControl("lblHistSindhEK");
                        if (lbEKHistSindh != null)
                            lbEKHistSindh.Text = String.Format("{0:0.000}", Convert.ToDouble(HistSindh * Constants.MAFFactor)); 

                        Label lbLKHistsindh = (Label)e.Row.FindControl("lblHistSindhLK");
                        if (lbLKHistsindh != null)
                            lbLKHistsindh.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryGrid(long _TDailyCalID)
        {
            try
            {
                gvHistory.DataSource = new SeasonalPlanningBLL().GetShareDistributionHistory(_TDailyCalID);
                gvHistory.DataBind();
                lblName.Text = "History";
                lblName.ForeColor = Color.White;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState[TDailyCalID] = Convert.ToInt64((sender as LinkButton).CommandArgument);
                BindHistoryGrid(Convert.ToInt64(ViewState[TDailyCalID]));

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalHistory').modal();", true);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHistory.PageIndex = e.NewPageIndex;
                BindHistoryGrid(Convert.ToInt64(ViewState[TDailyCalID]));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvHistory.EditIndex = -1;
                BindHistoryGrid(Convert.ToInt64(ViewState[TDailyCalID]));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}