using System.Data.SqlServerCe;
using System.IO;
using System.Runtime.CompilerServices;
using Woorkie.Core.Nhibernate;

namespace Woorkie.Core.Test.Nhibernate
{
    public class NhibernateDbContextTestBase
    {
        protected NhibernateDbContext CreateContext(bool recreateDb = true, [CallerMemberName] string callerMemberName = null)
        {
            var dbFilePath = GetDbFilePath(callerMemberName);
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);

            var connectionStringProvider = MssqlCeConnectionStringProvider.ForFile(dbFilePath);

            new SqlCeEngine(connectionStringProvider.ConnectionString).CreateDatabase();

            return new DbContextFactory(connectionStringProvider).Create();
        }

        protected string GetDbFilePath(string testMethod)
        {
            return GetType().FullName + (string.IsNullOrWhiteSpace(testMethod) ? "" : "." + testMethod) + ".sdf";
        }
    }
}
