<%@ Page Title="AddRemainingPayment" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddRemainingPayment.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.AddRemainingPayment" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/Payments.ascx" TagPrefix="uc1" TagName="Payments" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .padding-right-number {
            padding-right: 35px !important;
        }

        @media only screen and (min-width: 1300px) {
            .padding-right-number {
                padding-right: 45px !important;
            }
        }

        @media only screen and (min-width: 1400px) {
            .gridReachStartingRDs {
                width: 15%;
            }
        }

        @media only screen and (min-width: 1500px) {
            .gridReachStartingRDs {
                width: 14%;
            }
        }

        @media only screen and (min-width: 1400px) {
            .padding-right-number {
                padding-right: 75px !important;
            }
        }

        .Hide {
            display: none;
        }
    </style>
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Add Payment</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                 
                    <uc1:Payments runat="server" id="Payments"  />
                    
                </div>

                <div class="form-horizontal">
                   
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                           
                                <asp:GridView ID="gvRemainingAmount" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                       DataKeyNames="ID,PaymentTypeID,PaymentType,Attachment" ShowHeaderWhenEmpty="True" AllowPaging="True" OnRowDataBound="gvRemainingAmount_RowDataBound" OnRowCommand="gvRemainingAmount_RowCommand"
                        OnRowCancelingEdit="gvRemainingAmount_RowCancelingEdit" OnRowUpdating="gvRemainingAmount_OnRowUpdating" OnRowDeleting="gvRemainingAmount_OnRowDeleting" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Payment Type">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlPaymentType" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Amount">
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control decimal2PInput required" ID="txtAmount" required="required" runat="server" MaxLength="10" onchange ="txtChange(this);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                   <ItemStyle CssClass="text-right padding-right-number" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Date">
                                <EditItemTemplate>
                                   <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDate" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="Attachment">
                                <EditItemTemplate>
                                   <uc1:FileUploadControl runat="server"  ID="FileUpload" Size="1" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%--<asp:HyperLink ID="hlAttachement" NavigateUrl='<%# PMIU.WRMIS.Common.Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.Auctions , Convert.ToString(Eval("Attachment"))) %>' CssClass="" runat="server" Text="Attachment" />--%>
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                        <asp:Button ID="btnPayment" runat="server" Text="" CommandName="AddPayment" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" Visible="<%# base.CanAdd %>"  />
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlActionGroupedAssets" runat="server" HorizontalAlign="Center">
                                       <%--<asp:Button ID="btnEditGroupedAssets" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />--%>
                                        <asp:Button ID="lbtnDeleteGroupedAssets" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>"  />
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionGroupedAssets" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveGroupedAssets" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelGroupedAssets" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                           
                        </div>

                        
                    </div>
                </div>
                       
               
                      

                    <div class="row" runat="server" id="divSave">
                                <div class="col-md-12">
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                                  <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />--%>
                                </div>
                            </div>
              
                    </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" />
    <asp:HiddenField ID="hdnAuctionPriceID" runat="server" />
    

    <script type="text/javascript">
       

        function txtChange(txt) {
            debugger;
            var a = $('#<%= hdnAuctionNoticeID.ClientID %>');
            var b = $('#<%= hdnAuctionPriceID.ClientID %>');
            var gridRow = txt.parentNode.parentNode;
            var val = gridRow.getElementsByTagName("select");
            var AuctionNoticeID = a[0].defaultValue;
            var AuctionPriceID = b[0].defaultValue;
            var EnteredAmount = txt.value;
            var SelectedIndex = val[0].selectedIndex;;
            if (EnteredAmount != "" && EnteredAmount != "0")
            {
                 $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("AddRemainingPayment.aspx/CompareAmount") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    data: '{_AuctionNoticeID: "' + AuctionNoticeID + '",_AuctionPriceID:"' + AuctionPriceID + '",_EnteredAmount:"' + EnteredAmount + '",_Type:"' + SelectedIndex + '"}',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        var a = data.d;
                       //console.log(a);
                        if (a != "") {
                            txt.value = "";
                            $('#lblMsgs').addClass('ErrorMsg').show();
                            $('#lblMsgs').html(a);
                            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                    }
                });
            }
            else {
                txt.value = "";
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Entered Amount must be greater than 0");
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            }
           
        }


       
</script>
</asp:Content>