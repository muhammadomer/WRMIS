<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndentHistory.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.DailyData.IndentHistory" %>

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



                        <div class="table-responsive">
                            <table class="table tbl-info">

                                <tr>
                                    <th>Channel Name</th>
                                    <th>Channel Type</th>
                                    <th>Total RDs (ft)</th>
                                    <th></th>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblChannelName" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblChannelType" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblTotalRD" runat="server" Text="" /></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <th>Flow Type</th>
                                    <th>Command Name</th>
                                    <th>Division</th>
                                    <th></th>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblFlowType" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblCommandName" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblDivision" runat="server" Text="" /></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <th>Sub Division</th>
                                    <th>IMIS CODE</th>
                                    <th></th>
                                    <th></th>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblSubDivision" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblIMISCode" runat="server" Text="" /></td>
                                    <td></td>
                                    <td></td>
                                </tr>

                            </table>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblFromDate" runat="server" class="col-sm-4 col-lg-3 control-label">From Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblToDate" runat="server" class="col-sm-4 col-lg-3 control-label">To Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>

                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <br />
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" align="center" Text="Search" OnClick="btnSearch_Click"></asp:Button>
                                            <asp:LinkButton ID="lnkBtnBack" Text="Back" CssClass="btn" OnClick="lnkBtnBack_Click" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <br />

                                <div class="table-responsive">
                                    <asp:GridView ID="gvIndentHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True"
                                        OnPageIndexChanging="gvIndentHistory_PageIndexChanging" OnRowDataBound="gvIndentHistory_RowDataBound"
                                        AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="From Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent.FromDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="control-label" Text='<%# Eval("ToDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent (Cusec)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentValue" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent.IndentValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent.Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
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
            </div>
        </div>
    </div>

</asp:Content>
