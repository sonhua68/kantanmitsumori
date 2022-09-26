using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class SelCarController : BaseController
    {
        private readonly IEstMainService _appService;

        private readonly ILogger<SelCarController> _logger;
        private readonly ISelCarService _selCarService;
        private readonly IEstMainService _estMainService;
        public SelCarController(IEstMainService appService, ISelCarService selCarService, IEstMainService estMainService, IConfiguration config, ILogger<SelCarController> logger) : base(config)
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
            _estMainService = estMainService;
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
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetListASOPCarName(int markId)
        {
            var response = _selCarService.GetListASOPCarName(markId);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NextGrade([FromForm] RequestSelGrd requestData)
        {
            var response = _selCarService.chkGetListRuiBetSu(requestData, (int)enTypeButton.isNextGrade);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
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
                return ErrorAction(response);
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
                return ErrorAction(response);
            }
            setTokenCookie(response.Data!.AccessToken);
            return Ok(response);
        }

        #endregion SelCar
    }
}
