<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNew.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure.AddNew" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/ControlledInfrastructure/YearValidations.js"></script>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Barrage / Headwork</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnControlInfrastructureID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                            <%--<asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />--%>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="ddlControlInfrastructureType" ID="lblControlInfrastructureType" Text="Type" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlControlInfrastructureType" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtName" ID="lblName" Text="Name" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="input required form-control" required="required" pattern="[0-9a-zA-Z.\.\-_:/,!\(\)\[\]\{\}].{2,250}" title="Name must be of minim 2 characters which is upto 250 characters" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="ddlRiver" ID="lblRiver" Text="River" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlRiver" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="ddlProvince" ID="lblProvince" Text="Province" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtYearofConstruction" ID="lblYearofConstruction" Text="Year of Construction" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtYearofConstruction" runat="server" CssClass="integerInput form-control" Style="text-align: left" onblur="yearValidation(this.value,event)" onkeypress="yearValidation(this.value,event)" MaxLength="4"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlYearofConstruction" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>--%>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 text-left form-group">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-xs-4 col-lg-4 control-label" />
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                        <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">InActive</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 text-left form-group"></div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-left form-group">
                                <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="col-xs-2 col-lg-2 control-label" Style="padding: 0px 1.9% 0 0;" />
                                <div class="col-lg-10 col-sm-10 controls" style="padding: 0px 4% 0 1%;">
                                    <asp:TextBox TextMode="MultiLine" Height="100" ID="txtDescription" runat="server" class="input form-control" pattern=".{0,250}" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~\Modules\IrrigationNetwork\ReferenceData\ControlledInfrastructure\Search.aspx" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
