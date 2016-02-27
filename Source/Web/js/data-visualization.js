// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
App.dataVisualization = (function($, sigma) {
    var sigmaContainerId = null;
    var dataFileName = null;
    var sigmaInstance = null;
    var filters = {};
    var graph = null;
    var sigmaProperties = null;
    var nodeLabels = [];
    var nodeMap = {};
    
    var activeFilterId = null;
    var activeFilterVal = null;
    var sourceColor = '#67A9CF';
    var targetColor = '#EF8A62';

    var $loading = $('#loading');

    function init(options, callback) {
        $loading.show();
        sigmaContainerId = options.sigmaContainerId;
        dataFileName = options.dataFilePath;
        sigmaProperties = options.sigmaProperties;
        var viscontainer = document.getElementById(sigmaContainerId);

        // adjust height of graph to screen
        var winHeight = $(window).height() - $('#navbar').height();
        if (winHeight > 400) {
            $(viscontainer).height(winHeight);
        }

        sigmaInstance = sigma.init(viscontainer)
            .drawingProperties(sigmaProperties['drawing'])
            .graphProperties(sigmaProperties['graph'])
            .mouseProperties({ maxRatio: 128 });

        sigmaInstance.parseJson(dataFileName, function(){
            sigmaInstance.draw();
            // create array of node labels used for auto complete once
            if (0 == nodeLabels.length) {
                sigmaInstance.iterNodes(function(n){
                    nodeLabels.push(n.label);
                    nodeMap[n.label] = n.id;
                    n.attr.label = n.label;// needed for highlighting
                });
                nodeLabels.sort();
            }

            initSearch();

            if (callback) { 
                callback();
            }

            $loading.hide();
        });

        sigmaInstance.bind('upnodes', function(event) {
            // on node click
            hnode = sigmaInstance.getNodes(event.content)[0];
            if(document.location.hash.replace(/#/i, '') == hnode.attr.label) {
                resetSearch();
                return;
            }
            document.location.hash = hnode.attr.label;
        });

        sigmaInstance.bind('overnodes', function(event) {
            // on node hover by mouse
            var nodeData = sigmaInstance.getNodes(event.content)[0];
            var tooltipData = nodeData.attr.attributes;

            if(tooltipData.Level != 3 || document.location.hash && !nodeData.attr.hl) {
                return;
            }

            App.tooltip.show(nodeData);
        });

        sigmaInstance.bind('outnodes', function(event) {
            // on mouse out of node
        });

        addWheelHandler();
    }

    function addWheelHandler() {
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

    // called with array of ids of attributes to use as filters
    function getFilters(attribs) {
        sigmaInstance.iterNodes(function(n) {
            for (i in attribs) {
                var aname = attribs[i];
                if (n.attr.attributes.hasOwnProperty(aname)) {
                    var vals = n.attr.attributes[aname].split('|');
                    for (v in vals) {
                        val = vals[v];
                        if (!filters.hasOwnProperty(val)) {
                            filters[val] = 0;
                        }
                        filters[val]++;
                    }
                }
            }
        });
        // sort by frequencies of filter attributes
        var sorted = [];
        for (var a in filters) {
            sorted.push([a, filters[a]]);
        }
        sorted.sort(function(a, b) { return b[1] - a[1] });
        return sorted;
    }

    function nodeHasFilter(node, filterid, filterval) {
        return node.attr.attributes.hasOwnProperty(filterid) && 
            node.attr.attributes[filterid].indexOf(filterval) !== -1;
    }

    // show only nodes that match filter
    function setFilter(filterid, filterval) {
        activeFilterId = filterid;
        activeFilterVal = filterval;
        sigmaInstance.iterNodes(function(n){
            n.hidden = filterval ? 1 : 0;
            if (nodeHasFilter(n, filterid, filterval)) {
                n.hidden = 0;
            }
        }).draw(2,2,2);
    }

    // return true if given node does not satisfy set filter, else false
    function filteredOut(node) {
        if (activeFilterId !== null && activeFilterVal !== null && 
            !nodeHasFilter(node, activeFilterId, activeFilterVal)) {
            return true;
        }
        return false;
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
        if (filteredOut(node)) { 
            return; 
        }
        if (color) { 
            setColor(node, color);
        }
        resetNode(node, 0);
    }

    function highlightNode(node) {
        sigmaInstance.position(0,0,1);
        sigmaInstance.goTo(node.displayX, node.displayY, 2);
        sigmaInstance.position(0,0,1);

        var sources = {};
        var targets = {};
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
        }).draw(2,2,2);
    }

    function clear() {
        sigmaInstance.emptyGraph();
        document.getElementById(sigmaContainerId).innerHTML = '';
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
            if (filteredOut(n)) {
                n.hidden = 1;
            }
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
        activeFilterVal = null;
        resetNodes();
    }

    function reset() {
        activeFilterId = null;
        activeFilterVal = null;
        resetNodes();
        dialog.hide();
    }

    return {
        init: init,
        getFilters: getFilters
    };
})($, sigma);