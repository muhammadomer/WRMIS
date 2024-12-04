<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DivisionDistrictRelation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.DivisionDistrictRelation" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Division-District Relation</h3>

                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                    </div>
                    <br />
                    <%--<b>District(s) that are associated with above mentioned division</b>--%>

                    <div class="table-responsive">

                        <asp:GridView ID="gvDistricts" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            ShowHeaderWhenEmpty="True" CssClass="table header" OnRowDataBound="gvDistricts_RowDataBound"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="District Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrictName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-4" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Associated">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDistrict" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-8" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                        <asp:LinkButton ID="LbtnReset" Text="Reset" CssClass="btn" OnClick="lnkBtnCancel_Click" runat="server" />
                                </div>
                            </div>
                        </div>


                        </br>
                        </br>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

