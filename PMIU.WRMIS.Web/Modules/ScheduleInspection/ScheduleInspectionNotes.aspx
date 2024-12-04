<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleInspectionNotes.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ScheduleInspectionNotes" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucScheduleDetail" Src="~/Modules/ScheduleInspection/Controls/ScheduleDetail.ascx" TagName="ScheduleDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Inspection Notes</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <ucScheduleDetail:ScheduleDetail runat="server" ID="ScheduleDetail" />
            </div>
            <asp:HiddenField ID="hdnScheduleStatusID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSchedulePreparedByID" runat="server" Value="0" />
            <h5><b>Gauge Inspection</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvGuage" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,GaugeID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvGuage_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sub Division">
                            <ItemTemplate>
                                <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspection Areas">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeInspectionRD" runat="server" Text='<%# Eval("InspectionRD") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Visit">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeInspectionDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeInspectionRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblInspectionNotesGaugeReading" Visible="<%# IsToDisplayInspectionLink() %>" runat="server" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("ScheduleDetailID","~/Modules/ScheduleInspection/AddGaugeInspection.aspx?ScheduleDetailID={0}") %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <h5><b>Discharge Table Calculation</b></h5>
            <div class="table-responsive">

                <asp:GridView ID="gvDischarge" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,GaugeID,GaugeLevelID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvDischarge_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Divsion">
                            <ItemTemplate>
                                <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Inspection Areas">
                            <ItemTemplate>
                                <asp:Label ID="lblInspection" runat="server" Text='<%# Eval("InspectionRD") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Visit">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblDischargeRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlInspectioNotesDischargeTable" runat="server" Visible="<%# IsToDisplayInspectionLink() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <h5><b>Inspection of Outlet Alteration</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvOutlet" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,OutletID,ScheduleID" EmptyDataText="No record found"
                    OnRowDataBound="gvOutlet_RowDataBound"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletDivName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Divsion">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletSubDivisionName" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Outlet Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletInspection" runat="server" Text='<%# Eval("OutletName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Visit">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletDate" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control" ID="txtOutletRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOutletRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="gvSOutletAltIN" runat="server" Visible="<%# IsToDisplayInspectionLink() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <h5><b>Outlet Performance</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvOutPer" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,OutletID,ScheduleID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvOutPer_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Divsion">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceSubDiv" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceChnl" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Outlet Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceRD" runat="server" Text='<%# Eval("OutletName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Visit">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceDate" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="gvINOPR" runat="server" Visible="<%# IsToDisplayInspectionLink() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

             <h5><b>Outlet Checking</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvOutletChecking" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,OutletID,ChannelID,OutletCheckingID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvOutletChecking_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Divsion">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceSubDiv" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceChnl" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Outlet Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceRD" runat="server" Text='<%# Eval("OutletName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Visit">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceDate" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletPerformanceRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="gvOChl" runat="server" Visible="<%# IsToDisplayInspectionLink() %>"
                                    NavigateUrl='<%# String.Format("OutletChecking.aspx?ScheduleDetailID={0}&ChannelID={1}&Outlet={2}&ScheduleID={3}&From={4}", Eval("ScheduleDetailID"),Eval("ChannelID"),Eval("OutletID"),(Request.QueryString["ScheduleID"]),"IN") %>'
                                    ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

            <h5><b>Tender Monitoring</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvTenderMonitoring" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,TenderNoticeID,TenderWorksID,WorkSourceID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvTenderMonitoring_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblTendersDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tender Notice">
                            <ItemTemplate>
                                <asp:Label ID="lblTenderNotice" runat="server" Text='<%# Eval("TenderNoticeName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Work">
                            <ItemTemplate>
                                <asp:Label ID="lblTenderWork" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tender Opening Date">
                            <ItemTemplate>
                                <asp:Label ID="lblTenderOpeningDate" runat="server" Text='<%# Eval("TenderOpeningDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblTenderRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlTenders" runat="server" Visible="<%# IsToDisplayInspectionLinkTender() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <h5><b>Works Inspections</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvClosureOperations" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,WorkSourceID,WorkSource,CWPID,RefMonitoringID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvClosureOperations_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblClosureDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Work Source">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkSource" runat="server" Text='<%# Eval("WorkSource") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Work">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkName" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Monitoring Date">
                            <ItemTemplate>
                                <asp:Label ID="lblMonitoringDate" runat="server" Text='<%# Eval("MonitoringDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblClosureRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlClosure" runat="server" Visible="<%# IsToDisplayInspectionLinkTender() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
             <h5><b>General Inspections</b></h5>
            <div class="table-responsive">
                <asp:GridView ID="gvGeneralInspections" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,ScheduleID,GeneralInspectionTypeID,Location,ScheduleDate,IsInspected" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" OnRowDataBound="gvGeneralInspections_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Inspection Type">
                            <ItemTemplate>
                                <asp:Label ID="lblGeneralInspectionType" runat="server" Text='<%# Eval("GeneralInspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Schedule Date">
                            <ItemTemplate>
                                <asp:Label ID="lblScheduleDate" runat="server" Text='<%# Eval("ScheduleDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-4" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" minlength="3" maxlength="250" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlGeneralInspections" runat="server" Visible="<%# IsToDisplayInspectionLinkGeneral() %>" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 edit" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div class="row" runat="server" id="divSave">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField runat="server" ID="hdnScheduleID" Value="0" />
</asp:Content>
