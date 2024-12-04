<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IrrigatorFeedbackHistory.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.IrrigatorFeedbackHistory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Irrigator Feedback History</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <table class="table tbl-info">

                            <tr>
                                <th width="33.3%">Division</th>
                                <th width="33.3%">Channel Name</th>
                                <th width="33.3%">Name</th>
                                
                            </tr>

                            <tr>

                                <td>
                                    <asp:Label ID="lblDivision" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblChannelName" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblIrrigatorName" runat="server" Text="" /></td>
                                
                            </tr>

                        </table>
                    </div>
                    <br />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblFromDate" runat="server" class="col-sm-4 col-lg-3 control-label">From Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblToDate" runat="server" class="col-sm-4 col-lg-3 control-label">To Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>

                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <br />
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" align="center" OnClick="btnSearch_Click" Text="Search"></asp:Button>
                                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" />
                                        </div>
                                    </div>
                                </div>

                                <br />

                                <div class="table-responsive">
                                    <asp:GridView ID="gvFeedbackHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True"
                                        AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" PageSize="10"
                                        OnRowDataBound="gvFeedbackHistory_RowDataBound" OnPageIndexChanging="gvFeedbackHistory_PageIndexChanging" OnPageIndexChanged="gvFeedbackHistory_PageIndexChanged">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Call Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFeedbackDate" runat="server" CssClass="control-label" Text='<%# Eval("FeedbackDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tail Status by Irrigator">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTailStatus" runat="server" CssClass="control-label" Text='<%# Eval("TailStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rotational Violation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRotationalViolation" runat="server" CssClass="control-label" Text='<%# Eval("RotaionalViolation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Water Theft">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWaterTheft" runat="server" CssClass="control-label" Text='<%# Eval("WaterTheft") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                                <ItemTemplate>
                                                    <%--<asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />--%>
                                                    <asp:HyperLink ID="hlAddFeedback" runat="server" CssClass="btn btn-primary btn_24 view-feedback" NavigateUrl='<%# String.Format("~/Modules/IrrigatorsFeedback/AddIrrigatorFeedback.aspx?IrrigatorFeedbackID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="View Feedback"></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

