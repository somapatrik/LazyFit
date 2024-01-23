using LazyFit.ViewModels.FoodViewModels;

namespace LazyFit.Views.Food;

public partial class FoodChartResultView : ContentView
{
	FoodChartResultViewModel _viewModel;
    public FoodChartResultView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new FoodChartResultViewModel();
	}
}