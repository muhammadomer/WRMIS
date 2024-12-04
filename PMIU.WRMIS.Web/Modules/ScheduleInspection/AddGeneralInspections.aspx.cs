using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddGeneralInspections : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                  
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        hdnScheduleDetailID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        if (!string.IsNullOrEmpty(Request.QueryString["IsViewMode"]))
                        {
                            bool IsViewMode = Convert.ToBoolean(Request.QueryString["IsViewMode"]);

                            BindScheduleData(Convert.ToInt64(hdnScheduleDetailID.Value),true,IsViewMode);
                        }
                        
                        }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["GID"]))
                        {
                            bool IsScheduled = false;
                            hdnGID.Value = Convert.ToString(Request.QueryString["GID"]);
                            BindScheduleData(Convert.ToInt64(hdnScheduleDetailID.Value),IsScheduled,true);
                        }
                        else
                        {
                            BindScheduleData(Convert.ToInt64(hdnScheduleDetailID.Value));
                        }
                        
                    }
                    //if (!string.IsNullOrEmpty(Request.QueryString["Source"]))
                    //{

                    //    string Source = Convert.ToString(Request.QueryString["Source"]);
                    //    hdnSource.Value = Source;
                    //    if (Source == "C")
                    //    {
                    //        hlBack.NavigateUrl = "~/Modules/ScheduleInspection/ScheduleCalendar.aspx";
                            
                    //    }
                    //    else if (Source == "I")
                    //    {
                    //        hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchInspection.aspx";
                    //    }
                    //    else if (Source == "N")
                    //    {
                    //        long ScheduleID = new ScheduleInspectionBLL().GetScheduleIDByScheduleDetailGeneralID(Convert.ToInt64(hdnScheduleDetailID.Value));
                    //        hlBack.NavigateUrl = "~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + ScheduleID;

                    //    }
                    //}
                    //else
                    //{
                    //    hlBack.CssClass = "btn disabled";
                    //    txtInspectionDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    //}
                    
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
              
                SI_GeneralInspections mdlGeneralInspection = new SI_GeneralInspections();
                mdlGeneralInspection.GeneralInspectionTypeID = Convert.ToInt64(ddlGeneralInspectionType.SelectedItem.Value);
                DateTime dt = DateTime.Parse(txtInspectionDate.Text);
                DateTime result = dt.Add(DateTime.Now.TimeOfDay);
                mdlGeneralInspection.InspectionDate = result;
                mdlGeneralInspection.InspectionDetails = txtDetails.Text;
                mdlGeneralInspection.InspectionLocation = txtLocation.Text;
                mdlGeneralInspection.Remarks = txtRemarks.Text;
                mdlGeneralInspection.CreatedBy = Convert.ToInt64(SessionManagerFacade.UserInformation.ID);
                mdlGeneralInspection.CreatedDate = DateTime.Now;
                if (Convert.ToInt64(hdnScheduleDetailID.Value) > 0)
                {
                    mdlGeneralInspection.ScheduleDetailGeneralID = Convert.ToInt64(hdnScheduleDetailID.Value);
                }
                //new WRException(Constants.UserID, new Exception{ Convert.ToString(SessionManagerFacade.UserInformation.ID)}).LogException(Constants.MessageCategory.WebApp);
                //List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.ScheduleInspection);
                //if (lstNameofFiles == null || lstNameofFiles.Count() == 0)
                //{
                //    Master.ShowMessage("Attachment is required.", SiteMaster.MessageType.Error);
                //    return;
                //}

                using (TransactionScope transaction = new TransactionScope())
                {
                    ScheduleInspectionBLL ScheduleBLL = new ScheduleInspectionBLL();
                    long GeneralInspectionID = ScheduleBLL.SaveGeneralInspection(mdlGeneralInspection);
                    if (GeneralInspectionID > 0)
                    {
                        List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.ScheduleInspection);
                        if (lstNameofFiles != null || lstNameofFiles.Count > 0)
                        {
                            for (int i = 0; i < lstNameofFiles.Count; i++)
                            {
                                SI_GeneralInspectionsAttachment mdlGeneralInspectionAttachments = new SI_GeneralInspectionsAttachment();
                                mdlGeneralInspectionAttachments.GeneralInspectionsID = GeneralInspectionID;
                                mdlGeneralInspectionAttachments.Attachment = lstNameofFiles[i].Item3;
                                mdlGeneralInspectionAttachments.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                                mdlGeneralInspectionAttachments.CreatedDate = DateTime.Now;
                                ScheduleBLL.SaveGeneralInspectionAttachments(mdlGeneralInspectionAttachments);

                            }
                        }
                    }
                    transaction.Complete();
                   //btnSave.CssClass = "btn disabled";
                    //Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                }

                if (hdnSource.Value.ToString() == "A" || hdnSource.Value.ToString() == "I")
                {
                    SearchInspection.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/SearchInspection.aspx", false);
                }
                else if (hdnSource.Value.ToString() == "C")
                {
                    ScheduleCalendar.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleCalendar.aspx", false);
                }
                else if (hdnSource.Value.ToString() == "N")
                {
                    ScheduleInspectionNotes.IsSaved = true;
                    long ScheduleID = new ScheduleInspectionBLL().GetScheduleIDByScheduleDetailGeneralID(Convert.ToInt64(hdnScheduleDetailID.Value));
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + ScheduleID, false);
                }

                
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void Check_InspectionDates(object sender, EventArgs e)
        {
            try
            {
                if (txtInspectionDate.Text != "")
                {
                    if (Convert.ToInt64(hdnScheduleID.Value) != 0)
                    {
                        bool IsInspectionDateInValid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtInspectionDate.Text));
                        if (IsInspectionDateInValid)
                        {
                            Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                            string Now = Utility.GetFormattedDate(DateTime.Now);
                            txtInspectionDate.Text = Now;
                        } 
                    }
                 
                }

            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindScheduleData(long _ScheduleDetailID,bool IsScheduled = true, bool IsviewMode = false)
        {
            try
            {
                if (_ScheduleDetailID > 0)
                {
                    if (IsviewMode)
                    {
                        dynamic ScheduleData = new ScheduleInspectionBLL().GetScheduleDataForGeneralIsnpections(_ScheduleDetailID);
                        string Title = ScheduleData.GetType().GetProperty("Name").GetValue(ScheduleData, null);
                        string Status = ScheduleData.GetType().GetProperty("Status").GetValue(ScheduleData, null);
                        string PreparedBy = ScheduleData.GetType().GetProperty("PreparedBy").GetValue(ScheduleData, null);
                        var InspectionTypeID = ScheduleData.GetType().GetProperty("GinspectionType").GetValue(ScheduleData, null);
                        var InspectionDate = ScheduleData.GetType().GetProperty("GInspectionDate").GetValue(ScheduleData, null);
                        var Location = ScheduleData.GetType().GetProperty("GLocation").GetValue(ScheduleData, null);
                        var InspectionDetails = ScheduleData.GetType().GetProperty("GDetails").GetValue(ScheduleData, null);
                        var Remarks = ScheduleData.GetType().GetProperty("GRemarks").GetValue(ScheduleData, null);
                        var GID = ScheduleData.GetType().GetProperty("GeneralInspectionID").GetValue(ScheduleData, null);
                        DateTime FromDate = ScheduleData.GetType().GetProperty("FromDate").GetValue(ScheduleData, null);
                        DateTime ToDate = ScheduleData.GetType().GetProperty("ToDate").GetValue(ScheduleData, null);
                        ScheduleInspection.Controls.ScheduleDetail.Title = Title;
                        ScheduleInspection.Controls.ScheduleDetail.Status = Status;
                        ScheduleInspection.Controls.ScheduleDetail.PreparedBy = PreparedBy;
                        ScheduleInspection.Controls.ScheduleDetail.FromDate = Utility.GetFormattedDate(FromDate);
                        ScheduleInspection.Controls.ScheduleDetail.ToDate = Utility.GetFormattedDate(ToDate);
                        hdnScheduleID.Value = Utility.GetDynamicPropertyValue(ScheduleData,"ID");
                        BindInspectionTypeDropDown(Convert.ToString(InspectionTypeID));
                        txtInspectionDate.Text = Utility.GetFormattedDate(DateTime.Now); //Utility.GetFormattedDateTime(InspectionDate);
                        txtInspectionDate.Enabled = false;
                        txtInspectionDate.CssClass = "form-control date-picker";
                        spanInspectionDate.Visible = false;
                        txtLocation.Text = Location;
                        txtLocation.Enabled = false;
                        txtLocation.CssClass = "form-control";
                        txtDetails.Text = InspectionDetails;
                        txtDetails.Enabled = false;
                        txtDetails.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                        txtRemarks.Text = Remarks;
                        txtRemarks.Enabled = false;
                        btnSave.CssClass = "btn btn-primary disabled";
                        //VIEW UPLOADED FILES AS LINK
                        List<string> lstFileNames = new ScheduleInspectionBLL().GetUploadedFileNamesForGeneralInspections(Convert.ToInt64(GID));
                        //FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                        //FileUploadControl.UploadedFilesNames(Configuration.ScheduleInspection, lstFileNames);
                        if (lstFileNames.Count > 0)
                            PreviewImage(lstFileNames);

                    }
                    else
                    {
                        dynamic ScheduleData = new ScheduleInspectionBLL().GetScheduleDataForGeneralIsnpections(_ScheduleDetailID);
                        string Title = ScheduleData.GetType().GetProperty("Name").GetValue(ScheduleData, null);
                        string Status = ScheduleData.GetType().GetProperty("Status").GetValue(ScheduleData, null);
                        string PreparedBy = ScheduleData.GetType().GetProperty("PreparedBy").GetValue(ScheduleData, null);
                        var InspectionTypeID = ScheduleData.GetType().GetProperty("InspectionTypeID").GetValue(ScheduleData, null);
                        var InspectionDate = ScheduleData.GetType().GetProperty("InspectionDate").GetValue(ScheduleData, null);
                        var Location = ScheduleData.GetType().GetProperty("Location").GetValue(ScheduleData, null);
                        DateTime FromDate = ScheduleData.GetType().GetProperty("FromDate").GetValue(ScheduleData, null);
                        DateTime ToDate = ScheduleData.GetType().GetProperty("ToDate").GetValue(ScheduleData, null);
                        ScheduleInspection.Controls.ScheduleDetail.Title = Title;
                        ScheduleInspection.Controls.ScheduleDetail.Status = Status;
                        ScheduleInspection.Controls.ScheduleDetail.PreparedBy = PreparedBy;
                        ScheduleInspection.Controls.ScheduleDetail.FromDate = Utility.GetFormattedDate(FromDate);
                        ScheduleInspection.Controls.ScheduleDetail.ToDate = Utility.GetFormattedDate(ToDate);
                        hdnScheduleID.Value = Utility.GetDynamicPropertyValue(ScheduleData, "ID");
                        BindInspectionTypeDropDown(Convert.ToString(InspectionTypeID));
                        txtInspectionDate.Text = Utility.GetFormattedDate(DateTime.Now);//Utility.GetFormattedDate(InspectionDate);
                        txtLocation.Text = Location;
                    }
                   
                }
                else
                {
                    USerControl.Visible = false;
                    BindInspectionTypeDropDown(string.Empty);

                    if (!IsScheduled)
                    {
                        SI_GeneralInspections mdlGeneralInspection = new ScheduleInspectionBLL().GetGeneralInspectionsByID(Convert.ToInt64(hdnGID.Value));
                        BindInspectionTypeDropDown(Convert.ToString(mdlGeneralInspection.GeneralInspectionTypeID));
                        txtInspectionDate.Text = Utility.GetFormattedDate(DateTime.Now);//Utility.GetFormattedDateTime(mdlGeneralInspection.InspectionDate);
                        txtInspectionDate.Enabled = false;
                        txtInspectionDate.CssClass = "form-control date-picker";
                        spanInspectionDate.Visible = false;
                        txtLocation.Text = mdlGeneralInspection.InspectionLocation;
                        txtLocation.Enabled = false;
                        txtLocation.CssClass = "form-control";
                        txtDetails.Text = mdlGeneralInspection.InspectionDetails;
                        txtDetails.Enabled = false;
                        txtDetails.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                        txtRemarks.Text = mdlGeneralInspection.Remarks;
                        txtRemarks.Enabled = false;
                        btnSave.CssClass = "btn btn-primary disabled";
                        //VIEW UPLOADED FILES AS LINK
                        List<string> lstFileNames = new ScheduleInspectionBLL().GetUploadedFileNamesForGeneralInspections(Convert.ToInt64(hdnGID.Value));
                        //FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.View);
                        //FileUploadControl.UploadedFilesNames(Configuration.ScheduleInspection, lstFileNames);
                        if (lstFileNames.Count > 0)
                            PreviewImage(lstFileNames);
                    }
                }
               
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
            private void BindInspectionTypeDropDown(string _SelectedVal)
        {
            try
            {
                Dropdownlist.DDLGeneralInspectionType(ddlGeneralInspectionType, false, (int)Constants.DropDownFirstOption.Select);
                if (!string.IsNullOrEmpty(_SelectedVal))
                {
                    Dropdownlist.SetSelectedValue(ddlGeneralInspectionType, _SelectedVal);
                    ddlGeneralInspectionType.CssClass = "form-control disabled";
                    ddlGeneralInspectionType.Enabled = false;
                    
                }
                else
                {
                    ddlGeneralInspectionType.Enabled = true;
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddGaugeInspection);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void PreviewImage(List<string> lstName)
        {
            //string filename = new System.IO.FileInfo(_Subject).Name;
            ////lnkFile.Text = "File: " + filename;
            ////lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            //string AttachmentPath = filename;
            //List<string> lstName = new List<string>();
            //lstName.Add(AttachmentPath);
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = lstName.Count;
            FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.ScheduleInspection, lstName);
            FileUploadControl.Size = 0;
        }
    }
}