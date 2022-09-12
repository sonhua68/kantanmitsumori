using AutoMapper;
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


namespace KantanMitsumori.Service.ASEST
{
    internal class SelCarService : ISelCarService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public SelCarService(IMapper mapper, ILogger<SelCarService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public ResponseBase<List<ResponseTbRuibetsuN>> chkGetListRuiBetSu(RequestSelGrd requestSel, int Flg)
        {
            try
            {
                var tbRuibetsuList = new List<ResponseTbRuibetsuN>();
                if (Flg == (int)enTypeButton.isNextGrade) // btnNextGrade_Click
                {
                    tbRuibetsuList = _unitOfWork.RuibetsuNs.GetList(n => n.MakerId == requestSel.sesMakID
                    && n.ModelName == requestSel.sesCarNM).Select(i => _mapper.Map<ResponseTbRuibetsuN>(i))
                    .OrderBy(n => n.GradeName).OrderBy(n => n.RegularCase).OrderBy(n => n.DispVol)
                    .ToList();

                }
                else // 2 =btnChkModel_Click
                {
                    if (!string.IsNullOrEmpty(requestSel.KbnSet))
                    {
                        tbRuibetsuList = _unitOfWork.RuibetsuNs.GetList(n => n.SetNumber == requestSel.CaseSet
                        && n.ClassNumber == requestSel.KbnSet).Select(i => _mapper.Map<ResponseTbRuibetsuN>(i))
                        .OrderBy(n => n.GradeName).OrderBy(n => n.RegularCase).OrderBy(n => n.DispVol).OrderBy(n => n.DriveTypeCode)
                       .ToList();
                    }
                    else
                    {
                        tbRuibetsuList = _unitOfWork.RuibetsuNs.GetList(n =>
                        n.SetNumber == requestSel.CaseSet).Select(i => _mapper.Map<ResponseTbRuibetsuN>(i))
                        .OrderBy(n => n.GradeName).OrderBy(n => n.RegularCase).OrderBy(n => n.DispVol)
                        .OrderBy(n => n.DriveTypeCode).ToList();

                    }

                }
                if (tbRuibetsuList.Count == 0)
                {

                    return ResponseHelper.Ok<List<ResponseTbRuibetsuN>>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003));
                }
                return ResponseHelper.Ok<List<ResponseTbRuibetsuN>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), tbRuibetsuList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "chkCar");
                return ResponseHelper.Error<List<ResponseTbRuibetsuN>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
        }

        public ResponseBase<List<ResponseAsopCarname>> GetListASOPCarName(int makId)
        {
            try
            {
                var carNamesList = _unitOfWork.ASOPCarNames.GetList(n => n.MekerCode == makId).Select(i => _mapper.Map<ResponseAsopCarname>(i)).OrderBy(n => n.CarmodelName).ToList();
                if (carNamesList == null)
                {
                    return ResponseHelper.Error<List<ResponseAsopCarname>>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<List<ResponseAsopCarname>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), carNamesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListASOPCarName");
                return ResponseHelper.Error<List<ResponseAsopCarname>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }

        }

        public ResponseBase<List<ResponseAsopMaker>> GetListASOPMakers()
        {
            try
            {
                var makerList = _unitOfWork.ASOPMakers.GetAll().Select(i => _mapper.Map<ResponseAsopMaker>(i)).OrderBy(n => n.MakerCode).ToList();
                if (makerList == null)
                {
                    return ResponseHelper.Error<List<ResponseAsopMaker>>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<List<ResponseAsopMaker>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), makerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListASOPMakers");
                return ResponseHelper.Error<List<ResponseAsopMaker>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }

        }


    }
}
