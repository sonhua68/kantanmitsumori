using System.Text.Json.Serialization;

namespace KantanMitsumori.Model.Response
{
    public class ResponseEstMainModel
    {
        public EstModel EstModel { get; set; }
        public EstModelView EstModelView { get; set; }
        public EstCustomerModel EstCustomerModel { get; set; }
        public EstimateIdeModel EstIDEModel { get; set; }

        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}