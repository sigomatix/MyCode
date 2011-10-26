TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts = function () {

    /* Mock expected jquery calls */
    var headlineElement = {
        attr: function (attributeName) {
            if (attributeName == "href") {
                return "/MarketUpdate/Pricing/";
            }
        }
    };

    var dialogParameters;
    modalDialog = function (obj) { dialogParameters = obj };

    $ = function (selector) {
        if (selector == headlineElement) {
            return headlineElement;
        }
    }

    /* Act */
    ViewNewsArticle(headlineElement);

    /* Assert */
    Assert.AreEqual("400px", dialogParameters.width);
    Assert.AreEqual("245px", dialogParameters.height);
    Assert.AreEqual("/MarketUpdate/Pricing/", dialogParameters.href);

}

TestViewNewsArticleShouldOpenABigDialogForRegularNews = function () {

    /* Mock expected jquery calls */
    var headlineElement = {
        attr : function (attributeName) {
            if (attributeName == "href") {
                return "/MarketUpdate/General/";
            }
        }
    };

    var dialogParameters;
    modalDialog = function (obj) { dialogParameters = obj };

    $ = function (selector) {
        if (selector == headlineElement) {
            return headlineElement;
        }
        if (selector == '#cboxLoadedContent > div') {
            return lightbox;
        }
    }

    /* Act */
    ViewNewsArticle(headlineElement);

    /* Assert */
    Assert.AreEqual("900px", dialogParameters.width);
    Assert.AreEqual("650px", dialogParameters.height);
    Assert.AreEqual("/MarketUpdate/General/", dialogParameters.href);

}


TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox = function () {

    /* Mock expected jquery calls */
    var headlineElement = {
        attr: function (attributeName) {
            if (attributeName == "href") {
                return "/MarketUpdate/General/";
            }
        }
    };

    var lightbox = { addClass: function (cssClass) { this.cssClass = cssClass; }, removeClass: function (cssClass) { this.cssClass = ""; } }

    var dialogParameters;
    modalDialog = function (obj) { dialogParameters = obj };

    $ = function (selector) {
        if (selector == headlineElement) {
            return headlineElement;
        }
        if (selector == '#cboxLoadedContent > div') {
            return lightbox;
        }
    }

    /* Act */
    ViewNewsArticle(headlineElement);

    /* Assert */
    dialogParameters.onCompleteCall();
    Assert.AreEqual("dialog_loaded", lightbox.cssClass);
    dialogParameters.onClosedCall();
    Assert.AreEqual("", lightbox.cssClass);

}