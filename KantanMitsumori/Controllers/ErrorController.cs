using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class ErrorController : BaseController
    {

        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger, IConfiguration config) : base(config)
        {
            _logger = logger;
        }
    }
}