using LazyFit.ViewModels.Pressure;

namespace LazyFit.Views.Pressure;

public partial class PressureCardLineView : ContentView
{
	PressureCardLineViewModel _viewModel;
	public PressureCardLineView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new PressureCardLineViewModel();
	}
}