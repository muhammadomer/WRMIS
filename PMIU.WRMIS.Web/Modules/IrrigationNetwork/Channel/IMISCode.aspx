<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IMISCode.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.IMISCode" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/IrrigationNetwork/Controls/ChannelDetails.ascx" TagPrefix="ucChannelDetail" TagName="ChannelDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>IMIS Code</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucChannelDetail:ChannelDetails runat="server" ID="ChannelDetails" />
            <br />
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label id="lblParentFeeder" class="col-sm-4 col-lg-3 control-label">Parent Feeder</label>
                            <div class="col-sm-8 col-lg-8 controls">
                                <asp:DropDownList TabIndex="1" ID="ddlParentFeeder" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlParentFeeder_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem>Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvIMIS" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="ChannelID" runat="server" Text='<%# Eval("ParentFeederChannelID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channels ">
                                                <ItemTemplate>
                                                    <asp:Label ID="ChannelName" runat="server" Text='<%#Eval("ChannelName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="160px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IMIS Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="ChannelType" runat="server" Text='<%# Eval("IMISCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <%--<PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />--%>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblIMISCode" class="col-sm-4 col-lg-3 control-label" Visible="False">IMIS Code</asp:Label>
                            <asp:Label runat="server" ID="lblIMISFirstThree" class="control-label"></asp:Label>
                            <asp:TextBox ID="txtIMISCode" runat="server" Style="display: inline" Width="35px" CssClass="form-control required integerInput" oninput="InputWithLengthValidation(this,'3');" required="true" Visible="False" MaxLength="3"></asp:TextBox>
                            <asp:TextBox ID="txtEscapeChannel" runat="server" Style="display: inline" Width="35px" CssClass="form-control required integerInput" oninput="InputWithLengthValidation(this,'2');" required="true" Visible="False" MaxLength="2"></asp:TextBox>
                            <asp:Label runat="server" ID="lblIMISLast" class="control-label"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" Visible="<%# base.CanAdd %>" />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
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
