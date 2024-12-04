using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class Province : BasePage
    {
        List<CO_Province> lstProvince = new List<CO_Province>();
        protected void Page_Load(object sender, EventArgs e)
        {
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
        /// Created on 04-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Province);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds provinces to the grid
        /// and shows the page according to the added record.
        /// Created on 04-11-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            lstProvince = new ProvinceBLL().GetAllProvinces();

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstProvince.Select(p => p.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvProvince.PageIndex = itemIndex / gvProvince.PageSize;
            //}
            #endregion

            gvProvince.DataSource = lstProvince;
            gvProvince.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for add/edit operation.
        /// Created On 04-11-2015
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <param name="_ProvinceName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _ProvinceID, string _ProvinceName)
        {
            ProvinceBLL bllProvince = new ProvinceBLL();

            CO_Province mdlSearchedProvince = bllProvince.GetProvinceByName(_ProvinceName);

            if (mdlSearchedProvince != null && _ProvinceID != mdlSearchedProvince.ID)
            {
                Master.ShowMessage(Message.ProvinceNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created On 04-11-2015
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _ProvinceID)
        {
            ProvinceBLL bllProvince = new ProvinceBLL();

            bool IsExist = bllProvince.IsProvinceIDExists(_ProvinceID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvProvince_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProvince.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProvince_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvProvince.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProvince_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstProvince = new ProvinceBLL().GetAllProvinces();

                    CO_Province mdlProvince = new CO_Province();

                    mdlProvince.ID = 0;
                    mdlProvince.Name = "";
                    mdlProvince.Description = "";
                    lstProvince.Add(mdlProvince);

                    gvProvince.PageIndex = gvProvince.PageCount;
                    gvProvince.DataSource = lstProvince;
                    gvProvince.DataBind();

                    gvProvince.EditIndex = gvProvince.Rows.Count - 1;
                    gvProvince.DataBind();

                    gvProvince.Rows[gvProvince.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProvince_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ProvinceID = Convert.ToInt64(((Label)gvProvince.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(ProvinceID))
                {
                    return;
                }

                ProvinceBLL bllProvince = new ProvinceBLL();

                bool IsDeleted = bllProvince.DeleteProvince(ProvinceID);

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

        protected void gvProvince_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvProvince.EditIndex = e.NewEditIndex;

                BindGrid();

                gvProvince.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProvince_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ProvinceID = Convert.ToInt64(((Label)gvProvince.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string ProvinceName = ((TextBox)gvProvince.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string ProvinceDescription = ((TextBox)gvProvince.Rows[RowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(ProvinceID, ProvinceName))
                {
                    return;
                }

                CO_Province mdlProvince = new CO_Province();

                mdlProvince.ID = ProvinceID;
                mdlProvince.Name = ProvinceName;
                mdlProvince.Description = ProvinceDescription;

                ProvinceBLL bllProvince = new ProvinceBLL();

                bool IsRecordSaved = false;

                if (ProvinceID == 0)
                {
                    IsRecordSaved = bllProvince.AddProvince(mdlProvince);
                }
                else
                {
                    IsRecordSaved = bllProvince.UpdateProvince(mdlProvince);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (ProvinceID == 0)
                    {
                        gvProvince.PageIndex = 0;
                    }

                    gvProvince.EditIndex = -1;
                    BindGrid(mdlProvince.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProvince_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvProvince.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}