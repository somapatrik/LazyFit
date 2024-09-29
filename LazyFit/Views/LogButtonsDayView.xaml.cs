using LazyFit.ViewModels;
using Mopups.Pages;
using System.ComponentModel.Design.Serialization;

namespace LazyFit.Views;

public partial class LogButtonsDayView : PopupPage
{
    private LogDayViewModel _viewModel;

    public LogButtonsDayView(DateTime _ActionDate)
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogDayViewModel(_ActionDate);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        DrinkButton.TranslationX = this.Width;
        FoodButton.TranslationX =  this.Width;

        await DrinkButton.TranslateTo(0, 0, 500);
             
        await FoodButton.TranslateTo(0, 0, 500);
    }


}