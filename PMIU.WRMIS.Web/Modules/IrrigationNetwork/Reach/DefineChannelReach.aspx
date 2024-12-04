<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DefineChannelReach.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach.DefineChannelReach" %>

<%@ Import Namespace="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><%--<i class="fa fa-file"></i>--%>Define Channel Reach</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />

            <asp:Table ID="tblChannelDetail" runat="server" CssClass="table tbl-info">
                <asp:TableRow>
                    <asp:TableHeaderCell>Channel Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Channel Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Total R.Ds. (ft)</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblChannelName" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblChannelType" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblTotalRDs" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnChannelTotalRDs" Value="0" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Flow Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Command Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>IMIS Code</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblFlowType" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCommandName" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblIMISCode" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <hr>

            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvChannelReach" runat="server" DataKeyNames="ID,StartingRDTotal,EndingRDTotal,Remarks,StartingRD,EndingRD" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                    OnRowCommand="gvChannelReach_RowCommand" OnRowCancelingEdit="gvChannelReach_RowCancelingEdit" OnRowEditing="gvChannelReach_RowEditing"
                                    OnRowDataBound="gvChannelReach_RowDataBound" OnRowDeleting="gvChannelReach_RowDeleting" OnRowUpdating="gvChannelReach_RowUpdating"
                                    on OnPageIndexChanging="gvChannelReach_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Reach Starting  R.Ds. (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartingRD" runat="server" Text='<%#Eval("StartingRD") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditStartingRD" runat="server" Text='<%# Eval("StartingRD") %>' CssClass="hidden"></asp:Label>
                                                <asp:Panel ID="pnlStartingRD" runat="server">
                                                    <asp:TextBox ID="txtStartingRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                    +
                                                <asp:TextBox ID="txtStartingRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reach Ending  R.Ds. (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndingRD" runat="server" Text='<%# Eval("EndingRD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEndingRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                +
                                                <asp:TextBox ID="txtEndingRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" class="form-control" Style="max-width: 95%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAddChannelReach" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnAddChannelReach" visible="<%# base.CanAdd %>" runat="server" Text="" CommandName="AddChannelReach" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlChannelReach" runat="server" HorizontalAlign="Center">
                                                    <asp:HyperLink ID="hlLSectionStartingRD" runat="server" NavigateUrl='<%# GetPageURL(PageNames.LSectionParameters, Convert.ToString(Eval("ID")), Convert.ToString(Eval("StartingRDTotal")),Convert.ToString(Container.DataItemIndex+1),"s") %>' ToolTip="L Section Starting RD" CssClass="btn btn-primary btn_24 starting-rd" Text="">
                                                    </asp:HyperLink>

                                                    <asp:HyperLink ID="hlLSectionEndingRD" runat="server" NavigateUrl='<%# GetPageURL(PageNames.LSectionParameters, Convert.ToString(Eval("ID")), Convert.ToString(Eval("EndingRDTotal")),Convert.ToString(Container.DataItemIndex+1),"e") %>' ToolTip="L Section Ending RD" CssClass="btn btn-primary btn_24 ending-rd" Text="">
                                                    </asp:HyperLink>
                                                    <asp:HyperLink ID="hlLSectionHistoryStartingRD" runat="server" NavigateUrl='<%# GetPageURL(PageNames.LSectionHistory, Convert.ToString(Eval("ID")), Convert.ToString(Eval("StartingRDTotal")),Convert.ToString(Container.DataItemIndex+1),"s") %>' ToolTip="L Section History Starting RD" CssClass="btn btn-primary btn_24 starting-rd-history" Text="">
                                                    </asp:HyperLink>

                                                    <asp:HyperLink ID="hlLSectionHistoryEndingRD" runat="server" NavigateUrl='<%# GetPageURL(PageNames.LSectionHistory, Convert.ToString(Eval("ID")), Convert.ToString(Eval("EndingRDTotal")),Convert.ToString(Container.DataItemIndex+1),"e") %>' ToolTip="L Section History Ending RD" CssClass="btn btn-primary btn_24 ending-rd-history" Text="">
                                                    </asp:HyperLink>
                                                    <asp:Button ID="btnEditChannelReach" visible="<%# base.CanEdit %>" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                    <asp:Button ID="btnDeleteChannelReach" visible="<%# base.CanDelete %>" runat="server" Text="" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditChannelReach" runat="server" HorizontalAlign="Center">
                                                    <asp:Button runat="server" ID="btnSaveChannelReach" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                    <asp:Button ID="btnCancelChannelReach" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
    </script>
</asp:Content>
