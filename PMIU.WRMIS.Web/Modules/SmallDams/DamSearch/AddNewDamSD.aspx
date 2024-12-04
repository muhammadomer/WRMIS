<%@ Page Title="AddNewDamSD" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddNewDamSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.AddNewDamSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnDamTypeID" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 id="MainHeading" runat="server">Add New Dam</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <asp:Panel ID="Panel1" runat="server" TabIndex="0" GroupingText="Basic Information">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDamName" runat="server" Text="Dam Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtDamName" runat="server" MaxLength="30" required="true" CssClass="required form-control" TabIndex="1"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDamType" runat="server" Text="Dam Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDamType" runat="server" CssClass="required  form-control" required="True" TabIndex="2">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblCostofProject" runat="server" Text="Cost of Project (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtCostofProject" pattern="^[0-9]*$" runat="server" minlength="1" MaxLength="15" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblYearofCompletion" runat="server" Text="Completion Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtYearofCompletion" runat="server" pattern="^[0-9]*$" MaxLength="4" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label" style="padding-top:1%;">Status</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" TabIndex="5">
                                                <asp:ListItem class="radio-inline" Selected="True" Value="1" Text="Operational"/>
                                                <asp:ListItem class="radio-inline" Value="0" style="margin-left: 15px;" Text="Non-Operational"/>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control  commentsMaxLengthRow multiline-no-resize" MaxLength="250"  TextMode="MultiLine" Rows="5" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" TabIndex="7" GroupingText="Physical Location">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDivision" AutoPostBack="true" runat="server" CssClass="required  form-control" required="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" TabIndex="7">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlSubDivision" runat="server" CssClass="required  form-control" required="True" TabIndex="8">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDistrict" runat="server" Text="District" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDistrict" AutoPostBack="true" runat="server" CssClass="required form-control" required="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" TabIndex="9">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblTehsil" runat="server" Text="Tehsil" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlTehsil" AutoPostBack="true" runat="server" CssClass="required form-control" required="true" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged" TabIndex="10">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblNearestVillage" runat="server" Text="Village" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlNearestVillage" runat="server" CssClass="required form-control" required="true" TabIndex="11">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblPassingbyimportantroad" runat="server" Text="Important Road" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtPassingbyimportantroad" pattern="^[0-9a-zA-Z ]+$" runat="server" MaxLength="100" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtLocation" runat="server"  MaxLength="100" CssClass="form-control" TabIndex="13"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblStreamNullah" runat="server" Text="Stream/Nullah" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtStreamNullah" runat="server" pattern="^[0-9a-zA-Z ]+$" MaxLength="100" CssClass="form-control" TabIndex="14"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblNA" runat="server" Text="NA" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtNA" runat="server" pattern="^[0-9a-zA-Z ]*$" MaxLength="25" CssClass="form-control" TabIndex="15"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblPP" runat="server" Text="PP" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtPP" runat="server" pattern="^[0-9a-zA-Z ]*$" MaxLength="25" CssClass="form-control" TabIndex="16"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblUCC" runat="server" Text="UC" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtUCC" runat="server" pattern="^[0-9a-zA-Z ]*$" MaxLength="25" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" TabIndex="18" />
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="/Modules/SmallDams/DamSearch/SearchSD.aspx?ShowHistroy=1" CssClass="btn btn-default" TabIndex="19">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
