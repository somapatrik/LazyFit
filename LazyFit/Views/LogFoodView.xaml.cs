using LazyFit.ViewModels;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class LogFoodView : PopupPage
{

	LogFoodViewModel viewModel;
	public LogFoodView()
	{
		InitializeComponent();
		BindingContext = viewModel = new LogFoodViewModel();
	}
}