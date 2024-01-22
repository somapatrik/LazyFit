using LazyFit.ViewModels.Fasting;

namespace LazyFit.Views;

public partial class FastList : ContentView
{
	FastHistoryViewModel FastHistoryViewModel;
	public FastList()
	{
		InitializeComponent();
		BindingContext = FastHistoryViewModel = new FastHistoryViewModel();
	}

}