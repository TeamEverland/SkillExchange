namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public string Town { get; set; }

        public ICollection<UserSkillViewModel> Offering { get; set; }

        public ICollection<UserSkillViewModel> Seeking { get; set; }

        public string Description { get; set; }
    }
}