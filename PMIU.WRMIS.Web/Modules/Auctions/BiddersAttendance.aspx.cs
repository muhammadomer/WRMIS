using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class BiddersAttendance : BasePage
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

                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        //hlNext.NavigateUrl = "~/Modules/Auctions/BiddingProcess.aspx?AuctionNoticeID="+Convert.ToInt64(hdnAuctionNoticeID.Value);
                        hlBack.NavigateUrl = "~/Modules/Auctions/AuctionCommitteAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindAssetsDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindBiddersGrid();
                        anchCommittee.HRef = string.Format("~/Modules/Auctions/AuctionCommitteAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidding.HRef = string.Format("~/Modules/Auctions/BiddingProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidderSelection.HRef = string.Format("~/Modules/Auctions/BidderSelection.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        if (!base.CanAdd)
                        {
                            ddlAssets.Visible = false;
                            lblAsset.Visible = false;
                            btnSave.Visible = false;
                            BindBiddersGridForViewMode();
                            FileUploadControl.Visible = false;
                        }
                    }
                }
            }
            catch (WRException exp)
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
        private void BindAssetsDropDown(long _AuctionNoticeID)
        {
            try
            {
                //List<dynamic> lstAssets = new AuctionBLL().GetAssetsByNoticeID(_AuctionNoticeID);
                Dropdownlist.BindDropdownlist<List<object>>(ddlAssets, new AuctionBLL().GetAssetsByNoticeID(_AuctionNoticeID), (int)Constants.DropDownFirstOption.Select);

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindBiddersGrid()
        {
            try
            {
                //string FilePath = new AuctionBLL().GetBidderAttendanceAttachment(Convert.ToInt64(hdnAuctionNoticeID.Value));
                //if (!string.IsNullOrEmpty(FilePath))
                //{
                ////    hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, FilePath);
                ////hlAttachment.Text = FilePath.Substring(FilePath.LastIndexOf('_') + 1);
                ////hlAttachment.Attributes["FullName"] = FilePath;
                ////hlAttachment.Visible = true;
                //    List<string> File = new List<string>();
                //    File.Add(FilePath);
                //    PreviewImage(File[0]);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}
                List<dynamic> lstBidders = new List<dynamic>();//new AuctionBLL().GetBiddersByNoticeAndAssetID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                gvBiddersAttendance.DataSource = lstBidders;
                gvBiddersAttendance.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindBiddersGridForViewMode()
        {
            try
            {
                //string FilePath = new AuctionBLL().GetBidderAttendanceAttachment(Convert.ToInt64(hdnAuctionNoticeID.Value));
                //if (!string.IsNullOrEmpty(FilePath))
                //{
                //    //hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, FilePath);
                //    //hlAttachment.Text = FilePath.Substring(FilePath.LastIndexOf('_') + 1);
                //    //hlAttachment.Attributes["FullName"] = FilePath;
                //    //lblAtt.InnerText = "Attachment";
                //    //hlAttachment.Visible = true;
                //    List<string> File = new List<string>();
                //    File.Add(FilePath);
                //    PreviewImage(File[0]);
                    
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}
                List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetIDForViewMode(Convert.ToInt64(hdnAuctionNoticeID.Value));
                gvBiddersAttendance.DataSource = lstBidders;
                gvBiddersAttendance.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlAssets.SelectedItem.Value))
                {
                    List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                    gvBiddersAttendance.DataSource = lstBidders;
                    gvBiddersAttendance.DataBind();

                    string FilePath = new AuctionBLL().GetBidderAttendanceAttachment(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        List<string> File = new List<string>();
                        File.Add(FilePath);
                        if (File.Count > 0)
                        {
                            PreviewImage(File[0]); 
                        }
                        
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                    }
                }
              

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<AC_BidderEarnestMoney> lstNew = new List<AC_BidderEarnestMoney>();
                bool IsAttUpdated = true;
                foreach (GridViewRow gvrow in gvBiddersAttendance.Rows)
                {
                    AC_BidderEarnestMoney mdlBidderEarnestMoney = new AC_BidderEarnestMoney();
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    CheckBox chkSelect = (CheckBox)gvrow.FindControl("chkAlternate");
                    if (chk.Checked)
                    {
                        mdlBidderEarnestMoney.Attended = true;
                        TextBox BidderRepresentative = (TextBox)gvrow.FindControl("txtAlternateName");
                        TextBox AlternateRemarks = (TextBox)gvrow.FindControl("txtAlternateRemarks");
                        Label ID = (Label)gvrow.FindControl("ID");

                        mdlBidderEarnestMoney.ID = Convert.ToInt64(ID.Text);
                        mdlBidderEarnestMoney.BidderRepresentative = BidderRepresentative.Text;
                        mdlBidderEarnestMoney.Remarks = AlternateRemarks.Text;
                        mdlBidderEarnestMoney.ModifiedDate = DateTime.Now;
                        mdlBidderEarnestMoney.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        lstNew.Add(mdlBidderEarnestMoney);
                    }
                   
                }
                //if hlImage.Text 
                AC_AuctionOpening mdlAuctionOpening = new AC_AuctionOpening();
                mdlAuctionOpening.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                if (lstNameofFiles.Count > 0)
                {
                    mdlAuctionOpening.BidderAttendanceFile = lstNameofFiles[0].Item3;
                    IsAttUpdated = true;
                }
                else
                {
                   // mdlAuctionOpening.BidderAttendanceFile = hlAttachment.Attributes["FullName"];
                    IsAttUpdated = false;
                }
                mdlAuctionOpening.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                mdlAuctionOpening.ModifiedDate = DateTime.Now;

                AuctionBLL Auctionbll = new AuctionBLL();
                using (TransactionScope transaction = new TransactionScope())
                {

                    for (int i = 0; i < lstNew.Count; i++)
                    {
                        Auctionbll.UpdateBidderAttendance(lstNew.ElementAt(i));
                    }
                    Auctionbll.UpdateBidderAttendanceAttachement(mdlAuctionOpening,IsAttUpdated);

                    transaction.Complete();
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                    gvBiddersAttendance.DataSource = lstBidders;
                    gvBiddersAttendance.DataBind();
                    string FilePath = new AuctionBLL().GetBidderAttendanceAttachment(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        List<string> File = new List<string>();
                        File.Add(FilePath);
                        if (File.Count > 0)
                        {
                            PreviewImage(File[0]);
                        }
                        
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvBiddersAttendance_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    CheckBox chkAllSelect = (CheckBox)e.Row.FindControl("chkAllSelect");
                    if (!base.CanAdd)
                    {
                        chkAllSelect.Enabled = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    TextBox txtAlternateName = (TextBox)e.Row.FindControl("txtAlternateName");
                    TextBox txtAlternateRemarks = (TextBox)e.Row.FindControl("txtAlternateRemarks");
                    if (!base.CanAdd)
                    {
                        chkSelect.Enabled = false;
                        txtAlternateName.Enabled = false;
                        txtAlternateName.Attributes.Remove("required");
                        txtAlternateName.CssClass = "form-control";
                        txtAlternateRemarks.Enabled = false;
                        txtAlternateRemarks.Attributes.Remove("required");
                        txtAlternateRemarks.CssClass = "form-control";
                    }



                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
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