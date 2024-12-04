<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyIndents.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.DailyIndents" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Indents</h3>

                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Sub Division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" CssClass="form-control required" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" required="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" required="required" CssClass="date-picker form-control required"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>

                                <!-- END Left Side -->
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblChannel" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Channel Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlChannel" CssClass="form-control" runat="server" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearchChannel" Text="Search" CssClass="btn btn-primary" runat="server" OnClick="btnSearchChannel_Click" />
                                    <asp:HyperLink ID="hlPlaceIndent" Text="Place Indent" CssClass="btn btn-success" runat="server" />
                                    <asp:LinkButton ID="btnReset" runat="server" CssClass="btn" Text="Reset" OnClick="btnReset_Click" />
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive">

                                    <asp:GridView ID="gvBySubDivision" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" Visible="false" OnRowDataBound="gvBySubDivision_RowDataBound"
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />

                                                <FooterTemplate>
                                                    <itemtemplate>
                                                        <b><asp:Label Text="Sub Divisional Total" runat="server" /></b>
                                                    </itemtemplate>
                                                </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-CssClass="numberAlign" FooterStyle-CssClass="numberAlign">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# (Convert.ToDouble(Eval("OutletIndent")) == null ? 0 : Convert.ToDouble(Eval("OutletIndent"))) +  (Convert.ToDouble(Eval("TotalOfftakeIndent")) == null ? 0 : Convert.ToDouble(Eval("TotalOfftakeIndent"))) +  (Convert.ToDouble(Eval("PercentageIncrementValue")) == null ? 0 : Convert.ToDouble(Eval("PercentageIncrementValue"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4 numberAlign" />

                                                <FooterTemplate>
                                                    <itemtemplate>
                                                        <b><asp:Label ID="lblSubDivisionalTotal" runat="server" /></b>
                                                    </itemtemplate>
                                                </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Placement Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentDate" runat="server" CssClass="control-label" Text='<%# Eval("IndentDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlIndent" runat="server" CssClass="btn btn-primary btn_24 indents" NavigateUrl='<%# String.Format("~/Modules/DailyData/ViewIndents.aspx?IndentID={0}&Date={1}&SubDivID={2}&ParentChannelID={3}", Convert.ToString(Eval("ID")),Convert.ToDateTime(Eval("IndentDate")).ToString("MM/dd/yyyy"),Convert.ToString(Eval("SubDivID")),Convert.ToString(Eval("ParentChannelID"))) %>' ToolTip="Details"></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>



                                    <asp:GridView ID="gvByChannel" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" Visible="false" OnRowDataBound="gvByChannel_RowDataBound"
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubDivision" runat="server" CssClass="control-label" Text='<%# Eval("SubDivName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-CssClass="numberAlign">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndents" runat="server" CssClass="control-label" Text='<%# (Convert.ToDouble(Eval("OutletIndent")) == null ? 0 : Convert.ToDouble(Eval("OutletIndent"))) +  (Convert.ToDouble(Eval("TotalOfftakeIndent")) == null ? 0 : Convert.ToDouble(Eval("TotalOfftakeIndent"))) +  (Convert.ToDouble(Eval("PercentageIncrementValue")) == null ? 0 : Convert.ToDouble(Eval("PercentageIncrementValue"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4 numberAlign" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Placement Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentDate" runat="server" CssClass="control-label" Text='<%# Eval("IndentDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlIndent" runat="server" CssClass="btn btn-primary btn_24 indents" NavigateUrl='<%# String.Format("~/Modules/DailyData/ViewIndents.aspx?IndentID={0}&Date={1}&SubDivID={2}&ParentChannelID={3}", Convert.ToString(Eval("ID")),Convert.ToDateTime(Eval("IndentDate")).ToString("MM/dd/yyyy"),Convert.ToString(Eval("SubDivID")),Convert.ToString(Eval("ParentChannelID"))) %>' ToolTip="Details"></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
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
            </div>
        </div>
    </div>
</asp:Content>
