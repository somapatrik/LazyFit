using LazyFit.ViewModels.Fasting;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class StartFastingView : PopupPage
{
	ChooseFastViewModel _viewModel;

	public event EventHandler NewFastStarted;

	public StartFastingView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new ChooseFastViewModel();
	}


    protected override void OnDisappearing()
    {
        base.OnDisappearing();

		if (_viewModel.OptionSelected)
			NewFastStarted?.Invoke(this, EventArgs.Empty);

    }
}