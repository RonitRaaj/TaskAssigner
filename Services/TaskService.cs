using Microsoft.EntityFrameworkCore;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskDTO>> GetAllAvailableTasksAsync()
    {
        var tasks = await _context.Tasks.Where(t => t.Status == TaskStatus.NotStarted).ToListAsync();
        
        return tasks.Select(t => new TaskDTO
        {
            Title = t.Title,
            Description = t.Description,
            Budget = t.Budget,
            RequiredSkills = t.RequiredSkills
        });
    }

    public async Task<List<TaskDTO>> GetTaskBySkillsAsync(string Skills)
    {
        var tasks = await _context.Tasks.Where(t => t.RequiredSkills.Contains(Skills) || t.RequiredSkills == string.Empty).ToListAsync();
        if (tasks == null) throw new KeyNotFoundException($"Task not found with skills: {Skills}");
        return tasks.Select(t => new TaskDTO
        {
            Title = t.Title,
            Description = t.Description,
            Budget = t.Budget,
            RequiredSkills = t.RequiredSkills
        }).ToList();
    }

    public async Task<TaskDTO> CreateTaskAsync(TaskDTO taskDTO , int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException($"User not found with ID: {userId}");
        if (user.Role != "Client") throw new InvalidOperationException("Only Clients can create tasks.");
        var task = new TaskModel
        {
            Title = taskDTO.Title,
            Description = taskDTO.Description,
            Budget = taskDTO.Budget,
            RequiredSkills = taskDTO.RequiredSkills,
            Status = TaskStatus.NotStarted,
            OwnerId = userId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskDTO
        {
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            RequiredSkills = task.RequiredSkills
        };
    }

    public async Task<TaskDTO> UpdateTaskAsync(int id, TaskDTO taskDTO , int userId)
    {
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null) throw new KeyNotFoundException($"Task not found with ID: {id}");
        if (existingTask.OwnerId != userId) throw new InvalidOperationException("Only Owners can update their tasks.");

        existingTask.Title = taskDTO.Title;
        existingTask.Description = taskDTO.Description;
        existingTask.Budget = taskDTO.Budget;
        existingTask.RequiredSkills = taskDTO.RequiredSkills;

        await _context.SaveChangesAsync();
        return new TaskDTO
        {
            Title = existingTask.Title,
            Description = existingTask.Description,
            Budget = existingTask.Budget,
            RequiredSkills = existingTask.RequiredSkills
        };
    }

    public async Task<bool> AssignTaskAsync(int taskId, int userId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        var user = await _context.Users.FindAsync(userId);
        if (task == null || user == null) throw new KeyNotFoundException("Task or User not found");
        if (user.Role != "Freelancer") throw new InvalidOperationException("Task can only be assigned to Freelancers.");
        if (task.Status != TaskStatus.NotStarted) throw new InvalidOperationException("This task is already assigned or completed.");

        var assignment = new AssignmentModel
        {
            TaskId = taskId,
            UserId = userId,
            AssignedDate = DateTime.UtcNow
        };
        task.Status = TaskStatus.InProgress;

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompletedTaskAsync(TaskCompletionDTO taskCompletionDTO, int userId)
    {
        var task = await _context.Tasks.FindAsync(taskCompletionDTO.TaskId);
        if (task == null) throw new KeyNotFoundException($"Task not found with ID: {taskCompletionDTO.TaskId}");

        var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.TaskId == taskCompletionDTO.TaskId);
        if (assignment == null) throw new InvalidOperationException("Task is not assigned to any user.");

        var freelancer = await _context.Users.FindAsync(assignment.UserId);
        if (freelancer == null) throw new KeyNotFoundException($"Freelancer not found with ID: {assignment.UserId}");

        if (task.OwnerId != userId) throw new InvalidOperationException("Only Owners can mark tasks as completed.");
        if (task.Status == TaskStatus.Completed) throw new InvalidOperationException("Task is already completed.");

        freelancer.RatingCount++;
        freelancer.Rating = (freelancer.Rating * (freelancer.RatingCount - 1) + taskCompletionDTO.Rating) / freelancer.RatingCount;

        task.Status = TaskStatus.Completed;
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return true;
    }
}