using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Infrastructure.ViewModel
{
    public class LoginUserViewModel
    {
        
        [Required]
        [MinLength(8), MaxLength(15)]
        public string Username { get; set; } = string.Empty;


        [MinLength(10), MaxLength(500)]
        [Required]
        public string Password { get; set; } = string.Empty;


        [DataType(DataType.EmailAddress)]
        [MaxLength(75)]
        [Required]
        public string Email { get; set; } = string.Empty;
        
    }
}
