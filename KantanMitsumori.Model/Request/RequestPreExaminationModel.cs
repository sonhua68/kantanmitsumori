using KantanMitsumori.Model.Response;

namespace KantanMitsumori.Model.Request
{
    public class RequestPreExaminationModel
    {
        public EstModel EstModel { get; set; }
        public EstimateIdeModel EstIDEModel { get; set; }
        public MemberIDEModel MemberIDE { get; set; }
        public string CarTypeIDE { get; set; }
        public string CompanyName { get; set; }
        public string PlanName { get; set; }
        public int GuaranteeFee { get; set; }
        public int GuaranteeFeeEx { get; set; }
    }
}