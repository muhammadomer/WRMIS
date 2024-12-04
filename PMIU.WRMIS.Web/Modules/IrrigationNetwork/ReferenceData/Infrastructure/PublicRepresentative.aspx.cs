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
  public partial class PublicRepresentative : BasePage
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
            BindInfrastructureRepresentativeGridView(infrastructureID);
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
    private void BindInfrastructureRepresentativeGridView(long _InfrastructureID)
    {
      try
      {
        List<object> lstStoneStock = new InfrastructureBLL().GetRepresentativeByInfrastructureID(_InfrastructureID);
        gvInfrastructureRepresentative.DataSource = lstStoneStock;
        gvInfrastructureRepresentative.DataBind();
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      try
      {

        if (e.CommandName == "AddInfrastructureRepresentative")
        {
          List<object> lstRepresentative = new InfrastructureBLL().GetRepresentativeByInfrastructureID(Convert.ToInt64(hdnProtectionInfrastructureID.Value));

          lstRepresentative.Add(
          new
          {
            ID = 0,
            Name = string.Empty,
            Details = string.Empty,
            ContactNumber = string.Empty,
            CreatedDate = DateTime.Now,
            CreatedBy = string.Empty
          });

          gvInfrastructureRepresentative.PageIndex = gvInfrastructureRepresentative.PageCount;
          gvInfrastructureRepresentative.DataSource = lstRepresentative;
          gvInfrastructureRepresentative.DataBind();

          gvInfrastructureRepresentative.EditIndex = gvInfrastructureRepresentative.Rows.Count - 1;
          gvInfrastructureRepresentative.DataBind();

        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowDataBound(object sender, GridViewRowEventArgs e)
    {

      try
      {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          AddDeletionConfirmMessage(e);

          if (gvInfrastructureRepresentative.EditIndex == e.Row.RowIndex)
          {
            //if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            //AddEditConfirmMessage(e);

            #region "Data Keys"
            DataKey key = gvInfrastructureRepresentative.DataKeys[e.Row.RowIndex];
            string name = Convert.ToString(key.Values[1]);
            string detail = Convert.ToString(key.Values[2]);
            string contactNumber = Convert.ToString(key.Values[3]);
            #endregion

            #region "Controls"
            TextBox txtName = (TextBox)e.Row.FindControl("txtName");
            TextBox txtDetail = (TextBox)e.Row.FindControl("txtDetail");
            TextBox txtContactNumber = (TextBox)e.Row.FindControl("txtContactNumber");
            #endregion

            if (txtName != null)
            {
              if (!string.IsNullOrEmpty(name))
                txtName.Text = name;
            }

            if (txtDetail != null)
            {
              if (!string.IsNullOrEmpty(detail))
                txtDetail.Text = detail;
            }

            if (txtContactNumber != null)
            {
              if (!string.IsNullOrEmpty(contactNumber))
                txtContactNumber.Text = contactNumber;
            }  

          }
        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowEditing(object sender, GridViewEditEventArgs e)
    {
      try
      {
        gvInfrastructureRepresentative.EditIndex = e.NewEditIndex;
        BindInfrastructureRepresentativeGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      try
      {
        gvInfrastructureRepresentative.EditIndex = -1;
        BindInfrastructureRepresentativeGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      try
      {
        UA_Users mdlUser = SessionManagerFacade.UserInformation;
        GridViewRow row = gvInfrastructureRepresentative.Rows[e.RowIndex];
        
        #region "Controls"
        TextBox txtName = (TextBox)row.FindControl("txtName");
        TextBox txtDetail = (TextBox)row.FindControl("txtDetail");
        TextBox txtContactNumber = (TextBox)row.FindControl("txtContactNumber");
        #endregion
               
        #region "Datakeys"
        DataKey key = gvInfrastructureRepresentative.DataKeys[e.RowIndex];
        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
        #endregion

        string infrastructureRepresentativeID = Convert.ToString(gvInfrastructureRepresentative.DataKeys[e.RowIndex].Values[0]);

        FO_InfrastructureRepresentative infrastructureRepresentative = new FO_InfrastructureRepresentative();

        infrastructureRepresentative.ID = Convert.ToInt64(infrastructureRepresentativeID);
        infrastructureRepresentative.ProtectionInfrastructureID = Convert.ToInt32(hdnProtectionInfrastructureID.Value);

        if (txtName != null)
          infrastructureRepresentative.Name = txtName.Text;

        if (txtDetail != null && txtDetail.Text !="")
          infrastructureRepresentative.Details = txtDetail.Text;

        if (txtContactNumber != null)
          infrastructureRepresentative.ContactNumber = txtContactNumber.Text;
        
        //if (new InfrastructureBLL().IsStoneStockFromRDToRDExits(infrastructureStoneStock))
        //{
        //  Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //  return;
        //}

        if (infrastructureRepresentative.ID == 0)
        {
          infrastructureRepresentative.CreatedBy = Convert.ToInt32(mdlUser.ID);
          infrastructureRepresentative.CreatedDate = DateTime.Now;
        }
        else
        {
          infrastructureRepresentative.CreatedBy = Convert.ToInt32(CreatedBy);
          infrastructureRepresentative.CreatedDate = Convert.ToDateTime(CreatedDate);
          infrastructureRepresentative.ModifiedBy = Convert.ToInt32(mdlUser.ID);
          infrastructureRepresentative.ModifiedDate = DateTime.Now;
        }

        bool IsSave = new InfrastructureBLL().SaveInfrastructureRepresentative(infrastructureRepresentative);

        if (IsSave)
        {
          // Redirect user to first page.
          if (Convert.ToInt64(infrastructureRepresentativeID) == 0)
            gvInfrastructureRepresentative.PageIndex = 0;

          gvInfrastructureRepresentative.EditIndex = -1;
          BindInfrastructureRepresentativeGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
          Master.ShowMessage(Message.RecordSaved.Description);
        }

      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      try
      {
        string stoneStockID = Convert.ToString(gvInfrastructureRepresentative.DataKeys[e.RowIndex].Values[0]);

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

        bool IsDeleted = new InfrastructureBLL().DeleteInfrastructureRepresentative(Convert.ToInt64(stoneStockID));
        if (IsDeleted)
        {
          BindInfrastructureRepresentativeGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
          Master.ShowMessage(Message.RecordDeleted.Description);
        }
      }
      catch (Exception ex)
      {
        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
      }
    }
    protected void gvInfrastructureRepresentative_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      try
      {
        gvInfrastructureRepresentative.PageIndex = e.NewPageIndex;
        gvInfrastructureRepresentative.EditIndex = -1;
        BindInfrastructureRepresentativeGridView(Convert.ToInt64(hdnProtectionInfrastructureID.Value));
      }
      catch (Exception exp)
      {
        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
      }

    }

    #endregion "End GridView Events"

    private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
    {
      Button btnDelete = (Button)_e.Row.FindControl("btnDeleteInfrastructureRepresentative");

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