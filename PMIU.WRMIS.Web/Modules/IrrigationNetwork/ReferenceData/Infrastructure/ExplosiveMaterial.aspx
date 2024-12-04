<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExplosiveMaterial.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.ExplosiveMaterial" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnProtectioninfrastructure" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBreachingSectionId" runat="server" Value="0" />
    <div class="box">

        <div class="box-content">
            <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />

            <div class="row">
                <div class="col-md-4"><strong>Reach RD Start</strong></div>
                <div class="col-md-4"><strong>Reach RD End</strong></div>
                <div class="col-md-4"><strong>No. of Rows</strong></div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="lblReachRDStart" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblReachRDEnd" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblNoOfRows" runat="server"></asp:Label>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4"><strong>No. of Lines</strong></div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="lblNoOfLines" runat="server"></asp:Label>
                </div>
            </div>
            <%--            <asp:Table ID="tblBreachingSectionDetail" runat="server" CssClass="table tbl-info">
                <asp:TableRow>
                    <asp:TableHeaderCell>Reach R.D Start</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Reach R.D End</asp:TableHeaderCell>
                    <asp:TableHeaderCell>No. of Rows</asp:TableHeaderCell>
                    <asp:TableHeaderCell>No. of Lines</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblReachRDStart" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblReachRDEnd" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblNoOfRows" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblNoOfLines" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>--%>
            <br />
            <div class="box-tool">
                <h4 style="display: inline">Explosive Material/Accessoriess</h4>
                <%--<span style="color: red; margin-left: 42%">Note: Mention units with Total Requirement</span>--%>
                <div class="box-tool">
                    <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="udpExplosiveMaterial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvExplosiveMaterial" runat="server" AutoGenerateColumns="false" CssClass="table header" GridLines="None" ShowHeaderWhenEmpty="true" DataKeyNames="ID,Custody,CustodyName,Quantity,LocationDescription,CreatedBy,CreatedDate"
                                    AllowSorting="false" AllowPaging="True" EmptyDataText="No record found" OnRowCommand="gvExplosiveMaterial_RowCommand" OnRowDeleting="gvExplosiveMaterial_RowDeleting"
                                    OnRowEditing="gvExplosiveMaterial_RowEditing" OnRowUpdating="gvExplosiveMaterial_RowUpdating" OnRowCancelingEdit="gvExplosiveMaterial_RowCancelingEdit"
                                    OnRowDataBound="gvExplosiveMaterial_RowDataBound" OnPageIndexChanging="gvExplosiveMaterial_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Custody">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustody" runat="server" Text='<%# Eval("CustodyName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlCustody" runat="server" required="required" CssClass="required form-control" Style="max-width: 80%;">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Requirement (Quantity with Units)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtQuantity" runat="server" required="required" CssClass="required form-control" MaxLength="50" Style="max-width: 70%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                            <%--<ItemStyle CssClass="text-center" />--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationDescription" runat="server" Text='<%# Eval("LocationDescription")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLocationDescription" runat="server" CssClass="form-control" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                    <asp:Button ID="Add" runat="server" Text="" CommandName="Add" Visible="<%# base.CanAdd%>" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlActionExplosiveMaterial" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEditExplosiveMaterial" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit%>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                    <asp:Button ID="lbtnDeleteExplosiveMaterial" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="<%# base.CanDelete%>"
                                                        OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditActionExplosiveMaterial" runat="server" HorizontalAlign="Center">
                                                    <asp:Button runat="server" ID="btnSaveExplosiveMaterialInfo" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                                    <asp:Button ID="lbtnCancelExplosiveMaterialInfo" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
    </script>
</asp:Content>
