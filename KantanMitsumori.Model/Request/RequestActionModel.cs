using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Model.Request
{
    public class RequestActionModel
    {
        [FromQuery(Name = "IsInpBack")]
        public string IsInpBack { get; set; }

        [FromQuery(Name = "Sel")]
        public string Sel { get; set; }
    }
}
