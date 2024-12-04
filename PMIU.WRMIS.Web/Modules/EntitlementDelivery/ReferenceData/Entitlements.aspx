<%@ Page Title="Entitlements" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Entitlements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.ReferenceData.Entitlements" %>

<%@ MasterType VirtualPath="~/Site.Master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Entitlements</h3>
                        </div>
                        <div class="box-content">
                            <div class="row">
                                <div class="form-horizontal">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblSeason" class="col-sm-4 col-lg-3 control-label">Command</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" required="true" ID="ddlCommand" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" Enabled="false" required="true" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12 ">
                                    <asp:GridView ID="gvJhelum" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        OnRowCancelingEdit="gvJhelum_RowCancelingEdit" OnRowUpdating="gvJhelum_RowUpdating" OnRowEditing="gvJhelum_RowEditing"
                                        ShowFooter="true" ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LJC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLJC" runat="server" Text='<%# Eval("LJC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLJC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("LJC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UJC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUJC" runat="server" Text='<%# Eval("UJC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUJC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("UJC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UCC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUCC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("UCC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LCC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLCC" runat="server" Text='<%# Eval("LCC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLCC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("LCC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LBDC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLBDC" runat="server" Text='<%# Eval("LBDC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLBDC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("LBDC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MRINT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMRINT" runat="server" Text='<%# Eval("MRINT")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMRINT" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("MRINT") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CBDC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCBDC" runat="server" Text='<%# Eval("CBDC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCBDC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("CBDC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UDC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUDC" runat="server" Text='<%# Eval("UDC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUDC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("UDC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LDC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLDC" runat="server" Text='<%# Eval("LDC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLDC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("LDC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UPC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUPC" runat="server" Text='<%# Eval("UPC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUPC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("UPC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ESC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblESC" runat="server" Text='<%# Eval("ESC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtESC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("ESC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFC" runat="server" Text='<%# Eval("FC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtFC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("FC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtQC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("QC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UBC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUBC" runat="server" Text='<%# Eval("UBC")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUBC" runat="server" CssClass="form-control integerInput required" required="required" MaxLength="4" Text='<%# Eval("UBC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:Button>
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                                    </asp:Panel>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnHistory" runat="server" CssClass="btn btn-primary btn_24 audit" ToolTip="History" OnClick="lbtnHistory_Click"></asp:LinkButton>
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
                    </div>
                </div>
            </div>

            <div class="modal fade" id="ModalHistory" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body" id="content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="box">
                                        <div class="box-title">
                                            <asp:Label ID="lblName" runat="server" Style="font-size: 30px;"></asp:Label>
                                        </div>
                                        <div class="box-content">
                                            <div class="table-responsive">
                                                <div class="row" id="gvOffenders" runat="server">
                                                    <div class="col-md-12">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvJhelumHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                                ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                                                OnPageIndexChanged="gvJhelumHistory_PageIndexChanged" OnPageIndexChanging="gvJhelumHistory_PageIndexChanging"
                                                                CellSpacing="-1" GridLines="None">

                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Period">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="LJC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLJC" runat="server" Text='<%# Eval("LJC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UJC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUJC" runat="server" Text='<%# Eval("UJC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UCC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="LCC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLCC" runat="server" Text='<%# Eval("LCC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="LBDC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLBDC" runat="server" Text='<%# Eval("LBDC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="MRINT">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMRINT" runat="server" Text='<%# Eval("MRINT")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="CBDC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCBDC" runat="server" Text='<%# Eval("CBDC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UDC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUDC" runat="server" Text='<%# Eval("UDC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="LDC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLDC" runat="server" Text='<%# Eval("LDC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UPC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUPC" runat="server" Text='<%# Eval("UPC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="ESC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblESC" runat="server" Text='<%# Eval("ESC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFC" runat="server" Text='<%# Eval("FC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UBC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUBC" runat="server" Text='<%# Eval("UBC")%>'></asp:Label>
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
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="modal-footer">
                            <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
