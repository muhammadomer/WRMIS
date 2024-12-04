<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchAssets.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets.SearchAssets" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Assets Search</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="box-content">
                <div class="form-horizontal">


                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblLevel" runat="server" Text="Level" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged">
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
                                        <asp:ListItem Value="">All</asp:ListItem>
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
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass=" rquired form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
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
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblYear" runat="server" Text="Asset Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblSubCategory" runat="server" Text="Sub-Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblFAssociation" runat="server" Text="Flood Association" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlFAssociation" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
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
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblAssetName" runat="server" Text="Asset Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtAssetName" runat="server" MaxLength="30" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                                <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/AssetsAndWorks/Assets/AddAssets.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAssets" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="AssetsID,AssetType,Status,CreatedBy"
                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" PageSize="10" CellSpacing="-1" GridLines="None"
                                OnPageIndexChanging="gvAssets_PageIndexChanging" OnPageIndexChanged="gvAssets_PageIndexChanged" OnRowDeleting="gvAssets_RowDeleting" OnRowCommand="gvAssets_RowCommand" OnRowDataBound="gvAssets_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetsID" runat="server" Text='<%# Eval("AssetsID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetName" runat="server" Text='<%#Eval("AssetName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CategoryName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub-Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCategory" runat="server" Text='<%#Eval("SubCategoryName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetLevel" runat="server" Text='<%#Eval("IrrigationLevelID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Flood Association">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFOBit" runat="server" Text='<%#(Boolean.Parse(Eval("FOAssociation").ToString())) ? "Yes" : "No" %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("AssetType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="HeaderAction" HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnInspection" runat="server" ToolTip="Inspection" CommandName="Inspection" CommandArgument='<%# Eval("AssetsID") %>' CssClass="btn btn-primary btn_32 AddPrgs"></asp:Button>
                                            <asp:Button ID="btnInspectionHistory" runat="server" ToolTip="Inspection History" CommandName="InspectionHistory" CommandArgument='<%# Eval("AssetsID") %>' CssClass="btn btn-primary btn_32 Prgs"></asp:Button>
                                              <asp:HyperLink ID="hlView" runat="server" ToolTip="View"
                                                CssClass="btn btn-primary btn_24 view"
                                                NavigateUrl='<%# Eval("AssetsID","~/Modules/AssetsAndWorks/Assets/AddAssets.aspx?View={0}")%>'>
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("AssetsID","~/Modules/AssetsAndWorks/Assets/AddAssets.aspx?AssetsID={0}")%>'>
                                            </asp:HyperLink>
                                            <asp:Button ID="ButtonDelete" Visible="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-center" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnAssetsID" runat="server" Value="0" />
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
