

namespace KantanMitsumori.Model
{
    public class ResponseBase<T>
    {
        public T? Data { get; set; }
        public int ResultStatus { get; set; }
        public string MessageCode { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
    }
}