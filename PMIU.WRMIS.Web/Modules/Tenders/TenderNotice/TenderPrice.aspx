<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TenderPrice.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.TenderPrice" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="uc1" TagName="ViewWorks" %>
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
                <h3 id="htitle" runat="server">Tender Price</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <uc1:ViewWorks runat="server" ID="ViewWorks" />
                </div>

                <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li id="liCommittee" style="width: 20%; text-align: center" runat="server"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Committee Attendance</a></li>
                            <li id="liContractor" style="width: 20%; text-align: center" runat="server"><a id="anchContractor" runat="server" aria-controls="ContractorsAttendance" role="tab">Contractors Attendance</a></li>
                            <li id="liPrice" runat="server" style="width: 20%; text-align: center" class="active"><a id="anchTenderPrice" runat="server" aria-controls="TenderPrice" role="tab">Tender Price</a></li>
                            <li id="liReport" runat="server" style="width: 20%; text-align: center"><a id="anchReport" runat="server" aria-controls="ADMReport" role="tab">ADM Report</a></li>
                            <li id="liStatement" runat="server" style="width: 20%; text-align: center"><a id="anchstatement" runat="server" aria-controls="ComparativeStatement" role="tab">Comparative Statement</a></li>

                        </ul>
                    </div>
                </div>

                <div class="form-horizontal">

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Company Name</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList CssClass="form-control required" required="true" ID="ddlCompanyName" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Estimate</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control EstimateVal required" required="true" ID="ddlEstimate" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlEstimate_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>

                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <div class="form-group">

                                        <div class="col-sm-4 controls">
                                            <asp:TextBox runat="server" ID="txtValue" type="text" AutoPostBack="true" MaxLength="90" CssClass="form-control Percentage" OnTextChanged="Calculate_Amount"> </asp:TextBox>

                                        </div>
                                        <label runat="server" id="lblPercentage" class="control-label">%</label>
                                    </div>
                                </div>
                                <%--<div class="col-md-6">
                                  <label runat="server" id="lblPercentage" class="col-sm-4 col-lg-3 control-label"  >%</label>  <asp:TextBox runat="server" ID="txtValue" type="text" class="form-control" > </asp:TextBox>
                                         
                            </div>--%>
                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvTenderPrice" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                            ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                            DataKeyNames="ID" OnRowDataBound="gvTenderPrice_RowDataBound">
                                            <Columns>

                                                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstimateType" runat="server" CssClass="control-label" Text='<%# Eval("EstimateType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="TP" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTPID" runat="server" CssClass="control-label" Text='<%# Eval("TPID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Item Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemDescription" runat="server" CssClass="control-label" Text='<%# Eval("ItemDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sanctioned Quantity">
                                                    <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                    <ItemTemplate>

                                                        <asp:TextBox runat="server" ID="txtSancQuan" Style="text-align: end;" type="text" Text='<%# Eval("SanctionedQty") %>' class="form-control" ClientIDMode="Static" ReadOnly="true"> </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Sanctioned Quantity" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSancQuantity" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit">
                                                    <%--        <HeaderStyle CssClass="col-md-2" />--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" CssClass="control-label" Text='<%# Eval("TSUnitName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Sanctioned Rate(Rs)">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblTSRate" runat="server" CssClass="control-label" Text='<%# Eval("TSRate") %>'></asp:Label>--%>
                                                        <asp:TextBox runat="server" ID="txtTSRate" type="text" Style="text-align: end;" Text='<%# Eval("TSRate") %>' class="form-control" ClientIDMode="Static" ReadOnly="true"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <%--       <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                                <ItemStyle CssClass="text-right padding-right-number" />--%>
                                                </asp:TemplateField>





                                                <asp:TemplateField HeaderText="Rate By Contractor(Rs)">
                                                    <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtContractorRate" MaxLength="90" Enabled="false" type="text" Style="text-align: end;" CssClass="form-control decimalInput integerInput" OnTextChanged="Calculate_AmountG" AutoPostBack="true" Text='<%# Eval("ContractorRate") %>'> </asp:TextBox>
                                                        <%--onclick="Calculation()"--%>
                                                        <%--onclick="Calculation(this.value)"--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount(Rs)">
                                                    <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtAmount" type="text" Style="text-align: end;" CssClass="form-control" ClientIDMode="Static" disabled="disabled" Text='<%# Eval("TotalItemAmount") %>'> </asp:TextBox>
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
                                    <div class="col-md-10" style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" CssClass="control-label" ClientIDMode="Static">Total</asp:Label>
                                    </div>
                                    <div class="col-md-2" style="text-align: right">
                                        <asp:Label ID="lblTotal" runat="server" CssClass="control-label TotalVal" ClientIDMode="Static"></asp:Label>
                                        <%--<label id="lblTotal" class="control-label TotalVal"></label>--%>
                                    </div>

                                </div>

                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr />


                    <div class="row">

                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvCallDepositDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    OnRowCommand="gvCallDepositDetail_RowCommand" OnRowDeleting="gvCallDepositDetail_RowDeleting"
                                    OnRowDataBound="gvCallDepositDetail_RowDataBound" OnRowEditing="gvCallDepositDetail_RowEditing"
                                    OnRowCancelingEdit="gvCallDepositDetail_RowCancelingEdit" OnRowUpdating="gvCallDepositDetail_RowUpdating"
                                    DataKeyNames="ID">
                                    <Columns>

                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Attachment" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAttachmentVal" runat="server" CssClass="control-label" Text='<%# Eval("Attachment") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTenerWContractorID" runat="server" CssClass="control-label" Text='<%# Eval("TenerWContractorID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Call Deposit No">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblCDRNo" runat="server" CssClass="control-label" Text='<%# Eval("CDRNO") %>'></asp:Label>--%>
                                                <asp:TextBox ID="txtCDRNo" runat="server" CssClass="form-control decimalIntegerInput" Width="70%" required="required" pattern=".{3,20}" MaxLength="20" Text='<%# Eval("CDRNO") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCDNo" class="form-control decimalIntegerInput required" pattern=".{3,20}" runat="server" TabIndex="1" required="true" MaxLength="20" Text='<%# Eval("CDRNO") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bank Detail">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBankDetail" runat="server" CssClass="form-control " Width="70%" required="required" MaxLength="250" pattern="[a-zA-Z0-9@#$%&*+\-_(),+':;?.,![\]\s\\/]+$" Text='<%# Eval("BankDetail") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDetail" class="form-control required" runat="server" MaxLength="250" TabIndex="2" required="true" pattern="[a-zA-Z0-9@#$%&*+\-_(),+':;?.,![\]\s\\/]+$" Text='<%# Eval("BankDetail") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount(Rs)">
                                            <%--        <HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAmount" runat="server" CssClass="control-label" Text='<%# Eval("Amount") %>'></asp:Label>--%>
                                                <asp:TextBox ID="txtDepositAmount" Style="text-align: end;" runat="server" CssClass="form-control decimalIntegerInput " Width="70%" required="required" pattern=".{3,20}" MaxLength="20" Text='<%# Eval("Amount") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAmount" class="form-control required decimalIntegerInput" runat="server" TabIndex="3" pattern=".{3,20}" required="true" MaxLength="20" Text='<%# Eval("Amount") %>'></asp:TextBox>
                                            </EditItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Call Deposit Receipt">
                                            <%--        <HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <%--  <asp:HyperLink ID="hlImage" NavigateUrl='<%# PMIU.WRMIS.Common.Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.TenderManagement , Convert.ToString(Eval("Attachment"))) %>' CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />--%>
                                                <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("Attachment")%>' Visible="false"></asp:Label>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" />
                                                <div id="FileUploadDiv" runat="server" visible="true">
                                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                                </div>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <%--<uc1:FileUploadControl runat="server" ID="FileUpload3" Size="1"  Visible="false" Name="UploadEdit" />--%>
                                                <uc1:FileUploadControl runat="server" ID="FileUpload" Size="1" Name="GridCtrl" />
                                                <asp:HyperLink ID="hlBankRecieptLnk" CssClass="" Visible="false" runat="server" Text="abc" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                    <asp:Button ID="btnAddBankDepositGrid" runat="server" Text="" Visible="<%# base.CanAdd %>" ToolTip="Add" OnClick="AddRow_Grid" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlActionAdvertisement" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="false"></asp:Button>
                                                    <asp:Button ID="lbtnDeleteAdvertisement" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="true"
                                                        OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />

                                                </asp:Panel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                </asp:Panel>
                                            </EditItemTemplate>

                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-8 text-right">
                            <div class="col-md-9">
                                <asp:Label ID="lblCallDepositTotal" runat="server" CssClass="control-label" ClientIDMode="Static" Style="padding-right: 10px;">Total</asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblCallDeposit" runat="server" CssClass="control-label" ClientIDMode="Static" Style="padding-right: 10px;"></asp:Label>
                            </div>

                        </div>

                    </div>

                    <hr />
                    <div class="row" runat="server" id="divSave">
                        <div class="col-md-12">
                            <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            <asp:HyperLink ID="hlBackToWork" runat="server" CssClass="btn">&nbsp;Back to Tender Work</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnWorkID" runat="server" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" />
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" />
    <asp:HiddenField ID="hdnTotalAmount" runat="server" Value="0" />

    <script type="text/javascript">
        DepositAmount();
        CalculationItemRate();
        //Calculation();


        //$('.blurClass').blur(function () {
        //    console.log("called");
        //    var value = $(this).val();
        //    CalculationItemRate();
        //MainContent_gvCallDepositDetail_FileUploadControl_0_ctrl0_FU_0
        //});
        function RemoveFormRequired(Index) {
            var id = '#MainContent_gvCallDepositDetail_FileUploadControl_' + Index + '_ctrl0_FU_' + Index;
            $(id).removeAttr("required");
        }
        function CalculationItemRate() {

            var sanction;
            var itemamount;
            var TotalAmount;
            var countSource = 0;
            var gv = document.getElementById("<%=gvTenderPrice.ClientID %>");

            for (var i = 0; i < gv.rows.length - 1; i++) {
                var txtAmountReceive = $("input[id*=txtContractorRate]");
                var sanction = $("input[id*=txtSancQuan]");
                var sanvalue = sanction[i].value;
                itemamount = txtAmountReceive[i].value;
                var val1 = Number(sanvalue.replace(/[^0-9\.]+/g, ""));
                var Val2 = Number(itemamount.replace(/[^0-9\.]+/g, ""));


                if (txtAmountReceive[i].value != '') {
                    //debugger;
                    //console.log(parseInt(Val2) * parseInt(val1));
                    var val = parseFloat(Val2) * parseFloat(val1);
                    var Total = round(val, 2);
                    Total = format1(Total);
                    $("input[id*=txtAmount]")[i].value = Total;

                }
            }

            for (var i = 0; i < gv.rows.length - 1; i++) {
                var txtAmountReceives = $("input[id*=txtAmount]");
                if (txtAmountReceives[i].value != '') {
                    var itemamounts = txtAmountReceives[i].value;
                    //console.log(itemamounts);
                    var TotalAmount = Number(itemamounts.replace(/[^0-9\.]+/g, ""));
                    countSource += TotalAmount;
                    var Total = round(countSource, 2);
                    console.log(Total);

                }

            }
            var Total = format1(countSource);
            $('#lblTotal').text(Total);

        }

        function round(value, exp) {
            if (typeof exp === 'undefined' || +exp === 0)
                return Math.round(value);

            value = +value;
            exp = +exp;

            if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
                return NaN;

            // Shift
            value = value.toString().split('e');
            value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

            // Shift back
            value = value.toString().split('e');
            return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
        }
        function Calculation() {

            var sanction;
            var itemamount;
            var TotalAmount;
            var countSource = 0;
            var gv = document.getElementById("<%=gvTenderPrice.ClientID %>");

            for (var i = 0; i < gv.rows.length - 1; i++) {
                var txtAmountReceives = $("input[id*=txtAmount]");
                if (txtAmountReceives[i].value != '') {
                    var itemamounts = txtAmountReceives[i].value;
                    var number = Number(itemamounts.replace(/[^0-9\.]+/g, ""));
                    var TotalAmount = parseFloat(number);
                    countSource += TotalAmount;
                    var Total = round(countSource, 2);
                    //console.log(countSource);

                }

            }
            Total = format1(Total);
            $('#lblTotal').text(Total);

        }

        //$('.blurDepositClass').blur(function () {

        //    DepositAmount();

        //});

        function DepositAmount() {
            var countSource = 0;
            var gv = document.getElementById("<%=gvCallDepositDetail.ClientID %>");
            for (var i = 0; i < gv.rows.length - 1; i++) {
                var txtAmount = $("input[id*=txtDepositAmount]");
                if (txtAmount.length == 0) {

                }
                else {
                    if (txtAmount[i].value != '') {
                        var itemamounts = txtAmount[i].value;
                        var number = Number(itemamounts.replace(/[^0-9\.]+/g, ""));

                        var TotalAmount = parseFloat(number);
                        countSource += TotalAmount;

                    }
                }


            }
            countSource = format1(countSource);
            $('#lblCallDeposit').text(countSource);
        }

        function format1(n) {
            return n.toFixed(2).replace(/./g, function (c, i, a) {
                return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
            });
        }

       <%-- function FillValues()
        {
            $('.TotalVal').text("");
            var Rt = "";
            var Qty = ""; 
            debugger;
            var gv = document.getElementById("<%=gvTenderPrice.ClientID %>");
            var ddlEstimateVal = $('.EstimateVal').val();
            if (ddlEstimateVal == 1) {
                $('.Percentage').val("");
                $('.Percentage').attr("disabled", "disabled");
                $('.Percentage').removeClass("required");
                $('.Percentage').removeAttr("required");
                $.each(gv.rows, function (key, value) {
                   if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 1) {
                                Rt = value.innerText;
                            }
                            if (key == 2) {
                              Qty = $(this).find("input").val();
                            }
                            if (key == 4) {
                                $(this).find("input").removeAttr("disabled");
                                $(this).find("input").val(Rt);
                            }
                            if (key == 5) {
                                var Quantity = parseInt(Qty);
                                var Rate = Number(Rt.replace(/[^0-9\.]+/g, ""));
                                $(this).find("input").val(Quantity * Rate);
                            }

                        });
                    }

                });
                Calculation();
            }
            else if (ddlEstimateVal == 2 || ddlEstimateVal == 3) {
                $.each(gv.rows, function (key, value) {
                    if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 4) {
                                $(this).find("input").attr("disabled", "disabled");
                                $(this).find("input").val("");
                            }
                            if (key == 5) {
                                $(this).find("input").val("");
                            }

                        });
                    }

                });
               
                $('.Percentage').removeAttr("disabled");
                $('.Percentage').attr("required", "required");
                $('.Percentage').addClass("required");
                $('.Percentage').val("");
            }
            else if (ddlEstimateVal == 4) {

                $.each(gv.rows, function (key, value) {
                    if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 4) {
                                $(this).find("input").removeAttr("disabled");
                                $(this).find("input").val("");
                            }
                            if (key == 5) {
                                $(this).find("input").val("");
                            }

                        });
                    }

                });

                $('.Percentage').attr("disabled", "disabled");
                $('.Percentage').removeClass("required");
                $('.Percentage').removeAttr("required");
                $('.Percentage').val("");
            }
            
            
        } 

        function FillGridValues()
        {
            var Rt = "";
            var Qty = ""; 
            debugger;
            var gv = document.getElementById("<%=gvTenderPrice.ClientID %>");
            var ddlEstimateVal = $('.EstimateVal').val();
            var txtpercentage = $('.Percentage').val();

            if (txtpercentage != "") {

                if (ddlEstimateVal == 2) {
                $.each(gv.rows, function (key, value) {
                    if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 1) {
                                Rt = value.innerText;
                            }
                            if (key == 2) {
                                Qty = $(this).find("input").val();
                            }
                            if (key == 4) {
                                $(this).find("input").removeAttr("disabled");
                                var RateOld = Number(Rt.replace(/[^0-9\.]+/g, ""));
                                var per = parseFloat(txtpercentage);
                                var AbvPer = (RateOld / 100) * per;
                                var Rate = RateOld + AbvPer;
                                $(this).find("input").val(Rate);
                            }
                            if (key == 5) {
                                var Quantity = parseInt(Qty);
                                var RateO = Number(Rt.replace(/[^0-9\.]+/g, ""));
                                var per = parseFloat(txtpercentage);
                                var AbvPer = (RateO / 100) * per;
                                var Rate = RateO + AbvPer;

                                $(this).find("input").val(Quantity * Rate);
                            }

                        });
                    }

                });
                Calculation();
            }
            else if (ddlEstimateVal == 3) {

                $.each(gv.rows, function (key, value) {
                    if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 1) {
                                Rt = value.innerText;
                            }
                            if (key == 2) {
                                Qty = $(this).find("input").val();
                            }
                            if (key == 4) {
                                $(this).find("input").removeAttr("disabled");
                                var RateOld = Number(Rt.replace(/[^0-9\.]+/g, ""));
                                var per = parseInt(txtpercentage);
                                var AbvPer = (RateOld / 100) * per;
                                var Rate = RateOld - AbvPer;
                                $(this).find("input").val(Rate);
                            }
                            if (key == 5) {
                                var Quantity = parseInt(Qty);
                                var RateO = Number(Rt.replace(/[^0-9\.]+/g, ""));
                                var per = parseInt(txtpercentage);
                                var AbvPer = (RateO / 100) * per;
                                var Rate = RateO - AbvPer;

                                $(this).find("input").val(Quantity * Rate);
                            }

                        });
                    }

                });
                Calculation();
            }
            }
            else {
                $.each(gv.rows, function (key, value) {
                    if (key > 0) {
                        $.each(value.cells, function (key, value) {
                            if (key == 4) {
                                $(this).find("input").removeAttr("disabled");
                                $(this).find("input").val("");
                            }
                            if (key == 5) {
                                $(this).find("input").val("");
                            }

                        });
                    }

                });
                $('.TotalVal').text("");
            }
              
        }--%>
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                }
            });
        };



    </script>
</asp:Content>
