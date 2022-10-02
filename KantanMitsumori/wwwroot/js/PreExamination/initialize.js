// 蜈ｱ騾壹げ繝ｭ繝ｼ繝舌Ν螳壽焚險ｭ螳�
var settings = {
    context : "/autoflat",
    servletMapping : "/app",
    applicationLevel : "/pc",
    commonPath : "/cms/pc/common/",
    apiContext : "/autoflat_api",
    apiServ : "/api",
    logServ : "/log",
    trackingOn : "trackingOn",
    headerHeight : 150
};

$(function() {
    if ($.browser.msie && $.browser.version <= 7) {
        $('body').prepend('<div class="error">縺泌茜逕ｨ縺�◆縺�縺�※縺�ｋ繝悶Λ繧ｦ繧ｶ縺ｫ縺ｯ蟇ｾ蠢懊＠縺ｦ縺�↑縺�◆繧√∝虚菴懊＠縺ｪ縺�＄繧後′縺斐＊縺�∪縺吶�</div>');
    }

    if (navigator.cookieEnabled == false) {
        location.href = settings.context + setting.servletMapping + "/error/no_support/";
    }
});

var images = {
    commodityNotFound : settings.context + "/cms" + settings.applicationLevel + "/images/CommodityNotFound.jpg",
    noPhotoCampaign : settings.context + "/cms" + settings.applicationLevel + "/images/NoPhoto_campaign.gif",
    noPhotoBrand : settings.context + "/cms" + settings.applicationLevel + "/images/NoPhoto_brand.gif",
    noPhotoCommodity : settings.context + "/cms" + settings.applicationLevel + "/images/NoPhoto_commodity.jpg",
    noPhotoOther : settings.context + "/cms" + settings.applicationLevel + "/images/NoPhoto_other.jpg",
    noPhotoGift : settings.context + "/cms" + settings.applicationLevel + "/images/NoPhoto_gift.jpg",
    smallLoading : settings.context + "/cms" + settings.applicationLevel + "/images/loading-small.gif",
    circleLoading : settings.context + "/cms" + settings.applicationLevel + "/images/loading_circle.gif"
};

var userAgent = window.navigator.userAgent.toLowerCase();
var appVersion = window.navigator.appVersion.toLowerCase();
if (userAgent.indexOf("msie") != -1) {
    if (appVersion.indexOf("msie 8.") != -1) {
        document.write('<meta http-equiv="X-UA-Compatible" content="IE=8" />');
    } else if (appVersion.indexOf("msie 9.") != -1) {
        document.write('<meta http-equiv="X-UA-Compatible" content="IE=9" />');
    } else if (appVersion.indexOf("msie 10.") != -1) {
        document.write('<meta http-equiv="X-UA-Compatible" content="IE=10" />');
    }
}
// 蜈ｱ騾壹せ繧ｯ繝ｪ繝励ヨ隱ｭ縺ｿ霎ｼ縺ｿ
document.write('<!--[if IE]>    <script src="//html5shiv.googlecode.com/svn/trunk/html5.js"></script>   <![endif]-->');
document.write('<script type="text/javascript" src="' + settings.context + '/cms' + settings.applicationLevel + '/scripts/jquery.lightbox_me.js" charset="UTF-8"></script>');
document.write('<script type="text/javascript" src="' + settings.context + '/cms/system/scripts/initialize.js"></script>');

/**
 * 逕ｻ髱｢驕ｷ遘ｻ逕ｨ
 */

// 繝医ャ繝励�繝ｼ繧ｸ驕ｷ遘ｻ
function moveTopPage() {
    if ($("#limitedLogin").val() == "true") {
      location.href = settings.context + "/app/common/limited_index/";
    } else {
      location.href = "https://www.idemitsu-autoflat.com/";
    }
};

// 蜷�判髱｢驕ｷ遘ｻ
function move(url) {
    location.href = settings.context + settings.servletMapping + url;
}

/**
 * PC逕ｨ繝昴ャ繝励い繝��陦ｨ遉ｺ
 */
var popup = function(id, options) {
    var $element = $("#" + id);
    var wx, wy; // 繧ｦ繧､繝ｳ繝峨え縺ｮ蟾ｦ荳雁ｺｧ讓�

    // 繧ｦ繧､繝ｳ繝峨え縺ｮ蠎ｧ讓吶ｒ逕ｻ髱｢荳ｭ螟ｮ縺ｫ縺吶ｋ縲�
    wx = $(window).width() / 2;
    if (wx < 0)
        wx = 0;
    wy = $(window).height() / 2 - ($element.height() / 2);
    if (wy < 0) {
        wy = $(window).height() / 2;
    }

    var opts = $.extend({}, {
        onLoad : function() {
        },
        onClose : function() {
        },
        centered : false,
        modalCSS : {
            top : wy,
            left : wx
        }
    }, options);

    $element.lightbox_me(opts);

    var $header = $($element.find(".frameHeader"));
    $header.css({
        cursor : "move"
    });
    $header.unbind("mousedown");
    $header.mousedown({
        mainid : id
    }, function(e) {
        var mx = e.pageX;
        var my = e.pageY;
        var mainid = e.data.mainid;
        $(document).on('mousemove.' + mainid, function(e) {
            wx += e.pageX - mx;
            wy += e.pageY - my;
            $('#' + mainid).css({
                top : wy,
                left : wx
            });
            mx = e.pageX;
            my = e.pageY;
            return false;
        }).one('mouseup', function(e) {
            $(document).off('mousemove.' + mainid);
        });
        return false;
    });
};