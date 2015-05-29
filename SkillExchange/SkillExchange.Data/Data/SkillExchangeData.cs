namespace SkillExchange.Data.Data
{
    using System;
    using System.Collections.Generic;
    using Context;
    using Models;
    using Repositories;

    public class SkillExchangeData : ISkillExchangeData
    {
        private readonly ISkillExchangeDbContext dbContext;

        private readonly IDictionary<Type, object> repositories;

        public SkillExchangeData(ISkillExchangeDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public IRepository<SkillCategory> SkillCategories
        {
            get { return this.GetRepository<SkillCategory>(); }
        }

        public IRepository<Town> Towns
        {
            get { return this.GetRepository<Town>(); }
        }

        public IRepository<UserSkill> UserSkills
        {
            get { return this.GetRepository<UserSkill>(); }
        }

        public IRepository<Approver> Approvers
        {
            get { return this.GetRepository<Approver>(); }
        }

        public IRepository<ExchangeType> ExchangeTypes
        {
            get { return this.GetRepository<ExchangeType>(); }
        }

        public IRepository<Notification> Notifications
        {
            get { return this.GetRepository<Notification>(); }
        }

        public IRepository<Skill> Skills
        {
            get { return this.GetRepository<Skill>(); }
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(Repository<T>);
                this.repositories.Add(typeof(T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
