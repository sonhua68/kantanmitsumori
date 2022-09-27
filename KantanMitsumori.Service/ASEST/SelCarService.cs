using AutoMapper;
using GrapeCity.ActiveReports.Core.DataProviders;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.ASESTSQL;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data.SqlClient;

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
                if (carNamesList.Count == 0)
                {
                    return ResponseHelper.Ok<List<ResponseAsopCarname>>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003));
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
                if (makerList.Count == 0)
                {
                    return ResponseHelper.Ok<List<ResponseAsopMaker>>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003));
                }
                return ResponseHelper.Ok<List<ResponseAsopMaker>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), makerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListASOPMakers");
                return ResponseHelper.Error<List<ResponseAsopMaker>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }

        }
        public ResponseBase<List<ResponseTbRuibetsuNew>> GetListRuiBetSu(RequestSelGrd requestSel)
        {
            try
            {
                var sql = SQLHelper.GetListTB_RUIBETSU_NEW(requestSel);
                var tbRuibetsuList = new List<ResponseTbRuibetsuNew>();
                switch (requestSel.colSort)
                {

                    case (int)enSortCar.isDefault:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                  .OrderByDescending(n => n.GradeNameOrd).ThenBy(n => n.GradeName)
                                                                  .ThenByDescending(n => n.RegularCaseOrd).ThenBy(n => n.RegularCase)
                                                                  .ThenByDescending(n => n.DispVolOrd).ThenBy(n => n.DispVol)
                                                                  .ThenByDescending(n => n.DriveTypeCodeOrd).ThenBy(n => n.DriveTypeCode)
                                                                  .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                  .ToList();
                        break;
                    case (int)enSortCar.isGradeName:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                 .OrderByDescending(n => n.GradeName).ThenBy(n => n.GradeNameOrd)
                                                                 .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                 .ToList();
                        break;
                    case (int)enSortCar.isRegularCase:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                  .OrderBy(n => n.RegularCase).ThenBy(n => n.RegularCaseOrd)
                                                                  .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                  .ToList();
                        break;
                    case (int)enSortCar.isRegularCaseDesc:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                  .OrderByDescending(n => n.RegularCase).ThenBy(n => n.RegularCaseOrd)
                                                                  .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                  .ToList();

                        break;
                    case (int)enSortCar.isDispVol:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                 .OrderBy(n => n.DispVolOrd).ThenBy(n => n.DispVol)                                                              
                                                              .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                              .ToList();


                        break;
                    case (int)enSortCar.isDispVolDesc:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                 .OrderByDescending(n => n.DispVolOrd).ThenBy(n => n.DispVol)                                                                  
                                                              .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                              .ToList();
                        break;
                    case (int)enSortCar.isDriveTypeCode:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                   .OrderBy(n => n.DriveTypeCodeOrd ).ThenBy(n => n.DriveTypeCode)
                                                                .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                .ToList();

                        break;

                    case (int)enSortCar.isDriveTypeCodeDesc:
                        tbRuibetsuList = _unitOfWork.DbContext.Set<TbRuibetsuEntity>().FromSqlRaw(sql)
                                                                            .OrderByDescending(n => n.DriveTypeCodeOrd).ThenBy(n => n.DriveTypeCode)
                                                                         .Select(i => _mapper.Map<ResponseTbRuibetsuNew>(i))
                                                                         .ToList();
                        break;

                }



                if (tbRuibetsuList.Count == 0)
                {

                    return ResponseHelper.Ok<List<ResponseTbRuibetsuNew>>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003));
                }
                return ResponseHelper.Ok<List<ResponseTbRuibetsuNew>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), tbRuibetsuList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListRuiBetSu");
                return ResponseHelper.Error<List<ResponseTbRuibetsuNew>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }

        }
        public ResponseBase<List<ResponseSerEst>> GetListSerEst(RequestSerEst requestSerEst)
        {
            try
            {
                var data = new List<ResponseSerEst>();
                var sql = SQLHelper.GetListSerEst(requestSerEst);
                switch (requestSerEst.colSort)
                {

                    case (int)enSortCarEst.isDefault:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                    .OrderByDescending(n => n.EstNo)
                                                                    .Select(i =>
                                                                    _mapper.Map<ResponseSerEst>(i))
                                                                    .ToList();
                        break;
                    case (int)enSortCarEst.isEstNo:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                    .OrderBy(n => n.EstNo)
                                                                    .Select(i =>
                                                                    _mapper.Map<ResponseSerEst>(i))
                                                                    .ToList();
                        break;
                    case (int)enSortCarEst.isTradeDate:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                  .OrderBy(n => n.TradeDate).ThenByDescending(n => n.EstNo)
                                                                  .Select(i =>
                                                                  _mapper.Map<ResponseSerEst>(i))
                                                                  .ToList();

                        break;
                    case (int)enSortCarEst.isTradeDateDesc:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                  .OrderByDescending(n => n.TradeDate).ThenByDescending(n => n.EstNo)
                                                                  .Select(i =>
                                                                  _mapper.Map<ResponseSerEst>(i))
                                                                  .ToList();
                        break;

                    case (int)enSortCarEst.isCustKName:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                  .OrderBy(n => n.CustKName).ThenByDescending(n => n.EstNo)
                                                                  .Select(i =>
                                                                  _mapper.Map<ResponseSerEst>(i))
                                                                  .ToList();

                        break;

                    case (int)enSortCarEst.isCustKNameDesc:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                  .OrderByDescending(n => n.CustKName).ThenBy(n => n.EstNo)
                                                                  .Select(i =>
                                                                  _mapper.Map<ResponseSerEst>(i))
                                                                  .ToList();


                        break;

                    case (int)enSortCarEst.isCarName:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                                       .OrderBy(n => n.CarName).ThenByDescending(n => n.EstNo)
                                                                       .Select(i =>
                                                                       _mapper.Map<ResponseSerEst>(i))
                                                                       .ToList();

                        break;
                    case (int)enSortCarEst.isCarNameDesc:
                        data = _unitOfWork.DbContext.Set<SerEstEntity>().FromSqlRaw(sql)
                                                               .OrderByDescending(n => n.CarName).ThenByDescending(n => n.EstNo)
                                                               .Select(i =>
                                                               _mapper.Map<ResponseSerEst>(i))
                                                               .ToList();
                        break;
                }
                if (data.Count == 0)
                {
                    return ResponseHelper.Ok<List<ResponseSerEst>>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), data);
                }
                return ResponseHelper.Ok<List<ResponseSerEst>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListSerEst");
                return ResponseHelper.Error<List<ResponseSerEst>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
        }


    }
}
