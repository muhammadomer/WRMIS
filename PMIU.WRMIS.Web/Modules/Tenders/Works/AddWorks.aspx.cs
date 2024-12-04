using PMIU.WRMIS.BLL.Tenders;
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
using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.Tenders.TenderNotice;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class AddWorks : BasePage
    {
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    hlBack.NavigateUrl = "~/Modules/Tenders/TenderNotice/SearchTenderNotice.aspx?ShowHistory=true";
                    SetPageTitle();
                    BindDropdown();
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderNoticeID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hdnTenderNoticeID.Value = Request.QueryString["TenderNoticeID"];
                        BindTenderDetailData(Convert.ToInt64(hdnTenderNoticeID.Value));
                        BindWorksGrid(Convert.ToInt64(hdnTenderNoticeID.Value));
                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindTenderDetailData(long _TenderNoticeID)
        {
            try
            {
                dynamic mdlTenderData = new TenderManagementBLL().GetTenderDataByID(_TenderNoticeID);
                string TenderNotice = Utility.GetDynamicPropertyValue(mdlTenderData, "Name");
                string Domain = Utility.GetDynamicPropertyValue(mdlTenderData, "Domain");
                string Division = Utility.GetDynamicPropertyValue(mdlTenderData, "Division");

                Tenders.Controls.AddWorks.TenderNotice = TenderNotice;
                Tenders.Controls.AddWorks.TenderDomain = Domain;
                Tenders.Controls.AddWorks.TenderDivision = Division;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindWorksGrid(long _TenderNoticeID)
        {
            try
            {
                List<dynamic> lstWorksData = new TenderManagementBLL().GetWorksDataByTenderNoticeID(_TenderNoticeID);
                gvTenderWorks.DataSource = lstWorksData;
                gvTenderWorks.DataBind();

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderWorks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region "Data Keys"

                    DataKey key = gvTenderWorks.DataKeys[e.Row.RowIndex];
                    long TenderWorkID = Convert.ToInt64(key.Values["TenderWorkID"]);
                    long WorkSourceID = Convert.ToInt64(key.Values["WorkSourceID"]);
                    long ID = Convert.ToInt64(key.Values["ID"]);

                    #endregion "Data Keys"
                    if (e.Row.RowIndex != -1)
                    {
                        var WorkStatus = e.Row.FindControl("lblWorkStatusID") as Label;
                        var WorkStatusID = Convert.ToInt64(WorkStatus.Text);
                        Button BtnDeleteWork = (Button)e.Row.FindControl("lbtnDeleteWork");
                        Button btnOpeningOffice = (Button)e.Row.FindControl("btnOpeningOffice");
                        HyperLink hlDetails = (HyperLink)e.Row.FindControl("gvWorkDetail");
                        HyperLink hlItems = (HyperLink)e.Row.FindControl("gvWorkItems");
                        HyperLink hlSoldList = (HyperLink)e.Row.FindControl("gvSoldTenderList");
                        HyperLink hlEvalCommittee = (HyperLink)e.Row.FindControl("gvTenderEvaluationCommitte");
                        //HyperLink hlOpeningProcess = (HyperLink)e.Row.FindControl("gvTenderOpeningProcess");
                        Button btnTenderOpeningProcess = (Button)e.Row.FindControl("btnTenderOpeningProcess");
                        hdnWorkStatusID.Value = WorkStatusID.ToString();
                        bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(TenderWorkID));
                        //bool? IsOpeningOfficeExist = new TenderManagementBLL().CheckOpeningOfficeByWorkSourceID(Convert.ToInt64(ID));

                        //if (IsOpeningOfficeExist == true)
                        //{
                        //    // Master.ShowMessage("Select Opening Office to Proceed", SiteMaster.MessageType.Error);
                        //    //btnTenderOpeningProcess.Enabled = false;
                        //    btnOpeningOffice.Enabled = true;
                        //}
                        //else
                        //{
                        //    //btnTenderOpeningProcess.Enabled = true;
                        //    btnOpeningOffice.Enabled = true;
                        //}
                        if (IsAwarded == true)
                        {
                            btnOpeningOffice.Enabled = false;
                            // btnTenderOpeningProcess.Enabled = true;
                        }
                        if ((long)Constants.WorkStatus.Cancelled == WorkStatusID)
                        {
                            //hlDetails.CssClass += " disabled";
                            //hlItems.CssClass += " disabled";
                            //hlSoldList.CssClass += " disabled";
                            //hlEvalCommittee.CssClass += " disabled";
                            //hlOpeningProcess.CssClass += " disabled";
                            //BtnDeleteWork.CssClass += " disabled";
                        }
                        else if ((long)Constants.WorkStatus.Closed == WorkStatusID)
                        {
                            // hlDetails.CssClass += " disabled";
                            //hlItems.CssClass += " disabled";
                            hlSoldList.CssClass += " disabled";
                            hlEvalCommittee.CssClass += " disabled";
                            //btnTenderOpeningProcess.CssClass += " disabled";
                            btnTenderOpeningProcess.Enabled = false;
                            BtnDeleteWork.CssClass += " disabled";
                        }
                        else if ((long)Constants.WorkStatus.Sold == WorkStatusID)
                        {
                            BtnDeleteWork.CssClass += " disabled";
                        }


                    }
                }

                if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                {
                    GridViewRow header = gvTenderWorks.HeaderRow;
                    if (header != null)
                    {
                        HyperLink btnAddWork = header.FindControl("btnAddWork") as HyperLink;
                        if (btnAddWork != null)
                        {
                            btnAddWork.Visible = false;
                        }
                    }
                }

            }

            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvTenderWorks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long TenderWorkID = Convert.ToInt64(GetDataKeyValue(gvTenderWorks, "ID", e.RowIndex));
                bool IsDeleted = new TenderManagementBLL().DeleteWorkByTenderWorkID(TenderWorkID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindWorksGrid(Convert.ToInt64(hdnTenderNoticeID.Value));
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

        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }

        public string GetURLValue()
        {
            string val = string.Empty;
            try
            {
                val =
                    string.Format("~/Modules/Tenders/Works/AddWorkItems.aspx?TenderNoticeID=" +
                                  Convert.ToInt64(hdnTenderNoticeID.Value));


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return val;
        }
        private void BindDropdown()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlOpeningOffice, CommonLists.GetTenderOffices());
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

        protected void gvTenderWorks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "OpeningOffice")
                {
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    DataKey key = gvTenderWorks.DataKeys[row.RowIndex];
                    Int64 TenderWorkID = Convert.ToInt64(key["TenderWorkID"]);
                    Int64 CWID = Convert.ToInt64(key["WorkSourceID"]);
                    Int64 ID = Convert.ToInt64(key["ID"]);

                    hdnTenderWorkID.Value = Convert.ToString(TenderWorkID);
                    hdnCWID.Value = Convert.ToString(CWID);
                    ViewState["ID"] = Convert.ToString(ID);

                    //ddlOpeningOffice.ClearSelection();
                    //ddlOfficeLocated.SelectedItem.Text = "All";


                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddOpeningOffices", "$('#AddOpeningOffices').modal();", true);
                }
                else if (e.CommandName == "TenderOpeningProcess")
                {
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    DataKey key = gvTenderWorks.DataKeys[row.RowIndex];
                  //  long ID = Convert.ToInt64(key["ID"]);
                    long WorkSourceID = Convert.ToInt64(key["WorkSourceID"]);
                    long TenderWorkID = Convert.ToInt64(e.CommandArgument);
                    bool? IsOpeningOfficeExist = new TenderManagementBLL().CheckOpeningOfficeByWorkSourceID(Convert.ToInt64(TenderWorkID));
                    if (IsOpeningOfficeExist == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "anything", "alert('Opening office is not entered for selected tender.Please enter opening office to proceed further');", true);
                    }
                    else
                    {
                        Response.Redirect("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToString(TenderWorkID) + "&WorkSourceID=" + Convert.ToString(WorkSourceID), false);
                    }


                }

            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlOpeningOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlOpeningOffice.SelectedItem.Value;
            if (value == "Z")
            {
                lblOfficeLocation.InnerText = "Zone";
                //Dropdownlist.DDLZones(ddlOfficeLocated);
                long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                Dropdownlist.DDLZoneByCirclelID(ddlOfficeLocated, Convert.ToInt64(CircleID), (int)Constants.DropDownFirstOption.NoOption);
            }
            else if (value == "C")
            {
                lblOfficeLocation.InnerText = "Circle";
                Dropdownlist.DDLCirlceByDivisionlID(ddlOfficeLocated, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), (int)Constants.DropDownFirstOption.NoOption);
                //Dropdownlist.DDLALLCircles(ddlOfficeLocated);
            }
            else if (value == "D")
            {
                lblOfficeLocation.InnerText = "Division";
                //Dropdownlist.DDLDivisions(ddlOfficeLocated);
                Dropdownlist.DDLWorksDivision(ddlOfficeLocated, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), (int)Constants.DropDownFirstOption.NoOption);

            }
            else
            {
                lblOfficeLocation.InnerText = "Office";
                Dropdownlist.DDLALLOtherTenderOffices(ddlOfficeLocated);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string Office = ddlOpeningOffice.SelectedItem.Text;
                long OfficeID = Convert.ToInt64(ddlOfficeLocated.SelectedItem.Value);


                bool IsRecordUpdated = new TenderManagementBLL().UpdateOpeningOfficeByTenderID(Convert.ToInt64(ViewState["ID"]), Office, OfficeID);
                if (IsRecordUpdated)
                {
                    Master.ShowMessage("Your Action has been Completed.", SiteMaster.MessageType.Success);
                    BindWorksGrid(Convert.ToInt64(hdnTenderNoticeID.Value));
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        private void LaodClosureWorkData(long p1, long p2)
        {
            throw new NotImplementedException();
        }
    }
}