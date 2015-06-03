namespace SkillExchange.Web.Areas.Admin.Models
{
    using System;
    using System.Linq.Expressions;
    using SkillExchange.Models;

    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<SkillCategory, CategoryModel>> ViewModel
        {
            get
            {
                return e => new CategoryModel
                {
                    Id = e.Id,
                    Name = e.Name
                };
            }
        }
    }
}