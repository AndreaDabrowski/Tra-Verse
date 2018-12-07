
// login email validation
var loginRXArray = [0, 0];
var boolArray = [false, false];

loginRXArray[0] = /^([A-z0-9\-.]{5,})@([A-z0-9\-.]+)\.([A-z]{2,5})$/;
loginRXArray[1] = /[A-z0-9]{6,}$/;

function testAllLogin() {
    /*test all values of boolArray to find any false
     * if one is false this function returns false
     * otherwise, meaning all values of boolArray are "true"
     * we remove the "disabled" attribute on the submit button*/

    for (var i = 0; i < boolArray.length; i++) {
        if (boolArray[i] === false) {
            // if some input was not valid, add 'disabled' to button
            $('#loginButton').attr('disabled');
            return false;
        }
    }
    // if all inputs true remove disabled
    $('#loginButton').removeAttr('disabled');
}

function testLoginEmail() {
    //get the value of the input
    var logEmValue = $('#loginEmail').val();
    //get the error message element
    var loginEmError = $('#loginEmError');

    //test the value of the input
    if (!loginRXArray[0].test(logEmValue)) {
        //if test fails:
        //change error message
        loginEmError.html('Invalid');
        loginEmError.css('color', 'red');
        boolArray[0] = false;
    }
    else {
        loginEmError.html('Valid');
        loginEmError.css('color', 'green');
        boolArray[0] = true;
    }
    testAllLogin();
}

function testLoginPass() {
    //get the value of the input
    var loginPassValue = $('#loginPassword').val();
    //get the error message element
    var loginPassError = $('#loginPassError');

    //test the value of the input
    if (!loginRXArray[1].test(loginPassValue)) {
        //if test fails:
        //change error message
        loginPassError.html('Invalid');
        loginPassError.css('color', 'red');
        boolArray[1] = false;
    }
    else {
        loginPassError.html('Valid');
        loginPassError.css('color', 'green');
        boolArray[1] = true;
    }
    testAllLogin();
}