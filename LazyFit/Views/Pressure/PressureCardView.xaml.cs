using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;

namespace LazyFit.Views.Pressure;

public partial class PressureCardView : ContentView
{
    public static readonly BindableProperty BloodPressureProperty = BindableProperty.Create(nameof(BloodPressure), typeof(BloodPressure), typeof(PressureCardView));

    public BloodPressure BloodPressure
    {
        get => (BloodPressure)GetValue(BloodPressureProperty);
        set => SetValue(BloodPressureProperty, value);
    }

    public PressureCardView()
	{
		InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //var bp = BloodPressure;
        //if (await Shell.Current.DisplayAlert($"Delete {bp.High} / {bp.Low}", $"Remove this entry from {bp.Time.ToString("d")} ?", "Delete", "Cancel"))
        //{
        //    await DB.DeleteItem(bp);
        //    WeakReferenceMessenger.Default.Send(new Messages.RefreshPressureCards(true));
        //}
    }
}