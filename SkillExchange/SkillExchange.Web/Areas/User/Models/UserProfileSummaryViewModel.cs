﻿namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class UserProfileSummaryViewModel
    {
        public string Username { get; set; }

        public string Town { get; set; }

        public IEnumerable<UserSkillViewModel> Offering { get; set; }

        public IEnumerable<UserSkillViewModel> Seeking { get; set; }

        public string Description { get; set; }
    }
}