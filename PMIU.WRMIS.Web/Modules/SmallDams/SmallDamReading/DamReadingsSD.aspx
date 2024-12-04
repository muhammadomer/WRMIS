<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DamReadingsSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading.DamReadingsSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDamReadingsData" TagName="DamReadingsData" Src="~/Modules/SmallDams/Controls/DamReadingsData.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnDamID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDate" runat="server" Value="0" />
   

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Small Dam Readings</h3>
                </div>
                <div class="box-content">
                    <ucDamReadingsData:DamReadingsData runat="server" id="DamReadingsData" />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDamLevel" runat="server" Text="Level (ft)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDamLevel" runat="server" CssClass="form-control decimal2PInput " Style="text-align:left;" MaxLength="7" Width="100%" oninput="javascript:ValueValidation(this,'0.00','3000')"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblLiveStorage" runat="server" Text="Live Storage (Aft)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtLiveStorage" runat="server" CssClass="form-control decimal2PInput" Style="text-align:left;" oninput="javascript:ValueValidation(this,'0.00','50000')"  MaxLength="5"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDischarge" runat="server" Text="Discharge (Cusec)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Style="text-align:left;"  MaxLength="5" oninput="javascript:ValueValidation(this,'0.00','99999')"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblGaugeReader" runat="server" Text="Gauge Reader" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtGaugeReader" runat="server" CssClass="form-control" pattern="^[a-zA-Z ]+$" onfocus="this.value = this.value;" Onkeyup="InputValidationText(this);" MaxLength="75"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                                        <asp:Label ID="lblCreatedDate" runat="server" CssClass="form-control" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="/Modules/SmallDams/SmallDamReading/SmallDamReadingsSD.aspx?ShowHistroy=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                                <div class="row">
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
