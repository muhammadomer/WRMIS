<%@ Page Title="Add Elevation Capacity" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddElevationCapacity.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.AddElevationCapacity" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Add Elevation Capacity</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date-picker" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblRimStation" class="col-sm-4 col-lg-3 control-label">Rim Station</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlRimStation" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblDate" class="col-sm-4 col-lg-3 control-label">Initial Level</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtInitialLevel" runat="server" MaxLength="4" CssClass="form-control integerInput required" required="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblRimStation" class="col-sm-4 col-lg-3 control-label">No. of Rows</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtNoOfRows" runat="server" MaxLength="3" CssClass="form-control integerInput required" required="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add Capacity" OnClick="btnAdd_Click" />
                    </div>
                    <br />
                    <asp:GridView ID="gvElevation" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("Level") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Capacity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control decimalInput" MaxLength="8" Text='<%# Eval("Capacity")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
