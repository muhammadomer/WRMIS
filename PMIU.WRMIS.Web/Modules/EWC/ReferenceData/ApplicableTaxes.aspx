<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" EnableEventValidation="false" CodeBehind="ApplicableTaxes.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.ReferenceData.ApplicableTaxes" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
            <div class="box">
                <div class="box-title">
                    <h3>Applicable Taxes</h3> 
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group"> 
                                <div class="col-sm-8 col-lg-3 controls">
                                    <asp:RadioButton CssClass="radio-inline" required="required" ID="rbEffluent" runat="server" AutoPostBack="true" GroupName="ViewType" Text="Effluent Water" OnCheckedChanged="rb_CheckedChanged"/>
                                </div>
                                <div class="col-sm-8 col-lg-3">
                                     <asp:RadioButton CssClass="radio-inline" required="required" ID="rbCanal" runat="server" AutoPostBack="true" GroupName="ViewType" Text ="Canal Special Water" OnCheckedChanged="rb_CheckedChanged"/> 
                                </div> 
                            </div>
                        </div>
                    </div> 
                    
                    </br>
                    </br>

                    <div class="table-responsive" id="divEffluent" runat="server" >
                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15"
                            OnRowCommand="gv_RowCommand" OnPageIndexChanging="gv_PageIndexChanging" ShowHeaderWhenEmpty="true"
                            OnRowUpdating="gv_RowUpdating" OnRowDeleting="gv_RowDeleting"
                            OnRowCancelingEdit="gv_RowCancelingEdit" CellSpacing="-1" GridLines="None"
                            OnRowEditing="gv_RowEditing" OnPageIndexChanged="gv_PageIndexChanged"
                            EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" 
                            >
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Applicable Tax" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Tax") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="30" onfocus="this.value = this.value;" 
                                            CssClass="form-control required" placeholder="Enter Appliable Tax" value='<%# Eval("Tax") %>' Width="90%"/>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Type" Visible="true">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" CssClass="control-label" Text='<%# Eval("Type") %>'></asp:Label>
                                    </ItemTemplate> 
                                    <EditItemTemplate> 
                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="required form-control" required="required"/>
                                        <asp:HiddenField ID="hdnFUnit" runat="server" Value='<%# Eval("TypeID") %>'></asp:HiddenField>
                                    </EditItemTemplate> 
                                </asp:TemplateField>

                               <asp:TemplateField HeaderText="Amount" Visible="true">
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                   <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmnt" runat="server" CssClass="control-label" Text='<%#   Eval ("AmountS") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAmnt" runat="server" required="required" MaxLength="8" onfocus="this.value = this.value;" 
                                            CssClass="form-control required decimalIntegerInput" value='<%# Eval("Amount") %>' Width="100%"/>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                              <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" CssClass="form-control" placeholder="Enter Description" value='<%# Eval("Description") %>' Onkeyup="InputValidationText(this);" Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" Enabled="<%# base.CanAdd %>" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server"  Enabled="<%# base.CanEdit %>"  CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server"  Enabled="<%# base.CanDelete %>"  CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

                    <div class="table-responsive" id="divCanal" runat="server" >
                    <asp:GridView ID="gvC" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15"
                        OnRowCommand="gv_RowCommand" OnPageIndexChanging="gv_PageIndexChanging" ShowHeaderWhenEmpty="true"
                        OnRowUpdating="gv_RowUpdating" OnRowDeleting="gv_RowDeleting"
                        OnRowCancelingEdit="gv_RowCancelingEdit" CellSpacing="-1" GridLines="None"
                        OnRowEditing="gv_RowEditing" OnPageIndexChanged="gv_PageIndexChanged"
                        EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" 
                        >
                        <Columns>

                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Applicable Tax" Visible="true">
                                <HeaderStyle CssClass="col-md-3" />
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Tax") %>'></asp:Label>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="30" onfocus="this.value = this.value;" 
                                        CssClass="form-control required" placeholder="Enter Appliable Tax" value='<%# Eval("Tax") %>' Width="90%"/>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type" Visible="true">
                                <HeaderStyle CssClass="col-md-2" />
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" CssClass="control-label" Text='<%# Eval("Type") %>'></asp:Label>
                                </ItemTemplate> 
                                <EditItemTemplate> 
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="required form-control" required="required"/>
                                    <asp:HiddenField ID="hdnFTypeID" runat="server" Value='<%# Eval("TypeID") %>'></asp:HiddenField>
                                </EditItemTemplate> 
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount" Visible="true">
                                <HeaderStyle CssClass="col-md-2" />
                                <ItemStyle CssClass="text-right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAmnt" runat="server" CssClass="control-label" Text='<%# Eval("AmountS") %>'></asp:Label>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAmnt" runat="server" required="required" MaxLength="8" onfocus="this.value = this.value;" 
                                        CssClass="form-control required decimalIntegerInput" value='<%# Eval("Amount") %>' Width="90%"/>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description" Visible="true">
                                <HeaderStyle CssClass="col-md-5" />
                                <ItemTemplate>
                                    <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" CssClass="form-control" placeholder="Enter Description" value='<%# Eval("Description") %>' Onkeyup="InputValidationText(this);" Width="90%" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Active" Visible="true">
                                <HeaderStyle CssClass="col-md-1" />
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle CssClass="col-md-1" />
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnAdd" runat="server" Enabled="<%# base.CanAdd %>" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                    </asp:Panel>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnEdit" runat="server"  Enabled="<%# base.CanEdit %>"  CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                        <asp:Button ID="btnDelete" runat="server"  Enabled="<%# base.CanDelete %>"  CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                    </asp:Panel>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                    </asp:Panel>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
                </div>
            </div>
  <%--      </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

