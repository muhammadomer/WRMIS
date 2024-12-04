<%@ Page Title="DeliveryDetails" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="DeliveryDetailsForSECE.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.DeliveryDetailsForSECE" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/Payments.ascx" TagPrefix="uc1" TagName="Payments" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Delivery Details</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                 
                    <uc1:Payments runat="server" id="Payments" />
                    
                </div>

                <div class="form-horizontal">
                   
                    <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblStatus" class="col-sm-4 col-lg-3 control-label">Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" TabIndex="1">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                      </div>
                                </div>
                            </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="2" TextMode="MultiLine" Rows="5" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                        </div>
                    <div class="row">
                          <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Upload</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                <%--<asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />--%>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                            </div>
                            
                        </div>
                    </div>
                        </div>
                    </div>
                       
               
                      

                    <div class="row" runat="server" id="divSave">
                                <div class="col-md-12">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                                  
                                </div>
                            </div>
              
                    </div>
            </div>
        </div>
    
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" />
    <asp:HiddenField ID="hdnAuctionPriceID" runat="server" />
    <asp:HiddenField ID="hdnAuctionAssetID" runat="server" />
    <asp:HiddenField ID="hdnApprovalAuthority" runat="server" />
    
    

    <script type="text/javascript">
       
   
</script>
</asp:Content>
