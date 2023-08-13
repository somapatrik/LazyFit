using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class FastingView : ContentView
{
	FastingViewModel _viewModel;
	public FastingView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastingViewModel();
		//RefreshData();
	}

	//public async void RefreshData()
	//{
 //       await _viewModel.RefreshFastData();

 //   }
}