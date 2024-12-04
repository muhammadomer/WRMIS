<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchBreachIncident.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.SearchBreachIncident" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Breach Incident</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Case ID</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtCaseID" type="text" class="form-control " placeholder="BRXXXXXX"> </asp:TextBox>
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
                                    <%--      <asp:Button runat="server" ID="btnBreachSearch" value=" Search " CssClass="btn btn-primary" Text="&nbsp;Search" OnClick="btnBreachSearch_Click" />--%>
                                    <asp:LinkButton TabIndex="10" ID="btnBreachSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnBreachSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                                    <asp:Button runat="server" ID="btnAddNewIncident" value=" Add New " CssClass="btn btn-success" Text="&nbsp;Add New" OnClick="btnAddNewIncident_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchBreachResult" runat="server" DataKeyNames="BreachCaseID" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    OnRowDataBound="gvSearchBreachResult_RowDataBound" OnPageIndexChanging="gvSearchBreachResult_PageIndexChanging" AllowPaging="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Case ID." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="BreachCaseID" runat="server" CssClass="control-label" Text='<%# Eval("BreachCaseID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Case No.">
                                            <ItemTemplate>
                                                <asp:Label ID="BreachCaseNo" runat="server" CssClass="control-label" Text='<%# Eval("BreachCaseNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannel" runat="server" CssClass="control-label" Text='<%# Eval("Channel") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type of Offence">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffenceType" runat="server" CssClass="control-label" Text='<%# Eval("TypeofOffence") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Offence Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffenceDate" runat="server" CssClass="control-label" Text='<%# Eval("IncidentDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlBreachCaseView" runat="server" ToolTip="View" CssClass="control-label btn btn-primary btn_24 view "  NavigateUrl='<%# Eval("BreachCaseID", "~/Modules/WaterTheft/ViewBreachCase.aspx?BreachCaseID={0}") %>'   Text=""> <%--NavigateUrl='<%# Eval("/Modules/WaterTheft/ViewBreachCase.aspx?BreachCaseID={0}") %>'--%>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle Width="24px" />
                                            <ItemStyle Width="25px" HorizontalAlign="right" />
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
