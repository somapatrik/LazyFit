using LazyFit.Models;
using LazyFit.ViewModels.Fasting;
using Mopups.Pages;

namespace LazyFit.Views.Fasting;

public partial class FastingReportPage : PopupPage
{
	FastReportViewModel _viewModel;
	public FastingReportPage(Fast finishedFast)
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastReportViewModel(finishedFast);
	}
}