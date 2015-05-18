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
