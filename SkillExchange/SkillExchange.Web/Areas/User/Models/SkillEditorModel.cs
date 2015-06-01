namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class SkillEditorModel
    {
        public string SkillExchangeType { get; set; }

        public int SkillListIndex { get; set; }

        public ICollection<CategoryOptionViewModel> Categories { get; set; } 
    }
}