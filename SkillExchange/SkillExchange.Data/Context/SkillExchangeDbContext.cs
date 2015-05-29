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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                    .HasRequired(m => m.Sender)
                    .WithMany(t => t.MessagesSent)
                    .HasForeignKey(m => m.SenderId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                        .HasRequired(m => m.Reciever)
                        .WithMany(t => t.MessagesRecieved)
                        .HasForeignKey(m => m.RecieverId)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


        public IDbSet<Message> Messages { get; set; }

        public IDbSet<SkillCategory> SkillCategories { get; set; }

        public IDbSet<Town> Towns { get; set; }

        public IDbSet<UserSkill> UserSkills { get; set; }

        public IDbSet<Approver> Approvers { get; set; }

        public IDbSet<ExchangeType> ExchangeTypes { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<Skill> Skills { get; set; }
    }
}
