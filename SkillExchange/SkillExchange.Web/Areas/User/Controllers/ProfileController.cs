﻿namespace SkillExchange.Web.Areas.User.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Transactions;
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
                    Content = ErrorMessages.AttemptToApproveOwnSkillMessage,
                    Type = NotificationMessageType.Error
                };

                return this.RedirectToAction("Error", "Home", new { area = "" });
            }
        }
    }
}