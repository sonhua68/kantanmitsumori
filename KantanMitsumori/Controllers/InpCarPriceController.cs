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
        private readonly ILogger<InpCarPriceController> _logger;
        private readonly IMapper _mapper;
        public InpCarPriceController(ILogger<InpCarPriceController> logger
            , IMapper mapper
            , IInpCarPriceService inpCarPriceService
            ) : base()
        {
            _inpCarPriceService = inpCarPriceService;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            // Create request model map values from logToken
            RequestInpCarPrice request = new RequestInpCarPrice();
            _mapper.Map(_logToken, request);
            // Get car price data
            var response = _inpCarPriceService.GetCarPriceInfo(request);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            // Show view
            return View("Index", response.Data);
        }

        [HttpPost]
        public  IActionResult Update(RequestUpdateInpCarPrice requestData)
        {
            // Create model for update car price
            var model = new RequestUpdateCarPrice();
            _mapper.Map(requestData, model);
            // Get car price data
            var response = _inpCarPriceService.Update(model);
            return Json(response);
        }

        public IActionResult Demo()
        {
            CreateLogTokenForDemo();
            return Index();
        }

        private void CreateLogTokenForDemo()
        {
            _logToken = new LogToken()
            {
                sesEstNo = "22092900010",
                sesEstSubNo = "01",
                UserNo = "88888195",
                UserNm = "DANGPHAM"
            };
        }
    }
}