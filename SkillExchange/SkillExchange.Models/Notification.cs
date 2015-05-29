namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        public Notification()
        {
            this.IsRead = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string RecieverId { get; set; }

        public virtual User Reciever { get; set; }

        [Required]
        public bool IsRead { get; set; }
    }
}
