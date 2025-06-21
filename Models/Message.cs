using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoxBoxApi.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("ChatId")]
        public int ChatId { get; set; }

        [Required]
        [Column("SenderId")]
        public int SenderId { get; set; }

        [Required]
        [Column("Content")]
        public string Content { get; set; } = null!;

        [Required]
        [Column("SentAt")]
        public DateTime SentAt { get; set; }

        [ForeignKey(nameof(ChatId))]
        public Chat? Chat { get; set; }
    }
}
