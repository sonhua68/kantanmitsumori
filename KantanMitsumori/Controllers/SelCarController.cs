using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class SelCarController : BaseController
    {
        private readonly IAppService _appService;

        private readonly ILogger<SelCarController> _logger;
        private readonly ISelCarService _selCarService;
        public SelCarController(IAppService appService, ISelCarService selCarService, IConfiguration config, ILogger<SelCarController> logger) : base(config)
        {
            _appService = appService;

            _logger = logger;
            _selCarService = selCarService;
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

        #endregion SelCar
    }
}
