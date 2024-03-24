using LazyFit.Models;
using LazyFit.ViewModels.Reports;
using Mopups.Pages;

namespace LazyFit.Views.Reports;

public partial class ActionDayView : PopupPage
{
	ActionDayViewModel _viewModel;  
	public ActionDayView(ActionSquareDate actionSquare)
	{
		InitializeComponent();
		
		BindingContext = _viewModel = new ActionDayViewModel(actionSquare);

	}
}