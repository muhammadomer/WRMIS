using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public delegate void DelegatePopulateData();
    public partial class Zone : BasePage
    {
        List<CO_Zone> lstZone = new List<CO_Zone>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //DelegatePopulateData delPopulate = new DelegatePopulateData(this.BindGrid);
            //DataPaging1.UpdatePageIndex = delPopulate;

            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 21-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Zone);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the grid
        /// and shows the page according to the added record.
        /// Created on 19-10-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid()
        {
            lstZone = new ZoneBLL().GetAllZones();

            //DataPaging1.TotalRecords = lstZone.Count;

            //lstZone = GetPageData<CO_Zone>(lstZone, DataPaging1.PageIndex, DataPaging1.RecordsPerPage);



            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstZone.Select(z => z.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvZone.PageIndex = itemIndex / gvZone.PageSize;
            //}
            #endregion

            gvZone.DataSource = lstZone;
            gvZone.DataBind();
        }

        protected void gvZone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstZone = new ZoneBLL().GetAllZones();

                    CO_Zone mdlZone = new CO_Zone();

                    mdlZone.ID = 0;
                    mdlZone.Name = "";
                    mdlZone.Description = "";
                    lstZone.Add(mdlZone);

                    //gvZone.PageIndex = gvZone.PageCount;
                    //DataPaging1.TotalRecords = lstZone.Count;


                    //int PageCount = (lstZone.Count % DataPaging1.RecordsPerPage == 0) ? lstZone.Count / DataPaging1.RecordsPerPage : (lstZone.Count / DataPaging1.RecordsPerPage) + 1;

                    //int PageCount = lstZone.Count / DataPaging1.RecordsPerPage;
                    //PageCount += (lstZone.Count % DataPaging1.RecordsPerPage == 0) ? 0 : 1;
                    //DataPaging1.PageIndex = PageCount;

                    //lstZone = GetPageData<CO_Zone>(lstZone, DataPaging1.PageIndex, DataPaging1.RecordsPerPage);

                    gvZone.PageIndex = gvZone.PageCount;
                    gvZone.DataSource = lstZone;
                    gvZone.DataBind();

                    gvZone.EditIndex = gvZone.Rows.Count - 1;
                    gvZone.DataBind();

                    gvZone.Rows[gvZone.Rows.Count - 1].FindControl("txtName").Focus();

                    //DataPaging1.DisableNow();

                    //gvZone.EditIndex = gvZone.Rows.Count - 1;
                    //gvZone.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvZone.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ZoneID = Convert.ToInt64(((Label)gvZone.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string ZoneName = ((TextBox)gvZone.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string ZoneDescription = ((TextBox)gvZone.Rows[RowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(ZoneID, ZoneName))
                {
                    return;
                }

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                ZoneBLL bllZone = new ZoneBLL();

                CO_Zone mdlZone = bllZone.GetZoneByID(ZoneID);

                bool IsRecordSaved = false;

                if (mdlZone == null)
                {
                    mdlZone = new CO_Zone
                    {
                        Name = ZoneName,
                        Description = ZoneDescription,
                        CreatedBy = mdlUsers.ID,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = mdlUsers.ID,
                        ModifiedDate = DateTime.Now,
                        IsActive = true
                    };

                    IsRecordSaved = bllZone.AddZone(mdlZone);
                }
                else
                {
                    mdlZone.Name = ZoneName;
                    mdlZone.Description = ZoneDescription;
                    mdlZone.ModifiedBy = mdlUsers.ID;
                    mdlZone.ModifiedDate = DateTime.Now;

                    IsRecordSaved = bllZone.UpdateZone(mdlZone);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (ZoneID == 0)
                    {
                        gvZone.PageIndex = 0;
                    }

                    gvZone.EditIndex = -1;
                    BindGrid();
                }

                //DataPaging1.EnableNow();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvZone.EditIndex = e.NewEditIndex;

                BindGrid();

                gvZone.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ZoneID = Convert.ToInt64(((Label)gvZone.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(ZoneID))
                {
                    return;
                }

                ZoneBLL bllZone = new ZoneBLL();

                bool IsDeleted = bllZone.DeleteZone(ZoneID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvZone.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function check whether data is valid for add/edit operation.
        /// Created On 19-10-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <param name="_ZoneName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _ZoneID, string _ZoneName)
        {
            ZoneBLL bllZone = new ZoneBLL();

            CO_Zone mdlSearchedZone = bllZone.GetZoneByName(_ZoneName);

            if (mdlSearchedZone != null && _ZoneID != mdlSearchedZone.ID)
            {
                Master.ShowMessage(Message.ZoneNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created On 19-10-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _ZoneID)
        {
            ZoneBLL bllZone = new ZoneBLL();

            bool IsExist = bllZone.IsZoneIDExists(_ZoneID);

            if (!IsExist)
            {
                long ZoneIrrigationLevelID = 1;

                IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _ZoneID);
            }

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvZone_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvZone.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}