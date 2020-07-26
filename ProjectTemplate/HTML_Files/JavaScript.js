"use strict";

function processForm(inputForm) {

    if (!inputForm.checkValidity()) {
        alert("See highlighted input boxes, there are input errors!");
    }
    else {

        var createAccountForm = document.getElementById('createAccountFormId')

        var fname = document.getElementById(fNameCreateId).value;

        alert(fname);


        //window.location.href = 'file:///C:/Users/baden/OneDrive/Desktop/CIS%20440%20Project/LoginPage/Login%20Page.html';

    }

}