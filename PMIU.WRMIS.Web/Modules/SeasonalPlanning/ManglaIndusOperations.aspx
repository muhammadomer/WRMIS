<%@ Page Title="Seasonal Planning" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ManglaIndusOperations.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ManglaIndusOperations" %>


<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .SPtable > tbody > tr > td {
            padding: 3px;
        }
    </style>

    <div class="box">
        <div class="box-title">
            <h3 id="htitle" runat="server">Seasonal Planning</h3>
        </div>
        <div class="box-content">

            <div id="divPanel" class="panel panel-default" runat="server">
                <div id="Tabs" role="tabpanel">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li id="liBalance" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/SeasonalPlanning.aspx" id="anchBalance" runat="server" onserverclick="anchBalance_ServerClick" role="tab">Balance Reservoir</a></li>
                        <li id="liManglaOperations" style="width: 16%; text-align: center" runat="server" class="active"><a href="~/Modules/SeasonalPlanning/ManglaIndusOperations.aspx" id="anchDivision" runat="server" role="tab">Mangla Operations</a></li>
                        <li id="liIndusOperations" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/IndusOperations.aspx" id="a1" runat="server" role="tab">Indus Operations</a></li>
                        <li id="liAnticipatedJC" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedJC.aspx" id="a2" runat="server" role="tab">Anticipated JC</a></li>
                        <li id="liAnticipatedIndus" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusRiver.aspx" id="a3" runat="server" role="tab">Anticipated Indus River</a></li>
                        <li id="liAnticipatedBasin" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusBasin.aspx" id="a4" runat="server" role="tab">Anticipated Indus Basin</a></li>
                    </ul>
                </div>
            </div>

            <div class="table-big" style="border: 0px;">
                <asp:GridView ID="gvMangla" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                    ShowHeaderWhenEmpty="True" CssClass="table header SPtable" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowDataBound="gvMangla_RowDataBound">
                    <Columns>
                        <%--<asp:TemplateField Visible="false">
                        <ItemTemplate>                            
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-1" />
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                <asp:Label ID="lblPeriod" runat="server" Text='<%# Eval("ShortName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mangla Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblJCInflow" runat="server" Text='<%# Eval("Inflows") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mangla Str(-)/Rel(+)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblStorageRelease" runat="server" Text='<%# Eval("StorageRelease") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Live Content(MAF)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblLiveContent" runat="server" Text='<%# Eval("LiveContent") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reservoir Level" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="ReservoirLvl" runat="server" Text='<%# Eval("ReservoirLevel") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mangla Outflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblOutflow" runat="server" Text='<%# Eval("Outflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chenab Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblChenabInflow" runat="server" Text='<%# Eval("Chenab") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ER Component" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblEastern" runat="server" Text='<%# Eval("Eastern") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemInflows" runat="server" Text='<%# Eval("SystemInflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Loss/Gain" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblLossGain" runat="server" Text='<%# Eval("LossGain") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Balance Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblBalanceInflow" runat="server" Text='<%# Eval("BalanceInflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Canal WDLS" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblProposedCanals" runat="server" Text='<%# Eval("ProposedCanalWDL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="16%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RQBS Outflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemOutflow" runat="server" Text='<%# Eval("SystemOutflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="14%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shortage" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblShortage" runat="server" Text='<%# Eval("Shortage") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="14%" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <label style="font-weight: bold;">Note: All values in '000 cusecs except as noted.</label><br />
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>
</asp:Content>
