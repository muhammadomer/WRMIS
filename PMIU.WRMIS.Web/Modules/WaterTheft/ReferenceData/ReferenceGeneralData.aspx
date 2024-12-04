<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReferenceGeneralData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ReferenceData.ReferenceGeneralData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:HiddenField ID="hdnOffenceID" runat="server" Value="0" />

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 >Reference General</h3>
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                               <div id="DivGeneral" runat="server">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label runat="server" id="lblType" class="col-sm-4 col-lg-3 control-label">Ignored Feet </label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:TextBox runat="server" ID="txtFeetToIgnor" type="text" class="form-control" > </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Button runat="server" ID="btnUpdateFeet" value=" Update " CssClass="btn btn-primary" Text="&nbsp;Update" OnClick="btnUpdateFeetToIgnor_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <br />
       <%--             <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END Main Content -->
</asp:Content>
