using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Service.Helper
{
    public class CommonFuncHelper
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CommonFuncHelper(IMapper mapper, ILogger<CommonFuncHelper> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // 消費税税率ID（5%)
        private const int TAX_5_PERCENT_ID = 1;
        // 消費税税率ID（8%)
        private const int TAX_8_PERCENT_ID = 2;
        // 消費税税率ID（10%)
        private const int TAX_10_PERCENT_ID = 3;
        // 消費税税率（5%)
        private const decimal TAX_5_PERCENT_VALUE = 0.05M;
        // 消費税税率（8%)
        private const decimal TAX_8_PERCENT_VALUE = 0.08M;
        // 消費税税率（10%)
        private const decimal TAX_10_PERCENT_VALUE = 0.1M;

        // デフォルトの消費税率を定義。10%へ完全以降が終わったら変更する
        private const int DEFAULT_TAX_ID = TAX_10_PERCENT_ID;
        // デフォルトの消費税率を定義。10%へ完全以降が終わったら変更する
        private const decimal DEFAULT_TAX_PERCENT_VALUE = TAX_10_PERCENT_VALUE;
        // 移行前の消費税の税率
        private const decimal OLD_TAX = TAX_8_PERCENT_VALUE;
        // 消費税率の移行日（併用期間開始日）
        private DateTime SWICH_DATE = DateTime.Parse("2019/09/18");
        // NULL値
        private DateTime NULL_DATE = default(DateTime);

        /// <summary>
        /// 消費税率を取得
        /// </summary>
        /// <returns></returns>
        public decimal getTax(DateTime uDate, decimal taxRatio, string userNo)
        {
            string vUserNo;
            int taxID;

            if (!NULL_DATE.Equals(uDate) & SWICH_DATE > uDate)
            {
                return OLD_TAX;
            }

            var vTax = taxRatio;

            if (vTax == -1M)
            {
                vUserNo = string.IsNullOrEmpty(userNo) ? "" : userNo;
                taxID = getTaxRatioID(vUserNo);

                if (taxID == TAX_5_PERCENT_ID)
                {
                    vTax = TAX_5_PERCENT_VALUE;
                }
                else if (taxID == TAX_8_PERCENT_ID)
                {
                    vTax = TAX_8_PERCENT_VALUE;
                }
                else if (taxID == TAX_10_PERCENT_ID)
                {
                    vTax = TAX_10_PERCENT_VALUE;
                }
                else
                {
                    vTax = DEFAULT_TAX_PERCENT_VALUE;
                }
            }

            return vTax;
        }

        /// <summary>
        /// 消費税税率ID取得
        /// </summary>
        /// <param name="inUserNo"></param>
        /// <returns></returns>
        public int getTaxRatioID(string inUserNo)
        {
            try
            {
                var taxRatioId = _unitOfWork.TaxRatioDefs.GetSingleOrDefault(t => t.UserNo == inUserNo).TaxRatioId.GetValueOrDefault();

                if (string.IsNullOrEmpty(taxRatioId.ToString()))
                {
                    return -1;
                }

                return taxRatioId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getTaxRatioID " + "CTAX-010D");
                return -1;
            }
        }


        /// <summary>
        /// 会員初期値データ取得
        /// </summary>
        /// <param name="inUserNo"></param>
        /// <returns></returns>
        public ResponseBase<UserDefModel> getUserDefData(string inUserNo)
        {
            try
            {
                var taxRatioId = _mapper.Map<UserDefModel>(_unitOfWork.UserDefs.Query(x => x.UserNo == inUserNo && x.Dflag == false));

                return ResponseHelper.Ok<UserDefModel>("Error", "CUSR-010D", taxRatioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getUserDefData " + "CUSR-010D");
                return ResponseHelper.Error<UserDefModel>("Error", "CUSR-010D");
            }
        }

        /// <summary>
        /// 会員番号デコード vEncNo:エンコードされた会員番号
        /// </summary>
        /// <param name="vEncNo"></param>
        /// <param name="vDecNo"></param>
        /// <returns></returns>
        public bool DecUserNo(string vEncNo, string vDecNo)
        {
            string wOne = "";
            string wStr = "";
            string wInt = "";
            int i = 0;

            try
            {
                // 文字数分ループ
                for (i = 1; i < vEncNo.Length; i++)
                {
                    wOne = CommonFunction.Mid(vEncNo, i, 1);
                    // 取り出した文字が英小文字(a～z)の場合
                    if (Strings.Asc(wOne) >= 97 && Strings.Asc(wOne) <= 122)
                    {
                        // 英小文字と数字がすでに格納されていれば1文字分デコード
                        if (wStr.Length == 1 && wInt.Length > 0)
                        {
                            vDecNo += Strings.Chr(Strings.Asc(wStr) - Convert.ToInt32(wInt));
                            // ワーククリア
                            wStr = "";
                            wInt = "";
                        }

                        //英小文字を格納
                        wStr = wOne;
                    }
                    else
                    {
                        // 数字を格納（'－'マイナスもありえる）
                        wInt += wOne;
                    }
                }

                // 最後の文字分のデコード
                vDecNo += Strings.Chr(Strings.Asc(wStr) - Convert.ToInt32(wInt));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - DecUserNo - GCMF-010C ◆会員認証エラー◆ 復号化前会員番号：{0}", vEncNo);
                return false;
            }
        }
    }
}