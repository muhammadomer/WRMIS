<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StonePositionOnsiteMonitoring.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring.StonePositionOnsiteMonitoring" %>

<%@ Register Src="~/Modules/FloodOperations/Controls/OMDetail.ascx" TagPrefix="uc1" TagName="OMDetail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Stone Position Monitoring for Onsite Monitoring</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="tbl-info">
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="YearText" Text="Year" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="ZoneText" Text="Zone" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="CircleText" Text="Circle" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="DivisionText" Text="Division" Font-Bold="True" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="lblYear" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblZone" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblCircle" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="lblinf" Text="Infrastructure Name" Font-Bold="True" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-md-3">
                            <asp:Label ID="lblinfraname" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <br/>
                <%--<uc1:OMDetail runat="server" ID="OMDetail" />--%>
                <div class="table-responsive">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvStonePosition" runat="server" DataKeyNames="SDID,QtyOfStone,RD" CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True" OnRowDataBound="gvStonePosition_RowDataBound" OnRowEditing="gvStonePosition_RowEditing"
                                OnRowCancelingEdit="gvStonePosition_RowCancelingEdit" OnRowUpdating="gvStonePosition_RowUpdating" OnPageIndexChanging="gvStonePosition_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" CssClass="control-label" Text='<%# Eval("RD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"></ItemStyle>
                                        <HeaderStyle CssClass="col-md-1 text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deployed Quantity (‘000 cft)">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DeployQty" runat="server" Text='<%# Eval("QtyOfStone") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Onsite Quantity (‘000 cft)">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldep" runat="server" CssClass="control-label" Text='<%# Eval("OnSiteQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtonsiteQty" runat="server" CssClass="rquired form-control integerInput" pattern="^(0|[0-9][0-9]*)$" required="true" MaxLength="8" Text='<%# Eval("OnSiteQty") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Onsite Balance Quantity (‘000 cft)">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_BalnceQty" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionInfrastructures" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEdit" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                <asp:Button ID="btnDelete" runat="server" Text="" CommandName="Delete" Visible="false" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditActionInfrastructures" runat="server" HorizontalAlign="Center">
                                                <%-- <asp:Button ID="Button1" runat="server"  CssClass="btn btn-primary btn_32 view-feedback" ToolTip="Items" Enabled="false" />--%>
                                                <asp:Button runat="server" ID="btnSave" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                <asp:Button ID="btnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
