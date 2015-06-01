namespace SkillExchange.Web.Areas.User.Models
{
    public class UserSkillViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public UserSkillState State { get; set; }
    }
}