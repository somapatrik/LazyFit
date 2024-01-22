using LazyFit.ViewModels.Weight;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class EnterWeightView : PopupPage
{
    public EnterWeightViewModel _viewModel;

    public event EventHandler EnterWeightClosed;

    public EnterWeightView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new EnterWeightViewModel();

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        EnterWeightClosed?.Invoke(this, EventArgs.Empty);
    }
}