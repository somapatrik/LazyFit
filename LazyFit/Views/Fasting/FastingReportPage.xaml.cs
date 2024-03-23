using LazyFit.ViewModels.Fasting;
using Mopups.Pages;

namespace LazyFit.Views.Fasting;

public partial class FastingReportPage : PopupPage
{
	FastReportViewModel _viewModel;
	public FastingReportPage(Guid fastId)
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastReportViewModel(fastId);
	}
}