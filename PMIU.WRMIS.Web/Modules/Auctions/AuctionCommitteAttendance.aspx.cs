using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class AuctionCommitteAttendance : BasePage
    {
        #region ViewState Constants

        string AuctionMembersList = "AuctionMembersList";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindCommiteeMembersGrid(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindAuctionCommitteMembersData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidders.HRef = string.Format("~/Modules/Auctions/BiddersAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidding.HRef = string.Format("~/Modules/Auctions/BiddingProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidderSelection.HRef = string.Format("~/Modules/Auctions/BidderSelection.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        if (!base.CanAdd)
                        {
                            txtOpenedBy.Visible = false;
                            txtDesignation.Visible = false;
                            txtMonitordByName.Enabled = false;
                            txtMonitordByName.Attributes.Remove("required");
                            txtMonitordByName.CssClass = "form-control";
                            ddlMonitoredBy.Enabled = false;
                            ddlMonitoredBy.Attributes.Remove("required");
                            ddlMonitoredBy.CssClass = "form-control";
                            lblOpenedBy.Visible = false;
                            lblbDesignation.Visible = false;
                            FileUploadControl.Visible = false;
                            lblAttachment.Visible = false;
                            btnSave.Visible = false;
                        }
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuctionCommiteeMembers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtMemberNameG = (TextBox)e.Row.FindControl("txtMemberName");
                    TextBox txtDesignationG = (TextBox)e.Row.FindControl("txtDesignation");
                    if (!base.CanAdd)
                    {
                        txtMemberNameG.Enabled = false;
                        txtMemberNameG.Attributes.Remove("required");
                        txtMemberNameG.CssClass = "form-control";
                        txtDesignationG.Enabled = false;
                        txtDesignationG.Attributes.Remove("required");
                        txtDesignationG.CssClass = "form-control";
                    }
                    


                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionDetailData(long _AuctionNoticeID)
        {
            try
            {
                AC_AuctionNotice mdlAuctionData = new AuctionBLL().GetAuctionDetailsByID(_AuctionNoticeID);
                string AuctionNotice = mdlAuctionData.AuctionTitle;
                string OpeningDate = Utility.GetFormattedDate(mdlAuctionData.OpeningDate);
                string SubmissionDate = Utility.GetFormattedDate(mdlAuctionData.SubmissionDate);

                Auctions.Controls.AuctionNotice.AuctionNoticeName = AuctionNotice;
                Auctions.Controls.AuctionNotice.OpeningDate = OpeningDate;
                Auctions.Controls.AuctionNotice.SubmissionDate = SubmissionDate;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindCommiteeMembersGrid(long _AuctionNoticeID)
        {
            try
            {
                List<object> lstMembers = new List<object>();
                if (ViewState[AuctionMembersList] != null)
                {
                    gvAuctionCommiteeMembers.DataSource = (List<Member>)ViewState[AuctionMembersList];
                }
                else
                {
                    gvAuctionCommiteeMembers.DataSource = lstMembers;
                }
                gvAuctionCommiteeMembers.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuctionCommiteeMembers_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddMember")
                {
                    List<dynamic> lstCommitteMembers = new AuctionBLL().GetCommiteeMembersByNoticeID(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    //lstGaugeInspection.Add(GetNewScheduleDetail());
                    lstCommitteMembers.Insert(0, GetNewMember());
                    gvAuctionCommiteeMembers.PageIndex = gvAuctionCommiteeMembers.PageCount;
                    gvAuctionCommiteeMembers.DataSource = lstCommitteMembers;
                    gvAuctionCommiteeMembers.DataBind();

                    gvAuctionCommiteeMembers.EditIndex = 0;//gvGaugeInspection.Rows.Count - 1;
                    gvAuctionCommiteeMembers.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuctionCommiteeMembers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAuctionCommiteeMembers.EditIndex = -1;
                BindCommiteeMembersGrid(Convert.ToInt64(hdnAuctionNoticeID.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuctionCommiteeMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = 0;
                if (ViewState[AuctionMembersList] != null)
                {
                    List<Member> lstMembers = (List<Member>)ViewState[AuctionMembersList];
                    List<Member> lstNew = new List<Member>();
                    if (lstMembers.Count > 0)
                    {
                        for (int i = 0; i < lstMembers.Count; i++)
                        {
                            TextBox MemberName = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[0].FindControl("txtMemberName");
                            TextBox Designation = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[1].FindControl("txtDesignation");
                            lstNew.Add(new Member { MemberName = MemberName.Text, Designation = Designation.Text });
                            rowIndex++;
                        }
                        lstNew.RemoveAt(e.RowIndex);
                        ViewState[AuctionMembersList] = lstNew;
                        gvAuctionCommiteeMembers.DataSource = lstNew;
                        gvAuctionCommiteeMembers.DataBind();

                    }
                }



            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void AddRow_Grid(object sender, EventArgs e)
        {
            try
            {

                int rowIndex = 0;
                if (ViewState[AuctionMembersList] != null)
                {
                    List<Member> lstAdvertisementSource = (List<Member>)ViewState[AuctionMembersList];
                    List<Member> lstNew = new List<Member>();
                    if (lstAdvertisementSource.Count > 0)
                    {
                        for (int i = 0; i < lstAdvertisementSource.Count; i++)
                        {
                            TextBox MemberName = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[0].FindControl("txtMemberName");
                            TextBox Designation = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[1].FindControl("txtDesignation");
                            lstNew.Add(new Member { MemberName = MemberName.Text, Designation = Designation.Text });
                            rowIndex++;
                        }
                        lstNew.Add(new Member { MemberName = "", Designation = "" });
                        ViewState[AuctionMembersList] = lstNew;
                        gvAuctionCommiteeMembers.DataSource = lstNew;
                        gvAuctionCommiteeMembers.DataBind();

                    }
                    else
                    {
                        List<Member> lstAdvertisementSourceNew = new List<Member>();
                        lstAdvertisementSourceNew.Insert(0, GetNewMemberViewState());
                        ViewState[AuctionMembersList] = lstAdvertisementSourceNew;
                        gvAuctionCommiteeMembers.DataSource = lstAdvertisementSourceNew;
                        gvAuctionCommiteeMembers.DataBind();
                    }

                }
                else
                {
                    List<Member> lstAdvertisementSource = new List<Member>();
                    lstAdvertisementSource.Insert(0, GetNewMemberViewState());
                    ViewState[AuctionMembersList] = lstAdvertisementSource;
                    gvAuctionCommiteeMembers.DataSource = lstAdvertisementSource;
                    gvAuctionCommiteeMembers.DataBind();
                }



            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private Member GetNewMemberViewState()
        {
            Member MemberDet = new Member
            {

                MemberName = string.Empty,
                Designation = string.Empty,
            };
            return MemberDet;
        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private object GetNewMember()
        {
            object MemberInfo = new
            {
                ID = 0,
                AuctionNoticeID = 0,
                AuctionOpeningID = 0,
                MemberName = string.Empty,
                Designation = string.Empty
            };
            return MemberInfo;
        }
        private void BindAuctionCommitteMembersData(long _AuctionNoticeID)
        {
            try
            {
                AC_AuctionOpening mdlAuctionOpening = new AuctionBLL().getAuctionOpeningDataByNoticeID(_AuctionNoticeID);
                hdnAuctionOpeningID.Value = Convert.ToString(mdlAuctionOpening.ID);
                txtMonitordByName.Text = mdlAuctionOpening.MoniteringPerson;
                if (mdlAuctionOpening.MoniteredBy.ToUpper() == "ADM")
                {
                    ddlMonitoredBy.SelectedValue = "12";
                }
                else if (mdlAuctionOpening.MoniteredBy.ToUpper() == "MA")
                {
                    ddlMonitoredBy.SelectedValue = "13";
                }

                txtOpenedBy.Text = mdlAuctionOpening.OpenedBy;
                txtDesignation.Text = mdlAuctionOpening.Designation;
                if (!string.IsNullOrEmpty(mdlAuctionOpening.ACMAttendanceFile))
                {
                    List<string> File = new List<string>();
                    File.Add(mdlAuctionOpening.ACMAttendanceFile);
                    if (File.Count > 0)
                    {
                        PreviewImage(File[0]);
                    } 
                }
              
                

                //hlAttendanceSheet.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, mdlAuctionOpening.ACMAttendanceFile);
                //hlAttendanceSheet.Text = mdlAuctionOpening.ACMAttendanceFile.Substring(mdlAuctionOpening.ACMAttendanceFile.LastIndexOf('_') + 1);
                //hlAttendanceSheet.Attributes["FullName"] = mdlAuctionOpening.ACMAttendanceFile;
                //hlAttendanceSheet.Visible = true;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Script1", "RemoveRequired();", true);

                List<dynamic> lstMembers = new AuctionBLL().GetCommitteMembersByNoticeID(_AuctionNoticeID);
                List<Member> PData = new List<Member>();
                foreach (var item in lstMembers)
                {
                    PData.Add(new Member { MemberName = Utility.GetDynamicPropertyValue(item, "MemberName"), Designation = Utility.GetDynamicPropertyValue(item, "Designation") });
                }
                ViewState[AuctionMembersList] = PData;
                gvAuctionCommiteeMembers.DataSource = lstMembers;
                gvAuctionCommiteeMembers.DataBind();

                //if (!IsEditMode)
                //{
                //    BinScreenControls();
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvAuctionCommiteeMembers.Rows.Count == 0)
                {
                    Master.ShowMessage(Message.EnterAuctionCommitteMembers.Description, SiteMaster.MessageType.Error);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Calculation();", true);
                    return;
                }

                AC_AuctionOpening MdlAuctionOpening = new AC_AuctionOpening();
                if (hdnAuctionOpeningID.Value != "0")
                {
                    MdlAuctionOpening.ID = Convert.ToInt64(hdnAuctionOpeningID.Value);
                    MdlAuctionOpening.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    MdlAuctionOpening.ModifiedDate = DateTime.Now;
                }
                //if hlImage.Text 
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                if (lstNameofFiles.Count > 0)
                {
                    MdlAuctionOpening.ACMAttendanceFile = lstNameofFiles[0].Item3;
                }
                    
                else
                {
                    //MdlAuctionOpening.ACMAttendanceFile = hlAttendanceSheet.Attributes["FullName"];
                }
                MdlAuctionOpening.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                MdlAuctionOpening.MoniteredBy = Convert.ToString(ddlMonitoredBy.SelectedItem.Text);
                MdlAuctionOpening.MoniteringPerson = txtMonitordByName.Text;
                MdlAuctionOpening.OpenedBy = txtOpenedBy.Text;
                MdlAuctionOpening.Designation = txtDesignation.Text;
                MdlAuctionOpening.CreatedDate = DateTime.Now;
                MdlAuctionOpening.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;


                List<Member> lstMembers = new List<Member>();

                int rowIndex = 0;
                List<Member> lstMembersSource = (List<Member>)ViewState[AuctionMembersList];
                List<Member> lstNew = new List<Member>();
                if (lstMembersSource != null)
                {
                    if (lstMembersSource.Count > 0)
                    {
                        for (int i = 0; i < lstMembersSource.Count; i++)
                        {
                            TextBox MemberName = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[0].FindControl("txtMemberName");
                            TextBox Designation = (TextBox)gvAuctionCommiteeMembers.Rows[rowIndex].Cells[1].FindControl("txtDesignation");
                            lstNew.Add(new Member { MemberName = MemberName.Text, Designation = Designation.Text });
                            rowIndex++;
                        }

                        ViewState[AuctionMembersList] = lstNew;
                        lstMembers = (List<Member>)ViewState[AuctionMembersList];

                    }

                }

                bool IsSaved = false;
                long AuctionOpeningID = 0;

                using (TransactionScope transaction = new TransactionScope())
                {
                    AuctionOpeningID = new AuctionBLL().SaveAuctionOpening(MdlAuctionOpening);
                    if (hdnAuctionOpeningID.Value != "0")
                    {
                        bool isDeleted = new AuctionBLL().DeleteCommitteMembersforUpdation(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    }
                    if (lstMembersSource != null)
                    {
                        foreach (var item in lstMembers)
                        {
                            AC_AuctionCommiteeMembers AuctionMembers = new AC_AuctionCommiteeMembers();

                            if (hdnAuctionOpeningID.Value != "0")
                            {
                                AuctionMembers.AuctionOpeningID = Convert.ToInt64(hdnAuctionOpeningID.Value);
                            }
                            else
                            {
                                AuctionMembers.AuctionOpeningID = AuctionOpeningID;
                            }
                            AuctionMembers.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                            AuctionMembers.MemberName = item.MemberName;
                            AuctionMembers.Designation = item.Designation;
                            AuctionMembers.CreatedDate = DateTime.Now.Date;
                            AuctionMembers.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;

                            IsSaved = new AuctionBLL().SaveCommittemembers(AuctionMembers);


                        }
                    }

                    transaction.Complete();

                    Auctions.BiddersAttendance.IsSaved = true;
                    Response.Redirect("~/Modules/Auctions/BiddersAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value), false); 
                }


                //Works.AddWorks.IsSaved = true;
                //Response.Redirect("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID, false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        [Serializable]
        public class Member
        {
            public string MemberName { get; set; }
            public string Designation { get; set; }

        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Auctions);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void PreviewImage(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            //lnkFile.Text = "File: " + filename;
            //lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = 1;
            FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.Auctions, lstName);

        }
    }
}