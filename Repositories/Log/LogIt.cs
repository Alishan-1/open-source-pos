using System;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Dapper;
using Repositories.SqlServer;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Repositories.Log
{

    public class LogIt : ILogIt
    {
        public string Appname = "FoodAppLogIt";
        public string ErrorLogPath = "ErrorLog";
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogRepository _repo;
        
        public LogIt(ILogRepository repo, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _repo = repo;
            
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            ErrorLogPath = Path.Combine(hostingEnvironment.ContentRootPath, ErrorLogPath);
            
        }

        public async Task<long> AppReqLogFunc(int UserID, string Title, string DecryptedData, string LogData)
        {
            long result = 0L;

            try
            {
                // execute the stored procedure
                result = await _repo.WithConnectionLog(async c =>
               {

                   var p = new DynamicParameters();
                   
                   p.Add("@UserID", UserID, dbType: DbType.UInt32, direction: ParameterDirection.Input);
                   p.Add("@Title", Title, dbType: DbType.String, direction: ParameterDirection.Input);
                   p.Add("@DecryptedData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                   p.Add("@LogData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                   p.Add("@RespAgainstID", dbType: DbType.Int64, direction: ParameterDirection.Output);
                   //p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                   // map the result from stored procedure to  data model
                   var dbResults = await c.ExecuteScalarAsync("Proc_AppRequestLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                   return p.Get<long>("@RespAgainstID"); ;
               });

                return result;

            }
            catch (Exception ex)
            {
                APPExceptionLogClass objAPPExceptionLogClass = new APPExceptionLogClass();
                objAPPExceptionLogClass.APPRequestLogID = result;
                objAPPExceptionLogClass.APPResponseLogID = 0;
                objAPPExceptionLogClass.ExceptionTitle = "AppReqLogFunc: " + ex.Message;
                objAPPExceptionLogClass.ExceptionDetail = ex.StackTrace;
                await APPExceptionLogFunc(objAPPExceptionLogClass);
                return 0;
            }



        }

        public async Task<long> AppRespLogFunc(long RespAgainstID, int UserID, string Title, string DecryptedData, string LogData)
        {
            long result = 0;

            try
            {
                // execute the stored procedure
                result = await _repo.WithConnectionLog(async c =>
                {

                    var p = new DynamicParameters();
                    
                    p.Add("@RespAgainstID", UserID, dbType: DbType.UInt64, direction: ParameterDirection.Input);
                    p.Add("@UserID", UserID, dbType: DbType.UInt32, direction: ParameterDirection.Input);
                    p.Add("@Title", Title, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@DecryptedData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@LogData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@APPResponseLogID", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    //p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteScalarAsync("Proc_AppResponseLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return p.Get<long>("@RespAgainstID"); ;
                });

                return result;

            }
            catch (Exception ex)
            {
                APPExceptionLogClass objAPPExceptionLogClass = new APPExceptionLogClass();
                objAPPExceptionLogClass.APPRequestLogID = result;
                objAPPExceptionLogClass.APPResponseLogID = 0;
                objAPPExceptionLogClass.ExceptionTitle = "AppRespLogFunc: " + ex.Message;
                objAPPExceptionLogClass.ExceptionDetail = ex.StackTrace;
                await APPExceptionLogFunc(objAPPExceptionLogClass);
                return 0;
            }
        }

        public async Task<int> APPExceptionLogFunc(APPExceptionLogClass objAPPExceptionLogClass)
        {
            try
            {
                // execute the stored procedure called
                var result = await _repo.WithConnectionLog(async c =>
                {

                    var p = new DynamicParameters();

                    p.Add("@ExceptionTitle", objAPPExceptionLogClass.ExceptionTitle, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@ExceptionDetail", objAPPExceptionLogClass.ExceptionDetail, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@APPRequestLogID", objAPPExceptionLogClass.APPRequestLogID, dbType: DbType.Int64, direction: ParameterDirection.Input);
                    p.Add("@APPResponseLogID", objAPPExceptionLogClass.APPResponseLogID, dbType: DbType.Int64, direction: ParameterDirection.Input);

                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteAsync("Proc_APPExceptionLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return dbResults;
                });

                return result;
            }

            catch (Exception ex)
            {
                LogErrorToFile("APPExceptionLogFunc", ex.Message, true);
                return 0;
            }
        }

        public void LogErrorToFile(string methodName, string sMessage, bool IsMobile)
        {

            string logFile = "";
            string errorFile = "";

            if (IsMobile == true)
            {
                logFile = Path.Combine( ErrorLogPath +  $"App{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.log");
                errorFile = Path.Combine(ErrorLogPath + $"App{Path.DirectorySeparatorChar}err{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.err");

            }
            else
            {
                logFile = Path.Combine(ErrorLogPath + $"{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.log");
                errorFile = Path.Combine(ErrorLogPath + $"err{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.err");
            }

            try
            {

                if (!Directory.Exists(ErrorLogPath))
                {
                    Directory.CreateDirectory(ErrorLogPath);
                }

                if (!Directory.Exists(Path.Combine(ErrorLogPath + "err")))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + "err"));
                }

                if (!Directory.Exists(Path.Combine(ErrorLogPath + "App")))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + "App"));
                }


                if (!Directory.Exists(Path.Combine(ErrorLogPath + string.Format("App{0}err", Path.DirectorySeparatorChar))))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + string.Format("App{0}err", Path.DirectorySeparatorChar)));
                }

                File.AppendAllText(logFile, "" + DateTime.Now.ToString("yyyyMMdd:HH:mm:ss") + "\r\n");
                File.AppendAllText(logFile, ":" + methodName + "- " + sMessage + "\r\n");
                File.AppendAllText(logFile, "" + "\r\n");
            }
            catch (Exception e)
            {
                File.AppendAllText(errorFile, "" + DateTime.Now.ToString("yyyyMMdd:HH:mm:ss") + "\r\n");
                File.AppendAllText(errorFile, "FMS:LogErrorToFile - An error occured: " + e.Message + "\r\n");
                File.AppendAllText(errorFile, "" + "\r\n");
            }

        }

        public async Task<long> ReqLogFunc(int UserID, string Title, string DecryptedData, string LogData)
        {
            long result = 0;

            try
            {

                result = await _repo.WithConnectionLog(async c =>
                {

                    var p = new DynamicParameters();

                    p.Add("@UserID", UserID, dbType: DbType.UInt32, direction: ParameterDirection.Input);
                    p.Add("@Title", Title, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@DecryptedData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@LogData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@RespAgainstID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteAsync("Proc_RequestLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return dbResults;
                });

                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogFunc(ex);
                return 0;
            }
        }
        public async Task<long> RespLogFunc(long RespAgainstID, int UserID, string Title, string DecryptedData, string LogData)
        {
            long result = 0;

            try
            {
                result = await _repo.WithConnectionLog(async c =>
                {

                    var p = new DynamicParameters();
                    p.Add("@RespAgainstID", dbType: DbType.Int64, direction: ParameterDirection.Input);
                    p.Add("@UserID", UserID, dbType: DbType.UInt32, direction: ParameterDirection.Input);
                    p.Add("@Title", Title, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@DecryptedData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@LogData", UserID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@ResponseLogID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteAsync("Proc_ResponseLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return dbResults;
                });

                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogFunc(ex);
                return 0;
            }
        }
        public void ExceptionLogFunc(ExceptionLogClass objExceptionLogClass)
        {
            try
            {
               var result = _repo.WithConnectionLog(async c =>
                {

                    var p = new DynamicParameters();
                    p.Add("@ExceptionTitle", objExceptionLogClass.ExceptionTitle, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@ExceptionDetail", objExceptionLogClass.ExceptionDetail, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@RequestLogID", objExceptionLogClass.RequestLogID, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("@ResponseLogID", objExceptionLogClass.ResponseLogID, dbType: DbType.String, direction: ParameterDirection.Input);
                    

                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteAsync("Proc_ExceptionLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return dbResults;
                });
            }
            catch (Exception ex)
            {
                LogErrorToFile("ExceptionLogFunc", ex.Message, false);
            }
        }

        public void ExceptionLogFunc(Exception objException)
        {
            try
            {
                var result = _repo.WithConnectionLog(async c =>
                {
                    var st = new StackTrace(objException, true); // create the stack trace
                    var query = st.GetFrames()         // get the frames
                                  .Select(frame => new
                                  {                   // get the info
                                     FileName = frame.GetFileName(),
                                      LineNumber = frame.GetFileLineNumber(),
                                      ColumnNumber = frame.GetFileColumnNumber(),
                                      Method = frame.GetMethod(),
                                      Class = frame.GetMethod().DeclaringType,
                                  }).FirstOrDefault();
                    
                    var p = new DynamicParameters();

                    //old code commented
                    //p.Add("@ExceptionTitle", $"{query.Class.DeclaringType.Name}|{query.Method.Name}: " + objException.Message, dbType: DbType.String, direction: ParameterDirection.Input);
                    //p.Add("@ExceptionDetail", objException.StackTrace, dbType: DbType.String, direction: ParameterDirection.Input);
                    //p.Add("@RequestLogID", query.LineNumber, dbType: DbType.String, direction: ParameterDirection.Input);
                    //p.Add("@ResponseLogID", query.ColumnNumber, dbType: DbType.String, direction: ParameterDirection.Input);

                    //new code added
                    p.Add("AppID", 1, dbType: DbType.Int64, direction: ParameterDirection.Input);
                    p.Add("ExceptionType", objException.GetType().FullName, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("Detail", objException.Message, dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("Number", "Number", dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("SourceScreen", "SourceScreen", dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("SourceMethod", "SourceMethod", dbType: DbType.String, direction: ParameterDirection.Input);
                    p.Add("StackTrace", objException.StackTrace, dbType: DbType.String, direction: ParameterDirection.Input);
                    //new code end
                    // map the result from stored procedure to  data model
                    var dbResults = await c.ExecuteAsync("Proc_ExceptionLog_Insert", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 999);
                    return dbResults;
                });
            }
            catch (Exception ex)
            {
                LogErrorToFile("ExceptionLogFunc", ex.Message, false);
            }
        }

    }

    public class FLogIt : IFLogIt
    {
        public string Appname = "FoodAppLogIt";
        public string ErrorLogPath = "ErrorLog";
        private readonly IHostingEnvironment _hostingEnvironment;

        public FLogIt(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
           // _repo = repo;

            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            ErrorLogPath = Path.Combine(hostingEnvironment.ContentRootPath, ErrorLogPath);

        }

        public void LogErrorToFile(string methodName, string sMessage, bool IsMobile)
        {

            string logFile = "";
            string errorFile = "";

            if (IsMobile == true)
            {
                logFile = Path.Combine(ErrorLogPath + $"App{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.log");
                errorFile = Path.Combine(ErrorLogPath + $"App{Path.DirectorySeparatorChar}err{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.err");

            }
            else
            {
                logFile = Path.Combine(ErrorLogPath + $"{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.log");
                errorFile = Path.Combine(ErrorLogPath + $"err{Path.DirectorySeparatorChar}{Appname}_{methodName}_{DateTime.Now.ToString("yyyyMMdd")}.err");
            }

            try
            {

                if (!Directory.Exists(ErrorLogPath))
                {
                    Directory.CreateDirectory(ErrorLogPath);
                }

                if (!Directory.Exists(Path.Combine(ErrorLogPath + "err")))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + "err"));
                }

                if (!Directory.Exists(Path.Combine(ErrorLogPath + "App")))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + "App"));
                }


                if (!Directory.Exists(Path.Combine(ErrorLogPath + string.Format("App{0}err", Path.DirectorySeparatorChar))))
                {
                    Directory.CreateDirectory(Path.Combine(ErrorLogPath + string.Format("App{0}err", Path.DirectorySeparatorChar)));
                }

                File.AppendAllText(logFile, "" + DateTime.Now.ToString("yyyyMMdd:HH:mm:ss") + "\r\n");
                File.AppendAllText(logFile, ":" + methodName + "- " + sMessage + "\r\n");
                File.AppendAllText(logFile, "" + "\r\n");
            }
            catch (Exception e)
            {
                File.AppendAllText(errorFile, "" + DateTime.Now.ToString("yyyyMMdd:HH:mm:ss") + "\r\n");
                File.AppendAllText(errorFile, "FMS:LogErrorToFile - An error occured: " + e.Message + "\r\n");
                File.AppendAllText(errorFile, "" + "\r\n");
            }

        }

        

    }

   
}
