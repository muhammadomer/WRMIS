using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.BLL.Notifications;
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
    public partial class DeliveryDetailsForSECE : BasePage
    {
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
                        if (!string.IsNullOrEmpty(Request.QueryString["AuctionPriceID"]))
                        {
                            hdnAuctionPriceID.Value = Convert.ToString(Request.QueryString["AuctionPriceID"]);
                            long ApprovalAuthorityID = new AuctionBLL().GetApprovalAuthorityByAuctionNoticeID(Convert.ToInt64(hdnAuctionNoticeID.Value));
                            BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value), ApprovalAuthorityID);
                            //BindDeliveryStatus();
                            if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                            {
                                hlBack.NavigateUrl = "~/Modules/Auctions/ApprovalProcessForXEN.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                            }
                            else
                            {
                                hlBack.NavigateUrl = "~/Modules/Auctions/ApprovalProcessForSECE.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                            }
                            
                        }
                          
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionDetailData(long _AuctionNoticeID, long _AuctionPriceID, long ApprovalAuthorityID)
        {
            try
            {
                AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(_AuctionPriceID);
                dynamic mdlAwardedAsset = new AuctionBLL().GetAuctionDetails(mdlAuctionPrice);
                hdnAuctionAssetID.Value = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "AuctionAssetID");
                double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), _AuctionPriceID);
                double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), _AuctionPriceID);
                string DeliveryStatus = new AuctionBLL().GetStatusByNoticeAndPriceID(_AuctionNoticeID, _AuctionPriceID);
                    //Utility.GetDynamicPropertyValue(mdlAwardedAsset,"Status");
                if (TotalTokenMoneySubmitted == null)
                {
                    TotalTokenMoneySubmitted = 0;
                }
                if (TotalRemainingAmountSubmitted == null)
                {
                    TotalRemainingAmountSubmitted = 0;
                }

                double TotalAmountPaid = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "EarnestMoneySubmitted")) + TotalTokenMoneySubmitted + TotalRemainingAmountSubmitted;

                double BalanceAmount = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate")) - TotalAmountPaid;

                Auctions.Controls.Payments.AuctionNoticeName = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "NoticeName");
                Auctions.Controls.Payments.OpeningDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "OpeningDate")));
                Auctions.Controls.Payments.SubmissionDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "SubmissionDate")));
                Auctions.Controls.Payments.Category = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "AssetCategory");
                Auctions.Controls.Payments.SubCategory = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "AssetSubCategory");
                Auctions.Controls.Payments.AssetName = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "AssetName");
                Auctions.Controls.Payments.EarnestMoney = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "EarnestMoney");
                Auctions.Controls.Payments.BidderRate = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate");
                Auctions.Controls.Payments.TokenMoney = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "TokenMoney");
                Auctions.Controls.Payments.TotalTokenMoneyPaid = Convert.ToString(TotalTokenMoneySubmitted);
                Auctions.Controls.Payments.TotalAmountPaid = Convert.ToString(TotalAmountPaid);
                Auctions.Controls.Payments.BalanceAmount = Convert.ToString(BalanceAmount);
                if (!string.IsNullOrEmpty(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "SubmissionFeeDate")))
                {
                    Auctions.Controls.Payments.BalanceSubmissionDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "SubmissionFeeDate")));
                }
                else
                {
                    Auctions.Controls.Payments.BalanceSubmissionDate = "N/A";
                }
                

               
                    
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.SE || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.CE)
                    {
                        BindApprovalAuthorityDropDown(SessionManagerFacade.UserInformation.DesignationID.Value);
                    if (BalanceAmount == 0 && (DeliveryStatus == "Pending" || DeliveryStatus == null))
                    {

                   
                            if (ApprovalAuthorityID == (long)Constants.ApprovalAuthorities.SE)
                            {
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.SE)
                                {
                                    ddlStatus.Enabled = true;
                                    ddlStatus.Attributes.Add("required", "required");
                                    ddlStatus.CssClass = "form-control required";
                                    txtRemarks.Enabled = true;
                                    txtRemarks.Attributes.Add("required", "required");
                                    txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize required";
                                    btnSave.Enabled = true;
                                    //txtRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                                    
                                }
                           
                            }
                            else if (ApprovalAuthorityID == (long)Constants.ApprovalAuthorities.CE)
                            {
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.CE)
                                {
                                    ddlStatus.Enabled = true;
                                    ddlStatus.Attributes.Add("required", "required");
                                    ddlStatus.CssClass = "form-control required";
                                    txtRemarks.Enabled = true;
                                    txtRemarks.Attributes.Add("required", "required");
                                    txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize required";
                                    btnSave.Enabled = true;
                                    //txtRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                                   
                                }
                               
                            }
                        }
                    else if ((BalanceAmount == 0 && (DeliveryStatus == "Approved" || DeliveryStatus == "Delivered" || DeliveryStatus == "Cancelled")))
                    {
                        
                        AC_AuctionStatusDetails mdlAuctionStatusDetail = new AuctionBLL().GetAuctionStatusForSECE(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value), Convert.ToInt32(SessionManagerFacade.UserInformation.DesignationID));
                        ddlStatus.Enabled = false;
                        ddlStatus.Attributes.Remove("required");
                        ddlStatus.CssClass = "form-control";
                        txtRemarks.Enabled = false;
                        txtRemarks.Attributes.Remove("required");
                        txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                        btnSave.Enabled = false;
                        txtRemarks.Text = mdlAuctionStatusDetail.DeliveryStatusRemarks;
                        if (mdlAuctionStatusDetail.DeliveryStatus == "Approved")
                        {
                            ddlStatus.SelectedIndex = 1;
                        }
                        else if (mdlAuctionStatusDetail.DeliveryStatus == "Cancelled")
                        {
                            ddlStatus.SelectedIndex = 2;
                        }
                        string img = mdlAuctionStatusDetail.DeliveryAttachment;
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                FileUploadControl.Visible = false;
                                PreviewImage(FileName[0]);
                            }
                        }
                    }

                     
                    }
                    else if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                    {
                        BindApprovalAuthorityDropDown(3);
                        AC_AuctionStatusDetails mdlAuctionStatusDetail = new AuctionBLL().GetAuctionStatusForSECE(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value), Convert.ToInt32(Constants.Designation.SE));
                        ddlStatus.Enabled = false;
                        ddlStatus.Attributes.Remove("required");
                        ddlStatus.CssClass = "form-control";
                        txtRemarks.Enabled = false;
                        txtRemarks.Attributes.Remove("required");
                        txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                        btnSave.Enabled = false;
                        txtRemarks.Text = mdlAuctionStatusDetail.DeliveryStatusRemarks;
                        if (mdlAuctionStatusDetail.DeliveryStatus == "Approved")
                        {
                            ddlStatus.SelectedIndex = 1;
                        }
                        else if (mdlAuctionStatusDetail.DeliveryStatus == "Cancelled")
                        {
                            ddlStatus.SelectedIndex = 2;
                        }
                        string img = mdlAuctionStatusDetail.DeliveryAttachment;
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                PreviewImage(FileName[0]);
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

        private void BindApprovalAuthorityDropDown(long DesignationID)
        {
            try
            {
                if (DesignationID == (long)Constants.Designation.XEN)
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlStatus, CommonLists.GetStatusForXEN(), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlStatus, CommonLists.GetStatusForSE(), (int)Constants.DropDownFirstOption.Select);
                }
               

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

     

        private void BindDeliveryStatus()
        {
            try
            {
                dynamic StatusData = new AuctionBLL().GetDeliveryStatusData(Convert.ToInt64(hdnAuctionAssetID.Value));
                txtRemarks.Text = Utility.GetDynamicPropertyValue(StatusData, "Remarks");
                
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(Utility.GetDynamicPropertyValue(StatusData, "DeliveryStatus")));

                List<string> lstFileNames = new List<string>();
                lstFileNames.Add(Utility.GetDynamicPropertyValue(StatusData, "Attachment"));
                FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                FileUploadControl.UploadedFilesNames(Configuration.Auctions, lstFileNames);

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
                AC_AuctionStatusDetails mdlAuctionAssets = new AC_AuctionStatusDetails();
                //mdlAuctionAssets.ID = Convert.ToInt64(hdnAuctionAssetID.Value);
                mdlAuctionAssets.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                mdlAuctionAssets.AuctionPriceID = Convert.ToInt64(hdnAuctionPriceID.Value);
                mdlAuctionAssets.DesignationID = (int)SessionManagerFacade.UserInformation.DesignationID;
                if (ddlStatus.SelectedItem.Text.ToUpper() == "DELIVER")
                {
                    mdlAuctionAssets.DeliveryStatus = "Delivered";
                }
                else if (ddlStatus.SelectedItem.Text.ToUpper() == "APPROVE")
                {
                    mdlAuctionAssets.DeliveryStatus = "Approved";
                }
                else if (ddlStatus.SelectedItem.Text.ToUpper() == "CANCEL")
                {
                    mdlAuctionAssets.DeliveryStatus = "Cancelled";
                }
                else
                {
                    mdlAuctionAssets.DeliveryStatus = ddlStatus.SelectedItem.Text;
                }
                
                mdlAuctionAssets.DeliveryStatusRemarks = txtRemarks.Text;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                if (lstNameofFiles.Count > 0)
                    mdlAuctionAssets.DeliveryAttachment = lstNameofFiles[0].Item3;
                mdlAuctionAssets.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlAuctionAssets.CreatedDate = DateTime.Now;


                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsUpdated = new AuctionBLL().UpdateDeliveryStatus(mdlAuctionAssets);

                    if (IsUpdated)
                    {
                        long AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                        long AuctionPriceID = Convert.ToInt64(hdnAuctionPriceID.Value);
                        long AuctionAssetID = Convert.ToInt64(hdnAuctionAssetID.Value);
                        long UserID = SessionManagerFacade.UserInformation.ID;
                        
                        if (ddlStatus.SelectedItem.Text.ToUpper() == "APPROVE")
                        {
                            long EventID = (long)NotificationEventConstants.Auctions.AuctionApproved;
                            NotifyEvent _event = new NotifyEvent();
                            _event.Parameters.Add("AuctionNoticeID", AuctionNoticeID);
                            _event.Parameters.Add("AuctionAssetsID", AuctionAssetID);
                            _event.Parameters.Add("AuctionPriceID", AuctionPriceID);
                            _event.AddNotifyEvent(EventID, UserID);
                            
                        }
                        else if (ddlStatus.SelectedItem.Text.ToUpper() == "CANCEL")
                        {
                            long EventID = (long)NotificationEventConstants.Auctions.AuctionCancel;
                            NotifyEvent _event = new NotifyEvent();
                            _event.Parameters.Add("AuctionNoticeID", AuctionNoticeID);
                            _event.Parameters.Add("AuctionAssetsID", AuctionAssetID);
                            _event.Parameters.Add("AuctionPriceID", AuctionPriceID);
                            _event.AddNotifyEvent(EventID, UserID);
                        }
                    }
                    transaction.Complete();
                    
                    //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                Auctions.ApprovalProcessForSECE.IsSaved = true;
                Response.Redirect("~/Modules/Auctions/ApprovalProcessForSECE.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value), false);
  
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
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
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = 1;
            FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.Auctions, lstName);

        }
    }
}