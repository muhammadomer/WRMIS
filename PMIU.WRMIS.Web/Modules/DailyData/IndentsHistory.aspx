<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndentsHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.IndentsHistory" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Indents History</h3>

                </div>
                <div class="box-content">

                    <div class="form-horizontal">


                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvIndentHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True"
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                        OnRowDataBound="gvIndentHistory_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("CO_Channel.NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Placement Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOfftakeIndentDate" runat="server" CssClass="control-label" Text='<%# Eval("OfftakeIndentDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Indent" ItemStyle-CssClass="numberAlign">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelIndent" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4 numberAlign" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent.Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" OnClick="btnBack_Click" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
