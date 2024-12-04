<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlacingIndents.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.PlacingIndents" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Placing Indents</h3>

                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Sub Division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" CssClass="form-control required" runat="server" AutoPostBack="true" TabIndex="1" required="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" required="required" CssClass="disabled-todayPast-date-picker form-control required"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>

                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblChannel" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Channel Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlChannel" CssClass="form-control required" runat="server" TabIndex="3" required="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearchChannel" Text="Search" CssClass="btn btn-primary" runat="server" OnClick="btnSearchChannel_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" />
                                    <%--<asp:HyperLink ID="btnPlaceIndent" Text="Place Indent" CssClass="btn btn-success" runat="server"></asp:HyperLink>
                                <asp:LinkButton ID="btnReset" runat="server" CssClass="btn" Text="Reset"/>--%>
                                </div>
                            </div>
                        </div>

                        <div id="divHeader" runat="server" class="table-responsive" visible="false">
                            <table class="table tbl-info" visible="false">

                                <tr>
                                    <th>IMIS Code</th>
                                    <th>Current Sub-Division Indent</th>
                                    <th>Current Sub-Division Indent Placement Date</th>
                                    <th></th>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblIMISCode" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblCurrentSubDivisionIndent" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblCurrentSubDivisionIndentDate" runat="server" Text=""  /></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <th>Total Indent at current Sub-Divisional head</th>
                                    <th>Lower Sub-Division Indent</th>
                                    <th>Lower Sub-Division Indent Placement Date</th>
                                    <th></th>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblTotalIndnetAtCurrentSubDivisionalHead" runat="server" Text=""/></td>
                                    <td>
                                        <asp:Label ID="lblLowerSubDivisionIndent" runat="server" Text="" /></td>
                                    <td>
                                        <asp:Label ID="lblLowerSubDivisionIndentDate" runat="server" Text="" /></td>
                                    <td></td>
                                </tr>

                            </table>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvIndents" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowFooter="true"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                        OnRowDataBound="gvIndents_RowDataBound" OnRowEditing="gvIndents_RowEditing" OnRowCancelingEdit="gvIndents_RowCancelingEdit"
                                        DataKeyNames="ChannelRD">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                                    <asp:Label ID="lblIndentID" runat="server" CssClass="control-label" Text='<%# Eval("IndentID") %>' Visible="false" />
                                                    <asp:Label ID="lblDirectOfftakesID" runat="server" CssClass="control-label" Text='<%# Eval("DirectOfftakeID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblIndentValue" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Direct Offtake">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDirectOfftakes" runat="server" CssClass="control-label" Text='<%# Eval("DirectOfftakeName") %>'></asp:Label>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    <b>
                                                        <asp:Label Text="Offtakes Total Indents" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label Text="Direct Outlets Total Indent" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label ID="lblPercent" Text="" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label Text="Indent At Sub-Divisional Gauge" runat="server" Visible="true" /></b>
                                                </FooterTemplate>

                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Offtake Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelType" runat="server" CssClass="control-label" Text='<%# Eval("ChannelType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Parent RD (ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParrentRD" runat="server" CssClass="control-label" Text='<%# Eval("ChannelRD") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Indent Placement Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOfftakeIndentDate" runat="server" CssClass="control-label" Text='<%# Eval("OfftakeIndentDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField HeaderText="Indent Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentTime" runat="server" CssClass="control-label" Text='<%# Eval("OfftakeIndentTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Indent (Cusec)" FooterStyle-CssClass="numberAlign">
                                                <%-- <ItemTemplate>
                                                    <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("ChannelIndent") %>'></asp:Label>
                                                </ItemTemplate>--%>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIndent" runat="server" required="required" MaxLength="10" onfocus="this.value = this.value;" CssClass="form-control decimalInput required" placeholder="Enter Indent Value" value='<%# Eval("ChannelIndent") %>' ClientIDMode="Static" />
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    <b>
                                                        <asp:Label ID="lblOfftakesTotalIndents" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label ID="lblDirectOutletsTotalIndent" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label ID="lblIncrementedIndentAt10P" runat="server" Visible="true" /></b><br>
                                                    <br>
                                                    <b>
                                                        <asp:Label ID="lblIndentAtSubDivisionalGauge" runat="server" Visible="true" /></b>
                                                </FooterTemplate>

                                                <HeaderStyle CssClass="col-md-1 numberAlign" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remarks">
                                                <%--<ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>--%>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" onfocus="this.value = this.value;" CssClass="form-control" placeholder="Enter Remarks" value='<%# Eval("Remarks") %>' ClientIDMode="Static" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <%--<asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />--%>
                                                    <asp:LinkButton ID="hlIndent" runat="server" OnClick="hlIndent_Click" CssClass="btn btn-primary btn_24 indents"  ToolTip="Details"></asp:LinkButton>
                                                </ItemTemplate>

                                                <%-- <EditItemTemplate>
                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="right">
                                                        <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                    </asp:Panel>
                                                </EditItemTemplate>--%>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" />
                                    <%--<asp:HyperLink ID="btnPlaceIndent" Text="Place Indent" CssClass="btn btn-success" runat="server"></asp:HyperLink>
                                <asp:LinkButton ID="btnReset" runat="server" CssClass="btn" Text="Reset"/>--%>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
</asp:Content>
