using AutoMapper;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service
{
    public class AppService : IAppService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;



        public AppService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase<int>>  CreateMaker(MakerModel model)
        {
              ResponseBase<int> iResult = new ResponseBase<int>();
            try
            {

             var   data = _mapper.Map<MMaker>(model);
                _unitOfWork.Makers.Add(data);
                await _unitOfWork.CommitAsync();
                iResult.Data = 0;
                iResult.MessageCode = "E0001";
                iResult.MessageCode = "";
                return ResponseHelper.Ok<int>("", "", 0);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error" + ex.Message);
                return ResponseHelper.Ok<int>("", "", 0);
            }
        }

        public ResponseBase<List<MakerModel>> GetMaker()
        {
            try
            {
                ResponseBase<List<MakerModel>> iResult = new ResponseBase<List<MakerModel>>();

                var makerList = _unitOfWork.Makers.GetAll().Select(i => _mapper.Map<MakerModel>(i)).ToList();

                return ResponseHelper.Ok<List<MakerModel>>("", "", makerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EstNo={0} EstSubNo={1}", "00000", "000000");
                return ResponseHelper.Error<List<MakerModel>>("Error", "Error");
            }
        }

       
    }
}