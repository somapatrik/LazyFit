using LazyFit.Services;

namespace LazyFit;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		DB data = new DB();
		data.InitDB();

		MainPage = new AppShell();
	}
}
