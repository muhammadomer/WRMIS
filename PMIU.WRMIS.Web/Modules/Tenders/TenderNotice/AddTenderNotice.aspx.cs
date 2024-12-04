using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using System.Globalization;
using PMIU.WRMIS.Web.Modules.Tenders.Controls;
using System.Transactions;

namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class AddTenderNotice : BasePage
    {
        #region ViewState Constants

        string AdvertisementSourcesList = "AdvertisementSourcesList";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindDomainDropDown(mdlUser);
                    BindDivisionDropdown(mdlUser);
                    //hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                    hlBack.NavigateUrl = "~/Modules/Tenders/TenderNotice/SearchTenderNotice.aspx?ShowHistory=true";
                    SetPageTitle();
                    bool IsEditMode = false;
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderNoticeID"]))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["IsEditMode"]))
                        {
                            IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);
                            hdnIsEditMode.Value = Convert.ToString(IsEditMode);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequiredKeyWord();", true);
                        }
                        hdnTenderNoticeID.Value = Request.QueryString["TenderNoticeID"];
                        BindTenderNoticeData(Convert.ToInt64(hdnTenderNoticeID.Value), IsEditMode);
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
                    long TenderNoticeID = Convert.ToInt64(GetDataKeyValue(gvAdvertisementSource, "TenderNoticeID", e.Row.RowIndex));


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
                if (hdnTenderNoticeID.Value != "0")
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


        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);
                ddlDivision.SelectedIndex = 1;
            }
            else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
            }
            else if (_MdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                ddlDivision.SelectedIndex = 1;

            }

        }

        private void BindDomainDropDown(UA_Users _MdlUser)
        {
            try
            {
                // Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDomain, new TenderManagementBLL().GetDomainsByUserID(_MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID));
                if (_MdlUser.UA_Designations.IrrigationLevelID != null)
                {
                    Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
                    ddlDomain.SelectedIndex = 1;
                }
                else
                {
                    Dropdownlist.DDLDomainByUserID(ddlDomain, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
                    
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                if (mdlUsers.DesignationID == (long)Constants.Designation.ChiefMonitoring)
                {
                    long SelectedDomainID = Convert.ToInt64(ddlDomain.SelectedValue);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, new TenderManagementBLL().GetDivisionByDomainID(SelectedDomainID));
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
                if (gvAdvertisementSource.Rows.Count == 0)
                {
                    Master.ShowMessage(Message.TM_EnterAdvertismentSource.Description, SiteMaster.MessageType.Error);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Calculation();", true);
                    return;
                }
                string SubmissionTime = TimePickerSubmission.GetTime();
                string OpeningTime = TimePickerOpening.GetTime();
                var SubmissionDateTime = Utility.GetParsedDateTime(txtSubmissionDate.Text, SubmissionTime);
                var OpeningDateTime = Utility.GetParsedDateTime(txtOpeningDate.Text, OpeningTime);
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
               TM_TenderNotice TenderNoticeModel = new TM_TenderNotice();
                if (hdnTenderNoticeID.Value != "0")
                {
                    TenderNoticeModel.ID = Convert.ToInt64(hdnTenderNoticeID.Value);
                }
                //if hlImage.Text 
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);
                if (lstNameofFiles.Count > 0)
                    TenderNoticeModel.TenderNoticeFile = lstNameofFiles[0].Item3;
                else
                    TenderNoticeModel.TenderNoticeFile = hdnFileName.Value;
                TenderNoticeModel.Name = txtTenderNotice.Text;
                TenderNoticeModel.DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
                TenderNoticeModel.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                TenderNoticeModel.CreatedDate = DateTime.Now;
                TenderNoticeModel.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                TenderNoticeModel.Remarks = txtRemarks.Text;
                TenderNoticeModel.BidOpeningDate = Utility.GetParsedDateTime(txtOpeningDate.Text, OpeningTime);
                TenderNoticeModel.BidSubmissionDate = Utility.GetParsedDateTime(txtSubmissionDate.Text, SubmissionTime);
                
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


                    if (hdnTenderNoticeID.Value != "0")
                    {
                        bool isDeleted = new TenderManagementBLL().DeletePublishingSourceForUpdation(Convert.ToInt64(hdnTenderNoticeID.Value));
                    }
                }

                bool IsSaved = false;
                long TenderNoticeID = 0;

                using (TransactionScope transaction = new TransactionScope())
                {
                    TenderNoticeID = new TenderManagementBLL().SaveTenderNotice(TenderNoticeModel);
                    if (lstAdvertisementSource != null)
                    {
                        foreach (var item in PublishingSourceList)
                        {
                            TM_TenderPublishedIn publishingSource = new TM_TenderPublishedIn();

                            if (hdnTenderNoticeID.Value != "0")
                            {
                                publishingSource.TenderNoticeID = Convert.ToInt64(hdnTenderNoticeID.Value);
                            }
                            else
                            {
                                publishingSource.TenderNoticeID = TenderNoticeID;
                            }
                            publishingSource.PublisingSource = item.AdvertisementSource;
                            publishingSource.PublishedDate = Utility.GetParsedDate(item.AdvertisementDate);
                            publishingSource.CreatedDate = DateTime.Now.Date;
                            publishingSource.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;

                            IsSaved = new TenderManagementBLL().SavePublishingSource(publishingSource);

                        }
                    }

                    transaction.Complete();
                }
           //     new TenderManagementBLL().GetTenderNoticeID();
                    if (hdnTenderNoticeID.Value == "0")
                    {
                        PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                        _event.Parameters.Add("TenderNoticeID", TenderNoticeID);
                        _event.Parameters.Add("TenderWorkID", 0);
                        _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.AddTenderNotice, (int)SessionManagerFacade.UserInformation.ID);
                    }
                    
                    Works.AddWorks.IsSaved = true;
                    Response.Redirect("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID, false);

                


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindTenderNoticeData(long _TenderNoticeID, bool IsEditMode)
        {
            try
            {
                TM_TenderNotice TenderNoticeData = new TenderManagementBLL().GetTenderNoticeByID(_TenderNoticeID);
                txtTenderNotice.Text = TenderNoticeData.Name;
                txtOpeningDate.Text = Utility.GetFormattedDate(TenderNoticeData.BidOpeningDate);
                txtSubmissionDate.Text = Utility.GetFormattedDate(TenderNoticeData.BidSubmissionDate);
                string OpeningTime = Utility.GetFormattedTime(TenderNoticeData.BidOpeningDate);
                var OT = Convert.ToDateTime(OpeningTime);
                TimePickerOpening.SetTime(OT.ToString("hh"), OT.ToString("mm"), OT.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                string SubmissionTime = Utility.GetFormattedTime(TenderNoticeData.BidSubmissionDate);
                var ST = Convert.ToDateTime(SubmissionTime);
                TimePickerSubmission.SetTime(ST.ToString("hh"), ST.ToString("mm"), ST.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                txtRemarks.Text = TenderNoticeData.Remarks;
                ddlDomain.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDomain, Convert.ToString(TenderNoticeData.DomainID));
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, new TenderManagementBLL().GetDivisionByDomainID(TenderNoticeData.DomainID));
                ddlDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(TenderNoticeData.DivisionID));
                hdnFileName.Value = TenderNoticeData.TenderNoticeFile;
                if (IsEditMode)
                {
                    //hlImage.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, TenderNoticeData.TenderNoticeFile);
                    //hlImage.Visible = true;

                    if (!string.IsNullOrEmpty(TenderNoticeData.TenderNoticeFile))
                    {
                        //hlImage.NavigateUrl = Utility.GetImageURL(Configuration.Complaints, Convert.ToString(mdlComplaint.Attachment));
                        //hlImage.Visible = true;
                        string AttachmentPath = Convert.ToString(TenderNoticeData.TenderNoticeFile);
                        List<string> lstName = new List<string>();
                        lstName.Add(AttachmentPath);
                        FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        FileUploadControl1.Size = lstName.Count;
                        FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.TenderManagement, lstName);

                    }
                }
                else
                {
                    List<string> FileName = new List<string>();
                    FileName.Add(TenderNoticeData.TenderNoticeFile);
                    FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                    FileUploadControl.UploadedFilesNames(Configuration.TenderManagement, FileName);
                }

                List<dynamic> PublishingData = new TenderManagementBLL().GetPublishingSourceByTenderNoticeID(_TenderNoticeID);
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
                txtRemarks.Enabled = false;
                SubmissionDate.Visible = false;
                OpeningDate.Visible = false;
                txtSubmissionDate.Enabled = false;
                txtSubmissionDate.CssClass = "aspNetDisabled form-control";
                txtTenderNotice.Enabled = false;
                txtTenderNotice.CssClass = "aspNetDisabled form-control";
                ddlDivision.Enabled = false;
                ddlDivision.CssClass = "aspNetDisabled form-control";
                ddlDomain.Enabled = false;
                TimePickerSubmission.DisbaleTimePicker();
                TimePickerOpening.DisbaleTimePicker();
                ddlDomain.CssClass = "aspNetDisabled form-control";
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
        public string GetClassValue()
        {
            string ClassVal = string.Empty;
            try
            {
                if ("" == Convert.ToString(hdnIsEditMode.Value))
                {
                    ClassVal = "form-control date-picker required";
                }
                else
                {
                    if (!Convert.ToBoolean(hdnIsEditMode.Value))
                    {
                        ClassVal = "aspNetDisabled form-control date-picker";
                    }
                    else
                    {
                        ClassVal = "form-control date-picker required";

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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
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
    }
}