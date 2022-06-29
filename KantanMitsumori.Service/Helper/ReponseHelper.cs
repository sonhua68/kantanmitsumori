using KantanMitsumori.Model;

namespace KantanMitsumori.Service.Helper
{
    public class ResponseHelper
    {
        public static ResponseBase<T> Ok<T>(string messageCode, string messageContent, T data) => new ResponseBase<T> { ResultStatus = 0, MessageCode = messageCode, MessageContent = messageContent, Data = data };
        public static ResponseBase<T> Ok<T>(string messageCode, string messageContent) => new ResponseBase<T> { ResultStatus = 0, MessageCode = messageCode, MessageContent = messageContent };
        public static ResponseBase<T> Error<T>(string messageCode, string messageContent, T data) => new ResponseBase<T> { ResultStatus = -1, MessageCode = messageCode, MessageContent = messageContent, Data = data };
        public static ResponseBase<T> Error<T>(string messageCode, string messageContent) => new ResponseBase<T> { ResultStatus = -1, MessageCode = messageCode, MessageContent = messageContent };

    }
}
