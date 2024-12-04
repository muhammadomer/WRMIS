using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class ChannelLineDiagram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                BindsDropDowns();
            }
        }

        public void BindsDropDowns()
        {
            //Dropdownlist.DDLDivisionNames(ddlDivision);
            Dropdownlist.DDLDivisionsByDomainID(ddlDivision, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.Select, true);
        }
        public void BindGrid()
        {
            try
            {

                string Div = ddlDivision.SelectedItem.Text.ToString();
                if (Div != "Select")
                {
                    string Channel = ddlChannel.SelectedItem.Text.ToString();
                    if (txtDateTime.Value != "")
                    {
                        DateTime dt = Convert.ToDateTime(txtDateTime.Value);
                        //List<PW_LineDiagram_Result> lstResult = new ChannelBLL().GetChannelLineDiagram(Div, Channel, dt);
                        DataSet lstResult = new ChannelBLL().GetChannelLineDiagram(Div, Channel, dt);
                        gvChannelLineDiagram.DataSource = lstResult;
                        gvChannelLineDiagram.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sDivID = ddlDivision.SelectedItem.Text.ToString();
                long DivID = Convert.ToInt32(ddlDivision.SelectedItem.Value);

                List<object> lstChannels = new ChannelBLL().GetMainBranchLinkChannels(DivID);
                ddlChannel.DataSource = lstChannels;
                ddlChannel.DataTextField = "Name";
                ddlChannel.DataValueField = "ID";
                ddlChannel.DataBind();
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void gvChannelLineDiagram_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                Label lblROff = (Label)e.Row.FindControl("lblROfftakes");
                Label lblRRD = (Label)e.Row.FindControl("lblRRD");
                Image imagee = (Image)e.Row.FindControl("imgSource");
                Label lblLOff = (Label)e.Row.FindControl("lblLOfftakes");
                Label lblLRD = (Label)e.Row.FindControl("lblLRD");

                string ColorName = gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[3].ToString();
                e.Row.Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml(ColorName);
                int Side = Convert.ToInt32(gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[0]);

                if (Side == 1) // right 
                {
                    lblROff.Text = gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    lblRRD.Text = gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[2].ToString();
                    imagee.ImageUrl = "~/Design/img/BranchRight.png";
                }
                else if (Side == 2) // Left
                {
                    lblLOff.Text = gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    lblLRD.Text = gvChannelLineDiagram.DataKeys[e.Row.RowIndex].Values[2].ToString();
                    imagee.ImageUrl = "~/Design/img/BranchLeft.png";
                }
            }
        }
    }
}