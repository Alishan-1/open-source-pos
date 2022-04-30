namespace Repositories.Common
{
    public interface IConnection
    {
        string ConnectionString { get; }
        string ConnectionStringLog { get; }
        string FNNConnectionString { get; }
    }
}