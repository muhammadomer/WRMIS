<%@ Page Title="BiddersAttendance" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="BiddersAttendance.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.BiddersAttendance" %>

<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Bidders Attendance</h3>
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
                            <li id="liBidders" style="width: 20%; text-align: center" runat="server" class="active"><a id="anchBidders" runat="server" aria-controls="BiddersAttendance" role="tab">Bidders Attendance</a></li>
                            <li id="liBidding" runat="server" style="width: 20%; text-align: center" ><a id="anchBidding" runat="server" aria-controls="Bidding" role="tab">Bidding</a></li>
                            <li id="liBidderSelection" runat="server" style="width: 20%; text-align: center"><a id="anchBidderSelection" runat="server" aria-controls="Bidder Selection" role="tab">Bidder Selection</a></li>

                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <%--<asp:UpdatePanel runat="server">
                        <ContentTemplate>--%>

                        
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label" id="lblAsset" runat="server">Assets</label>
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
                            <asp:GridView ID="gvBiddersAttendance" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                DataKeyNames="ID" OnRowDataBound="gvBiddersAttendance_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" Checked='<%# bool.Parse(Eval("isAttended").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Company/Bidder Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBidder" runat="server" CssClass="control-label" Text='<%# Eval("Bidder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Bidder Name">
                                        
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtAlternateName"  type="text" class="form-control" Text='<%# Eval("AlternateName") %>' ClientIDMode="Static" MaxLength="100" disabled="disabled"> </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Remarks">
                                      
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtAlternateRemarks" type="text" class="form-control" Text='<%# Eval("AlternateRemarks") %>' ClientIDMode="Static" MaxLength="100" disabled="disabled"> </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-lg-2 control-label" id="lblAtt" runat="server">Attach Attendance Sheet</label>
                            <div class="col-sm-2 col-lg-3 controls">
                             <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                            </div>
                            <%--<asp:HyperLink ID="hlAttachment" runat="server" Visible="false" FielName=""></asp:HyperLink>--%>
                            <div><uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" /></div>
                            
                        </div>
                    </div>
                </div>
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
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvBiddersAttendance.ClientID %>");
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


    </script>

    <script type="text/javascript">
        $(function () {
            var IsXen = "<%= base.CanAdd %>";
            //console.log(IsXen);
                if (IsXen == "True") {
                    $("input[type=text]").each(function (idx, elem) {
                        var textboxVal = $(elem).val();
                        if (textboxVal === "") {

                            $(elem).attr("disabled", "disabled");
                        }
                        else {
                            $(elem).removeAttr("disabled");
                        }

                    });
                }
       




            //Enable Disable TextBoxes in a Row when the Row CheckBox is checked.
            $("[id*=chkSelect]").bind("click", function () {

                //Find and reference the GridView.
                var grid = $(this).closest("table");



                //If the CheckBox is Checked then enable the TextBoxes in thr Row.:eq(3)
                if (!$(this).is(":checked")) {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#FFF" });
                    $("input[type=text]", td).attr("disabled", "disabled");
                    $("#txtAlternateName", td).removeAttr("required");
                    $("#txtAlternateName", td).removeClass("required");

                } else {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#D8EBF2" });
                    $("input[type=text]", td).removeAttr("disabled");
                    //debugger;
                    $("#txtAlternateName", td).attr("required", "required");
                    $("#txtAlternateName", td).addClass("required");

                }


            });


        });
        function RemoveRequired() {
            $('.CtrlClass0').removeAttr("required");
        }
</script>
</asp:Content>
