using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.ReferenceData
{
    public partial class StructureData : BasePage
    {
        List<CO_GaugeSlipSite> lstStructureData = new List<CO_GaugeSlipSite>();
        public double MaxAFSQ = 1499999.99;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindStructureDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 22-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.StructureData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds structures to the structure dropdown
        /// Created on 23-11-2015
        /// </summary>
        private void BindStructureDropdown()
        {
            Dropdownlist.DDLStructures(ddlStructure, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds structure data to the grid based on the selected Structure ID 
        /// Created on 23-11-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid()
        {
            long StructureID = Convert.ToInt64(ddlStructure.SelectedItem.Value);
            lstStructureData = new StructureBLL().GetGaugeSlipSiteByStationID(StructureID);

            gvStructureData.DataSource = lstStructureData;
            gvStructureData.DataBind();
        }

        /// <summary>
        /// This function check whether Up Stream and Down Stream exist.
        /// If it does not exist then function adds these to database.
        /// Created On 27-11-2015
        /// </summary>
        /// <param name="_StructureID"></param>
        private void CheckDefaultValues(long _StructureID)
        {
            StructureBLL bllStructure = new StructureBLL();

            List<CO_GaugeSlipSite> lstGaugeSlipSite = bllStructure.GetGaugeSlipSiteByStationID(_StructureID);

            CO_GaugeSlipSite mdlGaugeSlipSite = new CO_GaugeSlipSite();

            if (!lstGaugeSlipSite.Any(g => g.Name.ToUpper().Trim() == Constants.UpStream.ToUpper().Trim()))
            {
                mdlGaugeSlipSite.Name = Constants.UpStream;
                mdlGaugeSlipSite.StationID = _StructureID;

                bllStructure.AddGaugeSlipSite(mdlGaugeSlipSite);
            }

            if (!lstGaugeSlipSite.Any(g => g.Name.ToUpper().Trim() == Constants.DownStream.ToUpper().Trim()))
            {
                mdlGaugeSlipSite.Name = Constants.DownStream;
                mdlGaugeSlipSite.StationID = _StructureID;

                bllStructure.AddGaugeSlipSite(mdlGaugeSlipSite);
            }
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 26-11-2015
        /// </summary>
        /// <param name="_StructureDataID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _StructureDataID)
        {
            StructureBLL bllStructure = new StructureBLL();

            bool IsExist = bllStructure.IsGaugeSlipSiteIDExists(_StructureDataID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        private bool IsValidAddEdit(long _StructureDataID, string _SiteName, string _Channel, long _StructureID)
        {
            long ChannelID = -1;

            if (_Channel != String.Empty)
            {
                ChannelID = Convert.ToInt64(_Channel);
            }

            StructureBLL bllStructure = new StructureBLL();

            CO_GaugeSlipSite mdlSearchedGaugeSlipSite = bllStructure.GetGaugeSlipSiteBySiteNameAndChannelID(_SiteName, ChannelID, _StructureID);

            if (mdlSearchedGaugeSlipSite != null && _StructureDataID != mdlSearchedGaugeSlipSite.ID)
            {
                if (ChannelID != -1)
                {
                    Master.ShowMessage(Message.SiteChannelNameExists.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    Master.ShowMessage(Message.SiteNameExists.Description, SiteMaster.MessageType.Error);
                }

                return false;
            }

            return true;
        }

        protected void ddlStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlStructure.SelectedItem.Value == String.Empty)
                {
                    gvStructureData.Visible = false;
                    litGridTitle.Visible = false;
                }
                else
                {
                    long StructureID = Convert.ToInt64(ddlStructure.SelectedItem.Value);

                    gvStructureData.EditIndex = -1;
                    gvStructureData.PageIndex = 0;

                    CO_Station mdlStation = new StructureBLL().GetStationByID(StructureID);

                    if (mdlStation.StructureTypeID == (long)Constants.StructureType.Barrage || mdlStation.StructureTypeID == (long)Constants.StructureType.Headwork)
                    {
                        CheckDefaultValues(StructureID);
                    }

                    BindGrid();

                    gvStructureData.Visible = true;
                    litGridTitle.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStructureData.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStructureData.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long StructureID = Convert.ToInt64(ddlStructure.SelectedItem.Value);

                    lstStructureData = new StructureBLL().GetGaugeSlipSiteByStationID(StructureID);

                    CO_GaugeSlipSite mdlGaugeSlip = new CO_GaugeSlipSite();

                    mdlGaugeSlip.ID = 0;
                    mdlGaugeSlip.Name = "";
                    mdlGaugeSlip.Description = "";
                    mdlGaugeSlip.AFSQ = null;
                    lstStructureData.Add(mdlGaugeSlip);

                    gvStructureData.PageIndex = gvStructureData.PageCount;
                    gvStructureData.DataSource = lstStructureData;
                    gvStructureData.DataBind();

                    gvStructureData.EditIndex = gvStructureData.Rows.Count - 1;
                    gvStructureData.DataBind();

                    gvStructureData.Rows[gvStructureData.Rows.Count - 1].FindControl("txtSiteName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long StructureDataID = Convert.ToInt64(((Label)gvStructureData.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(StructureDataID))
                {
                    return;
                }

                StructureBLL bllStructure = new StructureBLL();

                bool IsDeleted = bllStructure.DeleteGaugeSlipSite(StructureDataID);

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

        protected void gvStructureData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStructureData.EditIndex = e.NewEditIndex;

                BindGrid();

                TextBox txtSiteName = (TextBox)gvStructureData.Rows[e.NewEditIndex].FindControl("txtSiteName");

                if (txtSiteName.Text == Constants.UpStream || txtSiteName.Text == Constants.DownStream)
                {
                    gvStructureData.Rows[e.NewEditIndex].FindControl("txtRemarks").Focus();
                }
                else
                {
                    gvStructureData.Rows[e.NewEditIndex].FindControl("txtSiteName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long StructureID = Convert.ToInt64(ddlStructure.SelectedItem.Value);

                long StructureDataID = Convert.ToInt64(((Label)gvStructureData.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string SiteName = ((TextBox)gvStructureData.Rows[RowIndex].Cells[1].FindControl("txtSiteName")).Text.Trim();
                string Channel = ((DropDownList)gvStructureData.Rows[RowIndex].Cells[2].FindControl("ddlChannel")).SelectedItem.Value;
                string Gauge = ((DropDownList)gvStructureData.Rows[RowIndex].Cells[3].FindControl("ddlGaugeRd")).SelectedItem.Value;
                string AFSQ = ((TextBox)gvStructureData.Rows[RowIndex].Cells[4].FindControl("txtAFSQ")).Text.Trim();
                string Remarks = ((TextBox)gvStructureData.Rows[RowIndex].Cells[5].FindControl("txtRemarks")).Text.Trim();

                if (!IsValidAddEdit(StructureDataID, SiteName, Channel, StructureID))
                {
                    return;
                }

                CO_GaugeSlipSite mdlGaugeSlipSite = new CO_GaugeSlipSite();

                mdlGaugeSlipSite.ID = StructureDataID;
                mdlGaugeSlipSite.Name = SiteName;

                if (Channel != String.Empty)
                {
                    mdlGaugeSlipSite.ChannelID = Convert.ToInt64(Channel);
                }

                if (Gauge != String.Empty)
                {
                    mdlGaugeSlipSite.GaugeID = Convert.ToInt64(Gauge);
                }

                if (AFSQ != String.Empty)
                {
                    mdlGaugeSlipSite.AFSQ = Convert.ToDouble(AFSQ);
                }

                mdlGaugeSlipSite.Description = Remarks;
                mdlGaugeSlipSite.StationID = StructureID;

                StructureBLL bllStructure = new StructureBLL();

                bool IsRecordSaved = false;

                if (StructureDataID == 0)
                {
                    mdlGaugeSlipSite.ID = bllStructure.GetMaxID() + 1;
                    IsRecordSaved = bllStructure.AddGaugeSlipSite(mdlGaugeSlipSite);
                }
                else
                {
                    IsRecordSaved = bllStructure.UpdateGaugeSlipSite(mdlGaugeSlipSite);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (StructureDataID == 0)
                    {
                        gvStructureData.PageIndex = 0;
                    }

                    gvStructureData.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvStructureData.EditIndex == e.Row.RowIndex)
                    {
                        TextBox txtSiteName = (TextBox)e.Row.FindControl("txtSiteName");
                        DropDownList ddlChannel = (DropDownList)e.Row.FindControl("ddlChannel");
                        DropDownList ddlGaugeRd = (DropDownList)e.Row.FindControl("ddlGaugeRd");
                        TextBox txtAFSQ = (TextBox)e.Row.FindControl("txtAFSQ");

                        if (txtSiteName.Text == Constants.UpStream || txtSiteName.Text == Constants.DownStream)
                        {
                            txtSiteName.Enabled = false;
                            ddlChannel.Enabled = false;
                            ddlGaugeRd.Enabled = false;
                            txtAFSQ.Enabled = false;
                            txtSiteName.CssClass = "form-control";
                        }
                        else
                        {
                            ddlChannel.Enabled = true;
                            ddlGaugeRd.Enabled = true;
                            txtAFSQ.Enabled = true;

                            txtAFSQ.Attributes.Add("onkeyup", "NumberValueValidation(this," + MaxAFSQ + ")");
                            txtAFSQ.Attributes.Add("placeholder", string.Format("{0} - {1}", 0, MaxAFSQ));

                            long StructureID = Convert.ToInt64(ddlStructure.SelectedItem.Value);

                            Dropdownlist.DDLStructureChannels(ddlChannel, StructureID, false, (int)Constants.DropDownFirstOption.Select, true);

                            Label lblChannelID = (Label)e.Row.FindControl("lblChannelID");
                            Dropdownlist.SetSelectedValue(ddlChannel, lblChannelID.Text);

                            if (ddlChannel.SelectedItem.Value != String.Empty)
                            {
                                ddlGaugeRd.Enabled = true;
                                ddlGaugeRd.CssClass = "form-control required";

                                long ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);

                                List<CO_ChannelGauge> lstChannelGauge = new StructureBLL().GetGaugesByChannelID(ChannelID, true);

                                ddlGaugeRd.DataSource = lstChannelGauge.Select(g => new { ID = g.ID, Rds = Calculations.GetRDText(g.GaugeAtRD) });

                                ddlGaugeRd.DataTextField = "Rds";
                                ddlGaugeRd.DataValueField = "ID";

                                ddlGaugeRd.DataBind();

                                ddlGaugeRd.Items.Insert(0, new ListItem("Select", ""));

                                Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                                ddlGaugeRd.SelectedValue = lblGaugeID.Text;

                                if (lblGaugeID.Text != String.Empty)
                                {
                                    CO_ChannelGauge mdlChannelGauge = lstChannelGauge.Where(g => g.ID == Convert.ToInt64(lblGaugeID.Text)).FirstOrDefault();

                                    if (mdlChannelGauge.DesignDischarge != null)
                                    {
                                        txtAFSQ.Enabled = false;
                                    }
                                    else
                                    {
                                        txtAFSQ.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                ddlGaugeRd.Enabled = false;
                                ddlGaugeRd.CssClass = "form-control";
                            }
                        }
                    }
                    else
                    {
                        Label lblGaugeRd = (Label)e.Row.FindControl("lblGaugeRd");

                        if (lblGaugeRd.Text != String.Empty)
                        {
                            double GaugeRd = Convert.ToDouble(lblGaugeRd.Text);
                            lblGaugeRd.Text = Calculations.GetRDText(GaugeRd);
                        }

                        Label lblSiteName = (Label)e.Row.FindControl("lblSiteName");

                        if (lblSiteName.Text == Constants.UpStream || lblSiteName.Text == Constants.DownStream)
                        {
                            Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                            btnDelete.Enabled = false;
                            btnDelete.OnClientClick = null;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlChannel = (DropDownList)sender;

                GridViewRow gvRow = (GridViewRow)ddlChannel.NamingContainer;

                if (gvRow != null)
                {
                    DropDownList ddlGaugeRd = (DropDownList)gvRow.FindControl("ddlGaugeRd");
                    TextBox txtAFSQ = (TextBox)gvRow.FindControl("txtAFSQ");

                    if (ddlChannel.SelectedItem.Value != String.Empty)
                    {
                        long ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);

                        List<CO_ChannelGauge> lstChannelGauge = new StructureBLL().GetGaugesByChannelID(ChannelID, true);

                        ddlGaugeRd.DataSource = lstChannelGauge.Select(g => new { ID = g.ID, Rds = Calculations.GetRDText(g.GaugeAtRD) });

                        ddlGaugeRd.DataTextField = "Rds";
                        ddlGaugeRd.DataValueField = "ID";

                        ddlGaugeRd.DataBind();

                        ddlGaugeRd.Items.Insert(0, new ListItem("Select", ""));

                        ddlGaugeRd.Enabled = true;
                        ddlGaugeRd.CssClass = "form-control required";
                        txtAFSQ.Enabled = true;
                    }
                    else
                    {
                        ddlGaugeRd.SelectedItem.Text = "Select";
                        ddlGaugeRd.Enabled = false;
                        ddlGaugeRd.CssClass = "form-control";
                        txtAFSQ.Enabled = true;
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlGaugeRd_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlGaugeRd = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlGaugeRd.NamingContainer;
                TextBox txtAFSQ = (TextBox)gvRow.FindControl("txtAFSQ");

                if (ddlGaugeRd.SelectedItem.Value != String.Empty)
                {
                    long GaugeID = Convert.ToInt64(ddlGaugeRd.SelectedItem.Value);

                    CO_ChannelGauge mdlChannelGauge = new ChannelBLL().GetChannelGaugeByID(GaugeID);

                    if (mdlChannelGauge.DesignDischarge != null)
                    {
                        txtAFSQ.Text = Convert.ToString(mdlChannelGauge.DesignDischarge);
                        txtAFSQ.Enabled = false;
                    }
                    else
                    {
                        txtAFSQ.Enabled = true;
                    }
                }
                else
                {
                    txtAFSQ.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructureData_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvStructureData.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}