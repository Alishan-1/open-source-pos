using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Repositories.Common;
using Repositories.Log;

namespace Repositories.SqlServer
{

    public class Repository : IRepository
    {
        private readonly IFLogIt _log;
        private readonly string _connectionString;
        private readonly string _connectionStringLog;
        private readonly string _fnnConnectionString;

        public Repository(IConnection db, IFLogIt log)
        {
            _connectionString = db.ConnectionString;
            _connectionStringLog = db.ConnectionStringLog;
            _fnnConnectionString = db.FNNConnectionString;
            _log = log;
        }

        // use for buffered queries that return a type
        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                _log.LogErrorToFile("WithConnection", "Timeout Exception - " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new TimeoutException("Timeout Exception", ex);

            }
            catch (SqlException ex)
            {
                _log.LogErrorToFile("WithConnection", "Sql Exception- " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new Exception("Sql Exception", ex);

            }
        }

        public SqlConnection GetConnection(bool multipleActiveResultSets = false)
        {
            try
            {
                var cs = _connectionString;
                if (multipleActiveResultSets)
                {
                    var scsb = new SqlConnectionStringBuilder(cs) { MultipleActiveResultSets = true };
                    if (scsb.ConnectionString != null) cs = scsb.ConnectionString;
                }
                var connection = new SqlConnection(cs);
                connection.Open();
                return connection;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("Timeout Exception", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception("Sql Exception", ex);
            }
        }

        public async Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connectionLog = new SqlConnection(_connectionStringLog))
                {
                    await connectionLog.OpenAsync();
                    return await getData(connectionLog);
                }
            }
            catch (TimeoutException ex)
            {
                _log.LogErrorToFile("WithConnectionLog", "Timeout Exception - " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new TimeoutException("Timeout Exception", ex);

            }
            catch (SqlException ex)
            {
                _log.LogErrorToFile("WithConnectionLog", "Sql Exception- " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new Exception("Sql Exception", ex);

            }
        }

        public SqlConnection GetConnectionLog(bool multipleActiveResultSets = false)
        {
            try
            {
                var cs = _connectionStringLog;
                if (multipleActiveResultSets)
                {
                    var scsb = new SqlConnectionStringBuilder(cs) { MultipleActiveResultSets = true };
                    if (scsb.ConnectionString != null) cs = scsb.ConnectionString;
                }
                var connection = new SqlConnection(cs);
                connection.Open();
                return connection;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("Timeout Exception Log", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception("Sql Exception Log", ex);
            }
        }

        public async Task<T> WithFNNConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_fnnConnectionString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                _log.LogErrorToFile("WithFNNConnection", "Timeout Exception - " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new TimeoutException("Timeout Exception", ex);

            }
            catch (SqlException ex)
            {
                _log.LogErrorToFile("WithFNNConnection", "Sql Exception- " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new Exception("Sql Exception", ex);

            }
        }

        public SqlConnection GetFNNConnection(bool multipleActiveResultSets = false)
        {
            try
            {
                var cs = _fnnConnectionString;
                if (multipleActiveResultSets)
                {
                    var scsb = new SqlConnectionStringBuilder(cs) { MultipleActiveResultSets = true };
                    if (scsb.ConnectionString != null) cs = scsb.ConnectionString;
                }
                var connection = new SqlConnection(cs);
                connection.Open();
                return connection;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("Timeout Exception", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception("Sql Exception", ex);
            }
        }

    }

    public class LogRepository : ILogRepository
    {
        private readonly IFLogIt _log;
        private readonly string _connectionString;
        private readonly string _connectionStringLog;

        public LogRepository(IConnection db, IFLogIt log)
        {
            _connectionString = db.ConnectionString;
            _connectionStringLog = db.ConnectionStringLog;
            _log = log;
        }

        // use for buffered queries that return a type
        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                _log.LogErrorToFile("WithConnection", "Timeout Exception - " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new TimeoutException("Timeout Exception", ex);

            }
            catch (SqlException ex)
            {
                _log.LogErrorToFile("WithConnection", "Sql Exception- " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new Exception("Sql Exception", ex);

            }
        }

        public SqlConnection GetConnection(bool multipleActiveResultSets = false)
        {
            try
            {
                var cs = _connectionString;
                if (multipleActiveResultSets)
                {
                    var scsb = new SqlConnectionStringBuilder(cs) { MultipleActiveResultSets = true };
                    if (scsb.ConnectionString != null) cs = scsb.ConnectionString;
                }
                var connection = new SqlConnection(cs);
                connection.Open();
                return connection;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("Timeout Exception", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception("Sql Exception", ex);
            }
        }

        public async Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connectionLog = new SqlConnection(_connectionStringLog))
                {
                    await connectionLog.OpenAsync();
                    return await getData(connectionLog);
                }
            }
            catch (TimeoutException ex)
            {
                _log.LogErrorToFile("WithConnectionLog", "Timeout Exception - " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new TimeoutException("Timeout Exception", ex);

            }
            catch (SqlException ex)
            {
                _log.LogErrorToFile("WithConnectionLog", "Sql Exception- " + ex.Message, false);
                return Task.FromException<T>(ex).Result;
                //throw new Exception("Sql Exception", ex);

            }
        }

        public SqlConnection GetConnectionLog(bool multipleActiveResultSets = false)
        {
            try
            {
                var cs = _connectionStringLog;
                if (multipleActiveResultSets)
                {
                    var scsb = new SqlConnectionStringBuilder(cs) { MultipleActiveResultSets = true };
                    if (scsb.ConnectionString != null) cs = scsb.ConnectionString;
                }
                var connection = new SqlConnection(cs);
                connection.Open();
                return connection;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("Timeout Exception Log", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception("Sql Exception Log", ex);
            }
        }
    }
}
