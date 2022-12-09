using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using ToDo.Models;

namespace ToDo.ViewModels;

public partial class ToDoItemViewModel : BaseViewModel
{
    [ObservableProperty]
    public ToDoModel toDo;
    
    public ToDoItemViewModel()
    {
    }

    public ToDoItemViewModel(ToDoModel todo)
    {
        ToDo = todo;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        System.Diagnostics.Debug.WriteLine($"ToDoItemViewModel => PropertyChanged: e.PropertyName");
        if (e.PropertyName == nameof(ToDo.Done))
        {
            System.Diagnostics.Debug.WriteLine("ToDoItemViewModel => ToDo Done Changed!");
        }
    }
}
