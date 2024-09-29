using Mopups.Pages;

namespace LazyFit.Views;

public partial class LogButtonsView : PopupPage
{
    public LogButtonsView() 
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        ContentStack.TranslationY = this.Height;
        await ContentStack.TranslateTo(0, 0, 500);
    }
}