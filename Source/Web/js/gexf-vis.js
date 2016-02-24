// Inspired by http://exploringdata.github.io/vis/programming-languages-influence-network
var App = {};
App.visgexf = (function() {
    var visualizationId = null;
    var dataFileName = null;
    var sigmaInstance = null;
    var filters = {};
    var graph = null;
    var visualizationProperties = null;
    var nodelabels = [];
    var nodemap = {};
    var searchInput = $('.typeahead');
    var activeFilterId = null;
    var activeFilterVal = null;
    var sourceColor = '#67A9CF';
    var targetColor = '#EF8A62';

    var tooltipElement = $("#tooltip");
    var tooltipLibContent = $("#lib_content");
    var tooltipGuruContent = $("#guru_content");
    var tooltipCourceContent = $("#cource_content");
    var tooltipGroupContent = $("#group_content");
    var tooltipLastTimeShown = new Date();
    var tooltipHideDelaySeconds = 3;

    function init(visid, filename, props, callback) {
        $('#loading').show();
        visualizationId = visid;
        dataFileName = filename;
        visualizationProperties = props;
        var viscontainer = document.getElementById(visualizationId);
        // adjust height of graph to screen
        var winHeight = $(window).height() - $('#navbar').height();
        if (winHeight > 400) {
            $(viscontainer).height(winHeight);
        }
        sigmaInstance = sigma.init(viscontainer)
            .drawingProperties(visualizationProperties['drawing'])
            .graphProperties(visualizationProperties['graph'])
            .mouseProperties({maxRatio: 128});

        sigmaInstance.parseJson(dataFileName, function(){
            sigmaInstance.draw();
            // create array of node labels used for auto complete once
            if (0 == nodelabels.length) {
                sigmaInstance.iterNodes(function(n){
                    nodelabels.push(n.label);
                    nodemap[n.label] = n.id;
                    n.attr.label = n.label;// needed for highlighting
                });
                nodelabels.sort();
            }
            initSearch();
            searchInput.focus();
            // call callback after json is parsed
            if (callback) callback();
            $('#loading').hide();
        });

        initTooltip();

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

            showTooltip(nodeData, tooltipData);
        });

        sigmaInstance.bind('outnodes', function(event) {
            // on mouse out of node

            /*var $tooltip = tooltipElement;
            $tooltip.delay(500).fadeOut(1000);*/
        });

        var forEach = Array.prototype.forEach;
        var $$ = document.querySelectorAll.bind(document);

        forEach.call($$('.sigma-parent'), function(v) {
            v.addEventListener('mousewheel', MouseWheelHandler, false);
            v.addEventListener('DOMMouseScroll', MouseWheelHandler, false);
        });

        var depth = 0;
        
        function MouseWheelHandler(e) {
            if(!document.location.hash) {
                return;
            }

            var e = window.event || e;
            var delta = e.wheelDelta ? e.wheelDelta : -e.detail;
            
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

    function initTooltip() {
        /*var $tooltip = tooltipElement;

        $tooltip.hover(function() {
            $("#tooltip").fadeIn("fast");
        }).mouseleave(function() {
            $tooltip.fadeOut(1000);
        });*/
    }

    function showTooltip(nodeData, tooltipData) {
        var $tooltip = tooltipElement;

        $tooltip;

        var typeMondatoryField = {
            lib: "HtmlUrlGithubRepository",
            geek: "DisplayNameStackOverflowUser",
            cource: "NamePluralsightCourse"
        };
        
        if(tooltipData[typeMondatoryField.geek]) {
            initPersonTooltip();
        } else if(tooltipData[typeMondatoryField.lib]) {
            initLibTooltip();
        } else if(tooltipData[typeMondatoryField.cource]) {
            initCourseTooltip();
        }

        var tooltipPosition = getTooltipPosition(nodeData);

        $tooltip.css({
            top: tooltipPosition.y, 
            left: tooltipPosition.x,
            opacity: 1
        });

        $tooltip.delay(1000).show();
        tooltipLastTimeShown = new Date();

        setTimeout(onTooltipTimeout, tooltipHideDelaySeconds * 1000);

        function onTooltipTimeout() {
            var nowDate = new Date();
            var diffSeconds = (nowDate.getTime() - tooltipLastTimeShown.getTime()) / 1000;
            
            if(diffSeconds >= tooltipHideDelaySeconds) {
                $tooltip.hide();
                return;
            }

            setTimeout(onTooltipTimeout, tooltipHideDelaySeconds * 1000);
        }

        function getTooltipPosition(nodeData) {
            var $window = $(window);
            var $tooltip = $("#tooltip");

            var winWidth = $window.width();
            var winHeight = $window.height();

            var tooltipWidth = $tooltip.outerWidth();
            var tooltipHeight = $tooltip.outerHeight();

            var marginX = 10;
            var marginY = 50;

            var x = nodeData.displayX + tooltipWidth >= winWidth ? 
                (winWidth - tooltipWidth - marginX) : nodeData.displayX;

            var y = nodeData.displayY + marginY + tooltipHeight >= winHeight ? 
                (nodeData.displayY - tooltipHeight + 30) : (nodeData.displayY + marginY);

            //console.log("displayY:" + nodeData.displayY + " tooltipHeight:" + tooltipHeight + " y:" + y)

            return {
                x: x,
                y: y
            };
        }

        function initPersonTooltip() {
            var $tooltip = tooltipElement;

            tooltipLibContent.hide();
            tooltipCourceContent.hide();
            tooltipGroupContent.hide();
            tooltipGuruContent.show();

            $("#guru_name")
                .text(tooltipData["DisplayNameStackOverflowUser"])
                .attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

            $("#guru_profile_url").attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

            var avatarUrl = tooltipData["ProfileImageStackOverflowUser"];
            
            /*if(avatarUrl.indexOf("gravatar.com/avatar/") > 0) { // fix gravatar.com avatar paths
                var i = avatarUrl.indexOf("?");
                if(i > 0){
                    avatarUrl = avatarUrl.substring(0, i);
                    avatarUrl = avatarUrl + ".jpg?s=128";
                    avatarUrl = avatarUrl.replace(/https/gi, 'http');
                }
            }*/

            if(avatarUrl && avatarUrl.indexOf("gravatar.com/avatar/") < 0) {
                $("#guru_avatar").attr("src", avatarUrl);
            } else {
                $("#guru_avatar").attr("src", "images/no-avatar.jpg");
            }

            console.log(avatarUrl)
            
            $("#guru_site").attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

            var badges = {};
            var badgesRawData = tooltipData["BadgeCountsStackOverflowUser"];
            if(badgesRawData) {
                badgesRawData = badgesRawData.replace(/([a-zA-Z][^:]*)(?=\s*:)/g, '"$1"'); // add quotes to make valid json
                badges = JSON.parse(badgesRawData);
            }

            var $goldBadge = $("#guru_badges_gold");
            var $silverBadge = $("#guru_badges_silver");
            var $bronzeBadge = $("#guru_badges_bronze");

            if(badges["Gold"]){
                $goldBadge.show().find(".badgecount").text(badges["Gold"]);
            } else {
                $goldBadge.hide().find(".badgecount").text("");
            }

            if(badges["Silver"]){
                $silverBadge.show().find(".badgecount").text(badges["Gold"]);
            } else {
                $silverBadge.hide().find(".badgecount").text("");
            }

            if(badges["Bronze"]){
                $bronzeBadge.show().find(".badgecount").text(badges["Bronze"]);
            } else {
                $bronzeBadge.hide().find(".badgecount").text("");
            }
        }

        function initLibTooltip() {
            var $tooltip = tooltipElement;

            tooltipGuruContent.hide();
            tooltipCourceContent.hide();
            tooltipGroupContent.hide();
            tooltipLibContent.show();
            
            $("#lib_name").text(nodeData.label);
            $("#lib_url").attr("href", tooltipData["HtmlUrlGithubRepository"]);
            $("#lib_description").text(tooltipData["DescriptionGithubRepository"]);
            $("#lib_stars_count").text(tooltipData["StargazersCountGithubRepository"]);
        }

        function initCourseTooltip() {
            tooltipGuruContent.hide();
            tooltipLibContent.hide();
            tooltipGroupContent.hide();
            tooltipCourceContent.show();

            $("#cource_name").text(tooltipData["NamePluralsightCourse"]);
            $("#cource_url").attr("href", tooltipData["UrlPluralsightCourse"]);
        }
    }

    // set the color of node or edge
    function setColor(o, c) {
        // don't change node an edge colors of undirected graphs
        if ('undirected' == visualizationProperties.type) return;
        o.attr.hl = true;
        o.attr.color = o.color;
        o.color = c;
    }

    function hex2dec(hexval) {
        return parseInt('0x' + hexval).toString(10)
    }

    // set the opacity of node or edge
    function setOpacity(o, alpha) {
        var r,g,b;
        var color = o.color;
        if (0 == color.indexOf('rgba')) {
            var m = color.match(/(\d+),(\d+),(\d+),(\d*.?\d+)/);
            if (m) {
              var colors = m.slice(1,5);
              r = colors[0];
              g = colors[1];
              b = colors[2];
            }
        } else if (0 == color.indexOf('rgb')) {
            var m = color.match(/(\d+),(\d+),(\d+)/);
            if (m) {
                var colors = m.slice(1,5);
                r = colors[0];
                g = colors[1];
                b = colors[2];
            }
        } else if (0 == color.indexOf('#')) {
              r = hex2dec(color.slice(1,3));
              g = hex2dec(color.slice(3,5));
              b = hex2dec(color.slice(5,7));
        }

        if (r && g && b) {
            o.color = 'rgba(' + r + ',' + g + ',' + b + ',' + alpha + ')';
        }
    }

    // called with array of ids of attributes to use as filters
    function getFilters(attrids) {
        sigmaInstance.iterNodes(function(n) {
            for (i in attrids) {
                var aname = attrids[i];
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
        return node.attr.attributes.hasOwnProperty(filterid) && -1 !== node.attr.attributes[filterid].indexOf(filterval)
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
        if (null !== activeFilterId
            && null !== activeFilterVal
            && !nodeHasFilter(node, activeFilterId, activeFilterVal)) {
            return true;
        }
        return false;
    }

    function resetNode(node, forceLabel) {
        node.hidden = 0;
        node.forceLabel = forceLabel;
        if (!node.label) node.label = node.attr.label;
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

        var sources = {},
            targets = {};
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
        document.getElementById(visualizationId).innerHTML = '';
    }

    function initSearch() {
        var labels = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: $.map(nodelabels, function(label) { return { value: label }; }),
            limit: 20
        });
        labels.initialize();
        var updater = function(event) {
            event.preventDefault();
            redirectHash(searchInput.val());
        };

        searchInput.typeahead({
              hint: true,
              highlight: true
          }, {
              name: 'languages',
              displayKey: 'value',
              source: labels.ttAdapter()
        }).on('typeahead:selected', updater);
        $('#highlight-node').on('submit', updater);

        if (document.location.hash) {
            redirectHash();
        }

        // search on hash change, unless it should trigger info or comments view
        $(window).bind('hashchange', function(event) {
            redirectHash();
        });
    }

    function redirectHash(q) {
        if (q) {
            document.location.hash = q;
            return;
        }
        var h = decodeURIComponent(document.location.hash.replace(/^#/, ''));
        nodeSearch(h);
    }

    function queryHasResult(q) {
        return -1 !== nodelabels.indexOf(q);
    }

    function nodeSearch(query) {
        resetFilter();
        if (queryHasResult(query)) {
            document.location.hash = query;
            searchInput.val(query);
            node = sigmaInstance.getNodes(nodemap[query])
            highlightNode(node);
            return query;
        }
    }

    function resetNodes() {
        sigmaInstance.iterNodes(function(n){
            if (n.attr.hl) {
                n.color = n.attr.color;
                n.attr.hl = false;
            }
            resetNode(n, 0);
            if (filteredOut(n)) {
                n.hidden = 1;
            }
        }).iterEdges(function(e){
          if (e.attr.hl) {
              e.color = e.attr.color;
              e.attr.hl = false;
          }
          e.hidden = 0;
        }).draw(2,2,2);
    }

    function resetSearch() {
        document.location.href = document.location.pathname;

        /*document.location.hash = "";
        sigmaInstance = null;

        $('#sig').remove(); 
        $('#vis').html('<div id="sig"></div>'); 

        init('sig', gexf, visualizationProperties, function() {
            var filterid = 'paradigms';
            var filters = getFilters([filterid]);
        });*/
    }

    function resetFilter() {
        activeFilterId = null;
        activeFilterVal = null;
        resetNodes();
    }

    function reset() {
        activeFilterId = null;
        activeFilterVal = null;
        searchInput.val('');
        resetNodes();
        dialog.hide();
    }

    return {
        init: init,
        getFilters: getFilters
    }
})();