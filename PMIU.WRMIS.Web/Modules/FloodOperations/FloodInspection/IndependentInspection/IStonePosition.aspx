<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IStonePosition.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.IStonePosition" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Stone Position for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvStonePosition" runat="server" DataKeyNames="IStonePositionID,RD,TotalRD,BeforeFloodQty,AvailableQty,ConsumedQty,CreatedDate,CreatedBy"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvStonePosition_RowCommand" OnRowDataBound="gvStonePosition_RowDataBound" OnRowEditing="gvStonePosition_RowEditing" OnRowCancelingEdit="gvStonePosition_RowCancelingEdit"
                            OnRowUpdating="gvStonePosition_RowUpdating" OnRowDeleting="gvStonePosition_RowDeleting" OnPageIndexChanging="gvStonePosition_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Registered">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityRegistered" runat="server" Text='<%#Eval("BeforeFloodQty") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Avalible">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityAvalible" runat="server" Text='<%#Eval("AvailableQty") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQuantityAvalible" runat="server" pattern="[0-9]{0,4}" required="required" CssClass=" form-control integerInput required" Text='<%#Eval("AvailableQty") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Consumed">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityConsumed" runat="server" Text='<%#Eval("ConsumedQty") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%--<asp:Panel ID="pnlAddProblemFI" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddProblemFI" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddStonePos" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionStonePos" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditStonePos" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <%--<asp:Button ID="btnDeleteProblemFI" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />--%>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionStonePos" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveStonePos" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelProblemFI" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
</asp:Content>
