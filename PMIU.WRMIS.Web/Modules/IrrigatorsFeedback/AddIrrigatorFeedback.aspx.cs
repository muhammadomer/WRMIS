using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigatorsFeedback;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.ComplaintsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigatorsFeedback
{
    public partial class AddIrrigatorFeedback : BasePage
    {
        long IrrigatorID;
        long IrrigatorFeedbackID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                    //SetPageTitle();
                    //hlBack.NavigateUrl = "~/Modules/IrrigatorsFeedback/SearchIrrigatorFeedback.aspx?ShowHistory=true";
                    if (!string.IsNullOrEmpty(Request.QueryString["IrrigatorID"]))
                    {
                        LoadAddIrrigatorFeedback();
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["IrrigatorFeedbackID"]))
                    {
                        LoadViewIrrigatorFeedback();
                    }
                    BindChart();
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindData(long _IrrigatorID)
        {
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            IF_Irrigator mdlIrrigatorFeedback = bllIrrigatorFeedback.GetIrrigatorByID(_IrrigatorID);
            DataTable dtIrrigatorInformation = bllIrrigatorFeedback.GetIrrigatorInformation(_IrrigatorID);

            hdnDivisionID.Value = dtIrrigatorInformation.Rows[0]["DivisionID"].ToString();
            hdnChannelID.Value = dtIrrigatorInformation.Rows[0]["ChannelID"].ToString();
            Session["CHANNELID"] = Convert.ToString(hdnChannelID.Value);

            lblCallDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
            //lblZone.Text = dtIrrigatorInformation.Rows[0]["Zone"].ToString();
            //lblCircle.Text = dtIrrigatorInformation.Rows[0]["Circle"].ToString();
            lblDivision.Text = dtIrrigatorInformation.Rows[0]["Division"].ToString();
            lblChannel.Text = dtIrrigatorInformation.Rows[0]["Channel"].ToString();
            lblIrrigatorName.Text = dtIrrigatorInformation.Rows[0]["IrrigatorName"].ToString();
            lblIrrigatorMobileNo1.Text = dtIrrigatorInformation.Rows[0]["Mobile1"].ToString();
            lblIrrigatorMobileNo2.Text = dtIrrigatorInformation.Rows[0]["Mobile2"].ToString();
            lblHeadPercent.Text = String.Format("{0:0.00}", (dtIrrigatorInformation.Rows[0]["HeadPercent"]));
            //lblHeadPercent.Text = dtIrrigatorInformation.Rows[0]["HeadPercent"].ToString();
            //lblTailStatusDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(dtIrrigatorInformation.Rows[0]["TailStatusDate"].ToString()));
            //lblTailStatusTime.Text = Utility.GetFormattedTime(Convert.ToDateTime(dtIrrigatorInformation.Rows[0]["TailStatusTime"].ToString()));
            if (dtIrrigatorInformation.Rows[0]["TailStatusDate"].ToString() != String.Empty && dtIrrigatorInformation.Rows[0]["TailStatusTime"].ToString() != String.Empty)
            {
                lblTailStatusDateAndTime.Text = Utility.GetFormattedDate(Convert.ToDateTime(dtIrrigatorInformation.Rows[0]["TailStatusDate"].ToString())) + " " + Utility.GetFormattedTime(Convert.ToDateTime(dtIrrigatorInformation.Rows[0]["TailStatusTime"].ToString()));
            }

            //if (dtIrrigatorInformation.Rows[0]["IrrigatorStatus"].ToString() == "1")
            //{
            //    lblIrrigatorStatus.Text = "Active";
            //}
            //else
            //{
            //    lblIrrigatorStatus.Text = "InActive";
            //}

            dynamic IrrigatorStatus = CommonLists.GetStatuses(dtIrrigatorInformation.Rows[0]["IrrigatorStatus"].ToString()).FirstOrDefault<dynamic>();
            lblIrrigatorStatus.Text = IrrigatorStatus.GetType().GetProperty("Name").GetValue(IrrigatorStatus, null);

            //if (dtIrrigatorInformation.Rows[0]["TailStatus"].ToString() == "1")
            //{
            //    lblTailStatus.Text = "Dry";
            //}
            //else if (dtIrrigatorInformation.Rows[0]["TailStatus"].ToString() == "2")
            //{
            //    lblTailStatus.Text = "Short Tail";
            //}
            //else if (dtIrrigatorInformation.Rows[0]["TailStatus"].ToString() == "3")
            //{
            //    lblTailStatus.Text = "Authorized Tail";
            //}
            //else if (dtIrrigatorInformation.Rows[0]["TailStatus"].ToString() == "4")
            //{
            //    lblTailStatus.Text = "Excessive Tail";
            //}


            dynamic TailStatus = CommonLists.GetTailStatuses(dtIrrigatorInformation.Rows[0]["TailStatus"].ToString()).FirstOrDefault<dynamic>();
            if (TailStatus != null)
            {
                lblTailStatus.Text = TailStatus.GetType().GetProperty("Name").GetValue(TailStatus, null);
            }



            #region Dropdowns


            Dropdownlist.DDLYesNo(ddlRotationalViolation);
            Dropdownlist.DDLYesNo(ddlWaterTheft);
            Dropdownlist.DDLTailStatus(ddlTailStatusByIrrigator);
            Dropdownlist.DDLVillagesByDivisionID(ddlVillage, mdlIrrigatorFeedback.CO_Division.ID);
            #endregion



        }

        //protected void ddlWaterTheft_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        if (ddlWaterTheft.SelectedItem.Value == string.Empty || Convert.ToInt64(ddlWaterTheft.SelectedItem.Value) == 0)
        //        {
        //            txtLocal.Enabled = false;
        //            txtLocal.Text = "";

        //            txtTotalRDLeft.Enabled = false;
        //            txtTotalRDLeft.Text = "";

        //            txtTotalRDRight.Enabled = false;
        //            txtTotalRDRight.Text = "";

        //            ddlVillage.Enabled = false;
        //            ddlVillage.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            IrrigatorID = Convert.ToInt64(Request.QueryString["IrrigatorID"]);
        //            //IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
        //            //IF_Irrigator mdlIrrigatorFeedback = bllIrrigatorFeedback.GetIrrigatorByID(IrrigatorID);


        //            txtLocal.Enabled = true;
        //            txtTotalRDLeft.Enabled = true;
        //            txtTotalRDRight.Enabled = true;
        //            ddlVillage.Enabled = true;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        private void AddIrrigatorsFeedback()
        {
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            IF_IrrigatorFeedback mdlIrrigatorFeedback = new IF_IrrigatorFeedback();
            IrrigatorID = Convert.ToInt64(Request.QueryString["IrrigatorID"]);

            mdlIrrigatorFeedback.IrrigatorID = IrrigatorID;
            mdlIrrigatorFeedback.FeedbackDate = DateTime.Now;
            mdlIrrigatorFeedback.TailStatus = ddlTailStatusByIrrigator.SelectedItem.Value;
            mdlIrrigatorFeedback.RotaionalViolation = ddlRotationalViolation.SelectedItem.Value;
            mdlIrrigatorFeedback.WaterTheft = ddlWaterTheft.SelectedItem.Value;
            mdlIrrigatorFeedback.Remarks = txtRemarks.Text;

            if (Convert.ToInt64(ddlWaterTheft.SelectedItem.Value) == 1)
            {
                mdlIrrigatorFeedback.LocalName = txtLocal.Text;
                if (ddlVillage.SelectedItem.Value != "")
                {
                    mdlIrrigatorFeedback.VillageID = Convert.ToInt64(ddlVillage.SelectedItem.Value);
                }
                if (txtTotalRDLeft.Text == "" || txtTotalRDRight.Text == "")
                {
                    mdlIrrigatorFeedback.RD = null;
                }
                else
                {
                    if (txtTotalRDLeft.Text == "")
                    {
                        mdlIrrigatorFeedback.RD = Calculations.CalculateTotalRDs("0", txtTotalRDRight.Text);
                    }
                    else if (txtTotalRDRight.Text == "")
                    {
                        mdlIrrigatorFeedback.RD = Calculations.CalculateTotalRDs(txtTotalRDLeft.Text, "0");
                    }
                    else
                    {
                        mdlIrrigatorFeedback.RD = Calculations.CalculateTotalRDs(txtTotalRDLeft.Text, txtTotalRDRight.Text);
                    }

                }

            }

            long IrrigatorFeedbackID = bllIrrigatorFeedback.AddIrrigatorFeedback(mdlIrrigatorFeedback);
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
            long DivisionID = Convert.ToInt64(hdnDivisionID.Value);
            long ChannelID = Convert.ToInt64(hdnChannelID.Value);
            string CompStatus = null;
            string ComplaintDetails = null;
            string AGTailStatus = null;
            string AGRotationalViolation = null;
            string AGWaterTheft = null;

            if (ddlTailStatusByIrrigator.SelectedItem.Value != string.Empty)
            {
                long TailStatusValue = Convert.ToInt64(ddlTailStatusByIrrigator.SelectedItem.Value);
                if (TailStatusValue == (long)Constants.TailStatusByIrrigator.Dry)
                {
                    ComplaintDetails = "This complaint is generated from Irrigator Feedback on Channel: " + lblChannel.Text + ", and tail status is: " + ddlTailStatusByIrrigator.SelectedItem.Text;
                    CompStatus = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.IF_DT), DivisionID, IrrigatorFeedbackID, ChannelID, ComplaintDetails);
                    AGTailStatus = CompStatus;
                }
                else if (TailStatusValue == (long)Constants.TailStatusByIrrigator.ShortTail)
                {
                    ComplaintDetails = "This complaint is generated from Irrigator Feedback on Channel: " + lblChannel.Text + ", and tail status is: " + ddlTailStatusByIrrigator.SelectedItem.Text;
                    CompStatus = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.IF_ST), DivisionID, IrrigatorFeedbackID, ChannelID, ComplaintDetails);
                    AGTailStatus = CompStatus;
                }
            }
            if (ddlRotationalViolation.SelectedItem.Value != string.Empty && Convert.ToInt64(ddlRotationalViolation.SelectedItem.Value) == (long)Constants.YesNo.Yes)
            {
                ComplaintDetails = "This complaint is generated for rotational violation from Irrigator Feedback on Channel: " + lblChannel.Text;
                CompStatus = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.IF_RV), DivisionID, IrrigatorFeedbackID, ChannelID, ComplaintDetails);
                AGRotationalViolation = CompStatus;
            }
            if (ddlWaterTheft.SelectedItem.Value != string.Empty && Convert.ToInt64(ddlWaterTheft.SelectedItem.Value) == (long)Constants.YesNo.Yes)
            {
                ComplaintDetails = "This complaint is generated for water theft from Irrigator Feedback on Channel: " + lblChannel.Text;
                CompStatus = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.IF_WT), DivisionID, IrrigatorFeedbackID, ChannelID, ComplaintDetails);
                AGWaterTheft = CompStatus;
            }
            //hdnDivisionID.Value = "0";
            if (CompStatus == null)
            {
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            else
            {
                UA_SMSNotification mdlSMS = new UA_SMSNotification();
                //mdlSMS.SMSText = "Complaint of Type:" + ddlComplaintType.SelectedItem.Text + " has been logged with Complaint Id:" + mdlAddComplaint.ComplaintNumber;


                if (AGTailStatus != null && AGRotationalViolation != null && AGWaterTheft != null)
                {
                    Master.ShowMessage("Record is saved and following Complaint are Generated: " + AGTailStatus + "," + AGWaterTheft + "," + AGRotationalViolation, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID: " + AGTailStatus + "," + AGWaterTheft + "," + AGRotationalViolation + ". The progress can be seen through Public Website.";
                }

                else if (AGTailStatus != null && AGRotationalViolation == null && AGWaterTheft == null)
                {
                    Master.ShowMessage("Record is saved and following Complaint is Generated: " + AGTailStatus, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGTailStatus + ". The progress can be seen through Public Website.";
                }
                else if (AGTailStatus == null && AGRotationalViolation != null && AGWaterTheft == null)
                {
                    Master.ShowMessage("Record is saved and following Complaint is Generated: " + AGRotationalViolation, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGRotationalViolation + ". The progress can be seen through Public Website.";
                }
                else if (AGTailStatus == null && AGRotationalViolation == null && AGWaterTheft != null)
                {
                    Master.ShowMessage("Record is saved and following Complaint is Generated: " + AGWaterTheft, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGWaterTheft + ". The progress can be seen through Public Website.";
                }

                if (AGTailStatus != null && AGRotationalViolation != null && AGWaterTheft == null)
                {
                    Master.ShowMessage("Record is saved and following Complaint are Generated: " + AGTailStatus + "," + AGRotationalViolation, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGTailStatus + "," + AGRotationalViolation + ". The progress can be seen through Public Website.";
                }
                if (AGTailStatus != null && AGRotationalViolation == null && AGWaterTheft != null)
                {
                    Master.ShowMessage("Record is saved and following Complaint are Generated: " + AGTailStatus + "," + AGWaterTheft, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGTailStatus + "," + AGWaterTheft + ". The progress can be seen through Public Website.";
                }
                if (AGTailStatus == null && AGRotationalViolation != null && AGWaterTheft != null)
                {
                    Master.ShowMessage("Record is saved and following Complaint are Generated: " + AGRotationalViolation + "," + AGWaterTheft, SiteMaster.MessageType.Success);
                    mdlSMS.SMSText = "Your feedback has been entered in the system with the Complaint ID:" + AGRotationalViolation + "," + AGWaterTheft + ". The progress can be seen through Public Website.";
                }


                ComplaintsManagementBLL bllComplaintsManagement = new ComplaintsManagementBLL();
                mdlSMS.NotificationEventID = 20;
                mdlSMS.UserID = 1;// SessionManagerFacade.UserInformation.ID;
                mdlSMS.MobileNumber = lblIrrigatorMobileNo1.Text;
                //mdlSMS.SMSText = "Complaint of Type:" + ddlComplaintType.SelectedItem.Text + " has been logged with Complaint Id:" + mdlAddComplaint.ComplaintNumber;
                mdlSMS.Status = 0;
                mdlSMS.TryCount = 0;
                mdlSMS.CreatedDate = DateTime.Now;
                mdlSMS.CreatedBy = SessionManagerFacade.UserInformation.ID;
                bool IsSaved = bllComplaintsManagement.InsertComplanintEntryInSMSTable(mdlSMS);
                //Master.ShowMessage(Message.RecordSavedComplaintGenerated.Description, SiteMaster.MessageType.Success);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AddIrrigatorsFeedback();
                btnSave.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }
        //        protected void ddlpopTailStatus_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            if (ddlpopTailStatus.SelectedItem.Value != "0")
        //            {
        //                DataSet DS = new ComplaintsManagementBLL().GetTailStatusLastTenDays(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt32(ddlpopTailStatus.SelectedItem.Value));
        //                //DataSet DS = new ComplaintsManagementBLL().GetTailStatusLastTenDays(2416);

        //                StringBuilder strScript = new StringBuilder();

        //                try
        //                {

        //                    if (DS.Tables[0].Rows.Count > 0)
        //                    {
        //                        dvLtScripts.InnerText = "";
        //                        chart_div.InnerText = "";

        //                        strScript.Append(@"
        //                    function drawVisualization() {         
        //                    var data = google.visualization.arrayToDataTable([  
        //                    ['ReadingDate', 'Dry','ShortTail','AuthorizedTail','ExcessiveTail'],");

        //                        foreach (DataRow row in DS.Tables[0].Rows)
        //                        {
        //                            string DailyDate = "";
        //                            if (row["ReadingDateTime"].ToString() != "")
        //                                DailyDate = Utility.GetFormattedDateTimeJavaScript(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
        //                            strScript.Append("['" + DailyDate + "'," + Convert.ToInt32(row["Dry"]) + "," + Convert.ToInt32(row["ShortTail"]) + "," + Convert.ToInt32(row["AuthorizedTail"]) + "," + Convert.ToInt32(row["ExcessiveTail"]) + "],");
        //                        }

        //                        strScript.Remove(strScript.Length - 1, 1);
        //                        strScript.Append("]);");

        //                        strScript.Append("var options = {bar: {groupWidth: '20%'}, legend: {position: 'top'}, tooltip: { trigger: 'none' }, title : '', vAxis: {title: 'Status', textStyle:{color: '#FFF'}},  hAxis: {title: 'Reading Date', slantedText:true, slantedTextAngle:90}, seriesType: 'bars', series: {4: {type: 'line'}} };");

        //                        strScript.Append("var chart = new google.visualization.ComboChart(document.getElementById('MainContent_chart_div')); google.visualization.events.addListener(chart, 'onmouseover', function(hover){if(hover){$('.google-visualization-tooltip-item:eq(1)').remove()}})\nchart.draw(data, options);}");
        //                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname", strScript.ToString(), true);
        //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
        //                    }
        //                    else
        //                    {
        //                        dvLtScripts.InnerText = "";
        //                        Literal myLiteral = new Literal();

        //                        myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found.</span>";
        //                        myLiteral.ID = "ltScripts";
        //                        chart_div.Visible = false;
        //                        dvLtScripts.Controls.Add(myLiteral);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    dvLtScripts.InnerText = "";
        //                    Literal myLiteral = new Literal();

        //                    myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found</span>";
        //                    myLiteral.ID = "ltScripts";
        //                    dvLtScripts.Controls.Add(myLiteral);
        //                    chart_div.Visible = false;
        //                    new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
        //                }
        //                finally
        //                {
        //                    strScript.Clear();
        //                }
        //            }
        //        }
        private void BindChart()
        {
            DataSet DS = new ComplaintsManagementBLL().GetTailStatusLastTenDays(Convert.ToInt64(hdnChannelID.Value));

            StringBuilder strScript = new StringBuilder();

            try
            {

                if (DS.Tables[0].Rows.Count > 0)
                {
                    dvLtScripts.InnerText = "";
                    chart_div.InnerText = "";

                    strScript.Append(@"
                    function drawVisualization() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['ReadingDate', 'Dry','ShortTail','AuthorizedTail','ExcessiveTail'],");

                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        string DailyDate = "";
                        if (row["ReadingDateTime"].ToString() != "")
                            //DailyDate = Utility.GetFormattedDateTimeJavaScript(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
                            DailyDate = Convert.ToString(row["ReadingDateTime"]);

                        strScript.Append("['" + DailyDate + "'," + Convert.ToInt32(row["Dry"]) + "," + Convert.ToInt32(row["ShortTail"]) + "," + Convert.ToInt32(row["AuthorizedTail"]) + "," + Convert.ToInt32(row["ExcessiveTail"]) + "],");
                        //strScript.Append("['" + DailyDate + "','" + Convert.ToInt32(row["Status"]) + "'],");
                    }

                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");

                    strScript.Append("var options = {bar: {groupWidth: '20%'}, width: '900', legend: {position: 'top'}, tooltip: { trigger: 'none' }, title : '', vAxis: {title: 'Tail Status', textStyle:{color: '#FFF'}},  hAxis: {title: 'Gauge Reading Date'}, seriesType: 'bars', series: {4: {type: 'line'}} };");

                    strScript.Append("var chart = new google.visualization.ComboChart(document.getElementById('MainContent_chart_div')); google.visualization.events.addListener(chart, 'onmouseover', function(hover){if(hover){$('.google-visualization-tooltip-item:eq(1)').remove()}})\nchart.draw(data, options);}");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname", strScript.ToString(), true);







                    //                    //                    strScript.Append(@"function drawVisualization() {         
                    //                    //                                                            var data = google.visualization.arrayToDataTable([  
                    //                    //                                                            ['ReadingDate', 'Status'],");

                    //                    //foreach (DataRow row in DS.Tables[0].Rows)
                    //                    //{
                    //                    //    string DailyDate = "";
                    //                    //    if (row["ReadingDateTime"].ToString() != "")
                    //                    //        DailyDate = Utility.GetFormattedDateTimeJavaScript(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
                    //                    //    strScript.Append("['" + DailyDate + "','" + row["Status"] + "'],");
                    //                    //}

                    //                    //strScript.Remove(strScript.Length - 1, 1);
                    //                    //strScript.Append("]);");
                    //                    //strScript.Append(@"var view = new google.visualization.DataView(data);");
                    //                    //view.hideRows([0, 1, 2, 3]);");

                    //                    //strScript.Append("var options = {bar: {groupWidth: '20%'}, legend: {position: 'none'}, title : '', vAxis: {title: 'Status'},  hAxis: {title: 'ReadingDate', slantedText: true}, seriesType: 'bars' };");
                    //                    //strScript.Append("var chart =new google.charts.Bar(document.getElementById('MainContent_chart_div'));chart.draw(view, google.charts.Bar.convertOptions(options));}");
                    //                    //strScript.Append("var chart =new google.visualization.ColumnChart(document.getElementById('MainContent_chart_div'));chart.draw(view, options);}");
                    //                    //                    strScript.Append(@"function drawVisualization() {     
                    //                    //                                            var data = new google.visualization.DataTable();
                    //                    //                                            data.addColumn('string', 'ReadingDate');
                    //                    //                                            data.addColumn('string', 'Status');
                    //                    //                                            data.addRows([");

                    //                    //                    foreach (DataRow row in DS.Tables[0].Rows)
                    //                    //                    {
                    //                    //                       string DailyDate = Utility.GetFormattedDate(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
                    //                    //                       strScript.Append("['" + DailyDate + "','" + row["Status"] + "'],");
                    //                    //                    }
                    //                    //                    strScript.Append("]);");

                    //                    //                    strScript.Append("var options = {bar: {groupWidth: '20%'}, legend: {position: 'none'}, title : '', vAxis: {title: 'Status'},  hAxis: {title: 'ReadingDate'}, seriesType: 'bars', series: {3: {type: 'area'}} };");
                    //                    //                    strScript.Append("var chart = new google.charts.Bar(document.getElementById('MainContent_chart_div'));");
                    //                    //                    strScript.Append("chart.draw(data, google.charts.Bar.convertOptions(options));}");



                    //                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname", strScript.ToString(), true);

                }
                else
                {
                    dvLtScripts.InnerText = "";
                    Literal myLiteral = new Literal();

                    myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found.</span>";
                    myLiteral.ID = "ltScripts";
                    chart_div.Visible = false;
                    dvLtScripts.Controls.Add(myLiteral);
                }
            }
            catch (Exception ex)
            {
                dvLtScripts.InnerText = "";
                Literal myLiteral = new Literal();

                myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found</span>";
                myLiteral.ID = "ltScripts";
                dvLtScripts.Controls.Add(myLiteral);
                chart_div.Visible = false;
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
            finally
            {
                strScript.Clear();
            }
        }

        /// <summary>
        /// this function sets the title of the page Add Irrigator Feedback
        /// Created On: 6/6/16 
        /// </summary>
        private void SetAddIrrigatorFeedbackPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddIrrigatorFeedback);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function sets the title of the page View Irrigator Feedback
        /// Created On: 27/6/16 
        /// </summary>
        private void SetViewIrrigatorFeedbackPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewIrrigatorFeedback);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function is binded at page load to set add irrigator feedback page title and enable disable some fields.
        /// </summary>
        private void LoadAddIrrigatorFeedback()
        {
            SetAddIrrigatorFeedbackPageTitle();
            hlBack.NavigateUrl = "~/Modules/IrrigatorsFeedback/SearchIrrigatorFeedback.aspx?ShowHistory=true";
            IrrigatorID = Convert.ToInt64(Request.QueryString["IrrigatorID"]);
            BindData(IrrigatorID);
            txtLocal.Enabled = false;
            txtTotalRDLeft.Enabled = false;
            txtTotalRDRight.Enabled = false;
            ddlVillage.Enabled = false;
            btnSave.Visible = base.CanAdd;
            ltlPageTitle.Text = "Add Irrigator Feedback";
        }

        /// <summary>
        /// this function is binded at page load to set View irrigator feedback page title and enable disable some fields and to bind dropdowns.
        /// </summary>
        private void LoadViewIrrigatorFeedback()
        {
            SetViewIrrigatorFeedbackPageTitle();
            IrrigatorFeedbackID = Convert.ToInt64(Request.QueryString["IrrigatorFeedbackID"]);
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            IF_IrrigatorFeedback mdlIrrigatorFeedback = bllIrrigatorFeedback.GetIrrigatorFeedbackByID(IrrigatorFeedbackID);
            BindData(mdlIrrigatorFeedback.IF_Irrigator.ID);
            hlBack.NavigateUrl = "~/Modules/IrrigatorsFeedback/IrrigatorFeedbackHistory.aspx?IrrigatorID=" + mdlIrrigatorFeedback.IF_Irrigator.ID + "&ShowHistory=true";

            ddlTailStatusByIrrigator.Enabled = false;
            ddlTailStatusByIrrigator.CssClass = "form-control";
            ddlRotationalViolation.Enabled = false;
            ddlRotationalViolation.CssClass = "form-control";
            ddlWaterTheft.Enabled = false;
            ddlWaterTheft.CssClass = "form-control";
            txtLocal.Enabled = false;
            txtTotalRDLeft.Enabled = false;
            txtTotalRDRight.Enabled = false;
            ddlVillage.Enabled = false;
            txtRemarks.Enabled = false;

            Dropdownlist.SetSelectedValue(ddlTailStatusByIrrigator, mdlIrrigatorFeedback.TailStatus);
            Dropdownlist.SetSelectedValue(ddlRotationalViolation, mdlIrrigatorFeedback.RotaionalViolation);
            Dropdownlist.SetSelectedValue(ddlWaterTheft, mdlIrrigatorFeedback.WaterTheft);
            Dropdownlist.SetSelectedValue(ddlVillage, Convert.ToString(mdlIrrigatorFeedback.VillageID));
            txtLocal.Text = mdlIrrigatorFeedback.LocalName;
            txtRemarks.Text = mdlIrrigatorFeedback.Remarks;
            Tuple<string, string> tuple = Calculations.GetRDValues(mdlIrrigatorFeedback.RD);
            txtTotalRDLeft.Text = tuple.Item1;
            txtTotalRDRight.Text = tuple.Item2;
            lblCallDate.Text = Convert.ToString(Utility.GetFormattedDate(mdlIrrigatorFeedback.FeedbackDate));

            btnSave.Visible = false;
            ltlPageTitle.Text = "View Irrigator Feedback";
        }

        protected void viewlasttendays_Click(object sender, EventArgs e)
        {
            try
            {
                //ddlpopTailStatus.ClearSelection();
                BindChart();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ViewRotationalProgram_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Now = DateTime.Now;

                if (!string.IsNullOrEmpty(Request.QueryString["IrrigatorFeedbackID"]))
                {
                    Now = Convert.ToDateTime(lblCallDate.Text);
                }

                string RPYear;
                long Season;
                if (new DateTime(Now.Year, 4, 1) <= Now && new DateTime(Now.Year, 9, 30) >= Now)
                {
                    Season = (long)Constants.Seasons.Kharif;
                    RPYear = Now.Year.ToString();
                }
                else
                {
                    Season = (long)Constants.Seasons.Rabi;
                    if (Now.Month >= 10 && Now.Month <= 12)
                    {
                        RPYear = Now.Year + "-" + Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        long Year = Now.Year - 1;
                        RPYear = Year + "-" + Now.ToString("yy");
                    }

                }

                IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
                long RPID = bllIrrigatorFeedback.GetRotationalProgramID(Convert.ToInt64(hdnDivisionID.Value), Convert.ToInt64(hdnChannelID.Value), Season, RPYear);

                if (RPID == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname2", "alert('No Rotational Program Exists');", true);
                }
                else
                {
                    string link = "../../Modules/RotationalProgram/ViewRotationalProgramDivisionSubDivision.aspx?RPID=" + RPID + "&FromIrrigator=" + true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + link + "','_blank');</script>", false);
                    //Response.Redirect("~/Modules/RotationalProgram/ViewRotationalProgramDivisionSubDivision.aspx?RPID=" + RPID + "&FromIrrigator=" + true, false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}