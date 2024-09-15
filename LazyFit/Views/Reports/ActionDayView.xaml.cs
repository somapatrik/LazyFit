using LazyFit.Models;
using LazyFit.ViewModels.Reports;

namespace LazyFit.Views.Reports;

public partial class ActionDayView : ContentPage
{
	ActionDayViewModel _viewModel;  
	public ActionDayView(ActionSquareDate actionSquare)
	{
		InitializeComponent();
		BindingContext = _viewModel = new ActionDayViewModel(actionSquare);

	}
}