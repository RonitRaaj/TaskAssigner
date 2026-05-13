public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? SkillSet { get; set; }
    public decimal? Rating { get; set; }
    public int? RatingCount { get; set; } = 0;
    public List<TaskModel>? OwnedTasks { get; set; } = new List<TaskModel>();
    public List<AssignmentModel>? Assignments { get; set; } = new List<AssignmentModel>();
}