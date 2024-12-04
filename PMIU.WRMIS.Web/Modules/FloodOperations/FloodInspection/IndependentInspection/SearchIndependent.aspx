<%@ Page Title="SearchIndependent" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchIndependent.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.SearchIndependent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Independent Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass=" rquired form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInsepectionType" runat="server" Text="Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInspectionType" runat="server" CssClass="form-control">
                                    <%--<asp:ListItem Value="">All</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFromDate" runat="server" Text="Date From" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblToDate" runat="server" Text="Date To" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <div id="divStatus" runat="server" style="display: none">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/FloodInspection/IndependentInspection/AddIndependent.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvSearchIndependent" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="FloodInspectionID,InspectionTypeID,InspectionType,InfrastructureType,InfrastructureStatusID,InfrastructureStatus,InspectionYear,DivisionID,StructureTypeID,StructureID"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvSearchIndependent_PageIndexChanging" OnPageIndexChanged="gvSearchIndependent_PageIndexChanged" OnRowDeleting="gvSearchIndependent_RowDeleting" OnRowDataBound="gvSearchIndependent_RowDataBound" OnRowCommand="gvSearchIndependent_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFloodInspectionID" runat="server" Text='<%# Eval("FloodInspectionID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionDate" runat="server" Text='<%#Eval("InspectionDate", "{0:dd-MMM-yyyy}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionType" runat="server" Text='<%#Eval("InspectionType") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfrastructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfrastructureType") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfrastructureName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFloodInspectionStatus" runat="server" Text='<%#Eval("InfrastructureStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-left" />
                                    <ItemStyle CssClass="text-left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlGeneralCondition" runat="server" ToolTip="General Condition" CssClass="btn btn-primary btn_24 gencond" Text="">
                                        </asp:HyperLink>
                                        <%-- <asp:HyperLink ID="hlGeneralCondition" runat="server" ToolTip="General Condition" CssClass="btn btn-primary btn_24 gencond" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/GCProtectionInfrastructure.aspx?FloodInspectionID={0}&InspectionYear={1}&InspectionTypeID={2}",Eval("FloodInspectionID"), Eval("InspectionYear"), Eval("InspectionTypeID")) %>' Text="">
                                        </asp:HyperLink>--%>
                                        <asp:HyperLink ID="hlRDwisecondition" runat="server" ToolTip="RD wise condition" CssClass="btn btn-primary btn_24 rdwisecond" NavigateUrl='<%# Eval("FloodInspectionID","~/Modules/FloodOperations/FloodInspection/IndependentInspection/RDWiseConditions.aspx?FloodInspectionID={0}") %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlMBStatus" runat="server" ToolTip="M.B Status" CssClass="btn btn-primary btn_24 measbook" NavigateUrl='<%# Eval("FloodInspectionID","~/Modules/FloodOperations/FloodInspection/IndependentInspection/MeasuringBookStatusPreFlood.aspx?FloodInspectionID={0}") %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlProblems" runat="server" ToolTip="Problems" CssClass="btn btn-primary btn_24 prob" NavigateUrl='<%# Eval("FloodInspectionID","~/Modules/FloodOperations/FloodInspection/IndependentInspection/ProblemForFI.aspx?FloodInspectionID={0}") %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlBreachingSection" runat="server" ToolTip="Breaching Section" CssClass="btn btn-primary btn_24 pfch" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/IBreachingSection.aspx?FloodInspectionID={0}&InspectionYear={1}&InspectionTypeID={2}",Eval("FloodInspectionID"), Eval("InspectionYear"), Eval("InspectionTypeID")) %>' Text="" Visible="false">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlStonePosition" runat="server" ToolTip="Stone Position" CssClass="btn btn-primary btn_24 stnpos" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/IStonePosition.aspx?FloodInspectionID={0}&InspectionYear={1}&InspectionTypeID={2}",Eval("FloodInspectionID"), Eval("InspectionYear"), Eval("InspectionTypeID")) %>' Text="" Visible="false">
                                        </asp:HyperLink>
                                        <asp:Button ID="hlPublish" runat="server" ToolTip="Publish" CssClass="btn btn-primary btn_24 publish" CommandName="Published" CommandArgument='<%# Eval("FloodInspectionID") %>' Text="" OnClientClick="return confirm('Are you sure you want to Publish this record?');" Enabled="false"></asp:Button>
                                        <%--<asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Publish" CssClass="btn btn-primary btn_24 tick" NavigateUrl='<%# Eval("FloodInspectionID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/Publish.aspx?DivisionSummaryID={0}") %>' Text="" Enabled="false">
                                            </asp:HyperLink>--%>
                                        <asp:Button ID="btnPrint" Enabled="<%# base.CanPrint%>" runat="server" formnovalidate="formnovalidate" ToolTip="Print" CssClass="btn btn-primary btn_24 print" Style="height: 24px; width: 29px;" CommandName="PrintReport" />
                                        <asp:Button ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditIndependentInspection" CommandArgument='<%#Eval("FloodInspectionID") %>'></asp:Button>
                                        <%--<asp:LinkButton ID="linkButtonDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete">
                                            </asp:LinkButton>--%>
                                        <asp:Button ID="btnDelete" runat="server" Enabled="<%# base.CanDelete %>" CommandName="Delete" CommandArgument='<%#Eval("FloodInspectionID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="delete"></asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnFloodInspectionID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
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
    <style type="text/css">
        .margincellitme {
            padding-left: 20px !important;
        }
    </style>
</asp:Content>
