namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Data;
    using Models;
    using Web.Controllers;

    public class CategoriesController : BaseController
    {
        public CategoriesController(ISkillExchangeData data)
            :base(data)
        {            
        }

        //
        // GET: Admin/Categories/Index
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var caterories = this.Data.SkillCategories
                .All()
                .OrderBy(c => c.Name)
                .Select(CategoryModel.ViewModel);
            return this.View(caterories);
        }

        //
        // GET: Admin/Categories/GetCategoryEditForm
        [HttpPost]
        public PartialViewResult GetCategoryEditForm(CategoryModel category)
        {
            var categoryViewModel = this.Data.SkillCategories
                .All()
                .Where(c => c.Id == category.Id)
                .Select(CategoryModel.ViewModel)
                .FirstOrDefault();

            return this.PartialView("_CategoryEditForm", categoryViewModel);
        }
    }
}