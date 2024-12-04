using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class ViewComparativeStatement : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    btnSave.Visible = base.CanAdd;
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hdnWorkID.Value = Request.QueryString["TenderWorkID"];
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnWorkID.Value));
                        hlBackToWork.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        //long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnWorkID.Value));
                        hdnTenderNoticeID.Value = Convert.ToString(TenderNoticeID);

                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        BindComparativeStatementGridData(Convert.ToInt64(hdnWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchCommittee.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchContractor.HRef = string.Format("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));


                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN || mdlUser.DesignationID == (long)Constants.Designation.CE || mdlUser.DesignationID == (long)Constants.Designation.SE)
                        {
                            anchReport.Disabled = true;
                            anchReport.Attributes.Add("style", "color:black");
                        }
                        else
                        {
                            anchReport.Disabled = false;
                            anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        }

                        //if (base.CanView)
                        //anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchstatement.HRef = string.Format("~/Modules/Tenders/TenderNotice/ViewComparativeStatement.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchTenderPrice.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                    }

                    bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnWorkID.Value));

                    if (IsAwarded == true)
                    {
                        btnSave.Visible = false;
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindComparativeStatementGridData(long _WorkID, long _WorkSourceID)
        {
            try
            {
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _WorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");

                DataTable lstComparativeStatementData = new TenderManagementBLL().GetComparativeStatementDataByWorkandNoticeID(_WorkID);
                // List<object> lstComparativeStatementData = new TenderManagementBLL().GetWorkItemsforViewByWorKID(_WorkID);

                lstComparativeStatementData.Columns["Sanctioned Qty"].SetOrdinal(1);
                lstComparativeStatementData.Columns["Sanctioned Rate"].SetOrdinal(2);
                lstComparativeStatementData.Columns["Sanctioned Total"].SetOrdinal(3);
                DataRow drTotal = lstComparativeStatementData.NewRow();
                int CompanyCount = lstComparativeStatementData.Columns.Count - 4;
                int NumberOfCompanies = CompanyCount / 2;

                //For Sanctioned Sum

                string SName = lstComparativeStatementData.Columns[3].ColumnName;
                var Sresult = lstComparativeStatementData.AsEnumerable()
                 .Select(x => x[SName].ToString().Replace(",", "")).ToList().Select(x => Convert.ToDouble(x)).Sum();


                drTotal[SName] = string.Format("{0:#,##0.00}", double.Parse(Sresult.ToString()));


                //For Contractors Sum
                int Iterator = 5;
                for (int i = 0; i < NumberOfCompanies; i++)
                {
                    string ValName = lstComparativeStatementData.Columns[Iterator].ColumnName;

                    //object sumObject;
                    //sumObject = lstComparativeStatementData.Compute("Sum(["+Name+"])", "");
                    //string val = sumObject.ToString();
                    var result = lstComparativeStatementData.AsEnumerable()
                     .Select(x => x[ValName].ToString().Replace(",", "")).ToList().Select(x => Convert.ToDouble(x)).Sum();


                    drTotal[ValName] = string.Format("{0:#,##0.00}", double.Parse(result.ToString()));




                    Iterator = Iterator + 2;
                }
                lstComparativeStatementData.Rows.Add(drTotal);
                gvComparativeStatement.DataSource = lstComparativeStatementData;
                gvComparativeStatement.DataBind();


                List<object> lstofContractorandAmount = new TenderManagementBLL().GetContractorandAmountByWorkID(_WorkID);
                gvaward.DataSource = lstofContractorandAmount;
                gvaward.DataBind();

                //foreach (GridViewRow gvrow in gvComparativeStatement.Rows)
                //{
                //    if (gvrow.RowIndex == gvComparativeStatement.Rows.Count - 1)
                //    {
                //        gvrow.Font.Bold = true;
                //    }

                //}

                gvComparativeStatement.Rows[gvComparativeStatement.Rows.Count - 1].Font.Bold = true;



                object WorkItemRate = new TenderManagementBLL().GetWorkItemRateBySourceID(_WorkSourceID);
                string Amount = Utility.GetDynamicPropertyValue(WorkItemRate, "Rate");
                lblSanctionAmount.Text = string.Format("{0:#,##0.00}", double.Parse(Amount));
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComparativeStatement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                foreach (GridViewRow gvrow in gvComparativeStatement.Rows)
                {
                    gvrow.HorizontalAlign = HorizontalAlign.Right;

                }
                //foreach (GridViewRow gvrow in gvComparativeStatement.Rows)
                //{
                //    Label t = gvrow.Cells[1].FindControl("ItemDescription") as Label;


                //}

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TableCell statusCell = e.Row.Cells[1];
                    string value = statusCell.Text;
                    long valeu = Convert.ToInt64(Convert.ToDouble(value));
                    statusCell.Text = Convert.ToString(valeu);
                }



            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                TM_TenderWorksContractors mdlTenderWorkContractor = new TM_TenderWorksContractors();
                TM_TenderWorks mdlTenderWork = new TM_TenderWorks();
                TenderManagementBLL TenderMgmtBLL = new TenderManagementBLL();
                bool IsPreparedReport = TenderMgmtBLL.IsADMPreparedReport(Convert.ToInt64(hdnWorkID.Value));
                if (!IsPreparedReport)
                {
                    Master.ShowMessage("Please first add observations in ADM report", SiteMaster.MessageType.Error);
                    return;
                }

                foreach (GridViewRow gvrow in gvaward.Rows)
                {
                    Label ID = (Label)gvrow.FindControl("ID");
                    RadioButton rdButton = (RadioButton)gvrow.FindControl("rdButton");
                    TextBox Remarks = (TextBox)gvrow.FindControl("txtRemarks");
                    if (gvrow.RowIndex == 0)
                    {
                        mdlTenderWorkContractor.Awarded = false;
                        mdlTenderWorkContractor.ID = Convert.ToInt64(ID.Text);
                        if (Remarks.Text != "")
                            mdlTenderWorkContractor.AwardedRemarks = Remarks.Text;
                        else
                            mdlTenderWorkContractor.AwardedRemarks = "Lowest Amount Bidder.";

                        long IsUpdated = TenderMgmtBLL.UpdateTenderWorkContractorAwardFieldByID(mdlTenderWorkContractor);

                    }

                    if (rdButton.Checked)
                    {
                        mdlTenderWorkContractor.Awarded = true;
                        mdlTenderWorkContractor.ID = Convert.ToInt64(ID.Text);
                        mdlTenderWorkContractor.AwardedRemarks = Remarks.Text;
                        long IsUpdated = TenderMgmtBLL.UpdateTenderWorkContractorAwardFieldByID(mdlTenderWorkContractor);
                    }

                }

                if (mdlTenderWorkContractor.Awarded != true)
                {
                    Master.ShowMessage("Please select awarded company", SiteMaster.MessageType.Success);
                    return;
                }


                mdlTenderWork.ID = Convert.ToInt64(hdnWorkID.Value);
                long TenderWorkID = TenderMgmtBLL.UpdateTenderWorkStatusIDByWorkID(mdlTenderWork);
                TM_TenderWorks _mdltenderWork = TenderMgmtBLL.GetTenderWorkEntity(TenderWorkID);
                if (_mdltenderWork.WorkSource.ToUpper() == "ASSET")
                {
                    bool IsUpdated = TenderMgmtBLL.UpdateAssetWorkStatus(_mdltenderWork.WorkSourceID);
                }

                PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                _event.Parameters.Add("TenderNoticeID", hdnTenderNoticeID.Value);
                _event.Parameters.Add("TenderWorkID", Convert.ToInt64(hdnWorkID.Value));
                _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.ContractorisAwarded, (int)SessionManagerFacade.UserInformation.ID);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                Response.Redirect(("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + (Convert.ToInt64(hdnTenderNoticeID.Value))), false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvaward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnWorkID.Value));
                // List<object> lstofContractorandAmount = new TenderManagementBLL().GetContractorandAmountByWorkID(Convert.ToInt64(hdnWorkID.Value));

                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    RadioButton aRadioButton = (RadioButton)e.Row.FindControl("rdButton");
                //    TextBox textBox1 = (TextBox)e.Row.FindControl("txtRemarks");
                //    aRadioButton.Attributes.Add("onClick", "JavaScript:ShowText('" + aRadioButton.ClientID + "','" + textBox1.ClientID + "')");
                //}

                foreach (GridViewRow gvrow in gvaward.Rows)
                {
                    RadioButton rdButton1 = (RadioButton)gvrow.FindControl("rdButton");
                    RadioButton RadioButton = (RadioButton)gvrow.Cells[0].FindControl("rdButton");
                    TextBox txtRemarks = (TextBox)gvrow.Cells[0].FindControl("txtRemarks");


                    if (gvaward.Rows.Count >= 1)
                    {
                        if (e.Row.RowIndex == 1)
                        {
                            if (IsAwarded != true)
                            {
                                RadioButton.Checked = true;
                                txtRemarks.Text = "Lowest Amount Bidder.";
                            }
                        }
                    }
                    else
                    {
                        if (e.Row.RowIndex == -1)
                        {
                            if (IsAwarded != true)
                            {
                                RadioButton.Checked = true;
                                txtRemarks.Text = "Lowest Amount Bidder.";
                            }
                        }
                    }

                    if (rdButton1.Checked)
                    {
                        rdButton1.Checked = true;
                    }


                    if (IsAwarded == true)
                    {
                        rdButton1.Enabled = false;
                    }
                    else
                    {
                        rdButton1.Enabled = true;

                    }


                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvComparativeStatement_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                // HeaderRow.BackColor = Color.FromName("gray");
                HeaderRow.CssClass = "table header";
                int ContractorRateColumn = e.Row.Cells.Count - 4;

                TableCell HeaderCell2 = new TableCell();
                HeaderCell2.Text = "RATE AS PER T.S. ESTIMATE";
                HeaderCell2.ColumnSpan = 4;
                HeaderCell2.RowSpan = 2;
                HeaderCell2.CssClass = "header";
                HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                HeaderCell2.BorderColor = System.Drawing.Color.FromName("#d2c3c3");
                HeaderCell2.BackColor = System.Drawing.Color.FromName("#E6E6E6");
                HeaderCell2.BorderWidth = 1;
                HeaderCell2.Font.Bold = true;
                HeaderRow.Cells.Add(HeaderCell2);

                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "RATE QUOTED BY CONTRACTOR";
                HeaderCell2.ColumnSpan = ContractorRateColumn;
                HeaderCell2.Attributes.CssStyle["text-align"] = "center";
                HeaderCell2.BorderWidth = 1;
                HeaderCell2.Font.Bold = true;
                HeaderCell2.BorderColor = System.Drawing.Color.FromName("#d2c3c3");
                HeaderCell2.BackColor = System.Drawing.Color.FromName("#E6E6E6");
                HeaderRow.Cells.Add(HeaderCell2);

                //HeaderCell2 = new TableCell();
                //HeaderCell2.Text = "Office details";
                //HeaderCell2.ColumnSpan = 2;
                //HeaderRow.Cells.Add(HeaderCell2);

                gvComparativeStatement.Controls[0].Controls.AddAt(0, HeaderRow);

                GridViewRow HeaderRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                HeaderRow1.CssClass = "table header";
                //HeaderRow1.BackColor = Color.FromName("red");



                for (int i = 1; i < e.Row.Cells.Count; i += 2)
                {
                    //if (i == 3)
                    //{
                    //    TableCell HeaderCell = new TableCell();
                    //    HeaderRow1.Cells.Add(HeaderCell);
                    //    TableCell HeaderCell1 = new TableCell();
                    //    HeaderRow1.Cells.Add(HeaderCell1);
                    //    TableCell HeaderCell3 = new TableCell();
                    //    HeaderRow1.Cells.Add(HeaderCell3);
                    //    TableCell HeaderCell4 = new TableCell();
                    //    HeaderRow1.Cells.Add(HeaderCell4);
                    //}

                    if (i > 3)
                    {
                        TableCell HeaderCell = new TableCell();
                        // TableCell Cell;
                        string cell = e.Row.Cells[i].Text.Trim();
                        int index = cell.IndexOf(" Total");
                        if (index != -1)
                        {
                            cell = cell.Remove(index);
                        }

                        //  TableCell HeaderCell = new TableCell();
                        HeaderCell.Text = cell;
                        HeaderCell.ColumnSpan = 2;
                        HeaderCell.BorderWidth = 1;
                        HeaderCell.BorderColor = System.Drawing.Color.FromName("#d2c3c3");
                        HeaderCell.BackColor = System.Drawing.Color.FromName("#E6E6E6");
                        HeaderCell.Font.Bold = true;
                        //  gvComparativeStatement.Rows[0].Cells[2].Text = cell;
                        // gvComparativeStatement.Columns[i].HeaderText = cell;
                        HeaderCell.Attributes.CssStyle["text-align"] = "center";
                        HeaderRow1.Cells.Add(HeaderCell);

                        //TableCell HeaderCell5 = new TableCell();
                        //HeaderRow1.Cells.Add(HeaderCell5);
                    }
                }


                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (i > 3)
                    {
                        if (i % 2 != 0)
                            e.Row.Cells[i].Text = "Amount";
                        else
                            e.Row.Cells[i].Text = "Rate";

                        e.Row.Cells[i].BorderWidth = 1;
                        e.Row.Cells[i].BorderColor = System.Drawing.Color.FromName("#d2c3c3");
                        //  e.Row.Cells[i].Attributes.CssStyle["text-align"] = "center";
                    }
                    else
                    {
                        e.Row.Cells[i].BorderWidth = 1;
                        e.Row.Cells[i].BorderColor = System.Drawing.Color.FromName("#d2c3c3");
                    }
                    // e.Row.Cells[0].Width = new Unit("400px");


                }

                HeaderRow.Attributes.Add("class", "header");
                HeaderRow1.Attributes.Add("class", "header");
                gvComparativeStatement.Controls[0].Controls.AddAt(1, HeaderRow1);
            }
        }


    }
}