<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Items.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.ReferenceData.Items" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <div class="box">
        <div class="box-title">
            <h3>Items</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12 font-size-13">
                            Item Category&nbsp;&nbsp;<asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="form-control radio-inline" Style="max-width: 20%;" AutoPostBack="True"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <br />
                        <asp:GridView ID="gvItems" runat="server" DataKeyNames="ID,Name,Description,MajorMinor,ItemCategoryID,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvItems_RowCommand" OnRowDataBound="gvItems_RowDataBound"
                            OnRowEditing="gvItems_RowEditing" OnRowCancelingEdit="gvItems_RowCancelingEdit"
                            OnRowUpdating="gvItems_RowUpdating"
                            OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Item Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" pattern="[a-zA-Z\-:].{3,90}" MaxLength="90" class="required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Major/Minor" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMajorMinor" runat="server" Text='<%# Eval("MajorMinor") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlMajorMinor" runat="server" required="required" CssClass="required form-control" Style="max-width: 90%;" AutoPostBack="True"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddItems" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddItems" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddItems" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionItems" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditItems" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteItems" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionItems" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveItems" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelItems" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
