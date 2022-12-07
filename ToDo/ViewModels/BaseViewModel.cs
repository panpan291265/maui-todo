
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDo.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool processing = false;
}
