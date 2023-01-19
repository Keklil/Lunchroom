$(document).ready(function () {
    var placeholderElement = $('#menu-placeholder');
    placeholderElement.hide();
    var buttonLunch = $('button[data-toggle="lunchsets"]');
    $.get(buttonLunch.data('url')).done(function (data) {
        placeholderElement.html(data);
        placeholderElement.fadeIn();
    })
    
    buttonLunch.click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.fadeOut();
            placeholderElement.html(data);
            placeholderElement.fadeIn();
        });
    });
});