(function(win, doc) {
    function initVisualization() {
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

        App.dataVisualization.init(options);
    }

    $(function(){
        initVisualization();
    });
}(window, document));