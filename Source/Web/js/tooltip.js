App.tooltip = (function($) {

    var $tooltip = $("#tooltip");
    var $libContent = $("#lib_content");
    var $guruContent = $("#guru_content");
    var $courceContent = $("#cource_content");
    var lastTimeShown = getNowTime();
    var hideDelaySeconds = 3;

    var requiredAttribute = {
        lib: "HtmlUrlGithubRepository",
        geek: "DisplayNameStackOverflowUser",
        cource: "NamePluralsightCourse"
    };

    function show(nodeData){
        initContent(nodeData);
        setPosition(nodeData);
        doShow();
    }
      
    function initContent(nodeData){
        var tooltipData = nodeData.attr.attributes;
        
        if(tooltipData[requiredAttribute.geek]) {
            initGeekContent(tooltipData);
            return;
        }

        if(tooltipData[requiredAttribute.lib]) {
            initLibContent(nodeData);
            return;
        }

        if(tooltipData[requiredAttribute.cource]) {
            initCourceContent(tooltipData);
            return;
        }
    }

    function initGeekContent(tooltipData) {
        $libContent.hide();
        $courceContent.hide();
        $guruContent.show();

        $("#guru_name")
            .text(tooltipData["DisplayNameStackOverflowUser"])
            .attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

        $("#guru_profile_url").attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

        var avatarUrl = tooltipData["ProfileImageStackOverflowUser"];

        if(avatarUrl && avatarUrl.indexOf("gravatar.com/avatar/") < 0) {
            $("#guru_avatar").attr("src", avatarUrl);
        } else {
            $("#guru_avatar").attr("src", "images/no-avatar.jpg");
        }

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

    function initLibContent(nodeData) {
        var tooltipData = nodeData.attr.attributes;
        
        $guruContent.hide();
        $courceContent.hide();
        $libContent.show();
        
        $("#lib_name").text(nodeData.label);
        $("#lib_url").attr("href", tooltipData["HtmlUrlGithubRepository"]);
        $("#lib_description").text(tooltipData["DescriptionGithubRepository"]);
        $("#lib_stars_count").text(tooltipData["StargazersCountGithubRepository"]);
    }

    function initCourceContent(tooltipData) {
        $guruContent.hide();
        $libContent.hide();
        $courceContent.show();

        $("#cource_name").text(tooltipData["NamePluralsightCourse"]);
        $("#cource_url").attr("href", tooltipData["UrlPluralsightCourse"]);
    }

    function setPosition(nodeData){
        var coordinates = findCoordinates(nodeData);

        $tooltip.css({
            left: coordinates.x,
            top: coordinates.y,
            opacity: 1
        });
    }

    function findCoordinates(nodeData){
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

        return {
            x: x,
            y: y
        };
    }

    function doShow() {
        $tooltip.delay(1000).show();
        lastTimeShown = getNowTime();

        setTimeout(onTooltipTimeout, hideDelaySeconds * 1000);
    }

    function onTooltipTimeout() {
        var nowDate = getNowTime();
        var diffSeconds = (nowDate - lastTimeShown) / 1000;
        
        if(diffSeconds >= hideDelaySeconds) {
            $tooltip.hide();
            return;
        }

        setTimeout(onTooltipTimeout, hideDelaySeconds * 1000);
    }

    function getNowTime() {
        var nowDate = new Date();
        return nowDate.getTime();
    }

    return {
        show: show
    };

})(jQuery);
