using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.Models
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(8), MaxLength(15)]
        public string Username { get; set; } = string.Empty;


        [MinLength(10), MaxLength(15)]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;


        [DataType(DataType.EmailAddress)]
        [MaxLength(75)]
        [Required]
        public string Email { get; set; } = string.Empty;

        public ICollection<TaskItem>? Tasks { get; set; }
        
    }
}
