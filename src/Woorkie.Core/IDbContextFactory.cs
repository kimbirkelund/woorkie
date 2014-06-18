using Woorkie.Core.Nhibernate;

namespace Woorkie.Core
{
    public interface IDbContextFactory
    {
        IDbContext Create();
    }
}
