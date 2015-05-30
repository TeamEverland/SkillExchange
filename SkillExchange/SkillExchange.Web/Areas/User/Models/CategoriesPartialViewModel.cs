namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class CategoriesPartialViewModel
    {
        public int? SelectedCategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}