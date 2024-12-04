<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchDepartmental.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental.SearchDepartmental" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Departmental Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
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
                </div>
                <div class="row">
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
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <div id="divStatus" runat="server" style="display: none">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFromDate" runat="server" Text="Date From" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
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
                            <asp:Label ID="lblToDate" runat="server" Text="Date To" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
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
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            <asp:HyperLink ID="hlAddNewDivisionSummary" runat="server" NavigateUrl="~/Modules/FloodOperations/FloodInspection/Departmental/AddDepartmental.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvSearchDepartmental" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            DataKeyNames="FloodInspectionID,Division,InspectionDate,InfrastructureStatus,StatusID,InspectionYear"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                            GridLines="None"
                            OnPageIndexChanging="gvSearchDepartmental_PageIndexChanging"
                            OnPageIndexChanged="gvSearchDepartmental_PageIndexChanged"
                            OnRowDeleting="gvSearchDepartmental_RowDeleting"
                            OnRowDataBound="gvSearchDepartmental_RowDataBound"
                            OnRowCommand="gvSearchDepartmental_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-5" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Inspection Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionDate" runat="server" Text='<%#Eval("InspectionDate", "{0:dd-MMM-yyyy}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFloodInspectionStatus" runat="server" Text='<%#Eval("InfrastructureStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-left" />
                                    <ItemStyle CssClass="text-left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlInfrastructure" runat="server" ToolTip="Infrastructure" CssClass="btn btn-primary btn_24 infrastructure_parent" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/FloodInspection/Departmental/InfrastructuresDepartmental.aspx?FloodInspectionID={0}&InspectionYear={1}",Eval("FloodInspectionID"), Eval("InspectionYear")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlMembersDepartmental" runat="server" ToolTip="Members Detail" CssClass="btn btn-primary btn_24 member_detail" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/FloodInspection/Departmental/MembersDepartmental.aspx?FloodInspectionID={0}&InspectionYear={1}",Eval("FloodInspectionID"), Eval("InspectionYear")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlAttachments" runat="server" ToolTip="Attachments" CssClass="btn btn-primary btn_24 attachment" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/FloodInspection/Departmental/AttachmentsDepartmental.aspx?FloodInspectionID={0}&InspectionYear={1}",Eval("FloodInspectionID"), Eval("InspectionYear")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:Button ID="hlPublish" runat="server" ToolTip="Publish" CssClass="btn btn-primary btn_24 publish" CommandName="Published" CommandArgument='<%# Eval("FloodInspectionID") %>' Text="" Enabled="false"></asp:Button>
                                        <asp:HyperLink ID="hlPrintPreview" runat="server" Enabled="false" ToolTip="Print Preview" CssClass="btn btn-primary btn_24 print_preview" NavigateUrl='<%# Eval("FloodInspectionID","#") %>' Visible="false" Text="">
                                        </asp:HyperLink>
                                        <asp:Button ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditAddDep" CommandArgument='<%# Eval("FloodInspectionID") %>'></asp:Button>
                                        <asp:Button ID="linkButtonDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
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
