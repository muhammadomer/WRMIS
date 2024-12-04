<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewComparativeStatement.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.ViewComparativeStatement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="uc1" TagName="ViewWorks" %>
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

        .rbl input[type="radio"] {
            width: 20px;
            display: block;
            float: left;
        }

        .rbl label {
            width: 200px;
            display: block;
            float: left;
        }



        div.ComparativeStatement {
            background: #fff none repeat scroll 0 0;
            border: 0px solid #666;
            margin: 5px;
            padding: 5px;
            position: relative;
            width: 1050px;
            height: 100%;
            overflow-y: hidden;
        }

        p {
            margin: 10px;
            padding: 5px;
            /*border: 2px solid #666;
    width: 1000px;
    height: 1000px;*/
        }
    </style>
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Comparative Statement</h3>
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
                            <li id="liPrice" runat="server" style="width: 20%; text-align: center"><a id="anchTenderPrice" runat="server" aria-controls="TenderPrice" role="tab">Tender Price</a></li>
                            <li id="liReport" runat="server" style="width: 20%; text-align: center"><a id="anchReport" runat="server" aria-controls="ADMReport" role="tab">ADM Report</a></li>
                            <li id="liStatement" runat="server" style="width: 20%; text-align: center" class="active"><a id="anchstatement" runat="server" aria-controls="ComparativeStatement" role="tab">Comparative Statement</a></li>

                        </ul>
                    </div>
                </div>

                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-12 ">
                            <div class="form-group">
                                <div class="col-sm-8 col-lg-9 controls">
                                    <%--<asp:RadioButton name="rdDiv" runat="server"  id="rdComparative"  ClientIDMode="Static" GroupName="Div"/>
                                       <asp:RadioButton name="rdDiv" runat="server"  id="rdAward"  ClientIDMode="Static" GroupName="Div"/>--%>

                                    <asp:RadioButtonList ID="rdComparativeStatement" runat="server" RepeatDirection="Horizontal" CssClass="rbl">

                                        <asp:ListItem Selected="true" Value="1">View Comparative Statement</asp:ListItem>
                                        <asp:ListItem Value="2">Award Contractor</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="row ComparativeStatement" id="DivComparativeStatement">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvComparativeStatement" runat="server" AutoGenerateColumns="true" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="False"
                                    OnRowDataBound="gvComparativeStatement_RowDataBound" OnRowCreated="gvComparativeStatement_RowCreated" ShowHeader="true" ShowFooter="False">
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <%--       <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-8" style="padding-left: 465px">
                                <asp:Label ID="Label1" runat="server" CssClass="control-label" ClientIDMode="Static">Total</asp:Label>
                            </div>
                            <div class="col-md-4" style="padding-left: 230px;">
                                <asp:Label ID="lblTotal" runat="server" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                            </div>

                        </div>

                    </div>--%>
                    <hr />
                    <asp:UpdatePanel runat="server" ID="update">
                        <ContentTemplate>
                            <div class="row" id="DivAward">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group" style="padding-left: 31px;">
                                            <asp:Label ID="lblText" runat="server" Text="Rate as Per Technical Sanctioned Estimate (Rs):"></asp:Label>
                                            <asp:Label ID="lblSanctionAmount" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>



                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvaward" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                            ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                            DataKeyNames="ID" OnRowDataBound="gvaward_RowDataBound">
                                            <Columns>

                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Contractors">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContractor" runat="server" CssClass="control-label" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-5" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" CssClass="control-label" Text='<%#Eval("Amount","{0:n0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 " />
                                                    <ItemStyle CssClass="text-right padding-right-number" />
                                                    <%--<HeaderStyle CssClass="col-md-1" />--%>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Award">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdButton" name="rdBAward" ClientIDMode="Static" runat="server" CssClass="control-label" Checked='<%# bool.Parse(Eval("Checked").ToString()) %>' GroupName="SelectGroup" onclick="RadioCheck(this);" />
                                                        <%--onclick = "RadioCheck(this);"--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtRemarks" type="text" required="true" CssClass="form-control textboxx " ClientIDMode="Static" Visible="true" Enabled="false" Text='<%# Eval("Remarks") %>'> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-5" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%--<hr />--%>
                            <div class="row" runat="server" id="divSave">
                                <div class="col-md-12" style="padding-left: 30px;">
                                    <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBackToWork" runat="server" CssClass="btn">&nbsp;Back to Tender Work</asp:HyperLink>
                                </div>
                            </div>



                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnWorkID" runat="server" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" />
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" />

    <script type="text/javascript">

        $(document).ready(function () {
            $('#DivComparativeStatement').show();
            $('#DivAward').hide();
            $('#MainContent_btnSave').hide();
            $('#MainContent_rdComparativeStatement_0').on('change', function () {

                if ($(this).is(':checked')) {
                    $('#DivComparativeStatement').show();
                    $('#DivAward').hide();
                    $('#MainContent_btnSave').hide();
                }
            });
            $('#MainContent_rdComparativeStatement_1').on('change', function () {
                if ($(this).is(':checked')) {
                    $('#DivComparativeStatement').hide();
                    $('#DivAward').show();
                    $('#MainContent_btnSave').show();
                }
            });



        });

        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gvaward.ClientID%>");
            var rbs = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        $("input[id*=txtRemarks]").attr("disabled", "disabled");
                        //$("input[id*=txtRemarks]").val("");
                        break;
                    }

                }
                else {
                    $("input[id*=txtRemarks]").attr("disabled", "disabled");
                    //$("input[id*=txtRemarks]").val("");
                }
            }


            //for (var i = 0; i < gv.rows.length - 1; i++) {
            //    console.log(rb.checked);
            //    if (rb.checked) {
            //        $("input[id*=txtRemarks]").val("abcdef");
            //    }



            //}

        }


        $("[id*=rdButton]").bind("click", function () {
            //Find and reference the GridView.
            var grid = $(this).closest("table");
            //If the CheckBox is Checked then enable the TextBoxes in thr Row.
            if (!$(this).is(":checked")) {
                var td = $("td", $(this).closest("tr"));
                //td.css({ "background-color": "#FFF" });
                $("input[type=text]", td).attr("disabled", "disabled");

            } else {
                var td = $("td", $(this).closest("tr"));
                //td.css({ "background-color": "#D8EBF2" });
                $("input[type=text]", td).removeAttr("disabled");

            }


        });


        //function ShowText(radio, textBox) {
        //    console.log(radio,textBox)
        //    var RadioButton = document.getElementById(radio);
        //    if (RadioButton.checked == true) {
        //        document.getElementById(textBox).style.visibility = "visible";
        //     //   $("#textBox").attr("enabled");
        //       // $("#textBox").removeAttr("disabled");
        //    }
        //}

        $("div.ComparativeStatement").scrollTop(300);



    </script>

</asp:Content>
