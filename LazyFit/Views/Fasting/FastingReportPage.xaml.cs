using LazyFit.ViewModels.Fasting;

namespace LazyFit.Views.Fasting;

public partial class FastingReportPage : ContentPage
{
	FastReportViewModel _viewModel;
	public FastingReportPage(Guid fastId)
	{
		InitializeComponent();
		BindingContext = _viewModel = new FastReportViewModel(fastId);
	}
}