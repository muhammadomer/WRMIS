<%@ Page Title="Structure Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StructureData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ReferenceData.StructureData" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Sites for Structure/Barrage/Dam/Channel</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Structure Name</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlStructure" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlStructure_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <b>
                <asp:Literal ID="litGridTitle" runat="server" Visible="false">Canals and Sites for Discharge Measurement</asp:Literal>
            </b>
            <hr>
            <div class="table-responsive">
                <asp:GridView ID="gvStructureData" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" OnRowDataBound="gvStructureData_RowDataBound" OnRowCommand="gvStructureData_RowCommand"
                    OnRowCancelingEdit="gvStructureData_RowCancelingEdit" OnRowUpdating="gvStructureData_RowUpdating"
                    OnRowEditing="gvStructureData_RowEditing" OnRowDeleting="gvStructureData_RowDeleting" AllowPaging="True"
                    OnPageIndexChanging="gvStructureData_PageIndexChanging" CssClass="table header"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false" OnPageIndexChanged="gvStructureData_PageIndexChanged">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Site Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSiteName" runat="server" required="true" MaxLength="30" placeholder="Enter Site Name" CssClass="form-control required" Text='<%# Eval("Name") %>' Width="90%" onfocus="this.value = this.value;" onkeyup="InputTextWithLengthValidation(this, 3)" ClientIDMode="Static" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("CO_Channel.Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblChannelID" runat="server" CssClass="control-label" Text='<%# Eval("ChannelID") %>' Visible="false" />
                                <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Map Gauge RD">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeRd" runat="server" CssClass="control-label" Text='<%# Eval("CO_ChannelGauge.GaugeAtRD") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                <asp:DropDownList ID="ddlGaugeRd" runat="server" required="true" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlGaugeRd_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAFSQ" runat="server" MaxLength="10" CssClass="form-control decimalInput" placeholder="Enter AFSQ" Text='<%# Eval("AFSQ") %>' Width="90%" ClientIDMode="Static" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" MaxLength="150" placeholder="Enter Remarks" Text='<%# Eval("Description") %>' onfocus="this.value = this.value;" Width="90%" onkeyup="InputValidationText(this)" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>" />
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
