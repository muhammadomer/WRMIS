<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CampSitesFFP.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.CampSitesFFP" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDepartmentalInspectionDetail" TagName="DepartmentalInspectionDetail" Src="~/Modules/FloodOperations/Controls/DepartmentalInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFFPStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Infrastructures for Departmental Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucDepartmentalInspectionDetail:DepartmentalInspectionDetail runat="server" ID="DepartmentalInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvInfrastructures" runat="server" DataKeyNames="ID,StructureTypeID,StructureID,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvInfrastructures_RowCommand" OnRowDataBound="gvInfrastructures_RowDataBound"
                            OnRowEditing="gvInfrastructures_RowEditing" OnRowCancelingEdit="gvInfrastructures_RowCancelingEdit"
                            OnRowUpdating="gvInfrastructures_RowUpdating"
                            OnRowDeleting="gvInfrastructures_RowDeleting" OnPageIndexChanging="gvInfrastructures_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresType" runat="server" Text='<%# Eval("StructureTypeID") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInfrastructuresType" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructuresType_SelectedIndexChanged" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Infrastructure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresName" runat="server" Text='<%# Eval("StructureID") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInfrastructuresName" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRD" runat="server" CssClass="control-label" Text='<%# Eval("RD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRDLeft" runat="server" required="true" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtRDRight" runat="server" required="true" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px"></ItemStyle>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="256" pattern="^[a-zA-Z0-9 ]+$" Text='<%# Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>

                                        <asp:HyperLink ID="hlItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" Text="Items">
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddInfrastructures" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditInfrastructures" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteInfrastructures" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveInfrastructures" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelInfrastructures" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-default btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%--onclick="history.go(-1);return false;"--%>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
