<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActionOnSchedule.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ActionOnSchedule" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucScheduleDetail" Src="~/Modules/ScheduleInspection/Controls/ScheduleDetail.ascx" TagName="ScheduleDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Action On Schedule</h3>
        </div>
        <asp:HiddenField ID="hdnScheduleStatusID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSchedulePreparedByID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSchedulePreparedByDesignationID" runat="server" Value="0" />
        <div class="box-content">
            <div class="form-horizontal">
                <ucScheduleDetail:ScheduleDetail runat="server" ID="ScheduleDetail" />
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control multiline-no-resize" TabIndex="1" TextMode="MultiLine" Rows="5" MaxLength="250" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Action</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlAction" runat="server" Enabled="false" required>
                                    <asp:ListItem Text="Select" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" Enabled="false" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlScheduleDetail" runat="server" CssClass="btn btn-success">&nbsp;View Schedule Details</asp:HyperLink>
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <b style="padding: 8px;">
                            <asp:Literal ID="litCommentHistoryTitle" runat="server" Visible="false">
                                Comments History
                            </asp:Literal>
                        </b>
                    </div>
                </div>
                <div class="row" style="padding-top: 6px;">
                    <div class="col-md-12">

                        <div class="table-responsive">
                            <asp:GridView ID="gvrptCommentHistory" runat="server" DataKeyNames="ScheduleID" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" Visible="false" AllowSorting="false" AllowPaging="True">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ScheduleID" runat="server" Text='<%# Eval("ID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <ItemTemplate>
                                            <asp:Label ID="ScheduleStatusID" runat="server" Text='<%# Eval("UA_Users1.FirstName") +" "+ Eval("UA_Users1.LastName") +" , "+Eval("UA_Designations.Name") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="Name" runat="server" Text='<%# GetFormattedDate(Convert.ToString(Eval("CommentsDate"))) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Label ID="FromDate" runat="server" Text='<%# "Assigned to " + Eval("UA_Designations1.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:Label ID="ToDate" runat="server" Text='<%# Eval("Comments") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-5" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                        <%--<asp:Repeater runat="server" ID="rptCommentHistory" OnItemDataBound="rptCommentHistory_ItemDataBound">
                            <ItemTemplate>
                                <div class="table-responsive">
                                    <table class="table tbl-info">
                                        <tr>
                                            <th>From</th>
                                            <th>Date</th>
                                            <th>Action</th>
                                        </tr>
                                        <tr>
                                            <td><%# Eval("UA_Users1.FirstName") %> <%# Eval("UA_Users1.LastName") %>, <%# Eval("UA_Designations.Name") %></td>
                                            <td>
                                                <asp:Literal ID="litCommentDate" runat="server" Text='<%# Eval("CommentsDate") %>' />
                                            </td>
                                            <td>Assigned to <%# Eval("UA_Designations1.Name") %></td>
                                        </tr>
                                        <tr>
                                            <th colspan="3">Comment</th>
                                        </tr>
                                        <tr>
                                            <td colspan="3"><%# Eval("Comments") %></td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
