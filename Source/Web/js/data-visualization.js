// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
App.dataVisualization = (function($, sigma) {
    var sigmaInstance = null;
    var nodeLabels = [];
    var nodeMap = {};

    var minHeight = 400;

    var sigmaContainerId = "sigma_content";
    var dataFilePath = "data/data.json.js"; // fake, actually hasn't loaded async

    var sigmaOptions = {
        drawing: {
            defaultLabelColor: "#fff",
            defaultLabelSize: 12,
            defaultLabelBGColor: "#fff",
            defaultLabelHoverColor: "#000",
            labelThreshold: 4, // minimum size a node has to have to have its label being displayed
            defaultEdgeType: "curve"
        },
        graph: {
            minNodeSize: .5,
            maxNodeSize: 25,
            minEdgeSize: 1,
            maxEdgeSize: 1
        },
        forceLabel: 1,
        type: "directed",
        maxRatio: 128
    };

    var sigmaParentSelector = ".sigma-parent";
    var loadingSelector = "#loading";
    var navbarSelector = "#navbar";
    
    var nodeLevelAttrValues = {
        level1: "1",
        level2: "2",
        level3: "3"
    };

    function init() {
        var $loading = $(loadingSelector);

        $loading.show();

        var sigmaContainer = document.getElementById(sigmaContainerId);
        setSigmaContainerHeight(sigmaContainer);

        sigmaInstance = sigma.init(sigmaContainer)
            .drawingProperties(sigmaOptions.drawing)
            .graphProperties(sigmaOptions.graph)
            .mouseProperties(sigmaOptions.maxRatio);

        sigmaInstance.parseJson(dataFilePath, function() {
            sigmaInstance.draw();

            cacheNodeLabels();
            initSearch();

            $loading.hide();
        });

        initSigmaHandlers();
        initWheelHandler();
    }

    function cacheNodeLabels() {
        if(nodeLabels.length) {
            return;
        }

        sigmaInstance.iterNodes(function(node) {
            nodeLabels.push(node.label);
            nodeMap[node.label] = node.id;
            node.attr.label = node.label; // needed for highlighting
        });

        nodeLabels.sort();
    }

    function setSigmaContainerHeight(sigmaContainer) {
        // adjust height of graph to screen
        var winHeight = $(window).height() - $(navbarSelector).height();
        if (winHeight > minHeight) {
            $(sigmaContainer).height(winHeight);
        }
    }

    function initSigmaHandlers() {
        sigmaInstance.bind("upnodes", function(event) {
            // on node click
            var node = getNode(event);
            var isFocusedNode = document.location.hash.replace(/#/i, '') == node.attr.label;
            
            if(isFocusedNode) {
                resetSearch();
                return;
            }

            document.location.hash = node.attr.label;
        });

        sigmaInstance.bind("overnodes", function(event) {
            // on node hover by mouse
            var node = getNode(event);
            var isHiddenNode = document.location.hash && !node.attr.hl;

            var isTooltipNeeded = !isHiddenNode && node.attr.attributes.Level === nodeLevelAttrValues.level3;

            if(isTooltipNeeded) {
                App.tooltip.show(node);
            }
        });

        function getNode(event) {
            return sigmaInstance.getNodes(event.content)[0];
        }
    }

    function initWheelHandler() {
        var forEach = Array.prototype.forEach;

        forEach.call($(sigmaParentSelector), function(element) {
            element.addEventListener("mousewheel", mouseWheelHandler, false);
            element.addEventListener("DOMMouseScroll", mouseWheelHandler, false); // Fix for FF
        });

        var depth = 0;
        var maxDepth = 8;
        
        function mouseWheelHandler(e) {
            if(!document.location.hash) {
                return;
            }

            var e = window.event || e;
            var delta = e.wheelDelta ? e.wheelDelta : -e.detail; // Fix for FF
            
            if(delta > 0 && depth < maxDepth) {
                depth++;
            } else if(delta < 0 && depth >= 0) {
                depth--;
            }

            if(delta < 0 && depth < 0) {
                resetSearch();
            }
            
            return false;
        }
    }

    function highlightNode(highlightedNode) {
        sigmaInstance.position(0, 0, 1);
        sigmaInstance.goTo(highlightedNode.displayX, highlightedNode.displayY, 2);
        sigmaInstance.position(0, 0, 1);

        var sources = {};
        var targets = {};

        var sourceColor = "#67A9CF";
        var targetColor = "#EF8A62";

        sigmaInstance.iterEdges(function(edge) {
            if (highlightedNode.attr.attributes.Level === nodeLevelAttrValues.level1 && 
                edge.attr.attributes.Tag === highlightedNode.id) {
                targets[edge.target] = true;
                setColor(edge, sourceColor);
                edge.hidden = 0;
            } else if (edge.source != highlightedNode.id && edge.target != highlightedNode.id) {
                edge.hidden = 1;
            } else if (edge.source == highlightedNode.id) {
                targets[edge.target] = true;
                setColor(edge, sourceColor);
                edge.hidden = 0;
            } else if (edge.target == highlightedNode.id) {
                setColor(edge, targetColor);
                sources[edge.source] = true;
                edge.hidden = 0;
            }
        }).iterNodes(function(node) {
            if (node.id == highlightedNode.id) {
                showNode(node);
            } else if (sources[node.id]) {
                showNode(node, targetColor);
            } else if (targets[node.id]) {
                showNode(node, sourceColor);
            } else {
                setOpacity(node, .05);
                node.label = null;
            }
        }).draw(2, 2, 2);
    }

    // show node with optional color, check if it satisfies possibly set filter
    function showNode(node, color) {
        if (color) { 
            setColor(node, color);
        }
        resetNode(node, 0);
    }

    // set the color of node or edge
    function setColor(element, color) {
        element.attr.hl = true;
        element.attr.color = element.color;
        element.color = color;
    }

    // set the opacity of node or edge
    function setOpacity(element, alpha) {
        var r, g, b;
        var color = element.color;
        if (color.indexOf("rgba") === 0) {
            var m = color.match(/(\d+),(\d+),(\d+),(\d*.?\d+)/);
            if (m) {
              var colors = m.slice(1,5);
              r = colors[0];
              g = colors[1];
              b = colors[2];
            }
        } else if (color.indexOf("rgb") === 0) {
            var m = color.match(/(\d+),(\d+),(\d+)/);
            if (m) {
                var colors = m.slice(1,5);
                r = colors[0];
                g = colors[1];
                b = colors[2];
            }
        }

        if (r && g && b) {
            element.color = "rgba(" + r + "," + g + "," + b + "," + alpha + ")";
        }
    }

    function resetNode(node, forceLabel) {
        node.hidden = 0;
        node.forceLabel = forceLabel;
        node.label = node.label || node.attr.label;
        setOpacity(node, 1);
    }

    function initSearch() {
        if (document.location.hash) {
            redirectToHash();
        }

        // search on hash change, unless it should trigger info or comments view
        $(window).bind("hashchange", function(event) {
            redirectToHash();
        });
    }

    function redirectToHash(hash) {
        if (hash) {
            document.location.hash = hash;
            return;
        }
        var query = decodeURIComponent(document.location.hash.replace(/^#/, ''));
        nodeSearch(query);
    }

    function nodeSearch(query) {
        resetNodesAndEdges();
        if (queryHasResult(query)) {
            document.location.hash = query;
            var node = sigmaInstance.getNodes(nodeMap[query]);
            highlightNode(node);
        }
    }

    function queryHasResult(query) {
        return nodeLabels.indexOf(query) !== -1;
    }

    function resetSearch() {
        document.location.href = document.location.pathname;
    }

    function resetNodesAndEdges() {
        sigmaInstance.iterNodes(function(node) {
            if (node.attr.hl) {
                node.color = node.attr.color;
                node.attr.hl = false;
            }
            resetNode(node, 0);
        }).iterEdges(function(edge) {
            if (edge.attr.hl) {
                edge.color = edge.attr.color;
                edge.attr.hl = false;
            }
            edge.hidden = 0;
        }).draw(2, 2, 2);
    }

    return {
        init: init
    };
})($, sigma);