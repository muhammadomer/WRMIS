<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetAllocation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.AssetAllocation" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucResourceAllocationData" TagName="ResourceAllocationData" Src="~/Modules/Accounts/Controls/ResourceAllocationData.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnResourceAllocationID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Assets Allocation</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucResourceAllocationData:ResourceAllocationData runat="server" ID="ResourceAllocationData1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvAssetAllocation" runat="server"
                            DataKeyNames="ID,CategoryID,CategoryName,SubCategoryID,SubCategoryName,AssetTypeID,AssetTypeName,AssetItemID,AssetItemName,AssetAttributeID,AssetAttributeName,AssetAttributeValue,CreatedBy,CreatedDate,ResourceAllocationID,AssetType,LotQuantity"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvAssetAllocation_RowCommand" OnRowDataBound="gvAssetAllocation_RowDataBound"
                            OnRowEditing="gvAssetAllocation_RowEditing" OnRowCancelingEdit="gvAssetAllocation_RowCancelingEdit"
                            OnRowUpdating="gvAssetAllocation_RowUpdating"
                            OnRowDeleting="gvAssetAllocation_RowDeleting" OnPageIndexChanging="gvAssetAllocation_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="lblID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" CssClass="control-label" Text='<%# Eval("CategoryName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" runat="server" required="required" CssClass="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCategory" runat="server" CssClass="control-label" Text='<%# Eval("SubCategoryName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlSubCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Asset Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetType" runat="server" CssClass="control-label" Text='<%# Eval("AssetTypeName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAssetType" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged" runat="server" required="required" CssClass="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Assets Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsset" runat="server" CssClass="control-label" Text='<%# Eval("AssetItemName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAsset" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsset_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Asset Attribute ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetAttribute" runat="server" CssClass="control-label" Text='<%# Eval("AssetAttributeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAssetAttribute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetAttribute_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        <asp:Label ID="lblQuantity" runat="server" Visible="false" CssClass="control-label" Text="Quantity"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Attribute Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetAttributeValue" runat="server" CssClass="control-label" Text='<%# Eval("AssetAttributeValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblAssetAttributeValue" runat="server" CssClass="control-label" Text='<%# Eval("AssetAttributeValue") %>'></asp:Label>
                                        <asp:TextBox ID="txtAssetAttributeValue" runat="server" MaxLength="8" CssClass="form-control required" required="true" Text='<%#Eval("AssetAttributeValue")%>' pattern="^[0-9]*$" Visible="false" />
                                        <asp:Label ID="lblLotValue" runat="server" Text="" Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddAsset" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddAsset" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddAsset" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionAsset" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditAsset" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteAsset" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionAsset" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveAsset" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelAsset" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--onclick="history.go(-1);return false;"--%>
            <%--<div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>

</asp:Content>
