using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BoxBoxApi.Models
{
    [Table("Chats")]
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("User1Id")]
        public int User1Id { get; set; }

        [Required]
        [Column("User2Id")]
        public int User2Id { get; set; }

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}