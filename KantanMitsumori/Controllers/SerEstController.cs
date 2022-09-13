using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace KantanMitsumori.Controllers
{
    public class SerEstController : BaseController
    {
        private readonly IAppService _appService;

        private readonly ILogger<SerEstController> _logger;
        private readonly ISelCarService _selCarService;
        public SerEstController(IAppService appService, ISelCarService selCarService, IConfiguration config, ILogger<SerEstController> logger) : base(config)
        {
            _appService = appService;
            _logger = logger;
            _selCarService = selCarService;
        }

        #region SerEstController 
        public async Task<IActionResult> Index([FromForm] RequestSerEst requestData)
        {
            var response = _selCarService.GetListSerEst(requestData);           
            var dt = await PaginatedList<ResponseSerEst>.CreateAsync(response.Data!.AsQueryable(), requestData.pageNumber, requestData.pageSize);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }           
            return View(dt);
        }




        #endregion SerEstController
    }
}
