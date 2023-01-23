var menuPlaceholder = $('#lunch-placeholder');
var optionsPlaceholder = $('#options-placeholder');
menuPlaceholder.hide();
optionsPlaceholder.hide();
var buttonLunch = $('button[data-toggle="lunchsets"]');
var buttonOptions = $('button[data-toggle="options"]');

$(document).ready(function () {
    menuPlaceholder.fadeIn();
    optionsPlaceholder.hide();

    $(".option").off().click(function () {
        var inputElement = $(this).find('input[type=checkbox]').attr('id');
        clickCheckbox(inputElement);
    });
    $(".lunchset").click(function () {
        var inputElement = $(this).find('input[type=radio]').attr('id');
        if ($("#" + inputElement + "-card").hasClass("active-card")) {
            unclickRadio();
            removeActive();
            $('#submit').fadeOut();
            $('#counter').fadeOut();
        } else {
            unclickRadio();
            removeActive();
            makeActive(inputElement);
            clickRadio(inputElement);
        }
    });
});  

buttonLunch.click(function (event) {
    optionsPlaceholder.hide();
    menuPlaceholder.fadeIn();
});

buttonOptions.click(function (event) {
    menuPlaceholder.hide();
    optionsPlaceholder.fadeIn();
});

function unclickRadio() {
    $("input:radio").prop("checked", false);
}

function clickRadio(inputElement) {
    $("#" + inputElement).prop("checked", true);
    $('#submit').fadeIn();
    $('#counter').fadeIn();
}

function removeActive() {
    $(".lunchset").removeClass("active-card");
}

function makeActive(element) {
    $("#" + element + "-card").addClass("active-card");
}

function clickCheckbox(inputElement) {
    $('#submit').fadeIn();
    if ($("#" + inputElement + "-card").hasClass("active-card")){
        $("#" + inputElement + "-card").removeClass("active-card");
        $("#" + inputElement).prop("checked", false);
    } else {
        $("#" + inputElement + "-card").addClass("active-card");
        $("#" + inputElement).prop("checked", true);
    }
}

$('.minus').click(function () {
    var $input = $(this).parent().find('input');
    var count = parseInt($input.val()) - 1;
    count = count < 1 ? 1 : count;
    $input.val(count);
    $input.change();
    return false;
});
$('.plus').click(function () {
    var $input = $(this).parent().find('input');
    $input.val(parseInt($input.val()) + 1);
    $input.change();
    return false;
});