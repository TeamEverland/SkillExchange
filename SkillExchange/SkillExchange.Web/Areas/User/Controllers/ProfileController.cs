namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Common;
    using Data.Data;
    using Models;
    using SkillExchange.Models;
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
            var viewModel = new UserProfileViewModel();

            if (TempData["message"] != null)
            {
                viewModel.Message = (NotitficationMessage)TempData["message"];
            }

            var approvedSkillsByCurrentUserLogged = this.Data.Approvers
                .All()
                .Where(a => a.ApproverUserId == this.UserProfile.Id)
                .Select(a => a.UserSkillId)
                .ToList();

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
                            Id = s.Id,
                            SkillId = s.SkillId,
                            Name = s.Skill.Name,
                            ApproversCount = s.Approvers.Count,
                            CanBeApprovedByCurrentUserLogged = !approvedSkillsByCurrentUserLogged.Contains(s.Id)
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

            viewModel.UserProfile = userProfile;

            return this.View(viewModel);
        }

        public ActionResult Approve(int skillId)
        {
            var userOwner = this.Data.UserSkills
                .All()
                .Where(s => s.Id == skillId)
                .Select(s => s.User.UserName)
                .FirstOrDefault();

            if (userOwner != this.UserProfile.UserName)
            {
                if (!this.Data.Approvers
                    .All()
                    .Any(a => a.UserSkillId == skillId && a.ApproverUserId == this.UserProfile.Id))
                {
                    var approver = new Approver
                    {
                        ApproverUserId = this.UserProfile.Id,
                        UserSkillId = skillId
                    };

                    this.Data.Approvers.Add(approver);
                    var addResult = this.Data.SaveChanges();
                    if (addResult > 0)
                    {
                        TempData["message"] = new NotitficationMessage
                        {
                            Content = SuccessMessages.SkillSuccessfullyApprovedMessage,
                            Type = NotificationMessageType.Success
                        }; 
                    }
                    else
                    {
                        TempData["message"] = new NotitficationMessage
                        {
                            Content = "You have already approved this skill",
                            Type = NotificationMessageType.Error
                        };
                    }
                }
                else
                {
                    TempData["message"] = new NotitficationMessage
                    {
                        Content = "You have already approved this skill",
                        Type = NotificationMessageType.Error
                    };
                }
            }
            else
            {
                TempData["message"] = new NotitficationMessage
                {
                    Content = "You cannot approve your own skills",
                    Type = NotificationMessageType.Error
                };
            }

            return this.RedirectToAction("Show", "Profile", new { username = userOwner });
        }
    }
}