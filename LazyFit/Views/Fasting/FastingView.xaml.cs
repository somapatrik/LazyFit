using LazyFit.ViewModels;

namespace LazyFit.Views.Fasting;

public partial class FastingView : ContentView
{
	FastingViewModel _viewModel;
	public FastingView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastingViewModel();
	}

}