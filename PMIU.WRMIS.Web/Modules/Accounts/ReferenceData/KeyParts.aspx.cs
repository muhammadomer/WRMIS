using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class KeyParts : BasePage
    {
        #region Grid Data Key Index

        public const int KeyPartIDIndex = 0;

        #endregion

        List<AT_KeyParts> lstKeyParts = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindVehicleTypeDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 04-04-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the Vehicle Type dropdown
        /// Created On 04-04-2017
        /// </summary>
        private void BindVehicleTypeDropdown()
        {
            Dropdownlist.BindDropdownlist(ddlVehicleType, new ReferenceDataBLL().GetAssetType(), (int)Constants.DropDownFirstOption.Select, "AssetType", "ID");
        }

        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string VehicleType = ddlVehicleType.SelectedItem.Value;

                if (VehicleType != string.Empty)
                {
                    BindGrid();

                    gvKeyPart.Visible = true;
                }
                else
                {
                    gvKeyPart.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKeyPart_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvKeyPart.EditIndex = e.NewEditIndex;

                BindGrid();

                gvKeyPart.Rows[e.NewEditIndex].FindControl("txtPartName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKeyPart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvKeyPart.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKeyPart_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long KeyPartID = Convert.ToInt64(gvKeyPart.DataKeys[RowIndex].Values[KeyPartIDIndex]);
                string PartName = ((TextBox)gvKeyPart.Rows[RowIndex].FindControl("txtPartName")).Text.Trim();
                long AssetTypeID = Convert.ToInt64(ddlVehicleType.SelectedItem.Value);

                if (!IsValidAddEdit(AssetTypeID, PartName, KeyPartID))
                {
                    return;
                }

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                AT_KeyParts mdlKeyPart = bllReferenceData.GetKeyPartsByID(KeyPartID);

                int UserID = Convert.ToInt32(mdlUsers.ID);

                if (mdlKeyPart == null)
                {
                    mdlKeyPart = new AT_KeyParts
                    {
                        PartName = PartName,
                        AssetTypeID = AssetTypeID,
                        CreatedBy = UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = UserID,
                        ModifiedDate = DateTime.Now
                    };

                    bllReferenceData.AddKeyPart(mdlKeyPart);
                }
                else
                {
                    mdlKeyPart.PartName = PartName;
                    mdlKeyPart.ModifiedBy = UserID;
                    mdlKeyPart.ModifiedDate = DateTime.Now;

                    bllReferenceData.UpdateKeyPart(mdlKeyPart);
                }

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                if (KeyPartID == 0)
                {
                    gvKeyPart.PageIndex = 0;
                }

                gvKeyPart.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This functions checks if the record is valid
        /// Created On 04-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_PartName"></param>
        /// <param name="_KeyPartsID"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _AssetTypeID, string _PartName, long _KeyPartsID)
        {
            AT_KeyParts mdlKeyPart = new ReferenceDataBLL().GetPartByVehicleTypeAndName(_AssetTypeID, _PartName);

            if (mdlKeyPart != null && _KeyPartsID != mdlKeyPart.ID)
            {
                Master.ShowMessage(Message.PartNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvKeyPart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvKeyPart.PageIndex = e.NewPageIndex;

                gvKeyPart.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvKeyPart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long AssetTypeID = Convert.ToInt64(ddlVehicleType.SelectedItem.Value);

                    lstKeyParts = new ReferenceDataBLL().GetKeyPartsByVehicleType(AssetTypeID);

                    AT_KeyParts mdlKeyParts = new AT_KeyParts();

                    lstKeyParts.Add(mdlKeyParts);

                    gvKeyPart.PageIndex = gvKeyPart.PageCount;
                    gvKeyPart.DataSource = lstKeyParts;

                    gvKeyPart.EditIndex = (lstKeyParts.Count - 1) % gvKeyPart.PageSize;
                    gvKeyPart.DataBind();
                    gvKeyPart.Rows[gvKeyPart.Rows.Count - 1].FindControl("txtPartName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds data to the grid
        /// Created On 04-04-2017
        /// </summary>
        private void BindGrid()
        {
            long AssetTypeID = Convert.ToInt64(ddlVehicleType.SelectedItem.Value);

            lstKeyParts = new ReferenceDataBLL().GetKeyPartsByVehicleType(AssetTypeID);

            gvKeyPart.DataSource = lstKeyParts;
            gvKeyPart.DataBind();
        }

        protected void gvKeyPart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long KeyPartID = Convert.ToInt64(gvKeyPart.DataKeys[e.RowIndex].Values[KeyPartIDIndex]);

                if (!IsValidDelete(KeyPartID))
                {
                    return;
                }

                new ReferenceDataBLL().DeleteKeyPart(KeyPartID);

                Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _KeyPartID)
        {
            bool IsExist = new ReferenceDataBLL().IsKeyPartIDExists(_KeyPartID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }
    }
}