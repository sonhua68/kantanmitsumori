using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.ASEST
{
    public class SerEstService : ISerEstService
    {
        IEstMainService _estMainService { get; set; }
        ILogger _logger { get; set; }
        JwtSettings _jwtSettings;
        public SerEstService(IEstMainService estMainService, ILogger<SerEstService> logger, IOptions<JwtSettings> jwtSettings)
        {
            _estMainService = estMainService;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public ResponseBase<LogToken> GenerateToken(RequestSerEstExternal model)
        {
            try
            {
                // Validate model data
                if(string.IsNullOrEmpty(model.Mem) || string.IsNullOrEmpty(model.Mode))
                    return ResponseHelper.Error<LogToken>(HelperMessage.SMAL040S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL040S));

                // Decode userNo and load data of user
                var userInfo = _estMainService.getUserInfo(model.Mem);
                if (userInfo.ResultStatus != 0)
                    return ResponseHelper.Error<LogToken>(userInfo.MessageCode, userInfo.MessageCode);

                var token = new LogToken()
                {
                    UserNo = userInfo.Data!.UserNo,
                    UserNm = userInfo.Data!.UserNm,
                    sesMode = model.Mode,

                };
                token.Token = HelperToken.GenerateJsonToken(_jwtSettings, token);

                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), token);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetAsnetInfo");
                return ResponseHelper.Error<LogToken>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
            }            
        }
    }
}
