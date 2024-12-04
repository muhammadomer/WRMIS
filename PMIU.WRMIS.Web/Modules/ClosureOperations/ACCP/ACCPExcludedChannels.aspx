<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ACCPExcludedChannels.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP.ACCPExcludedChannels" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucACCP" TagName="AccpTitleYear" Src="~/Modules/ClosureOperations/UserControls/ACCPXChannels.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    
    <div class="col-md-12">
            <div class="box">
    <div class="box-title">
        <h3>Exclude Channels from Annual Canal Closure Programme</h3>
        <div class="box-tool">
            <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            <asp:HiddenField runat="server" ID="hfACCPID" />
            <asp:HiddenField runat="server" ID="hfMainCanalID" />
            <asp:HiddenField runat="server" ID="hfDetailID"  Value="0"/>
        </div>
    </div>
    <div class="box-content">
        <ucACCP:AccpTitleYear ID="ACCPIDMainCanalID" runat="server" />
        <div class="form-horizontal">
            <div id="dual-list-box" class="form-group row" style="width: 926px; height: 500px; margin-left: 26px;">
                <div class="col-md-4" style="float:left;">
                    <div class="col-lg-12 form-group">
                        <asp:Label ID="lblBranchCanalReadyToExclude" Text="Branch Canals" runat="server" CssClass="control-label"></asp:Label>
                        <asp:ListBox runat="server" data-title="users" ID="lstBoxChannels" SelectionMode="Multiple" class="unselected form-control" Style="height: 311px; margin-top:5px; width: 130%;" />
                    </div>
                </div>
                <div  style="float:left; margin-top:188px;margin-left:40px;" >
                    <div class="col-md-8 col-lg-9 controls" style="margin-bottom:10px;">
                        <Button id="btnAdd" runat="server" style="margin-top:10px;width:90px;"  data-type="str" class="str btn btn-default col-md-8 col-md-offset-2" type="button"><span class="glyphicon glyphicon-chevron-right"></span></Button>
                    </div>
                    <div class="col-md-8 col-lg-9 controls">
                        <button id="btnRemove" runat="server" style="margin-top:10px;width:90px;" onclick="RemoveValidation();"  data-type="stl" class="stl btn btn-default col-md-8 col-md-offset-2" type="button"><span class="glyphicon glyphicon-chevron-left"></span></button>
                    </div>
                </div>
                
                <div  class="col-md-4">
                    <div class="col-lg-12" style="margin-left: -69px;">
                        <asp:Label ID="lblExcludedBranchCanal" Text="Excluded Branch Canals" runat="server" CssClass="control-label"></asp:Label>
                        <asp:ListBox runat="server" required="required" ID="lstBoxExcludeChannels" ClientIDMode="Static" SelectionMode="Multiple" class="selected form-control" Style="height: 311px;margin-top:5px; width: 130%;" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClientClick="SelectAll(); return;" />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default"  Text="Back"></asp:HyperLink>
                    </div>
                </div>
     </div>
    </div>
    
</div>
        </div>

      <script src="../../../Scripts/dual-list-box.js"></script>
    <script type="text/javascript">
        $("select[id$='lstBoxChannels']").DualListBox();
        function RemoveValidation() {
            debugger;
           <%-- $("#" + '<%= btnSave.ClientID %>').hide();--%>
            $("#" + '<%= lstBoxExcludeChannels.ClientID %>').removeAttr("required");
        }
        function SelectAll() {
            // Set all option selected to avoid validation failure
            $('#lstBoxExcludeChannels option').prop('selected', true);
        }
        $("form").submit(function (event) {
            debugger;
            /* stop form from submitting normally */
            event.preventDefault();
            $('#lblMsgs').html('');
            $('#lblMsgs').removeClass('ErrorMsg').removeClass('SuccessMsg');

            var channelsIDs = [];
            
            $('#lstBoxExcludeChannels > option').each(function () {
                var $this = $(this);
                channelsIDs.push($this.val());
            });
            var DetailID = $("#" + '<%= hfDetailID.ClientID %>').val();
           // var DetailID = $('#hfDetailID').val();
            var dataToPost = {
                _XChannelsIDs: channelsIDs,
                _DetailID: DetailID
            };

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ACCPExcludedChannels.aspx/SaveExcludeChannels") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(dataToPost),
                success: function (data) {
                    if (data.d == true) {
                        $('#lstBoxExcludeChannels option').prop('selected', false);

                        $('#lblMsgs').html('Record saved successfully.');
                        $('#lblMsgs').addClass('SuccessMsg').show();
                        setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                    }
                    else {
                        $('#lblMsgs').addClass('ErrorMsg').show();
                        $('#lblMsgs').html('An internal server error occurred');
                        setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                    }
                }
            });
            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            return false;
        });
        </script>
</asp:Content>
