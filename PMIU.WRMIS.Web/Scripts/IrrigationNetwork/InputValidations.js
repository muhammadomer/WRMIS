function AllowedSpecialKeys(event) {
    var e = event || window.event;
    var key = e.keyCode || e.which;

    if (key == 46
        // Backspace and Tab 
        || key == 8 || key == 9
        // escape and enter
        // || key == 27 || key == 13
        // Allow: Ctrl+A
        || (key == 65 && e.ctrlKey === true)
        // Allow: Ctrl+C
        || (key == 67 && e.ctrlKey === true)
        // Allow: Ctrl+V
        || (key == 86 && e.ctrlKey === true)
        // Allow: Ctrl+X
        || (key == 88 && e.ctrlKey === true)
        // Allow: home, end, left, right
        || (key >= 35 && key <= 39)
        // Del and Insert
        || key == 46 || key == 45) {
        // let it happen, don't do anything
        // input is VALID
        return true;
    }
    //} else {
    //    // Ensure that it is a number and stop the keypress
    //    // input is INVALID
    //    if (event.shiftKey || (key < 48 || key > 57) && (key < 96 || key > 105)) {
    //        event.preventDefault();
    //    }
    //}
}

function ValidateDecimalInput(val, event) {
    var e = event || window.event;
    var key = e.keyCode || e.which;

    if (!(key == 46
        // 0-9 on key pad
        || (key >= 48 && key <= 57)
        // 0-9 on Numeric keypad
        || (key >= 96 && key <= 105)
        && AllowedSpecialKeys(event)))
        return false;

    var parts = val.value.split('.');
    if (parts.length > 2) return false;
    if (key == 46) return (parts.length == 1);
    if (parts[0].length >= 14) return false;
    if (parts.length == 2 && parts[1].length >= 2) return false;
}

function ValidateIntegerInput(event) {
    var e = event || window.event;
    var key = e.keyCode || e.which;

    if (!(// 0-9 on key pad
        (key >= 48 && key <= 57)
        // 0-9 on Numeric keypad
    || (key >= 96 && key <= 105)
        && AllowedSpecialKeys(event)))
        return false;
}

function ValidateAlphanumericInput(event) {
    var e = event || window.event;
    var key = e.keyCode || e.which;

    if (!(// 0-9 on key pad
        (key >= 48 && key <= 57)
        // 0-9 on Numeric keypad
    || (key >= 96 && key <= 105)))
        return false;
}

function AllowCharectersOnly(event) {

    var e = event || window.event;
    var key = e.keyCode || e.which;

    if (key == 46
        // Backspace and Tab 
        || key == 8 || key == 9
        // escape and enter
        // || key == 27 || key == 13
        // Allow: Ctrl+A
        || (key == 65 && e.ctrlKey === true)
        // Allow: Ctrl+C
        || (key == 67 && e.ctrlKey === true)
        // Allow: Ctrl+V
        || (key == 86 && e.ctrlKey === true)
        // Allow: Ctrl+X
        || (key == 88 && e.ctrlKey === true)
        // Allow: home, end, left, right
        || (key >= 35 && key <= 39)
        || (key >= 48 && key <= 96)
        || (key == 32)
        // Del and Insert
        || key == 46 || key == 45) {
        // let it happen, don't do anything
        // input is VALID
        return;
    } else {
        // Ensure that it is a number and stop the keypress
        // input is INVALID
        if (event.shiftKey || (key < 48 || key > 57) && (key < 96 || key > 105)) {
            event.preventDefault();
        }
    }
}

var regexInputValidation = /^[a-zA-Z0-9-, ]*$/;

function InputValidation(input) {

    var str = input.value;
    //var regex = /^[a-zA-Z0-9-/.() ]*$/;
    var regex = regexInputValidation;
    if (str.length != 0) {

        str = str.trim();

        if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter correct input.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
    //var str = input.value.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');

    //if (str.indexOf(' ') <= 2) {
    //    str = str.trim();
    //}

    //$("input[name$='" + input.id + "']").val(str);
}

function InputWithLengthValidation(input, minLength) {

    var str = input.value;

    var regex = regexInputValidation;
    if (str.length != 0) {

        str = str.trim();

        if (str.length < parseInt(minLength)) {
            input.setCustomValidity("Text should have at least " + minLength + " characters.");
        }
        else if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter correct input.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

var regexInputTextValidation = /^[a-zA-Z0-9-,\{\}\_/.\(\)\[\]\@$!: ]*$/;

function InputValidationText(input) {

    var str = input.value;
    var regex = regexInputTextValidation;
    if (str.length != 0) {

        str = str.trim();

        if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter correct input.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function InputTextWithLengthValidation(input, minLength) {

    var str = input.value;
    var regex = /^[a-zA-Z0-9-,\{\}\_/.\(\)\[\]\@$!: ]*$/;
    if (str.length != 0) {

        str = str.trim();

        if (str.length < parseInt(minLength)) {
            input.setCustomValidity("Text should have at least " + minLength + " characters.");
        }
        else if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter correct input.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function NumberValidation(input) {

    var str = input.value;
    var regex = /^[0-9]+([,.][0-9]+)?$/g;

    if (str.length != 0) {

        str = str.trim();

        if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter correct numeric input.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }

    //var ValidatedString = input.value.replace(/[^0-9\.]/g, '');

    //$("input[name$='" + input.id + "']").val(ValidatedString);
}

function TrimInput(input) {

    var TrimmedString = input.value.trim();

    $("input[name$='" + input.id + "']").val(TrimmedString);
}

var regexAlphabetValidation = /^[a-zA-Z, ]*$/;

function AlphabetValidation(input) {

    var str = input.value;
    var regex = regexAlphabetValidation;

    if (str.length != 0) {

        str = str.trim();

        if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter alphabets only.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function AlphabetsWithLengthValidation(input, minLength) {

    var str = input.value;

    var regex = regexAlphabetValidation;
    if (str.length != 0) {

        str = str.trim();

        if (str.length < parseInt(minLength)) {
            input.setCustomValidity("Text should have at least " + minLength + " characters.");
        }
        else if (str === "" || regex.test(str) == false) {
            input.setCustomValidity("Please enter alphabets only.");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function NumberValueValidation(input, maxValue) {

    var str = input.value;

    if (str.length != 0) {

        str = str.trim();

        if (!(str === "")) {

            var value = parseFloat(str);

            if (value > parseFloat(maxValue)) {
                input.setCustomValidity("Number cannot be greater than " + maxValue);

            }
            else {
                input.setCustomValidity("");
            }
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function ValueValidation(input, minValue, maxValue) {
    var str = input.value;
    //console.log(maxValue);
    str = str.trim();

    if (str.length != 0 && !(str === "")) {

        if (!(minValue.trim() === "") && !(maxValue.trim() === "")) {

            var value = parseFloat(str);

            if (value < parseFloat(minValue) || value > parseFloat(maxValue)) {
                input.setCustomValidity("Value should be between " + minValue + " and " + maxValue);
            }
            else {
                input.setCustomValidity("");
            }
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function PhoneNoWithLengthValidation(input, minLength) {

    var str = input.value;

    if (str.length != 0) {

        str = str.trim();

        if (str.length < parseInt(minLength)) {
            input.setCustomValidity("Mobile No. cannot be less than " + minLength + " digits");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function ValueValidationMultiples(input, minValue, maxValue, multipleValue) {

    var str = input.value;

    str = str.trim();

    if (str.length != 0 && !(str === "")) {

        if (!(minValue.trim() === "") && !(maxValue.trim() === "") && !(multipleValue.trim() === "")) {

            var value = parseFloat(str);

            if (value < parseFloat(minValue) || value > parseFloat(maxValue)) {
                input.setCustomValidity("Value should be between " + minValue + " and " + maxValue);
            }
            else if ((value % parseFloat(multipleValue)) != 0) {
                input.setCustomValidity("Value should be a multiple of " + multipleValue);
            }
            else {
                input.setCustomValidity("");
            }
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function LandlineNoWithLengthValidation(input, minLength) {
    var str = input.value;

    if (str.length != 0) {

        str = str.trim();

        if (str.length < parseInt(minLength)) {
            input.setCustomValidity("Landline No. cannot be less than " + minLength + " digits");
        }
        else {
            input.setCustomValidity("");
        }
    }
    else {
        input.setCustomValidity("");
    }
}

function AddCommas(txtBox) {
    if (txtBox.value.length > 0) {
        IsCommaExist = txtBox.value.split(',');
        if (IsCommaExist.length > 0) {
            txtBox.value = txtBox.value.replace(/\,/g, '');
        }
        flag = false;
        if (txtBox.value.substring(0, 1) == '-') {
            flag = true;
            txtBox.value = txtBox.value.substring(1);
        }
        txtBox.value = txtBox.value.replace(/(.)(?=(.{3})+$)/g, "$1,");
        if (flag) {
            txtBox.value = '-' + txtBox.value;
        }
    }
}

function RemoveCommas(txtBox) {
    if (txtBox.value != '') {
        a = txtBox.value.replace(/\,/g, '');
        txtBox.value = parseInt(a, 10);
    }
}

function PercentageValidation(input) {

    var regex = /^[0-9]{0,3}(\.[0-9]{0,2})?$/;
    var str = input.value;

    if (str.length != 0) {

        str = str.trim();

        if (regex.test(str) == false) {
            str = str.substring(0, str.length - 1);
        }

        $(input).val(str);
    }
}

function DisableControl(control) {
    if ((!$('#frm')[0].checkValidity || $('#frm')[0].checkValidity())) {
        setTimeout(function () { $(control)[0].disabled = true; }, 100);
    }
}