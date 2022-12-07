using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KantanMitsumori.Controllers
{
    public class ReportController : BaseController
    {       
        private readonly URLSettings _urlSettings;
        private readonly IMapper _mapper;
        private readonly CommonFuncHelper _commonFuncHelper;

        public ReportController(IMapper mapper,IOptions<URLSettings> urlSettings, CommonFuncHelper commonFuncHelper)
        {        
            _mapper = mapper;
            _urlSettings = urlSettings.Value;
            _commonFuncHelper = commonFuncHelper;
        }

        /// <summary>
        /// Download estimate report
        /// </summary>        
        public async Task<IActionResult> DownloadEstimateReport()
        {
            var requestModel = new RequestReport();
            _mapper.Map(_logSession, requestModel);
            requestModel.ReportType = ReportType.Estimate;
            var httpResponseMessage = await _commonFuncHelper.ExportFile("ReportEstimate", requestModel);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseFileError>(responseString);
                if (response!.ResultStatus != (int)enResponse.isError)
                {
                    var responseData = JsonConvert.DeserializeObject<ResponseFileModel>(responseString);
                    return File(responseData!.Data, responseData.ContentType, responseData.Name);
                }
                var responseError = ResponseHelper.Error<ResponseFileModel>(response.messageCode, response.messageContent);
                return ErrorAction(responseError);
            }
            else
            {
                var response = ResponseHelper.Error<ResponseFileModel>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST050S));
                return ErrorAction(response);
            }
        }


        /// <summary>
        /// Download order report
        /// </summary>        
        public async Task<IActionResult> DownloadOrderReport()
        {
            var requestModel = new RequestReport();
            _mapper.Map(_logSession, requestModel);
            requestModel.ReportType = ReportType.Order;
            var httpResponseMessage = await _commonFuncHelper.ExportFile("ReportOrder", requestModel);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseFileError>(responseString);
                if (response!.ResultStatus != (int)enResponse.isError)
                {
                    var responseData = JsonConvert.DeserializeObject<ResponseFileModel>(responseString);
                    return File(responseData!.Data, responseData.ContentType, responseData.Name);
                }
                var responseError = ResponseHelper.Error<ResponseFileModel>(response.messageCode, response.messageContent);
                return ErrorAction(responseError);
            }
            else
            {
                var response = ResponseHelper.Error<ResponseFileModel>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST050S));
                return ErrorAction(response);
            }
        }

    }



}
