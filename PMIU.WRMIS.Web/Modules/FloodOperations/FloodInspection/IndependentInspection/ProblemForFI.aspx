<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProblemForFI.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.ProblemForFI" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInfrastructureType" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Problem for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvProblemForFI" runat="server" DataKeyNames="ID,FromRD,ToRD,FromRDTotal,ToRDTotal,NatureofProblemID,RecommendedSolution,TentativeCostofRestoration,CreatedDate,CreatedBy"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvProblemForFI_RowCommand" OnRowDataBound="gvProblemForFI_RowDataBound"
                            OnRowEditing="gvProblemForFI_RowEditing" OnRowCancelingEdit="gvProblemForFI_RowCancelingEdit"
                            OnRowUpdating="gvProblemForFI_RowUpdating"
                            OnRowDeleting="gvProblemForFI_RowDeleting" OnPageIndexChanging="gvProblemForFI_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIrrigationFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right"></HeaderStyle>
                                    <ItemStyle CssClass="text-right"/>
                                </asp:TemplateField>
                                 <%--<asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1"></HeaderStyle>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIrrigationToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right"></HeaderStyle>
                                    <ItemStyle CssClass="text-right"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nature of Problem">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNatureofProblem" runat="server" Text='<%# Eval("NatureofProblem") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlNatureofProblem" runat="server" required="required" CssClass=" form-control required" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recommended Solution">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecommendedSolution" runat="server" Text='<%# Eval("RecommendedSolution")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRecommendedSolution" runat="server" CssClass="form-control" MaxLength="250" Style="max-width: 90%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tentative Cost of Restoration (Rs)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTentativeCostofRestoration" runat="server" Text='<%# Eval("TentativeCostofRestoration")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTentativeCostofRestoration" runat="server" pattern="[0-9]{0,7}" CssClass="form-control text-right" Style="max-width: 85%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right"></HeaderStyle>
                                    <ItemStyle CssClass="text-right"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-3"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddProblemFI" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddProblemFI" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddProblemFI" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionProblemFI" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditProblemFI" runat="server" Text="" CommandName="Edit" Enabled="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteProblemFI" runat="server" Text="" CommandName="Delete" Enabled="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionProblemFI" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveProblemFI" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelProblemFI" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
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

