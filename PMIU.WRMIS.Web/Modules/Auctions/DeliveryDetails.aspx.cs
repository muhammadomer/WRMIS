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
    public partial class DeliveryDetails : BasePage
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
                            hlBack.NavigateUrl = "~/Modules/Auctions/ApprovalProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
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
                string DeliveryStatus = Utility.GetDynamicPropertyValue(mdlAwardedAsset,"Status");
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
                

                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                {
                    BindApprovalAuthorityDropDown(SessionManagerFacade.UserInformation.DesignationID.Value);
                    if (BalanceAmount == 0 && DeliveryStatus == "Approved")
                    {
                        ddlStatus.Enabled = true;
                        ddlStatus.Attributes.Add("required", "required");
                        ddlStatus.CssClass = "form-control required";
                        txtRemarks.Enabled = true;
                        txtRemarks.Attributes.Add("required", "required");
                        txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize required";
                        btnSave.Enabled = true;
                        ddlStatus.Items.Insert(1, new ListItem("Approved", "0"));
                        ddlStatus.SelectedIndex = 1;
                        txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                        string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                PreviewImage(FileName[0]);
                            } 
                        }
                      
                        
                    }
                    else if (BalanceAmount == 0 && DeliveryStatus == "Cancelled")
                    {

                        ddlStatus.Enabled = true;
                        ddlStatus.Attributes.Add("required", "required");
                        ddlStatus.CssClass = "form-control required";
                        txtRemarks.Enabled = true;
                        txtRemarks.Attributes.Add("required", "required");
                        txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize required";
                        btnSave.Enabled = true;
                        txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                        ddlStatus.Items.Insert(1, new ListItem("Cancelled", "0"));
                        ddlStatus.SelectedIndex = 1;
                        string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                        //hlImage.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, img);
                        //hlImage.Text = img.Substring(img.LastIndexOf('_') + 1);
                        //hlImage.Attributes["FullName"] = img;
                        //hlImage.Visible = true;
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                PreviewImage(FileName[0]);
                            }
                        }
                      
                    }
                    else
                    {
                      
                        txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                        if (DeliveryStatus == "Pending" || DeliveryStatus == null)
                        {
                            ddlStatus.SelectedIndex = 1;
                            ddlStatus.Enabled = false;
                            ddlStatus.Attributes.Remove("required");
                            ddlStatus.CssClass = "form-control";
                            txtRemarks.Enabled = false;
                            txtRemarks.Attributes.Remove("required");
                            txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                            btnSave.Enabled = false;
                        }
                        else if (DeliveryStatus == "Delivered")
                        {
                            ddlStatus.SelectedIndex = 2;
                            ddlStatus.Enabled = false;
                            ddlStatus.Attributes.Remove("required");
                            ddlStatus.CssClass = "form-control";
                            txtRemarks.Enabled = false;
                            txtRemarks.Attributes.Remove("required");
                            txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                            btnSave.Enabled = false;
                        }
                        string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                PreviewImage(FileName[0]);
                            }
                        }
                     
                    }
                }
                else
                {
                    BindApprovalAuthorityDropDown(SessionManagerFacade.UserInformation.DesignationID.Value);
                    if (BalanceAmount == 0 && (DeliveryStatus == "Pending" || DeliveryStatus == "Cancelled" || DeliveryStatus == null))
                    {

                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.SE || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.CE)
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
                                    txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                                    if (DeliveryStatus == "Pending")
                                    {
                                        ddlStatus.Items.Insert(1, new ListItem("Pending", "0"));
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    else if (DeliveryStatus == "Cancelled")
                                    {
                                        ddlStatus.SelectedIndex = 2;
                                    }
                                    else if (DeliveryStatus == "Delivered")
                                    {
                                         ddlStatus.Items.Insert(1, new ListItem("Delivered", "0"));
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    else
                                    {
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                                    if (!string.IsNullOrEmpty(img))
                                    {
                                        List<string> FileName = new List<string>();
                                        FileName.Add(img);
                                        if (FileName.Count > 0)
                                        {
                                            PreviewImage(FileName[0]);
                                        }
                                    }
                                    
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
                                    txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                                    if (DeliveryStatus == "Pending")
                                    {
                                        ddlStatus.Items.Insert(1, new ListItem("Pending", "0"));
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    else if (DeliveryStatus == "Cancelled")
                                    {
                                        ddlStatus.SelectedIndex = 2;
                                    }
                                    else if (DeliveryStatus == "Delivered")
                                    {
                                        ddlStatus.Items.Insert(1, new ListItem("Delivered", "0"));
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    else
                                    {
                                        ddlStatus.SelectedIndex = 1;
                                    }
                                    string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                                    if (!string.IsNullOrEmpty(img))
                                    {
                                        List<string> FileName = new List<string>();
                                        FileName.Add(img);
                                        if (FileName.Count > 0)
                                        {
                                            PreviewImage(FileName[0]);
                                        } 
                                    }
                                   
                                }
                               
                            }
                        }

                     
                    }
                    else
                    {
                        ddlStatus.Enabled = false;
                        ddlStatus.Attributes.Remove("required");
                        ddlStatus.CssClass = "form-control";
                        txtRemarks.Enabled = false;
                        txtRemarks.Attributes.Remove("required");
                        txtRemarks.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                        btnSave.Enabled = false;
                        txtApprovalAuthRemarks.Text = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "Remarks");
                        if (DeliveryStatus == "Pending")
                        {
                            ddlStatus.Items.Insert(1, new ListItem("Pending", "0"));
                            ddlStatus.SelectedIndex = 1;
                        }
                        else if (DeliveryStatus == "Cancelled")
                        {
                            ddlStatus.SelectedIndex = 2;
                        }
                        else if (DeliveryStatus == "Delivered")
                        {
                            ddlStatus.Items.Insert(1, new ListItem("Delivered", "0"));
                            ddlStatus.SelectedIndex = 1;
                        }
                        else
                        {
                            ddlStatus.SelectedIndex = 1;
                        }
                        string img = Utility.GetDynamicPropertyValue(mdlAwardedAsset, "StatusAttachment");
                        if (!string.IsNullOrEmpty(img))
                        {
                            List<string> FileName = new List<string>();
                            FileName.Add(img);
                            if (FileName.Count > 0)
                            {
                                PreviewImage(FileName[0]);
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
                AC_AuctionAssets mdlAuctionAssets = new AC_AuctionAssets();
                mdlAuctionAssets.ID = Convert.ToInt64(hdnAuctionAssetID.Value);
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
                mdlAuctionAssets.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlAuctionAssets.ModifiedDate = DateTime.Now;


                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsUpdated = new AuctionBLL().UpdateDeliveryStatus(mdlAuctionAssets);
                    if (ddlStatus.SelectedItem.Text.ToUpper() == "DELIVER")
                    {
                        List<long> AssetItemIDs = new AuctionBLL().GetAssetItemIDs(Convert.ToInt64(hdnAuctionAssetID.Value));

                        foreach (var item in AssetItemIDs)
                        {
                            string AssetType = new AuctionBLL().GetAssteTypeByAssetItemID(item);
                            AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(Convert.ToInt64(hdnAuctionPriceID.Value));
                            string BidderName = new AuctionBLL().GetBidderNameByBidderID(mdlAuctionPrice.AuctionBidderID.Value);
                            if (AssetType.ToUpper() == "LOT")
                            {
                                int? LotQuantityAuctiond = new AuctionBLL().GetLotQuantityByAuctionAssetID(Convert.ToInt64(hdnAuctionAssetID.Value),item);
                                int AvailableLotQuantity = new AuctionBLL().GetAvailableLotQuantity(item);
                                int QuantityToUpdate = AvailableLotQuantity - LotQuantityAuctiond.Value;
                                if (QuantityToUpdate > 0)
                                {
                                    bool Isupdated = new AuctionBLL().UpdateLotQuantityInAsset(item, QuantityToUpdate);
                                }
                                else if (QuantityToUpdate <= 0)
                                {
                                    QuantityToUpdate = 0;
                                    bool IsQupdated = new AuctionBLL().UpdateLotQuantityInAsset(item, QuantityToUpdate);
                                    bool isUpdated = new AuctionBLL().UpdateAuctionStatus(item);
                                }

                                AM_AssetInspectionLot mdlInspectionLot = new AM_AssetInspectionLot();
                                mdlInspectionLot.AssetItemID = item;
                                mdlInspectionLot.InspectionDate = DateTime.Now;
                                if (LotQuantityAuctiond != null)
                                {
                                    mdlInspectionLot.Quantity = LotQuantityAuctiond.Value;
                                }
                                mdlInspectionLot.Status = "Auctioned";
                                mdlInspectionLot.CurrentAssetValue = mdlAuctionPrice.BidderRate;
                                mdlInspectionLot.Remarks = BidderName;
                                mdlInspectionLot.Source = Configuration.RequestSource.RequestFromWeb;
                                mdlInspectionLot.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                                mdlInspectionLot.CreatedByDesigID = SessionManagerFacade.UserInformation.DesignationID.Value;
                                mdlInspectionLot.CreatedDate = DateTime.Now;
                                bool IsSaved = new AuctionBLL().AddLotInspectionInAsset(mdlInspectionLot);

                            }
                            else
                            {
                                bool isUpdated = new AuctionBLL().UpdateAuctionStatus(item);
                                AM_AssetInspectionInd mdlAssetInspectionInd = new AM_AssetInspectionInd();
                                mdlAssetInspectionInd.AssetItemID = item;
                                mdlAssetInspectionInd.InspectionDate = DateTime.Now;
                                mdlAssetInspectionInd.Status = "Auctioned";
                                mdlAssetInspectionInd.CurrentAssetValue = mdlAuctionPrice.BidderRate;
                                mdlAssetInspectionInd.Remarks = BidderName;
                                mdlAssetInspectionInd.Source = Configuration.RequestSource.RequestFromWeb;
                                mdlAssetInspectionInd.CreatedDate = DateTime.Now;
                                mdlAssetInspectionInd.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                                mdlAssetInspectionInd.CreatedByDesigID = SessionManagerFacade.UserInformation.DesignationID.Value;
                                bool IsSaved = new AuctionBLL().AddIndividualInspectionInAsset(mdlAssetInspectionInd);
                            }
                            
                        }
                    }
                    
                    transaction.Complete();
                    
                    //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                Auctions.ApprovalProcess.IsSaved = true;
                Response.Redirect("~/Modules/Auctions/ApprovalProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value), false);
  
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