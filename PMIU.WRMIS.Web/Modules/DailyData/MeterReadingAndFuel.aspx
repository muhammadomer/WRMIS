<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeterReadingAndFuel.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.MeterReadingAndFuel" %>

<%@ Register TagPrefix="uc1" TagName="FileUploadControl" Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Smart Monitoring</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Activity" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlactivity" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblActivityBy" runat="server" Text="Activity By" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlActivityBy" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlActivityBy_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblFromDate" runat="server" Text="Date From" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control required date-picker" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblToDate" runat="server" Text="Date To" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control required date-picker" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>                         
                        </div>--%>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblADM" runat="server" Text="ADM" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlADM" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlADM_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblMA" runat="server" Text="MA" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlMA" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblFromDate" runat="server" Text="Date From" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control required date-picker" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblToDate" runat="server" Text="Date To" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control required date-picker" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                                    <%--<asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/FloodInspection/Joint/AddJoint.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>--%>
                                </div>
                            </div>

                            <div class="col-md-6" style="margin-top: 14px;">
                                <div class="col-md-offset-11">
                                    <asp:HyperLink ID="hlGIS" runat="server" Font-Bold="true" Text="Web GIS" Target="_blank"></asp:HyperLink>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvMeterFuelReading" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" DataKeyNames="VehicleReadingID,AttachmentFile1,ObservedBY,ReadingServerDate,ReadingType,MeterReading,PetrolQuantity"
                                        ShowHeaderWhenEmpty="true" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                        OnPageIndexChanging="gvMeterFuelReading_PageIndexChanging" OnPageIndexChanged="gvMeterFuelReading_PageIndexChanged"
                                        OnRowCommand="gvMeterFuelReading_RowCommand" OnRowDataBound="gvMeterFuelReading_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleReadingID" runat="server" Text='<%# Eval("VehicleReadingID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observation Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObservationDate" runat="server" Text='<%#Eval("ReadingServerDate", "{0:dd-MMM-yyyy}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTime" runat="server" Text='<%#Eval("ServerTime") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observed By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObserveBy" runat="server" Text='<%#Eval("ObservedBY") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Activity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblactivity" runat="server" Text='<%#Eval("ReadingType") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="">
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>--%>
                                            <%--   <asp:TemplateField HeaderText="Petrol Reading" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPetrolReading" runat="server" Text='<%#Eval("PetrolQuantity","{0:0,0.0}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text="-">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlMeterReadingData" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnViewImage" runat="server" Text="" CommandName="ViewImage" CssClass="btn btn-primary btn_24 viewimg" ToolTip="ViewImage" />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />

                                                <%--<ItemTemplate>
                                                    <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("AttachmentFile1")%>' Visible="False"></asp:Label>
                                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Name="FormCtrl" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />--%>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>


                                    <asp:GridView ID="gvWatertheft" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" DataKeyNames="CaseID,IsScheduled,Activity"
                                        ShowHeaderWhenEmpty="true" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                        OnPageIndexChanging="gvWatertheft_PageIndexChanging" OnPageIndexChanged="gvWatertheft_PageIndexChanged"
                                        OnRowCommand="gvWatertheft_RowCommand" OnRowDataBound="gvWatertheft_RowDataBound">
                                        <Columns>
                                            <%-- <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFloodInspectionID" runat="server" Text='<%# Eval("VehicleReadingID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTime" runat="server" Text='<%#Eval("Time") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observed By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObserveBy" runat="server" Text='<%#Eval("ObservedBy") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Activity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActivity" runat="server" Text='<%#Eval("Activity") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%#Eval("ChannelName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlMeterReadingData" runat="server" HorizontalAlign="Center">
                                                        <asp:HyperLink ID="hlView" runat="server" CssClass="btn btn-primary btn_24 view" ToolTip="View" Visible="true"></asp:HyperLink>
                                                        <asp:Button ID="btnViewRota" runat="server" Visible="false" Text="" CommandName="ViewImage" CssClass="btn btn-primary btn_24 viewimg" ToolTip="ViewImage" />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <%--<ItemTemplate>
                                                    <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("AttachmentFile1")%>' Visible="False"></asp:Label>
                                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Name="FormCtrl" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />--%>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Start Of View Gauge Image -->
            <div id="viewimage" class="modal fade">
                <div class="modal-dialog" style="max-height: 519px; max-width: 793.398px; overflow: initial !important">
                    <div class="modal-content" style="width: 730px">
                        <div class="modal-body" style="height: 500px; overflow: auto; width: 728px;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <h3 id="header" runat="server" title="title"></h3>
                                    <asp:Table ID="Table1" runat="server" CssClass="table tbl-info">
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>By</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Dated</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblUploadedBy" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblDateTime" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell runat="server" ID="thMeterReading">Meter Reading</asp:TableHeaderCell>
                                            <asp:TableHeaderCell runat="server" ID="thfuelreading">Fuel Reading</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblMeterReading" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblFuelReading" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <%--    <asp:TableRow>
                                            <asp:TableHeaderCell runat="server" ID="thPetrolReading">Fuel Reading</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblPetrolReading" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                        </asp:TableRow>--%>
                                    </asp:Table>
                                    <br />
                                    <center>
                                        <asp:Image ID="imgImage" runat="server" style="display: block; max-width: 100%; height:auto; max-height:500px;"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
                <!-- END Of View Gauge Image -->
            </div>

            <div id="idViewRotational" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <h3>Rotational Violation</h3>
                                    <asp:Table ID="Table2" runat="server" CssClass="table tbl-info">
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Observed By</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Dated</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblObservedBy" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblDate" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell runat="server">Head Gauge</asp:TableHeaderCell>
                                            <asp:TableHeaderCell runat="server">Violation</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblHG" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblViolation" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Channel Name</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Division Name</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblChnlName" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblDivName" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Group Preference</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Remarks</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblGP" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                    </asp:Table>
                                    <br />
                                    <center>
                                        <asp:Image ID="imgRotational" runat="server" style="display: block; max-width: 100%; height:auto; max-height:500px;"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div id="idViewLeaves" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>

                                    <h3>Leaves</h3>

                                    <asp:Table ID="Table3" runat="server" CssClass="table tbl-info">
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Observed By</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Leaves Dated</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblLObservedBy" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblLDate" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell runat="server">Leave Type</asp:TableHeaderCell>
                                            <asp:TableHeaderCell runat="server">Rain gauge</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblLLeaveType" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblLRainGauge" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Remarks</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblLRemarks" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                    </asp:Table>
                                    <br />
                                    <center>
                                        <asp:Image ID="imgLimage" runat="server" style="display: block; max-width: 100%; height:auto; max-height:500px;"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };

        $(function () {

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeDatePickerStateOnUpdatePanelRefresh();
                    }
                });
            };
        });

    </script>
</asp:Content>
