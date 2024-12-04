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
    public partial class BiddingProcess : BasePage
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
                        //hlNext.NavigateUrl = "~/Modules/Auctions/BidderSelection.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                        hlBack.NavigateUrl = "~/Modules/Auctions/BiddersAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                        BindAssetsDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindBiddersGrid();
                        anchCommittee.HRef = string.Format("~/Modules/Auctions/AuctionCommitteAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidders.HRef = string.Format("~/Modules/Auctions/BiddersAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidderSelection.HRef = string.Format("~/Modules/Auctions/BidderSelection.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        if (!base.CanAdd)
                        {
                            btnSave.Visible = false;
                            btnCloseAuctionT.Visible = false;
                            txtReason.Visible = false;
                            FileUploadControl.Visible = false;
                            lblUpload.Visible = false;
                            //lblStatus.Visible = false;
                            lblReason.Visible = false;
                        }
                    }
                }
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
                    List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetIDForBiddingProcess(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                    if (lstBidders.Count == 0)
                    {
                        //ChkBoxStatus.Enabled = false;
                        btnCloseAuctionT.Attributes.Remove("disabled");
                        long AuctionAssetDetailID = new AuctionBLL().getAssetDetailIDByAssetID(Convert.ToInt64(ddlAssets.SelectedItem.Value));
                        AC_AuctionPrice mdlAuctionPrice = new AuctionBLL().GetAuctionPriceEntityByDetailID(AuctionAssetDetailID);
                        if (mdlAuctionPrice != null)
                        {
                            txtReason.Text = mdlAuctionPrice.StatusRemarks;
                            List<string> FileName = new List<string>();
                            FileName.Add(mdlAuctionPrice.StatusAttachment);
                            FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                            FileUploadControl.UploadedFilesNames(Configuration.Auctions, FileName);
                            LinkButton.Enabled = false;
                        }
                        


                    }
                    else
                    {
                        bool IsAttended = false;

                        foreach (var item in lstBidders)
                        {
                            bool Val = Convert.ToBoolean(Utility.GetDynamicPropertyValue(item, "isChecked"));
                            if (Val)
                            {
                                IsAttended = true; 
                            }
                        }

                        if (!IsAttended)
                        {
                            btnCloseAuctionT.Attributes.Remove("disabled");
                        }
                    }
                    gvBiddersRate.DataSource = lstBidders;
                    gvBiddersRate.DataBind();
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
        private void BindAssetsDropDown(long _AuctionNoticeID)
        {
            try
            {
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
                //    hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, FilePath);
                //    hlAttachment.Text = FilePath.Substring(FilePath.LastIndexOf('_') + 1);
                //    hlAttachment.Attributes["FullName"] = FilePath;
                //    hlAttachment.Visible = true;
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}
                List<dynamic> lstBidders = new List<dynamic>();//new AuctionBLL().GetBiddersByNoticeAndAssetID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                gvBiddersRate.DataSource = lstBidders;
                gvBiddersRate.DataBind();
                //ChkBoxStatus.Enabled = false;
                btnCloseAuctionT.Attributes.Add("disabled", "disabled");
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

         protected void gvBiddersRate_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var CheckBox = e.Row.FindControl("chkSelect") as CheckBox;
                    var Rate = e.Row.FindControl("txtBidderRate") as TextBox;
                    if (e.Row.RowIndex != -1)
                    {
                        if (CheckBox.Checked)
                        {
                            btnCloseAuctionT.Attributes.Add("disabled", "disabled");
                            Rate.Attributes.Remove("disabled");
                            Rate.Attributes.Add("required", "required");
                            Rate.CssClass = "form-control decimalInput required";
                        }
                        else
                        {
                            Rate.Attributes.Add("disabled", "disabled");
                            Rate.Attributes.Remove("required");
                            Rate.CssClass = "form-control decimalInput";
                        }
                    }
                    if (!base.CanAdd)
                    {
                        CheckBox.Enabled = false;
                        Rate.Enabled = false;
                        Rate.Attributes.Remove("required");
                        Rate.CssClass = "form-control decimalInput";
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<AC_AuctionPrice> lstAuctionPrice = new List<AC_AuctionPrice>();
                List<long> lstAuctionPriceID = new List<long>();
                //AC_AuctionPrice mdlAuctionPriceStatus = new AC_AuctionPrice();
                //if (ChkBoxStatus.Checked)
                //{
                //    mdlAuctionPriceStatus.Status = "Closed";
                //    mdlAuctionPriceStatus.StatusRemarks = txtReason.Text;
                //    List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                //    if (lstNameofFiles.Count > 0)
                //    {
                //        mdlAuctionPriceStatus.StatusAttachment = lstNameofFiles[0].Item3;
                //    }
                //}
                //else
                //{
                    foreach (GridViewRow gvrow in gvBiddersRate.Rows)
                    {
                        AC_AuctionPrice mdlAuctionPrice = new AC_AuctionPrice();
                        CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                        Label ID = (Label)gvrow.FindControl("ID");
                        Label BidderID = (Label)gvrow.FindControl("BidderID");
                        Label AuctionAssetDetailID = (Label)gvrow.FindControl("AuctionAssetDetailID");
                        TextBox txtBidderRate = (TextBox)gvrow.FindControl("txtBidderRate");
                        if (chk.Checked)
                        {
                           if (!string.IsNullOrEmpty(ID.Text))
                            {
                                mdlAuctionPrice.ID = Convert.ToInt64(ID.Text);
                                mdlAuctionPrice.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                                mdlAuctionPrice.ModifiedDate = DateTime.Now;
                            }
                           else
                           {
                               mdlAuctionPrice.ID = 0;
                               mdlAuctionPrice.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                               mdlAuctionPrice.CreatedDate = DateTime.Now;
                           }

                            
                            mdlAuctionPrice.BidderRate = Convert.ToDouble(txtBidderRate.Text);
                            mdlAuctionPrice.AuctionBidderID = Convert.ToInt64(BidderID.Text);
                            mdlAuctionPrice.AuctionAssetDetailID = Convert.ToInt64(AuctionAssetDetailID.Text);
                            lstAuctionPrice.Add(mdlAuctionPrice);
                        }
                        else
                        {
                             if (!string.IsNullOrEmpty(ID.Text))
                            {
                               lstAuctionPriceID.Add(Convert.ToInt64(ID.Text));
                            }
                        }

                   // }
                }
                AuctionBLL Auctionbll = new AuctionBLL();
                using (TransactionScope transaction = new TransactionScope())
                {

                    //if (ChkBoxStatus.Checked)
                    //{
                    //    Auctionbll.SaveAuctionPriceForStatus(mdlAuctionPriceStatus);
                    //}
                    //else
                    //{
                        if (lstAuctionPrice.Count > 0)
                        {
                            for (int i = 0; i < lstAuctionPrice.Count; i++)
                            {
                                Auctionbll.SaveAuctionPrice(lstAuctionPrice.ElementAt(i));
                            }
                        //}
                    }
                    if (lstAuctionPriceID.Count > 0)
                    {
                        for (int i = 0; i < lstAuctionPriceID.Count; i++)
                            {
                                Auctionbll.DeleteAuctionPriceByID(lstAuctionPriceID.ElementAt(i));
                            }
                    }

                    transaction.Complete();
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    
                }
                List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetIDForBiddingProcess(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                gvBiddersRate.DataSource = lstBidders;
                gvBiddersRate.DataBind();

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

       protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                AC_AuctionPrice mdlAuctionPriceStatus = new AC_AuctionPrice();
                //mdlAuctionPriceStatus.AuctionBidderID = Convert.ToInt64(ddlAssets.SelectedItem.Value);
                long AuctionAssetDetailID = new AuctionBLL().getAssetDetailIDByAssetID(Convert.ToInt64(ddlAssets.SelectedItem.Value));
                mdlAuctionPriceStatus.AuctionAssetDetailID = AuctionAssetDetailID;
                mdlAuctionPriceStatus.Status = "Closed";
                mdlAuctionPriceStatus.StatusRemarks = txtReason.Text;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                if (lstNameofFiles.Count > 0)
                {
                    mdlAuctionPriceStatus.StatusAttachment = lstNameofFiles[0].Item3;
                }
                    bool IsClosed = new AuctionBLL().SaveAuctionPriceForStatus(mdlAuctionPriceStatus);
                    if (IsClosed)
                    {
                       Master.ShowMessage(Message.AuctionClosed.Description, SiteMaster.MessageType.Success); 
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error); 
                    }
                
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
       //protected void btnCloseAuction_Click(object sender, EventArgs e)
       //{
       //    try
       //    {
       //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseAuction", "$('#CloseAuction').modal();", true);

       //    }
       //    catch (Exception ex)
       //    {
       //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);

       //    }
       //}
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
    }
}