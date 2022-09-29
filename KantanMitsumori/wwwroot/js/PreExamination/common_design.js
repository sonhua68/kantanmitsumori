// JavaScript Document

$(window).scroll(function () {
  var st = $(window).scrollTop();
  /* ==========
  繝倥ャ繝繝ｼ霑ｽ蠕�
  ========== */
  if( window.innerWidth >= 769 ) {
    //PC繝倥ャ繝繝ｼ霑ｽ蠕�
    if( st > 1 ) {
      $(".header").addClass("headerFloating");
    } else if( st <= 0 ) {
      //繝倥ャ繝繝ｼ霑ｽ蠕鍋ｵゆｺ�
      $(".header").removeClass("headerFloating");
    }
  } else if( $(".header").hasClass("js-floating") ) {
    //SP繝倥ャ繝繝ｼ霑ｽ蠕�
    var showPoint = $(".header").hasClass("js-floatingPortal") ? $(".topNavWrap").offset().top + $(".topNavWrap").height() - 40 : $(".header").outerHeight();

    if( $(".header").hasClass("js-floatingPortal") && st > showPoint ) {
      $(".headerFloating").css("top", 0);
    } else if( $(".header").hasClass("js-floatingPortal") == false && st > $("header").height() ) {
      $(".headerFloating").css("top", 0);
    } else if( st <= $("header").height() && !$("html").hasClass("js-menuOpened") ) {
      //繝倥ャ繝繝ｼ霑ｽ蠕鍋ｵゆｺ�
      $(".headerFloating").removeAttr("style");
    }
  }



  /* ==========
  繝壹�繧ｸ繝医ャ繝苓ｿｽ蠕難ｼ�PC縺ｮ縺ｿ��
  ========== */
  if ( $(".pagetop").length > 0 ) {
    if( window.innerWidth >= 769 ) {
      var posR = $(window).width() > 1200 ? ($(window).width() - 1200) / 2 : 20, //蜿ｳ縺九ｉ縺ｮ菴咲ｽｮ
          endPoint = $(window).scrollTop() + $(window).height() - 110; //蝗ｺ螳壹☆繧倶ｽ咲ｽｮ

      if( endPoint >= $(".pagetop").offset().top ) {
        //蝗ｺ螳壹☆繧倶ｽ咲ｽｮ縺ｫ縺阪◆繧鋭tyle繧帝勁蜴ｻ縺吶ｋ
        $(".js-pcPagetop").removeAttr("style");
      } else if( st >= 300 ) {
        //繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縺御ｸ螳夐㍼莉･荳翫↓縺ｪ縺｣縺溘ｉfixed陦ｨ遉ｺ縺吶ｋ
        $(".js-pcPagetop").fadeIn().css({
          position: "fixed",
          bottom: "55px",
          right: posR,
        });
      } else if( st < 300 ) {
        //繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縺御ｸ螳夐㍼譛ｪ貅縺ｫ縺ｪ縺｣縺溘ｉ髱櫁｡ｨ遉ｺ縺ｫ縺吶ｋ
        $(".js-pcPagetop").fadeOut();
      }
    }
  }



});
$(function(){
  /* ==========
  繝倥ャ繝繝ｼ霑ｽ蠕灘�譛溯ｨｭ螳�
  ========== */
  createHeader();

  /* ==========
  繧ｹ繝�繝ｼ繧ｺ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ
  ========== */
  $('.js-scroll').click(function(){
    var speed = 400;
    var href= $(this).attr("href");
    var target = $(href == "#" || href == "" ? 'html' : href);
    var position = $(".headerFloating").length > 0 ? target.offset().top - $(".headerFloating").height() - 40 : target.offset().top - $(".header").height() - 20;
    $("html, body").animate({scrollTop:position}, speed, "swing");
    return false;
  });

  /* ==========
  繝｡繝九Η繝ｼ蜃ｦ逅�
  ========== */
  //繝｡繝九Η繝ｼ繧帝幕縺�
  $(document).on("click", ".js-menu", function() {
    var lPos = $(window).width() > 768 ? window.innerWidth - 320 : "22vw";
    //繝｡繝九Η繝ｼ繧定｡ｨ遉ｺ
    $(".js-menuPanel").css({
      left: lPos,
    });
    $(".js-menuClose").show();

    //繝翫ン縺ｨ縺雁撫縺�粋繧上○荳｡譁ｹ縺檎判髱｢蜀�↓蜈･繧峨↑縺��ｴ蜷医√Γ繝九Η繝ｼ驛ｨ蛻�ｒ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ蛹�
    var baseH = $(".navWrap ul").outerHeight(true), //繝翫ン縺ｮ鬮倥＆
    h = window.innerHeight - $(".navInquiry").outerHeight();
   //繝｡繝九Η繝ｼ - 縺雁撫縺�粋繧上○縺ｮ鬮倥＆
    $(".navWrap").css("max-height", h);

    //繝｡繝九Η繝ｼ縺ｫ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縺檎匱逕溘＠縺溷�ｴ蜷医√す繝｣繝峨え隕∫ｴ�繧定ｿｽ蜉�
    if ( baseH > h && $(".isScrollUp").length <= 0 ) {
      $(".navWrap").append("<div class=\"isScrollUp\"></div><div class=\"isScrollDown\"></div>");
      $(".isScrollDown").css({
        top: h - $(".isScrollDown").height(),
        right: 0,
      });
    } else if($(".isScrollDown").length > 0 ) {
      $(".isScrollDown").css("top", h - $(".isScrollDown").height());
      $(".isScrollDown, .isScrollUp").css({
        right: 0,
      });
    }
    //譛ｬ菴薙′繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縺励↑縺�ｈ縺�↓蝗ｺ螳�
    htmlFix();

    //繧ｪ繝ｼ繝舌�繝ｬ繧､
    $(".header .js-menu, .headerFloating .js-menu").after("<div class=\"navOverlay\"></div>");
    if ( $(".headerFloating").length > 0 && $(window).width() < 769) {
      $(".header .navOverlay").hide();
    }

    /* ==========
    繝｡繝九Η繝ｼ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ蜃ｦ逅�
    ========== */
    $(".navWrap").scroll(function () {
      var st = $(this).scrollTop(),
          h = $(this).outerHeight(),
          sb = $(this).find("ul").outerHeight() - h;
      //荳翫�繧ｷ繝｣繝峨え繧定｡ｨ遉ｺ
      if( st > 0 ) {
        $(".isScrollUp").show();
      } else if ( st <= 0 ) {
        $(".isScrollUp").hide();
      }
      //荳九�繧ｷ繝｣繝峨え繧定｡ｨ遉ｺ
      if ( st >= sb ) {
        $(".isScrollDown").hide();
      } else {
        $(".isScrollDown").show();
      }
    });
  });

  //繝｡繝九Η繝ｼ繧帝哩縺倥ｋ
  $(document).on("click", ".navOverlay, .js-menuClose, .navWrap a", function(){
    //譛ｬ菴薙�蝗ｺ螳壹ｒ隗｣髯､
    htmlUnFix()
    $(".navOverlay").remove();
    var w = $(".js-menuPanel").width() * -1; //迺ｰ蠅�〒蟷�′驕輔≧縺ｮ縺ｧ蜿門ｾ励☆繧�
    //繝｡繝九Η繝ｼ繧帝撼陦ｨ遉ｺ
    $(".js-menuPanel").css({
      left: "100%",
    });
    //繧ｷ繝｣繝峨え繧帝撼陦ｨ遉ｺ
    $(".isScrollDown, .isScrollUp").remove();

    $(".js-menuClose").hide();
    if( location.hash ) {
      setTimeout( function() {
        var target = location.hash,
            position = $(target).offset().top - $(".headerFloating").height() -40 ;
            $("html, body").animate({scrollTop:position}, 400);
      }, 100)
    }
  });



  /* ==========
  checkbox/radio class莉倥￠螟悶＠
  ========== */
  $("label :checked").parent().addClass("checked");
  $("input").click(function(e) {
    var t = e.target.type;
    var chk = $(this).is(':checked');
    var name = $(this).attr('name');
    if (t == 'checkbox') {
      if (chk == true) {
        $(this).parent().addClass('checked');
      } else {
        $(this).parent().removeClass('checked');
      }
      return true;
    } else if (t == 'radio') {
      if (chk == true) {
        $("input[name=" + name + "]").parents("li, label").removeClass('checked');
        $(this).parents("label").addClass('checked');
      }
      return true;
    }
  });


  /* ==========
  toggle
  ========== */
  //蛻晄悄陦ｨ遉ｺ險ｭ螳�
  $(".js-toggleTrigger").each(function() {
    var target = "." + $(this).attr("data-target");
    if($(this).hasClass("isOpen")) {
      $(target).show(); //霑ｽ蜉�鬆�岼繧帝撼陦ｨ遉ｺ
    } else {
      $(target).hide(); //霑ｽ蜉�鬆�岼繧定｡ｨ遉ｺ
    }
  });
  //click蜃ｦ逅�
  $(".js-toggleTrigger").click(function(){
    var target = "." + $(this).attr("data-target");
    if($(this).hasClass("isOpen")) {
      $(target).slideUp(); //霑ｽ蜉�鬆�岼繧帝撼陦ｨ遉ｺ
      $(this).removeClass("isOpen"); //class繧池emove
    } else {
      $(target).slideDown(); //霑ｽ蜉�鬆�岼繧定｡ｨ遉ｺ
      $(this).addClass("isOpen"); //class繧誕dd
    }
  });

  /* ==========
  toggle縲繧ｹ繝槭�縺ｮ縺ｿ
  ========== */
  //蛻晄悄陦ｨ遉ｺ險ｭ螳�
  if( window.innerWidth < 769 ) {
    $(".js-toggleTrigger-sp").each(function() {
      var target = "." + $(this).attr("data-target");
      if( !$(this).hasClass("isOpen") ) {
        $(target).hide();
      } else {
        $(target). show();
      }
    });
  }
    $(".js-toggleTrigger-sp").click(function(){
      if( window.innerWidth < 769 ) {
        var target = "." + $(this).attr("data-target");
        if($(this).hasClass("isOpen")) {
          $(target).slideUp(); //霑ｽ蜉�鬆�岼繧帝撼陦ｨ遉ｺ
          $(this).removeClass("isOpen"); //class繧池emove
        } else {
          $(target).slideDown(); //霑ｽ蜉�鬆�岼繧定｡ｨ遉ｺ
          $(this).addClass("isOpen"); //class繧誕dd
        }
      }
    });

  if($(".js-modal, .js-movieModal").length > 0) {
    /* ==========
    繝�く繧ｹ繝医さ繝ｳ繝�Φ繝�Δ繝ｼ繝繝ｫ襍ｷ蜍�
    ========== */
    $(".js-modal").colorbox({
      fixed: true,
      width: "900px",
      maxWidth: $(window).width() - 10,
      height: "95%",
      maxHeight: $(window).height() - 10,
      opacity: ".7",
      onOpen: function(){
        htmlFix();
       },
       onComplete: function(){
         modalClose();
       },
      onClosed: function(){
        htmlUnFix();
      }
    });
    //髢峨§繧九�繧ｿ繝ｳ
    function modalClose() {
      $(".buttonClose").click(function(e) {
        parent.$.fn.colorbox.close();
        e.preventDefault() ;
      });
    }


    /* ==========
    蜍慕判繝｢繝ｼ繝繝ｫ襍ｷ蜍�
    ========== */
    var movieH = $(window).width() < 769 ? $(window).height() - 10 : "435px";
    $(".js-movieModal").colorbox({
      fixed: true,
      width: "680px",
      maxWidth: $(window).width() - 10,
      height: movieH,
      opacity: ".7",
      onOpen: function(){
        htmlFix()
      },
      onComplete: function(){
        var contents = $(this).attr("data-url");
        $(".modal").append("<div class=\"movieWrap\"><iframe width=\"560\" height=\"315\"src=\""+ contents + "\" frameborder=\"0\" allow=\"autoplay; encrypted-media\" allowfullscreen></iframe></div>");
      },
      onClosed: function(){
        htmlUnFix();
      }
    });
  }

});

// $(window).load(function () {
// //URL縺ｫ繝上ャ繧ｷ繝･縺後≠繧句�ｴ蜷医�縲√�繝�ム繝ｼ蛻�せ繧ｯ繝ｭ繝ｼ繝ｫ縺吶ｋ
// if( location.hash ) {
// setTimeout( function() {
// var target = location.hash,
// position = $(target).offset().top - $(".headerFloating").height() -40 ;
// $("html, body").animate({scrollTop:position}, 400);
// }, 300)
// }
// });



//===================================
//縲繝ｪ繧ｵ繧､繧ｺ蜃ｦ逅�
//===================================
// 繝ｦ繝ｼ繧ｶ繝ｼ繧ｨ繝ｼ繧ｸ繧ｧ繝ｳ繝医�蛻､蛻･
var userAgent = navigator.userAgent;
//繧ｿ繧､繝槭�繧ｻ繝�ヨ
var commonTimer = null;
// 繧ｹ繝槭�繝医ヵ繧ｩ繝ｳ縺ｮ蝣ｴ蜷医�orientationchange繧､繝吶Φ繝医ｒ逶｣隕悶☆繧�
if (userAgent.indexOf("iPhone") >= 0 || userAgent.indexOf("iPad") >= 0 || userAgent.indexOf("Android") >= 0) {
  window.addEventListener("orientationchange", function () {
    clearTimeout(commonTimer);
    commonTimer = setTimeout(function() {
      resizeWindow();
    }, 200);
    commonTimer = setTimeout(function() {
      resizeModal();
    }, 300);
    }, false);
} else {
  $(window).on('resize', function(){
    clearTimeout(commonTimer);
    commonTimer = setTimeout(function() {
      resizeWindow();
      resizeModal();
    }, 200);
  });
}
function resizeWindow() {
  if( window.innerWidth < 769 ) {
    //繧ｹ繝槭�繧ｵ繧､繧ｺ縺ｫ蛻�ｊ譖ｿ繧上▲縺溘→縺�
    /* ==========
    繝倥ャ繝繝ｼ蜃ｦ逅�
    ========== */
    $("header").removeClass("headerFloating");
    createHeader();

    /* ==========
    繝壹�繧ｸ繝医ャ繝怜�逅�
    ========== */
    $(".js-pcPagetop").removeAttr("style");

  } else if( window.innerWidth >= 769) {
    //PC繧ｵ繧､繧ｺ縺ｫ蛻�ｊ譖ｿ繧上▲縺溘→縺�
    /* ==========
    繝倥ャ繝繝ｼ蜃ｦ逅�
    ========== */
    $("div.headerFloating").remove();

  }

  //蜈ｱ騾壹�蜃ｦ逅�
  /* ==========
  繝｡繝九Η繝ｼ蜃ｦ逅�
  ========== */
  adjustMenu();
}

function resizeModal() {
  var modalW = $(window).width() > 900 ? "900px" : $(window).width() - 10,
  modalH = $(window).height() > 715 ? "715px" : $(window).height() - 10;
  if($(".js-modal, .js-movieModal").length > 0) {
    $.colorbox.resize({
      width: modalW,
      height: modalH
    });
  }
}



//繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ遖∵ｭ｢
function htmlFix() {
  st = $(window).scrollTop(); //迴ｾ蝨ｨ縺ｮ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ菴咲ｽｮ繧貞叙蠕�
  //html閾ｪ菴薙ｒfixed縺ｫ險ｭ螳壹＠縺ｦtop菴咲ｽｮ繧呈欠螳�
  $("html").addClass("js-menuOpened").css("top", "-" + st +"px");
}
//繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ遖∵ｭ｢隗｣髯､
function htmlUnFix() {
  var scrollpos = parseInt($("html").css("top")); //top縺ｮ蛟､繧呈焚蛟､縺�縺大叙蠕�
  //html縺ｮclass縺ｨstyle繧池emove
  $("html").removeClass('js-menuOpened').removeAttr("style");
  //top縺ｮ蛟､縺後�繧､繝翫せ縺ｪ縺ｮ縺ｧ縲√�繧､繝翫せ繧帝㍾縺ｭ繧九％縺ｨ縺ｧ繝励Λ繧ｹ縺ｮ謨ｰ蛟､縺ｫ螟画鋤縺励※繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縲�
  window.scrollTo( 0 , -scrollpos );
}


//繝倥ャ繝繝ｼ逕滓�
function createHeader() {
  var st = $(window).scrollTop();

    if( window.innerWidth >= 769 && st > 1 ) {
      //繝倥ャ繝繝ｼ霑ｽ蠕難ｼ�PC�鋭t縺�0縺ｧ縺ｪ縺代ｌ縺ｰclass繧剃ｻ倅ｸ�
      $(".header").addClass("headerFloating");
    } else if( $(".header").hasClass("js-floating") && window.innerWidth < 769 ) {
      //繝倥ャ繝繝ｼ霑ｽ蠕難ｼ�SP�芽ｦ∫ｴ�繧偵さ繝斐�
      if ( $(".header").hasClass("js-floatingSimple") && $("div.headerFloating").length <= 0 ) {
        //SP繝倥ャ繝繝ｼ霑ｽ蠕薙�隕狗ｩ阪ｊ繝懊ち繝ｳ縺ｪ縺�
        $(".header").after("<div class=\"headerFloating\"><ul class=\"headerFloatNav\"></ul></div>");
        $(".headerLogo").clone().prependTo(".headerFloating");
        $(".headerNav").clone().appendTo(".headerFloatNav");
      } else if( $(".header").hasClass("js-floating") && $("div.headerFloating").length <= 0 ) {
        //SP繝倥ャ繝繝ｼ霑ｽ蠕薙�騾壼ｸｸ
        $(".header").after("<div class=\"headerFloating\"><div class=\"headerFloatNav\"></div><div class=\"headerButton\">縺ｾ縺壹�繧ｯ繝ｫ繝槭ｒ讀懃ｴ｢</div></div>");
        $(".headerLogo").clone().prependTo(".headerFloating");
        $(".headerNav").clone().appendTo(".headerFloatNav");
        $(".headerEstimate").clone().appendTo(".headerButton");
      }
    }

}

//繝｡繝九Η繝ｼ
function adjustMenu() {
  if( $("html").hasClass("js-menuOpened")) {

    //繧ｪ繝ｼ繝舌�繝ｬ繧､謫堺ｽ�
    if( $(window).width() > 768 ) {
        $(".header .navOverlay").show();
    } else {
      if ( $(".headerFloating").length > 0 ) {
        $(".header .navOverlay").hide();
      }
    }

    //菴咲ｽｮ螟画峩縲ゅΔ繝ｼ繝繝ｫ螻暮幕譎ゅ�蟇ｾ雎｡螟�
    if( $("#colorbox").css("display") == "none" ) {
      var lPos = $(window).width() > 768 ? window.innerWidth - 320 : "22vw";
      //繝｡繝九Η繝ｼ繧定｡ｨ遉ｺ
      $(".js-menuPanel").css({
        left: lPos,
      });
    }

    //繝｡繝九Η繝ｼ陦ｨ遉ｺ縺励◆縺ｾ縺ｾ繧ｵ繧､繧ｺ螟画峩縺励◆譎ゅ�繝翫ン縺ｮ鬮倥＆繧貞�險育ｮ�
    $(".navWrap").removeAttr("style"); //鬮倥＆縺悟叙繧後↑縺��縺ｧstyle髯､蜴ｻ
    setTimeout(function() {
      baseH = $(".navWrap").outerHeight(true), //繝翫ン縺ｮ鬮倥＆
      h = window.innerHeight - $(".navInquiry").innerHeight(); //繝｡繝九Η繝ｼ - 縺雁撫縺�粋繧上○縺ｮ鬮倥＆
      $(".navWrap").css("max-height", h);

      if( baseH <= h ) {
        $(".isScrollDown, .isScrollUp").remove();
      } else if ( baseH > h &&  $(".isScrollUp").length <= 0 && $("#colorbox").css("display") == "none" ) {
        //繝｡繝九Η繝ｼ縺ｫ繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ縺檎匱逕溘＠縺溷�ｴ蜷医√す繝｣繝峨え隕∫ｴ�繧定ｿｽ蜉�
        $(".navWrap").append("<div class=\"isScrollUp\"></div><div class=\"isScrollDown\"></div>");
          $(".isScrollDown").css({
            top: h - $(".isScrollDown").height(),
            right: 0,
          });
      } else {
        $(".isScrollDown").css("top", h - $(".isScrollDown").height());
      }
    }, 100)
  }

}