<%@ Page Title="SearchSD" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.SearchSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Dam Search</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDamName" runat="server" Text="Dam Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDamName" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnAddNew" runat="server" CssClass="btn btn-success" Text="Add New" OnClick="btnAddNew_Click" />
                                    <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/Modules/SmallDams/DamSearch/AddNewDamSD.aspx" CssClass="btn btn-success">&nbsp;Add new</asp:HyperLink>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDamSearch" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            DataKeyNames="ID,Division,SubDivision,DamName,DamType,Status,TechParaID"
                            ShowHeaderWhenEmpty="True" AllowPaging="True"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false"
                            OnPageIndexChanging="gvDamSearch_PageIndexChanging"
                            OnPageIndexChanged="gvDamSearch_PageIndexChanged"
                            OnRowDeleting="gvDamSearch_RowDeleting"
                            OnRowCommand="gvDamSearch_RowCommand"
                            OnRowDataBound="gvDamSearch_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubDivision" runat="server" Text='<%#Eval("SubDivision") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dam Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDamName" runat="server" Text='<%#Eval("DamName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dam Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDamType" runat="server" Text='<%#Eval("DamType") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" width="5%"/>
                                    <ItemStyle CssClass="text-left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlTecParameters" runat="server" ToolTip="Technical Parameters" CssClass="btn btn-primary btn_24 Technical_Parameters_32x32" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/TechnicalParametersSD.aspx?SmallDamID={0}&TechParaID={1}",Eval("ID"), Eval("TechParaID")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlChannels" runat="server" ToolTip="Channels" CssClass="btn btn-primary btn_24 channels_32x32" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/ChannelsSD.aspx?SmallDamID={0}",Eval("ID")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlOandMCost" runat="server" ToolTip="Annual Operation and Maintenance Cost" CssClass="btn btn-primary btn_24 Annual_maintane" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/AnnualOandMCostSD.aspx?SmallDamID={0}",Eval("ID")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:Button ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditDam" CommandArgument='<%# Eval("ID") %>'></asp:Button>
                                        <asp:Button ID="btnDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
