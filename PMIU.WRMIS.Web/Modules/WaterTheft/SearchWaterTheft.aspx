<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchWaterTheft.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.SearchWaterTheft" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Water Theft Incident</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Select Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Select Channel</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlChannel" runat="server" Enabled="false">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Assigned To</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlAssignto" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Offence Site</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlOffenceSite" runat="server" OnSelectedIndexChanged="ddlOffenceSite_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="All" Value="" />
                                            <asp:ListItem Text="Channel" Value="C" />
                                            <asp:ListItem Text="Outlet" Value="O" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Type of Offence</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlTypeofOffence" runat="server" Enabled="false">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="dvCaseID" runat="server" class="col-md-6" visible="false">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Case ID</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtCaseID" type="text" class="form-control" placeholder="WTXXXXXX" />
                                    </div>
                                </div>
                            </div>
                            <div id="dvCanalWire" runat="server" class="col-md-6" visible="false">
                                <div class="form-group">
                                    <asp:Label ID="lblCanalWire" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Text="Canal Wire #" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtCanalWire" type="text" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:LinkButton TabIndex="10" ID="btnSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnIncidentSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchWaterTheft" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    OnRowDataBound="gvSearchWaterTheft_RowDataBound" OnPageIndexChanging="gvSearchWaterTheft_PageIndexChanging"
                                    DataKeyNames="ID,AssignedByDesignationID,AssignedToDesignationID,StatusID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Case ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCaseNo" runat="server" CssClass="control-label" Text='<%# Eval("CaseNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannel" runat="server" CssClass="control-label" Text='<%# Eval("Channel") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type of Offence">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffenceType" runat="server" CssClass="control-label" Text='<%# Eval("TypeOfOffence") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reduced Distance (RD)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReducedDistance" runat="server" CssClass="control-label" Text='<%# Eval("ReducedDistance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Offence Site">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffenceSite" runat="server" CssClass="control-label" Text='<%# Eval("OffenceSite") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Incident Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIncidentDate" runat="server" CssClass="control-label" Text='<%# Eval("IncidentDateTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assigned To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignedTo" runat="server" CssClass="control-label" Text='<%# Eval("AssignedToDesignation") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlView" runat="server" CssClass="btn btn-primary btn_24 view" ToolTip="View" Visible="true"></asp:HyperLink>
                                                <asp:HyperLink ID="hlAppeal" runat="server" CssClass="btn btn-primary btn_24 appeal" ToolTip="Appeal" Visible="false"></asp:HyperLink>
                                                <asp:HyperLink ID="hlAssignBack" runat="server" CssClass="btn btn-primary btn_24 assign-back" ToolTip="Assign Back" Visible="false"></asp:HyperLink>
                                                <asp:LinkButton ID="lbtnPrint" runat="server" CssClass="btn btn-primary btn_24 print" ToolTip="Print" OnClick="lbtnPrint_Click" />
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
    <!-- END Main Content -->
</asp:Content>

