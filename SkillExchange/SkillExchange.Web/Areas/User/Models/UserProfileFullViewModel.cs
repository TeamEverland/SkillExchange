namespace SkillExchange.Web.Areas.User.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using SkillExchange.Models;

    public class UserProfileFullViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Town { get; set; }

        public IEnumerable<OfferingSkillViewModel> Offering { get; set; }

        public IEnumerable<UserSkillViewModel> Seeking { get; set; }

        public string Description { get; set; }

        public static Expression<Func<User, UserProfileFullViewModel>> ViewModel
        {
            get
            {
                return u => new UserProfileFullViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Town = u.Town.Name,
                    Email = u.Email,
                    Description = u.Description,
                    Offering = u.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => new OfferingSkillViewModel
                        {
                            Id = s.Id,
                            SkillId = s.SkillId,
                            Name = s.Skill.Name
                        }),
                    Seeking = u.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => new UserSkillViewModel
                        {
                            Id = s.Id,
                            Name = s.Skill.Name
                        })
                };
            }
        } 
    }
}