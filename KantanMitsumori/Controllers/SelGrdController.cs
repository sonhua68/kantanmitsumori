using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class SelGrdController : BaseController
    {
        private readonly IAppService _appService;

        private readonly ILogger<SelGrdController> _logger;
        private readonly ISelCarService _selCarService;
        public SelGrdController(IAppService appService, ISelCarService selCarService, IConfiguration config, ILogger<SelGrdController> logger) : base(config)
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
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




        #endregion SelCar
    }
}
