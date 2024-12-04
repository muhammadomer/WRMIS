<%@ Page Title="AddSchedule" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SearchSchedule.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.SearchSchedule" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Search Schedule</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlZone" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlCircle" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlSubDivision" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                        <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                        <span id="spantoDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlStatus" id="lblStatus" class="col-sm-4 col-lg-3 control-label">Status</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="CheckBoxApproval" class="col-sm-3 col-lg-2 control-label"></label>
                            <div class="col-sm-8 col-lg-9 controls" style="padding-left: 48px;">
                                <asp:CheckBox CssClass="checkbox" ID="CheckBoxApproval" runat="server" Text="Assigned to me for Approval" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                                <asp:HyperLink ID="hlNewSchedule" runat="server" NavigateUrl="~/Modules/ScheduleInspection/AddSchedule.aspx" CssClass="btn btn-success">&nbsp;Add New Schedule</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSISearch" runat="server" DataKeyNames="ScheduleID" AutoGenerateColumns="False" EmptyDataText="No record found" OnRowDataBound="gvSISearch_RowDataBound"
                                        ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvSearch_PageIndexChanging" OnRowDeleting="gvSISearch_RowDeleting" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="ScheduleID" runat="server" Text='<%# Eval("ScheduleID") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="HasDependants" runat="server" Text='<%# Eval("HasDependants") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="ScheduleStatusID" runat="server" Text='<%# Eval("StatusID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrepairedByDesignationID" runat="server" Text='<%# Eval("PrepairedByDesignationID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrepairedBy" runat="server" Text='<%# Eval("PrepairedBy") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Schedule Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="Name" runat="server" Text='<%#Eval("Name") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="FromDate" runat="server" Text='<%# Eval("FromDate", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="ToDate" runat="server" Text='<%# Eval("ToDate", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approval Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="StatusValue" runat="server" Text='<%# Eval("StatusValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prepared By">
                                                <ItemTemplate>
                                                    <asp:Label ID="CreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Execution Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="ScheduleStatus" runat="server" Text='<%# Eval("ScheduleStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="HeaderAction">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="gvSDetail" runat="server" ToolTip="Schedule Detail" CssClass="btn btn-primary btn_24 view-feedback" NavigateUrl='<%# Eval("ScheduleID", "~/Modules/ScheduleInspection/ScheduleDetail.aspx?ScheduleID={0}") %>' Text="">
                                                        
                                                    </asp:HyperLink>
                                                    <asp:HyperLink ID="gvActions" runat="server" ToolTip="Actions" CssClass="btn btn-primary btn_24 channel" NavigateUrl='<%# Eval("ScheduleID", "~/Modules/ScheduleInspection/ActionOnSchedule.aspx?ScheduleID={0}") %>' Text="">
                                                        
                                                    </asp:HyperLink>
                                                    <asp:HyperLink ID="gvINotes" runat="server" ToolTip="Inspection Notes" CssClass="btn btn-primary btn_24 parameters" NavigateUrl='<%# Eval("ScheduleID", "~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID={0}") %>' Text="">
                                                        
                                                    </asp:HyperLink>
                                                    <asp:HyperLink ID="gvEdit" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("ScheduleID", "~/Modules/ScheduleInspection/AddSchedule.aspx?ScheduleID={0}") %>' Text="">
                                                        
                                                    </asp:HyperLink>
                                                    <asp:LinkButton ID="linkButtonDelete" CommandName="Delete" runat="server" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


        </div>
    </div>


    <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />

    <script type="text/javascript">

        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                }
            });
        };
    </script>
</asp:Content>
