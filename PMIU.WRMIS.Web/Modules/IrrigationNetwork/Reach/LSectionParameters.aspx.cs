using PMIU.WRMIS.BLL.IrrigationNetwork.Reach;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach
{
    public partial class LSectionParameters : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string ReachID = Request.QueryString["ReachID"];
                    string channelID = Request.QueryString["ChannelID"];
                    string ReachRD = Request.QueryString["ReachRD"];
                    string ReachNo = Request.QueryString["ReachNo"];

                    txtParameterChangeDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

                    if (!string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(ReachID) && !string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(ReachID))
                    {
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ChannelID = Convert.ToInt64(channelID);
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ReachRD = Convert.ToInt64(ReachRD);
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ReachNo = Convert.ToInt64(ReachNo);
                        Dropdownlist.DDLLiningType(ddlTypeofLining);
                        Dropdownlist.BindDropdownlist<List<object>>(ddlLinedOrUnlined, CommonLists.GetLinedOrUnlined());
                        BindLSectionParameters();

                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Reach/DefineChannelReach.aspx?ChannelID=" + Convert.ToString(channelID);
                    }
                }
                if (ddlLinedOrUnlined.SelectedItem.Text == "Unlined" || ddlLinedOrUnlined.SelectedItem.Text == "Select")
                {
                    ddlTypeofLining.Enabled = false;
                    ddlTypeofLining.SelectedIndex = 0;
                    lblLacey.Text = "Lacey's f or Critical Velocity Ratio";
                }
                else
                {
                    lblLacey.Text = "Manning's Roughness Coefficient (N)";
                    ddlTypeofLining.Enabled = true;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long ReachID = Convert.ToInt64(Request.QueryString["ReachID"]);
                string RDLocaton = Request.QueryString["L"];
                string filePath = string.Empty;
                string fileName = string.Empty;
                string sGuid = Guid.NewGuid().ToString();
                if (fuLSection.HasFile == true)
                {
                    string[] validFileTypes = { ".jpg", ".jpeg" };
                    string fileExtension = System.IO.Path.GetExtension(fuLSection.PostedFile.FileName);

                    string FullFileName = fuLSection.FileName;
                    System.IO.FileInfo oFileInfo = new System.IO.FileInfo(FullFileName);
                    fileName = sGuid + "_" + oFileInfo.Name;

                    if (validFileTypes.Contains(fileExtension))
                    {
                        filePath = Utility.GetImagePath(Configuration.IrrigationNetwork);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(filePath);

                            filePath = filePath + "/" + fileName;

                            fuLSection.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        Master.ShowMessage("Invalid file types.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                CO_ChannelReachLSP mdlLSectionParameter = new CO_ChannelReachLSP();
                ReachBLL bllReach = new ReachBLL();
                DateTime dtMax = Convert.ToDateTime(bllReach.GetMaxDateByReachID(ReachID, RDLocaton));
                if (fuLSection.HasFile)
                {
                    mdlLSectionParameter.LSectionPhoto = fileName;
                }

                else
                {
                    mdlLSectionParameter.LSectionPhoto = hdnLabel.Text;
                }

                mdlLSectionParameter.ReachID = Convert.ToInt64(ReachID);
                mdlLSectionParameter.ParameterDate = Convert.ToDateTime(txtParameterChangeDate.Text);
                mdlLSectionParameter.NaturalSurfaceLevel = Convert.ToDouble(txtNaturalSurfaceLevel.Text);
                mdlLSectionParameter.AFS = Convert.ToDouble(txtAuthorizedFullSupply.Text);
                mdlLSectionParameter.BedLevel = Convert.ToDouble(txtBedLevel.Text);
                mdlLSectionParameter.FullSupplyLevel = Convert.ToDouble(txtFullSupplyLevel.Text);
                mdlLSectionParameter.BedWidth = Convert.ToDouble(txtBedWidth.Text);
                mdlLSectionParameter.FullSupplyDepth = Convert.ToDouble(txtFullSupplyDepth.Text);
                mdlLSectionParameter.SideSlop = Convert.ToDouble(txtSideSlop.Text);
                mdlLSectionParameter.SlopeIn = Convert.ToDouble(txtSlopeIn.Text);

                mdlLSectionParameter.CriticalVelocityRatio = Convert.ToDouble(txtCriticalVelocityRatio.Text);

                mdlLSectionParameter.FreeBoard = Convert.ToDouble(txtFreeBoard.Text);
                mdlLSectionParameter.LeftBankWidth = Convert.ToDouble(txtLeftBankWidth.Text);
                mdlLSectionParameter.RightBankWidth = Convert.ToDouble(txtRightBankWidth.Text);
                mdlLSectionParameter.RDLocation = RDLocaton;
                if (ddlLinedOrUnlined.SelectedIndex != 0)
                {
                    mdlLSectionParameter.LinedUnlined = Convert.ToBoolean(Convert.ToInt16(ddlLinedOrUnlined.SelectedItem.Value));
                }
                if (ddlTypeofLining.SelectedIndex != 0)
                {
                    mdlLSectionParameter.LiningTypeID = Convert.ToInt64(ddlTypeofLining.SelectedItem.Value);
                }



                if (mdlLSectionParameter.ParameterDate <= dtMax)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    bllReach.AddLSectionParameter(mdlLSectionParameter);

                    CO_ChannelReachLSP mdlLSP = new ReachBLL().GetLSectionParameterByReachID(ReachID, RDLocaton);
                    if (mdlLSP.LSectionPhoto != "")
                    {
                        GetReportDiv(mdlLSP.LSectionPhoto);
                    }


                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }



            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindLSectionParameters()
        {
            long ReachID = Convert.ToInt64(Request.QueryString["ReachID"]);
            string RDLocation = Convert.ToString(Request.QueryString["L"]).ToLower();

            CO_ChannelReachLSP mdlLSP = new ReachBLL().GetLSectionParameterByReachID(ReachID, RDLocation);

            if (mdlLSP != null)
            {
                txtNaturalSurfaceLevel.Text = Convert.ToDouble(mdlLSP.NaturalSurfaceLevel).ToString();
                txtAuthorizedFullSupply.Text = Convert.ToDouble(mdlLSP.AFS).ToString();
                txtBedLevel.Text = Convert.ToDouble(mdlLSP.BedLevel).ToString();
                txtFullSupplyLevel.Text = Convert.ToDouble(mdlLSP.FullSupplyLevel).ToString();
                txtBedWidth.Text = Convert.ToDouble(mdlLSP.BedWidth).ToString();
                txtFullSupplyDepth.Text = Convert.ToDouble(mdlLSP.FullSupplyDepth).ToString();
                txtSideSlop.Text = Convert.ToDouble(mdlLSP.SideSlop).ToString();
                txtSlopeIn.Text = Convert.ToDouble(mdlLSP.SlopeIn).ToString();
                txtCriticalVelocityRatio.Text = Convert.ToDouble(mdlLSP.CriticalVelocityRatio).ToString();
                txtFreeBoard.Text = Convert.ToDouble(mdlLSP.FreeBoard).ToString();
                txtLeftBankWidth.Text = Convert.ToDouble(mdlLSP.LeftBankWidth).ToString();
                txtRightBankWidth.Text = Convert.ToDouble(mdlLSP.RightBankWidth).ToString();
                Dropdownlist.SetSelectedValue(ddlTypeofLining, Convert.ToString(Convert.ToInt16(mdlLSP.LiningTypeID)));
                Dropdownlist.SetSelectedValue(ddlLinedOrUnlined, Convert.ToString(Convert.ToInt16(mdlLSP.LinedUnlined)));
                hdnLinedTypeID.Value = Convert.ToString(Convert.ToInt16(mdlLSP.LinedUnlined));
                hdnLabel.Text = mdlLSP.LSectionPhoto;
                if (mdlLSP.LSectionPhoto != "")
                {
                    GetReportDiv(mdlLSP.LSectionPhoto);
                }
                btnSave.Visible = base.CanEdit;
            }
            else
            {
                btnSave.Visible = base.CanAdd;
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.LSectionParameter);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void GetReportDiv(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            //lnkFile.Text = "File: " + filename;
            //lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl.Size = lstName.Count;
            FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.IrrigationNetwork, lstName);

        }
    }
}