using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models.StateModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppService _appService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IAppService appService, ILogger<HomeController> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Header()
        {
            var mode = new LogToken();
            mode.EstNo = "22071200085"; mode.EstSubNo = "01";
            mode.UserNo = "88888195";
            mode.UserNm = "testuser88888195";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;
            setTokenCookie(token);
            return PartialView("_Header", mode);
        }

        [HttpPost]
        public async Task<JsonResult> TestSummitFormAjax(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);
            return Json(response);
        }
        public async Task<IActionResult> Test(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);


            if (response.ResultStatus == 0)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        public IActionResult GetEstMain()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetEstMain([FromQuery] RequestHeaderModel request)
        {
            var comFuncs = new CommonFunction();

            var stateModel = new StateModel();
            stateModel.stateLoadWindow = "EstMain";

            if (Strings.InStr(request.IsInputBack, "1") == 0 && (String.IsNullOrEmpty(request.Sel) ? "0" : request.Sel) == "0")
            {
                stateModel.sesPriDisp = "0";
            }

            if (Strings.InStr(request.IsInputBack, "1") == 0 && (String.IsNullOrEmpty(request.Sel) ? "0" : request.Sel) == "1")
            {
                // セッションに保持していた会員ユーザーのお客様の情報をクリア
                setSesCustInfo(request, stateModel);
            }

            if (Strings.InStr(request.IsInputBack, "1") == 0)
            {
                Uri pageUrl;

                try
                {
                    string headRef = Request.Headers["Referer"];

                    pageUrl = new Uri(headRef);
                }
                catch (Exception)
                {
                    pageUrl = new Uri("http://www.asnet2.com/asest2/test.html");
                }

                if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
                {
                    // セッション情報クリア（ASNET/店頭商談NET の情報混在回避のため）
                    TempData.Clear();

                    stateModel.stateLoadWindow = "EstMain";

                    // ASNET、店頭商談NETの判定
                    if (request.Mode != "" && comFuncs.IsNumeric(request.Mode!))
                    {
                        stateModel.sesMode = request.Mode;
                    }
                    else
                    {
                        stateModel.sesMode = "";
                        stateModel.sesErrMsg = CommonConst.def_ErrMsg4 + CommonConst.def_ErrMsg4 + "SMAI-001P" + CommonConst.def_ErrCodeR;
                        Redirect(CommonConst.def_ErrPage);
                    }

                    // 価格表示有無の取得（店頭商談NET
                    if (request.PriDisp != "" && comFuncs.IsNumeric(request.PriDisp!))
                    {
                        stateModel.sesPriDisp = request.PriDisp;
                    }

                    if (request.leaseFlag != "" && comFuncs.IsNumeric(request.leaseFlag!))
                    {
                        stateModel.sesLeaseFlag = request.leaseFlag;
                    }
                    else
                    {
                        stateModel.sesLeaseFlag = "0";
                    }

                    // ASNET車両詳細ページからの情報を取得・DB保存
                    getAsnetInfo(request, stateModel);
                }
            }


            return View();
        }


        #region Private Function

        /// <summary>
        /// 会員ユーザーのお客様の情報をセッションにセット
        /// または、セッションに保持していた会員ユーザーのお客様の情報をクリア
        /// </summary>
        /// <param name="request"></param>
        /// <param name="stateModel"></param>
        /// <param name="flgRemove"></param>
        private void setSesCustInfo([FromQuery] RequestHeaderModel request, StateModel stateModel, bool flgRemove = false)
        {
            if (flgRemove)
            {
                TempData.Remove("sesCustNm_forPrint");
                TempData.Remove("sesCustZip_forPrint");
                TempData.Remove("sesCustAdr_forPrint");
                TempData.Remove("sesCustTel_forPrint");
            }
            else
            {
                stateModel.sesCustNm_forPrint = request.formEstMainModel!.CustNm_forPrint;
                stateModel.sesCustZip_forPrint = request.formEstMainModel!.CustZip_forPrint;
                stateModel.sesCustAdr_forPrint = request.formEstMainModel!.CustAdr_forPrint;
                stateModel.sesCustTel_forPrint = request.formEstMainModel!.CustAdr_forPrint;
            }
        }

        /// <summary>
        /// ASNET車両ページからの情報を取得、DB保存
        /// </summary>
        private void getAsnetInfo(RequestHeaderModel request, StateModel stateModel)
        {
            if (request.cot == "" || request.cna == "" || request.mem == "")
            {
                stateModel.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-020P" + CommonConst.def_ErrCodeR;
                Redirect(CommonConst.def_ErrPage);
                return;
            }

            string strTempImagePath;
            string strSavePath;

            // ユーザー情報
            string vUserNo = "", vUserNm = "", vUserAdr = "", vUserTel = "", dispUserInf = "";

            // 会員番号取得
            string encUsrNo = request.mem!.Trim();
            string decUsrNo = "";

            var comFuncs = new CommonFunction();

            if (!comFuncs.DecUserNo(encUsrNo, decUsrNo))
            {
                stateModel.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-021C" + CommonConst.def_ErrCodeR;
                Redirect(CommonConst.def_ErrPage);
            }


        }

        #endregion

    }
}