<%@ Page Title="Roles" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddBarrageDataFrequency.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.AddBarrageDataFrequency" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Data Frequency For Barrage/Headworks</h3>
                        </div>

                        <div class="box-content">
                            <div class="form-horizontal">

                                <div class="row">
                                    <div class="col-md-6 ">

                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Barrage/Headwork</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlBarrages" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBarrages_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Set Data Frequency</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlFrequency" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Reset" OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


