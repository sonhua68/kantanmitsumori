using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
   
    public class InpCarController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpCarController> _logger;
        public InpCarController(IAppService appService, IEstimateService estimateService, IConfiguration config,  ILogger<InpCarController> logger):base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        #region InpCar     
        public IActionResult Index()
        {            
            RequestInp request = new RequestInp();
            request.EstNo = _logToken.sesEstNo;
            request.EstSubNo = _logToken.sesEstSubNo;
            request.UserNo = _logToken.UserNo;
            request.TaxRatio = _logToken.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }  

        [HttpPost]
        public async Task<IActionResult> UpdateInputCar([FromForm] RequestUpdateInputCar requestData)
        {
            var response = await _estimateService.UpdateInputCar(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        #endregion InpCar
    }
}