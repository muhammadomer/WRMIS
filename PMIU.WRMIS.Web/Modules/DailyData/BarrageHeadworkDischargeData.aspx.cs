using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class BarrageHeadworkDischargeData : BasePage
    {
        #region View State Keys

        public const string lstIDIndex = "lstIDIndex";
        public const string TotalDischargeDS = "TotalDischargeDS";
        public const string ReasonForChange = "ReasonForChange";
        public const string DataAnalystUserID = "DataAnalystUserID";
        public const string DischargeList = "DischargeList";
        public const string DS = "DS";
        public const string DSGauges = "DSGauges";

        #endregion

        DailyDataBLL bllDailyData = new DailyDataBLL();
        DataTable lstBarrageDischarge;
        List<int> lstDischargeIndexs = new List<int>();
        List<int> lstIDIndexs = new List<int>();
        List<int> lstIDElements = new List<int>();
        List<int> lstGaugesIndex = new List<int>();
        int TotalDischargeUS = 0;
        int TimesColumnIndex = 0;
        int ChangedTimeColumnIndex = -1;

        string DataAnalyst = "DATA ANALYST";
        string XEN = "XEN";
        string SDO = "SDO";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    ViewState["LoggedUser"] = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                    BindBarrageDropdown();
                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                long BarrageID = ddlBarrage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlBarrage.SelectedItem.Value);
                CO_BarrageGaugeReadingFrequency mdlBarrageGaugeReadingFrequency = bllDailyData.GetBarrageGaugeReadingFrequency(BarrageID);


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        if (e.Row.Cells[i].Text.ToUpper().IndexOf("ID") != -1)
                        {
                            //IDColumnIndex = i;
                            e.Row.Cells[i].Attributes.Add("style", "display:none;");
                            lstIDIndexs.Add(i);
                            //ViewState[lstID] = lstIDIndexs;
                            if (e.Row.Cells[i].Text.ToUpper() != "DOWNSTREAM ID" && e.Row.Cells[i].Text.ToUpper() != "USERID")
                            {
                                lstIDElements.Add(i - 1);
                                ViewState[lstIDIndex] = lstIDElements;
                            }

                        }
                        else if (e.Row.Cells[i].Text.ToUpper().IndexOf("GAUGE") != -1)
                        {
                            lstGaugesIndex.Add(i);
                            //ViewState["GaugesList"] = lstGaugesIndex;
                        }
                        else if (e.Row.Cells[i].Text.ToUpper().IndexOf("DISCHARGE") != -1 && e.Row.Cells[i].Text.ToUpper() != "DOWNSTREAM DISCHARGE IN CUSECS" && e.Row.Cells[i].Text.ToUpper() != "UPSTREAM DISCHARGE IN CUSECS")
                        {
                            lstDischargeIndexs.Add(i);
                        }
                        else if (e.Row.Cells[i].Text.ToUpper() == "TIMESTAMP")
                        {
                            TimesColumnIndex = i;
                        }
                        else if (e.Row.Cells[i].Text.ToUpper() == "DOWNSTREAM DISCHARGE IN CUSECS")
                        {
                            ViewState[TotalDischargeDS] = i;
                        }
                        else if (e.Row.Cells[i].Text.ToUpper() == "REASON FOR CHANGE")
                        {
                            ViewState[ReasonForChange] = i;
                        }
                        if (e.Row.Cells[i].Text.ToUpper() == "UPSTREAM DISCHARGE IN CUSECS")
                        {
                            TotalDischargeUS = i;
                            ViewState["TotalDischargeUS"] = i;
                        }
                        if (e.Row.Cells[i].Text.ToUpper() == "DOWNSTREAM GAUGE IN FT.")
                        {
                            ViewState[DSGauges] = i;
                        }
                        if (e.Row.Cells[i].Text.ToUpper() == "USERID")
                        {
                            ViewState[DataAnalystUserID] = i;
                        }



                    }
                }

                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Cells[IDColumnIndex].Visible = false;
                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        if (lstIDIndexs.Any(y => y == i))
                        {
                            e.Row.Cells[i].Attributes.Add("style", "display:none;");
                            //((TextBox)gvBarrage.Rows[e.NewEditIndex].Cells[i - 1].Controls[0]).Enabled = false;
                        }
                    }

                    DateTime date = Utility.GetParsedDate(txtDate.Text);
                    int ReasonsForChange = Convert.ToInt32(ViewState[ReasonForChange]);

                    if (gvBarrage.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlReasonForChange = new DropDownList();

                        if (mdlBarrageGaugeReadingFrequency.CO_ReadingFrequency.ID == 4)
                        {
                            if (((TextBox)e.Row.Cells[1].Controls[0]).Text == "AM")
                            {
                                ((TextBox)e.Row.Cells[1].Controls[0]).Text = "AM";
                            }
                            else if (((TextBox)e.Row.Cells[1].Controls[0]).Text == "PM")
                            {
                                ((TextBox)e.Row.Cells[1].Controls[0]).Text = "PM";
                            }
                        }
                        bool flag = false;
                        for (int i = 1; i < e.Row.Cells.Count; i++)
                        {
                            if (i == ReasonsForChange)
                            {
                                TextBox txtReasonForChange = (TextBox)e.Row.Cells[i].Controls[0];
                                txtReasonForChange.Visible = false;

                                //DropDownList ddlReasonForChange = new DropDownList();
                                ddlReasonForChange.ID = "ddlReasonForChange";
                                ddlReasonForChange.ClientIDMode = ClientIDMode.Static;
                                ddlReasonForChange.Attributes.Add("runat", "server");
                                ddlReasonForChange.Attributes.Add("required", "required");
                                Dropdownlist.DDLReasonsForChange(ddlReasonForChange);
                                ddlReasonForChange.CssClass = "required form-control";
                                ddlReasonForChange.Attributes.Add("onChange", "javascript:GetReasonForChangeValue('" + ddlReasonForChange.ID + "','" + txtReasonForChangeValue.ID + "');");
                                e.Row.Cells[i].Controls.Add(ddlReasonForChange);

                                string ResonForChange = txtReasonForChange.Text;
                                if (ResonForChange != "")
                                {
                                    Dropdownlist.SetSelectedText(ddlReasonForChange, ResonForChange);
                                    txtReasonForChangeValue.Value = txtReasonForChange.Text;
                                }

                            }
                            else if (i != ReasonsForChange)
                            {
                                ((TextBox)e.Row.Cells[i].Controls[0]).CssClass = "form-control";
                            }
                            if (i != TimesColumnIndex && i != ReasonsForChange)
                            {
                                ((TextBox)e.Row.Cells[i].Controls[0]).CssClass = "form-control decimalInput";
                            }

                            //((TextBox)e.Row.Cells[i].Controls[0]).CssClass = "form-control";

                            //if (i != TimesColumnIndex && i != ReasonForChange)
                            //{
                            //    ((TextBox)e.Row.Cells[i].Controls[0]).CssClass = "form-control decimalInput";
                            //}



                            List<string> lstClientID = lstDischargeIndexs.Select(n => ((TextBox)e.Row.Cells[n].Controls[0]).ClientID).ToList();


                            if (lstDischargeIndexs.Any(x => x == i))
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                string jsArray = serializer.Serialize(lstClientID);
                                ((TextBox)e.Row.Cells[i - 1].Controls[0]).Attributes.Add("onblur", "javascript:CalculateDischarge('" + ((TextBox)e.Row.Cells[i - 2].Controls[0]).ClientID + "','" + ((TextBox)e.Row.Cells[i - 1].Controls[0]).ClientID + "','" + ((TextBox)e.Row.Cells[i].Controls[0]).ClientID + "','" + ((TextBox)e.Row.Cells[TotalDischargeUS].Controls[0]).ClientID + "','" + ((TextBox)e.Row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).ClientID + "','" + jsArray + "');");
                            }

                            if (i == Convert.ToInt32(ViewState[TotalDischargeDS]))
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                string jsArray = serializer.Serialize(lstClientID);
                                ((TextBox)e.Row.Cells[i].Controls[0]).Attributes.Add("onblur", "javascript:CalculateTotalDischargeUS('" + ((TextBox)e.Row.Cells[TotalDischargeUS].Controls[0]).ClientID + "','" + ((TextBox)e.Row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).ClientID + "','" + jsArray + "');");
                            }


                            if (lstIDIndexs.Any(y => y != i))
                            {
                                if (string.IsNullOrEmpty(((TextBox)e.Row.Cells[i].Controls[0]).Text) && i != ReasonsForChange)
                                {
                                    flag = true;
                                }
                            }
                        }

                        if (flag == true)
                        {
                            ddlReasonForChange.Enabled = false;
                            ddlReasonForChange.Attributes.Remove("required");
                            ddlReasonForChange.CssClass = "form-control";
                        }

                        ViewState[DischargeList] = lstDischargeIndexs;
                    }
                    else
                    {
                        Button btnEdit = (Button)e.Row.Cells[0].FindControl("btnEdit");

                        bool IsEdit = false;

                        for (int i = 1; i < e.Row.Cells.Count; i++)
                        {
                            if (!lstIDIndexs.Any(y => y == i) && i != TimesColumnIndex && i != ReasonsForChange)
                            {
                                if (!string.IsNullOrEmpty((e.Row.Cells[i]).Text) && e.Row.Cells[i].Text != "&nbsp;")
                                {
                                    IsEdit = true;
                                    break;
                                }
                            }
                        }

                        if (IsEdit)
                        {
                            btnEdit.CssClass = "btn btn-primary btn_32 edit";
                            btnEdit.ToolTip = "Edit";
                        }

                        UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                        string EditableRoles = Utility.ReadConfiguration("AllowEditRoles");
                        List<String> lstEditableUsers = EditableRoles.Split(',').ToList();

                        if (date < DateTime.Today && !(lstEditableUsers.Contains(mdlUsers.UA_Roles.Name) && (base.CanEdit || base.CanAdd)))
                        {
                            btnEdit.Enabled = false;

                            if (IsEdit == true)
                            {
                                btnEdit.Visible = base.CanEdit;
                            }
                            else
                            {
                                btnEdit.Visible = base.CanAdd;
                            }
                        }
                        else
                        {
                            if (IsEdit == true)
                            {
                                if (e.Row.Cells[Convert.ToInt32(ViewState[DataAnalystUserID])].Text != "&nbsp;" && e.Row.Cells[Convert.ToInt32(ViewState[DataAnalystUserID])].Text != string.Empty)
                                {
                                    long DataAnalystID = Convert.ToInt64(e.Row.Cells[Convert.ToInt32(ViewState[DataAnalystUserID])].Text);

                                    UA_Users mdlUser = new UserBLL().GetUserByID(DataAnalystID);

                                    if (mdlUser != null)
                                    {
                                        if (mdlUser.UA_Roles.Name.ToUpper() == DataAnalyst && (mdlUsers.UA_Roles.Name.ToUpper() == SDO || mdlUsers.UA_Roles.Name.ToUpper() == XEN))
                                        {
                                            btnEdit.Visible = base.CanEdit;
                                            btnEdit.Enabled = false;
                                        }
                                        else
                                        {
                                            btnEdit.Visible = base.CanEdit;
                                        }
                                    }
                                    else
                                    {
                                        btnEdit.Visible = base.CanEdit;
                                    }

                                }
                            }
                            else
                            {
                                btnEdit.Visible = base.CanAdd;
                            }
                        }
                    }
                    //else
                    //{
                    //    for (int j = 0; j < e.Row.Cells.Count; j++)
                    //    {
                    //        if (lstGaugesIndex.Any(x => x == j))
                    //        {

                    //            if (((Label)e.Row.Cells[j].Controls[0]).Text != "")
                    //            {
                    //                IsEdit = true;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //    ViewState["IsEdit"] = IsEdit;
                    //}

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrage_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                int RowIndex = e.RowIndex;
                GridViewRow row = gvBarrage.Rows[RowIndex];
                long BarrageID = ddlBarrage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlBarrage.SelectedItem.Value);

                string ReasonForChange = String.Empty; //((TextBox)row.Cells[Convert.ToInt32(ViewState[ReasonForChange])].Controls[0]).Text;
                //string ReasonForChange = ((DropDownList)row.Cells[Convert.ToInt32(ViewState[ReasonForChange])].Controls[1]).SelectedItem.Text;
                if (txtReasonForChangeValue.Value.ToUpper() != "SELECT" && txtReasonForChangeValue.Value.ToUpper() != "")
                {
                    ReasonForChange = txtReasonForChangeValue.Value;
                }

                txtReasonForChangeValue.Value = string.Empty;
                long UserID = (long)ViewState["LoggedUser"];
                DateTime Date = Convert.ToDateTime(txtDate.Text);
                string Time = "";

                long CanalID;
                bool IsExcessiveDischarge = false;
                lstIDElements = (List<int>)ViewState[lstIDIndex];
                DataTable dt = ViewState[DS] as DataTable;


                if (((TextBox)row.Cells[1].Controls[0]).Text == "AM")
                {
                    Time = "00:00";
                }
                else if (((TextBox)row.Cells[1].Controls[0]).Text == "PM")
                {
                    Time = "12:00";
                }
                else
                {
                    Time = ((TextBox)row.Cells[1].Controls[0]).Text;
                }
                DateTime TimeFormat = Convert.ToDateTime(Time);
                TimeFormat.ToString("HH:mm");
                //DateTime DateFormat = Date;
                //Date.ToString("dd-MM-yyyy");

                DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);
                string FormattedDateTime = ReadingDateTime.ToString("yyyy-MM-dd" + " " + "HH:mm");
                string FormattedTime = ReadingDateTime.ToString("HH:mm");
                lstDischargeIndexs = (List<int>)ViewState[DischargeList];

                StringBuilder builder = new StringBuilder();
                for (int j = 2; j < row.Cells.Count - 2; j++)
                {
                    builder.Append(((TextBox)row.Cells[j].Controls[0]).Text);
                    if (lstDischargeIndexs.Any(x => x == j) || (int)ViewState[TotalDischargeDS] == j || (int)ViewState["TotalDischargeUS"] == j)
                    {
                        builder.Append(" ");
                    }
                    else
                    {
                        builder.Append(",");
                    }
                }

                string Parameter = builder.ToString();

                //List<dynamic> lst = new List<dynamic>();
                //lst.Add(RowIndex);
                //for (int i = 1; i < row.Cells.Count; i++)
                //{
                //    lst.Add(((TextBox)row.Cells[i].Controls[0]).Text);
                //}
                if (((TextBox)row.Cells[Convert.ToInt32(ViewState[DSGauges])].Controls[0]).Text != "")
                {
                    if (Convert.ToDouble((((TextBox)row.Cells[Convert.ToInt32(ViewState[DSGauges])].Controls[0]).Text)) < 0)
                    {
                        Master.ShowMessage(Message.NegativeGaugeValue.Description, SiteMaster.MessageType.Error);
                        gvBarrage.EditIndex = -1;
                        BindGrid();
                        return;

                    }
                }

                CO_ChannelGauge mdlChannelGauge = null;
                for (int k = 0; k < lstIDElements.Count(); k++)
                {
                    CanalID = Convert.ToInt64(dt.Rows[RowIndex][lstIDElements.ElementAt(k)].ToString());
                    mdlChannelGauge = bllDailyData.GetDesignDischargeByChannelID(CanalID);
                    if (mdlChannelGauge != null)
                    {
                        double DesignDischargePercentage = Convert.ToDouble((mdlChannelGauge.DesignDischarge * 115) / 100);
                        if (((TextBox)row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).Text != "")
                        {
                            if (Convert.ToDouble(((TextBox)row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).Text) > DesignDischargePercentage)
                            {
                                IsExcessiveDischarge = true;
                                break;
                            }
                            else
                            {
                                IsExcessiveDischarge = false;
                            }
                        }
                    }
                }



                //CO_ChannelGauge mdlChannelGauge = bllDailyData.GetDesignDischargeByChannelID(1264);//channel id procedure se aaye gi
                //if (mdlChannelGauge != null)
                //{
                //    double DesignDischargePercentage = Convert.ToDouble((mdlChannelGauge.DesignDischarge * 115) / 100);
                //    if (((TextBox)row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).Text != "")
                //    {
                //        if (Convert.ToDouble(((TextBox)row.Cells[Convert.ToInt32(ViewState[TotalDischargeDS])].Controls[0]).Text) > DesignDischargePercentage)
                //        {
                //            Master.ShowMessage(Message.ExcessiveDischarge.Description, SiteMaster.MessageType.Warning);
                //        }
                //        else
                //        {
                //            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                //        }
                //    }

                //}

                //lstGaugesIndex = (List<int>)ViewState["GaugesList"];
                //for (int j = 0; j < row.Cells.Count; j++)
                //{
                //    if (lstGaugesIndex.Any(x => x == j))
                //    {
                //        if (lstBarrageDischarge.Rows[RowIndex][j].ToString() != "")
                //        {
                //            IsEdit = true;
                //            break;
                //        }
                //    }
                //}
                //if (Convert.ToBoolean(ViewState["IsEdit"]))
                //{

                //}
                //else
                //{
                //    bllDailyData.AddBarrageDischargeData(BarrageID, FormatedDateTime, Parameter, UserID);
                //}



                //DataTable dt = ViewState[DS] as DataTable;

                int rowindex = e.RowIndex;
                List<int> lstDischarge = new List<int>();

                bool allnull = true;

                int columns = dt.Columns.Count;

                for (int jj = 0; jj < columns; ++jj)
                {
                    if (!dt.Columns[jj].ColumnName.ToUpper().Contains("GAUGE"))
                        continue;

                    if (dt.Rows[rowindex][jj] != System.DBNull.Value)
                    {
                        allnull = false;
                        break;
                    }
                }

                for (int i = 0; i < columns; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Contains("DISCHARGE"))
                    {
                        lstDischarge.Add(i + 1);
                    }
                }

                for (int i = 0; i < lstDischarge.Count; i++)
                {
                    if (((TextBox)row.Cells[lstDischarge.ElementAt(i)].Controls[0]).Text == "")
                    {
                        Master.ShowMessage(Message.DTParametersNotAvailable.Description, SiteMaster.MessageType.Error);
                        gvBarrage.EditIndex = -1;
                        BindGrid();
                        return;
                    }
                }


                if (allnull)
                {
                    bllDailyData.AddBarrageDischargeData(BarrageID, ReadingDateTime.Date, FormattedTime, Parameter, UserID, true, "W");
                    //NotifyEvent _event = new NotifyEvent();
                    //_event.Parameters.Add("GaugeID", mdlChannelGauge.ID);
                    //_event.AddNotifyEvent((long)NotificationEventConstants.DailyData.AddDailyHourlyDataForBarrage, SessionManagerFacade.UserInformation.ID);
                }
                else
                {
                    bllDailyData.UpdateBarrageDischargeData(BarrageID, FormattedDateTime, Parameter, UserID, ReasonForChange, "W");
                }

                if (IsExcessiveDischarge)
                {
                    Master.ShowMessage(Message.ExcessiveDischarge.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }





            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            finally
            {
                gvBarrage.EditIndex = -1;
                BindGrid();
            }
        }

        protected void gvBarrage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvBarrage.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBarrage_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvBarrage.EditIndex = e.NewEditIndex;



                BindGrid();

                lstDischargeIndexs = (List<int>)ViewState[DischargeList];


                for (int i = 1; i < gvBarrage.Rows[e.NewEditIndex].Cells.Count; i++)
                {

                    //if (i % 2 == 1)
                    //    ((TextBox)gvBarrage.Rows[e.NewEditIndex].Cells[i].Controls[0]).Enabled = false;

                    if (lstDischargeIndexs.Any(x => x == i))
                    {
                        ((TextBox)gvBarrage.Rows[e.NewEditIndex].Cells[i - 1].Controls[0]).Attributes.Add("readonly", "true");
                    }
                    else if (i == TimesColumnIndex)
                    {
                        ((TextBox)gvBarrage.Rows[e.NewEditIndex].Cells[i - 1].Controls[0]).Attributes.Add("readonly", "true");
                    }
                    else if (i == TotalDischargeUS)
                    {
                        ((TextBox)gvBarrage.Rows[e.NewEditIndex].Cells[i - 1].Controls[0]).Attributes.Add("readonly", "true");
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 2/03/2016 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DischargeDataOfBarrage);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds barrages to the barrage dropdown
        /// Created on 03-03-2016
        /// </summary>
        private void BindBarrageDropdown()
        {
            Dropdownlist.DDLBarrages(ddlBarrage);
            /* ye kaam aaye ga jb tables ka structure change ho ga*/
            //UA_AssociatedStations mdlStation = bllDailyData.GetStationByUserID(SessionManagerFacade.UserInformation.ID);
            //if (SessionManagerFacade.UserInformation.UA_Designations.Name == "SDO")
            //{
            //    Dropdownlist.SetSelectedValue(ddlBarrage, Convert.ToString(mdlStation.StationID));
            //    ddlBarrage.Enabled = false;
            //    ddlBarrage.CssClass = "form-control";
            //}
            //else if (SessionManagerFacade.UserInformation.UA_Designations.Name == "XEN")
            //{
            //    Dropdownlist.SetSelectedValue(ddlBarrage, Convert.ToString(mdlStation.StationID));
            //    ddlBarrage.Enabled = false;
            //    ddlBarrage.CssClass = "form-control";
            //}
            //else if (SessionManagerFacade.UserInformation.UA_Designations.Name == "Data Analyst")
            //{
            //    Dropdownlist.DDLBarrages(ddlBarrage);
            //}
        }

        /// <summary>
        /// this function binds data to gridview
        /// created on: 03/03/2016
        /// </summary>
        private void BindGrid()
        {

            long BarrageID = ddlBarrage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlBarrage.SelectedItem.Value);
            DateTime Date = Convert.ToDateTime(txtDate.Text);
            List<int> AllIDIndexs = new List<int>();
            List<int> lstElementsAtIDIndex = new List<int>();
            DataTable dtAttributes;

            dtAttributes = new DailyDataBLL().GetBarrageAttribute(BarrageID);
            lstBarrageDischarge = new DailyDataBLL().GetBarrageDischargeData(BarrageID, Date);

            if (lstBarrageDischarge.Rows.Count == 0)
            {
                Master.ShowMessage(Message.EnterFrequency.Description, SiteMaster.MessageType.Error);
                gvBarrage.DataSource = null;
                gvBarrage.DataBind();
                return;
            }

            int columns = lstBarrageDischarge.Columns.Count;
            int AttributeRowsCount = dtAttributes.Rows.Count;

            for (int i = 0; i < AttributeRowsCount; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (lstBarrageDischarge.Columns[j].ColumnName.ToUpper().ToString() == dtAttributes.Rows[i][0].ToString().ToUpper())
                    {
                        lstBarrageDischarge.Columns[j].Expression = dtAttributes.Rows[i][1].ToString();
                        break;
                    }
                }
            }

            //for (int i = 0; i < columns; ++i)
            //{
            //    if (lstBarrageDischarge.Columns[i].ColumnName.ToUpper().Contains("ID"))
            //    {
            //        AllIDIndexs.Add(i);
            //    }
            //}


            //for (int i = 0; i < AllIDIndexs.Count; i++)
            //{
            //    for (int j = 0; j < lstBarrageDischarge.Rows.Count; j++)
            //    {
            //        if (lstBarrageDischarge.Rows[j][AllIDIndexs.ElementAt(i)].ToString() != "")
            //        {
            //            lstElementsAtIDIndex.Add(Convert.ToInt32(lstBarrageDischarge.Rows[j][AllIDIndexs.ElementAt(i)]));
            //            break;
            //        }
            //    }
            //}

            //for (int i = 0; i < AllIDIndexs.Count; i++)
            //{
            //    for (int j = 0; j < lstBarrageDischarge.Rows.Count; j++)
            //    {
            //        lstBarrageDischarge.Rows[j][AllIDIndexs.ElementAt(i)] = lstElementsAtIDIndex.ElementAt(i);
            //    }
            //}





            CO_BarrageGaugeReadingFrequency mdlBarrageGaugeReadingFrequency = bllDailyData.GetBarrageGaugeReadingFrequency(BarrageID);
            if (mdlBarrageGaugeReadingFrequency.CO_ReadingFrequency.ID == 4)
            {
                lstBarrageDischarge.Rows[0][0] = "AM";
                lstBarrageDischarge.Rows[1][0] = "PM";
            }
            //lstBarrageDischarge.Columns.Add("Upstream Discharge in Cusecs");



            ///////////////////////////

            //int columns = lstBarrageDischarge.Columns.Count;

            //foreach (DataRow dr in lstBarrageDischarge.Rows)
            //{
            //    double TotalUpstreamDischarge = 0.0;
            //    for (int jj = 0; jj < columns; ++jj)
            //    {
            //        if (!lstBarrageDischarge.Columns[jj].ColumnName.ToUpper().Contains("DISCHARGE"))
            //            continue;

            //        if (dr[jj] == System.DBNull.Value)
            //            break;


            //        double UpstreamDischarge = Convert.ToDouble(dr[jj].ToString());
            //        TotalUpstreamDischarge += UpstreamDischarge;
            //    }

            //    if (TotalUpstreamDischarge <= 0)
            //        dr["Upstream Discharge in Cusecs"] = 0;
            //    else
            //        dr["Upstream Discharge in Cusecs"] = TotalUpstreamDischarge.ToString();
            //}

            ///////////////////////////



            gvBarrage.DataSource = lstBarrageDischarge;

            TemplateField tc = (TemplateField)gvBarrage.Columns[0];

            ViewState[DS] = lstBarrageDischarge;

            gvBarrage.DataBind();

            gvBarrage.HeaderRow.Cells.Add(gvBarrage.HeaderRow.Cells[0]);

            foreach (GridViewRow gr in gvBarrage.Rows)
            {
                gr.Cells.Add(gr.Cells[0]);
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
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

        /// <summary>
        /// This function calculates the discharge based on the Gauge ID and Gauge Value
        /// Created On 10-03-2016
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns>double</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static double? CalculateDischarge(long _AttributeID, double _GaugeValue)
        {
            DailyDataBLL bllDailyData = new DailyDataBLL();
            CO_Attribute mdlAttribute = bllDailyData.GetAttribute(_AttributeID);
            double? Discharge = null;
            if (mdlAttribute.GaugeID != null)
            {
                Discharge = new DailyDataBLL().CalculateDischarge(Convert.ToInt64(mdlAttribute.GaugeID), _GaugeValue);
            }
            return Discharge;
        }


        #region Audit Trail

        List<int> lstIDIndexsAudit = new List<int>();
        protected void btnAuditTrail_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnAuditTrail = (Button)sender;
                GridViewRow gvRow = (GridViewRow)btnAuditTrail.NamingContainer;

                if (gvRow != null)
                {
                    DateTime Date = Convert.ToDateTime(txtDate.Text);
                    string Time = "";
                    Time = gvRow.Cells[0].Text;

                    if (Time == "AM")
                    {
                        Time = "00:00";
                    }
                    else if (Time == "PM")
                    {
                        Time = "12:00";
                    }

                    DateTime TimeFormat = Convert.ToDateTime(Time);
                    TimeFormat.ToString("HH:mm");
                    DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);
                    string FormatedDateTime = ReadingDateTime.ToString("yyyy-MM-dd" + " " + "HH:mm");

                    lblatBarrageName.Text = ddlBarrage.SelectedItem.Text;
                    lblatTimeHrs.Text = Utility.GetFormattedDateTime(ReadingDateTime);

                    long BarrageID = ddlBarrage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlBarrage.SelectedItem.Value);
                    BindAuditTrailGrid(BarrageID, FormatedDateTime);
                    BindGrid();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AuditHistory", "$('#auditTrail').modal();", true);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuditTrail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (e.Row.Cells[0].Text.ToUpper().IndexOf("TIME") != -1)
                    {
                        ChangedTimeColumnIndex = 0;
                    }

                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        if (e.Row.Cells[i].Text.ToUpper().IndexOf("ID") != -1)
                        {
                            e.Row.Cells[i].Attributes.Add("style", "display:none;");
                            lstIDIndexsAudit.Add(i);
                        }
                        else if (ChangedTimeColumnIndex == -1 && e.Row.Cells[i].Text.ToUpper().IndexOf("TIME") != -1)
                        {
                            ChangedTimeColumnIndex = i;
                        }
                    }
                }

                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        if (lstIDIndexsAudit.Any(y => y == i))
                        {
                            e.Row.Cells[i].Attributes.Add("style", "display:none;");
                        }
                    }

                    if (ChangedTimeColumnIndex != -1)
                    {
                        string ChangedTimeString = e.Row.Cells[ChangedTimeColumnIndex].Text;
                        DateTime ChangedTime = Convert.ToDateTime(ChangedTimeString);
                        e.Row.Cells[ChangedTimeColumnIndex].Text = Utility.GetFormattedDateTime(ChangedTime);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                gvBarrage.EditIndex = -1;
                BindGrid();
            }
            //finally
            //{
            //    gvBarrage.EditIndex = -1;
            //    BindGrid();
            //}
        }

        public void BindAuditTrailGrid(long _BarrageID, string _DateTime)
        {
            DataTable dtAuditTrail = new DataTable();

            dtAuditTrail = new DailyDataBLL().GetBarrageDailyDischargeDataHistory(_BarrageID, _DateTime);
            gvAuditTrail.DataSource = dtAuditTrail;
            gvAuditTrail.DataBind();
        }

        #endregion
    }
}