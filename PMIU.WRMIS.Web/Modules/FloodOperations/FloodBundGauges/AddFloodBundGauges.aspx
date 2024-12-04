<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddFloodBundGauges.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodBundGauges.AddFloodBundGauges" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Add Gauges Data</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnFloodInspectionID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                            <%--<asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />--%>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblStructureType" runat="server" Text="Structure Type" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlStructureType" runat="server" CssClass="form-control required" required="True" AutoPostBack="true" OnSelectedIndexChanged="ddlStructureType_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblStructureName" runat="server" Text="Structure Name" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlStructureName" runat="server" CssClass="required form-control" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlStructureName_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" required="True"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblTime" runat="server" Text="Time" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlTime" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                            <%--<asp:ListItem Text="All" Value="0" Selected="true"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <br />
                        </div>
                        <div id="HillTorrentNallah" runat="server">
                            <div class="table-responsive">
                                <asp:GridView ID="gvHilltorrentNallah" runat="server" DataKeyNames="ID,FGRID,StructureName,GuageReading,DischargeValue,CreatedDate"
                                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                    EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvHilltorrentNallah_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="StructureName" HeaderText="Location" ItemStyle-CssClass="col-lg-3" />
                                        <asp:TemplateField HeaderText="Guage Reading(ft)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGuageReading" runat="server" Text='<%# Eval("GuageReading") %>' class="decimalInput form-control" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-3 " />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discharge(Cusc)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDischarge" runat="server" Text='<%# Eval("DischargeValue") %>' class="decimalInput form-control" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-3 " />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>

                            </div>
                        </div>
                        <div id="Bund" runat="server">
                            <div class="table-responsive">
                                <asp:GridView ID="gvBund" runat="server" DataKeyNames="ID,FGRID,GaugeTypeName,GaugeRD,GuageReadingBund,CreatedDate"
                                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                    EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvBund_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="GaugeTypeName" HeaderText="Gauge Type" ItemStyle-CssClass="col-lg-3" />
                                        <asp:BoundField DataField="GaugeRD" HeaderText="RD" ItemStyle-CssClass="col-lg-1 text-center" />
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gauge Reading(ft)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGaugeReadingBund" runat="server" Text='<%# Eval("GuageReadingBund") %>' class="decimalInput form-control" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
