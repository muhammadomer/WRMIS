<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewOutletInspection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ViewOutletInspection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ScheduleInspection/Controls/OutletInspectionNotes.ascx" TagPrefix="uc1" TagName="ViewOutletInspectionNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3><span runat="server" id="pageTitleID">View Outlet Alteration Inspection</span></h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                    </div>
                </div>
                <div class="box-content">
                    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnInspectionTypeID" runat="server" />
                    <asp:HiddenField ID="hdnScheduleDetailChannelID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnDischarge" runat="server" Value="" />

                    <uc1:ViewOutletInspectionNotes runat="server" ID="ViewOutletInspectionNotes" />

                    <asp:Table ID="tblOutletAlterationInspection" Visible="true" runat="server" CssClass="table tbl-info">
                        <asp:TableRow>
                            <asp:TableHeaderCell Width="33.3%">Inspection Date</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Inspection Time</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Head above Crest of Outlet (H in ft)</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblInspectionDate" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblInspectionTime" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblHeadAboveCrest" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Working Head (ft)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Diameter/Breadth/ Width (Dia/B in ft)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Height of Outlet/Orifice (Y in ft)</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblWorkingHead" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDBWidth" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblOutletHeight" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Discharge (cusec)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Remarks</asp:TableHeaderCell>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblADischarge" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </asp:TableCell>

                        </asp:TableRow>
                    </asp:Table>

                    <asp:Table ID="tblOutletPerformanceInspection" Visible="false" runat="server" CssClass="table tbl-info">
                        <asp:TableRow>
                            <asp:TableHeaderCell Width="33.3%">Inspection Date</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Inspection Time</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Working Head (wh in ft)</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPInspectionDate" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblPInspectionTime" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblPWorkingHead" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Discharge (cusec)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Efficiency (Observed Discharge/Design Discharge x 100)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Head above Crest of Outlet (H in ft)</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPDischarge" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblPEfficiency" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblPHeadAboveCrest" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Height of Outlet/Orifice (Y in ft)</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Diameter/Breadth/ Width (Dia/B in ft)</asp:TableHeaderCell>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPerformanceOutletHeight" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblOutletDiameter" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="Label3" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableHeaderCell ColumnSpan="3">Remarks</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:Label ID="lblPRemarks" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
