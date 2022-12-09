using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDo.Models;
using ToDo.Services;
using static SQLite.SQLite3;

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
    private string searchTerm = string.Empty;

    [ObservableProperty]
    private bool includeDone = false;

    [ObservableProperty]
    private ObservableCollection<ToDoItemViewModel> toDoItems = new ObservableCollection<ToDoItemViewModel>();

    public bool CanCreateToDo => !string.IsNullOrWhiteSpace(Title);

    /*
    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IncludeDone))
        {
            LoadToDos();
        }
    }
    */

    private async Task LoadToDos()
    {
        ToDoItems.Clear();
        try
        {
            Processing = true;
            var todoItems = await this.service.GetToDos(searchTerm: SearchTerm, includeDone: IncludeDone);
            ToDoItems = new ObservableCollection<ToDoItemViewModel>(
                todoItems.Select(x => new ToDoItemViewModel(x))
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Load ToDos", "An unexpected error occured!", "Ok");
        }
        finally 
        { 
            Processing = false; 
        }
    }

    [RelayCommand]
    private async void SearchToDos()
    {
        await LoadToDos();
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
            System.Diagnostics.Debug.WriteLine(ex.Message);
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
    private async Task SaveToDo(ToDoItemViewModel todoItem)
    {
        try
        {
            await Task.Run(() =>
            {
                service.SaveToDo(todoItem?.ToDo);
            });
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Save ToDo", "An unexpected error occured!", "Ok");
        }
    }

    [RelayCommand]
    private async Task RemoveToDo(ToDoItemViewModel todoItem)
    {
        if (todoItem?.ToDo == null) return;
        bool confirm = await Shell.Current.DisplayAlert("ToDos", $"Are you sure you want to remove '{todoItem.ToDo.Title}'?", "Yes", "No");
        if (!confirm)
            return;
        try
        {
            await Task.Run(() =>
            {
                service.RemoveToDo(todoItem.ToDo);
            });
            await LoadToDos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Remove ToDo", "An unexpected error occured!", "Ok");
        }
    }
}
