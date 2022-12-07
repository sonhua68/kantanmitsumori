using AutoMapper;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpCarPriceController : BaseController
    {
        private readonly IInpCarPriceService _inpCarPriceService;
        private readonly IMapper _mapper;
        public InpCarPriceController(ILogger<InpCarPriceController> logger, IMapper mapper, IInpCarPriceService inpCarPriceService)
        {
            _inpCarPriceService = inpCarPriceService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            // Create request model map values from LogSession
            RequestInpCarPrice request = new RequestInpCarPrice();
            _mapper.Map(_logSession, request);
            // Get car price data
            var response = _inpCarPriceService.GetCarPriceInfo(request);
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            // Show view
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] RequestUpdateInpCarPrice requestData)
        {
            var model = new RequestUpdateCarPrice();
            _mapper.Map(requestData, model);
            var response = await _inpCarPriceService.UpdateCarPrice(model, _logSession!);
            return Ok(response);
        }
    }
}