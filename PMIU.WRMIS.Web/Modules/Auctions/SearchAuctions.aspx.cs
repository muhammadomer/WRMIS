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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class SearchAuctions : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        public const string UserSubDivisionKey = "UserSubDivision";
        public const string UserSectionKey = "UserSection";

        #endregion

        #region Hash Table keys

        public const string UserNameKey = "UserName";
        public const string NameKey = "Name";
        public const string StatusIDKey = "StatusID";
        public const string RoleIDKey = "RoleID";
        public const string OrganizationIDKey = "OrganizationID";
        public const string DesignationIDKey = "DesignationID";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string SubDivisionIDKey = "SubDivisionID";
        public const string SectionIDKey = "SectionID";
        public const string PageIndexKey = "PageIndex";

        #endregion

        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindUserLocation();
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);
                        if (IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchAuction] != null)
                            {
                                BindHistoryData();
                            }
                        }

                    }
                    hlAdd.Visible = base.CanAdd;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            ViewState.Add(UserIDKey, mdlUser.ID);

            if (mdlUser.RoleID != Constants.AdministratorRoleID)
            {
                if (mdlUser.UA_Designations.IrrigationLevelID != null)
                {
                    List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            #region Zone Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);
                            ddlCircle.SelectedIndex = 1;
                            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value), -1, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            #region Circle Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserZone.Add(mdlCircle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserCircle.Add((long)mdlDivision.CircleID);
                                lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            //Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            #region Sub Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSubDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserDivision.Add((long)mdlSubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            //ddlSubDivision.DataSource = lstSubDivision;
                            //ddlSubDivision.DataTextField = "Name";
                            //ddlSubDivision.DataValueField = "ID";
                            //ddlSubDivision.DataBind();
                            //ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();



                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                        {
                            #region Section Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSection.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserSubDivision.Add((long)mdlSection.SubDivID);
                                lstUserDivision.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            //ddlSubDivision.DataSource = lstSubDivision;
                            //ddlSubDivision.DataTextField = "Name";
                            //ddlSubDivision.DataValueField = "ID";
                            //ddlSubDivision.DataBind();
                            //ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();



                            #endregion
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
            ViewState.Add(UserSubDivisionKey, lstUserSubDivision);
            ViewState.Add(UserSectionKey, lstUserSection);
        }

        protected void gvAuctionNotice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string ID = GetDataKeyValue(gvAuctionNotice, "ID", e.Row.RowIndex);
                    HyperLink hlBidders = (HyperLink)e.Row.FindControl("hlBidders");
                    HyperLink hlApprovalProcess = (HyperLink)e.Row.FindControl("hlApprovalProcess");
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                    {
                        hlBidders.NavigateUrl = string.Format("~/Modules/Auctions/Bidders.aspx?AuctionNoticeID={0}", Convert.ToInt64(ID));
                        hlApprovalProcess.NavigateUrl = string.Format("~/Modules/Auctions/ApprovalProcessForXEN.aspx?AuctionNoticeID={0}", Convert.ToInt64(ID));
                        
                    }
                    else
                    {
                        hlBidders.NavigateUrl = string.Format("~/Modules/Auctions/ViewBidderList.aspx?AuctionNoticeID={0}", Convert.ToInt64(ID));
                        hlApprovalProcess.NavigateUrl = string.Format("~/Modules/Auctions/ApprovalProcessForSECE.aspx?AuctionNoticeID={0}", Convert.ToInt64(ID));
                    }


                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        protected void gvAuctionNotice_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvAuctionNotice.EditIndex = -1;
                BtnSearch_Click(null, null);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuctionNotice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAuctionNotice.PageIndex = e.NewPageIndex;
                BtnSearch_Click(null, null);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // UA_Users mdlUser = SessionManagerFacade.UserInformation;
                List<long> lstUserZone = new List<long>();
                List<long> lstUserCircle = new List<long>();
                List<long> lstUserDivision = new List<long>();
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];
                UA_Users mdlUser = new UserBLL().GetUserByID(UserID);
                List<dynamic> SearchCriteria = new List<dynamic>();
                List<long> DivisionIDs = new List<long>();
                if (DivisionID == -1)
                {
                    if (ddlDivision.SelectedItem.Text.ToUpper() == "ALL")
                    {
                        foreach (ListItem item in ddlDivision.Items)
                        {
                            if (item.Value != "")
                            {
                                DivisionIDs.Add(Convert.ToInt64(item.Value));
                            }
                           
                        }
                    }
                }
                else
                {
                    DivisionIDs.Add(DivisionID);
                }



                //if (DivisionID == -1 && mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                //{
                //    ddlDivision.SelectedIndex = 1;
                //    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                //}
                string AuctionName = txtAuctionNotice.Text;
                DateTime? OpeningFrom = null;
                DateTime? OpeningTo = null;
                if (txtOpeningDateFrom.Text != String.Empty)
                {
                    OpeningFrom = DateTime.Parse(txtOpeningDateFrom.Text);
                }
                if (txtOpeningDateTo.Text != String.Empty)
                {
                    OpeningTo = DateTime.Parse(txtOpeningDateTo.Text);
                }
                if ((OpeningFrom != null && OpeningTo != null) && (OpeningFrom > OpeningTo))
                {
                    Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description,
                        SiteMaster.MessageType.Error);
                    gvAuctionNotice.Visible = false;
                    return;
                }
                BindGrid(AuctionName, DivisionIDs, OpeningFrom, OpeningTo);
                SearchCriteria.Add(AuctionName);
                SearchCriteria.Add(DivisionID);
                SearchCriteria.Add(ZoneID);
                SearchCriteria.Add(CircleID);
                SearchCriteria.Add(OpeningFrom);
                SearchCriteria.Add(OpeningTo);
                SearchCriteria.Add(gvAuctionNotice.PageIndex);
                Session[SessionValues.SearchAuction] = SearchCriteria;

            }

            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid(string _AuctionNoticeName, List<long> _DivisionID, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo)
        {

            List<AC_AuctionNotice> lstAuctionNotice = new AuctionBLL().GetAllAuctionNotice(_AuctionNoticeName, _DivisionID, _OpeningDateFrom, _OpeningDateTo);
            gvAuctionNotice.DataSource = lstAuctionNotice;
            gvAuctionNotice.DataBind();
            gvAuctionNotice.Visible = true;
        }

        private void BindHistoryData()
        {
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.SearchAuction];
            txtAuctionNotice.Text = SearchCriteria[0];

            long DivisionID = Convert.ToInt64(SearchCriteria[1]);
            long ZoneID = Convert.ToInt64(SearchCriteria[2]);
            long CircleID = Convert.ToInt64(SearchCriteria[3]);

            if (ZoneID != -1)
            {
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID.ToString());
                if (CircleID != -1)
                {
                    // Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, new TenderManagementBLL().GetDivisionByDomainID(DomainID));
                    Dropdownlist.SetSelectedValue(ddlCircle, CircleID.ToString());
                }
                if (DivisionID != -1)
                {
                    Dropdownlist.SetSelectedValue(ddlDivision, DivisionID.ToString());
                }
            }

            txtOpeningDateFrom.Text = SearchCriteria[4];
            txtOpeningDateTo.Text = SearchCriteria[5];
            gvAuctionNotice.PageIndex = (int)SearchCriteria[6];
            BtnSearch_Click(null, null);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Auctions);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}