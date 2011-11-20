self.addEventListener('message', function (e) {
    switch (e.data.action) {
        case "getNumberOfLinks":
            getNumberOfLinksContinuation(e.data.params);
            getNumberOfLinksContinuation = null;
            break;
        case "show":
            showContinuation(e.data.params);
            showContinuation = null;
            break;
        case "hide":
            hideContinuation(e.data.params);
            hideContinuation = null;
            break;
        case "getLinkByIndex":
            getLinkByIndexContinuation(e.data.params);
            getLinkByIndexContinuation = null;
            break;
        case "search":
            currentLink = 0;
            searchText = e.data.params;
            getNumberOfLinks(function (n) {
                maxLinks = n;
                visitNextLink();
            });
            break;
    }
}, false);

function getNumberOfLinks(continuation) {
    getNumberOfLinksContinuation = continuation;
    self.postMessage({ action: "getNumberOfLinks" });
}

function show(linkId, continuation) {
    showContinuation = continuation;
    self.postMessage({ action: "show", params: linkId });
}

function hide(linkId, continuation) {
    hideContinuation = continuation;
    self.postMessage({ action: "hide", params: linkId });
}

function getLinkByIndex(linkId, continuation) {
    getLinkByIndexContinuation = continuation;
    self.postMessage({ action: "getLinkByIndex", params: linkId });
}

function visitNextLink() {
    if (currentLink < maxLinks) {
        getLinkByIndex(currentLink, function (link) {
            if (link.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                show(currentLink, function () { currentLink++; visitNextLink(); });
            }
            else {
                hide(currentLink, function () { currentLink++; visitNextLink(); });
            }
        });
    }
}





