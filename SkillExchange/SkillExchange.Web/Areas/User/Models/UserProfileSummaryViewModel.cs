namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using SkillExchange.Models;
    using System;
    using System.Linq;

    public class UserProfileSummaryViewModel
    {
        public string Username { get; set; }

        public string Town { get; set; }

        public IEnumerable<UserSkillViewModel> Offering { get; set; }

        public IEnumerable<UserSkillViewModel> Seeking { get; set; }

        public string Description { get; set; }

        public static Expression<Func<User, UserProfileSummaryViewModel>> ViewModel
        {
            get
            {
                return e => new UserProfileSummaryViewModel()
                {
                    Username = e.UserName,
                    Offering = e.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => new UserSkillViewModel { Id = s.SkillId, Name = s.Skill.Name }),
                    Seeking = e.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => new UserSkillViewModel { Id = s.Id, Name = s.Skill.Name })
                };
            }
        }
    }
}