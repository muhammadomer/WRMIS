<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddFFPStonePosition.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.AddFFPStonePosition" %>

<%@ Register Src="~/Modules/FloodOperations/Controls/FFPDetail.ascx" TagPrefix="uc1" TagName="FFPDetail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .no-padding-lr {
            padding: 0px !important;
        }
    </style>
    <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFFPStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Stone Position For Flood Fighting Plan</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">
            <uc1:FFPDetail runat="server" ID="FFPDetail" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvFFPStonePosition" runat="server" DataKeyNames="FFPStonePositionID,FloodInspectionDetailID,IStonePositonID,InfrastructureTypeID,InfrastructureType,InfrastructureName,RD,RequiredQty,AvailableQty,TotalQty,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowDataBound="gvFFPStonePosition_RowDataBound" OnRowCommand="gvFFPStonePosition_RowCommand"
                            OnRowEditing="gvFFPStonePosition_RowEditing" OnRowCancelingEdit="gvFFPStonePosition_RowCancelingEdit"
                            OnRowUpdating="gvFFPStonePosition_RowUpdating"
                            OnRowDeleting="gvFFPStonePosition_RowDeleting" OnPageIndexChanging="gvFFPStonePosition_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresType" runat="server" Text='<%# (Convert.ToString(Eval("InfrastructureType"))) == "Control Structure1" ? "Barrage/Headwork": Eval("InfrastructureType") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:DropDownList ID="ddlInfrastructuresType" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructuresType_SelectedIndexChanged" runat="server" required="required" class="required form-control"></asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlInfrastructuresType" AutoPostBack="true" runat="server" required="required" class="required form-control" OnSelectedIndexChanged="ddlInfrastructuresType_SelectedIndexChanged"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructuresName" runat="server" Text='<%#Eval("InfrastructureName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInfrastructuresName" runat="server" required="required" class="required form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRD" runat="server" CssClass="control-label" Text='<%# Eval("RD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlChannelRD" runat="server">
                                            <asp:TextBox ID="txtRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                            +
                                        <asp:TextBox ID="txtRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Available Quantity<br/>(‘000 cft) (A)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableQty" runat="server" CssClass="control-label" Text='<%# Eval("AvailableQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Net Quantity Required<br/>(‘000 cft) (B)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequiredQty" runat="server" CssClass="control-label" Text='<%# Eval("RequiredQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRequiredQty" runat="server" CssClass="form-control integerInput required" required="true" MaxLength="256" pattern="^[a-zA-Z0-9 ]+$" Text='<%# Eval("AvailableQty") %>' Width="100%" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center no-padding-lr" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Quantity<br/>(‘000 cft) (A+B)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalQty" runat="server" CssClass="control-label" Text='<%# Eval("TotalQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddFFPStonePosition" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddFFPStonePosition" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <asp:Panel ID="pnlActionFFPStonePosition" runat="server" HorizontalAlign="Center">

                                            <asp:Button ID="btnEditFFPStonePosition" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteFFPStonePosition" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionFFPStonePosition" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveFFPStonePosition" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelFFPStonePosition" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
