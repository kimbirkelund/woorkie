using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Woorkie.Core.Nhibernate
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly ISessionFactory _sessionFactory;

        public DbContextFactory(IConnectionStringProvider connectionStringProvider)
        {
            if (connectionStringProvider == null)
                throw new ArgumentNullException("connectionStringProvider");

            _sessionFactory = Fluently.Configure()
                                      .Database(MsSqlCeConfiguration.MsSqlCe40
                                                                    .ConnectionString(connectionStringProvider.ConnectionString)
                                                                    .ShowSql())
                                      .Mappings(m => m.FluentMappings
                                                      .AddFromAssemblyOf<DbContextFactory>())
                                      .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                                      .BuildSessionFactory();
        }

        public NhibernateDbContext Create()
        {
            return new NhibernateDbContext(_sessionFactory.OpenSession());
        }

        IDbContext IDbContextFactory.Create()
        {
            return Create();
        }
    }
}
