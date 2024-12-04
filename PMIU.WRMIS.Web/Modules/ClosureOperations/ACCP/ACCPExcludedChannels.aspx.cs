using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP
{
    public partial class ACCPExcludedChannels : BasePage
    {
        Dictionary<string, object> dd_ACCPExcludeChannels = new Dictionary<string, object>();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        List<object> lstOfObj = new List<object>();
        UA_RoleRights mdlRoleRights;
        protected void Page_Load(object sender, EventArgs e)
        {

            long ACCPID = Utility.GetNumericValueFromQueryString("ACCPID", 0);
            long MainCanalID = Utility.GetNumericValueFromQueryString("MainCanalID", 0);
            long DetailID = Utility.GetNumericValueFromQueryString("DetailID", 0);
            hfACCPID.Value = ACCPID.ToString();
            hfMainCanalID.Value = MainCanalID.ToString();
            hfDetailID.Value = DetailID.ToString();
            ACCPIDMainCanalID.ACCPID = ACCPID;
            ACCPIDMainCanalID.ChannelID = MainCanalID;
            SetPageTitle();
            hlBack.NavigateUrl = string.Format("~/Modules/ClosureOperations/ACCP/ACCPDetails.aspx?ACCPID=" + ACCPID + "");
            if (!Page.IsPostBack)
            {
                bindChnnels();
                EnableDisableControls(true);
            }
        }

        protected bool SaveExcludeChannelsChild(List<string> _XChannelsIDs, string _DetailID)
        {
            List<object> listOfChannelsReadyToExclude = new List<object>();
            List<string> listOfExcludeChannelsReadyToSave = new List<string>();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            List<object> SlistChannel = new List<object>();
            List<object> SlistXChannel = new List<object>();
            SlistChannel = Session["Channels"] as List<object>;
            #region Remove Exclude Channels
            SlistXChannel = Session["XChannels"] as List<object>;
            if (SlistXChannel.Count == _XChannelsIDs.Count) { return true;  }
            if (SlistXChannel.Count > _XChannelsIDs.Count)
            {
                if (_XChannelsIDs.Count == 0)
                {
                    foreach (object sXchnl in SlistXChannel)
                    {
                        dd_ACCPExcludeChannels.Clear();
                        dd_ACCPExcludeChannels.Add("ID", sXchnl.GetType().GetProperty("ID").GetValue(sXchnl));
                        dd_ACCPExcludeChannels.Add("DetailID", _DetailID);
                        bllCO.ACCPExclude_Operations(Constants.CRUD_DELETE, dd_ACCPExcludeChannels);
                    }
                }
                else
                {
                    foreach (object sXchnl in SlistXChannel)
                    {
                        bool isExist = false;
                        foreach (string Xchnl in _XChannelsIDs)
                        {
                            if (Convert.ToInt64(Xchnl) == Convert.ToInt64(sXchnl.GetType().GetProperty("ID").GetValue(sXchnl)))
                            {
                                isExist = true;
                            }
                        }
                        if (!isExist)
                        {
                            listOfExcludeChannelsReadyToSave.Add(Convert.ToString(sXchnl.GetType().GetProperty("ID").GetValue(sXchnl)));
                        }
                    }
                    
                    foreach (string sXchnl in listOfExcludeChannelsReadyToSave)
                    {
                        dd_ACCPExcludeChannels.Clear();
                        dd_ACCPExcludeChannels.Add("ID", sXchnl);
                        dd_ACCPExcludeChannels.Add("DetailID", _DetailID);
                        bllCO.ACCPExclude_Operations(Constants.CRUD_DELETE, dd_ACCPExcludeChannels);
                    }
                }

            }
            #endregion
            #region if Some add new Channel for Exclude
            else
            {

                if (SlistXChannel.Count < _XChannelsIDs.Count)
                {
                    if (SlistXChannel.Count > 0)
                    {
                        foreach (string Xchnl in _XChannelsIDs)
                        {
                            bool alreadySaved = false;
                            foreach (object sXchnl in SlistXChannel)
                            {
                                if (Convert.ToInt64(Xchnl) == Convert.ToInt64(sXchnl.GetType().GetProperty("ID").GetValue(sXchnl)))
                                {
                                    alreadySaved = true;
                                }
                            }
                            if (!alreadySaved)
                            {
                                listOfExcludeChannelsReadyToSave.Add(Xchnl);
                            }
                            
                        }
                        foreach (string Xchnl in listOfExcludeChannelsReadyToSave)
                        {
                            dd_ACCPExcludeChannels.Clear();
                            dd_ACCPExcludeChannels.Add("DetailID", _DetailID);
                            dd_ACCPExcludeChannels.Add("CreatedBy", mdlUser.ID);
                            dd_ACCPExcludeChannels.Add("ChannelID", Xchnl);
                            bllCO.ACCPExclude_Operations(Constants.CRUD_CREATE, dd_ACCPExcludeChannels);
                            
                        }
                    }
                    else
                    {
                        foreach (string Xchnl in _XChannelsIDs)
                        {
                            dd_ACCPExcludeChannels.Clear();
                            dd_ACCPExcludeChannels.Add("DetailID", _DetailID);
                            dd_ACCPExcludeChannels.Add("CreatedBy", mdlUser.ID);
                            dd_ACCPExcludeChannels.Add("ChannelID", Xchnl);
                            bllCO.ACCPExclude_Operations(Constants.CRUD_CREATE, dd_ACCPExcludeChannels);
                            
                        }
                    }

                }
                else
                {
                    // do nothing , go back to Detail screen. 
                }
            }
            #endregion
            Session["XChannels"] = bllCO.GetExcludedChannels(Convert.ToInt64(_DetailID));
            return true;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool SaveExcludeChannels(List<string> _XChannelsIDs, string _DetailID)
        {
            ACCPExcludedChannels aechnl = new ACCPExcludedChannels();
            return aechnl.SaveExcludeChannelsChild(_XChannelsIDs, _DetailID);
        }
        private void EnableDisableControls(bool value)
        {
            btnAdd.Disabled = value;
            btnRemove.Disabled = value;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ExcludeChanneFromACCP);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void bindChnnels()
        {
            try
            {

                Session.Remove("XChannels");
                Session.Remove("Channels");
                List<object> ChannelsReadyForExculde = new List<object>();// TO Add
                List<object> ChannelsReadyForExculdeShow = new List<object>(); //Exluded
                List<object> ExculdedChannels = new List<object>();
                ChannelsReadyForExculde = bllCO.GetChannelsforExclude(Convert.ToInt64(hfMainCanalID.Value));
                ExculdedChannels = bllCO.GetExcludedChannels(Convert.ToInt64(hfDetailID.Value));
                bool flag = false;
                foreach (object Chnl in ChannelsReadyForExculde)
                {
                    flag = false;
                    foreach (object Xchnl in ExculdedChannels)
                    {
                        if (Convert.ToInt64(Chnl.GetType().GetProperty("ID").GetValue(Chnl)) == Convert.ToInt64(Xchnl.GetType().GetProperty("ID").GetValue(Xchnl)))
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        ChannelsReadyForExculdeShow.Add(Chnl);
                    }
                }
                Session["XChannels"] = ExculdedChannels;
                Session["Channels"] = ChannelsReadyForExculdeShow;
                if (ChannelsReadyForExculdeShow != null && ChannelsReadyForExculdeShow.Count > 0)
                {
                    lstBoxChannels.DataSource = ChannelsReadyForExculdeShow;
                    lstBoxChannels.DataTextField = "ChannelName";
                    lstBoxChannels.DataValueField = "ID";
                    lstBoxChannels.DataBind();
                }
                if (ExculdedChannels != null && ExculdedChannels.Count > 0)
                {
                    lstBoxExcludeChannels.DataSource = ExculdedChannels;
                    lstBoxExcludeChannels.DataTextField = "XChannelName";
                    lstBoxExcludeChannels.DataValueField = "ID";
                    lstBoxExcludeChannels.DataBind();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}