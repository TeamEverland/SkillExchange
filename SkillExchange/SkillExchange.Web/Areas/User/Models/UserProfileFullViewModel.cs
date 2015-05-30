namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

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
    }
}