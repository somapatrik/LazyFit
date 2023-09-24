using LazyFit.ViewModels;

namespace LazyFit;

public partial class ResultsPage : ContentPage
{
    private ResultsViewModel _viewModel;

    public ResultsPage()
	{
		InitializeComponent();
        BindingContext = _viewModel = new ResultsViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}