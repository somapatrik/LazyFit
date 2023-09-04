using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class LogButtonsView : ContentView
{
    LogMoodViewModel _viewModel;

    public LogButtonsView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogMoodViewModel();
	}
}