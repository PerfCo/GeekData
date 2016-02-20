var langinfo = function(hlang) {
    var influenced = [],
        influencedby = [],
        desc = '';

    if ('undefined' !== typeof hlang.attr.attributes.influenced) {
        hlang.attr.attributes.influenced.split('|').forEach(function(i){
            influenced.push('<a href="#' + i + '">' + i + '</a>');
        });
    }
    if ('undefined' !== typeof hlang.attr.attributes.influencedby) {
        hlang.attr.attributes.influencedby.split('|').forEach(function(i){
            influencedby.push('<a href="#' + i + '">' + i + '</a>');
        });
    }      

    if (influenced.length) {
        desc += '<h4>Languages Influenced</h4><p>' + influenced.join(', ') + '</p>';
    }

    if (influencedby.length) {
        desc += '<h4>Influenced by</h4><p>' + influencedby.join(', ') + '</p>';
    }

    nodeinfo(hlang.label, desc);
};

var randomNodeColor = function(num) {
    var color = '#bfbab7';
    if (num > 40) color = '#006D2C';
    else if (num > 30) color = '#31A354';
    else if (num > 20) color = '#74C476';
    else if (num > 0) color = '#BAE4B3';
    return color;
};

var nodeClick = function(Graph) {
    Graph.sig.bind('upnodes', function(event){
        hlang = Graph.sig.getNodes(event.content)[0];
        langinfo(hlang);
    });
};

$(function(){
    var props = {
        drawing: {
            defaultLabelColor: '#fff',
            defaultLabelSize: 12,
            defaultLabelBGColor: '#fff',
            defaultLabelHoverColor: '#000',
            labelThreshold: 3,
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

    visgexf.init('sig', gexf, props, function() {
        var filterid = 'paradigms';
        var filters = visgexf.getFilters([filterid]);
        nodeClick(visgexf);
    });
});