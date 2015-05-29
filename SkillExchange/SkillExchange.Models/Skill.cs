namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual SkillCategory Category { get; set; }
    }
}
