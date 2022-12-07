using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service
{
    public class InpInitValService : IInpInitValService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private CommonEstimate _commonEst;
        public InpInitValService(IMapper mapper, CommonEstimate commonEst, ILogger<InpInitValService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
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
                return ResponseHelper.Error<ResponseUserDef>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

            }
        }
        public async Task<ResponseBase<int>> UpdateInpInitVal(RequestUpdateInpInitVal model, LogSession LogSession)
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
                    mUserDef = UpdateMUserDef(mUserDef, model);
                    _unitOfWork.UserDefs.Update(mUserDef);
                }
                InsertAndUpdateTaxRationDef(model);
                await _unitOfWork.CommitAsync();
                if (model.ButtonSummit == "btnHanei")
                {
                    if (!await _commonEst.CalcSum(model.EstNo!, model.EstSubNo!, LogSession!))
                    {
                        return ResponseHelper.Error<int>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                    }
                    var dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                    var dtEstimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                    UpdateEstimates(model, dtEstimates, dtEstimateSubs);
                    UpdateEstimateSub(model, dtEstimates, dtEstimateSubs);
                    await _unitOfWork.CommitAsync();
                    if (!await _commonEst.CalcSum(model.EstNo!, model.EstSubNo!, LogSession!))
                    {
                        return ResponseHelper.Error<int>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                    }
                }
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpInitVal");
                return ResponseHelper.Error<int>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        #region Func Private 
        private MUserDef UpdateMUserDef(MUserDef mMUserDeOdl, RequestUpdateInpInitVal model)
        {

            var mMUserDef = _mapper.Map<MUserDef>(model);
            mMUserDef.Rdate = mMUserDeOdl.Rdate;
            mMUserDef.Udate = mMUserDeOdl.Udate;
            mMUserDef.Dflag = mMUserDeOdl.Dflag;
            return mMUserDef;
        }
        private MUserDef InsertMUserDef(RequestUpdateInpInitVal model)
        {

            var mMUserDef = new MUserDef();
            mMUserDef = _mapper.Map<MUserDef>(model);
            mMUserDef.Rdate = DateTime.Now;
            mMUserDef.Udate = DateTime.Now;
            mMUserDef.Dflag = false;
            return mMUserDef;
        }
        private void InsertAndUpdateTaxRationDef(RequestUpdateInpInitVal model)
        {
            var taxRatioDef = new TTaxRatioDef();
            var data = _unitOfWork.TaxRatioDefs.Query(n => n.UserNo == model.UserNo).ToList();
            if (data == null)
            {

                taxRatioDef.UserNo = model.UserNo!;
                taxRatioDef.TaxRatioId = CommonConst.TAX_10_PERCENT_ID;
                _unitOfWork.TaxRatioDefs.Add(taxRatioDef);
            }
            else
            {
                taxRatioDef.TaxRatioId = CommonConst.TAX_10_PERCENT_ID;
                _unitOfWork.TaxRatioDefs.Update(taxRatioDef);
            }
        }
        private void UpdateEstimates(RequestUpdateInpInitVal model, TEstimate dtEstimates, TEstimateSub dtEstimateSubs)
        {

            if (dtEstimates != null && dtEstimateSubs != null)
            {
                bool isCheckHaiKi = model.Haiki > 660;
                dtEstimates!.ConTaxInputKb = model.ConTaxInputKb;
                dtEstimates.Rate = model.Rate;
                dtEstimates.TaxCheck = isCheckHaiKi ? model.TaxCheckH : model.TaxCheckK;
                dtEstimates.TaxGarage = isCheckHaiKi ? model.TaxGarageH : model.TaxGarageK;
                if (dtEstimates.TradeInPrice > 0 || dtEstimates.Balance > 0)
                {
                    dtEstimates.TaxTradeIn = isCheckHaiKi ? model.TaxTradeInH : model.TaxTradeInK;
                    dtEstimates.TaxFreeTradeIn = isCheckHaiKi ? model.TaxFreeTradeInH : model.TaxFreeTradeInK;
                }
                dtEstimates.TaxRecycle = isCheckHaiKi ? model.TaxRecycleH : model.TaxRecycleK;
                dtEstimates.TaxDelivery = isCheckHaiKi ? model.TaxDeliveryH : model.TaxDeliveryK;
                dtEstimates.TaxFreeCheck = isCheckHaiKi ? model.TaxFreeCheckH : model.TaxFreeCheckK;
                dtEstimates.TaxFreeGarage = isCheckHaiKi ? model.TaxFreeGarageH : model.TaxFreeGarageK;
                if (model.sesMode == "0" && dtEstimates.EstInpKbn != "2")
                {
                    var price = isCheckHaiKi ? (model.YtiRiekiH - dtEstimateSubs.YtiRieki) : (model.YtiRiekiK - dtEstimateSubs.YtiRieki);
                    dtEstimates.CarPrice = dtEstimates.CarPrice + (price);
                }
                if (model.SyakenNewZok!.Contains("zok"))
                {
                    dtEstimates.SyakenZok = isCheckHaiKi ? model.SyakenZokH : model.SyakenZokK;
                }
                else
                {
                    dtEstimates.SyakenNew = isCheckHaiKi ? model.SyakenNewH : model.SyakenNewK;
                }
                dtEstimates.ShopNm = model.ShopNm;
                dtEstimates.ShopAdr = model.ShopAdr;
                dtEstimates.ShopTel = model.ShopTel;
                dtEstimates.EstTanName = model.EstTanName;
                dtEstimates.Udate = DateTime.Now;
                dtEstimates.SekininName = model.SekininName;
                _unitOfWork.Estimates.Update(dtEstimates!);

            }
        }
        private void UpdateEstimateSub(RequestUpdateInpInitVal model, TEstimate dtEstimates, TEstimateSub dtEstimateSubs)
        {

            if (dtEstimateSubs != null)
            {
                bool isCheckHaiKi = model.Haiki > 660;
                dtEstimateSubs.TaxSet1Title = model.TaxSet1Title;
                dtEstimateSubs.TaxSet1 = isCheckHaiKi ? model.TaxSet1H : model.TaxSet1K;
                dtEstimateSubs.TaxSet2Title = model.TaxSet2Title;
                dtEstimateSubs.TaxSet2 = isCheckHaiKi ? model.TaxSet2H : model.TaxSet2K;
                dtEstimateSubs.TaxSet3Title = model.TaxSet3Title;
                dtEstimateSubs.TaxSet3 = isCheckHaiKi ? model.TaxSet3H : model.TaxSet3K;
                dtEstimateSubs.TaxFreeSet1Title = model.TaxFreeSet1Title;
                dtEstimateSubs.TaxFreeSet1 = isCheckHaiKi ? model.TaxFreeSet1H : Convert.ToInt32(model.TaxFreeSet1K);
                dtEstimateSubs.TaxFreeSet2Title = model.TaxFreeSet2Title;
                dtEstimateSubs.TaxFreeSet2 = isCheckHaiKi ? model.TaxFreeSet2H : model.TaxFreeSet2K;
                if (model.sesMode == "0" && dtEstimates.EstInpKbn != "2")
                {
                    dtEstimateSubs.YtiRieki = isCheckHaiKi ? model.YtiRiekiH : model.YtiRiekiK;
                }
                if (dtEstimates.TradeInPrice > 0 || dtEstimates.Balance > 0)
                {
                    dtEstimateSubs.TaxTradeInSatei = isCheckHaiKi ? model.TaxTradeInChkH : model.TaxTradeInChkK;
                }
                dtEstimateSubs.Udate = DateTime.Now;
                _unitOfWork.EstimateSubs.Update(dtEstimateSubs);
            }

        }
        #endregion Func Private 
    }

}