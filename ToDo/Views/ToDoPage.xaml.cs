using ToDo.ViewModels;

namespace ToDo.Views;

public partial class ToDoPage : ContentPage
{
	public ToDoPage(ToDoViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}