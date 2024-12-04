<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="HeadquarterDivision.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData.HeadquarterDivision" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Headquarter Division</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvHeadquarterDivision" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="gvHeadquarterDivision_PageIndexChanging" OnRowDataBound="gvHeadquarterDivision_RowDataBound"
                            OnPageIndexChanged="gvHeadquarterDivision_PageIndexChanged" DataKeyNames="ID"
                            EmptyDataText="No record found" CssClass="table header" BorderWidth="0px"
                            CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZoneID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zone" Visible="true">
                                    <HeaderStyle CssClass="col-md-5" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblZone" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Headquarter Division" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlHeadquarterDivision" runat="server" required="required" CssClass="form-control required" onfocus="this.value = this.value;" Width="90%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                    </div>
             <%--       <br />--%>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />

                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



