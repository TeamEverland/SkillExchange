namespace SkillExchange.Data.Data
{
    using Models;
    using Repositories;

    public interface ISkillExchangeData
    {
        IRepository<User> Users { get; }

        int SaveChanges();
    }
}
