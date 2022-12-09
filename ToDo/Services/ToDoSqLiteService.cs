using ToDo.Infrastructure;
using ToDo.Models;

namespace ToDo.Services;

public class ToDoSqLiteService : IToDoService
{
    private ToDoDb db;

    public ToDoSqLiteService(ToDoDb db)
    {
        this.db = db;
    }

    public async Task ClearAll()
    {
        this.db.DropDatabase();
    }

    public async Task<ICollection<ToDoModel>> GetToDos(string searchTerm = "", bool includeDone = false)
    {
        return await db.GetToDos(searchTerm: searchTerm, includeDone: includeDone);
    }

    public async Task<ToDoModel> FindToDo(string id)
    {
        var todo = await db.GetToDo(id);
        return todo;
    }

    public async Task<ToDoModel> SaveToDo(ToDoModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        if (string.IsNullOrWhiteSpace(model.Title))
            throw new ArgumentNullException(nameof(model.Title));
        var newToDo = await db.SaveToDo(model);
        return newToDo;
    }

    public async Task<ToDoModel> RemoveToDo(string id)
    {
        var existingToDo = await this.FindToDo(id);
        if (existingToDo != null)
            await RemoveToDo(existingToDo);
        return await Task.FromResult(existingToDo);
    }

    public async Task<ToDoModel> RemoveToDo(ToDoModel model)
    {
        var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        await db.DeleteToDo(model);
        return await Task.FromResult(model);
    }
}
