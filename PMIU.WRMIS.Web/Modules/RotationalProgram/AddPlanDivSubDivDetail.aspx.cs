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
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class AddPlanDivSubDivDetail : BasePage
    {
        #region Global Variable
        Decimal GroupSum = 0;
        Decimal TotalSum = 0;
        static long ProgramID = 97;
        static long BoundryID = 51;
        static int NoOfGroups = 1;
        static int NoOfSubGroups = 2;
        int BindingGroup = 0;
        int BindingSubGroup = 0;
        int IsGroup = 0;
        static bool IsClosure = false;
        List<RP_Channel> lstAllChannels = new List<RP_Channel>();
        List<RP_Channel> lstGroupChannels = new List<RP_Channel>();
        List<RP_Channel> lstSubGroupChannels = new List<RP_Channel>();
        static List<int> lstActualSubGroups = new List<int>();
        List<long?> lstEnteringChnlIDs = new List<long?>();
        List<dynamic> lstChannels;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();

                if (Request.QueryString["Msg"] != null)
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                lstChannels = new List<dynamic>();
                if (Request.QueryString["ID"] != null)
                {
                    ProgramID = Convert.ToInt64(Request.QueryString["ID"]);
                    BindBasicInfo(ProgramID);

                    lstActualSubGroups = new RotationalProgramBLL().GetActualSubGroups(ProgramID, NoOfGroups, NoOfSubGroups);
                    if (lstActualSubGroups.Count() > 0)
                        BindPreferenceGrid();
                    else
                    {
                        gvPreferanceDetail.Visible = false;
                        btnSendToSE.Visible = false;
                    }

                    ViewAttachment();
                    BindEnteringChannels();
                    NoOfGroupsBindingLogic();
                }
            }
        }

        public void NoOfGroupsBindingLogic()
        {
            try
            {
                BindingGroup = 1;
                BindingSubGroup = 1;
                if (NoOfGroups == 5)
                {
                    lstAllChannels = new RotationalProgramBLL().GetProgramMappedChannels(ProgramID);
                    List<dynamic> lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);
                    rptrGroups.DataSource = lstgroups1;
                    rptrGroups.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup1.DataSource = lstgroups1;
                    rptrGroup1.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup2.DataSource = lstgroups1;
                    rptrGroup2.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup3.DataSource = lstgroups1;
                    rptrGroup3.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup4.DataSource = lstgroups1;
                    rptrGroup4.DataBind();
                }
                else if (NoOfGroups == 4)
                {
                    lstAllChannels = new RotationalProgramBLL().GetProgramMappedChannels(ProgramID);
                    List<dynamic> lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);
                    rptrGroups.DataSource = lstgroups1;
                    rptrGroups.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup1.DataSource = lstgroups1;
                    rptrGroup1.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup2.DataSource = lstgroups1;
                    rptrGroup2.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup3.DataSource = lstgroups1;
                    rptrGroup3.DataBind();

                    rptrGroup4.Visible = false;
                }
                else if (NoOfGroups == 3)
                {
                    lstAllChannels = new RotationalProgramBLL().GetProgramMappedChannels(ProgramID);
                    List<dynamic> lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);
                    rptrGroups.DataSource = lstgroups1;
                    rptrGroups.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup1.DataSource = lstgroups1;
                    rptrGroup1.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup2.DataSource = lstgroups1;
                    rptrGroup2.DataBind();

                    rptrGroup3.Visible = false;
                }
                else if (NoOfGroups == 2)
                {
                    lstAllChannels = new RotationalProgramBLL().GetProgramMappedChannels(ProgramID);
                    List<dynamic> lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);
                    rptrGroups.DataSource = lstgroups1;
                    rptrGroups.DataBind();

                    lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);                    
                    rptrGroup1.DataSource = lstgroups1;
                    rptrGroup1.DataBind();

                    rptrGroup2.Visible = false;
                    rptrGroup3.Visible = false;
                }
                else if (NoOfGroups == 1)
                {
                    lstAllChannels = new RotationalProgramBLL().GetProgramMappedChannels(ProgramID);
                    List<dynamic> lstgroups1 = Utility.GetGroupName(BindingGroup);//(NoOfGroups);
                    rptrGroups.DataSource = lstgroups1;
                    rptrGroups.DataBind();

                    rptrGroup1.Visible = false;
                    rptrGroup2.Visible = false;
                    rptrGroup3.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
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
                    NoOfGroups = Convert.ToInt32(lblGroups.Text);
                    NoOfSubGroups = Convert.ToInt32(BasicInfo.GetType().GetProperty("SubGroups").GetValue(BasicInfo));
                    BoundryID = Convert.ToInt64(BasicInfo.GetType().GetProperty("BoundryID").GetValue(BasicInfo));
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
            lstGroupChannels = new List<RP_Channel>();
            lstGroupChannels = lstAllChannels.Where(q => q.GroupID == BindingGroup).ToList();
            List<dynamic> lstSubGroups = Utility.GetSubGroupsOfGroup(BindingGroup, -1);
            BindingGroup++;
            IsGroup = 0;
            TotalSum = 0;
            Repeater rptrSubGroups = (Repeater)(e.Item.FindControl("rptrSubGroups"));
            rptrSubGroups.DataSource = lstSubGroups;
            rptrSubGroups.DataBind();
        }

        protected void rptrSubGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (lstGroupChannels.Count() > 0)
            {
                lstSubGroupChannels = new List<RP_Channel>();
                CheckBox cb = (CheckBox)(e.Item.FindControl("cbSubGroup"));
                if (lstGroupChannels.FirstOrDefault().SubGroupID != null)
                {
                    lstSubGroupChannels = lstGroupChannels.Where(q => q.SubGroupID == BindingSubGroup).ToList();
                    //CheckBox cb = (CheckBox)(e.Item.FindControl("cbSubGroup"));
                    if (lstSubGroupChannels.Count() > 0)
                        cb.Checked = true;
                    //cb.Enabled = false;
                }
                else
                {
                    lstSubGroupChannels = lstGroupChannels;
                    IsGroup++;
                }
                cb.Enabled = false;
            }
            GroupSum = 0;
            BindingSubGroup++;

            if (lstChannels.Count() == 0)
                lstChannels = new RotationalProgramBLL().GetChannelsAgainstDivision(BoundryID, Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstEnteringChnlIDs, IsPostBack);
            Repeater rptrChannels = (Repeater)(e.Item.FindControl("rptrChannels"));
            rptrChannels.DataSource = lstChannels;
            rptrChannels.DataBind();
        }

        protected void rptrChannels_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField DivID = (HiddenField)(e.Item.FindControl("hdnID"));
                    CheckBox cb = (CheckBox)(e.Item.FindControl("cbChnl"));
                    TextBox txDis = (TextBox)(e.Item.FindControl("txtDischarge"));
                    Label ChnlName = (Label)(e.Item.FindControl("lblChnlName"));

                    long ChnlTypeID = Convert.ToInt64(e.Item.DataItem.GetType().GetProperty("ChannelTypeID").GetValue(e.Item.DataItem));
                    if (ChnlTypeID == (int)Constants.ChannelType.BranchCanal || ChnlTypeID == (int)Constants.ChannelType.MainCanal)
                    {
                        cb.Visible = false;
                        txDis.Visible = false;
                        ChnlName.Font.Bold = true;
                        //ChnlName.Font.Size = FontUnit.Medium;
                        ChnlName.Font.Underline = true;
                        ChnlName.Style.Add("margin-left", "10px");
                    }

                    if (IsGroup < 2)// jugad to check if group is selected then it does not check selected channel in every sub group 
                    {
                        txDis.Enabled = false;

                        foreach (var v in lstSubGroupChannels)
                        {
                            if (v.ChannelID == Convert.ToInt64(DivID.Value))
                            {
                                cb.Checked = true;
                                txDis.Enabled = true;
                                txDis.Text = v.Discharge.ToString();
                                GroupSum = GroupSum + Convert.ToDecimal(txDis.Text);
                                TotalSum = TotalSum + Convert.ToDecimal(txDis.Text);
                            }
                        }
                    }
                    else
                    {
                        foreach (var v in lstSubGroupChannels)
                            txDis.Enabled = false;
                    }

                    Repeater rit = (Repeater)sender;
                    TextBox txtSum = (TextBox)rit.Parent.FindControl("txtSum");
                    txtSum.Text = GroupSum.ToString();

                    TextBox txtTotSum = (TextBox)rit.Parent.Parent.Parent.FindControl("txtTotalSum");
                    txtTotSum.Text = TotalSum.ToString();

                    cb.Attributes.Add("onclick", "javascript:ChannelTextboxEnable('" + cb.ClientID + "', '" + txDis.ClientID + "', '" + txtSum.ClientID + "', '" + txtTotSum.ClientID + "')");
                    txDis.Attributes.Add("onchange", "javascript:UpdateSum('" + cb.ClientID + "', '" + txDis.ClientID + "', '" + txDis.Text + "', '" + txtSum.ClientID + "', '" + txtTotSum.ClientID + "')");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindEnteringChannels()
        {
            lstEnteringChnlIDs = new RotationalProgramBLL().GetEnteringChannels(ProgramID).ToList();
            List<object> lstEnteringchannels = new RotationalProgramBLL().GetEnteringChannelsAgainstDivision(BoundryID, Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID));
            gvEnteringChannels.DataSource = lstEnteringchannels;
            gvEnteringChannels.DataBind();
        }

        protected void btnMapDiv_Click(object sender, EventArgs e)
        {
            try
            {
                List<dynamic> lstChnls = new List<dynamic>();
                List<long> lstIDs = new List<long>();
                dynamic RetObj = null;
                string Selected = "";
                bool Break = false;
                bool NochannelSelected = true;
                BindingSubGroup = 1;
                int SubGroupSelection = 0;

                if (NoOfGroups >= 1)
                {
                    Break = RepeaterLoops(rptrGroups, NochannelSelected, Selected, SubGroupSelection, lstChnls, lstIDs);
                    if (Break)
                        return;
                }
                if (NoOfGroups >= 2)
                {
                    Break = RepeaterLoops(rptrGroup1, NochannelSelected, Selected, SubGroupSelection, lstChnls, lstIDs);
                    if (Break)
                        return;
                }
                if (NoOfGroups >= 3)
                {
                    Break = RepeaterLoops(rptrGroup2, NochannelSelected, Selected, SubGroupSelection, lstChnls, lstIDs);
                    if (Break)
                        return;
                }
                if (NoOfGroups >= 4)
                {
                    Break = RepeaterLoops(rptrGroup3, NochannelSelected, Selected, SubGroupSelection, lstChnls, lstIDs);
                    if (Break)
                        return;
                }
                if (NoOfGroups >= 5)
                {
                    RetObj = RepeaterLoops(rptrGroup4, NochannelSelected, Selected, SubGroupSelection, lstChnls, lstIDs);
                    if (Break)
                        return;
                }

                if (!Break) //(!NochannelSelected && !Break)
                {
                    List<dynamic> lstChnl = (from s in lstChnls  // check multiple selection of channels
                                             group s by s.ChannelID into g
                                             where g.Count() > 1
                                             select g.First()).ToList<dynamic>();
                    Break = false;
                    foreach (dynamic chnl in lstChnl)
                    {
                        if (chnl.ChannelID == -1 || chnl.ChannelID == -2)
                            continue;
                        else
                        {
                            Break = true;
                            break;
                        }
                    }

                    if (Break)
                        Master.ShowMessage(Message.MultipleChannelSelection.Description, SiteMaster.MessageType.Error);
                    else
                    {
                        string Result = new RotationalProgramBLL().SaveChannels(lstChnls, ProgramID, Convert.ToInt32(Session[SessionValues.UserID]), NoOfGroups);
                        if (Result == "Success")
                        {
                            lstActualSubGroups = new List<int>();
                            lstActualSubGroups = new RotationalProgramBLL().GetActualSubGroups(ProgramID, NoOfGroups, NoOfSubGroups);
                            BindPreferenceGrid();
                            bool Res = SaveSelectedEnteringChannels();

                            if (Res)
                                Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                            else
                                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);

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

                            // to disable check boxes of group and sub groups
                            lstChannels = new List<dynamic>();
                            BindEnteringChannels();
                            NoOfGroupsBindingLogic();
                        }
                        else if (Result == "Fail")
                            Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                        else
                        {
                            // to disable check boxes of group and sub groups
                            lstChannels = new List<dynamic>();
                            BindEnteringChannels();
                            NoOfGroupsBindingLogic();
                            Master.ShowMessage(Message.ChannelsAlreadyInApprovedDraft.Description + " " + Result, SiteMaster.MessageType.Error);
                        }

                    }
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool SaveSelectedEnteringChannels()
        {
            List<RP_EnteringChannel> lstEC = new List<RP_EnteringChannel>();
            RP_EnteringChannel objEC = new RP_EnteringChannel();
            bool Result = false;
            try
            {
                foreach (GridViewRow row in gvEnteringChannels.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cbEntrngChnls");
                        if (cb != null && cb.Checked)
                        {
                            Label lblChnlID = (Label)row.FindControl("lblID");
                            if (lblChnlID != null && lblChnlID.Text != "")
                            {
                                objEC = new RP_EnteringChannel();
                                objEC.RPID = ProgramID;
                                objEC.ChannelID = Convert.ToInt64(lblChnlID.Text);
                                objEC.CreatedDate = DateTime.Now;
                                objEC.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                                lstEC.Add(objEC);
                            }
                        }
                    }
                }
                Result = new RotationalProgramBLL().AddEnteringchannels(lstEC);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool RepeaterLoops(Repeater rptrGroups, bool NochannelSelected, string Selected, int SubGroupSelection, List<dynamic> lstChannels, List<long> lstIDs)
        {
            dynamic ObjAdd;
            bool Break = false;

            // for (int vLoop = 0; vLoop < rptrGroups.Items.Count; vLoop++)
            // {
            NochannelSelected = true;
            Repeater innerRepeater = (Repeater)rptrGroups.Items[0].FindControl("rptrSubGroups");
            for (int vinnerLoop = 0; vinnerLoop < innerRepeater.Items.Count; vinnerLoop++)
            {
                CheckBox SubGrp = (CheckBox)innerRepeater.Items[vinnerLoop].FindControl("cbSubGroup");
                if ((SubGrp.Checked == false && Selected == Constants.SubGroup) && vinnerLoop == 1)
                {
                    SubGroupSelection++;
                    Master.ShowMessage(Message.SubGroupSelection.Description, SiteMaster.MessageType.Error);
                    Break = true;
                    break;
                }

                if (Selected == Constants.Group || (SubGrp.Checked == false && Selected == Constants.SubGroup))
                    continue;

                SubGroupSelection++;
                if ((Selected == "" || Selected == Constants.SubGroup) && SubGrp.Checked == true)
                    Selected = Constants.SubGroup;
                else if ((Selected == "" || Selected == Constants.Group) && SubGrp.Checked == false)
                    Selected = Constants.Group;
                else
                {
                    Master.ShowMessage(Message.GroupAndSubGroupSelection.Description, SiteMaster.MessageType.Error);
                    Break = true;
                    break;
                }

                Repeater ChnlRepeater = (Repeater)innerRepeater.Items[vinnerLoop].FindControl("rptrChannels");
                for (int ChnlLoop = 0; ChnlLoop < ChnlRepeater.Items.Count; ChnlLoop++)
                {
                    CheckBox cb = (CheckBox)ChnlRepeater.Items[ChnlLoop].FindControl("cbChnl");
                    if (cb.Checked == true)
                    {
                        NochannelSelected = false;
                        TextBox txtDis = (TextBox)ChnlRepeater.Items[ChnlLoop].FindControl("txtDischarge");
                        HiddenField hfield = (HiddenField)ChnlRepeater.Items[ChnlLoop].FindControl("hdnID");
                        string ID = hfield.Value.ToString();
                        lstIDs.Add(Convert.ToInt64(ID));
                        ObjAdd = new
                        {
                            ChannelID = Convert.ToInt64(ID),
                            Selection = Selected.ToString(),
                            Discharge = Convert.ToDecimal(txtDis.Text == "" ? "0" : txtDis.Text)
                        };
                        lstChannels.Add(ObjAdd);
                    }
                }

                ObjAdd = new  // delimeter: which shows that one SUB GROUP selection ends
                {
                    ChannelID = -1,
                    Selection = "",
                    Discharge = -1
                };
                lstChannels.Add(ObjAdd);
                lstIDs.Add(-1);
            }

            if (Selected == Constants.SubGroup && SubGroupSelection <= 1)
            {
                Master.ShowMessage(Message.MoreThanOneSubGroupSelection.Description, SiteMaster.MessageType.Error);
                Break = true;
            }

            SubGroupSelection = 0;

            ObjAdd = new  // delimeter: which shows that one GROUP selection ends
            {
                ChannelID = -2,
                Selection = "",
                Discharge = -1
            };
            lstChannels.Add(ObjAdd);

            if (NochannelSelected)
            {
                Master.ShowMessage(Message.NoChannelSelected.Description, SiteMaster.MessageType.Error);
                Break = true;
            }
            //else
            //    Selected = "";
            // }            
            return Break;
        }

        public void BindPreferenceGrid()
        {
            try
            {
                gvPreferanceDetail.DataSource = new RotationalProgramBLL().GetDivSubDivPreferenceData(ProgramID);
                gvPreferanceDetail.DataBind();
                gvPreferanceDetail.Visible = true;
                btnSendToSE.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex == gvPreferanceDetail.EditIndex)
                    {
                        long SubGroupCount = 3;
                        string StartDate = GetDataKeyValue(gvPreferanceDetail, "StartDate", e.Row.RowIndex);
                        string EndDate = GetDataKeyValue(gvPreferanceDetail, "EndDate", e.Row.RowIndex);

                        DropDownList ddlGroup = new DropDownList();
                        DropDownList ddlSubGroup1 = new DropDownList();
                        DropDownList ddlSubGroup2 = new DropDownList();
                        DropDownList ddlSubGroup3 = new DropDownList();
                        TextBox txtStartDate = new TextBox();
                        TextBox txtEndDate = new TextBox();

                        long ID = Convert.ToInt64(((Label)e.Row.FindControl("lblID")).Text);
                        txtStartDate = (e.Row.FindControl("txtFromDate") as TextBox);
                        txtEndDate = (e.Row.FindControl("txtToDate") as TextBox);
                        txtStartDate.Text = StartDate;
                        txtEndDate.Text = EndDate;

                        if (txtStartDate != null && txtEndDate != null && lblClosureStart.Text != "-" && lblClosureEnd.Text != "-"
                            && txtStartDate.Text == lblClosureStart.Text && txtEndDate.Text == lblClosureEnd.Text)
                        {
                            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlG2");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlG3");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlG4");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG41");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG42");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG43");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlG5");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG51");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG52");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlSG53");
                            ddl.Visible = false;

                            ddl = (DropDownList)e.Row.FindControl("ddlPriority");
                            ddl.Visible = false;
                        }
                        else
                        {
                            ddlGroup = (DropDownList)e.Row.FindControl("ddlPriority");
                            ddlGroup.DataSource = Utility.GetPriority();
                            ddlGroup.DataValueField = "ID";
                            ddlGroup.DataTextField = "Name";
                            ddlGroup.DataBind();

                            if (NoOfGroups == (int)Constants.NoOfGroups.One)
                            {
                                string GroupID = GetDataKeyValue(gvPreferanceDetail, "GID1", e.Row.RowIndex);
                                string SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID11", e.Row.RowIndex);
                                string SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID12", e.Row.RowIndex);
                                string SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID13", e.Row.RowIndex);

                                if (!string.IsNullOrEmpty(SubGroupID1))
                                {
                                    SubGroupCount = 0;
                                    SubGroupCount++;
                                }

                                if (!string.IsNullOrEmpty(SubGroupID2))
                                    SubGroupCount++;

                                if (!string.IsNullOrEmpty(SubGroupID3))
                                    SubGroupCount++;

                                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                                ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                ddl.DataValueField = "ID";
                                ddl.DataTextField = "Name";
                                ddl.DataBind();
                                Dropdownlist.SetSelectedValue(ddl, GroupID);
                                ddl.Items.Insert(0, new ListItem("Select", ""));

                                if (ID != -1) // edit row 
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;
                                }
                                else
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                }
                            }
                            else if (NoOfGroups == (int)Constants.NoOfGroups.Two)
                            {
                                string GroupID = GetDataKeyValue(gvPreferanceDetail, "GID1", e.Row.RowIndex);
                                string SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID11", e.Row.RowIndex);
                                string SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID12", e.Row.RowIndex);
                                string SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID13", e.Row.RowIndex);

                                if (!string.IsNullOrEmpty(SubGroupID1))
                                {
                                    SubGroupCount = 0;
                                    SubGroupCount++;
                                }

                                if (!string.IsNullOrEmpty(SubGroupID2))
                                    SubGroupCount++;

                                if (!string.IsNullOrEmpty(SubGroupID3))
                                    SubGroupCount++;

                                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                                ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                ddl.DataValueField = "ID";
                                ddl.DataTextField = "Name";
                                ddl.DataBind();
                                Dropdownlist.SetSelectedValue(ddl, GroupID);
                                ddl.Items.Insert(0, new ListItem("Select", ""));

                                if (ID != -1) // edit row 
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID2", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID21", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID22", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID23", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));


                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;
                                }
                                else // add new row
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                }
                            }
                            else if (NoOfGroups == (int)Constants.NoOfGroups.Three)
                            {
                                string GroupID = GetDataKeyValue(gvPreferanceDetail, "GID1", e.Row.RowIndex);
                                string SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID11", e.Row.RowIndex);
                                string SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID12", e.Row.RowIndex);
                                string SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID13", e.Row.RowIndex);

                                if (!string.IsNullOrEmpty(SubGroupID1))
                                {
                                    SubGroupCount = 0;
                                    SubGroupCount++;
                                }

                                if (!string.IsNullOrEmpty(SubGroupID2))
                                    SubGroupCount++;

                                if (!string.IsNullOrEmpty(SubGroupID3))
                                    SubGroupCount++;

                                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                                ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                ddl.DataValueField = "ID";
                                ddl.DataTextField = "Name";
                                ddl.DataBind();
                                Dropdownlist.SetSelectedValue(ddl, GroupID);
                                ddl.Items.Insert(0, new ListItem("Select", ""));

                                if (ID != -1)
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID2", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID21", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID22", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID23", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID3", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID31", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID32", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID33", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;
                                }
                                else
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                }
                            }
                            else if (NoOfGroups == (int)Constants.NoOfGroups.Four)
                            {
                                string GroupID = GetDataKeyValue(gvPreferanceDetail, "GID1", e.Row.RowIndex);
                                string SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID11", e.Row.RowIndex);
                                string SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID12", e.Row.RowIndex);
                                string SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID13", e.Row.RowIndex);

                                if (!string.IsNullOrEmpty(SubGroupID1))
                                {
                                    SubGroupCount = 0;
                                    SubGroupCount++;
                                }

                                if (!string.IsNullOrEmpty(SubGroupID2))
                                    SubGroupCount++;

                                if (!string.IsNullOrEmpty(SubGroupID3))
                                    SubGroupCount++;

                                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                                ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                ddl.DataValueField = "ID";
                                ddl.DataTextField = "Name";
                                ddl.DataBind();
                                Dropdownlist.SetSelectedValue(ddl, GroupID);
                                ddl.Items.Insert(0, new ListItem("Select", ""));

                                if (ID != -1)
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID2", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID21", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID22", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID23", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID3", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID31", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID32", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID33", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID4", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID41", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID42", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID43", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG4");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG41");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG42");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG43");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), SubGroupCount);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;
                                }
                                else
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG4");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG41");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG42");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG43");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                }
                            }
                            else if (NoOfGroups == (int)Constants.NoOfGroups.Five)
                            {
                                string GroupID = GetDataKeyValue(gvPreferanceDetail, "GID1", e.Row.RowIndex);
                                string SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID11", e.Row.RowIndex);
                                string SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID12", e.Row.RowIndex);
                                string SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID13", e.Row.RowIndex);

                                if (!string.IsNullOrEmpty(SubGroupID1))
                                {
                                    SubGroupCount = 0;
                                    SubGroupCount++;
                                }

                                if (!string.IsNullOrEmpty(SubGroupID2))
                                    SubGroupCount++;

                                if (!string.IsNullOrEmpty(SubGroupID3))
                                    SubGroupCount++;

                                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlG1");
                                ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                ddl.DataValueField = "ID";
                                ddl.DataTextField = "Name";
                                ddl.DataBind();
                                Dropdownlist.SetSelectedValue(ddl, GroupID);
                                ddl.Items.Insert(0, new ListItem("Select", ""));


                                if (ID != -1)
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID2", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID21", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID22", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID23", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID3", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID31", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID32", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID33", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID4", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID41", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID42", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID43", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG4");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG41");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG42");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG43");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;

                                    GroupID = GetDataKeyValue(gvPreferanceDetail, "GID5", e.Row.RowIndex);
                                    SubGroupID1 = GetDataKeyValue(gvPreferanceDetail, "SGID51", e.Row.RowIndex);
                                    SubGroupID2 = GetDataKeyValue(gvPreferanceDetail, "SGID52", e.Row.RowIndex);
                                    SubGroupID3 = GetDataKeyValue(gvPreferanceDetail, "SGID53", e.Row.RowIndex);

                                    if (!string.IsNullOrEmpty(SubGroupID1))
                                    {
                                        SubGroupCount = 0;
                                        SubGroupCount++;
                                    }

                                    if (!string.IsNullOrEmpty(SubGroupID2))
                                        SubGroupCount++;

                                    if (!string.IsNullOrEmpty(SubGroupID3))
                                        SubGroupCount++;

                                    ddl = (DropDownList)e.Row.FindControl("ddlG5");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG51");
                                    if (SubGroupID1 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID1);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG52");
                                    if (SubGroupID2 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID2);
                                    }
                                    else
                                        ddl.Visible = false;

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG53");
                                    if (SubGroupID3 != "")
                                    {
                                        ddl.DataSource = Utility.GetSubGroupsOfGroup(Convert.ToInt64(GroupID), -1);
                                        ddl.DataValueField = "ID";
                                        ddl.DataTextField = "Name";
                                        ddl.DataBind();
                                        Dropdownlist.SetSelectedValue(ddl, SubGroupID3);
                                    }
                                    else
                                        ddl.Visible = false;
                                }
                                else
                                {
                                    ddl = (DropDownList)e.Row.FindControl("ddlSG11");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG12");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG13");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.One, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG2");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    Dropdownlist.SetSelectedValue(ddl, GroupID);
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG21");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG22");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG23");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Two, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG3");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG31");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG32");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG33");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Three, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG4");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG41");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG42");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG43");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Four, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlG5");
                                    ddl.DataSource = Utility.GetGroups(NoOfGroups);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("Select", ""));

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG51");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Five, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG52");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Five, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();

                                    ddl = (DropDownList)e.Row.FindControl("ddlSG53");
                                    ddl.DataSource = Utility.GetSubGroupsOfGroup((int)Constants.NoOfGroups.Five, SubGroupCount);
                                    ddl.DataValueField = "ID";
                                    ddl.DataTextField = "Name";
                                    ddl.DataBind();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<dynamic> lstSavedData = new RotationalProgramBLL().GetDivSubDivPreferenceData(ProgramID);
                    dynamic objNew;

                    if (lstSavedData.Count() == 0)
                    {
                        objNew = new
                       {
                           ID = -1,
                           StartDate = Utility.GetFormattedDate(Convert.ToDateTime(lblStartDate.Text)),
                           EndDate = "",

                           G1 = "",
                           GID1 = "",
                           SG11 = "",
                           SGID11 = "",
                           SG12 = "",
                           SGID12 = "",
                           SG13 = "",
                           SGID13 = "",

                           G2 = "",
                           GID2 = "",
                           SG21 = "",
                           SGID21 = "",
                           SG22 = "",
                           SGID22 = "",
                           SG23 = "",
                           SGID23 = "",

                           G3 = "",
                           GID3 = "",
                           SG31 = "",
                           SGID31 = "",
                           SG32 = "",
                           SGID32 = "",
                           SG33 = "",
                           SGID33 = "",

                           G4 = "",
                           GID4 = "",
                           SG41 = "",
                           SGID41 = "",
                           SG42 = "",
                           SGID42 = "",
                           SG43 = "",
                           SGID43 = "",

                           G5 = "",
                           GID5 = "",
                           SG51 = "",
                           SGID51 = "",
                           SG52 = "",
                           SGID52 = "",
                           SG53 = "",
                           SGID53 = ""
                       };
                    }
                    else
                    {
                        Label txtToDate = (Label)gvPreferanceDetail.Rows[lstSavedData.Count() - 1].FindControl("lblToDate");
                        DateTime Todate = DateTime.Now;
                        if (txtToDate != null)
                        {
                            Todate = Convert.ToDateTime(txtToDate.Text);
                            Todate = Todate.AddDays(1);
                        }


                        objNew = new
                        {
                            ID = -1,
                            StartDate = Utility.GetFormattedDate(Todate),
                            EndDate = "",

                            G1 = "",
                            GID1 = "",
                            SG11 = "",
                            SGID11 = "",
                            SG12 = "",
                            SGID12 = "",
                            SG13 = "",
                            SGID13 = "",

                            G2 = "",
                            GID2 = "",
                            SG21 = "",
                            SGID21 = "",
                            SG22 = "",
                            SGID22 = "",
                            SG23 = "",
                            SGID23 = "",

                            G3 = "",
                            GID3 = "",
                            SG31 = "",
                            SGID31 = "",
                            SG32 = "",
                            SGID32 = "",
                            SG33 = "",
                            SGID33 = "",

                            G4 = "",
                            GID4 = "",
                            SG41 = "",
                            SGID41 = "",
                            SG42 = "",
                            SGID42 = "",
                            SG43 = "",
                            SGID43 = "",

                            G5 = "",
                            GID5 = "",
                            SG51 = "",
                            SGID51 = "",
                            SG52 = "",
                            SGID52 = "",
                            SG53 = "",
                            SGID53 = ""
                        };
                    }

                    lstSavedData.Add(objNew);
                    gvPreferanceDetail.DataSource = lstSavedData;
                    gvPreferanceDetail.EditIndex = lstSavedData.Count() - 1;
                    gvPreferanceDetail.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                RP_Rotation_DivisionSubDivision objSave = new RP_Rotation_DivisionSubDivision();
                objSave.RPID = ProgramID;
                objSave.ID = Convert.ToInt64(((Label)gvPreferanceDetail.Rows[RowIndex].FindControl("lblID")).Text);
                objSave.StartDate = Convert.ToDateTime(((TextBox)gvPreferanceDetail.Rows[RowIndex].FindControl("txtFromDate")).Text);
                objSave.EndDate = Convert.ToDateTime(((TextBox)gvPreferanceDetail.Rows[RowIndex].FindControl("txtToDate")).Text);
                objSave.PriorityTypeID = 0;
                objSave.CreatedDate = DateTime.Now;
                objSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                if (RowIndex == 0)
                {
                    if (objSave.StartDate != Convert.ToDateTime(lblStartDate.Text))
                    {
                        Master.ShowMessage(Message.FromDateShouldBeEqual.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (lblClosureStart.Text != "-" && lblClosureEnd.Text != "-")
                {
                    if (((objSave.StartDate >= Convert.ToDateTime(lblClosureStart.Text) && objSave.StartDate <= Convert.ToDateTime(lblClosureEnd.Text))
                       || (objSave.EndDate >= Convert.ToDateTime(lblClosureStart.Text) && objSave.StartDate <= Convert.ToDateTime(lblClosureEnd.Text)))
                        && !((objSave.StartDate == Convert.ToDateTime(lblClosureStart.Text)) && (objSave.EndDate == Convert.ToDateTime(lblClosureEnd.Text))))
                    {
                        Master.ShowMessage(Message.ClosureDatesCannotBeSelected.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                DateTime LastDate;
                if (RowIndex - 1 >= 0)
                {
                    LastDate = Convert.ToDateTime(((Label)gvPreferanceDetail.Rows[RowIndex - 1].Cells[2].FindControl("lblToDate")).Text);
                    if (LastDate >= objSave.StartDate)
                    {
                        Master.ShowMessage(Message.FromDateShouldBeLess.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    if (LastDate.AddDays(1) != objSave.StartDate)
                    {
                        Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    if ((lblClosureStart.Text != "-" && lblClosureEnd.Text != "-") && (LastDate.AddDays(1) == Convert.ToDateTime(lblClosureStart.Text)))
                    {
                        if ((!IsClosure) && (Convert.ToDateTime(lblClosureEnd.Text).AddDays(1) != objSave.StartDate))
                        {
                            Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    //else
                    //{
                    //    if (LastDate.AddDays(1) != objSave.StartDate)
                    //    {
                    //        Master.ShowMessage(Message.SkipDays.Description, SiteMaster.MessageType.Error);
                    //        return;
                    //    }
                    //}
                }

                if (objSave.EndDate < objSave.StartDate)
                {
                    Master.ShowMessage(Message.EndDateMustBeGreater.Description, SiteMaster.MessageType.Error);
                    return;
                }
                using (TransactionScope transaction = new TransactionScope())
                {
                    DateTime NextDate;
                    if (RowIndex + 2 <= gvPreferanceDetail.Rows.Count)
                    {
                        NextDate = Convert.ToDateTime(((Label)gvPreferanceDetail.Rows[RowIndex + 1].FindControl("lblStartDate")).Text);
                        TimeSpan? TS = (objSave.EndDate - NextDate);

                        if (objSave.EndDate >= NextDate)
                        {
                            int DayDiff = TS.Value.Days + 1;

                            if (DayDiff >= 1)
                                new RotationalProgramBLL().AdjustDateDifference(ProgramID, NextDate, DayDiff);
                        }
                        else
                        {
                            int DayDiff = TS.Value.Days + 1;

                            if (DayDiff <= -1)
                                new RotationalProgramBLL().AdjustDateDifference(ProgramID, NextDate, DayDiff);
                        }
                    }

                    transaction.Complete();
                }

                if (!IsClosure)
                {
                    DropDownList ddlPriority = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlPriority");

                    if (ddlPriority.SelectedItem.Value != "")
                        objSave.PriorityTypeID = Convert.ToInt16(ddlPriority.SelectedItem.Value);
                    else
                        objSave.PriorityTypeID = 0;

                    if (NoOfGroups == (int)Constants.NoOfGroups.One)
                    {
                        DropDownList ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG1");
                        objSave.GroupPreference1 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG11");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference11 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG12");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference12 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG13");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference13 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference12)
                            || (objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference13)
                            || (objSave.SubGroupPreference12 != null && objSave.SubGroupPreference12 == objSave.SubGroupPreference13))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Two)
                    {
                        DropDownList ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG1");
                        objSave.GroupPreference1 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG11");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference11 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG12");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference12 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG13");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference13 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference12)
                            || (objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference13)
                           || (objSave.SubGroupPreference12 != null && objSave.SubGroupPreference12 == objSave.SubGroupPreference13))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG2");
                        if (ddl.Visible == true)
                            objSave.GroupPreference2 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG21");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference21 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG22");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference22 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG23");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference23 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference22)
                            || (objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference23)
                            || (objSave.SubGroupPreference22 != null && objSave.SubGroupPreference22 == objSave.SubGroupPreference23))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference2)
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Three)
                    {
                        DropDownList ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG1");
                        objSave.GroupPreference1 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG11");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference11 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG12");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference12 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG13");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference13 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference12)
                            || (objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference13)
                           || (objSave.SubGroupPreference12 != null && objSave.SubGroupPreference12 == objSave.SubGroupPreference13))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG2");
                        objSave.GroupPreference2 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG21");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference21 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG22");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference22 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG23");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference23 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference22)
                            || (objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference23)
                          || (objSave.SubGroupPreference22 != null && objSave.SubGroupPreference22 == objSave.SubGroupPreference23))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG3");
                        objSave.GroupPreference3 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG31");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference31 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG32");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference32 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG33");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference33 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference32)
                            || (objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference33)
                          || (objSave.SubGroupPreference32 != null && objSave.SubGroupPreference32 == objSave.SubGroupPreference33))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if ((objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference2)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference3)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference3))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Four)
                    {
                        DropDownList ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG1");
                        objSave.GroupPreference1 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG11");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference11 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG12");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference12 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG13");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference13 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference12)
                            || (objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference13)
                           || (objSave.SubGroupPreference12 != null && objSave.SubGroupPreference12 == objSave.SubGroupPreference13))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG2");
                        objSave.GroupPreference2 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG21");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference21 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG22");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference22 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG23");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference23 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference22)
                            || (objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference23)
                          || (objSave.SubGroupPreference22 != null && objSave.SubGroupPreference22 == objSave.SubGroupPreference23))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG3");
                        objSave.GroupPreference3 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG31");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference31 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG32");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference32 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG33");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference33 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference32)
                            || (objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference33)
                          || (objSave.SubGroupPreference32 != null && objSave.SubGroupPreference32 == objSave.SubGroupPreference33))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG4");
                        objSave.GroupPreference4 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG41");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference41 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG42");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference42 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG43");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference43 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference41 != null && objSave.SubGroupPreference41 == objSave.SubGroupPreference42)
                            || (objSave.SubGroupPreference41 != null && objSave.SubGroupPreference41 == objSave.SubGroupPreference43)
                          || (objSave.SubGroupPreference42 != null && objSave.SubGroupPreference42 == objSave.SubGroupPreference43))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if ((objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference2)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference3)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference4)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference3)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference4)
                            || (objSave.GroupPreference3 != null && objSave.GroupPreference3 == objSave.GroupPreference4))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Five)
                    {
                        DropDownList ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG1");
                        objSave.GroupPreference1 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG11");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference11 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG12");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference12 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG13");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference13 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference12)
                            || (objSave.SubGroupPreference11 != null && objSave.SubGroupPreference11 == objSave.SubGroupPreference13)
                           || (objSave.SubGroupPreference12 != null && objSave.SubGroupPreference12 == objSave.SubGroupPreference13))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG2");
                        objSave.GroupPreference2 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG21");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference21 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG22");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference22 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG23");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference23 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference22)
                            || (objSave.SubGroupPreference21 != null && objSave.SubGroupPreference21 == objSave.SubGroupPreference23)
                          || (objSave.SubGroupPreference22 != null && objSave.SubGroupPreference22 == objSave.SubGroupPreference23))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG3");
                        objSave.GroupPreference3 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG31");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference31 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG32");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference32 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG33");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference33 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference32)
                            || (objSave.SubGroupPreference31 != null && objSave.SubGroupPreference31 == objSave.SubGroupPreference33)
                          || (objSave.SubGroupPreference32 != null && objSave.SubGroupPreference32 == objSave.SubGroupPreference33))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG4");
                        objSave.GroupPreference4 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG41");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference41 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG42");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference42 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG43");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference43 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference41 != null && objSave.SubGroupPreference41 == objSave.SubGroupPreference42)
                            || (objSave.SubGroupPreference41 != null && objSave.SubGroupPreference41 == objSave.SubGroupPreference43)
                          || (objSave.SubGroupPreference42 != null && objSave.SubGroupPreference42 == objSave.SubGroupPreference43))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlG5");
                        objSave.GroupPreference5 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG51");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference51 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG52");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference52 = Convert.ToInt16(ddl.SelectedItem.Value);

                        ddl = (DropDownList)gvPreferanceDetail.Rows[RowIndex].FindControl("ddlSG53");
                        if (ddl.Visible == true)
                            objSave.SubGroupPreference53 = Convert.ToInt16(ddl.SelectedItem.Value);

                        if ((objSave.SubGroupPreference51 != null && objSave.SubGroupPreference51 == objSave.SubGroupPreference52)
                            || (objSave.SubGroupPreference51 != null && objSave.SubGroupPreference51 == objSave.SubGroupPreference53)
                            || (objSave.SubGroupPreference52 != null && objSave.SubGroupPreference52 == objSave.SubGroupPreference53))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if ((objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference2)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference3)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference4)
                            || (objSave.GroupPreference1 != null && objSave.GroupPreference1 == objSave.GroupPreference5)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference3)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference4)
                            || (objSave.GroupPreference2 != null && objSave.GroupPreference2 == objSave.GroupPreference5)
                            || (objSave.GroupPreference3 != null && objSave.GroupPreference3 == objSave.GroupPreference4)
                            || (objSave.GroupPreference3 != null && objSave.GroupPreference3 == objSave.GroupPreference5)
                            || (objSave.GroupPreference4 != null && objSave.GroupPreference4 == objSave.GroupPreference5))
                        {
                            Master.ShowMessage(Message.MultiplePreferences.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                }

                bool Result = new RotationalProgramBLL().SaveDivSubDivPreference(objSave);
                if (Result)
                {
                    gvPreferanceDetail.EditIndex = -1;
                    BindPreferenceGrid();
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPreferanceDetail.EditIndex = -1;
                BindPreferenceGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int MaxCount = 0;
            bool? Priority = false;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    MaxCount = lstActualSubGroups.Max();
                    Priority = new RotationalProgramBLL().GetPriority(ProgramID);
                    if (e.Row.Cells.Count >= 23 && Priority == true)
                        e.Row.Cells[23].Visible = true;
                    else if (e.Row.Cells.Count >= 23 && Priority == false)
                        e.Row.Cells[23].Visible = false;

                    if (NoOfGroups == (int)Constants.NoOfGroups.One)
                    {
                        if (lstActualSubGroups.Count() >= (int)Constants.NoOfGroups.One)
                        {
                            if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.Three)
                            {
                                e.Row.Cells[7].Visible = false;
                                e.Row.Cells[8].Visible = false;
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[10].Visible = false;

                                e.Row.Cells[11].Visible = false;
                                e.Row.Cells[12].Visible = false;
                                e.Row.Cells[13].Visible = false;

                                e.Row.Cells[14].Visible = false;
                                e.Row.Cells[15].Visible = false;
                                e.Row.Cells[16].Visible = false;

                                e.Row.Cells[17].Visible = false;
                                e.Row.Cells[18].Visible = false;
                                e.Row.Cells[19].Visible = false;

                                e.Row.Cells[20].Visible = false;
                                e.Row.Cells[21].Visible = false;
                                e.Row.Cells[22].Visible = false;
                            }
                            else if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.Two)
                            {
                                e.Row.Cells[6].Visible = false;

                                e.Row.Cells[7].Visible = false;
                                e.Row.Cells[8].Visible = false;
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[10].Visible = false;

                                e.Row.Cells[11].Visible = false;
                                e.Row.Cells[12].Visible = false;
                                e.Row.Cells[13].Visible = false;

                                e.Row.Cells[14].Visible = false;
                                e.Row.Cells[15].Visible = false;
                                e.Row.Cells[16].Visible = false;

                                e.Row.Cells[17].Visible = false;
                                e.Row.Cells[18].Visible = false;
                                e.Row.Cells[19].Visible = false;

                                e.Row.Cells[20].Visible = false;
                                e.Row.Cells[21].Visible = false;
                                e.Row.Cells[22].Visible = false;
                            }
                            else
                            {
                                e.Row.Cells[4].Visible = false;
                                e.Row.Cells[5].Visible = false;
                                e.Row.Cells[6].Visible = false;

                                e.Row.Cells[7].Visible = false;
                                e.Row.Cells[8].Visible = false;
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[10].Visible = false;

                                e.Row.Cells[11].Visible = false;
                                e.Row.Cells[12].Visible = false;
                                e.Row.Cells[13].Visible = false;

                                e.Row.Cells[14].Visible = false;
                                e.Row.Cells[15].Visible = false;
                                e.Row.Cells[16].Visible = false;

                                e.Row.Cells[17].Visible = false;
                                e.Row.Cells[18].Visible = false;
                                e.Row.Cells[19].Visible = false;

                                e.Row.Cells[20].Visible = false;
                                e.Row.Cells[21].Visible = false;
                                e.Row.Cells[22].Visible = false;
                            }
                        }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Two)
                    {
                        // if (lstActualSubGroups.Count() >= (int)Constants.NoOfGroups.Two)
                        // {
                        //if (MaxCount == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[6].Visible = false;
                        //else if (MaxCount == (int)Constants.NoOfSubGroups.zero)
                        //{
                        //    e.Row.Cells[6].Visible = false;
                        //    e.Row.Cells[5].Visible = false;
                        //    e.Row.Cells[4].Visible = false;
                        //}

                        if (MaxCount == (int)Constants.NoOfSubGroups.Three)
                        {
                            e.Row.Cells[11].Visible = false;
                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;

                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else if (MaxCount == (int)Constants.NoOfSubGroups.Two)
                        {
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[11].Visible = false;
                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;

                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[6].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[4].Visible = false;

                            e.Row.Cells[8].Visible = false;
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[11].Visible = false;
                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;

                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        //  }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Three)
                    {
                        //if (lstActualSubGroups.Count() >= (int)Constants.NoOfGroups.Three)
                        //{
                        //if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[6].Visible = false;
                        //else if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.One)
                        //{
                        //    e.Row.Cells[4].Visible = false;
                        //    e.Row.Cells[5].Visible = false;
                        //    e.Row.Cells[6].Visible = false;
                        //}

                        //if (lstActualSubGroups[1] == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[10].Visible = false;
                        //else if (lstActualSubGroups[1] == (int)Constants.NoOfSubGroups.One)
                        //{
                        //    e.Row.Cells[10].Visible = false;
                        //    e.Row.Cells[9].Visible = false;
                        //    e.Row.Cells[8].Visible = false;
                        //}

                        if (MaxCount == (int)Constants.NoOfSubGroups.Three)
                        {
                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else if (MaxCount == (int)Constants.NoOfSubGroups.Two)
                        {
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[8].Visible = false;

                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[15].Visible = false;
                            e.Row.Cells[16].Visible = false;

                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        //  }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Four)
                    {
                        //if (lstActualSubGroups.Count() >= (int)Constants.NoOfGroups.Four)
                        //{
                        //if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[6].Visible = false;
                        //else if (lstActualSubGroups[0] == (int)Constants.NoOfSubGroups.One)
                        //{
                        //    e.Row.Cells[4].Visible = false;
                        //    e.Row.Cells[5].Visible = false;
                        //    e.Row.Cells[6].Visible = false;
                        //}

                        //if (lstActualSubGroups[1] == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[10].Visible = false;
                        //else if (lstActualSubGroups[1] == (int)Constants.NoOfSubGroups.One)
                        //{
                        //    e.Row.Cells[8].Visible = false;
                        //    e.Row.Cells[9].Visible = false;
                        //    e.Row.Cells[10].Visible = false;
                        //}

                        //if (lstActualSubGroups[2] == (int)Constants.NoOfSubGroups.Two)
                        //    e.Row.Cells[14].Visible = false;
                        //else if (lstActualSubGroups[2] == (int)Constants.NoOfSubGroups.One)
                        //{
                        //    e.Row.Cells[12].Visible = false;
                        //    e.Row.Cells[13].Visible = false;
                        //    e.Row.Cells[14].Visible = false;
                        //}

                        if (MaxCount == (int)Constants.NoOfSubGroups.Three)
                        {
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else if (MaxCount == (int)Constants.NoOfSubGroups.Two)
                        {
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[8].Visible = false;
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[16].Visible = false;
                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        // }
                    }
                    else if (NoOfGroups == (int)Constants.NoOfGroups.Five)
                    {
                        if (MaxCount == (int)Constants.NoOfSubGroups.Two)
                        {
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[18].Visible = false;
                            e.Row.Cells[19].Visible = false;

                            //e.Row.Cells[20].Visible = false;
                            //e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].Visible = false;

                            e.Row.Cells[8].Visible = false;
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[10].Visible = false;

                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;

                            e.Row.Cells[16].Visible = false;
                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;

                            e.Row.Cells[20].Visible = false;
                            e.Row.Cells[21].Visible = false;
                            e.Row.Cells[22].Visible = false;
                        }
                        // }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPreferanceDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvPreferanceDetail.EditIndex = e.NewEditIndex;
                BindPreferenceGrid();
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

        protected void gvPreferanceDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(((Label)gvPreferanceDetail.Rows[e.RowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new RotationalProgramBLL().DeleteDivSubDivPref(ID);
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

        protected void ddlG1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlSG1 = (DropDownList)gvRow.FindControl("ddlSG11");
                DropDownList ddlSG2 = (DropDownList)gvRow.FindControl("ddlSG12");
                DropDownList ddlSG3 = (DropDownList)gvRow.FindControl("ddlSG13");
                long G1 = Convert.ToInt64(ddlControl.SelectedItem.Value);

                long SubGroups = new RotationalProgramBLL().GetSubGroupsOfGroup(ProgramID, G1);

                if (SubGroups > 0)
                {
                    if (SubGroups == 2)
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.ClearSelection();
                        ddlSG3.Visible = false;
                    }
                    else
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = true;
                        ddlSG3.ClearSelection();
                        ddlSG3.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG3.DataValueField = "ID";
                        ddlSG3.DataTextField = "Name";
                        ddlSG3.DataBind();
                    }
                }
                else
                {
                    ddlSG1.ClearSelection();
                    ddlSG1.Visible = false;

                    ddlSG2.ClearSelection();
                    ddlSG2.Visible = false;

                    ddlSG3.ClearSelection();
                    ddlSG3.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlG2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlSG1 = (DropDownList)gvRow.FindControl("ddlSG21");
                DropDownList ddlSG2 = (DropDownList)gvRow.FindControl("ddlSG22");
                DropDownList ddlSG3 = (DropDownList)gvRow.FindControl("ddlSG23");
                long G1 = Convert.ToInt64(ddlControl.SelectedItem.Value);

                long SubGroups = new RotationalProgramBLL().GetSubGroupsOfGroup(ProgramID, G1);

                if (SubGroups > 0)
                {
                    if (SubGroups == 2)
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = false;
                        ddlSG3.ClearSelection();
                    }
                    else
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = true;
                        ddlSG3.ClearSelection();
                        ddlSG3.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG3.DataValueField = "ID";
                        ddlSG3.DataTextField = "Name";
                        ddlSG3.DataBind();
                    }
                }
                else
                {
                    ddlSG1.ClearSelection();
                    ddlSG1.Visible = false;

                    ddlSG2.ClearSelection();
                    ddlSG2.Visible = false;

                    ddlSG3.ClearSelection();
                    ddlSG3.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlG3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlSG1 = (DropDownList)gvRow.FindControl("ddlSG31");
                DropDownList ddlSG2 = (DropDownList)gvRow.FindControl("ddlSG32");
                DropDownList ddlSG3 = (DropDownList)gvRow.FindControl("ddlSG33");
                long G1 = Convert.ToInt64(ddlControl.SelectedItem.Value);

                long SubGroups = new RotationalProgramBLL().GetSubGroupsOfGroup(ProgramID, G1);

                if (SubGroups > 0)
                {
                    if (SubGroups == 2)
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = false;
                        ddlSG3.ClearSelection();
                    }
                    else
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = true;
                        ddlSG3.ClearSelection();
                        ddlSG3.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG3.DataValueField = "ID";
                        ddlSG3.DataTextField = "Name";
                        ddlSG3.DataBind();
                    }
                }
                else
                {
                    ddlSG1.ClearSelection();
                    ddlSG1.Visible = false;

                    ddlSG2.ClearSelection();
                    ddlSG2.Visible = false;

                    ddlSG3.ClearSelection();
                    ddlSG3.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlG4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlSG1 = (DropDownList)gvRow.FindControl("ddlSG41");
                DropDownList ddlSG2 = (DropDownList)gvRow.FindControl("ddlSG42");
                DropDownList ddlSG3 = (DropDownList)gvRow.FindControl("ddlSG43");
                long G1 = Convert.ToInt64(ddlControl.SelectedItem.Value);

                long SubGroups = new RotationalProgramBLL().GetSubGroupsOfGroup(ProgramID, G1);

                if (SubGroups > 0)
                {
                    if (SubGroups == 2)
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = false;
                        ddlSG3.ClearSelection();
                    }
                    else
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = true;
                        ddlSG3.ClearSelection();
                        ddlSG3.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG3.DataValueField = "ID";
                        ddlSG3.DataTextField = "Name";
                        ddlSG3.DataBind();
                    }
                }
                else
                {
                    ddlSG1.ClearSelection();
                    ddlSG1.Visible = false;

                    ddlSG2.ClearSelection();
                    ddlSG2.Visible = false;

                    ddlSG3.ClearSelection();
                    ddlSG3.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlG5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlControl = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlControl.NamingContainer;
                DropDownList ddlSG1 = (DropDownList)gvRow.FindControl("ddlSG51");
                DropDownList ddlSG2 = (DropDownList)gvRow.FindControl("ddlSG52");
                DropDownList ddlSG3 = (DropDownList)gvRow.FindControl("ddlSG53");
                long G1 = Convert.ToInt64(ddlControl.SelectedItem.Value);

                long SubGroups = new RotationalProgramBLL().GetSubGroupsOfGroup(ProgramID, G1);

                if (SubGroups > 0)
                {
                    if (SubGroups == 2)
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = false;
                        ddlSG3.ClearSelection();
                    }
                    else
                    {
                        ddlSG1.Visible = true;
                        ddlSG1.ClearSelection();
                        ddlSG1.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG1.DataValueField = "ID";
                        ddlSG1.DataTextField = "Name";
                        ddlSG1.DataBind();

                        ddlSG2.Visible = true;
                        ddlSG2.ClearSelection();
                        ddlSG2.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG2.DataValueField = "ID";
                        ddlSG2.DataTextField = "Name";
                        ddlSG2.DataBind();

                        ddlSG3.Visible = true;
                        ddlSG3.ClearSelection();
                        ddlSG3.DataSource = Utility.GetSubGroupsOfGroup(G1, SubGroups);
                        ddlSG3.DataValueField = "ID";
                        ddlSG3.DataTextField = "Name";
                        ddlSG3.DataBind();
                    }
                }
                else
                {
                    ddlSG1.ClearSelection();
                    ddlSG1.Visible = false;

                    ddlSG2.ClearSelection();
                    ddlSG2.Visible = false;

                    ddlSG3.ClearSelection();
                    ddlSG3.Visible = false;
                }
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

                        DropDownList ddl = (DropDownList)gvRow.FindControl("ddlG1");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlG2");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlG3");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlG4");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlG5");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlSG11");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG12");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG13");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlSG21");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG22");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG23");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlSG31");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG32");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG33");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlSG41");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG42");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG43");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlSG51");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG52");
                        ddl.Visible = false;
                        ddl = (DropDownList)gvRow.FindControl("ddlSG53");
                        ddl.Visible = false;

                        ddl = (DropDownList)gvRow.FindControl("ddlPriority");
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

        protected void rptrGroup1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                lstGroupChannels = new List<RP_Channel>();
                lstGroupChannels = lstAllChannels.Where(q => q.GroupID == BindingGroup).ToList();
                List<dynamic> lstSubGroups = Utility.GetSubGroupsOfGroup(BindingGroup, -1);
                BindingGroup++;
                IsGroup = 0;
                TotalSum = 0;
                Repeater rptrSubGroups = (Repeater)(e.Item.FindControl("rptrSubGroups"));
                rptrSubGroups.DataSource = lstSubGroups;
                rptrSubGroups.DataBind();
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

        protected void cbEntrngChnls_CheckedChanged(object sender, EventArgs e)
        {
            lstEnteringChnlIDs = new List<long?>();
            try
            {
                foreach (GridViewRow row in gvEnteringChannels.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cbEntrngChnls");
                        if (cb != null && cb.Checked)
                        {
                            Label lblChnlID = (Label)row.FindControl("lblID");
                            if (lblChnlID.Text != "")
                                lstEnteringChnlIDs.Add(Convert.ToInt64(lblChnlID.Text));
                        }
                    }
                }
                lstChannels = new List<dynamic>();
                NoOfGroupsBindingLogic();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEnteringChannels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label ChnlID = (Label)e.Row.FindControl("lblID");
                    CheckBox cb = (CheckBox)e.Row.FindControl("cbEntrngChnls");

                    if (!IsPostBack && lstEnteringChnlIDs.Count() == 0)
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        if (ChnlID != null)
                        {
                            foreach (var v in lstEnteringChnlIDs)
                            {
                                if (v.Value == Convert.ToInt64(ChnlID.Text))
                                    cb.Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSendToSE_Click(object sender, EventArgs e)
        {
            try
            {
                RP_Approval objApproval = new RP_Approval();
                objApproval.RPID = ProgramID;
                objApproval.DesignationFromID = Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID);
                objApproval.DesignationToID = Convert.ToInt64(Constants.Designation.SE);
                objApproval.Status = Constants.RP_SendToSE;
                objApproval.CreatedDate = DateTime.Now;
                objApproval.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                objApproval.ModifiedDate = DateTime.Now;
                objApproval.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                new RotationalProgramBLL().SetApprovelStatus(objApproval, false);
                Response.RedirectPermanent((String.Format("SearchRotationalProgram.aspx?Msg={0}", true)), false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnClosureSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = new RotationalProgramBLL().AddClosureDates(ProgramID, Convert.ToDateTime(txtClosureStartDate.Text), Convert.ToDateTime(txtClosureEndDate.Text));
                if (Result)
                {
                    divClosure.Visible = false;
                    lblClosureStart.Text = Utility.GetFormattedDate(Convert.ToDateTime(txtClosureStartDate.Text));
                    lblClosureEnd.Text = Utility.GetFormattedDate(Convert.ToDateTime(txtClosureEndDate.Text));

                    if (gvPreferanceDetail.Rows.Count > 0)
                        BindPreferenceGrid();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Commented (Dynamic Grid effort)


        //public void BindPreferenceGrid()
        //{

        //    try
        //    {
        //        BoundField DateField = new BoundField();
        //        DateField.HeaderText = "ID";
        //        gvPreferance.Columns.Add(DateField);

        //        DateField = new BoundField();
        //        DateField.HeaderText = "From Date";
        //        gvPreferance.Columns.Add(DateField);

        //        DateField = new BoundField();
        //        DateField.HeaderText = "To Date";
        //        gvPreferance.Columns.Add(DateField);

        //        for (int i = 1; i <= NoOfGroups; i++)
        //        {
        //            BoundField PrefField = new BoundField();
        //            PrefField.HeaderText = "Group " + i.ToString();
        //            gvPreferance.Columns.Add(PrefField);
        //        }

        //        for (int i = 1; i <= NoOfGroups * NoOfSubGroups; i++)
        //        {
        //            BoundField PrefField = new BoundField();
        //            PrefField.HeaderText = "Sub Group " + i.ToString();
        //            gvPreferance.Columns.Add(PrefField);
        //        }

        //        List<dynamic> lstData = new RotationalProgramBLL().GetDivSubDivPreferenceData(ProgramID, NoOfGroups, NoOfSubGroups);
        //        gvPreferance.DataSource = lstData;
        //        gvPreferance.DataBind();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }

        //}

        //protected void gvPreferance_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (e.Row.RowIndex == gvPreferance.EditIndex)
        //            {
        //                Label lblID = new Label();
        //                lblID.ID = "lblID";
        //                lblID.CssClass = "control-label";
        //                lblID.Text = e.Row.DataItem.GetType().GetProperty("ID").GetValue(e.Row.DataItem).ToString();
        //                lblID.Attributes.Add("runat", "server");
        //                e.Row.Cells[1].Controls.Add(lblID);

        //                TextBox txtFromDate = new TextBox();
        //                txtFromDate.ID = "txtFromDate";
        //                txtFromDate.Attributes.Add("runat", "server");
        //                txtFromDate.Text = e.Row.DataItem.GetType().GetProperty("StartDate").GetValue(e.Row.DataItem).ToString();
        //                txtFromDate.CssClass = "date-picker form-control";
        //                e.Row.Cells[2].Controls.Add(txtFromDate);

        //                txtFromDate = new TextBox();
        //                txtFromDate.ID = "txtToDate";
        //                txtFromDate.Attributes.Add("runat", "server");
        //                txtFromDate.Text = e.Row.DataItem.GetType().GetProperty("EndDate").GetValue(e.Row.DataItem).ToString();
        //                txtFromDate.CssClass = "date-picker form-control";
        //                e.Row.Cells[3].Controls.Add(txtFromDate);

        //                DropDownList ddlPref1;
        //                int Cell = 4;

        //                for (int i = 1; i <= NoOfGroups; i++)
        //                {
        //                    ddlPref1 = new DropDownList();
        //                    ddlPref1.ID = "ddlGroup" + i.ToString();
        //                    ddlPref1.Attributes.Add("runat", "server");
        //                    ddlPref1.DataSource = Utility.GetGroups(NoOfGroups);
        //                    ddlPref1.DataTextField = "Name";
        //                    ddlPref1.DataValueField = "ID";
        //                    ddlPref1.DataBind();
        //                    ddlPref1.CssClass = "form-control";
        //                    e.Row.Cells[Cell].Controls.Add(ddlPref1);
        //                    Cell++;
        //                }

        //                for (int i = 1; i <= NoOfGroups * NoOfSubGroups; i++)
        //                {
        //                    ddlPref1 = new DropDownList();
        //                    ddlPref1.ID = "ddlSubGroup" + i.ToString();
        //                    ddlPref1.Attributes.Add("runat", "server");
        //                    ddlPref1.DataSource = Utility.GetSubGroups();
        //                    ddlPref1.DataTextField = "Name";
        //                    ddlPref1.DataValueField = "ID";
        //                    ddlPref1.DataBind();
        //                    ddlPref1.CssClass = "form-control";
        //                    e.Row.Cells[Cell].Controls.Add(ddlPref1);
        //                    Cell++;
        //                }
        //            }
        //            else
        //            {
        //                Label lblFromDate = new Label();
        //                lblFromDate.ID = "lblID";
        //                lblFromDate.CssClass = "control-label";
        //                lblFromDate.Text = e.Row.DataItem.GetType().GetProperty("ID").GetValue(e.Row.DataItem).ToString();
        //                e.Row.Cells[1].Controls.Add(lblFromDate);

        //                lblFromDate = new Label();
        //                lblFromDate.ID = "lblFromDate";
        //                lblFromDate.CssClass = "control-label";
        //                lblFromDate.Text = e.Row.DataItem.GetType().GetProperty("StartDate").GetValue(e.Row.DataItem).ToString();
        //                e.Row.Cells[2].Controls.Add(lblFromDate);

        //                lblFromDate = new Label();
        //                lblFromDate.ID = "lblToDate";
        //                lblFromDate.Text = e.Row.DataItem.GetType().GetProperty("EndDate").GetValue(e.Row.DataItem).ToString();
        //                lblFromDate.CssClass = "control-label";
        //                e.Row.Cells[3].Controls.Add(lblFromDate);

        //                Label lblPref1;
        //                int Cell = 4;
        //                string Name;

        //                for (int i = 1; i <= NoOfGroups; i++)
        //                {
        //                    Name = "GPref" + i.ToString();
        //                    lblPref1 = new Label();
        //                    lblPref1.ID = "ddlGroup" + i.ToString();
        //                    lblPref1.Text = e.Row.DataItem.GetType().GetProperty(Name).GetValue(e.Row.DataItem).ToString();
        //                    lblPref1.DataBind();
        //                    lblPref1.CssClass = "control-label";
        //                    e.Row.Cells[Cell].Controls.Add(lblPref1);
        //                    Cell++;
        //                }

        //                for (int i = 1; i <= NoOfGroups * NoOfSubGroups; i++)
        //                {
        //                    Name = "SGPref" + i.ToString();
        //                    lblPref1 = new Label();
        //                    lblPref1.ID = "ddlSubGroup" + i.ToString();
        //                    lblPref1.Text = e.Row.DataItem.GetType().GetProperty(Name).GetValue(e.Row.DataItem).ToString();
        //                    lblPref1.DataBind();
        //                    lblPref1.CssClass = "control-label";
        //                    e.Row.Cells[Cell].Controls.Add(lblPref1);
        //                    Cell++;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvPreferance_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "Add")
        //        {
        //            //TemplateField DateField = new TemplateField();
        //            //DateField.HeaderText = "From Date";
        //            //gvPreferance.Columns.Add(DateField);

        //            //DateField = new TemplateField();
        //            //DateField.HeaderText = "To Date";
        //            //gvPreferance.Columns.Add(DateField);

        //            //for (int i = 1; i <= NoOfGroups; i++)
        //            //{
        //            //    TemplateField PrefField = new TemplateField();
        //            //    PrefField.HeaderText = "Group " + i.ToString();
        //            //    gvPreferance.Columns.Add(PrefField);
        //            //}

        //            //for (int i = 1; i <= NoOfGroups * NoOfSubGroups; i++)
        //            //{
        //            //    TemplateField PrefField = new TemplateField();
        //            //    PrefField.HeaderText = "Sub Group " + i.ToString();
        //            //    gvPreferance.Columns.Add(PrefField);
        //            //}

        //            List<dynamic> lstData = new RotationalProgramBLL().GetDivSubDivPreferenceData(ProgramID, NoOfGroups, NoOfSubGroups);
        //            gvPreferance.DataSource = lstData;
        //            gvPreferance.DataBind();
        //            gvPreferance.EditIndex = gvPreferance.Rows.Count - 1;
        //            gvPreferance.DataBind();
        //        }

        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvPreferance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        RP_Rotation_DivisionSubDivision ObjSave = new RP_Rotation_DivisionSubDivision();
        //        int RowIndex = e.RowIndex;
        //        Label ddl = (Label)gvPreferance.Rows[RowIndex].Cells[1].Controls[0];

        //        //ObjSave.ID = Convert.ToInt64(((Label)gvPreferance.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
        //        //ObjSave.StartDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].Cells[1].FindControl("txtFromDate")).Text);
        //        //ObjSave.EndDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].Cells[2].FindControl("txtToDate")).Text);

        //        GridViewRow row = (GridViewRow)gvPreferance.Rows[e.RowIndex];

        //        string test = ((TextBox)row.Cells[2].Controls[0]).Text;

        //        ObjSave.ID = Convert.ToInt64(((Label)gvPreferance.Rows[RowIndex].Cells[1].Controls[0]).Text);
        //        ObjSave.StartDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].FindControl("txtFromDate")).Text);
        //        ObjSave.EndDate = Convert.ToDateTime(((TextBox)gvPreferance.Rows[RowIndex].FindControl("txtToDate")).Text);


        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvPreferance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvPreferance.EditIndex = -1;
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvPreferance_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {
        //        gvPreferance.EditIndex = e.NewEditIndex;
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvPreferance_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        #endregion

    }
}