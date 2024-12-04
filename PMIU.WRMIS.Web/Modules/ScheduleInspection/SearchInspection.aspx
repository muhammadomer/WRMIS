<%@ Page Title="SearchInspection" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SearchInspection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.SearchInspection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
     <ContentTemplate>
    <div class="box">
        <div class="box-title">
            <h3>Search Inspection</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlZone" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlCircle" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlSubDivision" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                    <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                    <span id="spantoDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlInspectionCategory" id="lblInspectionCategory" class="col-sm-4 col-lg-3 control-label">Inspection Type</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlInspectionCategory" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlInspectionType" id="lblInspectionType" class="col-sm-4 col-lg-3 control-label">Inspection Category</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:DropDownList ID="ddlInspectionType" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlInspectionType_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvGuage" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="SID,IsScheduled" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvGauge_RowCreated" OnRowDataBound="gvGauge_OnRowDataBound" OnPageIndexChanging="gvGuage_PageIndexChanging" CssClass="table header"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionArea" runat="server" Text='<%# Eval("InspectionArea") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gauge (ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeValue" runat="server" Text='<%# Eval("GaugeValue") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discharge (Cusecs)">
                            <ItemTemplate>
                                <asp:Label ID="lblDischarge" runat="server" Text='<%# Eval("Discharge") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Complaint">
                            <ItemTemplate>
                                <asp:Label ID="lblComplaintGenerated" runat="server" Text='<%# Eval("ComplaintGenerated") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlInspectionView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/AddGaugeInspection.aspx?ScheduleDetailID={0}&IsScheduled={1}", HttpUtility.UrlEncode(Eval("SID").ToString()), HttpUtility.UrlEncode(Eval("IsScheduled").ToString())) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvDischargeTableBedLevel" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ID,IsScheduled" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvDischargeTableBedLevel_RowCreated" OnRowDataBound="gvDischargeTableBedLevel_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvDischargeTableBedLevel_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionArea" runat="server" Text='<%# Eval("InspectionArea") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value of Exponent(n)">
                            <ItemTemplate>
                                <asp:Label ID="lblExponentValue" runat="server" Text='<%# Eval("ExponentValue") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mean Depth (D in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblMeanDepth" runat="server" Text='<%# Eval("MeanDepth") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlDischargeBedLvlView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/AddDischargeTableCalcBL.aspx?ScheduleDetailID={0}&IsScheduled={1}", HttpUtility.UrlEncode(Eval("ID").ToString()), HttpUtility.UrlEncode(Eval("IsScheduled").ToString())) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvDischargeTableCrestlvl" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ID,IsScheduled" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvDischargeTableCrestlvl_RowCreated" OnRowDataBound="gvDischargeTableCrestlvl_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvDischargeTableCrestlvl_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionArea" runat="server" Text='<%# Eval("InspectionArea") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Breadth of Fall(B in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblBreadthFall" runat="server" Text='<%# Eval("BreadthFall") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head above Crest  (H in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lbHeadAboveCrest" runat="server" Text='<%# Eval("HeadAboveCrest") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlDischargeCrestLvlView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/AddDischargeTableCalcCL.aspx?ScheduleDetailID={0}&IsScheduled={1}", HttpUtility.UrlEncode(Eval("ID").ToString()), HttpUtility.UrlEncode(Eval("IsScheduled").ToString())) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvOutletPerformance" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="OutletPerformanceID,IsScheduled" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvOutletPerformance_RowCreated" OnRowDataBound="gvOutletPerformance_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvOutletPerformance_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet">
                            <ItemTemplate>
                                <asp:Label ID="lblIOutlet" runat="server" Text='<%# Eval("Outlet") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head above Crest (H in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lbHeadAboveCrest" runat="server" Text='<%# Eval("HeadAboveCrest") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Working Head (wh in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkingHead" runat="server" Text='<%# Eval("WorkingHead") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlOutletPerformanceView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID={0}&IsScheduled={1}&Type={2}", HttpUtility.UrlEncode(Eval("OutletPerformanceID").ToString()), HttpUtility.UrlEncode(Eval("IsScheduled").ToString()), HttpUtility.UrlEncode("true")) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvOutletChecking" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="OutletCheckinID,IsScheduled,ScheduleID,OutletID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvOutletChecking_RowCreated" OnRowDataBound="gvOutletChecking_RowDataBound" CssClass="table header" OnPageIndexChanging="gvOutletChecking_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet">
                            <ItemTemplate>
                                <asp:Label ID="lblIOutlet" runat="server" Text='<%# Eval("Outlet") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head above Crest (H in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lbHeadAboveCrest" runat="server" Text='<%# Eval("HeadAboveCrest") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlOutletCheckingView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/OutletChecking.aspx?OutletCheckingID={0}&Outlet={1}&ScheduleID={2}&From={3}",(Eval("OutletCheckinID")),(Eval("OutletID")),(Eval("ScheduleID")),("SI")) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
                <asp:GridView ID="gvOutletAlterationHistory" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="OutletAlterationHistoryID,IsScheduled" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvOutletAlterationHistory_RowCreated" OnRowDataBound="gvOutletAlterationHistory_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvOutletAlterationHistory_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet">
                            <ItemTemplate>
                                <asp:Label ID="lblIOutlet" runat="server" Text='<%# Eval("Outlet") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height\Orifice (Y in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblHeightOrifice" runat="server" Text='<%# Eval("HeightOrifice") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Working Head(ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkingHead" runat="server" Text='<%# Eval("WorkingHead") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlOutletAlterationView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# string.Format("~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID={0}&IsScheduled={1}&Type={2}", HttpUtility.UrlEncode(Eval("OutletAlterationHistoryID").ToString()), HttpUtility.UrlEncode(Eval("IsScheduled").ToString()), HttpUtility.UrlEncode("false")) %>' Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvTenderMonitoring" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,TenderNoticeID,TenderWorkID,WorkSourceID,DivisionID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvTenderMonitoring_RowCreated" OnRowDataBound="gvTenderMonitoring_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvTenderMonitoring_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText=" Inspection Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("InspectionDatetime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspected By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tender Notice">
                            <ItemTemplate>
                                <asp:Label ID="lblTenderNotice" runat="server" Text='<%# Eval("TenderNotice") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Work">
                            <ItemTemplate>
                                <asp:Label ID="lblWork" runat="server" Text='<%# Eval("Work") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Opening Date">
                            <ItemTemplate>
                                <asp:Label ID="lblOpeningDate" runat="server" Text='<%# Eval("OpeningDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    <%--    <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>--%>
                        <%-- <asp:TemplateField HeaderText="Complaint">
                            <ItemTemplate>
                                <asp:Label ID="lblComplaintGenerated" runat="server" Text='<%# Eval("ComplaintGenerated") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlTenderMonitoring" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvClosureOperations" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ScheduleDetailID,WorkSourceID,WorkType,DivisionID,MonitoringDate,RefMonitoringID,CWPID" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowCreated="gvClosureOperations_RowCreated" OnRowDataBound="gvClosureOperations_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvClosureOperations_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText=" Inspection Date">
                            <ItemTemplate>
                                <asp:Label ID="lblMonitoringDate" runat="server" Text='<%# Eval("MonitoringDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspected By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Work Type">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkType" runat="server" Text='<%# Eval("WorkType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Work">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkName" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Monitoring Date">
                            <ItemTemplate>
                                <asp:Label ID="lblMonitoringDate" runat="server" Text='<%# Eval("MonitoringDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlClosureOperations" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

                <asp:GridView ID="gvGeneralInspections" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ID,ScheduleDetailID,InspectionTypeID,InspectionDate,InspectionDetails" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="true" OnRowDataBound="gvGeneralInspections_OnRowDataBound" CssClass="table header" OnPageIndexChanging="gvGeneralInspections_PageIndexChanging" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText=" Inspection Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionDate" runat="server" Text='<%# Eval("InspectionDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspected By">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspection Type">
                            <ItemTemplate>
                                <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("InspectionTypeCat") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inspection">
                            <ItemTemplate>
                                <asp:Label ID="lblInspection" runat="server" Text='<%# Eval("InspectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="HeaderAction">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlGeneralInspections" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" Text="">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

            <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
        </div>
    </div>
 </ContentTemplate>
            
 </asp:UpdatePanel>
     <script type="text/javascript">
         //On UpdatePanel Refresh
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         if (prm != null) {
             prm.add_endRequest(function (sender, e) {
                 if (sender._postBackSettings.panelsToUpdate != null) {
                     InitilizeDatePickerStateOnUpdatePanelRefresh();
                     ClearDateField();
                 }
             });
         };
    </script>
</asp:Content>
