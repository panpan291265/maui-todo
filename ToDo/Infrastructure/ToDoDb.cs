using SQLite;
using ToDo.Models;

namespace ToDo.Infrastructure;

public class ToDoDb
{
    SQLiteAsyncConnection Database;

    public ToDoDb()
    {
    }

    public async void DropDatabase()
    {
        if (Database != null)
        {
            await Database.DropTableAsync<ToDoModel>();
            await Database.CloseAsync();
            Database = null;
        }
        if (File.Exists(DbDefs.DatabasePath))
        {
            File.Delete(DbDefs.DatabasePath);
        }
    }

    private async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(DbDefs.DatabasePath, DbDefs.Flags);
        var result = await Database.CreateTableAsync<ToDoModel>();
    }

    public async Task<List<ToDoModel>> GetToDos()
    {
        await Init();
        var todos = await Database.Table<ToDoModel>().ToListAsync();
        return todos;
    }

    public async Task<List<ToDoModel>> GetToDosNotDone()
    {
        await Init();
        var todos = await Database.Table<ToDoModel>().Where(x => !x.Done).ToListAsync();
        return todos;

        // SQL queries are also possible
        //return await Database.QueryAsync<ToDoModel>("SELECT * FROM [ToDoModel] WHERE [Done] = 0");
    }

    public async Task<ToDoModel> GetToDo(string id)
    {
        await Init();
        var todo = await Database.Table<ToDoModel>()
            .Where(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefaultAsync();
        return todo;
    }

    public async Task<ToDoModel> SaveToDo(ToDoModel todo)
    {
        await Init();
        if (string.IsNullOrWhiteSpace(todo.Id))
        {
            todo.Id= Guid.NewGuid().ToString();
            await Database.InsertAsync(todo);
        }
        else
        {
            await Database.UpdateAsync(todo);
        }
        return todo;
    }

    public async Task<int> DeleteToDo(ToDoModel todo)
    {
        await Init();
        return await Database.DeleteAsync(todo);
    }
}
