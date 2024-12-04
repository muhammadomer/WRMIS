<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelParentFeeder.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.ChannelParentFeeder" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucChannelDetails" Src="~/Modules/IrrigationNetwork/Controls/ChannelDetails.ascx" TagName="ChannelDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTotalRDs" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Parent Channels and Channel Feeders</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucChannelDetails:ChannelDetail runat="server" ChannelID="<%= hdnChannelID.Value %>" ID="ChannelDetail" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvParentChannelsFeeders" runat="server" AutoGenerateColumns="false"
                        CssClass="table header" GridLines="None" ShowHeaderWhenEmpty="true"
                        AllowSorting="false" AllowPaging="True" EmptyDataText="No record found"
                        DataKeyNames="ID,ParentFeederChannelID,RelationshipTypeID,SideID,TotalChannelRD,TotalParentFeederChannelRD,StructureTypeID"
                        OnRowCommand="gvParentChannelsFeeders_RowCommand" OnRowDataBound="gvParentChannelsFeeders_RowDataBound"
                        OnRowCancelingEdit="gvParentChannelsFeeders_RowCancelingEdit" OnRowEditing="gvParentChannelsFeeders_RowEditing"
                        OnRowDeleting="gvParentChannelsFeeders_RowDeleting" OnRowUpdating="gvParentChannelsFeeders_RowUpdating" OnPageIndexChanging="gvParentChannelsFeeders_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Parent or Feeder Channel Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblParentChannelFeederName" runat="server" Text='<%# Eval("ParentFeederChannelName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlChannelParentFeeder" runat="server" OnSelectedIndexChanged="ddlChannelParentFeeder_SelectedIndexChanged" AutoPostBack="true" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle Width="275px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Relationship Type">
                                <ItemTemplate>
                                    <asp:Label ID="RelationShipName" runat="server" Text='<%# Eval("RelationshipTypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlRelationShip" runat="server" OnSelectedIndexChanged="ddlRelationShip_SelectedIndexChanged" AutoPostBack="true" required="required" CssClass="required form-control" Style="max-width: 90%;">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle Width="190px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Side">
                                <ItemTemplate>
                                    <asp:Label ID="Side" runat="server" Text='<%# Eval("SideName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSide" runat="server" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="R.D (ft) at Channel">
                                <ItemTemplate>
                                    <asp:Label ID="ChannelRD" runat="server" Text='<%# Eval("ChannelRD") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblChannelRD" CssClass="hidden" runat="server" Text='<%# Eval("ChannelRD") %>'></asp:Label>
                                    <asp:Panel ID="pnlChannelRD" runat="server">
                                        <asp:TextBox ID="txtLeftChannelRD" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                    <asp:TextBox ID="txtRightChannelRD" oninput="CompareRDValues(this)" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </asp:Panel>
                                </EditItemTemplate>
                                <ItemStyle Width="175px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="R.D (ft) at Parent or Feeder Channel">
                                <ItemTemplate>
                                    <asp:Label ID="ParentFeederChannelRD" runat="server" Text='<%# Eval("ParentFeederChannelRD") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblParentFeederChannelRD" CssClass="hidden" runat="server" Text='<%# Eval("ParentFeederChannelRD") %>'></asp:Label>
                                    <asp:Panel ID="pnlParentFeederRD" runat="server">
                                        <asp:TextBox ID="txtLeftParentFeederRD" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                    <asp:TextBox ID="txtRightParentFeederRD" oninput="CompareRDValues(this)" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;">
                                    </asp:TextBox>
                                    </asp:Panel>
                                </EditItemTemplate>
                                <ItemStyle Width="275px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                        <asp:Button ID="Add" runat="server" Text="" CommandName="Add" visible="<%# base.CanAdd %>" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlActionParentFeederChannel" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnEditParentFeederChannel" runat="server" Text="" CommandName="Edit" visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                        <asp:Button ID="lbtnDeleteParentFeederChannel" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" visible="<%# base.CanDelete %>"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionParentFeederChannel" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveGaugeInformation" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelParentFeederChannel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
            <br />
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
