// JavaScript Document

/* ==========
 ス�?ーズスクロール
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

    //IE用?�ラベル�??画像をクリ�?��したとき�?処�?
    //-----------------
    if ($.browser.msie) {
        $('label > img').click(function () {
            $('#'+$(this).parent().attr('for') ).focus().click();
        });
    }


    //レイアウト調整
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

    //行ぞろえ
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
    // 固定表示ウィンドウ
    var pricebox = $('#fixwin');
    var offset = pricebox.offset();             // 固定エリアの初期ポジション取得用
    var fixH = pricebox.height();               // 固定エリアの高さ
    var priceH = $("#priceview").height();      // 価格表示部�??高さ
    var optmaxH = Math.floor($("#optionlist").css("max-height").split("px")[0]);    // オプション表示のマックスの高さ
    var newopH;                                 // オプション表示のマックスの高さ:再設定時
    var linksT = $("#links").position().top;    // リンクエリアのY座�?
    var mt = 0;                                 // マ�?ジン調整用
    var headerH = $('#headerArea').height();
    // スクロール時�?処�?
    // ------------------------
    $(window).scroll(function () {
        fixH = pricebox.height();
        linksT = $("#links").position().top;

        if($(window).scrollTop() + headerH > offset.top){
            pricebox.css("margin-top","35px");
            // 固�?
            pricebox.addClass('fixed');

            // ペ�?ジの下部へ行ったときに固定エリアの位置を調整する
            if(linksT<(fixH + $(window).scrollTop())){
                mt = linksT - (fixH + $(window).scrollTop());
                pricebox.css("margin-top",mt+"px");
            }
        } else {
            // 固定解除
            pricebox.removeClass('fixed');

            // 固定エリアの位置調整
            pricebox.css("margin-top","0");
        }
    });

    // リサイズ時�?処�?
    // ------------------------
    $(window).resize(function(){
        newopH = $(window).height() - priceH;
        if(newopH < optmaxH){
            $("#optionlist").css("max-height",newopH+"px");
        }
    });
  }
}