using System;
using System.Data.SqlServerCe;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Woorkie.Core.Nhibernate
{
    public class NhDbContextFactory : IDbContextFactory
    {
        private readonly ISessionFactory _sessionFactory;

        public NhDbContextFactory(SqlCeConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null)
                throw new ArgumentNullException("connectionStringBuilder");

            _sessionFactory = Fluently.Configure()
                                      .Database(MsSqlCeConfiguration.MsSqlCe40
                                                                    .ConnectionString(connectionStringBuilder.ConnectionString))
                                      .Mappings(m => m.FluentMappings
                                                      .AddFromAssemblyOf<NhDbContextFactory>())
                                      .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                      .BuildSessionFactory();
        }

        public NhDbContext Create()
        {
            return new NhDbContext(_sessionFactory.OpenSession());
        }

        IDbContext IDbContextFactory.Create()
        {
            return Create();
        }
    }
}
