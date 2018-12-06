
// login email validation
var regexArray = [0, 0]
var testArray = [false, false]

regexArray[0] = /^([A-z0-9\-.]{5,})@([A-z0-9\-.]+)\.([A-z]{2,5})$/;
regexArray[1] = /[A-z0-9]{6,}$/;

function testAllLogin() {
    /*test all values of testArray to find any false
     * if one is false this function returns false
     * otherwise, meaning all values of testArray are "true"
     * we remove the "disabled" attribute on the submit button*/

    for (var i = 0; i < testArray.length; i++) {
        if (testArray[i] === false) {
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
    var loginEmError = $('#loginEmError')

    //test the value of the input
    if (!regexArray[0].test(logEmValue)) {
        //if test fails:
        //change error message
        loginEmError.html('Invalid');
        loginEmError.css('color', 'red');
        testArray[0] = false;
    }
    else {
        loginEmError.html('Valid');
        loginEmError.css('color', 'green');
        testArray[0] = true;
    }
    testAllLogin();
}

function testLoginPass() {
    //get the value of the input
    var loginPassValue = $('#loginPassword').val();
    //get the error message element
    var loginPassError = $('#loginPassError')

    //test the value of the input
    if (!regexArray[1].test(loginPassValue)) {
        //if test fails:
        //change error message
        loginPassError.html('Invalid');
        loginPassError.css('color', 'red');
        testArray[1] = false;
    }
    else {
        loginPassError.html('Valid');
        loginPassError.css('color', 'green');
        testArray[1] = true;
    }
    testAllLogin();
}