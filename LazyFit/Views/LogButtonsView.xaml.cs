using LazyFit.ViewModels.MoodViewModels;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class LogButtonsView : PopupPage
{
    LogButtonsViewModel _viewModel;

    public LogButtonsView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogButtonsViewModel();
	}
}