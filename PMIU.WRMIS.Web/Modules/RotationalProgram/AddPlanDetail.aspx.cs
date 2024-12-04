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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class AddPlanDetail : BasePage
    {
        #region Global Variable

        static long ProgramID = -1;
        static long BoundryID = -1;
        static int NoOfGroups = 1;
        int BindingGroup = 0;
        static bool IsClosure = false;
        List<RP_Division> lstAllDivisions = new List<RP_Division>();
        List<RP_Division> lstGroupDivisions = new List<RP_Division>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();

                if (Request.QueryString["Msg"] != null)
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);

                if (Request.QueryString["ID"] != null)
                {
                    ProgramID = Convert.ToInt64(Request.QueryString["ID"]);
                    BindBasicInfo(ProgramID);
                    BindingGroup = 0;
                    lstAllDivisions = new RotationalProgramBLL().GetProgramMappedDivisions(ProgramID);
                    List<dynamic> lstGroups = Utility.GetGroups(NoOfGroups);
                    rptrGroups.DataSource = lstGroups;
                    rptrGroups.DataBind();

                    if (lstAllDivisions.Count() > 0)
                        BindPreferenceGrid();
                    else
                        gvPreferance.Visible = false;
                    ViewAttachment();
                }
            }
        }

        public void ViewAttachment()
        {
            try
            {
                string ImageName = new RotationalProgramBLL().GetAttachmentfiles(ProgramID);
                if (!string.IsNullOrEmpty(ImageName))
                {
                    hlImage.NavigateUrl = Utility.GetImageURL(Configuration.RotationalProgram, ImageName);
                    hlImage.Visible = true;
                }
                else
                    hlImage.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindBasicInfo(long _ID)
        {
            try
            {
                object BasicInfo = new RotationalProgramBLL().GetBasicDetail(_ID);
                if (BasicInfo != null)
                {
                    lblRotationalProgram.Text = Convert.ToString(BasicInfo.GetType().GetProperty("PlanName").GetValue(BasicInfo));
                    lblNamelbl.Text = Convert.ToString(BasicInfo.GetType().GetProperty("LevelName").GetValue(BasicInfo));
                    lblName.Text = Convert.ToString(BasicInfo.GetType().GetProperty("BoundryName").GetValue(BasicInfo));
                    lblSeason.Text = Convert.ToString(BasicInfo.GetType().GetProperty("Season").GetValue(BasicInfo));
                    lblStartDate.Text = Convert.ToString(BasicInfo.GetType().GetProperty("StartDate").GetValue(BasicInfo));
                    lblYear.Text = Convert.ToString(BasicInfo.GetType().GetProperty("Year").GetValue(BasicInfo));
                    lblGroups.Text = Convert.ToString(BasicInfo.GetType().GetProperty("Groups").GetValue(BasicInfo));
                    BoundryID = Convert.ToInt64(BasicInfo.GetType().GetProperty("BoundryID").GetValue(BasicInfo));
                    NoOfGroups = Convert.ToInt32(BasicInfo.GetType().GetProperty("Groups").GetValue(BasicInfo));
                    lblClosureStart.Text = Convert.ToString(BasicInfo.GetType().GetProperty("ClosureStartDate").GetValue(BasicInfo));
                    if (lblClosureStart.Text == "")
                        lblClosureStart.Text = "-";
                    lblClosureEnd.Text = Convert.ToString(BasicInfo.GetType().GetProperty("ClosureEndDate").GetValue(BasicInfo));
                    if (lblClosureEnd.Text == "")
                        lblClosureEnd.Text = "-";

                    if (lblSeason.Text == "Rabi")
                    {
                        if (lblClosureStart.Text == "-" && lblClosureEnd.Text == "-")
                        {
                            divClosure.Visible = true;
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rptrGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    BindingGroup++;
                    lstGroupDivisions = lstAllDivisions.Where(q => q.GroupID == BindingGroup).ToList();
                    List<object> lstdivisions = new RotationalProgramBLL().GetDivisionsAgainstChannelID(BoundryID, (DataBinder.Eval(e.Item.DataItem, "Name").ToString()));
                    Repeater rptrDivisions = (Repeater)(e.Item.FindControl("rptrDivisions"));
                    rptrDivisions.DataSource = lstdivisions;
                    rptrDivisions.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rptrDivisions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField DivID = (HiddenField)(e.Item.FindControl("hdnID"));
                    CheckBox cb = (CheckBox)(e.Item.FindControl("cbDiv"));
                    foreach (var v in lstGroupDivisions)
                    {
                        if (v.DivisionID == Convert.ToInt64(DivID.Value.Remove(0, 1)))
                            cb.Checked = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnMapDiv_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> lstIDs = new List<long>();
                bool MultipleSelection = false;

                for (int vLoop = 0; vLoop < rptrGroups.Items.Count; vLoop++)
                {
                    if (lstIDs.Count() > 0)
                        lstIDs.Add(-1);

                    Repeater innerRepeater = (Repeater)rptrGroups.Items[vLoop].FindControl("rptrDivisions");
                    for (int vinnerLoop = 0; vinnerLoop < innerRepeater.Items.Count; vinnerLoop++)
                    {
                        CheckBox cb = (CheckBox)innerRepeater.Items[vinnerLoop].FindControl("cbDiv");
                        if (cb.Checked == true)
                        {
                            HiddenField hfield = (HiddenField)innerRepeater.Items[vinnerLoop].FindControl("hdnID");
                            string ID = hfield.Value.ToString();
                            ID = ID.Remove(0, 1);
                            lstIDs.Add(Convert.ToInt64(ID));
                        }
                    }
                }

                List<long> lstRepeat = (from s in lstIDs
                                        group s by s into g
                                        where g.Count() > 1
                                        select g.First()).ToList<long>();

                for (int i = 0; i < lstRepeat.Count(); i++)
                {
                    if (lstRepeat[i] != -1)
                    {
                        MultipleSelection = true;
                        break;
                    }
                }

                if (MultipleSelection) //((lstRepeat.Count() == 1 && lstRepeat.FirstOrDefault() != -1) || lstRepeat.Count() > 1)
                    Master.ShowMessage(Message.MultipleGroup.Description, SiteMaster.MessageType.Error);
                else
                {
                    bool Result = new RotationalProgramBLL().SaveDivisions(lstIDs, ProgramID, Convert.ToInt32(Session[SessionValues.UserID]), NoOfGroups);
                    if (Result)
                    {
                        BindPreferenceGrid();
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);

                        RP_Attachment Attachment = new RP_Attachment();
                        List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.RotationalProgram);
                        if (lstNameofFiles.Count > 0)
                        {
                            Attachment.RPID = ProgramID;
                            Attachment.FileURL = lstNameofFiles[0].Item3;
                            Attachment.CreatedDate = DateTime.Now;
                            Attachment.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                            new RotationalProgramBLL().SaveAttachments(Attachment);

                            ViewAttachment();
                        }
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnChannels_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton Sender = (LinkButton)sender;
                string ID = Sender.CommandArgument;
                ID = ID.Remove(0, 1);
                List<long?> lstEnteringChnlIDs = new List<long?>();
                gvChannels.DataSource = new RotationalProgramBLL().GetChannelsAgainstDivision(Convert.ToInt64(ID), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstEnteringChnlIDs, false);
                gvChannels.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalChannels').modal();", true);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex == gvPreferance.EditIndex)
                    {
                        DropDownList ddlFirst = new DropDownList();
                        DropDownList ddlSecond = new DropDownList();
                        DropDownList ddlThird = new DropDownList();
                        DropDownList ddlFourth = new DropDownList();
                        DropDownList ddlFifth = new DropDownList();
                        TextBox txtStartDate = new TextBox();
                        TextBox txtEndDate = new TextBox();

                        string PrefID1 = GetDataKeyValue(gvPreferance, "PrefID1", e.Row.RowIndex);
                        string PrefID2 = GetDataKeyValue(gvPreferance, "PrefID2", e.Row.RowIndex);
                        string PrefID3 = GetDataKeyValue(gvPreferance, "PrefID3", e.Row.RowIndex);
                        string PrefID4 = GetDataKeyValue(gvPreferance, "PrefID4", e.Row.RowIndex);
                        string PrefID5 = GetDataKeyValue(gvPreferance, "PrefID5", e.Row.RowIndex);
                        string StartDate = GetDataKeyValue(gvPreferance, "StartDate", e.Row.RowIndex);
                        string EndDate = GetDataKeyValue(gvPreferance, "EndDate", e.Row.RowIndex);

                        txtStartDate = (e.Row.FindControl("txtFromDate") as TextBox);
                        txtEndDate = (e.Row.FindControl("txtToDate") as TextBox);
                        txtStartDate.Text = StartDate;
                        txtEndDate.Text = EndDate;

                        if (NoOfGroups == 5)
                        {
                            ddlFirst = (e.Row.FindControl("ddlFirst") as DropDownList);
                            ddlSecond = (e.Row.FindControl("ddlSecond") as DropDownList);
                            ddlThird = (e.Row.FindControl("ddlThird") as DropDownList);
                            ddlFourth = (e.Row.FindControl("ddlFourth") as DropDownList);
                            ddlFifth = (e.Row.FindControl("ddlFifth") as DropDownList);

                            BindDropdown(ddlFirst, ddlSecond, ddlThird, ddlFourth, ddlFifth);
                            Dropdownlist.SetSelectedValue(ddlFirst, PrefID1);
                            Dropdownlist.SetSelectedValue(ddlSecond, PrefID2);
                            Dropdownlist.SetSelectedValue(ddlThird, PrefID3);
                            Dropdownlist.SetSelectedValue(ddlFourth, PrefID4);
                            Dropdownlist.SetSelectedValue(ddlFifth, PrefID5);

                        }
                        else if (NoOfGroups == 4)
                        {
                            ddlFirst = (e.Row.FindControl("ddlFirst") as DropDownList);
                            ddlSecond = (e.Row.FindControl("ddlSecond") as DropDownList);
                            ddlThird = (e.Row.FindControl("ddlThird") as DropDownList);
                            ddlFourth = (e.Row.FindControl("ddlFourth") as DropDownList);
                            ddlFifth = (e.Row.FindControl("ddlFifth") as DropDownList);
                            ddlFifth.Visible = false;

                            BindDropdown(ddlFirst, ddlSecond, ddlThird, ddlFourth, ddlFifth);
                            Dropdownlist.SetSelectedValue(ddlFirst, PrefID1);
                            Dropdownlist.SetSelectedValue(ddlSecond, PrefID2);
                            Dropdownlist.SetSelectedValue(ddlThird, PrefID3);
                            Dropdownlist.SetSelectedValue(ddlFourth, PrefID4);
                        }
                        else if (NoOfGroups == 3)
                        {
                            ddlFirst = (e.Row.FindControl("ddlFirst") as DropDownList);
                            ddlSecond = (e.Row.FindControl("ddlSecond") as DropDownList);
                            ddlThird = (e.Row.FindControl("ddlThird") as DropDownList);
                            ddlFourth = (e.Row.FindControl("ddlFourth") as DropDownList);
                            ddlFourth.Visible = false;
                            ddlFifth = (e.Row.FindControl("ddlFifth") as DropDownList);
                            ddlFifth.Visible = false;

                            BindDropdown(ddlFirst, ddlSecond, ddlThird, ddlFourth, ddlFifth);
                            Dropdownlist.SetSelectedValue(ddlFirst, PrefID1);
                            Dropdownlist.SetSelectedValue(ddlSecond, PrefID2);
                            Dropdownlist.SetSelectedValue(ddlThird, PrefID3);
                        }
                        else if (NoOfGroups == 2)
                        {
                            ddlFirst = (e.Row.FindControl("ddlFirst") as DropDownList);
                            ddlSecond = (e.Row.FindControl("ddlSecond") as DropDownList);
                            ddlThird = (e.Row.FindControl("ddlThird") as DropDownList);
                            ddlThird.Visible = false;
                            ddlFourth = (e.Row.FindControl("ddlFourth") as DropDownList);
                            ddlFourth.Visible = false;
                            ddlFifth = (e.Row.FindControl("ddlFifth") as DropDownList);
                            ddlFifth.Visible = false;

                            BindDropdown(ddlFirst, ddlSecond, ddlThird, ddlFourth, ddlFifth);
                            Dropdownlist.SetSelectedValue(ddlFirst, PrefID1);
                            Dropdownlist.SetSelectedValue(ddlSecond, PrefID2);
                        }
                        else if (NoOfGroups == 1)
                        {
                            ddlFirst = (e.Row.FindControl("ddlFirst") as DropDownList);
                            ddlSecond = (e.Row.FindControl("ddlSecond") as DropDownList);
                            ddlSecond.Visible = false;
                            ddlThird = (e.Row.FindControl("ddlThird") as DropDownList);
                            ddlThird.Visible = false;
                            ddlFourth = (e.Row.FindControl("ddlFourth") as DropDownList);
                            ddlFourth.Visible = false;
                            ddlFifth = (e.Row.FindControl("ddlFifth") as DropDownList);
                            ddlFifth.Visible = false;

                            BindDropdown(ddlFirst, ddlSecond, ddlThird, ddlFourth, ddlFifth);
                            Dropdownlist.SetSelectedValue(ddlFirst, PrefID1);
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPreferance.EditIndex = -1;
                BindPreferenceGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                RP_Rotation_Circle ObjSave = new RP_Rotation_Circle();
                bool Result = true;
                int RowIndex = e.RowIndex;
                ObjSave.RPID = ProgramID;
                ObjSave.ID = Convert.ToInt64(((Label)gvPreferance.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                ObjSave.StartDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].Cells[1].FindControl("txtFromDate")).Text);
                ObjSave.EndDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].Cells[2].FindControl("txtToDate")).Text);

                if (ObjSave.EndDate < ObjSave.StartDate)
                {
                    Master.ShowMessage(Message.EndDateMustBeGreater.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (RowIndex == 0)
                {
                    if (ObjSave.StartDate != Convert.ToDateTime(lblStartDate.Text))
                    {
                        Master.ShowMessage(Message.FromDateShouldBeEqual.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                //if ((ObjSave.StartDate == Convert.ToDateTime(lblClosureStart.Text)) && (ObjSave.EndDate == Convert.ToDateTime(lblClosureEnd.Text)))
                if (lblClosureStart.Text != "-" && lblClosureEnd.Text != "-")
                {
                    if (((ObjSave.StartDate >= Convert.ToDateTime(lblClosureStart.Text) && ObjSave.StartDate <= Convert.ToDateTime(lblClosureEnd.Text))
                       || (ObjSave.EndDate >= Convert.ToDateTime(lblClosureStart.Text) && ObjSave.StartDate <= Convert.ToDateTime(lblClosureEnd.Text)))
                        && !((ObjSave.StartDate == Convert.ToDateTime(lblClosureStart.Text)) && (ObjSave.EndDate == Convert.ToDateTime(lblClosureEnd.Text))))
                    {
                        Master.ShowMessage(Message.ClosureDatesCannotBeSelected.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                DateTime LastDate;
                if (RowIndex - 1 >= 0)
                {
                    LastDate = Convert.ToDateTime(((Label)gvPreferance.Rows[RowIndex - 1].Cells[2].FindControl("lblToDate")).Text);
                    if (LastDate >= ObjSave.StartDate)
                    {
                        Master.ShowMessage(Message.FromDateShouldBeLess.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    if (LastDate.AddDays(1) != ObjSave.StartDate)
                    {
                        Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    if ((lblClosureStart.Text != "-" && lblClosureEnd.Text != "-") && (LastDate.AddDays(1) == Convert.ToDateTime(lblClosureStart.Text)))
                    {
                        if ((!IsClosure) && (Convert.ToDateTime(lblClosureEnd.Text).AddDays(1) != ObjSave.StartDate))
                        {
                            Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                    }
                    //else
                    //{
                    //    if (LastDate.AddDays(1) != ObjSave.StartDate)
                    //    {
                    //        Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                    //        return;
                    //    }
                    //}
                }

                if (!IsClosure)
                {
                    if (NoOfGroups == 5)
                    {
                        DropDownList GroupPreference1 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[3].FindControl("ddlFirst"));
                        DropDownList GroupPreference2 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[4].FindControl("ddlSecond"));
                        DropDownList GroupPreference3 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[5].FindControl("ddlThird"));
                        DropDownList GroupPreference4 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[6].FindControl("ddlFourth"));
                        DropDownList GroupPreference5 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[7].FindControl("ddlFifth"));

                        ObjSave.GroupPreference1 = Convert.ToInt16(GroupPreference1.SelectedItem.Value);
                        ObjSave.GroupPreference2 = Convert.ToInt16(GroupPreference2.SelectedItem.Value);
                        ObjSave.GroupPreference3 = Convert.ToInt16(GroupPreference3.SelectedItem.Value);
                        ObjSave.GroupPreference4 = Convert.ToInt16(GroupPreference4.SelectedItem.Value);
                        ObjSave.GroupPreference5 = Convert.ToInt16(GroupPreference5.SelectedItem.Value);

                        if (ObjSave.GroupPreference1 == ObjSave.GroupPreference2 || ObjSave.GroupPreference1 == ObjSave.GroupPreference3
                            || ObjSave.GroupPreference1 == ObjSave.GroupPreference4 || ObjSave.GroupPreference1 == ObjSave.GroupPreference5
                            || ObjSave.GroupPreference2 == ObjSave.GroupPreference3 || ObjSave.GroupPreference2 == ObjSave.GroupPreference4 || ObjSave.GroupPreference2 == ObjSave.GroupPreference5
                            || ObjSave.GroupPreference3 == ObjSave.GroupPreference4 || ObjSave.GroupPreference3 == ObjSave.GroupPreference5
                            || ObjSave.GroupPreference4 == ObjSave.GroupPreference5)
                            Result = false;
                    }
                    else if (NoOfGroups == 4)
                    {
                        DropDownList GroupPreference1 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[3].FindControl("ddlFirst"));
                        DropDownList GroupPreference2 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[4].FindControl("ddlSecond"));
                        DropDownList GroupPreference3 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[5].FindControl("ddlThird"));
                        DropDownList GroupPreference4 = ((DropDownList)gvPreferance.Rows[RowIndex].Cells[6].FindControl("ddlFourth"));

                        ObjSave.GroupPreference1 = Convert.ToInt16(GroupPreference1.SelectedItem.Value);
                        ObjSave.GroupPreference2 = Convert.ToInt16(GroupPreference2.SelectedItem.Value);
                        ObjSave.GroupPreference3 = Convert.ToInt16(GroupPreference3.SelectedItem.Value);
                        ObjSave.GroupPreference4 = Convert.ToInt16(GroupPreference4.SelectedItem.Value);

                        if (ObjSave.GroupPreference1 == ObjSave.GroupPreference2 || ObjSave.GroupPreference1 == ObjSave.GroupPreference3 || ObjSave.GroupPreference1 == ObjSave.GroupPreference4
                            || ObjSave.GroupPreference2 == ObjSave.GroupPreference3 || ObjSave.GroupPreference2 == ObjSave.GroupPreference4
                            || ObjSave.GroupPreference3 == ObjSave.GroupPreference4)
                            Result = false;
                    }
                    else if (NoOfGroups == 3)
                    {

                        DropDownList GroupPreference1 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[3].FindControl("ddlFirst");
                        DropDownList GroupPreference2 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[4].FindControl("ddlSecond");
                        DropDownList GroupPreference3 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[5].FindControl("ddlThird");

                        ObjSave.GroupPreference1 = Convert.ToInt16(GroupPreference1.SelectedItem.Value);
                        ObjSave.GroupPreference2 = Convert.ToInt16(GroupPreference2.SelectedItem.Value);
                        ObjSave.GroupPreference3 = Convert.ToInt16(GroupPreference3.SelectedItem.Value);

                        if (ObjSave.GroupPreference1 == ObjSave.GroupPreference2 || ObjSave.GroupPreference1 == ObjSave.GroupPreference3 || ObjSave.GroupPreference2 == ObjSave.GroupPreference3)
                            Result = false;
                    }
                    else if (NoOfGroups == 2)
                    {
                        DropDownList GroupPreference1 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[3].FindControl("ddlFirst");
                        DropDownList GroupPreference2 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[4].FindControl("ddlSecond");

                        ObjSave.GroupPreference1 = Convert.ToInt16(GroupPreference1.SelectedItem.Value);
                        ObjSave.GroupPreference2 = Convert.ToInt16(GroupPreference2.SelectedItem.Value);
                        if (ObjSave.GroupPreference1 == ObjSave.GroupPreference2)
                            Result = false;
                    }
                    else if (NoOfGroups == 1)
                    {
                        DropDownList GroupPreference1 = (DropDownList)gvPreferance.Rows[RowIndex].Cells[3].FindControl("ddlFirst");
                        ObjSave.GroupPreference1 = Convert.ToInt16(GroupPreference1.SelectedItem.Value);

                    }
                }

                ObjSave.PriorityTypeID = 0;
                ObjSave.CreatedDate = DateTime.Now;
                ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                if (Result)
                {
                    Result = new RotationalProgramBLL().SaveCirclePreferences(ObjSave);
                    if (Result)
                    {
                        gvPreferance.EditIndex = -1;
                        BindPreferenceGrid();
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvPreferance.EditIndex = e.NewEditIndex;
                BindPreferenceGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<dynamic> lstData = new RotationalProgramBLL().GetCircleGridData(ProgramID);
                    dynamic NewRow;

                    if (lstData.Count() == 0)
                    {
                        NewRow = new
                        {
                            ID = -1,
                            StartDate = Utility.GetFormattedDate(Convert.ToDateTime(lblStartDate.Text)),
                            EndDate = "",
                            Pref1 = "",
                            PrefID1 = "",
                            Pref2 = "",
                            PrefID2 = "",
                            Pref3 = "",
                            PrefID3 = "",
                            Pref4 = "",
                            PrefID4 = "",
                            Pref5 = "",
                            PrefID5 = ""
                        };
                    }
                    else
                    {
                        Label txtToDate = (Label)gvPreferance.Rows[lstData.Count() - 1].FindControl("lblToDate");
                        DateTime Todate = DateTime.Now;
                        if (txtToDate != null)
                        {
                            Todate = Convert.ToDateTime(txtToDate.Text);
                            Todate = Todate.AddDays(1);
                        }

                        NewRow = new
                        {
                            ID = -1,
                            StartDate = Utility.GetFormattedDate(Todate),
                            EndDate = "",
                            Pref1 = "",
                            PrefID1 = "",
                            Pref2 = "",
                            PrefID2 = "",
                            Pref3 = "",
                            PrefID3 = "",
                            Pref4 = "",
                            PrefID4 = "",
                            Pref5 = "",
                            PrefID5 = ""
                        };
                    }

                    lstData.Add(NewRow);
                    gvPreferance.EditIndex = lstData.Count() - 1;
                    gvPreferance.DataSource = lstData;
                    gvPreferance.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindDropdown(DropDownList _ddlFirst, DropDownList ddlSecond, DropDownList ddlThird, DropDownList ddlFourth, DropDownList ddlFifth)
        {
            try
            {
                if (NoOfGroups == 5)
                {
                    _ddlFirst.DataSource = Utility.GetGroups(NoOfGroups);
                    _ddlFirst.DataTextField = "Name";
                    _ddlFirst.DataValueField = "ID";
                    _ddlFirst.DataBind();
                    _ddlFirst.Items.Insert(0, new ListItem("Select"));

                    ddlSecond.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlSecond.DataTextField = "Name";
                    ddlSecond.DataValueField = "ID";
                    ddlSecond.DataBind();
                    ddlSecond.Items.Insert(0, new ListItem("Select"));

                    ddlThird.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlThird.DataTextField = "Name";
                    ddlThird.DataValueField = "ID";
                    ddlThird.DataBind();
                    ddlThird.Items.Insert(0, new ListItem("Select"));

                    ddlFourth.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlFourth.DataTextField = "Name";
                    ddlFourth.DataValueField = "ID";
                    ddlFourth.DataBind();
                    ddlFourth.Items.Insert(0, new ListItem("Select"));

                    ddlFifth.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlFifth.DataTextField = "Name";
                    ddlFifth.DataValueField = "ID";
                    ddlFifth.DataBind();
                    ddlFifth.Items.Insert(0, new ListItem("Select"));
                }
                else if (NoOfGroups == 4)
                {
                    _ddlFirst.DataSource = Utility.GetGroups(NoOfGroups);
                    _ddlFirst.DataTextField = "Name";
                    _ddlFirst.DataValueField = "ID";
                    _ddlFirst.DataBind();
                    _ddlFirst.Items.Insert(0, new ListItem("Select"));

                    ddlSecond.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlSecond.DataTextField = "Name";
                    ddlSecond.DataValueField = "ID";
                    ddlSecond.DataBind();
                    ddlSecond.Items.Insert(0, new ListItem("Select"));

                    ddlThird.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlThird.DataTextField = "Name";
                    ddlThird.DataValueField = "ID";
                    ddlThird.DataBind();
                    ddlThird.Items.Insert(0, new ListItem("Select"));

                    ddlFourth.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlFourth.DataTextField = "Name";
                    ddlFourth.DataValueField = "ID";
                    ddlFourth.DataBind();
                    ddlFourth.Items.Insert(0, new ListItem("Select"));
                }
                else if (NoOfGroups == 3)
                {
                    _ddlFirst.DataSource = Utility.GetGroups(NoOfGroups);
                    _ddlFirst.DataTextField = "Name";
                    _ddlFirst.DataValueField = "ID";
                    _ddlFirst.DataBind();
                    _ddlFirst.Items.Insert(0, new ListItem("Select"));

                    ddlSecond.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlSecond.DataTextField = "Name";
                    ddlSecond.DataValueField = "ID";
                    ddlSecond.DataBind();
                    ddlSecond.Items.Insert(0, new ListItem("Select"));

                    ddlThird.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlThird.DataTextField = "Name";
                    ddlThird.DataValueField = "ID";
                    ddlThird.DataBind();
                    ddlThird.Items.Insert(0, new ListItem("Select"));
                }
                else if (NoOfGroups == 2)
                {
                    _ddlFirst.DataSource = Utility.GetGroups(NoOfGroups);
                    _ddlFirst.DataTextField = "Name";
                    _ddlFirst.DataValueField = "ID";
                    _ddlFirst.DataBind();
                    _ddlFirst.Items.Insert(0, new ListItem("Select"));

                    ddlSecond.DataSource = Utility.GetGroups(NoOfGroups);
                    ddlSecond.DataTextField = "Name";
                    ddlSecond.DataValueField = "ID";
                    ddlSecond.DataBind();
                    ddlSecond.Items.Insert(0, new ListItem("Select"));
                }
                else if (NoOfGroups == 1)
                {
                    _ddlFirst.DataSource = Utility.GetGroups(NoOfGroups);
                    _ddlFirst.DataTextField = "Name";
                    _ddlFirst.DataValueField = "ID";
                    _ddlFirst.DataBind();
                    _ddlFirst.Items.Insert(0, new ListItem("Select"));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindPreferenceGrid()
        {
            try
            {
                gvPreferance.DataSource = new RotationalProgramBLL().GetCircleGridData(ProgramID);
                gvPreferance.DataBind();
                gvPreferance.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }

        protected void gvPreferance_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (NoOfGroups == 1)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                else if (NoOfGroups == 2)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
                else if (NoOfGroups == 3)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[6].Visible = false;
                }
                else if (NoOfGroups == 4)
                    e.Row.Cells[7].Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(((Label)gvPreferance.Rows[e.RowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new RotationalProgramBLL().DeleteCirclePref(ID);
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    BindPreferenceGrid();
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
                TextBox txtFromDate = ((TextBox)gvRow.FindControl("txtFromDate"));
                TextBox txtToDate = (TextBox)gvRow.FindControl("txtToDate");

                if (txtFromDate != null && txtToDate != null && lblClosureStart.Text != "-" && lblClosureEnd.Text != "-")
                {
                    if (txtToDate.Text == lblClosureEnd.Text && txtFromDate.Text == lblClosureStart.Text)
                    {
                        IsClosure = true;
                        txtFromDate.Visible = false;
                        txtToDate.Visible = false;

                        DropDownList ddl = (DropDownList)gvRow.FindControl("ddlFirst");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSecond");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlThird");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlFourth");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlFifth");
                        ddl.Visible = false;
                        Master.ShowMessage(Message.ClosureDatesSelected.Description, SiteMaster.MessageType.Warning);
                    }
                    else
                        IsClosure = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnClosureSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = new RotationalProgramBLL().AddClosureDatesCircle(ProgramID, Convert.ToDateTime(txtClosureStartDate.Text), Convert.ToDateTime(txtClosureEndDate.Text));
                if (Result)
                {
                    divClosure.Visible = false;
                    lblClosureStart.Text = Utility.GetFormattedDate(Convert.ToDateTime(txtClosureStartDate.Text));
                    lblClosureEnd.Text = Utility.GetFormattedDate(Convert.ToDateTime(txtClosureEndDate.Text));

                    if (gvPreferance.Rows.Count > 0)
                        BindPreferenceGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}