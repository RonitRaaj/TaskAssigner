using System.ComponentModel.DataAnnotations;

public class TaskCompletionDTO
{
    [Required]
    public int TaskId { get; set; }
    public decimal? Rating { get; set; }
}