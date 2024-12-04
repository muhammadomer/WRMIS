<%@ Page Title="Seasonal Planning" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="IndusOperations.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.IndusOperations" %>

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
                        <li id="liVillage" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/SeasonalPlanning.aspx" id="anchBalance" onserverclick="anchBalance_ServerClick" runat="server" aria-controls="Village" role="tab">Balance Reservoir</a></li>
                        <li id="liDivision" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/ManglaIndusOperations.aspx" id="anchDivision" runat="server" aria-controls="Division" role="tab">Mangla Operations</a></li>
                        <li id="li1" runat="server" style="width: 16%; text-align: center" class="active"><a href="~/Modules/SeasonalPlanning/IndusOperations.aspx" id="a1" runat="server" aria-controls="Village" role="tab">Indus Operations</a></li>
                        <li id="li2" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedJC.aspx" id="a2" runat="server" aria-controls="Division" role="tab">Anticipated JC</a></li>
                        <li id="li3" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusRiver.aspx" id="a3" runat="server" aria-controls="Village" role="tab">Anticipated Indus River</a></li>
                        <li id="li4" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusBasin.aspx" id="a4" runat="server" aria-controls="Division" role="tab">Anticipated Indus Basin</a></li>
                    </ul>
                </div>
            </div>
            <div class="table-big" style="border: 0px;">
                <asp:GridView ID="gvIndus" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                    ShowHeaderWhenEmpty="True" CssClass="table header SPtable" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowDataBound="gvIndus_RowDataBound">
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
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tarbela Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblJCInflow" runat="server" Text='<%# Eval("Inflows") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tarbela Str(-)/Rel(+)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblStorageRelease" runat="server" Text='<%# Eval("StorageRelease") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Live Content(MAF)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblLiveContent" runat="server" Text='<%# Eval("LiveContent") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reservoir Level" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="ReservoirLvl" runat="server" Text='<%# Eval("ReservoirLevel") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tarbela Outflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblOutflow" runat="server" Text='<%# Eval("Outflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kabul Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Kabul") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chashma Str(-)/Rel(+)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ChashmaStorageRelease") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Live Content" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ChashmaLiveContent") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reservoir Level" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ChashmaLevel") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="J-C Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("JCOutflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("SystemInflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Loss/Gain" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("LossGain") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Balance Inflow" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("BalanceInflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Canal WDLS" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ProposedCanalWDL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kotri D/S" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("SystemOutflow") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shortage" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Shortage") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" Width="7%" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <label style="font-weight: bold;">Note: All values in '000 cusecs except as noted.</label><br />
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnsave_Click" />
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>
</asp:Content>



