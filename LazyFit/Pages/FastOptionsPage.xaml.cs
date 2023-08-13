using LazyFit.ViewModels;

namespace LazyFit.Pages;

public partial class FastOptionsPage : ContentPage
{
	ChooseFastViewModel _viewModel;
	public FastOptionsPage()
	{
		InitializeComponent();
		BindingContext = _viewModel = new ChooseFastViewModel();
	}
}