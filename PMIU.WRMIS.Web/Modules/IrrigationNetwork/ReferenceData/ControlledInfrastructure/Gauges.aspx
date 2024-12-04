<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gauges.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure.Gauges" %>



<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucControlInfrastructureDetails" TagName="ControlInfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/ControlledInfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <asp:HiddenField ID="hdnControlInfrastructureID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Gauges of Barrage/Headwork</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucControlInfrastructureDetails:ControlInfrastructureDetail runat="server" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h3>Gauges</h3>
                        <asp:GridView ID="gvGauges" runat="server" DataKeyNames="ID,GaugesTypeID,GaugesTypeName,NoOfGauges,UpstreamDownstream,Side,Remarks,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvGauges_RowCommand" OnRowDataBound="gvGauges_RowDataBound"
                            OnRowEditing="gvGauges_RowEditing" OnRowCancelingEdit="gvGauges_RowCancelingEdit"
                            OnRowUpdating="gvGauges_RowUpdating" OnRowDeleting="gvGauges_RowDeleting" OnPageIndexChanging="gvGauges_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Gauge Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeType" runat="server" Text='<%# Eval("GaugesTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGaugeType" runat="server" CssClass="form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <%--<ItemStyle Width="130px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No. of Gauges">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoOfGauges" runat="server" Text='<%# Eval("NoOfGauges") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNoOfGauges" runat="server" CssClass="form-control" Style="max-width: 90%"> </asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemStyle CssClass="text-left alignitems" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upstream/Downstream">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpstreamDownstream" runat="server" Text='<%# Eval("UpstreamDownstream ") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlUpstreamDownstream" runat="server" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <%--<ItemStyle Width="130px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Side">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSide" runat="server" Text='<%# Eval("Side") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlSide" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <%--<ItemStyle Width="130px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <%--<HeaderStyle Width="140px" />--%>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddGauges" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddGaugesInformation" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditGauges" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteGauges" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionGauges" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveGauges" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelGauges" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
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
    <style type="text/css">
        .alignitems {
            padding-left: 60px !important;
        }
    </style>
</asp:Content>
