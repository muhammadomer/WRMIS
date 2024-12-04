<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoneStock.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.StoneStock" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
  <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
  <asp:HiddenField ID="hdnProtectionInfrastructureID" runat="server" Value="0" />

  <div class="box">
    <div class="box-title">
      <h3>Stone Stock on Protection Infrastructure</h3>
      <div class="box-tool">
        <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
      </div>
    </div>
    <div class="box-content">
      <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />
      <div class="table-responsive">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:GridView ID="gvStoneStock" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,SanctionedLimit,CreatedBy,CreatedDate"
              CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
              EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
              OnRowCommand="gvStoneStock_RowCommand" OnRowDataBound="gvStoneStock_RowDataBound"
              OnRowEditing="gvStoneStock_RowEditing" OnRowCancelingEdit="gvStoneStock_RowCancelingEdit"
              OnRowUpdating="gvStoneStock_RowUpdating"
              OnRowDeleting="gvStoneStock_RowDeleting" OnPageIndexChanging="gvStoneStock_PageIndexChanging">
              <Columns>
                <asp:TemplateField HeaderText="From RD">
                  <ItemTemplate>
                    <asp:Label ID="lblIrrigationFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="txtFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                    +
                                        <asp:TextBox ID="txtFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                  </EditItemTemplate>
                  <ItemStyle CssClass="text-right"></ItemStyle>
                  <HeaderStyle CssClass="text-right col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To RD">
                  <ItemTemplate>
                    <asp:Label ID="lblIrrigationToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="txtToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                    +
                                        <asp:TextBox ID="txtToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                  </EditItemTemplate>
                  <ItemStyle CssClass="text-right"></ItemStyle>
                  <HeaderStyle CssClass="text-right col-md-1 col-md-offset-2" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Sanctioned Limit (‘000 cft)">
                  <ItemTemplate>
                    <asp:Label ID="lblSanctionedLimit" runat="server" Text='<%# Eval("SanctionedLimit") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="txtSanctionedLimit" runat="server" required="required" pattern="[0-9]{1,5}" MaxLength="5" class="integerInput required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                  </EditItemTemplate>
                  <ItemStyle CssClass="text-right"/>
                  <HeaderStyle CssClass="text-right col-md-2" />
                </asp:TemplateField>

                <asp:TemplateField>
                  <HeaderTemplate>
                    <asp:Panel ID="pnlAddStoneStock" runat="server" HorizontalAlign="Center">
                      <asp:Button ID="btnAddStoneStock" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddStoneStock" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                    </asp:Panel>
                  </HeaderTemplate>
                  <ItemTemplate>
                    <asp:Panel ID="pnlActionStoneStock" runat="server" HorizontalAlign="Center">
                      <asp:Button ID="btnEditStoneStock" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                      <asp:Button ID="btnDeleteStoneStock" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                    </asp:Panel>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:Panel ID="pnlEditActionStoneStock" runat="server" HorizontalAlign="Center">
                      <asp:Button runat="server" ID="btnSaveStoneStock" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                      <asp:Button ID="btnCancelStoneStock" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                    </asp:Panel>
                  </EditItemTemplate>                    
                  <ItemStyle CssClass="text-center"/>
                  <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
              </Columns>
              <PagerSettings Mode="NumericFirstLast" />
              <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
          </ContentTemplate>
        </asp:UpdatePanel>
      </div>
      <br />
      <%--onclick="history.go(-1);return false;"--%>
      <div class="form-group">
        <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    //On UpdatePanel Refresh
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
      prm.add_endRequest(function (sender, e) {
        if (sender._postBackSettings.panelsToUpdate != null) {
          InitilizeNumericValidation();
          AddInputTextboxAttributes();
        }
      });
    };

    function ConfirmIstoRecalculate() {
      var result = confirm('Hello');
      if (result) {
        //click ok button
        alert("testin true");
      }
      else {
        //click cancel button
        alert("testing false");
      }
    }
  </script>
</asp:Content>
