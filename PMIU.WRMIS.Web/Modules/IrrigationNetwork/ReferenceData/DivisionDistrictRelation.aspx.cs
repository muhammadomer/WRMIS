using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class DivisionDistrictRelation : BasePage
    {
        List<CO_District> lstDistrict = new List<CO_District>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDivisionDropdown();
                    BindGrid();
                    if (!base.CanEdit)
                    {
                        btnSave.Visible = false;
                        LbtnReset.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 22-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DivisionDistrict);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds all districts to the Grid
        /// Created On:04-11-2015
        /// </summary>
        private void BindGrid()
        {
            lstDistrict = new DistrictBLL().GetAllDistricts();
            gvDistricts.DataSource = lstDistrict;
            gvDistricts.DataBind();
        }
        /// <summary>
        /// this function binds dropdowns
        /// Created On:05-10-2015
        /// </summary>
        private void BindDropDown()
        {
            long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
            DivisionDistrictRelationBLL bllDivisionDistrict = new DivisionDistrictRelationBLL();
            List<CO_DistrictDivision> lstDivisionDistrict = bllDivisionDistrict.GetAllDistrictsByDivisionID(DivisionID);

            if (DivisionID != -1)
            {
                for (int i = 0; i < gvDistricts.Rows.Count; i++)
                {
                    long DistrictID = Convert.ToInt32(((Label)gvDistricts.Rows[i].Cells[0].FindControl("lblID")).Text);
                    CheckBox chkboxDistrict = (CheckBox)gvDistricts.Rows[i].Cells[2].FindControl("chkDistrict");
                    chkboxDistrict.Checked = false;
                    if (lstDivisionDistrict.Any(s => s.DistrictID == DistrictID))
                    {
                        chkboxDistrict.Checked = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < gvDistricts.Rows.Count; i++)
                {
                    CheckBox chkboxDistrict = (CheckBox)gvDistricts.Rows[i].Cells[2].FindControl("chkDistrict");
                    chkboxDistrict.Checked = false;
                }
            }
        }
        private void BindDivisionDropdown()
        {
            Dropdownlist.DDLDivisions(ddlDivision);
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropDown();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long DivisionID = Convert.ToInt32(ddlDivision.SelectedItem.Value);
            DivisionDistrictRelationBLL bllDivisionDistrict = new DivisionDistrictRelationBLL();
            List<CO_DistrictDivision> lstDivisionDistrict = bllDivisionDistrict.GetAllDistrictsByDivisionID(DivisionID);
            CO_DistrictDivision mdlDivisionDistrict = new CO_DistrictDivision();
            bool IsChecked = false;
            bool IsUpdated = false;

            for (int i = 0; i < gvDistricts.Rows.Count; i++)
            {
                CheckBox chkboxDistrict = (CheckBox)gvDistricts.Rows[i].Cells[2].FindControl("chkDistrict");




                long DistrictID = Convert.ToInt32(((Label)gvDistricts.Rows[i].Cells[0].FindControl("lblID")).Text);
                if (chkboxDistrict.Checked == true)
                {
                    IsChecked = true;
                    if (!lstDivisionDistrict.Any(s => s.DistrictID == DistrictID))
                    {
                        mdlDivisionDistrict.DistrictID = DistrictID;
                        mdlDivisionDistrict.DivisionID = DivisionID;
                        IsUpdated = bllDivisionDistrict.AddDistrictDivision(mdlDivisionDistrict);
                    }
                }
                else
                {
                    if (lstDivisionDistrict.Any(s => s.DistrictID == DistrictID))
                    {
                        IsUpdated = bllDivisionDistrict.DeleteDistrictDivision(DistrictID, DivisionID);
                    }
                }
            }
            if (IsUpdated)
            {
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }

            if (!IsChecked)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
            }

        }

        protected void lnkBtnCancel_Click(object sender, EventArgs e)
        {
            BindDropDown();
        }

        protected void gvDistricts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (!base.CanEdit)
                {
                    CheckBox chkDistrict = (CheckBox)e.Row.FindControl("chkDistrict");
                    chkDistrict.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}