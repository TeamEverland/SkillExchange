namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class TownsPartialViewModel
    {
        public int? SelectedTownId { get; set; }

        public IEnumerable<TownViewModel> Towns { get; set; } 
    }
}