using AutoMapper;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service.ASEST
{
    public class InpCustKanaService : IInpCustKanaService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public InpCustKanaService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

    }
}
