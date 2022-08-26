using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service
{
    public class EstimateSubService : IEstimateSubService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;



        public EstimateSubService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseBase<int>> Create(TEstimateSub model)
        {
            throw new NotImplementedException();
        }

        public ResponseBase<List<TEstimateSub>> GetList()
        {
            throw new NotImplementedException();
        }
    }
}