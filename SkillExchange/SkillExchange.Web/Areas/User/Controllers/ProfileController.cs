namespace SkillExchange.Web.Areas.User.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Common;
    using Data.Data;
    using Models;
    using SkillExchange.Models;
    using Web.Controllers;
    using WebGrease.Css.Extensions;

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
            var userProfile = this.Data.Users
                .All()
                .Where(u => u.Id == this.UserProfile.Id)
                .Select(u => new ProfileViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.LastName,
                    Email = u.Email,
                    Town = u.Town.Name,
                    Description = u.Description,
                    OfferingSkills = u.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => s.Skill.Name)
                        .ToList(),
                    SeekingSkills = u.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => s.Skill.Name)
                        .ToList()
                })
                .First();

            return this.View(userProfile);
        }

        // GET: User/Profile/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            var userProfile = this.Data.Users
                .All()
                .Where(u => u.Id == this.UserProfile.Id)
                .Select(u => new ProfileViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Email = u.Email,
                    Town = u.Town.Name,
                    Description = u.Description,
                    OfferingSkills = u.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => s.Skill.Name)
                        .ToList(),
                    SeekingSkills = u.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => s.Skill.Name)
                        .ToList()
                })
                .First();

            return this.View(userProfile);
        }

        // GET: User/Profile/Show
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

            if (userProfile != null)
            {
                userProfile.Offering.ForEach(s => s.ApproversCount =
                this.Data.Approvers.All().Count(a => a.UserSkillId == s.Id));

                viewModel.UserProfile = userProfile;

                return this.View(viewModel);
            }

            TempData["message"] = new NotitficationMessage
            {
                Content = ErrorMessages.RequestedNotExistingUserPprofileMessage,
                Type = NotificationMessageType.Error
            };

            return this.RedirectToAction("Error", "Home", new {area = "User"});

        }

        // POST: User/Profile/Approve
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int skillId)
        {
            var userOwner = this.Data.UserSkills
                .All()
                .FirstOrDefault(s => s.Id == skillId);

            if (userOwner != null)
            {
                if (userOwner.UserId != this.UserProfile.Id)
                {
                    bool skillIsAlreadyApproved = this.Data.Approvers
                        .All()
                        .Any(a => a.UserSkillId == skillId && a.ApproverUserId == this.UserProfile.Id);
                    if (!skillIsAlreadyApproved)
                    {
                        var approver = new Approver
                        {
                            ApproverUserId = this.UserProfile.Id,
                            UserSkillId = skillId
                        };

                        var notification = new Notification
                        {

                            Content = "<strong>" + this.UserProfile.UserName + "</strong> approved your skill " +
                                "<strong>" + this.Data.UserSkills.All().First(s => s.Id == skillId).Skill.Name + "</strong>",
                            RecieverId = userOwner.UserId
                        };

                        this.Data.Approvers.Add(approver);
                        this.Data.Notifications.Add(notification);
                        using (var dbContextTransaction = this.Data.BeginTransaction())
                        {
                            try
                            {
                                var addResult = this.Data.SaveChanges();
                                if (addResult > 0)
                                {
                                    dbContextTransaction.Commit();
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
                                        Content = ErrorMessages.ApproveSkillErrorMessage,
                                        Type = NotificationMessageType.Error
                                    };
                                }
                            }
                            catch (Exception)
                            {
                                dbContextTransaction.Rollback();
                                TempData["message"] = new NotitficationMessage
                                {
                                    Content = ErrorMessages.ApproveSkillErrorMessage,
                                    Type = NotificationMessageType.Error
                                };
                            }
                        }
                    }
                    else
                    {
                        TempData["message"] = new NotitficationMessage
                        {
                            Content = ErrorMessages.AlreadyApprovedSkillMessage,
                            Type = NotificationMessageType.Error
                        };
                    }
                }
                else
                {
                    TempData["message"] = new NotitficationMessage
                    {
                        Content = ErrorMessages.AttemptToApproveOwnSkillMessage,
                        Type = NotificationMessageType.Error
                    };
                }

                return this.RedirectToAction("Show", "Profile", new { username = userOwner.User.UserName });
            }
            else
            {
                TempData["message"] = new NotitficationMessage
                {
                    Content = ErrorMessages.RequestedNotExistingUserSkillMessage,
                    Type = NotificationMessageType.Error
                };

                return this.RedirectToAction("Error", "Home", new { area = "User" });
            }
        }
    }
}