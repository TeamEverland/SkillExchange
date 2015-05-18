namespace SkillExchange.Data.Context
{
    using System.Data.Entity;
    using Models;

    public interface ISkillExchangeDbContext
    {
        IDbSet<User> Users { get; }

        int SaveChanges();
    }
}
