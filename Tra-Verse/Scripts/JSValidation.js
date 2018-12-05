
// login email validation
function validateEmail() {
    var re = /^([0-9A-z]{5,30})(@[0-9A-z]{5,10})\.([0-9A-z]{2,3})$/;
    var email = document.getElementById('loginEmail').value;
    return re.test(email);
}

function validate() {
    var $result = $("#result");
    $result.text("");

    if (validateEmail()) {
        $result.text("Valid");
        $result.css("color", "green", "bold");
    } else {
        $result.text("Invalid");
        $result.css("color", "red", "bold");
    }
    return false;
}

// login password validation
function validatePassword() {
    var regEx = /^[A-z0-9]{6,}$/;
    var password = document.getElementById('loginPassword').value;
    return regEx.test(password);
}

function passValidate() {
    var $result = $("#passResult");
    $result.text("");

    if (validatePassword()) {
        $result.text("Valid");
        $result.css("color", "green", "bold");
    } else {
        $result.text("Invalid");
        $result.css("color", "red", "bold");
    }
    return false;
}

// register new user validation starts

// register email
function validateRegEmail() {
    var emailRE = /^([0-9A-z]{5,30})(@[0-9A-z]{5,10})\.([0-9A-z]{2,3})$/;
    var registrationEmail = document.getElementById('regEmail').value;
    return emailRE.test(registrationEmail);
}

function emailValidate() {
    var $result = $("#regEmailResult");
    $result.text("");

    if (validateRegEmail()) {
        $result.text("Valid");
        $result.css("color", "green", "bold");
    } else {
        $result.text("Invalid");
        $result.css("color", "red", "bold");
    }
    return false;
}

//register password
function validateRegPass() {
    var passRE = /^[A-z0-9]{6,}$/;
    var registrationPass = document.getElementById('regPassword').value;
    return passRE.test(registrationPass);
}

function regPassValidate() {
    var $result = $("#regPassResult");
    $result.text("");

    if (validateRegPass()) {
        $result.text("Valid");
        $result.css("color", "green", "bold");
    } else {
        $result.text("Invalid");
        $result.css("color", "red", "bold");
    }
    return false;
}

//confirm password reg ex
function validateConfirmPass() {
    var confirmPassRE = /^[A-z0-9]{6,}$/;
    var regConirmPass = document.getElementById('confPass').value;
    return confirmPassRE.test(regConirmPass);
}

function confPassValidate() {
    var $result = $("#confResult");
    $result.text("");

    if (validateConfirmPass()) {
        //if (($this.attr("confPass").value) === ($this.attr("regPassword").value)) {
            $result.text("Valid");
            $result.css("color", "green", "bold");
        //}
        //else {
            //$result.text("Invalid");
           // $result.css("color", "red", "bold");
       // }

    } else {
        $result.text("Invalid");
        $result.css("color", "red", "bold");
    }
    return false;
}
