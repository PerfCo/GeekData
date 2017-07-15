// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
App.dataVisualization = (function($, sigma) {
    var sigmaInstance = null;
    var nodeLabels = [];
    var nodeMap = {};

    var minHeight = 400;

    var sigmaContainerId = "sigma_content";
    var dataFilePath = "data/data.json.js"; // fake, actually hasn't loaded async
    
    var camera;

    var loadingSelector = "#loading";
    var navbarSelector = "#navbar";
    
    var nodeLevelAttrValues = {
        level1: "1",
        level2: "2",
        level3: "3"
    };

    var resetCameraHandler;

    function init() {
        var $loading = $(loadingSelector);

        $loading.show();

        // IMPORTANT: document.getElementById is required
        var sigmaContainer = document.getElementById(sigmaContainerId);
        setSigmaContainerHeight(sigmaContainer);

        var graphData = getGraphData();

        sigmaInstance = new sigma({
            graph: graphData,
            renderer: {
            // IMPORTANT:
            // This works only with the canvas renderer, so the
            // renderer type set as "canvas" is necessary here.
            container: sigmaContainer,
                type: 'canvas'
            },
            settings: {
                defaultLabelColor: "#fff",
                defaultLabelSize: 12,
                defaultLabelHoverColor: "#000",
                labelThreshold: 4, // minimum size a node has to have to have its label being displayed
                defaultEdgeType: "curve",
                minNodeSize: .5,
                maxNodeSize: 25,
                hideEdgesOnMove: true,
                minEdgeSize: 0.35,
                maxEdgeSize: 0.35,
                //borderSize: 2, // The size of the border of hovered nodes.
                animationsTime: 1000,
                verbose: true, // Indicates if sigma can log its errors and warnings
                zoomMax: 1 // The minimum zooming level.
            }
        });

        camera = sigmaInstance.cameras[0]; // needed for highlighting

        cacheNodeLabels();

        $loading.hide();

        initSearch();

        initSigmaHandlers();
        initWheelHandler();
    }

    function getGraphData() {
        var nodes = [];
        var edges = [];

        for (var i = 0; i < data.nodes.length; i++) {
            var node = data.nodes[i];
            node.borderColor = node.color;
            node.borderWidth = 0;
            node.attr = {};
            nodes.push(node);
        }

        for(var j = 0; j < data.edges.length; j++) {
            var edge = data.edges[j];
            edge.source = edge.sourceID;
            edge.target = edge.targetID;
            edge.type = "curve";
            edge.attr = {}; // needed for highlighting
            edges.push(edge);
        }

        return {
            nodes: nodes,
            edges: edges
        }
    };

    function cacheNodeLabels() {
        if(nodeLabels.length) {
            return;
        }

        sigmaInstance.graph.nodes().forEach(function(node) {
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
        sigmaInstance.bind("clickNode", function(event) {
            // on node click
            var node = getNode(event);
            var isFocusedNode = document.location.hash.replace(/#/i, '') == node.label;
            
            if(isFocusedNode) {
                resetSearch();
                return;
            }

            document.location.hash = node.attr.label;
        });

        sigmaInstance.bind("overNode", function(event) {
            // on node hovered by mouse
            var node = getNode(event);

            var isHiddenNode = document.location.hash && !node.label;

            var isTooltipNeeded = !isHiddenNode && node.attributes.Level === nodeLevelAttrValues.level3;

            if(isTooltipNeeded) {
                App.tooltip.show(node);
            }
        });

        function getNode(event) {
            return event.data.node;
        }
    }

    function initWheelHandler() {
        var canvasSelector = "#" + sigmaContainerId + " canvas:last-child";
        var canvasElement = document.querySelector(canvasSelector);

        canvasElement.addEventListener('DOMMouseScroll', mouseWheelHandler, false);
        canvasElement.addEventListener('mousewheel', mouseWheelHandler, false);

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
        sigma.misc.animation.camera(
            sigmaInstance.camera, {
                x: highlightedNode[sigmaInstance.camera.readPrefix + 'x'], 
                y: highlightedNode[sigmaInstance.camera.readPrefix + 'y'],
                ratio: 0.5
            }, {
                duration: 500
            }
        );

        var sources = {};
        var targets = {};

        var sourceColor = "#67A9CF";
        var targetColor = "#EF8A62";

        sigmaInstance.graph.edges().forEach(function(edge) {
            if (highlightedNode.attributes.Level === nodeLevelAttrValues.level1 && 
                edge.attributes.Tag === highlightedNode.id) {
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
        });

        sigmaInstance.graph.nodes().forEach(function(node) {
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
        });
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
        element.highlighted = true;
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
        if(!query) {
            return;
        }

        resetNodesAndEdges();
        if (queryHasResult(query)) {
            document.location.hash = query;
            var node = sigmaInstance.graph.nodes(nodeMap[query]);
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
        sigmaInstance.graph.nodes().forEach(function(node) {
            if (node.highlighted) {
                node.color = node.attr.color;
                node.highlighted = false;
            }
            resetNode(node, 0);
        });
        sigmaInstance.graph.edges().forEach(function(edge) {
            if (edge.highlighted) {
                edge.color = edge.attr.color;
                edge.highlighted = false;
            }
            edge.hidden = 0;
        });
    }

    return {
        init: init
    };
})($, sigma);