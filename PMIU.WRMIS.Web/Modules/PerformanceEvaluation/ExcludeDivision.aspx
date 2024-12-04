<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExcludeDivision.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ExcludeDivision" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Exclude Division from Performance Evaluation</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlCircle" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" runat="server" TabIndex="2" AutoPostBack="true">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label" style="padding-left: 0px;">Excluded Divisions</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <label class="checkbox-inline" style="padding-top: 0px;">
                                            <asp:CheckBox runat="server" ID="chkExclude" Text=" " TabIndex="11" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button TabIndex="10" ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvExcludeDivision" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                        GridLines="None" DataKeyNames="DivisionID,IsExcluded" OnPageIndexChanging="gvExcludeDivision_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivisionID" runat="server" CssClass="control-label" Text='<%# Eval("DivisionID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Zone">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblZone" runat="server" CssClass="control-label" Text='<%# Eval("Zone") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Circle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCircle" runat="server" CssClass="control-label" Text='<%# Eval("Circle") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("Division") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Exclude" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div class="controls">
                                                        <asp:CheckBox ID="chkIsExcluded" runat="server" Checked='<%# Eval("IsExcluded") %>' />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1 text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button TabIndex="10" ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
