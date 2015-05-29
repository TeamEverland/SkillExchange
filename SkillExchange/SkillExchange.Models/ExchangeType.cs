namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ExchangeType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
