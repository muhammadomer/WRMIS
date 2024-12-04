<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetsAttributes.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData.AssetsAttributes" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSubCategID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSubCategName" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Asset Attributes</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="tbl-info">
                <div class="row">
                    <div class="col-md-6">
                        <asp:Label ID="Label1" runat="server" Text="Asset Category" Font-Bold="true"></asp:Label>

                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label2" runat="server" Text="Asset Sub-Category" Font-Bold="true"></asp:Label>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="lblCategory" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblSubcategory" runat="server"></asp:Label>
                    </div>

                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvAssetAttribute" runat="server" DataKeyNames="ID,SubCategoryID,AttributeName,AttributeDataType,Description,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvAssetAttribute_RowCommand" OnRowDataBound="gvAssetAttribute_RowDataBound"
                            OnRowEditing="gvAssetAttribute_RowEditing" OnRowCancelingEdit="gvAssetAttribute_RowCancelingEdit"
                            OnRowUpdating="gvAssetAttribute_RowUpdating"
                            OnRowDeleting="gvAssetAttribute_RowDeleting" OnPageIndexChanging="gvAssetAttribute_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Attribute Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("AttributeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" pattern="[a-zA-Z()!-@#$%^&* ].{2,30}" MaxLength="30" class="required form-control"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attribute Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttributeType" runat="server" Text='<%# Eval("AttributeDataType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAttributeType" runat="server" required="required" CssClass="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" class="form-control"  Style="max-width: 100%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="ChkActive" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>

                                        <asp:Panel ID="pnlItemAtrribute" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddAtrribute" runat="server" Text="" CommandName="AddAtrribute" Visible="<%# base.CanAdd %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionAtrribute" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditAtrribute" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteAtrribute" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionAtrribute" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveAtrribute" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelAtrribute" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
