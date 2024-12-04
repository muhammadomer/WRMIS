<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KPISubCategoryWiseWeightage.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.KPISubCategoryWiseWeightage" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucEvaluationScoresDetail" TagName="EvaluationScoresDetail" Src="~/Modules/PerformanceEvaluation/Controls/EvaluationScoresDetail.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>KPI Sub Category Wise Weightage</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucEvaluationScoresDetail:EvaluationScoresDetail runat="server" ID="EvaluationScoresDetail1" />
            <br />
            <asp:HiddenField ID="hdnEvalScoreID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnIrrigationBoundaryID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCatID" runat="server" Value="0" />
            <div class="table-responsive">
                <asp:GridView ID="gvKPISubCategoryWiseWeightage" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    DataKeyNames="SubCatID,SubCatName,SubCatDescription,TotalPoints,Weightage,Source"
                    ShowHeaderWhenEmpty="True" AllowPaging="False" CssClass="table header"
                    OnRowDataBound="gvKPISubCategoryWiseWeightage_RowDataBound" ShowFooter="true"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSubCatID" runat="server" Text='<%# Eval("SubCatID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KPI Sub Category">
                            <ItemTemplate>
                                <asp:Label ID="lblSubCatName" runat="server" Text='<%# Eval("SubCatName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                Total
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                            <ItemStyle CssClass="bold" />
                            <FooterStyle CssClass="text-left text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblSubCatDescription" runat="server" Text='<%#Eval("SubCatDescription") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Points in Sub Category">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalPoints" runat="server" Text='<%#Eval("TotalPoints") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_TotalPoints" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weightage in the Sub Category">
                            <ItemTemplate>
                                <asp:Label ID="lblWeightage" runat="server" Text='<%#Eval("Weightage") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Total_Weightage" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <ItemStyle CssClass="text-right" />
                            <FooterStyle CssClass="text-right text-bold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Source of Data (Field or PMIU)">
                            <ItemTemplate>
                                <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
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
