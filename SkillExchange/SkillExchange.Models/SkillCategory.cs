namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SkillCategory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
