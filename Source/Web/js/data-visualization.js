// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
App.dataVisualization = (function($, sigma) {
    var sigmaInstance = null;
    var nodeLabels = [];
    var nodeMap = {};
    
    var filters = {};
    var activeFilterId = null;
    var activeFilterValue = null;

    var minHeight = 400;

    var sigmaContainerId = "sigma_content";
    var dataFilePath = "data/data.json.js"; // fake, actually hasn't loaded async

    var sigmaOptions = {
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
        type: 'directed',
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
        if (nodeLabels.length == 0) {
            sigmaInstance.iterNodes(function(n) {
                nodeLabels.push(n.label);
                nodeMap[n.label] = n.id;
                n.attr.label = n.label; // needed for highlighting
            });
            nodeLabels.sort();
        }
    }

    function setSigmaContainerHeight(sigmaContainer) {
        // adjust height of graph to screen
        var winHeight = $(window).height() - $(navbarSelector).height();
        if (winHeight > minHeight) {
            $(sigmaContainer).height(winHeight);
        }
    }

    function initSigmaHandlers() {
        sigmaInstance.bind('upnodes', function(event) {
            // on node click
            var node = getNode(event);
            var isFocusedNode = document.location.hash.replace(/#/i, '') == node.attr.label;
            
            if(isFocusedNode) {
                resetSearch();
                return;
            }

            document.location.hash = node.attr.label;
        });

        sigmaInstance.bind('overnodes', function(event) {
            // on node hover by mouse
            var node = getNode(event);
            var isHiddenNode = document.location.hash && !node.attr.hl;

            var isTooltipNeeded = !isHiddenNode && node.attr.attributes.Level === nodeLevelAttrValues.level3;

            if(isTooltipNeeded) {
                App.tooltip.show(node);
            }
        });

        sigmaInstance.bind('outnodes', function(event) {
            // on mouse out of node
        });

        function getNode(event) {
            return sigmaInstance.getNodes(event.content)[0];
        }
    }

    function initWheelHandler() {
        var forEach = Array.prototype.forEach;

        forEach.call($(sigmaParentSelector), function(v) {
            v.addEventListener('mousewheel', mouseWheelHandler, false);
            v.addEventListener('DOMMouseScroll', mouseWheelHandler, false); // Fix for FF
        });

        var depth = 0;
        
        function mouseWheelHandler(e) {
            if(!document.location.hash) {
                return;
            }

            var e = window.event || e;
            var delta = e.wheelDelta ? e.wheelDelta : -e.detail; // Fix for FF
            
            if(delta > 0 && depth < 8) {
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

    // set the color of node or edge
    function setColor(o, c) {
        o.attr.hl = true;
        o.attr.color = o.color;
        o.color = c;
    }

    // set the opacity of node or edge
    function setOpacity(o, alpha) {
        var r, g, b;
        var color = o.color;
        if (color.indexOf('rgba') === 0) {
            var m = color.match(/(\d+),(\d+),(\d+),(\d*.?\d+)/);
            if (m) {
              var colors = m.slice(1,5);
              r = colors[0];
              g = colors[1];
              b = colors[2];
            }
        } else if (color.indexOf('rgb') === 0) {
            var m = color.match(/(\d+),(\d+),(\d+)/);
            if (m) {
                var colors = m.slice(1,5);
                r = colors[0];
                g = colors[1];
                b = colors[2];
            }
        }

        if (r && g && b) {
            o.color = 'rgba(' + r + ',' + g + ',' + b + ',' + alpha + ')';
        }
    }

    function resetNode(node, forceLabel) {
        node.hidden = 0;
        node.forceLabel = forceLabel;
        node.label = node.label || node.attr.label;
        setOpacity(node, 1);
    }

    // show node with optional color, check if it satisfies possibly set filter
    function nodeShow(node, color) {
        if (color) { 
            setColor(node, color);
        }
        resetNode(node, 0);
    }

    function highlightNode(node) {
        sigmaInstance.position(0, 0, 1);
        sigmaInstance.goTo(node.displayX, node.displayY, 2);
        sigmaInstance.position(0, 0, 1);

        var sources = {};
        var targets = {};

        var sourceColor = '#67A9CF';
        var targetColor = '#EF8A62';

        sigmaInstance.iterEdges(function(e) {
            if (node.attr.attributes.Level === nodeLevelAttrValues.level1 && e.attr.attributes.Tag === node.id){
                targets[e.target] = true;
                setColor(e, sourceColor);
                e.hidden = 0;
            } else if (e.source != node.id && e.target != node.id) {
                e.hidden = 1;
            } else if (e.source == node.id) {
                targets[e.target] = true;
                setColor(e, sourceColor);
                e.hidden = 0;
            } else if (e.target == node.id) {
                setColor(e, targetColor);
                sources[e.source] = true;
                e.hidden = 0;
            }
        }).iterNodes(function(n){
            if (n.id == node.id) {
                nodeShow(n);
            } else if (sources[n.id]) {
                nodeShow(n, targetColor);
            } else if (targets[n.id]) {
                nodeShow(n, sourceColor);
            } else {
                setOpacity(n, .05);
                n.label = null;
            }
        }).draw(2, 2, 2);
    }

    function initSearch() {
        if (document.location.hash) {
            redirectToHash();
        }

        // search on hash change, unless it should trigger info or comments view
        $(window).bind('hashchange', function(event) {
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

    function queryHasResult(query) {
        return nodeLabels.indexOf(query) !== -1;
    }

    function nodeSearch(query) {
        resetFilter();
        if (queryHasResult(query)) {
            document.location.hash = query;
            var node = sigmaInstance.getNodes(nodeMap[query]);
            highlightNode(node);
        }
    }

    function resetNodes() {
        sigmaInstance.iterNodes(function(n) {
            if (n.attr.hl) {
                n.color = n.attr.color;
                n.attr.hl = false;
            }
            resetNode(n, 0);
        }).iterEdges(function(e) {
          if (e.attr.hl) {
              e.color = e.attr.color;
              e.attr.hl = false;
          }
          e.hidden = 0;
        }).draw(2, 2, 2);
    }

    function resetSearch() {
        document.location.href = document.location.pathname;
    }

    function resetFilter() {
        activeFilterId = null;
        activeFilterValue = null;
        resetNodes();
    }

    return {
        init: init
    };
})($, sigma);