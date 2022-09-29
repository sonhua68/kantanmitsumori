using AutoMapper;
using KantanMitsumori.Infrastructure.Base;
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

        //public I getEstSubData(string inEstNo, string inEstSubNo)
        //{
        //    try
        //    {
        //        var estSubModel = _unitOfWorkIDE.Me.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
        //        return estSubModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "getEstData - CEST-040D");
        //        return null;
        //    }
        //}
    }
}
