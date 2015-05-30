namespace SkillExchange.Web.Areas.User.Controllers
{
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

        public PartialViewResult Categories()
        {
            var userSkillsIds = this.Data.UserSkills
                .All()
                .DistinctBy(us => us.SkillId)
                .Select(us => us.SkillId).ToList();
            var skillsCatagoriesWithUsers =
                this.Data.Skills.All().Where(s => userSkillsIds.Contains(s.Id)).Select(s => s.CategoryId);

            return this.PartialView("_Categories");
        }

        public PartialViewResult Towns()
        {
            return this.PartialView("_Towns");
        }
    }
}