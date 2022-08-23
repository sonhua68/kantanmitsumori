using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Models.StateModel
{
    public class StateModel
    {
        [TempData]
        public string? stateLoadWindow { get; set; }

        [TempData]
        public string? sesPriDisp { get; set; }

        [TempData]
        public string? sesMode { get; set; }

        [TempData]
        public string? sesLeaseFlag { get; set; }

        [TempData]
        public string? sesErrMsg { get; set; }

        [TempData]
        public string? sesUserNo { get; set; }

        [TempData]
        public string? sesUserNm { get; set; }

        [TempData]
        public string? sesUserAdr { get; set; }

        [TempData]
        public string? sesUserTel { get; set; }

        [TempData]
        public string? sesdispUserInf { get; set; }

        [TempData]
        public string? sesPdfTitleKbn { get; set; }

        [TempData]
        public string? sesCustNm_forPrint { get; set; }

        [TempData]
        public string? sesCustZip_forPrint { get; set; }

        [TempData]
        public string? sesCustAdr_forPrint { get; set; }

        [TempData]
        public string? sesCustTel_forPrint { get; set; }
    }
}
