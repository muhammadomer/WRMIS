<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true"  CodeBehind="ViewRotationalProgramCircle.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.ViewRotationalProgramCircle" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Rotational Plan</h3>
                </div>
                <div class="box-content">
                    <div id="Print">
                    <div class="form-horizontal">
                        
                        <h2 id="RPTitle" runat="server" style="text-align:center"></h2>
                         <div><h4 id="RPDates" runat="server" style="text-align:center"></h4><div style="float:right"><asp:HyperLink ID="hlAttachment" runat="server" CssClass="btn btn-link hidden-print" Visible ="true"></asp:HyperLink></div></div>

                                    <div class="table-responsive">
                <asp:GridView ID="gvDivisions" runat="server" Visible="true" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="false" CssClass="table header" 
                    BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Group">
                            <ItemTemplate>
                                <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Divisions">
                            <ItemTemplate>
                                <asp:Label ID="lblDivisions" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-9" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>

            </div>
                             <div class="table-responsive">
                            <asp:GridView ID="gvDivisionsPreferences" runat="server" AutoGenerateColumns="true" EmptyDataText="No Record Found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false" OnRowCreated="gvDivisionsPreferences_RowCreated" OnRowDataBound="gvDivisionsPreferences_OnRowDataBound"
                                  ShowHeader="false" >
                            </asp:GridView>
                        </div>
                            </div>
                        </div>
                    <%--<input type='button' class="btn btn-primary" id='btn' value='Print' onclick='printDiv();' />--%>


                    <asp:LinkButton ID='btn' runat="server" class="btn btn-primary" Text="Print" OnClick="btn_Click"></asp:LinkButton>

                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                

                    </div>
                </div>
            </div>
   <asp:HiddenField ID="hdnGroupsQuantity" runat="server" Value="0" />
     <script>


         function printDiv() {
             var printContents = document.getElementById('Print').innerHTML;
             var originalContents = document.body.innerHTML;

             document.body.innerHTML = printContents;

             window.print();

             document.body.innerHTML = originalContents;
         }

    </script>
    </asp:Content>