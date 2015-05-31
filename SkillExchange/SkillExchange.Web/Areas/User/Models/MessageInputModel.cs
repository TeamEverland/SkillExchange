namespace SkillExchange.Web.Areas.User.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MessageInputModel
    {
        [Required]
        public string RecieverId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}