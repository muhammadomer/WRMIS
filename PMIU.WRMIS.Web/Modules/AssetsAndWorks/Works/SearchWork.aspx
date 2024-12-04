

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SearchWork.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Works.SearchWork" %>

<%@ MasterType VirtualPath="~/Site.Master" %> 

<asp:content id="ClosureWorkPlan_Screen" contentplaceholderid="MainContent" runat="server">     
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Asset Work  Search</h3>
                </div>
                <div class="box-content"> 
                    <div class="form-horizontal">
                        <div class="row" id="searchDiv">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control"  AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            </div>
                        <div class="row" >
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-control" >
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-control">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>                            
                        </div>  

                        <div class="row" >
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Work Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlWorkType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged"  CssClass="form-control" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Work Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlWorkStatus" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>                            
                        </div>
                        <div class="row" >
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Progress Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlProgressStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-control" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Work Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkName" runat="server"   CssClass="form-control" MaxLength="150"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>                            
                        </div>
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" ToolTip="Search" />  
                                    <asp:HyperLink ID="btnAddWork" runat="server" CssClass="btn btn-success" Text="Add New" NavigateUrl='/Modules/AssetsAndWorks/Works/AddWork.aspx' ToolTip="Add"> </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvWork" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" 
                                        OnPageIndexChanging="gvWork_PageIndexChanging" OnRowDeleting="gvWork_RowDeleting"  
                                        OnPageIndexChanged="gvWork_PageIndexChanged" OnRowCommand="gvWork_RowCommand" EmptyDataText="No record found" 
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" 
                                        ShowHeaderWhenEmpty="true" OnRowDataBound="gvWork_RowDataBound" OnRowCreated="gvWork_RowCreated">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Financial Year">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("FinancialYear") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Work Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("WorkType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Work Name" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWorkName" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estimated Cost (Rs.)" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEstimateCost" runat="server" Text='<%# Eval("EstimateCost","{0:#,##0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                                   <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                <ItemStyle Width=".7%" />
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Work Status" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contractor Name" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContractorName" runat="server" Text='<%# Eval("ContractorName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contractor Amount (Rs.)" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContractorAmount" runat="server" Text='<%# Eval("ContractorAmount","{0:#,##0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1 text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                                   <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUn" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                <ItemStyle Width=".7%" />
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Progress Percentage (XEN)" >
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblProgressStatus" runat="server" Text='<%# Eval("ProgressStatus") %>'></asp:Label>--%>
                                                    <asp:Label ID="lblProgressStatus" runat="server" Text='<%# Eval("ProgressPercentage") %>'></asp:Label>
                                                    
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1 text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action">
                                                <HeaderStyle CssClass="col-md-2 text-center" /> 
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                         <asp:HyperLink ID="hlWorkItemDetail" runat="server"  ToolTip="Work Detail" 
                                                            CssClass="btn btn-primary btn_24 view"  Text="" 
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/AssetsAndWorks/Works/AddWork.aspx?View={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:HyperLink ID="hlEdit" runat="server"  ToolTip="Edit Work Detail" Visible="<%# base.CanEdit %>" 
                                                            CssClass="btn btn-primary btn_24 edit"  Text="" 
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/AssetsAndWorks/Works/AddWork.aspx?Eidt={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:HyperLink ID="hlWorkItems" runat="server" ToolTip="Work Items" 
                                                            CssClass="btn btn-primary btn_24 audit"  Text="" 
                                                                NavigateUrl='<%# Eval("ID", "~/Modules/AssetsAndWorks/Works/WorkItem.aspx?CWID={0}")%>'>
                                                            <%--NavigateUrl='<%# Eval("~/Modules/AssetsAndWorks/Works/AddWorkItem.aspx")%>'--%>
                                                        </asp:HyperLink>
                                                        <asp:Button ID="btnPublish" runat="server" CommandName="Publish" CssClass="btn btn-primary btn_32 publish" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to Publish this work?');"  ToolTip="Publish"></asp:Button>    
                                                        <asp:Button ID="btnUnPublish" runat="server" CommandName="Unpublish" CssClass="btn btn-primary btn_32 publish" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to Un-Publish this work?');"  ToolTip="Un-Publish" Visible="false"></asp:Button>                                                    
                                                        <asp:HyperLink ID="hlProgress" runat="server" ToolTip="Progress" 
                                                            CssClass="btn btn-primary btn_24 AddPrgs"  Text="" 
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/AssetsAndWorks/Works/AddWorkProgress.aspx?AWID={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:HyperLink ID="hlProgressHistory" runat="server" ToolTip="Progress History" 
                                                            CssClass="btn btn-primary btn_24 Prgs"  Text="" 
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/AssetsAndWorks/Works/WorkProgessHistory.aspx?AWID={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' Visible='<%# base.CanDelete %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                     
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
    <script type="text/javascript" src="../../Scripts/jquery-1.10.2.min.js"></script> 
</asp:content>
