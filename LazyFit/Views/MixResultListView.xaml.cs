using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class MixResultListView : ContentView
{

	MixResultsViewModel _viewModel;
	public MixResultListView()
	{
		InitializeComponent();

		BindingContext = _viewModel = new MixResultsViewModel(); 
	}
}