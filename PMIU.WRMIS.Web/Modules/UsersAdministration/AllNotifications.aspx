<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllNotifications.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AllNotifications" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>All  Notifications</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-4 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-4 control-label">From Date</label>
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-4 control-label">To Date</label>
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                        <div class="col-sm-8 col-lg-8 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlStatus" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="fnc-btn">
                                    <%--      <asp:Button runat="server" ID="btnBreachSearch" value=" Search " CssClass="btn btn-primary" Text="&nbsp;Search" OnClick="btnBreachSearch_Click" />--%>
                                    <asp:LinkButton TabIndex="10" ID="btnSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                                <div class="col-md-6">
                                <div class="fnc-btn">
                                    <%--      <asp:Button runat="server" ID="btnBreachSearch" value=" Search " CssClass="btn btn-primary" Text="&nbsp;Search" OnClick="btnBreachSearch_Click" />--%>
                                    <asp:LinkButton TabIndex="10" ID="btnUnread" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnUnreadSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Mark As Unread</asp:LinkButton>
                                     <asp:LinkButton TabIndex="10" ID="btnread" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnreadSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Mark As Read</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchResult" runat="server" DataKeyNames="ID" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    OnRowDataBound="gvSearchResult_RowDataBound" OnPageIndexChanging="gvSearchResult_PageIndexChanging" AllowPaging="true">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID." Visible="false" >
                                            <ItemTemplate >
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Date" HeaderStyle-Width="170px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Notification">
                                            <ItemTemplate>
                                             <%--   <asp:hyperlink ID="lblAlertText" runat="server" CssClass="control-label" Text='<%# Eval("AlertText") %>'  NavigateUrl ='<%# Eval("AlertURL") %>'></asp:hyperlink>--%>
                                                 <asp:hyperlink ID="lblAlertText" runat="server" CssClass="control-label" data-id='<%# Eval("ID") %>' Text='<%# Eval("AlertText") %>'  NavigateUrl ='<%# Eval("AlertURL") %>' 
                                                      ></asp:hyperlink> 

                                               <%--onclick="GetValue(this);"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Status." Visible="false" >
                                            <ItemTemplate >
                                                <asp:Label ID="Status" runat="server" CssClass="control-label" Text='<%# Eval("Status") %>'></asp:Label>
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
            </div>
        </div>
    </div>
    <!-- END Main Content -->
       <script type="text/javascript">
           // for check all checkbox  
           function CheckAll(Checkbox) {
               var GridVwHeaderCheckbox = document.getElementById("<%=gvSearchResult.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
           }


           //function GetValue() {
           //    var state_name = $(this).attr('data-id')
           //    console.log(state_name);
           //    debugger;
               
           //    var val = value;
           //    alert(val);
           //}

           $("a[data-id]").click(function () {
               debugger;
               var rowid  = $(this).attr("data-id");

                    $.ajax({
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        type: 'GET',
                        data: { "rowid": rowid },
                        url: '<%= ResolveUrl("AllNotifications.aspx/ConvertToAsRead") %>',
                            success: function (data) {
                                // alert(data);
                            },
                            error: function (xhr, err) {
                                $('#ajaxResponse').html(xhr.responseText);
                            }
                        });
            

           });

           //$(function () {
           //    $("[id*=gvSearchResult] td").click(function () {
           //        DisplayDetails($(this).closest("tr"));
                  
           //    });
           //});

           //function DisplayDetails(row) {
           //    debugger;

           //    var message = "";
           //    message = "\nDescription: " + $("td", row).eq(2).html();
           //    alert(message);
           //    var myString = message.substr(message.indexOf("data-id") + 9)
           //    alert(myString);
           //}
      
    </script>
</asp:Content>
