using LazyFit.ViewModels.WeightViewModels;

namespace LazyFit.Views;

public partial class WeightChartView : ContentView
{
    WeightChartViewModel _viewModel;

    public WeightChartView()
	{
		InitializeComponent();
        BindingContext = _viewModel = new WeightChartViewModel();
	}
}