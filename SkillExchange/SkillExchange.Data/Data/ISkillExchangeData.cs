namespace SkillExchange.Data.Data
{
    using Models;
    using Repositories;

    public interface ISkillExchangeData
    {
        IRepository<User> Users { get; }

        IRepository<Message> Messages { get; }

        IRepository<SkillCategory> SkillCategories { get; }

        IRepository<Town> Towns { get; }

        IRepository<UserSkill> UserSkills { get; }

        IRepository<Approver> Approvers { get; }

        IRepository<ExchangeType> ExchangeTypes { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<Skill> Skills { get; } 

        int SaveChanges();
    }
}
