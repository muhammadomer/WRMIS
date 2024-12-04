<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisposalEmergencyPurchases.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.DisposalEmergencyPurchases" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnEPWorkID" runat="server" Value="0" />
    <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Material Disposal Details (Infrastructure Name)</h3>
        </div>
        <div class="box-content">
            <div class="tbl-info">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Year" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label2" runat="server" Text="Zone" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Circle" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label3" runat="server" Text="Nature of Work" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" Text="RD" Font-Bold="true"></asp:Label>
                    </div>


                    <div class="col-md-2">
                        <asp:Label ID="lbl_year" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lbl_zone" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_Circle" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblnatureofWork" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblRD" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gv_disposal" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,EPWorkID,DisposalDate,VehicleNumber,BuiltyNumber,QtyMaterial,Unit,Cost,CreatedBy,CreatedDate" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True"
                    OnRowCancelingEdit="gv_disposal_RowCancelingEdit" OnRowUpdating="gv_disposal_RowUpdating" OnRowEditing="gv_disposal_RowEditing"
                    OnRowDeleting="gv_disposal_RowDeleting" OnPageIndexChanging="gv_disposal_PageIndexChanging"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" OnPageIndexChanged="gv_disposal_PageIndexChanged" OnRowCommand="gv_disposal_RowCommand" OnRowDataBound="gv_disposal_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%#Eval("DisposalDate", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div>
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("DisposalDate", "{0:dd-MMM-yyyy}") %>' required="True" onfocus="this.value = this.value;"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Number">
                            <ItemTemplate>
                                <asp:Label ID="lblVehicleNumber" runat="server" CssClass="control-label" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<asp:TextBox ID="txtVehicleNumber" runat="server" required="true" MaxLength="10" pattern="^[a-zA-Z0-9 ]*$" CssClass="form-control required" Text='<%# Eval("VehicleNumber") %>'  />--%>
                                <asp:TextBox ID="txtVehicleNumber" runat="server" required="true" MaxLength="10" CssClass="form-control required" Text='<%# Eval("VehicleNumber") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bilty Number">
                            <ItemTemplate>
                                <asp:Label ID="lblBuiltyeNumber" runat="server" CssClass="control-label" Text='<%# Eval("BuiltyNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtBuiltyeNumber" runat="server" MaxLength="10" pattern="^[a-zA-Z0-9 ]*$" CssClass="form-control" Text='<%# Eval("BuiltyNumber") %>' Width="70%" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity of Materials">
                            <ItemTemplate>
                                <asp:Label ID="lblQtyMaterial" runat="server" CssClass="control-label" Text='<%# Eval("QtyMaterial") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQtyMaterial" runat="server" required="true" MaxLength="6" pattern="^(0|[0-9][0-9]*)$" class="integerInput  required form-control" Text='<%# Eval("QtyMaterial") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2 text-right" />
                            <ItemStyle CssClass="integerInput" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblUniss" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="2.5%" />

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Units">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server" CssClass="control-label" Text='<%# Eval("Unit") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUnit" runat="server" required="true" pattern="^[a-zA-Z0-9 ' ]*$" MaxLength="10" CssClass="form-control  required" Text='<%# Eval("Unit") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />


                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Cost(Rs)">
                            <ItemTemplate>
                                <asp:Label ID="lblCost" runat="server" CssClass="control-label" Text='<%# Eval("Cost","{0:#,##0.##}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCost" runat="server" required="true" pattern="^(0|[0-9][0-9]*)$" MaxLength="6" class="integerInput  required form-control" Text='<%# Eval("Cost") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1 text-right" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <%-- <asp:TemplateField HeaderText="Attachments">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlAttachments" runat="server" ToolTip="Attachments" CssClass="btn btn-primary btn_24 attachments" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/EmergencyPurchases/AttachmentDisposal.aspx?ID={0}") %>' Text="">
                                </asp:HyperLink>

                            </ItemTemplate>
                            <ItemStyle CssClass="text-center" />

                        </asp:TemplateField>--%>

                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary btn_32 attachment" ToolTip="Attachments" Enabled="false" />
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
                                    <asp:HyperLink ID="hlAttachments" runat="server" ToolTip="Attachments" CssClass="btn btn-primary btn_32 attachment" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/EmergencyPurchases/AttachmentDisposal.aspx?ID={0}") %>' Text="">
                                    </asp:HyperLink>
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
