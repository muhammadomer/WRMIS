<%@ Page Title="AddWorksItems" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddWorkItems.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.AddWorkItems" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/AddWorks.ascx" TagPrefix="ucAddWorksControl" TagName="AddWorksUserControl" %>
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
    </style>
    <div class="box">
        <div class="box-title">
            <h3>Add Works</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <ucAddWorksControl:AddWorksUserControl runat="server" ID="AddWorksUserControl" />

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label" style="padding-top: 7px;">Select Work</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:RadioButtonList ID="RadioButtonWorkType" runat="server" CssClass="My-Radio" RepeatDirection="Horizontal" OnSelectedIndexChanged="Enable_WorkType" AutoPostBack="true">
                                    <asp:ListItem class="radio-inline" Text="Closure Works" Value="1" Selected="True" />
                                    <asp:ListItem class="radio-inline" Text="Works" Value="2" style="margin-left: 15px;" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>


                <div id="ClosureWorkDiv" runat="server" visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblddlDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" CssClass="form-control" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblddlYear" class="col-sm-4 col-lg-3 control-label">Year</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server" TabIndex="3">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblddlClosureWorkType" class="col-sm-4 col-lg-3 control-label">Closure Work Type</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlClosureWorkType" CssClass="form-control" runat="server" TabIndex="4">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>

                <div id="AssetDiv" runat="server" visible="false">
                  

                        <div class="row">

                                    <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblAssetDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlAssetDivision" CssClass="form-control" runat="server" TabIndex="2"  >
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
              

                       
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblFiscalYear" class="col-sm-4 col-lg-3 control-label">Year</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlFiscalYear" CssClass="form-control" runat="server" TabIndex="3">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                                      <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblAssetType" class="col-sm-4 col-lg-3 control-label">Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlAssetType" CssClass="form-control" runat="server" TabIndex="4">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                    </div>
                     </div>
              

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-success">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvClosureWorks" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" Visible="false"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="False"
                                OnRowDataBound="gvClosureWorks_RowDataBound">


                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureWorkID" runat="server" CssClass="control-label" Text='<%# Eval("ClosureWorkID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Work type">
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureWorkType" runat="server" CssClass="control-label" Text='<%# Eval("ClosureWorkType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Work Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosureWorkName" runat="server" CssClass="control-label" Text='<%# Eval("ClosureWorkName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estimated Cost (Rs.)">
                                        <%--<HeaderStyle CssClass="col-md-2" />--%>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstimatedCost" runat="server" CssClass="control-label" Text='<%# Eval("EstimatedCost","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                        <ItemStyle CssClass="text-right padding-right-number" />
                                    </asp:TemplateField>

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>


                </div>




            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDivisionID" runat="server" />
    <script type="text/javascript">
        ////On UpdatePanel Refresh
               //var prm = Sys.WebForms.PageRequestManager.getInstance();
               //if (prm != null) {
               //    prm.add_endRequest(function (sender, e) {
               //        if (sender._postBackSettings.panelsToUpdate != null) {
               //            InitilizeDatePickerStateOnUpdatePanelRefresh();
               //        }
               //    });
               //};

               // for check all checkbox  
               function CheckAll(Checkbox) {
                   var GridVwHeaderCheckbox = document.getElementById("<%=gvClosureWorks.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        </script>
</asp:Content>
