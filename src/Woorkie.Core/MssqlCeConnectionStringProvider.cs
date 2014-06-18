using System;
using System.Data.SqlServerCe;

namespace Woorkie.Core
{
    public class MssqlCeConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString { get; private set; }

        public MssqlCeConnectionStringProvider(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            ConnectionString = connectionString;
        }

        public static MssqlCeConnectionStringProvider ForFile(string filePath)
        {
            var connectionString = new SqlCeConnectionStringBuilder
            {
                DataSource = filePath
            }.ConnectionString;
            return new MssqlCeConnectionStringProvider(connectionString);
        }
    }
}
