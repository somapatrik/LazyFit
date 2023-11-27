using LazyFit.ViewModels;
using Mopups.Pages;

namespace LazyFit.Views.Pressure;

public partial class EnterPressureView : PopupPage
{

	EnterPressureViewModel viewModel;
	public EnterPressureView()
	{
		InitializeComponent();
		BindingContext = viewModel = new EnterPressureViewModel();
	}
}