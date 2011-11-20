Searcher = (function () {
    var worker = new Worker("SearcherWorker.js");

    return {
        Init: function (getNumberOfLinks, getLinkByIndex, show, hide) {
            worker.addEventListener('message', function (e) {
                switch (e.data.action) {
                    case "getNumberOfLinks":
                        worker.postMessage({ action: e.data.action, params: getNumberOfLinks() });
                        break;
                    case "getLinkByIndex":
                        worker.postMessage({ action: e.data.action, params: getLinkByIndex(e.data.params) });
                        break;
                    case "show":
                        show(e.data.params);
                        worker.postMessage({ action: e.data.action, params: e.data.params });
                        break;
                    case "hide":
                        hide(e.data.params);
                        worker.postMessage({ action: e.data.action, params: e.data.params });
                        break;
                    case "debug":
                        console.log(e.data.params);
                        break;
                }
            }, false);
        },
        Search: function (text) {
            worker.postMessage({ action: "search", params: text });
        }
    }
})();


$(function () {

    $a = $("a");
    Searcher.Init(function () {
        return $a.length;
    }, function (linkIndex) {
        return $($a[linkIndex]).text();
    }, function (idx) {
        $($a[idx]).show();
    }, function (idx) {
        $($a[idx]).hide();
    });

    $("#SearchLink").click(function () {
        Searcher.Search($("#SearchBox").attr("value"));
    });

});






