using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
  public partial class StoneStock : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      long infrastructureID = 0;
      try
      {
        if (!IsPostBack)
        {
          
          infrastructureID = Utility.GetNumericValueFromQueryString("InfrastructureID", 0);

          if (infrastructureID > 0)
          {
            InfrastructureDetail.InfrastructureID = Convert.ToInt64(infrastructureID);
            hdnProtectionInfrastructureID.Value = Convert.ToString(infrastructureID);
            hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID={0}", infrastructureID);
            BindStoneStockGridView(infrastructureID);
            InfrastructureDetail.InfrastructureID = Convert.ToInt64(infrastructureID);
          }
        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }


    #region "GridView Events"
    private void BindStoneStockGridView(long _InfrastructureID)
    {
      try
      {
        List<object> lstStoneStock = new InfrastructureBLL().GetStoneStockByInfrastructureID(_InfrastructureID);
        gvStoneStock.DataSource = lstStoneStock;
        gvStoneStock.DataBind();
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      try
      {

        if (e.CommandName == "AddStoneStock")
        {
          List<object> lstStoneStock = new InfrastructureBLL().GetStoneStockByInfrastructureID(Convert.ToInt64(hdnProtectionInfrastructureID.Value));

          lstStoneStock.Add(
          new
          {
            ID = 0,
            FromRDTotal = string.Empty,
            ToRDTotal = string.Empty,
            FromRD = string.Empty,
            ToRD = string.Empty,
            SanctionedLimit = string.Empty,
            CreatedDate = DateTime.Now,
            CreatedBy = string.Empty
          });

          gvStoneStock.PageIndex = gvStoneStock.PageCount;
          gvStoneStock.DataSource = lstStoneStock;
          gvStoneStock.DataBind();

          gvStoneStock.EditIndex = gvStoneStock.Rows.Count - 1;
          gvStoneStock.DataBind();

        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      string FromLeftRD = string.Empty;
      string FromRightRD = string.Empty;
      string ToLeftRD = string.Empty;
      string ToRightRD = string.Empty;

      try
      {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          AddDeletionConfirmMessage(e);

          if (gvStoneStock.EditIndex == e.Row.RowIndex)
          {
            //if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            //AddEditConfirmMessage(e);

            #region "Data Keys"
            DataKey key = gvStoneStock.DataKeys[e.Row.RowIndex];
            string fromRD = Convert.ToString(key.Values[1]);
            string toRD = Convert.ToString(key.Values[2]);
            string sanctionedLimit = Convert.ToString(key.Values[3]);
            #endregion

            #region "Controls"
            TextBox txtFromRDLeft = (TextBox)e.Row.FindControl("txtFromRDLeft");
            TextBox txtFromRDRight = (TextBox)e.Row.FindControl("txtFromRDRight");
            TextBox txtToRDLeft = (TextBox)e.Row.FindControl("txtToRDLeft");
            TextBox txtToRDRight = (TextBox)e.Row.FindControl("txtToRDRight");
            TextBox txtSanctionedLimit = (TextBox)e.Row.FindControl("txtSanctionedLimit");
            #endregion

            if (txtSanctionedLimit != null)
            {
              if (!string.IsNullOrEmpty(sanctionedLimit))
                txtSanctionedLimit.Text = sanctionedLimit;
            }           

            // Check From RD is not null
            if (!string.IsNullOrEmpty(fromRD))
            {
              Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(fromRD));
              FromLeftRD = tupleFromRD.Item1;
              FromRightRD = tupleFromRD.Item2;
            }

            if (txtFromRDLeft != null)
              txtFromRDLeft.Text = FromLeftRD;
            if (txtFromRDRight != null)
              txtFromRDRight.Text = FromRightRD;

            // Check To RD is not null
            if (!string.IsNullOrEmpty(toRD))
            {
              Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(toRD));
              ToLeftRD = tupleToRD.Item1;
              ToRightRD = tupleToRD.Item2;
            }
            if (txtToRDLeft != null)
              txtToRDLeft.Text = ToLeftRD;
            if (txtToRDRight != null)
              txtToRDRight.Text = ToRightRD;

          }
        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowEditing(object sender, GridViewEditEventArgs e)
    {
      try
      {
        gvStoneStock.EditIndex = e.NewEditIndex;
        BindStoneStockGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      try
      {
        gvStoneStock.EditIndex = -1;
        BindStoneStockGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      try
      {
        UA_Users mdlUser = SessionManagerFacade.UserInformation;
        GridViewRow row = gvStoneStock.Rows[e.RowIndex];
        
        #region "Controls"
        TextBox txtFromRDLeft = (TextBox)row.FindControl("txtFromRDLeft");
        TextBox txtFromRDRight = (TextBox)row.FindControl("txtFromRDRight");
        TextBox txtToRDLeft = (TextBox)row.FindControl("txtToRDLeft");
        TextBox txtToRDRight = (TextBox)row.FindControl("txtToRDRight");
        TextBox txtSanctionedLimit = (TextBox)row.FindControl("txtSanctionedLimit");
        #endregion
               
        #region "Datakeys"
        DataKey key = gvStoneStock.DataKeys[e.RowIndex];
        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
        #endregion

        string stoneStockID = Convert.ToString(gvStoneStock.DataKeys[e.RowIndex].Values[0]);

        FO_InfrastructureStoneStock infrastructureStoneStock = new FO_InfrastructureStoneStock();

        infrastructureStoneStock.ID = Convert.ToInt64(stoneStockID);

       if (txtFromRDLeft != null & txtFromRDRight != null)
         infrastructureStoneStock.FromRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

        if (txtToRDLeft != null & txtToRDRight != null)
          infrastructureStoneStock.ToRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);

        infrastructureStoneStock.ProtectionInfrastructureID = Convert.ToInt32(hdnProtectionInfrastructureID.Value);

        if (txtSanctionedLimit != null)
          infrastructureStoneStock.SanctionedLimit = Convert.ToInt64(txtSanctionedLimit.Text);

        if (new InfrastructureBLL().IsStoneStockFromRDToRDExits(infrastructureStoneStock))
        {
          Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
          return;
        }

        if (infrastructureStoneStock.ID == 0)
        {
          infrastructureStoneStock.CreatedBy = Convert.ToInt32(mdlUser.ID);
          infrastructureStoneStock.CreatedDate = DateTime.Now;
        }
        else
        {
          infrastructureStoneStock.CreatedBy = Convert.ToInt32(CreatedBy);
          infrastructureStoneStock.CreatedDate = Convert.ToDateTime(CreatedDate);
          infrastructureStoneStock.ModifiedBy = Convert.ToInt32(mdlUser.ID);
          infrastructureStoneStock.ModifiedDate = DateTime.Now;
        }

        if (infrastructureStoneStock.FromRD >= infrastructureStoneStock.ToRD)
        {
          Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
          return;
        }

        //else if (new InfrastructureBLL().IsSectionRDsExists(irrigationBoundaries))
        //{
        //  Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //  return;
        //}
        //else if (ChannelDetails.ChannelTotalRDs < irrigationBoundaries.SectionRD)
        //{
        //  Master.ShowMessage("Section RD can not be greater than Channel Total RDs.", SiteMaster.MessageType.Error);
        //  return;
        //}

        //if (Convert.ToInt16(hdnDependanceExists.Value.ToString()) == 1)
        //{
        //  Master.ShowMessage("Physical location can not be edited.", SiteMaster.MessageType.Error);
        //  return;
        //}

        // Update IsCalculated bit in Channel table to recalculate Gauges
        //if (!new ChannelBLL().UpdateIsCalculated(Convert.ToInt64(hdnChannelID.Value), true))
        //{
        //  Master.ShowMessage("Internal server error occured.", SiteMaster.MessageType.Error);
        //  return;
        //}

        bool IsSave = new InfrastructureBLL().SaveInfrastructureStoneStock(infrastructureStoneStock);

        if (IsSave)
        {
          // Redirect user to first page.
          if (Convert.ToInt64(stoneStockID) == 0)
            gvStoneStock.PageIndex = 0;

          gvStoneStock.EditIndex = -1;
          BindStoneStockGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
          Master.ShowMessage(Message.RecordSaved.Description);
        }

      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      try
      {
        string stoneStockID = Convert.ToString(gvStoneStock.DataKeys[e.RowIndex].Values[0]);

        //if (Convert.ToInt16(hdnDependanceExists.Value.ToString()) == 1)
        //{
        //  Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
        //  return;
        //}

        //// Update IsCalculated bit in Channel table to recalculate Gauges
        //if (!new ChannelBLL().UpdateIsCalculated(Convert.ToInt64(hdnChannelID.Value), true))
        //{
        //  Master.ShowMessage("Internal server error occured.", SiteMaster.MessageType.Error);
        //  return;
        //}

        bool IsDeleted = new InfrastructureBLL().DeleteInfrastructureStoneStockByID(Convert.ToInt64(stoneStockID));
        if (IsDeleted)
        {
          BindStoneStockGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
          Master.ShowMessage(Message.RecordDeleted.Description);
        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvStoneStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      try
      {
        gvStoneStock.PageIndex = e.NewPageIndex;
        gvStoneStock.EditIndex = -1;
        BindStoneStockGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception exp)
      {
        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
      }

    }

    #endregion "End GridView Events"

    private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
    {
      Button btnDelete = (Button)_e.Row.FindControl("btnDeleteStoneStock");

      //if (btnDelete != null && Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
      //{
      //  btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this record?')) {return confirm('All data would be deleted.')} else return false;";
      //}
      //else 
      if (btnDelete != null)//&& Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 0)
      {
        btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
      }

    }

  }
}