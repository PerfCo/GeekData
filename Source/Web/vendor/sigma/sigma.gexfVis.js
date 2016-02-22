var visgexf = {
    visid: null,
    filename: null,
    sig: null,
    filters: {},
    graph: null,
    props: null,
    nodelabels: [],
    nodemap: {},
    searchInput: $('.typeahead'),
    activeFilterId: null,
    activeFilterVal: null,
    sourceColor: '#67A9CF',
    targetColor: '#EF8A62',

    tooltipElement: $("#tooltip"),
    tooltipLibContent: $("#lib_content"),
    tooltipGuruContent: $("#guru_content"),
    tooltipCourceContent: $("#cource_content"),
    tooltipGroupContent: $("#group_content"),
    tooltipLastTimeShown: new Date(),
    tooltipHideDelaySeconds: 3,

    init: function(visid, filename, props, callback) {
        $('#loading').show();
        visgexf.visid = visid;
        visgexf.filename = filename;
        visgexf.props = props;
        var viscontainer = document.getElementById(visid);
        // adjust height of graph to screen
        var h_win = $(window).height() - $('#navbar').height();
        var h_vis = $(viscontainer).height();
        if (h_win > 400) {
            $(viscontainer).height(h_win);
        }
        visgexf.sig = sigma.init(viscontainer)
            .drawingProperties(props['drawing'])
            .graphProperties(props['graph'])
            .mouseProperties({maxRatio: 128});

        visgexf.sig.parseJson(filename, function(){
            visgexf.sig.draw();
            // create array of node labels used for auto complete once
            if (0 == visgexf.nodelabels.length) {
                visgexf.sig.iterNodes(function(n){
                    visgexf.nodelabels.push(n.label);
                    visgexf.nodemap[n.label] = n.id;
                    n.attr.label = n.label;// needed for highlighting
                });
                visgexf.nodelabels.sort();
            }
            visgexf.initSearch();
            visgexf.searchInput.focus();
            // call callback after json is parsed
            if (callback) callback();
            $('#loading').hide();
        });

        visgexf.initTooltip();

        visgexf.sig.bind('upnodes', function(event) {
            // on node click
            hnode = visgexf.sig.getNodes(event.content)[0];
            if(document.location.hash.replace(/#/i, '') == hnode.attr.label) {
                visgexf.resetSearch();
                return;
            }
            document.location.hash = hnode.attr.label;
        });

        visgexf.sig.bind('overnodes', function(event) {
            // on node hover by mouse
            var nodeData = visgexf.sig.getNodes(event.content)[0];
            var tooltipData = nodeData.attr.attributes;

            if(tooltipData.Level == 1) {
                return;
            }

            visgexf.showTooltip(nodeData, tooltipData);
        });

        visgexf.sig.bind('outnodes', function(event) {
            // on mouse out of node

            /*var $tooltip = visgexf.tooltipElement;
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
                visgexf.resetSearch();
            }
            
            return false;
        }

        return visgexf;
    },

    initTooltip: function() {
        /*var $tooltip = visgexf.tooltipElement;

        $tooltip.hover(function() {
            $("#tooltip").fadeIn("fast");
        }).mouseleave(function() {
            $tooltip.fadeOut(1000);
        });*/
    },

    showTooltip: function(nodeData, tooltipData) {
        var $tooltip = visgexf.tooltipElement;

        $tooltip;

        var typeMondatoryField = {
            lib: "HtmlUrlGithubRepository",
            geek: "DisplayNameStackOverflowUser",
            cource: "NamePluralsightCourse"
        };

        var tooltipPosition = getTooltipPosition(nodeData);
        
        if(tooltipData.Level == 2) {
            initGroupTooltip();
        } else if(tooltipData[typeMondatoryField.geek]) {
            initPersonTooltip();
        } else if(tooltipData[typeMondatoryField.lib]) {
            initLibTooltip();
        } else if(tooltipData[typeMondatoryField.cource]) {
            initCourseTooltip();
        }

        $tooltip.css({
            top: tooltipPosition.y, 
            left: tooltipPosition.x,
            opacity: 1
        });

        $tooltip.delay(1000).show();
        visgexf.tooltipLastTimeShown = new Date();

        setTimeout(onTooltipTimeout, visgexf.tooltipHideDelaySeconds * 1000);

        function onTooltipTimeout() {
            var nowDate = new Date();
            var diffSeconds = (nowDate.getTime() - visgexf.tooltipLastTimeShown.getTime()) / 1000;
            
            if(diffSeconds >= visgexf.tooltipHideDelaySeconds) {
                $tooltip.hide();
                return;
            }

            setTimeout(onTooltipTimeout, visgexf.tooltipHideDelaySeconds * 1000);
        }

        function getTooltipPosition(nodeData) {
            var $window = $(window);
            var $tooltip = visgexf.tooltipElement;

            var winWidth = $window.width();
            var winHeight = $window.height();

            var tooltipWidth = $tooltip.width();
            var tooltipHeight = $tooltip.height();

            var marginX = 10;
            var marginY = 50;

            var x = nodeData.displayX + tooltipWidth >= winWidth ? 
                (winWidth - tooltipWidth - marginX) : nodeData.displayX;

            var y = nodeData.displayY + marginY + tooltipHeight >= winHeight ? 
                (nodeData.displayY - 2 * marginY) : nodeData.displayY + marginY;

            return {
                x: x,
                y: y
            };
        }

        function initPersonTooltip() {
            var $tooltip = visgexf.tooltipElement;

            visgexf.tooltipLibContent.hide();
            visgexf.tooltipCourceContent.hide();
            visgexf.tooltipGroupContent.hide();
            visgexf.tooltipGuruContent.show();

            $("#guru_name")
                .text(tooltipData["DisplayNameStackOverflowUser"])
                .attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

            $("#guru_profile_url").attr("href", tooltipData["ProfileUrlStackOverflowUser"]);
            $("#guru_avatar").attr("src", tooltipData["ProfileImageStackOverflowUser"]);

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

            /*"attributes" : {
                "ProfileImageStackOverflowUser" : " https://www.gravatar.com/avatar/7deca8ec973c3c0875e9a36e1e3e2c44?s\u003d128\u0026d\u003didenticon\u0026r\u003dPG",
                "ProfileUrlStackOverflowUser" : " http://stackoverflow.com/users/34397/slaks",                
                "TagsStackOverflowUser" : " C#",                
                "AccountIdStackOverflowUser" : " 15988",                
                "DisplayNameStackOverflowUser" : " SLaks",
                "BadgeCountsStackOverflowUser" : " Bronze: 1422, Gold: 81, Silver: 1216"
            },*/
        }

        function initLibTooltip() {
            var $tooltip = visgexf.tooltipElement;

            visgexf.tooltipGuruContent.hide();
            visgexf.tooltipCourceContent.hide();
            visgexf.tooltipGroupContent.hide();
            visgexf.tooltipLibContent.show();

            var libUrl = tooltipData["HtmlUrlGithubRepository"];
            var libUrlParts = libUrl.split("/");
            var libName = libUrlParts[libUrlParts.length - 1];
            
            $("#lib_name").text(libName);
            $("#lib_url").attr("href", libUrl);
            $("#lib_description").text(tooltipData["DescriptionGithubRepository"]);
            $("#lib_stars_count").text(tooltipData["StargazersCountGithubRepository"]);
            
            /*"attributes" : {                
                "TagsGithubRepository" : " C#",                
                "DescriptionGithubRepository" : " Polly is a .NET 3.5 / 4.0 / 4.5 / PCL library that allows developers to express transient exception handling policies such as Retry, Retry Forever, Wait and Retry or Circuit Breaker in a fluent manner.",
                "StargazersCountGithubRepository" : " 1279",                
                "HtmlUrlGithubRepository" : " https://github.com/App-vNext/Polly",                
            },*/
        }

        function initCourseTooltip() {
            visgexf.tooltipGuruContent.hide();
            visgexf.tooltipLibContent.hide();
            visgexf.tooltipGroupContent.hide();
            visgexf.tooltipCourceContent.show();

            $("#cource_name").text(tooltipData["NamePluralsightCourse"]);
            $("#cource_url").attr("href", tooltipData["UrlPluralsightCourse"]);

            /*
            "attributes" : {                
                "UrlPluralsightCourse" : " http://pluralsight.com/training/Courses/TableOfContents/skeet-async",
                "NamePluralsightCourse" : "Asynchronous C# 5.0",
                "TagsPluralsightCourse" : " C#",
            },
            */
        } // group_name

        function initGroupTooltip() {
            visgexf.tooltipGuruContent.hide();
            visgexf.tooltipLibContent.hide();
            visgexf.tooltipCourceContent.hide();
            visgexf.tooltipGroupContent.show();

            $("#group_name").text(nodeData.label);
        }
    },

    // set the color of node or edge
    setColor: function(o, c) {
        // don't change node an edge colors of undirected graphs
        if ('undirected' == visgexf.props.type) return;
        o.attr.hl = true;
        o.attr.color = o.color;
        o.color = c;
    },

    hex2dec: function(hexval) {
        return parseInt('0x' + hexval).toString(10)
    },

    // set the opacity of node or edge
    setOpacity: function(o, alpha) {
        var r,g,b;
        var color = o.color;
        // is it a hex color
        if (0 == color.indexOf('#')) {
            r = visgexf.hex2dec(color.slice(1,3));
            g = visgexf.hex2dec(color.slice(3,5));
            b = visgexf.hex2dec(color.slice(5,7));
        }
        else if (0 == color.indexOf('rgba')) {
            var m = color.match(/(\d+),(\d+),(\d+),(\d*.?\d+)/);
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
    },

    // called with array of ids of attributes to use as filters
    getFilters: function(attrids) {
        visgexf.sig.iterNodes(function(n) {
            for (i in attrids) {
                var aname = attrids[i];
                if (n.attr.attributes.hasOwnProperty(aname)) {
                    var vals = n.attr.attributes[aname].split('|');
                    for (v in vals) {
                        val = vals[v];
                        if (!visgexf.filters.hasOwnProperty(val)) {
                            visgexf.filters[val] = 0;
                        }
                        visgexf.filters[val]++;
                    }
                }
            }
        });
        // sort by frequencies of filter attributes
        var sorted = [];
        for (var a in visgexf.filters) {
            sorted.push([a, visgexf.filters[a]]);
        }
        sorted.sort(function(a, b) { return b[1] - a[1] });
        return sorted;
    },

    nodeHasFilter: function(node, filterid, filterval) {
        return node.attr.attributes.hasOwnProperty(filterid) && -1 !== node.attr.attributes[filterid].indexOf(filterval)
    },

    // show only nodes that match filter
    setFilter: function(filterid, filterval) {
        visgexf.activeFilterId = filterid;
        visgexf.activeFilterVal = filterval;
        visgexf.sig.iterNodes(function(n){
            n.hidden = filterval ? 1 : 0;
            if (visgexf.nodeHasFilter(n, filterid, filterval)) {
                n.hidden = 0;
            }
        }).draw(2,2,2);
    },

    // return true if given node does not satisfy set filter, else false
    filteredOut: function(node) {
      if (null !== visgexf.activeFilterId
          && null !== visgexf.activeFilterVal
          && !visgexf.nodeHasFilter(node, visgexf.activeFilterId, visgexf.activeFilterVal)) {
          return true;
      }
      return false;
    },

    resetNode: function(node, forceLabel) {
        node.hidden = 0;
        node.forceLabel = forceLabel;
        if (!node.label) node.label = node.attr.label;
        visgexf.setOpacity(node, 1);
    },

    // show node with optional color, check if it satisfies possibly set filter
    nodeShow: function(node, color) {
      if (visgexf.filteredOut(node)) { 
          return; 
      }
      if (color) { 
          visgexf.setColor(node, color);
      }
      visgexf.resetNode(node, 0);
    },

    highlightNode: function(node) {
        visgexf.sig.position(0,0,1);
        visgexf.sig.goTo(node.displayX, node.displayY, 2);
        visgexf.sig.position(0,0,1);

        var sources = {},
            targets = {};
        visgexf.sig.iterEdges(function(e){
            if (e.source != node.id && e.target != node.id) {
                e.hidden = 1;
            } else if (e.source == node.id) {
                targets[e.target] = true;
                visgexf.setColor(e, visgexf.sourceColor);
                e.hidden = 0;
            } else if (e.target == node.id) {
                visgexf.setColor(e, visgexf.targetColor);
                sources[e.source] = true;
                e.hidden = 0;
            }
        }).iterNodes(function(n){
            if (n.id == node.id) {
                visgexf.nodeShow(n);
            } else if (sources[n.id]) {
                visgexf.nodeShow(n, visgexf.targetColor);
            } else if (targets[n.id]) {
                visgexf.nodeShow(n, visgexf.sourceColor);
            } else {
                visgexf.setOpacity(n, .05);
                n.label = null;
            }
        }).draw(2,2,2);
    },

    clear: function() {
        visgexf.sig.emptyGraph();
        document.getElementById(visgexf.visid).innerHTML = '';
    },

    initSearch: function() {
        var labels = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: $.map(visgexf.nodelabels, function(label) { return { value: label }; }),
            limit: 20
        });
        labels.initialize();
        var updater = function(event) {
            event.preventDefault();
            visgexf.redirectHash(visgexf.searchInput.val());
        };

        visgexf.searchInput.typeahead({
              hint: true,
              highlight: true
          }, {
              name: 'languages',
              displayKey: 'value',
              source: labels.ttAdapter()
        }).on('typeahead:selected', updater);
        $('#highlight-node').on('submit', updater);

        if (document.location.hash) {
            visgexf.redirectHash();
        }

        // search on hash change, unless it should trigger info or comments view
        $(window).bind('hashchange', function(event) {
            visgexf.redirectHash();
        });
    },

    redirectHash: function(q) {
        if (q) {
            document.location.hash = q;
            return;
        }
        var h = decodeURIComponent(document.location.hash.replace(/^#/, ''));
        visgexf.nodeSearch(h);
    },

    queryHasResult: function(q) {
        return -1 !== visgexf.nodelabels.indexOf(q);
    },

    nodeSearch: function(query) {
        visgexf.resetFilter();
        if (visgexf.queryHasResult(query)) {
            document.location.hash = query;
            visgexf.searchInput.val(query);
            node = visgexf.sig.getNodes(visgexf.nodemap[query])
            visgexf.highlightNode(node);
            return query;
        }
    },

    resetNodes: function() {
        visgexf.sig.iterNodes(function(n){
            if (n.attr.hl) {
                n.color = n.attr.color;
                n.attr.hl = false;
            }
            visgexf.resetNode(n, 0);
            if (visgexf.filteredOut(n)) {
                n.hidden = 1;
            }
        }).iterEdges(function(e){
          if (e.attr.hl) {
              e.color = e.attr.color;
              e.attr.hl = false;
          }
          e.hidden = 0;
        }).draw(2,2,2);
    },

    resetSearch: function() {
        document.location.href = document.location.pathname;

        /*document.location.hash = "";
        visgexf.sig = null;

        $('#sig').remove(); 
        $('#vis').html('<div id="sig"></div>'); 

        visgexf.init('sig', gexf, visgexf.props, function() {
            var filterid = 'paradigms';
            var filters = visgexf.getFilters([filterid]);
        });*/
    },

    resetFilter: function() {
        visgexf.activeFilterId = null;
        visgexf.activeFilterVal = null;
        //$('.graphfilter li').removeClass('active');
        visgexf.resetNodes();
    },

    reset: function() {
        visgexf.activeFilterId = null;
        visgexf.activeFilterVal = null;
        visgexf.searchInput.val('');
        //$('.graphfilter li').removeClass('active');
        visgexf.resetNodes();
        dialog.hide();
    }
};