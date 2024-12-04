using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class TenderOpeningProcess : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    btnSave.Visible = base.CanAdd;
                    SetPageTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        BindCommitteeAttendeceData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        anchCommittee.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        anchContractor.HRef = string.Format("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN || mdlUser.DesignationID == (long)Constants.Designation.CE || mdlUser.DesignationID == (long)Constants.Designation.SE)
                        {
                            anchReport.Disabled = true;
                            anchReport.Attributes.Add("style", "color:black");

                        }
                        else
                        {
                            anchReport.Disabled = false;
                            anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        }


                        anchstatement.HRef = string.Format("~/Modules/Tenders/TenderNotice/ViewComparativeStatement.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        anchTenderPrice.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));

                        if (IsAwarded == true)
                        {
                            btnSave.Visible = false;
                            ddlMonitoredBy.Enabled = false;
                            ddlName.Enabled = false;
                            txtOpenedBy.Enabled = false;
                        }

                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindCommitteeAttendeceData(long _TenderWorkID, long _WorkSourceID)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");
                hdnDivisionID.Value = Utility.GetDynamicPropertyValue(WorkData, "DivisionID");

                List<object> CommitteeMembersData = new TenderManagementBLL().GetTenderCommitteeMembers(_TenderWorkID);
                gvEvalCommitteeAttend.DataSource = CommitteeMembersData;
                //  ViewState["vsCustomer"] = CommitteeMembersData;
                gvEvalCommitteeAttend.DataBind();
                dynamic ECMData = new TenderManagementBLL().GetECMDataByWorkID(_TenderWorkID);
                string MonitoredBy = Utility.GetDynamicPropertyValue(ECMData, "ECA_MonitoredBy");
                long DesignationID = 0;
                if (MonitoredBy == null)
                    ddlMonitoredBy.SelectedIndex = -1;



                if (!string.IsNullOrEmpty(MonitoredBy))
                {
                    if (MonitoredBy.Trim().ToUpper() == "MA")
                        ddlMonitoredBy.SelectedIndex = 2;
                    else
                        ddlMonitoredBy.SelectedIndex = 1;

                    long DivisionId = Convert.ToInt64(hdnDivisionID.Value);
                    if (ddlMonitoredBy.SelectedItem.Text.Trim().ToUpper() == "MA")
                        DesignationID = (long)Constants.Designation.MA;
                    else if (ddlMonitoredBy.SelectedItem.Text.Trim().ToUpper() == "ADM")
                        DesignationID = (long)Constants.Designation.ADM;
                    if (DesignationID != 0)
                    {
                        Dropdownlist.DDLGetUserByDivisionandDesignationID(ddlName, DivisionId, DesignationID, (int)Constants.DropDownFirstOption.Select);
                        string Name = Utility.GetDynamicPropertyValue(ECMData, "ECA_MonitoredName");
                        Dropdownlist.SetSelectedText(ddlName, Name);
                    }

                    if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                    {
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnSave.Visible = true;
                    }
                }
                else
                {
                    ddlMonitoredBy.SelectedIndex = -1;
                }
                txtOpenedBy.Text = Utility.GetDynamicPropertyValue(ECMData, "ECA_OpenedBy");

                System.Web.UI.HtmlControls.HtmlTableRow TableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                List<TM_TenderWorkAttachment> lstTenderWorkAttachments = new TenderManagementBLL().GetAllTenderWorkAttachments(_TenderWorkID, "ECA");

                if (lstTenderWorkAttachments.Count > 0)
                {
                    List<string> lstName = new List<string>();

                    foreach (var item in lstTenderWorkAttachments)
                    {
                        lstName.Add(item.Attachment);
                    }
                    string AttachmentPath = Convert.ToString(lstName);
                    FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    FileUploadControl1.Size = lstName.Count;
                    FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.TenderManagement, lstName);
                }

                //int Count = 0;
                //foreach (var item in lstTenderWorkAttachments)
                //{
                //    System.Web.UI.HtmlControls.HtmlTableCell TableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                //    HyperLink AttachmentHyperLink = new HyperLink();
                //    AttachmentHyperLink.ID = "lnk" + Count;
                //    AttachmentHyperLink.CssClass = "ADMLinks" + Count;
                //    AttachmentHyperLink.Text = item.Attachment.Substring(item.Attachment.LastIndexOf('_') + 1);
                //    AttachmentHyperLink.Attributes.Add("href", Utility.GetImageURL(Configuration.TenderManagement, item.Attachment));
                //    TableCell.Controls.Add(AttachmentHyperLink);
                //    TableCell.Style.Add("padding-left", "12px");
                //    TableRow.Controls.Add(TableCell);
                //    tblHyperlinks.Rows.Add(TableRow);
                //    Count++;
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}
                if (lstTenderWorkAttachments.Count > 0)
                {
                    HyperLinksDiv.Visible = true;
                }
                else
                {
                    HyperLinksDiv.Visible = false;

                }


                //string Attachment = Utility.GetDynamicPropertyValue(ECMData, "ECA_Attachment");
                //if (!string.IsNullOrEmpty(Attachment))
                //{
                //    hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, Attachment);
                //    //hlAttachment.Visible = true;

                //    hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.TenderManagement, Attachment);
                //    hlAttachment.Attributes["title"] = Attachment.Substring(Attachment.LastIndexOf('_') + 1);
                //    hlAttachment.Attributes["FileName"] = Attachment;
                //    hlAttachment.Visible = true;
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMonitoredBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            long DivisionId = Convert.ToInt64(hdnDivisionID.Value);
            long DesignationID = 0;
            if (!string.IsNullOrEmpty(ddlMonitoredBy.SelectedItem.Value))
            {
                DesignationID = Convert.ToInt64(ddlMonitoredBy.SelectedItem.Value);
            }

            if (DesignationID != 0)
            {
                Dropdownlist.DDLGetUserByDivisionandDesignationID(ddlName, DivisionId, DesignationID, (int)Constants.DropDownFirstOption.Select);
            }

        }

        protected void gvEvalCommitteeAttend_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtAlternateName = (TextBox)e.Row.FindControl("txtAlternateName");
                    TextBox txtAlternateDesignation = (TextBox)e.Row.FindControl("txtAlternateDesignation");
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkAlternate");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        chkSelect.Checked = true;
                    }
                    if (txtAlternateName.Text == "")
                    {
                        txtAlternateDesignation.Attributes.Add("disabled", "disabled");
                        txtAlternateName.Attributes.Add("disabled", "disabled");

                    }
                    else
                    {
                        chk.Checked = true;
                    }

                    bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));

                    if (IsAwarded == true)
                    {
                        chk.Enabled = false;
                        chkSelect.Enabled = false;
                    }
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            HttpFileCollection uploadedFiles = Request.Files;
            List<TM_TenderWorkAttachment> FileList = new List<TM_TenderWorkAttachment>();

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];
                if (userPostedFile.ContentLength > 0)
                {
                    string FullFileName = string.Empty;
                    string FileName = string.Empty;
                    string FileExt = string.Empty;
                    string sGuid = Guid.NewGuid().ToString();
                    FullFileName = userPostedFile.FileName;
                    FileExt = userPostedFile.ContentType;

                    System.IO.FileInfo oFileInfo = new System.IO.FileInfo(FullFileName);
                    string NewFileName = sGuid + "_" + oFileInfo.Name;
                    FileName = Path.GetFileNameWithoutExtension(oFileInfo.Name);

                    string filePath = Utility.GetImagePath(Configuration.TenderManagement);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        filePath = filePath + "\\" + NewFileName;
                        userPostedFile.SaveAs(filePath);

                        TM_TenderWorkAttachment mdlTenderWorkAttachments = new TM_TenderWorkAttachment();
                        mdlTenderWorkAttachments.Attachment = NewFileName;
                        mdlTenderWorkAttachments.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                        mdlTenderWorkAttachments.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlTenderWorkAttachments.CreatedDate = DateTime.Now;
                        mdlTenderWorkAttachments.AttachmentType = FileExt;
                        mdlTenderWorkAttachments.Source = "ECA";
                        FileList.Add(mdlTenderWorkAttachments);
                    }
                }

            }


            TenderManagementBLL TenderMgmtBLL = new TenderManagementBLL();
            List<TM_TenderCommitteeMembers> lstNew = new List<TM_TenderCommitteeMembers>();
            TM_TenderWorks mdlTenderWorks = new TM_TenderWorks();
            // TM_TenderWorkAttachment mdlTenderWorkAttachment = new TM_TenderWorkAttachment();
            foreach (GridViewRow gvrow in gvEvalCommitteeAttend.Rows)
            {
                TM_TenderCommitteeMembers mdlTenderCommittee = new TM_TenderCommitteeMembers();
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                CheckBox chkSelect = (CheckBox)gvrow.FindControl("chkAlternate");
                if (chk.Checked)
                {
                    mdlTenderCommittee.Attended = true;
                }
                TextBox AlternateName = (TextBox)gvrow.FindControl("txtAlternateName");
                TextBox AlternateDesig = (TextBox)gvrow.FindControl("txtAlternateDesignation");
                Label ID = (Label)gvrow.FindControl("ID");

                mdlTenderCommittee.ID = Convert.ToInt64(ID.Text);
                mdlTenderCommittee.AlternateName = AlternateName.Text;
                mdlTenderCommittee.AlternateDesignation = AlternateDesig.Text;
                mdlTenderCommittee.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                lstNew.Add(mdlTenderCommittee);

            }
            //List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);
            //  if (lstNameofFiles.Count > 0)
            //mdlTenderWorks.ECA_Attachment = lstNameofFiles[0].Item3;
            //  if (ddlMonitoredBy.SelectedItem.Text != "")
            mdlTenderWorks.ECA_MonitoredBy = ddlMonitoredBy.SelectedItem.Text;
            //   if (ddlName.SelectedIndex != -1)
            mdlTenderWorks.ECA_MonitoredName = ddlName.SelectedItem.Text;
            mdlTenderWorks.ECA_OpenedBy = txtOpenedBy.Text;

            mdlTenderWorks.ID = Convert.ToInt64(hdnTenderWorkID.Value);
            mdlTenderWorks.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);

            //mdlTenderWorkAttachment.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
            //mdlTenderWorkAttachment.Attachment = lstNameofFiles[0].Item3;
            //mdlTenderWorkAttachment.AttachmentType = "PNG";
            //mdlTenderWorkAttachment.Source = "ECA";
            //mdlTenderWorkAttachment.CreatedDate = DateTime.Now;
            //mdlTenderWorkAttachment.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);

            using (TransactionScope transaction = new TransactionScope())
            {

                for (int i = 0; i < lstNew.Count; i++)
                {
                    TenderMgmtBLL.UpdateTenderEvaluationCommittee(lstNew.ElementAt(i));
                }
                if (FileList.Count > 0)
                {
                    for (int i = 0; i < FileList.Count; i++)
                    {
                        TenderMgmtBLL.SaveTenderWorkAttachments(FileList.ElementAt(i));
                    }
                }
                TenderMgmtBLL.UpdateTenderWorksByWorkID(mdlTenderWorks);

                transaction.Complete();
                Works.AddContractorsAttendance.IsSaved = true;
                Response.Redirect("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value), false);
            }
            //if (gvEvalCommitteeAttend.Rows.Count > 0)
            //{
            //    DataTable dt = ViewState["vsCustomer"] as DataTable;
            //    if (dt != null)
            //    {
            //        if (dt.Rows.Count > 0)
            //        {
            //            string CustomerXML = DatatableToXml(dt);

            //            int id = new TenderManagementBLL().UpdateTenderCommitteeMembers(CustomerXML);
            //            //if (CustomerXML != null)
            //            //{
            //            //    using (SqlConnection con = new SqlConnection("Data Source=.;Trusted_Connection=true;Database=test"))
            //            //    {
            //            //        using (SqlCommand cmd = new SqlCommand())
            //            //        {
            //            //            cmd.Connection = con;
            //            //            cmd.CommandType = CommandType.StoredProcedure;
            //            //            cmd.CommandText = "Pr_SaveCustomer";
            //            //            cmd.Parameters.Add("@CustomerXml", SqlDbType.Xml, -1).Value = CustomerXML;
            //            //            con.Open();
            //            //            int i = cmd.ExecuteNonQuery();
            //            //            con.Close();
            //            //            if (i > 0)
            //            //            {
            //            //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Saved Successfully')", true);
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //        }
            //    }
            //}
        }

        public string GetURL(string Method, bool IsTenders)
        {
            string URL = string.Empty;
            try
            {
                if (IsTenders)
                {
                    URL = string.Format("~/Modules/Tenders/TenderNotice/" + Method + "?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                }
                else
                {
                    URL = string.Format("~/Modules/Tenders/Works/" + Method + "?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return URL;
        }
        //public string DatatableToXml(DataTable dt)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    dt.WriteXml(ms, true);
        //    ms.Seek(0, SeekOrigin.Begin);
        //    StreamReader sr = new StreamReader(ms);
        //    string strXML;
        //    strXML = sr.ReadToEnd();
        //    return (strXML);
        //}





    }
}