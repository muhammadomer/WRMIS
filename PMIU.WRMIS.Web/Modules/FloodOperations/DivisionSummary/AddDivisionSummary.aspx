<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddDivisionSummary.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary.AddDivisionSummary" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hdnDivisionSummaryID" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
            <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
            <div class="box">
                <div class="box-title">
                    <h3 runat="server" id="h3PageTitle">Division Summary Basic Information</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                    <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" />--%>
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/FloodOperations/DivisionSummary/SearchDivisionSummary.aspx" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
