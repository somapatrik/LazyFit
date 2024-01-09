using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;
using LazyFit.ViewModels.Pressure;

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

}