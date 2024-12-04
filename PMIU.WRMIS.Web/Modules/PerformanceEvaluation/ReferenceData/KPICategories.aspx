<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KPICategories.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ReferenceData.KPICategories" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>KPI Categories</h3>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <asp:GridView ID="gvKPICategories" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="ID,PEInclude"
                    OnRowEditing="gvKPICategories_RowEditing" OnRowCancelingEdit="gvKPICategories_RowCancelingEdit"
                    OnRowDataBound="gvKPICategories_RowDataBound" OnRowUpdating="gvKPICategories_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Category Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoryName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Default Weightage (Field Data)">
                            <ItemTemplate>
                                <asp:Label ID="lblWeightFieldDefault" runat="server" CssClass="control-label" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current Weightage (Field Data)">
                            <ItemTemplate>
                                <asp:Label ID="lblWeightFieldCurrent" runat="server" CssClass="control-label" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Default Weightage (PMIU Checking)">
                            <ItemTemplate>
                                <asp:Label ID="lblWeightPMIUDefault" runat="server" CssClass="control-label" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current Weightage (PMIU Checking)">
                            <ItemTemplate>
                                <asp:Label ID="lblWeightPMIUCurrent" runat="server" CssClass="control-label" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter Desciption" Text='<%# Eval("Description") %>' MaxLength="500" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Include in P.E. Calculation" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="controls">
                                    <asp:CheckBox ID="cbPEInclude" runat="server" Checked='<%# Eval("PEInclude") %>' Enabled="false" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="controls">
                                    <asp:CheckBox ID="cbPEInclude" runat="server" Checked='<%# Eval("PEInclude") %>' />
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderText="Action">
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                    <asp:HyperLink ID="hlKpisubcat" runat="server" ToolTip="KPI SubCategory" CssClass="btn btn-primary btn_32 details"
                                        NavigateUrl='<%# Eval("ID", "~/Modules/PerformanceEvaluation/ReferenceData/KPISubCategories.aspx?CatgID={0}") %>'>
                                    </asp:HyperLink>
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnReset" runat="server" Text="Reset to defaults" CssClass="btn btn-primary" OnClick="btnReset_Click" OnClientClick="return confirm('User entered values will be erased, are you sure you want to reset?');" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
