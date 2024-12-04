<%@ Page Title="TechnicalParametersSD" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TechnicalParametersSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.TechnicalParametersSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<%@ Register TagPrefix="ucDamNameType" TagName="DamNameType" Src="~/Modules/SmallDams/Controls/DamNameType.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSmallDamID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTechParaID" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Technical Parameters</h3>
                </div>
                <div class="box-content">
                    <ucDamNameType:DamNameType runat="server" ID="DamNameType" />
                    <div class="form-horizontal">
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Design Parameters">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblMaximumHeight" runat="server" Text="Maximum Height (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtMaximumHeight" runat="server" CssClass="form-control decimal2PInput text-right" Width="100%" oninput="javascript:ValueValidation(this,'0.00','1500.00')" MaxLength="7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDamLength" runat="server" Text="Dam Length (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtDamLength" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right" oninput="javascript:ValueValidation(this,'0','30000')" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblTotalRLDam" runat="server" Text="Top RL of Dam (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtTotalRLDam" runat="server" pattern="^[0-9.]*$" CssClass="form-control decimal2PInput  text-right" Width="100%" oninput="javascript:ValueValidation(this,'0.00','3000.00')" MaxLength="7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblCatchmentArea" runat="server" Text="Catchment Area (Sq Miles)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtCatchmentArea" runat="server" required="true" pattern="^[0-9.]*$" CssClass="required form-control text-right decimal2PInput" oninput="javascript:ValueValidation(this,'0.00','50000')" MaxLength="8"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblGrossStorageCapacity" runat="server" Text="Gross Storage Capacity (Aft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtGrossStorageCapacity" runat="server" CssClass="form-control text-right" pattern="^[0-9]*$" oninput="javascript:ValueValidation(this,'0','50000')" MaxLength="5"></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlGrossStorageCapacity" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblLiveStorageCapacity" runat="server" Text="Live Storage Capacity (Aft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtLiveStorageCapacity" runat="server" required="true" pattern="^[0-9.]*$" CssClass="required form-control text-right decimal2PInput" oninput="javascript:ValueValidation(this,'0.00','50000')" MaxLength="8"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblNormalPondLevel" runat="server" Text="Normal Pond Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtNormalPondLevel" runat="server" pattern="^[0-9.]*$" CssClass="form-control decimal2PInput text-right" Width="100%" oninput="javascript:ValueValidation(this,'0.00','3000.00')" MaxLength="7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblHighFloodLevel" runat="server" Text="High Flood Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtHighFloodLevel" runat="server" pattern="^[0-9.]*$" CssClass="form-control decimal2PInput text-right" Width="100%" oninput="javascript:ValueValidation(this,'0.00','3000.00')" MaxLength="7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDesignedCCA" runat="server" Text="Designed CCA (Acres)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtDesignedCCA" runat="server" CssClass="form-control text-right" pattern="^[0-9]*$" oninput="javascript:ValueValidation(this,'0','50000')" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>


                        <asp:Panel ID="Panel2" runat="server" GroupingText="Spillway Parameters">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblType" runat="server" Text="Type" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblLength" runat="server" Text="Length (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtLength" runat="server" CssClass="form-control text-right decimal2PInput" pattern="^[0-9.]*$" oninput="javascript:ValueValidation(this,'0.00','25000')" MaxLength="8"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDesignDischarge" runat="server" Text="Design Discharge (Cusecs)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtDesignDischarge" runat="server" CssClass="form-control text-right" pattern="^[0-9]*$" oninput="javascript:ValueValidation(this,'0','99999')" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblClearWaterWay" runat="server" Text="Clear Water Way (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtClearWaterWay" runat="server" CssClass="form-control text-right decimal2PInput" pattern="^[0-9.]*$" oninput="javascript:ValueValidation(this,'0.00','25000')" MaxLength="8"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </asp:Panel>

                        <asp:Panel ID="Panel3" runat="server" GroupingText="Other Parameters">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblWaterSupply" runat="server" Text="Water Supply" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:TextBox ID="txtWaterSupply" runat="server" CssClass="form-control commentsMaxLengthRow" pattern="^[0-9a-zA-Z ]*$" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblLandAcquirePond" runat="server" Text="Land Acquire for Pond(A-K-M)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls" style="display:flex;">
                                            <asp:TextBox ID="txtLandAcquirePondA" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right"  oninput="javascript:ValueValidation(this,'0','9000')" MaxLength="4"></asp:TextBox>
                                            -
                                            <asp:TextBox ID="txtLandAcquirePondK" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right"  oninput="javascript:ValueValidation(this,'0','8')" MaxLength="1"></asp:TextBox>
                                            -
                                            <asp:TextBox ID="txtLandAcquirePondM" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right"  oninput="javascript:ValueValidation(this,'0','20')" MaxLength="2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblLandAcquireChannel" runat="server" Text="Land Acquire for Channel(A-K-M)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls" style="display:flex;">
                                            <asp:TextBox ID="txtLandAcquireChannelA" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right" oninput="javascript:ValueValidation(this,'0','9000')" MaxLength="4"></asp:TextBox>
                                            - 
                                            <asp:TextBox ID="txtLandAcquireChannelK" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right" oninput="javascript:ValueValidation(this,'0','8')" MaxLength="1"></asp:TextBox>
                                            -
                                            <asp:TextBox ID="txtLandAcquireChannelM" runat="server" pattern="^[0-9]*$" CssClass="form-control text-right" oninput="javascript:ValueValidation(this,'0','20')" MaxLength="2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save " OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
