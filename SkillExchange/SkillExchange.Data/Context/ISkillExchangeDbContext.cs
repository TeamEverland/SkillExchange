namespace SkillExchange.Data.Context
{
    using System.Data.Entity;
    using Models;

    public interface ISkillExchangeDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<Message> Messages { get; }

        IDbSet<SkillCategory> SkillCategories { get; }

        IDbSet<Town> Towns { get; }

        IDbSet<UserSkill> UserSkills { get; }

        IDbSet<Approver> Approvers { get; }

        IDbSet<ExchangeType> ExchangeTypes { get; }

        IDbSet<Notification> Notifications { get; }

        IDbSet<Skill> Skills { get; }

        int SaveChanges();
    }
}
