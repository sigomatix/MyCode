/// <reference path="http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js" />

/* 

SPECIFICATION:

The searcher allow to search for a list of item containing a specified term.

The searcher needs an interface so that:
- He knows how search items there are
- He can obtain retrieve a specific search item by index
- He can decide if a specific search item should
    - Visible
    - Hidden

*/

Searcher = (function () {

    return {
        search: function () {
            return $.Deferred();
        }
    }

})();

test("When there is only one match it should make that match only visible", function () {

    var items = [{ text: "first term", visible: true }, { text: "second term", visible: true }, { text: "Searched Term", visible: true}];

    stop();

    Searcher.search("Searched Term").done(function () {
        equal(false, items[0].visible);
        equal(false, items[1].visible);
        equal(true, items[2].visible);
        start();
    });


});