(function(win, doc) {

    function getAncors() {
        var links = doc.getElementsByTagName("a");
        var ancors = [];
        for (var i = 0, length = links.length; i < length; ++i) {
            var href = links[i];
            if (href.host != win.location.host) {
                ancors.push(href);
            }
        }
        return ancors;
    }

    function onUrlClick(ancor) {
        var url = ancor.srcElement || ancor.target;
        while (url && (typeof url.tagName == "undefined" || url.tagName.toLowerCase() != "a" || !url.href)) {
            url = url.parentNode
        }
        if (!url || !url.href) {
            return
        }
        if (win.ga) {
            ga("send", "event", "Outbound link", "Click", url.href)
        }
        if (!url.target || url.target.match(/^_(self|parent|top)$/i)) {
            setTimeout(function() {
                ancor.location.href = url.href
            }, 150);
            if (ancor.preventDefault) {
                ancor.preventDefault()
            } else {
                ancor.returnValue = false
            }
        }
    }

    win.addEventListener("load", function() {
        var ancors = getAncors();
        for (var a in ancors) {
            ancors[a].addEventListener("click", onUrlClick)
        }
    })
}(window, document));