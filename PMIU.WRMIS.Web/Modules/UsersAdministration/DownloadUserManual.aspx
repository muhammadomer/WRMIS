<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DownloadUserManual.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.DownloadUserManual" %>

<%@ MasterType VirtualPath="~/Site.Master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>User Manuals</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"></a>
                    </div>
                </div>
                <div class="box-content">

                    <asp:GridView CssClass="table header" ID="gvGrid" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text="1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Module Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PDF Link">
                                <ItemTemplate>
                                    <%--<a id="aLink" runat="server" href="#" title="DownloadPDF" ></a>--%>

                                    <%--<asp:HyperLink ID="lnkUserManuals" Target="_blank" runat="server" >Download Manual</asp:HyperLink>--%>

                                    <asp:LinkButton ID="btn" runat="server" OnClick="btn_Click" Text="DownloadPDF"></asp:LinkButton>


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
    <%--</ContentTemplate>--%>
    <%-- </asp:UpdatePanel>--%>
</asp:Content>



