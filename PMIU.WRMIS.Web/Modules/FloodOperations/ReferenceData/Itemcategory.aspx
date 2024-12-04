<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Itemcategory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.ReferenceData.Itemcategory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <div class="box">
        <div class="box-title">
            <h3>Items category</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvItemCategory" runat="server" DataKeyNames="ID,Name,Description,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvItemCategory_RowCommand" OnRowDataBound="gvItemCategory_RowDataBound"
                            OnRowEditing="gvItemCategory_RowEditing" OnRowCancelingEdit="gvItemCategory_RowCancelingEdit"
                            OnRowUpdating="gvItemCategory_RowUpdating"
                            OnRowDeleting="gvItemCategory_RowDeleting" OnPageIndexChanging="gvItemCategory_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Item category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" pattern="[a-zA-Z\-:].{3,90}" MaxLength="90" class="required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="80" class="form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddItemCategory" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddItemCategory" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionItemCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditItemCategory" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteItemCategory" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionItemCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveItemCategory" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelItemCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
