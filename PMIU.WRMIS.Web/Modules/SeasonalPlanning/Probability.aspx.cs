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

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class Probability : BasePage
    {
        #region Variables

        public decimal Zero = 0;
        public decimal ZeroLK = 0;
        public decimal Five = 0;
        public decimal FiveLK = 0;
        public decimal Ten = 0;
        public decimal TenLK = 0;
        public decimal Fifteen = 0;
        public decimal FifteenLK = 0;
        public decimal Twenty = 0;
        public decimal TwentyLK = 0;
        public decimal TwentyFive = 0;
        public decimal TwentyFiveLK = 0;
        public decimal Thirty = 0;
        public decimal ThirtyLK = 0;
        public decimal ThirtyFive = 0;
        public decimal ThirtyFiveLK = 0;
        public decimal Fourty = 0;
        public decimal FourtyLK = 0;
        public decimal FourtyFive = 0;
        public decimal FourtyFiveLK = 0;
        public decimal Fifty = 0;
        public decimal FiftyLK = 0;
        public decimal FiftyFive = 0;
        public decimal FiftyFiveLK = 0;
        public decimal Sixty = 0;
        public decimal SixtyLK = 0;
        public decimal SixtyFive = 0;
        public decimal SixtyFiveLK = 0;
        public decimal Seventy = 0;
        public decimal SeventyLK = 0;
        public decimal SeventyFive = 0;
        public decimal SeventyFiveLK = 0;
        public decimal Eighty = 0;
        public decimal EightyLK = 0;
        public decimal EightyFive = 0;
        public decimal EightyFiveLK = 0;
        public decimal Ninety = 0;
        public decimal NinetyLK = 0;
        public decimal NinetyFive = 0;
        public decimal NinetyFiveLK = 0;
        public decimal Hundred = 0;
        public decimal HundredLK = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#Alert').modal();", true);
                BindRimStation();
            }
        }

        public void BindRimStation()
        {
            try
            {
                ddlRimStatn.DataSource = CommonLists.GetAllRimStations();
                ddlRimStatn.DataTextField = "Name";
                ddlRimStatn.DataValueField = "ID";
                ddlRimStatn.DataBind();
                gvProbability.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                gvProbability.DataSource = new List<string>();
                gvProbability.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#Alert').modal();", false);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProbability_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView GV = (GridView)sender;

                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Period";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Maximum";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "5%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "10%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "15%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "20%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "25%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "30%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "35%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "40%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "45%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "50%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "55%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "60%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "65%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "70%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "75%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "80%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "85%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "90%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "95%";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Minimum";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    gvProbability.Controls[0].Controls.AddAt(0, HeaderRow);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProbability_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int SeasonID = Convert.ToInt32(ddlSeason.SelectedItem.Value);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTenDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Kharif(MAF)" || lbl.Text == "Rabi(MAF)"))
                        e.Row.Font.Bold = true;
                }
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    Label lbID = (Label)e.Row.FindControl("lblID");
                //    Label lbZeroVol = (Label)e.Row.FindControl("lblZeroDis");
                //    Label lbFiveVol = (Label)e.Row.FindControl("lblFiveDis");
                //    Label lbTenVol = (Label)e.Row.FindControl("lblTenDis");
                //    Label lbFifteenVol = (Label)e.Row.FindControl("lblFifteenDis");
                //    Label lbTwentyVol = (Label)e.Row.FindControl("lblTwentyDis");
                //    Label lbTwentyFiveVol = (Label)e.Row.FindControl("lblTwentyFiveDis");
                //    Label lbThirtyVol = (Label)e.Row.FindControl("lblThirtyDis");
                //    Label lbThirtyFiveVol = (Label)e.Row.FindControl("lblThirtyFiveDis");
                //    Label lbFourtyVol = (Label)e.Row.FindControl("lblFourtyDis");
                //    Label lbFourtyFiveVol = (Label)e.Row.FindControl("lblFourtyFiveDis");
                //    Label lbFiftyVol = (Label)e.Row.FindControl("lblFiftyDis");
                //    Label lbFiftyFiveVol = (Label)e.Row.FindControl("lblFiftyFiveDis");
                //    Label lbSixtyVol = (Label)e.Row.FindControl("lblSixtyDis");
                //    Label lbSixtyFiveVol = (Label)e.Row.FindControl("lblSixtyFiveDis");
                //    Label lbSeventyVol = (Label)e.Row.FindControl("lblSeventyDis");
                //    Label lbSeventyFiveVol = (Label)e.Row.FindControl("lblSeventyFiveDis");
                //    Label lbEightyVol = (Label)e.Row.FindControl("lblEightyDis");
                //    Label lbEightyFiveVol = (Label)e.Row.FindControl("lblEightyFiveDis");
                //    Label lbNinetyVol = (Label)e.Row.FindControl("lblNinetyDis");
                //    Label lbNinetyFiveVol = (Label)e.Row.FindControl("lblNinetyFiveDis");
                //    Label lbMinimumVol = (Label)e.Row.FindControl("lblMinimumDis");

                //    int TDailyID = Convert.ToInt32(lbID.Text);

                //    if (SeasonID == (int)Constants.Seasons.Kharif)
                //    {
                //        if (TDailyID >= (int)Constants.SeasonDistribution.EKStart && TDailyID <= (int)Constants.SeasonDistribution.EKEnd)  // Represents Early Kharif Case
                //        {
                //            if (TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                //            {
                //                Zero += Convert.ToDecimal(lbZeroVol.Text) * Constants.TDailyFactor;
                //                Five += Convert.ToDecimal(lbFiveVol.Text) * Constants.TDailyFactor;
                //                Ten += Convert.ToDecimal(lbTenVol.Text) * Constants.TDailyFactor;
                //                Fifteen += Convert.ToDecimal(lbFifteenVol.Text) * Constants.TDailyFactor;
                //                Twenty += Convert.ToDecimal(lbTwentyVol.Text) * Constants.TDailyFactor;
                //                TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text) * Constants.TDailyFactor;
                //                Thirty += Convert.ToDecimal(lbThirtyVol.Text) * Constants.TDailyFactor;
                //                ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text) * Constants.TDailyFactor;
                //                Fourty += Convert.ToDecimal(lbFourtyVol.Text) * Constants.TDailyFactor;
                //                FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text) * Constants.TDailyFactor;
                //                Fifty += Convert.ToDecimal(lbFiftyVol.Text) * Constants.TDailyFactor;
                //                FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text) * Constants.TDailyFactor;
                //                Sixty += Convert.ToDecimal(lbSixtyVol.Text) * Constants.TDailyFactor;
                //                SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text) * Constants.TDailyFactor;
                //                Seventy += Convert.ToDecimal(lbSeventyVol.Text) * Constants.TDailyFactor;
                //                SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text) * Constants.TDailyFactor;
                //                Eighty += Convert.ToDecimal(lbEightyVol.Text) * Constants.TDailyFactor;
                //                EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text) * Constants.TDailyFactor;
                //                Ninety += Convert.ToDecimal(lbNinetyVol.Text) * Constants.TDailyFactor;
                //                NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text) * Constants.TDailyFactor;
                //                Hundred += Convert.ToDecimal(lbMinimumVol.Text) * Constants.TDailyFactor;
                //            }
                //            else
                //            {
                //                Zero += Convert.ToDecimal(lbZeroVol.Text);
                //                Five += Convert.ToDecimal(lbFiveVol.Text);
                //                Ten += Convert.ToDecimal(lbTenVol.Text);
                //                Fifteen += Convert.ToDecimal(lbFifteenVol.Text);
                //                Twenty += Convert.ToDecimal(lbTwentyVol.Text);
                //                TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text);
                //                Thirty += Convert.ToDecimal(lbThirtyVol.Text);
                //                ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text);
                //                Fourty += Convert.ToDecimal(lbFourtyVol.Text);
                //                FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text);
                //                Fifty += Convert.ToDecimal(lbFiftyVol.Text);
                //                FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text);
                //                Sixty += Convert.ToDecimal(lbSixtyVol.Text);
                //                SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text);
                //                Seventy += Convert.ToDecimal(lbSeventyVol.Text);
                //                SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text);
                //                Eighty += Convert.ToDecimal(lbEightyVol.Text);
                //                EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text);
                //                Ninety += Convert.ToDecimal(lbNinetyVol.Text);
                //                NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text);
                //                Hundred += Convert.ToDecimal(lbMinimumVol.Text);
                //            }
                //        }
                //        else if (TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDailyID <= (int)Constants.SeasonDistribution.LKEnd)  // Represents Late Kharif Case
                //        {
                //            if (TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily || TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                //            {
                //                ZeroLK += Convert.ToDecimal(lbZeroVol.Text) * Constants.TDailyFactor;
                //                FiveLK += Convert.ToDecimal(lbFiveVol.Text) * Constants.TDailyFactor;
                //                TenLK += Convert.ToDecimal(lbTenVol.Text) * Constants.TDailyFactor;
                //                FifteenLK += Convert.ToDecimal(lbFifteenVol.Text) * Constants.TDailyFactor;
                //                TwentyLK += Convert.ToDecimal(lbTwentyVol.Text) * Constants.TDailyFactor;
                //                TwentyFiveLK += Convert.ToDecimal(lbTwentyFiveVol.Text) * Constants.TDailyFactor;
                //                ThirtyLK += Convert.ToDecimal(lbThirtyVol.Text) * Constants.TDailyFactor;
                //                ThirtyFiveLK += Convert.ToDecimal(lbThirtyFiveVol.Text) * Constants.TDailyFactor;
                //                FourtyLK += Convert.ToDecimal(lbFourtyVol.Text) * Constants.TDailyFactor;
                //                FourtyFiveLK += Convert.ToDecimal(lbFourtyFiveVol.Text) * Constants.TDailyFactor;
                //                FiftyLK += Convert.ToDecimal(lbFiftyVol.Text) * Constants.TDailyFactor;
                //                FiftyLK += Convert.ToDecimal(lbFiftyFiveVol.Text) * Constants.TDailyFactor;
                //                SixtyLK += Convert.ToDecimal(lbSixtyVol.Text) * Constants.TDailyFactor;
                //                SixtyFiveLK += Convert.ToDecimal(lbSixtyFiveVol.Text) * Constants.TDailyFactor;
                //                SeventyLK += Convert.ToDecimal(lbSeventyVol.Text) * Constants.TDailyFactor;
                //                SeventyFiveLK += Convert.ToDecimal(lbSeventyFiveVol.Text) * Constants.TDailyFactor;
                //                EightyLK += Convert.ToDecimal(lbEightyVol.Text) * Constants.TDailyFactor;
                //                EightyFiveLK += Convert.ToDecimal(lbEightyFiveVol.Text) * Constants.TDailyFactor;
                //                NinetyLK += Convert.ToDecimal(lbNinetyVol.Text) * Constants.TDailyFactor;
                //                NinetyFiveLK += Convert.ToDecimal(lbNinetyFiveVol.Text) * Constants.TDailyFactor;
                //                HundredLK += Convert.ToDecimal(lbMinimumVol.Text) * Constants.TDailyFactor;
                //            }
                //            else
                //            {
                //                ZeroLK += Convert.ToDecimal(lbZeroVol.Text);
                //                FiveLK += Convert.ToDecimal(lbFiveVol.Text);
                //                TenLK += Convert.ToDecimal(lbTenVol.Text);
                //                FifteenLK += Convert.ToDecimal(lbFifteenVol.Text);
                //                TwentyLK += Convert.ToDecimal(lbTwentyVol.Text);
                //                TwentyFiveLK += Convert.ToDecimal(lbTwentyFiveVol.Text);
                //                ThirtyLK += Convert.ToDecimal(lbThirtyVol.Text);
                //                ThirtyFiveLK += Convert.ToDecimal(lbThirtyFiveVol.Text);
                //                FourtyLK += Convert.ToDecimal(lbFourtyVol.Text);
                //                FourtyFiveLK += Convert.ToDecimal(lbFourtyFiveVol.Text);
                //                FiftyLK += Convert.ToDecimal(lbFiftyVol.Text);
                //                FiftyLK += Convert.ToDecimal(lbFiftyFiveVol.Text);
                //                SixtyLK += Convert.ToDecimal(lbSixtyVol.Text);
                //                SixtyFiveLK += Convert.ToDecimal(lbSixtyFiveVol.Text);
                //                SeventyLK += Convert.ToDecimal(lbSeventyVol.Text);
                //                SeventyFiveLK += Convert.ToDecimal(lbSeventyFiveVol.Text);
                //                EightyLK += Convert.ToDecimal(lbEightyVol.Text);
                //                EightyFiveLK += Convert.ToDecimal(lbEightyFiveVol.Text);
                //                NinetyLK += Convert.ToDecimal(lbNinetyVol.Text);
                //                NinetyFiveLK += Convert.ToDecimal(lbNinetyFiveVol.Text);
                //                HundredLK += Convert.ToDecimal(lbMinimumVol.Text);
                //            }
                //        }
                //    }
                //    else   // Rabi Case
                //    {
                //        if (TDailyID >= (int)Constants.SeasonDistribution.RabiStart && TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                //        {
                //            if (TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily || TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                //                || TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily || TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                //            {
                //                Zero += Convert.ToDecimal(lbZeroVol.Text) * Constants.TDailyFactor;
                //                Five += Convert.ToDecimal(lbFiveVol.Text) * Constants.TDailyFactor;
                //                Ten += Convert.ToDecimal(lbTenVol.Text) * Constants.TDailyFactor;
                //                Fifteen += Convert.ToDecimal(lbFifteenVol.Text) * Constants.TDailyFactor;
                //                Twenty += Convert.ToDecimal(lbTwentyVol.Text) * Constants.TDailyFactor;
                //                TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text) * Constants.TDailyFactor;
                //                Thirty += Convert.ToDecimal(lbThirtyVol.Text) * Constants.TDailyFactor;
                //                ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text) * Constants.TDailyFactor;
                //                Fourty += Convert.ToDecimal(lbFourtyVol.Text) * Constants.TDailyFactor;
                //                FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text) * Constants.TDailyFactor;
                //                Fifty += Convert.ToDecimal(lbFiftyVol.Text) * Constants.TDailyFactor;
                //                FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text) * Constants.TDailyFactor;
                //                Sixty += Convert.ToDecimal(lbSixtyVol.Text) * Constants.TDailyFactor;
                //                SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text) * Constants.TDailyFactor;
                //                Seventy += Convert.ToDecimal(lbSeventyVol.Text) * Constants.TDailyFactor;
                //                SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text) * Constants.TDailyFactor;
                //                Eighty += Convert.ToDecimal(lbEightyVol.Text) * Constants.TDailyFactor;
                //                EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text) * Constants.TDailyFactor;
                //                Ninety += Convert.ToDecimal(lbNinetyVol.Text) * Constants.TDailyFactor;
                //                NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text) * Constants.TDailyFactor;
                //                Hundred += Convert.ToDecimal(lbMinimumVol.Text) * Constants.TDailyFactor;
                //            }
                //            else if (TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                //            {
                //                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                //                {

                //                    Zero += Convert.ToDecimal(lbZeroVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Five += Convert.ToDecimal(lbFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Ten += Convert.ToDecimal(lbTenVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Fifteen += Convert.ToDecimal(lbFifteenVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Twenty += Convert.ToDecimal(lbTwentyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Thirty += Convert.ToDecimal(lbThirtyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Fourty += Convert.ToDecimal(lbFourtyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Fifty += Convert.ToDecimal(lbFiftyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Sixty += Convert.ToDecimal(lbSixtyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Seventy += Convert.ToDecimal(lbSeventyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Eighty += Convert.ToDecimal(lbEightyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Ninety += Convert.ToDecimal(lbNinetyVol.Text) * Constants.TDailyLeapYearTrue;
                //                    NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text) * Constants.TDailyLeapYearTrue;
                //                    Hundred += Convert.ToDecimal(lbMinimumVol.Text) * Constants.TDailyLeapYearTrue;
                //                }
                //                else
                //                {
                //                    Zero += Convert.ToDecimal(lbZeroVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Five += Convert.ToDecimal(lbFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Ten += Convert.ToDecimal(lbTenVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Fifteen += Convert.ToDecimal(lbFifteenVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Twenty += Convert.ToDecimal(lbTwentyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Thirty += Convert.ToDecimal(lbThirtyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Fourty += Convert.ToDecimal(lbFourtyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Fifty += Convert.ToDecimal(lbFiftyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Sixty += Convert.ToDecimal(lbSixtyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Seventy += Convert.ToDecimal(lbSeventyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Eighty += Convert.ToDecimal(lbEightyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Ninety += Convert.ToDecimal(lbNinetyVol.Text) * Constants.TDailyLeapYearFalse;
                //                    NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text) * Constants.TDailyLeapYearFalse;
                //                    Hundred += Convert.ToDecimal(lbMinimumVol.Text) * Constants.TDailyLeapYearFalse;
                //                }
                //            }
                //            else
                //            {
                //                Zero += Convert.ToDecimal(lbZeroVol.Text);
                //                Five += Convert.ToDecimal(lbFiveVol.Text);
                //                Ten += Convert.ToDecimal(lbTenVol.Text);
                //                Fifteen += Convert.ToDecimal(lbFifteenVol.Text);
                //                Twenty += Convert.ToDecimal(lbTwentyVol.Text);
                //                TwentyFive += Convert.ToDecimal(lbTwentyFiveVol.Text);
                //                Thirty += Convert.ToDecimal(lbThirtyVol.Text);
                //                ThirtyFive += Convert.ToDecimal(lbThirtyFiveVol.Text);
                //                Fourty += Convert.ToDecimal(lbFourtyVol.Text);
                //                FourtyFive += Convert.ToDecimal(lbFourtyFiveVol.Text);
                //                Fifty += Convert.ToDecimal(lbFiftyVol.Text);
                //                FiftyFive += Convert.ToDecimal(lbFiftyFiveVol.Text);
                //                Sixty += Convert.ToDecimal(lbSixtyVol.Text);
                //                SixtyFive += Convert.ToDecimal(lbSixtyFiveVol.Text);
                //                Seventy += Convert.ToDecimal(lbSeventyVol.Text);
                //                SeventyFive += Convert.ToDecimal(lbSeventyFiveVol.Text);
                //                Eighty += Convert.ToDecimal(lbEightyVol.Text);
                //                EightyFive += Convert.ToDecimal(lbEightyFiveVol.Text);
                //                Ninety += Convert.ToDecimal(lbNinetyVol.Text);
                //                NinetyFive += Convert.ToDecimal(lbNinetyFiveVol.Text);
                //                Hundred += Convert.ToDecimal(lbMinimumVol.Text);
                //            }
                //        }
                //    }
                //}
                //else if (e.Row.RowType == DataControlRowType.Footer)
                //{
                //    if (SeasonID == (int)Constants.Seasons.Kharif)
                //    {
                //        Label lblEK0 = (Label)e.Row.FindControl("lblEK0");
                //        if (lblEK0 != null)
                //            lblEK0.Text = Convert.ToString(Math.Round(Zero * Constants.MAFFactor, 3));

                //        Label lblLK0 = (Label)e.Row.FindControl("lblLK0");
                //        if (lblLK0 != null)
                //            lblLK0.Text = Convert.ToString(Math.Round(ZeroLK * Constants.MAFFactor, 3));

                //        Label lblKharif0 = (Label)e.Row.FindControl("lblKharif0");
                //        if (lblKharif0 != null)
                //            lblKharif0.Text = Convert.ToString(Math.Round((Zero * Constants.MAFFactor) + (ZeroLK * Constants.MAFFactor), 3));

                //        Label lblEK5 = (Label)e.Row.FindControl("lblEK5");
                //        if (lblEK5 != null)
                //            lblEK5.Text = Convert.ToString(Math.Round(Five * Constants.MAFFactor, 3));

                //        Label lblLK5 = (Label)e.Row.FindControl("lblLK5");
                //        if (lblLK5 != null)
                //            lblLK5.Text = Convert.ToString(Math.Round(FiveLK * Constants.MAFFactor, 3));

                //        Label lblKharif5 = (Label)e.Row.FindControl("lblKharif5");
                //        if (lblKharif5 != null)
                //            lblKharif5.Text = Convert.ToString(Math.Round((Five * Constants.MAFFactor) + (FiveLK * Constants.MAFFactor), 3));

                //        Label lblEKTen = (Label)e.Row.FindControl("lblEKTen");
                //        if (lblEKTen != null)
                //            lblEKTen.Text = Convert.ToString(Math.Round(Ten * Constants.MAFFactor, 3));

                //        Label lblLKTen = (Label)e.Row.FindControl("lblLKTen");
                //        if (lblLKTen != null)
                //            lblLKTen.Text = Convert.ToString(Math.Round(TenLK * Constants.MAFFactor, 3));

                //        Label lblTotalTen = (Label)e.Row.FindControl("lblTotalTen");
                //        if (lblTotalTen != null)
                //            lblTotalTen.Text = Convert.ToString(Math.Round((Ten * Constants.MAFFactor) + (TenLK * Constants.MAFFactor), 3));

                //        Label lblEKFifteen = (Label)e.Row.FindControl("lblEKFifteen");
                //        if (lblEKFifteen != null)
                //            lblEKFifteen.Text = Convert.ToString(Math.Round(Fifteen * Constants.MAFFactor, 3));

                //        Label lblLKFifteen = (Label)e.Row.FindControl("lblLKFifteen");
                //        if (lblLKFifteen != null)
                //            lblLKFifteen.Text = Convert.ToString(Math.Round(FifteenLK * Constants.MAFFactor, 3));

                //        Label lblTotalFifteen = (Label)e.Row.FindControl("lblTotalFifteen");
                //        if (lblTotalFifteen != null)
                //            lblTotalFifteen.Text = Convert.ToString(Math.Round((Fifteen * Constants.MAFFactor) + (FifteenLK * Constants.MAFFactor), 3));

                //        Label lblEKTwenty = (Label)e.Row.FindControl("lblEKTwenty");
                //        if (lblEKTwenty != null)
                //            lblEKTwenty.Text = Convert.ToString(Math.Round(Twenty * Constants.MAFFactor, 3));

                //        Label lblLKTwenty = (Label)e.Row.FindControl("lblLKTwenty");
                //        if (lblLKTwenty != null)
                //            lblLKTwenty.Text = Convert.ToString(Math.Round(TwentyLK * Constants.MAFFactor, 3));

                //        Label lblTotalTwenty = (Label)e.Row.FindControl("lblTotalTwenty");
                //        if (lblTotalTwenty != null)
                //            lblTotalTwenty.Text = Convert.ToString(Math.Round((Twenty * Constants.MAFFactor) + (TwentyLK * Constants.MAFFactor), 3));

                //        Label lblEKTwentyFive = (Label)e.Row.FindControl("lblEKTwentyFive");
                //        if (lblEKTwentyFive != null)
                //            lblEKTwentyFive.Text = Convert.ToString(Math.Round(TwentyFive * Constants.MAFFactor, 3));

                //        Label lblLKTwentyFive = (Label)e.Row.FindControl("lblLKTwentyFive");
                //        if (lblLKTwentyFive != null)
                //            lblLKTwentyFive.Text = Convert.ToString(Math.Round(TwentyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalTwentyFive = (Label)e.Row.FindControl("lblTotalTwentyFive");
                //        if (lblTotalTwentyFive != null)
                //            lblTotalTwentyFive.Text = Convert.ToString(Math.Round((TwentyFive * Constants.MAFFactor) + (TwentyFiveLK * Constants.MAFFactor), 3));

                //        Label lblEKThirty = (Label)e.Row.FindControl("lblEKThirty");
                //        if (lblEKThirty != null)
                //            lblEKThirty.Text = Convert.ToString(Math.Round(Thirty * Constants.MAFFactor, 3));

                //        Label lblLKThirty = (Label)e.Row.FindControl("lblLKThirty");
                //        if (lblLKThirty != null)
                //            lblLKThirty.Text = Convert.ToString(Math.Round(ThirtyLK * Constants.MAFFactor, 3));

                //        Label lblTotalThirty = (Label)e.Row.FindControl("lblTotalThirty");
                //        if (lblTotalThirty != null)
                //            lblTotalThirty.Text = Convert.ToString(Math.Round((Thirty * Constants.MAFFactor) + (ThirtyLK * Constants.MAFFactor), 3));

                //        Label lblEKThirtyFive = (Label)e.Row.FindControl("lblEKThirtyFive");
                //        if (lblEKThirtyFive != null)
                //            lblEKThirtyFive.Text = Convert.ToString(Math.Round(ThirtyFive * Constants.MAFFactor, 3));

                //        Label lblLKThirtyFive = (Label)e.Row.FindControl("lblLKThirtyFive");
                //        if (lblLKThirtyFive != null)
                //            lblLKThirtyFive.Text = Convert.ToString(Math.Round(ThirtyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalThirtyFive = (Label)e.Row.FindControl("lblTotalThirtyFive");
                //        if (lblTotalThirtyFive != null)
                //            lblTotalThirtyFive.Text = Convert.ToString(Math.Round((ThirtyFive * Constants.MAFFactor) + (ThirtyFiveLK * Constants.MAFFactor), 3));

                //        Label lblEKFourty = (Label)e.Row.FindControl("lblEKFourty");
                //        if (lblEKFourty != null)
                //            lblEKFourty.Text = Convert.ToString(Math.Round(Fourty * Constants.MAFFactor, 3));

                //        Label lblLKFourty = (Label)e.Row.FindControl("lblLKFourty");
                //        if (lblLKFourty != null)
                //            lblLKFourty.Text = Convert.ToString(Math.Round(FourtyLK * Constants.MAFFactor, 3));

                //        Label lblTotalFourty = (Label)e.Row.FindControl("lblTotalFourty");
                //        if (lblTotalFourty != null)
                //            lblTotalFourty.Text = Convert.ToString(Math.Round((Fourty * Constants.MAFFactor) + (FourtyLK * Constants.MAFFactor), 3));

                //        Label lblEKFourtyFive = (Label)e.Row.FindControl("lblEKFourtyFive");
                //        if (lblEKFourtyFive != null)
                //            lblEKFourtyFive.Text = Convert.ToString(Math.Round(FourtyFive * Constants.MAFFactor, 3));

                //        Label lblLKFourtyFive = (Label)e.Row.FindControl("lblLKFourtyFive");
                //        if (lblLKFourtyFive != null)
                //            lblLKFourtyFive.Text = Convert.ToString(Math.Round(FourtyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalFourtyFive = (Label)e.Row.FindControl("lblTotalFourtyFive");
                //        if (lblTotalFourtyFive != null)
                //            lblTotalFourtyFive.Text = Convert.ToString(Math.Round((FourtyFive * Constants.MAFFactor) + (FourtyFiveLK * Constants.MAFFactor), 3));

                //        Label lblEKFifty = (Label)e.Row.FindControl("lblEKFifty");
                //        if (lblEKFifty != null)
                //            lblEKFifty.Text = Convert.ToString(Math.Round(Fifty * Constants.MAFFactor, 3));

                //        Label lblLKFifty = (Label)e.Row.FindControl("lblLKFifty");
                //        if (lblLKFifty != null)
                //            lblLKFifty.Text = Convert.ToString(Math.Round(FiftyLK * Constants.MAFFactor, 3));

                //        Label lblTotalFifty = (Label)e.Row.FindControl("lblTotalFifty");
                //        if (lblTotalFifty != null)
                //            lblTotalFifty.Text = Convert.ToString(Math.Round((Fifty * Constants.MAFFactor) + (FiftyLK * Constants.MAFFactor), 3));

                //        Label lblEKFiftyFive = (Label)e.Row.FindControl("lblEKFiftyFive");
                //        if (lblEKFiftyFive != null)
                //            lblEKFiftyFive.Text = Convert.ToString(Math.Round(FiftyFive * Constants.MAFFactor, 3));

                //        Label lblLKFiftyFive = (Label)e.Row.FindControl("lblLKFiftyFive");
                //        if (lblLKFiftyFive != null)
                //            lblLKFiftyFive.Text = Convert.ToString(Math.Round(FiftyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalFiftyFive = (Label)e.Row.FindControl("lblTotalFiftyFive");
                //        if (lblTotalFiftyFive != null)
                //            lblTotalFiftyFive.Text = Convert.ToString(Math.Round((FiftyFive * Constants.MAFFactor) + (FiftyFiveLK * Constants.MAFFactor), 3));

                //        Label lblEKSixty = (Label)e.Row.FindControl("lblEKSixty");
                //        if (lblEKSixty != null)
                //            lblEKSixty.Text = Convert.ToString(Math.Round(Sixty * Constants.MAFFactor, 3));

                //        Label lblLKSixty = (Label)e.Row.FindControl("lblLKSixty");
                //        if (lblLKSixty != null)
                //            lblLKSixty.Text = Convert.ToString(Math.Round(SixtyLK * Constants.MAFFactor, 3));

                //        Label lbltotalSixty = (Label)e.Row.FindControl("lbltotalSixty");
                //        if (lbltotalSixty != null)
                //            lbltotalSixty.Text = Convert.ToString(Math.Round((Sixty * Constants.MAFFactor) + (SixtyLK * Constants.MAFFactor), 3));


                //        Label lblEKSixtyFive = (Label)e.Row.FindControl("lblEKSixtyFive");
                //        if (lblEKSixtyFive != null)
                //            lblEKSixtyFive.Text = Convert.ToString(Math.Round(SixtyFive * Constants.MAFFactor, 3));

                //        Label lblLKSixtyFive = (Label)e.Row.FindControl("lblLKSixtyFive");
                //        if (lblLKSixtyFive != null)
                //            lblLKSixtyFive.Text = Convert.ToString(Math.Round(SixtyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalSixtyFive = (Label)e.Row.FindControl("lblTotalSixtyFive");
                //        if (lblTotalSixtyFive != null)
                //            lblTotalSixtyFive.Text = Convert.ToString(Math.Round((SixtyFive * Constants.MAFFactor) + (SixtyFiveLK * Constants.MAFFactor), 3));


                //        Label lblEKSeventy = (Label)e.Row.FindControl("lblEKSeventy");
                //        if (lblEKSeventy != null)
                //            lblEKSeventy.Text = Convert.ToString(Math.Round(Seventy * Constants.MAFFactor, 3));

                //        Label lblLKSeventy = (Label)e.Row.FindControl("lblLKSeventy");
                //        if (lblLKSeventy != null)
                //            lblLKSeventy.Text = Convert.ToString(Math.Round(SeventyLK * Constants.MAFFactor, 3));

                //        Label lblTotalSeventy = (Label)e.Row.FindControl("lblTotalSeventy");
                //        if (lblTotalSeventy != null)
                //            lblTotalSeventy.Text = Convert.ToString(Math.Round((Seventy * Constants.MAFFactor) + (SeventyLK * Constants.MAFFactor), 3));


                //        Label lblEKSeventyFive = (Label)e.Row.FindControl("lblEKSeventyFive");
                //        if (lblEKSeventyFive != null)
                //            lblEKSeventyFive.Text = Convert.ToString(Math.Round(SeventyFive * Constants.MAFFactor, 3));

                //        Label lblLKSeventyFive = (Label)e.Row.FindControl("lblLKSeventyFive");
                //        if (lblLKSeventyFive != null)
                //            lblLKSeventyFive.Text = Convert.ToString(Math.Round(SeventyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalSeventyFive = (Label)e.Row.FindControl("lblTotalSeventyFive");
                //        if (lblTotalSeventyFive != null)
                //            lblTotalSeventyFive.Text = Convert.ToString(Math.Round((SeventyFive * Constants.MAFFactor) + (SeventyFiveLK * Constants.MAFFactor), 3));


                //        Label lblEKEighty = (Label)e.Row.FindControl("lblEKEighty");
                //        if (lblEKEighty != null)
                //            lblEKEighty.Text = Convert.ToString(Math.Round(Eighty * Constants.MAFFactor, 3));

                //        Label lblLKEighty = (Label)e.Row.FindControl("lblLKEighty");
                //        if (lblLKEighty != null)
                //            lblLKEighty.Text = Convert.ToString(Math.Round(EightyLK * Constants.MAFFactor, 3));

                //        Label lblTotalEighty = (Label)e.Row.FindControl("lblTotalEighty");
                //        if (lblTotalEighty != null)
                //            lblTotalEighty.Text = Convert.ToString(Math.Round((Eighty * Constants.MAFFactor) + (EightyLK * Constants.MAFFactor), 3));


                //        Label lblEKEightyFive = (Label)e.Row.FindControl("lblEKEightyFive");
                //        if (lblEKEightyFive != null)
                //            lblEKEightyFive.Text = Convert.ToString(Math.Round(EightyFive * Constants.MAFFactor, 3));

                //        Label lblLKEightyFive = (Label)e.Row.FindControl("lblLKEightyFive");
                //        if (lblLKEightyFive != null)
                //            lblLKEightyFive.Text = Convert.ToString(Math.Round(EightyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalEightyFive = (Label)e.Row.FindControl("lblTotalEightyFive");
                //        if (lblTotalEightyFive != null)
                //            lblTotalEightyFive.Text = Convert.ToString(Math.Round((EightyFive * Constants.MAFFactor) + (EightyFiveLK * Constants.MAFFactor), 3));



                //        Label lblEKNinety = (Label)e.Row.FindControl("lblEKNinety");
                //        if (lblEKNinety != null)
                //            lblEKNinety.Text = Convert.ToString(Math.Round(Ninety * Constants.MAFFactor, 3));

                //        Label lblLKNinety = (Label)e.Row.FindControl("lblLKNinety");
                //        if (lblLKNinety != null)
                //            lblLKNinety.Text = Convert.ToString(Math.Round(NinetyLK * Constants.MAFFactor, 3));

                //        Label lblTotalNinety = (Label)e.Row.FindControl("lblTotalNinety");
                //        if (lblTotalNinety != null)
                //            lblTotalNinety.Text = Convert.ToString(Math.Round((Ninety * Constants.MAFFactor) + (NinetyLK * Constants.MAFFactor), 3));


                //        Label lblEKNinetyFive = (Label)e.Row.FindControl("lblEKNinetyFive");
                //        if (lblEKNinetyFive != null)
                //            lblEKNinetyFive.Text = Convert.ToString(Math.Round(NinetyFive * Constants.MAFFactor, 3));

                //        Label lblLKNinetyFive = (Label)e.Row.FindControl("lblLKNinetyFive");
                //        if (lblLKNinetyFive != null)
                //            lblLKNinetyFive.Text = Convert.ToString(Math.Round(NinetyFiveLK * Constants.MAFFactor, 3));

                //        Label lblTotalNinetyFive = (Label)e.Row.FindControl("lblTotalNinetyFive");
                //        if (lblTotalNinetyFive != null)
                //            lblTotalNinetyFive.Text = Convert.ToString(Math.Round((NinetyFive * Constants.MAFFactor) + (NinetyFiveLK * Constants.MAFFactor), 3));


                //        Label lblEKMinimum = (Label)e.Row.FindControl("lblEKMinimum");
                //        if (lblEKMinimum != null)
                //            lblEKMinimum.Text = Convert.ToString(Math.Round(Hundred * Constants.MAFFactor, 3));

                //        Label lblLKMinimum = (Label)e.Row.FindControl("lblLKMinimum");
                //        if (lblLKMinimum != null)
                //            lblLKMinimum.Text = Convert.ToString(Math.Round(HundredLK * Constants.MAFFactor, 3));

                //        Label lblTotalMinimum = (Label)e.Row.FindControl("lblTotalMinimum");
                //        if (lblTotalMinimum != null)
                //            lblTotalMinimum.Text = Convert.ToString(Math.Round((Hundred * Constants.MAFFactor) + (HundredLK * Constants.MAFFactor), 3));
                //    }
                //    else  //Rabi Case
                //    {
                //        Label lbEKPeriod = (Label)e.Row.FindControl("lblPeriodEK");
                //        if (lbEKPeriod != null)
                //            lbEKPeriod.Text = "Rabi(MAF)";

                //        Label lbLKPeriod = (Label)e.Row.FindControl("lblPeriodLK");
                //        if (lbLKPeriod != null)
                //            lbLKPeriod.Visible = false;

                //        Label lbTotalPeriod = (Label)e.Row.FindControl("lblPeriodTotal");
                //        if (lbTotalPeriod != null)
                //            lbTotalPeriod.Visible = false;

                //        Label lblEK0 = (Label)e.Row.FindControl("lblEK0");
                //        if (lblEK0 != null)
                //            lblEK0.Text = Convert.ToString(Math.Round(Zero * Constants.MAFFactor, 3));

                //        Label lblEK5 = (Label)e.Row.FindControl("lblEK5");
                //        if (lblEK5 != null)
                //            lblEK5.Text = Convert.ToString(Math.Round(Five * Constants.MAFFactor, 3));

                //        Label lblEKTen = (Label)e.Row.FindControl("lblEKTen");
                //        if (lblEKTen != null)
                //            lblEKTen.Text = Convert.ToString(Math.Round(Ten * Constants.MAFFactor, 3));

                //        Label lblEKFifteen = (Label)e.Row.FindControl("lblEKFifteen");
                //        if (lblEKFifteen != null)
                //            lblEKFifteen.Text = Convert.ToString(Math.Round(Fifteen * Constants.MAFFactor, 3));

                //        Label lblEKTwenty = (Label)e.Row.FindControl("lblEKTwenty");
                //        if (lblEKTwenty != null)
                //            lblEKTwenty.Text = Convert.ToString(Math.Round(Twenty * Constants.MAFFactor, 3));

                //        Label lblEKTwentyFive = (Label)e.Row.FindControl("lblEKTwentyFive");
                //        if (lblEKTwentyFive != null)
                //            lblEKTwentyFive.Text = Convert.ToString(Math.Round(TwentyFive * Constants.MAFFactor, 3));

                //        Label lblEKThirty = (Label)e.Row.FindControl("lblEKThirty");
                //        if (lblEKThirty != null)
                //            lblEKThirty.Text = Convert.ToString(Math.Round(Thirty * Constants.MAFFactor, 3));

                //        Label lblEKThirtyFive = (Label)e.Row.FindControl("lblEKThirtyFive");
                //        if (lblEKThirtyFive != null)
                //            lblEKThirtyFive.Text = Convert.ToString(Math.Round(ThirtyFive * Constants.MAFFactor, 3));

                //        Label lblEKFourty = (Label)e.Row.FindControl("lblEKFourty");
                //        if (lblEKFourty != null)
                //            lblEKFourty.Text = Convert.ToString(Math.Round(Fourty * Constants.MAFFactor, 3));

                //        Label lblEKFourtyFive = (Label)e.Row.FindControl("lblEKFourtyFive");
                //        if (lblEKFourtyFive != null)
                //            lblEKFourtyFive.Text = Convert.ToString(Math.Round(FourtyFive * Constants.MAFFactor, 3));

                //        Label lblEKFifty = (Label)e.Row.FindControl("lblEKFifty");
                //        if (lblEKFifty != null)
                //            lblEKFifty.Text = Convert.ToString(Math.Round(Fifty * Constants.MAFFactor, 3));

                //        Label lblEKFiftyFive = (Label)e.Row.FindControl("lblEKFiftyFive");
                //        if (lblEKFiftyFive != null)
                //            lblEKFiftyFive.Text = Convert.ToString(Math.Round(FiftyFive * Constants.MAFFactor, 3));

                //        Label lblEKSixty = (Label)e.Row.FindControl("lblEKSixty");
                //        if (lblEKSixty != null)
                //            lblEKSixty.Text = Convert.ToString(Math.Round(Sixty * Constants.MAFFactor, 3));

                //        Label lblEKSixtyFive = (Label)e.Row.FindControl("lblEKSixtyFive");
                //        if (lblEKSixtyFive != null)
                //            lblEKSixtyFive.Text = Convert.ToString(Math.Round(SixtyFive * Constants.MAFFactor, 3));

                //        Label lblEKSeventy = (Label)e.Row.FindControl("lblEKSeventy");
                //        if (lblEKSeventy != null)
                //            lblEKSeventy.Text = Convert.ToString(Math.Round(Seventy * Constants.MAFFactor, 3));

                //        Label lblEKSeventyFive = (Label)e.Row.FindControl("lblEKSeventyFive");
                //        if (lblEKSeventyFive != null)
                //            lblEKSeventyFive.Text = Convert.ToString(Math.Round(SeventyFive * Constants.MAFFactor, 3));

                //        Label lblEKEighty = (Label)e.Row.FindControl("lblEKEighty");
                //        if (lblEKEighty != null)
                //            lblEKEighty.Text = Convert.ToString(Math.Round(Eighty * Constants.MAFFactor, 3));

                //        Label lblEKEightyFive = (Label)e.Row.FindControl("lblEKEightyFive");
                //        if (lblEKEightyFive != null)
                //            lblEKEightyFive.Text = Convert.ToString(Math.Round(EightyFive * Constants.MAFFactor, 3));

                //        Label lblEKNinety = (Label)e.Row.FindControl("lblEKNinety");
                //        if (lblEKNinety != null)
                //            lblEKNinety.Text = Convert.ToString(Math.Round(Ninety * Constants.MAFFactor, 3));

                //        Label lblEKNinetyFive = (Label)e.Row.FindControl("lblEKNinetyFive");
                //        if (lblEKNinetyFive != null)
                //            lblEKNinetyFive.Text = Convert.ToString(Math.Round(NinetyFive * Constants.MAFFactor, 3));

                //        Label lblEKMinimum = (Label)e.Row.FindControl("lblEKMinimum");
                //        if (lblEKMinimum != null)
                //            lblEKMinimum.Text = Convert.ToString(Math.Round(Hundred * Constants.MAFFactor, 3));
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlRimStatn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvProbability.Visible = false;
                ddlYear.ClearSelection();
                ddlYear.Enabled = false;
                if (ddlRimStatn.SelectedItem.Value != "")
                {
                    ddlSeason.Enabled = true;
                    ddlSeason.ClearSelection();
                    //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                    //ddlSeason.DataTextField = "Name";
                    //ddlSeason.DataValueField = "ID";
                    //ddlSeason.DataBind();
                    Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());
                }
                else
                    ddlSeason.Enabled = true;

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
                gvProbability.Visible = false;
                if (ddlSeason.SelectedItem.Value != "")
                {
                    ddlYear.Enabled = true;
                    ddlYear.ClearSelection();
                    ddlYear.DataSource = new SeasonalPlanningBLL().GetProbabilityYears(Convert.ToInt32(ddlSeason.SelectedItem.Value));
                    ddlYear.DataTextField = "Year";
                    ddlYear.DataValueField = "Year";
                    ddlYear.DataBind();
                    ddlYear.Items.Insert(0, new ListItem("Select", ""));
                }
                else
                {
                    ddlYear.ClearSelection();
                    ddlYear.Enabled = false;

                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlYear.SelectedItem.Value != "")
                    BindGrid();
                else
                    gvProbability.Visible = false;
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
                gvProbability.Visible = true;
                gvProbability.DataSource = new SeasonalPlanningBLL().ViewProbability(Convert.ToInt32(ddlRimStatn.SelectedItem.Value), Convert.ToInt32(ddlSeason.SelectedItem.Value), ddlYear.SelectedItem.Value.ToString());
                gvProbability.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}