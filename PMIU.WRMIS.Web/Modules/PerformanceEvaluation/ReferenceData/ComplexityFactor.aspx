<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplexityFactor.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ReferenceData.ComplexityFactor" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Complexity Factor</h3>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <asp:GridView ID="gvComplexityFactor" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                    AllowPaging="false" DataKeyNames="ID" OnRowEditing="gvComplexityFactor_RowEditing" OnRowCancelingEdit="gvComplexityFactor_RowCancelingEdit"
                    OnRowUpdating="gvComplexityFactor_RowUpdating" OnRowDataBound="gvComplexityFactor_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Complexity Level">
                            <ItemTemplate>
                                <asp:Label ID="lblComplexityLevel" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityLevel") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Complexity Factor">
                            <ItemTemplate>
                                <asp:Label ID="lblComplexityFactor" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityFactor") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtComplexityFactor" runat="server" CssClass="form-control decimalInput required" MaxLength="5" placeholder="Enter Complexity Factor" Text='<%# Eval("ComplexityFactor") %>' oninput='ValueValidation(this,"1","100")' required="true" autocomplete="off" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Multiplication Factor">
                            <ItemTemplate>
                                <asp:Label ID="lblMultiplicationFactor" runat="server" CssClass="control-label" Text='<%# Eval("MultiplicationFactor") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
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
