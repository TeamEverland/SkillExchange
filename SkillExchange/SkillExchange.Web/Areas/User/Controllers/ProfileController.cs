namespace SkillExchange.Web.Areas.User.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Web.Controllers;
    using Common;
    using Data.Data;
    using Models;
    using SkillExchange.Models;

    using WebGrease.Css.Extensions;
    using EntityFramework.Extensions;

    public class ProfileController : BaseController
    {
        public ProfileController(ISkillExchangeData data)
            : base(data)
        {
        }

        //
        // GET: User/Profile/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var userProfile = this.Data.Users
                .All()
                .Where(u => u.Id == this.UserProfile.Id)
                .Select(ProfileModel.ViewModel)
                .First();

            return this.View(userProfile);
        }

        //
        // GET: User/Profile/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            var userProfile = this.Data.Users
                .All()
                .Where(u => u.Id == this.UserProfile.Id)
                .Select(ProfileModel.ViewModel)
                .First();

            Session["maxIndexOfferingSkillsList"] = userProfile.OfferingSkills.Count - 1;

            Session["maxIndexSeekingSkillsList"] = userProfile.SeekingSkills.Count - 1;

            return this.View(userProfile);
        }

        //
        // POST: User/Profile/Edit
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                #region Personal info edit
                var profileToBeEdited = this.Data.Users
                    .All()
                    .First(u => u.Id == this.UserProfile.Id);

                profileToBeEdited.FirstName = model.FirstName;
                profileToBeEdited.LastName = model.LastName;
                // Temporary removed
                // profileToBeEdited.UserName = model.Username;
                profileToBeEdited.Email = model.Email;
                profileToBeEdited.TownId = model.TownId;
                profileToBeEdited.Description = model.Description;
                #endregion

                #region Skills edit
                #region Offering skills edit
                if (model.OfferingSkills != null)
                {
                    foreach (var skill in model.OfferingSkills)
                    {
                        if (skill.State == UserSkillState.ExistingDeleted)
                        {
                            this.Data.Approvers.All().Where(a => a.UserSkillId == skill.Id).Delete();
                            this.Data.UserSkills.All().Where(s => s.Id == skill.Id).Delete();
                        }

                        if (skill.State == UserSkillState.New)
                        {
                            bool userHasTheSkill = this.Data.UserSkills
                                .All().Any(s => s.Skill.Name == skill.Name && s.UserId == this.UserProfile.Id);
                            if (!userHasTheSkill)
                            {
                                var newSkill = this.Data.Skills.All()
                                    .FirstOrDefault(s => s.Name == skill.Name);
                                if (newSkill == null)
                                {
                                    newSkill = new Skill
                                    {
                                        Name = skill.Name,
                                        CategoryId = skill.CategoryId
                                    };

                                    this.Data.Skills.Add(newSkill);
                                    this.Data.SaveChanges();
                                }

                                var newUserSkill = new UserSkill
                                {
                                    SkillId = newSkill.Id,
                                    UserId = this.UserProfile.Id,
                                    ExchangeTypeId = this.Data.ExchangeTypes.All().First(t => t.Name == "Offering").Id
                                };

                                this.Data.UserSkills.Add(newUserSkill);
                                this.Data.SaveChanges();
                            }
                            else
                            {
                                TempData["message"] = new NotificationMessage
                                {
                                    Content = string.Format("You already have the skill {0}", skill.Name),
                                    Type = NotificationMessageType.Error
                                };

                                return RedirectToAction("Edit", "Profile");
                            }
                        }

                        if (skill.State == UserSkillState.Existing)
                        {
                            var userSkill = this.Data.UserSkills.All().First(s => s.Id == skill.Id).Skill;
                            userSkill.CategoryId = skill.CategoryId;
                            userSkill.Name = skill.Name;

                            this.Data.Skills.Update(userSkill);
                            this.Data.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Seeking skills edit
                if (model.SeekingSkills != null)
                {
                    foreach (var skill in model.SeekingSkills)
                    {
                        if (skill.State == UserSkillState.ExistingDeleted)
                        {
                            this.Data.UserSkills.All().Where(s => s.Id == skill.Id).Delete();
                        }

                        if (skill.State == UserSkillState.New)
                        {
                            bool userHasTheSkill = this.Data.UserSkills
                                .All().Any(s => s.Skill.Name == skill.Name && s.UserId == this.UserProfile.Id);
                            if (!userHasTheSkill)
                            {
                                var newSkill = this.Data.Skills.All()
                                    .FirstOrDefault(s => s.Name == skill.Name);
                                if (newSkill == null)
                                {
                                    newSkill = new Skill
                                    {
                                        Name = skill.Name,
                                        CategoryId = skill.CategoryId
                                    };

                                    this.Data.Skills.Add(newSkill);
                                    this.Data.SaveChanges();
                                }

                                var newUserSkill = new UserSkill
                                {
                                    SkillId = newSkill.Id,
                                    UserId = this.UserProfile.Id,
                                    ExchangeTypeId = this.Data.ExchangeTypes.All().First(t => t.Name == "Seeking").Id
                                };

                                this.Data.UserSkills.Add(newUserSkill);
                                this.Data.SaveChanges();
                            }
                            else
                            {
                                TempData["message"] = new NotificationMessage
                                {
                                    Content = string.Format("You already have the skill {0}", skill.Name),
                                    Type = NotificationMessageType.Error
                                };

                                return RedirectToAction("Edit", "Profile");
                            }
                        }

                        if (skill.State == UserSkillState.Existing)
                        {
                            var userSkill = this.Data.UserSkills.All().First(s => s.Id == skill.Id).Skill;
                            userSkill.CategoryId = skill.CategoryId;
                            userSkill.Name = skill.Name;

                            this.Data.Skills.Update(userSkill);
                            this.Data.SaveChanges();
                        }
                    }
                }
                #endregion
                #endregion

                this.Data.Users.Update(profileToBeEdited);
                this.Data.SaveChanges();
            }

            return this.RedirectToAction("Index", "Profile");
        }

        //
        // GET: User/Profile/Show
        [Authorize]
        [HttpGet]
        public ActionResult Show(string username)
        {
            var viewModel = new UserProfileViewModel();

            if (TempData["message"] != null)
            {
                viewModel.Message = (NotificationMessage)TempData["message"];
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
                .Select(UserProfileFullViewModel.ViewModel)
                .FirstOrDefault();

            if (userProfile != null)
            {
                userProfile.Offering
                    .ForEach(s => s.CanBeApprovedByCurrentUserLogged =
                        !approvedSkillsByCurrentUserLogged.Contains(s.Id));
                userProfile.Offering
                    .ForEach(s => s.ApproversCount =
                this.Data.Approvers.All().Count(a => a.UserSkillId == s.Id));

                viewModel.UserProfile = userProfile;

                return this.View(viewModel);
            }

            TempData["message"] = new NotificationMessage
            {
                Content = ErrorMessages.RequestedNotExistingUserPprofileMessage,
                Type = NotificationMessageType.Error
            };

            return this.RedirectToAction("Error", "Home", new { area = "User" });

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
                                    TempData["message"] = new NotificationMessage
                                    {
                                        Content = SuccessMessages.SkillSuccessfullyApprovedMessage,
                                        Type = NotificationMessageType.Success
                                    };
                                }
                                else
                                {
                                    TempData["message"] = new NotificationMessage
                                    {
                                        Content = ErrorMessages.ApproveSkillErrorMessage,
                                        Type = NotificationMessageType.Error
                                    };
                                }
                            }
                            catch (Exception)
                            {
                                dbContextTransaction.Rollback();
                                TempData["message"] = new NotificationMessage
                                {
                                    Content = ErrorMessages.ApproveSkillErrorMessage,
                                    Type = NotificationMessageType.Error
                                };
                            }
                        }
                    }
                    else
                    {
                        TempData["message"] = new NotificationMessage
                        {
                            Content = ErrorMessages.AlreadyApprovedSkillMessage,
                            Type = NotificationMessageType.Error
                        };
                    }
                }
                else
                {
                    TempData["message"] = new NotificationMessage
                    {
                        Content = ErrorMessages.AttemptToApproveOwnSkillMessage,
                        Type = NotificationMessageType.Error
                    };
                }

                return this.RedirectToAction("Show", "Profile", new { username = userOwner.User.UserName });
            }
            else
            {
                TempData["message"] = new NotificationMessage
                {
                    Content = ErrorMessages.RequestedNotExistingUserSkillMessage,
                    Type = NotificationMessageType.Error
                };

                return this.RedirectToAction("Error", "Home", new { area = "User" });
            }
        }

        //
        // POST: User/Profile/LoggedUserGetUsername
        public ActionResult LoggedUserGetUsername()
        {
            return this.Content(this.UserProfile.UserName);
        }

        #region Data loading actions
        public ActionResult GetTowns()
        {
            var towns = this.Data.Towns.All()
                .Where(t => t.Id != this.UserProfile.TownId)
                .Select(t => new TownOptionViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList();

            return this.Json(towns.AsQueryable(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategories()
        {
            var categories = this.Data.SkillCategories.All()
                .Select(c => new CategoryOptionViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return this.Json(categories.AsQueryable(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSkills()
        {
            var skills = this.Data.Skills.All()
                .Select(s => new { s.Name })
                .ToList();

            return this.Json(skills.AsQueryable(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Actions for partial views rendering
        public ActionResult MessageForm(string recieverUsername)
        {
            var reciever = this.Data.Users.All().FirstOrDefault(u => u.UserName == recieverUsername);
            if (reciever != null)
            {
                var model = new MessageInputModel
                {
                    RecieverId = reciever.Id
                };

                return this.PartialView("_MessageForm", model);
            }

            TempData["message"] = new NotificationMessage
            {
                Type = NotificationMessageType.Error,
                Content = "Sorry, message reciever was not found"
            };

            return this.RedirectToAction("Error", "Home");
        }

        public ActionResult SkillEditor(string exchangeType)
        {
            var model = new SkillEditorModel();
            if (exchangeType == "Offering")
            {
                Session["maxIndexOfferingSkillsList"] = (int)Session["maxIndexOfferingSkillsList"] + 1;
                model.SkillListIndex = (int)Session["maxIndexOfferingSkillsList"];
            }
            else if (exchangeType == "Seeking")
            {
                Session["maxIndexSeekingSkillsList"] = (int)Session["maxIndexSeekingSkillsList"] + 1;
                model.SkillListIndex = (int)Session["maxIndexSeekingSkillsList"];
            }
            else
            {
                return this.Content("");
            }

            model.SkillExchangeType = exchangeType;
            var categories = this.Data.SkillCategories
                .All()
                .Select(c => new CategoryOptionViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToList();

            model.Categories = categories;
            return this.PartialView("_SkillEditor", model);
        }
        #endregion
    }
}