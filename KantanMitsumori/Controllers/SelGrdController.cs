using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Models;
using KantanMitsumori.Service.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class SelGrdController : BaseController
    {
        private readonly IEstMainService _appService;

        private readonly ILogger<SelGrdController> _logger;
        private readonly ISelCarService _selCarService;
        private readonly IEstMainService _estMainService;
        public SelGrdController(IEstMainService appService, ISelCarService selCarService, IEstMainService estMainService ,IConfiguration config, ILogger<SelGrdController> logger) : base(config)
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
            _estMainService = estMainService;
        }

        #region SelCar 
        public async Task<IActionResult> Index([FromForm] RequestSelGrd requestData)
        {
            var response = _selCarService.GetListRuiBetSu(requestData);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            var dt = await PaginatedList<ResponseTbRuibetsuNew>.CreateAsync(response.Data!.AsQueryable(), requestData.pageNumber, requestData.pageSize);
            return View(dt);
        }


        [HttpPost]
        public async Task<IActionResult> LoadData([FromForm] RequestSelGrd requestData)
        {

            var response = _selCarService.GetListRuiBetSu(requestData);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            var dt = await PaginatedList<ResponseTbRuibetsuNew>.CreateAsync(response.Data!.AsQueryable(), requestData.pageNumber, requestData.pageSize);

            if (dt.Count > 0)
            {
                dt[0].TotalPages = dt.TotalPages;
                dt[0].PageIndex = dt.PageIndex;
            }
            return Ok(dt);
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
