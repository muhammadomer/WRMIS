using Microsoft.Reporting.WebForms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.RotaionalProgram;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class SearchRotationalProgram : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();
                if (SessionManagerFacade.UserAssociatedLocations != null)
                {
                    if (Request.QueryString["Msg"] != null)
                        Master.ShowMessage(Message.SendToSEForApproval.Description, SiteMaster.MessageType.Success);

                    if (Request.QueryString["MsgApprove"] != null)
                        Master.ShowMessage(Message.DraftApproved.Description, SiteMaster.MessageType.Success);

                    if (Request.QueryString["MsgSendBack"] != null)
                        Master.ShowMessage(Message.DraftSendBack.Description, SiteMaster.MessageType.Success);

                    //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                    //ddlSeason.DataTextField = "Name";
                    //ddlSeason.DataValueField = "ID";
                    //ddlSeason.DataBind();
                    Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());

                    ddlYear.DataSource = new RotationalProgramBLL().GetYears();
                    ddlYear.DataTextField = "Year";
                    ddlYear.DataValueField = "Year";
                    ddlYear.DataBind();
                    ddlYear.Items.Insert(0, new ListItem("Select", ""));

                    if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.Circle)
                        SELoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));
                    else if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.Division)
                        XENLoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));
                    else if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.SubDivision)
                        SDOLoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));

                    LoadHistory();
                }
            }
        }

        public void SELoggedUser(long _CircleID)
        {
            try
            {
                ddlCircleName.DataSource = new List<CO_Circle> { new CircleBLL().GetByID(_CircleID) };
                ddlCircleName.DataValueField = "ID";
                ddlCircleName.DataTextField = "Name";
                ddlCircleName.DataBind();

                Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID, -1, 1);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void XENLoggedUser(long _DivisionID)
        {
            try
            {
                ddlDivision.DataSource = new List<CO_Division> { new DivisionBLL().GetByID(_DivisionID) };
                ddlDivision.DataValueField = "ID";
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataBind();

                long CircleID = new CircleBLL().GetCircleIDByDivisionID(_DivisionID);
                ddlCircleName.DataSource = new List<CO_Circle> { new CircleBLL().GetByID(CircleID) };
                ddlCircleName.DataValueField = "ID";
                ddlCircleName.DataTextField = "Name";
                ddlCircleName.DataBind();

                ddlSubDivision.DataSource = new SubDivisionBLL().GetSubDivisionsByDivisionID(_DivisionID);
                ddlSubDivision.DataTextField = "Name";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SDOLoggedUser(long _SubDivisionID)
        {
            try
            {
                ddlSubDivision.DataSource = new List<CO_SubDivision> { new SubDivisionBLL().GetByID(_SubDivisionID) };
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataTextField = "Name";
                ddlSubDivision.DataBind();

                long DivisionID = new DivisionBLL().GetDivisionIDBySubDivisionID(_SubDivisionID);
                ddlDivision.DataSource = new List<CO_Division> { new DivisionBLL().GetByID(DivisionID) };
                ddlDivision.DataValueField = "ID";
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataBind();

                long CircleID = new CircleBLL().GetCircleIDByDivisionID(DivisionID);
                ddlCircleName.DataSource = new List<CO_Circle> { new CircleBLL().GetByID(CircleID) };
                ddlCircleName.DataValueField = "ID";
                ddlCircleName.DataTextField = "Name";
                ddlCircleName.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadHistory()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["LoadHistory"]))
                {
                    ddlCircleName.SelectedValue = Convert.ToString(Session["Circle"]);
                    ddlDivision.SelectedValue = Convert.ToString(Session["Division"]);
                    ddlSeason.SelectedValue = Convert.ToString(Session["Season"]);
                    ddlYear.SelectedValue = Convert.ToString(Session["Year"]);

                    if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.SubDivision)
                    {
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value));
                        ddlSubDivision.Enabled = true;
                        if (Session["SubDivision"] != null)
                            ddlSubDivision.SelectedValue = Convert.ToString(Session["SubDivision"]);
                    }
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRotationalProg.Visible = false;
                if (ddlDivision.SelectedItem.Value != "")
                {
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value));
                    ddlSubDivision.Enabled = true;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRotationalProg.Visible = false;
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
                gvRotationalProg.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
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
                long CircleID = -1;
                long DivisionID = -1;
                long SubDivisionID = -1;
                string Year = "";
                long SeasonID = -1;

                if (ddlCircleName.SelectedItem.Value != "")
                    CircleID = Convert.ToInt64(ddlCircleName.SelectedItem.Value);
                if (ddlDivision.SelectedItem != null && ddlDivision.SelectedItem.Value != "")
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                if (ddlSubDivision.SelectedItem != null && ddlSubDivision.SelectedItem.Value != "")
                    SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                if (ddlYear.SelectedItem != null && ddlYear.SelectedItem.Value != "")
                    Year = ddlYear.SelectedItem.Value.ToString();
                if (ddlSeason.SelectedItem != null && ddlSeason.SelectedItem.Value != "")
                    SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);

                gvRotationalProg.DataSource = new RotationalProgramBLL().SearchRotationalPlan(CircleID, DivisionID, SubDivisionID, Year, SeasonID, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID));
                gvRotationalProg.DataBind();
                gvRotationalProg.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvRotationalProg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                long RPID = Convert.ToInt64(((Label)gvRow.FindControl("lblID")).Text);
                long IrrigationLevelID = new RotationalProgramBLL().GetIrrigationLevelIDByRPID(RPID);
                hfRPID.Value = RPID.ToString();
                if (e.CommandName == "Delete")
                {
                    long Result = new RotationalProgramBLL().DeleteProgram(RPID);
                    if (Result == 1)
                    {
                        BindGrid();
                        Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    }
                    else if (Result == 0)
                        Master.ShowMessage(Message.ApprovedDraftCannotBeDeleted.Description, SiteMaster.MessageType.Error);
                    else
                        Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }
                else if (e.CommandName == "Approve")
                {
                    long Result = new RotationalProgramBLL().ApproveDraft(RPID);
                    if (Result != -1)
                    {
                        BindGrid();
                        if (Result == 1)
                            Master.ShowMessage(Message.DraftApproved.Description, SiteMaster.MessageType.Success);
                        else
                            Master.ShowMessage(Message.DraftUnApproved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else if (e.CommandName == "NavigationEdit")
                {
                    if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.Circle)
                        Response.RedirectPermanent(String.Format("AddPlanDetail.aspx?ID={0}", RPID));
                    else
                        Response.RedirectPermanent(String.Format("AddPlanDivSubDivDetail.aspx?ID={0}", RPID));
                }
                else if (e.CommandName == "NavigationView")
                {
                    Session["Circle"] = ddlCircleName.SelectedItem.Value;
                    if (ddlDivision.SelectedItem != null)
                        Session["Division"] = ddlDivision.SelectedItem.Value;
                    if (ddlSubDivision.SelectedItem != null)
                        Session["SubDivision"] = ddlSubDivision.SelectedItem.Value;
                    Session["Season"] = ddlSeason.SelectedItem.Value;
                    Session["Year"] = ddlYear.SelectedItem.Value;

                    if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Division || IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        Response.RedirectPermanent(String.Format("ViewRotationalProgramDivisionSubDivision.aspx?RPID={0}", RPID), false);
                    else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        Response.RedirectPermanent(String.Format("ViewRotationalProgramCircle.aspx?RPID={0}", RPID), false);
                }
                else if (e.CommandName == "CalculateGini")
                {
                    bool Result = new RotationalProgramBLL().GetGini(RPID);
                    if (!Result)
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    else
                        BindGrid();
                }
                else if (e.CommandName == "CalculatePlanImplementation")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
                    List<List<dynamic>> lstReturnDates = new RotationalProgramBLL().GetWaraDates(RPID);

                    ddlWaraFrom.DataSource = lstReturnDates[0];
                    ddlWaraFrom.DataTextField = "FromDate";
                    ddlWaraFrom.DataValueField = "FromDate";
                    ddlWaraFrom.DataBind();
                    ddlWaraTo.DataSource = lstReturnDates[1];
                    ddlWaraTo.DataTextField = "ToDate";
                    ddlWaraTo.DataValueField = "ToDate";
                    ddlWaraTo.DataBind();

                    ddlWaraFrom.Attributes.Add("required", "required");
                    ddlWaraFrom.CssClass = ddlWaraFrom.CssClass.Replace("form-control", "form-control required");
                    ddlWaraTo.Attributes.Add("required", "required");
                    ddlWaraTo.CssClass = ddlWaraTo.CssClass.Replace("form-control", "form-control required");
                    txtStart.Attributes.Add("required", "required");
                    txtStart.CssClass = txtStart.CssClass.Replace("form-control decimalInput", "form-control decimalInput required");
                    txtEnd.Attributes.Add("required", "required");
                    txtEnd.CssClass = txtEnd.CssClass.Replace("form-control decimalInput ", "form-control decimalInput required");
                }
                else if (e.CommandName == "FootNote")
                {
                    txtFootNotes.Attributes.Add("maxlength", "2000");
                    txtFootNotes.Text = new RotationalProgramBLL().GetFootNotes(RPID);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalFootNote').modal();", true);
                }
                else if (e.CommandName == "Clone")
                {
                    Response.RedirectPermanent(String.Format("AddPlanBasicInfo.aspx?CloneID={0}", RPID), false);
                }
                else if (e.CommandName == "Graph")
                {
                    Response.RedirectPermanent(String.Format("GraphAndFrequencyBands.aspx?RP={0}", RPID), false);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRotationalProg_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent(String.Format("AddPlanBasicInfo.aspx?"));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Export

        protected void ExportToExcel(object sender, EventArgs e)
        {
            try
            {

                Button btn = (Button)sender;
                string ID = btn.CommandArgument.ToString();
                long IrrigationLevelID = new RotationalProgramBLL().GetIrrigationLevelIDByRPID(Convert.ToInt64(ID));
                if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                {
                    ExportRPForSE(Convert.ToInt64(ID));
                }
                else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Division || IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                {
                    ExportRPForXENAndSDO(Convert.ToInt64(ID));
                }



            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void ExportRPForSE(long _RPID)
        {
            try
            {
                List<dynamic> WaraData = new List<dynamic>();
                dynamic BasicInfo = new RotationalProgramBLL().GetBasicInfoForSeExport(_RPID);
                List<dynamic> DivisionData = new RotationalProgramBLL().GetDivisionsByGroups(_RPID);
                if (Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "SeasonID")) == 1)
                {
                    WaraData = new RotationalProgramBLL().GetWaraInfoForSEExport(_RPID, Utility.GetDynamicPropertyValue(BasicInfo, "ClosureStartDate"), Utility.GetDynamicPropertyValue(BasicInfo, "ClosureEndDate"));
                }
                else
                {
                    WaraData = new RotationalProgramBLL().GetWaraInfoForSEExport(_RPID, "", "");
                }

                IWorkbook workbook;

                //if (extension == "xlsx")
                //{
                workbook = new XSSFWorkbook();
                //}
                //else if (extension == "xls")
                //{
                //    workbook = new HSSFWorkbook();
                //}
                //else
                //{
                //    throw new Exception("This format is not supported");
                //}

                ISheet sheet1 = workbook.CreateSheet("RP");

                //make a header row
                IRow row1 = sheet1.CreateRow(0);
                IRow row2 = sheet1.CreateRow(1);

                NPOI.SS.UserModel.IFont boldFont = workbook.CreateFont();
                boldFont.Boldweight = (short)FontBoldWeight.Bold;
                ICellStyle boldStyle = workbook.CreateCellStyle();
                boldStyle.SetFont(boldFont);
                boldStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                boldStyle.FillPattern = FillPattern.SolidForeground;
                int CellCount = 0;
                int PreviousRow = 0;
                int RowCount = 1;
                int DivRow = 0;
                int NoOfGroups = 0;
                //For Basic Info
                for (int j = 0; j < 3; j++)
                {

                    String columnName;
                    ICell cell = row1.CreateCell(j);
                    if (j == 0)
                    {

                        columnName = "Start Date";
                        cell.SetCellValue(columnName);

                        cell.CellStyle = boldStyle;

                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(Utility.GetDynamicPropertyValue(BasicInfo, "StartDate"));

                    }
                    else if (j == 1)
                    {
                        columnName = "No. of Groups";
                        cell.SetCellValue(columnName);
                        cell.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "GroupQuantity")));
                        NoOfGroups = Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "GroupQuantity"));
                    }
                    else if (j == 2)
                    {
                        columnName = "No. of Divisions";


                        cell.SetCellValue(columnName);
                        cell.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "DivQuantity")));
                    }
                    CellCount = j;
                }

                //For Division Data
                for (int j = 0; j < DivisionData.Count; j++)
                {
                    if (j == 0)
                    {
                        CellCount = CellCount + 2;


                        ICell cell = row1.CreateCell(CellCount);
                        string columnName = "Groups";
                        cell.CellStyle = boldStyle;
                        cell.SetCellValue(columnName);

                        ICell cel2 = row2.CreateCell(CellCount);
                        cel2.SetCellValue(Utility.GetDynamicPropertyValue(DivisionData[j], "GroupName"));
                        CellCount = CellCount + 1;

                    }
                    else
                    {
                        IRow RowG = sheet1.CreateRow(RowCount);

                        ICell cell2 = RowG.CreateCell(CellCount);
                        cell2.SetCellValue(Utility.GetDynamicPropertyValue(DivisionData[j], "GroupName"));
                        PreviousRow = RowCount;
                        CellCount = CellCount + 1;
                    }

                    PropertyInfo[] propertyInfos = null;
                    propertyInfos = DivisionData[j].GetType().GetProperties();
                    List<string> Divisions = propertyInfos[1].GetValue(DivisionData[j]);

                    for (int i = 0; i < Divisions.Count; i++)
                    {
                        if (DivRow == 0)
                        {
                            ICell cell5 = row1.CreateCell(CellCount);
                            string columnName2 = "Division Name";
                            cell5.SetCellValue(columnName2);
                            cell5.CellStyle = boldStyle;


                            ICell cell3 = row2.CreateCell(CellCount);
                            cell3.SetCellValue(Divisions[i]);
                            RowCount = RowCount + 1;
                        }
                        else
                        {

                            if (PreviousRow != RowCount)
                            {
                                IRow row3 = sheet1.CreateRow(RowCount);
                                ICell cell3 = row3.CreateCell(CellCount);
                                cell3.SetCellValue(Divisions[i]);
                                PreviousRow = RowCount;
                                RowCount = RowCount + 1;
                            }
                            else
                            {
                                IRow row3 = sheet1.GetRow(RowCount);

                                ICell cell3 = row3.CreateCell(CellCount);
                                cell3.SetCellValue(Divisions[i]);
                                PreviousRow = RowCount;
                                RowCount = RowCount + 1;
                            }



                        }
                        DivRow++;
                    }
                    CellCount = CellCount - 1;

                }

                //For Wara/Position Data
                int WaraCellCount = 7;
                for (int j = 0; j < WaraData.Count; j++)
                {
                    if (j == 0)
                    {
                        ICell cell = row1.CreateCell(WaraCellCount);
                        cell.SetCellValue("Wara/Position");

                        cell.CellStyle = boldStyle;
                        ICell cell1 = row2.CreateCell(WaraCellCount);
                        cell1.SetCellValue(j + 1);


                        ICell cell2 = row1.CreateCell(WaraCellCount + 1);
                        cell2.SetCellValue("Days");

                        cell2.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(WaraCellCount + 1);
                        cel2.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraData[j], "Days")));

                        int ColName = 1;
                        for (int i = 9; i < NoOfGroups + 9; i++)
                        {
                            ICell cell3 = row1.CreateCell(i);
                            cell3.SetCellValue(ColName);

                            cell3.CellStyle = boldStyle;
                            ICell cell31 = row2.CreateCell(i);
                            cell31.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "GP" + Convert.ToString(ColName)));
                            ColName++;

                        }
                        WaraCellCount = WaraCellCount + ColName + 1;

                        //ICell cell4 = row1.CreateCell(WaraCellCount);
                        //cell4.SetCellValue("Priority");

                        //cell4.CellStyle = boldStyle;
                        //ICell cel42 = row2.CreateCell(WaraCellCount);
                        //cel42.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "Priority"));

                        WaraCellCount = 7;
                    }
                    else
                    {
                        if (j > NoOfGroups)
                        {
                            IRow rowW = sheet1.CreateRow(j + 1);
                            ICell cel2 = rowW.CreateCell(WaraCellCount);
                            cel2.SetCellValue(j + 1);
                            ICell cel3 = rowW.CreateCell(WaraCellCount + 1);
                            cel3.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraData[j], "Days")));

                            int ColName = 1;
                            for (int i = 9; i < NoOfGroups + 9; i++)
                            {
                                ICell cell3 = rowW.CreateCell(i);
                                cell3.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "GP" + Convert.ToString(ColName)));
                                ColName++;

                            }
                            WaraCellCount = WaraCellCount + ColName + 1;

                            //ICell cell4 = rowW.CreateCell(WaraCellCount);
                            //cell4.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "Priority"));

                            WaraCellCount = 7;
                        }
                        else
                        {
                            IRow rowW = sheet1.GetRow(j + 1);
                            ICell cel2 = rowW.CreateCell(WaraCellCount);
                            cel2.SetCellValue(j + 1);
                            ICell cel3 = rowW.CreateCell(WaraCellCount + 1);
                            cel3.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraData[j], "Days")));

                            int ColName = 1;
                            for (int i = 9; i < NoOfGroups + 9; i++)
                            {
                                ICell cell3 = rowW.CreateCell(i);
                                cell3.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "GP" + Convert.ToString(ColName)));
                                ColName++;

                            }
                            WaraCellCount = WaraCellCount + ColName + 1;

                            //ICell cell4 = rowW.CreateCell(WaraCellCount);

                            //cell4.SetCellValue(Utility.GetDynamicPropertyValue(WaraData[j], "Priority"));

                            WaraCellCount = 7;

                        }
                    }
                }
                sheet1.AutoSizeColumn(0);
                sheet1.AutoSizeColumn(1);
                sheet1.AutoSizeColumn(2);
                sheet1.AutoSizeColumn(5);
                sheet1.AutoSizeColumn(7);
                using (var exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    //if (extension == "xlsx") //xlsx file format
                    //{
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "RotationalProgram.xlsx"));
                    Response.BinaryWrite(exportData.GetBuffer());
                    //}
                    //else if (extension == "xls")  //xls file format
                    //{
                    //    Response.ContentType = "application/vnd.ms-excel";
                    //    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xls"));
                    //    Response.BinaryWrite(exportData.GetBuffer());
                    //}
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }

            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void ExportRPForXENAndSDO(long _RPID)
        {
            try
            {
                List<dynamic> WaraChannelsData = new List<dynamic>();
                dynamic BasicInfo = new RotationalProgramBLL().GetBasicInfoForXENSDOExport(_RPID);
                List<dynamic> ChannelsData = new RotationalProgramBLL().GetChannelsBySubGroups(_RPID);
                if (Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "SeasonID")) == 1)
                {
                    WaraChannelsData = new RotationalProgramBLL().GetChannelsWaraData(_RPID, Utility.GetDynamicPropertyValue(BasicInfo, "ClosureStartDate"), Utility.GetDynamicPropertyValue(BasicInfo, "ClosureEndDate"));
                }
                else
                {
                    WaraChannelsData = new RotationalProgramBLL().GetChannelsWaraData(_RPID, "", "");
                }
                List<dynamic> WaraCount = new RotationalProgramBLL().GetChannelsPreferences(WaraChannelsData);

                IWorkbook workbook;

                workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("RP");

                //make a header row
                IRow row1 = sheet1.CreateRow(0);
                IRow row2 = sheet1.CreateRow(1);

                NPOI.SS.UserModel.IFont boldFont = workbook.CreateFont();
                boldFont.Boldweight = (short)FontBoldWeight.Bold;
                ICellStyle boldStyle = workbook.CreateCellStyle();
                boldStyle.SetFont(boldFont);
                boldStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                boldStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                boldStyle.FillPattern = FillPattern.SolidForeground;
                int CellCount = 0;
                int PreviousRow = 0;
                int RowCount = 1;
                int DivRow = 0;
                //For Basic Info
                for (int j = 0; j < 3; j++)
                {
                    String columnName;
                    ICell cell = row1.CreateCell(j);
                    if (j == 0)
                    {

                        columnName = "Start Date";
                        cell.SetCellValue(columnName);

                        cell.CellStyle = boldStyle;

                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(Utility.GetDynamicPropertyValue(BasicInfo, "StartDate"));

                    }
                    else if (j == 1)
                    {
                        columnName = "No. of Groups";
                        cell.SetCellValue(columnName);
                        cell.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(ChannelsData.Count);
                    }
                    else if (j == 2)
                    {
                        columnName = "No. of Channels";


                        cell.SetCellValue(columnName);
                        cell.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(j);
                        cel2.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(BasicInfo, "ChannelQuantity")));
                    }
                    CellCount = j;
                }

                //For Channel Data
                for (int j = 0; j < ChannelsData.Count; j++)
                {
                    if (j == 0)
                    {
                        CellCount = CellCount + 2;


                        ICell cell = row1.CreateCell(CellCount);
                        string columnName = "Groups";
                        cell.CellStyle = boldStyle;
                        cell.SetCellValue(columnName);

                        ICell cel2 = row2.CreateCell(CellCount);
                        cel2.SetCellValue(Utility.GetDynamicPropertyValue(ChannelsData[j], "GroupName"));
                        CellCount = CellCount + 1;

                    }
                    else
                    {
                        IRow RowG = sheet1.CreateRow(RowCount);

                        ICell cell2 = RowG.CreateCell(CellCount);
                        cell2.SetCellValue(Utility.GetDynamicPropertyValue(ChannelsData[j], "GroupName"));
                        PreviousRow = RowCount;
                        CellCount = CellCount + 1;
                    }

                    PropertyInfo[] propertyInfos = null;
                    propertyInfos = ChannelsData[j].GetType().GetProperties();
                    List<dynamic> Channels = propertyInfos[1].GetValue(ChannelsData[j]);

                    for (int i = 0; i < Channels.Count(); i++)
                    {
                        if (DivRow == 0)
                        {
                            ICell cell5 = row1.CreateCell(CellCount);
                            string columnName2 = "Channel Name";
                            cell5.SetCellValue(columnName2);
                            cell5.CellStyle = boldStyle;


                            ICell cell3 = row2.CreateCell(CellCount);
                            cell3.SetCellValue(Utility.GetDynamicPropertyValue(Channels[i], "NAME"));

                            ICell cell6 = row1.CreateCell(CellCount + 1);
                            string columnName3 = "Design";
                            cell6.SetCellValue(columnName3);
                            cell6.CellStyle = boldStyle;


                            ICell cell4 = row2.CreateCell(CellCount + 1);
                            cell4.SetCellValue(Convert.ToDouble(Utility.GetDynamicPropertyValue(Channels[i], "Discharge")));
                            RowCount = RowCount + 1;
                        }
                        else
                        {

                            if (PreviousRow != RowCount)
                            {
                                IRow row3 = sheet1.CreateRow(RowCount);
                                ICell cell3 = row3.CreateCell(CellCount);
                                cell3.SetCellValue(Utility.GetDynamicPropertyValue(Channels[i], "NAME"));

                                ICell cell4 = row3.CreateCell(CellCount + 1);
                                cell4.SetCellValue(Convert.ToDouble(Utility.GetDynamicPropertyValue(Channels[i], "Discharge")));
                                PreviousRow = RowCount;
                                RowCount = RowCount + 1;

                            }
                            else
                            {
                                IRow row3 = sheet1.GetRow(RowCount);

                                ICell cell3 = row3.CreateCell(CellCount);
                                cell3.SetCellValue(Utility.GetDynamicPropertyValue(Channels[i], "NAME"));

                                ICell cell4 = row3.CreateCell(CellCount + 1);
                                cell4.SetCellValue(Convert.ToDouble(Utility.GetDynamicPropertyValue(Channels[i], "Discharge")));
                                PreviousRow = RowCount;
                                RowCount = RowCount + 1;

                            }
                        }
                        DivRow++;
                    }
                    CellCount = CellCount - 1;

                }

                //For Wara/Position Data
                int WaraCellCount = 8;
                for (int j = 0; j < WaraChannelsData.Count; j++)
                {
                    if (j == 0)
                    {
                        ICell cell = row1.CreateCell(WaraCellCount);
                        cell.SetCellValue("Wara/Position");

                        cell.CellStyle = boldStyle;
                        ICell cell1 = row2.CreateCell(WaraCellCount);
                        cell1.SetCellValue(j + 1);


                        ICell cell2 = row1.CreateCell(WaraCellCount + 1);
                        cell2.SetCellValue("Days");

                        cell2.CellStyle = boldStyle;
                        ICell cel2 = row2.CreateCell(WaraCellCount + 1);
                        cel2.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Days")));

                        int Col = 1;
                        int Pre = 0;
                        for (int i = 10; i < WaraCount[j].Count + 10; i++)
                        {
                            ICell cell3 = row1.CreateCell(i);
                            cell3.SetCellValue(Col);

                            cell3.CellStyle = boldStyle;
                            ICell cell31 = row2.CreateCell(i);
                            cell31.SetCellValue(WaraCount[j][Pre]);


                            Col++;
                            Pre++;

                        }
                        WaraCellCount = WaraCellCount + Col + 1;
                        if (Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority") != "0")
                        {
                            ICell cell4 = row1.CreateCell(WaraCellCount);
                            cell4.SetCellValue("Priority");

                            cell4.CellStyle = boldStyle;
                            ICell cel42 = row2.CreateCell(WaraCellCount);
                            cel42.SetCellValue(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority"));
                        }


                        WaraCellCount = 8;
                    }
                    else
                    {
                        if (j >= RowCount - 1)
                        {
                            IRow rowW = sheet1.CreateRow(j + 1);
                            ICell cel2 = rowW.CreateCell(WaraCellCount);
                            cel2.SetCellValue(j + 1);
                            ICell cel3 = rowW.CreateCell(WaraCellCount + 1);
                            cel3.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Days")));

                            int Col = 1;
                            int Pre = 0;
                            for (int i = 10; i < WaraCount[j].Count + 10; i++)
                            {
                                ICell cell3 = rowW.CreateCell(i);
                                cell3.SetCellValue(WaraCount[j][Pre]);

                                Col++;
                                Pre++;

                            }
                            WaraCellCount = WaraCellCount + Col + 1;

                            if (Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority") != "0")
                            {
                                ICell cell4 = rowW.CreateCell(WaraCellCount);
                                cell4.SetCellValue(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority"));
                            }


                            WaraCellCount = 8;
                        }
                        else
                        {
                            IRow rowW = sheet1.GetRow(j + 1);
                            ICell cel2 = rowW.CreateCell(WaraCellCount);
                            cel2.SetCellValue(j + 1);
                            ICell cel3 = rowW.CreateCell(WaraCellCount + 1);
                            cel3.SetCellValue(Convert.ToInt32(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Days")));

                            int Col = 1;
                            int Pre = 0;
                            for (int i = 10; i < WaraCount[j].Count + 10; i++)
                            {
                                ICell cell3 = rowW.CreateCell(i);
                                cell3.SetCellValue(WaraCount[j][Pre]);

                                Col++;
                                Pre++;

                            }
                            WaraCellCount = WaraCellCount + Col + 1;
                            if (Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority") != "0")
                            {
                                ICell cell4 = rowW.CreateCell(WaraCellCount);
                                cell4.SetCellValue(Utility.GetDynamicPropertyValue(WaraChannelsData[j], "Priority"));
                            }
                            WaraCellCount = 8;

                        }
                    }
                }

                sheet1.AutoSizeColumn(0);
                sheet1.AutoSizeColumn(1);
                sheet1.AutoSizeColumn(2);
                sheet1.AutoSizeColumn(5);
                sheet1.AutoSizeColumn(8);

                using (var exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "RotationalProgram.xlsx"));
                    Response.BinaryWrite(exportData.GetBuffer());
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }

            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion

        protected void gvRotationalProg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRotationalProg.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRotationalProg_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRotationalProg.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime WaraFrom = Convert.ToDateTime(ddlWaraFrom.Text);
                DateTime WaraTo = Convert.ToDateTime(ddlWaraTo.Text);
                double Max = Convert.ToDouble(txtEnd.Text) / 100;
                double Min = Convert.ToDouble(txtStart.Text) / 100;
                long RPID = Convert.ToInt64(hfRPID.Value);
                bool Result = new RotationalProgramBLL().GetPlanImplementation(RPID, Max, Min, WaraFrom, WaraTo);
                if (Result)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#ModalCalculatePlan').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalCalculatePlan", sb.ToString(), false);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnCompleteSeason_Click(object sender, EventArgs e)
        {
            try
            {
                ddlWaraFrom.SelectedIndex = 0;
                DateTime WaraFrom = Convert.ToDateTime(ddlWaraFrom.Text);
                DateTime WaraTo = Convert.ToDateTime(ddlWaraTo.Items[ddlWaraTo.Items.Count - 1].Text);
                double Max = Convert.ToDouble(txtEnd.Text) / 100;
                double Min = Convert.ToDouble(txtStart.Text) / 100;
                long RPID = Convert.ToInt64(hfRPID.Value);
                bool Result = new RotationalProgramBLL().GetPlanImplementation(RPID, Max, Min, WaraFrom, WaraTo);
                if (Result)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#ModalCalculatePlan').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalCalculatePlan", sb.ToString(), false);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            try
            {
                long RPID = Convert.ToInt64(hfRPID.Value);
                String FootNote = Convert.ToString(txtFootNotes.Text);
                bool Result = new RotationalProgramBLL().AddFootNotes(RPID, FootNote);
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalFootNote').modal('hide');", true);
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}