using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class SearchChannel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.Form.DefaultButton = btnSearch.UniqueID;

                    BindDropdownlists();
                    //BindSearchChannelGrid();
                    //image.Visible = false;
                    Banners.Visible = false;
                }

            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);

            }
        }

        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                // Bind Division dropdownlist 
                Dropdownlist.DDLDivisionNames_ForPublicWebSite(ddlDivision, (int)Constants.DropDownFirstOption.NoOption);
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion "Dropdownlists Events

        private void BindSearchChannelGrid()
        {
            try
            {
                string ChannelName = txtChannelName.Text.Trim();
                long? DivisionID = null;

                if (ddlDivision.SelectedItem.Value != "")
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                DataSet DS = new ChannelBLL().GetChannelInformation_SearchForPublicWebSite(DivisionID, ChannelName);

                if (DS.Tables != null && DS.Tables[0].Rows != null && DS.Tables[0].Rows.Count > 0)
                {
                   // image.Visible = true;
                    Banners.Visible = true;
                    //gvChannel.Columns[4].ItemStyle.Format = "##,0";
                    gvChannel.DataSource = DS.Tables[0];
                    gvChannel.DataBind();
                }
                else
                {
                    gvChannel.DataSource = DS;
                    gvChannel.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void btnChannelSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchChannelGrid();
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long channelID = 0;
                Label lblChannelID = (Label)e.Row.FindControl("ChannelID");
                Label lblChannelName = (Label)e.Row.FindControl("ChannelName");

                if (lblChannelID != null)
                {
                    channelID = Convert.ToInt64(lblChannelID.Text);
                }

                if (lblChannelName != null)
                {
                    lblChannelName.Text = "<a href='ChannelDetail.aspx?Channel=" + channelID.ToString() + "'>" + lblChannelName.Text + "</a>";
                }

                string ColorName = gvChannel.DataKeys[e.Row.RowIndex].Values[1].ToString();
                e.Row.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml(ColorName);

                //e.Row.Cells[4].Text = "Channel is running less than designed discharge at head and tails are as per Authorized Discharge ";

                //e.Row.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml("#708090");
            }
        }

        protected void gvChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannel.PageIndex = e.NewPageIndex;
                BindSearchChannelGrid();
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }

}