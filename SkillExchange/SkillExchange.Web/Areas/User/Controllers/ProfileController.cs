namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Data;
    using Models;
    using Web.Controllers;

    public class ProfileController : BaseController
    {
        public ProfileController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Profile/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Show(string username)
        {
            var userProfile = this.Data.Users
                .All()
                .Include(u => u.Town)
                .Where(u => u.UserName == username)
                .Select(u => new UserProfileFullViewModel
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
                            SkillId = s.SkillId,
                            Name = s.Skill.Name,
                            ApproversCount = s.Approvers.Count,
                            CanBeApprovedByCurrentUserLogged = !s.Approvers.Select(a => a.ApproverUserId).Contains(this.UserProfile.Id)
                        }),
                    Seeking = u.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => new UserSkillViewModel
                        {
                            Id = s.Id,
                            Name = s.Skill.Name
                        })
                })
                .FirstOrDefault();

            return this.View(userProfile);
        }
    }
}