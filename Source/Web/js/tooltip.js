App.tooltip = (function($) {

    var $tooltip = $("#tooltip");
    var $libContent = $("#lib_content");
    var $geekContent = $("#geek_content");
    var $courceContent = $("#cource_content");

    var $geekName = $("#geek_name");
    var $geekProfileUrl = $("#geek_profile_url");
    var $geekAvatar = $("#geek_avatar");
    var $geekSite = $("#geek_site");
    var $goldBadge = $("#geek_badges_gold");
    var $silverBadge = $("#geek_badges_silver");
    var $bronzeBadge = $("#geek_badges_bronze");

    var $libName = $("#lib_name");
    var $libUrl = $("#lib_url");
    var $libDescription = $("#lib_description");
    var $libStarsCount = $("#lib_stars_count");

    var $courceName = $("#cource_name");
    var $courceUrl = $("#cource_url");

    var lastTimeShown = getNowTime();
    var hideDelaySeconds = 3;
    var noAvatarPath = "images/no-avatar.jpg";

    var requiredAttribute = {
        lib: "HtmlUrlGithubRepository",
        geek: "DisplayNameStackOverflowUser",
        cource: "NamePluralsightCourse"
    };

    $geekAvatar.on("error", function() {
        $geekAvatar.attr("src", noAvatarPath);
    });

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
        hideAllContent();

        $geekName.html(tooltipData["DisplayNameStackOverflowUser"])
            .attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

        $geekProfileUrl.attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

        var avatarUrl = tooltipData["ProfileImageStackOverflowUser"];

        if(avatarUrl) {
            $geekAvatar.attr("src", avatarUrl);
        } else {
            $geekAvatar.attr("src", noAvatarPath);
        }

        $geekSite.attr("href", tooltipData["ProfileUrlStackOverflowUser"]);

        var badges = {};
        var badgesRawData = tooltipData["BadgeCountsStackOverflowUser"];
        if(badgesRawData) {
            badgesRawData = badgesRawData.replace(/([a-zA-Z][^:]*)(?=\s*:)/g, '"$1"'); // add quotes to make valid json
            badges = JSON.parse(badgesRawData);
        }

        var $goldBadge = $("#geek_badges_gold");
        var $silverBadge = $("#geek_badges_silver");
        var $bronzeBadge = $("#geek_badges_bronze");

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

        $geekContent.show();
    }

    function initLibContent(nodeData) {
        var tooltipData = nodeData.attr.attributes;

        hideAllContent();

        $libName.html(nodeData.label);
        $libUrl.attr("href", tooltipData["HtmlUrlGithubRepository"]);
        $libDescription.html(tooltipData["DescriptionGithubRepository"]);
        $libStarsCount.text(tooltipData["StargazersCountGithubRepository"]);

        $libContent.show();
    }

    function initCourceContent(tooltipData) {
        hideAllContent();

        $courceName.html(tooltipData["NamePluralsightCourse"]);
        $courceUrl.attr("href", tooltipData["UrlPluralsightCourse"]);

        $courceContent.show();
    }

    function hideAllContent() {
        $geekContent.hide();
        $libContent.hide();
        $courceContent.hide();
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
