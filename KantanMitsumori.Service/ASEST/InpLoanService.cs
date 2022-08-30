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
    public class InpLoanService : IInpLoanService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;



        public InpLoanService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public  ResponseBase<ResponseInpLoan> CalInpLoan(RequestCalInpLoan model)
        {
            try
            {
                ResponseInpLoan response = new ResponseInpLoan();
                CommonSimLon simLon = new CommonSimLon(_logger);
                simLon.SaleSumPrice = model.SaleSumPrice;
                simLon.Deposit = model.Deposit;
                simLon.MoneyRate = model.MoneyRate;
                simLon.PayTimes = model.PayTimes;
                simLon.Bonus = model.Bonus;
                simLon.FirstMonth = model.FirstMonth;
                simLon.BonusFirst = model.BonusFirst;
                simLon.BonusSecond = model.BonusSecond;
                simLon.ConTax = model.ConTax;

                if (simLon.calcRegLoan() == true)
                {
                    response.MoneyRate = simLon.MoneyRate;
                    response.Deposit = simLon.Deposit;
                    response.Principal = simLon.Principal;
                    response.Fee = simLon.Fee;
                    response.PayTotal = simLon.PayTotal;
                    response.FirstPayMonth = simLon.FirstPayMonth;
                    response.LastPayMonth = simLon.LastPayMonth;
                    response.PayMonth = simLon.PayMonth;
                    response.Bonus = simLon.Bonus;
                    response.BonusFirst = simLon.BonusFirst;
                    response.BonusSecond = simLon.BonusSecond;
                    response.BonusTimes = simLon.BonusTimes;
                    response.PayTimes = simLon.PayTimes;
                }
                return ResponseHelper.Ok<ResponseInpLoan>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CalInpLoan");
                return ResponseHelper.Error<ResponseInpLoan>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
    }
}