using System;
using System.Threading.Tasks;

namespace Repositories.Log
{
    public interface ILogIt
    {
        Task<int> APPExceptionLogFunc(APPExceptionLogClass objAPPExceptionLogClass);
        Task<long> AppReqLogFunc(int UserID, string Title, string DecryptedData, string LogData);
        Task<long> AppRespLogFunc(long RespAgainstID, int UserID, string Title, string DecryptedData, string LogData);
        void ExceptionLogFunc(ExceptionLogClass objExceptionLogClass);
        void ExceptionLogFunc(Exception objException);
        void LogErrorToFile(string methodName, string sMessage, bool IsMobile);
        Task<long> ReqLogFunc(int UserID, string Title, string DecryptedData, string LogData);
        Task<long> RespLogFunc(long RespAgainstID, int UserID, string Title, string DecryptedData, string LogData);
    }

    public interface IFLogIt
    {
        void LogErrorToFile(string methodName, string sMessage, bool IsMobile);
    }
    

    public class AppRequestLogClass
    {
        public int UserID { get; set; }
        public string Title { get; set; }
        public string DecryptedData { get; set; }
        public string LogData { get; set; }
        public long RespAgainstID { get; set; }

    }

    public class AppResponseLogClass
    {
        public long RespAgainstID { get; set; }
        public long APPResponseLogID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string DecryptedData { get; set; }
        public string LogData { get; set; }
    }

    public class APPExceptionLogClass
    {
        public string ExceptionTitle { get; set; }
        public string ExceptionDetail { get; set; }
        public long APPRequestLogID { get; set; }
        public long APPResponseLogID { get; set; }
    }

    public class ExceptionLogClass
    {
        public string ExceptionTitle { get; set; }
        public string ExceptionDetail { get; set; }
        public long RequestLogID { get; set; }
        public long ResponseLogID { get; set; }
    }
}