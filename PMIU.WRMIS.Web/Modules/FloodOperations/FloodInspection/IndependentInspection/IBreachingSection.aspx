<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IBreachingSection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.IBreachingSection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnProtectionInfrastructureID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Breaching Section for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">

            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <div class="table-responsive">
                <div class="hidden">
                    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIGCDrainID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIGCDrainCreatedBy" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIGCDrainCreatedDate" runat="server" Value="0" />
                </div>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <%--OnRowCommand="gvIBreachingSection_RowCommand" OnRowDeleting="gvIBreachingSection_RowDeleting OnPageIndexChanging="gvIBreachingSection_PageIndexChanging"--%>
                        <asp:GridView ID="gvIBreachingSection" runat="server"
                            DataKeyNames="InfraBreachingSectionID,IBreachingSectionID,FromRDTotal,ToRDTotal,FromRD,ToRD,AffectedRowsNo,AffectedLinersNo,RecommendedSolution,RestorationCost,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            OnRowDataBound="gvIBreachingSection_RowDataBound"
                            OnRowEditing="gvIBreachingSection_RowEditing" OnRowCancelingEdit="gvIBreachingSection_RowCancelingEdit"
                            OnRowUpdating="gvIBreachingSection_RowUpdating"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Reach RD Start">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromRD" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reach RD end">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="No. of Affected Rows">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAffectedRowsNo" runat="server" Text='<%# Eval("AffectedRowsNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAffectedRowsNo" runat="server" required="required" pattern="[0-9]{1,5}" MaxLength="5" class="integerInput required form-control" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="No. of Affected Liners">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAffectedLinersNo" runat="server" Text='<%# Eval("AffectedLinersNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAffectedLinersNo" runat="server" required="required" pattern="[0-9]{1,5}" MaxLength="5" class="integerInput required form-control" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Recommended Solution">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecommendedSolution" runat="server" Text='<%# Eval("RecommendedSolution") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRecommendedSolution" runat="server" class=" form-control" Style="max-width: 88%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tentative cost of Restoration (Rs.)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRestorationCost" runat="server" Text='<%# Eval("RestorationCost") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRestorationCost" runat="server" class="integerInput form-control" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddStoneStock" runat="server" HorizontalAlign="Center">
                                            <%-- <asp:Button ID="btnAddStoneStock" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddStoneStock" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />--%>
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionStoneStock" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditStoneStock" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteStoneStock" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" Style="display: none" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionStoneStock" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveStoneStock" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelStoneStock" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
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

