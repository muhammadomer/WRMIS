using PMIU.WRMIS.BLL.SeasonalPlanning;
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

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class SRMInflowForecasting : BasePage
    {
        #region Variables

        public static int Season = 1;
        public static int? JMKharifMaxProbability;
        public static int? JMLKMaxProbability;
        public static int? JMKharifMinProbability;
        public static int? JMLKMinProbability;
        public static int? JMKharifMLProbability;
        public static int? JMLKMLProbability;

        public static int? CMKharifMaxProbability;
        public static int? CMLKMaxProbability;
        public static int? CMKharifMinProbability;
        public static int? CMLKMinProbability;
        public static int? CMKharifMLProbability;
        public static int? CMLKMLProbability;

        public static int? ITKharifMaxProbability;
        public static int? ITLKMaxProbability;
        public static int? ITKharifMinProbability;
        public static int? ITLKMinProbability;
        public static int? ITKharifMLProbability;
        public static int? ITLKMLProbability;

        public static int? KNKharifMaxProbability;
        public static int? KNLKMaxProbability;
        public static int? KNKharifMinProbability;
        public static int? KNLKMinProbability;
        public static int? KNKharifMLProbability;
        public static int? KNLKMLProbability;

        List<object> lstMaxValues;
        List<object> lstMinValues;
        List<object> lstLikelyValues;
        List<SP_ForecastScenario> lstProbabilities;
        public static long DraftID = -1;

        #endregion

        #region ViewState

        public string AddView = "AddView";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
                    || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningApril)
                    Season = (int)Constants.Seasons.Kharif;

                if (!IsPostBack)
                {
                    SetTitle();

                    if (Convert.ToString(Request.QueryString["From"]).ToUpper() == "VIEW")
                    {
                        ViewState[AddView] = "VIEW";
                        GetSavedDraft();
                        divStep1.Visible = false;
                    }
                    else
                    {
                        ViewState[AddView] = "ADD";
                        bool Result = new SeasonalPlanningBLL().AllowedToAddMoreDrafts();
                        if (!Result)
                        {
                            GetSavedDraft();
                            divStep1.Visible = false;
                            ViewState[AddView] = "VIEW";
                            Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            txtName.Text = "SRM Draft for Kharif " + DateTime.Now.Year.ToString();
                    }

                    //if (Season == (int)Constants.Seasons.Rabi)
                    //{
                    //    lblKharifMax.Text = "Rabi (MAF)";
                    //    lblKharifMin.Text = "Rabi (MAF)";
                    //    lblKharifML.Text = "Rabi (MAF)";
                    //    trLKMax.Visible = false;
                    //    trLKMin.Visible = false;
                    //    trLKML.Visible = false;
                    //}
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }


        #region Step 1(Input Region)

        public void JhemulAtManglaRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtJMKharifMax.CssClass = txtJMKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifMax.Attributes.Add("required", "required");
                    txtJMLKMax.CssClass = txtJMLKMax.CssClass.Replace("form-control", "form-control required");
                    txtJMLKMax.Attributes.Add("required", "required");

                    txtJMKharifMin.CssClass = txtJMKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifMin.Attributes.Add("required", "required");
                    txtJMLKMin.CssClass = txtJMLKMin.CssClass.Replace("form-control", "form-control required");
                    txtJMLKMin.Attributes.Add("required", "required");

                    txtJMKharifML.CssClass = txtJMKharifML.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifML.Attributes.Add("required", "required");
                    txtJMLKML.CssClass = txtJMLKML.CssClass.Replace("form-control", "form-control required");
                    txtJMLKML.Attributes.Add("required", "required");
                }
                else
                {
                    txtJMKharifMax.CssClass = txtJMKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifMax.Attributes.Add("required", "required");
                    txtJMKharifMin.CssClass = txtJMKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifMin.Attributes.Add("required", "required");
                    txtJMKharifML.CssClass = txtJMKharifML.CssClass.Replace("form-control", "form-control required");
                    txtJMKharifML.Attributes.Add("required", "required");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void JhemulAtManglaNotRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtJMKharifMax.CssClass = txtJMKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifMax.Attributes.Add("required", "false");
                    txtJMLKMax.CssClass = txtJMLKMax.CssClass.Replace("form-control required", "form-control");
                    txtJMLKMax.Attributes.Add("required", "false");

                    txtJMKharifMin.CssClass = txtJMKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifMin.Attributes.Add("required", "false");
                    txtJMLKMin.CssClass = txtJMLKMin.CssClass.Replace("form-control required", "form-control");
                    txtJMLKMin.Attributes.Add("required", "false");

                    txtJMKharifML.CssClass = txtJMKharifML.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifML.Attributes.Add("required", "false");
                    txtJMLKML.CssClass = txtJMLKML.CssClass.Replace("form-control required", "form-control");
                    txtJMLKML.Attributes.Add("required", "false");
                }
                else
                {
                    txtJMKharifMax.CssClass = txtJMKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifMax.Attributes.Add("required", "false");
                    txtJMKharifMin.CssClass = txtJMKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifMin.Attributes.Add("required", "false");
                    txtJMKharifML.CssClass = txtJMKharifML.CssClass.Replace("form-control required", "form-control");
                    txtJMKharifML.Attributes.Add("required", "false");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void ChenabatMaralaRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtCMKharifMax.CssClass = txtCMKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifMax.Attributes.Add("required", "required");
                    txtCMLKMax.CssClass = txtCMLKMax.CssClass.Replace("form-control", "form-control required");
                    txtCMLKMax.Attributes.Add("required", "required");

                    txtCMKharifMin.CssClass = txtCMKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifMin.Attributes.Add("required", "required");
                    txtCMLKMin.CssClass = txtCMLKMin.CssClass.Replace("form-control", "form-control required");
                    txtCMLKMin.Attributes.Add("required", "required");

                    txtCMKharifML.CssClass = txtCMKharifML.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifML.Attributes.Add("required", "required");
                    txtCMLKML.CssClass = txtCMLKML.CssClass.Replace("form-control", "form-control required");
                    txtCMLKML.Attributes.Add("required", "required");
                }
                else
                {
                    txtCMKharifMax.CssClass = txtCMKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifMax.Attributes.Add("required", "required");
                    txtCMKharifMin.CssClass = txtCMKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifMin.Attributes.Add("required", "required");
                    txtCMKharifML.CssClass = txtCMKharifML.CssClass.Replace("form-control", "form-control required");
                    txtCMKharifML.Attributes.Add("required", "required");
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void ChenabatMaralaNotRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtCMKharifMax.CssClass = txtCMKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifMax.Attributes.Add("required", "false");
                    txtCMLKMax.CssClass = txtCMLKMax.CssClass.Replace("form-control required", "form-control");
                    txtCMLKMax.Attributes.Add("required", "false");

                    txtCMKharifMin.CssClass = txtCMKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifMin.Attributes.Add("required", "false");
                    txtCMLKMin.CssClass = txtCMLKMin.CssClass.Replace("form-control required", "form-control");
                    txtCMLKMin.Attributes.Add("required", "false");

                    txtCMKharifML.CssClass = txtCMKharifML.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifML.Attributes.Add("required", "false");
                    txtCMLKML.CssClass = txtCMLKML.CssClass.Replace("form-control required", "form-control");
                    txtCMLKML.Attributes.Add("required", "false");
                }
                else
                {
                    txtCMKharifMax.CssClass = txtCMKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifMax.Attributes.Add("required", "false");
                    txtCMKharifMin.CssClass = txtCMKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifMin.Attributes.Add("required", "false");
                    txtCMKharifML.CssClass = txtCMKharifML.CssClass.Replace("form-control required", "form-control");
                    txtCMKharifML.Attributes.Add("required", "false");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void IndusAtTarbelaRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtITKharifMax.CssClass = txtITKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtITKharifMax.Attributes.Add("required", "required");
                    txtITLKMax.CssClass = txtITLKMax.CssClass.Replace("form-control", "form-control required");
                    txtITLKMax.Attributes.Add("required", "required");

                    txtITKharifMin.CssClass = txtITKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtITKharifMin.Attributes.Add("required", "required");
                    txtITLKMin.CssClass = txtITLKMin.CssClass.Replace("form-control", "form-control required");
                    txtITLKMin.Attributes.Add("required", "required");

                    txtITKharifML.CssClass = txtITKharifML.CssClass.Replace("form-control", "form-control required");
                    txtITKharifML.Attributes.Add("required", "required");
                    txtITLKML.CssClass = txtITLKML.CssClass.Replace("form-control", "form-control required");
                    txtITLKML.Attributes.Add("required", "required");
                }
                else
                {
                    txtITKharifMax.CssClass = txtITKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtITKharifMax.Attributes.Add("required", "required");
                    txtITKharifMin.CssClass = txtITKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtITKharifMin.Attributes.Add("required", "required");
                    txtITKharifML.CssClass = txtITKharifML.CssClass.Replace("form-control", "form-control required");
                    txtITKharifML.Attributes.Add("required", "required");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void IndusAtTarbelaNotRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtITKharifMax.CssClass = txtITKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtITKharifMax.Attributes.Add("required", "false");
                    txtITLKMax.CssClass = txtITLKMax.CssClass.Replace("form-control required", "form-control");
                    txtITLKMax.Attributes.Add("required", "false");

                    txtITKharifMin.CssClass = txtITKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtITKharifMin.Attributes.Add("required", "false");
                    txtITLKMin.CssClass = txtITLKMin.CssClass.Replace("form-control required", "form-control");
                    txtITLKMin.Attributes.Add("required", "false");

                    txtITKharifML.CssClass = txtITKharifML.CssClass.Replace("form-control required", "form-control");
                    txtITKharifML.Attributes.Add("required", "false");
                    txtITLKML.CssClass = txtITLKML.CssClass.Replace("form-control required", "form-control");
                    txtITLKML.Attributes.Add("required", "false");
                }
                else
                {
                    txtITKharifMax.CssClass = txtITKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtITKharifMax.Attributes.Add("required", "false");
                    txtITKharifMin.CssClass = txtITKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtITKharifMin.Attributes.Add("required", "false");
                    txtITKharifML.CssClass = txtITKharifML.CssClass.Replace("form-control required", "form-control");
                    txtITKharifML.Attributes.Add("required", "false");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void KabulAtNowsheraRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtKNKharifMax.CssClass = txtKNKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifMax.Attributes.Add("required", "required");
                    txtKNLKMax.CssClass = txtKNLKMax.CssClass.Replace("form-control", "form-control required");
                    txtKNLKMax.Attributes.Add("required", "required");

                    txtKNKharifMin.CssClass = txtKNKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifMin.Attributes.Add("required", "required");
                    txtKNLKMin.CssClass = txtKNLKMin.CssClass.Replace("form-control", "form-control required");
                    txtKNLKMin.Attributes.Add("required", "required");

                    txtKNKharifML.CssClass = txtKNKharifML.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifML.Attributes.Add("required", "required");
                    txtKNLKML.CssClass = txtKNLKML.CssClass.Replace("form-control", "form-control required");
                    txtKNLKML.Attributes.Add("required", "required");
                }
                else
                {
                    txtKNKharifMax.CssClass = txtKNKharifMax.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifMax.Attributes.Add("required", "required");
                    txtKNKharifMin.CssClass = txtKNKharifMin.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifMin.Attributes.Add("required", "required");
                    txtKNKharifML.CssClass = txtKNKharifML.CssClass.Replace("form-control", "form-control required");
                    txtKNKharifML.Attributes.Add("required", "required");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void KabulAtNowsheraNotRequiredField()
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    txtKNKharifMax.CssClass = txtKNKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifMax.Attributes.Add("required", "false");
                    txtKNLKMax.CssClass = txtKNLKMax.CssClass.Replace("form-control required", "form-control");
                    txtKNLKMax.Attributes.Add("required", "false");

                    txtKNKharifMin.CssClass = txtKNKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifMin.Attributes.Add("required", "false");
                    txtKNLKMin.CssClass = txtKNLKMin.CssClass.Replace("form-control required", "form-control");
                    txtKNLKMin.Attributes.Add("required", "false");

                    txtKNKharifML.CssClass = txtKNKharifML.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifML.Attributes.Add("required", "false");
                    txtKNLKML.CssClass = txtKNLKML.CssClass.Replace("form-control required", "form-control");
                    txtKNLKML.Attributes.Add("required", "false");
                }
                else
                {
                    txtKNKharifMax.CssClass = txtKNKharifMax.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifMax.Attributes.Add("required", "false");
                    txtKNKharifMin.CssClass = txtKNKharifMin.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifMin.Attributes.Add("required", "false");
                    txtKNKharifML.CssClass = txtKNKharifML.CssClass.Replace("form-control required", "form-control");
                    txtKNKharifML.Attributes.Add("required", "false");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMKharifMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "" && txtJMLKMax.Text == "" && txtJMLKMin.Text == "" && txtJMLKML.Text == "")
                    {
                        if (txtJMKharifMax.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtJMLKMax.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "")
                //    {
                //        if (txtJMKharifMax.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMLKMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "" && txtJMKharifMax.Text == "" && txtJMLKMin.Text == "" && txtJMLKML.Text == "")
                    {
                        if (txtJMLKMax.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtJMKharifMin.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "")
                //    {
                //        if (txtJMLKMax.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMKharifMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMax.Text == "" && txtJMKharifML.Text == "" && txtJMLKMax.Text == "" && txtJMLKMin.Text == "" && txtJMLKML.Text == "")
                    {
                        if (txtJMKharifMin.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtJMLKMin.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMax.Text == "" && txtJMKharifML.Text == "")
                //    {
                //        if (txtJMKharifMin.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMLKMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "" && txtJMKharifMax.Text == "" && txtJMLKMax.Text == "" && txtJMLKML.Text == "")
                    {
                        if (txtJMLKMin.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtJMKharifML.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMin.Text == "" && txtJMKharifML.Text == "")
                //    {
                //        if (txtJMLKMin.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMKharifML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMin.Text == "" && txtJMKharifMax.Text == "" && txtJMLKMin.Text == "" && txtJMLKMax.Text == "" && txtJMLKML.Text == "")
                    {
                        if (txtJMKharifML.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtJMLKML.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMin.Text == "" && txtJMKharifMax.Text == "")
                //    {
                //        if (txtJMKharifML.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJMLKML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtJMKharifMin.Text == "" && txtJMKharifMax.Text == "" && txtJMKharifML.Text == "" && txtJMLKMin.Text == "" && txtJMLKMax.Text == "")
                    {
                        if (txtJMLKML.Text != "")
                            JhemulAtManglaRequiredField();
                        else
                            JhemulAtManglaNotRequiredField();
                    }
                    txtCMKharifMax.Focus();
                }
                //else
                //{
                //    if (txtJMKharifMin.Text == "" && txtJMKharifMax.Text == "")
                //    {
                //        if (txtJMLKML.Text != "")
                //            JhemulAtManglaRequiredField();
                //        else
                //            JhemulAtManglaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMKharifMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMin.Text == "" && txtCMKharifML.Text == "" && txtCMLKMax.Text == "" && txtCMLKMin.Text == "" && txtCMLKML.Text == "")
                    {
                        if (txtCMKharifMax.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtCMLKMax.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMin.Text == "" && txtCMKharifML.Text == "")
                //    {
                //        if (txtCMKharifMax.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMLKMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMin.Text == "" && txtCMKharifML.Text == "" && txtCMKharifMax.Text == "" && txtCMLKMin.Text == "" && txtCMLKML.Text == "")
                    {
                        if (txtCMLKMax.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtCMKharifMin.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMin.Text == "" && txtCMKharifML.Text == "")
                //    {
                //        if (txtCMLKMax.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMKharifMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMax.Text == "" && txtCMKharifML.Text == "" && txtCMLKMax.Text == "" && txtCMLKMin.Text == "" && txtCMLKML.Text == "")
                    {
                        if (txtCMKharifMin.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtCMLKMin.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMax.Text == "" && txtCMKharifML.Text == "")
                //    {
                //        if (txtCMKharifMin.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMLKMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "" && txtCMKharifML.Text == "" && txtCMLKMax.Text == "" && txtCMLKML.Text == "")
                    {
                        if (txtCMLKMin.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtCMKharifML.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "")
                //    {
                //        if (txtCMLKMin.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMKharifML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "" && txtCMLKMin.Text == "" && txtCMLKMax.Text == "" && txtCMLKML.Text == "")
                    {
                        if (txtCMKharifML.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtCMLKML.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "" && txtCMLKMin.Text == "" && txtCMLKMax.Text == "" && txtCMLKML.Text == "")
                //    {
                //        if (txtCMKharifML.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtCMLKML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "" && txtCMKharifML.Text == "" && txtCMLKMin.Text == "" && txtCMLKMax.Text == "")
                    {
                        if (txtCMLKML.Text != "")
                            ChenabatMaralaRequiredField();
                        else
                            ChenabatMaralaNotRequiredField();
                    }
                    txtITKharifMax.Focus();
                }
                //else
                //{
                //    if (txtCMKharifMax.Text == "" && txtCMKharifMin.Text == "")
                //    {
                //        if (txtCMLKML.Text != "")
                //            ChenabatMaralaRequiredField();
                //        else
                //            ChenabatMaralaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITKharifMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMin.Text == "" && txtITKharifML.Text == "" && txtITLKMax.Text == "" && txtITLKMin.Text == "" && txtITLKML.Text == "")
                    {
                        if (txtITKharifMax.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtITLKMax.Focus();
                }
                //else
                //{
                //    if (txtITKharifMin.Text == "" && txtITKharifML.Text == "")
                //    {
                //        if (txtITKharifMax.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITLKMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMin.Text == "" && txtITKharifML.Text == "" && txtITKharifMax.Text == "" && txtITLKMin.Text == "" && txtITLKML.Text == "")
                    {
                        if (txtITLKMax.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtITKharifMin.Focus();
                }
                //else
                //{
                //    if (txtITKharifMin.Text == "" && txtITKharifML.Text == "")
                //    {
                //        if (txtITLKMax.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITKharifMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMax.Text == "" && txtITKharifML.Text == "" && txtITLKMax.Text == "" && txtITLKMin.Text == "" && txtITLKML.Text == "")
                    {
                        if (txtITKharifMin.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtITLKMin.Focus();
                }
                //else
                //{
                //    if (txtITKharifMax.Text == "" && txtITKharifML.Text == "")
                //    {
                //        if (txtITKharifMin.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITLKMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMax.Text == "" && txtITKharifML.Text == "" && txtITKharifMin.Text == "" && txtITLKMax.Text == "" && txtITLKML.Text == "")
                    {
                        if (txtITLKMin.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtITKharifML.Focus();
                }
                //else
                //{
                //    if (txtITKharifMax.Text == "" && txtITKharifML.Text == "")
                //    {
                //        if (txtITLKMin.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITKharifML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMax.Text == "" && txtITKharifMin.Text == "" && txtITLKMax.Text == "" && txtITLKMin.Text == "" && txtITLKML.Text == "")
                    {
                        if (txtITKharifML.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtITLKML.Focus();
                }
                //else
                //{
                //    if (txtITKharifMax.Text == "" && txtITKharifMin.Text == "")
                //    {
                //        if (txtITKharifML.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtITLKML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtITKharifMax.Text == "" && txtITKharifMin.Text == "" && txtITKharifML.Text == "" && txtITLKMax.Text == "" && txtITLKMin.Text == "")
                    {
                        if (txtITLKML.Text != "")
                            IndusAtTarbelaRequiredField();
                        else
                            IndusAtTarbelaNotRequiredField();
                    }
                    txtKNKharifMax.Focus();
                }
                //else
                //{
                //    if (txtITKharifMax.Text == "" && txtITKharifMin.Text == "")
                //    {
                //        if (txtITLKML.Text != "")
                //            IndusAtTarbelaRequiredField();
                //        else
                //            IndusAtTarbelaNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNKharifMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMin.Text == "" && txtKNKharifML.Text == "" && txtKNLKMax.Text == "" && txtKNLKMin.Text == "" && txtKNLKML.Text == "")
                    {
                        if (txtKNKharifMax.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                    txtKNLKMax.Focus();
                }
                //else
                //{
                //    if (txtKNKharifMin.Text == "" && txtKNKharifML.Text == "")
                //    {
                //        if (txtKNKharifMax.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNLKMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMin.Text == "" && txtKNKharifML.Text == "" && txtKNKharifMax.Text == "" && txtKNLKMin.Text == "" && txtKNLKML.Text == "")
                    {
                        if (txtKNLKMax.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                    txtKNKharifMin.Focus();
                }
                //else
                //{
                //    if (txtKNKharifMin.Text == "" && txtKNKharifML.Text == "")
                //    {
                //        if (txtKNLKMax.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNKharifMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMax.Text == "" && txtKNKharifML.Text == "" && txtKNLKMax.Text == "" && txtKNLKMin.Text == "" && txtKNLKML.Text == "")
                    {
                        if (txtKNKharifMin.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                    txtKNLKMin.Focus();
                }
                //else
                //{
                //    if (txtKNKharifMax.Text == "" && txtKNKharifML.Text == "")
                //    {
                //        if (txtKNKharifMin.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNLKMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMax.Text == "" && txtKNKharifML.Text == "" && txtKNKharifMin.Text == "" && txtKNLKMax.Text == "" && txtKNLKML.Text == "")
                    {
                        if (txtKNLKMin.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                    txtKNKharifML.Focus();
                }
                //else
                //{
                //    if (txtKNKharifMax.Text == "" && txtKNKharifML.Text == "")
                //    {
                //        if (txtKNLKMin.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNKharifML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMax.Text == "" && txtKNKharifMin.Text == "" && txtKNLKMin.Text == "" && txtKNLKMax.Text == "" && txtKNLKML.Text == "")
                    {
                        if (txtKNKharifML.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                    txtKNLKML.Focus();
                }
                //else
                //{
                //    if (txtKNKharifMax.Text == "" && txtKNKharifMin.Text == "")
                //    {
                //        if (txtKNKharifML.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtKNLKML_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (txtKNKharifMax.Text == "" && txtKNKharifMin.Text == "" && txtKNKharifML.Text == "" && txtKNLKMin.Text == "" && txtKNLKMax.Text == "")
                    {
                        if (txtKNLKML.Text != "")
                            KabulAtNowsheraRequiredField();
                        else
                            KabulAtNowsheraNotRequiredField();
                    }
                }
                //else
                //{
                //    if (txtKNKharifMax.Text == "" && txtKNKharifMin.Text == "")
                //    {
                //        if (txtKNLKML.Text != "")
                //            KabulAtNowsheraRequiredField();
                //        else
                //            KabulAtNowsheraNotRequiredField();
                //    }
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Step Forecast

        protected void gvMax_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ViewState[AddView] != null && Convert.ToString(ViewState[AddView]).ToUpper() == "ADD")
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (Season == (int)Constants.Seasons.Kharif)
                        {
                            Label Header = (Label)e.Row.FindControl("lblJMKahrifPer");
                            Header.Text = "EK %:" + JMKharifMaxProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                            Header.Text = "LK %:" + JMLKMaxProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                            Header.Text = "EK %:" + CMKharifMaxProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                            Header.Text = "LK %:" + CMLKMaxProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                            Header.Text = "EK %:" + ITKharifMaxProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblITLKPer"));
                            Header.Text = "LK %:" + ITLKMaxProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                            Header.Text = "EK %:" + KNKharifMaxProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                            Header.Text = "LK %:" + KNLKMaxProbability.ToString();
                        }
                        //else
                        //{
                        //    Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        //    Header.Text = "Rabi %:" + JMKharifMaxProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        //    Header.Text = "Rabi %:" + CMKharifMaxProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        //    Header.Text = "Rabi %:" + ITKharifMaxProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        //    Header.Text = "Rabi %:" + KNKharifMaxProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        //    Header.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        //if (Season == (int)Constants.Seasons.Kharif)
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().JhelumMangla - lstMaxValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().ChenabMarala - lstMaxValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().IndusTarbela - lstMaxValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().KabulNowshera - lstMaxValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.LastOrDefault().KabulNowshera);
                        //}
                        //else
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(17).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(17).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(17).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstMaxValues != null && lstMaxValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMaxValues.ElementAtOrDefault(17).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                            e.Row.Font.Bold = true;
                    }
                }
                else  // View Case
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Label JMEK = (Label)e.Row.FindControl("lblJMKahrifPer");
                        Label JMLK = (Label)e.Row.FindControl("lblJMLKPer");
                        Label CMEK = (Label)e.Row.FindControl("lblCMKahrifPer");
                        Label CMLK = (Label)e.Row.FindControl("lblCMLKPer");
                        Label ITEK = (Label)e.Row.FindControl("lblITKahrifPer");
                        Label ITLK = (Label)e.Row.FindControl("lblITLKPer");
                        Label KNEK = (Label)e.Row.FindControl("lblKNKahrifPer");
                        Label KNLK = (Label)e.Row.FindControl("lblKNLKPer");

                        JMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                        JMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                        CMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                        CMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                        ITEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                        ITLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);

                        KNEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Maximum").FirstOrDefault().EkPercent);
                        KNLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Maximum").FirstOrDefault().LkPercent);
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                        {
                            e.Row.Font.Bold = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ViewState[AddView] != null && Convert.ToString(ViewState[AddView]).ToUpper() == "ADD")
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (Season == (int)Constants.Seasons.Kharif)
                        {
                            Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                            Header.Text = "EK %:" + JMKharifMinProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                            Header.Text = "LK %:" + JMLKMinProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                            Header.Text = "EK %:" + CMKharifMaxProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                            Header.Text = "LK %:" + CMLKMinProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                            Header.Text = "EK %:" + ITKharifMinProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblITLKPer"));
                            Header.Text = "LK %:" + ITLKMinProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                            Header.Text = "EK %:" + KNKharifMinProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                            Header.Text = "LK %:" + KNLKMinProbability.ToString();
                        }
                        //else
                        //{
                        //    Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        //    Header.Text = "Rabi %:" + JMKharifMinProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        //    Header.Text = "Rabi %:" + CMKharifMinProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        //    Header.Text = "Rabi %:" + ITKharifMinProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        //    Header.Text = "Rabi %:" + KNKharifMinProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        //    Header.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        //if (Season == (int)Constants.Seasons.Kharif)
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().JhelumMangla - lstMinValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().ChenabMarala - lstMinValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().IndusTarbela - lstMinValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().KabulNowshera - lstMinValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.LastOrDefault().KabulNowshera);
                        //}
                        //else
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(17).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(17).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(17).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstMinValues != null && lstMinValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(17).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                        {
                            e.Row.Font.Bold = true;
                        }
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Label JMEK = (Label)e.Row.FindControl("lblJMKahrifPer");
                        Label JMLK = (Label)e.Row.FindControl("lblJMLKPer");
                        Label CMEK = (Label)e.Row.FindControl("lblCMKahrifPer");
                        Label CMLK = (Label)e.Row.FindControl("lblCMLKPer");
                        Label ITEK = (Label)e.Row.FindControl("lblITKahrifPer");
                        Label ITLK = (Label)e.Row.FindControl("lblITLKPer");
                        Label KNEK = (Label)e.Row.FindControl("lblKNKahrifPer");
                        Label KNLK = (Label)e.Row.FindControl("lblKNLKPer");

                        JMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                        JMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                        CMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                        CMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                        ITEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                        ITLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);

                        KNEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Minimum").FirstOrDefault().EkPercent);
                        KNLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Minimum").FirstOrDefault().LkPercent);
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                        {
                            e.Row.Font.Bold = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLikely_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ViewState[AddView] != null && Convert.ToString(ViewState[AddView]).ToUpper() == "ADD")
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (Season == (int)Constants.Seasons.Kharif)
                        {
                            Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                            Header.Text = "EK %:" + JMKharifMLProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                            Header.Text = "LK %:" + JMLKMLProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                            Header.Text = "EK %:" + CMKharifMLProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                            Header.Text = "LK %:" + CMLKMLProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                            Header.Text = "EK %:" + ITKharifMLProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblITLKPer"));
                            Header.Text = "LK %:" + ITLKMLProbability.ToString();

                            Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                            Header.Text = "EK %:" + KNKharifMLProbability.ToString();
                            Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                            Header.Text = "LK %:" + KNLKMLProbability.ToString();
                        }
                        //else
                        //{
                        //    Label Header = (Label)(e.Row.FindControl("lblJMKahrifPer"));
                        //    Header.Text = "Rabi %:" + JMKharifMLProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblJMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblCMKahrifPer"));
                        //    Header.Text = "Rabi %:" + CMKharifMLProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblCMLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblITKahrifPer"));
                        //    Header.Text = "Rabi %:" + ITKharifMLProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblITLKPer"));
                        //    Header.Visible = false;

                        //    Header = (Label)(e.Row.FindControl("lblKNKahrifPer"));
                        //    Header.Text = "Rabi %:" + KNKharifMLProbability.ToString();
                        //    Header = (Label)(e.Row.FindControl("lblKNLKPer"));
                        //    Header.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        //if (Season == (int)Constants.Seasons.Kharif)
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().JhelumMangla - lstLikelyValues.ElementAtOrDefault(6).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().ChenabMarala - lstLikelyValues.ElementAtOrDefault(6).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().IndusTarbela - lstLikelyValues.ElementAtOrDefault(6).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().KabulNowshera - lstLikelyValues.ElementAtOrDefault(6).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.LastOrDefault().KabulNowshera);
                        //}
                        //else
                        //{
                        //    Label Footer = (Label)e.Row.FindControl("lblJMKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(17).JhelumMangla);

                        //    Footer = (Label)e.Row.FindControl("lblJMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblJMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstMinValues.ElementAtOrDefault(17).ChenabMarala);

                        //    Footer = (Label)e.Row.FindControl("lblCMLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblCMTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(17).IndusTarbela);

                        //    Footer = (Label)e.Row.FindControl("lblITLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblITTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNKharif");
                        //    if (Footer != null && lstLikelyValues != null && lstLikelyValues.Count() > 0)
                        //        Footer.Text = Convert.ToString(lstLikelyValues.ElementAtOrDefault(17).KabulNowshera);

                        //    Footer = (Label)e.Row.FindControl("lblKNLK");
                        //    if (Footer != null)
                        //        Footer.Visible = false;

                        //    Footer = (Label)e.Row.FindControl("lblKNTotal");
                        //    if (Footer != null)
                        //        Footer.Visible = false;
                        //}
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                        {
                            e.Row.Font.Bold = true;
                        }
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Label JMEK = (Label)e.Row.FindControl("lblJMKahrifPer");
                        Label JMLK = (Label)e.Row.FindControl("lblJMLKPer");
                        Label CMEK = (Label)e.Row.FindControl("lblCMKahrifPer");
                        Label CMLK = (Label)e.Row.FindControl("lblCMLKPer");
                        Label ITEK = (Label)e.Row.FindControl("lblITKahrifPer");
                        Label ITLK = (Label)e.Row.FindControl("lblITLKPer");
                        Label KNEK = (Label)e.Row.FindControl("lblKNKahrifPer");
                        Label KNLK = (Label)e.Row.FindControl("lblKNLKPer");

                        JMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                        JMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                        CMEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                        CMLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                        ITEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                        ITLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);

                        KNEK.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Likely").FirstOrDefault().EkPercent);
                        KNLK.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                    && q.Scenario == "Likely").FirstOrDefault().LkPercent);
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblTDaily");
                        if (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)")
                        {
                            e.Row.Font.Bold = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnForecast_Click(object sender, EventArgs e)
        {
            try
            {
                decimal? JM = null;
                decimal? JMLK = null;
                decimal? CM = null;
                decimal? CMLK = null;
                decimal? IT = null;
                decimal? ITLK = null;
                decimal? KN = null;
                decimal? KNLK = null;
                lstMaxValues = new List<object>();
                lstMinValues = new List<object>();
                lstLikelyValues = new List<object>();

                JMKharifMaxProbability = null;
                JMLKMaxProbability = null;
                JMKharifMinProbability = null;
                JMLKMinProbability = null;
                JMKharifMLProbability = null;
                JMLKMLProbability = null;

                CMKharifMaxProbability = null;
                CMLKMaxProbability = null;
                CMKharifMinProbability = null;
                CMLKMinProbability = null;
                CMKharifMLProbability = null;
                CMLKMLProbability = null;

                ITKharifMaxProbability = null;
                ITLKMaxProbability = null;
                ITKharifMinProbability = null;
                ITLKMinProbability = null;
                ITKharifMLProbability = null;
                ITLKMLProbability = null;

                KNKharifMaxProbability = null;
                KNLKMaxProbability = null;
                KNKharifMinProbability = null;
                KNLKMinProbability = null;
                KNKharifMLProbability = null;
                KNLKMLProbability = null;

                if (Season == (int)Constants.Seasons.Kharif)
                {
                    if (((TextBox)tblMax.FindControl("txtJMKharifMax")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifMax.Text);
                        JMKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtJMLKMax")).Text != "")
                    {
                        JMLK = Convert.ToDecimal(txtJMLKMax.Text);
                        JMLKMaxProbability = new SeasonalPlanningBLL().ForecastProbability(JMLK, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifMax")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifMax.Text);
                        CMKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMLKMax")).Text != "")
                    {
                        CMLK = Convert.ToDecimal(txtCMLKMax.Text);
                        CMLKMaxProbability = new SeasonalPlanningBLL().ForecastProbability(CMLK, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifMax")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifMax.Text);
                        ITKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITLKMax")).Text != "")
                    {
                        ITLK = Convert.ToDecimal(txtITLKMax.Text);
                        ITLKMaxProbability = new SeasonalPlanningBLL().ForecastProbability(ITLK, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifMax")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifMax.Text);
                        KNKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNLKMax")).Text != "")
                    {
                        KNLK = Convert.ToDecimal(txtKNLKMax.Text);
                        KNLKMaxProbability = new SeasonalPlanningBLL().ForecastProbability(KNLK, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }

                    lstMaxValues = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMaxProbability, JMLKMaxProbability, CMKharifMaxProbability, CMLKMaxProbability,
                                                                  ITKharifMaxProbability, ITLKMaxProbability, KNKharifMaxProbability, KNLKMaxProbability, true);
                    gvMax.DataSource = lstMaxValues;
                    gvMax.DataBind();


                    if (((TextBox)tblMax.FindControl("txtJMKharifMin")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifMin.Text);
                        JMKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtJMLKMin")).Text != "")
                    {
                        JMLK = Convert.ToDecimal(txtJMLKMin.Text);
                        JMLKMinProbability = new SeasonalPlanningBLL().ForecastProbability(JMLK, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifMin")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifMin.Text);
                        CMKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMLKMin")).Text != "")
                    {
                        CMLK = Convert.ToDecimal(txtCMLKMin.Text);
                        CMLKMinProbability = new SeasonalPlanningBLL().ForecastProbability(CMLK, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifMin")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifMin.Text);
                        ITKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITLKMin")).Text != "")
                    {
                        ITLK = Convert.ToDecimal(txtITLKMin.Text);
                        ITLKMinProbability = new SeasonalPlanningBLL().ForecastProbability(ITLK, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifMin")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifMin.Text);
                        KNKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNLKMin")).Text != "")
                    {
                        KNLK = Convert.ToDecimal(txtKNLKMin.Text);
                        KNLKMinProbability = new SeasonalPlanningBLL().ForecastProbability(KNLK, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    lstMinValues = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMinProbability, JMLKMinProbability, CMKharifMinProbability, CMLKMinProbability,
                                                                  ITKharifMinProbability, ITLKMinProbability, KNKharifMinProbability, KNLKMinProbability, true);
                    gvMin.DataSource = lstMinValues;
                    gvMin.DataBind();


                    if (((TextBox)tblMax.FindControl("txtJMKharifML")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifML.Text);
                        JMKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtJMLKML")).Text != "")
                    {
                        JMLK = Convert.ToDecimal(txtJMLKML.Text);
                        JMLKMLProbability = new SeasonalPlanningBLL().ForecastProbability(JMLK, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifML")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifML.Text);
                        CMKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMLKML")).Text != "")
                    {
                        CMLK = Convert.ToDecimal(txtCMLKML.Text);
                        CMLKMLProbability = new SeasonalPlanningBLL().ForecastProbability(CMLK, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifML")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifML.Text);
                        ITKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITLKML")).Text != "")
                    {
                        ITLK = Convert.ToDecimal(txtITLKML.Text);
                        ITLKMLProbability = new SeasonalPlanningBLL().ForecastProbability(ITLK, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifML")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifML.Text);
                        KNKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.EKEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNLKML")).Text != "")
                    {
                        KNLK = Convert.ToDecimal(txtKNLKML.Text);
                        KNLKMLProbability = new SeasonalPlanningBLL().ForecastProbability(KNLK, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Kharif, (int)Constants.SeasonDistribution.LKEnd);
                    }
                    lstLikelyValues = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMLProbability, JMLKMLProbability, CMKharifMLProbability, CMLKMLProbability,
                                                                  ITKharifMLProbability, ITLKMLProbability, KNKharifMLProbability, KNLKMLProbability, true);
                    gvLikely.DataSource = lstLikelyValues;
                    gvLikely.DataBind();
                }
                else
                {

                    JMLKMaxProbability = null;
                    JMLKMinProbability = null;
                    JMLKMLProbability = null;
                    CMLKMaxProbability = null;
                    CMLKMinProbability = null;
                    CMLKMLProbability = null;
                    ITLKMaxProbability = null;
                    ITLKMinProbability = null;
                    ITLKMLProbability = null;
                    KNLKMaxProbability = null;
                    KNLKMinProbability = null;
                    KNLKMLProbability = null;

                    if (((TextBox)tblMax.FindControl("txtJMKharifMax")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifMax.Text);
                        JMKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifMax")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifMax.Text);
                        CMKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifMax")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifMax.Text);
                        ITKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifMax")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifMax.Text);
                        KNKharifMaxProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    gvMax.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMaxProbability, JMLKMaxProbability, CMKharifMaxProbability, CMLKMaxProbability,
                                                                  ITKharifMaxProbability, ITLKMaxProbability, KNKharifMaxProbability, KNLKMaxProbability, true);
                    gvMax.DataBind();


                    if (((TextBox)tblMax.FindControl("txtJMKharifMin")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifMin.Text);
                        JMKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifMin")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifMin.Text);
                        CMKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifMin")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifMin.Text);
                        ITKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifMin")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifMin.Text);
                        KNKharifMinProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    gvMin.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMinProbability, JMLKMinProbability, CMKharifMinProbability, CMLKMinProbability,
                                                                  ITKharifMinProbability, ITLKMinProbability, KNKharifMinProbability, KNLKMinProbability, true);
                    gvMin.DataBind();



                    if (((TextBox)tblMax.FindControl("txtJMKharifML")).Text != "")
                    {
                        JM = Convert.ToDecimal(txtJMKharifML.Text);
                        JMKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(JM, (int)Constants.RimStationsIDs.JhelumATMangla, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtCMKharifML")).Text != "")
                    {
                        CM = Convert.ToDecimal(txtCMKharifML.Text);
                        CMKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(CM, (int)Constants.RimStationsIDs.ChenabAtMarala, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtITKharifML")).Text != "")
                    {
                        IT = Convert.ToDecimal(txtITKharifML.Text);
                        ITKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(IT, (int)Constants.RimStationsIDs.IndusAtTarbela, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    if (((TextBox)tblMax.FindControl("txtKNKharifML")).Text != "")
                    {
                        KN = Convert.ToDecimal(txtKNKharifML.Text);
                        KNKharifMLProbability = new SeasonalPlanningBLL().ForecastProbability(KN, (int)Constants.RimStationsIDs.KabulAtNowshera, (int)Constants.Seasons.Rabi, (int)Constants.SeasonDistribution.RabiEnd);
                    }
                    gvLikely.DataSource = new SeasonalPlanningBLL().GetForecastedValues(Season, JMKharifMLProbability, JMLKMLProbability, CMKharifMLProbability, CMLKMLProbability,
                                                                  ITKharifMLProbability, ITLKMLProbability, KNKharifMLProbability, KNLKMLProbability, true);
                    gvLikely.DataBind();
                }
                divStep1.Visible = false;
                divStepforecast.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long JMScenarioIDMax;
            long JMScenarioIDMin;
            long JMScenarioIDLikely;
            long CMScenarioIDMax;
            long CMScenarioIDMin;
            long CMScenarioIDLikely;
            long ITScenarioIDMax;
            long ITScenarioIDMin;
            long ITScenarioIDLikely;
            long KNScenarioIDMax;
            long KNScenarioIDMin;
            long KNScenarioIDLikely;
            try
            {
                new SeasonalPlanningBLL().DeletePreviousData(DraftID);

                SP_ForecastDraft Objsave = new SP_ForecastDraft();
                Objsave.Description = txtName.Text;
                Objsave.DraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                Objsave.SeasonID = (short)Season;
                Objsave.Year = (short)DateTime.Now.Year;
                Objsave.CreatedDate = DateTime.Now;
                Objsave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                Objsave.ModifiedDate = DateTime.Now;
                Objsave.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                DraftID = new SeasonalPlanningBLL().SaveStatisticalDraftBasicInfo(Objsave);

                if (Season == (int)Constants.Seasons.Kharif)
                {
                    SP_ForecastScenario ObjSave = new SP_ForecastScenario();
                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    if (JMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)JMKharifMaxProbability;
                    if (JMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)JMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    if (CMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)CMKharifMaxProbability;
                    if (CMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)CMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    if (ITKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)ITKharifMaxProbability;
                    if (ITLKMaxProbability != null)
                        ObjSave.LkPercent = (short)ITLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Maximum";
                    ObjSave.RabiPercent = null;
                    if (KNKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)KNKharifMaxProbability;
                    if (KNLKMaxProbability != null)
                        ObjSave.LkPercent = (short)KNLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMax = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JMKharifMaxProbability, JMLKMaxProbability, CMKharifMaxProbability, CMLKMaxProbability, ITKharifMaxProbability, ITLKMaxProbability, KNKharifMaxProbability, KNLKMaxProbability, JMScenarioIDMax, CMScenarioIDMax, ITScenarioIDMax, KNScenarioIDMax, Convert.ToInt32(Session[SessionValues.UserID]));

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    if (JMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)JMKharifMaxProbability;
                    if (JMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)JMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    if (CMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)CMKharifMaxProbability;
                    if (CMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)CMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    if (ITKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)ITKharifMaxProbability;
                    if (ITLKMaxProbability != null)
                        ObjSave.LkPercent = (short)ITLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Minimum";
                    ObjSave.RabiPercent = null;
                    if (KNKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)KNKharifMaxProbability;
                    if (KNLKMaxProbability != null)
                        ObjSave.LkPercent = (short)KNLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDMin = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JMKharifMinProbability, JMLKMinProbability, CMKharifMinProbability, CMLKMinProbability, ITKharifMinProbability, ITLKMinProbability,
                    KNKharifMinProbability, KNLKMinProbability, JMScenarioIDMin, CMScenarioIDMin, ITScenarioIDMin, KNScenarioIDMin, Convert.ToInt32(Session[SessionValues.UserID]));


                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    if (JMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)JMKharifMaxProbability;
                    if (JMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)JMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    JMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    if (CMKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)CMKharifMaxProbability;
                    if (CMLKMaxProbability != null)
                        ObjSave.LkPercent = (short)CMLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    CMScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    if (ITKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)ITKharifMaxProbability;
                    if (ITLKMaxProbability != null)
                        ObjSave.LkPercent = (short)ITLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ITScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);

                    ObjSave = new SP_ForecastScenario();
                    ObjSave.ForecastDraftID = DraftID;
                    ObjSave.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSave.Scenario = "Likely";
                    ObjSave.RabiPercent = null;
                    if (KNKharifMaxProbability != null)
                        ObjSave.EkPercent = (short)KNKharifMaxProbability;
                    if (KNLKMaxProbability != null)
                        ObjSave.LkPercent = (short)KNLKMaxProbability;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    KNScenarioIDLikely = new SeasonalPlanningBLL().SaveStatisticalDraftScenarios(ObjSave);
                    new SeasonalPlanningBLL().SaveForecastedValues(Season, JMKharifMLProbability, JMLKMLProbability, CMKharifMLProbability, CMLKMLProbability, ITKharifMLProbability, ITLKMLProbability,
                    KNKharifMLProbability, KNLKMLProbability, JMScenarioIDLikely, CMScenarioIDLikely, ITScenarioIDLikely, KNScenarioIDLikely, Convert.ToInt32(Session[SessionValues.UserID]));
                }
                Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                divStepforecast.Visible = false;
                GetSavedDraft();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Save and View
        public void GetSavedDraft()
        {
            try
            {
                gvView.DataSource = new SeasonalPlanningBLL().GetSRMDrafts();
                gvView.DataBind();
                divView.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int RecordID = Convert.ToInt32(((Label)gvView.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new SeasonalPlanningBLL().DeleteDraft(RecordID);
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    GetSavedDraft();
                }
                else
                    Master.ShowMessage(Message.NotallowedToDelete.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int RowID = e.NewEditIndex;
                long RecordID = Convert.ToInt64(((Label)gvView.Rows[RowID].Cells[0].FindControl("lblID")).Text);
                lstProbabilities = new List<SP_ForecastScenario>();
                lstProbabilities = new SeasonalPlanningBLL().GetSavedProbabilities(RecordID);
                gvMax.DataSource = new SeasonalPlanningBLL().GetSRMDetail(RecordID, "Maximum");
                gvMax.DataBind();
                // SetFooterForMaximumScenario();

                gvMin.DataSource = new SeasonalPlanningBLL().GetSRMDetail(RecordID, "Minimum");
                gvMin.DataBind();
                // SetFooterForMinimumScenario();

                gvLikely.DataSource = new SeasonalPlanningBLL().GetSRMDetail(RecordID, "Likely");
                gvLikely.DataBind();
                // SetFooterForLikelyScenario();

                btnSave.Visible = false;
                divView.Visible = false;
                btnbackStepForecast.Visible = false;
                divStepforecast.Visible = true;
                btnBackToView.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMaximumScenario()
        {
            try
            {
                GridViewRow row = gvMax.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvMax.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblcM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblcMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMax.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMax.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                row = gvMax.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvMax.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }


                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMax.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMax.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMinimumScenario()
        {
            try
            {
                GridViewRow row = gvMin.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvMin.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMin.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMin.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                row = gvMin.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvMin.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMin.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMin.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForLikelyScenario()
        {
            try
            {
                GridViewRow row = gvLikely.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvLikely.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;


                row = gvLikely.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvLikely.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        protected void btnBackViewDiv_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("InflowForecasting.aspx");
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnbackStepForecast_Click(object sender, EventArgs e)
        {
            try
            {
                divStep1.Visible = true;
                divStepforecast.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void btnBackToView_Click(object sender, EventArgs e)
        {
            try
            {
                divStepforecast.Visible = false;
                GetSavedDraft();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //protected void btnbackStepForecast_Click(object sender, EventArgs e)
        //{
        //    try
        //    {


        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
    }
}