namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Data;
    using Microsoft.Ajax.Utilities;
    using Models;
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
        public ActionResult Index()
        {
            return this.View();
        }

        [ChildActionOnly]
        public PartialViewResult Categories()
        {
            var userSkillsInCategory = this.Data.UserSkills
                .All()
                .Include(us => us.Skill)
                .DistinctBy(us => us.SkillId)
                .Select(us => new {us.Skill.CategoryId, us.SkillId})
                .GroupBy(us => us.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            var categories = this.Data.SkillCategories
                .All()
                .Select(c => new CategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();

            categories.ForEach(x => x.UserSkillsCount = userSkillsInCategory.ContainsKey(x.Id) ? userSkillsInCategory[x.Id] : 0);

            return this.PartialView("_Categories", categories);
        }

        [ChildActionOnly]
        public PartialViewResult Towns()
        {
            var userSkillsInTown = this.Data.UserSkills
                .All()
                .Include(us => us.User)
                .DistinctBy(us => us.User.TownId)
                .Select(us => new { us.User.TownId, us.SkillId })
                .GroupBy(us => us.TownId)
                .ToDictionary(g => g.Key, g => g.Count());

            var towns = this.Data.SkillCategories
                .All()
                .Select(c => new TownViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();

            towns.ForEach(x => x.UserSkillsCount = userSkillsInTown.ContainsKey(x.Id) ? userSkillsInTown[x.Id] : 0);

            return this.PartialView("_Towns", towns);
        }
    }
}