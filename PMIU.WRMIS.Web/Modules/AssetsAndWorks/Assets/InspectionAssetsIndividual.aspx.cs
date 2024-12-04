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
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;
using PMIU.WRMIS.BLL.UserAdministration;
using System.Data;
using PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls;
using System.Reflection;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets
{
    public partial class InspectionAssetsIndividual : BasePage
    {
        AssetsWorkBLL BLLAsset = new AssetsWorkBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdown();
                    txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                    int AssetsID = Utility.GetNumericValueFromQueryString("AssetsID", 0);
                    if (AssetsID > 0)
                    {
                        AssetsDetail._AssetsID = AssetsID;
                        hdnAssetsID.Value = Convert.ToString(AssetsID);

                        int InspectionIndID = Utility.GetNumericValueFromQueryString("InspectionIndID", 0);
                        if (InspectionIndID > 0)
                        {
                            LoadInspectionIndividual(InspectionIndID);
                            //hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/InspectionAssetsIndividual.aspx?AssetsID=" + hdnAssetsID.Value + "&InspectionIndID=" + InspectionIndID + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", ""));
                            hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/InspectionAssetsHistory.aspx?AssetsID=" + hdnAssetsID.Value + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", "") + "&RestoreState=" + 1);
                        }
                        else
                        {
                            txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/SearchAssets.aspx?RestoreState=1");
                        }
                        btnSave.Enabled = base.CanAdd;



                    }

                }
                FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.RemoveValidation);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region Set Page Title
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title
        #region Dropdown Lists Binding
        private void BindDropdown()
        {
            try
            {
                if (Utility.GetStringValueFromQueryString("AssetType", "") == "")
                    Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.NoOption);
                else
                    Dropdownlist.DDLAssetInspectionAllStatus(ddlStatus, (int)Constants.DropDownFirstOption.NoOption);
                // Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.NoOption);
                Dropdownlist.DDLAssetCondition(ddlcondition, false, (int)Constants.DropDownFirstOption.Select);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion Dropdown Lists Binding
        #region EditPopulate
        private void LoadInspectionIndividual(long _InspectionIndID)
        {
            List<string> FileName = new List<string>();
            object objDetail = BLLAsset.GeAssetInspectionInd(_InspectionIndID);
            if (objDetail != null)
            {
                string count = "";
                Type propertiesType = objDetail.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(objDetail, null);


                    if (prop.ToString().Contains("InspectionDate"))
                        txtDate.Text = propValue + "";
                    else if (prop.ToString().Contains("ConditionID"))
                        Dropdownlist.SetSelectedValue(ddlcondition, propValue + "");
                    else if (prop.ToString().Contains("Status"))
                        Dropdownlist.SetSelectedText(ddlStatus, propValue + "");
                    else if (prop.ToString().Contains("CurrentAssetValue"))
                        txtCurrentAssetValue.Text = propValue + "";
                    else if (prop.ToString().Contains("Remarks"))
                        txtRemarks.Text = propValue + "";
                    else if (prop.ToString().Contains("CreatedBy"))
                        hdnCreatedBy.Value = propValue + "";
                    else if (prop.ToString().Contains("CreatedDate"))
                        hdnCreatedDate.Value = propValue + "";


                    else if (prop.ToString().Contains("Attchment"))
                    {
                        if (!count.Equals("0"))
                        {
                            string atcmnt = propValue + "";
                            string[] arry = atcmnt.Split(';');
                            switch (arry.Length)
                            {
                                case 2:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;
                                    break;
                                case 3:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    break;
                                case 4:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    break;
                                case 5:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[3]);
                                    //HyperLink4.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    PreviewImage5(arry[3]);
                                    FileUploadControl5.Visible = true;


                                    break;
                                case 6:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[3]);
                                    //HyperLink4.Visible = true;
                                    //HyperLink5.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[4]);
                                    //HyperLink5.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    PreviewImage5(arry[3]);
                                    FileUploadControl5.Visible = true;

                                    PreviewImage6(arry[4]);
                                    FileUploadControl6.Visible = true;

                                    break;
                            }
                        }
                    }
                }
            }
            ViewInspection();


        }
        private void PreviewImage2(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl2.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl2.Size = 1;
            FileUploadControl2.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage3(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl3.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl3.Size = 1;
            FileUploadControl3.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage4(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl4.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl4.Size = 1;
            FileUploadControl4.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage5(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl5.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl5.Size = 1;
            FileUploadControl5.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage6(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl6.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl6.Size = 1;
            FileUploadControl6.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }

        #endregion EditPopulate
        private void ViewInspection()
        {
            txtDate.Enabled = false;
            ddlcondition.Enabled = false;
            ddlStatus.Enabled = false;
            txtCurrentAssetValue.Enabled = false;
            txtRemarks.Enabled = false;
            FileUploadControl1.Visible = false;
            btnSave.Visible = false;
            divAttchmentAdd.Visible = false;
            divAttchmentView.Visible = true;
            txtDate.CssClass = txtDate.CssClass.Replace("required", "");
            ddlcondition.CssClass = ddlcondition.CssClass.Replace("required", "");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = Utility.GetParsedDate(txtDate.Text);
                if (date > DateTime.Now)
                {
                    Master.ShowMessage(Message.AssetInspectionDateCannotBeFutureDate.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (txtCurrentAssetValue.Text != "")
                {
                    if (Convert.ToInt64(txtCurrentAssetValue.Text) == 0)
                    {
                        Master.ShowMessage("Current asset value should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.AssetsWorks);
                //if (lstNameofFiles == null || lstNameofFiles.Count() == 0)
                //{
                //    Master.ShowMessage("Attachment is required.", SiteMaster.MessageType.Error);
                //    return;
                //}

                AM_AssetInspectionInd mdl = new AM_AssetInspectionInd();
                mdl.AssetItemID = Convert.ToInt64(hdnAssetsID.Value);
                mdl.InspectionDate = date;
                if (ddlcondition.SelectedItem.Value != "")
                    mdl.ConditionID = Convert.ToInt64(ddlcondition.SelectedValue);
                //Kindly change status as it is a string now
                //mdl.Status = (ddlStatus.SelectedItem.Value == "1") ? true : false;
                mdl.Status = ddlStatus.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtCurrentAssetValue.Text))
                    mdl.CurrentAssetValue = Convert.ToInt32(txtCurrentAssetValue.Text);

                mdl.Remarks = txtRemarks.Text.Trim();
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.Source = "W";
                mdl.CreatedByDesigID = (long)SessionManagerFacade.UserInformation.DesignationID;

                bool isRecordSaved = (bool)BLLAsset.SaveAssetInspectionInd(mdl, lstNameofFiles);
                //////update Asset active/Inactive/////

                BLLAsset.AssetStatusIndividualUpdation(Convert.ToInt64(hdnAssetsID.Value), Convert.ToBoolean((ddlStatus.SelectedItem.Value == "1") ? 1 : 0));

                ////////////////////////////////
                SearchAssets.IsSaved = true;
                HttpContext.Current.Session.Add("AssetsID", hdnAssetsID.Value);
                Response.Redirect("SearchAssets.aspx?RestoreState=1", false);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }

}