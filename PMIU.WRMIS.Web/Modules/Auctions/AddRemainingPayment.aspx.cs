using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class AddRemainingPayment : BasePage
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
                        if (!string.IsNullOrEmpty(Request.QueryString["AuctionPriceID"]))
                        {
                            if (IsSaved)
                            {
                                Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                                _IsSaved = false; // Reset flag after displaying message.
                            }
                            hlBack.NavigateUrl = "~/Modules/Auctions/AddPayment.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                            hdnAuctionPriceID.Value = Convert.ToString(Request.QueryString["AuctionPriceID"]);
                            //BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                            BindPaymentsGridView(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                            //List<dynamic> lstAwardedAssets = new AuctionBLL().GetAuctionItemsForPayments(Convert.ToInt64(hdnAuctionNoticeID.Value));
                            //gvPayment.DataSource = lstAwardedAssets;
                            //gvPayment.DataBind();
                        }


                    }
                }
                BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRemainingAmount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddPayment")
                {
                    List<dynamic> lstPayments = new AuctionBLL().GetRemainingpaymentsbyNoticeID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value)); ;
                    lstPayments.Insert(0, GetNewPaymentDetail());
                    gvRemainingAmount.PageIndex = gvRemainingAmount.PageCount;
                    gvRemainingAmount.DataSource = lstPayments;
                    gvRemainingAmount.DataBind();

                    gvRemainingAmount.EditIndex = 0;//gvRemainingAmount.Rows.Count - 1;
                    gvRemainingAmount.DataBind();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvRemainingAmount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRemainingAmount.EditIndex = -1;
                BindPaymentsGridView(Convert.ToInt64(hdnAuctionNoticeID.Value),Convert.ToInt64(hdnAuctionPriceID.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvRemainingAmount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string File = Convert.ToString(GetDataKeyValue(gvRemainingAmount, "Attachment", e.Row.RowIndex));
                    Label lblPaymentType = (Label)e.Row.FindControl("lblPaymentType");
                    string PaymentType = GetDataKeyValue(gvRemainingAmount, "PaymentType", e.Row.RowIndex);
                    FileUploadControl FileUploadControl = (FileUploadControl)e.Row.FindControl("FileUploadControl1");
                    if (PaymentType.ToUpper() == "REMAININGAMOUNT")
                    {
                        lblPaymentType.Text = "Remaining Amount";
                    }
                    else if (PaymentType.ToUpper() == "TOKENMONEY")
                    {
                        lblPaymentType.Text = "Token Money";
                    }
                    if (!string.IsNullOrEmpty(File))
                    {
                        List<string> FileName = new List<string>();
                        FileName.Add(File);
                        string filename = new System.IO.FileInfo(FileName[0]).Name;
                        string AttachmentPath = filename;
                        List<string> lstName = new List<string>();
                        lstName.Add(AttachmentPath);
                        FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        FileUploadControl.Size = 1;
                        FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.Auctions, lstName);
                    }
                 
                }
                if (e.Row.RowType == DataControlRowType.DataRow && gvRemainingAmount.EditIndex == e.Row.RowIndex)
                {
                    long ID = Convert.ToInt64(GetDataKeyValue(gvRemainingAmount, "ID", e.Row.RowIndex));

                    #region "Datakeys"
                    string PaymentTypeID = GetDataKeyValue(gvRemainingAmount, "PaymentTypeID", e.Row.RowIndex);

                    #endregion


                    DropDownList ddlPaymentType = (DropDownList)e.Row.FindControl("ddlPaymentType");
                    TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                    TextBox txtDate = (TextBox)e.Row.FindControl("txtDate");
                    FileUploadControl FileUploadControl = (FileUploadControl)e.Row.FindControl("FileUpload");

                    if (ddlPaymentType != null)
                    {
                        BindPaymentTypeDropDown(ddlPaymentType);
                    }



                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void txtAmount_OnTextChanged(object sender, EventArgs e)
        //{
        //    TextBox TXTControl = (TextBox)sender;
        //    DropDownList ddlPaymentType = (DropDownList)TXTControl.NamingContainer.FindControl("ddlPaymentType");
        //    TextBox txtAmount = (TextBox)TXTControl.NamingContainer.FindControl("txtAmount");
        //    double PaidAmount = Convert.ToDouble(txtAmount.Text);
        //    if (!string.IsNullOrEmpty(ddlPaymentType.SelectedItem.Value))
        //    {
        //        AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(Convert.ToInt64(hdnAuctionPriceID.Value));
        //        dynamic mdlAwardedAsset = new AuctionBLL().GetAuctionDetails(mdlAuctionPrice);
        //        double TokenMoney = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "TokenMoney"));
        //        double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
        //        if (TotalTokenMoneySubmitted == null)
        //        {
        //            TotalTokenMoneySubmitted = 0;
        //        }
        //        if (ddlPaymentType.SelectedItem.Text.ToUpper() == "TOKEN MONEY")
        //        {
        //            double TokenMoneyToCompare = TokenMoney - TotalTokenMoneySubmitted.Value;
        //            if (PaidAmount > TokenMoneyToCompare)
        //            {
        //                txtAmount.Text = "";
        //                Master.ShowMessage(Message.ExcessiveTokenMoney.Description, SiteMaster.MessageType.Error);
        //            }
                    
        //        }
        //        else
        //        {
        //            double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
        //            if (TotalRemainingAmountSubmitted == null)
        //            {
        //                TotalRemainingAmountSubmitted = 0;
        //            }
        //            double TotalAmountPaid = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "EarnestMoneySubmitted")) + TotalTokenMoneySubmitted + TotalRemainingAmountSubmitted;
        //            double BalanceAmount = 0;
        //            if (TotalTokenMoneySubmitted == TokenMoney)
        //            {
        //                 BalanceAmount = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate")) - TotalAmountPaid;
        //            }
        //            else
        //            {
        //                double TokenMoneyToSubtract = TokenMoney - TotalTokenMoneySubmitted.Value;
        //                BalanceAmount = (Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate"))-TokenMoneyToSubtract) - TotalAmountPaid;

        //            }
                         
        //            if (PaidAmount > BalanceAmount)
        //            {
        //                txtAmount.Text = "";
        //                Master.ShowMessage(Message.ExcessiveRemainingAmount.Description, SiteMaster.MessageType.Error);
        //            }
                    

        //        }
        //    }
           
        //}
        protected void gvRemainingAmount_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long ID = Convert.ToInt64(GetDataKeyValue(gvRemainingAmount, "ID", e.RowIndex));
                GridViewRow row = gvRemainingAmount.Rows[e.RowIndex];
                #region "Datakeys"
                string PaymentTypeID = GetDataKeyValue(gvRemainingAmount, "PaymentTypeID", e.RowIndex);
                #endregion

                #region "Controls"
           
                DropDownList ddlPaymentType = (DropDownList)row.FindControl("ddlPaymentType");
                TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
                TextBox txtDate = (TextBox)row.FindControl("txtDate");
                FileUploadControl FileUploadControl = (FileUploadControl)row.FindControl("FileUpload");

                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                #endregion

                AC_AuctionPayment mdlPayment = new AC_AuctionPayment();
                mdlPayment.AuctionPriceID = Convert.ToInt64(hdnAuctionPriceID.Value);
                mdlPayment.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                mdlPayment.PaymentType = Regex.Replace(ddlPaymentType.SelectedItem.Text, @"\s+", "");
                mdlPayment.PaymentDate = Convert.ToDateTime(txtDate.Text);
                mdlPayment.PaymentAttachment = lstNameofFiles[0].Item3;
                mdlPayment.PaidAmount = Convert.ToDouble(txtAmount.Text);

                mdlPayment.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlPayment.CreatedDate = DateTime.Now;

                bool isSaved = new AuctionBLL().SaveRemainingPayment(mdlPayment);
                if (isSaved)
                {
                    //Notification when balance Amount is zero
                    AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(Convert.ToInt64(hdnAuctionPriceID.Value));
                    dynamic mdlAwardedAsset = new AuctionBLL().GetAuctionDetails(mdlAuctionPrice);

                    double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                    double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
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

                    long AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                    long AuctionPriceID = Convert.ToInt64(hdnAuctionPriceID.Value);
                    long AuctionAssetID = Convert.ToInt64(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "AuctionAssetID"));
                    long EventID = (long)NotificationEventConstants.Auctions.PendingForApprovalWhenBalanceZero;
                    long UserID = SessionManagerFacade.UserInformation.ID;
                    if (BalanceAmount == 0)
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("AuctionNoticeID", AuctionNoticeID);
                        _event.Parameters.Add("AuctionAssetsID", AuctionAssetID);
                        _event.Parameters.Add("AuctionPriceID", AuctionPriceID);
                        _event.AddNotifyEvent(EventID, UserID);
                    }

                    ////////////////////////////////////////
                    gvRemainingAmount.EditIndex = -1;
                    BindPaymentsGridView(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                    BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                    Response.Redirect(Request.RawUrl);
                    Master.ShowMessage(Message.RecordSaved.Description);
                    // Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRemainingAmount_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ID = Convert.ToInt64(GetDataKeyValue(gvRemainingAmount, "ID", e.RowIndex));
                bool IsDeleted = new AuctionBLL().DeletePaymentEntryByID(ID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindPaymentsGridView(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                    //BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(hdnAuctionPriceID.Value));
                    AddRemainingPayment.IsSaved = true;
                    Response.Redirect("~/Modules/Auctions/AddRemainingPayment.aspx?AuctionNoticeID="+Convert.ToInt64(hdnAuctionNoticeID.Value)+"&AuctionPriceID="+Convert.ToInt64(hdnAuctionPriceID.Value), false);
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindPaymentTypeDropDown(DropDownList DDL)
        {
            try
            {
                DDL.DataSource = CommonLists.GetPaymentTypes();
                DDL.DataTextField = "Name";
                DDL.DataValueField = "ID";
                DDL.DataBind();
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
        private dynamic GetNewPaymentDetail()
        {
            dynamic Payment = new
            {
                ID = 0,
                PaymentType = string.Empty,
                PaymentTypeID = 0,
                Amount = string.Empty,
                Date = string.Empty,
                Attachment = string.Empty
            };
            return Payment;
        }

        private void BindPaymentsGridView(long _AuctionNoticeID, long _AuctionPriceID)
        {
            try
            {
                List<dynamic> lstRemainingPayments = new AuctionBLL().GetRemainingpaymentsbyNoticeID(_AuctionNoticeID, _AuctionPriceID);
                gvRemainingAmount.DataSource = lstRemainingPayments;
                gvRemainingAmount.DataBind();
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindAuctionDetailData(long _AuctionNoticeID, long _AuctionPriceID)
        {
            try
            {
                AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(_AuctionPriceID);
                dynamic mdlAwardedAsset = new AuctionBLL().GetAuctionDetails(mdlAuctionPrice);

                double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(_AuctionNoticeID, _AuctionPriceID);
                double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(_AuctionNoticeID, _AuctionPriceID);
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
               }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CompareAmount(long _AuctionNoticeID, long _AuctionPriceID, string _EnteredAmount, int _Type)
        {
            string Message = "";
            AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntity(_AuctionPriceID);
            dynamic mdlAwardedAsset = new AuctionBLL().GetAuctionDetails(mdlAuctionPrice);
            double TokenMoney = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "TokenMoney"));
            double? TotalTokenMoneySubmitted = new AuctionBLL().TotalTokenMoneySubmitted(_AuctionNoticeID, _AuctionPriceID);
            double PaidAmount = Convert.ToDouble(_EnteredAmount);
            if (TotalTokenMoneySubmitted == null)
            {
                TotalTokenMoneySubmitted = 0;
            }
            if (_Type == 1)
            {
                double TokenMoneyToCompare = TokenMoney - TotalTokenMoneySubmitted.Value;
                if (PaidAmount > TokenMoneyToCompare)
                {
                    //txtAmount.Text = "";
                    //Master.ShowMessage(Message.ExcessiveTokenMoney.Description, SiteMaster.MessageType.Error);
                    Message = "Token Money is Excessive";
                }

            }
            else if (_Type == 2)
            {
                double? TotalRemainingAmountSubmitted = new AuctionBLL().TotalRemainingAmountSubmitted(_AuctionNoticeID, _AuctionPriceID);
                if (TotalRemainingAmountSubmitted == null)
                {
                    TotalRemainingAmountSubmitted = 0;
                }
                double TotalAmountPaid = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "EarnestMoneySubmitted")) + TotalTokenMoneySubmitted + TotalRemainingAmountSubmitted;
                double BalanceAmount = 0;
                if (TotalTokenMoneySubmitted == TokenMoney)
                {
                    BalanceAmount = Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate")) - TotalAmountPaid;
                }
                else
                {
                    double TokenMoneyToSubtract = TokenMoney - TotalTokenMoneySubmitted.Value;
                    BalanceAmount = (Convert.ToDouble(Utility.GetDynamicPropertyValue(mdlAwardedAsset, "BidderRate")) - TokenMoneyToSubtract) - TotalAmountPaid;

                }

                if (PaidAmount > BalanceAmount)
                {
                    //txtAmount.Text = "";
                    //Master.ShowMessage(Message.ExcessiveRemainingAmount.Description, SiteMaster.MessageType.Error);
                    Message = "Remaining Amount is Excessive";
                }


            }



            return Message;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Auctions);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}