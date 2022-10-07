using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Controllers
{
    public class SelGrdController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly ILogger<SelGrdController> _logger;
        private readonly ISelCarService _selCarService;
        private readonly IEstMainService _estMainService;
        private readonly CommonSettings _commonSettings;

        public SelGrdController(IEstMainService appService, ISelCarService selCarService, IEstMainService estMainService, ILogger<SelGrdController> logger, IOptions<CommonSettings> commonSettings) : base()
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
            _estMainService = estMainService;
            _commonSettings = commonSettings.Value;
        }

        #region SelCar 
        public async Task<IActionResult> Index([FromForm] RequestSelGrd requestData)
        {
            var response = _selCarService.GetListRuiBetSu(requestData);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
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
                return Ok(response);
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
                return Ok(response);
            }
            setTokenCookie(_commonSettings.JwtSettings.AccessExpires, response.Data!.AccessToken);
            return Ok(response);
        }
        #endregion SelCar
    }
}
