function ViewNewsArticle(headlineLink) {
    var $url = $(headlineLink).attr("href");

    if ($url.search("/MarketUpdate/Pricing/") > -1) {
        var dialogWidth = '400px';
        var dialogHeight = '245px';
    } else {
        var dialogWidth = '900px';
        var dialogHeight = '650px';
    }

    modalDialog({
        width: dialogWidth,
        height: dialogHeight,
        href: $url,
        onCompleteCall: function () {
            $('#cboxLoadedContent > div').addClass('dialog_loaded');
        },
        onClosedCall: function () {
            $('#cboxLoadedContent > div').removeClass('dialog_loaded');
        }
    });
}







