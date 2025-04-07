using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Infrastructure.ViewModel
{
    public class CreateTaskItemViewModel
    {
       
        [Required]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
