// JavaScript Document

/* ==========
 ã¹ã?ã¼ãºã¹ã¯ã­ã¼ã«
 ========== */
$(document).on("click", ".anchorHref", function() {
    if( location.hash ) {
        setTimeout( function() {
          var target = location.hash,
              position = $(target).offset().top - $(".headerFloating").height() -40 ;
              $("html, body").animate({scrollTop:position}, 400);
        }, 100)
      }
});


$(function(){

    //IEç¨?ã©ãã«å??ç»åãã¯ãªã?¯ããã¨ãã?å¦ç?
    //-----------------
    if ($.browser.msie) {
        $('label > img').click(function () {
            $('#'+$(this).parent().attr('for') ).focus().click();
        });
    }


    //ã¬ã¤ã¢ã¦ãèª¿æ´
    //-----------------
    //banner
    if($("#bnr").size()>0){
        $("#bnr li:last-child").css("margin","0");
    }

    //estimate
    if($("#estimate").size()>0){
        $(".btnestimate a").each(function(i) {
            var num = i+1;
            var top = $(this).position().top;
            $(this).bind("mouseover",function(){
                $("#trap").addClass("active");
                $(".estimateover").each(function(){
                    $(this).removeClass("active");
                });
                $("#btnestimate"+num+"on").addClass("active").css("top",top);
                $("#btnestimate"+num+"on").bind("mouseover",function(){
                    $(".pop").each(function(){
                        $(this).removeClass("active");
                    });
                    $("#estimatepop"+num).addClass("active").css("top",top);
                });
            });
        });
        $(".pop .col2 li:nth-child(even)").css("border-right","none");
        $("#trap").bind("mouseover",function(){
            $(".estimateover").each(function(){
                $(this).removeClass("active");
            });
            $(".pop").each(function(){
                $(this).removeClass("active");
            });
            $(this).removeClass("active");
        });
    }

    //links
    if($("#inlinks .box").size()>0){
        $("#inlinks .box:last-child").css("width","220px")
    }

    //btnlist2
    if($(".btnlist2 li").size()>0){
        $(".btnlist2 li:last-child").addClass("R");
    }

    if($("a.disabled").size()>0){
        $("a.disabled").each(function() {
            $(this).css("opacity","0.5");
            $(this).bind("click",function(){
                return false;
            });
        });
    }

    //è¡ããã
    //------------------------------
    if($(".lineset li").size()>0){
        var linearr = [];
        var ulcnt = $("ul.lineset").length;
        var colcnt = $("ul.lineset").data("col");
        $("ul.lineset").each(function(i){
            var len = $(this).length;
            var linecnt = 0;
            line = Math.floor(len/colcnt);
            if(len > (line*colcnt)){
                line++;
            }
            var alllinecnt = 0;
            $(this).children().each(function(j){
                if(j%colcnt == 0){
                    linecnt++;
                }
                linearr[i] = linecnt;
                if($(".sttl").size()>0){
                    $(this).find(".sttl").addClass("heightLineSttl1-"+i+"-"+linecnt);
                }
                if($(".sttl2").size()>0){
                    $(this).find(".sttl2").addClass("heightLineSttl2-"+i+"-"+linecnt);
                }
                if($(".txt").size()>0){
                    $(this).find(".txt").addClass("heightLineTxt"+i+"-"+linecnt);
                }
                $(this).addClass("heightLine"+i+"-"+linecnt);
            });
        });
        function setline(){
            for(var i=0; i < ulcnt; i++){
                for(var j=0; j <= linearr[i]; j++){
                    if($(".sttl").size()>0){
                        $(".heightLineSttl1-"+i+"-"+j).heightLine();
                    }
                    if($(".sttl2").size()>0){
                        $(".heightLineSttl2-"+i+"-"+j).heightLine();
                    }
                    if($(".txt").size()>0){
                        $(".heightLineTxt"+i+"-"+j).heightLine();
                    }
                    $(".heightLine"+i+"-"+j).heightLine();
                }
            }
        }
        setline();
        clearTimeout(linetimer);
        var linetimer = setTimeout(setline,100);
    }



})

function fixwinInit() {
  if($("#fixwin").size()>0){
    // åºå®è¡¨ç¤ºã¦ã£ã³ãã¦
    var pricebox = $('#fixwin');
    var offset = pricebox.offset();             // åºå®ã¨ãªã¢ã®åæãã¸ã·ã§ã³åå¾ç¨
    var fixH = pricebox.height();               // åºå®ã¨ãªã¢ã®é«ã
    var priceH = $("#priceview").height();      // ä¾¡æ ¼è¡¨ç¤ºé¨å??é«ã
    var optmaxH = Math.floor($("#optionlist").css("max-height").split("px")[0]);    // ãªãã·ã§ã³è¡¨ç¤ºã®ããã¯ã¹ã®é«ã
    var newopH;                                 // ãªãã·ã§ã³è¡¨ç¤ºã®ããã¯ã¹ã®é«ã:åè¨­å®æ
    var linksT = $("#links").position().top;    // ãªã³ã¯ã¨ãªã¢ã®Yåº§æ¨?
    var mt = 0;                                 // ãã?ã¸ã³èª¿æ´ç¨
    var headerH = $('#headerArea').height();
    // ã¹ã¯ã­ã¼ã«æã?å¦ç?
    // ------------------------
    $(window).scroll(function () {
        fixH = pricebox.height();
        linksT = $("#links").position().top;

        if($(window).scrollTop() + headerH > offset.top){
            pricebox.css("margin-top","35px");
            // åºå®?
            pricebox.addClass('fixed');

            // ãã?ã¸ã®ä¸é¨ã¸è¡ã£ãã¨ãã«åºå®ã¨ãªã¢ã®ä½ç½®ãèª¿æ´ãã
            if(linksT<(fixH + $(window).scrollTop())){
                mt = linksT - (fixH + $(window).scrollTop());
                pricebox.css("margin-top",mt+"px");
            }
        } else {
            // åºå®è§£é¤
            pricebox.removeClass('fixed');

            // åºå®ã¨ãªã¢ã®ä½ç½®èª¿æ´
            pricebox.css("margin-top","0");
        }
    });

    // ãªãµã¤ãºæã?å¦ç?
    // ------------------------
    $(window).resize(function(){
        newopH = $(window).height() - priceH;
        if(newopH < optmaxH){
            $("#optionlist").css("max-height",newopH+"px");
        }
    });
  }
}