using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Service.ASEST
{
    public class InpNotesService : IInpNotesService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;

        public InpNotesService(IMapper mapper, ILogger<InpNotesService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
        }

        public ResponseBase<ResponseInpNotes> getInfoNotes(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estSubData = _commonEst.getEstSubData(estNo, estSubNo);

                if (estSubData == null)
                {
                    return ResponseHelper.Error<ResponseInpNotes>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-041D" + CommonConst.def_ErrCodeR);
                }

                var model = new ResponseInpNotes();
                model.EstNo = estSubData.EstNo;
                model.EstSubNo = estSubData.EstSubNo;
                string[] arrNotes = string.IsNullOrWhiteSpace(estSubData.Notes) ? new string[2] { "", "" } : estSubData.Notes.Split(Constants.vbCrLf);
                model.Notes1 = arrNotes[0];
                model.Notes2 = arrNotes[1];

                return ResponseHelper.Ok<ResponseInpNotes>("OK", "OK", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHelper.Error<ResponseInpNotes>("Error", "Error");
            }
        }

        public async Task<ResponseBase<int>> UpdateInpNotes(RequestUpdateInpNotes model)
        {
            try
            {
                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == model.EstNo && x.EstSubNo == model.EstSubNo && x.Dflag == false);

                var strNotes = model.Notes1 + Constants.vbCrLf + model.Notes2;
                if (strNotes == Constants.vbCrLf)
                {
                    strNotes = "";
                }

                estSubModel.Notes = strNotes;
                estSubModel.Udate = DateTime.Now;

                _unitOfWork.EstimateSubs.Update(estSubModel);

                await _unitOfWork.CommitAsync();

                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpNotes");
                return ResponseHelper.Error<int>(HelperMessage.SICK010D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICK010D));
            }
        }

    }
}
