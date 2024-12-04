﻿<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"  CodeBehind="ReferenceAbianaData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ReferenceData.ReferenceAbianaData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:HiddenField ID="hdnOffenceID" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 >Reference Fine</h3>
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <div id="DivAbiana" runat="server">
                                    <asp:GridView ID="gvAbiana" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                                        EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True" AllowSorting="false"
                                        OnRowCancelingEdit="gvAbiana_RowCancelingEdit" OnRowEditing="gvAbiana_RowEditing"
                                        OnRowUpdating="gvAbiana_RowUpdating"
                                        CssClass="table header"
                                        BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <HeaderStyle CssClass="col-md-3" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAbianaID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <%-- Offence Type --%>
                                            <asp:TemplateField HeaderText="Area Type" Visible="true">
                                                <HeaderStyle CssClass="col-md-3" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAbianaName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtAreaType" runat="server" CssClass="form-control required" onfocus="this.value = this.value;" required="true" MaxLength="30" placeholder="Enter Area Type" Text='<%# Eval("Name") %>' ClientIDMode="Static" ReadOnly />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Fine Against 1 Unit (Rupees)" Visible="true" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle CssClass="col-md-3" />
                                                <ItemTemplate >
                                                    <asp:Label ID="lblAbianaRate" runat="server"  CssClass="control-label"  Text='<%# Eval("AbianaRate") %>'></asp:Label>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtAbianaRate" runat="server" CssClass="form-control required integerInput" onfocus="this.value = this.value;" required="true" MaxLength="5"  placeholder="Enter Abiana Rate" Text='<%# Eval("AbianaRate") %>' ClientIDMode="Static" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Percentage (Maximum % can be 20 )" Visible="true" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle CssClass="col-md-3" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAbianaPercentage" runat="server" CssClass="control-label integerInput" Text='<%# Eval("MaxPercentage") %>'></asp:Label>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMaxPercentage" runat="server" CssClass="form-control required integerInput" onfocus="this.value = this.value;" required="true" MaxLength="2" placeholder="Enter Max Percentage" autocomplete="off" onkeyup="return ValidateValue(this,20);"  Text='<%# Eval("MaxPercentage") %>' ClientIDMode="Static" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%-- Add, Edit, Delete Buttons --%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Panel ID="pnlAddType" runat="server" HorizontalAlign="Center">
                                                          <asp:Label ID="lblEdit" runat="server" CssClass="control-label" Text='Edit'></asp:Label>
                                                    </asp:Panel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnEdited" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" ></asp:Button>
                                                        <%--<asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>--%>
                                                    </asp:Panel>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnSaves" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                    </asp:Panel>
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                    </div>
                    <br />
       <%--             <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END Main Content -->
    <script>
        function ValidateValue(textboxID,max) {
            if ((textboxID.value >= 0 && textboxID.value <= max) && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {
                alert('Value range is 0 to' + ' ' + max);
                textboxID.value = '';
                return false;
            }
        }
    </script>
</asp:Content>
