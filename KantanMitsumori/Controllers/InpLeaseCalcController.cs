using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpLeaseCalcController : BaseController
    {


        private readonly ILogger<InpLeaseCalcController> _logger;
        private readonly IInpLeaseCalcService _inpLeaseCalc;
        private readonly IEstMainService _estMainService;
        public InpLeaseCalcController(IInpLeaseCalcService inpLeaseCalc, IEstMainService estMainService, IConfiguration config, ILogger<InpLeaseCalcController> logger) : base(config)
        {
            _logger = logger;
            _inpLeaseCalc = inpLeaseCalc;
            _estMainService = estMainService;
        }

        #region InpLeaseCalc 
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCarType()
        {
            //var response = await _estMainService.setFreeEst(requestData, _logToken);
            //if (response.ResultStatus == (int)enResponse.isError)
            //{
            //    return ErrorAction(response);
            //}
            //setTokenCookie(response.Data!.AccessToken);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LeaseCalc(RequestSelGrdFreeEst requestData)
        {
            var response = await _estMainService.setFreeEst(requestData, _logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            setTokenCookie(response.Data!.AccessToken);
            return Ok(response);
        }

        #endregion InpLeaseCalc
    }
}
