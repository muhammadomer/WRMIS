<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VillagesBenefittedSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.VillagesBenefittedSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDamChannels" TagName="DamChannels" Src="~/Modules/SmallDams/Controls/DamChannels.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnVillageBenefittedID" runat="server" Value="0" />
     <asp:HiddenField ID="hdnSmallDamID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnVillageID" runat="server" Value="0" />


    <div class="box">
        <div class="box-title">
            <h3>Villages Benefitted</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucDamChannels:DamChannels runat="server" ID="DamChannels1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvVillages" runat="server" DataKeyNames="ID,DivisionID,DivisionName,DistrictID,DistrictName,TehsilID,TehsilName,VillageID,VillageName,CreatedBy,CreatedDate" 
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvVillages_RowCommand" OnRowDataBound="gvVillages_RowDataBound"
                            OnRowEditing="gvVillages_RowEditing" OnRowCancelingEdit="gvVillages_RowCancelingEdit"
                            OnRowUpdating="gvVillages_RowUpdating"
                            OnRowDeleting="gvVillages_RowDeleting" OnPageIndexChanging="gvVillages_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="lblID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("DivisionName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDivision" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="District">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrict" runat="server"  CssClass="control-label" Text='<%# Eval("DistrictName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDistrict" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tehsil">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTehsil" runat="server" CssClass="control-label" Text='<%# Eval("TehsilName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTehsil" AutoPostBack="true" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Villages Benefitted">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVillage" runat="server" CssClass="control-label" Text='<%# Eval("VillageName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlVillage"  runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddVillage" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddVillage" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddVillage" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionVillage" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditVillage" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteVillage" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionVillage" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveVillage" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelVillage" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
                                    <asp:HyperLink ID="hlBack" runat="server"  CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
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
