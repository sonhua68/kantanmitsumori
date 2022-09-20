namespace KantanMitsumori.Model
{
    public class LogToken
    {
        public string UserNo { get; set; }
        public string UserNm { get; set; }
        public string Token { get; set; }
        public string stateLoadWindow { get; set; }
        public string sesPriDisp { get; set; }
        public string sesMode { get; set; }
        public string sesPdfTitleKbn { get; set; }
        public string sesCustNm_forPrint { get; set; } = "";
        public string sesCustZip_forPrint { get; set; } = "";
        public string sesCustAdr_forPrint { get; set; } = "";
        public string sesCustTel_forPrint { get; set; } = "";
        public string sesEstNo { get; set; }
        public string sesEstSubNo { get; set; }
        public decimal sesTaxRatio { get; set; } = -1M;
        public string sesCarImgPath { get; set; } // 車両画像格納場所
        public string sesCarImgPath1 { get; set; }// 車両画像格納場所(サブ1枚目)
        public string sesCarImgPath2 { get; set; }// 車両画像格納場所(サブ2枚目)
        public string sesCarImgPath3 { get; set; }// 車両画像格納場所(サブ3枚目)
        public string sesCarImgPath4 { get; set; }// 車両画像格納場所(サブ4枚目)
        public string sesCarImgPath5 { get; set; }// 車両画像格納場所(サブ5枚目)
        public string sesCarImgPath6 { get; set; }// 車両画像格納場所(サブ6枚目)
        public string sesCarImgPath7 { get; set; }// 車両画像格納場所(サブ7枚目)
        public string sesCarImgPath8 { get; set; }// 車両画像格納場所(サブ8枚目)
        public string sesMaker { get; set; }
        public string sesCarNM { get; set; }
        public string sesGrade { get; set; }
        public string sesKata { get; set; }
        public string sesHaiki { get; set; }
        public string sesSft { get; set; }
        public string sesLeaseFlag { get; set; }

    }
}