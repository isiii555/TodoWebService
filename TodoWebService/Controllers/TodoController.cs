using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Services.Todo;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            var item = await _todoService.GetTodoItem(id);


            return item is not null
                ? item
                : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _todoService.DeleteTodo(id);

            return result ? Ok() : BadRequest();
        }

        [HttpPost("createTodo")]

        public async Task<ActionResult<TodoItemDto>> CreateTodo(CreateTodoItemRequest createTodoItemRequest)
        {
            var item = await _todoService.CreateTodo(createTodoItemRequest);

            return item is not null ? item : BadRequest();
        }

        [HttpPatch("changeStatus/{id}")]

        public async Task<ActionResult<TodoItemDto>> ChangeStatus(int id,bool isCompleted)
        {
            var item = await _todoService.ChangeTodoItemStatus(id,isCompleted);

            return item is not null ? item : BadRequest();
        } 
    }
}
