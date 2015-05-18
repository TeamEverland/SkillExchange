namespace SkillExchange.Data.Context
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class SkillExchangeDbContext : IdentityDbContext<User>, ISkillExchangeDbContext
    {
        public SkillExchangeDbContext()
            : base("SkillExchangeDbConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<SkillExchangeDbContext, SkillExchangeDbMigrationConfiguration>());
        }

        public static SkillExchangeDbContext Create()
        {
            return new SkillExchangeDbContext();
        }
    }
}
