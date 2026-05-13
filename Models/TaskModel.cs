public enum TaskStatus { NotStarted, InProgress, Completed };
public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string RequiredSkills { get; set; } = string.Empty;

    public UserModel Owner { get; set; } = new UserModel();
    public int OwnerId { get; set; }
    public TaskStatus Status { get; set; }
    public AssignmentModel? Assignment { get; set; } = new AssignmentModel();
    
}

