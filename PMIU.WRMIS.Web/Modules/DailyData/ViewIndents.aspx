<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewIndents.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.ViewIndents" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>View Indents</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblChannel" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control required" AutoPostBack="True" required="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvIndents" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowFooter="true"
                            ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnRowDataBound="gvIndents_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Direct Offtake">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDirectOfftakes" runat="server" CssClass="control-label" Text='<%# Eval("CO_Channel.NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Offtakes Total Indents" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label Text="Direct Outlets Total Indent" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label ID="lblPercent" Text="" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label Text="Indent At Sub-Divisional Gauge" runat="server" Visible="true" /></b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Offtake Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChannelType" runat="server" CssClass="control-label" Text='<%# Eval("CO_Channel.CO_ChannelType.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Parent RD (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParrentRD" runat="server" CssClass="control-label" Text='<%# Eval("ChannelRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Indent Placement Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfftakeIndentDate" runat="server" CssClass="control-label" Text='<%# Eval("OfftakeIndentDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="Indent Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndentTime" runat="server" CssClass="control-label" Text='<%# Eval("OfftakeIndentDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-CssClass="numberAlign" FooterStyle-CssClass="numberAlign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblOfftakesTotalIndents" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label ID="lblDirectOutletsTotalIndent" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label ID="lblIncrementedIndentAt10P" runat="server" Visible="true" /></b><br>
                                        <br>
                                        <b>
                                            <asp:Label ID="lblIndentAtSubDivisionalGauge" runat="server" Visible="true" /></b>
                                    </FooterTemplate>

                                    <HeaderStyle CssClass="col-md-1 numberAlign" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Remarks") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlIndent" runat="server" CssClass="btn btn-primary btn_24 indents" NavigateUrl='<%# String.Format("~/Modules/DailyData/IndentsHistory.aspx?OffTakeID={0}&Date={1}&Page={2}", Convert.ToString(Eval("ChannelID")),Convert.ToDateTime(Eval("OfftakeIndentDate")).ToString("MM/dd/yyyy"),false) %>' ToolTip="Details"></asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" />
                                <%--<asp:HyperLink ID="btnPlaceIndent" Text="Place Indent" CssClass="btn btn-success" runat="server"></asp:HyperLink>
                                <asp:LinkButton ID="btnReset" runat="server" CssClass="btn" Text="Reset"/>--%>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
