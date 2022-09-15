// JScript File

function closeWin() {
    window.close();
}

/* 数値桁揃え関数 */
function format(val, digit, flg) {//val:数値;digit:桁;flg:(0:四捨五入;1:切り捨て;2:切り上げ)
    val *= Math.pow(10.0, digit);

    if (flg == 0)
        val = Math.round(val)
    else if (flg == 1)
        val = Math.floor(val)
    else if (flg == 2)
        val = Math.ceil(val);

    val += "";
    //０埋め処理
    var tmp = digit - val.length;
    if (0 < tmp) for (i = 0; i < tmp; i++) val = "0" + val;

    //.挿入処理
    if (digit) {
        var pat = "";
        for (i = 0; i < digit; i++) pat += ".";
        val = val.replace(eval("/(" + pat + "$)/"), ".$1");
        val = val.replace(/^\./, "0\.");
    }
    return (val);
}

function CommaSep(x) { // 引数の例としては 95839285734.3245
    var s = "" + x; // 確実に文字列型に変換する。例では "95839285734.3245"
    var p = s.indexOf("."); // 小数点の位置を0オリジンで求める。例では 11
    if (p < 0) { // 小数点が見つからなかった時
        p = s.length; // 仮想的な小数点の位置とする
    }
    var r = s.substring(p, s.length); // 小数点の桁と小数点より右側の文字列。例では ".3245"
    for (var i = 0; i < p; i++) { // (10 ^ i) の位について
        var c = s.substring(p - 1 - i, p - 1 - i + 1); // (10 ^ i) の位のひとつの桁の数字。例では "4", "3", "7", "5", "8", "2", "9", "3", "8", "5", "9" の順になる。
        if (c < "0" || c > "9") { // 数字以外のもの(符合など)が見つかった
            r = s.substring(0, p - i) + r; // 残りを全部付加する
            break;
        }
        if (i > 0 && i % 3 == 0) { // 3 桁ごと、ただし初回は除く
            r = "," + r; // カンマを付加する
        }
        r = c + r; // 数字を一桁追加する。
    }
    return r; // 例では "95,839,285,734.3245"
}

function dspNumber(inval) {
    var outval;
    if (inval == "" || inval == "0") {
        outval = "";
    } else {
        outval = CommaSep(inval) + " 円";
    }
    return outval;
}

/* 空欄チェック */
function chkNull(vnum) {
    //空欄
    if (vnum == '') {
        return Number(0);
    } else {
        return Number(vnum);
    }
}
// 数値チェック
function chkNum(strMsg, vnum) {
    var outMsg;
    if (vnum == "") {
        outMsg = "";
    } else if (isNaN(vnum)) {
        if (strMsg == '第1回支払月の年' || strMsg == '第1回支払月の月') {
            outMsg = '正しい年月を入力して下さい。';
        } else {
            outMsg = strMsg + 'に数字以外は入力できません。';
        }
    } else {
        outMsg = "";
    }
    return outMsg;
}

// マイナスチェック 
function chkMin(strMsg, vnum) {
    var outMsg;
    if (vnum == "") {
        outMsg = "";
    } else if (vnum < 0) {
        if (strMsg == '第1回支払月の年' || strMsg == '第1回支払月の月') {
            outMsg = '正しい年月を入力して下さい。';
        } else {
            outMsg = strMsg + 'にマイナス値は入力できません。';
        }
    } else {
        outMsg = "";
    }
    return outMsg;
}
// 小数点チェック 
function chkDecf(strMsg, vnum) {
    var outMsg;
    strNum = String(vnum);
    if (vnum == "") {
        outMsg = "";
    } else if (strNum.indexOf(".") > -1) {
        if (strMsg == '第1回支払月の年' || strMsg == '第1回支払月の月') {
            outMsg = '正しい年月を入力して下さい。';
        } else {
            outMsg = strMsg + 'に小数は入力できません。';
        }
    } else {
        outMsg = "";
    }
    return outMsg;
}

// 金額項目のトータルチェック
function chkKingaku(strMsg, vnum) {
    var outMsg = "";
    outMsg = chkNum(strMsg, vnum);
    if (outMsg != "") {
        return outMsg;
    }
    outMsg = chkMin(strMsg, vnum);
    if (outMsg != "") {
        return outMsg;
    }
    outMsg = chkDecf(strMsg, vnum);
    if (outMsg != "") {
        return outMsg;
    }
    return outMsg;
}

function isZenKana(obj) {
    var zen = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン　ァィゥェォッャュョー－ ガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴ";
    var str = obj; /* 入力値 */
    var mes = "";
    for (var i = 0; i < str.length; i++) {
        /* 文字列を１文字ずつ調べる */
        var tmp1 = str.substr(i, 1);
        for (var j = 0; j < zen.length; j++) {
            var tmp2 = zen.substr(j, 1);
            /* 含まれていた全角カナを格納 */
            if (tmp1 == tmp2) {
                mes += tmp1;
            }
        }
    }
    if (str == mes) {
        return true;
    } else {
        return false;
    }
}
/**********************************************
/* 画面の金額を数値へ変換
/*********************************************/
function cnvKin(inval) {

    var outval;

    if (inval == "") {
        outval = 0;
    } else {
        inval = inval.replace(/,/g, "");
        inval = inval.replace("円", "");
        inval = inval.replace("▲", "");
        outval = inval;
    }
    return Number(outval);

}
/**********************************************
/* #2057 Update Condition Into Check Bytes 2022/06/17 By Huy Modified
/* 入力桁数バイトチェック
/*********************************************/
function chkBytes(cName, cTxt, cMaxLen) {
    if (cTxt.length == 0) { return "" };
    var count = 0;
    var wstr = "";
    for (var i = 0; i < cTxt.length; i++) {
        wstr = cTxt.charAt(i);
        if (escape(wstr).length < 4 || wstr.match(/[｡-ﾟ]/)) {
            count = count + 1;
        } else {
            count = count + 2;
        }
    }

    if (count > cMaxLen) {
        return cName + "に入力できる文字数は半角" + cMaxLen + "（全角" + Math.floor(cMaxLen / 2) + "）文字までです";
    } else {
        return "";
    }
}
//2014/08/08 fukunaga add start
/**********************************************
/* Enterキーによるsubmitを無効にする
/*********************************************/
function NoSubmit(e) {
    if (!e) var e = window.event;
    if (e.keyCode == 13) {
        if (e.srcElement.type != 'submit' && e.srcElement.type != 'textarea') {
            return false;
        }
    }
}
//2014/08/08 fukunaga add end

// 単位切替え
function swichUnit(element) {
    if (document.getElementById('rad' + element + 'Other').checked == true) {
        document.getElementById('txt' + element).style.visibility = 'visible';
    } else {
        document.getElementById('txt' + element).value = '';
        document.getElementById('txt' + element).style.visibility = 'hidden';
    }
}
/*
 * StringNull
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/

function StringNull(value) {
    if (value == '0' || value == 0) {
        return "";
    } else {
        return value;
    }
}
/*
 * isNumeric
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/
function isNumeric(str) {
    if (typeof str != "string") return false
    return !isNaN(str) &&
        !isNaN(parseFloat(str))
}
/*
 * getWareki
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/
function getWareki(str) {
    let isYear = isNumeric(str);
    if (isYear) {
        let intYear = parseInt(str)
        if (1926 <= intYear & intYear <= 1988) {
            return "S" + (intYear - 1925);
        } else if (intYear <= 2018) {
            return "H" + (intYear - 1988);
        } else if (2019 <= intYear) {
            return "R" + (intYear - 2018);
        }
    }
   
}
const def_Space = "    "
/*
 * FormatDayGetYear
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/
function FormatDayGetYear(str) {
    let vlength = str.length;
    var year = "";
    switch (vlength) {     
        case 2:
            year = def_Space; 
            break;
        case 1:
            year = def_Space; 
            break;
        case 0:
            year = def_Space;  
            break;
        default:
            year = str.toString().substring(0, 4);  
            return year
    }
}

/*
 * FormatDayGetMonth
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/
function FormatDayGetMonth(str) {
    let vlength = str.length;
    var month = "";
    switch (vlength) {
        case 5:
            month = str.toString().substring(4, 5);
            break;
        case 4:
            month = def_Space;
            break;
        case 2:
            month = def_Space;
            break;
        case 0:
            month = def_Space;
            break;
       
        default:
            month = str.toString().substring(4, 6);
            return month
    }

}
/*
 * groupBy
 *  Create By HoaiPhong
 *  Date 2022/09/07
 /*/
function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}
/*
 * compose
 *  Create By HoaiPhong
 *  Date 2022/09/14
 /*/
String.prototype.compose = (function () {
    var re = /\{{(.+?)\}}/g;
    return function (o) {
        return this.replace(re, function (_, k) {
            return typeof o[k] != 'undefined' ? o[k] : '';
        });
    }
}());
