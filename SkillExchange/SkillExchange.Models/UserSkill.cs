namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Key]
        [Column(Order = 3)]
        public int SkillId { get; set; }

        public virtual Skill Skill { get; set; }

        [Required]
        public int ExchangeTypeId { get; set; }
    }
}
