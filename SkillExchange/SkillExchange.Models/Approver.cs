namespace SkillExchange.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Approver
    {
        [Key]
        [Column(Order = 1)]
        public string ApproverUserId { get; set; }

        public virtual User ApproverUser { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UserSkillId { get; set; }
    }
}
