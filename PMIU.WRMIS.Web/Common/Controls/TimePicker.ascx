<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimePicker.ascx.cs" Inherits="PMIU.WRMIS.Web.Common.Controls.TimePicker" %>
<div>
    <asp:DropDownList Visible="false" ID="ddlHour" style="height: 25px; width:126px;" runat="server" ToolTip="Hour">
        <asp:ListItem Text="01" Value="01"></asp:ListItem>
        <asp:ListItem Text="02" Value="02"></asp:ListItem>
        <asp:ListItem Text="03" Value="03"></asp:ListItem>
        <asp:ListItem Text="04" Value="04"></asp:ListItem>
        <asp:ListItem Text="05" Value="05"></asp:ListItem>
        <asp:ListItem Text="06" Value="06"></asp:ListItem>
        <asp:ListItem Text="07" Value="07"></asp:ListItem>
        <asp:ListItem Text="08" Value="08"></asp:ListItem>
        <asp:ListItem Text="09" Value="09"></asp:ListItem>
        <asp:ListItem Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="11" Value="11"></asp:ListItem>
        <asp:ListItem Text="12" Value="12"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList Visible="false" ID="ddlMinute" style="height: 25px; width:126px;" runat="server" ToolTip="Minute">
        <asp:ListItem Text="00" Value="00"></asp:ListItem>
        <asp:ListItem Text="01" Value="01"></asp:ListItem>
        <asp:ListItem Text="02" Value="02"></asp:ListItem>
        <asp:ListItem Text="03" Value="03"></asp:ListItem>
        <asp:ListItem Text="04" Value="04"></asp:ListItem>
        <asp:ListItem Text="05" Value="05"></asp:ListItem>
        <asp:ListItem Text="06" Value="06"></asp:ListItem>
        <asp:ListItem Text="07" Value="07"></asp:ListItem>
        <asp:ListItem Text="08" Value="08"></asp:ListItem>
        <asp:ListItem Text="09" Value="09"></asp:ListItem>
        <asp:ListItem Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="11" Value="11"></asp:ListItem>
        <asp:ListItem Text="12" Value="12"></asp:ListItem>
        <asp:ListItem Text="13" Value="13"></asp:ListItem>
        <asp:ListItem Text="14" Value="14"></asp:ListItem>
        <asp:ListItem Text="15" Value="15"></asp:ListItem>
        <asp:ListItem Text="16" Value="16"></asp:ListItem>
        <asp:ListItem Text="17" Value="17"></asp:ListItem>
        <asp:ListItem Text="18" Value="18"></asp:ListItem>
        <asp:ListItem Text="19" Value="19"></asp:ListItem>
        <asp:ListItem Text="20" Value="20"></asp:ListItem>
        <asp:ListItem Text="21" Value="21"></asp:ListItem>
        <asp:ListItem Text="22" Value="22"></asp:ListItem>
        <asp:ListItem Text="23" Value="23"></asp:ListItem>
        <asp:ListItem Text="24" Value="24"></asp:ListItem>
        <asp:ListItem Text="25" Value="25"></asp:ListItem>
        <asp:ListItem Text="26" Value="26"></asp:ListItem>
        <asp:ListItem Text="27" Value="27"></asp:ListItem>
        <asp:ListItem Text="28" Value="28"></asp:ListItem>
        <asp:ListItem Text="29" Value="29"></asp:ListItem>
        <asp:ListItem Text="30" Value="30"></asp:ListItem>
        <asp:ListItem Text="31" Value="31"></asp:ListItem>
        <asp:ListItem Text="32" Value="32"></asp:ListItem>
        <asp:ListItem Text="33" Value="33"></asp:ListItem>
        <asp:ListItem Text="34" Value="34"></asp:ListItem>
        <asp:ListItem Text="35" Value="35"></asp:ListItem>
        <asp:ListItem Text="36" Value="36"></asp:ListItem>
        <asp:ListItem Text="37" Value="37"></asp:ListItem>
        <asp:ListItem Text="38" Value="38"></asp:ListItem>
        <asp:ListItem Text="39" Value="39"></asp:ListItem>
        <asp:ListItem Text="40" Value="40"></asp:ListItem>
        <asp:ListItem Text="41" Value="41"></asp:ListItem>
        <asp:ListItem Text="42" Value="42"></asp:ListItem>
        <asp:ListItem Text="43" Value="43"></asp:ListItem>
        <asp:ListItem Text="44" Value="44"></asp:ListItem>
        <asp:ListItem Text="45" Value="45"></asp:ListItem>
        <asp:ListItem Text="46" Value="46"></asp:ListItem>
        <asp:ListItem Text="47" Value="47"></asp:ListItem>
        <asp:ListItem Text="48" Value="48"></asp:ListItem>
        <asp:ListItem Text="49" Value="49"></asp:ListItem>
        <asp:ListItem Text="50" Value="50"></asp:ListItem>
        <asp:ListItem Text="51" Value="51"></asp:ListItem>
        <asp:ListItem Text="52" Value="52"></asp:ListItem>
        <asp:ListItem Text="53" Value="53"></asp:ListItem>
        <asp:ListItem Text="54" Value="54"></asp:ListItem>
        <asp:ListItem Text="55" Value="55"></asp:ListItem>
        <asp:ListItem Text="56" Value="56"></asp:ListItem>
        <asp:ListItem Text="57" Value="57"></asp:ListItem>
        <asp:ListItem Text="58" Value="58"></asp:ListItem>
        <asp:ListItem Text="59" Value="59"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList Visible="false" ID="ddlAMPM" style="height: 25px; width:126px;" runat="server" ToolTip="AM/PM">
        <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
    </asp:DropDownList>



    <div class="input-group date" data-date-viewmode="years" runat="server" id="tpDiv">
        <span id="dvClockIcon" runat="server" class="input-group-addon"><i class="fa fa-clock-o"></i></span>
        <asp:TextBox ID="txtTimePicker" TabIndex="6" runat="server" required="true" class="required form-control timepicker-default" type="text"></asp:TextBox>
        <span class="input-group-addon clear" id="dvCrossIcon" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
    </div>



</div>
