using LazyFit.Services;

namespace LazyFit;

public partial class App : Application
{

    public App()
	{
		InitializeComponent();
		DatabaseInit();

		MainPage = new AppShell();
	}

	public void DatabaseInit()
	{
        DatabaseService data = new DatabaseService();
		Task.Run(data.FullInicialization);
    }
}
