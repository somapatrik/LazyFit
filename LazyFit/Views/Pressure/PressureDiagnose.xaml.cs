using LazyFit.Models.Pressure;
using LazyFit.ViewModels;
using Mopups.Pages;

namespace LazyFit.Views.Pressure;

public partial class PressureDiagnose : PopupPage
{
    PresureDiagnoseViewModel _viewModel;
    public PressureDiagnose(BloodPressure bloodPressure)
    {
		InitializeComponent();
        BindingContext = _viewModel = new PresureDiagnoseViewModel(bloodPressure);
    }
}