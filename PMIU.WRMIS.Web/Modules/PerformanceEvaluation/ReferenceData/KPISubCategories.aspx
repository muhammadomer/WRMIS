<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KPISubCategories.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ReferenceData.KPISubCategories" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <asp:HiddenField ID="hdnCategID" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>KPI Sub Categories</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblKPICategory" runat="server" Text="KPI Category Name" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 0px;" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlKPICategory" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlKPICategory_SelectedIndexChanged" required="true"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <asp:Panel runat="server" ID="pnlContent" Visible="false">
                <div class="table-responsive">
                    <asp:GridView ID="gvKPISubCategories" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="ID,PEInclude,BaseWeight"
                        OnRowEditing="gvKPISubCategories_RowEditing" OnRowCancelingEdit="gvKPISubCategories_RowCancelingEdit"
                        OnRowDataBound="gvKPISubCategories_RowDataBound" OnRowUpdating="gvKPISubCategories_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="KPI Sub Category Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Default Weightage">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightDefault" runat="server" CssClass="control-label" Text='<%# Eval("WeightDefault") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Current Weightage">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightCurrent" runat="server" CssClass="control-label" Text='<%# Eval("WeightCurrent") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWeightCurrent" runat="server" CssClass="form-control decimalIntegerInput required" placeholder="Enter Current Weight" Text='<%# Eval("WeightCurrent") %>' required="true" MaxLength="8" autocomplete="off" />
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weightage Source">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightageSource" runat="server" CssClass="control-label" Text='<%# Eval("Source") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter Desciption" Text='<%# Eval("Description") %>' MaxLength="500" />
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-4" />
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
                                <%--<asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back" NavigateUrl = "~/Modules/PerformanceEvaluation/ReferenceData/KPICategories.aspx"></asp:HyperLink>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
