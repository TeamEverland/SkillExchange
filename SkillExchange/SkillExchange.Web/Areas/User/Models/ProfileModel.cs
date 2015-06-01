namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class ProfileModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public int TownId { get; set; }

        public string Town { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public ICollection<UserOfferingSkillModel> OfferingSkills { get; set; }

        public ICollection<UserSeekingSkillModel> SeekingSkills { get; set; } 
    }
}