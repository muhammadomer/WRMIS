<%@ Page MaintainScrollPositionOnPostback="true" Title="AddSchedule" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ScheduleDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ScheduleDetail" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ScheduleInspection/Controls/ScheduleDetail.ascx" TagPrefix="ucScheduleDetail" TagName="ScheduleDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css" rel="stylesheet">
        .subject-info-box-1 {
            float: left;
            width: 40%;
        }

        select {
            height: 200px;
            padding: 0;
        }

        option {
            padding: 4px 10px 4px 10px;
        }

            option:hover {
                background: #EEEEEE;
            }

        .subject-info-arrows {
            float: left;
            width: 10%;
        }

        input {
            width: 70%;
            margin-bottom: 5px;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Inspection of Schedules</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"></a>
                    </div>
                </div>
                <div class="box-content">
                    <asp:HiddenField runat="server" ID="InspectionTypeID" Value="" />
                    <ucScheduleDetail:ScheduleDetails runat="server" ID="ScheduleDetails" />
                    <asp:UpdatePanel runat="server" ID="UpdatePanelSchduleInspection" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h5><b>Gauge Inspection</b></h5>
                            <asp:GridView ID="gvGaugeInspection" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,DivisionID,SubDivisionID,ChannelID,GaugeID,DateOfVisit,Remarks,CreatedBy,CreatedDate"
                                ShowHeaderWhenEmpty="True" OnRowDeleting="gvGaugeInspection_RowDeleting" OnRowEditing="gvGaugeInspection_RowEditing" OnRowCommand="gvGaugeInspection_RowCommand" AllowPaging="false" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DivisionName" HeaderText="Division" ReadOnly="true" />
                                    <asp:BoundField DataField="SubDivisionName" HeaderText="Sub Division" ReadOnly="true" />
                                    <asp:BoundField DataField="ChannelName" HeaderText="Channel Name" ReadOnly="true" />
                                    <asp:BoundField DataField="InspectionRD" HeaderText="Inspection Areas" ReadOnly="true" />
                                    <asp:BoundField DataField="DateOfVisit" HeaderText="Date of Visit" ReadOnly="true" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />
                                    <%-- <asp:TemplateField HeaderText="Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGaugeInspectionDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGaugeInspectionSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionSubDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGaugeInspectionChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionChannel_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inspection Areas">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGaugeInspectionAreas" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGaugeInspectionRD" runat="server" Text='<%# Eval("InspectionRD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date of Visit">
                                        <EditItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtGaugeInspectionDateOfVisit" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGaugeInspectionDateOfVisit" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtGaugeInspectionRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGaugeInspectionRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <%-- <HeaderTemplate>
                                             <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float: right;">
                                              <asp:LinkButton ID="btnMulGaugeInspection" runat="server" Text="" CommandName="AddMulGaugeInspection" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple RDs">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                            <%-- <asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right; margin-right: 5px;">
                                                <asp:LinkButton ID="btnGaugeInspection" runat="server" Text="" formnovalidate="formnovalidate" CommandName="AddGaugeInspection"  CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionGaugeInspectionEdit" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="btnEditGaugeInspection" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>
                                            <asp:Panel ID="pnlEditActionGaugeInspection" runat="server" HorizontalAlign="Center">
                                                <asp:Button runat="server" ID="btnSaveGaugeInformation" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                                <asp:Button ID="lbtnCancelGaugeInspection" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </EditItemTemplate>--%>
                                     
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionGaugeInspection" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="lbtnDeleteGaugeInspection" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float:right;">
                                                <asp:LinkButton ID="btnMulGaugeInspection" runat="server" Text="" CommandName="AddMulGaugeInspection" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple RDs">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>Discharge Table Calculation</b></h5>
                            <asp:GridView ID="gvDischargeInspection" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,DivisionID,SubDivisionID,ChannelID,GaugeID,DateOfVisit,Remarks,CreatedBy,CreatedDate"
                                ShowHeaderWhenEmpty="True" OnRowCommand="gvDischargeInspection_RowCommand"
                                OnRowDeleting="gvDischargeInspection_RowDeleting" AllowPaging="false"
                                OnRowEditing="gvDischargeInspection_RowEditing"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DivisionName" HeaderText="Division" ReadOnly="true" />
                                    <asp:BoundField DataField="SubDivisionName" HeaderText="Sub Division" ReadOnly="true" />
                                    <asp:BoundField DataField="ChannelName" HeaderText="Channel Name" ReadOnly="true" />
                                    <asp:BoundField DataField="InspectionRD" HeaderText="Inspection Areas" ReadOnly="true" />
                                    <asp:BoundField DataField="DateOfVisit" HeaderText="Date of Visit" ReadOnly="true" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />
                                    <%--<asp:TemplateField HeaderText="Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlDischargeDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDischargeDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sub Divsion">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlDischargeSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDischargeSubDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Channel Name">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlDischargeChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDischargeChannel_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Inspection Areas">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlDischargeInspectionAreas" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInspection" runat="server" Text='<%# Eval("InspectionRD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date of Visit">
                                        <EditItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtDischargeDateOfVisit" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtDischargeRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDischargeRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlEdit" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="btnEditDischarge" runat="server" Text="" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Edit"  CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnldel" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="lbtnDeleteDischarge" runat="server" Text="" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Delete" formnovalidate="formnovalidate" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float:right;">
                                                <asp:LinkButton ID="btnMulGaugeInspection" runat="server" Text="" CommandName="AddMulChnlDis"  CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple RDs">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                            <%--<asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right; margin-right: 5px;">
                                                <asp:LinkButton ID="btnAddDischarge" runat="server" Text="" formnovalidate="formnovalidate" CommandName="AddDischarge" Visible='<%# GetVisibleValue(base.CanAdd) %>' CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                                </asp:LinkButton>
                                            </asp:Panel>--%>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>Inspection of Outlet Alteration</b></h5>
                            <asp:GridView ID="gvOutletInspection" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,DivisionID,SubDivisionID,ChannelID,OutletID,DateOfVisit,Remarks,CreatedBy,CreatedDate"
                                ShowHeaderWhenEmpty="True" OnRowDeleting="gvOutletInspection_RowDeleting" OnRowEditing="gvOutletInspection_RowEditing" OnRowCommand="gvOutletInspection_RowCommand" AllowPaging="false"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DivisionName" HeaderText="Division" ReadOnly="true" />
                                    <asp:BoundField DataField="SubDivisionName" HeaderText="Sub Division" ReadOnly="true" />
                                    <asp:BoundField DataField="ChannelName" HeaderText="Channel Name" ReadOnly="true" />
                                    <asp:BoundField DataField="OutletName" HeaderText="Outlet Name" ReadOnly="true" />
                                    <asp:BoundField DataField="DateOfVisit" HeaderText="Date of Visit" ReadOnly="true" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />
                                    <%--  <asp:ButtonField CommandName="Edit"  ButtonType="Button" Text="Edit" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="btn btn-primary btn_24 edit" />
                                    <asp:ButtonField CommandName="Delete"  ButtonType="Button" Text="Delete"     ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="btn btn-primary btn_24 edit"/>--%>
                                    <%--<asp:ButtonField CommandName="AddOutlet" CausesValidation="false" ButtonType="Button" Text="Add"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-CssClass="btn btn-success btn_add plus" />--%>
                                    <%--<asp:ButtonField CommandName="AddMulChnlDis"   ButtonType="Button" HeaderText="Add Multiple" HeaderStyle-HorizontalAlign="Right"   />--%>

                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutletEdit" runat="server" HorizontalAlign="right">
                                                <asp:Button ID="btnEditOutlet" runat="server" Text="" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutlet" runat="server" HorizontalAlign="right">
                                                <asp:Button ID="lbtnDeleteOutlet" runat="server" Text="" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Delete" formnovalidate="formnovalidate"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float: right;">
                                                <asp:LinkButton ID="btnAddOutlet" runat="server" Text="" CommandName="AddMulChnlDis" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple RDs">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                            <%--<asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right; margin-right: 5px;">
                                                <asp:LinkButton ID="btnAddMultiPaleOutlet" runat="server" Text="" formnovalidate="formnovalidate" CommandName="AddOutlet" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                                </asp:LinkButton>
                                            </asp:Panel>--%>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>Outlet Performance</b></h5>
                            <asp:GridView ID="gvOutletPerformance" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,DivisionID,SubDivisionID,ChannelID,OutletID,DateOfVisit,Remarks,CreatedBy,CreatedDate"
                                ShowHeaderWhenEmpty="True" OnRowEditing="gvOutletPerformance_RowEditing" OnRowDeleting="gvOutletPerformance_RowDeleting"  OnRowCommand="gvOutletPerformance_RowCommand" AllowPaging="false"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DivisionName" HeaderText="Division" ReadOnly="true" />
                                    <asp:BoundField DataField="SubDivisionName" HeaderText="Sub Division" ReadOnly="true" />
                                    <asp:BoundField DataField="ChannelName" HeaderText="Channel Name" ReadOnly="true" />
                                    <asp:BoundField DataField="OutletName" HeaderText="Outlet Name" ReadOnly="true" />
                                    <asp:BoundField DataField="DateOfVisit" HeaderText="Date of Visit" ReadOnly="true" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />

                                    <%--<asp:TemplateField HeaderText="Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlOutletPerformanceDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletPerformanceDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sub Divsion">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlOutletPerformanceSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletPerformanceSubDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceSubDiv" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Channel Name">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlOutletPerformanceChannelName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletPerformanceChannelName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceChnl" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Outlet Name">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlOutletPerformanceInspectionRD" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceRD" runat="server" Text='<%# Eval("OutletName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date of Visit">
                                        <EditItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtOutletPerformanceDateOfVisit" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceDate" runat="server" Text='<%# Eval("DateOfVisit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtOutletPerformanceRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutletPerformanceRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    --%>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutletPerformanceEdit" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="btnEditOutletPerformance" runat="server" Text="" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutletPerformance" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="lbtnDeleteOutletPerformance" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>' formnovalidate="formnovalidate" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                    CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Style="float: right;">
                                                <asp:LinkButton ID="btnAddOutletPerformance" runat="server" Text="" CommandName="AddMulChnlDis" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                              <h5><b>Outlet Checking</b></h5>
                            <asp:GridView ID="gvOutletChecking" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,DivisionID,SubDivisionID,ChannelID,OutletID,DateOfVisit,Remarks,CreatedBy,CreatedDate"
                                ShowHeaderWhenEmpty="True" OnRowEditing="gvOutletChecking_RowEditing" OnRowDeleting="gvOutletChecking_RowDeleting"  OnRowCommand="gvOutletChecking_RowCommand" AllowPaging="false"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DivisionName" HeaderText="Division" ReadOnly="true" />
                                    <asp:BoundField DataField="SubDivisionName" HeaderText="Sub Division" ReadOnly="true" />
                                    <asp:BoundField DataField="ChannelName" HeaderText="Channel Name" ReadOnly="true" />
                                    <asp:BoundField DataField="OutletName" HeaderText="Outlet Name" ReadOnly="true" />
                                    <asp:BoundField DataField="DateOfVisit" HeaderText="Date of Visit" ReadOnly="true" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutletCheckingEdit" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="btnEditOutletChecking" runat="server" Text="" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionOutletChecking" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="lbtnDeleteOutletChecking" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>' formnovalidate="formnovalidate" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                    CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Style="float: right;">
                                                <asp:LinkButton ID="btnAddOutletChecking" runat="server" Text="" CommandName="AddMulChnlDis" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple">
                                                </asp:LinkButton>
                                            </asp:Panel>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>Tenders Monitoring</b></h5>
                            <asp:GridView ID="gvTendersMonitoring" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,TenderNoticeID,TenderWorksID,DivisionID,TenderOpeningDate,Remarks"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvTendersMonitoring_RowDataBound" OnRowCommand="gvTendersMonitoring_RowCommand" OnRowCancelingEdit="gvTendersMonitoring_RowCancelingEdit"
                                OnRowDeleting="gvTendersMonitoring_RowDeleting" AllowPaging="false" OnPageIndexChanging="gvTendersMonitoring_PageIndexChanging"
                                OnRowEditing="gvTendersMonitoring_RowEditing" OnRowUpdating="gvTendersMonitoring_RowUpdating"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlTenderMonitoringDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenderMonitoringDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderMonitoringDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tender Notice">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlTenderMonitoringTenderNotice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenderMonitoringTenderNotice_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderMonitoringTenderNotice" runat="server" Text='<%# Eval("TenderNoticeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Work">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlTenderMonitoringWorks" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderMonitoringWorks" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Tender Opening Date">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTenderOpeningDate" runat="server" CssClass="form-control" type="text" ReadOnly="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderOpeningDate" runat="server" Text='<%# Eval("TenderOpeningDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtTenderMonitoringRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderMonitoringRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="right">
                                                <asp:Button ID="btnAddTenderMonitoring" runat="server" Text="" CommandName="AddTenderMonitoring" Visible="<%# IsToDisplayLink() %>" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionTenderMonitoring" runat="server">
                                                <asp:Button ID="btnEditTenderMonitoring" runat="server" Text="" CommandName="Edit" Visible="<%# IsToDisplayLink() %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                <asp:Button ID="lbtnDeleteTenderMonitoring" runat="server" Text="" CommandName="Delete" Visible="<%# IsToDisplayLink() %>" formnovalidate="formnovalidate"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditActionTenderMonitoring" runat="server">
                                                <asp:Button runat="server" ID="btnSaveTenderMonitoring" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                                <asp:Button ID="lbtnCancelTenderMonitoring" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>Works Inspections</b></h5>
                            <asp:GridView ID="gvClosureOperations" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,WorkSourceID,WorkSource,DivisionID,MonitoringDate,Remarks"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvClosureOperations_RowDataBound" OnRowCommand="gvClosureOperations_RowCommand" OnRowCancelingEdit="gvClosureOperations_RowCancelingEdit"
                                OnRowDeleting="gvClosureOperations_RowDeleting" AllowPaging="false" OnPageIndexChanging="gvClosureOperations_PageIndexChanging"
                                OnRowEditing="gvClosureOperations_RowEditing" OnRowUpdating="gvClosureOperations_RowUpdating"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Division">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlClosureOperationsDivision" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureOperationsDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Work Type">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlClosureOperationsWorkSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClosureOperationsWorkSource_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureOperationsWorkSource" runat="server" Text='<%# Eval("WorkSource") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Work">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlClosureOperationsWorks" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureOperationsWorks" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Inspection Date">
                                        <EditItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtMonitoringDate" Text='<%# Eval("MonitoringDate", "{0:d-MMM-yyyy}") %>' runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnMonitoringDate" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonitoringDate" runat="server" Text='<%# Eval("MonitoringDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtClosureOperationsRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenderMonitoringRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-4" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                <asp:Button ID="btnAddTenderMonitoring" runat="server" Text="" CommandName="AddClosureWorks" Visible="<%# IsToDisplayLink() %>" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionCLosureOperations" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEditCLosureOperations" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" Visible="<%# IsToDisplayLink() %>" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                <asp:Button ID="lbtnDeleteCLosureOperations" runat="server" Text="" CommandName="Delete" Visible="<%# IsToDisplayLink() %>" formnovalidate="formnovalidate"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditActionCLosureOperations" runat="server" HorizontalAlign="Center">
                                                <asp:Button runat="server" ID="btnSaveTenderMonitoring" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                                <asp:Button ID="lbtnCancelCLosureOperations" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <h5><b>General Inspections</b></h5>
                            <asp:GridView ID="gvGeneralInspections" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                DataKeyNames="ScheduleDetailID,ScheduleID,GeneralInspectionTypeID,Location,ScheduleDate,Remarks"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvGeneralInspections_RowDataBound" OnRowCommand="gvGeneralInspections_RowCommand" OnRowCancelingEdit="gvGeneralInspections_RowCancelingEdit"
                                OnRowDeleting="gvGeneralInspections_RowDeleting" AllowPaging="false" OnPageIndexChanging="gvGeneralInspections_PageIndexChanging"
                                OnRowEditing="gvGeneralInspections_RowEditing" OnRowUpdating="gvGeneralInspections_RowUpdating"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Inspection Type">
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGeneralInspectionType" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGeneralInspectionType" runat="server" Text='<%# Eval("GeneralInspectionType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Location">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control required" ID="txtLocation" runat="server" required="required"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Schedule Date">
                                        <EditItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtScheduleDate" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnScheduleDate" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblScheduleDate" runat="server" Text='<%# Eval("ScheduleDate", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtRemarks" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-4" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                <asp:Button ID="btnGeneralInspections" runat="server" Text="" CommandName="AddGeneralInspections" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" Visible="<%# IsToDisplayLinkGeneral() %>" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionGeneralInspections" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEditGeneralInspections" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" Visible="<%# IsToDisplayLinkGeneral() %>" formnovalidate="formnovalidate" />
                                                <asp:Button ID="lbtnDeleteGeneralInspections" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="<%# IsToDisplayLinkGeneral() %>"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditActionGeneralInspections" runat="server" HorizontalAlign="Center">
                                                <asp:Button runat="server" ID="btnGeneralInspections" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                                <asp:Button ID="lbtnGeneralInspections" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <div class="row" runat="server" id="divSave">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdateProgress runat="server">
                                <ProgressTemplate>
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="../../Design/assets/prettyPhoto/images/prettyPhoto/default/loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <!--********************Inspection Area and Calcuation Discharge Popup*********************-->
    <div id="AddMultipalInspectionArea" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelInspectionArea" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label1" runat="server" Text="Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionDivision_Mul" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionDivision_Mul_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label4" runat="server" Text="Sub Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionSubDivision_Mul" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionSubDivision_Mul_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label5" runat="server" Text="Channel Name" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionChannel_Mul" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionChannel_Mul_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label6" runat="server" Text="Date of Visit" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtGaugeInspectionDateOfVisit_Mul" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" id="spnDateOfVisit_Mul" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:Label ID="Label7" runat="server" Text="Remarks" CssClass="col-md-2 control-label" />
                                                <div class="col-md-10 controls">
                                                    <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtGaugeInspectionRemarks_Mul" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="form-horizontal" id="Multselection">
                                    <div id="dual-list" class="form-group row" style="margin-left: 64px;">

                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label2" Text="Inspection Areas" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" data-title="users" ID="lstArea" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>
                                        <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                            <input type="button" id="btnAllRightInspectionAreas" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="right" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="left" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnAllLeftInspectionAreas" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                        </div>
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label3" Text="Selected Inspection Areas" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" ID="SelectedInspectino" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSaveAndAddMoreInspectionArea" runat="server" class="btnSave btn btn-primary" Style="width: 148px" OnClientClick="SelectAllChannelInspectionArea()" CausesValidation="true" Text="Save and add more" OnClick="btnSaveAndAddMoreInspectionArea_Click"></asp:Button>
                                <asp:Button ID="btnSaveChannelInspectionArea" runat="server" class="btnSave btn btn-primary" Style="width: 68px" OnClientClick="SelectAllChannelInspectionArea()" CausesValidation="true" Text="Save" OnClick="btnSaveChannelInspectionArea_Click"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="ModelPupDicchargeCalcution" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label8" runat="server" Text="Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionDivision_Dis" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionDivision_Dis_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label9" runat="server" Text="Sub Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionSubDivision_Dis" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionSubDivision_Dis_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label10" runat="server" Text="Channel Name" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlGaugeInspectionChannel_Dis" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeInspectionChannel_Dis_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label11" runat="server" Text="Date of Visit" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtGaugeInspectionDateOfVisit_Dis" runat="server" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" id="Span1" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:Label ID="Label12" runat="server" Text="Remarks" CssClass="col-md-2 control-label" />
                                                <div class="col-md-10 controls">
                                                    <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtGaugeInspectionRemarks_Dis" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="form-horizontal" id="ChannelDischageCalculation">
                                    <div id="dual-list_Dis" class="form-group row" style="margin-left: 64px;">
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label13" Text="Inspection Areas" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" data-title="users" ID="left_list_dis" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>
                                        <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                            <input type="button" id="btnAllRightDischageCalculation" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="right_d" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="left_d" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnAllLeftDischageCalculation" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                        </div>
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label14" Text="Selected Inspection Areas" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" ID="right_lst_discharge" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>


                            </div>
                            <div class="modal-footer">
                                 <asp:Button ID="btnSaveAndAddMoreDischargeCalculation" runat="server" class="btnSave btn btn-primary" Style="width: 148px" OnClientClick="SelectAllChannelDischargCalculation()" CausesValidation="true" Text="Save and add more" OnClick="btnSaveAndAddMoreDischargeCalculation_Click"></asp:Button>
                                <asp:Button ID="btnSaveDischargeCalculation" runat="server" class="btn btn-primary" Style="width: 68px" OnClientClick="SelectAllChannelDischargCalculation()" Text="Save" OnClick="btnSaveDischargeCalculation_Click"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!--***************************************************************************************-->
    <!--********************Inspection of outlet Alteration***********************************-->
    <div id="addMultiOutLetAlteration" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <asp:UpdatePanel ID="SchUpdatePanle" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label15" runat="server" Text="Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddldivision_Outlet_Alteration" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldivision_Outlet_Alteration_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label16" runat="server" Text="Sub Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlSubDivision_Outlet_Alteration" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDivision_Outlet_Alteration_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label17" runat="server" Text="Channel Name" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlChannelName_Outlet_Alteration" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelName_Outlet_Alteration_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label18" runat="server" Text="Date of Visit" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtDateOfVisit_Outlet_Alteration" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" id="Span2" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:Label ID="Label19" runat="server" Text="Remarks" CssClass="col-md-2 control-label" />
                                                <div class="col-md-10 controls">
                                                    <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtRemarks_Outlet_Alteration" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="form-horizontal" id="Multselection_Outlet_Alteration">
                                    <div id="dual-list_Outlet_Alteration" class="form-group row" style="margin-left: 64px;">
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label20" Text="Outlet Name" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" data-title="users" ID="lstBox_Left_Outlet_Alteration" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>

                                        <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                            <input type="button" id="AllRight_outlet" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnright_Outlet_Alteration" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnleft_Outlet_Alteration" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="AllLeft_outlet" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                        </div>
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label21" Text="Selected Outlet Name" Font-Bold="true" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" ID="lstBox_right_Outlet_Alteration" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 200px" />
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <div class="modal-footer">

                                <asp:Button ID="btnSaveAndAddMoreOutletPerformance" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 148px" CausesValidation="true" Text="Save and add more" OnClick="btnSaveAndAddMoreOutletPerformance_Click"></asp:Button>
                                <asp:Button ID="btnSaveAndAddMoreOutletAlteration" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 148px" CausesValidation="true" Text="Save and add more" OnClick="btnSaveAndAddMoreOutletAlteration_Click"></asp:Button>
                                <asp:Button ID="btnSaveAndAddMoreOutletChecking" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 148px" CausesValidation="true" Text="Save and add more" OnClick="btnSaveAndAddMoreOutletChecking_Click"></asp:Button>
                                
                                <asp:Button ID="btnSaveOutletAlteration" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 68px" CausesValidation="true" Text="Save" OnClick="btnSaveOutletAlteration_Click"></asp:Button>
                                <asp:Button ID="btnSaveOutletPerformance" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 68px" CausesValidation="true" Text="Save" OnClick="btnSaveOutletPerformance_Click"></asp:Button>
                                <asp:Button ID="btnSaveOutletChecking" runat="server" class="btn btn-primary" OnClientClick="SelectAllOutletAlteration()" Visible="false" Style="width: 68px" CausesValidation="true" Text="Save" OnClick="btnSaveOutletChecking_Click"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!--***************************************************************************************-->


    <div id="UpdatePopup" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Update Recored</h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <asp:HiddenField runat="server" ID="rowIndex_Common" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label22" runat="server" Text="Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddldivision_Common" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldivision_Common_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label23" runat="server" Text="Sub Division" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlSubDivision_Common" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDivision_Common_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label24" runat="server" Text="Channel Name" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlChannelName_Common" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelName_Common_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbCommonName" runat="server" Text="Outlet Name" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <asp:DropDownList ID="ddlOutletName_Common" runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-8">
                                                <asp:Label ID="Label26" runat="server" Text="Remarks" CssClass="col-md-2 control-label" />
                                                <div class="col-md-10 controls">
                                                    <asp:TextBox CssClass="form-control multiline-no-resize commentsLength" ID="txtRemarks_Common" runat="server" minlength="3" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="Label25" runat="server" Text="Date of Visit" CssClass="col-md-4 control-label" />
                                                <div class="col-md-8 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtDateofVist_Common" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" id="Span3" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnUpdate_Common" runat="server" class="btn btn-primary" Style="width: 68px" CausesValidation="true" Text="Save" OnClick="btnUpdate_Common_Click"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>

                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <!--********************-->


    <asp:HiddenField runat="server" ID="hdnScheduleID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnFromDate" Value="" />
    <asp:HiddenField runat="server" ID="hdnToDate" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusID" Value="" />
    <asp:HiddenField runat="server" ID="hdnPreparedByID" Value="" />
    <asp:HiddenField ID="hdnPreparedByDesignationID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIrrigationLvlId" runat="server" Value="" />
    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="" />
    <asp:HiddenField ID="scrollLeft" runat="server" Value="0" />
    <asp:HiddenField ID="scrollTop" runat="server" Value="0" />

    <%--  <input name="scrollLeft" id="scrollLeft" type="hidden" value="0" />
<input name="scrollTop" id="scrollTop" type="hidden" value="0" />--%>









    <script type="text/javascript">

        //**********************************************************************************//
        function SelectAllChannelDischargCalculation() {
            $('#right_lst_discharge option').prop('selected', true);
        }
        //**********************************************************************************//
        function SelectAllChannelInspectionArea() {
            $('#SelectedInspectino option').prop('selected', true);
        }
        //**********************************************************************************//
        function SelectAllOutletAlteration() {
            $('#lstBox_right_Outlet_Alteration option').prop('selected', true);
        }

        //**********************************************************************************//
        function closeModal_InspectionArea() {
            $("#AddMultipalInspectionArea").modal("hide");
        };
        function closeModal_DischargeCalculation() {
            $("#ModelPupDicchargeCalcution").modal("hide");
        };
        function closeModal_Outlet_PerAlter() {
            $("#addMultiOutLetAlteration").modal("hide");
        };
        //**********************************************************************************//
        function closeModal_Update() {
            $("#UpdatePopup").modal("hide");
        };
        $(function () {

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {

                        //**********************************************************************************//
                        $("#btnright_Outlet_Alteration").bind("click", function () {
                            debugger;
                            var options = $("[id*=lstBox_Left_Outlet_Alteration] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {
                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=lstBox_right_Outlet_Alteration]").append(opt);
                                }
                            }

                        });
                        $("#btnleft_Outlet_Alteration").bind("click", function () {
                            debugger;
                            var options = $("[id*=lstBox_right_Outlet_Alteration] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {
                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=lstBox_Left_Outlet_Alteration]").append(opt);
                                }
                            }
                        });
                        //**********************************************************************************//
                        $("#left_d").bind("click", function () {
                            debugger;
                            var options = $("[id*=right_lst_discharge] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {
                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=left_list_dis]").append(opt);
                                }
                            }
                        });
                        $("#right_d").bind("click", function () {
                            debugger;
                            var options = $("[id*=left_list_dis] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {


                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=right_lst_discharge]").append(opt);
                                }
                            }
                        });

                        //**********************************************************************************//
                        $("#left").bind("click", function () {
                            debugger;
                            var options = $("[id*=SelectedInspectino] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {
                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=lstArea]").append(opt);
                                }
                            }
                        });
                        $("#right").bind("click", function () {
                            debugger;
                            var options = $("[id*=lstArea] option:selected");
                            if (options.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            else {
                                for (var i = 0; i < options.length; i++) {
                                    var opt = $(options[i]).clone();
                                    $(options[i]).remove();
                                    $("[id*=SelectedInspectino]").append(opt);
                                }
                            }
                        });
                        //**********************************************************************************//



                        //******************************Button All Option clicked**********************************//

                        //********************right all**********************//
                        $('#btnAllRightInspectionAreas').click(function (e) {
                            var selectedOpts = $('#lstArea option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#SelectedInspectino').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        $('#btnAllRightDischageCalculation').click(function (e) {
                            var selectedOpts = $('#left_list_dis option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#right_lst_discharge').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        $('#AllRight_outlet').click(function (e) {
                            var selectedOpts = $('#lstBox_Left_Outlet_Alteration option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#lstBox_right_Outlet_Alteration').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        //********************Left all**********************//

                        $('#btnAllLeftInspectionAreas').click(function (e) {
                            var selectedOpts = $('#SelectedInspectino option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#lstArea').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        $('#btnAllLeftDischageCalculation').click(function (e) {
                            var selectedOpts = $('#right_lst_discharge option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#left_list_dis').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        $('#AllLeft_outlet').click(function (e) {
                            var selectedOpts = $('#lstBox_right_Outlet_Alteration option');
                            if (selectedOpts.length == 0) {
                                alert("Nothing to move.");
                                e.preventDefault();
                            }
                            $('#lstBox_Left_Outlet_Alteration').append($(selectedOpts).clone());
                            $(selectedOpts).remove();
                            e.preventDefault();
                        });
                        InitilizeDatePickerStateOnUpdatePanelRefresh();






                    }
                });
            };
        });
    </script>



    <script type="text/javascript">
        (function () {

            function GetCoords() {
                debugger;
                var scrollX, scrollY;
                if (document.all) {
                    if (!document.documentElement.scrollLeft)
                        scrollX = document.body.scrollLeft;
                    else
                        scrollX = document.documentElement.scrollLeft;

                    if (!document.documentElement.scrollTop)
                        scrollY = document.body.scrollTop;
                    else
                        scrollY = document.documentElement.scrollTop;
                }
                else {
                    scrollX = window.pageXOffset;
                    scrollY = window.pageYOffset;
                }
                document.getElementById('<%= scrollLeft.ClientID%>').value = scrollX;
                document.getElementById('<%= scrollTop.ClientID%>').value = scrollY;
                //alert(scrollX + '     ' + scrollY);
            }
            function Set_Scroll() {


                var x = document.getElementById('<%= scrollLeft.ClientID%>').value;
                var y = document.getElementById('<%= scrollTop.ClientID%>').value;
                window.scrollTo(x, y);
            }


            //$(window).load(Set_Scroll)
            //$(window).scroll(GetCoords);

        }()); //run this anonymous function immediately
    </script>

</asp:Content>
