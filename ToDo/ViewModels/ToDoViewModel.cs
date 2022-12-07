using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.ViewModels;

public partial class ToDoViewModel : BaseViewModel
{
    private IToDoService service;

    public ToDoViewModel(IToDoService service)
    {
        this.service = service;
        LoadToDos();
    }

    public string ServiceName => service?.GetType().Name.ToString() ?? "";

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private bool includeDone = false;

    [ObservableProperty]
    private ICollection<ToDoModel> toDos = new List<ToDoModel>();

    public bool CanCreateToDo => !string.IsNullOrWhiteSpace(Title);

    private async Task LoadToDos()
    {
        ToDos.Clear();
        try
        {
            Processing = true;
            ToDos = await this.service.GetToDos(includeDone: IncludeDone);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Load ToDos", "An unexpected error occured!", "Ok");
        }
        finally 
        { 
            Processing = false; 
        }
    }

    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IncludeDone))
        {
            LoadToDos();
        }
    }

    [RelayCommand]
    private async Task ClearAll()
    {
        bool confirm = await Shell.Current.DisplayAlert("ToDos", "Are you sure you want to clear all todos?", "Yes", "No");
        if (!confirm)
            return;
        try
        {
            await Task.Run(() =>
            {
                service.ClearAll();
            });
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Create ToDo", "An unexpected error occured!", "Ok");
        }
    }

    [RelayCommand]
    private async Task CreateToDo()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert("ToDos", "A title is required!", "Ok");
            return;
        }
        try
        {
            var todo = new ToDoModel()
            {
                Title = Title,
                Description = Description,
                Done = false
            };
            await Task.Run(() =>
            {
                service.SaveToDo(todo);
            });
            Title = string.Empty;
            Description = string.Empty;
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Create ToDo", "An unexpected error occured!", "Ok");
        }
    }

    [RelayCommand]
    private async Task SaveToDo(ToDoModel todo)
    {
        try
        {
            await Task.Run(() =>
            {
                service.SaveToDo(todo);
            });
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Save ToDo", "An unexpected error occured!", "Ok");
        }
    }

    [RelayCommand]
    private async Task RemoveToDo(ToDoModel todo)
    {
        if (todo == null) return;
        bool confirm = await Shell.Current.DisplayAlert("ToDos", $"Are you sure you want to remove '{todo.Title}'?", "Yes", "No");
        if (!confirm)
            return;
        try
        {
            await Task.Run(() =>
            {
                service.RemoveToDo(todo);
            });
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Remove ToDo", "An unexpected error occured!", "Ok");
        }
    }
}
