using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Controllers
{
    public class SelCarController : BaseController
    {
        private readonly ISelCarService _selCarService;
        private readonly IEstMainService _estMainService;
        private readonly JwtSettings _jwtSettings;

        public SelCarController(ISelCarService selCarService, IEstMainService estMainService, ILogger<SelCarController> logger, IOptions<JwtSettings> jwtSettings)
        {
            _selCarService = selCarService;
            _estMainService = estMainService;
            _jwtSettings = jwtSettings.Value;
        }

        #region SelCar 
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetListASOPMaker()
        {
            var response = _selCarService.GetListASOPMakers();
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetListASOPCarName(int markId)
        {
            var response = _selCarService.GetListASOPCarName(markId);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NextGrade([FromForm] RequestSelGrd requestData)
        {
            var response = _selCarService.chkGetListRuiBetSu(requestData, (int)enTypeButton.isNextGrade);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            else if (response.Data != null)
            {
                requestData.TypeButton = (int)enTypeButton.isNextGrade;
                return Ok(requestData);

            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult ChkModel([FromForm] RequestSelGrd requestData)
        {
            var response = _selCarService.chkGetListRuiBetSu(requestData, (int)enTypeButton.isChkModel);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            else if (response.Data != null)
            {
                requestData.TypeButton = (int)enTypeButton.isChkModel;
                requestData.sesMakID = response.Data[0].MakerId;
                requestData.sesMaker = response.Data[0].MakerName;
                requestData.sesCarNM = response.Data[0].ModelName;
                return Ok(requestData);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SetFreeEst(RequestSelGrdFreeEst requestData)
        {
            var response = await _estMainService.setFreeEst(requestData, _logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            setTokenCookie(_jwtSettings.AccessExpires, response.Data!.AccessToken);
            return Ok(response);
        }

        #endregion SelCar
    }
}
