<%@ Page Title="Bidders" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="Bidders.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.Bidders" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>
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
       <div class="box">
        <div class="box-title">
            <h3>Bidders</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <uc1:AuctionNotice runat="server" id="AuctionNotice" />
                  <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label id="lblddlBidderName" class="col-sm-4 col-lg-3 control-label">Bidder Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlBidderName" CssClass="form-control required" required="required" runat="server" AutoPostBack="True"  TabIndex="1" OnSelectedIndexChanged="ddlBidderName_OnSelectedIndexChanged">
                                            <asp:ListItem Value="">New Bidder</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                 <div class="form-group">
                            <%--<label id="lblAuctionNotice" class="col-sm-4 col-lg-3 control-label">Auction Notice</label>--%>
                            <div class="col-sm-12 col-lg-12 controls">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="required" MaxLength="100" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                            </div>
                      <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="btnSaveBidder" class="btn btn-primary" runat="server" Text="OK" OnClick="btnOK_Click"  />
                               </div>
                        </div>
                    </div>
                        </div>
             
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAssetsList" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" Visible="false" OnRowDataBound="gvAssetsList_OnRowDataBound"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" DataKeyNames="AuctionDetailID,isChecked,Attachement">
                                <Columns>
                              <asp:TemplateField HeaderStyle-Width="30px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("isChecked").ToString()) %>'  onchange ="chkChanged(this);"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Att" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Att" runat="server" CssClass="control-label" Text='<%# Eval("Attachement") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AuctionDetailID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="AuctionDetailID" runat="server" CssClass="control-label" Text='<%# Eval("AuctionDetailID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAsset" runat="server" CssClass="control-label" Text='<%# Eval("AssetName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Earnest Money(Rs)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEarnestMoney" runat="server" CssClass="control-label" Text='<%# Eval("EarnestMoney","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-right padding-right-number" />
                                <ItemStyle CssClass="text-right padding-right-number" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Earnest Money Submitted(Rs)">
                                        
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtSubmittedMoney" type="text" class="form-control" MaxLength="10" Text='<%# Eval("EarnestMoneySubmitted") %>' ClientIDMode="Static" onchange ="txtChange(this);" disabled="disabled"> </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Attachment">
                                      
                                        <ItemTemplate>
                                            <uc1:FileUploadControl runat="server"  ID="FileUpload" Size="1" Name="BidCtrl" />
                                            <%--<asp:HyperLink ID="hlAttachement" CssClass="" Visible="false" runat="server" Text="abc" />--%>
                                            <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                        
                    </div>
                </div>
        
                 </div>

          
               <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click"  />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    <script type="text/javascript">
        // for check all checkbox  
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvAssetsList.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                if (GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked) {
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].removeAttribute("disabled");
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].setAttribute("required", "required");
                    GridVwHeaderCheckbox.rows[i].cells[4].getElementsByTagName("INPUT")[0].setAttribute("required", "required");
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].className = "form-control required";
                }
                else {
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].setAttribute("disabled","disabled");
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].removeAttribute("required");
                    GridVwHeaderCheckbox.rows[i].cells[4].getElementsByTagName("INPUT")[0].removeAttribute("required");
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].className = "form-control";
                }
            }
        }


        <%--            $("input:checkbox").click(function () {
                if ($(this).is(":checked")) {
                    var GridView1 = document.getElementById('<%= gvEvalCommitteeAttend.ClientID %>');

                    for (var rowId = 1; rowId < GridView1.rows.length; rowId++) {
                        var textValue = GridView1.rows[rowId].cells[0].children[0];
                        alert(textValue.value);
                    }
                } else {
                    alert("false");
                }
            });--%>
        function AddFormRequired(Index) {
            var id = '#MainContent_gvAssetsList_FileUpload_' + Index + '_BidCtrl0_FU_' + Index;
            //console.log(id);
            $(id).attr("required", "required");
          }
        function RemoveFormRequired(Index) {
            var id = '#MainContent_gvAssetsList_FileUpload_' + Index + '_BidCtrl0_FU_' + Index;
            $(id).removeAttr("required");
          }

        
        function txtChange(txt) {
            var gridRow = txt.parentNode.parentNode;
            var inputs = gridRow.getElementsByTagName("input"); 
            var Spans = gridRow.getElementsByTagName("span");

            var Submitted = Number(inputs[1].value);
            var Earnest = Number(Spans[2].innerText.replace(/[^0-9\.]+/g, ""));
            console.log(Submitted);
            console.log(Earnest);
            
            if (Earnest < Submitted || Earnest > Submitted) {
                inputs[1].value = "";
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Submitted Money must be equal to Earnest Money");
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            }
        }
        function chkChanged(val) {
            var gridRow = val.parentNode.parentNode;
            var inputs = gridRow.getElementsByTagName("input");
            console.log(inputs[0].checked);
            if (inputs[0].checked) {
                inputs[1].setAttribute("required", "required");
                inputs[2].setAttribute("required", "required");
                inputs[1].removeAttribute("disabled");
                inputs[1].className = "form-control required";
            }
            else {
                inputs[1].removeAttribute("required");
                inputs[2].removeAttribute("required");
                inputs[1].setAttribute("disabled", "disabled");
                inputs[1].className = "form-control";
            }
            
            
            //var Submitted = Number(inputs[1].value);
           
          

            //if (Earnest < Submitted || Earnest > Submitted) {
            //    inputs[1].value = "";
            //    $('#lblMsgs').addClass('ErrorMsg').show();
            //    $('#lblMsgs').html("Submitted Money must be equal to Earnest Money");
            //    setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            //}
        }
    </script>
</asp:Content>
