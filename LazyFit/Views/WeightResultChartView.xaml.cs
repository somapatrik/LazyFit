using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class WeightResultChart : ContentView
{
	WeightChartResultViewModel _viewModel;
	public WeightResultChart()
	{
		InitializeComponent();
		BindingContext = _viewModel = new WeightChartResultViewModel();
	}
}