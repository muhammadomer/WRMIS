<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfrastructureParent.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.InfrastructureParent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnProtectioninfrastructure" runat="server" Value="0" />
    <asp:HiddenField ID="hdnParentid" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Parent Name of Protection Infrastructure</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <!-- BEGIN Left Side -->
                                    <div class="form-group">
                                        <asp:Label ID="lblParentType" runat="server" Text="Parent Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlParentType" runat="server" CssClass="input required form-control" required="required" data-errormessage-value-missing="This field is required" AutoPostBack="True" OnSelectedIndexChanged="ddlParentType_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblParentName" runat="server" Text="Parent Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlParentName" runat="server" CssClass="input required form-control" required="required" data-errormessage-value-missing="This field is required">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <!-- END Left Side -->
                                </div>
                                <div class="col-md-6 ">
                                    <!-- BEGIN Right Side -->
                                    <div class="form-group">
                                        <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" Enabled="false">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblOffTakingRD" runat="server" Text="Offtaking RD" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtTotalOfftakingLeftRD" runat="server" placeholder="Total RD" CssClass="integerInput RDMaxLength required form-control" required="required"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1 col-lg-1 controls">
                                            +
                                        </div>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtTotalOfftakingRightRD" runat="server" placeholder="Total RD" CssClass="integerInput RDMaxLength required form-control" required="required" oninput="CompareRDValues(this)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- END Right Side -->
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
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
