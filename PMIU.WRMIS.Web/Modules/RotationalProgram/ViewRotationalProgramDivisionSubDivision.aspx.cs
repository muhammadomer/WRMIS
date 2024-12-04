using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.RotaionalProgram;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class ViewRotationalProgramDivisionSubDivision : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtComments.Attributes.Add("maxlength", "2000");
                    SetTitle();
                    // hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
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


                        if (!string.IsNullOrEmpty(Request.QueryString["FromIrrigator"]))
                        {
                            hlAttachment.Visible = false;
                            btnPrint.Visible = false;
                            hlBack.Visible = false;
                            lbtnHistory.Visible = false;
                        }

                    }
                }

                if (!string.IsNullOrEmpty(Request.QueryString["RPID"]))
                {
                    long RPID = Convert.ToInt64(Convert.ToString(Request.QueryString["RPID"]));
                    dynamic ProgramNameDate = new RotationalProgramBLL().GetProgramNameAndDate(RPID, "D");
                    hdnGroupsQuantity.Value = Utility.GetDynamicPropertyValue(ProgramNameDate, "GroupQty");
                    RPTitle.InnerText = Utility.GetDynamicPropertyValue(ProgramNameDate, "ProgramName");
                    string StartDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(ProgramNameDate, "StartDate")));
                    string EndDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(ProgramNameDate, "EndDate")));
                    RPDates.InnerText = "W.E.F " + StartDate + " to " + EndDate;
                    BindChannelsGrid(RPID);
                    BindPreferencesGrid(RPID);
                    ApprovalButtonsShowHide(RPID);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferences_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    int GroupCount = 1;

                    int TotalCount = 0;
                    if (true == Convert.ToBoolean(hdnHidePriority.Value))
                    {
                        TotalCount = e.Row.Cells.Count;
                    }
                    else
                    {
                        TotalCount = e.Row.Cells.Count - 1;
                    }
                    for (int i = 2; i < TotalCount; i++)
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
                    //int index = e.Row.RowIndex;
                    //if (index == 0)
                    //{
                    //    if (e.Row.Cells[e.Row.Cells.Count-1].Text == "0")
                    //    {
                    //        hdnHidePriority.Value = Convert.ToString(true);
                    //    } 
                    //}
                    string FromDate = e.Row.Cells[0].Text;

                    e.Row.Cells[0].Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(FromDate)));
                    string ToDate = e.Row.Cells[1].Text;
                    e.Row.Cells[1].Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(ToDate)));

                    string CellText = e.Row.Cells[2].Text;
                    if (CellText.Replace(" ", "").ToUpper().Trim() == "CLOSUREPERIOD")
                    {
                        if (true == Convert.ToBoolean(hdnHidePriority.Value))
                        {
                            e.Row.Cells[2].ColumnSpan = e.Row.Cells.Count - 2;
                        }
                        else
                        {
                            e.Row.Cells[2].ColumnSpan = e.Row.Cells.Count - 3;
                        }

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

        protected void gvPreferences_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {


                GridViewRow HeaderRow1 = new GridViewRow(2, 0, DataControlRowType.Header, DataControlRowState.Insert);

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
                if (true != Convert.ToBoolean(hdnHidePriority.Value))
                {
                    HeaderRow1.Cells[e.Row.Cells.Count - 1].RowSpan = 2;
                    HeaderRow1.Cells[e.Row.Cells.Count - 1].Text = "Priority";
                }

                gvPreferences.Controls[0].Controls.AddAt(0, HeaderRow1);

                if (true == Convert.ToBoolean(hdnHidePriority.Value))
                {
                    HeaderRow1.Cells[2].ColumnSpan = e.Row.Cells.Count - 2;
                }
                else
                {
                    HeaderRow1.Cells[2].ColumnSpan = e.Row.Cells.Count - 3;
                }


                HeaderRow1.Cells[2].ColumnSpan = Convert.ToInt32(hdnGroupsQuantity.Value);
                HeaderRow1.Cells[2].Text = "Groups";
                for (int i = 3; i < Convert.ToInt32(hdnGroupsQuantity.Value) + 2; i++)
                {
                    HeaderRow1.Cells[i].Visible = false;
                }
                int SGroupCellNum = Convert.ToInt32(hdnGroupsQuantity.Value) + 2;

                //SubGroup Logic
                if (e.Row.Cells.Count == SGroupCellNum)//No Subgroups and Priority
                {
                    //No Logic
                }
                else if (e.Row.Cells.Count == SGroupCellNum + 1)//No SubGroups but With Priority
                {
                    HeaderRow1.Cells[SGroupCellNum].Text = "Priority";
                }
                else
                {
                    HeaderRow1.Cells[SGroupCellNum].Text = "Sub Groups";
                    int c2 = 0;
                    if (true == Convert.ToBoolean(hdnHidePriority.Value))
                    {
                        HeaderRow1.Cells[SGroupCellNum].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 2;
                        c2 = e.Row.Cells.Count;
                    }
                    else
                    {
                        HeaderRow1.Cells[SGroupCellNum].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 3;
                        c2 = e.Row.Cells.Count - 1;
                    }

                    for (int i = SGroupCellNum + 1; i < c2; i++)
                    {
                        HeaderRow1.Cells[i].Visible = false;
                    }
                }


                //HeaderRow1.Cells[2].Text = "Preferences";
                ////HeaderRow1.Cells[2].Visible = false;
                //int C1 = 0;
                //if (true == Convert.ToBoolean(hdnHidePriority.Value))
                //{
                //    C1 = e.Row.Cells.Count;
                //}
                //else
                //{
                //    C1 = e.Row.Cells.Count - 1;
                //}
                //for (int i = 3; i < C1; i++)
                //{
                //    HeaderRow1.Cells[i].Visible = false;
                //}






                //GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //for (int i = 0; i < e.Row.Cells.Count; i++)
                //{
                //    HeaderRow.CssClass = "table header";


                //    TableCell HeaderCell2 = new TableCell();
                //    HeaderCell2.Text = "";
                //    HeaderCell2.CssClass = "header";
                //    HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                //    HeaderCell2.BorderColor = System.Drawing.Color.DarkGray;
                //    HeaderCell2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
                //    HeaderCell2.BorderWidth = 1;
                //    HeaderCell2.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell2);


                //}

                //gvPreferences.Controls[0].Controls.AddAt(1, HeaderRow);

                //HeaderRow.Cells[0].Visible = false;
                //////HeaderRow.Cells[0].Text = "";
                //HeaderRow.Cells[1].Visible = false;
                //////HeaderRow.Cells[1].Text = "";
                //if (true != Convert.ToBoolean(hdnHidePriority.Value))
                //{
                //    HeaderRow.Cells[e.Row.Cells.Count - 1].Visible = false;
                //}

                ////HeaderRow.Cells[e.Row.Cells.Count - 1].Text = "";
                //HeaderRow.Cells[2].ColumnSpan = Convert.ToInt32(hdnGroupsQuantity.Value);
                //HeaderRow.Cells[2].Text = "Groups";
                //for (int i = 3; i < Convert.ToInt32(hdnGroupsQuantity.Value) + 2; i++)
                //{
                //    HeaderRow.Cells[i].Visible = false;
                //}
                //int SGroupCellNum = Convert.ToInt32(hdnGroupsQuantity.Value) + 2;

                ////SubGroup Logic
                //if (e.Row.Cells.Count == SGroupCellNum)//No Subgroups and Priority
                //{
                //    //No Logic
                //}
                //else if (e.Row.Cells.Count == SGroupCellNum + 1)//No SubGroups but With Priority
                //{
                //    HeaderRow.Cells[SGroupCellNum].Text = "Priority";
                //}
                //else
                //{
                //    HeaderRow.Cells[SGroupCellNum].Text = "Sub Groups";
                //    int c2 = 0;
                //    if (true == Convert.ToBoolean(hdnHidePriority.Value))
                //    {
                //        HeaderRow.Cells[SGroupCellNum].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 2;
                //        c2 = e.Row.Cells.Count;
                //    }
                //    else
                //    {
                //        HeaderRow.Cells[SGroupCellNum].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 3;
                //        c2 = e.Row.Cells.Count - 1;
                //    }

                //    for (int i = SGroupCellNum + 1; i < c2; i++)
                //    {
                //        HeaderRow.Cells[i].Visible = false;
                //    }
                //}

                GridViewRow HeaderRow3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    HeaderRow3.CssClass = "table header";


                    TableCell HeaderCell2 = new TableCell();
                    HeaderCell2.Text = "";
                    HeaderCell2.CssClass = "header";
                    HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                    HeaderCell2.BorderColor = System.Drawing.Color.DarkGray;
                    HeaderCell2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
                    HeaderCell2.BorderWidth = 1;
                    HeaderCell2.Font.Bold = true;
                    HeaderRow3.Cells.Add(HeaderCell2);


                }

                gvPreferences.Controls[0].Controls.AddAt(1, HeaderRow3);

                HeaderRow3.Cells[0].Visible = false;
                //HeaderRow3.Cells[0].Text = "From Date";
                HeaderRow3.Cells[1].Visible = false;
                //HeaderRow3.Cells[1].Text = "To Date";
                if (true != Convert.ToBoolean(hdnHidePriority.Value))
                {
                    HeaderRow3.Cells[e.Row.Cells.Count - 1].Visible = false;
                    //HeaderRow3.Cells[e.Row.Cells.Count - 1].Text = "Priority";
                }

                int GCount = 1;
                for (int i = 2; i < Convert.ToInt32(hdnGroupsQuantity.Value) + 2; i++)
                {
                    HeaderRow3.Cells[i].Text = Convert.ToString(GCount);
                    GCount++;
                }

                //SubGroups Logic
                int SGroupCellNum3 = Convert.ToInt32(hdnGroupsQuantity.Value) + 2;
                if (e.Row.Cells.Count == SGroupCellNum3)//No SubGroups and Priority
                {
                    //No Logic

                }
                else if (e.Row.Cells.Count == SGroupCellNum3 + 1)
                {
                    HeaderRow3.Cells[SGroupCellNum3].Text = "";
                }
                else
                {
                    HeaderRow3.Cells[SGroupCellNum3].Text = "";
                    int c3 = 0;
                    if (true == Convert.ToBoolean(hdnHidePriority.Value))
                    {
                        HeaderRow3.Cells[SGroupCellNum3].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 2;
                        c3 = e.Row.Cells.Count;
                    }
                    else
                    {
                        HeaderRow3.Cells[SGroupCellNum3].ColumnSpan = e.Row.Cells.Count - Convert.ToInt32(hdnGroupsQuantity.Value) - 3;
                        c3 = e.Row.Cells.Count - 1;
                    }

                    for (int i = SGroupCellNum3 + 1; i < c3; i++)
                    {
                        HeaderRow3.Cells[i].Visible = false;
                    }
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

        protected void gvChannels_OnDataBound(object sender, EventArgs e)
        {
            int RowSpanCount = 1;
            for (int i = 0; i < gvChannels.Rows.Count; i++)
            {

                if (i > 0)
                {
                    GridViewRow row = gvChannels.Rows[i];
                    GridViewRow previousRow = gvChannels.Rows[i - 1];
                    for (int j = 0; j < 1; j++)
                    {


                        Label CurrentGroupName = (Label)gvChannels.Rows[i].Cells[0].FindControl("lblGroupName");
                        Label PreviousGroupName = (Label)gvChannels.Rows[i - 1].Cells[0].FindControl("lblGroupName");


                        if (CurrentGroupName.Text == PreviousGroupName.Text)
                        {
                            RowSpanCount++;
                            if (RowSpanCount > 2)
                            {
                                previousRow = gvChannels.Rows[i - (RowSpanCount - 1)];
                            }
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += RowSpanCount;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                            else
                            {
                                row.Cells[j].Visible = false;
                                previousRow.Cells[j].RowSpan += 1;
                            }
                        }
                        else
                        {
                            RowSpanCount = 1;
                        }

                    }
                }

            }
        }

        protected void gvPreferences_OnDataBound(object sender, EventArgs e)
        {
            int HideColumnCount = 0;
            int gCount = 1;
            //Hide Groups
            for (int i = 2; i < 7; i++)
            {
                Label Group = (Label)gvPreferences.Rows[0].Cells[i].FindControl("lblGroup" + gCount);
                if (Group.Text == "N/A")
                {
                    gvPreferences.Columns[i].Visible = false;
                    HideColumnCount++;
                }
                gCount++;
            }
            //int SGroupstoHide = (5 - HideColumnCount) * 3;
            //int LoopStartPoint = 7 - HideColumnCount;
            //LoopStartPoint = LoopStartPoint + SGroupstoHide;
            //int ToCount = 22 - HideColumnCount;
            ////Hide Sub Groups
            //for (int i = LoopStartPoint; i < ToCount; i++)
            //{
            //    gvPreferences.Columns[i].Visible = false;
            //}

            //int RowCount = gvPreferences.Rows.Count;
            //int StartIndex = 7 - HideColumnCount;
            //int EndIndex = StartIndex + 9;
            //int SGCount = 1;
            //int RowStart = 0;
            //int CellNumber = StartIndex;
            //for (int i = StartIndex; i < EndIndex; i++)
            //{
            //    Label Group = (Label)gvPreferences.Rows[0].Cells[CellNumber].FindControl("lblSGroup" + SGCount);
            //    if (Group.Text != "N/A")
            //    {
            //        SGCount++;
            //        CellNumber++;
            //        continue;

            //    }
            //    else
            //    {
            //        int TotalNaCount = 0;
            //        for (int j = RowStart; j < RowCount; j++)
            //        {
            //            Label SGroup = (Label)gvPreferences.Rows[j].Cells[CellNumber].FindControl("lblSGroup" + SGCount);
            //            if (SGroup.Text != "N/A")
            //            {
            //                continue;

            //            }
            //            else
            //            {
            //                TotalNaCount++;
            //                if (RowCount == TotalNaCount)
            //                {
            //                    gvPreferences.Columns[CellNumber].Visible = false;
            //                    CellNumber =  CellNumber-2;

            //                }

            //            }
            //        }
            //        CellNumber++;
            //    }
            //    SGCount++;
            //}


        }

        private void BindChannelsGrid(long _RPID)
        {
            try
            {
                var data = new RotationalProgramBLL().GetGroupsSubGroupsChannels(_RPID);
                List<Channels> lstChannels = new List<Channels>();
                foreach (var item in data)
                {
                    Channels mdlChannel = new Channels();
                    mdlChannel.GroupName = Utility.GetDynamicPropertyValue(item, "GroupName");
                    mdlChannel.SubGroupName = new RotationalProgramBLL().GetSubGroupNameByID(Convert.ToInt32(Utility.GetDynamicPropertyValue(item, "SubGroupID")));
                    mdlChannel.Total = Convert.ToDouble(Utility.GetDynamicPropertyValue(item, "TotalDischarge"));

                    string TotalChannels = "";
                    PropertyInfo[] propertyInfos = null;
                    propertyInfos = item.GetType().GetProperties();
                    List<dynamic> Channels = propertyInfos[3].GetValue(item);
                    for (int i = 0; i < Channels.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (Channels.Count == 1)
                            {
                                TotalChannels += Utility.GetDynamicPropertyValue(Channels[i], "NAME") + "(" + Utility.GetDynamicPropertyValue(Channels[i], "Discharge") + ")";
                            }
                            else
                            {
                                TotalChannels += Utility.GetDynamicPropertyValue(Channels[i], "NAME") + "(" + Utility.GetDynamicPropertyValue(Channels[i], "Discharge") + ") , ";
                            }

                        }
                        else
                        {
                            if (i == Channels.Count - 1)
                            {
                                TotalChannels += Utility.GetDynamicPropertyValue(Channels[i], "NAME") + "(" + Utility.GetDynamicPropertyValue(Channels[i], "Discharge") + ")";
                            }
                            else
                            {
                                TotalChannels += Utility.GetDynamicPropertyValue(Channels[i], "NAME") + "(" + Utility.GetDynamicPropertyValue(Channels[i], "Discharge") + ") , ";
                            }

                        }

                    }
                    mdlChannel.ChannelName = TotalChannels;
                    lstChannels.Add(mdlChannel);

                }
                gvChannels.DataSource = lstChannels;
                gvChannels.DataBind();
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
                PreferencesData = new RotationalProgramBLL().GetPreferencesDataForView(_RPID);

                List<dynamic> lstPreferences = new List<dynamic>();

                int ColName = 1;


                foreach (var item in PreferencesData)
                {
                    dynamic expando = new ExpandoObject();
                    string FromDate = Utility.GetDynamicPropertyValue(item, "FromDate");
                    AddProperty(expando, "FromDate", FromDate);
                    string ToDate = Utility.GetDynamicPropertyValue(item, "ToDate");
                    AddProperty(expando, "ToDate", ToDate);
                    for (int i = 1; i <= 20; i++)
                    {
                        string Val = Utility.GetDynamicPropertyValue(item, "GP" + i);
                        if (Val != "N/A")
                        {
                            AddProperty(expando, "GP" + ColName, Val);
                            ColName++;
                        }

                    }
                    string Priority = Utility.GetDynamicPropertyValue(item, "Priority");
                    if (Priority != "0")
                    {
                        AddProperty(expando, "Priority", Priority);

                    }
                    else
                    {
                        hdnHidePriority.Value = Convert.ToString(true);
                    }


                    lstPreferences.Add(expando);

                }

                DataTable dt = new RotationalProgramBLL().ToDataTable(lstPreferences);


                gvPreferences.DataSource = dt;
                gvPreferences.DataBind();




            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }


        public class Channels
        {
            public string GroupName { get; set; }
            public string SubGroupName { get; set; }
            public string ChannelName { get; set; }
            public double Total { get; set; }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    RP_Approval objApproval = new RP_Approval();
                    objApproval.RPID = Convert.ToInt64(Convert.ToString(Request.QueryString["RPID"])); ;
                    objApproval.DesignationFromID = Convert.ToInt64(Constants.Designation.SE);
                    objApproval.DesignationToID = Convert.ToInt64(Constants.Designation.SE);
                    objApproval.Status = Constants.RP_Approved;
                    objApproval.Comments = txtComments.Text;
                    objApproval.CreatedDate = DateTime.Now;
                    objApproval.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    objApproval.ModifiedDate = DateTime.Now;
                    objApproval.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    new RotationalProgramBLL().SetApprovelStatus(objApproval, false);
                    transaction.Complete();
                }
                Response.RedirectPermanent((String.Format("SearchRotationalProgram.aspx?MsgApprove={0}", true)), false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            try
            {
                RP_Approval objApproval = new RP_Approval();
                objApproval.RPID = Convert.ToInt64(Convert.ToString(Request.QueryString["RPID"])); ;
                objApproval.DesignationFromID = Convert.ToInt64(Constants.Designation.SE);
                objApproval.Status = Constants.RP_Draft;
                objApproval.Comments = txtComments.Text;
                objApproval.CreatedDate = DateTime.Now;
                objApproval.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                objApproval.ModifiedDate = DateTime.Now;
                objApproval.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                new RotationalProgramBLL().SetApprovelStatus(objApproval, true);
                Response.RedirectPermanent((String.Format("SearchRotationalProgram.aspx?MsgSendBack={0}", true)), false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void ApprovalButtonsShowHide(long _RPID)
        {
            try
            {
                RP_Approval LastStatus = new RotationalProgramBLL().ShowHideApprovalButtons(_RPID);
                if (LastStatus != null)
                {
                    if ((LastStatus.DesignationFromID == (long)Constants.Designation.SDO || LastStatus.DesignationFromID == (long)Constants.Designation.XEN)
                        && LastStatus.Status == Constants.RP_SendToSE
                        && Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID) == (long)Constants.Designation.SE)
                    {
                        txtComments.Visible = true;
                        btnApprove.Visible = true;
                        btnSendBack.Visible = true;
                    }
                    else
                    {
                        txtComments.Visible = false;
                        btnApprove.Visible = false;
                        btnSendBack.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["RPID"]))
                {
                    ReportData mdlReportData = new ReportData();
                    ReportParameter ReportParameter = new ReportParameter("RPID", (Convert.ToString(Request.QueryString["RPID"])));
                    mdlReportData.Parameters.Add(ReportParameter);
                    mdlReportData.Name = Constants.RotationalProgramReport;
                    Session[SessionValues.ReportData] = mdlReportData;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "OpenModal();", true);
                gvComments.DataSource = new RotationalProgramBLL().GetCommentsHistory(Convert.ToInt64(Convert.ToString(Request.QueryString["RPID"])));
                gvComments.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}