using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.UserAdministration;
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

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class ViewIndents : BasePage
    {
        long IndentID;
        DateTime Date;
        long SubDivID;
        long ChannelID;
        double TotalOfftakeIndent = 0;
        public const string QSIndentID = "IndentID";
        public const string QSDate = "Date";
        public const string QSSubDivID = "SubDivID";
        public const string QSChannelID = "ParentChannelID";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.ViewIndents] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                    else
                    {
                        IndentID = Convert.ToInt64(Request.QueryString[QSIndentID]);
                        Date = Convert.ToDateTime(Request.QueryString[QSDate]);
                        SubDivID = Convert.ToInt64(Request.QueryString[QSSubDivID]);
                        ChannelID = Convert.ToInt64(Request.QueryString[QSChannelID]);

                        BindGrid(IndentID, Date);
                        BindDatesDropdown(SubDivID, ChannelID, Date);
                        ddlDate.ClearSelection();
                        Dropdownlist.SetSelectedText(ddlDate, Utility.GetFormattedDate(Date));

                        List<dynamic> SearchCriteria = new List<dynamic>();

                        SearchCriteria.Add(IndentID);
                        SearchCriteria.Add(Date);
                        SearchCriteria.Add(SubDivID);
                        SearchCriteria.Add(ChannelID);

                        Session[SessionValues.ViewIndents] = SearchCriteria;
                    }
                    hlBack.NavigateUrl = "~/Modules/DailyData/DailyIndents.aspx?ShowHistory=true";
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<dynamic> SearchCriteria = new List<dynamic>();

                IndentID = Convert.ToInt64(ddlDate.SelectedValue);
                Date = Convert.ToDateTime(ddlDate.SelectedItem.Text);
                if (!string.IsNullOrEmpty(Request.QueryString[QSIndentID]) && !string.IsNullOrEmpty(Request.QueryString[QSChannelID]))
                {
                    SubDivID = Convert.ToInt64(Request.QueryString[QSSubDivID]);
                    ChannelID = Convert.ToInt64(Request.QueryString[QSChannelID]);
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                {
                    List<dynamic> SearchCriteria2 = (List<dynamic>)Session[SessionValues.ViewIndents];
                    SubDivID = Convert.ToInt64(SearchCriteria2[2]);
                    ChannelID = Convert.ToInt64(SearchCriteria2[3]);
                }

                BindGrid(IndentID, Date);

                SearchCriteria.Add(IndentID);
                SearchCriteria.Add(Date);
                SearchCriteria.Add(SubDivID);
                SearchCriteria.Add(ChannelID);

                Session[SessionValues.ViewIndents] = SearchCriteria;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid(long _IndentID, DateTime _Date)
        {
            IndentsBLL bllIndents = new IndentsBLL();
            List<CO_ChannelIndentOfftakes> lstChannelIndentOfftakes = bllIndents.GetChannelIndentOfftakes(_IndentID, _Date);

            gvIndents.DataSource = lstChannelIndentOfftakes;
            gvIndents.DataBind();
        }

        protected void gvIndents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblOfftakeIndentDate = (Label)e.Row.FindControl("lblOfftakeIndentDate");
                    //Label lblIndentTime = (Label)e.Row.FindControl("lblIndentTime");
                    Label lblParrentRD = (Label)e.Row.FindControl("lblParrentRD");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");

                    if (lblOfftakeIndentDate.Text != "")
                    {
                        lblOfftakeIndentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblOfftakeIndentDate.Text));
                    }

                    //if (lblIndentTime.Text != "")
                    //{
                    //    lblIndentTime.Text = Utility.GetFormattedTime(Convert.ToDateTime(lblIndentTime.Text));
                    //}

                    if (lblParrentRD.Text != String.Empty)
                    {
                        double GaugeAtRD = Convert.ToDouble(lblParrentRD.Text);
                        lblParrentRD.Text = Calculations.GetRDText(GaugeAtRD);
                    }

                    if (lblIndent.Text != string.Empty)
                    {
                        double IndentVal = Convert.ToDouble(lblIndent.Text);
                        lblIndent.Text = String.Format("{0:0.00}", (IndentVal));
                    }

                    TotalOfftakeIndent = TotalOfftakeIndent + Convert.ToDouble(lblIndent.Text);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblOfftakesTotalIndents = (Label)e.Row.FindControl("lblOfftakesTotalIndents");
                    Label lblDirectOutletsTotalIndent = (Label)e.Row.FindControl("lblDirectOutletsTotalIndent");
                    Label lblIncrementedIndentAt10P = (Label)e.Row.FindControl("lblIncrementedIndentAt10P");
                    Label lblIndentAtSubDivisionalGauge = (Label)e.Row.FindControl("lblIndentAtSubDivisionalGauge");
                    Label lblPercent = (Label)e.Row.FindControl("lblPercent");

                    IndentsBLL bllIndents = new IndentsBLL();

                    if (!IsPostBack)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString[QSIndentID]))
                        {
                            IndentID = Convert.ToInt64(Request.QueryString[QSIndentID]);
                        }
                        else if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                        {
                            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.ViewIndents];

                            if (SearchCriteria != null)
                            {
                                IndentID = Convert.ToInt64(SearchCriteria[0]);
                            }
                        }
                    }
                    else
                    {
                        IndentID = Convert.ToInt64(ddlDate.SelectedValue);
                    }

                    CO_ChannelIndent mdlChannelIndent = bllIndents.GetSubDivisionAndChannelByIndentID(IndentID);
                    double? OutletIndent = bllIndents.GetOutletIndent(Convert.ToInt64(mdlChannelIndent.SubDivID), Convert.ToInt64(mdlChannelIndent.ParentChannelID));
                    double? Sum = 0;
                    double? PercentageIncrementValue = 0;
                    int SystemParametervalue = 0;

                    UA_SystemParameters mdlPercentageValue = new UserBLL().GetSystemParameterValue((short)Constants.SystemParameter.IndentPercentage);

                    SystemParametervalue = Convert.ToInt16(mdlPercentageValue.ParameterValue);


                    Sum = OutletIndent + TotalOfftakeIndent;
                    PercentageIncrementValue = (Sum * SystemParametervalue) / 100;

                    lblOfftakesTotalIndents.Text = String.Format("{0:0.00}", (TotalOfftakeIndent));
                    lblDirectOutletsTotalIndent.Text = String.Format("{0:0.00}", (OutletIndent));
                    lblIncrementedIndentAt10P.Text = String.Format("{0:0.00}", (PercentageIncrementValue));
                    lblIndentAtSubDivisionalGauge.Text = String.Format("{0:0.00}", (TotalOfftakeIndent + OutletIndent + PercentageIncrementValue));
                    lblPercent.Text = "Incremented Indent At " + SystemParametervalue + "%";

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDatesDropdown(long _SubDivID, long _ChannelID, DateTime _Date)
        {
            IndentsBLL bllIndents = new IndentsBLL();


            List<dynamic> lstChannelIndent = bllIndents.GetAllDatesAndIndentID(_SubDivID, _ChannelID, _Date);


            //List<string> lstDates = bllIndents.GetAllDates(_Date);

            ddlDate.DataSource = lstChannelIndent;
            ddlDate.DataTextField = "IndentDate";
            ddlDate.DataValueField = "ID";
            ddlDate.DataBind();
        }

        //protected void ddlDate_DataBound(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DropDownList list = sender as DropDownList;
        //        for (int i = 0; i < list.Items.Count; i++)
        //        {
        //            ddlDate.DataTextField = Utility.GetFormattedDate(Convert.ToDateTime(list.SelectedItem.Text));
        //        }
        //        //list.DataTextField = Utility.GetFormattedDate(Convert.ToDateTime("IndentDate"));
        //        //lstDateChannelIndentOfftakes.Add(Utility.GetFormattedDate(lstChannelIndentOfftakes.ElementAt(i)));
        //        //Utility.GetFormattedDate(Convert.ToDateTime(list.SelectedItem.Text));
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 16/08/2016 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewIndents);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindHistoryData()
        {
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.ViewIndents];

            long IndentID = Convert.ToInt64(SearchCriteria[0]);
            DateTime Date = SearchCriteria[1];
            long SubDivID = Convert.ToInt64(SearchCriteria[2]);
            long ChannelID = Convert.ToInt64(SearchCriteria[3]);

            //Date = DateTime.Parse(SearchCriteria[1]);
            ddlDate.ClearSelection();
            BindDatesDropdown(SubDivID, ChannelID, Date);
            Dropdownlist.SetSelectedText(ddlDate, Utility.GetFormattedDate(Date));
            BindGrid(IndentID, Date);
        }
    }
}