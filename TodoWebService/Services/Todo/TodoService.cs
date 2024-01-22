using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Todo
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItemDto?> ChangeTodoItemStatus(int id, bool isCompleted)
        {
            try
            {
                var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
                if (todoItem is not null)
                {
                    todoItem!.IsCompleted = isCompleted;
                    _context.TodoItems.Update(todoItem);
                    await _context.SaveChangesAsync();
                    return new TodoItemDto(todoItem.Id, todoItem.Text, todoItem.IsCompleted, todoItem.CreatedTime);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TodoItemDto?> CreateTodo(CreateTodoItemRequest request)
        {
            try
            {
                var newTodoItem = new TodoItem()
                {
                    Text = request.Text,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    IsCompleted = false,
                };
                await _context.TodoItems.AddAsync(newTodoItem);
                await _context.SaveChangesAsync();
                return new TodoItemDto(newTodoItem.Id, newTodoItem.Text, newTodoItem.IsCompleted, newTodoItem.CreatedTime);
            }
            catch (Exception)
            {
                //log
                return null;
            }
        }

        public async Task<bool> DeleteTodo(int id)
        {
            try
            {
                var todoItem = _context.TodoItems.FirstOrDefault(x => x.Id == id);
                if (todoItem is not null)
                {
                    _context.TodoItems.Remove(todoItem!);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                //log;
                return false;
            }
            return false;
        }

        public Task<List<TodoItemDto>> GetAll(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TodoItemDto?> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            return todoItem is not null
                ? new TodoItemDto(
                    Id: todoItem.Id,
                    Text: todoItem.Text,
                    IsCompleted: todoItem.IsCompleted,
                    CreatedTime: todoItem.CreatedTime)
                : null;
        }
    }
}
