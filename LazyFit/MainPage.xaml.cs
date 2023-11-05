using LazyFit.Models;
using LazyFit.Services;
using LazyFit.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;

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

