<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ViewSchedule.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ViewSchedule" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Schedule Details</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                    </div>
                </div>
                <div class="box-content">
                    <%--<div class="well well-sm">--%>
                        <asp:Table ID="tblChannelDetail" runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableHeaderCell>Schedule Title:</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblScheduleName" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableHeaderCell>Tour Description:</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblScheduleDescription" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableHeaderCell>From Date:</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblFromDate" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableHeaderCell>To Date:</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblToDate" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableHeaderCell>Status:</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    <%--</div>--%>

                    <div class="row">
                        <div class="col-md-12">
                            <h5><b>Gauge Inspection</b></h5>
                            <%--                            <div class="table-responsive">--%>
                            <asp:GridView ID="gvGaugeInspection" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                EmptyDataText="No Gauge Inspection record exists in the system.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel / Headwork Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelHeadwork" runat="server" Text='<%# Eval("ChannelHeadWorkName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inspection Areas">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInspectionAreas" runat="server" Text='<%# Eval("InspectionArea") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date of Visit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>

                        <div class="col-md-12">

                            <h5><b>Outlet Alteration</b></h5>
                            <%--                            <div class="table-responsive">--%>
                            <asp:GridView ID="gvOutletAlteration" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                EmptyDataText="No Outlet Alteration record exists in the system.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outlet RD's">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletRDs" runat="server" Text='<%# Eval("OutletRDs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date of Visit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--                            </div>--%>
                        </div>

                        <div class="col-md-12">

                            <h5><b>Works</b></h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvWorks" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Works record exists in the system.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type of Work">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeOfWork" runat="server" Text='<%# Eval("TypeOfWork") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Work Sub Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkSubCategory" runat="server" Text='<%# Eval("WorkSubCategory") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date of Visit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>

                        <div class="col-md-12">

                            <h5><b>Outlet Performance Register</b></h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvOutletPerformanceRegister" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Outlet Performance Register record exists in the system.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inspection Areas">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInspectionAreas" runat="server" Text='<%# Eval("InspectionAreas") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date of Visit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>

                        <div class="col-md-12">

                            <h5><b>Tender</b></h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvTender" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Tender record exists in the system.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tender Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTenderName" runat="server" Text='<%# Eval("TenderName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tender Date / Visit Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTenderVisitDate" runat="server" Text='<%# Eval("TenderVisitDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tender Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTenderTime" runat="server" Text='<%# Eval("TenderTime") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>

                        <div class="col-md-12">
                            <h5><b>Discharge Measurement</b></h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvDischargeMeasurement" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-hover fill-head" GridLines="None" ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Discharge Measurement record exists in the system.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel / Headwork Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelHeadwork" runat="server" Text='<%# Eval("ChannelHeadwork") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inspection Areas">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInspectionAreas" runat="server" Text='<%# Eval("InspectionAreas") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date of Visit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>
                    </div>

                    <div class="row">

                        <div class="col-md-12">
                            <center>
                                <button class="btn btn-primary"><i class="fa fa-cog"></i> New Scehdule</button> 
                                <button class="btn btn-default"><i class="fa fa-cog"></i> Edit</button>
                                <button class="btn btn-success"><i class="fa fa-cog"></i> Send for Approval</button>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
