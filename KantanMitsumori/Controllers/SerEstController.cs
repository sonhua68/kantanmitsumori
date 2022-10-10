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
    public class SerEstController : BaseController
    {
        private readonly ISelCarService _selCarService;
        private readonly IEstimateService _estimateService;
        private readonly IEstMainService _estMainService;
        private readonly ISerEstService _serEstService;
        private readonly JwtSettings _jwtSettings;

        public SerEstController(ISelCarService selCarService, IEstimateService estimateService, IEstMainService estMainService, IOptions<JwtSettings> jwtSettings, ISerEstService serEstService) : base()
        {
            _selCarService = selCarService;
            _estimateService = estimateService;
            _estMainService = estMainService;
            _jwtSettings = jwtSettings.Value;
            _serEstService = serEstService;
        }

        #region SerEstController      
       
        /// <summary>
        /// Call from external system
        /// </summary>        
        public IActionResult Index(string mem, string mode)
        {
            if(string.IsNullOrEmpty(mem) || string.IsNullOrEmpty(mode))
                return View();

            var request = new RequestSerEstExternal()
            {
                Mem = mem,
                Mode = mode
            };
            var res = _serEstService.GenerateToken(request);
            if (res.ResultStatus == (int)enResponse.isError)
                ErrorAction(res);
            setTokenCookie(_jwtSettings.AccessExpires, res.Data?.Token ?? "");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData([FromForm] RequestSerEst requestData)
        {

            requestData.EstUserNo = _logToken.UserNo;
            var response = _selCarService.GetListSerEst(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
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
                return Ok(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEstimate(RequestSerEst requestData)
        {
            var response = await _estimateService.DeleteEstimate(requestData.EstNo!, requestData.EstSubNo!);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddEstimate(RequestSerEst requestData)
        {
            var response = await _estMainService.AddEstimate(requestData, _logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            setTokenCookie(_jwtSettings.AccessExpires, response.Data!);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CalcSum(RequestSerEst requestData)
        {
            var response = await _estMainService.CalcSum(requestData, _logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return Ok(response);
            }
            setTokenCookie(_jwtSettings.AccessExpires, response.Data!);
            return Ok(response);
        }
        #endregion SerEstController
    }
}
