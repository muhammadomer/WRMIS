<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" EnableEventValidation="false" CodeBehind="PrintBillSetup.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.ReferenceData.PrintBillSetup" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .decimalIntegerInput {
            text-align: left;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Print Bill Setup</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Help Line No.</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtNo" ClientIDMode="Static" class="IntegerInput form-control" runat="server" MaxLength="18" placeholder="XXXXXXXXXX"></asp:TextBox>
                                        <%--MinLength="15" MaxLength="15" --%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </br>
                        <asp:Panel ID="pnlEff" runat="server" GroupingText="Effluent Waters">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Text 1</label>
                                        <div class="col-sm-7 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control multiline-no-resize required" required="required"
                                                minlength="3" MaxLength="170" TextMode="MultiLine" Rows="5" Columns="50"
                                                ID="txt1" runat="server" placeholder="" onkeyDown="checkTextAreaMaxLength(this,event,'170');"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Text 2</label>
                                        <div class="col-sm-7 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control multiline-no-resize" 
                                                minlength="3" MaxLength="160" TextMode="MultiLine" Rows="5" Columns="50"
                                                ID="txt2" runat="server" placeholder="" onkeyDown="checkTextAreaMaxLength(this,event,'160');"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlCanal" runat="server" GroupingText="Canal Special Waters">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Text 1</label>
                                        <div class="col-sm-7 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control multiline-no-resize required" required="required"
                                                minlength="3" MaxLength="170" TextMode="MultiLine" Rows="5" Columns="50"
                                                ID="txtText1Canal" runat="server" placeholder="" onkeyDown="checkTextAreaMaxLength(this,event,'170');"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Text 2</label>
                                        <div class="col-sm-7 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control multiline-no-resize"
                                                minlength="3" MaxLength="160" TextMode="MultiLine" Rows="5" Columns="50"
                                                ID="txtText2Canal" runat="server" placeholder="" onkeyDown="checkTextAreaMaxLength(this,event,'160');"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class=" col-md-6 fnc-btn">
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btn_Click" OnClientClick="return checkPhoneMaxMinLength();" />
                                <asp:Button runat="server" ID="btnCncl" CssClass="btn btn-default" Text="&nbsp;Cancel" OnClick="btn_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function checkTextAreaMaxLength(textBox, e, length) {
            debugger;
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 9) {

                var maxLength = parseInt(mLen);
                {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }
        }


        function checkPhoneMaxMinLength() {
            var MinLen = 4;
            var MaxLen = 18;
            var textBox = document.getElementById("txtNo");

            if (textBox.value.length < MinLen) {
                alert("Phone number should be of Minimum 4 characters. ");
                event.preventDefault();
                return false;
            }
            else if (textBox.value.length > MaxLen) {
                alert("Phone number should be of Maximum 18 characters. ");
                event.preventDefault();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
