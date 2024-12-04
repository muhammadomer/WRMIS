<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddAssets.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets.AddAssets" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .integerInput {
            text-align: left;
        }
    </style>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Add New Asset</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnAssetID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedBy" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblAssetName" runat="server" Text="Asset Name/ID" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtAssetName" runat="server" required="required" CssClass="form-control required" MaxLength="30"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCategory" runat="server" required="required" CssClass="form-control required" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblSubCategory" runat="server" Text="Sub-Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubCategory" runat="server" required="required" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Book Value (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtEstimatedvalue" runat="server" CssClass="integerInput form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Associate with Flood Operations" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdAssociatFlood" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rdAssociatFlood_SelectedIndexChanged">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Asset Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdAssetType" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rdAssetType_SelectedIndexChanged">
                                            <asp:ListItem Value="2" Selected="True">Individual Item</asp:ListItem>
                                            <asp:ListItem Value="1">Lot</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="div_QtyUnits" visible="false">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Quantity" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtQty" runat="server" required="required" CssClass="integerInput form-control required" MaxLength="8"></asp:TextBox>
                                    </div>
                                    <%--<div class="col-md-1 controls">
                                        <asp:Label ID="Label5" runat="server" Text="Units" CssClass="col-sm-4 col-lg-3 control-label" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtUnits" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    </div>--%>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Units" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtUnits" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <h3>Physical Location</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblLevel" runat="server" Text="Level" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlLevel" runat="server" AutoPostBack="True" required="required" CssClass="form-control required" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged">
                                            <%--<asp:ListItem Value="">All</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass=" rquired form-control">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lbloffice" runat="server" Text="Office" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlOffice" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row" runat="server" id="DivAssetDetail" visible="false">
                            <div class="col-md-12">
                                <h3>Asset Details</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>

                            <div class="col-md-6">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvAssetAttribute" runat="server" DataKeyNames="AssetAttributeID,AttributeID,AttributeName,AttributeDataType,AttributeValue,CreatedBy,CreatedDate"
                                        CssClass="table header" AutoGenerateColumns="False" ShowHeader="false"
                                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvAssetAttribute_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%# Eval("AttributeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAttributeValue" runat="server" class="form-control" MaxLength="30" Text='<%# Eval("AttributeValue") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <!-- END Main Content -->
    <%--    Text='<%# Eval("AttributeValue") %>'--%>
</asp:Content>

