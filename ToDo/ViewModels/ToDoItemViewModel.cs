using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.ViewModels;

public partial class ToDoItemViewModel : BaseViewModel
{
    private IToDoService service;
    private ToDoModel todo;
    
    public ToDoItemViewModel(IToDoService service)
    {
        this.service = service;
    }

    [RelayCommand]
    private async Task RemoveToDo()
    {
        /*
        try
        {
            await service.RemoveToDo(todo);
        } catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Remove ToDo", "An unexpected error occured!", "Ok");
        }
        */
        await Shell.Current.DisplayAlert("Remove ToDo", "Should remove todo item!", "Ok");
    }
}
