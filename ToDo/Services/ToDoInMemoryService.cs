using ToDo.Models;

namespace ToDo.Services;

public class ToDoInMemoryService : IToDoService
{
    private ICollection<ToDoModel> todos = new List<ToDoModel>();

    public async Task ClearAll()
    {
        todos.Clear();
    }

    public async Task<ICollection<ToDoModel>> GetToDos(string searchTerm = "", bool includeDone = false)
    {
        var todos = this.todos;
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            todos = todos.Where(x =>
            {
                if (x.Title?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
                if (x.Description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
                return false;
            }).ToList();
        }
        if (!includeDone)
        {
            todos = todos.Where(x => !x.Done).ToList();
        }
        return await Task.FromResult(todos);
    }

    public async Task<ToDoModel> FindToDo(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        var todo = this.todos.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(todo);
    }

    public async Task<ToDoModel> SaveToDo(ToDoModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        if (string.IsNullOrWhiteSpace(model.Title))
            throw new ArgumentNullException(nameof(model.Title));
        var existingToDo = await this.FindToDo(model.Id);
        if (existingToDo != null)
        {
            existingToDo.Title = model.Title;
            existingToDo.Description = model.Description;
            existingToDo.Done = model.Done;
        }
        else
        {
            model.Id = System.Guid.NewGuid().ToString();
            this.todos.Add(model);
        }
        return model;
    }

    public async Task<ToDoModel> RemoveToDo(string id)
    {
        var existingToDo = await this.FindToDo(id);
        if (existingToDo != null)
            this.todos.Remove(existingToDo);
        return await Task.FromResult(existingToDo);
    }

    public async Task<ToDoModel> RemoveToDo(ToDoModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        return await this.RemoveToDo(model.Id);
    }
}
