using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class AuctionAssetDetails : BasePage
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
                        hlBack.NavigateUrl = "~/Modules/Auctions/AddAuctionAssets.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value);
                        if (!string.IsNullOrEmpty(Request.QueryString["AuctionAssetID"]))
                        {
                            hdnAuctionAssetID.Value = Convert.ToString(Request.QueryString["AuctionAssetID"]);
                        }
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value),Convert.ToInt64(hdnAuctionAssetID.Value));
                        BindEarnesTokentMoneyDropDown(ddlEarnestMoney);
                        BindEarnesTokentMoneyDropDown(ddlTokenMoney);
                        bool IsEditMode = new AuctionBLL().IsAuctionAssetDetailExists(Convert.ToInt64(hdnAuctionAssetID.Value));
                        if (IsEditMode)
                        {
                            BindAuctionDetailDataEdit(Convert.ToInt64(hdnAuctionAssetID.Value));
                        }
                        if (!base.CanAdd)
                        {
                            txtReservePrice.Enabled = false;
                            txtReservePrice.Attributes.Remove("required");
                            txtReservePrice.CssClass = "form-control";
                            txtEarnestMoney.Enabled = false;
                            txtEarnestMoney.Attributes.Remove("required");
                            txtEarnestMoney.CssClass = "form-control";
                            txtTokenMoney.Enabled = false;
                            txtTokenMoney.Attributes.Remove("required");
                            txtTokenMoney.CssClass = "form-control";
                            txtBalanceAmountDate.Enabled = false;
                            ddlEarnestMoney.Enabled = false;
                            ddlEarnestMoney.CssClass = "form-control";
                            ddlEarnestMoney.Attributes.Remove("required");
                            ddlTokenMoney.Enabled = false;
                            ddlTokenMoney.CssClass = "form-control";
                            ddlTokenMoney.Attributes.Remove("required");
                            BtnSave.Visible = false;
                        }
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindAuctionDetailData(long _AuctionNoticeID, long _AuctionAssetID)
        {
            try
            {
                dynamic mdlAuctionDetails = new AuctionBLL().GetAuctionAssetDetails(_AuctionNoticeID, _AuctionAssetID);
                string AuctionNotice = Utility.GetDynamicPropertyValue(mdlAuctionDetails, "NoticeName");
                string OpeningDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlAuctionDetails, "OpeningDate")));
                string SubmissionDate = Utility.GetFormattedDate(Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlAuctionDetails, "SubmissionDate")));
                string Category = Utility.GetDynamicPropertyValue(mdlAuctionDetails, "Category");
                string SubCategory = Utility.GetDynamicPropertyValue(mdlAuctionDetails, "SubCategory");
                string AssetName = Utility.GetDynamicPropertyValue(mdlAuctionDetails, "AssetName");

                Auctions.Controls.AuctionItemDetails.AuctionNoticeName= AuctionNotice;
                Auctions.Controls.AuctionItemDetails.OpeningDate = OpeningDate;
                Auctions.Controls.AuctionItemDetails.SubmissionDate = SubmissionDate;
                Auctions.Controls.AuctionItemDetails.Category = Category;
                Auctions.Controls.AuctionItemDetails.SubCategory = SubCategory;
                Auctions.Controls.AuctionItemDetails.AssetName = AssetName;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindEarnesTokentMoneyDropDown(DropDownList DDL)
        {
            try
            {
                DDL.DataSource = CommonLists.GetEarnestTokenMoney();
                DDL.DataTextField = "Name";
                DDL.DataValueField = "ID";
                DDL.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
          private void BindAuctionDetailDataEdit(long _AuctionAssetID)
        {
            try
            {
                AC_AuctionAssetDetail mdlAuctionAssetDetails = new AuctionBLL().getAuctionAssetDetailByAssetID(_AuctionAssetID);
                txtReservePrice.Text = Convert.ToString(mdlAuctionAssetDetails.ReservePrice);
                if (mdlAuctionAssetDetails.EarnestMoneyType.ToUpper() == "LUMPSUM")
                {
                    ddlEarnestMoney.SelectedValue = "1";
                }
                else
                {
                    ddlEarnestMoney.SelectedValue = "2";
                }
                if (mdlAuctionAssetDetails.TokenMoneyType.ToUpper() == "LUMPSUM")
                {
                    ddlTokenMoney.SelectedValue = "1";
                }
                else
                {
                    ddlTokenMoney.SelectedValue = "2";
                }
                txtEarnestMoney.Text = Convert.ToString(mdlAuctionAssetDetails.EarnestMoney);
                txtTokenMoney.Text = Convert.ToString(mdlAuctionAssetDetails.TokenMoney);
                if (mdlAuctionAssetDetails.SubDateOfBalanceAmount != null)
                {
                    txtBalanceAmountDate.Text = Utility.GetFormattedDate(mdlAuctionAssetDetails.SubDateOfBalanceAmount);
                }
                hdnAuctionAssetDetailID.Value = Convert.ToString(mdlAuctionAssetDetails.ID);
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
                AC_AuctionAssetDetail mdlAuctionAssetDetails = new AC_AuctionAssetDetail();
                mdlAuctionAssetDetails.AuctionAssetID = Convert.ToInt64(hdnAuctionAssetID.Value);
                mdlAuctionAssetDetails.EarnestMoneyType = ddlEarnestMoney.SelectedItem.Value == "1" ? "LumpSum" : "%";
                mdlAuctionAssetDetails.EarnestMoney = Convert.ToDouble(txtEarnestMoney.Text);
                mdlAuctionAssetDetails.TokenMoneyType = ddlTokenMoney.SelectedItem.Value == "1" ? "LumpSum" : "%";
                mdlAuctionAssetDetails.TokenMoney = Convert.ToDouble(txtTokenMoney.Text);
                mdlAuctionAssetDetails.ReservePrice = Convert.ToDouble(txtReservePrice.Text);
                if (!string.IsNullOrEmpty(txtBalanceAmountDate.Text))
                {
                    mdlAuctionAssetDetails.SubDateOfBalanceAmount = Convert.ToDateTime(txtBalanceAmountDate.Text);
                }
                if (hdnAuctionAssetDetailID.Value != "0")
                {
                    mdlAuctionAssetDetails.ID = Convert.ToInt64(hdnAuctionAssetDetailID.Value);
                    mdlAuctionAssetDetails.ModifiedDate = DateTime.Now;
                    mdlAuctionAssetDetails.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                }
                bool IsSaved = new AuctionBLL().SaveAuctionAssetDetails(mdlAuctionAssetDetails);
                if (IsSaved)
                {
                    AddAuctionAssets.IsSaved = true;
                    Response.Redirect("~/Modules/Auctions/AddAuctionAssets.aspx?AuctionNoticeID=" + Convert.ToInt64(hdnAuctionNoticeID.Value), false);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
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