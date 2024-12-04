<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionAssetsIndividual.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets.InspectionAssetsIndividual" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register TagPrefix="ucAssets" TagName="Assets" Src="~/Modules/AssetsAndWorks/UserControls/AssetsDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .integerInput {
            text-align: left;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Inspection of Asset</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
                    </div>
                </div>
                <div class="box-content">
                    <ucAssets:Assets runat="server" ID="AssetsDetail" />
                    <div class="table-responsive">
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="hidden">
                                    <asp:HiddenField ID="hdnAssetsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedBy" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id="divDate">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtDate" TabIndex="4" required="required" runat="server" CssClass="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblcondition" runat="server" Text="Condition" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlcondition" runat="server"  CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                    <%--      <asp:ListItem Value="">Select</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblCurrentAssetValue" runat="server" Text="Current Asset Value (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtCurrentAssetValue" runat="server" CssClass="integerInput form-control" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" class="col-sm-4 col-lg-3 control-label" Text="Remarks"></asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox CssClass="form-control commentsMaxLengthRow multiline-no-resize"
                                                    MaxLength="250" TextMode="MultiLine" Rows="5" Columns="50"
                                                    ID="txtRemarks" runat="server" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" Text="Attachment" class="col-sm-4 col-lg-3 control-label"></asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls" id="divAttchmentAdd" runat="server">

                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="5" />
                                            </div>
                                            <div class="col-sm-8 col-lg-9 controls" id="divAttchmentView" runat="server" visible="false">
                                             <%--   <asp:HyperLink ID="HyperLink1" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink2" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink3" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink4" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink5" Text="Attachment" runat="server" Visible="false" />--%>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl2" Size="0" Visible="false" />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl3" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl4" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl5" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl6" Size="0" Visible="false"  />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn" style="margin-left: 26px;">
                                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

