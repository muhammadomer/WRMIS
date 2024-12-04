<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KPICategoryWiseWeightage.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.KPICategoryWiseWeightage" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucEvaluationScoresDetail" TagName="EvaluationScoresDetail" Src="~/Modules/PerformanceEvaluation/Controls/EvaluationScoresDetail.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>KPI Category Wise Weightage</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucEvaluationScoresDetail:EvaluationScoresDetail runat="server" ID="EvaluationScoresDetail" />
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvKPICategoryWiseWeightage" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    DataKeyNames="CatID,CatName,CatDescription,FDTotalPoints,FDWeightage,PMIUTotalPoints,PMIUWeightage,TotalWeightage"
                    ShowHeaderWhenEmpty="True" AllowPaging="False" CssClass="table header"
                    OnRowDataBound="gvKPICategoryWiseWeightage_RowDataBound" ShowFooter="true"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCatID" runat="server" Text='<%# Eval("CatID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KPI Category">
                            <ItemTemplate>
                                <asp:Label ID="lblCatName" runat="server" Text='<%# Eval("CatName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                Total
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                            <ItemStyle CssClass="text-left" />
                            <FooterStyle CssClass="text-left text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblCatDescription" runat="server" Text='<%#Eval("CatDescription") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Data - Total Points">
                            <ItemTemplate>
                                <asp:Label ID="FDTotalPoints" runat="server" Text='<%#Eval("FDTotalPoints") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_FDTotalPoints" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Data - Weightage">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldDataWeightage" runat="server" Text='<%#Eval("FDWeightage") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_FieldDataWeightage" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PMIU Checking - Total Point">
                            <ItemTemplate>
                                <asp:Label ID="lblPMIUTotalPoints" runat="server" Text='<%#Eval("PMIUTotalPoints") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_PMIUTotalPoints" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PMIU Checking - Weightage">
                            <ItemTemplate>
                                <asp:Label ID="lblPMIUWeightage" runat="server" Text='<%#Eval("PMIUWeightage") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_PMIUWeightage" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Weightage">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalWeightage" runat="server" Text='<%#Eval("TotalWeightage") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_TotalWeightage" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1 text-center" />
                            <ItemStyle CssClass="text-center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back" onclick="javascript:window.history.back();"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
