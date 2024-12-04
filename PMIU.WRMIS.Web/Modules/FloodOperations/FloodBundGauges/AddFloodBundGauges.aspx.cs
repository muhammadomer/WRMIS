using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodBundGauges
{
    public partial class AddFloodBundGauges : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                BindDropDownList();
                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodBundGauges/SearchFloodBundGauges.aspx");
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropDownList()
        {
            BindStructureTypeFloodBund();
            ddlTime.Items.Insert(0, new ListItem("Select", ""));
            // BindTimeFloodBund();
        }

        private void BindStructureTypeFloodBund()
        {
            Dropdownlist.DDLStructureTypesForFloodBund(ddlStructureType, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindTimeFloodBund()
        {
            try
            {
                //Dropdownlist.DDLTime(ddlTime, (int)Constants.DropDownFirstOption.All);

                List<string> TimeList = new FloodOperationsBLL().GetTimeListFloodBund();
                ddlTime.DataSource = TimeList;
                ddlTime.DataBind();
                //ddlTime.Items.Insert(0, "All");
                ddlTime.Items.Insert(0, new ListItem("Select", ""));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region gvHilltorrentNallah Method

        protected void gvHilltorrentNallah_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        #endregion gvHilltorrentNallah Method

        #region gvBund Method

        protected void gvBund_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        #endregion gvBund Method

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlStructureName.SelectedItem.Value != "")
            {
                long StructureTypeSelectedValue = Convert.ToInt64(ddlStructureType.SelectedItem.Value);
                if (StructureTypeSelectedValue == 1)
                {
                    var lstBund = new List<Tuple<string, string, string, string, string, string>>();
                    for (int m = 0; m < gvBund.Rows.Count; m++)
                    {
                        string ID = Convert.ToString(gvBund.DataKeys[m].Values["ID"]);
                        string FGRID = Convert.ToString(gvBund.DataKeys[m].Values["FGRID"]);
                        string CreatedDate = Convert.ToString(gvBund.DataKeys[m].Values["CreatedDate"]);
                        TextBox txtGaugeReadingBund = (TextBox)gvBund.Rows[m].FindControl("txtGaugeReadingBund");
                        string datetime = Convert.ToString(txtDate.Text) + " " + Convert.ToString(ddlTime.SelectedItem.Text);
                        if (txtGaugeReadingBund.Text != "")
                        {
                            lstBund.Add(new Tuple<string, string, string, string, string, string>(Convert.ToString(ddlStructureType.SelectedItem.Text), ID, datetime, txtGaugeReadingBund.Text, CreatedDate, FGRID));
                        }
                    }
                    bool isSaved = (bool)new FloodOperationsBLL().SaveFloodGaugeReadingBund(SessionManagerFacade.UserInformation, Convert.ToInt32(Session[SessionValues.UserID]), lstBund);
                    if (isSaved)
                    {
                        SearchFloodBundGauges.IsSaved = true;
                        Response.Redirect("SearchFloodBundGauges.aspx", false);
                    }
                }
                else if (StructureTypeSelectedValue == 2 || StructureTypeSelectedValue == 3)
                {
                    var listHillTorrent = new List<Tuple<string, string, string, string, string, string, string>>();
                    for (int m = 0; m < gvHilltorrentNallah.Rows.Count; m++)
                    {
                        string ID = Convert.ToString(gvHilltorrentNallah.DataKeys[m].Values["ID"]);
                        string FGRID = Convert.ToString(gvHilltorrentNallah.DataKeys[m].Values["FGRID"]);
                        string CreatedDate = Convert.ToString(gvHilltorrentNallah.DataKeys[m].Values["CreatedDate"]);
                        TextBox txtGuageReading = (TextBox)gvHilltorrentNallah.Rows[m].FindControl("txtGuageReading");
                        TextBox txtDischarge = (TextBox)gvHilltorrentNallah.Rows[m].FindControl("txtDischarge");
                        string datetime = Convert.ToString(txtDate.Text) + " " + Convert.ToString(ddlTime.SelectedItem.Text);
                        if (txtGuageReading.Text != "" && txtDischarge.Text != "")
                        {
                            listHillTorrent.Add(new Tuple<string, string, string, string, string, string, string>(Convert.ToString(ddlStructureType.SelectedItem.Text), ID, datetime, txtGuageReading.Text, txtDischarge.Text, CreatedDate, FGRID));
                        }
                    }
                    bool isSaved = (bool)new FloodOperationsBLL().SaveFloodGaugeReading(SessionManagerFacade.UserInformation, Convert.ToInt32(Session[SessionValues.UserID]), listHillTorrent);
                    if (isSaved)
                    {
                        SearchFloodBundGauges.IsSaved = true;
                        Response.Redirect("SearchFloodBundGauges.aspx", false);
                    }
                }
            }
        }

        protected void ddlStructureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTimeFloodBund();
            ddlTime.Attributes.Add("required", "required");
            ddlTime.CssClass = "form-control required";
        }

        protected void ddlStructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                if (ddlStructureType.SelectedItem.Value != "")
                {
                    long StructureTypeSelectedValue = Convert.ToInt64(ddlStructureType.SelectedItem.Value);
                    string StructureTypeSelectedtext = Convert.ToString(ddlStructureType.SelectedItem.Text);
                    if (StructureTypeSelectedValue == 1)
                    {
                        Dropdownlist.DDLStructureNameFloodBund(ddlStructureName, _Users.ID, StructureTypeSelectedtext, (int)Constants.DropDownFirstOption.Select);
                        gvHilltorrentNallah.Visible = false;
                    }
                    else if (StructureTypeSelectedValue == 2 || StructureTypeSelectedValue == 3)
                    {
                        Dropdownlist.DDLStructureNameFloodBund(ddlStructureName, _Users.ID, StructureTypeSelectedtext, (int)Constants.DropDownFirstOption.Select);
                        gvBund.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTime.SelectedItem.Value != "")
            {
                long StructureTypeSelectedValue = Convert.ToInt64(ddlStructureType.SelectedItem.Value);
                string StructureNameSelectedValue = Convert.ToString(ddlStructureName.SelectedItem.Text);
                if (StructureTypeSelectedValue == 1 && StructureNameSelectedValue != "")
                {
                    IEnumerable<DataRow> iegvBund = new FloodOperationsBLL().GetFloodBundAddGauges(Convert.ToString(ddlStructureType.SelectedItem.Text), StructureNameSelectedValue, Convert.ToString(ddlTime.SelectedItem.Text), Utility.GetParsedDate(txtDate.Text.Trim()));
                    var lstgvBund = iegvBund.Select(dataRow => new
                    {
                        ID = dataRow.Field<long>("ID"),
                        FGRID = dataRow.Field<long>("FGRID"),
                        GaugeTypeName = dataRow.Field<string>("GaugeTypeName"),
                        GaugeRD = Calculations.GetRDText(Convert.ToInt32(dataRow.Field<int?>("GaugeRD"))),
                        GuageReadingBund = dataRow.Field<double?>("GuageReadingBund"),
                        CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                    }).ToList();
                    gvBund.DataSource = lstgvBund;
                    gvBund.DataBind();
                    gvBund.Visible = true;
                    gvHilltorrentNallah.Visible = false;
                }
                else if ((StructureTypeSelectedValue == 2 || StructureTypeSelectedValue == 3) && StructureNameSelectedValue != "")
                {
                    IEnumerable<DataRow> iegvBund = new FloodOperationsBLL().GetFloodBundAddGauges(Convert.ToString(ddlStructureType.SelectedItem.Text), StructureNameSelectedValue, Convert.ToString(ddlTime.SelectedItem.Text), Utility.GetParsedDate(txtDate.Text.Trim()));
                    var lstgvBund = iegvBund.Select(dataRow => new
                    {
                        ID = dataRow.Field<long>("ID"),
                        FGRID = dataRow.Field<long>("FGRID"),
                        StructureName = dataRow.Field<string>("StructureName"),
                        GuageReading = dataRow.Field<double?>("GuageReading"),
                        DischargeValue = dataRow.Field<double?>("DischargeValue"),
                        CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                    }).ToList();
                    gvHilltorrentNallah.DataSource = lstgvBund;
                    gvHilltorrentNallah.DataBind();
                    gvBund.Visible = false;
                    gvHilltorrentNallah.Visible = true;
                }
            }
        }
    }
}