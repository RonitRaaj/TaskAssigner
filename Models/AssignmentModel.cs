public class AssignmentModel
{
    public int Id { get; set; }
    public TaskModel Task { get; set; } = new TaskModel();
    public int TaskId { get; set; }
    public UserModel User { get; set; } = new UserModel();
    public int UserId { get; set; }
    public DateTime AssignedDate { get; set; }
}