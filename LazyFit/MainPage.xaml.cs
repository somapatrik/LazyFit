using LazyFit.Models;
using LazyFit.Services;
using LazyFit.ViewModels;

namespace LazyFit;

public partial class MainPage : ContentPage
{
	MainViewModel viewModel;
	public MainPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new MainViewModel();
	}

}

