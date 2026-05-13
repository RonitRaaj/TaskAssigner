using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/TaskAssigner")]
public class TAController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IAuthService _authService;

    public TAController(ITaskService taskService, IAuthService authService)
    {
        _taskService = taskService;
        _authService = authService;
    }

    [HttpGet("tasks")]
    public async Task<IActionResult> GetAvailableTasks()
    {
        var tasks = await _taskService.GetAllAvailableTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("tasks/search")]
    public async Task<IActionResult> SearchTasksBySkills([FromQuery] string skills)
    {
        var tasks = await _taskService.GetTaskBySkillsAsync(skills);
        return Ok(tasks);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var token = await _authService.LoginAsync(loginDTO);
        return Ok(new { Token = token });
    }

    [HttpPost("Register/Client")]
    public async Task<IActionResult> ClientRegister([FromBody] ClientSignUpDTO clientSignUpDTO)
    {
        var token = await _authService.ClientRegisterAsync(clientSignUpDTO);
        return Ok(new { Token = token });
    }

    [HttpPost("Register/User")]
    public async Task<IActionResult> UserRegister([FromBody] UserSignUpDTO userSignUpDTO)
    {
        var token = await _authService.UserRegisterAsync(userSignUpDTO);
        return Ok(new { Token = token });
    }

    [HttpPost("tasks/create")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDTO)
    {
        var userId = User.GetUserId();
        var task = await _taskService.CreateTaskAsync(taskDTO , userId);
        return Ok(task);
    }

    [HttpPost("tasks/{id}")]
    [Authorize(Roles = "Freelancer")]
    public async Task<IActionResult> AssignTask(int id)
    {
        var userId = User.GetUserId();
        var assigntask = await _taskService.AssignTaskAsync(id, userId);
        return Ok(assigntask);
    }

    [HttpPost("tasks/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteTask([FromBody] TaskCompletionDTO taskCompletionDTO)
    {
        var userId = User.GetUserId();
        var completetask = await _taskService.CompletedTaskAsync(taskCompletionDTO, userId);
        return Ok(completetask);
    }

    [HttpPut("tasks/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDTO taskDTO)
    {
        var userId = User.GetUserId();
        var updatedTask = await _taskService.UpdateTaskAsync(id, taskDTO , userId);
        return Ok(updatedTask);
    }



}