namespace SkillExchange.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        public User()
        {
            this.Approvers = new HashSet<Approver>();
            this.Skills = new HashSet<UserSkill>();
        }

        [Required]
        [MinLength(5)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(5)]
        public string LastName { get; set; }

        public string Description { get; set; }

        [Required]
        public int? TownId { get; set; }

        public virtual Town Town { get; set; }

        public virtual ICollection<UserSkill> Skills { get; set; }

        public virtual ICollection<Approver> Approvers { get; set; }

        public virtual ICollection<Message> MessagesSent { get; set; }

        public virtual ICollection<Message> MessagesRecieved { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}
