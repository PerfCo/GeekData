(function(win, doc) {

    function getAncors() {
        var links = doc.getElementsByTagName("a");
        var ancors = [];
        for (var i = 0, length = links.length; i < length; ++i) {
            var href = links[i];
            if (href.host != win.location.host && href.className != "github-corner") {
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

    function initAncors() {
        var ancors = getAncors();
        for (var a in ancors) {
            ancors[a].addEventListener("click", onUrlClick)
        }
    }

    function initVisgexf() {
        var options = {

            sigmaContainerId: "sigma_content",
            dataFilePath: "data/data.json.js", // fake, actually hasn't loaded async
            
            sigmaProperties: {
                drawing: {
                    defaultLabelColor: '#fff',
                    defaultLabelSize: 12,
                    defaultLabelBGColor: '#fff',
                    defaultLabelHoverColor: '#000',
                    labelThreshold: 4, // minimum size a node has to have to have its label being displayed
                    defaultEdgeType: 'curve'
                },
                graph: {
                    minNodeSize: .5,
                    maxNodeSize: 25,
                    minEdgeSize: 1,
                    maxEdgeSize: 1
                },
                forceLabel: 1,
                type: 'directed'
            }
        };

        App.visgexf.init(options, function() {
            var filterid = 'paradigms';
            var filters = App.visgexf.getFilters([filterid]);
        });
    }

    $(function(){
        initVisgexf();
    });

    $(window).load(function () {
        initAncors();
    });
}(window, document));