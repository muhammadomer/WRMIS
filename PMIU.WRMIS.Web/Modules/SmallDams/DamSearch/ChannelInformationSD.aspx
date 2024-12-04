<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelInformationSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.ChannelInformationSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDamNameType" TagName="DamNameType" Src="~/Modules/SmallDams/Controls/DamNameType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSmallDamID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />


    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 id="MainHeading" runat="server">Add Channel Information</h3>
                </div>
                <div class="box-content">
                    <ucDamNameType:DamNameType runat="server" ID="DamNameType" />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblChannelCode" runat="server" Text="Channel Code" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtChannelCode" runat="server" required="true" CssClass="required form-control" MaxLength="7" pattern="^[0-9A-Za-z-]*$"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblChannelName" runat="server" Text="Channel Name" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtChannelName" runat="server" required="true" CssClass="required form-control" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblChannelCapacity" runat="server" Text="Capacity (Cusec)" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtChannelCapacity" runat="server" CssClass="form-control" pattern="^[0-9]*$" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDesignedCCA" runat="server" Text="CCA (Acres)" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtDesignedCCA" runat="server" CssClass="form-control" pattern="^[0-9]*$" MaxLength="7"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblParentType" runat="server" Text="Parent Type" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:DropDownList ID="ddlParentType" runat="server" AutoPostBack="true" required="true" CssClass="required form-control" OnSelectedIndexChanged="ddlParentType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblParent" runat="server" Text="Dam/Channel" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:DropDownList ID="ddlParent" runat="server" required="true" CssClass="required form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblOffTakingSide" runat="server" Text="Off-Taking Side" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:DropDownList ID="ddlOffTakingSide" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblOffTakingRD" runat="server" Text="Off-Taking RD" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls" style="display:flex;">
                                        <asp:TextBox ID="txtOffTakingRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="text-align:left" ></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtOffTakingRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="text-align:left"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblAuthorizedGauge" runat="server" Text="Authorized Gauge (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtAuthorizedGauge" runat="server" CssClass="form-control" MaxLength="5" pattern="^[0-9]*$"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblTailRD" runat="server" Text="Tail RD" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls" style="display:flex;">
                                        <asp:TextBox ID="txtTailRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="text-align:left"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtTailRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="text-align:left"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblMaxGaugeValue" runat="server" Text="Max Gauge (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtMaxGaugeValue" runat="server" CssClass="form-control decimal2PInput required" Style="text-align:left" required="true" Width="100%" oninput="javascript:ValueValidation(this,'0.00','30.00')" MaxLength="5" pattern="^[0-9.]*$"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblMaxDischargeValue" runat="server" Text="Max Discharge (cusec)" CssClass="col-sm-4 col-lg-5 control-label" />
                                    <div class="col-sm-8 col-lg-5 controls">
                                        <asp:TextBox ID="txtMaxDischargeValue" runat="server" CssClass="form-control decimal2PInput required" Style="text-align:left" required="true" Width="100%" oninput="javascript:ValueValidation(this,'0.00','99999.00')" MaxLength="8" pattern="^[0-9.]*$"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-5 control-label" style="padding-top:1%;">Status</label>
                                    <div class="col-sm-8 col-lg-7 controls">
                                        <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                            <asp:ListItem class="radio-inline" Selected="True" Value="1" Text="Active" />
                                            <asp:ListItem class="radio-inline" Value="0" style="margin-left: 15px;" Text="InActive" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
