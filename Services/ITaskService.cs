public interface ITaskService
{
    Task<IEnumerable<TaskDTO>> GetAllAvailableTasksAsync();
    Task<List<TaskDTO>> GetTaskBySkillsAsync(string Skills);
    Task<TaskDTO> CreateTaskAsync(TaskDTO taskDTO, int userId);
    Task<TaskDTO> UpdateTaskAsync(int id, TaskDTO taskDTO, int userId);
    Task<bool> AssignTaskAsync(int taskId, int userId);
    Task<bool> CompletedTaskAsync(TaskCompletionDTO taskCompletionDTO, int userId);
}