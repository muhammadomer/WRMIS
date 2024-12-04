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
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class Bidders : BasePage
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
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindBidderDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";

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
       private void BindBidderDropDown(long _AuctionNoticeID)
        {
            try
            {
                List<dynamic> lstBidders = new AuctionBLL().GetBiddersByAuctionNoticeID(_AuctionNoticeID);
                ddlBidderName.DataTextField = "Name";
                ddlBidderName.DataValueField = "ID";
                ddlBidderName.DataSource = lstBidders;
                ddlBidderName.DataBind();
                ListItem myDefaultItem = new ListItem("New Bidder", "0");
                myDefaultItem.Selected = true;
                ddlBidderName.Items.Insert(0, myDefaultItem);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlBidderName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlBidderName.SelectedItem.Value))
                {
                    
                    //txtName.ReadOnly = false;
                    //txtName.CssClass = "form-control required";
                    //txtName.Attributes.Add("required", "required");
                }
                else
                {
                    if (ddlBidderName.SelectedItem.Value != "0")
                    {
                        txtName.Text = ddlBidderName.SelectedItem.Text;
                    }
                    else
                    {
                        txtName.Text = "";
                    }
                    List<dynamic> lstAuctionAssets = new AuctionBLL().GetAssetsforBidderEarnestMoney(Convert.ToInt64(hdnAuctionNoticeID.Value),Convert.ToInt64(ddlBidderName.SelectedItem.Value));
                    gvAssetsList.DataSource = lstAuctionAssets;
                    gvAssetsList.DataBind();
                    gvAssetsList.Visible = true;
                    if (lstAuctionAssets.Count > 0)
                    {
                        btnSaveBidder.Visible = false;
                    }
                    else
                    {
                        btnSaveBidder.Visible = true;
                    }
                    //txtName.ReadOnly = true;
                    //txtName.CssClass = "form-control";
                    //txtName.Attributes.Remove("required");
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void txtSubmittedMoney_OnTextChanged(object sender, EventArgs e)
        {
            TextBox TXTControl = (TextBox)sender;
            Label lblEarnestMoney = (Label)TXTControl.NamingContainer.FindControl("lblEarnestMoney");
            TextBox txtSubmittedMoney = (TextBox)TXTControl.NamingContainer.FindControl("txtSubmittedMoney");
            double EarnestMoney = Convert.ToDouble(lblEarnestMoney.Text.Replace(",", ""));
            double Submitted = Convert.ToDouble(txtSubmittedMoney.Text);
            if (EarnestMoney != Submitted)
            {
                txtSubmittedMoney.Text = "";
                Master.ShowMessage(Message.EarnestMoneyNotEqual.Description, SiteMaster.MessageType.Error);
            }
           
           
        }
        protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox Control = (CheckBox)sender;
                int index = ((GridViewRow)Control.NamingContainer).RowIndex;
                Label ID = (Label)Control.NamingContainer.FindControl("ID");
                CheckBox chkSelect = (CheckBox)Control.NamingContainer.FindControl("chkSelect");
                //FileUploadControl FControl = (FileUploadControl)Control.NamingContainer.FindControl("FileUpload");
                TextBox txtSubmittedMoney = (TextBox)Control.NamingContainer.FindControl("txtSubmittedMoney");
                if (chkSelect.Checked && string.IsNullOrEmpty(ID.Text))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript"+ID, "AddFormRequired(" + index + ");", true);
                    txtSubmittedMoney.Attributes.Add("required", "required");
                    txtSubmittedMoney.Attributes.Remove("disabled");
                    txtSubmittedMoney.CssClass = "form-control required";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript"+ID, "RemoveFormRequired(" + index + ");", true);
                    txtSubmittedMoney.Attributes.Remove("required");
                    txtSubmittedMoney.Attributes.Add("disabled", "disabled");
                    txtSubmittedMoney.CssClass = "form-control";
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
          
            
        }

        protected void gvAssetsList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string isChecked = GetDataKeyValue(gvAssetsList, "isChecked", e.Row.RowIndex);
                    string Attachement = GetDataKeyValue(gvAssetsList, "Attachement", e.Row.RowIndex);
                    FileUploadControl FileUploadControl = (FileUploadControl)e.Row.FindControl("FileUploadControl1");
                    bool IsExists = Convert.ToBoolean(isChecked);
                    if(IsExists)
                    {
                        TextBox txtSubmittedMoney = (TextBox)e.Row.FindControl("txtSubmittedMoney");
                        HyperLink hlAttachement = (HyperLink)e.Row.FindControl("hlAttachement");
                        
                        txtSubmittedMoney.Attributes.Add("required", "required");
                        txtSubmittedMoney.Attributes.Remove("disabled");
                        txtSubmittedMoney.CssClass = "form-control required";

                        List<string> FileName = new List<string>();
                        FileName.Add(Attachement);


                        string filename = new System.IO.FileInfo(FileName[0]).Name;
                        string AttachmentPath = filename;
                        List<string> lstName = new List<string>();
                        lstName.Add(AttachmentPath);
                        FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        FileUploadControl.Size = 1;
                        FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.Auctions, lstName);
                        //hlAttachement.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, Attachement);
                        //hlAttachement.Text = Attachement.Substring(Attachement.LastIndexOf('_') + 1);
                        //hlAttachement.Attributes["FullName"] = Attachement;
                        //hlAttachement.Visible = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript" + e.Row.RowIndex, "RemoveFormRequired(" + e.Row.RowIndex + ");", true);
                    }
                    //else
                    //{
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript" + e.Row.RowIndex, "AddFormRequired(" + e.Row.RowIndex + ");", true);
                    //}
                    
                    //FileUploadControl FileUploadControl = (FileUploadControl)e.Row.FindControl("FileUpload");

                    //if (FileUploadControl != null)
                    //{
                    //    ScriptManager.GetCurrent(this).RegisterPostBackControl(FileUploadControl);  
                    //}
                    
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
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBidderName.SelectedItem.Value == "0")
                {
                    bool IsBidderDuplicate = new AuctionBLL().IsBidderDuplicate(Convert.ToInt64(hdnAuctionNoticeID.Value), txtName.Text.ToUpper());
                    if (!IsBidderDuplicate)
                    {
                        AC_AuctionBidder mdlBidder = new AC_AuctionBidder();
                        mdlBidder.BidderName = txtName.Text;
                        mdlBidder.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                        mdlBidder.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlBidder.CreatedDate = DateTime.Now;
                        bool isSaved = new AuctionBLL().SaveAuctionBidder(mdlBidder);
                        if (isSaved)
                        {
                            Master.ShowMessage(Message.BidderAddedSuccessfully.Description, SiteMaster.MessageType.Success);
                            BindBidderDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                            txtName.Text = "";
                        }
                    }
                    else
                    {
                        Master.ShowMessage(Message.BidderDuplication.Description, SiteMaster.MessageType.Error);
                        txtName.Text = "";
                    }
                  
                    
                    
                }
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
                List<AC_BidderEarnestMoney> lstNew = new List<AC_BidderEarnestMoney>();
                int ImgCount = 0;
                bool IsAttachmentChanged = true;
                foreach (GridViewRow gvrow in gvAssetsList.Rows)
                {
                   
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                   // CheckBox chkSelect = (CheckBox)gvrow.FindControl("chkAlternate");
                    if (chk.Checked)
                    {
                        AC_BidderEarnestMoney mdlBidderEarnestMoney = new AC_BidderEarnestMoney();
                        Label ID = (Label)gvrow.FindControl("ID");
                        //if (!string.IsNullOrEmpty(ID.Text))
                        //{
                        //    mdlBidderEarnestMoney.ID = Convert.ToInt64(ID.Text);
                        //    mdlBidderEarnestMoney.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                        //    mdlBidderEarnestMoney.ModifiedDate = DateTime.Now;

                        //}
                        Label AuctionDetailID = (Label)gvrow.FindControl("AuctionDetailID");
                        TextBox txtSubmittedMoney = (TextBox)gvrow.FindControl("txtSubmittedMoney");

                       // HyperLink hlAttachement = (HyperLink)gvrow.FindControl("hlAttachement");
                        Label Att = (Label)gvrow.FindControl("Att");
                        FileUploadControl FileUploadControl = (FileUploadControl)gvrow.FindControl("FileUpload");
                        List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions, "BidCtrl_"+ImgCount);
                        if (lstNameofFiles.Count == 0 && !string.IsNullOrEmpty(ID.Text))
                        {
                            mdlBidderEarnestMoney.EMAttachment = Att.Text;
                            IsAttachmentChanged = false;
                        }
                        else
                        {
                            mdlBidderEarnestMoney.EMAttachment = lstNameofFiles[0].Item3;
                            IsAttachmentChanged = true;
                        }

                        mdlBidderEarnestMoney.EarnestMoneySubmitted = Convert.ToDouble(txtSubmittedMoney.Text);
                        mdlBidderEarnestMoney.AuctionBidderID = Convert.ToInt64(ddlBidderName.SelectedItem.Value);
                        mdlBidderEarnestMoney.AuctionAssetDetailID = Convert.ToInt64(AuctionDetailID.Text);
                        
                        
                        lstNew.Add(mdlBidderEarnestMoney);
                    }
                    else
                    {
                        //Label ID = (Label)gvrow.FindControl("ID");
                        //if (!string.IsNullOrEmpty(ID.Text))
                        //{
                        //    bool IsDeleted = new AuctionBLL().DeleteBidderEarnestMoney(Convert.ToInt64(ID.Text));
                           
                        //}
                    }
                    ImgCount++;
                   }
                if (lstNew.Count > 0)
                {
                    AuctionBLL Auctionbll = new AuctionBLL();
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        bool IsDeleted = Auctionbll.DeleteBiddersforUpdation(Convert.ToInt64(ddlBidderName.SelectedItem.Value));
                        if (IsDeleted)
                        {
                            for (int i = 0; i < lstNew.Count; i++)
                            {
                                Auctionbll.SaveBidderEarnestMoney(lstNew[i], IsAttachmentChanged);
                            }
                            transaction.Complete();
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        }
                      
                    }
                    //BindBidderDropDown(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    //txtName.Text = "";
                    List<dynamic> lstAuctionAssets = new AuctionBLL().GetAssetsforBidderEarnestMoney(Convert.ToInt64(hdnAuctionNoticeID.Value), Convert.ToInt64(ddlBidderName.SelectedItem.Value));
                    gvAssetsList.DataSource = lstAuctionAssets;
                    gvAssetsList.DataBind();
                    gvAssetsList.Visible = true;
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