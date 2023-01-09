// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".option").off().click(function () {
        var inputElement = $(this).find('input[type=checkbox]').attr('id');
        clickCheckbox(inputElement);
    });
    $(".lunchset").click(function () {//Clicking the card
        var inputElement = $(this).find('input[type=radio]').attr('id');
        unclickRadio();
        removeActive();
        makeActive(inputElement);
        clickRadio(inputElement);
    });
});

function unclickRadio() {
    $("input:radio").prop("checked", false);
}

function clickRadio(inputElement) {
    $("#" + inputElement).prop("checked", true);
}

function removeActive() {
    $(".lunchset").removeClass("active-card");
}

function makeActive(element) {
    $("#" + element + "-card").addClass("active-card");
}

function clickCheckbox(inputElement) {
    if ($("#" + inputElement + "-card").hasClass("active-card")){
        console.log("active true");
        $("#" + inputElement + "-card").removeClass("active-card");
        $("#" + inputElement).prop("checked", false);
    } else {
        console.log("active false");
        $("#" + inputElement + "-card").addClass("active-card");
        $("#" + inputElement).prop("checked", true);  
    }
}