<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gauges.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.Gauges" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <asp:HiddenField ID="hdnProtectionInfrastructureID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Gauges of Protection Infrastructure</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h3>Gauges</h3>
                        <asp:GridView ID="gvGauges" runat="server" DataKeyNames="ID,GaugesTypeID,GaugesTypeName,GaugeRD,GaugeRDTotal,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvGauges_RowCommand" OnRowDataBound="gvGauges_RowDataBound"
                            OnRowEditing="gvGauges_RowEditing" OnRowCancelingEdit="gvGauges_RowCancelingEdit"
                            OnRowUpdating="gvGauges_RowUpdating" OnRowDeleting="gvGauges_RowDeleting" OnPageIndexChanging="gvGauges_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Gauge Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeType" runat="server" Text='<%# Eval("GaugesTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGaugeType" runat="server" CssClass="form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <%--<ItemStyle Width="130px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gauge RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeRD" runat="server" Text='<%# Eval("GaugeRD")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtGaugeRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtGaugeRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <%--<HeaderStyle Width="140px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddGauges" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddGaugesInformation" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditGauges" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteGauges" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveGauges" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelGauges" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
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
    </script>
</asp:Content>
