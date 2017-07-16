App.tooltip = (function($) {

    var tooltipSelector = "#tooltip";

    // cache elements for faster work
    var $tooltip = $(tooltipSelector);
    var $libContent = $("#lib_content");
    var $geekContent = $("#geek_content");
    var $courseContent = $("#course_content");

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

    var $courseName = $("#course_name");
    var $courseUrl = $("#course_url");


    var lastTimeShown = getNowTime();
    var hideDelaySeconds = 3;

    var millisecondsInSecond = 1000;
    var noAvatarPath = "images/no-avatar.jpg";

    var geekAttributeNames = {
        displayName: "DisplayNameStackOverflowUser",
        profileUrl: "ProfileUrlStackOverflowUser",
        profileImage: "ProfileImageStackOverflowUser",
        badgeCounts: "BadgeCountsStackOverflowUser",
        goldBadge: "Gold",
        silverBadge: "Silver",
        bronzeBadge: "Bronze",
    };

    var libAttributeNames = {
        url: "HtmlUrlGithubRepository",
        description: "DescriptionGithubRepository",
        starCount: "StargazersCountGithubRepository"
    };

    var courseAttributeNames = {
        name: "NamePluralsightCourse",
        url: "UrlPluralsightCourse"
    };

    var nodePositionAttributeNames = {
        x: "renderer1:x",
        y: "renderer1:y"
    };

    var requiredAttribute = {
        lib: libAttributeNames.url,
        geek: geekAttributeNames.displayName,
        course: courseAttributeNames.name
    };

    // show noAvatarPath if there is no avatar
    $geekAvatar.on("error", function() {
        $geekAvatar.attr("src", noAvatarPath);
    });

    function show(nodeData){
        initContent(nodeData);
        setPosition(nodeData);
        doShow();
    }

    function initContent(nodeData){
        var tooltipData = nodeData.attributes;

        if(tooltipData[requiredAttribute.geek]) {
            initGeekContent(tooltipData);
            return;
        }

        if(tooltipData[requiredAttribute.lib]) {
            initLibContent(nodeData);
            return;
        }

        if(tooltipData[requiredAttribute.course]) {
            initCourseContent(tooltipData);
            return;
        }
    }

    function initGeekContent(tooltipData) {
        hideAllContent();
        
        $geekName.html(tooltipData[geekAttributeNames.displayName])
            .attr("href", tooltipData[geekAttributeNames.profileUrl]);

        $geekProfileUrl.attr("href", tooltipData[geekAttributeNames.profileUrl]);

        var avatarUrl = tooltipData[geekAttributeNames.profileImage];

        if(avatarUrl) {
            $geekAvatar.attr("src", avatarUrl);
        } else {
            $geekAvatar.attr("src", noAvatarPath);
        }

        $geekSite.attr("href", tooltipData[geekAttributeNames.profileUrl]);

        var badgesCount = getBadgesCount(tooltipData);
        var countSelector = ".badgecount";

        if(badgesCount.gold) {
            $goldBadge.show().find(countSelector).text(badgesCount.gold);
        } else {
            $goldBadge.hide();
        }

        if(badgesCount.silver) {
            $silverBadge.show().find(countSelector).text(badgesCount.silver);
        } else {
            $silverBadge.hide();
        }

        if(badgesCount.bronze) {
            $bronzeBadge.show().find(countSelector).text(badgesCount.bronze);
        } else {
            $bronzeBadge.hide();
        }

        $geekContent.show();

        function getBadgesCount(tooltipData) {
            var badges = {};
            var badgesRawData = tooltipData[geekAttributeNames.badgeCounts];
            if(badgesRawData) {
                // TODO: make valid json on server
                badgesRawData = badgesRawData.replace(/([a-zA-Z][^:]*)(?=\s*:)/g, '"$1"'); // add quotes to make valid json
                badges = JSON.parse(badgesRawData);
            }

            var goldBadgesCount = badges[geekAttributeNames.goldBadge];
            var silverBadgesCount = badges[geekAttributeNames.silverBadge];
            var bronzeBadgesCount = badges[geekAttributeNames.bronzeBadge];

            return {
                gold: goldBadgesCount,
                silver: silverBadgesCount,
                bronze: bronzeBadgesCount
            };
        }
    }

    function initLibContent(nodeData) {
        var tooltipData = nodeData.attributes;

        hideAllContent();

        $libName.html(nodeData.label);
        $libUrl.attr("href", tooltipData[libAttributeNames.url]);
        $libDescription.html(tooltipData[libAttributeNames.description]);
        $libStarsCount.text(tooltipData[libAttributeNames.starCount]);

        $libContent.show();
    }

    function initCourseContent(tooltipData) {
        hideAllContent();

        $courseName.html(tooltipData[courseAttributeNames.name]);
        $courseUrl.attr("href", tooltipData[courseAttributeNames.url]);

        $courseContent.show();
    }

    function hideAllContent() {
        $geekContent.hide();
        $libContent.hide();
        $courseContent.hide();
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
        var $tooltip = $(tooltipSelector);

        var winWidth = $window.width();
        var winHeight = $window.height();

        var tooltipWidth = $tooltip.outerWidth();
        var tooltipHeight = $tooltip.outerHeight();

        var marginX = 10;
        var marginY = 50;
        var extraY = 30; // needed if tooltip overflows window height
        
        var nodeX = nodeData[nodePositionAttributeNames.x];
        var nodeY = nodeData[nodePositionAttributeNames.y];

        var isOverflowByX = nodeX + tooltipWidth >= winWidth;
        var isOverflowByY = nodeY + marginY + tooltipHeight >= winHeight;

        var x = isOverflowByX ? 
            (winWidth - tooltipWidth - marginX) : 
            nodeX;

        var y = isOverflowByY ? 
            (nodeY - tooltipHeight + extraY) : 
            (nodeY + marginY);

        return {
            x: x,
            y: y
        };
    }

    function doShow() {
        $tooltip.delay(1 * millisecondsInSecond).show();
        lastTimeShown = getNowTime();
        setHideTimeout();
    }

    function setHideTimeout() {
        setTimeout(onTooltipTimeout, hideDelaySeconds * millisecondsInSecond);
    }

    function onTooltipTimeout() {
        var nowDate = getNowTime();
        var diffSeconds = (nowDate - lastTimeShown) / millisecondsInSecond;
        
        if(diffSeconds >= hideDelaySeconds) {
            $tooltip.hide();
            return;
        }

        setHideTimeout();
    }

    function getNowTime() {
        var nowDate = new Date();
        return nowDate.getTime();
    }

    return {
        show: show
    };

})(jQuery);