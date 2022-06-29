namespace KantanMitsumori.Helper.Constant
{
    public class CommonConst
    {
        /// <summary>
        /// タイトル
        /// ※プログラムで制御してセットする必要がある場合は定数を使用
        /// -- タイトル（合計行用）
        /// </summary>
        public const string def_TitleCarPrice = "車両本体価格";
        public const string def_TitleDisCount = "値引き";
        public const string def_TitleSonota = "その他費用";
        public const string def_TitleSyakenNew = "車検整備費用";
        public const string def_TitleSyakenZok = "納車整備費用";
        public const string def_TitleOpSpeCial = "付属品・特別仕様";
        public const string def_TitleTaxInsEquivalent = "税金・保険料相当額";
        public const string def_TitleDaiko = "手続代行費用";
        public const string def_TitleConTaxTotalInTax = "（内消費税合計）";
        public const string def_TitleConTaxTotalOutTax = "消費税合計";
        public const string def_TitleCarKeiInTax = "現金販売価格（6～10）";
        public const string def_TitleCarKeiOutTax = "現金販売価格（6～10）";
        public const string def_TitleSalesSumInTax = "お支払総額（12～14）";
        public const string def_TitleSalesSumOutTax = "お支払総額（11～14）";
        //-- タイトル（明細行用）
        public const string def_TitleAutoTax = "自動車税";
        public const string def_TitleAutoTaxEquivalent = "自動車税相当額";
        public const string def_TitleDamageIns = "自賠責保険料";
        public const string def_TitleDamageInsEquivalent = "自賠責保険料相当額";
        //-- タイトル内消費税表示
        public const string def_TitleInTax = "（税込）";
        public const string def_TitleOutTax = "（税抜）";

        //単位 既定値
        //-- 走行距離
        public const string def_MilUnitTKM = "千km";
        public const string def_MilUnitTMILE = "千マイル";
        //-- 排気量
        public const string def_DispVolUnitCC = "cc";
        public const string def_DispVolUnitKW = "kw";
        public const string def_DispVolUnitRE = "RE";
        //-- 下取車走行距離
        public const string def_TradeInMilUnitKM = "km";

        //エラー関連
        //-- エラーメッセージ
        public const string def_ErrCodeL = "<br>（エラー番号：";
        public const string def_ErrCodeR = "）";
        // 一般エラー
        public const string def_ErrMsg1 = "エラーが発生しました。";
        // 各種情報取得エラー（車両重量、重量税、自賠責保険料などの情報を取得できない場合）
        public const string def_ErrMsg1_Maker = "エラーが発生しました。";
        // 会員番号取得エラー（セッション、POSTパラメータから会員番号を取得できない場合）
        public const string def_ErrMsg2 = "お客様の会員番号が確認できません。<BR>もう一度、ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。<BR><BR>　●一定時間ご利用がなかった場合、情報はクリアされます。<BR>　●「お気に入り」から直接表示した場合、会員番号が特定できません。";
        // 会員情報取得エラー（DBから会員情報を取得できない場合）
        public const string def_ErrMsg3 = "ユーザー情報が確認できませんでした。";
        // POST 送信項目 "mode" 情報取得エラー（ASNET、店頭商談NET からの遷移と認められない）
        public const string def_ErrMsg4 = "ページ遷移エラーが発生しました。<br>ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。";
        // 予期せぬエラー（IIS が検知したもの）
        public const string def_UnexpectedErrMsg = "エラーの発生により、操作を続けられません。（エラー番号：ISYS-010I）";
        // 予期せぬエラーの補足説明
        public const string def_UnexpectedErrInfo = "以下の原因が考えられます。<br><br>　●URL が正しくない、または ブラウザのブックマークから直接開いた<br>　●セッションタイムアウトが発生した<br>　●ブラウザ上で、URLが反転表示されている時に [Enter] キーを押した<br>　●入力項目に、HTML タグ（&lt;script&gt;、&lt;b&gt;など）が入っている";

        //-- エラーメッセージ格納用セッション変数名
        public const string sesErrMsg = "sesErrMsg";

        //-- エラーページリダイレクト先
        public const string def_ErrPage = "ErrAs.aspx";

        //オプションDDL用
        public const string def_Option1 = "ＥＴＣ車載器";
        public const string def_Option2 = "カーナビゲーション";
        public const string def_Option3 = "ドライブレコーダー";
        public const string def_Option4 = "スタッドレスタイヤ";

        //カラ時のスペース
        public const string def_Space = "    ";

        //自賠責保険基準月数
        public const int def_DamegeInsMonth25 = 25;

        //見積入力画面
        public const string sesUserNo = "sesUserNo";                        //ユーザーNo
        public const string sesUserNm = "sesUserNm";                        //ユーザー名
        public const string sesUserAdr = "sesUserAdr";                      //ユーザー住所
        public const string sesUserTel = "sesUserTel";                      //ユーザー電話
        public const string sesdispUserInf = "sesdispUserInf";              //ユーザー情報整形済み
        public const string sesCustNm_forPrint = "sesCustNm_forPrint";      //ユーザーのお客様の名前
        public const string sesCustZip_forPrint = "sesCustZip_forPrint";    //ユーザーのお客様の郵便番号
        public const string sesCustAdr_forPrint = "sesCustAdr_forPrint";    //ユーザーのお客様の住所
        public const string sesCustTel_forPrint = "sesCustTel_forPrint";    //ユーザーのお客様の電話番号
        public const string sesOneFlg = "sesOneFlg";                        //ワンプラorワンプラ以外フラグ
        public const string sesAAPlace = "sesAAPlace";                      //AA会場
        public const string sesAANo = "sesAANo";                            //出品No
        public const string sesAATime = "sesAATime";                        //出品期間
        public const string sesPrice = "sesPrice";                          //価格
        public const string sesHyk = "sesHyk";                              //評価点
        public const string sesYear = "sesYear";                            //年式
        public const string sesMaker = "sesMaker";                          //メーカー名
        public const string sesCarNM = "sesCarNM";                          //車名
        public const string sesGrade = "sesGrade";                          //グレード
        public const string sesHaiki = "sesHaiki";                          //排気量
        public const string sesKata = "sesKata";                            //型式
        public const string sesCarNo = "sesCarNo";                          //車台番号
        public const string sesSft = "sesSft";                              //シフト
        public const string sesSyaken = "sesSyaken";                        //車検
        public const string sesRun = "sesRun";                              //走行距離
        public const string sesColor = "sesColor";                          //車体色
        public const string sesNenryo = "sesNenryo";                        //燃料
        public const string sesCarReki = "sesCarReki";                      //車歴
        public const string sesOption = "sesOption";                        //オプション
        public const string sesRakuSatu = "sesRakuSatu";                    //落札料
        public const string sesRikusou = "sesRikusou";                      //陸送代
        public const string sesCarImgPath = "sesCarImgPath";                //車両画像格納場所
                                                                            //消費税税率追加
        public const string sesTaxRatio = "sesTaxRatio";                   //消費税 税率
                                                                           //商談メモ画像枚数追加
        public const string sesCarImgPath1 = "sesCarImgPath1";              //車両画像格納場所(サブ1枚目)
        public const string sesCarImgPath2 = "sesCarImgPath2";              //車両画像格納場所(サブ2枚目)
        public const string sesCarImgPath3 = "sesCarImgPath3";              //車両画像格納場所(サブ3枚目)
        public const string sesCarImgPath4 = "sesCarImgPath4";              //車両画像格納場所(サブ4枚目)
        public const string sesCarImgPath5 = "sesCarImgPath5";              //車両画像格納場所(サブ5枚目)
        public const string sesCarImgPath6 = "sesCarImgPath6";              //車両画像格納場所(サブ6枚目)
        public const string sesCarImgPath7 = "sesCarImgPath7";              //車両画像格納場所(サブ7枚目)
        public const string sesCarImgPath8 = "sesCarImgPath8";              //車両画像格納場所(サブ8枚目)
                                                                            //商談メモ画像枚数追加
        public const string sesASNETFLG = "sesASNETFLG";                    //見積検索画面の遷移元判定用
        public const string sesLoadWindow = "sesLoadWindow";               //見積メイン／見積検索／フリー見積
                                                                           //店頭商談NET対応
        public const string sesMode = "sesMode";                           //モード "0":ASNET, "1":店頭商談NET, "":左記以外 ※判定は文字列として行う（数値の場合、Nothingが0扱いになってしまうため）
        public const string sesPriDisp = "sesPriDisp";                   //価格表示　0:価格表示あり 1:価格表示なし

        public const string sesEstNo = "sesEstNo";                      //見積書番号
        public const string sesEstSubNo = "sesEstSubNo";                 //枝番

        //ローンシミュレーション関連
        //-- メッセージ
        //計算中に例外が発生して続行不能の場合
        public const string msgCalcException = "この条件では計算できません。別の条件を指定して下さい。";
        //ボーナス指定月が期間外の場合
        public const string msgBonusMonthErr = "ボーナス第1回支払月はお支払期間内を指定して下さい。";
        //ボーナス回数が一回の時
        public const string msgBonusTimesErr = "計算によりボーナス回数は1回となります。";
        //月々支払額が3000円未満の場合
        public const string msgPayMonthShort = "分割支払額が3000円未満です。";
        //分割支払金合計が1億円以上の場合
        public const string msgPayTotalOver = "分割支払金合計が1億円を超えています。";
        //ボーナス支払額合計が分割支払金合計の70%超の場合
        public const string msgBonusSevenOver = "ボーナス支払額合計が分割支払金合計の70%を超えています。";

        //-- ローン情報再計算ステータス
        public const int def_LoanInfo_Unexecuted = 0;   //メッセージ表示無
        public const int def_LoanInfo_Clear = 1;      //メッセージ表示（クリア）
        public const int def_LoanInfo_NormalEnd = 2;    //メッセージ表示（再計算 正常終了）
        public const int def_LoanInfo_Error = 3;      //メッセージ表示（再計算 エラー）

        //見積書or注文書 切り替え
        public const string sesPdfTitleKbn = "sesPdfTitleKbn";

        //見積データ削除処理用
        public const string sesEnableBackFlg = "sesEnableBackFlg"; //見積検索画面上、[戻る] ボタン押下時に使用する判定フラグ
        public const string sesCondEstNo = "sesCondEstNo";
        public const string sesCondEstSubNo = "sesCondEstSubNo";
        public const string sesCondFromSelectY = "sesCondFromSelectY";
        public const string sesCondFromSelectM = "sesCondFromSelectM";
        public const string sesCondFromSelectD = "sesCondFromSelectD";
        public const string sesCondToSelectY = "sesCondToSelectY";
        public const string sesCondToSelectM = "sesCondToSelectM";
        public const string sesCondToSelectD = "sesCondToSelectD";
        public const string sesCondCustKanaName = "sesCondCustKanaName";
        public const string sesCondMaker = "sesCondMaker";
        public const string sesCondModel = "sesCondModel";
        public const string sesCondChassisNo = "sesCondChassisNo";

        //見積検索用
        public const int def_DurationMonths = 3;                   //検索対象期間（月単位）
        public const string sesFromDate = "ses_FromDate";           //検索開始日付
        public const string sesToDate = "ses_ToDate";               //検索終了日付

        //フリー見積用
        public const string sesMakID = "sesMakID";                 //メーカーID
        public const string sesFreeEstBtn = "sesFreeEstBtn";           //車種指定画面での押下ボタン
        public const string sesEnableUseSelInfo = "sesEnableUseSelInfo"; //セッションの車種等選択情報を再表示に使用可能
        public const string def_SelMakerMsg = "メーカーを選択して下さい";
        public const string def_SelModelMsg = "車種を選択して下さい";
        public const string def_SelTypeMsg = "型式指定は必ず入力して下さい";
        public const string def_ModelNotFoundMsg = "該当する車種が見つかりません";
        public const string def_GradeNotFoundMsg = "該当するグレードが見つかりません";
        public const string def_DataNotFoundMsg = "該当するデータがありませんでした<br>[車名を直接入力する >>] ボタンから作成してください";
    }
}
