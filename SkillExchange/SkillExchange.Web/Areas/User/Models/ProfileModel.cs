namespace SkillExchange.Web.Areas.User.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;

    using SkillExchange.Models;

    public class ProfileModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Username { get; set; }

        [Required]
        public int TownId { get; set; }

        public string Town { get; set; }

        [Required]
        public string Email { get; set; }

        public string Description { get; set; }

        public IList<UserSkillViewModel> OfferingSkills { get; set; }

        public IList<UserSkillViewModel> SeekingSkills { get; set; }

        public static Expression<Func<User, ProfileModel>> ViewModel
        {
            get
            {
                return e => new ProfileModel
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Username = e.LastName,
                    Email = e.Email,
                    Town = e.Town.Name,
                    Description = e.Description,
                    OfferingSkills = e.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => new UserSkillViewModel
                        {
                            Id = s.Id,
                            Name = s.Skill.Name,
                            CategoryId = s.Skill.CategoryId,
                            CategoryName = s.Skill.Category.Name
                        })
                        .ToList(),
                    SeekingSkills = e.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => new UserSkillViewModel
                        {
                            Id = s.Id,
                            Name = s.Skill.Name,
                            CategoryId = s.Skill.CategoryId,
                            CategoryName = s.Skill.Category.Name
                        })
                        .ToList()
                };
            }
        } 
    }
}