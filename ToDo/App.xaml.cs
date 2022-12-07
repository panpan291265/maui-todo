using ToDo.Views;

namespace ToDo;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new Root();
	}
}
