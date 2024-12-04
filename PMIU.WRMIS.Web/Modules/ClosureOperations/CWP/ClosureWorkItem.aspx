<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClosureWorkItem.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.ClosureWorkItem" EnableEventValidation="false" %> 
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ClosureOperations/UserControls/CWInfo.ascx" TagPrefix="ucCWInfo" TagName="ucCWInfoControl" %> 
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Closure Work Item</h3>

                </div>
                <div class="box-content">
                    <ucCWInfo:ucCWInfoControl runat="server" ID="ucInfo" />  
                    <div class="table-responsive">
                        <asp:HiddenField ID="hdcF_ViewMode" Value="false" runat="server" />
                        <asp:HiddenField ID="hdnF_CWID" Value="" runat="server" />
                        <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                            OnRowCommand="gvUnits_RowCommand" OnPageIndexChanging="gvUnits_PageIndexChanging"
                            OnRowUpdating="gvUnits_RowUpdating" OnRowDeleting="gvUnits_RowDeleting"
                            OnRowCancelingEdit="gvUnits_RowCancelingEdit" ShowFooter="true"
                            OnRowEditing="gvUnits_RowEditing" OnPageIndexChanged="gvUnits_PageIndexChanged"
                            EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" 
                            CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvUnits_RowDataBound">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Item Description" Visible="true">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate> 
                                        <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("ItemDescription") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" required="required" MaxLength="250" onfocus="this.value = this.value;" CssClass="form-control required" placeholder="Enter item description" value='<%# Eval("ItemDescription") %>' ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit" Visible="true">
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" CssClass="control-label" Text='<%# Eval("Unit") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate> 
                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control"/>
                                        <asp:HiddenField ID="hdnFUnit" runat="server" Value='<%# Eval("UnitID") %>'></asp:HiddenField>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanctioned Quantity" Visible="true">
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSancQty" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedQty") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate> 
                                        <asp:TextBox ID="txtSnctQty" runat="server" placeholder="" required="required" MaxLength="10"  CssClass="integerInput form-control required" TextMode="Number" onkeyDown="checkTextAreaMaxLength(this,event,'10');" value='<%# Eval("SanctionedQty") %>'  />
                                    </EditItemTemplate>
                                </asp:TemplateField> 

                                <asp:TemplateField HeaderText="Technical Sanction Rate (Rs.)">
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTSRate" runat="server" Text='<%# Eval("TSRate") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTSRate" runat="server" placeholder="" required="required" MaxLength="10" CssClass="integerInput form-control required" TextMode="Number"  onkeyDown="checkTextAreaMaxLength(this,event,'10');"  value='<%# Eval("TSRate") %>'  />
                                    </EditItemTemplate>

                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanctioned Amount (Rs.)">
                                    <HeaderStyle CssClass="text-center" />                                    
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAmnt" runat="server" Text='<%# Utility.GetRoundOffValue( Convert.ToString( Eval("TSAmount") )) %>' MaxLength="15"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b> <asp:Label ID="lblTotal" runat="server" Text="" Visible="true" /> </b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle CssClass="text-right" />
                                    <FooterStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Awarded (Rs.)">
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label CssClass="text-right" ID="lblAmnt" runat="server"  ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                    <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn"> 
                                    <asp:HyperLink ID="hlBack" runat="server"  CssClass="btn btn-default" >&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script> 
        function checkTextAreaMaxLength(textBox, e, length) { 
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length; 
            var maxLength = parseInt(mLen);
                {
                    if (textBox.value.length > maxLength - 1) {
                        var charCode = (e.which) ? e.which : e.keyCode;
                        if ( ! (charCode <= 46)  ) {
                            if (window.event)//IE
                                e.returnValue = false;
                            else//Firefox
                                e.preventDefault();
                        }
                        //if (    isNaN(String.fromCharCode(event.which))) {
                        //    event.preventDefault(); //stop character from entering input
                        //}


                    //if (window.event)//IE
                    //    e.returnValue = false;
                    //else//Firefox
                    //    e.preventDefault();
                }
            }
        }
    </script>

</asp:Content>
