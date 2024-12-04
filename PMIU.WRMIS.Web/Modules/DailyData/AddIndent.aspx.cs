using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class AddIndent : BasePage
    {
        long ChannelID;
        long SubDivID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]) && !string.IsNullOrEmpty(Request.QueryString["SubDivID"]))
                    {
                        ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);
                        BindChannelData(ChannelID, SubDivID);
                        txtPlacementDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));



                        ////////////////////////////////////////////////////////
                        //IndentsBLL bllindents = new IndentsBLL();
                        //bllindents.GetLowerSubDivision(131169, 130);
                        //bllindents.GetIndentBySubDivIDandChannelID(1488, 133);
                        /////////////////////////////////////////////////////////
                        //double CurrentIndent;
                        //double LowerSubdivIndent;

                        //CO_ChannelIndent mdlChannelIndnetCurrent = bllIndents.GetIndentBySubDivIDandChannelID(171169, 60191);
                        //CurrentIndent = mdlChannelIndnetCurrent.IndentValue;

                        //long LowerSubDiv = bllIndents.GetLowerSubDivision(171169, 60191);

                        //CO_ChannelIndent mdlChannelIndnetLower = bllIndents.GetIndentBySubDivIDandChannelID(171169, LowerSubDiv);
                        //LowerSubdivIndent = mdlChannelIndnetLower.IndentValue;

                        //double SumIndent = AddSubdivisionIndents(CurrentIndent, LowerSubdivIndent);
                        //////////////////////////////////////////////////////////

                        //List<long> ListSubDiv=bllindents.GetListOfSubDivisions(131169);
                    }
                }

            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 8/12/2015 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddIndent);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds all the data to labels
        /// Created On: 8/12/2015
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_SubDivId"></param>
        private void BindChannelData(long _ChannelID, long _SubDivID)
        {
            ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
            SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);

            IndentsBLL bllIndents = new IndentsBLL();
            ChannelBLL bllChannel = new ChannelBLL();

            CO_Channel mdlChannel = bllChannel.GetChannelByID(_ChannelID);

            SubDivisionBLL bllSubDivision = new SubDivisionBLL();
            CO_SubDivision mdlSubDivison = bllSubDivision.GetSubDivisionsBySubDivisionID(_SubDivID);

            CO_ChannelIndent mdlChannelIndnetCurrent = bllIndents.GetIndentBySubDivIDandChannelID(_ChannelID, _SubDivID);
            long LowerSubDiv = bllIndents.GetLowerSubDivision(_ChannelID, _SubDivID);
            CO_ChannelIndent mdlChannelIndnetLower = bllIndents.GetIndentBySubDivIDandChannelID(_ChannelID, LowerSubDiv);

            lblChannelName.Text = mdlChannel.NAME;
            lblChannelType.Text = mdlChannel.CO_ChannelType.Name;
            lblTotalRD.Text = Calculations.GetRDText(mdlChannel.TotalRDs);
            lblFlowType.Text = mdlChannel.CO_ChannelFlowType.Name;
            lblCommandName.Text = mdlChannel.CO_ChannelComndType.Name;
            lblIMISCode.Text = mdlChannel.IMISCode;
            lblDivision.Text = mdlSubDivison.CO_Division.Name;
            lblSubDivision.Text = mdlSubDivison.Name;
            if (mdlChannelIndnetCurrent != null)
            {
                lblCurrentIndent.Text = Convert.ToString(mdlChannelIndnetCurrent.OutletIndent); //IndentValue);
            }

            if (mdlChannelIndnetLower != null)
            {
                List<long> lstSubDivision = bllIndents.GetListOfSubDivisions(ChannelID);
                long SubDivisonFromList;
                double CurrentIndentInLoop = 0.0;
                int ListIndex = lstSubDivision.FindIndex(a => a == SubDivID);
                for (int i = ListIndex + 1; i <= lstSubDivision.Count - 1; i++)
                {
                    SubDivisonFromList = lstSubDivision.ElementAt(i);
                    CO_ChannelIndent mdlChannelIndentCurrentInLoop = bllIndents.GetIndentBySubDivIDandChannelID(ChannelID, SubDivisonFromList);
                    if (mdlChannelIndentCurrentInLoop != null)
                    {
                        CurrentIndentInLoop = 1;//CurrentIndentInLoop + mdlChannelIndentCurrentInLoop.IndentValue;
                    }
                }

                double SumOfIndents = CurrentIndentInLoop;
                lblLowerSubDivisionIndent.Text = Convert.ToString(SumOfIndents);
                lblLowerSubDivisionIndentDate.Text = Convert.ToString(Utility.GetFormattedDate(mdlChannelIndnetLower.EntryDate));//FromDate));
            }
        }

        /// <summary>
        /// this function checks all the required validations and add indent to the database.
        /// Created On: 11/12/2015
        /// </summary>
        private void AddIndents()
        {
            double? DesignDischarge;
            long SectionID;
            ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
            SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);
            CO_ChannelIndent mdlChannelIndent = new CO_ChannelIndent();
            IndentsBLL bllIndents = new IndentsBLL();

            if (Convert.ToDateTime(txtPlacementDate.Text) < DateTime.Today)
            {
                Master.ShowMessage(Message.DateShouldBeGreaterThanOrEqualToCurrentDate.Description, SiteMaster.MessageType.Error);
                return;
            }
            else if (Convert.ToInt64(txtIndent.Text) < 0)
            {
                Master.ShowMessage(Message.NegativeValueNotAllowed.Description, SiteMaster.MessageType.Error);
                return;
            }
            else
            {

                //mdlChannelIndent.ChannelID = ChannelID;
                mdlChannelIndent.SubDivID = SubDivID;
                //mdlChannelIndent.FromDate = Convert.ToDateTime(txtPlacementDate.Text);
                //mdlChannelIndent.IndentValue = Convert.ToDouble(txtIndent.Text);
                //mdlChannelIndent.Remarks = txtRemarks.Text;

                CO_ChannelIndent mdlChannelIndnetExists = bllIndents.GetIndentBySubDivIDandChannelIDandDate(ChannelID, SubDivID, Convert.ToDateTime(txtPlacementDate.Text));
                if (mdlChannelIndnetExists != null && Convert.ToDateTime(txtPlacementDate.Text) == mdlChannelIndnetExists.EntryDate)// FromDate)
                {
                    //if (Convert.ToDateTime(txtPlacementDate.Text) == mdlChannelIndnetExists.FromDate)
                    //{
                    mdlChannelIndent.ID = mdlChannelIndnetExists.ID;
                    bllIndents.UpdateIndent(mdlChannelIndent);
                    //}
                }
                else
                {
                    bllIndents.AddIndnet(mdlChannelIndent);
                }

                CO_ChannelIrrigationBoundaries mdlChannelIB = bllIndents.GetSectionBySubDivIDAndChannelID(SubDivID, ChannelID);
                SectionID = Convert.ToInt64(mdlChannelIB.SectionID);
                CO_ChannelGauge mdlChannelGauge = bllIndents.GetDesignDischargeBySectionIDAndChannelID(SectionID, ChannelID);
                if (mdlChannelGauge == null)
                {
                    DesignDischarge = 0;
                }
                else
                {
                    DesignDischarge = mdlChannelGauge.DesignDischarge;
                }
                double DesignDischargePercentage = Convert.ToDouble((DesignDischarge * 115) / 100);
                if (Convert.ToDouble(txtIndent.Text) > DesignDischargePercentage)
                {
                    Master.ShowMessage(Message.IndentValueMoreThanDesignDischarge.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.IndentSaved.Description, SiteMaster.MessageType.Success);
                }

            }
            //if (Convert.ToDateTime(txtPlacementDate.Text) > DateTime.Now)
            //{
            //    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
            //    return;
            //}
            //long SectionID;
            //double? DesignDischarge;

            //CO_ChannelIndent mdlChannelIndent = new CO_ChannelIndent();
            //IndentsBLL bllIndent = new IndentsBLL();
            //ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
            //SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);


            //List<long> lstSubDivision = bllIndent.GetListOfSubDivisions(ChannelID);

            //int ListIndex = lstSubDivision.FindIndex(a => a == SubDivID);

            //double CurrentIndent;
            //double LowerSubdivIndent;

            //CO_ChannelIndent mdlChannelIndentCurrent = bllIndent.GetIndentBySubDivIDandChannelID(ChannelID, SubDivID);
            //if (mdlChannelIndentCurrent != null)
            //{
            //    CurrentIndent = mdlChannelIndentCurrent.IndentValue;
            //}
            //else
            //{
            //    CurrentIndent = 0.0;
            //}


            //long LowerSubDiv = bllIndent.GetLowerSubDivision(ChannelID, SubDivID);

            //CO_ChannelIndent mdlChannelIndentLower = bllIndent.GetIndentBySubDivIDandChannelID(ChannelID, LowerSubDiv);
            //if (mdlChannelIndentLower != null)
            //{
            //    LowerSubdivIndent = mdlChannelIndentLower.IndentValue;
            //}
            //else
            //{
            //    LowerSubdivIndent = 0.0;
            //}


            //double IndentValue = Convert.ToDouble(txtIndent.Text) - CurrentIndent;
            //double IndentValueToBeAdded = 0;

            //long SubDivisonFromList;
            //double CurrentIndentInLoop;


            //for (int i = ListIndex; i >= 0; i--)
            //{
            //    SubDivisonFromList = lstSubDivision.ElementAt(i);
            //    CO_ChannelIndent mdlChannelIndentCurrentInLoop = bllIndent.GetIndentBySubDivIDandChannelID(ChannelID, SubDivisonFromList);
            //    if (mdlChannelIndentCurrentInLoop != null)
            //    {
            //        CurrentIndentInLoop = mdlChannelIndentCurrentInLoop.IndentValue;
            //    }
            //    else
            //    {
            //        CurrentIndentInLoop = 0.0;
            //    }

            //    IndentValueToBeAdded = CurrentIndentInLoop + IndentValue;

            //    mdlChannelIndent.ChannelID = ChannelID;
            //    mdlChannelIndent.SubDivID = lstSubDivision.ElementAt(i);
            //    mdlChannelIndent.FromDate = Convert.ToDateTime(txtPlacementDate.Text);
            //    mdlChannelIndent.IndentValue = IndentValueToBeAdded;
            //    if (i == ListIndex)
            //    {
            //        mdlChannelIndent.IsAutoGenerated = false;
            //        mdlChannelIndent.Remarks = txtRemarks.Text;
            //    }
            //    else
            //    {
            //        mdlChannelIndent.IsAutoGenerated = true;
            //        mdlChannelIndent.Remarks = "";
            //    }


            //    DateTime dtMax = Convert.ToDateTime(bllIndent.GetMaxDateByChannelID(ChannelID, lstSubDivision.ElementAt(i)));

            //    CO_ChannelIrrigationBoundaries mdlChannelIB = bllIndent.GetSectionBySubDivIDAndChannelID(SubDivID, ChannelID);
            //    SectionID = Convert.ToInt64(mdlChannelIB.SectionID);

            //    CO_ChannelGauge mdlChannelGauge = bllIndent.GetDesignDischargeBySectionIDAndChannelID(SectionID, ChannelID);
            //    if (mdlChannelGauge == null)
            //    {
            //        DesignDischarge = 0;
            //    }
            //    else
            //    {
            //        DesignDischarge = mdlChannelGauge.DesignDischarge;
            //    }


            //    if (SubDivID == lstSubDivision.ElementAt(i))
            //    {

            //        if (mdlChannelIndentCurrent.FromDate > Convert.ToDateTime(txtPlacementDate.Text))
            //        {
            //            Master.ShowMessage(Message.SelectedDateIsLessThanCurrentIndentDate.Description, SiteMaster.MessageType.Error);
            //            return;
            //        }
            //        //else if (mdlChannelIndentCurrent.FromDate == Convert.ToDateTime(txtPlacementDate.Text) && mdlChannelIndentCurrent.IsAutoGenerated == false)
            //        else if (bllIndent.GetIndentWhereNotAutoGenerated(ChannelID, SubDivID, Convert.ToDateTime(txtPlacementDate.Text)))
            //        {
            //            Master.ShowMessage(Message.DateDuplication.Description, SiteMaster.MessageType.Error);
            //            return;
            //        }
            //        else if (Convert.ToDouble(txtIndent.Text) < LowerSubdivIndent)
            //        {
            //            Master.ShowMessage(Message.IndentNotSaved.Description, SiteMaster.MessageType.Error);
            //            return;
            //        }
            //    }

            //    bllIndent.AddIndnet(mdlChannelIndent);

            //    if (SubDivID == lstSubDivision.ElementAt(i))
            //    {


            //        double DesignDischargePercentage = Convert.ToDouble((DesignDischarge * 115) / 100);
            //        if (Convert.ToDouble(txtIndent.Text) > DesignDischargePercentage)
            //        {
            //            Master.ShowMessage(Message.IndentValueMoreThanDesignDischarge.Description, SiteMaster.MessageType.Success);
            //        }
            //        else
            //        {
            //            Master.ShowMessage(Message.IndentSaved.Description, SiteMaster.MessageType.Success);
            //        }


            //        btnSave.Enabled = false;
            //    }

            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AddIndents();
                btnSave.Enabled = false;
            }
            catch (Exception Exp)
            {
                Master.ShowMessage(Message.IndentNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lnkBtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Modules/DailyData/SearchForPlacingIndents.aspx?ShowHistory=true");
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}