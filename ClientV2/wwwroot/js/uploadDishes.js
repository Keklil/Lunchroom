$(document).ready(function () {
    var placeholderElement = $('#additional-dishes-placeholder');

    $('button[data-toggle="ajax-dishes"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
        });
    });
});