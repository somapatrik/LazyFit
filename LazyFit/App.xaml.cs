using LazyFit.Services;

namespace LazyFit;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

		InitDb();
	}

	public void InitDb()
	{
		DB.InitDB();
	}
}
