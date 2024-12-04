<%@ Page Title="Outlet Performance Data" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OutletPerformanceHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.OutletPerformanceHistory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Outlet Performance History</h3>
                </div>
                <div class="box-content" id="divMain" runat="server">
                    <div class="table-responsive">
                        <asp:Table ID="tableInfo" runat="server" CssClass="table tbl-info">

                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblChnlNamelbl" runat="server" Text="Channel Name"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblChnlTypelbl" runat="server" Text="Channel Type"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblTotalRDlbl" runat="server" Text="Total R.Ds. (ft)"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblChnlName" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblChnlType" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblTotalRD" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblflowtypelbl" runat="server" Text="Flow Type"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblCommandNamelbl" runat="server" Text="Command Name"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblOutletRDlbl" runat="server" Text="Outlet R.Ds. (ft)"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblflowtype" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblCommandName" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblOutletRD" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblOutletSidelbl" runat="server" Text="Outlet Side"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblDistrictlbl" runat="server" Text="District"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblTehsillbl" runat="server" Text="Tehsil"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblOutletSide" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblTehsil" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblPoliceStationlbl" runat="server" Text="Police Station"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblVillagelbl" runat="server" Text="Village"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblIMISlbl" runat="server" Text="IMIS Code"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblPoliceStation" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblVillage" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblIMIS" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <br />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">


                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShowHistory" runat="server" CssClass="btn btn-primary" Text="Show History" OnClick="btnShowHistory_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvOutletshistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                            CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvOutletshistory_PageIndexChanged" OnPageIndexChanging="gvOutletshistory_PageIndexChanging">
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Head above Crest of Outlet (H in ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAboveCrest" runat="server" Text='<%# Eval("HeightAboveCrest") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Height of Outlet/Orifice (Y in ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHeightOrifice" runat="server" Text='<%# Eval("HeightOrifice") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Diameter/Breadth/Width (Dia/B in ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWidthY" runat="server" Text='<%# Eval("DiameterB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Working Head (wh in ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkingHead" runat="server" Text='<%# Eval("WorkingHead") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Observed Discharge (cusec)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblObserveDisc" runat="server" Text='<%# Eval("ObservedDischarge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Efficiency %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEfficiency" runat="server" Text='<%# Eval("Efficiency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
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
