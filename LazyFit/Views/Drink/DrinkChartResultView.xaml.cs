using LazyFit.ViewModels.DrinkViewModels;

namespace LazyFit.Views.Drink;

public partial class DrinkChartResultView : ContentView
{
	DrinkChartResultViewModel _viewModel;
    public DrinkChartResultView()
	{
		InitializeComponent();
		//BindingContext = _viewModel = new DrinkChartResultViewModel();
	}
}