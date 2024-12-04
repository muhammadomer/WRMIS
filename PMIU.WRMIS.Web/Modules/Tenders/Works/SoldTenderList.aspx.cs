using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PMIU.WRMIS.Model;
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class SoldTenderList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                  //  UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        hdnTenderNoticeID.Value = Convert.ToString(TenderNoticeID);
                        BindWorkDetailData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        BindContractorsGrid(Convert.ToInt64(hdnTenderWorkID.Value));
                        
                    }

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript6", "RemoveFormRequired();", true);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindWorkDetailData(long _TenderWorkID,long _WorkSourceID)
        {
            try
            {
                dynamic mdlWorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(mdlWorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(mdlWorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(mdlWorkData, "WorkType");
                hdnWorkStatusID.Value = Utility.GetDynamicPropertyValue(mdlWorkData, "WorkStatusID");
                bool IsSubmitted = false;
                DateTime Submissiondate = Convert.ToDateTime(Utility.GetDynamicPropertyValue(mdlWorkData, "SubmissionDate"));
                if (DateTime.Now > Submissiondate)
                {
                    IsSubmitted = true;
                }
                hdnIsubmitted.Value = Convert.ToString(IsSubmitted);

                List<dynamic> lstItemsData = new TenderManagementBLL().GetSoldTenderListByWorkID(_TenderWorkID);

                if (lstItemsData.Count > 0)
                    btnCloseTender.Visible = false;

                gvSoldTenderList.DataSource = lstItemsData;
                gvSoldTenderList.DataBind();

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #region "Sold Tender Items Grid"

        protected void gvSoldTenderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                  //  HyperLink hlAttachmentName = (HyperLink)e.Row.FindControl("hlBankreceipt");
                    Label lblAttachment = (Label)e.Row.FindControl("lblAttachment");
                    //hlAttachmentName.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, hlAttachmentName.Text);
                    //hlAttachmentName.Text = hlAttachmentName.Text.Substring(hlAttachmentName.Text.LastIndexOf('_') + 1);

                    string AttachmentPath = lblAttachment.Text;
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        List<string> lstName = new List<string>();
                        lstName.Add(AttachmentPath);
                        WebFormsTest.FileUploadControl FileUploadControl1 = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl1");
                        FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        FileUploadControl1.Size = lstName.Count;
                        FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.TenderManagement, lstName);
                    }
                    Button btnEditSoldTenderGrid = (Button)e.Row.FindControl("btnEditSoldTenderGrid");

                    if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                        btnEditSoldTenderGrid.Visible = false;
                    if (IsAwarded == true)
                    {
                        btnEditSoldTenderGrid.Visible = false;
                    }
                }


                GridViewRow header = gvSoldTenderList.HeaderRow;
                if (header != null)
                {
                    Button btnSoldTenderGrid = header.FindControl("btnSoldTenderGrid") as Button;

                   bool IsSubmitted =   Convert.ToBoolean(hdnIsubmitted.Value);

                    if (btnSoldTenderGrid != null)
                    {
                       
                        if (IsAwarded == true)
                        {
                            btnSoldTenderGrid.CssClass += " disabled";
                           
                        }
                        if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                        {
                            btnSoldTenderGrid.CssClass += " disabled";
                        }
                        if (IsSubmitted )
                        {
                            btnSoldTenderGrid.CssClass += " disabled";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnCloseTender_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseTender", "$('#CloseTender').modal();", true);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);

            }
        }
        protected void gvSoldTenderList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

               // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript7", "AddFormRequired();", true);

                if (e.CommandName == "AddSoldTenderItem")
                {
                    List<dynamic> lstSoldTender = new TenderManagementBLL().GetSoldTenderListByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                    lstSoldTender.Add(
                    new
                    {
                        ID = 0,
                        CompanyName = string.Empty,
                        BankReceipt = string.Empty

                    });

                    gvSoldTenderList.PageIndex = gvSoldTenderList.PageCount;
                    gvSoldTenderList.DataSource = lstSoldTender;
                    gvSoldTenderList.DataBind();

                    gvSoldTenderList.EditIndex = gvSoldTenderList.Rows.Count - 1;
                    gvSoldTenderList.DataBind();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript8", "RemoveFormRequiredforadd();", true);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvSoldTenderList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                TenderManagementBLL TenderBLL = new TenderManagementBLL(); 
                TM_TenderWorksContractors mdlTenderWorkContractors = new TM_TenderWorksContractors();
                GridViewRow row = gvSoldTenderList.Rows[e.RowIndex];
                Label lblID = (Label)row.FindControl("lblID");

                TextBox txtCompanyName = (TextBox)row.FindControl("txtCompanyName");
                TextBox txtCompanyID = (TextBox)row.FindControl("txtCompanyID");

                HyperLink hlAttachmentName = (HyperLink)row.FindControl("hlBankRecieptLnk");
                var FileName = hlAttachmentName.Attributes["FullName"];
                FileUploadControl FileUploadControl = (FileUploadControl)row.FindControl("FileUpload");
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);

                if (lstNameofFiles.Count == 0)
                {
                    mdlTenderWorkContractors.BankReceipt = FileName;
                }
                else
                {
                    mdlTenderWorkContractors.BankReceipt = lstNameofFiles[0].Item3;
                }

                if (lblID.Text != "0")
                {
                    mdlTenderWorkContractors.ID = Convert.ToInt64(lblID.Text);

                }
                mdlTenderWorkContractors.ContractorsID = Convert.ToInt64(txtCompanyID.Text);
                mdlTenderWorkContractors.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                mdlTenderWorkContractors.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                mdlTenderWorkContractors.CreatedDate = DateTime.Now.Date;
                mdlTenderWorkContractors.Attended = false;
                //mdlTenderWorkContractors.BankReceipt = lstNameofFiles[0].Item3;
                bool IsExists = TenderBLL.IsContractorAlreadyAdded(mdlTenderWorkContractors.ContractorsID.Value, mdlTenderWorkContractors.TenderWorksID.Value);
                if (IsExists && mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                {
                    Master.ShowMessage(Message.TM_ContractorAlreadyExists.Description, SiteMaster.MessageType.Error);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript6", "RemoveFormRequired();", true);
                    return;
                }
                else
                {
                    bool IsSaved = TenderBLL.SaveTenderWorkContractor(mdlTenderWorkContractors);

                    if (IsSaved)
                    {
                        TenderBLL.UpdateTenderWorkStatusByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value), (int)Constants.WorkStatus.Sold);
                        gvSoldTenderList.EditIndex = -1;
                        BindContractorsGrid(Convert.ToInt64(hdnTenderWorkID.Value));
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);

                    }
                    else
                    {
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript3", "AddFormRequired();", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript4", "AddGridRequired();", true);
                }
               


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvSoldTenderList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSoldTenderList.EditIndex = -1;
                BindContractorsGrid(Convert.ToInt64(hdnTenderWorkID.Value));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "AddFormRequired();", true);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvSoldTenderList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript7", "RemoveFormRequired();", true);
                gvSoldTenderList.EditIndex = e.NewEditIndex;
                BindContractorsGrid(Convert.ToInt64(hdnTenderWorkID.Value));
                TextBox txtCompanyName = (TextBox)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("txtCompanyName");
                TextBox txtCompanyID = (TextBox)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("txtCompanyID");
                Label lblCompanyName = (Label)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("lblCompanyName");
                Label TenderWorkContractorID = (Label)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("lblID");
                FileUploadControl FileUpload2 = (FileUploadControl)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("FileUpload");
                //FileUploadControl FileUpload3 = (FileUploadControl)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("FileUpload3");
                FileUpload2.Visible = true;
                //FileUpload3.Visible = true;

                dynamic mdlTenderContractor = new TenderManagementBLL().GetTenderWorksContractor(Convert.ToInt64(TenderWorkContractorID.Text));
                txtCompanyID.Text = Utility.GetDynamicPropertyValue(mdlTenderContractor, "CompanyID");
                txtCompanyName.Text = Utility.GetDynamicPropertyValue(mdlTenderContractor, "CompanyName");
                HyperLink hlAttachmentName = (HyperLink)gvSoldTenderList.Rows[e.NewEditIndex].FindControl("hlBankRecieptLnk");
                string ImageText = Utility.GetDynamicPropertyValue(mdlTenderContractor, "BankReceipt");
                hlAttachmentName.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, ImageText);
                hlAttachmentName.Text = ImageText.Substring(ImageText.LastIndexOf('_') + 1);
                hlAttachmentName.Attributes["FullName"] = ImageText;
                hlAttachmentName.Visible = true;
                //BindContractorsGrid(Convert.ToInt64(hdnWorkID.Value));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript1", "RemoveFormRequired();", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript2", "RemoveGridRequired();", true);
                //txtDate.Text = Convert.ToString(obj.GetType().GetProperty("LatterDate").GetValue(obj));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindContractorsGrid(long _WorkID)
        {
            try
            {
                TenderManagementBLL TenderBLL = new TenderManagementBLL();
                List<dynamic> lstItemsData = new List<dynamic>();
                lstItemsData = TenderBLL.GetSoldTenderListByWorkID(_WorkID);
                if (lstItemsData.Count == 0)
                {
                    TenderBLL.UpdateTenderWorkStatusByTenderWorkID(_WorkID, (int)Constants.WorkStatus.NotSold);
                    lstItemsData = TenderBLL.GetSoldTenderListByWorkID(_WorkID);
                }
                else
                {
                    btnCloseTender.Visible = false;
                }
                gvSoldTenderList.DataSource = lstItemsData;
                gvSoldTenderList.DataBind();
 

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region "Contractor PopUp"

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static long SaveContractorNew(string _CompanyName, string _ContactPerson, string _Number, string _Address)
        {
            try
            {

                TM_Contractors mdlContractor = new TM_Contractors();
                mdlContractor.CompanyName = _CompanyName;
                mdlContractor.ContactPerson = _ContactPerson;
                mdlContractor.ContactNo = _Number;
                mdlContractor.Address = _Address;
                mdlContractor.IsActive = true;
                mdlContractor.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                mdlContractor.CreatedDate = DateTime.Now.Date;

                long ContractorID = new TenderManagementBLL().AddContractorFromSoldTenderList(mdlContractor);

                return ContractorID;

            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return 0;
            }

        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<dynamic> GetContractors()
        //{
        //    try
        //    {



        //        List<dynamic> lstContractors = new TenderManagementBLL().GetContractorsList();

        //        return lstContractors;
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

        //        return null;
        //    }
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<dynamic> GetContractors(string _Name)
        {
            try
            {

                _Name = _Name.Trim();


                List<dynamic> lstContractors = new TenderManagementBLL().GetContractorsList(_Name);

                return lstContractors;
            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return null;
            }
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TM_TenderWorks mdlTenderWorks = new TM_TenderWorks();
                Int32 _WorKStatusID = 0;
                long _WorkID = Convert.ToInt64(hdnTenderWorkID.Value);
                //if (ChkBoxStatus.Checked)
                //{
                    _WorKStatusID = (int)Constants.WorkStatus.Closed;

              //  }
                List<Tuple<string, string, string>> lstNameofFiles = FileUpload1.UploadNow(Configuration.TenderManagement);

                mdlTenderWorks.ID = _WorkID;
                //if (_WorKStatusID != 0)
                //{
                    mdlTenderWorks.WorkStatusID = _WorKStatusID;
                //}
                mdlTenderWorks.StatusReason = txtReason.Text;
                mdlTenderWorks.StatusAttachment = lstNameofFiles[0].Item3;
                mdlTenderWorks.StatusBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                mdlTenderWorks.StatusDate = DateTime.Now.Date;

                bool IsSaved = new TenderManagementBLL().SaveTenderWorkStatusInfromation(mdlTenderWorks);

                if (IsSaved)
                {
                    BindWorkDetailData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                    Master.ShowMessage(Message.TM_TenderClosed.Description, SiteMaster.MessageType.Success);
                    //AddWorks.IsSaved = true;
                    //Response.Redirect("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + Convert.ToInt64(hdnTenderNoticeID.Value), false); 
                }
                else
                {
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }

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
                if ((long)Constants.WorkStatus.Closed == Convert.ToInt64(hdnWorkStatusID.Value))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return true;
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
    }
}