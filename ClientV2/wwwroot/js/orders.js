var placeholderElement = $('#orders-placeholder');
placeholderElement.hide();
var url = placeholderElement.data('url');

$(document).ready(function () {
    $.get(url).done(function (data) {
        placeholderElement.html(data);
        placeholderElement.fadeIn();
    });
});    

$('#date').change(function (e) {
    var form = $('#date-form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        placeholderElement.fadeOut( function (){
            placeholderElement.html(data);
            placeholderElement.fadeIn();
        });
    });
});