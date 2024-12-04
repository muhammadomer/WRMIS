<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LastYearRestorationWorksFFP.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.LastYearRestorationWorksFFP" %>

<%@ Register Src="~/Modules/FloodOperations/Controls/FFPDetail.ascx" TagPrefix="uc1" TagName="FFPDetail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFFPStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Last Year Restoration Works</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <uc1:FFPDetail runat="server" ID="FFPDetail" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvLastRestorationWorks" runat="server" DataKeyNames="InfrastructureType,InfrastructureName,WorkType,WorkName,WorkStatus,Description"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnPageIndexChanging="gvLastRestorationWorks_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresType" runat="server" Text='<%# (Convert.ToString(Eval("InfrastructureType"))) == "Control Structure1" ? "Barrage/Headwork": Eval("InfrastructureType") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresName" runat="server" Text='<%#Eval("InfrastructureName") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Work Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkType" runat="server" Text='<%#Eval("WorkType") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Work Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkName" runat="server" Text='<%#Eval("WorkName") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Work Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkStatus" runat="server" Text='<%#Eval("WorkStatus") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <%--       <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddInfrastructures" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlItems" runat="server" ToolTip="Items" CssClass="btn btn-primary btn_32 view-feedback" NavigateUrl='<%# Eval("FFPCampSitesID","~/Modules/FloodOperations/FFP/AddCampSiteItems.aspx?ID={0}") %>'></asp:HyperLink>
                                            <asp:Button ID="btnEditInfrastructures" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteInfrastructures" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionInfrastructures" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary btn_32 view-feedback" ToolTip="Items" Enabled="false" />
                                            <asp:Button runat="server" ID="btnSaveInfrastructures" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelInfrastructures" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>--%>
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
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };
    </script>
</asp:Content>
