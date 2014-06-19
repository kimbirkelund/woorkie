namespace Woorkie.Core
{
    public interface IDbContextFactory
    {
        IDbContext Create();
    }
}
