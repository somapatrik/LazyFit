using LazyFit.ViewModels;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class StartFastingView
{
	ChooseFastViewModel _viewModel;
	public StartFastingView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new ChooseFastViewModel();
	}
}