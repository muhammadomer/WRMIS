<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchIrrigator.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.SearchIrrigator" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                Search Irrigator
            </h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblChannelName" runat="server" Text="Channel Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlChannelName" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblIrrigatorStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlIrrigatorStatus" runat="server" CssClass="form-control">
                                        <%--<asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="a">Active</asp:ListItem>
                                        <asp:ListItem Value="i">InActive</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblIrrigatorMobileNo" runat="server" Text="Mobile No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtIrrigatorMobileNo" runat="server" CssClass="form-control phoneNoInput" placeholder="XXXXXXXXXXX" onkeyup="PhoneNoWithLengthValidation(this, 11)" MaxLength="20" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                <asp:HyperLink ID="hlAddIrrigator" runat="server" CssClass="btn btn-success" Text="Add New Irrigator"></asp:HyperLink>
                                <%--<asp:HyperLink ID="hlAddIrrigator" runat="server" CssClass="btn btn-primary" Text="Add New Irrigator" NavigateUrl='<%# String.Format("~/Modules/IrrigatorsFeedback/AddIrrigator.aspx") %>'></asp:HyperLink>--%>
                            </div>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvIrrigator" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" AllowPaging="true"
                         CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" PageSize="10"
                            OnRowEditing="gvIrrigator_RowEditing" OnRowDataBound="gvIrrigator_RowDataBound"
                            OnPageIndexChanged="gvIrrigator_PageIndexChanged" OnPageIndexChanging="gvIrrigator_PageIndexChanging">
                            <Columns>
                                 <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIrrigatorID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvZone" runat="server" CssClass="control-label" Text='<%# Eval("CO_Division.CO_Circle.CO_Zone.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvCircle" runat="server" CssClass="control-label" Text='<%# Eval("CO_Division.CO_Circle.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvDivision" runat="server" CssClass="control-label" Text='<%# Eval("CO_Division.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Channel Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvChannel" runat="server" CssClass="control-label" Text='<%# Eval("CO_Channel.NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvIrrigatorName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvIrrigatorNumber" runat="server" CssClass="control-label" Text='<%# Eval("MobileNo1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvIrrigatorStatus" runat="server" CssClass="control-label" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                    <ItemTemplate>
                                        <%--<asp:Panel ID="pnlAction" runat="server" HorizontalAlign="center">--%>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"/>
                                        <%--</asp:Panel>--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
