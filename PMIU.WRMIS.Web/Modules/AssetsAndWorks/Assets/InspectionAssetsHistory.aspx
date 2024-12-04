<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionAssetsHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets.InspectionAssetsHistory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register TagPrefix="ucAssets" TagName="Assets" Src="~/Modules/AssetsAndWorks/UserControls/AssetsDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Asset Inspection History</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
                    </div>
                </div>
                <div class="box-content">
                    <ucAssets:Assets runat="server" ID="AssetsDetail" />
                    <div class="table-responsive">
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="hidden">
                                    <asp:HiddenField ID="hdnAssetsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedBy" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id="divDate">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtFromDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id="div1">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtToDate" TabIndex="4"  runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">

                                        <div class="form-group">
                                            <asp:Label ID="lblinspectedby" runat="server" Text="Inspected By" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlInspectedBy" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6" runat="server" id="DivStatus" visible="true">
                                        <div class="form-group">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn" style="margin-left: 26px;">
                                            <asp:Button runat="server" ID="btnShow" CssClass="btn btn-primary" Text="&nbsp;Show" OnClick="btnShow_Click" />
                                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="DivInd" visible="false">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvInspectionHistoryIndividual" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="AssetInspectionIndID" PageSize="10"
                                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvInspectionHistoryIndividual_PageIndexChanging"
                                                OnPageIndexChanged="gvInspectionHistoryIndividual_PageIndexChanged" OnRowCommand="gvInspectionHistoryIndividual_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetInspectionIndID" runat="server" Text='<%# Eval("AssetInspectionIndID") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inspection Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInspectionDate" runat="server" Text='<%#Eval("InspectionDate") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inspected By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInspectedBy" runat="server" Text='<%#Eval("InspectedBy") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Condition">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCondition" runat="server" Text='<%#Eval("Condition") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="HeaderAction">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkInspectionInd" runat="server" ToolTip="Detail" CommandName="InspectionInd" CommandArgument='<%# Eval("AssetInspectionIndID") %>' CssClass="btn btn-primary btn_32 view">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1 text-right" />

                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="DivLot" visible="false">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvInspectionHistoryLot" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="AssetInspectionLotID"
                                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvInspectionHistoryLot_PageIndexChanging"
                                                OnPageIndexChanged="gvInspectionHistoryLot_PageIndexChanged" OnRowCommand="gvInspectionHistoryLot_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetInspectionLotID" runat="server" Text='<%# Eval("AssetInspectionLotID") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inspection Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInspectionDate" runat="server" Text='<%#Eval("InspectionDate") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inspected By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInspectedby" runat="server" Text='<%#Eval("InspectedBy") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="HeaderAction">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkInspectionLot" runat="server" ToolTip="Detail" CommandName="InspectionLot" CommandArgument='<%# Eval("AssetInspectionLotID") %>' CssClass="btn btn-primary btn_32 view">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1 text-right" />

                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

