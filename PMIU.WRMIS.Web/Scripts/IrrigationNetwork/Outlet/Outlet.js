

function isNumber(me) {

    var val = me.value;
    if (val == ".") {
        return;
    }
    if (isNaN(val)) {
        me.value = "";
    }
}


function Validate() {
    var ddl = $("select[id$='ddlOutletSide']").val();// document.getElementById('<%=ddlOutletSide.ClientID%>');
    
    if (ddl != null) {
        if(ddl == "-1")
        {       
            return false;
        }
        else
        {
            return true;
        }
    }

    
}