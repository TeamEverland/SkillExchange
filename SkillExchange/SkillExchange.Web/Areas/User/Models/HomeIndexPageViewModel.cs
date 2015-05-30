﻿namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class HomeIndexPageViewModel
    {
        public int? SelectedCategoryId { get; set; }

        public int? SelectedTownId { get; set; }

        public IEnumerable<UserProfileViewModel> Users { get; set; }
    }
}