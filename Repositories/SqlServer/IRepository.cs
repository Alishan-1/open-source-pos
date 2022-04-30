using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Repositories.SqlServer
{
    public interface IRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnection(bool multipleActiveResultSets = false);

        Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnectionLog(bool multipleActiveResultSets = false);
        Task<T> WithFNNConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetFNNConnection(bool multipleActiveResultSets = false);

    }

    public interface ILogRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnection(bool multipleActiveResultSets = false);

        Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnectionLog(bool multipleActiveResultSets = false);
    }

    public interface ILLogRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnection(bool multipleActiveResultSets = false);

        Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnectionLog(bool multipleActiveResultSets = false);
    }

    public interface ILLLogRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnection(bool multipleActiveResultSets = false);

        Task<T> WithConnectionLog<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnectionLog(bool multipleActiveResultSets = false);
    }
}
