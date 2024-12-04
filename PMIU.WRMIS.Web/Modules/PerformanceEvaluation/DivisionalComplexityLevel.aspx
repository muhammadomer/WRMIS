<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DivisionalComplexityLevel.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.DivisionalComplexityLevel" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Divisional Complexity Level</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" required="true"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvDivisionalComplexity" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                    GridLines="None" Visible="false" OnPageIndexChanging="gvDivisionalComplexity_PageIndexChanging"
                    OnRowEditing="gvDivisionalComplexity_RowEditing" OnRowCancelingEdit="gvDivisionalComplexity_RowCancelingEdit"
                    DataKeyNames="ID,DivisionID,ComplexityID" OnRowDataBound="gvDivisionalComplexity_RowDataBound"
                    OnRowUpdating="gvDivisionalComplexity_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Complexity Level">
                            <ItemTemplate>
                                <asp:Label ID="lblComplexityLevel" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblComplexityLevelID" CssClass="control-label" runat="server" Text='<%# Eval("ComplexityID") %>' Visible="false" />
                                <asp:DropDownList ID="ddlComplexityLevel" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlComplexityLevel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Multiplication Factor">
                            <ItemTemplate>
                                <asp:Label ID="lblMultiplicationFactor" runat="server" CssClass="control-label" Text='<%# Eval("MultiplicationFactor") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                            <ItemStyle CssClass="text-right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" MaxLength="500" placeholder="Enter Remarks" Text='<%# Eval("Remarks") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
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
        </div>
    </div>
</asp:Content>
