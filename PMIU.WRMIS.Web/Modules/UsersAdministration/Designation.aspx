<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Designation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.Designation" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Designation</h3>
                    
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblOrganization" runat="server" Text="Organization" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged" required="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvDesignation" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvDesignation_RowCommand" OnPageIndexChanging="gvDesignation_PageIndexChanging"
                            OnRowUpdating="gvDesignation_RowUpdating" OnRowDeleting="gvDesignation_RowDeleting"
                            OnRowDataBound="gvDesignation_RowDataBound" OnRowCancelingEdit="gvDesignation_RowCancelingEdit"
                            OnRowEditing="gvDesignation_RowEditing" OnRowCreated="gvDesignation_RowCreated"
                            OnPageIndexChanged="gvDesignation_PageIndexChanged" CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("designation.ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblAuthorityRights" runat="server" Text='<%# Eval("designation.AuthorityRights") %>' Visible="false" />
                                        <asp:Label ID="lblTempAssignemnt" runat="server" Text='<%# Eval("designation.TempAssignment") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblWorkFlowDesignation" runat="server" Text='<%# Eval("designation.WorkflowDesignation") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Designation Name --%>
                                <asp:TemplateField HeaderText="Designation" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("designation.Name") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control required" onfocus="this.value = this.value;" required="true" MaxLength="30" placeholder="Enter Designation" Text='<%# Eval("designation.Name") %>' onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Irrigation Level --%>
                                <asp:TemplateField HeaderText="Level">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLevel" runat="server" CssClass="control-label" Text='<%# Eval("designation.UA_IrrigationLevel.Name") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control" />
                                        <asp:Label ID="lblHdnLevel" runat="server" Text='<%# Eval("designation.UA_IrrigationLevel.ID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Reporting to Organization --%>
                                <asp:TemplateField HeaderText="Organization">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportingToOrganization" runat="server" CssClass="control-label" Text='<%# Eval("reportingToOrganization") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlReportingToOrganization" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlReportingToOrganization_SelectedIndexChanged" />
                                        <asp:Label ID="lblHdnReportingToOrganization" runat="server" Text='<%# Eval("reportingToOrganizationID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Reporting to Designation --%>
                                <asp:TemplateField HeaderText="Designation">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportingToDesignation" runat="server" CssClass="control-label required" required="required" Text='<%# Eval("reportingToDesignation") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlReportingToDesignation" runat="server" CssClass="form-control" />
                                        <asp:Label ID="lblHdnReportingToDesignation" runat="server" Text='<%# Eval("reportingToDesignationID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Location Check Box --%>
                                <asp:TemplateField HeaderText="Loc">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkLoc" runat="server" CssClass="control-label"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Temp Assignment Check Box --%>
                                <asp:TemplateField HeaderText="Temp Assign">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkTempAssignment" runat="server" CssClass="control-label"></asp:CheckBox>
                                    </ItemTemplate>
                                    <%--<ItemStyle HorizontalAlign="Center" />--%>
                                </asp:TemplateField>

                                <%-- Add, Edit, Delete Buttons --%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("designation.ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
