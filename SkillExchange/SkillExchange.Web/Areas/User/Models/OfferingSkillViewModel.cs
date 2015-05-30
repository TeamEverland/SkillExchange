namespace SkillExchange.Web.Areas.User.Models
{
    public class OfferingSkillViewModel
    {
        public int SkillId { get; set; }

        public string Name { get; set; }

        public int ApproversCount { get; set; }

        public bool CanBeApprovedByCurrentUserLogged { get; set; }
    }
}