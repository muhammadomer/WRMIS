using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class BidderSelection : BasePage
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
                        hlBack.NavigateUrl = "~/Modules/Auctions/BiddingProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                        BindAssetsDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindBiddersGrid();
                        anchCommittee.HRef = string.Format("~/Modules/Auctions/AuctionCommitteAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidders.HRef = string.Format("~/Modules/Auctions/BiddersAttendance.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        anchBidding.HRef = string.Format("~/Modules/Auctions/BiddingProcess.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value));
                        if (!base.CanAdd)
                        {
                            btnSave.Visible = false;
                            txtReason.Enabled = false;
                            txtReason.Attributes.Remove("required");
                            txtReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize txtReason";
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
                    txtReason.Text = "";
                    List<dynamic> lstBidders = new AuctionBLL().GetBiddersByNoticeAndAssetIDForBidderSelection(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                    if (lstBidders.Count == 0)
                    {
                        gvBiddersRate.DataSource = lstBidders;
                        gvBiddersRate.DataBind();
                        txtReason.Attributes.Remove("requried");
                        txtReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize txtReason";
                        txtReason.Text = "";
                    }
                    else
                    {
                        gvBiddersRate.DataSource = lstBidders;
                        gvBiddersRate.DataBind();
                        txtReason.Attributes.Add("required", "required");
                        txtReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize txtReason required";
                    }
                    
                  
                }
                else
                {
                    txtReason.Attributes.Remove("required");
                    txtReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize txtReason";
                }

                if (!base.CanAdd)
                {
                    txtReason.Attributes.Remove("required");
                    txtReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize txtReason";
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindBiddersGrid()
        {
            try
            {
                List<dynamic> lstBidders = new List<dynamic>();//new AuctionBLL().GetBiddersByNoticeAndAssetID(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlAssets.SelectedItem.Value));
                gvBiddersRate.DataSource = lstBidders;
                gvBiddersRate.DataBind();
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
                Dropdownlist.BindDropdownlist<List<object>>(ddlAssets, new AuctionBLL().GetAssetsByNoticeID(_AuctionNoticeID), (int)Constants.DropDownFirstOption.Select);

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
                    var AuctionAssetDetailID = Convert.ToInt64(GetDataKeyValue(gvBiddersRate, "AuctionAssetDetailID", e.Row.RowIndex));
                    var IsAwarded = e.Row.FindControl("IsAwarded") as Label;
                    var RadioBtn = e.Row.FindControl("rdButton") as RadioButton;
                    var Reason = e.Row.FindControl("AwardedReason") as Label;

                    bool AwardedStatus = new AuctionBLL().GetAwardedStatusByBidderID(AuctionAssetDetailID);



                    bool IsAlreadyAwarded = false;
                    if (!string.IsNullOrEmpty(IsAwarded.Text))
                    {
                        IsAlreadyAwarded = Convert.ToBoolean(IsAwarded.Text);
                    }
                    else
                    {
                        IsAlreadyAwarded = false;
                    }

                  
                    if (e.Row.RowIndex == 0 && AwardedStatus == false)
                    {

                        RadioBtn.Checked = true;

                    }

                    if (IsAlreadyAwarded == true)
                    {

                        //if (!string.IsNullOrEmpty(Reason.Text))
                        //{
                        txtReason.Text = Reason.Text;
                        //}
                    }
                  
                    if (!base.CanAdd)
                    {
                        RadioBtn.Enabled = false;
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AC_AuctionPrice mdlAuctionPrice = new AC_AuctionPrice();
                foreach (GridViewRow gvrow in gvBiddersRate.Rows)
                {
                    Label ID = (Label)gvrow.FindControl("ID");
                    Label AuctionAssetDetailID = (Label)gvrow.FindControl("AuctionAssetDetailID");
                    bool IsUpdated = new AuctionBLL().UpdateAwardStatus(Convert.ToInt64(AuctionAssetDetailID.Text));
                    if (IsUpdated)
                    {
                        RadioButton rdButton = (RadioButton)gvrow.FindControl("rdButton");

                        if (rdButton.Checked)
                        {
                            mdlAuctionPrice.ID = Convert.ToInt64(ID.Text);
                            mdlAuctionPrice.Awarded = true;
                            mdlAuctionPrice.AwardedRemarks = txtReason.Text;
                            mdlAuctionPrice.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                            mdlAuctionPrice.ModifiedDate = DateTime.Now;
                        }
                    }
                  }
                bool IsSaved = new AuctionBLL().SaveAwardedAuctionAsset(mdlAuctionPrice);

                if (IsSaved)
                {
                    Master.ShowMessage(Message.AwardedSuccessfully.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (WRException exp)
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
    }
}