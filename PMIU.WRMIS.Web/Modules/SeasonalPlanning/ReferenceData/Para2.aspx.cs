using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class Para2 : BasePage
    {
        public decimal EK = 0;
        public decimal LK = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                    SetTitle();
                }
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
                gvPara.DataSource = new SeasonalPlanningBLL().GetPara2();
                gvPara.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Para2);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvPara_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvPara.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPara_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPara.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPara_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbTDailyID = (Label)e.Row.FindControl("lblID");
                    Label lblPageTotal = (Label)e.Row.FindControl("lblIndus");
                    int TDailyID = Convert.ToInt32(lbTDailyID.Text);

                    if (TDailyID >= (int)Constants.SeasonDistribution.EKStart && TDailyID <= (int)Constants.SeasonDistribution.EKEnd)  // Represents Early Kharif Case
                    {
                        if (TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            EK += Convert.ToDecimal(lblPageTotal.Text) * Constants.TDailyFactor;
                        else
                            EK += Convert.ToDecimal(lblPageTotal.Text);
                    }
                    else if (TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDailyID <= (int)Constants.SeasonDistribution.LKEnd)  // Represents Late Kharif Case
                    {
                        if (TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily || TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            LK += Convert.ToDecimal(lblPageTotal.Text) * Constants.TDailyFactor;
                        else
                            LK += Convert.ToDecimal(lblPageTotal.Text);
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lbEK = (Label)e.Row.FindControl("lblEK");
                    if (lbEK != null)
                        lbEK.Text = String.Format("{0:0.000}", Convert.ToDouble(EK * Constants.MAFFactor));

                    Label lbLK = (Label)e.Row.FindControl("lblLK");
                    if (lbLK != null)
                        lbLK.Text = String.Format("{0:0.000}", Convert.ToDouble(LK * Constants.MAFFactor));

                    Label lbTotal = (Label)e.Row.FindControl("lblTotal");
                    if (lbTotal != null)
                        lbTotal.Text = String.Format("{0:0.000}", Convert.ToDouble((EK * Constants.MAFFactor) + (LK * Constants.MAFFactor)));
                    gvPara.ShowFooter = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}