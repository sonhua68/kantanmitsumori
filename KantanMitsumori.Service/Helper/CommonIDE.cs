using AutoMapper;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model.Response;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service.Helper
{
    public class CommonIDE
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly IMapper _mapper;

        private CommonFuncHelper _commonFuncHelper;

        public CommonIDE(ILogger<CommonIDE> logger, IUnitOfWorkIDE unitOfWorkIDE, IMapper mapper, CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _unitOfWorkIDE = unitOfWorkIDE;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;
        }

        /// <summary>
        /// Get MT_IDE_MEMBER
        /// </summary>
        /// <param name="asMemberNum"></param>
        /// <returns></returns>
        public MemberIDEModel getMember(string asMemberNum)
        {
            var memberIDE = new MemberIDEModel();
            try
            {
                var data = _unitOfWorkIDE.Members.GetSingle(x => x.AsmemberNum == asMemberNum);
                if (data == null)
                {
                    data = new MtIdeMember();
                }

                memberIDE = _mapper.Map<MemberIDEModel>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getMember - CEST-040D");
            }

            return memberIDE;
        }

        /// <summary>
        /// Get MT_IDE_CARTYPE
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        public MtIdeCartype getCarType(int carType)
        {
            try
            {
                var data = _unitOfWorkIDE.CarTypes.GetSingle(x => x.CarType == carType);
                if (data == null)
                {
                    data = new MtIdeCartype();
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getCarType - CEST-040D");
                return null;
            }
        }

        /// <summary>
        ///  Get MT_IDE_VOLUNTARY_INSURANCE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MtIdeVoluntaryInsurance getVoluntaryInsurance(int id)
        {
            try
            {
                var data = _unitOfWorkIDE.VoluntaryInsurances.GetSingle(x => x.Id == id);
                if (data == null)
                {
                    data = new MtIdeVoluntaryInsurance();
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getVoluntaryInsurance - CEST-040D");
                return null;
            }
        }

        /// <summary>
        /// Get [MT_IDE_CONTRACT_PLAN]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MtIdeContractPlan getContractPlan(int id)
        {
            try
            {
                var data = _unitOfWorkIDE.ContractPlans.GetSingle(x => x.Id == id);
                if (data == null)
                {
                    data = new MtIdeContractPlan();
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getContractPlan - CEST-040D");
                return null;
            }
        }

        /// <summary>
        /// Get MT_IDE_GUARANTEE
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public MtIdeGuarantee getGuarantee(int year)
        {
            try
            {
                var data = _unitOfWorkIDE.Guarantees.GetSingle(x => x.Years == year);
                if (data == null)
                {
                    data = new MtIdeGuarantee();
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getGuarantee - CEST-040D");
                return null;
            }
        }
    }
}
