<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddACCP.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP.AddACCP" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3><asp:Label runat="server" ID="lblHeader" Text="Add Annual Canal Closure Programme"></asp:Label></h3>
                </div>                        

                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Title</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox  ID="txtTitle" required="required" CssClass="form-control required" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:HiddenField ID="hdnFldID" runat="server"></asp:HiddenField>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtYear" required="required" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachment</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <uc1:FileUploadControl runat="server" Mode="2" ID="AddFileUploadControl" Size="1" />
                                        <asp:HyperLink ID="hlImage" Text="Attachment" runat="server" Visible="false" />
                                    </div>
                                </div>
                            </div>
                             <div id="CellOfCopyLastYearDetail" runat="server" class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label"></label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:CheckBox runat="server" ID="chkCopyLastYearDetail" Text="&nbsp;&nbsp;&nbsp;Copy Closure Programme from last year" Checked="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSaveAccp" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/ClosureOperations/ACCP/AnnualCanalClosureProgram.aspx?CFCH=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
