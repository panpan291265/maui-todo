using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models;

public class ToDoModel
{
    [Key]
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Done { get; set; } = false;
}
