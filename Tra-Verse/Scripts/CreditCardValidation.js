var CreditRxArray = [0, 0, 0]
var VerifyArray = [false, false, false]

CreditRxArray[0] = /^[0-9]{16}$/; //credit card regex
CreditRxArray[1] = /^[A-z]{2,}\s[A-z]{2,}$/; //Name regex
CreditRxArray[2] = /^[0-9]{3}$/; //CVV regex

function testAllCredit() {
    /*test all values of testArray to find any false
     * if one is false this function returns false
     * otherwise, meaning all values of testArray are "true"
     * we remove the "disabled" attribute on the submit button*/

    for (var i = 0; i < VerifyArray.length; i++) {
        if (VerifyArray[i] === false) {
            // if some input was not valid, add 'disabled' to button
            $('#checkOutbutton').attr('disabled');
            return false;
        }
    }
    // if all inputs true remove disabled
    $('#checkOutbutton').removeAttr('disabled');
}

function testCreditCard() {
    //get the value of the input
    var cardNumValue = $('#cardNumber').val();
    //get the error message element
    var cardNumError = $('#cardNumError')

    //test the value of the input
    if (!CreditRxArray[0].test(cardNumValue)) {
        //if test fails:
        //change error message
        cardNumError.html('Invalid');
        cardNumError.css('color', 'red');
        VerifyArray[0] = false;
    }
    else {
        cardNumError.html('Valid');
        cardNumError.css('color', 'green');
        VerifyArray[0] = true;
    }
    testAllCredit();
}

function testName() {
    //get the value of the input
    var cardNameValue = $('#nameOnCard').val();
    //get the error message element
    var nameError = $('#nameError')

    //test the value of the input
    if (!CreditRxArray[1].test(cardNameValue)) {
        //if test fails:
        //change error message
        nameError.html('Invalid');
        nameError.css('color', 'red');
        VerifyArray[1] = false;
    }
    else {
        nameError.html('Valid');
        nameError.css('color', 'green');
        VerifyArray[1] = true;
    }
    testAllCredit();
}

function testCVVCode() {
    //get the value of the input
    var cvvValue = $('#cvvPass').val();
    //get the error message element
    var cvvError = $('#cvvError')

    //test the value of the input
    if (!CreditRxArray[2].test(cvvValue)) {
        //if test fails:
        //change error message
        cvvError.html('Invalid');
        cvvError.css('color', 'red');
        VerifyArray[2] = false;
    }
    else {
        cvvError.html('Valid');
        cvvError.css('color', 'green');
        VerifyArray[2] = true;
    }
    testAllCredit();
}