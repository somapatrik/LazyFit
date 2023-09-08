using LazyFit.ViewModels;
using Mopups.Pages;

namespace LazyFit.Views;

public partial class EnterWeightView : PopupPage
{
    public EnterWeightViewModel _viewModel;

    public EnterWeightView()
	{
		InitializeComponent();
		BindingContext = _viewModel = new EnterWeightViewModel();

    }
}