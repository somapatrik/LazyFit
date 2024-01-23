using LazyFit.ViewModels.MoodViewModels;

namespace LazyFit.Views;

public partial class LogButtonsView : ContentView
{
    LogButtonsViewModel _viewModel;

    public LogButtonsView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogButtonsViewModel();
	}
}