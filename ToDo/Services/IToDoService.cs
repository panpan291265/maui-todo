using ToDo.Models;

namespace ToDo.Services;

public interface IToDoService
{
    Task ClearAll();
    Task<ICollection<ToDoModel>> GetToDos(bool includeDone = false);
    Task<ToDoModel> FindToDo(string id);
    Task<ToDoModel> SaveToDo(ToDoModel model);
    Task<ToDoModel> RemoveToDo(string id);
    Task<ToDoModel> RemoveToDo(ToDoModel model);
}
