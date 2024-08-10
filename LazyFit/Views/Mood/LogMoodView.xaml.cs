using Mopups.Pages;
using LazyFit.ViewModels.MoodViewModels; 

namespace LazyFit.Views.Mood;

public partial class LogMoodView : PopupPage
{

	LogMoodViewModel _viewModel;
	public LogMoodView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogMoodViewModel();
	}
}