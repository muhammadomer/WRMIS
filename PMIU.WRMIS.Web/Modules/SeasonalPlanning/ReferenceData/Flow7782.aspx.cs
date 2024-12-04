using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class Flow7782 : BasePage
    {
        public decimal Indus = 0;
        public decimal LKIndus = 0;
        public decimal JC = 0;
        public decimal LKJC = 0;

        #region ViewState

        public string TDailyID = "TDailyID";

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
                gvFlow.Visible = false;

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
                if (ddlSeason.SelectedItem.Text != "")
                {
                    BindData();
                    gvFlow.EditIndex = -1;
                    gvFlow.PageIndex = 0;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindData()
        {
            try
            {
                gvFlow.DataSource = new SeasonalPlanningBLL().GetFlows(Convert.ToInt64(ddlSeason.SelectedItem.Value));
                gvFlow.DataBind();
                gvFlow.Visible = true;

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Flow7782);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvFlow_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvFlow.EditIndex = -1;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFlow_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvFlow.EditIndex = -1;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFlow_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowID = e.RowIndex;
                long ID = Convert.ToInt64(((Label)gvFlow.Rows[RowID].Cells[0].FindControl("lblId")).Text);
                Decimal JC = Convert.ToDecimal(((TextBox)gvFlow.Rows[RowID].Cells[2].FindControl("txtJC")).Text);
                bool Result = new SeasonalPlanningBLL().UpdateFlow7782(ID, JC, Convert.ToInt64(Session[SessionValues.UserID]));
                if (Result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    gvFlow.EditIndex = -1;
                    gvFlow.PageIndex = 1;
                    BindData();
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFlow_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvFlow.EditIndex = e.NewEditIndex;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFlow_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFlow.PageIndex = e.NewPageIndex;
                BindData();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFlow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbTDailyID = (Label)e.Row.FindControl("lblTDailyID");
                    Label lbIndus = (Label)e.Row.FindControl("lblIndus");
                    Label lbJC = (Label)e.Row.FindControl("lblJC");
                    int TDailyID = Convert.ToInt32(lbTDailyID.Text);

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        if (TDailyID >= (int)Constants.SeasonDistribution.EKStart && TDailyID <= (int)Constants.SeasonDistribution.EKEnd)  // Represents Early Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                Indus += Convert.ToDecimal(lbIndus.Text) * Constants.TDailyFactor;
                                JC += Convert.ToDecimal(lbJC.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                Indus += Convert.ToDecimal(lbIndus.Text);
                                JC += Convert.ToDecimal(lbJC.Text);
                            }
                        }
                        else if (TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDailyID <= (int)Constants.SeasonDistribution.LKEnd)  // Represents Late Kharif Case
                        {
                            if (TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily || TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                LKIndus += Convert.ToDecimal(lbIndus.Text) * Constants.TDailyFactor;
                                LKJC += Convert.ToDecimal(lbJC.Text) * Constants.TDailyFactor;
                            }
                            else
                            {
                                LKIndus += Convert.ToDecimal(lbIndus.Text);
                                LKJC += Convert.ToDecimal(lbJC.Text);
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
                                Indus += Convert.ToDecimal(lbIndus.Text) * Constants.TDailyFactor;
                                JC += Convert.ToDecimal(lbJC.Text) * Constants.TDailyFactor;
                            }
                            else if (TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    Indus += Convert.ToDecimal(lbIndus.Text) * Constants.TDailyLeapYearTrue;
                                    JC += Convert.ToDecimal(lbJC.Text) * Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    Indus += Convert.ToDecimal(lbIndus.Text) * Constants.TDailyLeapYearFalse;
                                    JC += Convert.ToDecimal(lbJC.Text) * Constants.TDailyLeapYearFalse;
                                }
                            }
                            else
                            {
                                Indus += Convert.ToDecimal(lbIndus.Text);
                                JC += Convert.ToDecimal(lbJC.Text);
                            }
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        Label lbEKIndus = (Label)e.Row.FindControl("lblInudsEK");
                        if (lbEKIndus != null)
                            lbEKIndus.Text = String.Format("{0:0.000}", Convert.ToDouble(Indus * Constants.MAFFactor));

                        Label lbLKIndus = (Label)e.Row.FindControl("lblIndusLK");
                        if (lbLKIndus != null)
                            lbLKIndus.Text = String.Format("{0:0.000}", Convert.ToDouble(LKIndus * Constants.MAFFactor)); 

                        Label lbTotalIndus = (Label)e.Row.FindControl("lblIndusTotal");
                        if (lbTotalIndus != null)
                            lbTotalIndus.Text = String.Format("{0:0.000}", Convert.ToDouble((Indus * Constants.MAFFactor) + (LKIndus * Constants.MAFFactor))); 

                        Label lbEKJC = (Label)e.Row.FindControl("lblJCEK");
                        if (lbEKJC != null)
                            lbEKJC.Text = String.Format("{0:0.000}", Convert.ToDouble(JC * Constants.MAFFactor)); 

                        Label lbLKJC = (Label)e.Row.FindControl("lblJCLK");
                        if (lbLKJC != null)
                            lbLKJC.Text = String.Format("{0:0.000}", Convert.ToDouble(LKJC * Constants.MAFFactor)); 

                        Label lbTotalJC = (Label)e.Row.FindControl("lblJCTotal");
                        if (lbTotalJC != null)
                            lbTotalJC.Text = String.Format("{0:0.000}", Convert.ToDouble((JC * Constants.MAFFactor) + (LKJC * Constants.MAFFactor))); 
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

                        Label lbEKIndus = (Label)e.Row.FindControl("lblInudsEK");
                        if (lbEKIndus != null)
                            lbEKIndus.Text = String.Format("{0:0.000}", Convert.ToDouble(Indus * Constants.MAFFactor)); 

                        Label lbLKIndus = (Label)e.Row.FindControl("lblIndusLK");
                        if (lbLKIndus != null)
                            lbLKIndus.Visible = false;

                        Label lbEKJC = (Label)e.Row.FindControl("lblJCEK");
                        if (lbEKJC != null)
                            lbEKJC.Text = String.Format("{0:0.000}", Convert.ToDouble(JC * Constants.MAFFactor)); 

                        Label lbLKJC = (Label)e.Row.FindControl("lblJCLK");
                        if (lbLKJC != null)
                            lbLKJC.Visible = false;
                    }
                }
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
                Session[TDailyID] = Convert.ToInt64((sender as LinkButton).CommandArgument);
                BindFlowHistory(Convert.ToInt64(Session[TDailyID]));

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalHistory').modal();", true);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindFlowHistory(long _TDailyCalID)
        {
            try
            {
                gvHistory.DataSource = new SeasonalPlanningBLL().GetFlow7782History(_TDailyCalID);
                gvHistory.DataBind();
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
                BindFlowHistory(Convert.ToInt64(Session[TDailyID]));
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
                BindFlowHistory(Convert.ToInt64(Session[TDailyID]));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}