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
    public class EstimateIdeService : IEstimateIdeService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;



        public EstimateIdeService(IMapper mapper, ILogger<EstimateIdeService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseBase<int>> Create(TEstimateIde model)
        {
            throw new NotImplementedException();
        }

        public  ResponseBase<List<TEstimateIde>> GetList()
        {
            throw new NotImplementedException();
        }
    }
}