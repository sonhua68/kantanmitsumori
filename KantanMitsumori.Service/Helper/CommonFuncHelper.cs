using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using Microsoft.Extensions.Logging;
using System.Text;

namespace KantanMitsumori.Service.Helper
{
    public class CommonFuncHelper
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CommonSettings _commonSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        public CommonFuncHelper(IMapper mapper, ILogger<CommonFuncHelper> logger, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, CommonSettings commonSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonSettings = commonSettings; 
            _httpClientFactory = httpClientFactory;
        }

        #region Constant Initialization

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


        #endregion

        #region Public Function

        /// <summary>
        /// 消費税率を取得
        /// </summary>
        /// <returns></returns>
        public decimal getTax(DateTime uDate, decimal taxRatio, string userNo)
        {
            string vUserNo;
            int? taxID;

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
        public int? getTaxRatioID(string inUserNo)
        {
            try
            {
                var taxRatio = _unitOfWork.TaxRatioDefs.GetSingle(t => t.UserNo == inUserNo);

                if (taxRatio == null || string.IsNullOrEmpty(taxRatio.TaxRatioId.ToString()))
                {
                    return -1;
                }

                return taxRatio!.TaxRatioId;
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

        public UserDefModel? getUserDefData(string inUserNo)
        {
            try
            {
                var dtUserDef = _mapper.Map<UserDefModel>(_unitOfWork.UserDefs.GetSingle(x => x.UserNo == inUserNo && x.Dflag == false));

                return dtUserDef;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getUserDefData " + "CUSR-010D");
                return null;
            }
        }

        /// <summary>
        /// 会員番号デコード vEncNo:エンコードされた会員番号
        /// </summary>
        /// <param name="vEncNo"></param>
        /// <param name="vDecNo"></param>
        /// <returns></returns>
        public bool DecUserNo(string vEncNo, ref string vDecNo)
        {
            string wStr = "";
            string wInt = "";

            try
            {
                // 文字数分ループ
                foreach (char item in vEncNo)
                {
                    // 取り出した文字が英小文字(a～z)の場合
                    if (item >= 97 && item <= 122)
                    {
                        // 英小文字と数字がすでに格納されていれば1文字分デコード
                        if (wStr.Length == 1 && wInt.Length > 0)
                        {
                            vDecNo += Convert.ToChar(Convert.ToChar(wStr) - Convert.ToInt32(wInt));
                            // ワーククリア
                            wStr = "";
                            wInt = "";
                        }

                        //英小文字を格納
                        wStr = item.ToString();
                    }
                    else
                    {
                        // 数字を格納（'－'マイナスもありえる）
                        wInt += item;
                    }
                }

                // 最後の文字分のデコード
                vDecNo += Convert.ToChar(Convert.ToChar(wStr) - Convert.ToInt32(wInt));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - DecUserNo - GCMF-010C ◆会員認証エラー◆ 復号化前会員番号：{0}", vEncNo);
                return false;
            }
        }

        /// <summary>
        /// コーナータイプの取得
        /// 存在 = True　無し = False
        /// </summary>
        /// <param name="inCor"></param>
        /// <param name="intCornerType"></param>
        /// <returns></returns>
        public int GetCornerType(string inCor)
        {
            try
            {
                var dtTbSys = _unitOfWork.Syss.GetSingle(x => x.Corner == inCor);

                if (!string.IsNullOrEmpty(dtTbSys.Corner))
                {
                    return dtTbSys.CornerType;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - GetCornerType - GCMF-090D");
                return -1;
            }

        }


        /// <summary>
        /// 開催数の取得
        /// 存在 = True　無し = False
        /// </summary>
        /// <param name="inCor"></param>
        /// <param name="intAACount"></param>
        /// <returns></returns>
        public int GetAACount(string inCor)
        {
            try
            {
                var dtSys = _unitOfWork.Syss.GetSingle(x => x.Corner == inCor);

                if (dtSys != null)
                {
                    return Convert.ToInt32(dtSys.Aacount);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - GetAACount - GCMF-090D");
                return -1;
            }
        }

        // ***************************************************************************
        // * 画像のダウンロード
        // ***************************************************************************
        // 2011/04/12 商談メモ対応画像枚数追加
        // 2017/08/24 画像の保存名を指定できるようにI/F変更。指定されなかった場合は、URLのファイル名を利用する。
        public void DownloadImg(string url, string SesName, string DefImage, ref string fileName, string strSaveName)
        {
            try
            {
                Uri uri = new Uri(url);
                string strCarImgPlace;
                // 画像用に年月フォルダを作成する。
                strCarImgPlace = _commonSettings.PhysicalPathSettings.def_CarImgPlace;
                strCarImgPlace = strCarImgPlace + DateTime.Today.ToString("yyyMM") + "/";
                if (!Directory.Exists(strCarImgPlace))
                {
                    Directory.CreateDirectory(strCarImgPlace);
                }
                // 画像用に年月日フォルダを作成する。
                strCarImgPlace = strCarImgPlace + DateTime.Today.ToString("yyyMMdd") + "/";
                if (!Directory.Exists(strCarImgPlace))
                {
                    Directory.CreateDirectory(strCarImgPlace);
                }

                // 保存先のファイル名
                if (string.IsNullOrEmpty(strSaveName))
                {
                    fileName = strCarImgPlace + uri.Segments.LastOrDefault();
                }
                else
                {
                    fileName = Path.Combine(strCarImgPlace, strSaveName);
                }

                // Use HttpClient to download image
                var httpClient = _httpClientFactory.CreateClient();
                var task = httpClient.GetByteArrayAsync(uri);
                task.Wait();
                var data = task.Result;
                File.WriteAllBytes(fileName, task.Result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - DownloadImg - GCMF-030F 取得失敗 " + url);
                return;
            }
        }



        public void CheckImgPath(string strImagePath, string strSesName, string strDefImage, ref string strOutImagePath, string strImgSuffix, string cor, string fex)
        {
            string strOutImg = "";
            string strSaveName = "";
            if (string.IsNullOrEmpty(strImagePath))
            {
                strOutImagePath = "";
            }
            else
            {
                string strTempImagePath = strImagePath.ToUpper();
                if (!strTempImagePath.EndsWith(".JPG") & !strTempImagePath.EndsWith(".GIF") & !strTempImagePath.EndsWith(".PNG") & strImgSuffix is not null)
                {
                    strSaveName = cor + fex + strImgSuffix;
                }
                DownloadImg(strImagePath, strSesName, strDefImage, ref strOutImg, strSaveName);
                strOutImagePath = strOutImg;
            }
        }

        /// <summary>
        /// リサイクル預託金の取得
        /// 存在 = True　無し = False
        /// </summary>
        /// <param name="inCor"></param>
        /// <param name="inFullExhNum"></param>
        /// <param name="intTaxFreeRecycle"></param>
        /// <returns></returns>
        public int GetRecDeposit(string inCor, string inFullExhNum)
        {
            if (inFullExhNum.Length != 8)
            {
                return 0;
            }
            try
            {
                var dtPsinfos = _unitOfWork.Psinfos.GetSingle(x => x.Corner == inCor && x.ExhNum == inFullExhNum);

                if (dtPsinfos != null)
                {
                    return dtPsinfos.RecycleFlag == 1 ? Convert.ToInt32(dtPsinfos.RecyclingCharge) : 0;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // エラーログ書出し
                _logger.LogError(ex, "CommonFuncs - GetRecDeposit - GCMF-100D");
                return -1;
            }

        }

        /// <summary>
        /// 税金・保険料の自動計算
        /// 可 = True　不可 = Fals
        /// </summary>
        /// <param name="inMakerName"></param>
        /// <returns></returns>
        public bool enableTaxCalc(string inMakerName)
        {
            // メーカ名が "" の場合、自動計算対象とみなす
            if (string.IsNullOrEmpty(inMakerName))
                return true;

            // 除外リスト読み込み
            var enc = Encoding.GetEncoding("shift_jis");
            string[] arrExclusionList;
            try
            {
                string strExclusionList = File.ReadAllText(_commonSettings.PhysicalPathSettings.def_ExclusionListOfAutoCalc, enc);
                arrExclusionList = strExclusionList.Split("\r\n");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - enableTaxCalc - GCMF-110F");
                return false;
            }

            if (0 <= Array.IndexOf(arrExclusionList, inMakerName.Trim()))
                // 除外リストに存在する場合、自動計算不可
                return false;

            return true;
        }



        public int getCarTax(int intRegistMonth, int intExaust)
        {
            if (intExaust <= 660)
            {
                return 0;
            }
            int intYEAR_AMOUNT = getYearAmount(intExaust);
            if (intYEAR_AMOUNT == -1)
                return -1;
            int intPassedMonth = getCarTaxPassedMonth(intRegistMonth);
            decimal dblCarTax = Math.Round(intYEAR_AMOUNT * (intPassedMonth / 12.0m), 2);
            return (int)CommonFunction.ToRoundDown(dblCarTax, -2);
        }
        public bool getSelfInsurance(int intExaust, string inYear, string inMonth, int inUserDefMonth, ref int outSelfIns, ref int outRemIns)
        {
            try
            {
                DateTime vSyaken;
                int SyakenDiff = 0;
                outRemIns = inUserDefMonth > 0 ? inUserDefMonth : CommonConst.def_DamegeInsMonth25;
                if ((inYear != "" & inMonth != "") && CommonFunction.IsDate(inYear + "/" + inMonth + "/01"))
                {
                    vSyaken = DateTime.Parse(inYear + "/" + inMonth + "/01");
                    SyakenDiff = CommonFunction.DateDiff(IntervalEnum.Months, DateTime.Now, vSyaken);
                    if (SyakenDiff > 0)
                        outRemIns = SyakenDiff + 1;
                }
                int intCarType = intExaust > 660 ? 1 : 2;
                int intRemIns = outRemIns;
                var dtSelfInsurance = _unitOfWork.SelfInsurances.GetSingle(x => x.CarType == intCarType && x.RemainInspection == intRemIns && x.Dflag == false);
                if (dtSelfInsurance == null)
                    return false;
                outSelfIns = Convert.ToInt32(dtSelfInsurance.SelfInsurance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getSelfInsurance error ");
                return false;
            }

            return true;
        }
        public int? getSelfInsurance(int intExaust, int inRemIns)
        {
            try
            {
                int carType = 0;
                if (intExaust > 600)
                {
                    carType = 1;
                }
                else
                {
                    carType = 2;
                }

                var dt = _unitOfWork.SelfInsurances.GetSingle(x => x.CarType == carType && x.RemainInspection == inRemIns && x.Dflag == false);
                return dt.SelfInsurance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getSelfInsurance");
                return 0;
            }
        }

        #endregion

        #region Private Function 

        private int getYearAmount(int intTargetExault)
        {
            try
            {
                var dtCarTax = _unitOfWork.CarTaxs.GetSingle(x => x.ExaustFrom <= intTargetExault && x.ExaustTo >= intTargetExault && x.Dflag == false);
                if (dtCarTax != null)
                {
                    return Convert.ToInt32(dtCarTax.YearAmount);
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getYearAmount");
                return -1;
            }
        }
        private int getCarTaxPassedMonth(int intRegistMonth)
        {
            int intCloseMonth = 3;
            int intPassedMonth = -1;

            if (intRegistMonth > 3)
                intCloseMonth += 12;
            intPassedMonth = intCloseMonth - intRegistMonth;
            return intPassedMonth;
        }

        #endregion

    }
}