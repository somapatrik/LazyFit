using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;
using LazyFit.ViewModels.Pressure;

namespace LazyFit.Views.Pressure;

public partial class PressureCardView : ContentView
{
   // PressureCardViewModel _viewModel;

    public static readonly BindableProperty BloodPressureProperty = BindableProperty.Create(nameof(BloodPressure), typeof(BloodPressure), typeof(PressureCardView));

    public BloodPressure BloodPressure
    {
        get => (BloodPressure)GetValue(BloodPressureProperty);
        set
        {
            SetValue(BloodPressureProperty, value);
            //SetViewModel();
        }
    }

    //private void SetViewModel()
    //{
    //    _viewModel = new PressureCardViewModel(BloodPressure);
    //}

    public PressureCardView()
	{
		InitializeComponent();
        //_viewModel = new PressureCardViewModel(BloodPressure);

#if ANDROID

		Microsoft.Maui.Handlers.BorderHandler.Mapper.AppendToMapping("longBorder", (handler, view) =>
		{
            handler.PlatformView.LongClick += PlatformView_LongClick;
		});

#endif
    }


#if ANDROID
    private async void PlatformView_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
    {
        if (await Shell.Current.DisplayAlert("Remove pressure", "Remove", "Delete", "Cancel"))
        {
            //await _viewModel.DeletePressure();
            await DB.DeleteItem(BloodPressure);
            WeakReferenceMessenger.Default.Send(new Messages.RefreshPressureCards(true));
        }
    }
#endif
}