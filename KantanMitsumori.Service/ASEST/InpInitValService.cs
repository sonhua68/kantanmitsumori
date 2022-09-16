using AutoMapper;
using GrapeCity.Enterprise.Data.VisualBasicReplacement;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace KantanMitsumori.Service
{
    public class InpInitValService : IInpInitValService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InpInitValService(IMapper mapper, ILogger<InpInitValService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public ResponseBase<ResponseUserDef> GetUserDefData(string userNo)
        {
            try
            {
                var UserDefs = _unitOfWork.UserDefs.Query(n => n.UserNo == userNo && n.Dflag == false).Select(i => _mapper.Map<ResponseUserDef>(i)).FirstOrDefault();
                if (UserDefs == null)
                {
                    return ResponseHelper.Error<ResponseUserDef>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<ResponseUserDef>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), UserDefs!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserDefData");
                return ResponseHelper.Error<ResponseUserDef>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
        }

        public async Task<ResponseBase<int>> UpdateInpInitVal(RequestUpdateInpInitVal model)
        {
            try
            {
                var mUserDef = _unitOfWork.UserDefs.GetSingle(n => n.UserNo == model.UserNo && n.Dflag == false);
                if (mUserDef == null)
                {
                    var mUserDefnew = InsertMUserDef(model);
                    _unitOfWork.UserDefs.Add(mUserDefnew);
                }
                else
                {
                    mUserDef = UpdateMUserDef(model);
                    _unitOfWork.UserDefs.Update(mUserDef);
                }
                InsertAndUpdateTaxRationDef(model);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpInitVal");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
        #region Update

        private MUserDef UpdateMUserDef(RequestUpdateInpInitVal model)
        {
            var mMUserDef = new MUserDef();
            return mMUserDef;
        }
        private MUserDef InsertMUserDef(RequestUpdateInpInitVal model)
        {
            var mMUserDef = new MUserDef();
            return mMUserDef;
        }
        private void InsertAndUpdateTaxRationDef(RequestUpdateInpInitVal model)
        {
            var taxRatioDef = new TTaxRatioDef();
            var data = _unitOfWork.TaxRatioDefs.Query(n => n.UserNo == model.UserNo).ToList();
            if (data == null)
            {

                taxRatioDef.UserNo = model.UserNo!;
                taxRatioDef.TaxRatioId = 0;
                _unitOfWork.TaxRatioDefs.Add(taxRatioDef);
            }
            else
            {
                taxRatioDef.TaxRatioId = 0;
                _unitOfWork.TaxRatioDefs.Update(taxRatioDef);
            }
        }
        #endregion Update
    }



}