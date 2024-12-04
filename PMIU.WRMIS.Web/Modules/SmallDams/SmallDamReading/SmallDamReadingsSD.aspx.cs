using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common;

using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;

namespace PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading
{
    public partial class SmallDamReadingsSD : BasePage
    {
        #region Initialize

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    btnSave.Enabled = base.CanAdd;
                    SetPageTitle();
                    SmallDamsBLL smallDamBll = new SmallDamsBLL();
                    InitialBind();

                }
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Events

        #region Grid

        protected void gvChannelReading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Key
                //ID,Division,DamName,DamType

                DataKey key = gvChannelReading.DataKeys[e.Row.RowIndex];
                string ID = key.Values["ID"].ToString();
                string ChannelCode = key.Values["ChannelCode"].ToString();
                string Name = key.Values["Name"].ToString();
                string Capacity = Convert.ToString(key.Values["Capacity"]);
                string Gauge = Convert.ToString(key.Values["Gauge"]);
                string Discharge = Convert.ToString(key.Values["Discharge"]);
                string ReadingDate = Convert.ToString(key.Values["ReadingDate"]);
                string FromTime = Convert.ToString(key.Values["FromTime"]);
                string ToTime = Convert.ToString(key.Values["ToTime"]);
                string ReaderName = Convert.ToString(key.Values["ReaderName"]);
                string Remarks = Convert.ToString(key.Values["Remarks"]);
                string MaxGauge = Convert.ToString(key.Values["MaxGauge"]);
                string MaxDischarge = Convert.ToString(key.Values["MaxDischarge"]);
                #endregion
                #region Control
                Label lblChannelCode = (Label)e.Row.FindControl("lblChannelCode");
                Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                Label lblChannelCapacity = (Label)e.Row.FindControl("lblChannelCapacity");
                TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");

                TextBox txtReadingDate = (TextBox)e.Row.FindControl("txtReadingDate");

                TextBox txtFromDate = (TextBox)e.Row.FindControl("txtFromDate");
                TextBox txtFromTime = (TextBox)e.Row.FindControl("txtFromTime");

                TextBox txtToDate = (TextBox)e.Row.FindControl("txtToDate");
                TextBox txtToTime = (TextBox)e.Row.FindControl("txtToTime");

                TextBox txtReaderName = (TextBox)e.Row.FindControl("txtReaderName");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");

                TimePicker tpFrom = (TimePicker)e.Row.FindControl("tpFrom");
                TimePicker tpTo = (TimePicker)e.Row.FindControl("tpTo");



                #endregion
                if (ChannelCode != "")
                    lblChannelCode.Text = ChannelCode;
                if (Name != "")
                    lblChannelName.Text = Name;
                if (Capacity != "")
                    lblChannelCapacity.Text = Capacity;


                if (MaxGauge != "")
                {
                    txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + 0.00 + "','" + MaxGauge + "');");
                }

                if (MaxDischarge != "")
                {
                    txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + 0.00 + "','" + MaxDischarge + "');");
                }

                if (Gauge != "")
                    txtGauge.Text = String.Format("{0:0.00}",Convert.ToDouble(Gauge));
                else txtGauge.Text = "";

                if (Discharge != "")
                    txtDischarge.Text = String.Format("{0:0.00}",Convert.ToDouble(Discharge));
                else txtDischarge.Text = "";

                string res = hdnDate.Value;
                //if (ReadingDate != "")
                //    txtReadingDate.Text = ReadingDate;
                //else txtReadingDate.Text = hdnDate.Value;


                if (FromTime != "")
                {
                    txtFromDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(FromTime));
                    txtFromTime.Text = String.Format("{0:HH:mm}", Convert.ToDateTime(FromTime));

                    tpFrom.SetTime(FromTime);
                }
                else
                {
                    txtFromDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
                    txtFromTime.Text = "";
                }

                if (ToTime != "")
                {
                    txtToDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ToTime));
                    txtToTime.Text = String.Format("{0:HH:mm}", Convert.ToDateTime(ToTime));

                    tpTo.SetTime(ToTime);
                }
                else
                {
                    txtToDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
                    txtToTime.Text = "";
                }

                if (ReaderName != "")
                    txtReaderName.Text = ReaderName;
                else txtReaderName.Text = "";
                if (Remarks != "")
                    txtRemarks.Text = Remarks;
                else txtRemarks.Text = "";



            }
        }

        protected void gvChannelReading_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvChannelReading.EditIndex = -1;
                BindSearchReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReading_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannelReading.PageIndex = e.NewPageIndex;
                gvChannelReading.EditIndex = -1;
                BindSearchReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Dropdown


        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
            if (ddlDivision.SelectedIndex == 0)
            {
                EmptyDropdown();

            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
            if (ddlSubDivision.SelectedIndex == 0)
            {
                Dropdownlist.DDLDamName(ddlDamName, false, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLDamName(ddlDamName, false, Convert.ToInt64(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
        }


        protected void ddlDamName_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmptyGrid();
        }
        #endregion

        #region TextChanged


        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            EmptyGrid();
        }
        //protected void txtFromDate_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox fromDatetxt = (TextBox)sender;
        //    if (fromDatetxt.Text != "")
        //    {
        //        DateTime fromDate = Convert.ToDateTime(fromDatetxt.Text);

        //        if (fromDate != Convert.ToDateTime(txtDate.Text))
        //        {
        //            Master.ShowMessage("From date is not valid.", SiteMaster.MessageType.Error);
        //        }
        //    }
        //}

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            TextBox toDatetxt = (TextBox)sender;
            if (toDatetxt.Text != "")
            {
                DateTime toDate = Convert.ToDateTime(toDatetxt.Text);

                if (toDate < Convert.ToDateTime(txtDate.Text))
                {
                    Master.ShowMessage("To date is not valid.", SiteMaster.MessageType.Error);
                }
            }

        }

        #endregion

        #region Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hdnDamID.Value = ddlDamName.SelectedItem.Value;
            hdnDate.Value = Convert.ToString(Convert.ToDateTime(txtDate.Text));
            if (rdbtnEntryType.SelectedValue == "1")
            {
                BindSearchReadingGrid();
            }
            else
            {
                SaveHistoryValues();
                Response.Redirect("DamReadingsSD.aspx?DamID=" + hdnDamID.Value + "&Date=" + hdnDate.Value, false);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUsers = SessionManagerFacade.UserInformation;
            hdnDamID.Value = ddlDamName.SelectedItem.Value;
            hdnDate.Value = Convert.ToString(Convert.ToDateTime(txtDate.Text));
            //bool saveResult = false;
            //int validDate = 0;

            List<SD_SmallChannelData> lstSCD = new List<SD_SmallChannelData>();

            foreach (GridViewRow Row in gvChannelReading.Rows)
            {

                DataKey key = gvChannelReading.DataKeys[Row.RowIndex];
                SD_SmallChannelData smallChData = new SD_SmallChannelData();

                smallChData.ID = Convert.ToInt64(key.Values["ID"]);



                if (smallChData.ID == 0)
                {
                    smallChData.CreatedBy = Convert.ToInt32(mdlUsers.ID);
                    smallChData.CreatedDate = DateTime.Now;
                    smallChData.ModifiedBy = null;
                    smallChData.ModifiedDate = null;
                }
                else
                {
                    smallChData.ModifiedBy = Convert.ToInt32(mdlUsers.ID);
                    smallChData.ModifiedDate = DateTime.Now;
                    smallChData.CreatedBy = Convert.ToInt32(key.Values["CreatedBy"]);
                    smallChData.CreatedDate = Convert.ToDateTime(key.Values["CreatedDate"]);
                }

                smallChData.SmallChannelID = Convert.ToInt64(key.Values["SmallChannelID"]);

                TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                if (txtGauge.Text != "")
                    smallChData.Gauge = Convert.ToDouble(txtGauge.Text);

                TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                if (txtDischarge.Text != "")
                    smallChData.Discharge = Convert.ToDouble(txtDischarge.Text);

                smallChData.ReadingDate = Convert.ToDateTime(hdnDate.Value);

                TextBox txtReaderName = (TextBox)Row.FindControl("txtReaderName");
                if (txtReaderName.Text != "")
                    smallChData.ReaderName = Convert.ToString(txtReaderName.Text);

                TextBox txtRemarks = (TextBox)Row.FindControl("txtRemarks");
                if (txtRemarks.Text != "")
                    smallChData.Remarks = Convert.ToString(txtRemarks.Text);



                ////////////////// New Code ////////////////////////////

                TextBox txtFromDate = (TextBox)Row.FindControl("txtFromDate");
                TimePicker tpFrom = (TimePicker)Row.FindControl("tpFrom");
                string tpFromStr = tpFrom.GetTime();

                string dtFromStr = txtFromDate.Text + " " + tpFromStr;

                DateTime dtFrom = DateTime.Parse(dtFromStr);


                TextBox txtToDate = (TextBox)Row.FindControl("txtToDate");
                TimePicker tpTo = (TimePicker)Row.FindControl("tpTo");
                string tpToStr = tpTo.GetTime();

                string dtToStr = txtToDate.Text + " " + tpToStr;

                DateTime dtTo = DateTime.Parse(dtToStr);

                smallChData.FromTime = dtFrom;
                smallChData.ToTime = dtTo;

                lstSCD.Add(smallChData);

                if (dtTo < dtFrom)
                {
                    Master.ShowMessage("The To DateTime can't be lesser than to From DateTime.", SiteMaster.MessageType.Error);
                    return;
                }





                //TextBox fromDate = (TextBox)Row.FindControl("txtFromDate");
                ////TextBox fromTime = (TextBox)Row.FindControl("txtFromTime");
                //TimePicker tpFrom = (TimePicker)Row.FindControl("tpFrom");
                //string tpFromStr = tpFrom.GetTime();

                ////string[] SplitFromTime = fromTime.Text.Split(':');
                //DateTime FromDateTime = new DateTime();
                
                //FromDateTime = Convert.ToDateTime(fromDate.Text);
                //FromDateTime = Convert.ToDateTime(tpFromStr);

                ////FromDateTime = FromDateTime.AddHours(Convert.ToInt32(SplitFromTime[0]));
                ////FromDateTime = FromDateTime.AddMinutes(Convert.ToInt32(SplitFromTime[1]));

                //smallChData.FromTime = FromDateTime;

                //TextBox toDate = (TextBox)Row.FindControl("txtToDate");
                ////TextBox toTime = (TextBox)Row.FindControl("txtToTime");
                //TimePicker tpTo = (TimePicker)Row.FindControl("tpTo");
                //string tpToStr = tpTo.GetTime();


                //if (toDate.Text != "" && (Convert.ToDateTime(toDate.Text) < Convert.ToDateTime(txtDate.Text)))
                //{
                //    validDate = 1;
                //}
                //if (toDate.Text != "" && (Convert.ToDateTime(toDate.Text) == Convert.ToDateTime(txtDate.Text)))
                //{
                //    //if (fromTime.Text != "" && toTime.Text != "")
                //    //{
                //    //if (fromTime.Text != "00:00" && toTime.Text != "00:00")
                //    if (tpFromStr != "12:00 AM" && tpToStr != "12:00 AM")
                //    {


                //        //int THour = Convert.ToDateTime(toTime.Text).Hour;
                //        //int FHour = Convert.ToDateTime(fromTime.Text).Hour;
                //        //int TMin = Convert.ToDateTime(toTime.Text).Minute;
                //        //int FMin = Convert.ToDateTime(fromTime.Text).Minute;

                //        int THour = Convert.ToDateTime(tpToStr).Hour;
                //        int FHour = Convert.ToDateTime(tpFromStr).Hour;
                //        int TMin = Convert.ToDateTime(tpToStr).Minute;
                //        int FMin = Convert.ToDateTime(tpFromStr).Minute;

                //        if (THour == FHour)
                //        {
                //            if (FMin > TMin)
                //            {
                //                validDate = 5;
                //            }
                //        }
                //        else if (FHour > THour)
                //        {
                //            validDate = 6;
                //        }
                //    }
                //    //}

                //    else if (tpFromStr != "12:00 AM" && tpToStr == "12:00 AM")
                //    {
                //        validDate = 2;
                //    }
                //    else if (tpFromStr == "12:00 AM" && tpToStr != "12:00 AM")
                //    {
                //        validDate = 3;
                //    }
                //    //else
                //    //{
                //    //    validDate = 4;
                //    //}

                //}
                ////string[] SplitToTime = toTime.Text.Split(':');
                //DateTime ToDateTime = new DateTime();
                //ToDateTime = Convert.ToDateTime(tpToStr);
                ////ToDateTime = ToDateTime.AddHours(Convert.ToInt32(SplitToTime[0]));
                ////ToDateTime = ToDateTime.AddMinutes(Convert.ToInt32(SplitToTime[1]));

                //smallChData.ToTime = ToDateTime;



                //if (validDate == 0)
                //{
                    
                //}

            }

            bool res = new SmallDamsBLL().SaveChannelData(lstSCD);
            if (res)
            {
                Master.ShowMessage("Data Saved Successfully.", SiteMaster.MessageType.Success);
                //saveResult = true;
            }
            else
            {
               // saveResult = false;
            }



            //if (saveResult)
            //{
            //    Master.ShowMessage(Message.ValidRecordsSaved.Description, SiteMaster.MessageType.Success);
            //}
            //else
            //{
            //    if (validDate == 0)
            //    {
            //        Master.ShowMessage(Message.SomeRecordsNotSaved.Description, SiteMaster.MessageType.Error);
            //    }
            //    else if (validDate == 1)
            //    {
            //        //Todate Less then Date
            //        Master.ShowMessage("Records not save because ToDate should not less then from date.", SiteMaster.MessageType.Error);
            //    }
            //    else if (validDate == 2)
            //    {
            //        //To time is empty
            //        Master.ShowMessage("Running Time To is required.", SiteMaster.MessageType.Error);
            //    }
            //    else if (validDate == 3)
            //    {
            //        //From time is empty
            //        Master.ShowMessage("Running Time From is required.", SiteMaster.MessageType.Error);
            //    }
            //    //else if (validDate == 4)
            //    //{
            //    //    //both to from time empty
            //    //    Master.ShowMessage("Records not save some records have invalid dates.", SiteMaster.MessageType.Error);
            //    //}
            //    else if (validDate == 5)
            //    {
            //        //from min is less then to min
            //        Master.ShowMessage("Invalid minutes enter.", SiteMaster.MessageType.Error);
            //    }
            //    else if (validDate == 6)
            //    {
            //        //from hour les then to hour
            //        Master.ShowMessage("Invalid hours enter.", SiteMaster.MessageType.Error);
            //    }
            //}
            ////reload grid
            BindSearchReadingGrid();

        }
        #endregion

        #endregion


        #region Function
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        void InitialBind()
        {
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
            if (boundryLvlID == null)
            {
                boundryLvlID = 0;
            }
            if (userID > 0)
            {
                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                if (ddlDivision.Items.Count == 2)
                {
                    ddlDivision.SelectedIndex = 1;
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    EmptyDropdown();
                }
            }
            else
            {
                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                if (ddlDivision.Items.Count == 2)
                {
                    ddlDivision.SelectedIndex = 1;
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    EmptyDropdown();
                }
            }


            if (!string.IsNullOrEmpty(Request.QueryString["ShowHistroy"]))
            {
                HistoryValues();
            }
        }

        private void EmptyDropdown()
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLDamName(ddlDamName, true, -1, (int)Constants.DropDownFirstOption.Select);

        }

        private void EmptyGrid()
        {
            if (gvChannelReading.Visible == true)
            {
                btnSave.Visible = false;
                gvChannelReading.DataSource = null;
                gvChannelReading.DataBind();
                gvChannelReading.Visible = false;
            }

        }
        protected void SaveHistoryValues()
        {
            string DivisionID = "0";
            string SubDivisionID = "0";
            string DamNameID = "0";
            string EntryType = "0";
            string date = "0";
            if (ddlDivision.SelectedIndex != 0)
            {
                DivisionID = ddlDivision.SelectedIndex.ToString();
            }
            if (ddlSubDivision.SelectedIndex != 0)
            {
                SubDivisionID = ddlSubDivision.SelectedIndex.ToString();
            }
            if (ddlDamName.SelectedIndex != 0)
            {
                DamNameID = ddlDamName.SelectedIndex.ToString();
            }

            EntryType = rdbtnEntryType.SelectedValue;

            date = txtDate.Text;

            Session["SearchValuesSD"] = null;
            object obj = new
            {
                DivisionID,
                SubDivisionID,
                DamNameID,
                EntryType,
                date

            };
            Session["SearchValuesSD"] = obj;
        }

        protected void HistoryValues()
        {
            object currentObj = Session["SearchValuesSD"] as object;
            if (currentObj != null)
            {
                ddlDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                if (ddlDivision.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }

                ddlSubDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("SubDivisionID").GetValue(currentObj));
                if (ddlSubDivision.SelectedIndex != 0)
                {
                    Dropdownlist.DDLDamName(ddlDamName, false, Convert.ToInt64(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                ddlDamName.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DamNameID").GetValue(currentObj));
                rdbtnEntryType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("EntryType").GetValue(currentObj));
                txtDate.Text = Convert.ToString(currentObj.GetType().GetProperty("date").GetValue(currentObj));


                //BindSearchReadingGrid();
            }

        }

        private void BindSearchReadingGrid()
        {
            try
            {
                Int64? SelectedDivisionID = null;
                Int64? SelectedSubDivisionID = null;
                Int64? SelectedDamID = null;
                DateTime ReadingDate = System.DateTime.Now;

                if (ddlDivision.SelectedIndex != 0)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedDivisionID = 0;
                }
                if (ddlSubDivision.SelectedIndex != 0)
                {
                    SelectedSubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedSubDivisionID = 0;
                }
                if (ddlDamName.SelectedIndex != 0)
                {
                    SelectedDamID = Convert.ToInt64(ddlDamName.SelectedItem.Value);
                }
                else
                {
                    SelectedDamID = 0;
                }

                ReadingDate = Convert.ToDateTime(txtDate.Text);

                SaveHistoryValues();
                btnSave.Visible = true;
                List<SDChannelReading1_Result> lstDamType = new SmallDamsBLL().GetChannelReading(SelectedDamID, SelectedDivisionID, SelectedSubDivisionID, ReadingDate);
                gvChannelReading.DataSource = lstDamType;
                gvChannelReading.DataBind();
                gvChannelReading.Visible = true;


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion







    }
}