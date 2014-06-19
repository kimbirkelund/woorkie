using System.Data.SqlServerCe;
using System.IO;
using System.Runtime.CompilerServices;

namespace Woorkie.Core.Nhibernate.Test
{
    public class NhDbContextTestBase
    {
        protected NhDbContext CreateContext(bool recreateDb = true, [CallerMemberName] string callerMemberName = null)
        {
            var dbFilePath = GetDbFilePath(callerMemberName);
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);

            var connectionStringProvider = new SqlCeConnectionStringBuilder
            {
                DataSource = dbFilePath
            };

            new SqlCeEngine(connectionStringProvider.ConnectionString).CreateDatabase();

            return new NhDbContextFactory(connectionStringProvider).Create();
        }

        protected string GetDbFilePath(string testMethod)
        {
            return GetType().FullName + (string.IsNullOrWhiteSpace(testMethod) ? "" : "." + testMethod) + ".sdf";
        }
    }
}
