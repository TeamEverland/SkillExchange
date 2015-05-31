namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Data;
    using Models;
    using SkillExchange.Models;
    using Web.Controllers;

    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Home/Index
        [HttpGet]
        public ActionResult Index(int? categoryId, int? townId)
        {
            IQueryable<User> query;

            if (categoryId == null && townId == null)
            {
                var adminRole = this.Data.Roles.All().FirstOrDefault(r => r.Name == "Administrator");
                var adminRoleId = adminRole != null ? adminRole.Id : string.Empty;

                query = this.Data.Users
                    .All()
                    .Where(u => !u.Roles.Select(r => r.RoleId).Contains(adminRoleId));
            }
            else
            {
                query = this.Data.Users
                    .All();

                if (categoryId != null)
                {
                    query = query.Where(u => u.Skills.Select(s => s.Skill.CategoryId).Contains(categoryId.Value));
                }

                if (townId != null)
                {
                    query = query.Where(u => u.TownId == townId);
                }
            }

            var users = query
                .Select(u => new UserProfileSummaryViewModel
                {
                    Username = u.UserName,
                    Offering = u.Skills
                        .Where(s => s.ExchangeType.Name == "Offering")
                        .Select(s => new UserSkillViewModel { Id = s.SkillId, Name = s.Skill.Name }),
                    Seeking = u.Skills
                        .Where(s => s.ExchangeType.Name == "Seeking")
                        .Select(s => new UserSkillViewModel { Id = s.Id, Name = s.Skill.Name })
                })
                .ToList();

            var viewModel = new HomeIndexPageViewModel
            {
                SelectedCategoryId = categoryId,
                SelectedTownId = townId,
                Users = users
            };

            return this.View(viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult Categories(int? categoryId)
        {
            var userSkillsInCategory = this.Data.UserSkills
                .All()
                .Include(us => us.Skill)
                .Select(us => new {us.Skill.CategoryId, us.SkillId})
                .GroupBy(us => us.CategoryId)
                .ToDictionary(g => g.Key, g => g.Distinct().Count());

            var categories = this.Data.SkillCategories
                .All()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();

            categories.ForEach(x => x.UserSkillsCount = userSkillsInCategory.ContainsKey(x.Id) ? userSkillsInCategory[x.Id] : 0);

            var viewModel = new CategoriesPartialViewModel
            {
                SelectedCategoryId = categoryId,
                Categories = categories
            };

            return this.PartialView("_Categories", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult Towns(int? townId)
        {
            var userSkillsInTown = this.Data.UserSkills
                .All()
                .Include(us => us.User)
                .Select(us => new { us.User.TownId, us.SkillId })
                .GroupBy(us => us.TownId)
                .ToDictionary(g => g.Key, g => g.Distinct().Count());

            var towns = this.Data.Towns
                .All()
                .Select(c => new TownViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();

            towns.ForEach(x => x.UserSkillsCount = userSkillsInTown.ContainsKey(x.Id) ? userSkillsInTown[x.Id] : 0);

            var viewModel = new TownsPartialViewModel
            {
                SelectedTownId = townId,
                Towns = towns
            };

            return this.PartialView("_Towns", viewModel);
        }
    }
}