<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplaintsDashboard.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.ComplaintsDashboard" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Complaints Dashboard</h3>
                </div>
                <div class="box-content" style="height: 525px;">

                    <div class="form-horizontal">
                        <div class="row">

                            <div class="col-md-12">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">From </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtFromDate" ClientIDMode="Static" TabIndex="1" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                    <div class="col-md-3 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">To </label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtToDate" ClientIDMode="Static" TabIndex="2" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">By</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control Hide" ID="ddlShowComplaints" ClientIDMode="Static"  style="width:135px;" runat="server" TabIndex="3" onchange="ddlHideShowComplaintsBy(this)">
                                                </asp:DropDownList>
                                                <%--OnSelectedIndexChanged="ddlShowComplaints_SelectedIndexChanged"--%>
                                            </div>
                                        </div>
                                        </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label id="lblComplaintType" clientidmode="Static" class="col-sm-4 col-lg-3 control-label" runat="server">Type</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control Hide" ClientIDMode="Static"  ID="ddlComplaintType" runat="server" TabIndex="4">
                                                </asp:DropDownList>
                                                
                                                <%-- OnSelectedIndexChanged="ddlComplaintType_SelectedIndexChanged"--%>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <label id="Label1" clientidmode="Static" class="col-sm-4 col-lg-3 control-label" runat="server"></label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%-- OnSelectedIndexChanged="ddlComplaintType_SelectedIndexChanged"--%>
                                                <asp:LinkButton TabIndex="5" ID="btnComplaintsSearch" ClientIDMode="Static" runat="server" Text="Refresh" CssClass="btn btn-primary"><%--<i class="fa fa-search"></i>--%>Refresh</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            


                            <div class="col-md-4 " >
                               <%-- style="border-right: 1px #E3E3E3 solid;"--%>
                                <div id="chartscount" runat="server">
                                    <div id="charts" class="col-md-12 text-center" style="width: 100%; height: 300px;"></div>
                                </div>

                            </div>

                            <div class="col-md-8">



                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="barchars" runat="server" clientidmode="Static">
                                            <div id="barchart_Counts" class="col-md-12 text-center" style="width: 100%; height: 300px;"></div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div id="CompTypeChart" runat="server" clientidmode="Static">
                                    <div id="ComplaintTypeChart" class="col-md-12 text-center" style="width: 100%; height: 300px; padding-left: 119px"></div>
                                </div>




                            </div>

                        </div>

                        <br />
                        <br />
                        <br />
                        <br />

                        <br />
                        <br />
                        <br />
                              <br />
                        <br />
                        <br />
                        <br />

                        <br />
                        <br />
                        <br />
                     <br />
                        <br />
                        <br />
                        <br />

                        <br />
                        <br />
                        <br />
                              <br />
                        <br />
                     <br />
                        <br />
                        <asp:Label ID="lblStatusTitle"  runat="server" Style="padding-left: 150px;">Total Complaints by Status:</asp:Label>
                        <asp:Label ID="lblStatuCount" runat="server" ClientIDMode="Static"></asp:Label>

                        <asp:Label ID="lblCompSource" ClientIDMode="Static" runat="server" Style="padding-left: 400px;" >Total Complaints by Source :</asp:Label>
                        <asp:Label ID="lblSourceComplaints" ClientIDMode="Static" runat="server" ></asp:Label>

                        <asp:Label ID="lblCompType" runat="server" Style="padding-left: 400px;" >Total Complaint by Types:</asp:Label>
                        <asp:Label ID="lblTotalComplaintTypes" runat="server" ClientIDMode="Static" ></asp:Label>

<%--                        <br /><br />
                        <asp:Label ID="lblLegend" runat="server" CssClass="control-label" ClientIDMode="Static">
                           DF=Default Source,CM=Chief Minister,CS=ChieftSecretary,SI=Secretary Irrigation
                            TP=Tehsil Programme,AG=Auto Generated,MI= Minister Irrigation

                        </asp:Label>--%>


                        <%--    <asp:Label ID="lblSourceTitle" runat="server" style="padding-left: 480px;">Total Complaint By Source:</asp:Label>
                        <asp:Label ID="lblSourceCount" runat="server"></asp:Label>--%>
                        <hr />

                        <%--      <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-2 col-md-offset-10">
                                    <asp:Button runat="server" ID="btnAdvanceSearch" value="Search" CssClass="btn btn-primary" Text="Advance Search" OnClick="btnAdvanceSearch_Click" />
                                </div>
                            </div>
                        </div>--%>



                        <asp:HiddenField ID="InBox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="InProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="Resolved" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="hdnDefaultSourceInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnDefaultSourceInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnDefaultSourceResolved" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="hdnChiefMinisterInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnChiefMinisterInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnChiefMinisterResolved" runat="server" ClientIDMode="Static" />


                        <asp:HiddenField ID="hdnChieftSecretaryInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnChieftSecretaryInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnChieftSecretaryResolved" runat="server" ClientIDMode="Static" />


                        <asp:HiddenField ID="hdnSecretaryIrrigationInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnSecretaryIrrigationInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnSecretaryIrrigationResolved" runat="server" ClientIDMode="Static" />


                        <asp:HiddenField ID="hdnTehsilProgrammeInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnTehsilProgrammeInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnTehsilProgrammeResolved" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="hdnAutomaticInbox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnAutomaticInprogess" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnAutomaticResolved" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="CompTypeInBox" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="CompTypeInProgress" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="CompTypeResolved" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="hdnComplaintName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnComplaintTitle" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnComplaintTypeId" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnStatusComplaintName" runat="server" ClientIDMode="Static" />
                    </div>
                </div>
            </div>
        </div>



    <!-- END Main Content -->
    <script src="../../Scripts/Complaints/loader.js"></script>
    <script src="../../Scripts/Complaints/jsapi.js"></script>

    <script type="text/javascript">
        var lblComplaintTypes = $('#<%= lblTotalComplaintTypes.ClientID %>');
        var lblComType = $('#<%= lblCompType.ClientID %>');

        var lblTitleSource = $('#<%= lblCompSource.ClientID %>');
        var lblSource = $('#<%= lblSourceComplaints.ClientID %>');

        var lblStatus = $('#<%= lblStatuCount.ClientID %>');
        var lblStatusTitle = $('#<%= lblStatusTitle.ClientID %>');



        $(document).ready(function () {

            $('.CtrlClass0').blur();
            $('.CtrlClass0').removeAttr('required');
            $('#ddlComplaintType').hide();
            $('#lblComplaintType').hide();
            $('#CompTypeChart').hide();
            lblComType.hide();
            lblTitleSource.hide();
            lblSource.hide();
            lblStatus.hide();
            lblStatusTitle.hide();
            $('#btnComplaintsSearch').click(function (event) {
                lblStatus.show();
                lblStatusTitle.show();

                event.preventDefault();
                var FromDate = $("#txtFromDate").val();
                var ToDate = $("#txtToDate").val();
                var CompTypeID = $('#ddlComplaintType').val();
                var CompByID = $('#ddlShowComplaints').val();
                if (CompByID == "2") {
                    lblComplaintTypes.hide();
                    lblComType.hide();
                    lblTitleSource.show();
                    lblSource.show();
                    window.localStorage.setItem("DefaultSourceInbox", 0);
                    window.localStorage.setItem("DefaultSourceInProgress", 0);
                    window.localStorage.setItem("DefaultSourceResolved", 0);
                    window.localStorage.setItem("ChiefMinisterInbox", 0);
                    window.localStorage.setItem("ChiefMinisterInprogress", 0);
                    window.localStorage.setItem("ChiefMinisterResolved", 0);
                    window.localStorage.setItem("ChieftSecretaryInbox", 0);
                    window.localStorage.setItem("ChieftSecretaryInProcess", 0);
                    window.localStorage.setItem("ChieftSecretaryResolved", 0);
                    window.localStorage.setItem("SecretaryIrrigationInbox", 0);
                    window.localStorage.setItem("SecretaryIrrigationInProcess", 0);
                    window.localStorage.setItem("SecretaryIrrigationResolved", 0);
                    window.localStorage.setItem("TehsilProgrammeInbox", 0);
                    window.localStorage.setItem("TehsilProgrammeInProcess", 0);
                    window.localStorage.setItem("TehsilProgrammeResolved", 0);
                    window.localStorage.setItem("AutoGeneratedInbox", 0);
                    window.localStorage.setItem("AutoGeneratedInProcess", 0);
                    window.localStorage.setItem("AutoGeneratedResolved", 0);
                    window.localStorage.setItem("MinisterIrrigationInbox", 0);
                    window.localStorage.setItem("MinisterIrrigationInProcess", 0);
                    window.localStorage.setItem("MinisterIrrigationResolved", 0);

                    $.ajax({
                        type: "POST",
                        url: 'ComplaintsDashboard.aspx/GetComplaintResultsBySource',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            FromDate: FromDate, ToDate: ToDate
                        }),
                        success: function (result) {
                            var countSource = 0;
                           // console.log(result.d.length);
                            $.each(result.d, function (key, val) {

                                var ComplaintSource = val.ComplaintSourceID;
                                var ComplaintStatus = val.ComplaintStatusID;

                                if (ComplaintSource == "1" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("DefaultSourceInbox", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "1" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("DefaultSourceInProgress", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "1" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("DefaultSourceResolved", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "2" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("ChiefMinisterInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "2" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("ChiefMinisterInprogress", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "2" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("ChiefMinisterResolved", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "3" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("ChieftSecretaryInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "3" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("ChieftSecretaryInProcess", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "3" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("ChieftSecretaryResolved", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "4" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("SecretaryIrrigationInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "4" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("SecretaryIrrigationInProcess", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "4" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("SecretaryIrrigationResolved", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "5" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("TehsilProgrammeInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "5" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("TehsilProgrammeInProcess", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "5" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("TehsilProgrammeResolved", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "6" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("AutoGeneratedInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "6" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("AutoGeneratedInProcess", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "6" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("AutoGeneratedResolved", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "7" && ComplaintStatus == "1") {
                                    window.localStorage.setItem("MinisterIrrigationInbox", val.Count);
                                    countSource += val.Count;
                                }

                                else if (ComplaintSource == "7" && ComplaintStatus == "2") {
                                    window.localStorage.setItem("MinisterIrrigationInProcess", val.Count);
                                    countSource += val.Count;
                                }
                                else if (ComplaintSource == "7" && ComplaintStatus == "3") {
                                    window.localStorage.setItem("MinisterIrrigationResolved", val.Count);
                                    countSource += val.Count;
                                }

                            });
                            lblSource.text(countSource);
                            google.load("visualization", "1", { packages: ["corechart"], callback: drawpieCharts });


                            function drawpieCharts() {
                                var DFInbox = parseInt(window.localStorage.getItem("DefaultSourceInbox"));
                                var DFInprogress = parseInt(window.localStorage.getItem("DefaultSourceInProgress"));
                                var DFResolved = parseInt(window.localStorage.getItem("DefaultSourceResolved"));

                                var CMInbox = parseInt(window.localStorage.getItem("ChiefMinisterInbox"));
                                var CMInprogress = parseInt(window.localStorage.getItem("ChiefMinisterInprogress"));
                                var CMResolved = parseInt(window.localStorage.getItem("ChiefMinisterResolved"));


                                var CSInbox = parseInt(window.localStorage.getItem("ChieftSecretaryInbox"));
                                var CSInprogress = parseInt(window.localStorage.getItem("ChieftSecretaryInProcess"));
                                var CSResolved = parseInt(window.localStorage.getItem("ChieftSecretaryResolved"));

                                var CIInbox = parseInt(window.localStorage.getItem("SecretaryIrrigationInbox"));
                                var CIInprogress = parseInt(window.localStorage.getItem("SecretaryIrrigationInProcess"));
                                var CIResolved = parseInt(window.localStorage.getItem("SecretaryIrrigationResolved"));


                                var TPInbox = parseInt(window.localStorage.getItem("TehsilProgrammeInbox"));
                                var TPInprogress = parseInt(window.localStorage.getItem("TehsilProgrammeInProcess"));
                                var TPResolved = parseInt(window.localStorage.getItem("TehsilProgrammeResolved"));


                                var AMInbox = parseInt(window.localStorage.getItem("AutoGeneratedInbox"));
                                var AMInprogress = parseInt(window.localStorage.getItem("AutoGeneratedInProcess"));
                                var AMResolved = parseInt(window.localStorage.getItem("AutoGeneratedResolved"));


                                var MIInbox = parseInt(window.localStorage.getItem("MinisterIrrigationInbox"));
                                var MIInprogress = parseInt(window.localStorage.getItem("MinisterIrrigationInProcess"));
                                var MIResolved = parseInt(window.localStorage.getItem("MinisterIrrigationResolved"));

                                var data = google.visualization.arrayToDataTable([
                                  ['Source', 'Inbox', 'InProgress', 'Resolved'],
                                  ['DH', DFInbox, DFInprogress, DFResolved],
                                  ['CM', CMInbox, CMInprogress, CMResolved],
                                  ['CS', CSInbox, CSInprogress, CSResolved],
                                  ['SI', CIInbox, CIInprogress, CIResolved],
                                  ['TP', TPInbox, TPInprogress, TPResolved],
                                  ['AG', AMInbox, AMInprogress, AMResolved],
                                   ['MI', MIInbox, MIInprogress, MIResolved]
                                ]);

                                var options = {
                                    legend: { position: 'bottom', alignment: 'end' },
                                    title: 'Complaints By Source',
                                    width: 700,
                                    height: 425,
                                    chart: {
                                        title: 'Complaints By Source',
                                    },

                                    // bars: 'horizontal', // Required for Material Bar Charts.
                                    colors: ['86C157', '4BCAAD', '2FA3EE'],
 
                                };
                                var chart = new google.visualization.ColumnChart(document.getElementById('barchart_Counts'));
                                chart.draw(data, options);
                                google.visualization.events.addListener(chart, 'select', selectHandler);
                                function selectHandler() {
                                    var selectedItem = chart.getSelection()[0];
                                    if (selectedItem) {
                                        var Type = data.getValue(selectedItem.row, 0);
                                        var Status = data.getColumnLabel(selectedItem.column);
                                        SearchComplaintBySource(Type, Status)
                                        
                                    }
                                   // NavigationEventHandler(selectedItem, data);
                                }
                             
                            }

                     
                          },
                        error: function (req, status, error) {
                            //error                        
                        }
                    });


                }
                else if (CompTypeID == "") {

                }
                else {
                    lblTitleSource.hide();
                    lblSource.hide();
                    lblComplaintTypes.show();

                    if (window.localStorage.getItem("CompTypeInBox") != null)
                        window.localStorage.removeItem("CompTypeInBox");

                    if (window.localStorage.getItem("CompTypeInProgress") != null)
                        window.localStorage.removeItem("CompTypeInProgress");

                    if (window.localStorage.getItem("CompTypeResolved") != null)
                        window.localStorage.removeItem("CompTypeResolved");

                    $.ajax({
                        type: "POST",
                        url: 'ComplaintsDashboard.aspx/SearchResults',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            FromDate: FromDate, ToDate: ToDate,
                            CompTypeID: CompTypeID
                        }),
                        success: function (result) {
                            var count = 0;
                            $.each(result.d, function (key, val) {

                                var Value = val.ComplaintStatusID;
                                $('#hdnComplaintName').val(val.Name);
                                //window.localStorage.setItem("ComplaintName", val.Name);
                                $('#hdnComplaintTypeId').val(val.ComplaintTypeID);

                                if (Value == "1") {
                                    window.localStorage.setItem("CompTypeInBox", val.Count);
                                    count += val.Count;
                                }
                                else if (Value == "2") {
                                    window.localStorage.setItem("CompTypeInProgress", val.Count);
                                    count += val.Count;
                                }
                                else if (Value == "3") {
                                    window.localStorage.setItem("CompTypeResolved", val.Count);
                                    count += val.Count;
                                }

                            });
                            lblComplaintTypes.text(count);
                            lblComType.show();
                            google.load("visualization", "1", { packages: ["corechart"], callback: drawpieCharts });

                            function drawpieCharts() {

                                var Compinbox = parseInt(window.localStorage.getItem("CompTypeInBox"));// $("#CompTypeInBox").val();
                                var CompInprogress = parseInt(window.localStorage.getItem("CompTypeInProgress"));  //parseInt($("#CompTypeInProgress").val());
                                var Compresolved = parseInt(window.localStorage.getItem("CompTypeResolved"));
                                var ComplaintName = $("#hdnComplaintName").val();
                                var ComplaintTypeID = $("#hdnComplaintTypeId").val();
                                var data = new google.visualization.DataTable();
                                data.addColumn('string', 'Name');
                                data.addColumn('number', 'Type');
                                data.addRows([
                                  ['Inbox', Compinbox],
                                  ['InProgress', CompInprogress],
                                  ['Resolved', Compresolved]
                                ]);

                                var options = {
                                    title: ComplaintName,
                                    width: 375,
                                    height: 425,
                                    legend: { position: 'bottom', alignment: 'end' },
                                    pieSliceText: 'value-and-percentage',
                                    colors: ['86C157', '4BCAAD', '2FA3EE']
                                };

                                var chart1_chart = new google.visualization.PieChart(document.getElementById('ComplaintTypeChart'));
                                var StInbox = 'Inbox';
                                var StInProgress = 'InProgress';

                                function selectHandler() {

                                    var selectedItem = chart1_chart.getSelection()[0];
                                    if (selectedItem) {
                                        var Status = data.getValue(selectedItem.row, 0);
                                        // console.log(Stat);
                                        if (StInbox == Status) {
                                            window.location.href = "SearchComplaints.aspx?ComplaintTypeStatus=" + ComplaintTypeID + "&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val() + "&CompStatus=1";
                                        }
                                        else if (StInProgress == Status) {
                                            window.location.href = "SearchComplaints.aspx?ComplaintTypeStatus=" + ComplaintTypeID + "&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val() + "&CompStatus=2";
                                        }
                                        else {
                                            window.location.href = "SearchComplaints.aspx?ComplaintTypeStatus=" + ComplaintTypeID + "&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val() + "&CompStatus=3";
                                        }

                                    }
                                }

                                google.visualization.events.addListener(chart1_chart, 'select', selectHandler);
                                //chart.draw(data, options);

                                chart1_chart.draw(data, options);
                                $("#hdnComplaintTypeId").val(0);
                                $("#hdnComplaintName").val("");
                            }

                        },
                        error: function (req, status, error) {
                            //error                        
                        }
                    });



                }

                //Complaint Status Graph .....

                if (window.localStorage.getItem("CompStatusInBox") != null)
                    window.localStorage.removeItem("CompStatusInBox");

                if (window.localStorage.getItem("CompStatusInProgress") != null)
                    window.localStorage.removeItem("CompStatusInProgress");

                if (window.localStorage.getItem("CompStatusResolved") != null)
                    window.localStorage.removeItem("CompStatusResolved");


                $.ajax({
                    type: "POST",
                    url: 'ComplaintsDashboard.aspx/GetComplaintStatusResults',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        FromDate: FromDate, ToDate: ToDate
                    }),
                    success: function (result) {
                        var count = 0;
                        $.each(result.d, function (key, val) {
                            var Value = val.Value;

                            // $('#hdnStatusComplaintName').val(val.Name);

                            if (Value == "1") {
                                window.localStorage.setItem("CompStatusInBox", val.Count);
                                count += val.Count;
                            }
                            else if (Value == "2") {
                                window.localStorage.setItem("CompStatusInProgress", val.Count);
                                count += val.Count;
                            }
                            else if (Value == "3") {
                                window.localStorage.setItem("CompStatusResolved", val.Count);
                                count += val.Count;
                            }

                        });
                        lblStatus.text(count);
                        // lblComType.show();
                        google.load("visualization", "1", { packages: ["corechart"], callback: drawStatusPieCharts });

                        function drawStatusPieCharts() {
                            var Compstinbox = parseInt(window.localStorage.getItem("CompStatusInBox"));// $("#CompTypeInBox").val();
                            var CompstInprogress = parseInt(window.localStorage.getItem("CompStatusInProgress"));  //parseInt($("#CompTypeInProgress").val());
                            var Compstresolved = parseInt(window.localStorage.getItem("CompStatusResolved"));


                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Name');
                            data.addColumn('number', 'Type');
                            data.addRows([
                              ['Inbox', Compstinbox],
                              ['InProgress', CompstInprogress],
                              ['Resolved', Compstresolved]
                            ]);

                            var options = {
                                title: 'Complaint By Status',
                                width: 375,
                                height: 425,
                                legend: { position: 'bottom', alignment: 'end' },
                                pieSliceText: 'value-and-percentage',
                                colors: ['86C157', '4BCAAD', '2FA3EE'],
                                is3D: false,
                                //legend: 'none' 
                            };

                            var StatusCharts = new google.visualization.PieChart(document.getElementById('charts'));
                            var StInbox = 'Inbox';
                            var StInProgress = 'InProgress';

                            function selectHandler() {

                                var selectedItem = StatusCharts.getSelection()[0];
                                if (selectedItem) {
                                    var Status = data.getValue(selectedItem.row, 0);
                                    // console.log(Stat);
                                    if (StInbox == Status) {
                                        window.location.href = "SearchComplaints.aspx?Status=1&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val();
                                    }
                                    else if (StInProgress == Status) {
                                        window.location.href = "SearchComplaints.aspx?Status=2&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val();
                                    }
                                    else {
                                        window.location.href = "SearchComplaints.aspx?Status=3&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val();
                                    }

                                }
                            }
                            google.visualization.events.addListener(StatusCharts, 'select', selectHandler);
                            //chart.draw(data, options);
                            StatusCharts.draw(data, options);

                        }

                    },
                    error: function (req, status, error) {
                        //error                        
                    }
                });
            });




        });

        function ddlHideShowComplaintsBy(ID) {
            var ComplaintTypeorSource = $(ID).val();
            // ddlComplaintType.SelectedIndex = 0;
            if (ComplaintTypeorSource == "2") {
                $('#barchars').show();
                $('#CompTypeChart').hide();
                $('#ddlComplaintType').hide();
                $('#lblComplaintType').hide();
                $('#hdnComplaintName').val(" ");
                lblSource.show();
                lblTitleSource.show();

                lblComplaintTypes.hide();
                lblComType.hide();

            }
            else if (ComplaintTypeorSource == "1") {

                $('#ddlComplaintType').show();
                $('#lblComplaintType').show();
                $('#barchars').hide();
                $('#CompTypeChart').show();
                lblSource.hide();
                lblTitleSource.hide();

                lblComplaintTypes.show();
                lblComType.show();

            }
            else {
                $('#ddlComplaintType').hide();
                $('#lblComplaintType').hide();
                $('#barchars').hide();
            }
        }

        jQuery(function () {
            jQuery('#btnComplaintsSearch').click();
        });




        function SearchComplaintBySource(Type, Status) {
            var ToDate = $("#txtDateCSTo").val();
            var FromDate = $("#txtDateCSFrom").val();
            var StatusID = 0;
            if (Status == 'Inbox')
                StatusID = 1;
            else if (Status == 'InProgress')
                StatusID = 2;
            else if (Status == 'Resolved')
                StatusID = 3;

            window.location.href = "SearchComplaints.aspx?ComplaintSource=" + Type + "&ToDate=" + $("#txtToDate").val() + "&FromDate=" + $("#txtFromDate").val() + "&CompStatus=" + StatusID;

            //var URL = "ChartDetailedReport.aspx?ReportType=3&StatusID=" + StatusID + "&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
            //window.open(URL, '_blank');
        }






    </script>









</asp:Content>

