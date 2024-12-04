using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using PMIU.WRMIS.BLL.Notifications;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class PlacingIndents : BasePage
    {
        double TotalOfftakeIndent = 0;
        long? IndentID = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    btnSearchChannel.Visible = base.CanView;
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(1)));
                    BindSubDivisionDropdown();
                    btnSave.Visible = false;
                    ddlChannel.Items.Insert(0, new ListItem("Select", ""));
                    ddlChannel.Enabled = false;
                    divHeader.Visible = false;
                    hlBack.NavigateUrl = "~/Modules/DailyData/DailyIndents.aspx";

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.PlacingIndents] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindSubDivisionDropdown()
        {
            //IndentsBLL bllIndents = new IndentsBLL();
            //UA_Users mdlUser = SessionManagerFacade.UserInformation;
            //List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


            //ddlSubDivision.DataSource = lstSubDivision;
            //ddlSubDivision.DataTextField = "NAME";
            //ddlSubDivision.DataValueField = "ID";
            //ddlSubDivision.DataBind();
            //ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
            }
            else if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionsOFXENByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
            }
            ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
        }

        private void BindChannelDropdown()
        {
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            //List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);

            if (ddlSubDivision.SelectedItem.Value == String.Empty)
            {
                ddlChannel.SelectedIndex = 0;
                ddlChannel.Enabled = false;
                gvIndents.Visible = false;
                divHeader.Visible = false;
                btnSave.Visible = false;
            }
            else
            {
                List<dynamic> lstChannelsBySubDivIDAndUserID = bllIndents.GetChannelsByUserIDAndSubDivID(mdlUser.ID, Convert.ToInt64(mdlUser.UA_Designations.IrrigationLevelID), Convert.ToInt64(ddlSubDivision.SelectedItem.Value), "P");


                ddlChannel.DataSource = lstChannelsBySubDivIDAndUserID;
                ddlChannel.DataTextField = "ChannelName";
                ddlChannel.DataValueField = "ChannelID";
                ddlChannel.DataBind();
                ddlChannel.Items.Insert(0, new ListItem("Select", ""));
                ddlChannel.Enabled = true;
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindChannelDropdown();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            List<dynamic> lstChannelIndentOfftakes = new List<dynamic>();
            long ChannelID = 0;
            short? SortOrder = 0;

            //long GaugeID = bllIndents.GetGaugeIDByChannelIDAndSubDivID(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue));
            //DateTime? Date = bllIndents.CalculateIndentDate(Convert.ToInt32(ddlChannel.SelectedValue), Convert.ToInt32(GaugeID), Convert.ToDateTime(txtDate.Text));
            //if (Date.Value.Date != Convert.ToDateTime(txtDate.Text).Date)
            //{
            //    Master.ShowMessage(Message.SuggestedDate.Description, SiteMaster.MessageType.Warning);
            //}


            //txtDate.Text = Convert.ToString(Utility.GetFormattedDate(Date));

            IndentID = bllIndents.GetIndentIDBySubDivIDAndParentChannelIDAndDate(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue), Convert.ToDateTime(txtDate.Text));
            if (IndentID != null)
            {
                lstChannelIndentOfftakes = bllIndents.GetChannelIndentsOffTakesFromOffTakesTable(IndentID.Value, Convert.ToDateTime(txtDate.Text));
                btnSave.Visible = base.CanEdit;
            }

            else
            {
                List<DD_GetChannelsByUserIDForIndents_Result> lstChannelsBySubDivIDAndUserID = bllIndents.GetChannelsByUserIDAndSubDivIDTest(mdlUser.ID, Convert.ToInt64(mdlUser.UA_Designations.IrrigationLevelID), Convert.ToInt64(ddlSubDivision.SelectedItem.Value), "C");
                List<DD_GetChannelsByUserIDForIndents_Result> lstChannelsBySubDivIDAndUserID2 = null;//lstChannelsBySubDivIDAndUserID;
                //long ChannelID = 2602;//Convert.ToInt64(ddlChannel.SelectedItem.Value);
                ChannelID = Convert.ToInt64(ddlChannel.SelectedValue);
                SortOrder = lstChannelsBySubDivIDAndUserID.Where(X => X.ChannelID == ChannelID).Select(X => X.SortOrder).FirstOrDefault();
                lstChannelsBySubDivIDAndUserID2 = lstChannelsBySubDivIDAndUserID.Where(x => x.SortOrder == SortOrder && x.ParentChild == "C").ToList();
                if (lstChannelsBySubDivIDAndUserID2.Count == 0)
                {
                    lstChannelsBySubDivIDAndUserID2 = lstChannelsBySubDivIDAndUserID.Where(x => x.SortOrder == SortOrder && x.ParentChild == "P").ToList();
                }


                for (int i = 0; i < lstChannelsBySubDivIDAndUserID2.Count; i++)
                {
                    lstChannelIndentOfftakes.Add(bllIndents.GetChannelIndentsOffTakesFromChannelTable(Convert.ToInt64(lstChannelsBySubDivIDAndUserID2.ElementAt(i).ChannelID), Convert.ToDateTime(txtDate.Text)));
                }
                btnSave.Visible = base.CanAdd;

            }
            gvIndents.DataSource = lstChannelIndentOfftakes;
            gvIndents.DataBind();
        }

        protected void gvIndents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //double number;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblOfftakeIndentDate = (Label)e.Row.FindControl("lblOfftakeIndentDate");
                    //Label lblIndentTime = (Label)e.Row.FindControl("lblIndentTime");
                    Label lblParrentRD = (Label)e.Row.FindControl("lblParrentRD");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndentValue");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");
                    //lblOfftakeIndentDate.Text = Convert.ToString(DateTime.Now);
                    if (lblParrentRD.Text != String.Empty)
                    {
                        double GaugeAtRD = Convert.ToDouble(lblParrentRD.Text);
                        lblParrentRD.Text = Calculations.GetRDText(GaugeAtRD);
                    }
                    if (lblOfftakeIndentDate.Text != "")
                    {
                        lblOfftakeIndentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblOfftakeIndentDate.Text));
                    }
                    if (lblIndent.Text != null)
                    {
                        double IndentVal = Convert.ToDouble(lblIndent.Text);
                        txtIndent.Text = String.Format("{0:0.00}", (IndentVal));
                    }
                    //if (Double.TryParse(lblIndentTime.Text, out number))
                    //{
                    //    double hours = Convert.ToDouble(lblIndentTime.Text);
                    //    TimeSpan interval = TimeSpan.FromHours(hours);
                    //    DateTime Time = new DateTime(interval.Ticks);
                    //    lblIndentTime.Text = Utility.GetFormattedTime(Convert.ToDateTime(Time));
                    //}
                    //else
                    //{
                    //    lblIndentTime.Text = Utility.GetFormattedTime(Convert.ToDateTime(lblIndentTime.Text));
                    //}

                    TotalOfftakeIndent = TotalOfftakeIndent + Convert.ToDouble(lblIndent.Text);
                    //if (lblIndentTime.Text != "" && lblIndentTime.Text != "0")
                    //{
                    //    lblIndentTime.Text = Utility.GetFormattedTime(Convert.ToDateTime(lblIndentTime.Text));
                    //}
                    //else
                    //{
                    //    double hours = Convert.ToDouble(lblIndentTime.Text);
                    //    TimeSpan interval = TimeSpan.FromHours(hours);
                    //    Convert.ToDateTime(interval.Milliseconds);
                    //}
                }



                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblOfftakesTotalIndents = (Label)e.Row.FindControl("lblOfftakesTotalIndents");
                    Label lblDirectOutletsTotalIndent = (Label)e.Row.FindControl("lblDirectOutletsTotalIndent");
                    Label lblIncrementedIndentAt10P = (Label)e.Row.FindControl("lblIncrementedIndentAt10P");
                    Label lblIndentAtSubDivisionalGauge = (Label)e.Row.FindControl("lblIndentAtSubDivisionalGauge");
                    Label lblPercent = (Label)e.Row.FindControl("lblPercent");

                    //String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().Discharge),

                    IndentsBLL bllIndents = new IndentsBLL();
                    double? OutletIndent = bllIndents.GetOutletIndent(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue));
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

        protected void btnSearchChannel_Click(object sender, EventArgs e)
        {
            try
            {
                //List<dynamic> SearchCriteria = new List<dynamic>();

                BindChannelData();
                BindGrid();
                //btnSave.Visible = true;
                divHeader.Visible = true;
                gvIndents.Visible = true;

                //SearchCriteria.Add(ddlSubDivision.SelectedValue);
                //SearchCriteria.Add(ddlChannel.SelectedValue);
                //SearchCriteria.Add(txtDate.Text);
                //SearchCriteria.Add(lblIMISCode.Text);
                //SearchCriteria.Add(lblCurrentSubDivisionIndent.Text);
                //SearchCriteria.Add(lblCurrentSubDivisionIndentDate.Text);
                //SearchCriteria.Add(lblTotalIndnetAtCurrentSubDivisionalHead.Text);
                //SearchCriteria.Add(lblLowerSubDivisionIndent.Text);
                //SearchCriteria.Add(lblLowerSubDivisionIndentDate.Text);
                //SearchCriteria.Add(btnSave.Visible);
                //Session[SessionValues.PlacingIndents] = SearchCriteria;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvIndents.EditIndex = e.NewEditIndex;
                BindGrid();
                gvIndents.Rows[e.NewEditIndex].FindControl("txtIndent").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIndents.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindChannelData()
        {
            lblCurrentSubDivisionIndent.Text = "";
            lblCurrentSubDivisionIndentDate.Text = "";
            lblLowerSubDivisionIndent.Text = "";
            lblLowerSubDivisionIndentDate.Text = "";
            lblIMISCode.Text = "";
            lblTotalIndnetAtCurrentSubDivisionalHead.Text = "";

            long SubDivID = Convert.ToInt64(ddlSubDivision.SelectedValue);
            long ChannelID = Convert.ToInt64(ddlChannel.SelectedValue);

            ChannelBLL bllChannel = new ChannelBLL();
            CO_Channel mdlChannel = bllChannel.GetChannelByID(ChannelID);

            IndentsBLL bllIndents = new IndentsBLL();
            //CO_ChannelIndent mdlChannelIndentCurrent = bllIndents.GetIndentBySubDivIDandChannelID(ChannelID, SubDivID);
            CO_ChannelIndent mdlChannelIndentCurrent = bllIndents.GetCurrentIndentBySubDivIDAndChannelID(SubDivID, ChannelID, Convert.ToDateTime(txtDate.Text));
            long LowerSubDiv = bllIndents.GetLowerSubDivision(ChannelID, SubDivID);
            //CO_ChannelIndent mdlChannelIndentLower = bllIndents.GetIndentBySubDivIDandChannelID(ChannelID, LowerSubDiv);
            CO_ChannelIndent mdlChannelIndentLower = bllIndents.GetCurrentIndentBySubDivIDAndChannelID(LowerSubDiv, ChannelID, Convert.ToDateTime(txtDate.Text));

            if (mdlChannel != null)
            {
                lblIMISCode.Text = mdlChannel.IMISCode;
            }

            if (mdlChannelIndentCurrent != null)
            {
                //String.Format("{0:0.00}", (TotalOfftakeIndent));
                lblCurrentSubDivisionIndent.Text = String.Format("{0:0.00}", (mdlChannelIndentCurrent.OutletIndent + mdlChannelIndentCurrent.TotalOfftakeIndent + mdlChannelIndentCurrent.PercentageIncrementValue));
                lblCurrentSubDivisionIndentDate.Text = Utility.GetFormattedDate(mdlChannelIndentCurrent.IndentDate);
            }

            if (mdlChannelIndentLower != null || LowerSubDiv != -1)
            {
                List<long> lstSubDivision = bllIndents.GetListOfSubDivisions(ChannelID);
                long SubDivisonFromList;
                double? CurrentIndentInLoop = 0.0;
                int ListIndex = lstSubDivision.FindIndex(a => a == SubDivID);
                for (int i = ListIndex + 1; i <= lstSubDivision.Count - 1; i++)
                {
                    SubDivisonFromList = lstSubDivision.ElementAt(i);
                    CO_ChannelIndent mdlChannelIndentCurrentInLoop = bllIndents.GetIndentBySubDivIDandChannelID(ChannelID, SubDivisonFromList);
                    if (mdlChannelIndentCurrentInLoop != null)
                    {
                        CurrentIndentInLoop = CurrentIndentInLoop + (mdlChannelIndentCurrentInLoop.OutletIndent + mdlChannelIndentCurrentInLoop.TotalOfftakeIndent + mdlChannelIndentCurrentInLoop.PercentageIncrementValue);
                    }
                }

                double? SumOfIndents = CurrentIndentInLoop;
                if (mdlChannelIndentLower != null)
                {
                    lblLowerSubDivisionIndentDate.Text = Utility.GetFormattedDate(mdlChannelIndentLower.IndentDate);
                    lblLowerSubDivisionIndent.Text = String.Format("{0:0.00}", (SumOfIndents));
                }
                if (mdlChannelIndentCurrent != null)
                {
                    lblTotalIndnetAtCurrentSubDivisionalHead.Text = String.Format("{0:0.00}", (SumOfIndents + mdlChannelIndentCurrent.OutletIndent + mdlChannelIndentCurrent.TotalOfftakeIndent + mdlChannelIndentCurrent.PercentageIncrementValue));
                }
            }
            else
            {
                lblTotalIndnetAtCurrentSubDivisionalHead.Text = lblCurrentSubDivisionIndent.Text;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    IndentsBLL bllIndents = new IndentsBLL();
                    long GaugeID = bllIndents.GetGaugeIDByChannelIDAndSubDivID(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue));
                    if (GaugeID==0)
                    {
                         Master.ShowMessage(Message.SubDivisinalGaugeNotFound.Description, SiteMaster.MessageType.Error);
                         return;
                    }
                    DateTime? Date = bllIndents.CalculateIndentDate(Convert.ToInt32(ddlChannel.SelectedValue), Convert.ToInt32(GaugeID), Convert.ToDateTime(txtDate.Text));
                    double TotalOfftakeIndent = 0;
                    double? Sum = 0;
                    double? OutletIndent = bllIndents.GetOutletIndent(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue));
                    double? PercentageIncrementValue = 0;
                    int SystemParametervalue = 0;
                    bool FlagForNotification = false;

                    CO_ChannelGauge mdlChannelGauge = new CO_ChannelGauge();
                    CO_ChannelIndent mdlChannelIndent = new CO_ChannelIndent();
                    CO_ChannelIndent mdlChannelIndentforAdd = new CO_ChannelIndent();

                    mdlChannelGauge = bllIndents.GetSubDivGaugeID(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue), Convert.ToInt64(Constants.GaugeCategory.SubDivisionalGauge));
                    mdlChannelIndent = bllIndents.GetIndentBySubDivisionIDAndChannelIDAndDate(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue), Convert.ToDateTime(txtDate.Text)).FirstOrDefault();


                    mdlChannelIndentforAdd.SubDivID = Convert.ToInt64(ddlSubDivision.SelectedValue);
                    mdlChannelIndentforAdd.ParentChannelID = Convert.ToInt64(ddlChannel.SelectedValue);
                    mdlChannelIndentforAdd.SubDivGaugeID = mdlChannelGauge.ID;
                    mdlChannelIndentforAdd.EntryDate = DateTime.Today;
                    mdlChannelIndentforAdd.IndentDate = Date;//Convert.ToDateTime(txtDate.Text);
                    //mdlChannelIndentforAdd.OutletIndent
                    //mdlChannelIndentforAdd.TotalOfftakeIndent = TotalOfftakeIndent;
                    //mdlChannelIndentforAdd.PercentageIncrementValue
                    //mdlChannelIndentforAdd.PercentageIncrement

                    if (mdlChannelIndent == null)
                    {
                        mdlChannelIndentforAdd.CreatedBy = mdlUser.ID;
                        mdlChannelIndentforAdd.CreatedDate = DateTime.Now;
                        bllIndents.AddIndnet(mdlChannelIndentforAdd);
                    }

                    mdlChannelIndent = bllIndents.GetIndentBySubDivisionIDAndChannelIDAndDate(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue), Convert.ToDateTime(txtDate.Text)).FirstOrDefault();

                    foreach (TableRow Row in gvIndents.Rows)
                    {
                        Label lblID = (Label)Row.FindControl("lblID");
                        Label lblIndentID = (Label)Row.FindControl("lblIndentID");
                        Label lblDirectOfftakes = (Label)Row.FindControl("lblDirectOfftakesID");
                        Label lblParrentRD = (Label)Row.FindControl("lblParrentRD");
                        Label lblOfftakeIndentDate = (Label)Row.FindControl("lblOfftakeIndentDate");
                        //Label lblIndentTime = (Label)Row.FindControl("lblIndentTime");
                        TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                        TextBox txtRemarks = (TextBox)Row.FindControl("txtRemarks");
                        //Label lblOfftakesTotalIndents = (Label)Row.FindControl("lblOfftakesTotalIndents");

                        string Indent = txtIndent.Text.Trim();
                        string Remarks = txtRemarks.Text.Trim();

                        string[] TotalRD = lblParrentRD.Text.Split('+');
                        long LeftRD = Convert.ToInt64(TotalRD[0]) * 1000;
                        long RightRD = Convert.ToInt64(TotalRD[1]);
                        long TotalRDs = LeftRD + RightRD;

                        //string[] TimeSpliter = lblIndentTime.Text.Split(' ');
                        //DateTime Time = Convert.ToDateTime(TimeSpliter[0]);
                        //double Time = Convert.ToDouble(lblIndentTime.Text);
                        //TimeSpan Time = Convert.ToDateTime(lblIndentTime.Text).TimeOfDay;


                        CO_ChannelIndentOfftakes mdlChannelIndentOfftakes = new CO_ChannelIndentOfftakes();


                        mdlChannelIndentOfftakes.ID = Convert.ToInt64(lblID.Text.Trim());
                        mdlChannelIndentOfftakes.IndentID = Convert.ToInt64(mdlChannelIndent.ID);
                        mdlChannelIndentOfftakes.ChannelID = Convert.ToInt64(lblDirectOfftakes.Text.Trim());
                        mdlChannelIndentOfftakes.ChannelRD = Convert.ToInt32(TotalRDs);
                        mdlChannelIndentOfftakes.ChannelIndent = Convert.ToDouble(Indent);
                        mdlChannelIndentOfftakes.OfftakeIndentDate = Convert.ToDateTime(lblOfftakeIndentDate.Text.Trim()).Add(Date.Value.TimeOfDay);//.Add(Time);
                        mdlChannelIndentOfftakes.Remarks = txtRemarks.Text;

                        bool flag = bllIndents.GetOfftakeIndentBySubDivisionIDAndChannelIDAndDate(mdlChannelIndent.ID, Convert.ToInt64(lblDirectOfftakes.Text), Convert.ToDateTime(txtDate.Text));

                        if (flag)
                        {
                            mdlChannelIndentOfftakes.CreatedDate = DateTime.Now;
                            mdlChannelIndentOfftakes.CreatedBy = mdlUser.ID;
                            bllIndents.AddChannelIndnetOfftakes(mdlChannelIndentOfftakes);
                        }
                        else
                        {
                            mdlChannelIndentOfftakes.ModifiedDate = DateTime.Now;
                            mdlChannelIndentOfftakes.ModifiedBy = mdlUser.ID;
                            bllIndents.UpdateChannelIndnetOfftakes(mdlChannelIndentOfftakes);
                        }

                        TotalOfftakeIndent = TotalOfftakeIndent + Convert.ToDouble(Indent);
                    }

                    mdlChannelIndent = null;
                    mdlChannelIndentforAdd = new CO_ChannelIndent();

                    UA_SystemParameters mdlPercentageValue = new UserBLL().GetSystemParameterValue((short)Constants.SystemParameter.IndentPercentage);

                    SystemParametervalue = Convert.ToInt16(mdlPercentageValue.ParameterValue);
                    Sum = OutletIndent + TotalOfftakeIndent;
                    PercentageIncrementValue = (Sum * SystemParametervalue) / 100;

                    mdlChannelIndent = bllIndents.GetIndentBySubDivisionIDAndChannelIDAndDate(Convert.ToInt64(ddlSubDivision.SelectedValue), Convert.ToInt64(ddlChannel.SelectedValue), Convert.ToDateTime(txtDate.Text)).FirstOrDefault();

                    mdlChannelIndentforAdd.ID = mdlChannelIndent.ID;
                    mdlChannelIndentforAdd.SubDivID = Convert.ToInt64(ddlSubDivision.SelectedValue);
                    mdlChannelIndentforAdd.ParentChannelID = Convert.ToInt64(ddlChannel.SelectedValue);
                    mdlChannelIndentforAdd.SubDivGaugeID = mdlChannelGauge.ID;
                    mdlChannelIndentforAdd.EntryDate = DateTime.Now;
                    mdlChannelIndentforAdd.IndentDate = Date; //Convert.ToDateTime(txtDate.Text);
                    mdlChannelIndentforAdd.OutletIndent = OutletIndent;
                    mdlChannelIndentforAdd.TotalOfftakeIndent = TotalOfftakeIndent;
                    mdlChannelIndentforAdd.PercentageIncrementValue = PercentageIncrementValue;
                    mdlChannelIndentforAdd.PercentageIncrement = Convert.ToInt16(SystemParametervalue);

                    if (mdlChannelIndent != null)
                    {
                        //if (mdlChannelIndent.ModifiedBy != null)
                        //{
                        //    NotifyEvent _event = new NotifyEvent();
                        //    _event.Parameters.Add("GaugeID", mdlChannelGauge.ID);
                        //    _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.ChangeIndent, SessionManagerFacade.UserInformation.ID);
                        //}
                        mdlChannelIndentforAdd.ModifiedBy = mdlUser.ID;
                        mdlChannelIndentforAdd.ModifiedDate = DateTime.Now;
                        bllIndents.UpdateIndent(mdlChannelIndentforAdd);

                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("GaugeID", mdlChannelGauge.ID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.ChangeIndent, SessionManagerFacade.UserInformation.ID);

                    }
                    transaction.Complete();
                }
                BindGrid();
                BindChannelData();
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                btnSave.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }

        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 16/08/2016 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PlacingIndents);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindHistoryData()
        {
            BindSubDivisionDropdown();


            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.PlacingIndents];
            long SubDivID = Convert.ToInt64(SearchCriteria[0]);
            ddlSubDivision.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlSubDivision, SubDivID.ToString());

            BindChannelDropdown();
            long ChannelID = Convert.ToInt64(SearchCriteria[1]);
            ddlChannel.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlChannel, ChannelID.ToString());
            ddlChannel.Enabled = true;
            txtDate.Text = SearchCriteria[2];

            lblIMISCode.Text = SearchCriteria[3];
            lblCurrentSubDivisionIndent.Text = SearchCriteria[4];
            lblCurrentSubDivisionIndentDate.Text = SearchCriteria[5];
            lblTotalIndnetAtCurrentSubDivisionalHead.Text = SearchCriteria[6];
            lblLowerSubDivisionIndent.Text = SearchCriteria[7];
            lblLowerSubDivisionIndentDate.Text = SearchCriteria[8];

            btnSave.Visible = SearchCriteria[9];

            divHeader.Visible = true;
            BindGrid();
        }

        protected void hlIndent_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnIndent = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)btnIndent.NamingContainer;

                Label lblDirectOfftakesID = (Label)gvRow.FindControl("lblDirectOfftakesID");
                Label lblOfftakeIndentDate = (Label)gvRow.FindControl("lblOfftakeIndentDate");

                List<dynamic> SearchCriteria = new List<dynamic>();
                SearchCriteria.Add(ddlSubDivision.SelectedValue);
                SearchCriteria.Add(ddlChannel.SelectedValue);
                SearchCriteria.Add(txtDate.Text);
                SearchCriteria.Add(lblIMISCode.Text);
                SearchCriteria.Add(lblCurrentSubDivisionIndent.Text);
                SearchCriteria.Add(lblCurrentSubDivisionIndentDate.Text);
                SearchCriteria.Add(lblTotalIndnetAtCurrentSubDivisionalHead.Text);
                SearchCriteria.Add(lblLowerSubDivisionIndent.Text);
                SearchCriteria.Add(lblLowerSubDivisionIndentDate.Text);
                SearchCriteria.Add(btnSave.Visible);
                Session[SessionValues.PlacingIndents] = SearchCriteria;


                Response.Redirect("~/Modules/DailyData/IndentsHistory.aspx?OffTakeID=" + lblDirectOfftakesID.Text + "&Date=" + Convert.ToDateTime(lblOfftakeIndentDate.Text).ToString("MM/dd/yyyy") + "&Page=" + true, false);
                //NavigateUrl='<%# String.Format("~/Modules/DailyData/IndentsHistory.aspx?OffTakeID={0}&Date={1}&Page={2}", Convert.ToString(Eval("DirectOfftakeID")),Convert.ToDateTime(Eval("OfftakeIndentDate")).ToString("MM/dd/yyyy"),true) %>'
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}