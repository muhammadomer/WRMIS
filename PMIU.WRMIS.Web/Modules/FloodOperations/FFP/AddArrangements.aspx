<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddArrangements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.AddArrangements" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/FloodOperations/Controls/FFPDetail.ascx" TagPrefix="uc1" TagName="FFPDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFFPStatus" runat="server" />
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Literal runat="server" ID="ltlPageTitle">Arrangements For Flood Fighting Plan</asp:Literal>
                    </h3>

                </div>
                <div class="box-content">
                    <uc1:FFPDetail runat="server" ID="FFPDetail" />
                    <div class="table-responsive">
                        <asp:GridView ID="gvArrangements" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10" DataKeyNames="FFPArrangeTypeID,FFPArrangeTypeName,Description,FFPArrangeID"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" OnRowDataBound="gvArrangements_RowDataBound"
                            OnPageIndexChanging="gvArrangements_PageIndexChanging" OnPageIndexChanged="gvArrangements_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                               <%-- <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("FFPArrangeID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Arrangement Type">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblArrangementType" runat="server" CssClass="control-label" Text='<%#Eval("FFPArrangeTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-md-5" />
                                    <ItemTemplate>
                                   <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" CssClass="form-control"  Text='<%#Eval("Description") %>' Width="100%" />
                                    </ItemTemplate>
                                
                                </asp:TemplateField>

                         

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>
</asp:Content>

