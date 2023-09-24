using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class FastChart : ContentView
{
    private FastChartViewModel _viewModel;

    public FastChart()
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastChartViewModel();
	}
}