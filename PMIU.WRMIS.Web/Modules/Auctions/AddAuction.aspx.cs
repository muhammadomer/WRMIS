using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
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
    public partial class AddAuction : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        public const string UserSubDivisionKey = "UserSubDivision";
        public const string UserSectionKey = "UserSection";

        #endregion
        #region ViewState Constants

        string AdvertisementSourcesList = "AdvertisementSourcesList";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    //BindDomainDropDown(mdlUser);
                    //BindDivisionDropdown(mdlUser);
                    ////hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                    TimePickerOpeningAlternate.RemoveTimePickerValidations();
                    hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                    SetPageTitle();
                    BindDropDowns();
                    bool IsEditMode = false;
                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["IsEditMode"]))
                        {
                            IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);
                            hdnIsEditMode.Value = Convert.ToString(IsEditMode);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequiredKeyWord();", true);
                        }
                        hdnAuctionNoticeID.Value = Request.QueryString["AuctionNoticeID"];
                        BindAuctionNoticeData(Convert.ToInt64(hdnAuctionNoticeID.Value), IsEditMode);
                        BindAdvertisementSourceGridView();
                    }
                    else
                    {

                        BindAdvertisementSourceGridView();
                        
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Advertisement Grid
        protected void gvAdvertisementSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvAdvertisementSource.EditIndex == e.Row.RowIndex)
                {
                    //long AuctionNoticeID = Convert.ToInt64(GetDataKeyValue(gvAdvertisementSource, "AuctionNoticeID", e.Row.RowIndex));


                    #region "Controls"
                    TextBox txtAdvertisementSource = (TextBox)e.Row.FindControl("txtAdvertisementSource");
                    TextBox txtAdvertisementDate = (TextBox)e.Row.FindControl("txtAdvertisementDate");
                    #endregion


                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdvertisementSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (hdnAuctionNoticeID.Value != "0")
                {
                    if (e.CommandName == "AddAdvertisement")
                    {
                        Button ddlChannel = (Button)sender;

                        GridViewRow gvRow = (GridViewRow)ddlChannel.NamingContainer;

                        TextBox txtAdvertisementSource = (TextBox)gvRow.FindControl("txtAdvertisementSource");

                        List<AdvertisementEdit> lstAdvertisementSource = (List<AdvertisementEdit>)gvAdvertisementSource.DataSource;

                        if (lstAdvertisementSource == null)
                        {
                            lstAdvertisementSource = new List<AdvertisementEdit>();
                        }

                        lstAdvertisementSource.Insert(gvAdvertisementSource.Rows.Count, GetNewAdvertisementDetailViewStateEdit());
                        gvAdvertisementSource.DataSource = lstAdvertisementSource;
                        gvAdvertisementSource.EditIndex = gvAdvertisementSource.Rows.Count;
                        gvAdvertisementSource.DataBind();
                    }
                }
                else
                {
                    if (e.CommandName == "AddAdvertisement")
                    {
                        Button ddlChannel = (Button)sender;

                        GridViewRow gvRow = (GridViewRow)ddlChannel.NamingContainer;

                        TextBox txtAdvertisementSource = (TextBox)gvRow.FindControl("txtAdvertisementSource");

                        List<Advertisement> lstAdvertisementSource = (List<Advertisement>)gvAdvertisementSource.DataSource;

                        if (lstAdvertisementSource == null)
                        {
                            lstAdvertisementSource = new List<Advertisement>();
                        }

                        lstAdvertisementSource.Insert(gvAdvertisementSource.Rows.Count, GetNewAdvertisementDetailViewState());
                        gvAdvertisementSource.DataSource = lstAdvertisementSource;
                        gvAdvertisementSource.EditIndex = gvAdvertisementSource.Rows.Count;
                        gvAdvertisementSource.DataBind();
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdvertisementSource_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = 0;
                if (ViewState[AdvertisementSourcesList] != null)
                {
                    List<Advertisement> lstAdvertisementSource = (List<Advertisement>)ViewState[AdvertisementSourcesList];
                    List<Advertisement> lstNew = new List<Advertisement>();
                    if (lstAdvertisementSource.Count > 0)
                    {
                        for (int i = 0; i < lstAdvertisementSource.Count; i++)
                        {
                            TextBox AdvertisementSource = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[0].FindControl("txtAdvertisementSource");
                            TextBox AdvertisementDate = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[1].FindControl("txtAdvertisementDate");
                            lstNew.Add(new Advertisement { AdvertisementSource = AdvertisementSource.Text, AdvertisementDate = AdvertisementDate.Text });
                            rowIndex++;
                        }
                        lstNew.RemoveAt(e.RowIndex);
                        ViewState[AdvertisementSourcesList] = lstNew;
                        gvAdvertisementSource.DataSource = lstNew;
                        gvAdvertisementSource.DataBind();

                    }
                }



            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindAdvertisementSourceGridView()
        {
            try
            {

                List<object> lstAdvertisementSource = new List<object>();
                if (ViewState[AdvertisementSourcesList] != null)
                {
                    gvAdvertisementSource.DataSource = (List<Advertisement>)ViewState[AdvertisementSourcesList];
                }
                else
                {
                    gvAdvertisementSource.DataSource = lstAdvertisementSource;
                }
                gvAdvertisementSource.DataBind();
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


        private Advertisement GetNewAdvertisementDetailViewState()
        {
            Advertisement AdvertisementDetail = new Advertisement
            {

                AdvertisementSource = string.Empty,
                AdvertisementDate = string.Empty,
            };
            return AdvertisementDetail;
        }

        private AdvertisementEdit GetNewAdvertisementDetailViewStateEdit()
        {
            AdvertisementEdit AdvertisementDetail = new AdvertisementEdit
            {
                ID = 0,
                AdvertisementSource = string.Empty,
                AdvertisementDate = string.Empty,
            };
            return AdvertisementDetail;
        }


        protected void AddRow_Grid(object sender, EventArgs e)
        {
            try
            {

                int rowIndex = 0;
                if (ViewState[AdvertisementSourcesList] != null)
                {
                    List<Advertisement> lstAdvertisementSource = (List<Advertisement>)ViewState[AdvertisementSourcesList];
                    List<Advertisement> lstNew = new List<Advertisement>();
                    if (lstAdvertisementSource.Count > 0)
                    {
                        for (int i = 0; i < lstAdvertisementSource.Count; i++)
                        {
                            TextBox AdvertisementSource = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[0].FindControl("txtAdvertisementSource");
                            TextBox AdvertisementDate = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[1].FindControl("txtAdvertisementDate");
                            lstNew.Add(new Advertisement { AdvertisementSource = AdvertisementSource.Text, AdvertisementDate = AdvertisementDate.Text });
                            rowIndex++;
                        }
                        lstNew.Add(new Advertisement { AdvertisementSource = "", AdvertisementDate = "" });
                        ViewState[AdvertisementSourcesList] = lstNew;
                        gvAdvertisementSource.DataSource = lstNew;
                        gvAdvertisementSource.DataBind();

                    }
                    else
                    {
                        List<Advertisement> lstAdvertisementSourceNew = new List<Advertisement>();
                        lstAdvertisementSourceNew.Insert(0, GetNewAdvertisementDetailViewState());
                        ViewState[AdvertisementSourcesList] = lstAdvertisementSourceNew;
                        gvAdvertisementSource.DataSource = lstAdvertisementSourceNew;
                        gvAdvertisementSource.DataBind();
                    }

                }
                else
                {
                    List<Advertisement> lstAdvertisementSource = new List<Advertisement>();
                    lstAdvertisementSource.Insert(0, GetNewAdvertisementDetailViewState());
                    ViewState[AdvertisementSourcesList] = lstAdvertisementSource;
                    gvAdvertisementSource.DataSource = lstAdvertisementSource;
                    gvAdvertisementSource.DataBind();
                }



            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        public void BindDropDowns()
        {
            BindApprovalAuthorityDropDown();
            BindAuctionCategoryDropDown();
            BindAuctionTypeDropDown();
            UA_Users _MdlUser = SessionManagerFacade.UserInformation;
            Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindApprovalAuthorityDropDown()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlApprovalAuthority, new AuctionBLL().GetApprovalAuthorities(), (int)Constants.DropDownFirstOption.Select);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindAuctionTypeDropDown()
        {
            try
            {
                ddlAuctionType.DataSource = CommonLists.GetAuctionTypes();
                ddlAuctionType.DataTextField = "Name";
                ddlAuctionType.DataValueField = "ID";
                ddlAuctionType.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionCategoryDropDown()
        {
            try
            {
                ddlAuctionCategory.DataSource = CommonLists.GetAuctionCategories();
                ddlAuctionCategory.DataTextField = "Name";
                ddlAuctionCategory.DataValueField = "ID";
                ddlAuctionCategory.DataBind();
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
                if (gvAdvertisementSource.Rows.Count == 0)
                {
                    Master.ShowMessage(Message.TM_EnterAdvertismentSource.Description, SiteMaster.MessageType.Error);
                    return;
                }
                string SubmissionTime = TimePickerSubmission.GetTime();
                string OpeningTime = TimePickerOpening.GetTime();
                string AlternateOpeningTime = TimePickerOpeningAlternate.GetTime();
                var SubmissionDateTime = Utility.GetParsedDateTime(txtSubmissionDate.Text, SubmissionTime);
                var OpeningDateTime = Utility.GetParsedDateTime(txtOpeningDate.Text, OpeningTime);
                if (!string.IsNullOrEmpty(txtAlternateOpeningDate.Text))
                {
                    var AlternateOpeningDateTime = Utility.GetParsedDateTime(txtAlternateOpeningDate.Text, AlternateOpeningTime);
                }


                var TimeDifference = TimeSpan.Compare(OpeningDateTime.TimeOfDay, SubmissionDateTime.TimeOfDay);
                if (OpeningDateTime.Date < SubmissionDateTime.Date)
                {
                    Master.ShowMessage(Message.TM_OpeningDateDiff.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (OpeningDateTime.Date == SubmissionDateTime.Date)
                {
                    if (TimeDifference == -1 || TimeDifference == 0)
                    {
                        Master.ShowMessage(Message.TM_OpeningTimeDiff.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                DateTime? DateVal = null;
                double? Val = null;
                AC_AuctionNotice AuctionNoticeModel = new AC_AuctionNotice();
                if (hdnAuctionNoticeID.Value != "0")
                {
                    AuctionNoticeModel.ID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                }
                //if hlImage.Text 
                //List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                //if (lstNameofFiles.Count > 0)
                //    AuctionNoticeModel.AuctionNoticeFile = lstNameofFiles[0].Item3;
                //else
                //    AuctionNoticeModel.AuctionNoticeFile = hdnFileName.Value;
                AuctionNoticeModel.AuctionTitle = txtAuctionNoticetitle.Text;
                AuctionNoticeModel.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                AuctionNoticeModel.ApprovalAuthorityID = Convert.ToInt64(ddlApprovalAuthority.SelectedItem.Value);
                AuctionNoticeModel.AuctionTypeID = Convert.ToInt64(ddlAuctionType.SelectedItem.Value);
                AuctionNoticeModel.AuctionCategoryID = Convert.ToInt64(ddlAuctionCategory.SelectedItem.Value);
                AuctionNoticeModel.FromDate = txtFromDate.Text == "" ? DateVal : Convert.ToDateTime(txtFromDate.Text);
                AuctionNoticeModel.ToDate = txtToDate.Text == "" ? DateVal : Convert.ToDateTime(txtToDate.Text);
                AuctionNoticeModel.CreatedDate = DateTime.Now;
                AuctionNoticeModel.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                AuctionNoticeModel.AuctionDetail = txtAuctionDetails.Text;
                AuctionNoticeModel.OpeningDate = Utility.GetParsedDateTime(txtOpeningDate.Text, OpeningTime);
                AuctionNoticeModel.OpeningPlace = txtOpeningPlace.Text;
                AuctionNoticeModel.SubmissionDate = Utility.GetParsedDateTime(txtSubmissionDate.Text, SubmissionTime);
                AuctionNoticeModel.SubmissionFee = txtSubmissionFee.Text == "" ? Val : Convert.ToDouble(txtSubmissionFee.Text);
                AuctionNoticeModel.AlternateOpeningDate = txtAlternateOpeningDate.Text == "" ? DateVal : Utility.GetParsedDateTime(txtAlternateOpeningDate.Text, AlternateOpeningTime);
                AuctionNoticeModel.AlternateOpeningPlace = txtAlternateOpeningPlace.Text;

                List<Advertisement> PublishingSourceList = new List<Advertisement>();

                int rowIndex = 0;
                List<Advertisement> lstAdvertisementSource = (List<Advertisement>)ViewState[AdvertisementSourcesList];
                List<Advertisement> lstNew = new List<Advertisement>();
                if (lstAdvertisementSource != null)
                {
                    if (lstAdvertisementSource.Count > 0)
                    {
                        for (int i = 0; i < lstAdvertisementSource.Count; i++)
                        {
                            TextBox AdvertisementSource = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[0].FindControl("txtAdvertisementSource");
                            TextBox AdvertisementDate = (TextBox)gvAdvertisementSource.Rows[rowIndex].Cells[1].FindControl("txtAdvertisementDate");
                            lstNew.Add(new Advertisement { AdvertisementSource = AdvertisementSource.Text, AdvertisementDate = AdvertisementDate.Text });
                            rowIndex++;
                        }

                        ViewState[AdvertisementSourcesList] = lstNew;
                        PublishingSourceList = (List<Advertisement>)ViewState[AdvertisementSourcesList];

                    }

                }

                bool IsSaved = false;
                long AuctionNoticeID = 0;

                using (TransactionScope transaction = new TransactionScope())
                {
                    if (hdnAuctionNoticeID.Value != "0")
                    {
                        bool isDeleted = new AuctionBLL().DeletePublishingSourceforUpdation(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    }
                    AuctionNoticeID = new AuctionBLL().SaveAuctionNotice(AuctionNoticeModel);
                    if (lstAdvertisementSource != null)
                    {
                        foreach (var item in PublishingSourceList)
                        {
                            AC_AdvertisementSource publishingSource = new AC_AdvertisementSource();

                            if (hdnAuctionNoticeID.Value != "0")
                            {
                                publishingSource.AcutionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                            }
                            else
                            {
                                publishingSource.AcutionNoticeID = AuctionNoticeID;
                            }
                            publishingSource.AdvertisementSource = item.AdvertisementSource;
                            publishingSource.AdvertisementDate = Utility.GetParsedDate(item.AdvertisementDate);
                            publishingSource.CreatedDate = DateTime.Now.Date;
                            publishingSource.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;

                            IsSaved = new AuctionBLL().SavePublishingSource(publishingSource);

                        }

                        List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Auctions);
                        if (lstNameofFiles != null && lstNameofFiles.Count > 0)
                        {
                            bool IsDeleted = new AuctionBLL().DeleteAuctionNoticeAttachmentsforUpdation(AuctionNoticeID);
                            if (IsDeleted)
                            {
                                for (int i = 0; i < lstNameofFiles.Count; i++)
                                {
                                    AC_AuctionNoticeAttachment mdlAtt = new AC_AuctionNoticeAttachment();
                                    mdlAtt.AcutionNoticeID = AuctionNoticeID;
                                    mdlAtt.Attachment = lstNameofFiles[i].Item3;
                                    mdlAtt.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                                    mdlAtt.CreatedDate = DateTime.Now;
                                    bool IsSavedAtt = new AuctionBLL().SaveAuctionNoticeAttachment(mdlAtt);
                                } 
                            }
                            
                        }
                    }

                    transaction.Complete();
                    SearchAuctions.IsSaved = true;
                    Response.Redirect("~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true", false);
                    //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
                //if (hdnAuctionNoticeID.Value == "0")
                //{
                //    PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                //    _event.Parameters.Add("TenderNoticeID", TenderNoticeID);
                //    _event.Parameters.Add("TenderWorkID", 0);
                //    _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.AddTenderNotice, (int)SessionManagerFacade.UserInformation.ID);
                //}

                //Works.AddWorks.IsSaved = true;
                //Response.Redirect("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID, false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlAuctionCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlAuctionCategory.SelectedItem.Text))
                {
                    if (Convert.ToInt32(ddlAuctionCategory.SelectedItem.Value) == (int)Constants.AuctionCategories.TemporaryORLease)
                    {
                        txtFromDate.CssClass = "form-control disabled-Past-date-picker required";
                        txtFromDate.Attributes.Remove("disabled");
                        txtFromDate.Attributes.Add("required", "required");
                        txtToDate.CssClass = "form-control disabled-Past-date-picker required";
                        txtToDate.Attributes.Remove("disabled");
                        txtToDate.Attributes.Add("required", "required");
                    }
                    else
                    {
                        txtFromDate.CssClass = "form-control disabled-Past-date-picker disbaled";
                        txtFromDate.Text = "";
                        txtFromDate.Attributes.Add("disabled", "disabled");
                        txtToDate.CssClass = "form-control disabled-Past-date-picker disbaled";
                        txtToDate.Text = "";
                        txtToDate.Attributes.Add("disabled", "disabled");
                    } 
                }
                else
                {
                    txtFromDate.CssClass = "form-control disabled-Past-date-picker disbaled";
                    txtFromDate.Text = "";
                    txtFromDate.Attributes.Add("disabled", "disabled");
                    txtToDate.CssClass = "form-control disabled-Past-date-picker disbaled";
                    txtToDate.Text = "";
                    txtToDate.Attributes.Add("disabled", "disabled");
                }
             

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindAuctionNoticeData(long _AuctionNoticeID, bool IsEditMode)
        {
            try
            {
                AC_AuctionNotice AuctionNoticeData = new AuctionBLL().GetAuctionNoticeByID(_AuctionNoticeID);
                txtAuctionNoticetitle.Text = AuctionNoticeData.AuctionTitle;
                txtOpeningDate.Text = Utility.GetFormattedDate(AuctionNoticeData.OpeningDate);
                txtSubmissionDate.Text = Utility.GetFormattedDate(AuctionNoticeData.SubmissionDate);
                string OpeningTime = Utility.GetFormattedTime(AuctionNoticeData.OpeningDate);
                var OT = Convert.ToDateTime(OpeningTime);
                TimePickerOpening.SetTime(OT.ToString("hh"), OT.ToString("mm"), OT.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                string SubmissionTime = Utility.GetFormattedTime(AuctionNoticeData.SubmissionDate);
                var ST = Convert.ToDateTime(SubmissionTime);
                TimePickerSubmission.SetTime(ST.ToString("hh"), ST.ToString("mm"), ST.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                txtAuctionDetails.Text = AuctionNoticeData.AuctionDetail;
                txtOpeningPlace.Text = AuctionNoticeData.OpeningPlace;
                txtAlternateOpeningPlace.Text = AuctionNoticeData.AlternateOpeningPlace;
                txtSubmissionFee.Text = AuctionNoticeData.SubmissionFee == null ? "" : Convert.ToString(AuctionNoticeData.SubmissionFee);
                
                if (AuctionNoticeData.AlternateOpeningDate != null)
                {
                    txtAlternateOpeningDate.Text = Utility.GetFormattedDate(AuctionNoticeData.AlternateOpeningDate);
                    string AOpeningTime = Utility.GetFormattedTime(AuctionNoticeData.AlternateOpeningDate.Value);
                    var AOT = Convert.ToDateTime(AOpeningTime);
                    TimePickerOpeningAlternate.SetTime(AOT.ToString("hh"), AOT.ToString("mm"), AOT.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                }
                if (AuctionNoticeData.AuctionCategoryID == 1)
                {
                    txtFromDate.Text = Utility.GetFormattedDate(AuctionNoticeData.FromDate);
                    txtToDate.Text = Utility.GetFormattedDate(AuctionNoticeData.ToDate);
                    txtFromDate.CssClass = "form-control disabled-Past-date-picker required";
                    txtToDate.CssClass = "form-control disabled-Past-date-picker required";
                    txtFromDate.Attributes.Add("required", "required");
                    txtToDate.Attributes.Add("required", "required");
                    txtFromDate.Attributes.Remove("Disabled");
                    txtToDate.Attributes.Remove("Disabled");
                }
                Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(AuctionNoticeData.DivisionID));
                Dropdownlist.SetSelectedValue(ddlApprovalAuthority, Convert.ToString(AuctionNoticeData.ApprovalAuthorityID));
                Dropdownlist.SetSelectedValue(ddlAuctionCategory, Convert.ToString(AuctionNoticeData.AuctionCategoryID));
                Dropdownlist.SetSelectedValue(ddlAuctionType, Convert.ToString(AuctionNoticeData.AuctionTypeID));

                //hdnFileName.Value = AuctionNoticeData.AuctionNoticeFile;
                if (IsEditMode)
                {
                    List<string> FileNames = new AuctionBLL().GetAuctionNoticeAttachment(_AuctionNoticeID);
                    //FileName.Add(AuctionNoticeData.AuctionNoticeFile);
                    if (FileNames.Count > 0)
                    {
                        PreviewImage(FileNames, IsEditMode);
                    }
                    
                    //hlImage.NavigateUrl = Utility.GetImageURL(Configuration.Auctions, AuctionNoticeData.AuctionNoticeFile);
                    //hlImage.Visible = true;
                }
                else
                {
                    List<string> FileNames = new AuctionBLL().GetAuctionNoticeAttachment(_AuctionNoticeID);
                    //FileName.Add(AuctionNoticeData.AuctionNoticeFile);
                    FileUploadControl.Visible = false;
                    DivToChange.Attributes.Remove("class");
                    DivToChange.Attributes.Add("class", "col-md-1");
                    //FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                    //FileUploadControl.UploadedFilesNames(Configuration.Auctions, FileName);
                    if (FileNames.Count > 0)
                    {
                        PreviewImage(FileNames, IsEditMode);
                    }
                }

                List<dynamic> PublishingData = new AuctionBLL().GetPublishingSourceByAuctionNoticeID(AuctionNoticeData.ID);
                List<Advertisement> PData = new List<Advertisement>();
                foreach (var item in PublishingData)
                {
                    string Date = Utility.GetDynamicPropertyValue(item, "AdvertisementDate");
                    PData.Add(new Advertisement { AdvertisementSource = Utility.GetDynamicPropertyValue(item, "AdvertisementSource"), AdvertisementDate = Utility.GetFormattedDate(Convert.ToDateTime(Date)) });
                }
                ViewState[AdvertisementSourcesList] = PData;
                gvAdvertisementSource.DataSource = PublishingData;
                gvAdvertisementSource.DataBind();

                if (!IsEditMode)
                {
                    BinScreenControls();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void BinScreenControls()
        {
            try
            {
                txtOpeningDate.Enabled = false;
                txtOpeningDate.CssClass = "aspNetDisabled form-control";
                txtAuctionDetails.Enabled = false;
                SubmissionDate.Visible = false;
                ToDate.Visible = false;
                FromDate.Visible = false;
                AlternateOpeningDate.Visible = false;
                OpeningDate.Visible = false;
                txtOpeningPlace.Enabled = false;
                txtOpeningPlace.CssClass = "aspNetDisabled form-control";
                txtAlternateOpeningDate.Enabled = false;
                txtAlternateOpeningDate.CssClass = "aspNetDisabled form-control";
                txtAlternateOpeningPlace.Enabled = false;
                txtAlternateOpeningPlace.CssClass = "aspNetDisabled form-control";
                txtSubmissionFee.Enabled = false;
                txtSubmissionFee.CssClass = "aspNetDisabled form-control";
                txtSubmissionDate.Enabled = false;
                txtSubmissionDate.CssClass = "aspNetDisabled form-control";
                txtAuctionNoticetitle.Enabled = false;
                txtAuctionNoticetitle.CssClass = "aspNetDisabled form-control";
                ddlDivision.Enabled = false;
                ddlDivision.CssClass = "aspNetDisabled form-control";
                ddlAuctionCategory.Enabled = false;
                ddlAuctionCategory.CssClass = "aspNetDisabled form-control";
                ddlApprovalAuthority.Enabled = false;
                ddlApprovalAuthority.CssClass = "aspNetDisabled form-control";
                ddlAuctionType.Enabled = false;
                TimePickerSubmission.DisbaleTimePicker();
                TimePickerOpening.DisbaleTimePicker();
                TimePickerOpeningAlternate.DisbaleTimePicker();
                ddlAuctionType.CssClass = "aspNetDisabled form-control";
                BtnSave.CssClass += "btn btn-primary disabled";
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        public bool GetEnableValue()
        {
            try
            {
                if ("" == Convert.ToString(hdnIsEditMode.Value))
                {
                    return true;
                }
                else
                {
                    if (!Convert.ToBoolean(hdnIsEditMode.Value))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return true;
        }
        public string GetClassValue(string val)
        {
            string ClassVal = string.Empty;
            try
            {
                if ("" == Convert.ToString(hdnIsEditMode.Value))
                {
                    if (val == "d")
                    {
                        ClassVal = "form-control date-picker required";
                    }
                    else
                    {
                        ClassVal = "form-control required";
                    }
                    
                }
                else
                {
                    if (!Convert.ToBoolean(hdnIsEditMode.Value))
                    {
                        if (val == "d")
                        {
                            ClassVal = "aspNetDisabled form-control date-picker";
                        }
                        else
                        {
                            ClassVal = "aspNetDisabled form-control";
                        }
                        
                    }
                    else
                    {
                        if (val == "d")
                        {
                            ClassVal = "form-control date-picker required";
                        }
                        else
                        {
                            ClassVal = "form-control required";
                        }
                        

                    }
                }



            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ClassVal;
        }

        public bool GetVisibleValue()
        {
            bool visible = false;
            try
            {
                if ("" == Convert.ToString(hdnIsEditMode.Value))
                {
                    visible = true;
                }
                else
                {
                    if (!Convert.ToBoolean(hdnIsEditMode.Value))
                    {
                        visible = false;
                    }
                    else
                    {
                        visible = true;
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return visible;
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

        [Serializable]
        public class Advertisement
        {
            public string AdvertisementSource { get; set; }
            public string AdvertisementDate { get; set; }

        }

        [Serializable]
        public class AdvertisementEdit
        {
            public int ID { get; set; }
            public string AdvertisementSource { get; set; }
            public string AdvertisementDate { get; set; }

        }
        private void PreviewImage(List<string> _Subject, bool IsEditMode)
        {
            List<string> lstName = new List<string>();
            foreach (var item in _Subject)
            {
                string filename = new System.IO.FileInfo(item).Name;
                lstName.Add(filename);
            }
            
            //lnkFile.Text = "File: " + filename;
            //lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            //string AttachmentPath = filename;
            
            
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = _Subject.Count;
            //if (!IsEditMode)
            //{
                FileUploadControl1.ViewUploadedFilesAsThumbnailHorizontally(Configuration.Auctions, lstName);
            //}
            //else
            //{
            //    FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.Auctions, lstName);
            //}
           

        }
    }
}