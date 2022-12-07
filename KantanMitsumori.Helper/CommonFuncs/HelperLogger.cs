//using AutoMapper;
//using KantanMitsumori.Model;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json.Linq;
//using System;
//using System.ComponentModel;
//using System.Data;
//using System.Reflection;

//namespace KantanMitsumori.Helper.CommonFuncs
//{
//    public class HelperLogger
//    {
//        public static string UserNo { get; set; } = "";
//        public static LogSession logSession { get; set; }
//        public static void LogInformation(ILogger logger, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Information, message, args);
//            }

//        }
//        public static void LogInformation(ILogger logger, EventId eventId, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Information, eventId, message, args);
//            }
//        }

//        public static void LogInformation(ILogger logger, Exception? exception, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Information, exception, message, args);
//            }
//        }

//        public static void LogError(ILogger logger, Exception? exception, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Error, exception, message, args);
//            }

//        }
//        public static void LogError(ILogger logger, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Error, message, args);
//            }

//        }
//        public static void LogDebug(ILogger logger, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Debug, message, args);
//            }
          
//        }
//        public static void LogTrace(ILogger logger, string? message, params object?[] args)
//        {
//            var state = SetState();
//            using (logger.BeginScope(state))
//            {
//                logger.Log(LogLevel.Trace, message, args);
//            }
//        }
//        private static Dictionary<string, object> SetState()
//        {            
//            if (logSession == null)
//            {
//                logSession = new LogSession();
//                logSession.UserNo = UserNo;
//            }
//            var state = new Dictionary<string, object>
//            {
//                ["UserNo"] = logSession.UserNo!,
//                ["UserNm"] = logSession.UserNm!,
//            };
//            return state;
//        }
//    }
//}
