using ToDo.Models;
using ToDo.Services;
using ToDo.ViewModels;

namespace ToDo.Views;

public partial class ToDoItemTemplate : ContentView
{
    public ToDoItemTemplate()
    {
        InitializeComponent();
    }

    private void Done_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // TODO => This is the wrong way to do it, try to avoid it if possible !!!
        var checkbox = sender as CheckBox;
        var todo = checkbox?.BindingContext as ToDoModel;
        var vm = checkbox?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.BindingContext as ToDoViewModel;
        if (vm != null && todo != null)
            vm.SaveToDoCommand.Execute(todo);
    }

    /*
    public void RemoveToDoClicked(object sender, EventArgs args)
    {
        // TODO => This is the wrong way to do it, try to avoid it if possible !!!
        var button = sender as Button;
        var todo = button.BindingContext as ToDoModel;
        var vm = button.Parent.Parent.Parent.Parent.Parent.BindingContext as ToDoViewModel;
        if (vm != null)
            vm.RemoveToDoCommand.Execute(todo);
        else
            Shell.Current.DisplayAlert("Remove ToDo", $"Should remove todo item with id '{todo.Id}'", "Ok");
    }
    */
}