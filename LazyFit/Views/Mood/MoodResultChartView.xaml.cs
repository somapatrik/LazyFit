using LazyFit.ViewModels.MoodViewModels;

namespace LazyFit.Views.Mood;

public partial class MoodResultChartView : ContentView
{
	MoodChartResultViewModel _viewModel;
	public MoodResultChartView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new MoodChartResultViewModel();
	}
}