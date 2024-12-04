<%@ Page Title="BiddingProcess" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="BiddingProcess.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.BiddingProcess" %>


<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Bidding</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                     <uc1:AuctionNotice runat="server" ID="AuctionNotice" />
                </div>

          <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                                      <ul class="nav nav-tabs" role="tablist">
                           <li id="liCommittee" style="width: 20%; text-align: center" runat="server"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Auction Committee Attendance</a></li>
                            <li id="liBidders" style="width: 20%; text-align: center" runat="server"><a id="anchBidders" runat="server" aria-controls="BiddersAttendance" role="tab">Bidders Attendance</a></li>
                            <li id="liBidding" runat="server" style="width: 20%; text-align: center" class="active" ><a id="anchBidding" runat="server" aria-controls="Bidding" role="tab">Bidding</a></li>
                            <li id="liBidderSelection" runat="server" style="width: 20%; text-align: center"><a id="anchBidderSelection" runat="server" aria-controls="Bidder Selection" role="tab">Bidder Selection</a></li>

                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <%--<asp:UpdatePanel runat="server">
                        <ContentTemplate>--%>
                        <div class="col-md-3" style="float:right;"> 
                    <div style="float:right;">
               <%-- <asp:LinkButton ID="btnCloseAuction" runat="server" ClientIDMode="Static"  CssClass="btn btn-primary" Text="Close Auction"></asp:LinkButton>--%>
                        <button name="Close" id="btnCloseAuctionT" type="button" runat="server" class="btn btn-primary" onclick="$('#myModal').modal();">Close Asset</button>
                    <br /><br />
                        </div>
                    </div>
                        
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Assets</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList CssClass="form-control" required="required" ID="ddlAssets" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlAssets_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        
                    </div>
             <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                

                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvBiddersRate" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                DataKeyNames="ID,BidderID,AuctionAssetDetailID" OnRowDataBound="gvBiddersRate_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select Bidder">
                                        <%--<HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" CssClass="chkbox" runat="server" ClientIDMode="Static" Checked='<%# bool.Parse(Eval("isChecked").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BidderID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="BidderID" runat="server" CssClass="control-label" Text='<%# Eval("BidderID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auction Asset Detail ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="AuctionAssetDetailID" runat="server" CssClass="control-label" Text='<%# Eval("AuctionAssetDetailID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bidder Detail">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBidderDetail" runat="server" CssClass="control-label" Text='<%# Eval("BidderDetail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Rate by Bidder(Rs.)">
                                        
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtBidderRate"  type="text" class="form-control decimalInput" Text='<%# Eval("BidderRate") %>' ClientIDMode="Static" MaxLength="10" disabled="disabled"> </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                        
                    </div>
                </div>

                    <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content" style="height:275px;">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Close Auction</h4>
        </div>
        <div class="modal-body">
             <div class="form-horizontal">
         
                  <div class="col-md-12">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-2 control-label" runat="server" id="lblReason">Reason</label>
                                    <div class="col-sm-6 col-lg-7 controls"> 
                                        <asp:TextBox ID="txtReason" TextMode="multiline" ClientIDMode="Static"  Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize txtReason" TabIndex="5" MaxLength="250"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                           <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-lg-2 control-label" runat="server" id="lblUpload">Upload</label>
                            <div class="col-sm-3 col-lg-4 controls">
                             <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                            </div>
                            <asp:HyperLink ID="hlAttachment" runat="server" Visible="false" CssClass="btn btn-primary btn_24 viewimg" title="" FielName=""></asp:HyperLink>
                        </div>
                    </div>
            </div>
                 </div>
          <div class="form-horizontal" style="float:right">
             <div class="col-md-12" style="padding-right:30px;">
                        <div class="form-group">

                            <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
         <asp:Button ID="LinkButton" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnClose_Click"></asp:Button>

                            </div>

             </div>
              </div>
            </div>
         <%--  <div class="modal-footer" style="border-top:none;">
      
        </div>--%>
        </div>
       
      </div>

                    <%--Old Closing DIv--%>
                    
                           <%--<div class="row">

                                                          <div class="col-md-12">
                        <div class="form-group">
                            <label for="ChkBoxStatus" class="col-sm-2 col-lg-1 control-label" style="padding-top:6px;" runat="server" id="lblStatus">Status</label>
                            <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                <asp:CheckBox CssClass="checkbox" ID="ChkBoxStatus" runat="server" Text="Closed" ClientIDMode="Static"/>
                            </div>
                        </div>
                    </div>


                                                           <div class="col-md-12">
                                <div class="form-group">
                                    <label class="col-sm-2 col-lg-1 control-label" runat="server" id="lblReason">Reason</label>
                                    <div class="col-sm-6 col-lg-7 controls"> 
                                        <asp:TextBox ID="txtReason" TextMode="multiline" ClientIDMode="Static"  Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize txtReason" TabIndex="5" MaxLength="250" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                           <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-lg-2 control-label" runat="server" id="lblUpload">Upload</label>
                            <div class="col-sm-3 col-lg-4 controls">
                             <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                            </div>
                            <asp:HyperLink ID="hlAttachment" runat="server" Visible="false" CssClass="btn btn-primary btn_24 viewimg" title="" FielName=""></asp:HyperLink>
                        </div>
                    </div>

                                                        </div>--%>
               
                      <%--<div class="" id="HyperLinksDiv" runat="server" visible="false">

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <h4>Attachments</h4>
                                        <table id="tblHyperlinks" runat="server">
                                        </table>
                                    </div>
                                </div>
                            </div>--%>

                    <div class="row" runat="server" id="divSave">
                                <div class="col-md-12">
                                   <%-- <asp:LinkButton TabIndex="10" ID="LinkButton1" runat="server" Text="Save & Proceed" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:LinkButton>--%>
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <%--<asp:HyperLink ID="hlNext" runat="server" CssClass="btn btn-primary">&nbsp;Next</asp:HyperLink>--%>
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                                    <%--<asp:HyperLink ID="hlBackToWork" runat="server" CssClass="btn">&nbsp;Back to Tender Work</asp:HyperLink>--%>
                                </div>
                            </div>
              
                    </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" />
    

    <script type="text/javascript">
        // for check all checkbox  
        <%--function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvBiddersRate.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                // debugger;
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                if (GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked) {
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].removeAttribute('disabled');
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].removeAttribute('disabled');
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].setAttribute("required", "required");
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].className = "form-control required";
                }
                else {
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].setAttribute("disabled", "disabled");
                    GridVwHeaderCheckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].setAttribute("disabled", "disabled");
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].removeAttribute("required");
                    GridVwHeaderCheckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].className = "form-control";
                }


            }
        }--%>


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


    </script>

    <script type="text/javascript">
        $(function () {
            $('.CtrlClass0').removeAttr("required");
            //$("input[type=text]").each(function (idx, elem) {
            //    var textboxVal = $(elem).val();
            //    if (textboxVal === "") {

            //        $(elem).attr("disabled", "disabled");
            //    }
            //    else {
            //        $(elem).removeAttr("disabled");
            //    }

            //});

            $("[id*=ChkBoxStatus]").bind("click", function () {

                if ($('#ChkBoxStatus').is(':checked')) {

                    $('#txtReason').removeAttr("disabled");
                    $('#txtReason').attr("required", "required");
                    $('#txtReason').addClass("required");
                    $('.CtrlClass0').attr("required", "required");
                }
                else {
                    $('#txtReason').attr("disabled","disabled");
                    $('#txtReason').removeAttr("required");
                    $('#txtReason').removeClass("required");
                    $('.CtrlClass0').removeAttr("required");
                }
              
            });


            //Enable Disable TextBoxes in a Row when the Row CheckBox is checked.
            $("[id*=chkSelect]").bind("click", function () {

                //Find and reference the GridView.
                var grid = $(this).closest("table");



                //If the CheckBox is Checked then enable the TextBoxes in thr Row.:eq(3)
                if (!$(this).is(":checked")) {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#FFF" });
                    $("input[type=text]", td).attr("disabled", "disabled");
                    $("#txtBidderRate", td).removeAttr("required");
                    $("#txtBidderRate", td).removeClass("required");

                } else {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#D8EBF2" });
                    $("input[type=text]", td).removeAttr("disabled");
                    //debugger;
                    $("#txtBidderRate", td).attr("required", "required");
                    $("#txtBidderRate", td).addClass("required");

                }
                //debugger;
                //if ($('.chkbox:checked').length == 0) {

                //    alert("no");
                //}
                
                //var totalCheckboxes = $("#<%=gvBiddersRate.ClientID%> input[id*='chkSelect']:checkbox").size();
                var checkedCheckboxes = $("#<%=gvBiddersRate.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

                if (checkedCheckboxes == 0)
                {
                    $('#ChkBoxStatus').removeAttr("disabled"); 
                    //$('#txtReason').removeAttr("disabled");
                    //$('#txtReason').addClass("required");
                    //$('#txtReason').attr("required","required");
                    //$('.CtrlClass0').attr("required","required");
                }
                else {
                    $('#ChkBoxStatus').attr("disabled","disabled");
                    //$('#txtReason').attr("disabled","disabled");
                    //$('#txtReason').removeClass("required");
                    //$('#txtReason').removeAttr("required");
                    //$('.CtrlClass0').removeAttr("required");
                }
       
    


            });


        });

        //function OpenModal() {
            
        //    $("#myModal").modal();
        
        //}
           
            

     
        function RemoveRequired() {
            $('.CtrlClass0').removeAttr("required");
        }
</script>
</asp:Content>

