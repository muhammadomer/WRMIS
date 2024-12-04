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
    public partial class InspectionAssetsLot : BasePage
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
                        AM_AssetItems ObjAssetItems = new AssetsWorkBLL().GetAssetID(AssetsID);
                        if (ObjAssetItems != null)
                        {
                            hdnLotQuantity.Value = Convert.ToString(ObjAssetItems.LotQuantity);
                            hdnAvailableQuantity.Value = Convert.ToString(ObjAssetItems.AssetAvailableQuantity);
                        }

                        int InspectionLotID = Utility.GetNumericValueFromQueryString("InspectionLotID", 0);
                        if (InspectionLotID > 0)
                        {
                            LoadInspectionLot(InspectionLotID);
                            BindsGridConditions();
                            //hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/InspectionAssetsHistory.aspx?AssetsID=" + hdnAssetsID.Value + "&InspectionLotID=" + InspectionLotID + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", ""));
                            hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/InspectionAssetsHistory.aspx?AssetsID=" + hdnAssetsID.Value + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", "") + "&RestoreState=" + 1);
                        }
                        else
                        {
                            BindsGrid();
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
                //Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.NoOption);
                // Dropdownlist.DDLAssetCondition(ddlcondition, false, (int)Constants.DropDownFirstOption.Select);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion Dropdown Lists Binding


        #region EditPopulate
        private void LoadInspectionLot(long _InspectionLotID)
        {
            divAttchmentView.Visible = true;
            object objDetail = BLLAsset.GeAssetInspectionLot(_InspectionLotID);
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
                    else if (prop.ToString().Contains("Quantity"))
                        txtNumber.Text = propValue + "";
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
        private void ViewInspection()
        {
            txtDate.Enabled = false;
            txtNumber.Enabled = false;
            ddlStatus.Enabled = false;
            txtCurrentAssetValue.Enabled = false;
            txtRemarks.Enabled = false;
            FileUploadControl1.Visible = false;
            btnSave.Visible = false;
            divAttchmentAdd.Visible = false;
            divAttchmentView.Visible = true;
            DivViewCondition.Visible = true;
            DivNewCondition.Visible = false;
            txtDate.CssClass = txtDate.CssClass.Replace("required", "");
            txtNumber.CssClass = txtNumber.CssClass.Replace("required", "");
        }
        private void BindsGridConditions()
        {
            try
            {
                List<object> lstCondition = BLLAsset.GetAssetshistoryLotCondition(Utility.GetNumericValueFromQueryString("InspectionLotID", 0));
                gvViewCondition.DataSource = lstCondition;
                gvViewCondition.DataBind();
                gvViewCondition.Enabled = false;
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion EditPopulate
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
                //if (Convert.ToInt32(Convert.ToInt32(txtNumber.Text)+Convert.ToInt32(hdnAvailableQuantity.Value)) > Convert.ToInt32(hdnLotQuantity.Value) && (ddlStatus.SelectedItem.Value == "1"))
                //{
                //    Master.ShowMessage(Message.AssetInspectionLotQuantityActive.Description, SiteMaster.MessageType.Error);
                //    return;
                //}
                //if (Convert.ToInt32(txtNumber.Text) > Convert.ToInt32(hdnLotQuantity.Value) && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value)==0)
                //{
                //    Master.ShowMessage(Message.AssetInspectionLotQuantityInActive.Description, SiteMaster.MessageType.Error);
                //    return;
                //}
                //if (Convert.ToInt32(txtNumber.Text) > Convert.ToInt32(hdnAvailableQuantity.Value) && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) != 0)
                //{
                //    Master.ShowMessage(Message.AssetInspectionLotQuantityInActive.Description, SiteMaster.MessageType.Error);
                //    return;
                //}
                if (Convert.ToInt32(txtNumber.Text)==0 && (ddlStatus.SelectedItem.Value == "1"))
                {
                    Master.ShowMessage(Message.AssetInspectionLotQuantityActiveZero.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (Convert.ToInt32(txtNumber.Text) > Convert.ToInt32(hdnLotQuantity.Value) && (ddlStatus.SelectedItem.Value == "1"))
                {
                    Master.ShowMessage(Message.AssetInspectionLotQuantityActive.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (Convert.ToInt32(Convert.ToInt32(hdnAvailableQuantity.Value)-Convert.ToInt32(Convert.ToInt32(txtNumber.Text)))<0 && (ddlStatus.SelectedItem.Value == "2"))
                {
                    Master.ShowMessage(Message.AssetInspectionLotQuantityInActive.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (txtNumber.Text != "")
                {
                    if (Convert.ToInt64(txtNumber.Text) == 0)
                    {
                        Master.ShowMessage("Number should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
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


                /////////////////////////////Asset Condition//////////////////////////////////////
                var listCondition = new List<Tuple<string, string, string>>();
                List<string> LotQty = new List<string>();
                int countMessage = 0;
                for (int m = 0; m < gvAssetCondition.Rows.Count; m++)
                {
                    TextBox txtNumberGrid = (TextBox)gvAssetCondition.Rows[m].FindControl("txtNumberGrid");
                    DropDownList ddlCondition = (DropDownList)gvAssetCondition.Rows[m].FindControl("ddlCondition");
                    TextBox txtRemarksGrid = (TextBox)gvAssetCondition.Rows[m].FindControl("txtRemarks");
                    // string AssetConditionID = gvAssetCondition.DataKeys[m].Values[0].ToString();

                    if (txtNumberGrid.Text != "" && ddlCondition.SelectedValue != "")
                    {

                        listCondition.Add(new Tuple<string, string, string>(txtNumberGrid.Text, ddlCondition.SelectedValue, txtRemarksGrid.Text));
                        LotQty.Add(txtNumberGrid.Text);
                        int TotalQty = LotQty.Sum(x => Convert.ToInt32(x));

                        //if (Convert.ToInt32(Convert.ToInt32(TotalQty) + Convert.ToInt32(hdnAvailableQuantity.Value)) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage == 0 && (ddlStatus.SelectedItem.Value == "1"))
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionIndividualConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}
                        //if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage == 0 && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) == 0)
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionIndividualConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}
                        //if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnAvailableQuantity.Value) && countMessage == 0 && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) != 0)
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionIndividualConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}


                        if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage == 0 && (ddlStatus.SelectedItem.Value == "1"))
                        {
                            Master.ShowMessage(Message.AssetInspectionIndividualConditionQuantity.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if (Convert.ToInt32(hdnAvailableQuantity.Value) - Convert.ToInt32(TotalQty) < 0 && countMessage == 0 && (ddlStatus.SelectedItem.Value == "2"))
                        {
                            Master.ShowMessage(Message.AssetInspectionIndividualConditionQuantity.Description, SiteMaster.MessageType.Error);
                            return;
                        }


                        //sum message
                        //if (Convert.ToInt32(Convert.ToInt32(TotalQty) + Convert.ToInt32(hdnAvailableQuantity.Value)) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage != 0 && (ddlStatus.SelectedItem.Value == "1"))
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionSumConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}
                        //if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage != 0 && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) == 0)
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionSumConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}
                        //if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnAvailableQuantity.Value) && countMessage != 0 && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) != 0)
                        //{
                        //    Master.ShowMessage(Message.AssetInspectionSumConditionQuantity.Description, SiteMaster.MessageType.Error);
                        //    return;
                        //}

                        if (Convert.ToInt32(TotalQty) > Convert.ToInt32(hdnLotQuantity.Value) && countMessage != 0 && (ddlStatus.SelectedItem.Value == "1"))
                        {
                            Master.ShowMessage(Message.AssetInspectionSumConditionQuantity.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if ( Convert.ToInt32(hdnAvailableQuantity.Value)-Convert.ToInt32(TotalQty)<0 && countMessage != 0 && (ddlStatus.SelectedItem.Value == "2") && Convert.ToInt32(hdnAvailableQuantity.Value) != 0)
                        {
                            Master.ShowMessage(Message.AssetInspectionSumConditionQuantity.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if (txtNumberGrid.Text != "")
                        {
                            if (txtNumberGrid.Text == "0")
                            {
                                Master.ShowMessage("Condition of asset number should be greater than zero.", SiteMaster.MessageType.Error);
                                return;
                            }
                        }

                    }
                    countMessage++;
                }
                //////////////////////////////////////////////////////////////////

                AM_AssetInspectionLot mdl = new AM_AssetInspectionLot();
                mdl.AssetItemID = Convert.ToInt64(hdnAssetsID.Value);
                mdl.InspectionDate = date;
                if (ddlStatus.SelectedItem.Value == "1")
                {
                    mdl.Quantity = Convert.ToInt32(txtNumber.Text);
                }
                else
                {
                    mdl.Quantity =Convert.ToInt32(txtNumber.Text);
                }
                //Kindly change status as it is a string now.
                //mdl.Status = (ddlStatus.SelectedItem.Value == "1") ? true : false;
                mdl.Status = ddlStatus.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtCurrentAssetValue.Text))
                    mdl.CurrentAssetValue = Convert.ToInt64(txtCurrentAssetValue.Text);

                mdl.Remarks = txtRemarks.Text.Trim();
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.Source = "W";
                mdl.CreatedByDesigID = (long)SessionManagerFacade.UserInformation.DesignationID;

                bool isRecordSaved = (bool)BLLAsset.SaveAssetInspectionLot(mdl, lstNameofFiles, listCondition);
                //////update Asset active/Inactive/////

                BLLAsset.AssetStatusLotUpdation(Convert.ToInt64(hdnAssetsID.Value), Convert.ToBoolean((ddlStatus.SelectedItem.Value == "1") ? 1 : 0),Convert.ToInt32(hdnAvailableQuantity.Value), Convert.ToInt32(txtNumber.Text));

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
        private void BindsGrid()
        {

            List<object> lst = new List<object>();

            gvAssetCondition.DataSource = lst;
            gvAssetCondition.DataBind();
        }
        protected void gvAssetCondition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddCondition")
                {
                    if (gvAssetCondition.Rows.Count == 0)
                    {
                        SetfirstRow();
                    }
                    else
                    {
                        AddNewRowToGrid();
                    }


                }
                else if (e.CommandName == "Delete")
                {
                    //   LinkButton lb = (LinkButton)sender;
                    //   GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
                    GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int rowID = gvRow.RowIndex;
                    if (ViewState["CurrentTable"] != null)
                    {

                        DataTable dt = (DataTable)ViewState["CurrentTable"];
                        if (dt.Rows.Count >= 1)
                        {
                            if (gvRow.RowIndex <= dt.Rows.Count - 1)
                            {
                                dt.Rows.Remove(dt.Rows[rowID]);

                            }
                        }
                        ViewState["CurrentTable"] = dt;
                        gvAssetCondition.DataSource = dt;
                        gvAssetCondition.DataBind();
                    }
                    SetPreviousRow();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region ConditionRowAdd
        private void SetfirstRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));

            dr = dt.NewRow();
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            gvAssetCondition.DataSource = dt;
            gvAssetCondition.DataBind();

            TextBox txtNumberGrid = (TextBox)gvAssetCondition.Rows[0].Cells[0].FindControl("txtNumberGrid");
            DropDownList ddlCondition = (DropDownList)gvAssetCondition.Rows[0].Cells[1].FindControl("ddlCondition");
            TextBox txtRemarks = (TextBox)gvAssetCondition.Rows[0].Cells[2].FindControl("txtRemarks");
            Dropdownlist.DDLAssetCondition(ddlCondition, false, (int)Constants.DropDownFirstOption.Select);

        }
        private void AddNewRowToGrid()
        {

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                    {
                        TextBox txtNumberGrid = (TextBox)gvAssetCondition.Rows[i].Cells[0].FindControl("txtNumberGrid");
                        DropDownList ddlCondition = (DropDownList)gvAssetCondition.Rows[i].Cells[1].FindControl("ddlCondition");
                        TextBox txtRemarks = (TextBox)gvAssetCondition.Rows[i].Cells[2].FindControl("txtRemarks");

                        dtCurrentTable.Rows[i]["Column1"] = txtNumberGrid.Text;
                        dtCurrentTable.Rows[i]["Column2"] = ddlCondition.SelectedItem.Text;
                        dtCurrentTable.Rows[i]["Column3"] = txtRemarks.Text;

                    }

                    gvAssetCondition.DataSource = dtCurrentTable;
                    gvAssetCondition.DataBind();
                }
            }

            SetPreviousRow();
        }
        private void SetPreviousRow()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtNumberGrid = (TextBox)gvAssetCondition.Rows[rowIndex].Cells[0].FindControl("txtNumberGrid");
                        DropDownList ddlCondition = (DropDownList)gvAssetCondition.Rows[rowIndex].Cells[1].FindControl("ddlCondition");
                        TextBox txtRemarks = (TextBox)gvAssetCondition.Rows[rowIndex].Cells[2].FindControl("txtRemarks");

                        Dropdownlist.DDLAssetCondition(ddlCondition, false, (int)Constants.DropDownFirstOption.Select);

                        if (i <= dt.Rows.Count - 1)
                        {
                            txtNumberGrid.Text = dt.Rows[i]["Column1"].ToString();
                            ddlCondition.ClearSelection();
                            ddlCondition.Items.FindByText(dt.Rows[i]["Column2"].ToString()).Selected = true;
                            txtRemarks.Text = dt.Rows[i]["Column3"].ToString();

                        }

                        rowIndex++;
                    }
                }
            }
        }
        #endregion ConditionRowAdd

        protected void gvAssetCondition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}