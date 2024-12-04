using PMIU.WRMIS.BLL.ClosureOperations;
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
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP
{
    public partial class ACCPOrders : BasePage
    {
        Dictionary<string, object> dd_ACCP = new Dictionary<string, object>();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        long _ACCPID;
        List<string> FileName;
        protected void Page_Load(object sender, EventArgs e)
        {
            _ACCPID = Utility.GetNumericValueFromQueryString("ACCPID", 0);
            if (!IsPostBack)
            {
                SetPageTitle();


                if (_ACCPID > 0)
                {
                    ACCPID.ACCPID = _ACCPID;
                    hdnACCPID.Value = Convert.ToString(_ACCPID);
                    BindgvACCPOrders(_ACCPID);
                }
                hlBack.NavigateUrl = string.Format("~/Modules/ClosureOperations/ACCP/AnnualCanalClosureProgram.aspx?CFCH=1");

            }
        }




        #region Funcions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AnnualCanalClosureWorkPlanOrders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindgvACCPOrders(long ACCPID = 1)
        {
            try
            {
                List<object> lstACCPOrders = new ClosureOperationsBLL().GetACCPOrdersByACCPID(Utility.GetNumericValueFromQueryString("ACCPID", 0));
                Session["ACCPOrders"] = lstACCPOrders;
                gvACCPOrder.DataSource = lstACCPOrders;
                gvACCPOrder.DataBind();
                gvACCPOrder.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion


        #region Events
        protected void gvACCPOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvACCPOrder.PageIndex = e.NewPageIndex;
                gvACCPOrder.EditIndex = -1;
                BindgvACCPOrders();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvACCPOrder.EditIndex = -1;
                BindgvACCPOrders(Utility.GetNumericValueFromQueryString("ACCPID", 0));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {



                if (e.CommandName == "AddACCPOrder")
                {
                    List<object> lstACCPOrder = Session["ACCPOrders"] as List<object>;
                    lstACCPOrder.Add(
                    new
                    {
                        ID = 0,
                        LatterNo = string.Empty,
                        LatterDate = DateTime.Now,
                        FileName = string.Empty,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvACCPOrder.PageIndex = gvACCPOrder.PageCount;
                    gvACCPOrder.DataSource = lstACCPOrder;
                    gvACCPOrder.DataBind();
              
                    gvACCPOrder.EditIndex = gvACCPOrder.Rows.Count - 1;
                    gvACCPOrder.DataBind();

                    ((HyperLink)gvACCPOrder.Rows[gvACCPOrder.EditIndex].FindControl("hlFileName")).Visible =false;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvACCPOrder.DataKeys[e.RowIndex].Values[0]);
                dd_ACCP.Clear();
                dd_ACCP.Add("ID", ID);
                object obj = bllCO.ACCPOrders_Operations(Constants.CRUD_READ, dd_ACCP);
                string fileName = Convert.ToString(obj.GetType().GetProperty("FileName").GetValue(obj));
                string resurl = Utility.GetImagePath(Configuration.ClosureOperations) + "/" + fileName;
                if (System.IO.File.Exists(resurl))
                {
                    System.IO.File.Delete(resurl);
                }
                bool isDeleted = (bool)bllCO.ACCPOrders_Operations(Constants.CRUD_DELETE, dd_ACCP);
                if (isDeleted)
                {
                    BindgvACCPOrders();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                gvACCPOrder.EditIndex = e.NewEditIndex;
                BindgvACCPOrders();
                TextBox txtLatterNo = (TextBox)gvACCPOrder.Rows[e.NewEditIndex].FindControl("txtLatterNo");
                TextBox txtDate = (TextBox)gvACCPOrder.Rows[e.NewEditIndex].FindControl("txtDate");
                Label labID = (Label)gvACCPOrder.Rows[e.NewEditIndex].FindControl("lblID");
                HyperLink hlFileName = (HyperLink)gvACCPOrder.Rows[e.NewEditIndex].FindControl("hlFileName");
                FileUploadControl FileUpload2 = (FileUploadControl)gvACCPOrder.Rows[e.NewEditIndex].FindControl("FileUpload2");
                FileUploadControl FileUpload3 = (FileUploadControl)gvACCPOrder.Rows[e.NewEditIndex].FindControl("FileUpload3");

                hlFileName.Text = "View Attachment";
                FileUpload2.Visible = false;
                FileUpload3.Visible = true;
                FileUpload2.Mode = Convert.ToInt32(Constants.ModeValue.RemoveValidation);
                dd_ACCP.Clear();
                dd_ACCP.Add("ID", labID.Text);
                object obj = bllCO.ACCPOrders_Operations(Constants.CRUD_READ, dd_ACCP);
                txtLatterNo.Text = Convert.ToString(obj.GetType().GetProperty("LatterNo").GetValue(obj));
                DateTime ltrDt = Convert.ToDateTime(obj.GetType().GetProperty("LatterDate").GetValue(obj));

                txtDate.Text = Utility.GetFormattedDate(ltrDt);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvACCPOrder_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"
                DataKey key = gvACCPOrder.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvACCPOrder.Rows[e.RowIndex];
                FileUploadControl FileUploadControlAdd = (FileUploadControl)row.FindControl("FileUpload2");
                FileUploadControl FileUploadControlEdit = (FileUploadControl)row.FindControl("FileUpload3");
                TextBox txtLatterNo = (TextBox)row.FindControl("txtLatterNo");
                TextBox txtDate = (TextBox)row.FindControl("txtDate");
                #endregion
                string fileName = "";
                string letterNo = txtLatterNo.Text.Trim();
                string latterdate = txtDate.Text.Trim();
                long ACCPID = Convert.ToInt64(hdnACCPID.Value);
                bool isSaved = false;

                dd_ACCP.Clear();
                dd_ACCP.Add("", letterNo);
                dd_ACCP.Add("LatterNo", letterNo);
                dd_ACCP.Add("LatterDate", latterdate);

                dd_ACCP.Add("ACCPID", ACCPID);
                dd_ACCP.Add("ID", ID);
                dd_ACCP.Add("UserID", mdlUser.ID);
                if (ID == "0")
                {
                    List<Tuple<string, string, string>> lstNameofFiles = FileUploadControlAdd.UploadNow(Configuration.ClosureOperations);
                    List<object> lstOfBindObjects = Session["ACCPOrders"] as List<object>;
                    object obj = (from o in lstOfBindObjects where o.GetType().GetProperty("LatterNo").GetValue(o).ToString().Trim().ToLower() == letterNo.Trim().ToLower() && o.GetType().GetProperty("LatterDate").GetValue(0).ToString() == latterdate.ToString() select o).FirstOrDefault();
                    if (obj == null)
                    {
                        fileName = lstNameofFiles.Count == 0 ? "" : lstNameofFiles[0].Item3;
                        dd_ACCP.Add("FileName", fileName);
                        isSaved = (bool)bllCO.ACCPOrders_Operations(Constants.CRUD_CREATE, dd_ACCP);
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueLetterNoandLetterDate.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else
                {
                    List<Tuple<string, string, string>> lstNameofFiles = FileUploadControlEdit.UploadNow(Configuration.ClosureOperations);
                    fileName = lstNameofFiles.Count == 0 ? "" : lstNameofFiles[0].Item3;
                    dd_ACCP.Add("FileName", fileName);
                    isSaved = (bool)bllCO.ACCPOrders_Operations(Constants.CRUD_UPDATE, dd_ACCP);
                }
                if (Convert.ToInt64(ID) == 0)
                    gvACCPOrder.PageIndex = 0;
                gvACCPOrder.EditIndex = -1;
                BindgvACCPOrders(ACCPID);
                Master.ShowMessage(Message.RecordSaved.Description);


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}