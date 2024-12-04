using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.RotaionalProgram;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class ViewRotationalProgramCircle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    //hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                    hlBack.NavigateUrl = "~/Modules/RotationalProgram/SearchRotationalProgram.aspx?LoadHistory=true";
                    if (!string.IsNullOrEmpty(Request.QueryString["RPID"]))
                    {
                        long RPID = Convert.ToInt64(Convert.ToString(Request.QueryString["RPID"]));
                        string FileName = new RotationalProgramBLL().GetAttachmentfiles(RPID);
                        if (!string.IsNullOrEmpty(FileName))
                        {
                            hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.RotationalProgram, FileName);
                            hlAttachment.Text = FileName.Substring(FileName.LastIndexOf('_') + 1);
                            hlAttachment.Attributes["FullName"] = FileName;
                            hlAttachment.Visible = true;
                        }
                        dynamic ProgramNameDate = new RotationalProgramBLL().GetProgramNameAndDate(RPID,"C");
                        hdnGroupsQuantity.Value = Utility.GetDynamicPropertyValue(ProgramNameDate, "GroupQty");
                        RPTitle.InnerText = Utility.GetDynamicPropertyValue(ProgramNameDate, "ProgramName");
                        string StartDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(ProgramNameDate, "StartDate")));
                        string EndDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(ProgramNameDate, "EndDate")));
                        RPDates.InnerText = "W.E.F " + StartDate + " to " + EndDate;
                        BindDivisonsGrid(RPID);
                        BindPreferencesGrid(RPID);
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDivisonsGrid(long _RPID)
        {
            try
            {
                List<dynamic> DivisionData = new RotationalProgramBLL().GetDivisionsByGroups(_RPID);
                List<Divisions> lstDivisions = new List<Divisions>();
                foreach (var item in DivisionData)
                {
                    Divisions mdlDivision = new Divisions();
                    mdlDivision.GroupName = Utility.GetDynamicPropertyValue(item, "GroupName");
                    

                    string TotalDivisions = "";
                    PropertyInfo[] propertyInfos = null;
                    propertyInfos = item.GetType().GetProperties();
                    List<string> Divisions = propertyInfos[1].GetValue(item);
                    for (int i = 0; i < Divisions.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (Divisions.Count == 1)
                            {
                                TotalDivisions += Divisions[i];
                            }
                            else
                            {
                                TotalDivisions += Divisions[i] + " ,";
                            }

                        }
                        else
                        {
                            if (i == Divisions.Count - 1)
                            {
                                TotalDivisions += Divisions[i];
                            }
                            else
                            {
                                TotalDivisions += Divisions[i] + " ,";
                            }

                        }

                    }
                    mdlDivision.DivisionName = TotalDivisions;
                    lstDivisions.Add(mdlDivision);

                }
                gvDivisions.DataSource = lstDivisions;
                gvDivisions.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindPreferencesGrid(long _RPID)
        {
            try
            {
                List<dynamic> PreferencesData = new List<dynamic>();
                PreferencesData = new RotationalProgramBLL().GetPreferencesDataDivisionForView(_RPID);

                List<dynamic> lstPreferences = new List<dynamic>();

                int ColName = 1;


                foreach (var item in PreferencesData)
                {
                    dynamic expando = new ExpandoObject();
                    string FromDate = Utility.GetDynamicPropertyValue(item, "FromDate");
                    AddProperty(expando, "FromDate", FromDate);
                    string ToDate = Utility.GetDynamicPropertyValue(item, "ToDate");
                    AddProperty(expando, "ToDate", ToDate);
                    for (int i = 1; i <= 5; i++)
                    {
                        string Val = Utility.GetDynamicPropertyValue(item, "GP" + i);
                        if (Val != "N/A")
                        {
                            AddProperty(expando, "GP" + ColName, Val);
                            ColName++;
                        }

                    }
                    //string Priority = Utility.GetDynamicPropertyValue(item, "Priority");
                    //AddProperty(expando, "Priority", Priority);

                    lstPreferences.Add(expando);

                }

                DataTable dt = new RotationalProgramBLL().ToDataTable(lstPreferences);


                gvDivisionsPreferences.DataSource = dt;
                gvDivisionsPreferences.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionsPreferences_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    int GroupCount = 1;
                    for (int i = 2; i < e.Row.Cells.Count - 1; i++)
                    {
                        if (i < Convert.ToInt32(hdnGroupsQuantity.Value) + 2)
                        {
                            e.Row.Cells[i].Text = Convert.ToString(GroupCount);
                            GroupCount++;
                        }
                        else
                        {
                            e.Row.Cells[i].Text = "";
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string FromDate = e.Row.Cells[0].Text;

                    e.Row.Cells[0].Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(FromDate)));
                    string ToDate = e.Row.Cells[1].Text;
                    e.Row.Cells[1].Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(ToDate)));



                    string CellText = e.Row.Cells[2].Text;
                    if (CellText == "&nbsp;")
                    {
                        e.Row.Cells[2].ColumnSpan = e.Row.Cells.Count - 2;
                        e.Row.Cells[2].Text = "Closure Period";
                        e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        for (int i = 3; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionsPreferences_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {


                GridViewRow HeaderRow1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    HeaderRow1.CssClass = "table header";


                    TableCell HeaderCell2 = new TableCell();
                    HeaderCell2.Text = "";
                    HeaderCell2.CssClass = "header";
                    HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                    HeaderCell2.BorderColor = System.Drawing.Color.DarkGray;
                    HeaderCell2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
                    HeaderCell2.BorderWidth = 1;
                    HeaderCell2.Font.Bold = true;
                    HeaderRow1.Cells.Add(HeaderCell2);


                }
                HeaderRow1.Cells[0].RowSpan = 2;
                HeaderRow1.Cells[0].Text = "From Date";
                HeaderRow1.Cells[1].RowSpan = 2;
                HeaderRow1.Cells[1].Text = "To Date";
                //HeaderRow1.Cells[e.Row.Cells.Count - 1].RowSpan = 2;
                //HeaderRow1.Cells[e.Row.Cells.Count - 1].Text = "Priority";
                gvDivisionsPreferences.Controls[0].Controls.AddAt(0, HeaderRow1);
                HeaderRow1.Cells[2].ColumnSpan = e.Row.Cells.Count - 2;
                HeaderRow1.Cells[2].Text = "Groups";
                for (int i = 3; i < e.Row.Cells.Count; i++)
                {
                    HeaderRow1.Cells[i].Visible = false;
                }






                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    HeaderRow.CssClass = "table header";


                    TableCell HeaderCell2 = new TableCell();
                    HeaderCell2.Text = "";
                    HeaderCell2.CssClass = "header";
                    HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                    HeaderCell2.BorderColor = System.Drawing.Color.DarkGray;
                    HeaderCell2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
                    HeaderCell2.BorderWidth = 1;
                    HeaderCell2.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell2);


                }

                gvDivisionsPreferences.Controls[0].Controls.AddAt(1, HeaderRow);

                HeaderRow.Cells[0].Visible = false;
                ////HeaderRow.Cells[0].Text = "";
                HeaderRow.Cells[1].Visible = false;
                ////HeaderRow.Cells[1].Text = "";
                //HeaderRow.Cells[e.Row.Cells.Count - 1].Visible = false;
                ////HeaderRow.Cells[e.Row.Cells.Count - 1].Text = "";
                //HeaderRow.Cells[2].ColumnSpan = Convert.ToInt32(hdnGroupsQuantity.Value);
                //HeaderRow.Cells[2].Text = "Groups";
                int ColNum = 1;
                for (int i = 2; i < Convert.ToInt32(hdnGroupsQuantity.Value) + 2; i++)
                {
                    HeaderRow.Cells[i].Text = Convert.ToString(ColNum);
                    ColNum++;
                }
               
            }
        }
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {

            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        } 
        public class Divisions
        {
            public string GroupName { get; set; }
            public string DivisionName { get; set; }
        
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["RPID"]))
                {
                    ReportData mdlReportData = new ReportData();
                    ReportParameter ReportParameter = new ReportParameter("RPID", (Convert.ToString(Request.QueryString["RPID"])));
                    mdlReportData.Parameters.Add(ReportParameter);
                    mdlReportData.Name = Constants.RotationalProgramReportforCircle;
                    Session[SessionValues.ReportData] = mdlReportData;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
       
    }
}