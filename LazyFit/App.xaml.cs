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

	public async void InitDb()
	{
		await DB.InitDB();
	}
}
