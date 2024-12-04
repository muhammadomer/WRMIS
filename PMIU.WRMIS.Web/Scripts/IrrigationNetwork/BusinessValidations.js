function CompareRDValues(txtInput) {
    if (parseInt(txtInput.value) > 999) {
        txtInput.setCustomValidity('RD at right side cannot contain the value bigger than 999.');
    } else {
        // input is valid -- reset the error message
        txtInput.setCustomValidity('');
    }
}

function ValidateIMISCodeLength(txtInput) {
    if (parseInt(txtInput.value.length) != 17) {
        txtInput.setCustomValidity('IMIS Code length should be 17.');
    } else {
        // input is valid -- reset the error message
        txtInput.setCustomValidity('');
    }
}

function CompareCCAAreas(txtInput) {
    var GCAAcre = $("[id$=txtGrossCommandArea]");

    if (GCAAcre.val() != '' && txtInput.value != '') {
        if (parseInt(txtInput.value) > parseInt(GCAAcre.val())) {
            txtInput.setCustomValidity('Cultureable Command Area is always smaller than or equals to Gross Command Area.');
        }
        else {
            // input is valid -- reset the error message
            txtInput.setCustomValidity('');
        }
    }
}

function IndentValueValidation(txtInput) {
    if (parseFloat(txtInput.value) > 299999.99) {
        txtInput.setCustomValidity('Indent value bigger than 299999.99');
    } else {
        // input is valid -- reset the error message
        txtInput.setCustomValidity('');
    }
}

function ValidateIntergerInputRange(txtInput, minValue, maxValue) {
    minValue = minValue.trim();
    maxValue = maxValue.trim();
    var inputValue = txtInput.value.trim();
    //console.log(inputValue);
    //console.log(minValue);
    //console.log(maxValue);
    if (inputValue.length != 0 && !(inputValue === "")) {
        // console.log('validate input');
        if (!(minValue === "") && !(maxValue === "")) {
            //  console.log('validate min max value');
            inputValue = parseInt(inputValue);

            if (inputValue < parseInt(minValue) || inputValue > parseInt(maxValue)) {
                //console.log('input is invalid');
                txtInput.setCustomValidity("Value should be between " + minValue + " and " + maxValue);
                console.log('vali');
            }
            else {
                txtInput.setCustomValidity("");
            }
        }
        else {
            txtInput.setCustomValidity("");
        }
    }
    else {
        txtInput.setCustomValidity("");
    }
}

function ValidateGCA(txtInput) {
    var CCAAcre = $("[id$=txtCultureableCommandArea");

    if (CCAAcre.val().trim() != '' && txtInput.value.trim() != '') {
        if (parseInt(txtInput.value) < parseInt(CCAAcre.val())) {
            txtInput.setCustomValidity('Gross Command Area is always greater than or equals to Cultureable Command Area.');
        }
        else {
            // input is valid -- reset the error message
            txtInput.setCustomValidity('');
        }
    }
    else {
        txtInput.setCustomValidity('');
    }

    CCAAcre[0].setCustomValidity('');
}

function ValidateCCA(txtInput) {
    var GCAAcre = $("[id$=txtGrossCommandArea");

    if (GCAAcre.val().trim() != '' && txtInput.value.trim() != '') {
        if (parseInt(txtInput.value) > parseInt(GCAAcre.val())) {
            txtInput.setCustomValidity('Cultureable Command Area is always smaller than or equals to Gross Command Area.');
        }
        else {
            // input is valid -- reset the error message
            txtInput.setCustomValidity('');
        }
    }
    else {
        txtInput.setCustomValidity('');
    }

    GCAAcre[0].setCustomValidity('');
}
