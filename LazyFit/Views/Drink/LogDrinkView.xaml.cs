using LazyFit.ViewModels.DrinkViewModels;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class LogDrinkView : PopupPage
{
	LogDrinkViewModel _viewModel;

	public LogDrinkView()
	{
		InitializeComponent();

		BindingContext = _viewModel = new LogDrinkViewModel();
	}
}