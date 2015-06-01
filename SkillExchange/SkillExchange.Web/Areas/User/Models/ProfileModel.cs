namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProfileModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string Email { get; set; }

        public string Description { get; set; }

        public IList<UserSkillViewModel> OfferingSkills { get; set; }

        public IList<UserSkillViewModel> SeekingSkills { get; set; } 
    }
}