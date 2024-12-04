using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.ScheduleInspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ScheduleDetail : BasePage
    {
        private static bool _IsSaved = false;
        private int BtnEditIndex = 6;
        private int BtnDeleteIndex = 7;
        private int BtnAddIndex = 8;
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


            if (!IsPostBack)
            {

                SetPageTitle();
                hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchSchedule.aspx";
                if (!string.IsNullOrEmpty(Request.QueryString["isSaved"]))
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["ScheduleID"]))
                {
                    if (_IsSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        _IsSaved = false; // Reset flag after displaying message.
                    }
                    hdnScheduleID.Value = Convert.ToString(Request.QueryString["ScheduleID"]);

                    SI_Schedule Schedule = new ScheduleInspectionBLL().GetScheduleBasicInformation(Convert.ToInt64(hdnScheduleID.Value));
                    hdnFromDate.Value = Convert.ToString(Schedule.FromDate);
                    hdnToDate.Value = Convert.ToString(Schedule.ToDate);

                    long PreparedByID = 0;
                    long StatusID = 0;
                    long DesignationID = 0;

                    GetScheduleDataInformation(ref PreparedByID, ref StatusID, ref DesignationID);
                    hdnStatusID.Value = Convert.ToString(StatusID);
                    hdnPreparedByID.Value = Convert.ToString(PreparedByID);
                    hdnPreparedByDesignationID.Value = Convert.ToString(DesignationID);
                    BindGaugeInspectionGridView();
                    BindDischargeTableInspectionGridView();
                    BindOutletAltrationGrid();
                    BindOutletPerformanceGrid();
                    BindTendersMonitoringGrid();
                    BindClosureOperationsGrid();
                    BindGeneralInspectionsGrid();
                    BindOutletCheckingGrid();
                    if (!GetVisibleValue(CanAdd))
                    {
                        gvGaugeInspection.Columns[BtnAddIndex].Visible = false;
                        gvDischargeInspection.Columns[BtnAddIndex].Visible = false;
                        gvOutletInspection.Columns[BtnAddIndex].Visible = false;
                        gvOutletPerformance.Columns[BtnAddIndex].Visible = false;
                        gvOutletChecking.Columns[BtnAddIndex].Visible = false;
                    }
                    if (!GetVisibleValue(CanDelete))
                    {
                        gvGaugeInspection.Columns[BtnDeleteIndex].Visible = false;
                        gvDischargeInspection.Columns[BtnDeleteIndex].Visible = false;
                        gvOutletInspection.Columns[BtnDeleteIndex].Visible = false;
                        gvOutletPerformance.Columns[BtnDeleteIndex].Visible = false;
                        gvOutletChecking.Columns[BtnDeleteIndex].Visible = false;
                    }
                    if (!GetVisibleValue(CanEdit))
                    {
                        gvGaugeInspection.Columns[BtnEditIndex].Visible = false;
                        gvDischargeInspection.Columns[BtnEditIndex].Visible = false;
                        gvOutletInspection.Columns[BtnEditIndex].Visible = false;
                        gvOutletPerformance.Columns[BtnEditIndex].Visible = false;
                        gvOutletChecking.Columns[BtnEditIndex].Visible = false;
                    }
                }
            }
        }
        private List<object> LoadAllRegionDDByUser()
        {
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

            if (IrrigationLevelID == null)
                return new List<object>();

            List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(userID, Convert.ToInt32(IrrigationLevelID));
            return lstData;

        }

        #region Gauge Inspection
        //protected void gvGaugeInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //try
        //    //{
        //    //    if (e.Row.RowType == DataControlRowType.DataRow && gvGaugeInspection.EditIndex == e.Row.RowIndex)
        //    //    {
        //    //        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGaugeInspection, "ScheduleDetailID", e.Row.RowIndex));

        //    //        #region "Datakeys"
        //    //        string DivisionID = GetDataKeyValue(gvGaugeInspection, "DivisionID", e.Row.RowIndex);
        //    //        string SubDivisionID = GetDataKeyValue(gvGaugeInspection, "SubDivisionID", e.Row.RowIndex);
        //    //        string ChannelID = GetDataKeyValue(gvGaugeInspection, "ChannelID", e.Row.RowIndex);
        //    //        string GaugeID = GetDataKeyValue(gvGaugeInspection, "GaugeID", e.Row.RowIndex);
        //    //        string DateOfVisit = GetDataKeyValue(gvGaugeInspection, "DateOfVisit", e.Row.RowIndex);
        //    //        string Remarks = GetDataKeyValue(gvGaugeInspection, "Remarks", e.Row.RowIndex);
        //    //        #endregion

        //    //        #region "Controls"
        //    //        DropDownList ddlGaugeInspectionDivision = (DropDownList)e.Row.FindControl("ddlGaugeInspectionDivision");
        //    //        DropDownList ddlGaugeInspectionSubDivision = (DropDownList)e.Row.FindControl("ddlGaugeInspectionSubDivision");
        //    //        DropDownList ddlGaugeInspectionChannel = (DropDownList)e.Row.FindControl("ddlGaugeInspectionChannel");
        //    //        DropDownList ddlGaugeInspectionAreas = (DropDownList)e.Row.FindControl("ddlGaugeInspectionAreas");

        //    //        TextBox txtGaugeInspectionRemarks = (TextBox)e.Row.FindControl("txtGaugeInspectionRemarks");
        //    //        TextBox txtGaugeInspectionDateOfVisit = (TextBox)e.Row.FindControl("txtGaugeInspectionDateOfVisit");
        //    //        #endregion

        //    //        int select = (int)Constants.DropDownFirstOption.Select;
        //    //        int noOption = (int)Constants.DropDownFirstOption.NoOption;
        //    //        List<object> lstData = LoadAllRegionDDByUser();

        //    //        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
        //    //        if (ddlGaugeInspectionDivision != null && lstDivision.Count > 0)
        //    //        {
        //    //            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlGaugeInspectionDivision, lstDivision, lstDivision.Count == 1 ? noOption : select);
        //    //            Dropdownlist.SetSelectedValue(ddlGaugeInspectionDivision, DivisionID);
        //    //        }
        //    //        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
        //    //        if (ddlGaugeInspectionSubDivision != null && lstSubDivision.Count > 0)
        //    //        {
        //    //            if (string.IsNullOrEmpty(ddlGaugeInspectionDivision.SelectedItem.Value))
        //    //            {
        //    //                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision, true);
        //    //            }
        //    //            else
        //    //            {
        //    //                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlGaugeInspectionSubDivision, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
        //    //                Dropdownlist.SetSelectedValue(ddlGaugeInspectionSubDivision, SubDivisionID);
        //    //            }

        //    //        }
        //    //        if (ddlGaugeInspectionChannel != null)
        //    //        {
        //    //            if (string.IsNullOrEmpty(ddlGaugeInspectionSubDivision.SelectedItem.Value))
        //    //            {
        //    //                Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel, 0, true);
        //    //            }
        //    //            else
        //    //            {
        //    //                Dropdownlist.DDLChannelsBySubDivID(ddlGaugeInspectionChannel, Convert.ToInt64(ddlGaugeInspectionSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
        //    //                Dropdownlist.SetSelectedValue(ddlGaugeInspectionChannel, ChannelID);
        //    //            }
        //    //        }
        //    //        if (ddlGaugeInspectionAreas != null)
        //    //        {
        //    //            if (string.IsNullOrEmpty(ddlGaugeInspectionChannel.SelectedItem.Value))
        //    //            {
        //    //                Dropdownlist.DDLStructureChannels(ddlGaugeInspectionAreas, 0, true);
        //    //            }
        //    //            else
        //    //            {
        //    //                Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeInspectionAreas, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlGaugeInspectionChannel.SelectedItem.Value)));
        //    //                Dropdownlist.SetSelectedValue(ddlGaugeInspectionAreas, GaugeID);
        //    //            }
        //    //        }
        //    //        if (txtGaugeInspectionDateOfVisit != null)
        //    //            txtGaugeInspectionDateOfVisit.Text = DateOfVisit;

        //    //        if (txtGaugeInspectionRemarks != null)
        //    //            txtGaugeInspectionRemarks.Text = Remarks;
        //    //    }
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    //}
        //}
        protected void gvGaugeInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Add Single
                //if (e.CommandName == "AddGaugeInspection")
                //{
                //    List<object> lstGaugeInspection = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.GaugeReading);
                //    lstGaugeInspection.Insert(0, GetNewScheduleDetail());
                //    BindNewScheduleDetailGridView(gvGaugeInspection, lstGaugeInspection);
                //    RemoveAllpopupValidation();
                //}
                #endregion
                #region Add Multiple
                if (e.CommandName == "AddMulGaugeInspection")
                {
                    List<object> lstData = LoadAllRegionDDByUser();
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    lstArea.Items.Clear();
                    SelectedInspectino.Items.Clear();
                    OnPoupValidation(1);
                    txtGaugeInspectionDateOfVisit_Mul.Text = string.Empty;
                    txtGaugeInspectionDateOfVisit_Mul.Text = string.Empty;

                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlGaugeInspectionDivision_Mul, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        if (ddlGaugeInspectionDivision_Mul != null && lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlGaugeInspectionDivision_Mul, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddlGaugeInspectionDivision_Mul, "");
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddlGaugeInspectionDivision_Mul.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Mul, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlGaugeInspectionSubDivision_Mul, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlGaugeInspectionSubDivision_Mul, "");
                            }

                        }

                        if (string.IsNullOrEmpty(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Mul, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlGaugeInspectionChannel_Mul, Convert.ToInt64(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlGaugeInspectionChannel_Mul, "");
                        }


                        if (string.IsNullOrEmpty(ddlGaugeInspectionChannel_Mul.SelectedItem.Value))
                        {
                            object ob = new { ID = 0, Name = "" };
                            List<object> lst = new List<object>();
                            lst.Add(ob);
                            lstArea.DataSource = lst;
                            lstArea.DataTextField = "Name";
                            lstArea.DataValueField = "ID";
                            lstArea.DataBind();
                        }
                        else
                        {
                            lstArea.DataSource = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlGaugeInspectionChannel_Mul.SelectedItem.Value));
                            lstArea.DataTextField = "Name";
                            lstArea.DataValueField = "ID";
                            lstArea.DataBind();
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddlGaugeInspectionDivision_Mul);
                        Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Mul, true);
                        Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Mul, 0, true);
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StatusDischargeArea", "$('#AddMultipalInspectionArea').modal();", true);
                }
                #endregion
                #region Edit
                if (e.CommandName == "Edit")
                {
                    GridView gv = sender as GridView;
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    #region "Datakeys"
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gv, "ScheduleDetailID", rowIndex));
                    string DivisionID = GetDataKeyValue(gv, "DivisionID", rowIndex);
                    string SubDivisionID = GetDataKeyValue(gv, "SubDivisionID", rowIndex);
                    string ChannelID = GetDataKeyValue(gv, "ChannelID", rowIndex);
                    string GaugeID = GetDataKeyValue(gv, "GaugeID", rowIndex);
                    string DateOfVisit = GetDataKeyValue(gv, "DateOfVisit", rowIndex);
                    string Remarks = GetDataKeyValue(gv, "Remarks", rowIndex);
                    #endregion
                    #region RestControls
                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                    Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                    lbCommonName.Text = "Inspection Areas";
                    ViewState["InspectionTypeID"] = Convert.ToString((long)Constants.SIInspectionType.GaugeReading);
                    txtDateofVist_Common.Text = string.Empty;
                    txtRemarks_Common.Text = string.Empty;
                    #endregion
                    #region Binding Controls
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    List<object> lstData = LoadAllRegionDDByUser();

                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Common, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Common, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);
                            }
                        }

                        if (ddlChannelName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                                Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);
                            }
                        }
                        if (ddlOutletName_Common != null)///// Common name outlet and  Inspection area for first 2 grid it map on Inspection Area and next 2 it map on outlet name
                        {
                            if (string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                                Dropdownlist.SetSelectedValue(ddlOutletName_Common, GaugeID);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Common);
                        Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);

                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt64(ddldivision_Common.SelectedItem.Value));
                        Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);

                        Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);

                        Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                        Dropdownlist.SetSelectedValue(ddlOutletName_Common, GaugeID);
                    }
                    if (txtDateofVist_Common != null)
                        txtDateofVist_Common.Text = DateOfVisit;

                    if (txtRemarks_Common != null)
                        txtRemarks_Common.Text = Remarks;
                    rowIndex_Common.Value = rowIndex.ToString();
                    #endregion
                    OnPoupValidation(4);//update popu validation  set for 4
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#UpdatePopup').modal();", true);
                }
                #endregion
                #region Delete
                if (e.CommandName == "Delete")
                {
                    try
                    {
                        GridView gv = sender as GridView;
                        int rowIndex = int.Parse(e.CommandArgument.ToString());
                        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gv, "ScheduleDetailID", rowIndex));
                        bool isRecordExists = new ScheduleInspectionBLL().IsGaugeInspectionNotesExist(ScheduleDetailID);

                        if (!isRecordExists)
                        {
                            bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
                            if (isDeleted)
                            {
                                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                                BindGaugeInspectionGridView();
                            }
                            else
                                Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    catch (Exception exp)
                    {
                        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                    }

                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGaugeInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try
            //{
            //    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGaugeInspection, "ScheduleDetailID", e.RowIndex));
            //    bool isRecordExists = new ScheduleInspectionBLL().IsGaugeInspectionNotesExist(ScheduleDetailID);

            //    if (!isRecordExists)
            //    {
            //        bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
            //        if (isDeleted)
            //        {
            //            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
            //            BindGaugeInspectionGridView();
            //        }
            //        else
            //            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //    }
            //    else
            //        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        //protected void gvGaugeInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //try
        //    //{
        //    //    gvGaugeInspection.PageIndex = e.NewPageIndex;
        //    //    gvGaugeInspection.EditIndex = -1;
        //    //    BindGaugeInspectionGridView();
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    //}
        //}
        protected void gvGaugeInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //try
            //{
            //    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGaugeInspection, "ScheduleDetailID", e.RowIndex));
            //    GridViewRow row = gvGaugeInspection.Rows[e.RowIndex];
            //    #region "Datakeys"
            //    string DivisionID = GetDataKeyValue(gvGaugeInspection, "DivisionID", e.RowIndex);
            //    string SubDivisionID = GetDataKeyValue(gvGaugeInspection, "SubDivisionID", e.RowIndex);
            //    string ChannelID = GetDataKeyValue(gvGaugeInspection, "ChannelID", e.RowIndex);
            //    string GaugeID = GetDataKeyValue(gvGaugeInspection, "GaugeID", e.RowIndex);
            //    string DateOfVisit = GetDataKeyValue(gvGaugeInspection, "DateOfVisit", e.RowIndex);
            //    string Remarks = GetDataKeyValue(gvGaugeInspection, "Remarks", e.RowIndex);

            //    #endregion
            //    #region "Controls"
            //    //DropDownList ddlGaugeInspectionDivision = (DropDownList)row.FindControl("ddlGaugeInspectionDivision");
            //    //DropDownList ddlGaugeInspectionSubDivision = (DropDownList)row.FindControl("ddlGaugeInspectionSubDivision");
            //    //DropDownList ddlGaugeInspectionChannel = (DropDownList)row.FindControl("ddlGaugeInspectionChannel");
            //    //DropDownList ddlGaugeInspectionAreas = (DropDownList)row.FindControl("ddlGaugeInspectionAreas");

            //    //TextBox txtGaugeInspectionRemarks = (TextBox)row.FindControl("txtGaugeInspectionRemarks");
            //    //TextBox txtGaugeInspectionDateOfVisit = (TextBox)row.FindControl("txtGaugeInspectionDateOfVisit");
            //    #endregion
            //    #region Get Values from Controls
            //    if (ValidateDateOfVisit(txtRemarks_Common.Text))
            //    {
            //        SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(ScheduleDetailID, txtRemarks_Common.Text);
            //        ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Common.SelectedItem.Value);
            //        ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value);
            //        ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value);
            //        ScheduleDetailChannel.GaugeID = Convert.ToInt64(ddlOutletName_Common.SelectedItem.Value);
            //        ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.GaugeReading;
            //        ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateofVist_Common.Text);
            //        ScheduleDetailChannel.Remarks = txtRemarks_Common.Text;
            //        bool isSaved = false;
            //        if (ScheduleDetailChannel.ID > 0)
            //        {
            //            isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
            //            gvGaugeInspection.EditIndex = -1;
            //            BindGaugeInspectionGridView();
            //        }
            //        else
            //        {
            //            if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
            //            {
            //                isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
            //                if (isSaved)
            //                {
            //                    // Redirect user to first page.
            //                    if (Convert.ToInt64(ScheduleDetailID) == 0)
            //                        gvGaugeInspection.PageIndex = 0;

            //                    gvGaugeInspection.EditIndex = -1;
            //                    BindGaugeInspectionGridView();
            //                    Master.ShowMessage(Message.RecordSaved.Description);
            //                }
            //            }
            //            else
            //            {
            //                Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
            //            }
            //        }
            //    }
            //    else
            //        Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
            //    #endregion
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        public void BindGaugeInspectionGridView()
        {
            try
            {
                GridView gvGaugeInspection = (GridView)UpdatePanelSchduleInspection.FindControl("gvGaugeInspection");
                gvGaugeInspection.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.GaugeReading);
                gvGaugeInspection.DataBind();
                UpdatePanelSchduleInspection.Update();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{
            //    RemoveAllpopupValidation();
            //    gvGaugeInspection.EditIndex = e.NewEditIndex;
            //    BindGaugeInspectionGridView();

            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        //protected void gvGaugeInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    //try
        //    //{
        //    //    gvGaugeInspection.EditIndex = -1;
        //    //    BindGaugeInspectionGridView();
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    //}
        //}
        //protected void ddlGaugeInspectionDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindSubDivisionsByDivisionID(sender, "ddlGaugeInspectionDivision", "ddlGaugeInspectionSubDivision");
        //}
        //protected void ddlGaugeInspectionSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetChannelsBySubDivID(sender, "ddlGaugeInspectionDivision", "ddlGaugeInspectionSubDivision", "ddlGaugeInspectionChannel", "ddlGaugeInspectionAreas");
        //}
        //protected void ddlGaugeInspectionChannel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetInspectionAreas(sender, "ddlGaugeInspectionChannel", "ddlGaugeInspectionAreas");
        //}
        #endregion

        #region Discharge table Calculation
        public void BindDischargeTableInspectionGridView()
        {
            try
            {
                GridView gvDischargeInspection = (GridView)UpdatePanelSchduleInspection.FindControl("gvDischargeInspection");
                gvDischargeInspection.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.DischargeTableCalculation);
                gvDischargeInspection.DataBind();
                UpdatePanelSchduleInspection.Update();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDischargeInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{

            //    if (e.Row.RowType == DataControlRowType.DataRow && gvDischargeInspection.EditIndex == e.Row.RowIndex)
            //    {
            //        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvDischargeInspection, "ScheduleDetailID", e.Row.RowIndex));

            //        #region "Datakeys"
            //        string DivisionID = GetDataKeyValue(gvDischargeInspection, "DivisionID", e.Row.RowIndex);
            //        string SubDivisionID = GetDataKeyValue(gvDischargeInspection, "SubDivisionID", e.Row.RowIndex);
            //        string ChannelID = GetDataKeyValue(gvDischargeInspection, "ChannelID", e.Row.RowIndex);
            //        string GaugeID = GetDataKeyValue(gvDischargeInspection, "GaugeID", e.Row.RowIndex);
            //        string DateOfVisit = GetDataKeyValue(gvDischargeInspection, "DateOfVisit", e.Row.RowIndex);
            //        string Remarks = GetDataKeyValue(gvDischargeInspection, "Remarks", e.Row.RowIndex);
            //        #endregion

            //        #region "Controls"
            //        DropDownList ddlDischargeDivision = (DropDownList)e.Row.FindControl("ddlDischargeDivision");
            //        DropDownList ddlDischargeSubDivision = (DropDownList)e.Row.FindControl("ddlDischargeSubDivision");
            //        DropDownList ddlDischargeChannel = (DropDownList)e.Row.FindControl("ddlDischargeChannel");
            //        DropDownList ddlDischargeInspectionAreas = (DropDownList)e.Row.FindControl("ddlDischargeInspectionAreas");

            //        TextBox txtDischargeRemarks = (TextBox)e.Row.FindControl("txtDischargeRemarks");
            //        TextBox txtDischargeDateOfVisit = (TextBox)e.Row.FindControl("txtDischargeDateOfVisit");
            //        #endregion

            //        int select = (int)Constants.DropDownFirstOption.Select;
            //        int noOption = (int)Constants.DropDownFirstOption.NoOption;
            //        List<object> lstData = LoadAllRegionDDByUser();

            //        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
            //        if (ddlDischargeDivision != null && lstDivision.Count > 0)
            //        {
            //            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDischargeDivision, lstDivision, lstDivision.Count == 1 ? noOption : select);
            //            Dropdownlist.SetSelectedValue(ddlDischargeDivision, DivisionID);
            //        }
            //        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
            //        if (ddlDischargeSubDivision != null && lstSubDivision.Count > 0)
            //        {
            //            if (string.IsNullOrEmpty(ddlDischargeDivision.SelectedItem.Value))
            //            {
            //                Dropdownlist.DDLSubDivisions(ddlDischargeSubDivision, true);
            //            }
            //            else
            //            {
            //                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlDischargeSubDivision, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
            //                Dropdownlist.SetSelectedValue(ddlDischargeSubDivision, SubDivisionID);
            //            }

            //        }
            //        if (ddlDischargeChannel != null)
            //        {
            //            if (string.IsNullOrEmpty(ddlDischargeSubDivision.SelectedItem.Value))
            //            {
            //                Dropdownlist.DDLStructureChannels(ddlDischargeChannel, 0, true);
            //            }
            //            else
            //            {
            //                Dropdownlist.DDLChannelsBySubDivID(ddlDischargeChannel, Convert.ToInt64(ddlDischargeSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            //                Dropdownlist.SetSelectedValue(ddlDischargeChannel, ChannelID);
            //            }
            //        }
            //        if (ddlDischargeInspectionAreas != null)
            //        {
            //            if (string.IsNullOrEmpty(ddlDischargeChannel.SelectedItem.Value))
            //            {
            //                Dropdownlist.DDLStructureChannels(ddlDischargeInspectionAreas, 0, true);
            //            }
            //            else
            //            {
            //                Dropdownlist.BindDropdownlist<List<object>>(ddlDischargeInspectionAreas, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlDischargeChannel.SelectedItem.Value)));
            //                Dropdownlist.SetSelectedValue(ddlDischargeInspectionAreas, GaugeID);
            //            }
            //        }
            //        if (txtDischargeDateOfVisit != null)
            //            txtDischargeDateOfVisit.Text = DateOfVisit;

            //        if (txtDischargeRemarks != null)
            //            txtDischargeRemarks.Text = Remarks;
            //    }
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        protected void gvDischargeInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Add Single
                //if (e.CommandName == "AddDischarge")
                //{
                //    List<object> lstDischargeInspection = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.DischargeTableCalculation);
                //    lstDischargeInspection.Insert(0, GetNewScheduleDetail());
                //    BindNewScheduleDetailGridView(gvDischargeInspection, lstDischargeInspection);
                //    RemoveAllpopupValidation();
                //}
                #endregion
                #region Add Multiple
                if (e.CommandName == "AddMulChnlDis")
                {
                    List<object> lstData = LoadAllRegionDDByUser();
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    left_list_dis.Items.Clear();
                    right_lst_discharge.Items.Clear();
                    txtGaugeInspectionRemarks_Dis.Text = string.Empty;
                    txtGaugeInspectionDateOfVisit_Dis.Text = string.Empty;
                    OnPoupValidation(2);

                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlGaugeInspectionDivision_Dis, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        if (ddlGaugeInspectionDivision_Dis != null && lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlGaugeInspectionDivision_Dis, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddlGaugeInspectionDivision_Dis, "");
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddlGaugeInspectionDivision_Dis.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Dis, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlGaugeInspectionSubDivision_Dis, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlGaugeInspectionSubDivision_Dis, "");
                            }

                        }

                        if (string.IsNullOrEmpty(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Dis, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlGaugeInspectionChannel_Dis, Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlGaugeInspectionChannel_Dis, "");
                        }


                        if (string.IsNullOrEmpty(ddlGaugeInspectionChannel_Dis.SelectedItem.Value))
                        {
                            object ob = new { ID = 0, Name = "" };
                            List<object> lst = new List<object>();
                            lst.Add(ob);
                            lstArea.DataSource = lst;
                            lstArea.DataTextField = "Name";
                            lstArea.DataValueField = "ID";
                            lstArea.DataBind();
                        }
                        else
                        {
                            lstArea.DataSource = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlGaugeInspectionChannel_Dis.SelectedItem.Value));
                            lstArea.DataTextField = "Name";
                            lstArea.DataValueField = "ID";
                            lstArea.DataBind();
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddlGaugeInspectionDivision_Dis);
                        Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Dis, true);
                        Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Dis, 0, true);
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StatusDischargeInspection", "$('#ModelPupDicchargeCalcution').modal();", true);


                }
                #endregion
                #region Edit
                if (e.CommandName == "Edit")
                {
                    GridView gv = sender as GridView;
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    #region "Datakeys"
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gv, "ScheduleDetailID", rowIndex));
                    string DivisionID = GetDataKeyValue(gv, "DivisionID", rowIndex);
                    string SubDivisionID = GetDataKeyValue(gv, "SubDivisionID", rowIndex);
                    string ChannelID = GetDataKeyValue(gv, "ChannelID", rowIndex);
                    string GaugeID = GetDataKeyValue(gv, "GaugeID", rowIndex);
                    string DateOfVisit = GetDataKeyValue(gv, "DateOfVisit", rowIndex);
                    string Remarks = GetDataKeyValue(gv, "Remarks", rowIndex);
                    #endregion
                    #region RestControls
                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                    Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                    lbCommonName.Text = "Inspection Areas";
                    ViewState["InspectionTypeID"] = Convert.ToString((long)Constants.SIInspectionType.DischargeTableCalculation);
                    txtDateofVist_Common.Text = string.Empty;
                    txtRemarks_Common.Text = string.Empty;
                    #endregion
                    #region Binding Controls
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    List<object> lstData = LoadAllRegionDDByUser();


                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Common, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Common, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);
                            }
                        }

                        if (ddlChannelName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                                Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);
                            }
                        }
                        if (ddlOutletName_Common != null)///// Common name outlet and  Inspection area for first 2 grid it map on Inspection Area and next 2 it map on outlet name
                        {
                            if (string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                                Dropdownlist.SetSelectedValue(ddlOutletName_Common, GaugeID);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Common);
                        Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);

                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt64(ddldivision_Common.SelectedItem.Value));
                        Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);

                        Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);

                        Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                        Dropdownlist.SetSelectedValue(ddlOutletName_Common, GaugeID);
                    }
                    if (txtDateofVist_Common != null)
                        txtDateofVist_Common.Text = DateOfVisit;

                    if (txtRemarks_Common != null)
                        txtRemarks_Common.Text = Remarks;
                    rowIndex_Common.Value = rowIndex.ToString();
                    #endregion
                    OnPoupValidation(4);//update popu validation  set for 4
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#UpdatePopup').modal();", true);
                }
                #endregion
                #region Delete
                if (e.CommandName == "Delete")
                {
                    try
                    {
                        GridView gv = sender as GridView;
                        int rowIndex = int.Parse(e.CommandArgument.ToString());
                        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gv, "ScheduleDetailID", rowIndex));
                        bool isRecordExists = new ScheduleInspectionBLL().IsGaugeInspectionNotesExist(ScheduleDetailID);

                        if (!isRecordExists)
                        {
                            bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
                            if (isDeleted)
                            {
                                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                                BindDischargeTableInspectionGridView();
                            }
                            else
                                Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    catch (Exception exp)
                    {
                        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                    }

                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void gvDischargeInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        gvDischargeInspection.PageIndex = e.NewPageIndex;
        //        gvDischargeInspection.EditIndex = -1;
        //        BindDischargeTableInspectionGridView();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        protected void gvDischargeInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try
            //{
            //    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvDischargeInspection, "ScheduleDetailID", e.RowIndex));

            //    bool isRecordExists = new ScheduleInspectionBLL().IsDTParameterInspectionNotesExist(ScheduleDetailID);
            //    if (!isRecordExists)
            //    {
            //        bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
            //        if (isDeleted)
            //        {
            //            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
            //            BindDischargeTableInspectionGridView();
            //        }
            //        else
            //            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //    }
            //    else
            //        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        protected void gvDischargeInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{
            //    RemoveAllpopupValidation();
            //    gvDischargeInspection.EditIndex = e.NewEditIndex;
            //    BindDischargeTableInspectionGridView();
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        //protected void gvDischargeInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvDischargeInspection.EditIndex = -1;
        //        BindDischargeTableInspectionGridView();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        protected void gvDischargeInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvDischargeInspection, "ScheduleDetailID", e.RowIndex));
                GridViewRow row = gvDischargeInspection.Rows[e.RowIndex];

                #region "Datakeys"
                string DivisionID = GetDataKeyValue(gvDischargeInspection, "DivisionID", e.RowIndex);
                string SubDivisionID = GetDataKeyValue(gvDischargeInspection, "SubDivisionID", e.RowIndex);
                string ChannelID = GetDataKeyValue(gvDischargeInspection, "ChannelID", e.RowIndex);
                string GaugeID = GetDataKeyValue(gvDischargeInspection, "GaugeID", e.RowIndex);
                string DateOfVisit = GetDataKeyValue(gvDischargeInspection, "DateOfVisit", e.RowIndex);
                string Remarks = GetDataKeyValue(gvDischargeInspection, "Remarks", e.RowIndex);

                #endregion

                #region "Controls"
                DropDownList ddlDischargeDivision = (DropDownList)row.FindControl("ddlDischargeDivision");
                DropDownList ddlDischargeSubDivision = (DropDownList)row.FindControl("ddlDischargeSubDivision");
                DropDownList ddlDischargeChannel = (DropDownList)row.FindControl("ddlDischargeChannel");
                DropDownList ddlDischargeInspectionAreas = (DropDownList)row.FindControl("ddlDischargeInspectionAreas");

                TextBox txtDischargeRemarks = (TextBox)row.FindControl("txtDischargeRemarks");
                TextBox txtDischargeDateOfVisit = (TextBox)row.FindControl("txtDischargeDateOfVisit");
                #endregion

                if (ValidateDateOfVisit(txtDischargeDateOfVisit.Text))
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(ScheduleDetailID, txtDischargeRemarks.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlDischargeDivision.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlDischargeSubDivision.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlDischargeChannel.SelectedItem.Value);
                    ScheduleDetailChannel.GaugeID = Convert.ToInt64(ddlDischargeInspectionAreas.SelectedItem.Value);
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.DischargeTableCalculation;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDischargeDateOfVisit.Text);

                    bool isSaved = false;
                    if (ScheduleDetailChannel.ID > 0)
                    {
                        isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                        gvDischargeInspection.EditIndex = -1;
                        BindDischargeTableInspectionGridView();
                    }

                    else
                    {
                        if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
                        {
                            isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                            if (isSaved)
                            {
                                gvDischargeInspection.EditIndex = -1;
                                BindDischargeTableInspectionGridView();
                                Master.ShowMessage(Message.RecordSaved.Description);
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                        }
                    }

                }
                else
                    Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void ddlDischargeDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindSubDivisionsByDivisionID(sender, "ddlDischargeDivision", "ddlDischargeSubDivision");
        //}
        //protected void ddlDischargeSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetChannelsBySubDivID(sender, "ddlDischargeDivision", "ddlDischargeSubDivision", "ddlDischargeChannel", "ddlDischargeInspectionAreas");
        //}
        //protected void ddlDischargeChannel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetInspectionAreas(sender, "ddlDischargeChannel", "ddlDischargeInspectionAreas");
        //}

        #endregion

        #region Outlet Altration
        public void BindOutletAltrationGrid()
        {
            try
            {
                //GridView gvOutletInspection = (GridView)UpdatePanelSchduleInspection.FindControl("gvOutletInspection");
                gvOutletInspection.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletAlteration);
                gvOutletInspection.DataBind();
                UpdatePanelSchduleInspection.Update();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void gvOutletInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow && gvOutletInspection.EditIndex == e.Row.RowIndex)
        //        {
        //            long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletInspection, "ScheduleDetailID", e.Row.RowIndex));

        //            #region "Datakeys"
        //            string DivisionID = GetDataKeyValue(gvOutletInspection, "DivisionID", e.Row.RowIndex);
        //            string SubDivisionID = GetDataKeyValue(gvOutletInspection, "SubDivisionID", e.Row.RowIndex);
        //            string ChannelID = GetDataKeyValue(gvOutletInspection, "ChannelID", e.Row.RowIndex);
        //            string OutletID = GetDataKeyValue(gvOutletInspection, "OutletID", e.Row.RowIndex);
        //            string DateOfVisit = GetDataKeyValue(gvOutletInspection, "DateOfVisit", e.Row.RowIndex);
        //            string Remarks = GetDataKeyValue(gvOutletInspection, "Remarks", e.Row.RowIndex);
        //            #endregion

        //            #region "Controls"
        //            DropDownList ddlOutleDivision = (DropDownList)e.Row.FindControl("ddlOutleDivision");
        //            DropDownList ddlOutletSubDivision = (DropDownList)e.Row.FindControl("ddlOutletSubDivision");
        //            DropDownList ddlOutletChannel = (DropDownList)e.Row.FindControl("ddlOutletChannel");
        //            DropDownList ddlOutletInspectionRD = (DropDownList)e.Row.FindControl("ddlOutletInspectionRD");

        //            TextBox txtOutletRemarks = (TextBox)e.Row.FindControl("txtOutletRemarks");
        //            TextBox txtOutletDateOfVisit = (TextBox)e.Row.FindControl("txtOutletDateOfVisit");
        //            #endregion

        //            int select = (int)Constants.DropDownFirstOption.Select;
        //            int noOption = (int)Constants.DropDownFirstOption.NoOption;
        //            List<object> lstData = LoadAllRegionDDByUser();

        //            List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
        //            if (ddlOutleDivision != null && lstDivision.Count > 0)
        //            {
        //                Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlOutleDivision, lstDivision, lstDivision.Count == 1 ? noOption : select);
        //                Dropdownlist.SetSelectedValue(ddlOutleDivision, DivisionID);
        //            }
        //            List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
        //            if (ddlOutletSubDivision != null && lstSubDivision.Count > 0)
        //            {
        //                if (string.IsNullOrEmpty(ddlOutleDivision.SelectedItem.Value))
        //                {
        //                    Dropdownlist.DDLSubDivisions(ddlOutletSubDivision, true);
        //                }
        //                else
        //                {
        //                    Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlOutletSubDivision, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
        //                    Dropdownlist.SetSelectedValue(ddlOutletSubDivision, SubDivisionID);
        //                }
        //            }

        //            if (ddlOutletChannel != null)
        //            {
        //                if (string.IsNullOrEmpty(ddlOutletSubDivision.SelectedItem.Value))
        //                {
        //                    Dropdownlist.DDLStructureChannels(ddlOutletChannel, 0, true);
        //                }
        //                else
        //                {
        //                    Dropdownlist.DDLChannelsBySubDivID(ddlOutletChannel, Convert.ToInt64(ddlOutletSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
        //                    Dropdownlist.SetSelectedValue(ddlOutletChannel, ChannelID);
        //                }
        //            }
        //            if (ddlOutletInspectionRD != null)
        //            {
        //                if (string.IsNullOrEmpty(ddlOutletChannel.SelectedItem.Value))
        //                {
        //                    Dropdownlist.DDLStructureChannels(ddlOutletInspectionRD, 0, true);
        //                }
        //                else
        //                {
        //                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutletInspectionRD, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlOutletChannel.SelectedItem.Value)));
        //                    Dropdownlist.SetSelectedValue(ddlOutletInspectionRD, OutletID);
        //                }
        //            }
        //            if (txtOutletDateOfVisit != null)
        //                txtOutletDateOfVisit.Text = DateOfVisit;

        //            if (txtOutletRemarks != null)
        //                txtOutletRemarks.Text = Remarks;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        protected void gvOutletInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region ADD Single
                //if (e.CommandName == "AddOutlet")
                //{
                //    List<object> lstOutletInspection = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletAlteration);

                //    //lstOutletInspection.Add(GetNewScheduleDetail());
                //    lstOutletInspection.Insert(0, GetNewScheduleDetail());
                //    BindNewScheduleDetailGridView(gvOutletInspection, lstOutletInspection);
                //    RemoveAllpopupValidation();
                //}
                #endregion
                #region ADD Multiple
                if (e.CommandName == "AddMulChnlDis")
                {
                    List<object> lstData = LoadAllRegionDDByUser();
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    lstBox_Left_Outlet_Alteration.Items.Clear();
                    lstBox_right_Outlet_Alteration.Items.Clear();
                    OnPoupValidation(3);
                    txtRemarks_Outlet_Alteration.Text = string.Empty;
                    txtDateOfVisit_Outlet_Alteration.Text = string.Empty;
                    btnSaveOutletAlteration.Visible = true;
                    btnSaveOutletPerformance.Visible = false;
                    btnSaveOutletChecking.Visible = false;

                    btnSaveAndAddMoreOutletPerformance.Visible = false;
                    btnSaveAndAddMoreOutletChecking.Visible = false;
                    btnSaveAndAddMoreOutletAlteration.Visible = true;

                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Outlet_Alteration, "");
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Outlet_Alteration.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Outlet_Alteration, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Outlet_Alteration, "");
                            }

                        }

                        if (string.IsNullOrEmpty(ddlSubDivision_Outlet_Alteration.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Outlet_Alteration, Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlChannelName_Outlet_Alteration, "");
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Outlet_Alteration);
                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                        Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StatusDischargeInspection", "$('#addMultiOutLetAlteration').modal();", true);

                }
                #endregion
                #region Edit
                if (e.CommandName == "Edit")
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    #region "Datakeys"
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletInspection, "ScheduleDetailID", rowIndex));
                    string DivisionID = GetDataKeyValue(gvOutletInspection, "DivisionID", rowIndex);
                    string SubDivisionID = GetDataKeyValue(gvOutletInspection, "SubDivisionID", rowIndex);
                    string ChannelID = GetDataKeyValue(gvOutletInspection, "ChannelID", rowIndex);
                    string OutletID = GetDataKeyValue(gvOutletInspection, "OutletID", rowIndex);
                    string DateOfVisit = GetDataKeyValue(gvOutletInspection, "DateOfVisit", rowIndex);
                    string Remarks = GetDataKeyValue(gvOutletInspection, "Remarks", rowIndex);
                    #endregion
                    #region RestControls
                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                    Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                    txtDateofVist_Common.Text = string.Empty;
                    txtRemarks_Common.Text = string.Empty;
                    #endregion
                    #region Binding Controls
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    List<object> lstData = LoadAllRegionDDByUser();
                    lbCommonName.Text = "Outlet Name";
                    ViewState["InspectionTypeID"] = Convert.ToString((long)Constants.SIInspectionType.OutletAlteration);
                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);


                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Common, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Common, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);
                            }
                        }

                        if (ddlChannelName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                                Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);
                            }
                        }
                        if (ddlOutletName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                                Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Common);
                        Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);

                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt64(ddldivision_Common.SelectedItem.Value));
                        Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);

                        Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);

                        Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                        Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                    }
                    if (txtDateofVist_Common != null)
                        txtDateofVist_Common.Text = DateOfVisit;

                    if (txtRemarks_Common != null)
                        txtRemarks_Common.Text = Remarks;
                    rowIndex_Common.Value = rowIndex.ToString();
                    #endregion
                    OnPoupValidation(4);//update popu validation  set for 4
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#UpdatePopup').modal();", true);
                }
                #endregion
                #region Delete
                if (e.CommandName == "Delete")
                {
                    try
                    {
                        int rowIndex = int.Parse(e.CommandArgument.ToString());
                        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletInspection, "ScheduleDetailID", rowIndex));

                        bool isRecordExists = new ScheduleInspectionBLL().IsOutletAltrationInspNotesExist(ScheduleDetailID);
                        if (!isRecordExists)
                        {
                            bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
                            if (isDeleted)
                            {
                                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                                BindOutletAltrationGrid();
                            }
                            else
                                Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    catch (Exception exp)
                    {
                        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try
            //{
            //    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletInspection, "ScheduleDetailID", e.RowIndex));

            //    bool isRecordExists = new ScheduleInspectionBLL().IsOutletAltrationInspNotesExist(ScheduleDetailID);
            //    if (!isRecordExists)
            //    {
            //        bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
            //        if (isDeleted)
            //        {
            //            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
            //            BindOutletAltrationGrid();
            //        }
            //        else
            //            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //    }
            //    else
            //        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        protected void gvOutletInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletInspection.PageIndex = e.NewPageIndex;
                gvOutletInspection.EditIndex = -1;
                BindOutletAltrationGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void gvOutletInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvOutletInspection.EditIndex = -1;
        //        BindOutletAltrationGrid();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        protected void gvOutletInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //RemoveAllpopupValidation();
                //gvOutletInspection.EditIndex = e.NewEditIndex;
                //BindOutletAltrationGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletInspection, "ScheduleDetailID", e.RowIndex));
                GridViewRow row = gvOutletInspection.Rows[e.RowIndex];

                #region "Datakeys"
                string DivisionID = GetDataKeyValue(gvOutletInspection, "DivisionID", e.RowIndex);
                string SubDivisionID = GetDataKeyValue(gvOutletInspection, "SubDivisionID", e.RowIndex);
                string ChannelID = GetDataKeyValue(gvOutletInspection, "ChannelID", e.RowIndex);
                string OutletID = GetDataKeyValue(gvOutletInspection, "OutletID", e.RowIndex);
                string DateOfVisit = GetDataKeyValue(gvOutletInspection, "DateOfVisit", e.RowIndex);
                string Remarks = GetDataKeyValue(gvOutletInspection, "Remarks", e.RowIndex);

                #endregion

                #region "Controls"
                DropDownList ddlOutleDivision = (DropDownList)row.FindControl("ddlOutleDivision");
                DropDownList ddlOutletSubDivision = (DropDownList)row.FindControl("ddlOutletSubDivision");
                DropDownList ddlOutletChannel = (DropDownList)row.FindControl("ddlOutletChannel");
                DropDownList ddlOutletInspectionRD = (DropDownList)row.FindControl("ddlOutletInspectionRD");

                TextBox txtOutletRemarks = (TextBox)row.FindControl("txtOutletRemarks");
                TextBox txtOutletDateOfVisit = (TextBox)row.FindControl("txtOutletDateOfVisit");
                #endregion


                if (ValidateDateOfVisit(txtOutletDateOfVisit.Text))
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(ScheduleDetailID, txtOutletRemarks.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlOutleDivision.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlOutletSubDivision.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlOutletChannel.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = Convert.ToInt64(ddlOutletInspectionRD.SelectedItem.Value);
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletAlteration;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtOutletDateOfVisit.Text);

                    bool isSaved = false;
                    if (ScheduleDetailChannel.ID > 0)
                    {
                        isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                        gvOutletInspection.EditIndex = -1;
                        BindOutletAltrationGrid();
                    }
                    else
                    {
                        if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
                        {
                            isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                            if (isSaved)
                            {
                                gvOutletInspection.EditIndex = -1;
                                BindOutletAltrationGrid();
                                Master.ShowMessage(Message.RecordSaved.Description);
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                        }
                    }


                }
                else
                    Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlOutleDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubDivisionsByDivisionID(sender, "ddlOutleDivision", "ddlOutletSubDivision");
        }
        protected void ddlOutletSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChannelsBySubDivID(sender, "ddlOutleDivision", "ddlOutletSubDivision", "ddlOutletChannel", "ddlOutletInspectionRD");
        }
        protected void ddlOutletChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChannelOutlets(sender, "ddlOutletChannel", "ddlOutletInspectionRD");
        }

        #endregion

        #region Outlet Performance Register

        public void BindOutletPerformanceGrid()
        {
            try
            {
                GridView gvOutletPerformance = (GridView)UpdatePanelSchduleInspection.FindControl("gvOutletPerformance");
                gvOutletPerformance.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletPerformance);
                gvOutletPerformance.DataBind();
                UpdatePanelSchduleInspection.Update();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvOutletPerformance.EditIndex == e.Row.RowIndex)
                {
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletPerformance, "ScheduleDetailID", e.Row.RowIndex));

                    #region "Datakeys"
                    string DivisionID = GetDataKeyValue(gvOutletPerformance, "DivisionID", e.Row.RowIndex);
                    string SubDivisionID = GetDataKeyValue(gvOutletPerformance, "SubDivisionID", e.Row.RowIndex);
                    string ChannelID = GetDataKeyValue(gvOutletPerformance, "ChannelID", e.Row.RowIndex);
                    string OutletID = GetDataKeyValue(gvOutletPerformance, "OutletID", e.Row.RowIndex);
                    string DateOfVisit = GetDataKeyValue(gvOutletPerformance, "DateOfVisit", e.Row.RowIndex);
                    string Remarks = GetDataKeyValue(gvOutletPerformance, "Remarks", e.Row.RowIndex);
                    #endregion

                    #region "Controls"
                    DropDownList ddlOutletPerformanceDivision = (DropDownList)e.Row.FindControl("ddlOutletPerformanceDivision");
                    DropDownList ddlOutletPerformanceSubDivision = (DropDownList)e.Row.FindControl("ddlOutletPerformanceSubDivision");
                    DropDownList ddlOutletPerformanceChannelName = (DropDownList)e.Row.FindControl("ddlOutletPerformanceChannelName");
                    DropDownList ddlOutletPerformanceInspectionRD = (DropDownList)e.Row.FindControl("ddlOutletPerformanceInspectionRD");

                    TextBox txtOutletPerformanceRemarks = (TextBox)e.Row.FindControl("txtOutletPerformanceRemarks");
                    TextBox txtOutletPerformanceDateOfVisit = (TextBox)e.Row.FindControl("txtOutletPerformanceDateOfVisit");
                    #endregion

                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    List<object> lstData = LoadAllRegionDDByUser();

                    List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                    if (ddlOutletPerformanceDivision != null && lstDivision.Count > 0)
                    {
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlOutletPerformanceDivision, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        Dropdownlist.SetSelectedValue(ddlOutletPerformanceDivision, DivisionID);
                    }
                    List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                    if (ddlOutletPerformanceSubDivision != null && lstSubDivision.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ddlOutletPerformanceDivision.SelectedItem.Value))
                        {
                            Dropdownlist.DDLSubDivisions(ddlOutletPerformanceSubDivision, true);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlOutletPerformanceSubDivision, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddlOutletPerformanceSubDivision, SubDivisionID);
                        }
                    }
                    if (ddlOutletPerformanceChannelName != null)
                    {
                        if (string.IsNullOrEmpty(ddlOutletPerformanceSubDivision.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlOutletPerformanceChannelName, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlOutletPerformanceChannelName, Convert.ToInt64(ddlOutletPerformanceSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlOutletPerformanceChannelName, ChannelID);
                        }
                    }
                    if (ddlOutletPerformanceInspectionRD != null)
                    {
                        if (string.IsNullOrEmpty(ddlOutletPerformanceChannelName.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlOutletPerformanceInspectionRD, 0, true);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlOutletPerformanceInspectionRD, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlOutletPerformanceChannelName.SelectedItem.Value)));
                            Dropdownlist.SetSelectedValue(ddlOutletPerformanceInspectionRD, OutletID);
                        }
                    }
                    if (txtOutletPerformanceDateOfVisit != null)
                        txtOutletPerformanceDateOfVisit.Text = DateOfVisit;

                    if (txtOutletPerformanceRemarks != null)
                        txtOutletPerformanceRemarks.Text = Remarks;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Add Single
                //if (e.CommandName == "AddOutletPerformance")
                //{
                //    List<object> lstGaugeInspection = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletPerformance);
                //    lstGaugeInspection.Insert(0, GetNewScheduleDetail());
                //    BindNewScheduleDetailGridView(gvOutletPerformance, lstGaugeInspection);
                //    RemoveAllpopupValidation();


                //}
                #endregion
                #region Add Multiple
                if (e.CommandName == "AddMulChnlDis")
                {
                    List<object> lstData = LoadAllRegionDDByUser();
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    lstBox_Left_Outlet_Alteration.Items.Clear();
                    lstBox_right_Outlet_Alteration.Items.Clear();
                    btnSaveOutletAlteration.Visible = false;
                    btnSaveOutletPerformance.Visible = true;
                    btnSaveOutletChecking.Visible = false;

                    btnSaveAndAddMoreOutletPerformance.Visible = true;
                    btnSaveAndAddMoreOutletChecking.Visible = false;
                    btnSaveAndAddMoreOutletAlteration.Visible = false;

                    OnPoupValidation(3);
                    txtRemarks_Outlet_Alteration.Text = string.Empty;
                    txtDateOfVisit_Outlet_Alteration.Text = string.Empty;

                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Outlet_Alteration, "");
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Outlet_Alteration.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Outlet_Alteration, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Outlet_Alteration, "");
                            }

                        }

                        if (string.IsNullOrEmpty(ddlSubDivision_Outlet_Alteration.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Outlet_Alteration, Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlChannelName_Outlet_Alteration, "");
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Outlet_Alteration);
                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                        Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StatusDischargeInspection", "$('#addMultiOutLetAlteration').modal();", true);
                }
                #endregion
                #region Edit
                if (e.CommandName == "Edit")
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    #region "Datakeys"
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletPerformance, "ScheduleDetailID", rowIndex));
                    string DivisionID = GetDataKeyValue(gvOutletPerformance, "DivisionID", rowIndex);
                    string SubDivisionID = GetDataKeyValue(gvOutletPerformance, "SubDivisionID", rowIndex);
                    string ChannelID = GetDataKeyValue(gvOutletPerformance, "ChannelID", rowIndex);
                    string OutletID = GetDataKeyValue(gvOutletPerformance, "OutletID", rowIndex);
                    string DateOfVisit = GetDataKeyValue(gvOutletPerformance, "DateOfVisit", rowIndex);
                    string Remarks = GetDataKeyValue(gvOutletPerformance, "Remarks", rowIndex);
                    #endregion
                    #region RestControls
                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                    Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                    txtDateofVist_Common.Text = string.Empty;
                    txtRemarks_Common.Text = string.Empty;
                    #endregion
                    #region Binding Controls
                    int select = (int)Constants.DropDownFirstOption.Select;
                    int noOption = (int)Constants.DropDownFirstOption.NoOption;
                    List<object> lstData = LoadAllRegionDDByUser();
                    lbCommonName.Text = "Outlet Name";
                    ViewState["InspectionTypeID"] = Convert.ToString((long)Constants.SIInspectionType.OutletPerformance);
                    if (lstData != null && lstData.Count > 0)
                    {
                        List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                        if (lstDivision.Count > 0)
                        {
                            Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Common, lstDivision, lstDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);
                        }
                        List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                        if (lstSubDivision.Count > 0)
                        {
                            if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Common, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                                Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);
                            }
                        }

                        if (ddlChannelName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                                Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);
                            }
                        }
                        if (ddlOutletName_Common != null)
                        {
                            if (string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                            {
                                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                            }
                            else
                            {
                                Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                                Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                            }
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisionNames(ddldivision_Common);
                        Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);

                        Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt64(ddldivision_Common.SelectedItem.Value));
                        Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);

                        Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);

                        Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                        Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                    }
                    if (txtDateofVist_Common != null)
                        txtDateofVist_Common.Text = DateOfVisit;

                    if (txtRemarks_Common != null)
                        txtRemarks_Common.Text = Remarks;
                    rowIndex_Common.Value = rowIndex.ToString();
                    #endregion
                    OnPoupValidation(4);//update popu validation  set for 4
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#UpdatePopup').modal();", true);
                }
                #endregion
                #region Delete
                if (e.CommandName == "Delete")
                {
                    try
                    {
                        int rowIndex = int.Parse(e.CommandArgument.ToString());
                        long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletPerformance, "ScheduleDetailID", rowIndex));

                        bool isRecordExists = new ScheduleInspectionBLL().IsOutletPerformanceInspNotesExist(ScheduleDetailID);
                        if (!isRecordExists)
                        {
                            bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
                            if (isDeleted)
                            {
                                Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                                BindOutletPerformanceGrid();
                            }
                            else
                                Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    catch (Exception exp)
                    {
                        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try
            //{
            //    int rowIndex = int.Parse(e.CommandArgument.ToString());
            //    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletPerformance, "ScheduleDetailID", e.RowIndex));

            //    bool isRecordExists = new ScheduleInspectionBLL().IsOutletPerformanceInspNotesExist(ScheduleDetailID);
            //    if (!isRecordExists)
            //    {
            //        bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
            //        if (isDeleted)
            //        {
            //            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
            //            BindOutletPerformanceGrid();
            //        }
            //        else
            //            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //    }
            //    else
            //        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}


        }
        protected void gvOutletPerformance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletPerformance.PageIndex = e.NewPageIndex;
                gvOutletPerformance.EditIndex = -1;
                BindOutletPerformanceGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOutletPerformance.EditIndex = -1;
                BindOutletPerformanceGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletPerformance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{
            //    RemoveAllpopupValidation();
            //    gvOutletPerformance.EditIndex = e.NewEditIndex;
            //    BindOutletPerformanceGrid();
            //}
            //catch (Exception exp)
            //{
            //    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        protected void gvOutletPerformance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletPerformance, "ScheduleDetailID", e.RowIndex));
                GridViewRow row = gvOutletPerformance.Rows[e.RowIndex];

                #region "Datakeys"
                string DivisionID = GetDataKeyValue(gvOutletPerformance, "DivisionID", e.RowIndex);
                string SubDivisionID = GetDataKeyValue(gvOutletPerformance, "SubDivisionID", e.RowIndex);
                string ChannelID = GetDataKeyValue(gvOutletPerformance, "ChannelID", e.RowIndex);
                string OutletID = GetDataKeyValue(gvOutletPerformance, "OutletID", e.RowIndex);
                string DateOfVisit = GetDataKeyValue(gvOutletPerformance, "DateOfVisit", e.RowIndex);
                string Remarks = GetDataKeyValue(gvOutletPerformance, "Remarks", e.RowIndex);



                #endregion

                #region "Controls"
                DropDownList ddlOutletPerformanceDivision = (DropDownList)row.FindControl("ddlOutletPerformanceDivision");
                DropDownList ddlOutletPerformanceSubDivision = (DropDownList)row.FindControl("ddlOutletPerformanceSubDivision");
                DropDownList ddlOutletPerformanceChannelName = (DropDownList)row.FindControl("ddlOutletPerformanceChannelName");
                DropDownList ddlOutletPerformanceInspectionRD = (DropDownList)row.FindControl("ddlOutletPerformanceInspectionRD");

                TextBox txtOutletPerformanceRemarks = (TextBox)row.FindControl("txtOutletPerformanceRemarks");
                TextBox txtOutletPerformanceDateOfVisit = (TextBox)row.FindControl("txtOutletPerformanceDateOfVisit");
                #endregion

                if (ValidateDateOfVisit(txtOutletPerformanceDateOfVisit.Text))
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(ScheduleDetailID, txtOutletPerformanceRemarks.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlOutletPerformanceDivision.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlOutletPerformanceSubDivision.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlOutletPerformanceChannelName.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = Convert.ToInt64(ddlOutletPerformanceInspectionRD.SelectedItem.Value);
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletPerformance;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtOutletPerformanceDateOfVisit.Text);



                    bool isSaved = false;
                    if (ScheduleDetailChannel.ID > 0)
                    {
                        isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                        gvOutletPerformance.EditIndex = -1;
                        BindOutletPerformanceGrid();
                    }
                    else
                    {
                        if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
                        {
                            isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                            if (isSaved)
                            {
                                gvOutletPerformance.EditIndex = -1;
                                BindOutletPerformanceGrid();
                                Master.ShowMessage(Message.RecordSaved.Description);
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                        }
                    }





                }
                else
                    Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlOutletPerformanceDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubDivisionsByDivisionID(sender, "ddlOutletPerformanceDivision", "ddlOutletPerformanceSubDivision");
        }
        protected void ddlOutletPerformanceSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChannelsBySubDivID(sender, "ddlOutletPerformanceDivision", "ddlOutletPerformanceSubDivision", "ddlOutletPerformanceChannelName", "ddlOutletPerformanceInspectionRD");
        }
        protected void ddlOutletPerformanceChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChannelOutlets(sender, "ddlOutletPerformanceChannelName", "ddlOutletPerformanceInspectionRD");
        }

        #endregion

        #region Outlet Checking
        protected void gvOutletChecking_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvOutletChecking_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Add Multiple
            if (e.CommandName == "AddMulChnlDis")
            {
                List<object> lstData = LoadAllRegionDDByUser();
                int select = (int)Constants.DropDownFirstOption.Select;
                int noOption = (int)Constants.DropDownFirstOption.NoOption;
                lstBox_Left_Outlet_Alteration.Items.Clear();
                lstBox_right_Outlet_Alteration.Items.Clear();

                btnSaveOutletAlteration.Visible = false;
                btnSaveOutletPerformance.Visible = false;
                btnSaveOutletChecking.Visible = true;

                btnSaveAndAddMoreOutletPerformance.Visible = false;
                btnSaveAndAddMoreOutletChecking.Visible = true;
                btnSaveAndAddMoreOutletAlteration.Visible = false;

                OnPoupValidation(3);
                txtRemarks_Outlet_Alteration.Text = string.Empty;
                txtDateOfVisit_Outlet_Alteration.Text = string.Empty;

                if (lstData != null && lstData.Count > 0)
                {
                    List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                    Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                    if (lstDivision.Count > 0)
                    {
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Outlet_Alteration, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        Dropdownlist.SetSelectedValue(ddldivision_Outlet_Alteration, "");
                    }
                    List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                    if (lstSubDivision.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ddldivision_Outlet_Alteration.SelectedItem.Value))
                        {
                            Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Outlet_Alteration, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddlSubDivision_Outlet_Alteration, "");
                        }

                    }

                    if (string.IsNullOrEmpty(ddlSubDivision_Outlet_Alteration.SelectedItem.Value))
                    {
                        Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                    }
                    else
                    {
                        Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Outlet_Alteration, Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlChannelName_Outlet_Alteration, "");
                    }
                }
                else
                {
                    Dropdownlist.DDLDivisionNames(ddldivision_Outlet_Alteration);
                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                    Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StatusDischargeInspection", "$('#addMultiOutLetAlteration').modal();", true);
            }
            #endregion
            #region Edit
            if (e.CommandName == "Edit")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                #region "Datakeys"
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletChecking, "ScheduleDetailID", rowIndex));
                string DivisionID = GetDataKeyValue(gvOutletChecking, "DivisionID", rowIndex);
                string SubDivisionID = GetDataKeyValue(gvOutletChecking, "SubDivisionID", rowIndex);
                string ChannelID = GetDataKeyValue(gvOutletChecking, "ChannelID", rowIndex);
                string OutletID = GetDataKeyValue(gvOutletChecking, "OutletID", rowIndex);
                string DateOfVisit = GetDataKeyValue(gvOutletChecking, "DateOfVisit", rowIndex);
                string Remarks = GetDataKeyValue(gvOutletChecking, "Remarks", rowIndex);
                #endregion
                #region RestControls
                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                txtDateofVist_Common.Text = string.Empty;
                txtRemarks_Common.Text = string.Empty;
                #endregion
                #region Binding Controls
                int select = (int)Constants.DropDownFirstOption.Select;
                int noOption = (int)Constants.DropDownFirstOption.NoOption;
                List<object> lstData = LoadAllRegionDDByUser();
                lbCommonName.Text = "Outlet Name";
                ViewState["InspectionTypeID"] = Convert.ToString((long)Constants.SIInspectionType.OutletChecking);
                if (lstData != null && lstData.Count > 0)
                {
                    List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                    if (lstDivision.Count > 0)
                    {
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddldivision_Common, lstDivision, lstDivision.Count == 1 ? noOption : select);
                        Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);
                    }
                    List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
                    if (lstSubDivision.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
                        {
                            Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision_Common, lstSubDivision, lstSubDivision.Count == 1 ? noOption : select);
                            Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);
                        }
                    }

                    if (ddlChannelName_Common != null)
                    {
                        if (string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);
                        }
                    }
                    if (ddlOutletName_Common != null)
                    {
                        if (string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                        {
                            Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                            Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                        }
                    }
                }
                else
                {
                    Dropdownlist.DDLDivisionNames(ddldivision_Common);
                    Dropdownlist.SetSelectedValue(ddldivision_Common, DivisionID);

                    Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt64(ddldivision_Common.SelectedItem.Value));
                    Dropdownlist.SetSelectedValue(ddlSubDivision_Common, SubDivisionID);

                    Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.SetSelectedValue(ddlChannelName_Common, ChannelID);

                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value)));
                    Dropdownlist.SetSelectedValue(ddlOutletName_Common, OutletID);
                }
                if (txtDateofVist_Common != null)
                    txtDateofVist_Common.Text = DateOfVisit;

                if (txtRemarks_Common != null)
                    txtRemarks_Common.Text = Remarks;
                rowIndex_Common.Value = rowIndex.ToString();
                #endregion
                OnPoupValidation(4);//update popu validation  set for 4
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateRecord", "$('#UpdatePopup').modal();", true);
            }
            #endregion
            #region Delete
            if (e.CommandName == "Delete")
            {
                try
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvOutletChecking, "ScheduleDetailID", rowIndex));

                    bool isRecordExists = new ScheduleInspectionBLL().IsOutletPerformanceInspNotesExist(ScheduleDetailID);
                    if (!isRecordExists)
                    {
                        bool isDeleted = new ScheduleInspectionBLL().DeleteGaugeRecord(ScheduleDetailID);
                        if (isDeleted)
                        {
                            Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                            BindOutletCheckingGrid();
                        }
                        else
                            Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                    }
                    else
                        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                }
                catch (Exception exp)
                {
                    new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
            #endregion
        }

        protected void gvOutletChecking_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        public void BindOutletCheckingGrid()
        {
            try
            {
                GridView gvOutletChecking = (GridView)UpdatePanelSchduleInspection.FindControl("gvOutletChecking");
                gvOutletChecking.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletChecking);
                gvOutletChecking.DataBind();
                UpdatePanelSchduleInspection.Update();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Tenders Monitoring
        public void BindTendersMonitoringGrid()
        {
            try
            {
                gvTendersMonitoring.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForTenderMonitoring(Convert.ToInt64(hdnScheduleID.Value));
                gvTendersMonitoring.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvTendersMonitoring.EditIndex == -1)
                {
                    Button EditTender = (Button)e.Row.FindControl("btnEditTenderMonitoring");
                    Button DeleteTender = (Button)e.Row.FindControl("lbtnDeleteTenderMonitoring");
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                    {
                        EditTender.CssClass = "btn btn-primary btn_24 edit";
                        DeleteTender.CssClass = "btn btn-danger btn_24 delete";
                    }
                    else
                    {
                        EditTender.CssClass = "btn btn-primary btn_24 edit disabled";
                        DeleteTender.CssClass = "btn btn-danger btn_24 delete disabled";
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow && gvTendersMonitoring.EditIndex == e.Row.RowIndex)
                {


                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvTendersMonitoring, "ScheduleDetailID", e.Row.RowIndex));

                    #region "Datakeys"
                    string TenderNoticeID = GetDataKeyValue(gvTendersMonitoring, "TenderNoticeID", e.Row.RowIndex);
                    string TenderWorkID = GetDataKeyValue(gvTendersMonitoring, "TenderWorksID", e.Row.RowIndex);
                    string DivisionID = GetDataKeyValue(gvTendersMonitoring, "DivisionID", e.Row.RowIndex);
                    string OpeningDate = GetDataKeyValue(gvTendersMonitoring, "TenderOpeningDate", e.Row.RowIndex);
                    string Remarks = GetDataKeyValue(gvTendersMonitoring, "Remarks", e.Row.RowIndex);
                    #endregion

                    #region "Controls"
                    DropDownList ddlTenderMonitoringDivision = (DropDownList)e.Row.FindControl("ddlTenderMonitoringDivision");
                    DropDownList ddlTenderMonitoringTenderNotice = (DropDownList)e.Row.FindControl("ddlTenderMonitoringTenderNotice");
                    DropDownList ddlTenderMonitoringWorks = (DropDownList)e.Row.FindControl("ddlTenderMonitoringWorks");


                    TextBox txtTenderOpeningDate = (TextBox)e.Row.FindControl("txtTenderOpeningDate");
                    TextBox txtTenderMonitoringRemarks = (TextBox)e.Row.FindControl("txtTenderMonitoringRemarks");
                    #endregion


                    if (ddlTenderMonitoringDivision != null)
                    {

                        Dropdownlist.DDLDivisionsByUserID(ddlTenderMonitoringDivision, Convert.ToInt64(hdnPreparedByID.Value), Convert.ToInt64(hdnIrrigationLvlId.Value), (int)Constants.DropDownFirstOption.Select);

                        Dropdownlist.SetSelectedValue(ddlTenderMonitoringDivision, DivisionID);
                    }

                    if (ddlTenderMonitoringTenderNotice != null)
                    {
                        if (ddlTenderMonitoringDivision.SelectedItem.Value == "")
                        {
                            Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderMonitoringTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), true);
                        }
                        else
                        {
                            Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderMonitoringTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), false, Convert.ToInt64(ddlTenderMonitoringDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlTenderMonitoringTenderNotice, TenderNoticeID);
                        }

                    }
                    if (ddlTenderMonitoringWorks != null)
                    {
                        if (ddlTenderMonitoringTenderNotice.SelectedItem.Value == "")
                        {
                            Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderMonitoringWorks, true);
                        }
                        else
                        {
                            Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderMonitoringWorks, false, Convert.ToInt64(ddlTenderMonitoringTenderNotice.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlTenderMonitoringWorks, TenderWorkID);
                        }
                    }

                    if (txtTenderOpeningDate != null)
                        txtTenderOpeningDate.Text = OpeningDate;

                    if (txtTenderMonitoringRemarks != null)
                        txtTenderMonitoringRemarks.Text = Remarks;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddTenderMonitoring")
                {
                    List<object> lstTenders = new ScheduleInspectionBLL().GetScheduleDetailForTenderMonitoring(Convert.ToInt64(hdnScheduleID.Value));
                    lstTenders.Insert(0, GetNewTenderDetail());
                    BindNewScheduleDetailGridView(gvTendersMonitoring, lstTenders);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvTendersMonitoring, "ScheduleDetailID", e.RowIndex));
                long TenderWorkID = Convert.ToInt64(GetDataKeyValue(gvTendersMonitoring, "TenderWorksID", e.RowIndex));

                // bool isADMRecordExists = new ScheduleInspectionBLL().IsADMRecordExists(TenderWorkID);
                //  if (!isADMRecordExists)
                //  {
                bool isDeleted = new ScheduleInspectionBLL().DeleteTenderRecord(ScheduleDetailID);
                if (isDeleted)
                {
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindTendersMonitoringGrid();
                }
                else
                    Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                //}
                // else
                //  Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTendersMonitoring.PageIndex = e.NewPageIndex;
                gvTendersMonitoring.EditIndex = -1;
                BindTendersMonitoringGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTendersMonitoring.EditIndex = -1;
                BindTendersMonitoringGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                RemoveAllpopupValidation();
                gvTendersMonitoring.EditIndex = e.NewEditIndex;
                BindTendersMonitoringGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTendersMonitoring_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvTendersMonitoring, "ScheduleDetailID", e.RowIndex));
                GridViewRow row = gvTendersMonitoring.Rows[e.RowIndex];

                #region "Datakeys"
                string TenderNoticeID = GetDataKeyValue(gvTendersMonitoring, "TenderNoticeID", row.RowIndex);
                string TenderWorkID = GetDataKeyValue(gvTendersMonitoring, "TenderWorksID", row.RowIndex);
                string DivisionID = GetDataKeyValue(gvTendersMonitoring, "DivisionID", row.RowIndex);
                string OpeningDate = GetDataKeyValue(gvTendersMonitoring, "TenderOpeningDate", row.RowIndex);
                string Remarks = GetDataKeyValue(gvTendersMonitoring, "Remarks", row.RowIndex);
                #endregion

                #region "Controls"
                DropDownList ddlTenderMonitoringDivision = (DropDownList)row.FindControl("ddlTenderMonitoringDivision");
                DropDownList ddlTenderMonitoringTenderNotice = (DropDownList)row.FindControl("ddlTenderMonitoringTenderNotice");
                DropDownList ddlTenderMonitoringWorks = (DropDownList)row.FindControl("ddlTenderMonitoringWorks");
                TextBox txtTenderOpeningDate = (TextBox)row.FindControl("txtTenderOpeningDate");
                TextBox txtTenderMonitoringRemarks = (TextBox)row.FindControl("txtTenderMonitoringRemarks");
                #endregion



                SI_ScheduleDetailTender ScheduleDetailTender = GetScheduleDetailTenderEntity(ScheduleDetailID, txtTenderMonitoringRemarks.Text);
                ScheduleDetailTender.DivisionID = Convert.ToInt64(ddlTenderMonitoringDivision.SelectedItem.Value);
                ScheduleDetailTender.TenderNoticeID = Convert.ToInt64(ddlTenderMonitoringTenderNotice.SelectedItem.Value);
                ScheduleDetailTender.TenderWorksID = Convert.ToInt64(ddlTenderMonitoringWorks.SelectedItem.Value);
                ScheduleDetailTender.OpeningDate = Convert.ToDateTime(txtTenderOpeningDate.Text);
                ScheduleDetailTender.IsActive = true;

                //bool IsExists = new ScheduleInspectionBLL().IsScheduleDetailTenderExists(Convert.ToInt64(ddlTenderMonitoringTenderNotice.SelectedItem.Value), Convert.ToInt64(ddlTenderMonitoringWorks.SelectedItem.Value), Convert.ToInt64(hdnScheduleID.Value));

                //if (IsExists)
                //{
                //    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                //    return;
                //}
                bool isSaved = new ScheduleInspectionBLL().SaveTenderScheduleDetail(ScheduleDetailTender);

                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ScheduleDetailID) == 0)
                        gvTendersMonitoring.PageIndex = 0;

                    gvTendersMonitoring.EditIndex = -1;
                    BindTendersMonitoringGrid();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }



            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlTenderMonitoringDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTenderNoticesByDivisionID(sender, "ddlTenderMonitoringDivision", "ddlTenderMonitoringTenderNotice", "ddlTenderMonitoringWorks");
        }
        protected void ddlTenderMonitoringTenderNotice_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTenderWorksByTenderNoticeID(sender, "ddlTenderMonitoringTenderNotice", "ddlTenderMonitoringWorks");
        }

        private void GetTenderNoticesByDivisionID(object sender, string _DDLDivisionID, string _DDLTenerMonitoringTenderNoticeID, string _ddlTenderMonitoringWorks)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlDivision = (DropDownList)gvRow.FindControl(_DDLDivisionID);
                DropDownList ddlTenderNotice = (DropDownList)gvRow.FindControl(_DDLTenerMonitoringTenderNoticeID);
                DropDownList ddlTenderWorks = (DropDownList)gvRow.FindControl(_ddlTenderMonitoringWorks);

                if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                {
                    Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), true);
                    Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderWorks, true);

                }
                else
                {
                    long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), false, divisionID, (int)Constants.DropDownFirstOption.Select);


                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetTenderWorksByTenderNoticeID(object sender, string _DDLTenerMonitoringTenderNoticeID, string _ddlTenderMonitoringWorks)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlTenderNotice = (DropDownList)gvRow.FindControl(_DDLTenerMonitoringTenderNoticeID);
                DropDownList ddlTenderWorks = (DropDownList)gvRow.FindControl(_ddlTenderMonitoringWorks);
                TextBox txtOpeningDate = (TextBox)gvRow.FindControl("txtTenderOpeningDate");

                if (string.IsNullOrEmpty(ddlTenderNotice.SelectedItem.Value))
                {
                    Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderWorks, true);

                }
                else
                {
                    long TenderNoticeID = Convert.ToInt64(ddlTenderNotice.SelectedItem.Value);
                    Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderWorks, false, TenderNoticeID, (int)Constants.DropDownFirstOption.Select);
                    DateTime OpeningDate = new ScheduleInspectionBLL().GetOpeningDateByTenderNoticeID(TenderNoticeID);
                    txtOpeningDate.Text = Utility.GetFormattedDate(OpeningDate);

                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Closure Operations
        public void BindClosureOperationsGrid()
        {
            try
            {
                gvClosureOperations.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForClosureOperations(Convert.ToInt64(hdnScheduleID.Value));
                gvClosureOperations.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow && gvTendersMonitoring.EditIndex == -1)
                //{
                //    Button EditTender = (Button)e.Row.FindControl("btnEditTenderMonitoring");
                //    Button DeleteTender = (Button)e.Row.FindControl("lbtnDeleteTenderMonitoring");
                //    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                //    {
                //        EditTender.CssClass = "btn btn-primary btn_24 edit";
                //        DeleteTender.CssClass = "btn btn-danger btn_24 delete";
                //    }
                //    else
                //    {
                //        EditTender.CssClass = "btn btn-primary btn_24 edit disabled";
                //        DeleteTender.CssClass = "btn btn-danger btn_24 delete disabled";
                //    }
                //}
                if (e.Row.RowType == DataControlRowType.DataRow && gvClosureOperations.EditIndex == e.Row.RowIndex)
                {


                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvClosureOperations, "ScheduleDetailID", e.Row.RowIndex));

                    #region "Datakeys"
                    string MonitoringDate = GetDataKeyValue(gvClosureOperations, "MonitoringDate", e.Row.RowIndex);
                    string WorkSourceID = GetDataKeyValue(gvClosureOperations, "WorkSourceID", e.Row.RowIndex);
                    string WorkSource = GetDataKeyValue(gvClosureOperations, "WorkSource", e.Row.RowIndex);
                    string DivisionID = GetDataKeyValue(gvClosureOperations, "DivisionID", e.Row.RowIndex);
                    string Remarks = GetDataKeyValue(gvClosureOperations, "Remarks", e.Row.RowIndex);
                    #endregion

                    #region "Controls"
                    DropDownList ddlClosureOperationsDivision = (DropDownList)e.Row.FindControl("ddlClosureOperationsDivision");
                    DropDownList ddlClosureOperationsWorkSource = (DropDownList)e.Row.FindControl("ddlClosureOperationsWorkSource");
                    DropDownList ddlClosureOperationsWorks = (DropDownList)e.Row.FindControl("ddlClosureOperationsWorks");


                    TextBox txtMonitoringDate = (TextBox)e.Row.FindControl("txtMonitoringDate");
                    TextBox txtClosureOperationsRemarks = (TextBox)e.Row.FindControl("txtClosureOperationsRemarks");
                    #endregion


                    if (ddlClosureOperationsDivision != null)
                    {

                        Dropdownlist.DDLDivisionsByUserID(ddlClosureOperationsDivision, Convert.ToInt64(hdnPreparedByID.Value), Convert.ToInt64(hdnIrrigationLvlId.Value), (int)Constants.DropDownFirstOption.Select);

                        Dropdownlist.SetSelectedValue(ddlClosureOperationsDivision, DivisionID);
                    }

                    if (ddlClosureOperationsWorkSource != null)
                    {
                        if (ddlClosureOperationsDivision.SelectedItem.Value == "")
                        {
                            ddlClosureOperationsWorkSource.DataSource = CommonLists.GetWorkSource();
                            ddlClosureOperationsWorkSource.DataTextField = "Name";
                            ddlClosureOperationsWorkSource.DataValueField = "ID";
                            ddlClosureOperationsWorkSource.DataBind();
                        }
                        else
                        {
                            ddlClosureOperationsWorkSource.DataSource = CommonLists.GetWorkSource();
                            ddlClosureOperationsWorkSource.DataTextField = "Name";
                            ddlClosureOperationsWorkSource.DataValueField = "ID";
                            ddlClosureOperationsWorkSource.DataBind();
                            ddlClosureOperationsWorkSource.Items.FindByText(WorkSource).Selected = true;
                        }

                    }
                    if (ddlClosureOperationsWorks != null)
                    {
                        if (ddlClosureOperationsWorkSource.SelectedItem.Value == "")
                        {
                            Dropdownlist.DDLClosureOperationsWorks(ddlClosureOperationsWorks, true);
                        }
                        else
                        {
                            Dropdownlist.DDLClosureOperationsWorks(ddlClosureOperationsWorks, false, ddlClosureOperationsWorkSource.SelectedItem.Text, Convert.ToInt64(ddlClosureOperationsDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.SetSelectedValue(ddlClosureOperationsWorks, WorkSourceID);
                        }
                    }

                    if (txtMonitoringDate != null)
                        txtMonitoringDate.Text = MonitoringDate;

                    if (txtClosureOperationsRemarks != null)
                        txtClosureOperationsRemarks.Text = Remarks;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddClosureWorks")
                {
                    List<object> lstClosureWorks = new ScheduleInspectionBLL().GetScheduleDetailForClosureOperations(Convert.ToInt64(hdnScheduleID.Value));
                    lstClosureWorks.Insert(0, GetNewClosureDetail());
                    BindNewScheduleDetailGridView(gvClosureOperations, lstClosureWorks);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvClosureOperations, "ScheduleDetailID", e.RowIndex));
                //long TenderWorkID = Convert.ToInt64(GetDataKeyValue(gvTendersMonitoring, "TenderWorksID", e.RowIndex));

                //bool isADMRecordExists = new ScheduleInspectionBLL().IsADMRecordExists(TenderWorkID);
                //if (!isADMRecordExists)
                //{
                bool isDeleted = new ScheduleInspectionBLL().DeleteCLosureWorkRecord(ScheduleDetailID);
                if (isDeleted)
                {
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindClosureOperationsGrid();
                }
                else
                    Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                //}
                // else
                //  Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvClosureOperations.PageIndex = e.NewPageIndex;
                gvClosureOperations.EditIndex = -1;
                BindClosureOperationsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvClosureOperations.EditIndex = -1;
                BindClosureOperationsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                RemoveAllpopupValidation();
                gvClosureOperations.EditIndex = e.NewEditIndex;
                BindClosureOperationsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvClosureOperations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvClosureOperations.Rows[e.RowIndex];
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvClosureOperations, "ScheduleDetailID", e.RowIndex));

                #region "Datakeys"
                string MonitoringDate = GetDataKeyValue(gvClosureOperations, "MonitoringDate", e.RowIndex);
                string WorkSourceID = GetDataKeyValue(gvClosureOperations, "WorkSourceID", e.RowIndex);
                string WorkSource = GetDataKeyValue(gvClosureOperations, "WorkSource", e.RowIndex);
                string DivisionID = GetDataKeyValue(gvClosureOperations, "DivisionID", e.RowIndex);
                string Remarks = GetDataKeyValue(gvClosureOperations, "Remarks", e.RowIndex);
                #endregion

                #region "Controls"
                DropDownList ddlClosureOperationsDivision = (DropDownList)row.FindControl("ddlClosureOperationsDivision");
                DropDownList ddlClosureOperationsWorkSource = (DropDownList)row.FindControl("ddlClosureOperationsWorkSource");
                DropDownList ddlClosureOperationsWorks = (DropDownList)row.FindControl("ddlClosureOperationsWorks");
                TextBox txtMonitoringDate = (TextBox)row.FindControl("txtMonitoringDate");
                TextBox txtClosureOperationsRemarks = (TextBox)row.FindControl("txtClosureOperationsRemarks");
                #endregion

                if (ValidateDateOfVisit(txtMonitoringDate.Text))
                {
                    SI_ScheduleDetailWorks ScheduleDetailWorks = GetScheduleDetailWorksEntity(ScheduleDetailID, txtClosureOperationsRemarks.Text);
                    ScheduleDetailWorks.DivisionID = Convert.ToInt64(ddlClosureOperationsDivision.SelectedItem.Value);
                    ScheduleDetailWorks.WorkSource = Convert.ToString(ddlClosureOperationsWorkSource.SelectedItem.Text);
                    ScheduleDetailWorks.WorkSourceID = Convert.ToInt64(ddlClosureOperationsWorks.SelectedItem.Value);
                    ScheduleDetailWorks.MonitoringDate = Convert.ToDateTime(txtMonitoringDate.Text);
                    ScheduleDetailWorks.IsActive = true;
                    bool IsExists = false;//new ScheduleInspectionBLL().IsScheduleDetailWorkExists(Convert.ToInt64(ddlClosureOperationsWorks.SelectedItem.Value), Convert.ToString(ddlClosureOperationsWorkSource.SelectedItem.Text), Convert.ToInt64(hdnScheduleID.Value));

                    if (IsExists)
                    {
                        Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                    bool isSaved = new ScheduleInspectionBLL().SaveWorkScheduleDetail(ScheduleDetailWorks);

                    if (isSaved)
                    {
                        // Redirect user to first page.
                        if (Convert.ToInt64(ScheduleDetailID) == 0)
                            gvClosureOperations.PageIndex = 0;

                        gvClosureOperations.EditIndex = -1;
                        BindClosureOperationsGrid();
                        Master.ShowMessage(Message.RecordSaved.Description);
                    }
                }
                else
                    Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        //protected void ddlClosureOperationsDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //GetTenderNoticesByDivisionID(sender, "ddlTenderMonitoringDivision", "ddlTenderMonitoringTenderNotice", "ddlTenderMonitoringWorks");
        //}
        protected void ddlClosureOperationsWorkSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetClosureWorksByDivisionIDAndWorkSource(sender, "ddlClosureOperationsDivision", "ddlClosureOperationsWorkSource", "ddlClosureOperationsWorks");
        }

        private void GetClosureByDivisionID(object sender, string _DDLDivisionID, string _DDLTenerMonitoringTenderNoticeID, string _ddlTenderMonitoringWorks)
        {
            try
            {
                //DropDownList ddlControl = (DropDownList)sender;
                //GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                //DropDownList ddlDivision = (DropDownList)gvRow.FindControl(_DDLDivisionID);
                //DropDownList ddlTenderNotice = (DropDownList)gvRow.FindControl(_DDLTenerMonitoringTenderNoticeID);
                //DropDownList ddlTenderWorks = (DropDownList)gvRow.FindControl(_ddlTenderMonitoringWorks);

                //if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                //{
                //    Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), true);
                //    Dropdownlist.DDLTenderMonitoringTenderWorks(ddlTenderWorks, true);

                //}
                //else
                //{
                //    long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                //    Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderNotice, Convert.ToDateTime(hdnFromDate.Value), Convert.ToDateTime(hdnToDate.Value), false, divisionID, (int)Constants.DropDownFirstOption.Select);


                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetClosureWorksByDivisionIDAndWorkSource(object sender, string _ddlClosureOperationsDivision, string _ddlClosureOperationsWorkSource, string _ddlClosureOperationsWorks)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlClosureDivision = (DropDownList)gvRow.FindControl(_ddlClosureOperationsDivision);
                DropDownList ddlWorkSource = (DropDownList)gvRow.FindControl(_ddlClosureOperationsWorkSource);
                DropDownList ddlClosureWork = (DropDownList)gvRow.FindControl(_ddlClosureOperationsWorks);
                TextBox txtOpeningDate = (TextBox)gvRow.FindControl("txtTenderOpeningDate");

                if (string.IsNullOrEmpty(ddlClosureDivision.SelectedItem.Value) || string.IsNullOrEmpty(ddlWorkSource.SelectedItem.Value))
                {
                    Dropdownlist.DDLTenderMonitoringTenderWorks(ddlClosureWork, true);

                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlClosureDivision.SelectedItem.Value);
                    string WorkSource = ddlWorkSource.SelectedItem.Text;
                    if (WorkSource.ToUpper() == "ASSET")
                    {
                        Dropdownlist.DDLTenderMonitoringTenderWorks(ddlClosureWork, true);
                    }
                    else
                    {
                        Dropdownlist.DDLClosureOperationsWorks(ddlClosureWork, false, WorkSource, DivisionID, (int)Constants.DropDownFirstOption.Select);
                    }


                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion

        #region General Inspections
        public void BindGeneralInspectionsGrid()
        {
            try
            {
                gvGeneralInspections.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForGeneralInspections(Convert.ToInt64(hdnScheduleID.Value));
                gvGeneralInspections.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow && gvTendersMonitoring.EditIndex == -1)
                //{
                //    Button EditTender = (Button)e.Row.FindControl("btnEditTenderMonitoring");
                //    Button DeleteTender = (Button)e.Row.FindControl("lbtnDeleteTenderMonitoring");
                //    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                //    {
                //        EditTender.CssClass = "btn btn-primary btn_24 edit";
                //        DeleteTender.CssClass = "btn btn-danger btn_24 delete";
                //    }
                //    else
                //    {
                //        EditTender.CssClass = "btn btn-primary btn_24 edit disabled";
                //        DeleteTender.CssClass = "btn btn-danger btn_24 delete disabled";
                //    }
                //}
                if (e.Row.RowType == DataControlRowType.DataRow && gvGeneralInspections.EditIndex == e.Row.RowIndex)
                {


                    long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGeneralInspections, "ScheduleDetailID", e.Row.RowIndex));

                    #region "Datakeys"
                    string ScheduleID = GetDataKeyValue(gvGeneralInspections, "ScheduleID", e.Row.RowIndex);
                    string GeneralInspectionTypeID = GetDataKeyValue(gvGeneralInspections, "GeneralInspectionTypeID", e.Row.RowIndex);
                    string Location = GetDataKeyValue(gvGeneralInspections, "Location", e.Row.RowIndex);
                    string ScheduleDate = GetDataKeyValue(gvGeneralInspections, "ScheduleDate", e.Row.RowIndex);
                    string Remarks = GetDataKeyValue(gvGeneralInspections, "Remarks", e.Row.RowIndex);
                    #endregion

                    #region "Controls"
                    DropDownList ddlGeneralInspectionType = (DropDownList)e.Row.FindControl("ddlGeneralInspectionType");

                    TextBox txtLocation = (TextBox)e.Row.FindControl("txtLocation");
                    TextBox txtScheduleDate = (TextBox)e.Row.FindControl("txtScheduleDate");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                    #endregion


                    if (ddlGeneralInspectionType != null)
                    {

                        Dropdownlist.DDLGeneralInspectionType(ddlGeneralInspectionType, false, (int)Constants.DropDownFirstOption.Select);

                        Dropdownlist.SetSelectedValue(ddlGeneralInspectionType, GeneralInspectionTypeID);
                    }
                    if (txtLocation != null)
                        txtLocation.Text = Location;
                    if (txtScheduleDate != null)
                        txtScheduleDate.Text = ScheduleDate;
                    if (txtRemarks != null)
                        txtRemarks.Text = Remarks;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddGeneralInspections")
                {
                    List<object> lstGeneralInspections = new ScheduleInspectionBLL().GetScheduleDetailForGeneralInspections(Convert.ToInt64(hdnScheduleID.Value));
                    lstGeneralInspections.Insert(0, GetNewGeneralInspectionDetail());
                    BindNewScheduleDetailGridView(gvGeneralInspections, lstGeneralInspections);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGeneralInspections, "ScheduleDetailID", e.RowIndex));
                bool isExists = new ScheduleInspectionBLL().IsGeneralInspectionAdded(ScheduleDetailID);
                if (!isExists)
                {
                    bool isDeleted = new ScheduleInspectionBLL().DeleteGeneralInspection(ScheduleDetailID);
                    if (isDeleted)
                    {
                        Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                        BindGeneralInspectionsGrid();
                    }
                    else
                        Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvClosureOperations.PageIndex = e.NewPageIndex;
                gvClosureOperations.EditIndex = -1;
                BindClosureOperationsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGeneralInspections.EditIndex = -1;
                BindGeneralInspectionsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                RemoveAllpopupValidation();
                gvGeneralInspections.EditIndex = e.NewEditIndex;
                BindGeneralInspectionsGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGeneralInspections_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvGeneralInspections.Rows[e.RowIndex];
                long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gvGeneralInspections, "ScheduleDetailID", e.RowIndex));

                #region "Datakeys"
                string ScheduleID = GetDataKeyValue(gvGeneralInspections, "ScheduleID", e.RowIndex);
                string GeneralInspectionTypeID = GetDataKeyValue(gvGeneralInspections, "GeneralInspectionTypeID", e.RowIndex);
                string Location = GetDataKeyValue(gvGeneralInspections, "Location", e.RowIndex);
                string ScheduleDate = GetDataKeyValue(gvGeneralInspections, "ScheduleDate", e.RowIndex);
                string Remarks = GetDataKeyValue(gvGeneralInspections, "Remarks", e.RowIndex);
                #endregion

                #region "Controls"
                DropDownList ddlGeneralInspectionType = (DropDownList)row.FindControl("ddlGeneralInspectionType");

                TextBox txtLocation = (TextBox)row.FindControl("txtLocation");
                TextBox txtScheduleDate = (TextBox)row.FindControl("txtScheduleDate");
                TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                #endregion

                if (ValidateDateOfVisit(txtScheduleDate.Text))
                {
                    SI_ScheduleDetailGeneral ScheduleDetailGeneralInspections = GetScheduleDetailGeneralEntity(ScheduleDetailID, txtRemarks.Text);
                    ScheduleDetailGeneralInspections.GeneralInspectionTypeID = Convert.ToInt64(ddlGeneralInspectionType.SelectedItem.Value);
                    ScheduleDetailGeneralInspections.Location = txtLocation.Text;
                    ScheduleDetailGeneralInspections.ScheduleDate = Convert.ToDateTime(txtScheduleDate.Text);

                    //bool IsExists = new ScheduleInspectionBLL().IsScheduleDetailGeneralExists(Convert.ToInt64(hdnScheduleID.Value), Convert.ToInt64(ddlGeneralInspectionType.SelectedItem.Value), txtLocation.Text);

                    //if (IsExists)
                    //{
                    //    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    //    return;
                    //}
                    bool isSaved = new ScheduleInspectionBLL().SaveGeneralScheduleDetail(ScheduleDetailGeneralInspections);

                    if (isSaved)
                    {
                        // Redirect user to first page.
                        if (Convert.ToInt64(ScheduleDetailID) == 0)
                            gvGeneralInspections.PageIndex = 0;

                        gvGeneralInspections.EditIndex = -1;
                        BindGeneralInspectionsGrid();
                        Master.ShowMessage(Message.RecordSaved.Description);
                    }
                }
                else
                    Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }



        #endregion
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        /// 

        #region Other
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ActionOnSchedule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private bool ValidateDateOfVisit(string _DateOfVisit)
        {
            if (Convert.ToDateTime(hdnFromDate.Value) <= Convert.ToDateTime(_DateOfVisit) && Convert.ToDateTime(hdnToDate.Value) >= Convert.ToDateTime(_DateOfVisit))
                return true;
            else
                return false;
        }
        private object GetNewScheduleDetail()
        {
            object scheduleDetail = new
            {
                ScheduleDetailID = 0,
                DivisionName = string.Empty,
                DivisionID = string.Empty,
                SubDivisionName = string.Empty,
                SubDivisionID = string.Empty,
                ChannelName = string.Empty,
                ChannelID = string.Empty,
                InspectionRD = string.Empty,
                GaugeID = string.Empty,
                DateOfVisit = string.Empty,
                Remarks = string.Empty,
                OutletID = string.Empty,
                OutletName = string.Empty,
                CreatedBy = string.Empty,
                CreatedDate = string.Empty
            };
            return scheduleDetail;
        }
        private object GetNewTenderDetail()
        {
            object TenderDetail = new
            {
                ScheduleDetailID = 0,
                DivisionName = string.Empty,
                DivisionID = string.Empty,
                TenderNoticeName = string.Empty,
                TenderNoticeID = string.Empty,
                WorkName = string.Empty,
                TenderOpeningDate = string.Empty,
                TenderWorksID = string.Empty,
                Remarks = string.Empty
            };
            return TenderDetail;
        }
        private object GetNewClosureDetail()
        {
            object ClosureDetail = new
            {
                ScheduleDetailID = 0,
                DivisionName = string.Empty,
                DivisionID = string.Empty,
                WorkSourceID = string.Empty,
                WorkSource = string.Empty,
                WorkName = string.Empty,
                MonitoringDate = string.Empty,
                Remarks = string.Empty
            };
            return ClosureDetail;
        }
        private object GetNewGeneralInspectionDetail()
        {
            object InspectionDetail = new
            {
                ScheduleDetailID = 0,
                ScheduleID = string.Empty,
                GeneralInspectionTypeID = string.Empty,
                Location = string.Empty,
                ScheduleDate = string.Empty,
                GeneralInspectionType = string.Empty,
                Remarks = string.Empty
            };
            return InspectionDetail;
        }
        private SI_ScheduleDetailChannel GetScheduleDetailChannelEntity(long _ScheduleDetailID, string _Remarks)
        {
            SI_ScheduleDetailChannel ScheduleDetailChannel = new SI_ScheduleDetailChannel();
            ScheduleDetailChannel.ID = _ScheduleDetailID;
            ScheduleDetailChannel.ScheduleID = Convert.ToInt64(hdnScheduleID.Value);
            ScheduleDetailChannel.Remarks = _Remarks;
            if (_ScheduleDetailID > 0)
            {
                ScheduleDetailChannel.CreatedBy = !string.IsNullOrEmpty(hdnPreparedByID.Value) ? Convert.ToInt64(hdnPreparedByID.Value) : Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailChannel.CreatedDate = !string.IsNullOrEmpty(hdnCreatedDate.Value) ? Convert.ToDateTime(hdnCreatedDate.Value) : DateTime.Now;
            }
            else
            {
                ScheduleDetailChannel.CreatedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailChannel.CreatedDate = DateTime.Now;
            }
            ScheduleDetailChannel.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
            ScheduleDetailChannel.ModifiedDate = DateTime.Now;
            return ScheduleDetailChannel;
        }
        private SI_ScheduleDetailTender GetScheduleDetailTenderEntity(long _ScheduleDetailID, string _Remarks)
        {
            SI_ScheduleDetailTender ScheduleDetailTender = new SI_ScheduleDetailTender();
            ScheduleDetailTender.ID = _ScheduleDetailID;
            ScheduleDetailTender.ScheduleID = Convert.ToInt64(hdnScheduleID.Value);
            ScheduleDetailTender.Remarks = _Remarks;
            if (_ScheduleDetailID > 0)
            {
                ScheduleDetailTender.CreatedBy = !string.IsNullOrEmpty(hdnPreparedByID.Value) ? Convert.ToInt64(hdnPreparedByID.Value) : Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailTender.CreatedDate = !string.IsNullOrEmpty(hdnCreatedDate.Value) ? Convert.ToDateTime(hdnCreatedDate.Value) : DateTime.Now;
            }
            else
            {
                ScheduleDetailTender.CreatedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailTender.CreatedDate = DateTime.Now;
            }
            ScheduleDetailTender.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
            ScheduleDetailTender.ModifiedDate = DateTime.Now;
            return ScheduleDetailTender;
        }
        private SI_ScheduleDetailWorks GetScheduleDetailWorksEntity(long _ScheduleDetailID, string _Remarks)
        {
            SI_ScheduleDetailWorks ScheduleDetailWorks = new SI_ScheduleDetailWorks();
            ScheduleDetailWorks.ID = _ScheduleDetailID;
            ScheduleDetailWorks.ScheduleID = Convert.ToInt64(hdnScheduleID.Value);
            ScheduleDetailWorks.Remarks = _Remarks;
            if (_ScheduleDetailID > 0)
            {
                ScheduleDetailWorks.CreatedBy = !string.IsNullOrEmpty(hdnPreparedByID.Value) ? Convert.ToInt64(hdnPreparedByID.Value) : Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailWorks.CreatedDate = !string.IsNullOrEmpty(hdnCreatedDate.Value) ? Convert.ToDateTime(hdnCreatedDate.Value) : DateTime.Now;
            }
            else
            {
                ScheduleDetailWorks.CreatedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailWorks.CreatedDate = DateTime.Now;
            }
            ScheduleDetailWorks.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
            ScheduleDetailWorks.ModifiedDate = DateTime.Now;
            return ScheduleDetailWorks;
        }
        private SI_ScheduleDetailGeneral GetScheduleDetailGeneralEntity(long _ScheduleDetailID, string _Remarks)
        {
            SI_ScheduleDetailGeneral ScheduleDetailGeneral = new SI_ScheduleDetailGeneral();
            ScheduleDetailGeneral.ID = _ScheduleDetailID;
            ScheduleDetailGeneral.ScheduleID = Convert.ToInt64(hdnScheduleID.Value);
            ScheduleDetailGeneral.Remarks = _Remarks;
            if (_ScheduleDetailID > 0)
            {
                ScheduleDetailGeneral.CreatedBy = !string.IsNullOrEmpty(hdnPreparedByID.Value) ? Convert.ToInt64(hdnPreparedByID.Value) : Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailGeneral.CreatedDate = !string.IsNullOrEmpty(hdnCreatedDate.Value) ? Convert.ToDateTime(hdnCreatedDate.Value) : DateTime.Now;
            }
            else
            {
                ScheduleDetailGeneral.CreatedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                ScheduleDetailGeneral.CreatedDate = DateTime.Now;
            }
            ScheduleDetailGeneral.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
            ScheduleDetailGeneral.ModifiedDate = DateTime.Now;
            return ScheduleDetailGeneral;
        }
        private void BindNewScheduleDetailGridView(GridView _GridView, List<object> _lstScheduleDetail)
        {
            try
            {
                _GridView = (GridView)UpdatePanelSchduleInspection.FindControl(_GridView.ID);
                _GridView.DataSource = _lstScheduleDetail;
                _GridView.DataBind();
                _GridView.EditIndex = 0;
                _GridView.DataBind();
                //_GridView.PageIndex = _GridView.PageCount;
                //_GridView.DataSource = _lstScheduleDetail;
                //_GridView.DataBind();
                //_GridView.EditIndex = 0;
                //GridView grid3 = (GridView)UpdatePanelSchduleInspection.FindControl(_GridView.ID);
                //grid3.DataBind();


                //_GridView.DataBind();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetChannelsBySubDivID(object sender, string _DDLDivisionID, string _DDLSubDivisionID, string _DDLChannelID, string _DDLInspection)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlDivision = (DropDownList)gvRow.FindControl(_DDLDivisionID);
                DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl(_DDLSubDivisionID);
                DropDownList ddlChannel = (DropDownList)gvRow.FindControl(_DDLChannelID);
                DropDownList ddlInspection = (DropDownList)gvRow.FindControl(_DDLInspection);

                if (string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                {
                    Dropdownlist.DDLStructureChannels(ddlChannel, 0, true);
                    Dropdownlist.DDLStructureChannels(ddlInspection, 0, true);
                }
                else
                {
                    long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                    Dropdownlist.DDLChannelsBySubDivID(ddlChannel, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetChannelOutlets(object sender, string _DDLChannelID, string _DDLInspection)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)DDLControl.NamingContainer;
                DropDownList DDLChannel = gvRow.FindControl(_DDLChannelID) as DropDownList;
                DropDownList DDLInspection = gvRow.FindControl(_DDLInspection) as DropDownList;
                if (string.IsNullOrEmpty(DDLChannel.SelectedItem.Value))
                {
                    Dropdownlist.DDLStructureChannels(DDLInspection, 0, true);
                }
                else
                {
                    List<object> lstInspections = new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(DDLChannel.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(DDLInspection, lstInspections);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetInspectionAreas(object sender, string _DDLChannelID, string _DDLInspectionID)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlChannel = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLChannelID);
                DropDownList ddlInspectionAreas = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLInspectionID);
                if (string.IsNullOrEmpty(ddlChannel.SelectedItem.Value))
                {
                    Dropdownlist.DDLStructureChannels(ddlInspectionAreas, 0, true);
                }
                else
                {
                    List<object> lstInspections = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannel.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlInspectionAreas, lstInspections);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindSubDivisionsByDivisionID(object sender, string _DDLDivisionID, string _DDLSubDivisionID)
        {
            try
            {
                DropDownList ddlDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl(_DDLSubDivisionID);
                    if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                    {
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
                    }
                    else
                    {
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt32(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetScheduleDataInformation(ref long _PreparedByID, ref long _StatusID, ref long _DesignationID)
        {
            try
            {
                dynamic ScheduleData = new ScheduleInspectionBLL().GetScheduleData(Convert.ToInt64(hdnScheduleID.Value));
                ScheduleInspection.Controls.ScheduleDetail.Title = ScheduleData.GetType().GetProperty("Name").GetValue(ScheduleData, null);
                ScheduleInspection.Controls.ScheduleDetail.Status = ScheduleData.GetType().GetProperty("Status").GetValue(ScheduleData, null);
                ScheduleInspection.Controls.ScheduleDetail.PreparedBy = ScheduleData.GetType().GetProperty("PreparedBy").GetValue(ScheduleData, null);
                ScheduleInspection.Controls.ScheduleDetail.FromDate = Utility.GetFormattedDate(ScheduleData.GetType().GetProperty("FromDate").GetValue(ScheduleData, null));
                ScheduleInspection.Controls.ScheduleDetail.ToDate = Utility.GetFormattedDate(ScheduleData.GetType().GetProperty("ToDate").GetValue(ScheduleData, null));
                _PreparedByID = ScheduleData.GetType().GetProperty("PreparedByID").GetValue(ScheduleData, null);
                hdnPreparedByID.Value = Convert.ToString(ScheduleData.GetType().GetProperty("PreparedByID").GetValue(ScheduleData, null));
                hdnCreatedDate.Value = Convert.ToString(ScheduleData.GetType().GetProperty("CreatedDate").GetValue(ScheduleData, null));
                _StatusID = ScheduleData.GetType().GetProperty("StatusID").GetValue(ScheduleData, null);
                _DesignationID = ScheduleData.GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleData, null);
                hdnIrrigationLvlId.Value = Convert.ToString(ScheduleData.GetType().GetProperty("IrrigationLevelId").GetValue(ScheduleData, null));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        public bool IsToDisplayLink()
        {
            bool Flag = false;
            var _StatusID = Convert.ToInt64(hdnStatusID.Value);
            try
            {
                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                {
                    if (_StatusID == (long)Constants.SIScheduleStatus.Approved || _StatusID == (long)Constants.SIScheduleStatus.PendingForApproval || _StatusID == (long)Constants.SIScheduleStatus.Rejected)
                    {
                        Flag = false;
                    }
                    else
                    {
                        if (SessionManagerFacade.UserInformation.ID == Convert.ToInt64(hdnPreparedByID.Value))
                        {
                            Flag = true;
                        }
                        else
                        {
                            Flag = false;
                        }

                    }

                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Flag;
        }

        public bool IsToDisplayLinkGeneral()
        {
            bool Flag = false;
            var _StatusID = Convert.ToInt64(hdnStatusID.Value);
            try
            {
                //if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                //{
                if (_StatusID == (long)Constants.SIScheduleStatus.Approved || _StatusID == (long)Constants.SIScheduleStatus.Rejected)
                {
                    Flag = false;
                }
                else if (_StatusID == (long)Constants.SIScheduleStatus.PendingForApproval && SessionManagerFacade.UserInformation.ID != Convert.ToInt64(hdnPreparedByID.Value))
                {
                    Flag = true;
                }
                else if (_StatusID == (long)Constants.SIScheduleStatus.PendingForApproval && SessionManagerFacade.UserInformation.ID == Convert.ToInt64(hdnPreparedByID.Value))
                {
                    Flag = false;
                }
                else
                {
                    Flag = true;
                    //if (SessionManagerFacade.UserInformation.ID == Convert.ToInt64(hdnPreparedByID.Value))
                    //{
                    //    Flag = true;
                    //}
                    //else
                    //{
                    //    Flag = false;
                    //}

                }

                //}

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Flag;
        }
        public bool GetVisibleValue(bool _RoleRight)
        {
            if (!_RoleRight)
                return false;

            bool ClassVal = true;
            var _StatusID = Convert.ToInt64(hdnStatusID.Value);
            var _PreparedByID = Convert.ToInt64(hdnPreparedByID.Value);
            var _PrepairedByDesignationID = Convert.ToInt64(hdnPreparedByDesignationID.Value);
            try
            {
                bool IsScheduleInspectiosnsExists = new ScheduleInspectionBLL().IsScheduleInspectionsExists(Convert.ToInt64(Request.QueryString["ScheduleID"]));
                if (IsScheduleInspectiosnsExists)
                {
                    ClassVal = false;
                }
                else
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (mdlUser.DesignationID == (long)Constants.Designation.MA || mdlUser.DesignationID == (long)Constants.Designation.SDO)
                    {

                        if (_StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            ClassVal = false;

                        }
                        else if (_StatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            ClassVal = false;
                        }
                        else if (_StatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            ClassVal = false;
                        }
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.ADM || mdlUser.DesignationID == (long)Constants.Designation.XEN)
                    {
                        if (mdlUser.ID == _PreparedByID)
                        {
                            if (_StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                            {

                                ClassVal = false;
                            }
                            else if (_StatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                ClassVal = false;
                            }
                            else if (_StatusID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                ClassVal = false;
                            }
                        }
                        else
                        {
                            if (_StatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                ClassVal = false;
                            }
                            else if (_StatusID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                ClassVal = false;
                            }
                        }




                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirector || mdlUser.DesignationID == (long)Constants.Designation.SE)
                    {
                        if (mdlUser.ID == _PreparedByID)
                        {

                        }
                        else
                        {
                            if (_PrepairedByDesignationID == (long)Constants.Designation.SDO)
                            {
                                ClassVal = false;
                            }
                            else
                            {
                                if (_StatusID == (long)Constants.SIScheduleStatus.Approved)
                                {
                                    ClassVal = false;
                                }
                                else if (_StatusID == (long)Constants.SIScheduleStatus.Rejected)
                                {
                                    ClassVal = false;
                                }

                            }

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return ClassVal;
        }
        #endregion
        #region Gauge Inspection save Multiple Records
        protected void btnSaveChannelInspectionArea_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtGaugeInspectionDateOfVisit_Mul.Text))
            {
                string rightSelectedItems = Request.Form[SelectedInspectino.UniqueID];
                bool isSaved = false;
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtGaugeInspectionRemarks_Mul.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlGaugeInspectionDivision_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlGaugeInspectionChannel_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.GaugeReading;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtGaugeInspectionDateOfVisit_Mul.Text);
                    ScheduleDetailChannel.GaugeID = item;
                    // ScheduleDetailChannel.Remarks = txtGaugeInspectionRemarks_Mul.Text;
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }

                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        RemoveAllpopupValidation();
                        BindGaugeInspectionGridView();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_InspectionArea();", true);
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
                else
                {
                    SelectedInspectino.ClearSelection();
                    ddlGaugeInspectionChannel_Mul_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

        }
        protected void ddlGaugeInspectionDivision_Mul_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlGaugeInspectionDivision_Mul.SelectedItem.Value))
            {
                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Mul, true);
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Mul, false, Convert.ToInt32(ddlGaugeInspectionDivision_Mul.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
            }
            Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Mul, 0, true);
            lstArea.Items.Clear();
            SelectedInspectino.Items.Clear();
        }
        protected void ddlGaugeInspectionSubDivision_Mul_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value))
            {
                long SubDivisionID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value);
                Dropdownlist.DDLChannelsBySubDivID(ddlGaugeInspectionChannel_Mul, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                txtGaugeInspectionDateOfVisit_Mul.CssClass = "form-control required date-picker";
                txtGaugeInspectionDateOfVisit_Mul.Attributes.Add("required", "true");
            }
            else
            {
                Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Mul, 0, true);
            }
            lstArea.Items.Clear();
            SelectedInspectino.Items.Clear();

        }
        protected void ddlGaugeInspectionChannel_Mul_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGaugeInspectionChannel_Mul.SelectedItem.Value))
            {
                List<object> lstInspections = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlGaugeInspectionChannel_Mul.SelectedItem.Value));
                lstArea.DataSource = lstInspections;
                lstArea.DataTextField = "Name";
                lstArea.DataValueField = "ID";
                lstArea.DataBind();
            }
            else
            {
                lstArea.Items.Clear();
                SelectedInspectino.Items.Clear();
            }

        }

        #endregion
        #region Discharge Table Calculation save Mulitple Records
        protected void ddlGaugeInspectionDivision_Dis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlGaugeInspectionDivision_Dis.SelectedItem.Value))
            {
                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Dis, true);
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlGaugeInspectionSubDivision_Dis, false, Convert.ToInt32(ddlGaugeInspectionDivision_Dis.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
            }
            Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Dis, 0, true);
            right_lst_discharge.Items.Clear();
            left_list_dis.Items.Clear();
        }
        protected void ddlGaugeInspectionSubDivision_Dis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value))
            {
                long SubDivisionID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value);
                Dropdownlist.DDLChannelsBySubDivID(ddlGaugeInspectionChannel_Dis, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                txtGaugeInspectionDateOfVisit_Dis.CssClass = "form-control required date-picker";
                txtGaugeInspectionDateOfVisit_Dis.Attributes.Add("required", "true");
                left_list_dis.Items.Clear();
                right_lst_discharge.Items.Clear();
            }
            else
            {
                Dropdownlist.DDLStructureChannels(ddlGaugeInspectionChannel_Dis, 0, true);
                left_list_dis.Items.Clear();
                right_lst_discharge.Items.Clear();
            }
        }
        protected void ddlGaugeInspectionChannel_Dis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGaugeInspectionChannel_Dis.SelectedItem.Value))
            {
                List<object> lstInspections = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlGaugeInspectionChannel_Dis.SelectedItem.Value));
                left_list_dis.DataSource = lstInspections;
                left_list_dis.DataTextField = "Name";
                left_list_dis.DataValueField = "ID";
                left_list_dis.DataBind();
            }
            else
            {
                left_list_dis.Items.Clear();
                right_lst_discharge.Items.Clear();
            }

        }
        protected void btnSaveDischargeCalculation_Click(object sender, EventArgs e)
        {

            if (ValidateDateOfVisit(txtGaugeInspectionDateOfVisit_Dis.Text))
            {

                string rightSelectedItems = Request.Form[right_lst_discharge.UniqueID];
                bool isSaved = false;

                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtGaugeInspectionRemarks_Dis.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlGaugeInspectionDivision_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlGaugeInspectionChannel_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.GaugeID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.DischargeTableCalculation;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtGaugeInspectionDateOfVisit_Dis.Text);
                    //ScheduleDetailChannel.Remarks = txtGaugeInspectionRemarks_Dis.Text;
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                    //isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        RemoveAllpopupValidation();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_DischargeCalculation();", true);
                        BindDischargeTableInspectionGridView();
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
                else
                {
                    right_lst_discharge.ClearSelection();
                    ddlGaugeInspectionChannel_Dis_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }
        #endregion
        #region Outlet Alteration, Outlet Performance and OutletChecking save Multiple Records
        protected void ddldivision_Outlet_Alteration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddldivision_Outlet_Alteration.SelectedItem.Value))
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, true);
                Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision_Outlet_Alteration, false, Convert.ToInt32(ddldivision_Outlet_Alteration.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
            }
            Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
            lstBox_right_Outlet_Alteration.Items.Clear();
            lstBox_Left_Outlet_Alteration.Items.Clear();
        }

        protected void ddlSubDivision_Outlet_Alteration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSubDivision_Outlet_Alteration.SelectedItem.Value))
            {

                long SubDivisionID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Outlet_Alteration, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                txtDateOfVisit_Outlet_Alteration.CssClass = "form-control required date-picker";
                txtDateOfVisit_Outlet_Alteration.Attributes.Add("required", "true");
            }
            else
            {
                Dropdownlist.DDLStructureChannels(ddlChannelName_Outlet_Alteration, 0, true);
            }
            lstBox_Left_Outlet_Alteration.Items.Clear();
            lstBox_right_Outlet_Alteration.Items.Clear();
        }

        protected void ddlChannelName_Outlet_Alteration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlChannelName_Outlet_Alteration.SelectedItem.Value))
            {
                List<object> lstInspections = new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value));
                lstBox_Left_Outlet_Alteration.DataSource = lstInspections;
                lstBox_Left_Outlet_Alteration.DataTextField = "Name";
                lstBox_Left_Outlet_Alteration.DataValueField = "ID";
                lstBox_Left_Outlet_Alteration.DataBind();
            }
            else
            {
                lstBox_Left_Outlet_Alteration.Items.Clear();
                lstBox_right_Outlet_Alteration.Items.Clear();
            }

        }

        protected void btnSaveOutletAlteration_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {
                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletAlteration;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    //ScheduleDetailChannel.Remarks = txtRemarks_Outlet_Alteration.Text;
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {

                        RemoveAllpopupValidation();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_Outlet_PerAlter();", true);
                        BindOutletAltrationGrid();
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        //RemoveAllpopupValidation();
                        ////if (string.IsNullOrEmpty(Request.QueryString["isSaved"]))
                        ////    Response.Redirect(Request.RawUrl + "&isSaved=ture");
                        ////else
                        ////{
                        ////    Response.Redirect(Request.RawUrl);
                        ////}

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_Outlet_PerAlter();", true);
                        //BindOutletAltrationGrid();
                        //UpdatePanelSchduleInspection.Update();
                        //GridView grid3 = (GridView)UpdatePanelSchduleInspection.FindControl("gvOutletInspection");
                        //grid3.DataBind();
                        //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }


        protected void btnSaveOutletPerformance_Click(object sender, EventArgs e)
        {

            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {


                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletPerformance;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        RemoveAllpopupValidation();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_Outlet_PerAlter();", true);
                        BindOutletPerformanceGrid();
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

        }

        protected void btnSaveOutletChecking_Click(object sender, EventArgs e)
        {

            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {


                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletChecking;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        RemoveAllpopupValidation();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_Outlet_PerAlter();", true);
                        BindOutletCheckingGrid();
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

        }
        #endregion
        #region Validation

        #region Remove Validation
        public void RemoveAllpopupValidation()
        {
            rDischCalcu();
            rGaugeInspec();
            rOutletAlter_Performance();
            rUpdatedPoup();
        }
        #endregion
        protected void OnPoupValidation(int PoupID)
        {
            switch (PoupID)
            {
                case 1:
                    oGaugeInspec();
                    rDischCalcu();
                    rOutletAlter_Performance();
                    rUpdatedPoup();
                    break;
                case 2:
                    rGaugeInspec();
                    oDischCalcu();
                    rOutletAlter_Performance();
                    rUpdatedPoup();
                    break;
                case 3:
                    rGaugeInspec();
                    rDischCalcu();
                    oOutletAlter_Performance();
                    rUpdatedPoup();
                    break;
                case 4:
                    rGaugeInspec();
                    rDischCalcu();
                    rOutletAlter_Performance();
                    oUpdatedPoup();
                    break;
                default:
                    break;
            }
        }
        #region Remove Validation
        protected void rGaugeInspec()
        {
            #region GaugeIncpection
            ddlGaugeInspectionDivision_Mul.CssClass = "";
            ddlGaugeInspectionDivision_Mul.Attributes.Remove("required");
            ddlGaugeInspectionSubDivision_Mul.CssClass = "form-control";
            ddlGaugeInspectionSubDivision_Mul.Attributes.Remove("required");
            ddlGaugeInspectionChannel_Mul.CssClass = "form-control";
            ddlGaugeInspectionChannel_Mul.Attributes.Remove("required");
            txtGaugeInspectionDateOfVisit_Mul.CssClass = "form-control date-picker";
            txtGaugeInspectionDateOfVisit_Mul.Attributes.Remove("required");
            SelectedInspectino.CssClass = "form-control";
            SelectedInspectino.Attributes.Remove("required");
            #endregion
        }
        protected void rDischCalcu()
        {
            #region Discharge calculation
            ddlGaugeInspectionDivision_Dis.CssClass = "form-control";
            ddlGaugeInspectionDivision_Dis.Attributes.Remove("required");
            ddlGaugeInspectionSubDivision_Dis.CssClass = "form-control";
            ddlGaugeInspectionSubDivision_Dis.Attributes.Remove("required");
            ddlGaugeInspectionChannel_Dis.CssClass = "form-control";
            ddlGaugeInspectionChannel_Dis.Attributes.Remove("required");
            txtGaugeInspectionDateOfVisit_Dis.CssClass = "form-control required date-picker";
            txtGaugeInspectionDateOfVisit_Dis.Attributes.Remove("required");
            right_lst_discharge.CssClass = "form-control";
            right_lst_discharge.Attributes.Remove("required");
            #endregion
        }
        protected void rOutletAlter_Performance()
        {
            #region Outlet Alteration and Performance
            ddldivision_Outlet_Alteration.CssClass = "form-control";
            ddldivision_Outlet_Alteration.Attributes.Remove("required");
            ddlSubDivision_Outlet_Alteration.CssClass = "form-control";
            ddlSubDivision_Outlet_Alteration.Attributes.Remove("required");
            ddlChannelName_Outlet_Alteration.CssClass = "form-control";
            ddlChannelName_Outlet_Alteration.Attributes.Remove("required");
            txtDateOfVisit_Outlet_Alteration.CssClass = "form-control";
            txtDateOfVisit_Outlet_Alteration.Attributes.Remove("required");
            lstBox_right_Outlet_Alteration.Attributes.Remove("required");
            #endregion
        }
        protected void rUpdatedPoup()
        {
            #region Outlet Alteration and Performance
            ddldivision_Common.CssClass = "form-control";
            ddldivision_Common.Attributes.Remove("required");
            ddlSubDivision_Common.CssClass = "form-control";
            ddlSubDivision_Common.Attributes.Remove("required");
            ddlChannelName_Common.CssClass = "form-control";
            ddlChannelName_Common.Attributes.Remove("required");
            txtDateofVist_Common.CssClass = "form-control";
            txtDateofVist_Common.Attributes.Remove("required");
            ddlOutletName_Common.CssClass = "form-control";
            ddlOutletName_Common.Attributes.Remove("required");
            #endregion
        }
        #endregion

        #region OnValidation
        protected void oGaugeInspec()
        {
            #region Validation GaugeInspec
            ddlGaugeInspectionDivision_Mul.CssClass = "form-control required";
            ddlGaugeInspectionDivision_Mul.Attributes.Add("required", "true");
            ddlGaugeInspectionSubDivision_Mul.CssClass = "form-control required";
            ddlGaugeInspectionSubDivision_Mul.Attributes.Add("required", "true");
            ddlGaugeInspectionChannel_Mul.CssClass = "form-control required";
            ddlGaugeInspectionChannel_Mul.Attributes.Add("required", "true");
            txtGaugeInspectionDateOfVisit_Mul.CssClass = "form-control required date-picker";
            txtGaugeInspectionDateOfVisit_Mul.Attributes.Add("required", "true");
            SelectedInspectino.CssClass = "form-control required";
            SelectedInspectino.Attributes.Add("required", "true");
            #endregion
        }
        protected void oDischCalcu()
        {
            #region Validation DischCalcu
            ddlGaugeInspectionDivision_Dis.CssClass = "form-control required";
            ddlGaugeInspectionDivision_Dis.Attributes.Add("required", "true");

            ddlGaugeInspectionSubDivision_Dis.CssClass = "form-control required";
            ddlGaugeInspectionSubDivision_Dis.Attributes.Add("required", "true");

            ddlGaugeInspectionChannel_Dis.CssClass = "form-control required";
            ddlGaugeInspectionChannel_Dis.Attributes.Add("required", "true");

            txtGaugeInspectionDateOfVisit_Dis.CssClass = "form-control required date-picker";
            txtGaugeInspectionDateOfVisit_Dis.Attributes.Add("required", "true");

            right_lst_discharge.CssClass = "form-control required";
            right_lst_discharge.Attributes.Add("required", "true");
            #endregion
        }
        protected void oOutletAlter_Performance()
        {
            #region Validation outlet alteration
            ddldivision_Outlet_Alteration.CssClass = "form-control required";
            ddldivision_Outlet_Alteration.Attributes.Add("required", "true");
            ddlSubDivision_Outlet_Alteration.CssClass = "form-control required";
            ddlSubDivision_Outlet_Alteration.Attributes.Add("required", "true");
            ddlChannelName_Outlet_Alteration.CssClass = "form-control required";
            ddlChannelName_Outlet_Alteration.Attributes.Add("required", "true");
            txtDateOfVisit_Outlet_Alteration.CssClass = "form-control required date-picker";
            txtDateOfVisit_Outlet_Alteration.Attributes.Add("required", "true");
            lstBox_right_Outlet_Alteration.Attributes.Add("required", "true");
            #endregion
        }
        protected void oUpdatedPoup()
        {
            ddldivision_Common.CssClass = "form-control required";
            ddldivision_Common.Attributes.Add("required", "true");
            ddlSubDivision_Common.CssClass = "form-control required";
            ddlSubDivision_Common.Attributes.Add("required", "true");
            ddlChannelName_Common.CssClass = "form-control required";
            ddlChannelName_Common.Attributes.Add("required", "true");
            txtDateofVist_Common.CssClass = "form-control required date-picker";
            txtDateofVist_Common.Attributes.Add("required", "true");
            ddlOutletName_Common.Attributes.Add("required", "true");
            ddlOutletName_Common.CssClass = "form-control required";
        }
        #endregion
        #endregion

        #region Gauge Inspection, Gauge Discharge, Outlet Alteration, Outlet Performance and OutletChecking Update Records
        protected void ddldivision_Common_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddldivision_Common.SelectedItem.Value))
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, true);
                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision_Common, false, Convert.ToInt32(ddldivision_Common.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
            }
        }
        protected void ddlSubDivision_Common_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSubDivision_Common.SelectedItem.Value))
            {

                long SubDivisionID = Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value);
                Dropdownlist.DDLChannelsBySubDivID(ddlChannelName_Common, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                txtDateofVist_Common.CssClass = "form-control required date-picker";
                txtDateofVist_Common.Attributes.Add("required", "true");
            }
            else
            {
                Dropdownlist.DDLStructureChannels(ddlChannelName_Common, 0, true);
                Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);
            }
        }
        protected void ddlChannelName_Common_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCommonName.Text == "Inspection Areas")
            {
                if (!string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                {
                    List<object> lstInspections = new ScheduleInspectionBLL().GetGaugeInspectionArea(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, lstInspections);
                }
                else
                {
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlChannelName_Common.SelectedItem.Value))
                {
                    List<object> lstOutlets = new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutletName_Common, lstOutlets);
                }
                else
                {
                    Dropdownlist.DDLStructureChannels(ddlOutletName_Common, 0, true);

                }
            }

        }
        protected void btnUpdate_Common_Click(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(rowIndex_Common.Value);
            // e.Row.RowIndex = rowIndex;
            #region "Datakeys"
            long InspectionTypeID = (long)Convert.ToInt64(ViewState["InspectionTypeID"]);
            GridView gv = InspectionTypeID == 1 ? gvGaugeInspection : InspectionTypeID == 2 ? gvDischargeInspection : InspectionTypeID == 3 ? gvOutletPerformance : InspectionTypeID == 4 ? gvOutletInspection : gvOutletChecking;
            string CommonString = (InspectionTypeID == 1 || InspectionTypeID == 2) == true ? "GaugeID" : (InspectionTypeID == 3 || InspectionTypeID == 4 || InspectionTypeID == 5) == true ? "OutletID" : "";
            long ScheduleDetailID = Convert.ToInt64(GetDataKeyValue(gv, "ScheduleDetailID", rowIndex));
            // string DivisionID = GetDataKeyValue(gvOutletInspection, "DivisionID", rowIndex);
            // string SubDivisionID = GetDataKeyValue(gvOutletInspection, "SubDivisionID", rowIndex);
            //string ChannelID = GetDataKeyValue(gvOutletInspection, "ChannelID", rowIndex);
            //string OutletID = GetDataKeyValue(gvOutletInspection, CommonString, rowIndex);
            //string DateOfVisit = GetDataKeyValue(gvOutletInspection, "DateOfVisit", rowIndex);
            #endregion
            if (ValidateDateOfVisit(txtDateofVist_Common.Text))
            {
                SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(ScheduleDetailID, txtRemarks_Common.Text);
                ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Common.SelectedItem.Value);
                ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Common.SelectedItem.Value);
                ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Common.SelectedItem.Value);
                if (InspectionTypeID == 1 || InspectionTypeID == 2)
                {
                    ScheduleDetailChannel.GaugeID = Convert.ToInt64(ddlOutletName_Common.SelectedItem.Value);
                }
                else
                {
                    ScheduleDetailChannel.OutletID = Convert.ToInt64(ddlOutletName_Common.SelectedItem.Value);
                }

                ScheduleDetailChannel.InspectionTypeID = InspectionTypeID;//(long)Constants.SIInspectionType.GaugeReading;
                ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateofVist_Common.Text);



                bool isSaved = false;
                if (ScheduleDetailChannel.ID > 0)
                {
                    if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
                    {
                        isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "closeModal_Update();", true);
                        if (InspectionTypeID == (long)(Constants.SIInspectionType.GaugeReading))
                        {
                            BindGaugeInspectionGridView();
                        }
                        else if (InspectionTypeID == (long)(Constants.SIInspectionType.DischargeTableCalculation))
                        {
                            BindDischargeTableInspectionGridView();
                        }
                        else if (InspectionTypeID == (long)(Constants.SIInspectionType.OutletAlteration))
                        {
                            BindOutletAltrationGrid();
                        }
                        else if (InspectionTypeID == (long)(Constants.SIInspectionType.OutletPerformance))
                        {
                            BindOutletPerformanceGrid();
                        }
                        else
                        {
                            BindOutletCheckingGrid();
                        }

                        Master.ShowMessage(Message.RecordSaved.Description);
                    }
                    else
                    {
                        Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                    }
                }
                //else
                //{
                //    if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ScheduleDetailChannel))
                //    {
                //        isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(ScheduleDetailChannel);
                //        if (isSaved)
                //        {
                //            gvOutletInspection.EditIndex = -1;
                //            BindOutletAltrationGrid();
                //            Master.ShowMessage(Message.RecordSaved.Description);
                //        }
                //    }
                //    else
                //    {
                //        Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                //    }
                //}


            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

        }
        #endregion

        #region Gauge Inspection, Gauge Discharge, Outlet Alteration, Outlet Performance and OutletChecking Save and Add More
        protected void btnSaveAndAddMoreInspectionArea_Click(object sender, EventArgs e)
        {

            if (ValidateDateOfVisit(txtGaugeInspectionDateOfVisit_Mul.Text))
            {
                string rightSelectedItems = Request.Form[SelectedInspectino.UniqueID];
                bool isSaved = false;
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtGaugeInspectionRemarks_Mul.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlGaugeInspectionDivision_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlGaugeInspectionChannel_Mul.SelectedItem.Value);
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.GaugeReading;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtGaugeInspectionDateOfVisit_Mul.Text);
                    ScheduleDetailChannel.GaugeID = item;
                    // ScheduleDetailChannel.Remarks = txtGaugeInspectionRemarks_Mul.Text;
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }

                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {

                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        ddlGaugeInspectionSubDivision_Mul_SelectedIndexChanged(null, null);
                        lstArea.Items.Clear();
                        SelectedInspectino.Items.Clear();
                        BindGaugeInspectionGridView();
                    }
                }
                else
                {
                    lstArea.Items.Clear();
                    SelectedInspectino.Items.Clear();
                    ddlGaugeInspectionChannel_Mul_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);

        }

        protected void btnSaveAndAddMoreDischargeCalculation_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtGaugeInspectionDateOfVisit_Dis.Text))
            {

                string rightSelectedItems = Request.Form[right_lst_discharge.UniqueID];
                bool isSaved = false;

                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtGaugeInspectionRemarks_Dis.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddlGaugeInspectionDivision_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlGaugeInspectionSubDivision_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlGaugeInspectionChannel_Dis.SelectedItem.Value);
                    ScheduleDetailChannel.GaugeID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.DischargeTableCalculation;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtGaugeInspectionDateOfVisit_Dis.Text);
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        ddlGaugeInspectionSubDivision_Dis_SelectedIndexChanged(null, null);
                        right_lst_discharge.Items.Clear();
                        left_list_dis.Items.Clear();
                        BindDischargeTableInspectionGridView();
                    }
                }
                else
                {
                    right_lst_discharge.ClearSelection();
                    ddlGaugeInspectionChannel_Dis_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }

        protected void btnSaveAndAddMoreOutletPerformance_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {


                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletPerformance;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    //ScheduleDetailChannel.Remarks = txtRemarks_Outlet_Alteration.Text;
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        lstBox_right_Outlet_Alteration.Items.Clear();
                        lstBox_Left_Outlet_Alteration.Items.Clear();
                        ddlSubDivision_Outlet_Alteration_SelectedIndexChanged(null, null);
                        BindOutletPerformanceGrid();
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }

        protected void btnSaveAndAddMoreOutletAlteration_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {
                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletAlteration;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        lstBox_right_Outlet_Alteration.Items.Clear();
                        lstBox_Left_Outlet_Alteration.Items.Clear();
                        ddlSubDivision_Outlet_Alteration_SelectedIndexChanged(null, null);
                        BindOutletAltrationGrid();
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }

        protected void btnSaveAndAddMoreOutletChecking_Click(object sender, EventArgs e)
        {
            if (ValidateDateOfVisit(txtDateOfVisit_Outlet_Alteration.Text))
            {
                string rightSelectedItems = Request.Form[lstBox_right_Outlet_Alteration.UniqueID];
                List<long> lstSelectedInspection = new List<long>();
                SelectedInspectino.Items.Clear();
                if (!string.IsNullOrEmpty(rightSelectedItems))
                {
                    foreach (string item in rightSelectedItems.Split(','))
                    {
                        lstSelectedInspection.Add(Convert.ToInt64(item));
                    }
                }
                bool isSaved = false;
                List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = new List<SI_ScheduleDetailChannel>();
                foreach (long item in lstSelectedInspection)
                {
                    SI_ScheduleDetailChannel ScheduleDetailChannel = GetScheduleDetailChannelEntity(0, txtRemarks_Outlet_Alteration.Text);
                    ScheduleDetailChannel.DivisionID = Convert.ToInt64(ddldivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.SubDivID = Convert.ToInt64(ddlSubDivision_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannelName_Outlet_Alteration.SelectedItem.Value);
                    ScheduleDetailChannel.OutletID = item;
                    ScheduleDetailChannel.InspectionTypeID = (long)Constants.SIInspectionType.OutletChecking;
                    ScheduleDetailChannel.ScheduleDate = Convert.ToDateTime(txtDateOfVisit_Outlet_Alteration.Text);
                    lstScheduleDetailChannel.Add(ScheduleDetailChannel);
                }
                if (!new ScheduleInspectionBLL().IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel))
                {
                    isSaved = new ScheduleInspectionBLL().SaveGaugeInspectionScheduleDetail(lstScheduleDetailChannel);
                    if (isSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        lstBox_right_Outlet_Alteration.Items.Clear();
                        lstBox_Left_Outlet_Alteration.Items.Clear();
                        ddlSubDivision_Outlet_Alteration_SelectedIndexChanged(null, null);
                        BindOutletCheckingGrid();
                    }
                }
                else
                {
                    lstBox_right_Outlet_Alteration.ClearSelection();
                    ddlChannelName_Outlet_Alteration_SelectedIndexChanged(null, null);
                    Master.ShowMessage(Message.DuplicateRecord.Description, SiteMaster.MessageType.Error);
                }

            }
            else
                Master.ShowMessage(Message.VisitNotInScheduledDates.Description, SiteMaster.MessageType.Error);
        }
        #endregion
    }
}