var regexArray = [0, 0, 0]
var testArray = [false, false, false]

regexArray[0] = /^([A-z0-9\-.]{5,})@([A-z0-9\-.]+)\.([A-z]{2,5})$/;
regexArray[1] = /[A-z0-9]{6,}$/;
regexArray[2] = /[A-z0-9]{6,}$/;

function testAllRegister() {
    /*test all values of testArray to find any false
     * if one is false this function returns false
     * otherwise, meaning all values of testArray are "true"
     * we remove the "disabled" attribute on the submit button*/

    for (var i = 0; i < testArray.length; i++) {
        if (testArray[i] === false) {
            // if some input was not valid, add 'disabled' to button
            $('#regButton').attr('disabled');
            return false;
        }
    }
    // if all inputs true remove disabled
    $('#regButton').removeAttr('disabled');
}

function testRegisterEmail() {
    //get the value of the input
    var regEmValue = $('#regEmail').val();
    //get the error message element
    var regEmailError = $('#regEmailError')

    //test the value of the input
    if (!regexArray[0].test(regEmValue)) {
        //if test fails:
        //change error message
        regEmailError.html('Invalid');
        regEmailError.css('color', 'red');
        testArray[0] = false;
    }
    else {
        regEmailError.html('Valid');
        regEmailError.css('color', 'green');
        testArray[0] = true;
    }
    testAllRegister();
}

function testRegPass() {
    //get the value of the input
    var regPassValue = $('#regPassword').val();
    //get the error message element
    var regPassError = $('#regPassError')

    //test the value of the input
    if (!regexArray[1].test(regPassValue)) {
        //if test fails:
        //change error message
        regPassError.html('Invalid');
        regPassError.css('color', 'red');
        testArray[1] = false;
    }
    else {
        regPassError.html('Valid');
        regPassError.css('color', 'green');
        testArray[1] = true;
    }
    testAllRegister();
}

function checkPassMatch() {
    var regPassValue = $('#regPassword').val();
    var conPassValue = $('#confPass').val();
    var confPassError = $('#confPassError')

    //test the value of the input
    if (conPassValue !== regPassValue) {
        //if test fails:
        //change error message
        confPassError.html('Invalid');
        confPassError.css('color', 'red');
        testArray[2] = false;
    }
    else {
        confPassError.html('Valid');
        confPassError.css('color', 'green');
        testArray[2] = true;
    }
    testAllRegister();
}