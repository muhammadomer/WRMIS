<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchTenderNotice.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.SearchTenderNotice" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Search Tender Notice</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">

                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblTenderNotice" class="col-sm-4 col-lg-3 control-label">Tender Notice</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtTenderNotice" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblStatus" class="col-sm-4 col-lg-3 control-label">Status</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" TabIndex="2">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Expired"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlDomain" class="col-sm-4 col-lg-3 control-label">Domain</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDomain" CssClass="form-control" runat="server" AutoPostBack="True"  TabIndex="3" OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged" >
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" CssClass="form-control" runat="server" TabIndex="4">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblSubmissionFrom" runat="server" class="col-sm-4 col-lg-3 control-label">Submission From</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtSubmissionFrom" runat="server" CssClass=" date-picker form-control" TabIndex="5"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblSubmissionTo" runat="server" class="col-sm-4 col-lg-3 control-label">Submission to</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtSubmissionTo" runat="server" CssClass="date-picker form-control" TabIndex="6"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblOpeningFrom" runat="server" class="col-sm-4 col-lg-3 control-label">Opening From</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtOpeningFrom" runat="server" CssClass="date-picker form-control" TabIndex="7"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblOpeningTo" runat="server" class="col-sm-4 col-lg-3 control-label">Opening To</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtOpeningTo" runat="server" CssClass="date-picker form-control" TabIndex="8"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" TabIndex="9" Visible="<%# base.CanEdit %>" />
                                <asp:HyperLink ID="hlAdd" runat="server"  NavigateUrl="~/Modules/Tenders/TenderNotice/AddTenderNotice.aspx" CssClass="btn btn-success" TabIndex="10">Add</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvTenderNotice" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvTenderNotice_RowDataBound" AllowPaging="true"
                                OnPageIndexChanged="gvTenderNotice_PageIndexChanged" OnPageIndexChanging="gvTenderNotice_PageIndexChanging"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>

                                    <asp:TemplateField HeaderText="Tender Notice">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlTenderNotice" runat="server" Text='<%#Eval("Name") %>' NavigateUrl='<%# String.Format("~/Modules/Tenders/TenderNotice/AddTenderNotice.aspx?TenderNoticeID={0}&IsEditMode={1}", Convert.ToString(Eval("ID")),false) %>'>
                                        </asp:HyperLink>
                                            <%--<asp:Label ID="lblTenderNotice" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-4" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("CO_Division.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submission Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmissionDate" runat="server" CssClass="control-label" Text='<%# Eval("BidSubmissionDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Opening Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpeningDate" runat="server" CssClass="control-label" Text='<%# Eval("BidOpeningDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEdit" runat="server" CssClass="btn btn-primary btn_32 edit" NavigateUrl='<%# String.Format("~/Modules/Tenders/TenderNotice/AddTenderNotice.aspx?TenderNoticeID={0}&IsEditMode={1}", Convert.ToString(Eval("ID")),true) %>' ToolTip="Edit" />
                                            <asp:HyperLink ID="hlIndent" runat="server"  CssClass="btn btn-primary btn_24 workdetails" NavigateUrl='<%# String.Format("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Works"></asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
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
</asp:Content>
