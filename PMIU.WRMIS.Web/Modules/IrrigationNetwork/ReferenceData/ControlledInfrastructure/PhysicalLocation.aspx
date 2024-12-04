﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhysicalLocation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure.PhysicalLocation" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucControlInfrastructureDetails" TagName="ControlInfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/ControlledInfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <asp:HiddenField ID="hdnControlInfrastructureID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCotrolInfrastructureTypeID" runat="server" Value="0" />


    <div class="box">
        <div class="box-title">
            <h3>Physical Location of Barrage/Headwork</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucControlInfrastructureDetails:ControlInfrastructureDetail runat="server" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h3>Irrigation Boundaries</h3>
                        <asp:GridView ID="gvIrrigationBoundaries" runat="server" DataKeyNames="ID,ZoneID,CircleID,DivisionID,FromRDTotal,ToRDTotal,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvIrrigationBoundaries_RowCommand" OnRowDataBound="gvIrrigationBoundaries_RowDataBound"
                            OnRowEditing="gvIrrigationBoundaries_RowEditing" OnRowCancelingEdit="gvIrrigationBoundaries_RowCancelingEdit"
                            OnRowUpdating="gvIrrigationBoundaries_RowUpdating"
                            OnRowDeleting="gvIrrigationBoundaries_RowDeleting" OnPageIndexChanging="gvIrrigationBoundaries_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZoneName" runat="server" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlZone" runat="server" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;" AutoPostBack="True"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircleName" runat="server" Text='<%# Eval("CircleName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlCircle" runat="server" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;" AutoPostBack="True"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDivision" runat="server" required="required" CssClass="required form-control" Style="max-width: 90%;" AutoPostBack="True"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddIrrigationBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddIrrigationBoundaries" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddIrrigationBoundaries" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionIrrigationBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditIrrigationBoundaries" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteIrrigationBoundaries" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionIrrigationBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveIrrigationBoundaries" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelIrrigationBoundaries" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>Administrative Boundaries</h3>
                        <asp:GridView ID="gvAdministrativeBoundaries" runat="server" DataKeyNames="ID,DistrictID,TehsilID,PoliceStationID,VillageID,FromRDTotal,ToRDTotal,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvAdministrativeBoundaries_RowCommand" OnRowDataBound="gvAdministrativeBoundaries_RowDataBound"
                            OnRowEditing="gvAdministrativeBoundaries_RowEditing" OnRowCancelingEdit="gvAdministrativeBoundaries_RowCancelingEdit"
                            OnRowUpdating="gvAdministrativeBoundaries_RowUpdating" OnRowDeleting="gvAdministrativeBoundaries_RowDeleting" OnPageIndexChanging="gvAdministrativeBoundaries_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="District">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrictName" runat="server" Text='<%# Eval("DistrictName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tehsil">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTehsilName" runat="server" Text='<%# Eval("TehsilName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTehsil" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Police Station">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPoliceStationName" runat="server" Text='<%# Eval("PoliceStationName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPoliceStation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPoliceStation_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Village">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVillageName" runat="server" Text='<%# Eval("VillageName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlVillage" runat="server" required="required" CssClass="required form-control "></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddAdminBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddAdminBoundaries" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddAdminBoundaries" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionAdminBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditAdminBoundaries" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteAdminBoundaries" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionAdminBoundaries" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveAdminBoundaries" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelAdminBoundaries" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };
    </script>
</asp:Content>
