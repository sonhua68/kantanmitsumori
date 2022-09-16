using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Models;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace KantanMitsumori.Controllers
{
    public class SerEstController : BaseController
    {
        private readonly IAppService _appService;
        private readonly ILogger<SerEstController> _logger;
        private readonly ISelCarService _selCarService;
        private readonly IEstimateService _estimateService;
        public SerEstController(IAppService appService, ISelCarService selCarService, IEstimateService estimateService, IConfiguration config, ILogger<SerEstController> logger) : base(config)
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
            _estimateService = estimateService;
        }

        #region SerEstController      
        public  IActionResult Index()
        {
        
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoadData([FromForm] RequestSerEst requestData)
        {

            requestData.EstUserNo = _logToken.UserNo;
            var response = _selCarService.GetListSerEst(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            var dt = await PaginatedList<ResponseSerEst>.CreateAsync(response.Data!.AsQueryable(), requestData.pageNumber, requestData.pageSize);
            if (dt.Count > 0)
            {
                dt[0].TotalPages = dt.TotalPages;
                dt[0].PageIndex = dt.PageIndex;
            }           
            return Ok(dt);
        }
        [HttpGet]
        public IActionResult GetMakerNameAndModelName(string makerName)
        {
            var response = _estimateService.GetMakerNameAndModelName(_logToken.UserNo!, makerName);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEstimate(RequestSerEst requestData)
        {
            var response = await _estimateService.DeleteEstimate(requestData.EstNo!, requestData.EstSubNo!);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        #endregion SerEstController
    }
}
