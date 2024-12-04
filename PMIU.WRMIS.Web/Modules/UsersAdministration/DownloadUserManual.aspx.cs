using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using System.Data;
using System.Web.UI.HtmlControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class DownloadUserManual : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    List<object> lst = new UserBLL().GenerateMenu_Dashboard(SessionManagerFacade.UserInformation.RoleID.Value);
                    gvGrid.DataSource = lst;
                    gvGrid.DataBind();
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.UserManuals);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btn_Click(object sender, EventArgs e)
        {

            try
            {

                LinkButton lbtn = (LinkButton)sender;
                if (lbtn != null)
                {
                    GridViewRow row = (GridViewRow)lbtn.NamingContainer;
                    if (row != null)
                    {
                        Label lblNam = (Label)row.FindControl("lblName");

                        //string Path = Utility.ReadConfiguration("ManualsPath");
                        //bool Exist = System.IO.File.Exists(@Path + lblNam.Text + ".pdf");
                        //bool Exist = System.IO.File.Exists(@"E:\UserManuals\" + lblNam.Text + ".pdf");

                        var relativePath = "~/UserManuals/" + lblNam.Text + ".pdf";
                        var absolutePath = HttpContext.Current.Server.MapPath(relativePath);
                        bool Exist = System.IO.File.Exists(absolutePath);

                        if (Exist)
                        {
                            //byte[] bytesPDF = System.IO.File.ReadAllBytes(@"E:\UserManuals\" + lblNam.Text + ".pdf");
                            byte[] bytesPDF = System.IO.File.ReadAllBytes(absolutePath);

                            if (bytesPDF != null)
                            {
                                string Name = lblNam.Text.Replace(" ", "").Replace(".", "");
                                Response.AddHeader("content-disposition", "attachment;filename=" + Name + ".pdf");
                                Response.ContentType = "application/octectstream";
                                Response.BinaryWrite(bytesPDF);
                                Response.End();
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.FilePathProblem.Description, SiteMaster.MessageType.Error);
                        }
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            // Response.Redirect("~/UserManuals/" + "User Administration" + ".pdf");
        }

    }
}