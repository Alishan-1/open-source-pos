using Repositories.Log;
using System;
using System.Collections.Generic;

namespace Repositories.Common
{
    public class Connection : IConnection
    {
        public Connection(string connectionString)
        {
            // must use a guard clause to ensure something is injected
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString", "Connection expects constructor injection for connectionString param.");

            // we have a value by now so assign it
            ConnectionString = connectionString;
        }

        public Connection(string connectionString, string connectionStringLog) : this(connectionString)
        {
            // must use a guard clause to ensure something is injected
            if (string.IsNullOrEmpty(connectionStringLog))
                throw new ArgumentNullException("connectionStringLog", "Connection expects constructor injection for connectionStringLog param.");

            // we have a value by now so assign it
            ConnectionStringLog = connectionStringLog;
        }
        public Connection(string fnnConnectionString, string connectionString, string connectionStringLog) : this(connectionString, connectionStringLog)
        {
            // must use a guard clause to ensure something is injected
            if (string.IsNullOrEmpty(fnnConnectionString))
                throw new ArgumentNullException("FNNConnectionString", "Connection expects constructor injection for connectionStringLog param.");

            // we have a value by now so assign it
            FNNConnectionString = fnnConnectionString;
        }

        public string ConnectionString { get; set; }

        public string ConnectionStringLog { get; set; }

        public string FNNConnectionString { get; set; }
    }
}

public static class Extensions
{
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
    {
        return new HashSet<T>(source);
    }
}