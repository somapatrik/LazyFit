using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class FastingView : ContentView
{
	FastingViewModel _viewModel;
	public FastingView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastingViewModel();
	}
}