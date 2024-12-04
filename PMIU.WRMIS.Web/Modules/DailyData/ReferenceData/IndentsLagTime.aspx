<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndentsLagTime.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ReferenceData.Indents" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Indents Lag Time</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblSubDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Sub Division</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlSubDivision" CssClass="form-control required" runat="server" AutoPostBack="true" TabIndex="1" required="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblChannel" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control required" AutoPostBack="True" required="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvIndents" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True"
                    OnRowCancelingEdit="gvIndents_RowCancelingEdit" OnRowUpdating="gvIndents_RowUpdating"
                    OnRowEditing="gvIndents_RowEditing" AllowPaging="True" OnRowDataBound="gvIndents_RowDataBound"
                    OnPageIndexChanging="gvIndents_PageIndexChanging" CssClass="table header"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false" OnPageIndexChanged="gvIndents_PageIndexChanged">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gauge Type">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeType" runat="server" CssClass="control-label" Text='<%# Eval("CO_ChannelGauge.CO_GaugeCategory.Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gauge at RD (ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeAtRD" runat="server" CssClass="control-label" Text='<%# Eval("LowerRD") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Upper RD (ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblUpperRD" runat="server" CssClass="control-label" Text='<%# Eval("UpperRD") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Distance (ft)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblDistance" runat="server" CssClass="control-label" Text='<%# Convert.ToInt64(Eval("LowerRD"))  -  Convert.ToInt64(Eval("UpperRD")) %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Velocity (ft/sec)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblVelocity" runat="server" CssClass="control-label" Text='<%# Eval("Velocity") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVelocity" runat="server" CssClass="form-control decimalInput required" required="required" onkeyup="ValueValidation(this, '0.1','15')" onfocus="this.value = this.value;" placeholder="Enter Velocity" Text='<%# Eval("Velocity") %>' ClientIDMode="Static" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Lag Time (Hrs)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblLagTime" runat="server" CssClass="control-label" Text='<%# Eval("LagTime") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                </asp:Panel>
                            </EditItemTemplate>

                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                    <%--<asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />--%>
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
