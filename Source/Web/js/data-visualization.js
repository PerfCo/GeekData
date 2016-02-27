// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
App.dataVisualization = (function($, sigma) {
    var sigmaInstance = null;
    var nodeLabels = [];
    var nodeMap = {};
    
    var filters = {};
    var activeFilterId = null;
    var activeFilterValue = null;

    var minHeight = 400;
    
    var $loading = $('#loading');

    function init(options, callback) {
        $loading.show();
        var sigmaContainerId = options.sigmaContainerId;
        var dataFileName = options.dataFilePath;
        sigmaProperties = options.sigmaProperties;

        var sigmaContainer = document.getElementById(sigmaContainerId);
        setSigmaContainerHeight(sigmaContainer);

        sigmaInstance = sigma.init(sigmaContainer)
            .drawingProperties(sigmaProperties['drawing'])
            .graphProperties(sigmaProperties['graph'])
            .mouseProperties({ maxRatio: 128 });

        sigmaInstance.parseJson(dataFileName, function() {
            sigmaInstance.draw();
            // create array of node labels used for auto complete once
            if (0 == nodeLabels.length) {
                sigmaInstance.iterNodes(function(n) {
                    nodeLabels.push(n.label);
                    nodeMap[n.label] = n.id;
                    n.attr.label = n.label; // needed for highlighting
                });
                nodeLabels.sort();
            }

            initSearch();

            if (callback) { 
                callback();
            }

            $loading.hide();
        });

        initSigmaHandlers();
        initWheelHandler();
    }

    function setSigmaContainerHeight(sigmaContainer) {
        // adjust height of graph to screen
        var winHeight = $(window).height() - $('#navbar').height();
        if (winHeight > minHeight) {
            $(sigmaContainer).height(winHeight);
        }
    }

    function initSigmaHandlers() {
        sigmaInstance.bind('upnodes', function(event) {
            // on node click
            var node = sigmaInstance.getNodes(event.content)[0];
            if(document.location.hash.replace(/#/i, '') == node.attr.label) {
                resetSearch();
                return;
            }
            document.location.hash = node.attr.label;
        });

        sigmaInstance.bind('overnodes', function(event) {
            // on node hover by mouse
            var node = sigmaInstance.getNodes(event.content)[0];
            var tooltipData = node.attr.attributes;

            if(tooltipData.Level !== "3" || document.location.hash && !node.attr.hl) {
                return;
            }

            App.tooltip.show(node);
        });

        sigmaInstance.bind('outnodes', function(event) {
            // on mouse out of node
        });
    }

    function initWheelHandler() {
        var forEach = Array.prototype.forEach;

        forEach.call($('.sigma-parent'), function(v) {
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
        // don't change node an edge colors of undirected graphs
        if (sigmaProperties.type == "undirected") { 
            return; 
        }

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

    function nodeHasFilter(node, filterId, filterValue) {
        return node.attr.attributes.hasOwnProperty(filterId) && 
            node.attr.attributes[filterId].indexOf(filterValue) !== -1;
    }

    function resetNode(node, forceLabel) {
        node.hidden = 0;
        node.forceLabel = forceLabel;
        if (!node.label) { 
            node.label = node.attr.label;
        }
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

        sigmaInstance.iterEdges(function(e){
            if (node.attr.attributes.Level === "1" && e.attr.attributes.Tag === node.id){
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
        var labels = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: $.map(nodeLabels, function(label) { 
                return { value: label }; 
            }),
            limit: 20
        });

        labels.initialize();

        if (document.location.hash) {
            redirectHash();
        }

        // search on hash change, unless it should trigger info or comments view
        $(window).bind('hashchange', function(event) {
            redirectHash();
        });
    }

    function redirectHash(hash) {
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
            return query;
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