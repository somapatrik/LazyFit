using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class LatestActionsView : ContentView
{
    private LatestActionsViewModel _viewModel;

    public LatestActionsView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LatestActionsViewModel();
	}
}