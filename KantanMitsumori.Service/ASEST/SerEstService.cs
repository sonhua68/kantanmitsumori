using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
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
        private readonly ILogger _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CommonFuncHelper _commonFuncHelper;

        public SerEstService(ILogger<SerEstService> logger,
            IOptions<JwtSettings> jwtSettings,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;
        }

        public ResponseBase<LogToken> GenerateToken(RequestSerEstExternal model)
        {
            try
            {                
                // Decode userNo
                string decUsrNo = "";
                if (!_commonFuncHelper.DecUserNo(model.Mem.Trim(), ref decUsrNo))
                    return ResponseHelper.Error<LogToken>(HelperMessage.SSLE011C, KantanMitsumoriUtil.GetMessage(HelperMessage.SSLE011C));

                // Get userInfo
                var userInfo = getUserName(decUsrNo);
                if(userInfo == null)
                    return ResponseHelper.Error<LogToken>(HelperMessage.SSLE012D, KantanMitsumoriUtil.GetMessage(HelperMessage.SSLE012D));

                // Create token
                var token = new LogToken()
                {
                    UserNo = userInfo.UserNo,
                    UserNm = userInfo.UserNm,
                    sesMode = model.Mode,
                };
                token.Token = HelperToken.GenerateJsonToken(_jwtSettings, token);

                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(HelperMessage.I0002), token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAsnetInfo");
                return ResponseHelper.Error<LogToken>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST050S));
            }
        }

        private UserModel? getUserName(string userNo)
        {
            try
            {
                return _mapper.Map<UserModel>(_unitOfWork.Users.GetSingle(x => x.UserNo == userNo));                                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "◆会員認証エラー◆ 復号化後会員番号：", userNo);
                return null;
            }
        }        
    }
}
