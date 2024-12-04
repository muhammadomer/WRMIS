<%@ Page Title="AddWorks" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddWorks.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.AddWorks" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/AddWorks.ascx" TagPrefix="ucAddWorksControl" TagName="AddWorksUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .padding-right-number {
            padding-right: 35px !important;
        }

        @media only screen and (min-width: 1300px) {
            .padding-right-number {
                padding-right: 45px !important;
            }
        }

        @media only screen and (min-width: 1400px) {
            .gridReachStartingRDs {
                width: 15%;
            }
        }

        @media only screen and (min-width: 1500px) {
            .gridReachStartingRDs {
                width: 14%;
            }
        }

        @media only screen and (min-width: 1400px) {
            .padding-right-number {
                padding-right: 75px !important;
            }
        }
    </style>

    <div class="box">
        <div class="box-title">
            <h3>Tender Works</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <ucAddWorksControl:AddWorksUserControl runat="server" ID="AddWorksUserControl" />
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvTenderWorks" runat="server" DataKeyNames="TenderWorkID,ID,WorkSourceID" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" OnRowDataBound="gvTenderWorks_RowDataBound" OnRowDeleting="gvTenderWorks_RowDeleting" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True" OnRowCommand="gvTenderWorks_RowCommand">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTenderWorkID" runat="server" Text='<%# Eval("TenderWorkID") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkSourceID" runat="server" Text='<%# Eval("WorkSourceID") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkStatusID" runat="server" Text='<%# Eval("WorkStatusID") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkSource" runat="server" Text='<%# Eval("WorkSource") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name of Work/Tender">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkName" runat="server" Text='<%#Eval("WorkName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Work Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkType" runat="server" Text='<%# Eval("WorkType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estimated Cost(Rs)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimatedCost" runat="server" Text='<%# Eval("EstimatedCost","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                            <ItemStyle CssClass="text-right padding-right-number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle CssClass="" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                    <asp:HyperLink ID="btnAddWork" runat="server" Text="" CommandName="AddWork" Visible="true" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" NavigateUrl="<%# GetURLValue() %>" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="gvWorkDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_24 parameters" NavigateUrl='<%# string.Format("~/Modules/Tenders/Works/ViewWorkTenderDetails.aspx?CWID={0}&TenderWorkID={1}", HttpUtility.UrlEncode(Eval("WorkSourceID").ToString()),HttpUtility.UrlEncode(Eval("TenderWorkID").ToString())) %>' Text="">
                                                </asp:HyperLink>
                                                <asp:Button ID="btnOpeningOffice" runat="server" CommandName="OpeningOffice" CssClass="btn btn-primary btn_32 history" CommandArgument='<%# Eval("ID") %>' ToolTip="Opening Office" formnovalidate="formnovalidate"></asp:Button>
                                                <asp:HyperLink ID="gvWorkItems" runat="server" ToolTip="View Work Items" CssClass="btn btn-primary btn_24 tenderworks" NavigateUrl='<%# string.Format("~/Modules/Tenders/Works/ViewWorkItems.aspx?WorkSourceID={0}&WorkSource={1}&TenderWorkID={2}", HttpUtility.UrlEncode(Eval("WorkSourceID").ToString()), HttpUtility.UrlEncode(Eval("WorkSource").ToString()), HttpUtility.UrlEncode(Eval("TenderWorkID").ToString())) %>' Text="">
                                                        
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="gvSoldTenderList" runat="server" ToolTip="Sold Tender List" CssClass="btn btn-primary btn_24 soldtenderlist" NavigateUrl='<%# string.Format("~/Modules/Tenders/Works/SoldTenderList.aspx?TenderWorkID={0}&WorkSourceID={1}", HttpUtility.UrlEncode(Eval("TenderWorkID").ToString()), HttpUtility.UrlEncode(Eval("WorkSourceID").ToString())) %>' Text="">
                                                        
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="gvTenderEvaluationCommitte" runat="server" ToolTip="Tender Evaluation Committe" CssClass="btn btn-primary btn_24 tenderevalcommittee" NavigateUrl='<%# string.Format("~/Modules/Tenders/TenderNotice/ViewTenderEvaluationCommittee.aspx?TenderWorkID={0}&WorkSourceID={1}", HttpUtility.UrlEncode(Eval("TenderWorkID").ToString()), HttpUtility.UrlEncode(Eval("WorkSourceID").ToString())) %>' Text="">
                                                        
                                                </asp:HyperLink>

                                                <%--<asp:HyperLink ID="gvTenderOpeningProcess" runat="server" ToolTip="Tender Opening Process" CssClass="btn btn-primary btn_24 tenderworksProcess" NavigateUrl='<%# string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID={0}&WorkSourceID={1}", HttpUtility.UrlEncode(Eval("TenderWorkID").ToString()), HttpUtility.UrlEncode(Eval("WorkSourceID").ToString())) %>' Text="">
                                                                                                                                                                                                                                        
                                                </asp:HyperLink>--%>

                                                <asp:Button ID="btnTenderOpeningProcess" runat="server" ToolTip="Tender Opening Process" CssClass="btn btn-primary btn_24 tenderworksProcess" CommandName="TenderOpeningProcess" CommandArgument='<%# Eval("TenderWorkID") %>' Text="" formnovalidate="formnovalidate" />
                                                <asp:Button ID="lbtnDeleteWork" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="true"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" HorizontalAlign="center" />
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
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row">
                <div id="AddOpeningOffices" class="modal fade">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="box">
                                <div class="box-title">
                                    <h5>Tender Opening Office</h5>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal">
                                        <asp:UpdatePanel runat="server" ID="up">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Opening Office</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:DropDownList CssClass="form-control required" ID="ddlOpeningOffice" runat="server" required="true" OnSelectedIndexChanged="ddlOpeningOffice_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Text="All" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" id="lblOfficeLocation" runat="server"></label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:DropDownList CssClass="form-control required" ID="ddlOfficeLocated" runat="server" required="true">
                                                                    <asp:ListItem Text="All" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSave1" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:Button>
                                        <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnWorkStatusID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCWID" runat="server" Value="0" />

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
